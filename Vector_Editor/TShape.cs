using System.Drawing;

namespace Vector_Editor
{
    public class TShape
    {
        public TLstPointer<TPoint> Item; //Список точек фигуры
        public int Count { get => Item.Count; } //Количество точек фигуры
        public bool isSelect; //Выделена ли фигура

        public TShape() //Конструктор инициализирует список точек фигуры.
        {
            Item = new TLstPointer<TPoint>();
        }
        public TShape(TLstPointer<TPoint> ShapePoints) //Конструктор создает фигуру из списка 
        {
            Item = new TLstPointer<TPoint>();
            for (int i = 0; i < ShapePoints.Count; i++)
            {
                Item.Add(ShapePoints.GetItem(i));
            }
        }

        public void AddPoint(TPoint _point) //Добавляет фигуру в конец
        {
            Item.Add(_point);
        }
        public void SetPoint(int position, TPoint _point) //Устанавливает значение
        {
            Item.SetItem(position, _point);
        }
        public void RemovePoint(int position) //удаляет фигуру
        {
            Item.Remove(position);
        }
        public Point[] GetArray() //Преобразуем список TPoint в массив Point и возвращает его
        {
            Point[] Array = new Point[Item.Count];

            for (int i = 0; i < Item.Count; i++)
            {
                Array[i] = Item.GetItem(i)._point;
            }
            return Array;
        }

        public TPoint GetCenterPoint()
        {
            var Count = Item.Count;
            double SumX = 0;
            double SumY = 0;
            for (int i = 0; i < Count; i++)
            {
                SumX += Item.GetItem(i).X;
                SumY += Item.GetItem(i).Y;
            }
            return new TPoint(SumX / Count, SumY / Count);
        }

        public void Moving(TPoint MovePos) //Перемещает фигуру в указанную точку
        {
            TPoint Diff,Move,Center;
            Center = GetCenterPoint();
            for (int i = 0; i < Count; i++)
            {
                //Разница между каждой точкой и центром фигуры
                Diff = new TPoint(Center.X - Item.GetItem(i).X, Center.Y - Item.GetItem(i).Y);
                //Возвращаем эту разницу ориентируясь на новое место
                Move = new TPoint(MovePos.X - Diff.X, MovePos.Y - Diff.Y);
                Item.SetItem(i, Move);
            }
        }

        public void Select() //выделяет фигуру и все ее точки
        {
            isSelect = true;
            for (int i = 0; i < Count; i++)
            {
                Item.GetItem(i).Select();
            }
        }
        public void Deselect() //снимает выделение с фигуруры и всех ее точек
        {
            isSelect = false;
            for (int i = 0; i < Count; i++)
            {
                Item.GetItem(i).Deselect();
            }
        }
    }
}
