using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    internal sealed class GameMap
    {
        private Cell[,] map { get; set; }
        internal PictureBox[,] images { get; set; }
        private bool playerFlag { get; set; }
        internal readonly int cellSize;
        internal readonly int mapSize;
        private Player player1;
        private Player player2;
        private Label playerStatus1;
        private Label playerStatus2;

        public GameMap(int mapSize, int cellSize)
        {
            this.mapSize = mapSize;
            this.cellSize = cellSize;
            this.map = new Cell[mapSize, mapSize];
            this.images = new PictureBox[mapSize, mapSize];
            this.player1 = new Player(new Tank(0, 0, 180));
            this.player2 = new Player(new Tank(mapSize - 1, mapSize - 1, 0));
            this.playerFlag = true;
            this.playerStatus1 = new Label
            {
                Text = $"player 1: {player1.lives} lives\naction points: {player1.actionPoints}",
                Location = new Point((int)(cellSize * (mapSize + 0.5)), cellSize),
                Size = new Size((int)(cellSize * 2.5), cellSize),
                TabStop = false,
                BackColor = Color.Green
            };
            this.playerStatus2 = new Label
            {
                Text = $"player 2: {player2.lives} lives\naction points: {player2.actionPoints}",
                Location = new Point((int)(cellSize * (mapSize + 0.5)), this.playerStatus1.Location.Y + cellSize),
                Size = new Size((int)(cellSize * 2.5), cellSize),
                TabStop = false,
                BackColor = Color.Orange
            };
        }

        internal void DrawStartMap(Form form)
        {
            if (player1.lives == 0)
            {
                EndGame(form, new PlayerWin2());
            }
            if (player2.lives == 0)
            {
                EndGame(form, new PlayerWin1());
            }

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = new EmptyCell();
                    images[i, j] = map[i, j].getPicBox(cellSize, i, j);
                    form.Controls.Add(images[i, j]);
                }
            }

            ChangeCell(form, player1.playerTank.xPos, player1.playerTank.yPos, new PlayerTank1(player1));
            ChangeCell(form, player2.playerTank.xPos, player2.playerTank.yPos, new PlayerTank2(player2));

            form.Controls.Add(playerStatus1);
            form.Controls.Add(playerStatus2);
        }

        private async void EndGame(Form curForm, Form openForm)
        {
            openForm.Show();
            curForm.Enabled = false;
            await Task.Delay(3000);
            curForm.Close();
        }

        private void hitHandler(Form form, int xPos, int yPos)
        {
            if (map[xPos, yPos].GetType() == typeof(PlayerTank2))
            {
                TankDestroyed(form, player2);
            }
            if (map[xPos, yPos].GetType() == typeof(PlayerTank1))
            {
                TankDestroyed(form, player1);
            }
        }

        internal async void TankFire(Form form)
        {
            Player curPlayer = (playerFlag) ? player1 : player2;
            if (curPlayer.actionPoints < 2) return;

            curPlayer.actionPoints -= 2; 
            
            updateStatus(form);
                
            switch (curPlayer.playerTank.rotation)
            {
                case 0:
                    int nextPos = curPlayer.playerTank.yPos - 1;
                    while (nextPos >= 0)
                    {
                        hitHandler(form, curPlayer.playerTank.xPos, nextPos);
                        ChangeCell(form, curPlayer.playerTank.xPos, nextPos, new Buttet());
                            
                        await Task.Delay(50);
                    
                        ChangeCell(form, curPlayer.playerTank.xPos, nextPos, new EmptyCell());
                        nextPos--;
                    }
                    break;
                case 90:
                    nextPos = curPlayer.playerTank.xPos + 1;
                    while (nextPos < mapSize)
                    {
                        hitHandler(form, nextPos, curPlayer.playerTank.yPos);    
                        ChangeCell(form, nextPos, curPlayer.playerTank.yPos, new Buttet());
                        
                        await Task.Delay(50);
                        
                        ChangeCell(form, nextPos, curPlayer.playerTank.yPos, new EmptyCell());
                        nextPos++;
                    }
                    break;
                case 180:
                    nextPos = player1.playerTank.yPos + 1;
                    while (nextPos < mapSize)
                    {
                        hitHandler(form, curPlayer.playerTank.xPos, nextPos);
                        ChangeCell(form, curPlayer.playerTank.xPos, nextPos, new Buttet());
                        
                        await Task.Delay(50);
                        
                        ChangeCell(form, curPlayer.playerTank.xPos, nextPos, new EmptyCell());
                        nextPos++;
                    }
                    break;
                case 270:
                    nextPos = curPlayer.playerTank.xPos - 1;
                    while (nextPos >= 0)
                    {
                        hitHandler(form, nextPos, curPlayer.playerTank.yPos);
                        ChangeCell(form, nextPos, curPlayer.playerTank.yPos, new Buttet());
                        
                        await Task.Delay(50);
                        
                        ChangeCell(form, nextPos, curPlayer.playerTank.yPos, new EmptyCell());
                        nextPos--;
                    }
                    break;
            }
            if (curPlayer.actionPoints == 0) { passTheMove(form); return; }   
        }

        internal void TankMove(Form form, string dir)
        {
            Player curPlayer = (playerFlag) ? player1 : player2;

            int mul = (dir == "F") ? 1 : -1;
            
            if (curPlayer.playerTank.rotation == 0)
            {
                int nextPos = curPlayer.playerTank.yPos - mul * 1;
                if (nextPos >= mapSize || nextPos < 0) return;
                ChangeCell(form, curPlayer.playerTank.xPos, curPlayer.playerTank.yPos, new EmptyCell());
                curPlayer.playerTank.yPos = nextPos;
            }
            if (curPlayer.playerTank.rotation == 90)
            {
                int nextPos = curPlayer.playerTank.xPos + mul * 1;
                if (nextPos >= mapSize || nextPos < 0) return;
                ChangeCell(form, curPlayer.playerTank.xPos, curPlayer.playerTank.yPos, new EmptyCell());
                curPlayer.playerTank.xPos = nextPos;
            }
            if (curPlayer.playerTank.rotation == 180)
            {
                int nextPos = curPlayer.playerTank.yPos + mul * 1;
                if (nextPos >= mapSize || nextPos < 0) return;
                ChangeCell(form, curPlayer.playerTank.xPos, curPlayer.playerTank.yPos, new EmptyCell());
                curPlayer.playerTank.yPos = nextPos;
            }
            if (curPlayer.playerTank.rotation == 270)
            {
                int nextPos = curPlayer.playerTank.xPos - mul * 1;
                if (nextPos >= mapSize || nextPos < 0) return;
                ChangeCell(form, curPlayer.playerTank.xPos, curPlayer.playerTank.yPos, new EmptyCell());
                curPlayer.playerTank.xPos = nextPos;
            }
                
            curPlayer.actionPoints--;
                
            updateStatus(form);

            Cell curPlayerCell;
            
            if (playerFlag) curPlayerCell = new PlayerTank1(curPlayer);
            else curPlayerCell = new PlayerTank2(curPlayer);

            ChangeCell(form, curPlayer.playerTank.xPos, curPlayer.playerTank.yPos, curPlayerCell);
                
            if (player1.playerTank.xPos == player2.playerTank.xPos && player1.playerTank.yPos == player2.playerTank.yPos) 
                if (playerFlag) TankDestroyed(form, player2);
                else TankDestroyed(form, player1);
            
            if (curPlayer.actionPoints == 0) { passTheMove(form); return; }
        }

        private void TankDestroyed(Form form, Player player)
        {
            player.lives--;
            
            updateStatus(form);
            
            player1.playerTank.xPos = player1.playerTank.yPos = 0;
            player2.playerTank.xPos = player2.playerTank.yPos = mapSize - 1;
            
            player1.playerTank.rotation = 180;
            player2.playerTank.rotation = 0;
            
            form.Controls.Clear();
            
            DrawStartMap(form);
            
            player2.actionPoints = Player.MAX_ACTION_POINTS_COUNT;
            player1.actionPoints = Player.MAX_ACTION_POINTS_COUNT;
            
            updateStatus(form);
        }

        private void updateStatus(Form form)
        {
            playerStatus1.Text = $"player 1: {player1.lives} lives\naction points: {player1.actionPoints}";
            playerStatus2.Text = $"player 2: {player2.lives} lives\naction points: {player2.actionPoints}";
        }

        internal void TankRotate(Form form, string dir)
        {
            Player curPlayer = (playerFlag) ? player1 : player2;
            if (curPlayer.actionPoints == 0) { passTheMove(form); return; }
            
            int value = (dir == "R") ? 90 : -90;
            curPlayer.playerTank.rotation += value;
            
            if (curPlayer.playerTank.rotation < 0) curPlayer.playerTank.rotation = 270;
            if (curPlayer.playerTank.rotation > 270) curPlayer.playerTank.rotation = 0;
            
            Cell curPlayerCell;
            if (playerFlag)
                curPlayerCell = new PlayerTank1(player1);
            else
                curPlayerCell = new PlayerTank2(player2);
            ChangeCell(form, curPlayer.playerTank.xPos, curPlayer.playerTank.yPos, curPlayerCell);
        }

        private void ChangeCell(Form form, int xPos, int yPos, Cell cell)
        {
            form.Controls.Remove(images[xPos, yPos]);
            map[xPos, yPos] = cell;
            images[xPos, yPos] = map[xPos, yPos].getPicBox(cellSize, xPos, yPos);
            form.Controls.Add(images[xPos, yPos]);
        }

        internal void passTheMove(Form form)
        {
            playerFlag = !playerFlag;
            player2.actionPoints = Player.MAX_ACTION_POINTS_COUNT; 
            player1.actionPoints = Player.MAX_ACTION_POINTS_COUNT;
            updateStatus(form);
            turnAnnounce();
        }

        internal void turnAnnounce()
        {
            if (playerFlag) MessageBox.Show("Now it's player 1 turn!");
            else MessageBox.Show("Now it's player 2 turn!");
        }
    }
}
