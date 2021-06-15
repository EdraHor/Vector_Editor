using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector_Editor
{
    public class TShape
    {
        public TLstPointer<TPoint> Shape;
        public int Count { get => Shape.Count; }

        public TShape()
        {
            Shape = new TLstPointer<TPoint>();
        }

        public void AddPoint(TPoint _point)
        {
            Shape.Add(_point);
        }
        public void SetPoint(int position, TPoint _point)
        {
            Shape.SetItem(position, _point);
        }
        public void RemovePoint(int position)
        {
            Shape.Remove(position);
        }
        public Point[] GetArray()
        {
            Point[] Array = new Point[Shape.Count];

            for (int i = 0; i < Shape.Count; i++)
            {
                Array[i] = Shape.GetItem(i).Point;
            }
            return Array;
        }

        public TPoint GetCenterPoint()
        {
            var Count = Shape.Count;
            double SumX = 0;
            double SumY = 0;
            for (int i = 0; i < Count; i++)
            {
                SumX += Shape.GetItem(i).X;
                SumY += Shape.GetItem(i).Y;
            }
            return new TPoint(SumX / Count, SumY / Count);
        }

        public void Moving(TPoint MovePos)
        {
            TPoint Diff,Move,Center;
            Center = GetCenterPoint();
            for (int i = 0; i < Count; i++)
            {
                //Разница между каждой точкой и центром фигуры
                Diff = new TPoint(Center.X - Shape.GetItem(i).X, Center.Y - Shape.GetItem(i).Y);
                //Возвращаем эту разницу ориентируясь на новое место
                Move = new TPoint(MovePos.X - Diff.X, MovePos.Y - Diff.Y);
                Shape.SetItem(i, Move);
            }
        }
    }
}
