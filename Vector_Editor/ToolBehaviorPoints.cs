using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorPoints : IToolBehavior
    {
        TLstShape<TShape> shapeList; // Список фигур
        private TShape shape; // Фигура
        private TPoint _tempMiddlePoint; // временное хранение ближайшей точки на ребре
        private int _tempMiddleIndex; // временный индекс той вершины, после которой можно вставить промежуточную ближайшую точку


        public void Enter(TLstShape<TShape> ShapeList)
        {
            Console.WriteLine("Enter Points behavior");
            shapeList = ShapeList;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Points behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            TPoint mousePos = new TPoint(e.X, e.Y); //Запоминаем позицию мыши

            if (list.isInFirst)
            {
                if (!list.EqualPoints(mousePos, 10) && !shapeList.EqualPoints(mousePos, 10))
                    list.AddToFirst(mousePos);
            }
            else
            {
                if (!list.isMiddlePoint)
                {
                    if (!list.EqualPoints(mousePos, 10) && !shapeList.EqualPoints(mousePos, 10))
                    list.Add(mousePos);
                    else if (list.EqualPoints(mousePos, 10) && list.IsFirst(mousePos, 10)
                            && list.Count > 1) //замыкаем линию и создаем фигуру
                    {
                        shape = new TShape(list);
                        shapeList.Add(shape);
                        list.Clear();
                    }
                }
                else
                {
                    if (list.Count > 1)
                    {
                        if (!list.EqualPoints(_tempMiddlePoint, 10)) //Проверка наличия точек около новой
                        {
                            list.InstanceItem(_tempMiddleIndex, _tempMiddlePoint); //Вставляем точку между двумя
                        }                                                           
                    }
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            if (list.isMiddlePoint)
            {
                TPoint mousePos = new TPoint(e.X, e.Y); //Запоминаем позицию мыши
                if (list.Count > 1)
                {
                    Vector2 mouseVec = new Vector2(mousePos.X, mousePos.Y);

                    // перебираем все рёбра и ищем ближайшую к курсору точку на ребре
                    float minDist = float.MaxValue;

                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        Vector2 v2 = new Vector2(list.GetItem(i).X, list.GetItem(i).Y);
                        Vector2 v1 = new Vector2(list.GetItem(i + 1).X, list.GetItem(i + 1).Y);

                        Vector2 closest = list.FindNearestPointOnLine(v1, v2, mouseVec);

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

        public void MouseUp(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {

        }

        public void Paint(PaintEventArgs e, TLstPointer<TPoint> list)
        {
            if (list.Count > 1 && list.isMiddlePoint)
            {
                e.Graphics.DrawEllipse(new Pen(Color.Red), _tempMiddlePoint.X - 5, _tempMiddlePoint.Y - 5, 10, 10);
            }
        }
    }
}
