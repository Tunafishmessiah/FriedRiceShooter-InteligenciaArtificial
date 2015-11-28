using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private Ship player;
        private state next; 

        public AI(Vector2 position, GraphicsDeviceManager graphics, Texture2D ShipTexture, Texture2D BulletTexture, SpriteBatch Sprite, Ship player)
            : base(position,graphics,ShipTexture,BulletTexture, Sprite)
        {
            this.player = player;
            next.position = 4;
            color = Color.Red;
            bullets = 0;
        }

        public override void Update(GameTime gametime)
        {
            next.position = 4;
            Look();
            Think();
            //next = Think();
            base.Update(gametime);
        }

        public override void rotate()
        {
            AimAt(player.Position);
        }

        public override void move()
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

            foreach (Bullet shot in player.ShotsFired)
            {
                futureBulletPositions.Add(shot.Position + shot.speed * shot.getVelocity());
                Vector2 d = shot.speed;
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

                    distances += futureDistance * dot ;
                }                

                float centerDistance = (ScreenSize / 2f - nextPosition).Length();

                float score;// = distances * 20 - centerDistance;
                if (distances == 0)
                    score = -centerDistance;
                else
                    score = distances;

                if (score >= bestScore)
                {
                    bestIndex = i;
                    bestScore = score;
                }

                next.position = bestIndex;
                next.shoting = true;
            }
            Console.WriteLine(bestScore);
        }
        
        private Vector2 CalculateNextPosition(int index)
        {
            Vector2 nextPosition = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            switch (index)
            {
                //move up
                case 0:
                    nextPosition = new Vector2(this.Position.X, this.Position.Y - getSpeed());
                    break;

                //Move down
                case 1:
                    nextPosition = new Vector2(this.Position.X, this.Position.Y + getSpeed());
                    break;

                //Move left
                case 2:
                    nextPosition = new Vector2(this.Position.X - getSpeed(), this.Position.Y);
                    break;

                //Move right
                case 3:
                    nextPosition = new Vector2(this.Position.X + getSpeed(), this.Position.Y);
                    break;

                //Don't move
                case 4:
                    nextPosition = this.Position;
                    break;

            }
            return nextPosition;
        }
    }
}
