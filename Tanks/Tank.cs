using System;
using System.Collections.Generic;
using System.Text;

namespace Tanks
{
    internal sealed class Tank
    {
        internal int xPos { get; set; }
        internal int yPos { get; set; }
        internal int rotation { get; set; }

        public Tank(int xPos, int yPos, int rotation)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.rotation = rotation;
        }
    }
}
