
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace Chungus2D.PhysicsEngine
{

    public abstract class Collider
    {
        public static readonly Color StaticColor = Color.Yellow;
        public static readonly Color DynamicColor = Color.Blue;

        public IEntity Entity { get; set; }
        public bool FlaggedForRemoval { get; set; }
        protected Vector2 OffSet { get; set; }
        public ColliderType ColliderType { get; private set; }
        public CollisionCategory CollisionCategories { get; set; }
        public CollisionCategory CategoriesCollidesWith { get; set; }

        /// <summary>
        /// This collider will not be physically affected by these categories
        /// </summary>
        public CollisionCategory UnaffectedByCategories { get; set; }
        /// <summary>
        /// Categories here will not be physically affected by this collider
        /// </summary>
        public CollisionCategory UnaffectsCategories { get; set; }


        public bool IsSensor { get; set; }
        public Vector3 Velocity { get; protected set; }
        public bool HadCollision { get; protected set; }
        public object UserData { get; set; }

        public Action<Collider> OnCollidesAction { get; set; }
        public Action<Collider> OnSeparatesAction { get; set; }

        public float Mass { get; set; } = 1f;
        public float Restituion { get; set; } = .7f;

        public Dictionary<Collider, bool> CurrentContacts;

        public int ContactCount => CurrentContacts.Count;

        public Prism Prism;

        public abstract float Height { get; }

        protected float HighestZEncountered { get; private set; }

        public Collider(ColliderType colliderType, CollisionCategory collisionCategory, CollisionCategory collidesWith, Vector2 offSet)
        {

            ColliderType = colliderType;
            CollisionCategories = collisionCategory;
            CategoriesCollidesWith = collidesWith;
            CurrentContacts = new Dictionary<Collider, bool>();
            IsSensor = false;
            OffSet = offSet;
        }

        public abstract void ForceWarp(Vector3 newPos);

        public abstract Vector3 GetPosition();
        public void ResetCollision()
        {
            HadCollision = false;
        }

        public virtual void Update(GameTime gameTime)
        {

            //Apply much less friction at sea level
            //if (ColliderType == ColliderType.Dynamic &&
            //  Entity.Position.Z <= Entity.BaseZHeight)
            //{
            //    if (Velocity.Length() > 0.01f)
            //    {
            //        float friction = Entity.Position.Z > StageManager.Sea.SeaLevel ? 0.1f : 0.01f;

            //        Vector3 frictionForce = -friction * Velocity;

            //        // Apply the friction force to the velocity
            //        //Ignore vertical friction, gravitizers take care of that
            //        Velocity += new Vector3(frictionForce.X, frictionForce.Y, 0);
            //        // Velocity = new Vector3(Velocity.X, Velocity.Y, 0);
            //    }

            //}
            HighestZEncountered = 0;



        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        public bool Resolve(Collider other)
        {
       
            if (NoCategoryOverlap(other))
                return false;

            if (DidCollide(other))
            {
    
                if (ColliderType == ColliderType.Dynamic)
                {

                    if (other.CollisionCategories == CollisionCategory.HeightArea)
                    {

                        float otherHeight = (float)other.UserData;
                        if (Prism.Bottom > other.Prism.Bottom && otherHeight > HighestZEncountered)
                            HighestZEncountered = otherHeight;
                    }

                }
                ReactToCollision(other);
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual void CleanupPhase()
        {


            if (CurrentContacts.Count > 0)
            {
                if (FlaggedForRemoval)
                {
                    foreach (var kvp in CurrentContacts)
                        kvp.Key.CurrentContacts.Remove(this);
                    return;
                }

                List<Collider> collidersToRemove = new List<Collider>();
                foreach (var kvp in CurrentContacts)
                {
                    if (!kvp.Value)
                    {
                        //false means we had a collision last frame, but not this one, therefore we should remove it and fire the separate action
                        collidersToRemove.Add(kvp.Key);
                        OnSeparates(kvp.Key);
                        kvp.Key.OnSeparates(this);
                        kvp.Key.CurrentContacts.Remove(this);
                    }
                    CurrentContacts[kvp.Key] = false;

                }
                foreach (var collider in collidersToRemove)
                {
                    CurrentContacts.Remove(collider);
                }
            }

        }
        /// <summary>
        /// Only dynamic colliders should react to collisions. Static colliders will be unaffected
        /// </summary>
        protected virtual void ReactToCollision(Collider other)
        {

     
            if (!CurrentContacts.ContainsKey(other))
            {
                CurrentContacts.Add(other, true);
                OnCollides(other);
            }
            else
            {
                //We're still in contact, signal this to prevent a separate event in the cleanup phase
                CurrentContacts[other] = true;
            }
            HadCollision = true;
        }
        private bool NoCategoryOverlap(Collider other)
        {
            return ((CollisionCategories & other.CategoriesCollidesWith) == CollisionCategory.None ||
                 (CategoriesCollidesWith & other.CollisionCategories) == CollisionCategory.None);
        }

        public abstract bool DidCollide(Collider other);
        public void SetVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        protected Color ColorFromColliderType()
        {
            Color color = StaticColor;
            switch (ColliderType)
            {
                case ColliderType.Static:
                    color = StaticColor;
                    break;
                case ColliderType.Dynamic:
                    color = DynamicColor;
                    break;
                default:
                    color = StaticColor;
                    break;
            }

            //reduce color intensity if sensor
            if (IsSensor)
                color.A = 100;

            return color;
        }



        public void OnCollides(Collider other)
        {

            OnCollidesAction?.Invoke(other);
        }

        public void OnSeparates(Collider other)
        {
            OnSeparatesAction?.Invoke(other);

        }
    }
}