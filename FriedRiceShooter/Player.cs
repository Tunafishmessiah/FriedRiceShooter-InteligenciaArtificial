using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace FriedRiceShooter
{
    class Player : Ship
    {
        public Player(Vector2 position, GraphicsDeviceManager graphics, Texture2D shipTexture, Texture2D bulletTexture, SpriteBatch sprite)
            : base(position, graphics, shipTexture, bulletTexture, sprite) { }
        

        public override void Rotate()
        {
            MouseState mouse = Mouse.GetState();
            AimAt(new Vector2(mouse.Position.X, mouse.Position.Y));
            
            //Shooting is here as well
            if (mouse.LeftButton == ButtonState.Pressed && !shooting)
            {
                this.Shoot();
            }
        }

        public override void Move()
        {    
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.W))
            {
                this.MoveUp();
            }
            else if (k.IsKeyDown(Keys.S))
            {
                this.MoveDown();
            }
            else if (k.IsKeyDown(Keys.A))
            {
                this.MoveLeft();
            }
            else if (k.IsKeyDown(Keys.D))
            {
                this.MoveRight();
            }
        }
    }
}
