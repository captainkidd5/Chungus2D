using Chungus2D.EC;
using Chungus2D.PhysicsEngine.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.PhysicsEngine.Modifiers
{
    internal class Gravitizer : PhysicsComponent
    {
        private static readonly float s_Gravity = 500f;
        private static readonly float s_TerminalVelocity = -200f;
        public Gravitizer()
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            Vector3 oldVelocity = Collider.Velocity;

            float delta = s_Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float newVelocity = oldVelocity.Z - delta;

            if (newVelocity < s_TerminalVelocity)
                newVelocity = s_TerminalVelocity;

            Collider.SetVelocity(new Vector3(oldVelocity.X, oldVelocity.Y, newVelocity));



        }

    }
}
