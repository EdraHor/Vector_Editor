using System;
using System.Drawing;
namespace Vector_Editor
{
    public class TPoint
    {
        //Добавить размер?
        public int x;
        public int y;
        public bool Active;
        public Color color = Color.Black;

        public TPoint() { }
        public TPoint(int xP, int yP) 
        {
            x = xP;
            y = yP;
        }
        public TPoint(double xP, double yP)
        {
            x = (int)Math.Round(xP);
            y = (int)Math.Round(yP);
        }
        public TPoint(string xP, string yP)
        {
            x = Convert.ToInt32(xP);
            y = Convert.ToInt32(yP);
        }

        public void SetPoint(int xc, int yc )
        {
            x = xc;
            y = yc;
            Active = true;
        }
        public void SetDisable()
        {
            x = 0;
            y = 0;
            Active = false;
            color = Color.Black;
        }

        public Tuple<int,int> GetPoints()
        {
            return Tuple.Create(x, y);
        }
        public int GetPointX()
        {
            return x;
        }
        public int GetPointY()
        {
            return y;
        }
        public string GetString()
        {
            return "x: " + x + "; y: " + y;
        }
    }
}
