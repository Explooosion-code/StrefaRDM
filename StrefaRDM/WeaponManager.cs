using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace StrefaRDM
{
    public class WeaponManager : BaseScript
    {
        List<string> WeaponList = new List<string>() {};
        int lastCount = -1;
        public WeaponManager()
        {
            EventHandlers["srdm:updateWeapons"] += new Action<dynamic>(arr =>
            {
                List<string> temp = new List<string>() { };
                foreach(string a in arr)
                {
                    temp.Add(a);
                }
                WeaponList = temp;
                if (lastCount < WeaponList.Count && lastCount != -1)
                {
                    SetTextEntry_2("STRING");
                    AddTextComponentString("Zdobyłeś nową ~b~broń~w~!");
                    DrawSubtitleTimed(15000, false);
                }

                lastCount = WeaponList.Count;
                RefreshWeapons();
            });
        }

        void RefreshWeapons()
        {
            int ped = GetPlayerPed(-1);
            uint CurWpn = 0;
            GetCurrentPedWeapon(ped, ref CurWpn, true);

            RemoveAllPedWeapons(ped, true);

            foreach(string hash in WeaponList)
            {
                GiveWeaponToPed(ped, (uint)GetHashKey(hash), 1000, false, false);
                SetPedInfiniteAmmo(ped, true, (uint)GetHashKey(hash));
            }

            SetCurrentPedWeapon(ped, CurWpn, true);
        }
    }
}
