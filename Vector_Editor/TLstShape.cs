using System;

namespace Vector_Editor
{                   
    public class TLstShape<T> //содержит фигуры 
    {
        public TNode<TShape> FirstItem { get; private set; } //Первый элемент
        public TNode<TShape> LastItem { get; private set; }//Последний элемент
        public int Count { get; private set; } //Количество элементов списка
        public int ShapeSides = 4;

        public void Add(TShape data) //Добавить в конец списка
        {
            TNode<TShape> node = new TNode<TShape>(data);

            if (LastItem == null)
                FirstItem = node;
            else
                LastItem.Next = node;
            LastItem = node;

            Count++;
        }
        public void AddToFirst(TShape data) //Добавить в начало списка
        {
            TNode<TShape> node = new TNode<TShape>(data);
            node.Next = FirstItem;
            FirstItem = node;
            if (Count == 0)
                LastItem = FirstItem;
            Count++;
        }

        public void InstanceItem(int position, TShape data)//Вставляет фигуру по позиции
        {
            if (position <= 0 || position > Count) new Exception("Позиция за границами списка");

            TNode<TShape> current = new TNode<TShape>(data);
            TNode<TShape> previous = GetNode(position - 1);

            current.Next = GetNode(position);
            if (position > 0)
                previous.Next = current;
            else
                FirstItem = current;

            Count++;
        }

        public bool EqualPoints(TPoint data, int R)
        {
            TNode<TShape> current = FirstItem;
            while (current != null) //Перебираем все фигуры
            {
                var Shape = current.Data.Item; //Запоминаем фигуру
                for (int i = 0; i < Shape.Count; i++) //Проходим по всем точкам в фигуре
                {
                    if (Math.Abs(data.X - Shape.GetItem(i).X) < R && Math.Abs(data.Y - Shape.GetItem(i).Y) < R)
                        return true;
                }
                current = current.Next;
            }

            return false; //Проверка на близость точек
        }

        public int GetItemPosition(TShape data)
        {
            TNode<TShape> current = FirstItem;
            int Position = 0;
            while (current != null)
            {
                if (current.Data == data) return Position;
                Position++;
                current = current.Next;
            }
            new Exception("Элемент не найден!");
            return 0;
        } //Возвращает номер фигуры в списке

        public void Clear() //Очистка списка
        {
            FirstItem = null; //Очистив первый и последние элементы списка мы разрушаем связи
            LastItem = null; //между элементами в результате чего сборщик мусора сам удаляет
            Count = 0;       //остальные элементы списка
        }

        public TShape GetItem(int position) //возвращает объект фигуры
        {
            if (position <= Count)
            {
                TNode<TShape> current = FirstItem;
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
        public TNode<TShape> GetNode(int position) //возвращает ячейку списка
        {
            if (position <= Count)
            {
                TNode<TShape> current = FirstItem;
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

        public void MovingSelected(TPoint MousePos) //Перемещение всех фигур в указанную точку
        {
            TPoint Diff, Move;

            for (int i = 0; i < Count; i++)
            {
                var Shape = GetItem(i).Item;
                if (GetItem(i).isSelect)
                {
                    //Разница между позицией мыши и каждым центром фигуры
                    Diff = new TPoint(MousePos.X - Shape.GetCenterPoint().X, 
                    MousePos.Y - Shape.GetCenterPoint().Y);
                    //Возвращаем эту разницу ориентируясь на новое место
                    Move = new TPoint(MousePos.X + Diff.X, MousePos.Y + Diff.Y);
                    GetItem(i).Moving(Move);
                }
            }
        }

        //Console.WriteLine(Move.GetString());

        public bool Remove(int position) //Удаляет фигуру по позиции в списке
        {
            TNode<TShape> current = FirstItem;
            TNode<TShape> previous = null;

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
    }
}
