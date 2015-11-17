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
        private const int Velocity = 30;
        private Vector2 speed;
        public bool OutOfBounds;
        public Bullet(Vector2 ShipPosition, float ShipRotation,Texture2D BulletTexture, SpriteBatch Sprite, GraphicsDeviceManager graphics)
            : base(ShipPosition, BulletTexture, Sprite, graphics)
        {
            this.Scale = new Vector2(.6f,.5f);
            this.speed = Vector2.UnitX;
            this.speed = Vector2.Transform(speed, Matrix.CreateRotationZ(ShipRotation));
            this.rotation = ShipRotation +(float)(Math.PI/2);
            this.OutOfBounds = false;

            this.Update();
        }

        public void Update()
        {
            this.Position = Position + (speed*Velocity);

            if (this.Position.X > this.ScreenSize.X || this.Position.X < 0 || this.Position.Y > this.ScreenSize.Y || this.Position.Y < 0)
            {
                this.OutOfBounds = true;
            }
        }

    }
}
