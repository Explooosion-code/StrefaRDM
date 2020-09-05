using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM
{
    public class UI : BaseScript
    {

        public UI()
        {
            Tick += onTick;
        }


        async Task onTick()
        {
            if (IsPedArmed(GetPlayerPed(-1), 6))
            {
                int currentAmmo = Game.PlayerPed.Weapons.Current.AmmoInClip;
                int maxAmmo = Game.PlayerPed.Weapons.Current.MaxAmmoInClip;
                DrawTxt(0.92f, 0.02f, $"{currentAmmo} / {maxAmmo}", 0.8f);
            }

            int hp = Game.PlayerPed.Health;
            hp = hp < 0 ? 0 : hp;
            if (hp > 40)
            {
                DrawTxt(0.92f, 0.92f, $"{hp}", 0.8f);
            }
            else
            {
                DrawTxt(0.92f, 0.92f, $"{hp}", 0.8f, 191, 27, 84);
            }
            DrawTxt(0.945f, 0.925f, "➕", 0.45f, 255, 0, 0);
        }

        static void DrawTxt(float x, float y, string text, float scale = 0.55f, int r = 255, int g = 255, int b = 255)
        {
            SetTextScale(scale, scale);
            SetTextFont(4);
            SetTextProportional(true);
            SetTextColour(r, g, b, 215);
            SetTextEntry("STRING");
            SetTextOutline();
            AddTextComponentString(text);
            DrawText(x, y);
        }
    }
}
