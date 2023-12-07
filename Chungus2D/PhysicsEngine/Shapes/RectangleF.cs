using Chungus2D.PhysicsEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chungus2D.PhysicsEngine.Shapes
{
    public struct RectangleF
    {

        public float X;
        public float Y;

        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;



        public void Draw(SpriteBatch spriteBatch, float layerDepth, Color? color = null, float? thickness = null)
        {

            DrawH.DrawRectangle(spriteBatch, this, layerDepth, color, thickness);
        }
 


        public Rectangle GetXNARectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        public static readonly RectangleF Empty = new RectangleF(0, 0, 0, 0);


    }
}
