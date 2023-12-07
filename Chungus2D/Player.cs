using Chungus2D.PhysicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chungus2D.PhysicsEngine;
using Microsoft.Xna.Framework.Input;
using Chungus2D.PhysicsEngine.Helpers;

namespace Chungus2D
{
    internal class Player
    {

        private readonly int _speed = 100;
        private readonly int _radius = 16;
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }

        private KeyboardState _newKeyboardState;
        private KeyboardState _oldKeyboardState;

    
        public Player(GraphicsDevice graphics)
        {
            Position = new Vector3(graphics.Viewport.Width / 2 / 2, graphics.Viewport.Height / 2  / 2, 40);

            Collider = new SphereCollider(ColliderType.Dynamic, Position, _radius, CollisionCategory.Player,
                CollisionCategory.Solid | CollisionCategory.Item);

            Collider.DrawPrism = false;
            Collider.ApplyGravity();
            Game1.World.Add(Collider);
        }
        public void Update(GameTime gameTime)
        {
            Collider.Update(gameTime);
            Collider.LayerDepth = DrawH.GetYAxisLayerDepth(Collider.Position);

            _newKeyboardState = Keyboard.GetState();

            //Vector3 velocity = Collider.Velocity;
            //retain gravity
            Vector3 velocity = new Vector3(0,0, Collider.Velocity.Z);

            if (_newKeyboardState.IsKeyDown(Keys.W))
                velocity.Y = -_speed;
            if (_newKeyboardState.IsKeyDown(Keys.S))
                velocity.Y = _speed;
            if (_newKeyboardState.IsKeyDown(Keys.A))
                velocity.X= -_speed;
            if (_newKeyboardState.IsKeyDown(Keys.D))
                velocity.X = _speed;

            if (_newKeyboardState.IsKeyDown(Keys.Space) && _oldKeyboardState.IsKeyUp(Keys.Space))
                Collider.Jump(new Vector3(0,0, 100),Vector3.Zero, Vector3.Zero);
            if(_newKeyboardState.IsKeyDown(Keys.Q) && _oldKeyboardState.IsKeyUp(Keys.Q))
            {
                Collider littleSphereCollider = new SphereCollider(ColliderType.Dynamic, new Vector3(Position.X, Position.Y, Position.Z - 8), 12, CollisionCategory.Item,
      CollisionCategory.Solid | CollisionCategory.Item | CollisionCategory.Player);
                littleSphereCollider.ApplyGravity();
                littleSphereCollider.DrawPrism = false;
                Collider.Jump(new Vector3(0, 0, 100), Vector3.Zero, Vector3.Zero);

            }
            Collider.SetVelocity(velocity);


            //Not used, but shows how you could obtain a position from the collider to perhaps draw the player
            Position = Collider.Position;

            _oldKeyboardState = _newKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //todo: draw player
        }

    }
}
