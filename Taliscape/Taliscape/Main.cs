using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Taliscape.Objects;
using Taliscape.Properties;
using System.Security.Cryptography;

namespace Taliscape
{
    public partial class Taliscape : Form
    {
        //For Everything
        Random random = new Random();

        //For GenerateGrid
        private const int GridWidth = 12;
        private const int GridHeight = 8;
        private const int TileSize = 64;
        private const int Spacing = 1;
        // Stores The Tiles In A Dictionary For Future Use
        private Dictionary<string, PictureBox> tiles = new Dictionary<string, PictureBox>();

        //For GameSetup
        private Button onePlayer;
        private Button twoPlayer;
        private Button threePlayer;
        private Button fourPlayer;

        //For StartGame
        private Label playerCountDisplay;
        private Label playerTurnDisplay;
        private Label playerMovesDisplay;
        private object[] playerList;
        private object[] playerSprites;

        //For GameTick
        public bool GamePlaying = false;
        private Button passTurn;
        private Button rollDice;
        public int turnOrder = 0;

        public Taliscape()
        {
            InitializeComponent();

            // Generate Game Map
            GenerateGrid();
            // Run Player Setup
            GameSetup();
        }

        private void GenerateGrid()
        {
            //Grid Parameters
            int gridWidth = GridWidth * (TileSize + Spacing) - Spacing;
            int gridHeight = GridHeight * (TileSize + Spacing) - Spacing;

            //Center Of Screen Calculation
            int startX = (ClientSize.Width - gridWidth) / 2;
            int startY = (ClientSize.Height - gridHeight) / 2;

            //Generate Grid
            for (int row = 0; row < GridHeight; row++)
            {
                for (int col = 0; col < GridWidth; col++)
                {
                    //Check For Edge/Corner Peice
                    if (row == 0 || row == GridHeight - 1 || col == 0 || col == GridWidth - 1)
                    {
                        BoardTile.TilePosition boardPos = new BoardTile.TilePosition(col, row);
                        BoardTile thisTile = new BoardTile();

                        PictureBox pictureBox = new PictureBox
                        {
                            Size = new Size(TileSize, TileSize),
                            Location = new Point(startX + col * (TileSize + Spacing), startY + row * (TileSize + Spacing)),
                            BackColor = Color.Black
                        };

                        //Village tile (Player Origin)
                        if (row == 0 && col == 0) 
                        {
                            string tileName = "Village";
                            pictureBox.Name = tileName;
                            // Store the reference to the picture box in the dictionary
                            tiles.Add(tileName, pictureBox);
                            thisTile.TileType = "Village";
                            pictureBox.Image = BoardTiles.test;
                            //Add ClickOnTile Function to the Tile
                            pictureBox.Click += (sender, e) => ClickOnTile(sender, e, (Player)playerList[turnOrder], tileName);

                        }
                        else if (row == 0 && col == 6)
                        {
                            string tileName = "Bandit Camp";
                            pictureBox.Name = tileName;
                            // Store the reference to the picture box in the dictionary
                            tiles.Add(tileName, pictureBox);
                            thisTile.TileType = "Bandit";
                            pictureBox.Image = BoardTiles.BanditCamp;
                            //Add ClickOnTile Function to the Tile
                            pictureBox.Click += (sender, e) => ClickOnTile(sender, e, (Player)playerList[turnOrder], tileName);
                        }
                        else
                        {
                            string tileName = $"Tile_{row}_{col}";
                            pictureBox.Name = tileName;
                            // Store the reference to the picture box in the dictionary
                            tiles.Add(tileName, pictureBox);
                            thisTile.TileType = "Wilderness";
                            pictureBox.Image = BoardTiles.EdgePiece_01;
                            //Add ClickOnTile Function to the Tile
                            pictureBox.Click += (sender, e) => ClickOnTile(sender, e, (Player)playerList[turnOrder], tileName);
                        }

                        Label label = new Label
                        {
                            Text = thisTile.TileType,
                            //Text = $"({col}, {row})", //For Testing
                            AutoSize = true,
                            BackColor = Color.Transparent,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 8),
                            Location = new Point(2, 2)
                        };

                        pictureBox.Controls.Add(label);

                        Controls.Add(pictureBox);
                        pictureBox.SendToBack();
                    }
                }
            }
        }

