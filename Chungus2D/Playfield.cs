using Chungus2D.PhysicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chungus2D
{
    public class Playfield
    {
        public Collider Ground { get; set; }

        private readonly float _width = 400; //x
        private readonly float _length = 400; //y
        private readonly float _height = 4; //z



        public Playfield(GraphicsDevice graphics)
        {
            Vector3 position = new Vector3(graphics.Viewport.Width /2 - _width/2, graphics.Viewport.Height/2 - _length/2, 0);

            Prism groundDimensions = new Prism(position, _width, _length, _height);

            Ground = new PrismCollider(ColliderType.Static, groundDimensions,
                CollisionCategory.Solid, CollisionCategory.Player | CollisionCategory.Item, Vector2.Zero);

            Game1.World.Add(Ground);

            Prism crate = new Prism(new Vector3(50, 50, 0), 32, 32, 32);

            AddObstacle(crate);
            crate.Position = new Vector3(100, 100, 0);
            AddObstacle(crate);
        }


        private void AddObstacle(Prism prism)
        {
           Collider collider = new PrismCollider(ColliderType.Static, prism,
                CollisionCategory.Solid, CollisionCategory.Player | CollisionCategory.Item, Vector2.Zero);

            Game1.World.Add(collider);

        }
        public void Update(GameTime gameTime)
        {
            Ground.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
