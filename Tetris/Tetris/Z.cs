using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class Z : Shape
    {
        private const int height = 2;
        private const int width = 3;
        private bool rightLeaning;

        public int Height { get; set; }

        public int Width { get; set; }

        public bool RightLeaning { 
            
            get { return rightLeaning; }
            set { rightLeaning = value; } 
        }

        public Z(int numOfBricks, bool leaning) : base(numOfBricks)
        {
            rightLeaning = leaning;
            if (rightLeaning)
                base.ShapeColor = ColorTris.VIOLET;
            else
                base.ShapeColor = ColorTris.MAGENTA;
        }
    }
}
