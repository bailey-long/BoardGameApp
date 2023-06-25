using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taliscape.Objects
{
    internal class Player
    {
        public int PlayerNumber { get; set; }
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
