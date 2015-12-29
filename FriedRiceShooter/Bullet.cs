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

        public List<Bullet> shotsFired;
        private Ship owner;


        public Bullet(Vector2 shipPosition, float shipRotation,Texture2D bulletTexture, SpriteBatch sprite, GraphicsDeviceManager graphics, bool isPLayer, List<Bullet> shotsFired, Ship owner)
            : base(shipPosition, bulletTexture, sprite, graphics)
        {
            this.shotsFired = shotsFired;
            this.owner = owner;
            this.scale = new Vector2(.6f,.5f);
            this.velocity = Vector2.UnitX;
            this.velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(shipRotation));
            this.rotation = shipRotation +(float)(Math.PI/2);
            this.outOfBounds = false;

            if (!isPLayer)
                this.color = Color.Red;            
        }

        public void Update(GameTime gametime)
        {
            Position = Position + (velocity*speed);
            base.Update(gametime);
            if ((Position.X > this.screenSize.X || Position.X < 0 || Position.Y > this.screenSize.Y || Position.Y < 0) && !outOfBounds)
            {
                outOfBounds = true;
                this.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        { 	        
            if (disposed)
                return;
            if (disposing)
            {
                shotsFired.Remove(this);
            }

            base.Dispose(disposing);
        }

        public Ship Owner
        {
            get { return owner; }
        }

        
    }
}
