using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chungus2D.PhysicsEngine
{
    public interface IEntity
    {
        public Vector3 Position { get; set; }

        /// <summary>
        /// This is the level of the ground this entity is on, or directly above
        /// </summary>
        public float BaseZHeight { get; set; }
    }
}
