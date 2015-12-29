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
            rayCenter = Math.Min(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight) / 2;
        }

        public override void Update(GameTime gametime)
        {
            Look();
            Think();
            base.Update(gametime);
        }

        public override void Rotate()
        {
            AimAt(player.Position + player.direction * (Position - player.Position).Length() / Bullet.speed * 2f);
        }

        public override void Move()
        {
            switch (next.position)
            {
                case 0:
                    //up
                    this.MoveUp();
                    direction = -Vector2.UnitY;
                    break;
                case 1:
                    //down
                    this.MoveDown();
                    direction = Vector2.UnitY;
                    break;
                case 2:
                    //left
                    this.MoveLeft();
                    direction = -Vector2.UnitX;
                    break;
                case 3:
                    //right
                    this.MoveRight();
                    direction = Vector2.UnitX;
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
            bool ofensivo = (bullets / (player.bullets == 0? 0.5f : player.bullets)) > 0.5f; 

            float bestScore = float.NegativeInfinity;
            int bestIndex = 4;

            for (int i = 0; i <= 4; i++)
            {
                float distances = 0;
                Vector2 nextPosition;

                nextPosition = CalculateNextPosition(i);
                int count = futureBulletPositions.Count;
                
                //Funnção balas
                if (count != 0)
                {
                    distances = ThinkAboutBullets(count, nextPosition);
                }
                else
                {
                    distances = ThinkAboutPlayer(nextPosition);
                }

                Vector2 centerDistanceVector = screenSize / 2f - nextPosition;
                int badSide = GetCurrentQuadrant(centerDistanceVector);

                float centerDistance = centerDistanceVector.Length();

                float score = ThinkAboutWalls(centerDistance, i, badSide);
                
                if (centerDistance > 100)
                    score += distances - (centerDistance * .004f);
                else 
                    score += distances;

                if (score >= bestScore)
                {
                    bestIndex = i;
                    bestScore = score;
                }
            }
            next.position = bestIndex;
            next.shoting = ofensivo;
        }

        private float ThinkAboutBullets(int count, Vector2 nextPosition)
        {
            float distances = 0;
            for (int j = 0; j < count; j++)
            {
                //dodging bullets
                Vector2 framing = nextPosition - futureBulletPositions[j];
                float futureDistance = framing.Length();
                framing.Normalize();
                float dot = Vector2.Dot(framing, bulletDirections[j]);
                if (dot >= 0)
                    dot = 1 - dot;

                //if bullets are coming near him
                Vector2 actualFraming = nextPosition - player.shotsFired[j].Position;
                if (actualFraming.Length() < futureDistance)
                    dot = 0;

                distances += futureDistance * dot * 200;
            }
            return distances;
        }

        private float ThinkAboutPlayer(Vector2 nextPosition)
        {
            float distances = 0;
            bool above = false;
            bool right = false;
   
            float playerDistance = (nextPosition - player.Position).Length();

            //Checking which side he is from the enemy
            //RightSide?
            if (this.Position.X > player.Position.X)
                right = true;
            //Above?
            if (this.Position.Y < player.Position.Y)
                above = true;

            if (playerDistance > 250)
            {
                distances = 200 * (150 / playerDistance);
            }

            else if (playerDistance < 130)
            {
                switch (next.position)
                {
                    case 0:
                        //up
                        if (above) distances = 100;
                        else distances = 0;
                        break;
                    case 1:
                        //down
                        if (!above) distances = 100;
                        else distances = 0;
                        break;
                    case 2:
                        //left
                        if (!right) distances = 100;
                        else distances = 0;
                        break;
                    case 3:
                        //right
                        if (right) distances = 100;
                        else distances = 0;
                        break;
                    case 4:
                        distances = 0;
                        break;
                }
            }

            else
            {
                ////orbit atempt

                //if (this.Position.X - player.Position.X > 0)
                //{
                //    if (this.Position.X < nextPosition.X)
                //        distances = 100;
                //}
                //else
                //{
                //    if (this.Position.X > nextPosition.X)
                //        distances = 100;
                //}

                

                //Making him spin a little
                if (above && right)
                {
                    //Making a point that the ai must be away of, here it's a point on the right of the player
                    Vector2 RDistance = new Vector2(player.Position.X + 250, player.Position.Y);
                    distances = (RDistance - this.Position).Length();
                }
                else if (above && !right)
                {
                    //a point above the player
                    Vector2 RDistance = new Vector2(player.Position.X, player.Position.Y + 250);
                    distances = (RDistance - this.Position).Length();
                }
                else if (!above && !right)
                {
                    //A point on the left of the player
                    Vector2 RDistance = new Vector2(player.Position.X - 250, player.Position.Y);
                    distances = (RDistance - this.Position).Length();
                }
                else
                {
                    //A point bellow the player
                    Vector2 RDistance = new Vector2(player.Position.X, player.Position.Y + 250);
                    distances = (RDistance - this.Position).Length();
                }
            }

            if (next.position == 0) distances = 0;

            return distances;
        }

        private int GetCurrentQuadrant(Vector2 centerDistanceVector)
        {
            float dX, dY;
            int badSide = 4;
            dX = centerDistanceVector.X;
            dY = centerDistanceVector.Y;

            if (Math.Abs(dX) > Math.Abs(dY))
            {
                //Closer to the sides
                if (dX > 0)
                    badSide = 2;
                else 
                    badSide = 3;
            }
            else
            {
                //Closer to the top or the bottom
                if (dY > 0)
                    badSide = 0;
                else 
                    badSide = 1;
            }

            return badSide;
        }

        private float ThinkAboutWalls(float centerDistance, int i, float badSide)
        {
            float score = 0;
            if (centerDistance > 250 && i == badSide)
            {
                switch (i)
                {
                    //0 cima
                    case 0:
                        //if (distanceVector.Y > 0)
                        score = -500;
                        break;
                    //1 Baixo
                    case 1:
                        //if (distanceVector.Y < 0)
                        score = -500;
                        break;

                    //2 Esquerda
                    case 2:
                        //if(distanceVector.X < 0)
                        score = -500;
                        break;

                    //3 Direita
                    case 3:
                        //if (distanceVector.X > 0)
                        score = -500;
                        break;

                    //4 Parar
                    case 4:
                        score = -500;
                        break;
                }
            }
            return score;
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
