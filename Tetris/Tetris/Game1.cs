using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Shape[] fallingShapes = new Shape[100];
        private Rectangle[,] rects = new Rectangle[100, 4];
        private Texture2D[] textures = new Texture2D[100];
        private Texture2D blueBrick;
        private Texture2D greenBrick;
        private Texture2D magentaBrick;
        private Texture2D orangeBrick;
        private Texture2D tealBrick;
        private Texture2D violetBrick;
        private Texture2D yellowBrick;
        private float timer;
        private float timerDescent;
        private float timerDescAccelerated;
        private float timerSet;
        private Random rnd = new Random();
        private int counter = 0;
        private bool going = true;
        private bool once = true;
        private bool verticalLine = false;
        private bool shiftedZ = false;
        private byte lRotation = 0;
        private byte tRotation = 0;
        private bool collision = false;
        private bool onceBottom = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 340;
            graphics.PreferredBackBufferHeight = 520;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Texture2D(GraphicsDevice, 1, 1);
            }

            fallingShapes[counter] = new Square(4, ColorTris.BLUE);
            rects[counter, 0] = new Rectangle(160, 20, 20, 20);
            rects[counter, 1] = new Rectangle(180, 20, 20, 20);
            rects[counter, 2] = new Rectangle(160, 0, 20, 20);
            rects[counter, 3] = new Rectangle(180, 0, 20, 20);
            textures[counter] = new Texture2D(GraphicsDevice, 1, 1);
            textures[counter].SetData(new Color[] { Color.Transparent });
            counter++;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            blueBrick = Content.Load<Texture2D>("blue");
            textures[counter - 1] = blueBrick;
            greenBrick = Content.Load<Texture2D>("green");
            magentaBrick = Content.Load<Texture2D>("magenta");
            orangeBrick = Content.Load<Texture2D>("orange");
            tealBrick = Content.Load<Texture2D>("teal");
            violetBrick = Content.Load<Texture2D>("violet");
            yellowBrick = Content.Load<Texture2D>("yellow");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            // TODO: Add your update logic here
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timerDescent += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timerDescAccelerated += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timerSet += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i <= counter; i++)
            {
                Rectangle r0 = new Rectangle(rects[counter - 1, 0].X, rects[counter - 1, 0].Y + 5, 20, 20);
                Rectangle r1 = new Rectangle(rects[counter - 1, 1].X, rects[counter - 1, 1].Y + 5, 20, 20);
                Rectangle r2 = new Rectangle(rects[counter - 1, 2].X, rects[counter - 1, 2].Y + 5, 20, 20);
                Rectangle r3 = new Rectangle(rects[counter - 1, 3].X, rects[counter - 1, 3].Y + 5, 20, 20);
                if (i != counter - 1 && (r0.Intersects(rects[i, 1]) || r1.Intersects(rects[i, 1]) || r2.Intersects(rects[i, 1]) || r3.Intersects(rects[i, 1])
                    || r0.Intersects(rects[i, 2]) || r1.Intersects(rects[i, 2]) || r2.Intersects(rects[i, 2]) || r3.Intersects(rects[i, 2])
                    || r0.Intersects(rects[i, 3]) || r1.Intersects(rects[i, 3]) || r2.Intersects(rects[i, 3]) || r3.Intersects(rects[i, 3])
                    || r0.Intersects(rects[i, 0]) || r1.Intersects(rects[i, 0]) || r2.Intersects(rects[i, 0]) || r3.Intersects(rects[i, 0])))
                {
                    if (onceBottom && !collision)
                    {
                        collision = true;
                        timerSet = 0;
                        onceBottom = false;
                        break;
                    }
                }
            }

            if (rects[counter - 1, 0].Bottom >= 520 || rects[counter - 1, 1].Bottom >= 520 || rects[counter - 1, 2].Bottom >= 520 || rects[counter - 1, 3].Bottom >= 520)
            {
                //going = false;
                if (onceBottom && !collision)
                {
                    collision = true;
                    timerSet = 0;
                    onceBottom = false;
                }
            }

            if (timerSet >= 0.5 && collision)
            {
                going = false;
                collision = false;
            }
                

            if (timerDescent >= 0.5)
            {
                timerDescent = 0;
              if (going && !collision)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        rects[counter - 1, i].Y += 20;
                    }
                } 
            }

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left) && going && timer >= 0.2 && rects[counter - 1, 0].X > 0 && rects[counter - 1, 3].X > 0 && rects[counter - 1, 1].X > 0 && rects[counter - 1, 2].X > 0) 
            {
                for (int i = 0; i < 4; i++)
                {
                    rects[counter - 1, i].X -= 20;
                }
                timer = 0;
            }
            if (ks.IsKeyDown(Keys.Right) && going && timer >= 0.2 && rects[counter - 1, 0].X < 320 && rects[counter - 1, 3].X < 320 && rects[counter - 1, 1].X < 320 && rects[counter - 1, 2].X < 320)
            {
                for (int i = 0; i < 4; i++)
                {
                    rects[counter - 1, i].X += 20;
                }
                timer = 0;
            }

            if (ks.IsKeyDown(Keys.Down) && going && timerDescAccelerated >= 0.05 && !collision)
            {
                for (int i = 0; i < 4; i++)
                {
                    rects[counter - 1, i].Y += 20;
                    timerDescAccelerated = 0;
                }
            }
            int index = 0;
            int numColor = 0;
            if (/*rects[counter - 1, 0].Bottom >= 520 || */!going)
            {
                index = rnd.Next(5);
                switch (index)
                {
                    case 0:
                        fallingShapes[counter] = new Square(4, ColorTris.BLUE);
                        break;
                    case 1:
                        fallingShapes[counter] = new Line(4, ColorTris.YELLOW);
                        break;
                    case 2:
                        numColor = rnd.Next(10);
                        if (numColor >= 5)
                            fallingShapes[counter] = new L(4, true);
                        else 
                            fallingShapes[counter] = new L(4, false);
                        break;
                    case 3:
                        numColor = rnd.Next(10);
                        if (numColor >= 5)
                            fallingShapes[counter] = new Z(4, true);
                        else 
                            fallingShapes[counter] = new Z(4, false);
                        break;
                    case 4:
                        fallingShapes[counter] = new T(4, ColorTris.TEAL);
                        break;
                }
                buildShape(fallingShapes[counter]);
                textures[counter] = new Texture2D(GraphicsDevice, 1, 1);
                onceBottom = true;
                switch (index)
                {
                    case 0:
                        textures[counter] = blueBrick;
                        break;
                    case 1:
                        textures[counter] = yellowBrick;
                        break;
                    case 2:
                        if (numColor >= 5)
                            textures[counter] = greenBrick;
                        else
                            textures[counter] = orangeBrick;
                        break;
                    case 3:
                        if (numColor >= 5)
                            textures[counter] = violetBrick;
                        else
                            textures[counter] = magentaBrick;
                        break;
                    case 4:
                        textures[counter] = tealBrick;
                        break;
                }
                
                counter++;
                going = true;
            }

            if (ks.IsKeyDown(Keys.Space))
                rotateShape(fallingShapes[counter - 1]);

            if (ks.IsKeyUp(Keys.Space))
            {
                once = true;
            }

            checkIfWin();

            base.Update(gameTime);
        }

        private void buildShape(Shape shape)
        {
            if (shape is Square)
            {
                rects[counter, 0] = new Rectangle(160, 20, 20, 20);
                rects[counter, 1] = new Rectangle(180, 20, 20, 20);
                rects[counter, 2] = new Rectangle(160, 0, 20, 20);
                rects[counter, 3] = new Rectangle(180, 0, 20, 20);

            } else if (shape is Line)
            {
                verticalLine = false;
                rects[counter, 0] = new Rectangle(140, 0, 20, 20);
                rects[counter, 1] = new Rectangle(160, 0, 20, 20);
                rects[counter, 2] = new Rectangle(180, 0, 20, 20);
                rects[counter, 3] = new Rectangle(200, 0, 20, 20);

            } else if (shape is L)
            {
                lRotation = 0;
                if (((L)shape).RightLeaning)
                {
                    rects[counter, 0] = new Rectangle(160, 40, 20, 20);
                    rects[counter, 1] = new Rectangle(180, 40, 20, 20);
                    rects[counter, 2] = new Rectangle(160, 20, 20, 20);
                    rects[counter, 3] = new Rectangle(160, 0, 20, 20);
                } else
                {
                    rects[counter, 0] = new Rectangle(180, 40, 20, 20);
                    rects[counter, 1] = new Rectangle(160, 40, 20, 20);
                    rects[counter, 2] = new Rectangle(180, 20, 20, 20);
                    rects[counter, 3] = new Rectangle(180, 0, 20, 20);
                }
            } else if (shape is Z)
            {
                shiftedZ = false;
                if (((Z)shape).RightLeaning)
                {
                    rects[counter, 0] = new Rectangle(160, 20, 20, 20);
                    rects[counter, 1] = new Rectangle(180, 20, 20, 20);
                    rects[counter, 2] = new Rectangle(180, 0, 20, 20);
                    rects[counter, 3] = new Rectangle(200, 0, 20, 20);
                }
                else
                {
                    rects[counter, 0] = new Rectangle(200, 20, 20, 20);
                    rects[counter, 1] = new Rectangle(180, 20, 20, 20);
                    rects[counter, 2] = new Rectangle(180, 0, 20, 20);
                    rects[counter, 3] = new Rectangle(160, 0, 20, 20);
                }
            } else if (shape is T)
            {
                tRotation = 0;
                rects[counter, 0] = new Rectangle(160, 20, 20, 20);
                rects[counter, 1] = new Rectangle(180, 20, 20, 20);
                rects[counter, 2] = new Rectangle(200, 20, 20, 20);
                rects[counter, 3] = new Rectangle(180, 0, 20, 20);
            }
        }

        private void rotateShape(Shape shape)
        {
            if (shape is Line && once)
            {
                if (!verticalLine)
                {
                    rects[counter - 1, 1].X -= 20;
                    rects[counter - 1, 1].Y += 20;
                    rects[counter - 1, 2].X -= 40;
                    rects[counter - 1, 2].Y += 40;
                    rects[counter - 1, 3].X -= 60;
                    rects[counter - 1, 3].Y += 60;
                    once = false;
                    verticalLine = true;
                }
                else
                {
                    rects[counter - 1, 1].X += 20;
                    rects[counter - 1, 1].Y -= 20;
                    rects[counter - 1, 2].X += 40;
                    rects[counter - 1, 2].Y -= 40;
                    rects[counter - 1, 3].X += 60;
                    rects[counter - 1, 3].Y -= 60;
                    once = false;
                    verticalLine = false;
                }
            }
            else if (shape is L)
            {
                if (((L)shape).RightLeaning && once)
                {
                    switch(lRotation)
                    {
                        case 0:
                            rects[counter - 1, 1].Y -= 20;
                            rects[counter - 1, 3].X += 40;
                            rects[counter - 1, 3].Y += 20;
                            lRotation = 1;
                            break;
                        case 1:
                            rects[counter - 1, 0].X += 20;
                            rects[counter - 1, 3].X -= 20;
                            rects[counter - 1, 3].Y += 40;
                            lRotation = 2;
                            break;
                        case 2:
                            rects[counter - 1, 0].Y -= 20;
                            rects[counter - 1, 1].Y -= 20;
                            rects[counter - 1, 3].X -= 40;
                            rects[counter - 1, 3].Y -= 40;
                            lRotation = 3;
                            break;
                        case 3:
                            rects[counter - 1, 1].Y += 20;
                            rects[counter - 1, 1].X += 20;
                            rects[counter - 1, 2].Y -= 20;
                            rects[counter - 1, 2].X += 20;
                            rects[counter - 1, 3].X += 40;
                            rects[counter - 1, 3].Y -= 40;
                            lRotation = 0;
                            break;
                    }
                    once = false;
                }
                else if (!((L)shape).RightLeaning && once)
                {
                    switch (lRotation)
                    {
                        case 0:
                            rects[counter - 1, 1].Y -= 20;
                            rects[counter - 1, 1].X += 20;
                            rects[counter - 1, 2].X += 20;
                            rects[counter - 1, 2].Y += 20;
                            rects[counter - 1, 3].X += 40;
                            rects[counter - 1, 3].Y += 40;
                            lRotation = 1;
                            break;
                        case 1:
                            rects[counter - 1, 1].Y += 20;
                            rects[counter - 1, 1].X += 20;
                            rects[counter - 1, 2].X -= 20;
                            rects[counter - 1, 2].Y += 20;
                            rects[counter - 1, 3].X -= 40;
                            rects[counter - 1, 3].Y += 40;
                            lRotation = 2;
                            break;
                        case 2:
                            rects[counter - 1, 1].Y += 20;
                            rects[counter - 1, 1].X -= 20;
                            rects[counter - 1, 2].X -= 20;
                            rects[counter - 1, 2].Y -= 20;
                            rects[counter - 1, 3].X -= 40;
                            rects[counter - 1, 3].Y -= 40;
                            lRotation = 3;
                            break;
                        case 3:
                            rects[counter - 1, 1].Y -= 20;
                            rects[counter - 1, 1].X -= 20;
                            rects[counter - 1, 2].Y -= 20;
                            rects[counter - 1, 2].X += 20;
                            rects[counter - 1, 3].Y -= 40;
                            rects[counter - 1, 3].X += 40;
                            lRotation = 0;
                            break;
                    }
                    once = false;
                }
            }
            else if (shape is Z)
            {
                if (((Z)shape).RightLeaning && once)
                {
                    if (!shiftedZ)
                    {
                        rects[counter - 1, 0].Y -= 40;
                        rects[counter - 1, 1].X -= 20;
                        rects[counter - 1, 1].Y -= 20;
                        rects[counter - 1, 3].Y += 20;
                        rects[counter - 1, 3].X -= 20;
                        once = false;
                        shiftedZ = true;
                    } else
                    {
                        rects[counter - 1, 0].Y += 40;
                        rects[counter - 1, 1].X += 20;
                        rects[counter - 1, 1].Y += 20;
                        rects[counter - 1, 3].Y -= 20;
                        rects[counter - 1, 3].X += 20;
                        once = false;
                        shiftedZ = false;
                    }                 
                }
                else if (!((Z)shape).RightLeaning && once)
                {
                    if (!shiftedZ)
                    {
                        rects[counter - 1, 0].X -= 20;
                        rects[counter - 1, 0].Y -= 40;
                        rects[counter - 1, 1].X -= 20;
                        once = false;
                        shiftedZ = true;
                    }
                    else
                    {
                        rects[counter - 1, 0].X += 20;
                        rects[counter - 1, 0].Y += 40;
                        rects[counter - 1, 1].X += 20;
                        once = false;
                        shiftedZ = false;
                    }                
                }
            }
            else if (shape is T && once)
            {
                switch (tRotation)
                {
                    case 0:
                        rects[counter - 1, 0].Y += 20;
                        rects[counter - 1, 0].X += 20;
                        tRotation = 1;
                        break;
                    case 1:
                        rects[counter - 1, 3].Y += 20;
                        rects[counter - 1, 3].X -= 20;
                        tRotation = 2;
                        break;
                    case 2:
                        rects[counter - 1, 2].Y -= 20;
                        rects[counter - 1, 2].X -= 20;
                        tRotation = 3;
                        break;
                    case 3:
                        rects[counter - 1, 0].Y -= 20;
                        rects[counter - 1, 0].X -= 20;
                        rects[counter - 1, 2].Y += 20;
                        rects[counter - 1, 2].X += 20;
                        rects[counter - 1, 3].Y -= 20;
                        rects[counter - 1, 3].X += 20;
                        tRotation = 0;
                        break;
                }
                once = false;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i <= counter; i++)
            {
                if (fallingShapes[i] is Square)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.Blue);
                    }
                } else if (fallingShapes[i] is Line)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.Gold);
                    }
                } else if (fallingShapes[i] is L && ((L)fallingShapes[i]).RightLeaning)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.GreenYellow);
                    }
                }
                else if (fallingShapes[i] is L && !((L)fallingShapes[i]).RightLeaning)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.Orange);
                    }
                }
                else if (fallingShapes[i] is Z && ((Z)fallingShapes[i]).RightLeaning)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.Lavender);
                    }
                }
                else if (fallingShapes[i] is Z && !((Z)fallingShapes[i]).RightLeaning)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.DeepPink);
                    }
                }
                else if (fallingShapes[i] is T)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        spriteBatch.Draw(textures[i], rects[i, j], Color.Turquoise);
                    }
                } 
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void checkIfWin() 
        {
            
            for (int k = 500; k >= 300; k -= 20)
            {
                int lineCounter = 0;
                for (int i = 0; i < counter; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (rects[i, j].Y == k)
                            lineCounter++;
                    }
                }
                if (lineCounter == 17)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (rects[i, j].Y == k)
                                rects[i, j] = new Rectangle(0, 0, 0, 0);

                            if (rects[i, j].Y < k)
                                rects[i, j].Y += 20;
                        }
                    }
                }
            }


        }
    }

}