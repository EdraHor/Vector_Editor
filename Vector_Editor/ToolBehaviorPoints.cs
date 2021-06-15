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

        public void Enter(TLstShape<TShape> ShapeList)
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
                if (!list.EqualPoints(point, 10)) // Точки между линии //Не полностью работает работает!
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

        }
    }
}
