using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Vector_Editor.Lists;
using System.Collections;
using System;

namespace Vector_Editor
{
    public class TShapeList : IEnumerable<TFigure>
    {
        //перебор сразу всего, ма сначала перебираем этот гланый словарь, внутри этого перебора
        //перебрать внутренний список

        //  * Убрать список точек
        //  * Создать общий класс фигуры и общий список фигур, но внутри каждой фигуры оставлять ключи для
        //    для быстрого доступа и перебора значений в списке (словаре)
        //  * Перенести отрисовку фигур внутри самих фигур


        //Нужно создать общий список и присоеденить его к спискам фигур и точек

        public Dictionary<Guid, TFigure> _list; //Словарь хранящий списки всех фигур
        public TShapeList()
        {
            _list = new Dictionary<Guid, TFigure>(); //Инициализация словаря
            polygonKeys = new TList<Guid>();
            rectangleKeys = new TList<Guid>();
            selectedKeys = new TList<Guid>();
            copiedFigures = new TList<TFigure>();
        }
        public TList<Guid> polygonKeys;
        public TList<Guid> rectangleKeys;
        public TList<Guid> selectedKeys;
        public TList<TFigure> copiedFigures;



        public void AddToSelect(Guid id)
        {
            selectedKeys.Add(id);
        }
        public void Deselect()
        {
            selectedKeys.Clear();
            foreach (var shape in this)
            {
                shape.Deselect();
            }
        }
        public void CopySelected()
        {
            copiedFigures.Clear();
            foreach (var key in selectedKeys)
            {
                copiedFigures.Add(_list[key]);
            }
        }
        public int PasteOffset = 20; //смещение фигуры при вставке
        public void Paste()
        {
            foreach (var shape in copiedFigures)
            {
                //вставка
                var CopiedShape = shape.Clone();
                SpecAdd(CopiedShape);

                //смещение
                CopiedShape.Moving(new TPoint(shape.GetCenterPoint().X + PasteOffset,
                    shape.GetCenterPoint().Y + PasteOffset));
            }
        }


        public void Remove(Guid id)
        {
            if (_list.ContainsKey(id)) //если такой ключ существует
            {
                _list.Remove(id);
            }
        }

        public void Clear()
        {
            _list.Clear();
            polygonKeys.Clear();
            rectangleKeys.Clear();
        }
        public void Add(TFigure figure)
        {
            if (figure is TPolygon) //Проверка на то дейтвительно ли это полигон
            {
                _list.Add(figure.ID, figure);
                polygonKeys.Add(figure.ID);
            }
            else if (figure is TRectangle) //Проверка на то дейтвительно ли это прямоугольник
            {
                _list.Add(figure.ID, figure);
                rectangleKeys.Add(figure.ID);
            }
        }
        public void SpecAdd(TFigure figure)
        {
            _list.Add(figure.ID, figure);
            polygonKeys.Add(figure.ID);
        }
        public void AddPolygon(TFigure figure) //Инициализируем словарь со всеми инструментами
        {
            if (figure is TPolygon) //Проверка на то дейтвительно ли это полигон
            {
                _list.Add(figure.ID, figure);
                polygonKeys.Add(figure.ID);
            }
        }
        public void AddRectangle(TFigure figure) //Инициализируем словарь со всеми инструментами
        {
            if (figure is TRectangle) //Проверка на то дейтвительно ли это полигон
            {
                _list.Add(figure.ID, figure);
                rectangleKeys.Add(figure.ID);
            }
        }

        public bool EqualPoints(TPoint data, int R) //Проверка конкретной точки на близость в 
        {                                           //радиусе R с другими точками в списке
            var equal = false;
            foreach (var item in _list.Values)
            {
                if (item.Points.EqualPoints(data, R)) equal = true;
            }
            return equal;
        }

        TPoint oldMousePos;
        public bool isMouseUp = false;
        public void MovingSelected(TPoint MousePos) //Перемещение всех фигур за позицией мыши
        {
            TPoint Diff;
            if (!isMouseUp) //Сохраняем позицию мыши в начале клика
            {
                oldMousePos = MousePos;
                isMouseUp = true;
            }
            foreach (var shape in this)
            {
                if (shape.isSelect)
                    foreach (var point in shape)
                    {
                        //Высчитываем разницу между предыдущей и текущей позицией мыши
                        Diff = new TPoint(oldMousePos.X - MousePos.X, oldMousePos.Y - MousePos.Y);
                        //Перемещаем все точки на разницу позиций мыши
                        point.SetPoint(point.X - Diff.X, point.Y - Diff.Y);
                    }
            }
            oldMousePos = MousePos; //сохраняем старое положение мыши

            //foreach (var key in selectedKeys)
            //{
            //    foreach (var point in _list[key])
            //    {
            //        //Высчитываем разницу между предыдущей и текущей позицией мыши
            //        Diff = new TPoint(oldMousePos.X - MousePos.X, oldMousePos.Y - MousePos.Y);
            //        //Перемещаем все точки на разницу позиций мыши
            //        point.SetPoint(point.X - Diff.X, point.Y - Diff.Y);
            //    }
            //}
        }
        public TPoint GetCenter()
        {
            var Count = _list.Count;
            double SumX = 0;
            double SumY = 0;
            foreach (var shape in this)
            {
                SumX += shape.GetCenterPoint().X;
                SumY += shape.GetCenterPoint().Y;
            }
            return new TPoint(SumX / Count, SumY / Count);
        }

        public TPoint GetCenterSelected()
        {
            var Count = 0;
            double SumX = 0;
            double SumY = 0;
            foreach (var shape in this)
            {
                if (shape.isSelect)
                {
                    SumX += shape.GetCenterPoint().X;
                    SumY += shape.GetCenterPoint().Y;
                    Count++;
                }
            }
            return new TPoint(SumX / Count, SumY / Count);
        }

        public void RotateAt(TPoint point, double angle) //поворот всех фигур относительно центра
        {
            foreach (var key in selectedKeys)
            {                
                _list[key].Points.RotateAt(point, angle);
            }
        }

        public void TransformAt(int xt, int yt) // Перемещение всех точек на указаннное растояние
        {
            foreach (var shape in this)
            {
                shape.Points.TransformAt(xt, yt);
            }
        }



        //реализация интерфейса IEnumerable для быстрого перебора всех элементов списка
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<TFigure> IEnumerable<TFigure>.GetEnumerator()
        {
            foreach (var figure in _list.Values)
            {
                yield return figure;
            }
        }

    }
}
