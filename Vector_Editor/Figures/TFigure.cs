using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Vector_Editor
{
    public class TFigure : IEnumerable<TPoint> //ICloneable
    {
        public TListOfPoints Points { get; protected set; } //Список точек фигуры
        public int Count { get => Points.Count; } //Количество точек фигуры
        public Guid ID; //уникальный номер фигуры
        public bool isSelect; //Выделена ли фигура
        public bool isBezier = false;
        public bool isClosed = false;

        public TPoint FirstPoint => Points.FirstItem.Data;
        public TPoint LastPoint => Points.LastItem.Data;

        public TFigure() //Конструктор инициализирует список точек фигуры.
        {
            Points = new TListOfPoints();
            ID = Guid.NewGuid();
        }
        public TFigure(TListOfPoints ShapePoints) //Конструктор создает фигуру из списка 
        {
            ID = Guid.NewGuid();
            Points = new TListOfPoints();
            for (int i = 0; i < ShapePoints.Count; i++)
            {
                Points.Add(ShapePoints.GetItem(i));
            }
        }

        public virtual void Draw(Graphics g)
        {
            var i = 0;
            if (!isClosed) //если полигон еще не закончен, то мы не ресуем последнюю линию
                foreach (var item in Points)
                {
                    g.DrawLine(Options.Pen, item.ToPoint(), GetItem(i - 1).ToPoint());
                    i++;
                }
            else
            if (isClosed)
            {
                g.FillPolygon(Options.Brush, GetArray());
                g.DrawPolygon(Options.Pen, GetArray());
            }
        }


        public void AddPoint(TPoint _point) //Добавляет фигуру в конец
        {
            Points.Add(_point);
        }
        public TPoint GetItem(int pos) => Points.GetItem(pos);
        public void SetPoint(int position, TPoint _point) //Устанавливает значение
        {
            Points.SetItem(position, _point);
        }
        public void RemovePoint(int position) //удаляет точку
        {
            if (Points.Count > 1)
                Points.Remove(position);
            else
                Options.ListOfShapes._list.Remove(ID);
        }
        public Point[] GetArray() //Преобразуем список TPoint в массив Point и возвращает его
        {
            Point[] Array = new Point[Points.Count];

            for (int i = 0; i < Points.Count; i++)
            {
                Array[i] = Points.GetItem(i)._point;
            }
            return Array;
        }

        public Point[] GetArray(int upToPos) //Преобразуем список TPoint в массив Point и возвращает его
        {
            Point[] Array = new Point[upToPos];

            for (int i = 0; i < upToPos; i++)
            {
                Array[i] = Points.GetItem(i)._point;
            }
            return Array;
        }

        public TPoint GetCenterPoint()
        {
            var Count = Points.Count;
            double SumX = 0;
            double SumY = 0;
            for (int i = 0; i < Count; i++)
            {
                SumX += Points.GetItem(i).X;
                SumY += Points.GetItem(i).Y;
            }
            return new TPoint(SumX / Count, SumY / Count);
        }

        public void Moving(TPoint MovePos) //Перемещает фигуру в указанную точку
        {
            TPoint Diff, Move, Center;
            Center = GetCenterPoint();
            for (int i = 0; i < Count; i++)
            {
                //Разница между каждой точкой и центром фигуры
                Diff = new TPoint(Center.X - Points.GetItem(i).X, Center.Y - Points.GetItem(i).Y);
                //Возвращаем эту разницу ориентируясь на новое место
                Move = new TPoint(MovePos.X - Diff.X, MovePos.Y - Diff.Y);
                Points.SetItem(i, Move);
            }
        }

        public void Select() //выделяет фигуру и все ее точки
        {
            isSelect = true;
            for (int i = 0; i < Count; i++)
            {
                Points.GetItem(i).Select();
            }
        }
        public void Deselect() //снимает выделение с фигуруры и всех ее точек
        {
            isSelect = false;
            for (int i = 0; i < Count; i++)
            {
                Points.GetItem(i).Deselect();
            }
        }

        // реализация интерфейса IEnumerable для быстрого перебора всех элементов списка
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<TPoint> IEnumerable<TPoint>.GetEnumerator()
        {
            TNode<TPoint> current = Points.FirstItem;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        public virtual TFigure Clone()
        {
            var figure = new TFigure();
            figure.isClosed = isClosed;
            foreach (var point in this)
            {
                figure.Points.Add(new TPoint(point.X, point.Y));
            }
            return figure;
        }
    }
}
