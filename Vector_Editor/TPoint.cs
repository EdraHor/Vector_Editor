using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Vector_Editor
{
    public class TPoint
    {
        public Point Point;
        public int X { get => Point.X; set => Point.X = X; } //При вызове X,Y ссылаются на координаты Point
        public int Y { get => Point.Y; set => Point.Y = Y; } //

        public bool Active;
        public bool isInShape;
        public int ShapeIndex;
        public Color color = Color.Black;

        public TPoint() { }
        public TPoint(int xP, int yP) 
        {
            Point.X = xP;
            Point.Y = yP;
        }
        public TPoint(double xP, double yP) //&&&&&&&&&&&&& or not
        {
            Point.X = (int)Math.Round(xP);
            Point.Y = (int)Math.Round(yP);
        }
        public TPoint(string xP, string yP)
        {
            Point.X = Convert.ToInt32(xP);
            Point.Y = Convert.ToInt32(yP);
        }

        public void SetPoint(int xc, int yc )
        {
            Point.X = xc;
            Point.Y = yc;
            Active = true;
        }

        public string GetString()
        {
            return "x: " + Point.X + "; y: " + Point.X;
        }
    }
}
