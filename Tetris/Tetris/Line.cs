using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class Line : Shape
    {

        public const int height = 4;
        public const int width = 1;

        public Line(int numOfBricks, ColorTris mColor) : base(numOfBricks, mColor)
        {
            
        }
    }
}
