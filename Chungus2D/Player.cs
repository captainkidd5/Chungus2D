using Chungus2D.PhysicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chungus2D.PhysicsEngine;
namespace Chungus2D
{
    internal class Player
    {
        public Collider Collider { get; set; }

        public Vector3 Position { get; set; }
        public Player()
        {
            Position = new Vector3(50, 50, 0);
            Collider = new SphereCollider(ColliderType.Dynamic, Position, 16, CollisionCategory.Player,
                CollisionCategory.Solid | CollisionCategory.Item,Vector2.Zero);
            Game1.World.Add(Collider);
        }
        public void Update(GameTime gameTime)
        {
            Collider.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }

    }
}
