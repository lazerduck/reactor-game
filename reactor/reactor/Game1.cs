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

namespace reactor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<ReactorCore> Cores = new List<ReactorCore>();
        ReactorCore Wall;
        MouseState NewMouse;
        MouseState OldMouse;
        KeyboardState NewKey;

        double MouseHeat;
        double Power;
        double Damage;
        bool MouseActivated;
        double CoolTimer;
        double ShieldTimer;
        bool CanShield = true;
        bool CanCool = true;
        double TotalDamage;

        //tasks
        int TaskNo = 0;
        double TaksTimer = 0;
        double []Levels = {5,10,15,17,100,16,20};
        string []Tasks = {"start","Main Engines","Shields Test","Weapons Test", "Reactor Overload Test","Jump to hyper Space","Incoming fire"};
        double[] TaksTime = {4, 10,5,10,2,20,10};

        //textures
        Texture2D CoreWhite;
        Texture2D Dot;
        SpriteFont Font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            base.Initialize();
        }
        public int CalcPosition(int Col, int Row)
        {
            int[] Layer = { 7, 8, 9, 10, 11, 12, 11, 10, 9, 8, 7};
            int Column = Col;

            int CalcPos = 0;
            for (int i = 0; i < Row; i++)
            {
                CalcPos += Layer[i];
            }
            CalcPos += Column;
            return CalcPos;
        }

        public void FindNeighbours(int pos)
        {
            int[] Layer = { 7, 8, 9, 10, 11, 12, 11, 10, 9, 8, 7 };
            int Column = 0;
            int Row = 0;
            int CalcPos = 0;
            for (int i = 0; i < pos; i++)
            {
                Column++;
                if (Column >= Layer[Row])
                {
                    Row++;
                    Column = 0;
                }
            }
            CalcPos = CalcPosition(Column, Row);
            Cores[CalcPos].Tint = Color.Gray;

            if (Column != 0 && Column != Layer[Row]-1)
            {
                Cores[CalcPos].Tint = Color.Blue;
            }
            if (Row == 0)
            {
                Cores[pos].Top = Wall;
                Cores[pos].TopLeft = Wall;
                Cores[pos].TopRight = Wall;
                Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
            }
            if (Row == 1)
            {
                if (Column == 0 )
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Wall;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[CalcPosition(Column, Row - 1)].Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                    Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[CalcPosition(Column - 1, Row - 1)].Tint = Color.Red;
                    Cores[pos].TopRight = Wall;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                    Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
                }
                else
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[CalcPosition(Column - 1, Row - 1)].Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[CalcPosition(Column, Row - 1)].Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                    Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
                }
            }

            if (Row ==2 || Row ==3)
            {
                if (Column == 0)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Wall;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[CalcPosition(Column, Row - 1)].Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                    Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[CalcPosition(Column - 1, Row - 1)].Tint = Color.Red;
                    Cores[pos].TopRight = Wall;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                    Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
                }
                else
                {
                    Cores[pos].Top = Cores[CalcPosition(Column - 1, Row - 2)];
                    Cores[CalcPosition(Column - 1, Row - 2)].Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[CalcPosition(Column - 1, Row - 1)].Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[CalcPosition(Column, Row - 1)].Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column + 1, Row + 2)];
                    Cores[CalcPosition(Column + 1, Row + 2)].Tint = Color.Red;
                }
            }

            if (Row == 4)
            {
                if (Column == 0)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Wall;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[CalcPosition(Column, Row - 1)].Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column, Row + 2)];
                    Cores[CalcPosition(Column, Row + 2)].Tint = Color.Red;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[CalcPosition(Column - 1, Row - 1)].Tint = Color.Red;
                    Cores[pos].TopRight = Wall;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[CalcPosition(Column + 1, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column, Row + 2)];
                    Cores[CalcPosition(Column, Row + 2)].Tint = Color.Red;
                }
                else
                {
                    Cores[pos].Top = Cores[CalcPosition(Column - 1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column + 1, Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column, Row + 2)];
                    Cores[pos].Bottom.Tint = Color.Red;
                }
            }
            if (Row == 5)
            {
                if (Column == 0)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Wall;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[CalcPosition(Column, Row - 1)].Tint = Color.Red;
                    Cores[pos].BottomLeft = Wall;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column, Row + 1)];
                    Cores[CalcPosition(Column, Row + 1)].Tint = Color.Red;
                    Cores[pos].Bottom = Wall;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Wall;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[CalcPosition(Column - 1, Row - 1)].Tint = Color.Red;
                    Cores[pos].TopRight = Wall;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column-1, Row + 1)];
                    Cores[CalcPosition(Column-1, Row + 1)].Tint = Color.Red;
                    Cores[pos].BottomRight = Wall;
                    Cores[pos].Bottom = Wall;
                }
                else
                {
                    Cores[pos].Top = Cores[CalcPosition(Column - 1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column - 1, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column-1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column , Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column-1, Row + 2)];
                    Cores[pos].Bottom.Tint = Color.Red;
                }
            }

            if (Row == 6)
            {
                if (Column == 0)
                {
                    Cores[pos].Top = Cores[CalcPosition(Column , Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column+1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Wall;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column, Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Wall;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Cores[CalcPosition(Column, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column-1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Wall;
                    Cores[pos].Bottom = Wall;
                }
                else
                {
                    Cores[pos].Top = Cores[CalcPosition(Column , Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column , Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column+1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column-1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column , Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column-1, Row + 2)];
                    Cores[pos].Bottom.Tint = Color.Red;
                }
            }
            if (Row == 7 || Row == 8)
            {
                if (Column == 0)
                {
                    Cores[pos].Top = Cores[CalcPosition(Column+1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Wall;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column, Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Wall;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Cores[CalcPosition(Column+1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column - 1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Wall;
                    Cores[pos].Bottom = Wall;
                }
                else
                {
                    Cores[pos].Top = Cores[CalcPosition(Column+1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column - 1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column, Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Cores[CalcPosition(Column - 1, Row + 2)];
                    Cores[pos].Bottom.Tint = Color.Red;
                }
            }
            if (Row == 9)
            {
                if (Column == 0)
                {
                    Cores[pos].Top = Cores[CalcPosition(Column + 1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Wall;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column, Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Wall;
                }
                else if (Column == Layer[Row] - 1)
                {
                    Cores[pos].Top = Cores[CalcPosition(Column + 1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column - 1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Wall;
                    Cores[pos].Bottom = Wall;
                }
                else
                {
                    Cores[pos].Top = Cores[CalcPosition(Column + 1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Cores[CalcPosition(Column - 1, Row + 1)];
                    Cores[pos].BottomLeft.Tint = Color.Red;
                    Cores[pos].BottomRight = Cores[CalcPosition(Column, Row + 1)];
                    Cores[pos].BottomRight.Tint = Color.Red;
                    Cores[pos].Bottom = Wall;
                }
            }
            if (Row == 10)
            {
                
                    Cores[pos].Top = Cores[CalcPosition(Column + 1, Row - 2)];
                    Cores[pos].Top.Tint = Color.Red;
                    Cores[pos].TopLeft = Cores[CalcPosition(Column, Row - 1)];
                    Cores[pos].TopLeft.Tint = Color.Red;
                    Cores[pos].TopRight = Cores[CalcPosition(Column + 1, Row - 1)];
                    Cores[pos].TopRight.Tint = Color.Red;
                    Cores[pos].BottomLeft = Wall;
                    Cores[pos].BottomRight = Wall;
                    Cores[pos].Bottom = Wall;
                
            }
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CoreWhite = Content.Load<Texture2D>("coreWhite");
            Font = Content.Load<SpriteFont>("SpriteFont1");
            Dot = Content.Load<Texture2D>("Dot");
            for (int j = 1; j < 7; j++)
            {
                for (int i = 0; i < 6+j; i++)
                {
                    ReactorCore TempR = new ReactorCore(CoreWhite, new Rectangle(300 - ((CoreWhite.Width + 15)*j)/2 + ((CoreWhite.Width + 15) * i), 100 + j*15, CoreWhite.Width, CoreWhite.Height));
                    Cores.Add(TempR);
                }
            }
            for (int j = 5; j > 0; j--)
            {
                for (int i = 0; i < 6+ j; i++)
                {
                    ReactorCore TempR = new ReactorCore(CoreWhite, new Rectangle(300 - ((CoreWhite.Width + 15) * (j)) / 2 + ((CoreWhite.Width + 15) * i), 100 + 6 * 15 + (6-j) * 15, CoreWhite.Width, CoreWhite.Height));
                    Cores.Add(TempR);
                }
            }
            Wall = new ReactorCore(CoreWhite, new Rectangle(0, 0, 0, 0));
            Wall.Wall = true;

            int []Layer = {7,8,9,10,11,12,11,10,9,8,7};
            int Column = 0;
            int Row = 0;

            for (int i = 0; i < Cores.Count; i++)
            {
                

                Column++;
                if (Column == Layer[Row])
                {
                    Row++;
                    Column = 0;
                }
            }
            for (int i = 0; i < Cores.Count; i++)
            {
                FindNeighbours(i);
            }
                
            
            
        }

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
            TotalDamage = 0;
            
            if (Powers.Shield)
            {
                ShieldTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (ShieldTimer > 7000)
                {
                    Powers.Shield = false;
                }
            }
            else
            {
                if (ShieldTimer > 0)
                {
                    ShieldTimer-=3;
                }
                else
                {
                    CanShield = true;
                }

            }
            if (Powers.Cool)
            {
                CoolTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (CoolTimer > 7000)
                {
                    Powers.Cool = false;
                }
            }
            else
            {
                if (CoolTimer > 0)
                {
                    CoolTimer-=3;
                }
                else
                {
                    CanCool = true;
                }

            }
            NewMouse = Mouse.GetState();
            NewKey = Keyboard.GetState();
            // Allows the game to exit
            if (NewKey.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if(NewKey.IsKeyDown(Keys.Space))
            {
                foreach (ReactorCore R in Cores)
                {
                    R.Activated = false;
                }
            }
            if (NewKey.IsKeyDown(Keys.Enter))
            {
                foreach (ReactorCore R in Cores)
                {
                    R.Activated = true;
                }
            }
            if (NewKey.IsKeyDown(Keys.Z))
            {
                if (CanShield)
                {
                    if (!Powers.Shield)
                    {
                        if (ShieldTimer < 10)
                        {
                            Powers.Shield = true;
                            CanShield = true;
                            ShieldTimer = 0;
                        }
                    }
                }
            }
            if (NewKey.IsKeyDown(Keys.X))
            {
                if (CanCool)
                {
                    if (!Powers.Cool)
                    {
                        if (CoolTimer < 10)
                        {
                            Powers.Cool = true;
                            CanCool = false;
                            CoolTimer = 0;
                        }
                    }
                }
            }
            Power = 0;
            foreach (ReactorCore R in Cores)
            {
                R.Update(gameTime);
                TotalDamage += R.Damage;
                Power += R.Power;
                if (NewMouse.LeftButton == ButtonState.Pressed && OldMouse.LeftButton == ButtonState.Released)
                {
                    if (R.Destination.Contains(NewMouse.X, NewMouse.Y))
                    {
                        if (R.Activated)
                        {
                            R.Activated = false;
                        }
                        else
                        {
                            R.Activated = true;
                        }
                    }
                }
                else
                {
                    if (R.Destination.Contains(NewMouse.X, NewMouse.Y))
                    {
                        MouseHeat = R.Heat;
                        MouseActivated = R.Activated;
                        Damage = R.Damage;
                    }
                }
            }
            OldMouse = NewMouse;
            if (Power > Levels[TaskNo])
            {
                TaksTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (TaksTimer > TaksTime[TaskNo]*1000)
            {
                TaksTimer = 0;
                TaskNo++;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (ReactorCore R in Cores)
            {
                R.Draw(spriteBatch);
            }
            spriteBatch.DrawString(Font, "Heat = " + MouseHeat, new Vector2(20, 20), Color.Black);
            spriteBatch.DrawString(Font, "Power = " + Power, new Vector2(20, 40), Color.Black);
            spriteBatch.DrawString(Font, "Damage = " + Damage, new Vector2(20, 60), Color.Black);
            spriteBatch.DrawString(Font, "Activated = " + MouseActivated, new Vector2(20, 80), Color.Black);
            spriteBatch.DrawString(Font, "Shielding = " + Powers.Shield, new Vector2(20, graphics.PreferredBackBufferHeight - 50), Color.Black);
            spriteBatch.DrawString(Font, "Emergancy Cooling = " + Powers.Cool, new Vector2(20, graphics.PreferredBackBufferHeight - 30), Color.Black);
            spriteBatch.Draw(Dot, new Rectangle(220, graphics.PreferredBackBufferHeight - 40,(int)(ShieldTimer/100),10), Color.Red);
            spriteBatch.Draw(Dot, new Rectangle(300, graphics.PreferredBackBufferHeight - 20, (int)(CoolTimer / 100), 10), Color.Red);
            spriteBatch.DrawString(Font, "Total damage = " + TotalDamage/Cores.Count, new Vector2(20, graphics.PreferredBackBufferHeight - 70), Color.Black);
            spriteBatch.DrawString(Font, "Task: " + Tasks[TaskNo], new Vector2(500, 20), Color.Black);
            if (Levels[TaskNo] > Power)
            {
                spriteBatch.DrawString(Font, "Power Required: " + Levels[TaskNo], new Vector2(500, 40), Color.Red);
            }
            else
            {
                spriteBatch.DrawString(Font, "Power Required: " + Levels[TaskNo], new Vector2(500, 40), Color.Green);
            }
            spriteBatch.Draw(Dot, new Rectangle(500, 70, (int)(TaksTime[TaskNo]) * 10, 10), Color.Black);
            spriteBatch.Draw(Dot, new Rectangle(500, 70, (int)(TaksTimer)/100, 10), Color.Red);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
