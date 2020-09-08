using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM
{
    public class PlayerList : BaseScript
    {
        int scaleform = -1;
        List<dynamic> playersInfo = new List<dynamic>();
        bool tickon = false;

        public PlayerList()
        {
            EventHandlers["srdm:syncPlayerData"] += new Action<List<dynamic>>(playersData =>
            {
                playersInfo = playersData;
            });

            EventHandlers["playerSpawned"] += new Action(() =>
            {
                if (!tickon)
                {
                    tickon = true;
                    Tick += onTick;
                }
            });
        }

        async Task onTick()
        {
            if(Game.IsControlJustPressed(0, Control.MultiplayerInfo))
            {
                await initSF();
                while (Game.IsControlPressed(0, Control.MultiplayerInfo))
                {
                    await Delay(0);
                    draw();
                }
            }
        }

        async Task initSF()
        {
            if (scaleform != -1) {
                SetScaleformMovieAsNoLongerNeeded(ref scaleform);
                await Delay(1);
            }

            scaleform = RequestScaleformMovie("mp_online_list_card");
            while (!HasScaleformMovieLoaded(scaleform)) {
                await Delay(1);
            }

            generatePage();
        }

        void generatePage()
        {
            PushScaleformMovieFunction(scaleform, "ADD_SLOT");
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterInt(0); 
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterString("~b~Nick (ID)");
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterString("~b~Kills");
            PopScaleformMovieFunctionVoid();
            int i = 0;
            foreach(var player in playersInfo)
            {
                if (i > 14)
                {
                    break;
                }
                PushScaleformMovieFunction(scaleform, "ADD_SLOT");
                PushScaleformMovieFunctionParameterInt(i + 1);
                PushScaleformMovieFunctionParameterInt(1);
                PushScaleformMovieFunctionParameterInt(i);
                PushScaleformMovieFunctionParameterInt(2);
                PushScaleformMovieFunctionParameterInt(0);
                PushScaleformMovieFunctionParameterInt(0);
                string name = player.name;
                int id = player.sourceId;
                int kills = player.sessionKills;

                if(id == Game.Player.ServerId)
                    PushScaleformMovieFunctionParameterString($"~g~{name}  ({id})");
                else
                    PushScaleformMovieFunctionParameterString($"{name}  ({id})");

                PushScaleformMovieFunctionParameterInt(0);
                PushScaleformMovieFunctionParameterInt(0);
                if (id == Game.Player.ServerId)
                    PushScaleformMovieFunctionParameterString("~g~" + kills.ToString());
                else
                    PushScaleformMovieFunctionParameterString(kills.ToString()); ;

                PushScaleformMovieFunctionParameterBool(false);
                PushScaleformMovieFunctionParameterBool(false);

                PushScaleformMovieFunctionParameterBool(true);


                PopScaleformMovieFunctionVoid();
                i++;
            }
        }

        void draw()
        {
            PushScaleformMovieFunction(scaleform, "SET_TITLE");
            PushScaleformMovieFunctionParameterString("~b~Statystyki graczy online~w~");
            PopScaleformMovieFunctionVoid();
            DrawScaleformMovie(scaleform, 0.5f, 0.45f, 0.25f, 0.7f, 255, 255, 255, 255, 0);
        }
    }
}
