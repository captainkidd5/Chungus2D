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

namespace Chungus2D
{
    internal class Player
    {
        public Collider Collider { get; set; }

        public Vector3 Position { get; set; }

        private KeyboardState _newKeyboardState;
        private KeyboardState _oldKeyboardState;

        private int _speed = 50;
        public Player()
        {
            Position = new Vector3(80, 80, 50);
            Collider = new SphereCollider(ColliderType.Dynamic, Position, 16, CollisionCategory.Player,
                CollisionCategory.Solid | CollisionCategory.Item, Vector2.Zero);
            Collider.ApplyGravity();
            Game1.World.Add(Collider);
        }
        public void Update(GameTime gameTime)
        {
            Collider.Update(gameTime);
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
                Collider.Jump();

            Collider.SetVelocity(velocity);



            _oldKeyboardState = _newKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

    }
}
