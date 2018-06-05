using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    abstract class Shape
    {
      
        private Brick[] bricks;
        private ColorTris shapeColor;

        public Shape(int numOfBricks, ColorTris mColor)
        {
            bricks = new Brick[numOfBricks];
            shapeColor = mColor;
        }

        public Shape(int numOfBricks)
        {
            bricks = new Brick[numOfBricks];
        }

        public ColorTris ShapeColor
        {
            get { return shapeColor; }
            set { shapeColor = value; }
        }

    }
}
