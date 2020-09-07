using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace StrefaRDM_Server
{
    public static class Config
    {
        public static int MapChangeTime = 2700;
        public static Dictionary<int, string> Weapons = new Dictionary<int, string>()
        {
           [0] = "WEAPON_SNSPISTOL",
           [10] = "WEAPON_SNSPISTOL_MK2",
           [20] = "WEAPON_PISTOL",
           [30] = "WEAPON_PISTOL_MK2",
           [40] = "WEAPON_VINTAGEPISTOL",
           [50] = "WEAPON_COMBATPISTOL",
           [60] = "WEAPON_HEAVYPISTOL",
           [70] = "WEAPON_PISTOL50",
           [80] = "WEAPON_MARKSMANPISTOL"
        };

        public static List<Position> Positions = new List<Position>()
        {
            new Position("Obóz altruistów", new Vector3(-1116.06f, 4924.5f, 218.12f), 90, new List<Vector3>()
            {
                new Vector3(-1139.475f, 4964.874f, 222.2f),
                new Vector3(-1096.64f, 4949.015f, 218.35f),
                new Vector3(-1098.707f, 4893.458f, 216.06f),
                new Vector3(-1149.881f, 4896.621f, 218.015f),
                new Vector3(-1117.222f, 4909.602f, 218.595f),
                new Vector3(-1144.576f, 4908.97f, 220.015f),
                new Vector3(-1176.066f, 4926.375f, 223.335f),
                new Vector3(-1137.537f, 4873.377f, 215.1685f),
                new Vector3(-1116.98f, 4870.667f, 213.9198f),
                new Vector3(-1100.748f, 4871.551f, 216.0679f),
                new Vector3(-1093.014f, 4876.756f, 216.1674f),
                new Vector3(-1078.978f, 4887.657f, 214.5597f),
                new Vector3(-1069.12f, 4895.438f, 214.2716f),
                new Vector3(-1058.847f, 4914.82f, 211.8191f),
                new Vector3(-1046.577f, 4917.658f, 212.529f),
                new Vector3(-1080.473f, 4937.037f, 213.1833f),
                new Vector3(-1080.183f, 4939.354f, 217.5342f),
                new Vector3(-1101.18f, 4940.833f, 218.3542f),
                new Vector3(-1110.891f, 4935.228f, 218.3636f),
                new Vector3(-1133.892f, 4947.772f, 222.2696f),
                new Vector3(-1135.602f, 4940.981f, 222.2687f),
                new Vector3(-1145.813f, 4939.789f, 222.2687f),
                new Vector3(-1148.433f, 4950.222f, 226.2442f),
                new Vector3(-1176.49f, 4925.866f, 223.3712f),
                new Vector3(-1144.94f, 4908.629f, 220.9688f),
                new Vector3(-1125.888f, 4906.435f, 218.595f),
                new Vector3(-1122.096f, 4896.856f, 217.2648f),
                new Vector3(-1105.77f, 4905.598f, 216.3882f),
            }),
            
            new Position("Grove St.", new Vector3(81.34679f, -1919.468f, 21.28778f), 100, new List<Vector3>()
            {
                new Vector3(67.59438f, -1963.655f, 20.90981f),
                new Vector3(62.59244f, -1959.223f, 20.90981f),
                new Vector3(61.68524f, -1946.939f, 20.90981f),
                new Vector3(57.29707f, -1932.395f, 21.53981f),
                new Vector3(42.09629f, -1939.189f, 21.53981f),
                new Vector3(41.76267f, -1928.778f, 21.53981f),
                new Vector3(49.43159f, -1918.765f, 21.53981f),
                new Vector3(58.1202f, -1919.424f, 21.53981f),
                new Vector3(102.4601f, -1971.691f, 20.7838f),
                new Vector3(97.3771f, -1991.405f, 20.7838f),
                new Vector3(128.6842f, -1941.918f, 20.6158f),
                new Vector3(137.9398f, -1946.549f, 20.3428f),
                new Vector3(129.9886f, -1958.876f, 18.43179f),
                new Vector3(130.4154f, -1916.992f, 20.8888f),
                new Vector3(140.0901f, -1920.691f, 20.8888f),
                new Vector3(88.21669f, -1907.991f, 21.1618f),
                new Vector3(98.16854f, -1905.216f, 21.1618f),
                new Vector3(99.48105f, -1914.114f, 21.1618f),
                new Vector3(137.5636f, -1893.168f, 23.3458f),
                new Vector3(172.5484f, -1913.888f, 20.8888f),
                new Vector3(137.4298f, -1967.123f, 22.2538f),
            })
        };
    }
}