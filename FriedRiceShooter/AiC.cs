using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriedRiceShooter
{
    class AiC : Ship
    {
    struct state
    {
        public bool shoting;
        public int position;
        public float stateScore;

        public state(bool s, int p, int sc)
        {
            this.shoting = s;
            this.position = p;
            this.stateScore = sc;
        }
    }
        public Ship player;
        private state next; 

        public AiC(Vector2 position, GraphicsDeviceManager graphics, Texture2D shipTexture, Texture2D bulletTexture, SpriteBatch sprite, Ship player)
            : base(position,graphics,shipTexture,bulletTexture, sprite)
        {
            this.player = player;
            next.position = 4;
        }

        public override void Update(GameTime gametime)
        {
            try
            {
                next = Think();
            }
            catch (Exception)
            {
            }

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
            return nextPosition;
        }

        private state Think()
        {
            state best =  new state(false, 0, 0);
            state actual = new state(false, 0, 0);

            List<Bullet> fEnemy = new List<Bullet>();

            float bulletDistance = 0, wallDistance = 0, playerDistance = 0;
            int actualPosition = 0;
            bool actualShot;

            #region predictions
            foreach (Bullet shot in player.shotsFired)
            {
                //Saber as proximas posições das balas
                Bullet N = shot;
                N.Position += (N.velocity * Bullet.speed);

                fEnemy.Add(N);
            }
            #endregion

            for (int i = 0; i <= 4; i++)
            {
                bulletDistance = 0;
                wallDistance = 0;
                playerDistance = 0;

                actual = new state(false, 0, 0);

                Vector2 nextPosition = Vector2.Zero;


                switch (i)
                {
                        //move up
                    case 0:
                        actualPosition = 0;
                        nextPosition = new Vector2(Position.X + this.texture.Width/2, Position.Y  -(this.texture.Height/2) - speed);
                        break;

                        //Move down
                    case 1:
                        actualPosition = 1;
                        nextPosition = new Vector2(Position.X + this.texture.Width / 2, Position.Y - (this.texture.Height / 2) + speed);
                        break;

                        //Move left
                    case 2:
                        actualPosition = 2;
                        nextPosition = new Vector2(Position.X + (this.texture.Width / 2) - speed, Position.Y - (this.texture.Height / 2));
                        break;

                        //Move right
                    case 3:
                        actualPosition = 3;
                        nextPosition = new Vector2(Position.X + (this.texture.Width / 2) + speed, Position.Y - (this.texture.Height / 2));
                        break;

                        //Don't move
                    case 4:
                        actualPosition = 4;
                        nextPosition = Position;
                        break;
                }


                //Dar score pela distancia aos tiros
                if (player.shotsFired.Count > 0)
                {
                    foreach (Bullet shot in fEnemy)
                    {
                        //Verifica a distancia entre a proxima posição da nave e a proxima posição dos tiros
                        Vector2 distance = nextPosition - shot.Position;
                        bulletDistance += distance.Length() * 1;
                    }
                }


                //Dar score pela distancia as paredes, sendo que o score maximo é o centro do ecrã
                //So da score pela parede mais proxima no eixo do X

                Vector2 screenPosX = new Vector2(this.screenSize.X - Position.X, this.screenSize.Y - Position.Y);

                wallDistance = screenPosX.Length() * -.8f;


                //Avaliar distancia ao jogador
                Vector2 dist = new Vector2(nextPosition.X - player.Position.X, nextPosition.Y - player.Position.Y);

                if (dist.Length() <= 150)
                {
                    actualShot = true;
                    playerDistance += dist.Length() * 1.8f;
                }
                else
                {
                    actualShot = false;
                    playerDistance += dist.Length() * -.4f;
                }

                ////Avaliar aproximação ao jogador
                Vector2 lastDist = new Vector2(Position.X - player.Position.X, Position.Y - player.Position.Y);
                if (lastDist.Length() < dist.Length())
                { playerDistance *= 2; }
                else
                { playerDistance *= 1; }
                

                actual.stateScore = bulletDistance + wallDistance + playerDistance ;
               
                actual.position = actualPosition;
                actual.shoting = actualShot;
                Console.WriteLine(actual.stateScore);

                if (actual.stateScore > best.stateScore)
                    best = actual;
            }

            return best;
        }

        //private void MyThink()
        //{
        //    float defensivo = bullets / player.bullets == 0? -1 : player.bullets; 

        //    List<Vector2> positions = new List<Vector2>();
        //    foreach (Bullet shot in player.ShotsFired)
        //    {
        //        positions.Add(shot.Position + shot.speed * shot.getVelocity());
        //    }

        //    float bestScore = float.NegativeInfinity;
        //    int bestIndex = 4;
        //    for (int i = 0; i <= 4; i++)
        //    {
        //        float distances = 0;
        //        Vector2 nextPosition;
        //        nextPosition = CalculateNextPosition(i);
        //        foreach (Vector2 bullet in positions)
        //        {
        //            distances += (bullet - nextPosition).Length();
        //        }

        //        float centerDistance = (nextPosition - ScreenSize/2).Length();

        //        float score = distances * 2 - centerDistance;

        //        if (score > bestScore)
        //        {
        //            bestIndex = i;
        //            bestScore = score;
        //        }
        //    }

        //    next.position = bestIndex;
        //    next.shoting = true;
        //}
    }
}