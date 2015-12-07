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
        protected Texture2D texture;
        private Vector2 position;
        protected Color color;
        private Rectangle tangle;//Rectangle to show the texture
        private Rectangle hitbox;
        protected float rotation;
        protected Vector2 scale;
        private SpriteEffects effect;
        private Vector2 origin;
        protected SpriteBatch spriter;
        protected Vector2 screenSize;

        public Sprite(Vector2 position, Texture2D texture, SpriteBatch sprite, GraphicsDeviceManager graphics)
        {
            color = Color.White;
            this.position = position;
            rotation = 0;
            scale = new Vector2(1, 1);
            effect = SpriteEffects.None;

            spriter = sprite;

            this.texture = texture;

            tangle = new Rectangle(0, 0, this.texture.Width, this.texture.Height);

            hitbox = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);

            origin = new Vector2(this.texture.Width / 2, this.texture.Height / 2);

            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        public virtual void Draw()
        {
            spriter.Draw(texture, position,tangle, color, rotation, origin, scale, effect, 1f);
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                hitbox = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);
            }
        }
    }
}
