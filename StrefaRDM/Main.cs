using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace StrefaRDM
{
    public class Main : BaseScript
    {
        static Vector3 playerSpawnPos = new Vector3(0f,0f,0f);
        bool tickson = false;
        int maxDist = 100;
        static List<Vector3> RandomRespawns = new List<Vector3>();

        public Main()
        {
            NetworkSetFriendlyFireOption(true);
            TriggerServerEvent("srdm:getCurrentPosition");
            EventHandlers["srdm:setCurrentPosition"] += new Action<dynamic, List<dynamic>, int>((spawnpos, respawns, mD) =>
            {
                playerSpawnPos = spawnpos;
                RandomRespawns = new List<Vector3>();
                maxDist = mD;
                foreach(var pos in respawns)
                {
                    RandomRespawns.Add(pos);
                }
            });
            EventHandlers["srdm:roundChanged"] += new Action(async () =>
            {
                SetToRandomResp();
                FreezeEntityPosition(Game.PlayerPed.Handle, true);
                SetPlayerInvincible(Game.Player.Handle, true);
                await Delay(5000);
                FreezeEntityPosition(Game.PlayerPed.Handle, false);
                SetPlayerInvincible(Game.Player.Handle, false);
            });
            EventHandlers["playerSpawned"] += new Action(async () =>
            {
                while (playerSpawnPos.X == 0f)
                {
                    await Delay(5);
                }
                SetEntityCoords(Game.PlayerPed.Handle, playerSpawnPos.X, playerSpawnPos.Y, playerSpawnPos.Z,
                    false, false, false, false);
                Exports["spawnmanager"].setAutoSpawn(false);
                if (!tickson)
                {
                    tickson = true;
                    Tick += dist;
                    Tick += stamina;
                }
                TriggerServerEvent("srdm:requestUserInfo");
                TriggerServerEvent("srdm:playerSpawned");
                SetToRandomResp();
            });
            Tick += noHandFight;
            Tick += drpc;
            Tick += noPedsAndVehs;
        }
        async Task drpc()
        {
            await Delay(5000);
            SetDiscordAppId("729017581624885421");

            SetDiscordRichPresenceAsset("logomain");

            SetDiscordRichPresenceAssetText("StrefaRDM");

            SetRichPresence($"ID: {Game.Player.ServerId} - {Game.Player.Name} | Graczy: {Players.ToList().Count}/32");
        }

        async Task noPedsAndVehs()
        {
            await Delay(100);
            Vector3 plyCoords = GetEntityCoords(GetPlayerPed(-1), true);
            ClearAreaOfPeds(plyCoords.X, plyCoords.Y, plyCoords.Z, 200.0f, 1);
            ClearAreaOfVehicles(plyCoords.X, plyCoords.Y, plyCoords.Z, 200.0f, true, true, true, true, true);
        }
        async Task dist()
        {
            await Delay(500);
            Vector3 coords = Game.PlayerPed.Position;
            float dist = World.GetDistance(coords, playerSpawnPos);

            if (dist > maxDist && !Game.PlayerPed.IsDead)
            {
                SetToRandomResp();
                Utils.ShowNotif("~r~You can't leave the area of fight");
            }
        }

        async Task stamina()
        {
            await Delay(10);
            StatSetInt((uint)GetHashKey("MP0_STAMINA"), 100, true);
            SetPedArmour(Game.PlayerPed.Handle, 0);
            NetworkSetTalkerProximity(0.000f);
        }

        async Task noHandFight()
        {
            DisableControlAction(1, 140, true);
            DisableControlAction(1, 141, true);
            DisableControlAction(1, 142, true);
        }

        public static void SetToRandomResp()
        {
            Random random = new Random();

            int randomLocation = random.Next(0, RandomRespawns.Count - 1);

            Vector3 position = RandomRespawns[randomLocation];
            SetEntityCoords(Game.PlayerPed.Handle, position.X, position.Y, position.Z,
                    false, false, false, false);
        }
    }
}
