
using Chungus2D.PhysicsEngine.Modifiers;
using Core.PhysicsEngine.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Chungus2D.PhysicsEngine
{

    public abstract class Collider
    {
        public static readonly Color StaticColor = Color.Yellow;
        public static readonly Color DynamicColor = Color.Blue;

        private List<PhysicsComponent> _components;
        public Vector3 Position { get; set; }
        public bool FlaggedForRemoval { get; set; }

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
        public bool DrawPrism { get; set; } = true;

        public int ContactCount => CurrentContacts.Count;

        /// <summary>
        /// All colliders must have a prism in order to be used within the quad tree
        /// </summary>
        public Prism Prism;

        public abstract float Height { get; }

        public float LayerDepth { get; set; }


        public Collider(ColliderType colliderType, CollisionCategory collisionCategory, CollisionCategory collidesWith)
        {
            _components = new List<PhysicsComponent>();
            ColliderType = colliderType;
            CollisionCategories = collisionCategory;
            CategoriesCollidesWith = collidesWith;
            CurrentContacts = new Dictionary<Collider, bool>();
            IsSensor = false;
        }
        private void AddComponent(PhysicsComponent component)
        {
            _components.Add(component);
            component.Attach(this);
        }
        public abstract void ForceWarp(Vector3 newPos);

        public abstract Vector3 GetPosition();
        public void ResetCollision()
        {
            HadCollision = false;
        }

        public void ApplyGravity()
        {


            AddComponent(new Gravitizer());
        }

        public void Jump()
        {
                Tosser bouncer = new Tosser();
                bouncer.MaxBounces = 0;
                AddComponent(bouncer);
        }

        public virtual void Update(GameTime gameTime)
        {


            for (int i = _components.Count - 1; i >= 0; i--)
            {
                _components[i].Update(gameTime);
                if (_components[i].FlaggedForRemoval)
                    _components.RemoveAt(i);
            }



        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = _components.Count - 1; i >= 0; i--)
                _components[i].Draw(spriteBatch);
        }
        public void Resolve(Collider other)
        {

            if (NoCategoryOverlap(other))
                return;

            if (DidCollide(other))
                ReactToCollision(other);
         

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