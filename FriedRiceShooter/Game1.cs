﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FriedRiceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player mainShip;
        AI dummy;
        AiC dummy2;
        public Texture2D ShipTexture, BulletTexture;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
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
            //mainShip = new Player(new Vector2(graphics.PreferredBackBufferWidth / 4,
            //    graphics.PreferredBackBufferHeight / 2),
            //    graphics, ShipTexture, BulletTexture,
            //    spriteBatch);

            //dummy = new AI(new Vector2((graphics.PreferredBackBufferWidth / 4) * 3,
            //    graphics.PreferredBackBufferHeight / 2),
            //    graphics,
            //    ShipTexture,
            //    BulletTexture,
            //    spriteBatch,
            //    mainShip);
            
            //dummy2 = new AiC(new Vector2((graphics.PreferredBackBufferWidth / 4) * 3,
            //    graphics.PreferredBackBufferHeight / 2),
            //    graphics,
            //    ShipTexture,
            //    BulletTexture,
            //    spriteBatch,
            //    mainShip);

            dummy2 = new AiC(new Vector2((graphics.PreferredBackBufferWidth / 4),
                graphics.PreferredBackBufferHeight / 2),
                graphics,
                ShipTexture,
                BulletTexture,
                spriteBatch,
                dummy);

            dummy = new AI(new Vector2((graphics.PreferredBackBufferWidth / 4) * 3,
                graphics.PreferredBackBufferHeight / 2),
                graphics,
                ShipTexture,
                BulletTexture,
                spriteBatch,
                dummy2);


        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //mainShip.Update(gameTime);
            dummy2.Update(gameTime);
            dummy.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            //mainShip.Draw();

            // TODO: Add your drawing code here
            dummy.Draw();
            dummy2.Draw();

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
