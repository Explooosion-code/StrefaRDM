using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using CitizenFX.Core;

namespace StrefaRDM_Server
{
    public class SrdmPlayer
    {
        public string source { get; private set; }
        public Player player { get; private set; }
        public int allKills { get; private set; }
        public string hex { get; private set; }
        public string ped { get; set; }
        public int sessionKills { get; private set; }
        private List<string> weapons;
        private int wpnCount;
        public SrdmPlayer(Player _player, string _source, int _kills, string _hex, string _ped = null)
        {
            source = _source;
            allKills = _kills;
            hex = _hex;
            ped = _ped;
            player = _player;

            weapons = getWeapons();
            wpnCount = weapons.Count;
            BaseScript.TriggerClientEvent(player, "srdm:updateWeapons", weapons);
            BaseScript.TriggerClientEvent(player, "srdm:loadPed", ped);
        }

        private List<string> getWeapons()
        {
            List<string> retVal = new List<string>();

            foreach (KeyValuePair<int, string> weapon in Config.Weapons)
            {
                if (allKills >= weapon.Key)
                {
                    retVal.Add(weapon.Value);
                }
            }

            return retVal;
        }

        public void addKill()
        {
            sessionKills++;
            allKills++;
            refreshWeapons();
        }

        public void resetKills()
        {
            sessionKills = 0;
            allKills = 0;
            refreshWeapons();
        }
        
        private void refreshWeapons()
        {
            weapons = getWeapons();
            wpnCount = weapons.Count;
            BaseScript.TriggerClientEvent(player, "srdm:updateWeapons", weapons);
            Main.syncData();
        }
    }
}