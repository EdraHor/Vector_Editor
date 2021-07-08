using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorPoints : IToolBehavior
    {
        TLstShape<TShape> shapeList; // Список фигур
        TLstPointer<TPoint> _list;
        private TShape shape; // Фигура
        private TPoint _tempMiddlePoint; // временное хранение ближайшей точки на ребре
        private int _tempMiddleIndex; // временный индекс той вершины, после которой можно вставить промежуточную ближайшую точку


        public void Enter(TLstShape<TShape> ShapeList, TLstPointer<TPoint> list)
        {
            Console.WriteLine("Enter Points behavior");
            shapeList = ShapeList;
            _list = list;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Points behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e)
        {
            TPoint mousePos = new TPoint(e.X, e.Y); //Запоминаем позицию мыши

            if (_list.isInFirst && e.Button == MouseButtons.Left)
            {
                if (!_list.EqualPoints(mousePos, 10) && !shapeList.EqualPoints(mousePos, 10))
                    _list.AddToFirst(mousePos);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (!_list.isMiddlePoint)
                {
                    if (!_list.EqualPoints(mousePos, 10) && !shapeList.EqualPoints(mousePos, 10))
                    _list.Add(mousePos);
                    else if (_list.EqualPoints(mousePos, 10) && _list.IsFirst(mousePos, 10)
                            && _list.Count > 1) //замыкаем линию и создаем фигуру
                    {
                        shape = new TShape(_list);
                        shapeList.Add(shape);
                        _list.Clear();
                    }
                }
                else
                {
                    if (_list.Count > 1)
                    {
                        if (!_list.EqualPoints(_tempMiddlePoint, 10)) //Проверка наличия точек около новой
                        {
                            _list.InstanceItem(_tempMiddleIndex, _tempMiddlePoint); //Вставляем точку между двумя
                        }                                                           
                    }
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e)
        {
            if (_list.isMiddlePoint)//Если включен режим рисования точек между точками
            {
                TPoint mousePos = new TPoint(e.X, e.Y); //Запоминаем позицию мыши
                if (_list.Count > 1)
                {
                    Vector2 mouseVec = new Vector2(mousePos.X, mousePos.Y);

                    // перебираем все рёбра и ищем ближайшую к курсору точку на ребре
                    float minDist = float.MaxValue;

                    for (int i = 0; i < _list.Count - 1; i++)
                    {
                        Vector2 v2 = new Vector2(_list.GetItem(i).X, _list.GetItem(i).Y);
                        Vector2 v1 = new Vector2(_list.GetItem(i + 1).X, _list.GetItem(i + 1).Y);

                        Vector2 closest = _list.FindNearestPointOnLine(v1, v2, mouseVec);

                        float dist = Vector2.DistanceSquared(mouseVec, closest);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            _tempMiddlePoint = new TPoint(closest);
                            _tempMiddleIndex = i + 1;
                        }
                    }
                }
            }
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e)
        {

        }

        public void Paint(PaintEventArgs e)
        {
            if (_list.Count > 1 && _list.isMiddlePoint)
            {
                e.Graphics.DrawEllipse(new Pen(Color.Red), _tempMiddlePoint.X - 5, _tempMiddlePoint.Y - 5, 10, 10);
            }
        }
    }
}
