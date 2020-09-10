using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM_Server
{
    public class Main : BaseScript
    {
        // Index of zone that is currently running
        private int currentZone = new Random().Next(0, Config.Positions.Count - 1);
        // In seconds, is decremented every 1000ms
        private int timeToNextZone =Config.MapChangeTime; 
        // Holds all SrdmPlayer objects
        static Dictionary<string, SrdmPlayer> playersData = new Dictionary<string, SrdmPlayer>();

        public Main()
        {
            /*RegisterCommand("addkill", new Action<int, List<object>, string>((source, args, raw) =>
            {
                string src = source.ToString();
                playersData[src].addKill();
            }), false);*/
            
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["srdm:savePed"] += new Action<Player, string>(savePed);
            EventHandlers["srdm:onPlayerKilled"] += new Action<int>((id) =>
            {
                string source = id.ToString();

                if (playersData[source] != null)
                {
                    playersData[source].addKill();
                }
            });
            
            EventHandlers["srdm:requestUserInfo"] += new Action(() =>
            {
                syncData();
            });
            
            EventHandlers["srdm:getCurrentPosition"] += new Action(() =>
            {
                syncZoneInfo();
            });

            EventHandlers["srdm:playerSpawned"] += new Action<Player>(Spawned); // Create server SrdmPlayer object for source

            Tick += OnTick;
            Tick += MapTick;
        }

        async Task OnTick()
        {
            await Delay(120000); // You still save when player dropps but just to make sure lets save every 2 minutes

            foreach (KeyValuePair<string, SrdmPlayer> player in playersData)
            {
                savePlayer(player.Value);
                await Delay(1); // Just to not spam the db send one per frame
            }
        }

        async Task MapTick()
        {
            await Delay(1000);

            timeToNextZone--;

            if (timeToNextZone == 0)
            {
                int newZone = currentZone;

                while (newZone == currentZone) // Not let the map be the same for two rounds
                {
                    await Delay(1);
                    newZone = new Random().Next(0, Config.Positions.Count); // Picking a random zone;
                }

                currentZone = newZone;

                timeToNextZone = Config.MapChangeTime; // reset the time

                foreach (KeyValuePair<string, SrdmPlayer> player in playersData)
                {
                    player.Value.resetKills(false); // Reset all players kills. Sync set to false so you dont send a lot of events to client
                }

                syncData(); // Better to send information once after everything is done.

                Exports["mysql-async"].mysql_execute("UPDATE users SET kills = 0", new{}); // Reset kills for offline players too

                syncZoneInfo();

                TriggerClientEvent("chatMessage", "SYSTEM", new[] {255, 0, 0},
                    "Map changed! Currently playing on: " + Config.Positions[currentZone].Name + "!");
                TriggerClientEvent("srdm:roundChanged");

                await Delay(100);

                for (int i = 5; i > 0; i--)
                {
                    TriggerClientEvent("chatMessage","SYSTEM", new[] { 255, 0, 0 }, "Starting in " + i); // Count down to start
                    await Delay(1000);
                }
            } else if (timeToNextZone % 300 == 0)
            {
                syncZoneInfo(); // Sync zone info every 5 minutes just to make sure everything is fine.
                TriggerClientEvent("chatMessage", "SYSTEM", new[] {255, 0, 0},
                    "Map will change in: " + Math.Floor(timeToNextZone / 60.0) + " minutes!");
            }
        }

        private void Spawned([FromSource] Player source)
        {
            string hex = "steam:" + source.Identifiers["steam"]; // Get player that is in the database
            Exports["mysql-async"].mysql_fetch_all("SELECT * FROM users WHERE steamhex = @sh", new 
            {
               @sh = hex
            }, new Action<List<dynamic>>((result) =>
            {
                if (result[0] == null)
                {
                    Exports["mysql-async"].mysql_execute("INSERT INTO users (steamhex, kills) VALUES (@hex , 0)",
                    new 
                    {
                        @hex = hex
                    });
                    
                    playersData.Add(source.Handle, new SrdmPlayer(source, source.Handle, 0, hex));
                    syncData(); // sync after creating new player so the player is added to playerList
                    return;
                }

                int kills = result[0].kills;
                string ped = result[0].ped;
                playersData.Add(source.Handle, new SrdmPlayer(source, source.Handle, kills, hex, ped));
                syncData(); // sync after creating new player so the player is added to playerList
            }));
        }

        public static async Task syncData()
        {
            List<dynamic> retVal = new List<dynamic>();

            foreach (KeyValuePair<string, SrdmPlayer> pair in playersData)
            {
                retVal.Add(new
                {
                    name = GetPlayerName(pair.Value.source), 
                    sourceId = int.Parse(pair.Value.source),
                    sessionKills = pair.Value.sessionKills
                });
            }

            retVal = retVal.OrderBy(o => -1 * o.sessionKills).ToList(); // sort the table by kills
            TriggerClientEvent("srdm:syncPlayerData", retVal); // send data to all clients
        }
        
        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            string source = player.Handle;

            try
            {
                SrdmPlayer srdmPlayer = playersData[source];

                if (srdmPlayer != null)
                {
                    savePlayer(srdmPlayer); // save player on disconnect
                }

                playersData.Remove(source);
                syncData(); // Remove player from player list
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Player {player.Handle} was not in StrefaRDM player list!");
            }
        }
        
        private void savePed([FromSource]Player player, string pedModel)
        {
            string source = player.Handle;

            if (playersData[source] != null && !string.IsNullOrEmpty(pedModel))
            {
                playersData[source].ped = pedModel;
            }
        }

        public void savePlayer(SrdmPlayer player)
        {
            Exports["mysql-async"].mysql_execute("UPDATE users SET kills = @kills, ped = @ped WHERE steamhex = @hex", // Update the db
                new
                {
                    @hex = player.hex,
                    @kills = player.allKills.ToString(), 
                    @ped = player.ped
                });
        }

        void syncZoneInfo()
        {
            TriggerClientEvent("srdm:setCurrentPosition", Config.Positions[currentZone].Center, 
                Config.Positions[currentZone].Respawns,
                Config.Positions[currentZone].MaxDist
            ); // send all zone data
        }
    }
}