        private void GameSetup()
        {
            onePlayer = new Button
            {
                Location = new System.Drawing.Point(50, 80),
                Size = new System.Drawing.Size(85, 23),
                Text = "One Player"
            };
            onePlayer.Click += (sender, e) => StartGame(sender, e, 1);
            Controls.Add(onePlayer);

            twoPlayer = new Button
            {
                Location = new System.Drawing.Point(50, 110),
                Size = new System.Drawing.Size(85, 23),
                Text = "Two Players"
            };
            twoPlayer.Click += (sender, e) => StartGame(sender, e, 2);
            Controls.Add(twoPlayer);

            threePlayer = new Button
            {
                Location = new System.Drawing.Point(50, 140),
                Size = new System.Drawing.Size(85, 23),
                Text = "Three Players"
            };
            threePlayer.Click += (sender, e) => StartGame(sender, e, 3);
            Controls.Add(threePlayer);

            fourPlayer = new Button
            {
                Location = new System.Drawing.Point(50, 170),
                Size = new System.Drawing.Size(85, 23),
                Text = "Four Players"
            };
            fourPlayer.Click += (sender, e) => StartGame(sender, e, 4);
            Controls.Add(fourPlayer);

        }

        private void StartGame(object sender, EventArgs e, int playerCount)
        {

            // Delete Buttons
            Controls.Remove(onePlayer);
            Controls.Remove(twoPlayer);
            Controls.Remove(threePlayer);
            Controls.Remove(fourPlayer);

            // Get References To Starting Tile
            PictureBox tile = tiles["Village"]; // Access Village Tile
            Point tileLocation = tile.Location; // Retrieve the location of the tile

            // Setup Players
            playerList = new object[playerCount];
            playerSprites = new object[playerCount];

            for (int i = 0; i < playerCount; i++)
            { 
                // Player One
                if (i == 0)
                {
                    Player player = new Player
                    {
                        PlayerNumber = i,
                        Lives = 5,
                        Attack = 1,
                        Magic = 1,
                        Moves = 0,
                        OffsetY = 20,
                        OffsetX = 5,
                        HasPriority = true,
                    };
                    playerList[i] = player;


                    PictureBox playerSprite = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        Image = EntityIcons.PlayerOne,
                        Size = new Size(16, 16),
                        Location = new Point(tileLocation.X + player.OffsetX, tileLocation.Y + player.OffsetY),
                    };
                    Controls.Add(playerSprite);
                    playerSprite.BringToFront();
                    playerSprites[i] = playerSprite;
                }

                // Player Two
                else if (i == 1)
                {
                    Player player = new Player
                    {
                        PlayerNumber = i,
                        Lives = 5,
                        Attack = 1,
                        Magic = 1,
                        Moves = 0,
                        OffsetY = 45,
                        OffsetX = 5,
                        HasPriority = false,
                    };
                    playerList[i] = player;

                    PictureBox playerSprite = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        Image = EntityIcons.PlayerTwo,
                        Size = new Size(16, 16),
                        Location = new Point(tileLocation.X + player.OffsetX, tileLocation.Y + player.OffsetY),
                    };
                    Controls.Add(playerSprite);
                    playerSprite.BringToFront();
                    playerSprites[i] = playerSprite;
                }

