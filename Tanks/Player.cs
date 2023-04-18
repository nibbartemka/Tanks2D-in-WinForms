using System;
using System.Collections.Generic;
using System.Text;

namespace Tanks
{
    internal sealed class Player
    {
        private const int MAX_LIVES_COUNT = 3;
        internal const int MAX_ACTION_POINTS_COUNT = 5;
        internal int lives { get; set; }
        internal readonly Tank playerTank;
        internal int actionPoints { get; set; }
        public Player(Tank playerTank)
        {
            this.playerTank = playerTank;
            this.lives = MAX_LIVES_COUNT;
            this.actionPoints = MAX_ACTION_POINTS_COUNT;
        }
    }
}
