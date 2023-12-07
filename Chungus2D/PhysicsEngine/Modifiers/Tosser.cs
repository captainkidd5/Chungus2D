
using Chungus2D.PhysicsEngine.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.PhysicsEngine.Modifiers
{
    internal class Tosser : PhysicsComponent
    {

        private Vector3 _acceleration;
        private Vector3 _multiplier;
        private readonly float _upwardsAcceleration = 150f;

        public Tosser(Vector3? initialThrowDirection = null, Vector3? momentum = null, Vector3? multiplier = null)
        {
            if (initialThrowDirection.HasValue)
            {
                _multiplier = multiplier.HasValue ? multiplier.Value :Vector3.One;
                _acceleration = initialThrowDirection.Value * _multiplier + (momentum.HasValue ? momentum.Value : new Vector3(0,0, _upwardsAcceleration));
            }
            else
            {
                _acceleration = new Vector3(0, 0, _upwardsAcceleration);
            }



        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Collider.SetVelocity(new Vector3(Collider.Velocity.X + _acceleration.X, Collider.Velocity.Y + _acceleration.Y, _acceleration.Z));

            Destroy();






        }


    }
}
