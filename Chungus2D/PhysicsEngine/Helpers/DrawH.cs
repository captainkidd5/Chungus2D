using Chungus2D.PhysicsEngine.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chungus2D.PhysicsEngine.Helpers
{
    internal static class DrawH
    {
        private static Texture2D s_pixel;
        private static readonly float s_defaultThickness = 1f;
        public static readonly float s_LayerMultiplier = .00001f;


        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            s_pixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] colorData = { Color.WhiteSmoke };
            s_pixel.SetData(colorData);
        }
        public static float GetYAxisLayerDepth(Vector3 position, float additionalOffset = 0)
        {
            float depth = (position.Y + position.Z + additionalOffset) * s_LayerMultiplier;

            return 1 - depth;
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, float layerDepth, Color? color = null, float? thickness = null)

        {
            color = color ?? Color.White;
            thickness = thickness ?? s_defaultThickness;
            // calculate the distance between the two vectors
            float distance = Vector2.Distance(p1, p2);

            // calculate the angle between the two vectors
            float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            spriteBatch.Draw(s_pixel, p1, null, color.Value, angle, Vector2.Zero, new Vector2(distance, thickness.Value), SpriteEffects.None, layerDepth);
        }

        internal static void DrawRectangle(SpriteBatch spriteBatch, RectangleF rect, float layerDepth, Color? color = null, float? thickness = null)
        {
            float offSetLayerDepth = layerDepth - DrawH.s_LayerMultiplier * 2;
            spriteBatch.Draw(s_pixel, rect.GetXNARectangle(), null,Color.White,0f,Vector2.Zero,SpriteEffects.None, layerDepth );
            //DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), offSetLayerDepth, Color.Red, thickness);
            DrawLine(spriteBatch, new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height), offSetLayerDepth, Color.Red, thickness);
            DrawLine(spriteBatch, new Vector2(rect.X + rect.Width, rect.Y + rect.Height), new Vector2(rect.X, rect.Y + rect.Height), offSetLayerDepth, Color.Red, thickness);
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X, rect.Y), offSetLayerDepth, Color.Red, thickness);


        }
        internal static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, Vector2[] points, float layerDepth, Color? color = null, float? thickness = null)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < points.Length - 1)
                    DrawLine(spriteBatch, center + points[i], center + points[i + 1], layerDepth, color, thickness);
                //else connect the last and first point to close the shape
                else
                    DrawLine(spriteBatch, center + points[i], center + points[0], layerDepth, color, thickness);

            }
        }
    }
}
