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
    class Sprite
    {
        public Texture2D texture;
        public Vector2 Position;
        public Color color;
        Rectangle Tangle;//Rectangle to show the texture
        Rectangle Hitbox;
        public float rotation;
        public Vector2 Scale;
        private SpriteEffects Effect;
        private Vector2 Origin;
        public SpriteBatch Spriter;
        public Vector2 ScreenSize;

        public Sprite(Vector2 Pos ,  Texture2D texture, SpriteBatch sprite, GraphicsDeviceManager graphics)
        {
            color = Color.White;
            Position = Pos;
            rotation = 0;
            Scale = new Vector2(1, 1);
            Effect = SpriteEffects.None;

            this.Spriter = sprite;

            this.texture = texture;

            Tangle = new Rectangle(1, 1, this.texture.Width, this.texture.Height);

            Hitbox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.texture.Width, this.texture.Height);

            Origin = new Vector2(this.texture.Width / 2, this.texture.Height / 2);

            ScreenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        public void Draw()
        {
            this.Spriter.Draw(texture, Position,Tangle, color, this.rotation,Origin,Scale, Effect, 1f);
        }
    }
}
