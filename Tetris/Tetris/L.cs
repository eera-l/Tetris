using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class L : Shape
    {

        private const int height = 3;
        private const int width = 2;
        private bool rightLeaning;

        public int Height { get; set; }

        public int Width { get; set; }

        public bool RightLeaning
        {
            get { return rightLeaning; }
            set { rightLeaning = value; }
        }

        public L(int numOfBricks, bool leaning) : base(numOfBricks)
        {
            rightLeaning = leaning;
            if (rightLeaning)
                base.ShapeColor = ColorTris.GREEN;
            else
                base.ShapeColor = ColorTris.ORANGE;
        }
    }
}