                // Player Three
                else if (i == 2)
                {
                    Player player = new Player
                    {
                        PlayerNumber = i,
                        Lives = 5,
                        Attack = 1,
                        Magic = 1,
                        Moves = 0,
                        OffsetY = 45,
                        OffsetX = 43,
                        HasPriority = false,
                    };
                    playerList[i] = player;

                    PictureBox playerSprite = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        Image = EntityIcons.PlayerThree,
                        Size = new Size(16, 16),
                        Location = new Point(tileLocation.X + player.OffsetX, tileLocation.Y + player.OffsetY),
                    };
                    Controls.Add(playerSprite);
                    playerSprite.BringToFront();
                    playerSprites[i] = playerSprite;
                }

                // Player Four
                else if (i == 3)
                {
                    Player player = new Player
                    {
                        PlayerNumber = i,
                        Lives = 5,
                        Attack = 1,
                        Magic = 1,
                        Moves = 0,
                        OffsetY = 20,
                        OffsetX = 43,
                        HasPriority = false,
                    };
                    playerList[i] = player;

                    PictureBox playerSprite = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        Image = EntityIcons.PlayerFour,
                        Size = new Size(16, 16),
                        Location = new Point(tileLocation.X + player.OffsetX, tileLocation.Y + player.OffsetY),
                    };
                    Controls.Add(playerSprite);
                    playerSprite.BringToFront();
                    playerSprites[i] = playerSprite;
                }
            }
            GameTick(playerCount, playerList, playerSprites);
        }

        private void GameTick([Optional] int playerCount, [Optional] object[] playerList, [Optional] object[] playerSprites)
        {
            //Check Who Has Turn Priority
            for (int i = 0; i < playerCount; i++)
            {
                Player player = (Player)playerList[i];
                PictureBox playerSprite = (PictureBox)playerSprites[i];

                // Display Player Count
                playerCountDisplay = new Label
                {
                    Text = $"Players: {playerCount.ToString()}",
                    Location = new System.Drawing.Point(20, 70),
                    Size = new System.Drawing.Size(200, 20),
                    Font = new Font("Arial", 12),
                };
                Controls.Add(playerCountDisplay);

                if (player.HasPriority)
                {
                    playerSprite.BackColor = Color.Gold;

                    // Display The Current Players Turn
                    playerTurnDisplay = new Label
                    {
                        Text = $"It is Player {i + 1}'s turn",
                        Location = new System.Drawing.Point(20, 120),
                        Size = new System.Drawing.Size(250, 20),
                        Font = new Font("Arial", 12),
                    };
                    Controls.Add(playerTurnDisplay);

                    // Give The Active Player Control
                    rollDice = new Button
                    {
                        Location = new System.Drawing.Point(20, 250),
                        Size = new System.Drawing.Size(200, 80),
                        Text = "Roll Dice",
                        BackColor = Color.Gold,
                    };
                    Controls.Add(rollDice);
                    rollDice.Click += (sender, e) => GetPlayerMoves(sender, e, turnOrder, playerList);

                    passTurn = new Button
                    {
                        Location = new System.Drawing.Point(20, 180),
                        Size = new System.Drawing.Size(100, 40),
                        Text = $"Player {i + 1} Pass Turn",
                        BackColor = Color.DeepSkyBlue,
                    };
                    Controls.Add(passTurn);
                    passTurn.Click += (sender, e) => NextTurn(sender, e, turnOrder, playerCount, playerList);
                }
                else
                {
                    playerSprite.BackColor = Color.Black;
                };
            };
        }

        //Thanks Dom
        private void NextTurn(object sender, EventArgs e, int playerWithPriority, int playerCount, object[] playerList)
        {
            //Remove Old Text And Buttons
            Controls.Remove(playerTurnDisplay);
            Controls.Remove(playerMovesDisplay);
            Controls.Remove(rollDice);
            Controls.Remove(passTurn);

            // Get Player Reference Whose Turn Is Ending
            Player lastPlayer = (Player)playerList[(playerWithPriority + playerCount) % playerCount];

            // Get Reference to the Next Player in Turn Order
            int nextPlayerCount = (playerWithPriority + 1) % playerCount;
            Player nextPlayer = (Player)playerList[nextPlayerCount];

            // Change Turns
            lastPlayer.HasPriority = false;
            lastPlayer.Moves = 0;
            nextPlayer.HasPriority = true;

            // Resume Game And Pass Turn Order
            int nextTurnOrder = (turnOrder + 1) % playerCount;
            turnOrder = nextTurnOrder;
            GameTick(playerCount, playerList, playerSprites);
        }

        //Tile Click Registration
        private void ClickOnTile(object sender, EventArgs e, Player player, String tileName)
        {

            // Get References To Tiles
            PictureBox tile = tiles[tileName]; // Access Clicked Tile
            Point tileLocation = tile.Location; // Retrieve the location of the tile

            //Can Player Move
            if (player.HasPriority && player.Moves >= 1)
            {
                Point newLocation = new Point(tileLocation.X + player.OffsetX, tileLocation.Y + player.OffsetY);
                Controls.Remove(playerMovesDisplay);
                player.Moves -= 1;
                MovePlayer(player, newLocation);
            }
        }

        //Move The Player
        private void MovePlayer(Player player, Point newLocation)
        {
            // Move the player's PictureBox to the new location
            PictureBox playerSprite = (PictureBox)playerSprites[player.PlayerNumber];
            playerSprite.Location = newLocation;

            // Display Current Moves Available
            playerMovesDisplay = new Label
            {
                Text = $"Moves: {player.Moves}",
                Location = new System.Drawing.Point(20, 350),
                Size = new System.Drawing.Size(250, 20),
                Font = new Font("Arial", 12),
            };
            Controls.Add(playerMovesDisplay);
        }

        //Roll Player Moves For Turn
        private void GetPlayerMoves(object sender, EventArgs e, int playerWithPriority, object[] playerList)
        {

            //Remove Old Text
            Controls.Remove(rollDice);
            Controls.Remove(playerMovesDisplay);

            Player player = (Player)playerList[playerWithPriority];
            player.Moves = random.Next(1, 6);

            // Display Current Moves Available
            playerMovesDisplay = new Label
            {
                Text = $"Moves: {player.Moves}",
                Location = new System.Drawing.Point(20, 350),
                Size = new System.Drawing.Size(250, 20),
                Font = new Font("Arial", 12),
            };
            Controls.Add(playerMovesDisplay);

        }

    }
}
