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

            //debug
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
                
                //Funnção balas

                if (count != 0)
                {

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
                }
                else
                {
                    Vector2 shipDistance = nextPosition - player.Position;

                    if (shipDistance.Length() > 150)
                    {
                        distances = 200 * (150 / shipDistance.Length());
                    }

                    else {
                        //orbit atempt

                        if (this.Position.X - player.Position.X > 0)
                        {
                            if (this.Position.X < nextPosition.X)
                                distances = 100;
                        }
                        else
                        {
                            if (this.Position.X > nextPosition.X)
                                distances = 100;
                        }
                    }

                }

                Vector2 distanceVector = screenSize / 2f - nextPosition;

                float dX, dY;
                int badSide = 4;
                dX = distanceVector.X;
                dY = distanceVector.Y;

                if (dX < 0)
                    dX *= -1;
                if (dY < 0)
                    dY *= -1;

                if (dX > dY)
                {
                    //Closer to the sides
                    if (distanceVector.X > 0)
                        badSide = 2;

                    else badSide = 3;
                }
                else
                {
                    //Closer to the top or the bottom
                    if (distanceVector.Y > 0)
                        badSide = 0;
                    else badSide = 1;
                }


                float centerDistance = (screenSize / 2f - nextPosition).Length();

                float score = 0;

                if (centerDistance > 250)
                {
                    
                    switch (i)
                    {
                        //0 cima
                        case 0:
                            //if (distanceVector.Y > 0)
                            if (badSide == 0) 
                            {
                                score = -500;
                            }
                            break;
                        //1 Baixo
                        case 1:
                            //if (distanceVector.Y < 0)
                            if(badSide == 1)
                            {
                                score = -500;
                            }
                            break;

                        //2 Esquerda
                        case 2:
                            //if(distanceVector.X < 0)
                            if(badSide == 2)
                            {
                                score = -500;
                            }
                            break;

                        //3 Direita
                        case 3:
                            //if (distanceVector.X > 0)
                            if(badSide == 3)
                            {
                                score = -500;
                            }
                            break;

                        //4 Parar
                        case 4:
                            score = -500;
                            break; 
                    }
 
                }


                if (centerDistance > 100)
                    score += distances - (centerDistance * .004f);

                else score += distances;



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
