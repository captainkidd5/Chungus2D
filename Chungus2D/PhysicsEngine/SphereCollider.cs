using Chungus2D.PhysicsEngine;
using Chungus2D.PhysicsEngine.Helpers;
using Chungus2D.PhysicsEngine.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chungus2D.PhysicsEngine
{
    internal class SphereCollider : Collider
    {
        public Sphere Sphere;

        public override float Height => Sphere.Radius * 2;
        public SphereCollider(ColliderType colliderType, Vector3 center, int radius, CollisionCategory collisionCategory, CollisionCategory collidesWith,
            int drawPrecision = 12) : base(colliderType, collisionCategory, collidesWith)
        {
            Sphere = new Sphere(center, radius, drawPrecision);
            Prism = new Prism(new Vector3(Sphere.Center.X - Sphere.Radius,
                Sphere.Center.Y - Sphere.Radius, Sphere.Center.Z), Sphere.Radius * 2, Sphere.Radius * 2, Sphere.Radius * 2);
        }
        public override void ForceWarp(Vector3 newPos)
        {
            Sphere.Update(newPos);
            Prism.Update(new Vector3(Sphere.Center.X - Sphere.Radius, Sphere.Center.Y - Sphere.Radius, Sphere.Center.Z));

        }
        public override Vector3 GetPosition()
        {
            return Sphere.Center;
        }
        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);

            Sphere.Update(Sphere.Center + Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            Prism.Update(new Vector3(Sphere.Center.X - Sphere.Radius, Sphere.Center.Y - Sphere.Radius, Sphere.Center.Z));




        }

        public override void CleanupPhase()
        {

            if (ColliderType != ColliderType.Static)
               Position = Sphere.Bottom;
            base.CleanupPhase();
        }
        /// <summary>
        /// This should never be called by static colliders
        /// </summary>
        public override bool DidCollide(Collider other)
        {

            if (other is SphereCollider)
            {
                Sphere otherSphere = (other as SphereCollider).Sphere;


                bool didCollide = Sphere.Intersects(otherSphere);


                return didCollide;
            }
            else if (other is PrismCollider)
            {
                return Sphere.Intersects(other.Prism);
            }

            return false;
        }

        private void RestitutionCalculations(Collider other, Vector3 collisionNormal)
        {
            if (other.ColliderType == ColliderType.Static)
                return;

            // Calculate relative velocity
            Vector3 relativeVelocity = Velocity - other.Velocity;

            // Calculate the impulse (change in momentum) based on restitution (elasticity)
            float impulse = -(1 + Restituion) * Vector3.Dot(relativeVelocity, collisionNormal);

            // Apply the impulse to both spheres
            float inverseMass1 = 1f / Mass;
            float inverseMass2 = 1f / other.Mass;
            float totalInverseMass = inverseMass1 + inverseMass2;

            Vector3 impulsePerMass = collisionNormal * (impulse / totalInverseMass);
            if (Vec3H.AnyNaN(impulsePerMass))
                throw new Exception($"Nan");
            Velocity += impulsePerMass * inverseMass1;

            if (other.ColliderType == ColliderType.Dynamic && UnaffectsCategories != other.CollisionCategories)
                other.SetVelocity(other.Velocity - impulsePerMass * inverseMass2);
        }

        protected override void ReactToCollision(Collider other)
        {
            base.ReactToCollision(other);
            //sensors cannot physically affect, or be affected by other colliders



            if (IsSensor || other.IsSensor || ColliderType == ColliderType.Static)
                return;
            if (UnaffectedByCategories == other.CollisionCategories || other.UnaffectedByCategories == CollisionCategories)
                return;

            if (other.UnaffectsCategories == CollisionCategories)
                return;
            if (other is SphereCollider)
            {
                
                Sphere otherSphere = (other as SphereCollider).Sphere;

                // Calculate the vector from the center of Sphere to the center of otherSphere
                Vector3 collisionVector = otherSphere.Center - Sphere.Center;

                // Calculate the distance between the centers of the two spheres
                float distanceBetweenSpheres = collisionVector.Length();

                // Calculate the combined radii
                float combinedRadii = Sphere.Radius + otherSphere.Radius;

                if (distanceBetweenSpheres < combinedRadii)
                {
                    // Calculate the penetration depth
                    float penetrationDepth = combinedRadii - distanceBetweenSpheres;


                    // Calculate the collision normal (unit vector pointing from Sphere to otherSphere)
                    Vector3 collisionNormal = collisionVector / distanceBetweenSpheres;

                    //Force the sphere upwards for edge case
                    if (Vec3H.AnyNaN(collisionNormal))
                        collisionNormal = new Vector3(0, 0, 1);

                    // Move the spheres apart to resolve the collision
                    Sphere.Center -= collisionNormal * penetrationDepth;

                    RestitutionCalculations(other, collisionNormal);

                }
            }
            else if (other is PrismCollider)
            {
                Prism otherPrism = (other as PrismCollider).Prism;

                // Calculate the closest point on the Prism to the Sphere
                float closestX = Math.Max(otherPrism.Left, Math.Min(Sphere.Center.X, otherPrism.Right));
                float closestY = Math.Max(otherPrism.Back, Math.Min(Sphere.Center.Y, otherPrism.Front));
                float closestZ = Math.Max(otherPrism.Bottom, Math.Min(Sphere.Center.Z, otherPrism.Top));

                // Calculate the distance from the Sphere's center to the closest point on the Prism
                Vector3 closestPoint = new Vector3(closestX, closestY, closestZ);
                float distance = Vector3.Distance(Sphere.Center, closestPoint);

                if (distance  < Sphere.Radius)
                {
                    if (closestPoint - Sphere.Center == Vector3.Zero)
                        return;
                    // There is a collision
                    // Calculate the penetration depth
                    float penetrationDepth = (float)Sphere.Radius - distance;

                    // Calculate the collision normal (pointing from the Sphere to the closest point on the Prism)
                    Vector3 collisionNormal = Vector3.Normalize(closestPoint - Sphere.Center);

                    //Force the sphere upwards for edge case
                    if (Vec3H.AnyNaN(collisionNormal))
                    {
                        collisionNormal = new Vector3(0, 0, 1);
                    }

                    // Move the Sphere to resolve the collision

                    Sphere.Center -= collisionNormal * penetrationDepth;
                    RestitutionCalculations(other, collisionNormal);

                }


            }
            float newFrictionAmt =1 - (1 + Friction) / 100;

            Velocity *= newFrictionAmt;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                Color color = HadCollision ? PhysicsWorld.S_CollidedColor : ColorFromColliderType();

                Sphere.Draw(spriteBatch, LayerDepth,  color);

            if (DrawPrism)
            {
                Prism.Draw(spriteBatch, LayerDepth, color * .15f);
            }
        }
    }
}