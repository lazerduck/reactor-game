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
    class ReactorCore
    {
        public ReactorCore Top;
        public ReactorCore TopLeft;
        public ReactorCore BottomLeft;
        public ReactorCore Bottom;
        public ReactorCore BottomRight;
        public ReactorCore TopRight;
        public double Heat;
        public double Conductivity;
        public double Damage;
        public bool Activated;
        public bool Opperational;
        public bool Wall;
        public double Time;
        public double Power;
        Texture2D Sprite;
        public Color Tint = Color.LightGreen;
        public Rectangle Destination;
        public ReactorCore(Texture2D Sprite, Rectangle Destination)
        {
            this.Sprite = Sprite;
            this.Destination = Destination;
            Heat = 0;
            Conductivity = 0.03;
            Damage = 0;
            Activated = false;
            Opperational = true;
            Wall = false;
            Time = 0;
            Power = 0;
        }
        public void Update(GameTime gameTime)
        {
            //Color
            if (!Wall)
            {
                if (Damage > 99)
                {
                    Damage = 100;
                    Activated = false;
                    Tint = new Color((int)Heat, 0, 0);
                }
                Time += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Activated)
                {
                    Tint = new Color((int)Heat, 254 - (int)Heat, 0);
                    Power = Heat / 255;
                }
                else
                {
                    Power = 0;
                    if (Damage < 100)
                    {
                        Tint = new Color((int)Heat, 0, 254 - (int)Heat);
                    }
                }
                if (Time > 100)
                {
                    if (!Powers.Shield)
                    {
                        if (Heat > 250)
                        {
                            Damage += 0.5;
                        }
                    }
                    if (Powers.Cool)
                    {
                        Heat -= 5;
                    }
                    if (Activated)
                    {
                        Heat+=6;
                    }
                    else
                    {
                        Heat-=4;
                    }
                    if (Heat > Top.Heat)
                    {
                        Top.Heat += Heat * Conductivity;
                    }
                    if (Heat > TopRight.Heat)
                    {
                        TopRight.Heat += Heat * Conductivity;
                    }
                    if (Heat > TopLeft.Heat)
                    {
                        TopLeft.Heat += Heat * Conductivity;
                    }
                    if (Heat > Bottom.Heat)
                    {
                        Bottom.Heat += Heat * Conductivity;
                    }
                    if (Heat > BottomLeft.Heat)
                    {
                        BottomLeft.Heat += Heat * Conductivity;
                    }
                    if (Heat > BottomRight.Heat)
                    {
                        BottomRight.Heat += Heat * Conductivity;
                    }
                    Heat -= 3*Heat * Conductivity;
                    Time = 0;
                }
                if (Heat < 0)
                {
                    Heat = 0;
                }
                if (Heat > 255)
                {
                    Heat = 255;
                }
            }
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Destination, Tint);
        }
    }
}
