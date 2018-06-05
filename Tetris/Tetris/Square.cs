using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class Square : Shape
    {
       
        private const int height = 20;
        private const int width = 20;

        public Square(int numOfBricks, ColorTris mColor) : base(numOfBricks, mColor)
        {
            
        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }
        
    }
}
