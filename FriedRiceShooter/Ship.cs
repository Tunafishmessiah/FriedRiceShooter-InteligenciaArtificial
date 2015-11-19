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
        int HitPoints;
        private const int speed = 5;
        public GraphicsDeviceManager graphics;
        public Texture2D BulletTexture;
        public List<Bullet> ShotsFired;
        public bool shooting;
        private double timer;

        public Ship(Vector2 position, GraphicsDeviceManager graphics, Texture2D ShipTexture, Texture2D BulletTexture, SpriteBatch Sprite)
            : base(position, ShipTexture, Sprite, graphics)
        {
            this.HitPoints = 10;
            this.graphics = graphics;
            this.BulletTexture = BulletTexture;

            ShotsFired = new List<Bullet>();
            shooting = false;
        }

        public virtual void Update(GameTime gametime)
        {
            foreach (Bullet bullet in ShotsFired.ToList())
            {
                bullet.Update();
                if (bullet.OutOfBounds)
                {
                    ShotsFired.Remove(bullet);
                }
            }

            if (shooting)
            {
                timer += gametime.ElapsedGameTime.TotalSeconds;
                if (timer > 1)
                {
                    shooting = false;
                }
            }
            move();
            rotate();

        }

        public new virtual void Draw()
        {
            base.Draw();
            foreach (Bullet bullet in ShotsFired)
            {
                bullet.Draw();
            }
        }

        public void MoveRight()
        {
            //Locking it on the screen side
            if ((this.Position.X + this.texture.Width) + speed > this.ScreenSize.X)
            {
                this.Position = new Vector2(this.ScreenSize.X - this.texture.Width, this.Position.Y);
            }
            //letting him move
            else {
                this.Position = new Vector2(this.Position.X + speed, this.Position.Y);
            }
        }

        public void MoveLeft()
        {
            //Locking it on the screen side
            if (this.Position.X - speed > 0)
            {
                this.Position = new Vector2(this.Position.X - speed, this.Position.Y);
            }
            //letting him move
            else this.Position = new Vector2(0, this.Position.Y);
        }
        
        public void MoveUp()
        {
            //Locking it on the screen side
            if (this.Position.Y - speed > 0)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - speed);
            }
            //letting him move
            else
            {
                this.Position = new Vector2(this.Position.X, 0);
            }
        }
        
        public void MoveDown()
        {
            //Locking it on the screen side
            if ((this.Position.Y + this.texture.Height) + speed < this.ScreenSize.Y)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + speed);
            }
            //letting him move
            else
            {
                this.Position = new Vector2(this.Position.X, this.ScreenSize.Y - texture.Height);
            }
        }

        public void Hit()
        {
            this.HitPoints = this.HitPoints--;
        }

        public int GetHp()
        { return this.HitPoints; }

        public void Shoot()
        {
            Bullet shot = new Bullet(this.Position, this.rotation, BulletTexture, this.Spriter, this.graphics);
            ShotsFired.Add(shot);
            shooting = true;
            timer = 0;
        }

        public abstract void rotate();

        public abstract void move();

        public void AimAt(Vector2 at)
        {
            float l = (float)(at.X - this.Position.X);
            float a = (float)(at.Y - this.Position.Y);
            this.rotation = (float)(Math.Atan2(a, l));
        }

        public int getSpeed()
        { return speed; }
    }
}