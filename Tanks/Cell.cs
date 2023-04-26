using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
    public abstract class Cell
    {
        internal Image image;
        public Cell(Image image)
        {
            this.image = image;
        }
        internal PictureBox getPicBox(int cellSize,  int i, int j)
        {
            PictureBox picBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(i * cellSize, j * cellSize),
                Size = new Size(cellSize, cellSize),
                Image = new Bitmap(this.image)
            };
            return picBox;
        }
    }

    internal sealed class EmptyCell : Cell
    {
        public EmptyCell() : base(Image.FromFile("Images\\emptyCell.png")) { }
    }

    internal sealed class PlayerTank1 : Cell
    {
        public PlayerTank1(Player player) : base(Image.FromFile($"Images\\tank1_rot{player.playerTank.rotation}.png")) { }
    }

    internal sealed class PlayerTank2 : Cell
    {
        public PlayerTank2(Player player) : base(Image.FromFile($"Images\\tank2_rot{player.playerTank.rotation}.png")) { }
    }

    internal sealed class Buttet : Cell
    {
        public Buttet() : base(Image.FromFile("Images\\bullet.png")) { }
    }

    internal sealed class Wall : Cell
    {
        public Wall() : base(Image.FromFile("Images\\wall.png")) { }
    }
}
