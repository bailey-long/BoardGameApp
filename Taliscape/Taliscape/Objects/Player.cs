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
        public string SpritePath { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
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

        public void GetVisuals()
        {
            switch (PlayerNumber)
            {
                case 0:
                    SpritePath = "C:\\Users\\Loopypew\\workspace\\BoardGameApp\\Taliscape\\Taliscape\\Sprites\\EntityIcons\\Players\\PlayerOne.png";
                    break;

                case 1:
                    SpritePath = "C:\\Users\\Loopypew\\workspace\\BoardGameApp\\Taliscape\\Taliscape\\Sprites\\EntityIcons\\Players\\PlayerTwo.png";
                    break;

                case 2:
                    SpritePath = "C:\\Users\\Loopypew\\workspace\\BoardGameApp\\Taliscape\\Taliscape\\Sprites\\EntityIcons\\Players\\PlayerThree.png";
                    break;

                case 3:
                    SpritePath = "C:\\Users\\Loopypew\\workspace\\BoardGameApp\\Taliscape\\Taliscape\\Sprites\\EntityIcons\\Players\\PlayerFour.png";
                    break;
            }
        }

    }
}
