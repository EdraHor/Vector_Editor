using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorErase : IToolBehavior
    {
        bool isFocused = false;
        int deletePoint;
        Pen pen = new Pen(Color.Black);
        Font drawFont = new Font("Arial", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        int margin = 4;

        public void Enter(List<TLstPointer<TPoint>> ShapeList)
        {
            Console.WriteLine("Enter Erase behavior");
        }

        public void Exit()
        {
            Console.WriteLine("Exit Erase behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Math.Abs(e.X - list.GetItem(i).x) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).y) < 10)
                {
                    deletePoint = i;
                    isFocused = true;
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            for (int i = 0; i < list.Count; i++) //Выделение точки при наведении
            {
                if (Math.Abs(e.X - list.GetItem(i).x) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).y) < 10)
                {
                    list.GetItem(i).color = Color.Red;
                }
                else list.GetItem(i).color = Color.Black;
            }
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            if (isFocused) list.Remove(deletePoint);

            isFocused = false;
            deletePoint = 0;
        }

        public void Paint(PaintEventArgs e, TLstPointer<TPoint> list)
        {
            Pen pen = new Pen(Color.Black);
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
