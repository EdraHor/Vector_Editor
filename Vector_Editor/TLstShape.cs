using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector_Editor
{                      //без второго списка. Дать точкам bool isInShape и индексы;
                       //мы перебираем точки с isInShape(0), если попадается isInShape(1) начинаем рисовать
                       //фигуру

    //Создать список с индексами на фигуры другого списка. Рисуем все точки, соединяем линиями по списку индексов.
    //Если нужно выбрать фигуру из ее центра проходимся списку фигур и меняем все ее координаты
    //

    //Сделать 2 независимых списка. Один содержит точки, другой фигуры со списками точек. Для фигур сделать:
    // * отдельные режимы удаления и перемещения? Слишком сильно засоряется код. Проще реализовать.
    // * проходится по обоим спискам подряд? Проще рисовать. Нагромаждаются инструменты. 


    public class TLstShape<T> //содержит фигуры 
    {
        public Node<TShape> FirstItem { get; private set; } //Первый элемент
        public Node<TShape> LastItem { get; private set; }//Последний элемент
        public int Count { get; private set; } //Количество элементов списка
        public bool drawLines = false;
        public bool smartPoint = false;
        public bool transformAndRotate = false;
        public int ShapeSides = 3;

        public void Add(TShape data) //Добавить в конец списка
        {
            Node<TShape> node = new Node<TShape>(data);

            if (LastItem == null)
                FirstItem = node;
            else
                LastItem.Next = node;
            LastItem = node;

            Count++;
        }
        public void AddToFirst(TShape data) //Добавить в начало списка
        {
            Node<TShape> node = new Node<TShape>(data);
            node.Next = FirstItem;
            FirstItem = node;
            if (Count == 0)
                LastItem = FirstItem;
            Count++;
        }

        public void InstanceItem(int position, TShape data)//Вставляет точку по позиции
        {
            if (position <= 0 || position > Count) new Exception("Позиция за границами списка");

            Node<TShape> current = new Node<TShape>(data);
            Node<TShape> previous = GetNode(position - 1);

            current.Next = GetNode(position);
            if (position > 0)
                previous.Next = current;
            else
                FirstItem = current;

            Count++;
        }

        public bool EqualPoints(TPoint data, int R)
        {
            Node<TShape> current = FirstItem;
            while (current != null) //Перебираем все фигуры
            {
                var Shape = current.Data.Shape; //Запоминаем фигуру
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
            Node<TShape> current = FirstItem;
            int Position = 0;
            while (current != null)
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

        public TShape GetItem(int position) //возвращает объект точки
        {
            if (position <= Count)
            {
                Node<TShape> current = FirstItem;
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
        public Node<TShape> GetNode(int position) //возвращает ячейку списка
        {
            if (position <= Count)
            {
                Node<TShape> current = FirstItem;
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
        //public string GetStringPoint(int position)
        //{
        //    return "(" + position.ToString() + ") x: " + GetItem(position).x.ToString()
        //    + "; y: " + GetItem(position).y.ToString();
        //}

        public bool Remove(int position) //Удаляет точку по позиции в списке
        {
            Node<TShape> current = FirstItem;
            Node<TShape> previous = null;

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
