using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Vector_Editor.Lists;
using System.Collections;
using System.Collections.Generic;

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

        public Dictionary<short, TFigure> _list; //Словарь хранящий списки всех фигур
        public TShapeList()
        {
            _list = new Dictionary<short, TFigure>(); //Инициализация словаря
        }
        //TListOfPoints pointsList; //список точек
        //TListOfShape shapesList; //список фигур
        //TListOfRectangles rectanglesList; //список прямоугольников


        //private readonly short POINTS =     1;
        //private readonly short SHAPES =     2;
        //private readonly short RECTANGLES = 3;
        //short trigngle = 4;
        //short bezier = 5;

        short curKey = 0;

        public void AddPolygon(TFigure figure) //Инициализируем словарь со всеми инструментами
        {
            if (figure is TPolygon) //Проверка на то дейтвительно ли это полигон
                _list.Add(curKey, figure);
            curKey++;
        }
        public void AddRectangle(TFigure figure) //Инициализируем словарь со всеми инструментами
        {
            if (figure is TRectangle) //Проверка на то дейтвительно ли это полигон
                _list.Add(curKey, figure);
            curKey++;
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

        //public TListOfRectangles GetRectangles()
        //{
        //    var shape = (TListOfRectangles)_list[RECTANGLES];
        //    return shape;
        //}
        //public TListOfShape GetShapes()
        //{
        //    var shape = (TListOfShape)_list[SHAPES];
        //    return shape;
        //}
        //public TListOfPoints GetPoints()
        //{
        //    var shape = (TListOfPoints)_list[POINTS];
        //    return shape;
        //}
        //public void AddToRectangles(TFigure newShape)
        //{
        //    GetRectangles().Add(newShape);
        //}







        //Осуществлять здесь перебор всех тивов данных через Foreach
    }
}
