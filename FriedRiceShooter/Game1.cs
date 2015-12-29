using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FriedRiceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ship mainShip;
        Ship dummy;
        public Texture2D ShipTexture, BulletTexture;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / 1000f);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ShipTexture = Content.Load<Texture2D>("Ship");
            BulletTexture = Content.Load<Texture2D>("Bullet");

            //Initialize ships here, for loading less resources
            mainShip = new Player(new Vector2(graphics.PreferredBackBufferWidth / 4,
               graphics.PreferredBackBufferHeight / 2),
                graphics, ShipTexture, BulletTexture,
                spriteBatch);
            /*mainShip = new AI(new Vector2((graphics.PreferredBackBufferWidth / 4) * 3,
                graphics.PreferredBackBufferHeight / 2),
                graphics,
                ShipTexture,
                BulletTexture,
                spriteBatch,
                null); */

            dummy = new AI(new Vector2((graphics.PreferredBackBufferWidth / 4) * 3,
                graphics.PreferredBackBufferHeight / 2),
                graphics,
                ShipTexture,
                BulletTexture,
                spriteBatch,
                mainShip); 
            
            //((AI)(mainShip)).player = dummy;
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mainShip.Update(gameTime);
            dummy.Update(gameTime);
            base.Update(gameTime);

            PhysicsWorld.CheckCollision();

            Console.WriteLine("Player: " + dummy.hitPoints + " " + mainShip.hitPoints + " :AI");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            mainShip.Draw();
            dummy.Draw();

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
