using System;
using System.Collections;
using System.Collections.Generic;

namespace Vector_Editor
{
    public class TList<T> : IEnumerable<T>
    {
        public TNode<T> FirstItem { get; protected set; } //Первый элемент
        public TNode<T> LastItem { get; protected set; }//Последний элемент
        public int Count { get; protected set; } //Количество элементов списка


        public void Add(T data) //Добавить в конец списка
        {
            TNode<T> node = new TNode<T>(data);

            if (LastItem == null)
                FirstItem = node;
            else
                LastItem.Next = node;
            LastItem = node;

            Count++;
        }
        public void AddToFirst(T data) //Добавить в начало списка
        {
            TNode<T> node = new TNode<T>(data);
            node.Next = FirstItem;
            FirstItem = node;
            if (Count == 0)
                LastItem = FirstItem;
            Count++;
        }


        public T this[int i] //Позвляет использовать квадатные скобки как с 
        {                         //обычным массивом или списком
            get { return GetItem(i); }
            set { SetItem(i, (T)value); }
        }

        public void InstanceItem(int position, T data)//Вставляет точку по позиции
        {
            position++;
            if (position <= 0 || position > Count) return;

            TNode<T> current = new TNode<T>(data);
            TNode<T> previous = GetNode(position - 1);

            current.Next = GetNode(position);
            if (position > 0)
                previous.Next = current;
            else
                FirstItem = current;

            Count++;
        }

        public T PreviousItem(T data) //возвращает объект точки
        {
            TNode<T> current = FirstItem;
            bool isFind = false;
            while (current != LastItem && !isFind)
            {
                if (current.Next.Data.Equals(data))
                {
                    isFind = true;
                    return current.Data;
                }
                current = current.Next;
            }
            return LastItem.Data;
        }
        public TNode<T> PreviousNode(int position)//возвращает ячейку списка
        {
            if (position <= Count)
            {
                TNode<T> current = FirstItem;
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
        public int GetPointPosition(T data)
        {
            TNode<T> current = FirstItem;
            int Position = 0;
            while (current != null)
            {
                if (current.Data.Equals(data)) return Position;
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

        public T GetItem(int position) //возвращает объект точки
        {
            if (position <= Count)
            {
                TNode<T> current = FirstItem;
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
        public TNode<T> GetNode(int position) //возвращает ячейку списка
        {
            if (position <= Count)
            {
                TNode<T> current = FirstItem;
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

        public void SetItem(int position, T data) //Устанавливает точку в конкретную позицию
        {
            if (position <= Count)
            {
                TNode<T> current = FirstItem;
                for (int i = 0; i < position; i++)
                {
                    current = current.Next;
                }
                current.Data = data;
            }
            else new Exception("Элемент за границами списка");
        }

        public T[] ToArray()
        {
            T[] Array = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                Array[i] = GetItem(i);
                i++;
            }
            return Array;
        }

        public bool Remove(int position) //Удаляет точку по позиции в списке
        {
            TNode<T> current = FirstItem;
            TNode<T> previous = null;

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

        // реализация интерфейса IEnumerable для быстрого перебора всех элементов списка
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            TNode<T> current = FirstItem;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}
