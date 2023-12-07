using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chungus2D.PhysicsEngine.Helpers;

namespace Chungus2D.PhysicsEngine.Shapes

{
    public class Sphere
    {
        public Vector3 Center { get; set; }
        public float Radius { get; private set; }
        public Vector2[] Points { get; private set; }

        public Vector3 Bottom => new Vector3(Center.X, Center.Y, Center.Z - Radius);

        public Sphere(Vector3 center, int radius, int drawPrecision = 12)
        {
            Center = center;
            Points = new Vector2[drawPrecision];
            Radius = radius;
            Points = GetPoints();
        }

        private Vector2[] GetPoints(int precision = 12)
        {
            Vector2[] points = new Vector2[precision];
            float angle = 0;
            for (int i = 0; i < precision; i++)
            {
                angle += (float)Math.PI * 2 / precision;
                points[i] = new Vector2((float)(Radius * Math.Cos(angle)), (float)(Radius * Math.Sin(angle)));
            }

            return points;
        }

        public bool Intersects(Sphere other)
        {
            float distanceBetweenSpheresSquared =
        (other.Center.X - Center.X) * (other.Center.X - Center.X) +
        (other.Center.Y - Center.Y) * (other.Center.Y - Center.Y) +
        (other.Center.Z - Center.Z) * (other.Center.Z - Center.Z);

            float sumOfRadiiSquared = (Radius + other.Radius) * (Radius + other.Radius);

            //Give it a buffer of 1 ?
            bool didCollide = distanceBetweenSpheresSquared - 1 < sumOfRadiiSquared;

            return didCollide;
        }

 
        public bool Intersects(Prism other)
        {


            // Find the closest point on the prism to the center of the sphere
            float closestX = MathHelper.Clamp(Center.X, other.Left, other.Right);
            float closestY = MathHelper.Clamp(Center.Y, other.Back, other.Front);
            float closestZ = MathHelper.Clamp(Center.Z, other.Bottom, other.Top);

            // Calculate the distance between the sphere's center and the closest point on the prism
            float distanceSquared = (Center.X - closestX) * (Center.X - closestX) +
                                    (Center.Y - closestY) * (Center.Y - closestY) +
                                    (Center.Z - closestZ) * (Center.Z - closestZ);

            // Check if the distance squared is less than the sphere's radius squared
            bool didCollide = distanceSquared <= Radius * Radius;

            return didCollide;
        }
        public void Update(Vector3 position)
        {
            Center = position;

        }
        public void Draw(SpriteBatch spriteBatch, float layerDepth, Color? color = null, float? thickness = null)
        {

            DrawH.DrawCircle(spriteBatch, new Vector2(Center.X, Center.Y - Center.Z), Points, layerDepth, color, thickness);

            //draw line from center of sphere to top of sphere to display radius
            DrawH.DrawLine(spriteBatch, new Vector2(Center.X, Center.Y - Center.Z), new Vector2(Center.X, Center.Y - Center.Z - Radius), layerDepth, color * .5f, thickness);
        }

    }
}
