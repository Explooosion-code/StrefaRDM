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
        private int currentZone = 1;
        private int timeToNextZone =Config.MapChangeTime; 
        static Dictionary<string, SrdmPlayer> playersData = new Dictionary<string, SrdmPlayer>();

        public Main()
        {
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

            EventHandlers["srdm:playerSpawned"] += new Action<Player>(Spawned);

            Tick += OnTick;
            Tick += MapTick;
        }

        async Task OnTick()
        {
            await Delay(120000);

            foreach (KeyValuePair<string, SrdmPlayer> player in playersData)
            {
                savePlayer(player.Value);
                await Delay(1);
            }
        }

        async Task MapTick()
        {
            await Delay(1000);

            timeToNextZone--;

            if (timeToNextZone == 0)
            {
                int newZone = currentZone;

                while (newZone == currentZone)
                {
                    await Delay(1);
                    newZone = new Random().Next(0, Config.Positions.Count -1);
                }

                currentZone = newZone;

                timeToNextZone = Config.MapChangeTime;

                foreach (KeyValuePair<string, SrdmPlayer> player in playersData)
                {
                    player.Value.resetKills();
                }

                Exports["mysql-async"].mysql_execute("UPDATE users SET kills = 0",
                    new
                    {
                        
                    });

                syncZoneInfo();

                TriggerClientEvent("chatMessage", "SYSTEM", new[] {255, 0, 0},
                    "Nastąpiła zmiana mapy! Obecna mapa to: " + Config.Positions[currentZone].Name + "!");
                TriggerClientEvent("srdm:roundChanged");

                await Delay(100);

                for (int i = 5; i > 0; i--)
                {
                    TriggerClientEvent("chatMessage","SYSTEM", new[] { 255, 0, 0 }, "Start za " + i);
                    await Delay(1000);
                }
            } else if (timeToNextZone % 300 == 0)
            {
                syncZoneInfo();
                TriggerClientEvent("chatMessage", "SYSTEM", new[] {255, 0, 0},
                    "Za: " + Math.Floor(timeToNextZone / 60.0) + " minut(y) nastąpi zmiana mapy!");
            }
        }

        private void Spawned([FromSource] Player source)
        {
            string hex = "steam:" + source.Identifiers["steam"];
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
                        @he = hex
                    });
                    
                    playersData.Add(source.Handle, new SrdmPlayer(source, source.Handle, 0, hex));
                    syncData();
                    return;
                }

                int kills = result[0].kills;
                string ped = result[0].ped;
                playersData.Add(source.Handle, new SrdmPlayer(source, source.Handle, kills, hex, ped));
                syncData();
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

            retVal = retVal.OrderBy(o => -1 * o.sessionKills).ToList();
            TriggerClientEvent("srdm:syncPlayerData", retVal);
        }
        
        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            string source = player.Handle;

            SrdmPlayer srdmPlayer = playersData[source];

            if (srdmPlayer != null)
            {
                savePlayer(srdmPlayer);
            }
            syncData();
            playersData.Remove(source);
        }
        
        private void savePed([FromSource]Player player, string pedModel)
        {
            string source = player.Handle;

            if (playersData[source] != null)
            {
                playersData[source].ped = pedModel;
            }
        }

        public void savePlayer(SrdmPlayer player)
        {
            Exports["mysql-async"].mysql_execute("UPDATE users SET kills = @kills, ped = @ped WHERE steamhex = @hex",
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
            );
        }
    }
}
