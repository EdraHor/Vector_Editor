using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector_Editor
{
    public class TLstPointer<T>
    {
        public Node<TPoint> FirstItem { get; private set; } //Первый элемент
        public Node<TPoint> LastItem { get; private set; }//Последний элемент
        public int Count { get; private set; } //Количество элементов списка
        public bool drawLines = false;
        public bool smartPoint = false;
        public bool transformAndRotate = false;
        public int ShapeSides = 3;

        public void Add(TPoint data) //Добавить в конец списка
        {
            Node<TPoint> node = new Node<TPoint>(data);

            if (LastItem == null)
                FirstItem = node;
            else
                LastItem.Next = node;
            LastItem = node;

            Count++;
        }
        public void AddToFirst(TPoint data) //Добавить в начало списка
        {
            Node<TPoint> node = new Node<TPoint>(data);
            node.Next = FirstItem;
            FirstItem = node;
            if (Count == 0)
                LastItem = FirstItem;
            Count++;
        }

        public void RemoveItem(TPoint data) //Удаляет первую попавшеюся точку
        {
            Node<TPoint> current = FirstItem;
            Node<TPoint> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;

                        if (current.Next == null)
                            LastItem = previous;
                    }
                    else
                    {
                        FirstItem = FirstItem.Next;
                        if (FirstItem == null)
                            LastItem = null;
                    }
                    Count--;
                }
                previous = current;
                current = current.Next;
            }
        }
        public void RemoveItem(int position) //Удаляет точку по позиции
        {
            Node<TPoint> current = FirstItem;
            Node<TPoint> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(GetItem(position)))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;

                        if (current.Next == null)
                            LastItem = previous;
                    }
                    else
                    {
                        FirstItem = FirstItem.Next;
                        if (FirstItem == null)
                            LastItem = null;
                    }
                    Count--;
                }
                previous = current;
                current = current.Next;
            }
        }

        public object this[int i] //Позвляет использовать квадатные скобки как с 
        {                         //обычным массивом или списком
            get { return GetItem(i); }
            set { SetItem(i, (TPoint)value); }
        }

        public void InstanceItem(int position, TPoint data)//Вставляет точку по позиции
        {
            if (position <= 0 || position > Count) new Exception("Позиция за границами списка");

            Node<TPoint> current = new Node<TPoint>(data);
            Node<TPoint> previous = GetNode(position - 1);

            current.Next = GetNode(position);
            if (position > 0)
                previous.Next = current;
            else
                FirstItem = current;

            Count++;
        }

        public TPoint PreviousItem(TPoint point) //возвращает объект точки
        {
            Node<TPoint> current = FirstItem;
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
        public Node<TPoint> PreviousNode(int position)//возвращает ячейку списка
        {
            if (position <= Count)
            {
                Node<TPoint> current = FirstItem;
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
            Node<TPoint> current = FirstItem;
            while (current != null)
            {
                if (Math.Abs(data.x - current.Data.x) < R && Math.Abs(data.y - current.Data.y) < R) 
                    return true;
                current = current.Next;
            }
            return false; //Проверка на близость точек
        }
        public bool EqualPoints(System.Windows.Forms.MouseEventArgs e, int R) //Проверка по позиции мыши
                                                                              //точек на близость 
        {                          
            Node<TPoint> current = FirstItem;
            while (current != null)
            {
                if (Math.Abs(e.X - current.Data.x) < 10 &&
                    Math.Abs(e.Y - current.Data.y) < 10)
                    return true;
                current = current.Next;
            }
            return false; //Проверка на близость точек
        }

        public int GetItemPosition(TPoint data)
        {
            Node<TPoint> current = FirstItem;
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
                Node<TPoint> current = FirstItem;
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
        public Node<TPoint> GetNode(int position) //возвращает ячейку списка
        {
            if (position <= Count)
            {
                Node<TPoint> current = FirstItem;
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
            return "(" + position.ToString() + ") x: " + GetItem(position).x.ToString()
            + "; y: " + GetItem(position).y.ToString();
        }
        
        public void SetItem(int position, TPoint data) //Устанавливает точку в конкретную позицию
        {
            if (position <= Count)
            {
                Node<TPoint> current = FirstItem;
                for (int i = 1; i < position; i++)
                {
                    current = current.Next;
                }
                current.Data = data;
            }
            else new Exception("Элемент за границами списка");
        }

        public bool Remove(int position) //Удаляет точку по позиции в списке
        {
            Node<TPoint> current = FirstItem;
            Node<TPoint> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(GetItem(position)))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            LastItem = previous;
                    }
                    else
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
        public int GetDistanceToPoint(TPoint point,int pos)
        {
            return (int)Math.Sqrt(Math.Pow(point.x - GetItem(pos).x, 2) + Math.Pow(point.y - GetItem(pos).y, 2));
        }

        public void RotateAt(int xO, int yO, float angle)
        {
            for (int i = 0; i < Count; i++)
            {
                float angleRad = (float)(angle * Math.PI / 180);
                int x = GetItem(i).x;
                int y = GetItem(i).y;
                
                GetItem(i).x = xO+(int)Math.Round((x - xO) * Math.Cos(angleRad) - (y - yO) * Math.Sin(angleRad));
                GetItem(i).y = yO+(int)Math.Round((x - xO) * Math.Sin(angleRad) + (y - yO) * Math.Cos(angleRad));
            }
        }
        public void RotateAt(TPoint point, double angle)
        {
            float angleRad = (float)(angle * Math.PI / 180);
            for (int i = 0; i < Count; i++)
            {
                int x = GetItem(i).x; int y = GetItem(i).y;
                int xO = point.x; int yO = point.y;

                GetItem(i).x = xO+(int)Math.Round((x - xO) * Math.Cos(angleRad) - (y - yO) * Math.Sin(angleRad));
                GetItem(i).y = yO+(int)Math.Round((x - xO) * Math.Sin(angleRad) + (y - yO) * Math.Cos(angleRad));
            }
        }
        public void TransformAt(int xt, int yt)
        {
            for (int i = 0; i < Count; i++)
            {
                GetItem(i).x += xt;
                GetItem(i).y += yt;
            }
        }

        public TPoint GetCenterPoint()
        {
            double SumX = 0;
            double SumY = 0;
            for (int i = 0; i < Count; i++)
            {
                SumX += GetItem(i).x;
                SumY += GetItem(i).y;
            }
            return new TPoint(SumX / Count, SumY / Count);
        }
    }

}
