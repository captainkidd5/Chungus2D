using Chungus2D.PhysicsEngine;
using Chungus2D.PhysicsEngine.Helpers;
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

        private readonly float _height = 4; //z

        private readonly Vector3 Position;

        private Player _player;

        public Playfield(GraphicsDevice graphics)
        {
            _player = new Player(graphics);
            Position = Vector3.Zero;

            Prism groundDimensions = new Prism(Position, graphics.Viewport.Width, graphics.Viewport.Height, _height);

            Ground = new PrismCollider(ColliderType.Static, groundDimensions,
                CollisionCategory.Solid, CollisionCategory.Player | CollisionCategory.Item, Vector2.Zero);
            Ground.DrawPrism = false;
            Ground.LayerDepth = .99f;
            Game1.World.Add(Ground);

            Prism crate1 = new Prism(Position + new Vector3(50, 50, 0), 32, 32, 32);
            AddObstacle(crate1);

            Prism crate2 = new Prism(Position + new Vector3(200, 200, 0), 24, 24, 24);
            AddObstacle(crate2);

            Prism crate3 = new Prism(Position + new Vector3(200, 250, 0), 48, 48, 12);
            AddObstacle(crate3);
        }


        private void AddObstacle(Prism prism)
        {
           Collider collider = new PrismCollider(ColliderType.Static, prism,
                CollisionCategory.Solid, CollisionCategory.Player | CollisionCategory.Item, Vector2.Zero);

            collider.LayerDepth = DrawH.GetYAxisLayerDepth(collider.Position);

            Game1.World.Add(collider);

        }
        public void Update(GameTime gameTime)
        {
            Ground.Update(gameTime);
            _player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
