using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM
{
    class KillStats : BaseScript
    {
        bool isAlredyDead;
        public KillStats()
        {
            Tick += onTick;
        }

        async Task onTick()
        {
            Ped playerPed = Game.PlayerPed;

            if(playerPed.IsDead)
            {
                if(!isAlredyDead)
                {
                    isAlredyDead = true;
                    Entity killer = playerPed.GetKiller();
                    if (killer.Model.IsPed)
                    {
                        if (killer.Handle == playerPed.Handle) return;
                        foreach (Player p in Players.ToList())
                        {
                            if (p.Character.Handle == killer.Handle)
                            {
                                TriggerServerEvent("srdm:onPlayerKilled", p.ServerId);
                                break;
                            }
                        }
                    }
                }
            } else
            {
                if(isAlredyDead)
                {
                    isAlredyDead = false;
                }
            }
        }
    }
}
