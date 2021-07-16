using System;
using System.Numerics;
using System.Windows.Forms;

namespace Vector_Editor
{
    public class TListOfPoints : TList<TPoint>
    {
        public bool EqualPoints(TPoint data, int R) //Проверка конкретной точки на близость в 
        {                                           //радиусе R с другими точками в списке
            TNode<TPoint> current = FirstItem;
            while (current != null)
            {
                if (Math.Abs(data.X - current.Data.X) < R && Math.Abs(data.Y - current.Data.Y) < R) 
                    return true;
                current = current.Next;
            }
            return false; //Проверка на близость точек
        }
        public bool EqualPoints(MouseEventArgs e, int R) //Проверка по позиции мыши //точек на близость 
        {                          
            TNode<TPoint> current = FirstItem;
            while (current != null)
            {
                if (Math.Abs(e.X - current.Data.X) < R &&
                    Math.Abs(e.Y - current.Data.Y) < R)
                    return true;
                current = current.Next;
            }
            return false; //Проверка на близость точек
        }

        public bool IsFirst(TPoint data, int R) //Возвращает true если это первый элемент
        {
            TNode<TPoint> current = FirstItem;
            while (current != null)
            {
                if (Math.Abs(data.X - current.Data.X) < R && Math.Abs(data.Y - current.Data.Y) < R)
                {
                    if (current.Data == FirstItem.Data)
                        return true;
                }
                current = current.Next;
            }
            return false; //Проверка на близость точек
        }

        public string GetStringPoint(int position)
        {
            return "(" + position.ToString() + ") x: " + GetItem(position).X.ToString()
            + "; y: " + GetItem(position).Y.ToString();
        }
    
        public int GetNearPoint(TPoint point) //Возвращает позицию ближайшей к данной точки
        {
            double Dist;
            double minDist = GetDistanceToPoint(point, 0);
            int NearPointPosition = 0;

            for (int i = 1; i < Count; i++)
            {
                Dist = GetDistanceToPoint(point, i);

                if (Dist < minDist)
                {
                    minDist = Dist;
                    NearPointPosition = i;
                }
            }
            if (NearPointPosition < 1)
                return NearPointPosition+1;
            else 
                return NearPointPosition;

        }
        public int GetDistanceToPoint(TPoint point,int pos) //Возвращает растояние до точки
        {
            return (int)Math.Sqrt(Math.Pow(point.X - GetItem(pos).X, 2) + Math.Pow(point.Y - GetItem(pos).Y, 2));
        }
        public int GetDistanceToPoint(int pos1, int pos2) //Возвращает растояние до точки
        {
            return (int)Math.Sqrt(Math.Pow(GetItem(pos1).X - GetItem(pos2).X, 2) + 
                                  Math.Pow(GetItem(pos1).Y - GetItem(pos2).Y, 2));
        }

        public void RotateAt(TPoint point, double angle)//поворот всех точек относительно центра
        {
            float angleRad = (float)(angle * Math.PI / 180);
            for (int i = 0; i < Count; i++)
            {
                int x = GetItem(i).X; int y = GetItem(i).Y;
                int xO = point.X; int yO = point.Y;
                SetItem(i, new TPoint(xO + (int)Math.Round((x - xO) * Math.Cos(angleRad) - (y - yO) * Math.Sin(angleRad)),
                    yO + (int)Math.Round((x - xO) * Math.Sin(angleRad) + (y - yO) * Math.Cos(angleRad))));
            }
        }
        public void TransformAt(int xt, int yt) // Перемещение всех точек на указаннное растояние
        {
            for (int i = 0; i < Count; i++)
            {
                SetItem(i, new TPoint(GetItem(i).X + xt, GetItem(i).Y += yt));
            }
        }

        public TPoint GetCenterPoint() //Возвращает центральную точку точек
        {
            double SumX = 0;
            double SumY = 0;
            for (int i = 0; i < Count; i++)
            {
                SumX += GetItem(i).X;
                SumY += GetItem(i).Y;
            }
            return new TPoint(SumX / Count, SumY / Count);
        }

        public Vector2 FindNearestPointOnLine(Vector2 origin, Vector2 end, Vector2 point) //ближайшая к данной точка на
        {                                                                                  //отрезке
            Vector2 heading = Vector2.Normalize(end - origin); //расчитываем направление прямой (от -1 до 1)
            float magnitude = Vector2.Distance(end, origin); // расчитываем длину прямой

            Vector2 diff = point - origin; //расчет растония до точки

            float dotP = Vector2.Dot(diff, heading); //скалярное произведение растояния до точки и направления прямой
            if (dotP < 0) dotP = 0; //отсекание отрицательного скаляра
            else if (dotP > magnitude) dotP = magnitude; //поиск минимального растояния

            return origin + heading * dotP;
        }
    }

}
