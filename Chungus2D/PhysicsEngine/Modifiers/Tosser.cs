
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
        public float MaxBounces { get; set; } = 3;

        private Vector3 _acceleration;
        private int _numTimesBounced = 0;
        private Vector3 _multiplier;



        public Tosser(Vector3? initialThrowDirection = null, Vector3? momentum = null, Vector3? multiplier = null)
        {
            if (initialThrowDirection.HasValue)
            {
                _multiplier = multiplier.HasValue ? multiplier.Value : new Vector3(40, 40, 40);
                _acceleration = initialThrowDirection.Value * _multiplier + ( momentum.HasValue ? momentum.Value : Vector3.Zero);
            }
            else
            {
                _acceleration = new Vector3(0,0,150);
            }



        }
        private bool _setInitial;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        
            if (_numTimesBounced > MaxBounces)
            {
  

               Collider.SetVelocity(new Vector3(0, 0, 0));
                Destroy();
       

                return;
            }
    
       
            if (!_setInitial)
            {
                Collider.SetVelocity(new Vector3(Collider.Velocity.X + _acceleration.X, Collider.Velocity.Y + _acceleration.Y, _acceleration.Z));
                _setInitial = true;
                return;
            }
            if (MaxBounces == 0)
            {
                Destroy();
                return;

            }

            //if ((Collider as SphereCollider).Sphere.Bottom.Z <= Entity.BaseZHeight)
            //{

            //    _acceleration = 2 * _acceleration / 3;
            //    _numTimesBounced++;
          
            //    Collider.SetVelocity(new Vector3(_acceleration.X, _acceleration.Y, _acceleration.Z));


            //}


        }


    }
}
