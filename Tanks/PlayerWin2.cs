using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tanks
{
    public partial class PlayerWin2 : Form
    {
        public PlayerWin2()
        {
            InitializeComponent();
            PictureBox picBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(0, 0),
                Size = new Size(this.Width, this.Height),
                Image = new Bitmap(Image.FromFile($"Images\\PlayerWin2.png"))
            };
            this.Controls.Add(picBox);
        }

        private void PlayerWin2_Load(object sender, EventArgs e)
        {

        }
    }
}
