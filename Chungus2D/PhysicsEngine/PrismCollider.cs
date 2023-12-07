
using Chungus2D.PhysicsEngine.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Chungus2D.PhysicsEngine
{
    internal class PrismCollider : Collider
    {
        public override float Height => Prism.Height;

        public PrismCollider(ColliderType colliderType, Prism cube, CollisionCategory collisionCategory, CollisionCategory collidesWith, Vector2 offSet) :
            base(colliderType, collisionCategory, collidesWith)
        {
            Prism = cube;
            Position = Prism.Position;

        }
        public PrismCollider(ColliderType colliderType, Vector3 pos, int width, int height, int length, CollisionCategory collisionCategory, CollisionCategory collidesWith, Vector2 offSet) :
           base(colliderType, collisionCategory, collidesWith)
        {
            Prism = new Prism(pos, width, height, length);
        }
        public override Vector3 GetPosition()
        {
            return Prism.Position;
        }
        public override void Update(GameTime gameTime)
        {
       

            if (!IsSensor)
                Position = Prism.Position ;

            base.Update(gameTime);

            Prism.Update(Prism.Position + Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        public override void ForceWarp(Vector3 newPos)
        {
            Prism.Update(newPos);
        }
        public override void CleanupPhase()
        {
            if (Prism.Bottom < 0 && ColliderType != ColliderType.Static)
            {
                Prism.Update(new Vector3(Prism.X, Prism.Y, 0));
                Velocity = new Vector3(Velocity.X, Velocity.Y, 0);

            }

            base.CleanupPhase();
        }
        protected override void ReactToCollision(Collider other)
        {
            base.ReactToCollision(other);

        }
        public override bool DidCollide(Collider other)
        {
            if (other is SphereCollider)
            {
                Sphere otherSphere = (other as SphereCollider).Sphere;

                bool didCollide = Prism.Intersects(otherSphere);

                return didCollide;
            }
            else if (other is PrismCollider)
            {
                return Prism.Intersects(ref other.Prism);
            }

            return false;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (DrawPrism)
            {
                Color color = HadCollision ? PhysicsWorld.S_CollidedColor : ColorFromColliderType();
                Prism.Draw(spriteBatch, LayerDepth, Color.Purple);
            }
        }
    }
}