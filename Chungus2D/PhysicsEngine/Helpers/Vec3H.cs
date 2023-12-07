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

        /// <summary>
        /// Checks if two given positions are close to one another
        /// </summary>
        /// <param name="marginOfErrorInPixels">Larger the value, the less precise you have to be</param>
        /// <returns></returns>
        public static bool WithinRangeOf(Vector3 currentPos, Vector3 goal, int marginOfErrorInPixels = 2)
        {
            if (currentPos.X + marginOfErrorInPixels > goal.X && currentPos.X - marginOfErrorInPixels < goal.X
                && currentPos.Y + marginOfErrorInPixels > goal.Y && currentPos.Y - marginOfErrorInPixels < goal.Y
                    && currentPos.Z + marginOfErrorInPixels > goal.Z && currentPos.Z - marginOfErrorInPixels < goal.Z)
            {
                return true;
            }
            return false;
        }

        public static bool MoveTowardsVector(Vector3 goal, Vector3 currentPos, ref Vector3 velocity, GameTime gameTime, float errorMargin, float speedMultiplier = 1f)
        {
            // If we're already at the goal, return immediately
            if (WithinRangeOf(currentPos, goal, (int)errorMargin))
                return true;

            // Find the direction from current position to the goal
            Vector3 direction = Vector3.Normalize(goal - currentPos);

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector3.Dot(direction, Vector3.Normalize(goal - currentPos)) + 1) < 0.1f)
                currentPos = goal;

            // Return whether we've reached the goal or not, with a leeway based on errorMargin
            if (Math.Abs(currentPos.X - goal.X) < errorMargin &&
                Math.Abs(currentPos.Y - goal.Y) < errorMargin &&
                Math.Abs(currentPos.Z - goal.Z) < errorMargin)
            {
                return true;
            }

            velocity = direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds * speedMultiplier;

            return false;
        }
    }
}
