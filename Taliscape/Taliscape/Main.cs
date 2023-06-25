using System;
using System.Drawing;
using System.Windows.Forms;
using Taliscape.Objects;

namespace Taliscape
{
    public partial class Taliscape : Form
    {
        private const int GridWidth = 12;
        private const int GridHeight = 8;
        private const int TileSize = 64;
        private const int Spacing = 1;

        public Taliscape()
        {
            InitializeComponent();

            // Generate Game Map
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            int gridWidth = GridWidth * (TileSize + Spacing) - Spacing;
            int gridHeight = GridHeight * (TileSize + Spacing) - Spacing;

            int startX = (ClientSize.Width - gridWidth) / 2;
            int startY = (ClientSize.Height - gridHeight) / 2;

            for (int row = 0; row < GridHeight; row++)
            {
                for (int col = 0; col < GridWidth; col++)
                {
                    if (row == 0 || row == GridHeight - 1 || col == 0 || col == GridWidth - 1)
                    {
                        BoardTile.TilePosition position = new BoardTile.TilePosition(col, row);

                        PictureBox pictureBox = new PictureBox
                        {
                            Size = new Size(TileSize, TileSize),
                            Location = new Point(startX + col * (TileSize + Spacing), startY + row * (TileSize + Spacing)),
                            Image = Image.FromFile("C:\\Users\\Loopypew\\workspace\\BoardGameApp\\Taliscape\\Taliscape\\Sprites\\BoardTiles\\edgePiece_01.png"),
                            BackColor = Color.Black
                        };

                        Label label = new Label
                        {
                            Text = $"({col}, {row})",
                            AutoSize = true,
                            BackColor = Color.Transparent,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 8),
                            Location = new Point(2, 2)
                        };

                        if (row == 0 && col == 0) 
                        {
                            label.Text = "The Village";
                            pictureBox.Image = Image.FromFile("C:\\Users\\Loopypew\\workspace\\BoardGameApp\\Taliscape\\Taliscape\\Sprites\\BoardTiles\\Village1.png");
                        }

                        pictureBox.Controls.Add(label);

                        Controls.Add(pictureBox);
                    }
                }
            }
        }
    }
}
