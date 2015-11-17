using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriedRiceShooter
{
    class Quim : Ship
    {
        Player player;

        public Quim(Vector2 position, GraphicsDeviceManager graphics, Texture2D ShipTexture, Texture2D BulletTexture, SpriteBatch Sprite, Player player)
            : base(position,graphics,ShipTexture,BulletTexture, Sprite)
        {
            this.player = player;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

        }

        public override void rotate()
        {

        }

        public override void move()
        {
        }
    }
}
