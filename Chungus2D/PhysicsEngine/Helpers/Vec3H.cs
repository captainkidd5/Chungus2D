using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chungus2D.PhysicsEngine.Helpers
{
    public static class Vec3H
    {
        public static bool AnyNaN(Vector3 vector3)
        {
            return float.IsNaN(vector3.X) || float.IsNaN(vector3.Y) || float.IsNaN(vector3.Z);
        }
    }
}
