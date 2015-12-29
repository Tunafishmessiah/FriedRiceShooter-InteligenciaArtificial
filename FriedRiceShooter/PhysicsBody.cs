using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriedRiceShooter
{
    class PhysicsBody : IDisposable
    {
        Rectangle tangle;
        Sprite owner;


        public PhysicsBody(Rectangle tangle, Sprite owner)
        {
            this.tangle = tangle;
            this.owner = owner;
            PhysicsWorld.AddBody(this);
        }

        public virtual void Update(GameTime gametime)
        {
            tangle.Location = new Point((int)owner.Position.X, (int)owner.Position.Y);
        }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                PhysicsWorld.RemoveBody(this);
            }
            disposed = true;
        }

        ~PhysicsBody()
        {
            Dispose(false);
        }

        public Rectangle Tangle
        {
            get { return tangle; }
        }

        public Sprite Owner
        {
            get { return owner; }
        }

    }
}
