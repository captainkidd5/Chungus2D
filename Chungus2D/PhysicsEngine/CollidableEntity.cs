using System;
using System.Collections.Generic;
using System.Text;

namespace _2._5DPhysicsPhysicsEngine
{
    internal interface ICollidableEntity

    {
        public Collider Collider { get;}

        public CollisionCategory GetCollisionCategory();
        public CollisionCategory GetCollidesWith();
    }
}
