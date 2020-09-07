using System.Collections.Generic;
using CitizenFX.Core;

namespace StrefaRDM_Server
{
    public struct Position
    {
        public string Name;
        public Vector3 Center;
        public int MaxDist;
        public List<Vector3> Respawns;

        public Position(string _name, Vector3 _center, int _maxDist, List<Vector3> _respawns)
        {
            Name = _name;
            Center = _center;
            MaxDist = _maxDist;
            Respawns = _respawns;
        }
    }
}