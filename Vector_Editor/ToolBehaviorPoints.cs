using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorPoints : IToolBehavior
    {
        Pen pen = new Pen(Color.Black);
        Font drawFont = new Font("Arial", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        int margin = 4;

        public void Enter(List<TLstPointer<TPoint>> ShapeList)
        {
            Console.WriteLine("Enter Points behavior");
        }

        public void Exit()
        {
            Console.WriteLine("Exit Points behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            TPoint point = new TPoint(e.X, e.Y);

            if (!list.smartPoint)
            {
                if (!list.EqualPoints(point, 10))
                    list.Add(point);
            }
            else
            {
                if (!list.EqualPoints(point, 10)) // Точки между линии // Не полностью работает работает!
                {
                    if (list.Count>0)
                        list.InstanceItem(list.GetNearPoint(point)+1,point);
                    else
                        list.Add(point);
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {

        }

        public void MouseUp(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {

        }

        public void Paint(PaintEventArgs e, TLstPointer<TPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                e.Graphics.DrawEllipse(new Pen(list.GetItem(i).color),
                list.GetItem(i).x - 5, list.GetItem(i).y - 5, 10, 10); //Рисуем все точки

                e.Graphics.DrawString(i.ToString(), drawFont, drawBrush, 
                    list.GetItem(i).x + margin, list.GetItem(i).y + margin);

                if (i > 0 && list.drawLines)
                {
                    e.Graphics.DrawLine(pen, list.GetItem(i).x, list.GetItem(i).y,
                        list.GetItem(i - 1).x, list.GetItem(i - 1).y);
                }
            }
        }
    }
}
