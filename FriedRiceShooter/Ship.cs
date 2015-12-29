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

    abstract class Ship : Sprite
    {
        public int hitPoints;
        public int bullets = 30;
        public const int speed = 5;
        private const float cooldown = 0.25f;
        public GraphicsDeviceManager graphics;
        public Texture2D bulletTexture;
        public List<Bullet> shotsFired;
        public bool shooting;
        private double timer;
        public Vector2 direction;

        public Ship(Vector2 position, GraphicsDeviceManager graphics, Texture2D shipTexture, Texture2D bulletTexture, SpriteBatch sprite)
            : base(position, shipTexture, sprite, graphics)
        {
            this.graphics = graphics;
            this.bulletTexture = bulletTexture;

            shotsFired = new List<Bullet>();
            shooting = false;
        }

        public override void Update(GameTime gametime)
        {
            foreach (Bullet bullet in shotsFired.ToList())
            {
                bullet.Update(gametime);
            }

            if (shooting)
            {
                timer += gametime.ElapsedGameTime.TotalSeconds;
                if (timer > cooldown)
                {
                    shooting = false;
                }
            } 

            Rotate();
            Move();
            Position = Vector2.Clamp(Position, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(screenSize.X - texture.Width / 2, screenSize.Y - texture.Height / 2));
            
            base.Update(gametime);
        }

        public override void Draw()
        {
            base.Draw();
            foreach (Bullet bullet in shotsFired)
            {
                bullet.Draw();
            }
        }

        public override void Collision(Sprite other)
        {
            if (other is Bullet)
            {
                Bullet b = (Bullet)other;
                if (b.Owner != this)
                {
                    Hit();
                    b.Dispose();
                }
            }
        }

        public void MoveRight()
        {
            //Locking it on the screen side
            if ((Position.X + texture.Width/2) + speed > screenSize.X)
            {
                Position = new Vector2(screenSize.X - texture.Width/2, Position.Y);
            }
            //letting him move
            else {
                Position = new Vector2(Position.X + speed, Position.Y);
            }
        }

        public void MoveLeft()
        {
            //Locking it on the screen side
            if (Position.X - speed > texture.Width/2)
            {
                Position = new Vector2(Position.X - speed, Position.Y);
            }
            //letting him move
            else Position = new Vector2(texture.Width/2, Position.Y);
        }
        
        public void MoveUp()
        {
            //Locking it on the screen side
            if (Position.Y - speed > texture.Height/2)
            {
                Position = new Vector2(Position.X, Position.Y - speed);
            }
            //letting him move
            else
            {
                Position = new Vector2(Position.X, texture.Height/2);
            }
        }
        
        public void MoveDown()
        {
            //Locking it on the screen side
            if ((Position.Y + this.texture.Height/2) + speed < this.screenSize.Y)
            {
                Position = new Vector2(Position.X, Position.Y + speed);
            }
            //letting him move
            else
            {
                Position = new Vector2(Position.X, this.screenSize.Y - texture.Height/2);
            }
        }

        public void Hit()
        {
            hitPoints++;
        }
        
        public void Shoot()
        {
            if (bullets > 0)
            {
                bullets--;
                Bullet shot = new Bullet(Position, this.rotation, bulletTexture, this.spriter, this.graphics, this is Player, shotsFired, this);
                shotsFired.Add(shot);
                shooting = true;
                timer = 0;
            }
        }

        public abstract void Rotate();

        public abstract void Move();

        public void AimAt(Vector2 at)
        {
            float l = (float)(at.X - Position.X);
            float a = (float)(at.Y - Position.Y);
            rotation = (float)(Math.Atan2(a, l));
        }
    }
}