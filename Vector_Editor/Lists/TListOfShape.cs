using System;
using System.Collections;
using System.Collections.Generic;

namespace Vector_Editor
{                   
    public class TListOfShape : TList<TFigure>, IEnumerable<TFigure> //содержит фигуры 
    {
        public int ShapeSides = 4;

        public bool EqualPoints(TPoint data, int R)
        {
            TNode<TFigure> current = FirstItem;
            while (current != null) //Перебираем все фигуры
            {
                var Shape = current.Data.Points; //Запоминаем фигуру
                for (int i = 0; i < Shape.Count; i++) //Проходим по всем точкам в фигуре
                {
                    if (Math.Abs(data.X - Shape.GetItem(i).X) < R && Math.Abs(data.Y - Shape.GetItem(i).Y) < R)
                        return true;
                }
                current = current.Next;
            }

            return false; //Проверка на близость точек
        }

        public int GetItemPosition(TFigure data)
        {
            TNode<TFigure> current = FirstItem;
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

        public void MovingSelected(TPoint MousePos) //Перемещение всех фигур в указанную точку
        {
            TPoint Diff, Move;

            for (int i = 0; i < Count; i++)
            {
                var Shape = GetItem(i).Points;
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

        // реализация интерфейса IEnumerable для быстрого перебора всех элементов списка
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<TFigure> IEnumerable<TFigure>.GetEnumerator()
        {
            TNode<TFigure> current = FirstItem;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}
