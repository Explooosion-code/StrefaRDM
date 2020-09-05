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
        public bool isYours = false;
        public bool isKiller = false;

        public KillFeedItem(string _kl, string _kld)
        {
            Killer = _kl.Length > 12 ? _kl.Substring(0, 10) + "..." : _kl;
            Killed = _kld.Length > 12 ? _kld.Substring(0, 10) + "..." : _kld;
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
            return Killer == "" ? $"{Killed}  ->   {Killed}" : $"{Killer}  ->  {Killed}";
        }
    }
}
