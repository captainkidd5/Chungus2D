using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chungus2D.PhysicsEngine
{
    internal interface ICollidableEntity
    {
        public Collider Collider { get;}

        public float LayerDepth { get; set; }
        public void WarpTo(Vector3 newPosition);
        public CollisionCategory GetCollisionCategory();
        public CollisionCategory GetCollidesWith();
    }
}
