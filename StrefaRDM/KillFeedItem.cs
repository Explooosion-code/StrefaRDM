using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace StrefaRDM
{
    public class KillFeedItem
    {
        public string Killer, Killed;
        public float frameTime;
        public int timeleft = 8; // sec
        public bool isYours;
        public bool isKiller;
        public bool isHeadShot;

        public KillFeedItem(string _kl, string _kld, bool headshot = false)
        {
            Killer = _kl.Length > 12 ? _kl.Substring(0, 10) + "..." : _kl;
            Killed = _kld.Length > 12 ? _kld.Substring(0, 10) + "..." : _kld;
            isHeadShot = headshot;
            if(_kl == Game.Player.Name)
            {
                isYours = true;
                isKiller = true;
            } else if (_kld == Game.Player.Name)
            {
                isYours = true;
                isKiller = false;
            }
        }

        public string GetDisplay()
        {
            if (isHeadShot)
            {
                return Killer == "" ? $"{Killed}  -> 💀 {Killed}" : $"{Killer}  -> 💀 {Killed}";
            }
            return Killer == "" ? $"{Killed}  ->   {Killed}" : $"{Killer}  ->  {Killed}";
        }
    }
}
