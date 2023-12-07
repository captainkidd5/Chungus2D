using Core.Globals.Classes;
using Core.Globals.Classes.EC;
using Core.SoundEngine.Classes;
using Core.SpriteEngine.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Core.DataModels.Enums;

namespace Core.PhysicsEngine.Modifiers
{
    public class Tosser : Component
    {
        public float MaxBounces { get; set; } = 3;

        //private static readonly float s_baseAcceleration = 5f;
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
            ICollidableEntity collidableEntity = Entity as ICollidableEntity;

            if (!(Entity is ICollidableEntity))
                throw new Exception($"Entity {Entity.GetType().Name} does not implement ICollidableEntity");
            if (_numTimesBounced > MaxBounces)
            {
  

                collidableEntity.Collider.SetVelocity(new Vector3(0, 0, 0));
                Destroy();
       

                return;
            }
    
       
            if (!_setInitial)
            {
                 collidableEntity.Collider.SetVelocity(new Vector3(collidableEntity.Collider.Velocity.X + _acceleration.X, collidableEntity.Collider.Velocity.Y + _acceleration.Y, _acceleration.Z));
                _setInitial = true;
                return;
            }
            if (MaxBounces == 0)
            {
                Destroy();
                return;

            }

            if ((collidableEntity.Collider as SphereCollider).Sphere.Bottom.Z <= Entity.BaseZHeight)
            {

                _acceleration = 2 * _acceleration / 3;
                _numTimesBounced++;
                string soundAtTile = Entity.GetSoundAtTile();
                if (soundAtTile != null)
                    SoundUtility.PlayEffect(soundAtTile,entityPos: Entity.Position);
                collidableEntity.Collider.SetVelocity(new Vector3(_acceleration.X, _acceleration.Y, _acceleration.Z));


            }


        }


    }
}
