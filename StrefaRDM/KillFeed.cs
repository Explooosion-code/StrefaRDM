using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM
{
    public class KillFeed : BaseScript
    {
        List<Player> DeadPlayers = new List<Player>();
        List<KillFeedItem> items = new List<KillFeedItem>();
        public KillFeed()
        {
            Tick += onTick;
            Tick += drawTick;
        }

        async Task onTick()
        {
            foreach(Player p in Players.ToList())
            {
                if(p.IsDead)
                {
                    if(!DeadPlayers.Contains(p))
                    {
                        DeadPlayers.Add(p);
                        Entity killer = p.Character.GetKiller();
                        if (killer != null)
                        {
                            if (killer.Exists())
                            {
                                if (killer.Model.IsPed)
                                {
                                    bool found = false;

                                    if(killer.Handle == Game.PlayerPed.Handle)
                                    {
                                        Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
                                    }

                                    foreach (Player playerKiller in Players.ToList())
                                    {
                                        if (playerKiller.Character.Handle == killer.Handle)
                                        {
                                            bool wasHeadshot = Utils.GetPedHitLocation(p.Character.Handle) == "head";
                                            items.Add(new KillFeedItem(playerKiller.Name, p.Name, wasHeadshot));
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                    {
                                        items.Add(new KillFeedItem("", p.Name));
                                    }
                                }
                                else
                                {
                                    items.Add(new KillFeedItem("", p.Name));
                                }
                            }
                            else
                            {
                                items.Add(new KillFeedItem("", p.Name));
                            }
                        }
                        else
                        {
                            items.Add(new KillFeedItem("", p.Name));
                        }
                    }
                } else
                {
                    if(DeadPlayers.Contains(p))
                    {
                        DeadPlayers.Remove(p);
                    }
                }
            }
        }

        async Task drawTick()
        {

            if(items.Count > 0)
            {
                int i = 0;
                float offset = 0.04f;
                List<KillFeedItem> toRemove = new List<KillFeedItem>();
                foreach(KillFeedItem kfi in items) 
                {
                    if (kfi.timeleft == 0)
                    {
                        toRemove.Add(kfi);
                    }
                    else
                    {
                        float realoffset = i * offset;
                        string text = kfi.GetDisplay();
                        float rightOffset = (float)text.Length / 470.0f;
                        SetTextScale(0.4f, 0.4f);
                        SetTextFont(4);
                        SetTextProportional(true);
                        if (kfi.isYours)
                        {
                            if(kfi.isKiller)
                            {
                                SetTextColour(47, 176, 35, 215);
                            }
                            else
                            {
                                SetTextColour(191, 27, 84, 215); 
                            }
                        }
                        else
                        {
                            SetTextColour(255, 255, 255, 215);
                        }
                        SetTextOutline();
                        SetTextEntry("STRING");
                        SetTextCentre(false);
                        AddTextComponentString(text);
                        DrawText(0.87f - rightOffset, 0.05f + realoffset);
                        kfi.frameTime += GetFrameTime();
                        int r = 0;
                        int g = 0;
                        int b = 0;
                        if(kfi.isYours)
                        {
                            r = 180;
                            g = 233;
                            b = 237;
                        }
                        DrawRect(0.95f - rightOffset, 0.0625f + realoffset, .3f, 0.03f, r, g, b, 50);
                        if (kfi.frameTime > 1f)
                        {
                            kfi.frameTime = 0;
                            kfi.timeleft--;
                        }
                        i++;
                    }
                }

                if(toRemove.Count > 0)
                {
                    foreach(KillFeedItem kfi in toRemove)
                    {
                        items.Remove(kfi);
                    }
                }
            }
        }
    }
}
