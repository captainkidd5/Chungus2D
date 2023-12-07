
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.PhysicsEngine.Modifiers
{
    public class Magnetizer : Component
    {
        private Entity _entityToMoveTowards;
        public Magnetizer(Entity entityToMoveTowards)
        {

            _entityToMoveTowards = entityToMoveTowards;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!(Entity is ICollidableEntity))
                throw new Exception($"Entity {Entity.GetType().Name} does not implement ICollidableEntity");
            ICollidableEntity cEntity = Entity as ICollidableEntity;
            Vector3 currentVelocity = cEntity.Collider.Velocity;
            if (Vec3H.MoveTowardsVector(_entityToMoveTowards.Position,Entity.Position,ref currentVelocity,gameTime,1,8))
            {
                Console.WriteLine("test");
            }
            cEntity.Collider.SetVelocity(currentVelocity);


        }
    }
}
