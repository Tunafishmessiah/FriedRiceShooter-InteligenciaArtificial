using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FriedRiceShooter
{
    struct state
    {
        public bool shoting;
        public int position;
        public float stateScore;

        public state(bool S, int P, int Sc)
        {
            this.shoting = S;
            this.position = P;
            this.stateScore = Sc;
        }
    }

    class AI : Ship
    {
        public Ship player;
        private state next;
        private float rayCenter;

        public AI(Vector2 position, GraphicsDeviceManager graphics, Texture2D ShipTexture, Texture2D BulletTexture, SpriteBatch Sprite, Ship player)
            : base(position,graphics,ShipTexture,BulletTexture, Sprite)
        {
            this.player = player;
            next.position = 4;
            color = Color.Red;
            bullets = 0;
            rayCenter = Math.Min(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight) / 2;
        }

        public override void Update(GameTime gametime)
        {
            next.position = 4;
            Look();
            Think();
            base.Update(gametime);
        }

        public override void Rotate()
        {
            AimAt(player.Position);
        }

        public override void Move()
        {
            switch (next.position)
            {
                case 0:
                    //up
                    this.MoveUp();
                    break;
                case 1:
                    //down
                    this.MoveDown();
                    break;
                case 2:
                    //left
                    this.MoveLeft();
                    break;
                case 3:
                    //right
                    this.MoveRight();
                    break;
                case 4:
                    //no move
                    break;
            }

            if (shooting != next.shoting && !shooting)
            {
                Shoot();
                shooting = next.shoting;
            }
        }
        
        List<Vector2> futureBulletPositions = new List<Vector2>();
        List<Vector2> bulletDirections = new List<Vector2>();

        private void Look()
        {
            futureBulletPositions.Clear();
            bulletDirections.Clear();

            foreach (Bullet shot in player.shotsFired)
            {
                futureBulletPositions.Add(shot.Position + shot.velocity * Bullet.speed);
                Vector2 d = shot.velocity;
                d.Normalize();
                bulletDirections.Add(d);
            }
        }

        private void Think()
        {
            float defensivo = bullets / (player.bullets == 0? -1 : player.bullets); 

            float bestScore = float.NegativeInfinity;
            int bestIndex = 4;

            for (int i = 0; i <= 4; i++)
            {
                float distances = 0;
                Vector2 nextPosition;

                nextPosition = CalculateNextPosition(i);

                int count = futureBulletPositions.Count;

                for (int j = 0; j < count; j++)
                {
                    Vector2 framing = nextPosition - futureBulletPositions[j];
                    float futureDistance = framing.Length();
                    framing.Normalize();
                    float dot = Vector2.Dot(framing, bulletDirections[j]);
                    if (dot >= 0)
                        dot = 1 - dot;

                    distances += futureDistance * dot * 200;
                }

                float centerDistance = (screenSize / 2f - nextPosition).Length();

                float score;

                score = distances - centerDistance;

                if (score >= bestScore)
                {
                    bestIndex = i;
                    bestScore = score;
                }

                next.position = bestIndex;
                next.shoting = true;
            }
        }
        
        private Vector2 CalculateNextPosition(int index)
        {
            Vector2 nextPosition = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            switch (index)
            {
                //move up
                case 0:
                    nextPosition = new Vector2(Position.X, Position.Y - speed);
                    break;

                //Move down
                case 1:
                    nextPosition = new Vector2(Position.X, Position.Y + speed);
                    break;

                //Move left
                case 2:
                    nextPosition = new Vector2(Position.X - speed, Position.Y);
                    break;

                //Move right
                case 3:
                    nextPosition = new Vector2(Position.X + speed, Position.Y);
                    break;

                //Don't move
                case 4:
                    nextPosition = Position;
                    break;

            }
            nextPosition = Vector2.Clamp(nextPosition, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(screenSize.X - texture.Width / 2, screenSize.Y - texture.Height / 2));
            return nextPosition;
        }
    }
}
