using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Taliscape.Properties;

namespace Taliscape.Objects
{
    internal class Player
    {
        public int PlayerNumber { get; set; }
        public string SpritePath { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Moves { get; set; }
        public bool HasPriority { get; set; }
        public int SpacesToMove { get; set; }
        public int Lives { get; set; }
        public int Attack { get; set; }
        public int Magic { get; set; }
        public int Gold { get; set; }
        public object[] Inventory { get; private set; }

        public Player()
        {
            Inventory = new object[]{
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };
        }
    }
}
