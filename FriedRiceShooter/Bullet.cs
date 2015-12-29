using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace FriedRiceShooter
{
    class Bullet : Sprite
    {
        public const int speed = 6;
        public Vector2 velocity;
        public bool outOfBounds;

        public Bullet(Vector2 shipPosition, float shipRotation,Texture2D bulletTexture, SpriteBatch sprite, GraphicsDeviceManager graphics, bool isPLayer)
            : base(shipPosition, bulletTexture, sprite, graphics)
        {
            this.scale = new Vector2(.6f,.5f);
            this.velocity = Vector2.UnitX;
            this.velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(shipRotation));
            this.rotation = shipRotation +(float)(Math.PI/2);
            this.outOfBounds = false;

            if (!isPLayer)
                this.color = Color.Red;
            
            this.Update();
        }

        public void Update()
        {
            Position = Position + (velocity*speed);

            if (Position.X > this.screenSize.X || Position.X < 0 || Position.Y > this.screenSize.Y || Position.Y < 0)
            {
                outOfBounds = true;
            }
        }
    }
}
