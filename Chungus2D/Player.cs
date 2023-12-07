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
        public Collider Collider { get; set; }

        public Vector3 Position { get; set; }

        private KeyboardState _newKeyboardState;
        private KeyboardState _oldKeyboardState;

        private int _speed = 100;
        private int _radius = 16;
        public Player(GraphicsDevice graphics)
        {
            Vector3 Position = new Vector3(graphics.Viewport.Width / 2 / 2, graphics.Viewport.Height / 2  / 2, 40);

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
                Collider.Jump();

            Collider.SetVelocity(velocity);



            _oldKeyboardState = _newKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

    }
}
