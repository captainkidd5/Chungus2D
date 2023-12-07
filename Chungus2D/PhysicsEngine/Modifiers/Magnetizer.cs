
using Chungus2D.PhysicsEngine;
using Chungus2D.PhysicsEngine.Helpers;
using Chungus2D.PhysicsEngine.Modifiers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.PhysicsEngine.Modifiers
{
    internal class Magnetizer : PhysicsComponent
    {
        //Magnetizes to this collider
        private Collider _other;
        public Magnetizer(Collider other)
        {
            _other = other;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector3 currentVelocity = Collider.Velocity;
            Vec3H.MoveTowardsVector(Collider.Position, Collider.Position, ref currentVelocity, gameTime, 1, 8);
            Collider.SetVelocity(currentVelocity);


        }
    }
}
