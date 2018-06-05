using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class T : Shape
    {

        private const int height = 2;
        private const int width = 3;

        public int Height { get; set; }

        public int Width { get; set; }


        public T(int numOfBricks, ColorTris mColor) : base(numOfBricks, mColor)
        {
            
        }
    }
}
