using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM
{
  public static class Utils
  {
    public static string GetPedHitLocation(int ped)
    {
      string str;
      int num = 0;
      GetPedLastDamageBone(ped, ref num);
      switch (num)
      {
        case 0:
          str = "body";
          break;
        case 1356:
          str = "head";
          break;
        case 2108:
          str = "leg";
          break;
        case 2992:
          str = "arm";
          break;
        case 4089:
          str = "arm";
          break;
        case 4090:
          str = "arm";
          break;
        case 4137:
          str = "arm";
          break;
        case 4138:
          str = "arm";
          break;
        case 4153:
          str = "arm";
          break;
        case 4154:
          str = "arm";
          break;
        case 4169:
          str = "arm";
          break;
        case 4170:
          str = "arm";
          break;
        case 4185:
          str = "arm";
          break;
        case 4186:
          str = "arm";
          break;
        case 5232:
          str = "arm";
          break;
        case 6286:
          str = "arm";
          break;
        case 6442:
          str = "leg";
          break;
        case 10706:
          str = "body";
          break;
        case 11174:
          str = "head";
          break;
        case 11816:
          str = "body";
          break;
        case 12844:
          str = "head";
          break;
        case 14201:
          str = "leg";
          break;
        case 16335:
          str = "leg";
          break;
        case 17188:
          str = "head";
          break;
        case 17719:
          str = "head";
          break;
        case 18905:
          str = "arm";
          break;
        case 19336:
          str = "head";
          break;
        case 20178:
          str = "head";
          break;
        case 20279:
          str = "head";
          break;
        case 20623:
          str = "head";
          break;
        case 20781:
          str = "leg";
          break;
        case 21550:
          str = "head";
          break;
        case 22711:
          str = "arm";
          break;
        case 23553:
          str = "body";
          break;
        case 23639:
          str = "leg";
          break;
        case 24806:
          str = "leg";
          break;
        case 24816:
          str = "body";
          break;
        case 24817:
          str = "body";
          break;
        case 24818:
          str = "body";
          break;
        case 25260:
          str = "head";
          break;
        case 26610:
          str = "arm";
          break;
        case 26611:
          str = "arm";
          break;
        case 26612:
          str = "arm";
          break;
        case 26613:
          str = "arm";
          break;
        case 26614:
          str = "arm";
          break;
        case 27474:
          str = "head";
          break;
        case 28252:
          str = "arm";
          break;
        case 28422:
          str = "arm";
          break;
        case 29868:
          str = "head";
          break;
        case 31086:
          str = "head";
          break;
        case 35502:
          str = "leg";
          break;
        case 35731:
          str = "head";
          break;
        case 36029:
          str = "arm";
          break;
        case 36864:
          str = "leg";
          break;
        case 37119:
          str = "arm";
          break;
        case 37193:
          str = "head";
          break;
        case 39317:
          str = "head";
          break;
        case 40269:
          str = "arm";
          break;
        case 43536:
          str = "head";
          break;
        case 43810:
          str = "arm";
          break;
        case 45509:
          str = "arm";
          break;
        case 45750:
          str = "head";
          break;
        case 46078:
          str = "leg";
          break;
        case 46240:
          str = "head";
          break;
        case 47419:
          str = "head";
          break;
        case 47495:
          str = "head";
          break;
        case 49979:
          str = "head";
          break;
        case 51826:
          str = "leg";
          break;
        case 52301:
          str = "leg";
          break;
        case 56604:
          str = "body";
          break;
        case 57005:
          str = "arm";
          break;
        case 57597:
          str = "body";
          break;
        case 57717:
          str = "leg";
          break;
        case 58271:
          str = "leg";
          break;
        case 58331:
          str = "head";
          break;
        case 58866:
          str = "arm";
          break;
        case 58867:
          str = "arm";
          break;
        case 58868:
          str = "arm";
          break;
        case 58869:
          str = "arm";
          break;
        case 58870:
          str = "arm";
          break;
        case 60309:
          str = "arm";
          break;
        case 61007:
          str = "arm";
          break;
        case 61163:
          str = "arm";
          break;
        case 61839:
          str = "head";
          break;
        case 63931:
          str = "leg";
          break;
        case 64016:
          str = "arm";
          break;
        case 64017:
          str = "arm";
          break;
        case 64064:
          str = "arm";
          break;
        case 64065:
          str = "arm";
          break;
        case 64080:
          str = "arm";
          break;
        case 64081:
          str = "arm";
          break;
        case 64096:
          str = "arm";
          break;
        case 64097:
          str = "arm";
          break;
        case 64112:
          str = "arm";
          break;
        case 64113:
          str = "arm";
          break;
        case 64729:
          str = "body";
          break;
        case 65068:
          str = "head";
          break;
        case 65245:
          str = "leg";
          break;
        default:
          str = "unknown";
          break;
      }
      return str;
    }

    public static void ShowNotif(string message)
    {
      SetNotificationTextEntry("STRING");
      AddTextComponentString(message);
      DrawNotification(false, true);
    }

    public static async Task ShowBigMessage(string message, string subMessage, int time, bool click = false)
    {
      int scaleformHandle = RequestScaleformMovie("mp_big_message_freemode");
      float elapsed = 0;
      while (!HasScaleformMovieLoaded(scaleformHandle)) {
        await BaseScript.Delay(1);
      }

      if (click)
      {
        PlaySoundFrontend(-1, "HACKING_CLICK", "", true);
      }

      while (time > 0)
      {
        await BaseScript.Delay(0);
        BeginScaleformMovieMethod(scaleformHandle, "SHOW_SHARD_WASTED_MP_MESSAGE");
        PushScaleformMovieMethodParameterString(message);
        PushScaleformMovieMethodParameterString(subMessage);
        PushScaleformMovieMethodParameterInt(0);
        EndScaleformMovieMethod();
        DrawScaleformMovieFullscreen(scaleformHandle, 255, 255, 255, 255, 0);
        if(elapsed > 1)
        {
          elapsed = 0;
          if(time > 0)
            time--;
        }
        elapsed += GetFrameTime();
      }
    }
  }
}
