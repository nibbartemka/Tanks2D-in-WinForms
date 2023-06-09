using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tanks
{
    public partial class Form1 : Form
    {
        GameMap gameMap = new GameMap(10, 50);
        public Form1()
        {
            InitializeComponent();
            this.Width = gameMap.cellSize * (gameMap.mapSize + 4);
            this.Height = gameMap.cellSize * (gameMap.mapSize + 2);
            gameMap.DrawStartMap(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameMap.turnAnnounce();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    gameMap.TankRotate(this, "R");
                    break;
                case Keys.A:
                    gameMap.TankRotate(this, "L");
                    break;
                case Keys.W:
                    gameMap.TankMove(this, "F");
                    break;
                case Keys.S:
                    gameMap.TankMove(this, "B");
                    break;
                case Keys.Q:
                    gameMap.passTheMove(this);
                    break;
                case Keys.Space:
                    gameMap.TankFire(this);
                    break;
            }
        }
    }
}
