using Core.Globals.Classes.EC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.PhysicsEngine.Modifiers
{
    public class Gravitizer : Component
    {
        private static readonly float s_Gravity = 10f;
        private static readonly float s_TerminalVelocity = -200f;
        public Gravitizer()
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
     
            ICollidableEntity collidableEntity = Entity as ICollidableEntity;
            //keep this commented out, gravity should be applied at all times
            //if (Entity.Position.Z <= Entity.BaseZHeight)
            //    return;
            Vector3 oldVelocity = collidableEntity.Collider.Velocity;
        
            float delta = s_Gravity  * 50 *(float)gameTime.ElapsedGameTime.TotalSeconds;
            float newVelocity = oldVelocity.Z - delta;

            if(newVelocity < s_TerminalVelocity)
                newVelocity = s_TerminalVelocity;

            collidableEntity.Collider.SetVelocity(new Vector3(oldVelocity.X, oldVelocity.Y, newVelocity));



        }
        
    }
}
