
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chungus2D.PhysicsEngine
{
    [Flags]
    public enum CollisionCategory
    {
        None = 0,
        All = int.MaxValue,
        Solid = 1,
        Cat2 = 2,
        Cat3 = 4,
        Cat4 = 8,
        Cat5 = 16,
        Cat6 = 32,
        Cat7 = 64,
        Cat8 = 128,
        Cat9 = 256,
        Cat10 = 512,
        Cat11 = 1024,
        Cat12 = 2048,
        Cat13 = 4096,
        Cat14 = 8192,
        Cat15 = 16384,
        Cat16 = 32768,
        Cat17 = 65536,
        Cat18 = 131072,
        Cat19 = 262144,
        Cat20 = 524288,
        Cat21 = 1048576,
        Cat22 = 2097152,
        Cat23 = 4194304,
        Cat24 = 8388608,
        Cat25 = 16777216,
        Cat26 = 33554432,
        Cat27 = 67108864,
        Cat28 = 134217728,
        Cat29 = 268435456,
        Cat30 = 536870912,
        Cat31 = 1073741824
    }

    /// <summary>
    /// Static will never have forces affect it, but can be manually moved. Dynamic will act upon, and be acted upon
    /// </summary>
    public enum ColliderType
    {
        Static = 1,
        Dynamic = 2
    }
    public class PhysicsWorld
    {

        public int DynamicCount { get; private set; }
        public int StaticCount { get; private set; }
        public int ContactCount { get; private set; }



        public static readonly Color S_DynamicColor = Color.Blue;
        public static readonly Color S_StaticColor = Color.Yellow;
        public static readonly Color S_CollidedColor = Color.Red;


        private QuadTree<Collider> _quadTree;
        private List<Collider> _colliders;
        private List<Collider> _collisions;
        public void Initialize()
        {

            CreateQuadTree();
            _colliders = new List<Collider>();
            _collisions = new List<Collider>();

        }

        private void CreateQuadTree()
        {
            _quadTree = new QuadTree<Collider>(4, 16, new Prism(
             new Vector3(0,
             0, 0),
             1600,
            1600,
          1600));
        }
        public void Add(Collider collider)
        {
            _colliders.Add(collider);

        }


        public void Update()
        {
         
                Reset();
                DoSimulation();
            

        }

        private void Reset()
        {
            CreateQuadTree();
            DynamicCount = 0;
            StaticCount = 0;
            ContactCount = 0;
            _quadTree.Clear();
        }

        private void DoSimulation()
        {


            foreach (Collider originalCollider in _colliders)
            {
                originalCollider.ResetCollision();
                _quadTree.Insert(originalCollider, originalCollider.Prism);

            }

            foreach (Collider originalCollider in _colliders)
            {
                if (originalCollider.ColliderType == ColliderType.Static)
                    StaticCount++;
                else if (originalCollider.ColliderType == ColliderType.Dynamic)
                    DynamicCount++;

                ContactCount += originalCollider.ContactCount;

                //fills _collisions list with all possible collisions filtered by quad tree
                _quadTree.FindCollisions(originalCollider, ref _collisions);
                foreach (Collider collision in _collisions)
                {
                    //do not collide with self
                    if (originalCollider == collision)
                        continue;
          
                    //React to collision with other collider (undo any penetration into other colliders)
                    if (originalCollider.Resolve(collision))
                        Console.WriteLine("test");
                }
                _collisions.Clear();


            }
            for (int i = _colliders.Count - 1; i >= 0; i--)
            {
                //modify collider contact list, separation events
                _colliders[i].CleanupPhase();

                if (_colliders[i].FlaggedForRemoval)
                    _colliders.RemoveAt(i);
            }

            //two things touching should just mean a single contact
            ContactCount = ContactCount / 2;



        }


    }
}