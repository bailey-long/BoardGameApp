using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taliscape.Objects
{
    internal class BoardTile
    {
        public string TileType { get; private set; }
        public readonly struct TilePosition
        {
            public int X { get; }
            public int Y { get; }

            public TilePosition(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
