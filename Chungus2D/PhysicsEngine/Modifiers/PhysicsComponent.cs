using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chungus2D.PhysicsEngine.Modifiers
{
    internal abstract class PhysicsComponent
    {

        public Collider Collider { get; protected set; }
        public bool FlaggedForRemoval { get; private set; }


        /// <summary>

        public virtual void Update(GameTime gameTime)
        {


        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Attach(Collider collider)
        {
            Collider = collider;
        }


        public virtual void Destroy()
        {
            FlaggedForRemoval = true;
        }
    }

}