using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorPoints : IToolBehavior
    {
        //TListOfShape shapeList; // Список фигур
        //TListOfPoints pointList;
        private TFigure _figure; // Фигура
        private TPolygon _poly = new TPolygon();
        private TPoint _tempMiddlePoint; // временное хранение ближайшей точки на ребре
        private int _tempMiddleIndex; // временный индекс той вершины, после которой можно вставить промежуточную ближайшую точку
        bool isShapeStart = false;
        TShapeList _mainList;
        Graphics _graphics;

        public void Enter(TShapeList mainList, Graphics graphics)
        {
            Console.WriteLine("Enter Points behavior");
            //shapeList = ShapeList;
            _mainList = mainList;
            _graphics = graphics;
            //_list = list;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Points behavior");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {

            if (Options.isInFirst && e.Button == MouseButtons.Left)
            {
                if (!_mainList.EqualPoints(mousePos, 10))
                    _poly.Points.AddToFirst(mousePos);
            }
            else if (e.Button == MouseButtons.Left) //При левом клике
            {
                if (!Options.isMiddlePoint) //создание свободной фигуры
                {
                    if (!isShapeStart)
                    {
                        _mainList.AddPolygon(_poly);
                        isShapeStart = true;
                    }
                    if (!_mainList.EqualPoints(mousePos,10))
                    {
                        _poly.AddPoint(mousePos);
                    }
                    else if (_mainList.EqualPoints(mousePos, 10) && _poly.Points.IsFirst(mousePos, 10)
                            && _poly.Points.Count > 1) //замыкаем линию и создаем фигуру
                    {
                        _poly.isClosed = true;
                        _poly = new TPolygon();
                        isShapeStart = false;
                    }
                }
                else //режим вставки точек между двумя
                {
                    foreach (var shapes in _mainList._list.Values)
                    {
                        if (shapes.Points.Count > 1)
                        {
                            if (!shapes.Points.EqualPoints(_tempMiddlePoint, 10)) //Проверка наличия точек около новой
                            {
                                shapes.Points.InstanceItem(_tempMiddleIndex, _tempMiddlePoint); //Вставляем точку между двумя
                            }
                        }
                    }
                }
            }
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            if (Options.isMiddlePoint)//Если включен режим рисования точек между точками
            {
                foreach (var shapes in _mainList._list.Values)
                {
                    if (shapes.Points.Count > 1)
                    {
                        Vector2 mouseVec = new Vector2(mousePos.X, mousePos.Y);

                        // перебираем все рёбра и ищем ближайшую к курсору точку на ребре
                        float minDist = float.MaxValue;
                        var i = 0;
                        foreach (var points in shapes.Points)
                        {
                            Vector2 v2 = new Vector2(points.X, points.Y);
                            Vector2 v1 = new Vector2(shapes.GetItem(i + 1).X, shapes.GetItem(i + 1).Y);

                            Vector2 closest = shapes.Points.FindNearestPointOnLine(v1, v2, mouseVec);

                            float dist = Vector2.DistanceSquared(mouseVec, closest);
                            if (dist < minDist)
                            {
                                minDist = dist;
                                _tempMiddlePoint = new TPoint(closest);
                                _tempMiddleIndex = i + 1;
                            }
                            i++;
                        }
                    }
                }
            }
            else if (!Options.isInFirst) //При левом клике
            {
                //создание свободной фигуры
                if (_poly.Count!=0)
                    _graphics.DrawLine(new Pen(Color.Black), _poly.LastPoint.ToPoint(), mousePos.ToPoint());
            }
        }

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {

        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {


            foreach (var shapes in _mainList._list.Values)
            {
                var pointList = shapes.Points;
                if (pointList.Count > 1 && Options.isMiddlePoint)
                {
                    e.Graphics.DrawEllipse(new Pen(Color.Red), _tempMiddlePoint.X - 5, _tempMiddlePoint.Y - 5, 10, 10);
                }
            }


        }
    }
}
