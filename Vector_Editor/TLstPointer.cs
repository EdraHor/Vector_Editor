using System;
using System.Numerics;

namespace Vector_Editor
{
    public class TLstPointer<T>
    {
        public TNode<TPoint> FirstItem { get; private set; } //Первый элемент
        public TNode<TPoint> LastItem { get; private set; }//Последний элемент
        public int Count { get; private set; } //Количество элементов списка
        public bool isDrawLines = false;
        public bool isMiddlePoint = false;
        public bool isTransformAndRotate = false;
        public bool isInFirst = false;

        public void Add(TPoint data) //Добавить в конец списка
        {
            TNode<TPoint> node = new TNode<TPoint>(data);

            if (LastItem == null)
                FirstItem = node;
            else
                LastItem.Next = node;
            LastItem = node;

            Count++;
        }
        public void AddToFirst(TPoint data) //Добавить в начало списка
        {
            TNode<TPoint> node = new TNode<TPoint>(data);
            node.Next = FirstItem;
            FirstItem = node;
            if (Count == 0)
                LastItem = FirstItem;
            Count++;
        }


        public object this[int i] //Позвляет использовать квадатные скобки как с 
        {                         //обычным массивом или списком
            get { return GetItem(i); }
            set { SetItem(i, (TPoint)value); }
        }

        public void InstanceItem(int position, TPoint data)//Вставляет точку по позиции
        {
            position++;
            if (position <= 0 || position > Count) return;

            TNode<TPoint> current = new TNode<TPoint>(data);
            TNode<TPoint> previous = GetNode(position - 1);

            current.Next = GetNode(position);
            if (position > 0)
                previous.Next = current;
            else
                FirstItem = current;

            Count++;
        }

        public TPoint PreviousItem(TPoint point) //возвращает объект точки
        {
            TNode<TPoint> current = FirstItem;
            bool isFind = false;
            while (current!=LastItem && !isFind)
            {
                if (current.Next.Data == point)
                {
                    isFind = true;
                    return current.Data;
                }
                current = current.Next;
            }
            return new TPoint(4040, 4040);
        } 
        public TNode<TPoint> PreviousNode(int position)//возвращает ячейку списка
        {
            if (position <= Count)
            {
                TNode<TPoint> current = FirstItem;
                for (int i = 1; i < position - 1; i++) //Перебираем до position-1 
                {
                    current = current.Next;
                }
                return current;
            }
            else
            {
                new Exception("Элемент за границами списка");
                return null;
            }
        }

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
        public bool EqualPoints(System.Windows.Forms.MouseEventArgs e, int R) //Проверка по позиции мыши
                                                                              //точек на близость 
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

        public int GetPointPosition(TPoint data)
        {
            TNode<TPoint> current = FirstItem;
            int Position = 0;
            while (current!=null)
            {
                if (current.Data == data) return Position;
                Position++;
                current = current.Next;
            }
            new Exception("Элемент не найден!");
            return 0;
        } //Возвращает номер точки в списке
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

        public void Clear() //Очистка списка
        {
            FirstItem = null; //Очистив первый и последние элементы списка мы разрушаем связи
            LastItem = null; //между элементами в результате чего сборщик мусора сам удаляет
            Count = 0;       //остальные элементы списка
        }

        public TPoint GetItem(int position) //возвращает объект точки
        {
            if (position <= Count)
            {
                TNode<TPoint> current = FirstItem;
                for (int i = 0; i < position; i++)
                {
                    current = current.Next;
                }
                return current.Data;
            }
            else
            {
                new Exception("Элемент за границами списка!");
                return FirstItem.Data;
            }
        } 
        public TNode<TPoint> GetNode(int position) //возвращает ячейку списка
        {
            if (position <= Count)
            {
                TNode<TPoint> current = FirstItem;
                for (int i = 1; i < position; i++)
                {
                    current = current.Next;
                }
                return current;
            }
            else
            {
                new Exception("Элемент за границами списка");
                return null;
            }
        }
        public string GetStringPoint(int position)
        {
            return "(" + position.ToString() + ") x: " + GetItem(position).X.ToString()
            + "; y: " + GetItem(position).Y.ToString();
        }
        
        public void SetItem(int position, TPoint data) //Устанавливает точку в конкретную позицию
        {
            if (position <= Count)
            {
                TNode<TPoint> current = FirstItem;
                for (int i = 0; i < position; i++)
                {
                    current = current.Next;
                }
                current.Data = data;
            }
            else new Exception("Элемент за границами списка");
        }

        public bool Remove(int position) //Удаляет точку по позиции в списке
        {
            TNode<TPoint> current = FirstItem;
            TNode<TPoint> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(GetItem(position)))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null) //Если удаляемый элемент последний в списке
                            LastItem = previous;
                    }
                    else //Если удаляемый элемент первый в списке
                    {
                        FirstItem = FirstItem.Next;

                        if (FirstItem == null)
                            LastItem = null;
                    }
                    Count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            return false;
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
