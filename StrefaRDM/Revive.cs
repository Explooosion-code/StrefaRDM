using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM
{
    public class Revive : BaseScript
    {
        public bool IsDead = false;
        private bool countdownPlaying = false;
        private WeaponHash currentWeapon;
        public Revive()
        {
            Tick += onTick;
        }

        async Task startCountDown()
        {
            float frames = 0f;
            int time = 2;
            int ScaleformHandle = RequestScaleformMovie("mp_big_message_freemode");
            while (!HasScaleformMovieLoaded(ScaleformHandle)) {
                await Delay(1);
            }


            while (!IsControlJustReleased(0, 38) || time > 0)
            {
                await Delay(0);
                BeginScaleformMovieMethod(ScaleformHandle, "SHOW_SHARD_WASTED_MP_MESSAGE");
                PushScaleformMovieMethodParameterString("~r~You Died");
                if(frames > 1)
                {
                    frames = 0;
                    if(time > 0)
                        time--;
                }
                if (time <= 0)
                {
                    PushScaleformMovieMethodParameterString("Press [~b~E~w~] to respawn");
                } else
                {
                    if(time == 1)
                        PushScaleformMovieMethodParameterString($"Respawn available in ~b~{time}~w~ second");
                    else
                        PushScaleformMovieMethodParameterString($"Respawn available in ~b~{time}~w~ seconds");
                }
                PushScaleformMovieMethodParameterInt(5);
                EndScaleformMovieMethod();
                DrawScaleformMovieFullscreen(ScaleformHandle, 255, 255, 255, 255, 0);
                frames += GetFrameTime();
            }

            RevivePlayer();

            SetScaleformMovieAsNoLongerNeeded(ref ScaleformHandle);
            countdownPlaying = false;
        }

        async Task onTick()
        {

            int ped = GetPlayerPed(-1);
          
            if(IsEntityDead(ped) && !countdownPlaying)
            {
                SetPlayerInvincible(PlayerId(), true);
                SetEntityHealth(ped, 1);
                currentWeapon = Game.PlayerPed.Weapons.Current;
                startCountDown();
                countdownPlaying = true;
            }
        }

        void RevivePlayer()
        {
            Vector3 pos = Game.PlayerPed.Position;
            float heading = Game.PlayerPed.Heading;
            NetworkResurrectLocalPlayer(pos.X, pos.Y, pos.Z, heading, true, false);
            SetPlayerInvincible(Game.Player.Handle, false);
            ClearPedBloodDamage(Game.PlayerPed.Handle);
            Main.SetToRandomResp();
            Game.PlayerPed.Weapons.Select(currentWeapon, true);
            Game.PlayerPed.Weapons[currentWeapon].AmmoInClip = Game.PlayerPed.Weapons[currentWeapon].MaxAmmoInClip;
        }
    }
}
