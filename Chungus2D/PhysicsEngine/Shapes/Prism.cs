using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chungus2D.PhysicsEngine.Shapes;

namespace Chungus2D.PhysicsEngine
{
 public struct Prism
{
    public Vector3 Position;
    //X,Y,Z is 
    public float Width;
    public float Length;
    public float Height;

    public float X => Position.X;
    public float Y => Position.Y;
    public float Z => Position.Z;

    public Vector3 Center => new Vector3(X + Width / 2, Y + Length / 2, Z + Height / 2);
    public Prism(Vector3 position, float width, float length, float height)
    {
        Position = position;
        Width = width;
        Length = length;
        Height = height;
    }
    public Prism(Vector3 position, float dimension)
    {
        Position = position;
        Width = dimension;
        Length = dimension;
        Height = dimension;
    }
    public Prism(Vector3 position, Vector3 size)
    {
        Position = position;
        Width = size.X;
        Length = size.Y;
        Height = size.Z;
    }

    public float Left => Position.X;
    public float Right => Position.X + Width;
    public float Top => Position.Z + Height;
    public float Bottom => Position.Z;

    public float Front => Position.Y + Length;
    public float Back => Position.Y;


    public void Update(Vector3 position)
    {
        Set(position.X, position.Y, position.Z);

    }

    public void Draw(SpriteBatch spriteBatch, float layerDepth, Microsoft.Xna.Framework.Color? color = null, float? thickness = null)
    {
        Primitives2D.DrawRectangle(spriteBatch, new RectangleF(Position.X, Position.Y - Position.Z, Width, Length), layerDepth, color, thickness);

        Primitives2D.DrawRectangle(spriteBatch, new RectangleF(Position.X, Position.Y - Height - Position.Z, Width, Length), layerDepth, color * .25f, thickness);

        Primitives2D.DrawLine(spriteBatch, new Vector2(Position.X, Position.Y - Position.Z), new Vector2(Position.X, Position.Y - Height - Position.Z), layerDepth, color * .5f, thickness);



    }


    public bool Contains(float x, float y)
    {
        return (x >= X && x <= Right && y >= Y && y <= Front);
    }



    /// <summary>
    /// Does not accoutn for the Z axis, using this for the quad tree
    /// </summary>
    public bool ContainsNoZ(ref Prism other)
    {
        bool xContained = (this.Left <= other.Left && this.Right >= other.Right);
        bool yContained = (this.Front >= other.Front && this.Back <= other.Back);

        // bool zContained = (this.Top >= other.Top && this.Bottom <= other.Bottom);

        return xContained && yContained;
    }

    /// <summary>
    /// Does not accoutn for the Z axis, using this for the quad tree
    /// </summary>w
    public bool IntersectsNoZ(ref Prism other)
    {

        bool xOverlap = this.X <= other.Right && other.X <= this.Right;
        bool yOverlap = this.Y <= other.Front && other.Y <= this.Front;


        //bool xOverlap = (this.Right >= other.Left && this.Left <= other.Right);

        //bool yOverlap = (this.Back >= other.Front && this.Front <= other.Back);
        // bool zOverlap = (this.Bottom >= other.Top && this.Top <= other.Bottom);

        bool intersects = xOverlap && yOverlap;
        if (intersects)
            Console.WriteLine("test");
        return intersects;
    }

    public bool Intersects(ref Prism other)
    {

        bool xOverlap = this.X <= other.Right && other.X <= this.Right;
        bool yOverlap = this.Y <= other.Front && other.Y <= this.Front;


        //bool xOverlap = (this.Right >= other.Left && this.Left <= other.Right);

        //bool yOverlap = (this.Back >= other.Front && this.Front <= other.Back);
        bool zOverlap = (this.Bottom <= other.Bottom && this.Top <= other.Top);

        bool intersects = xOverlap && yOverlap && zOverlap;

        return intersects;
    }
    public bool Intersects(Sphere sphere)
    {
        // Find the closest point on the prism to the center of the sphere
        float closestX = MathHelper.Clamp(sphere.Center.X, this.Left, this.Right);
        float closestY = MathHelper.Clamp(sphere.Center.Y, this.Back, this.Front);
        float closestZ = MathHelper.Clamp(sphere.Center.Z, this.Bottom, this.Top);

        // Calculate the distance between the sphere's center and the closest point on the prism
        float distanceSquared = Vector3.DistanceSquared(sphere.Center, new Vector3(closestX, closestY, closestZ));

        // Check if the distance squared is less than the sphere's radius squared
        bool didCollide = (distanceSquared <= sphere.Radius * sphere.Radius);

        return didCollide;
    }


    public void Set(float x, float y, float z)
    {
        Position = new Vector3(x, y, z);

    }
    public void Set(float x, float y, float z, float width, float height)
    {
        Position = new Vector3(x, y, z);
        Width = width;
        Length = height;
    }


}
}
