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

        public void Enter(TLstShape<TShape> ShapeList)
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
                if (Math.Abs(e.X - list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).Y) < 10)
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
                if (Math.Abs(e.X - list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).Y) < 10)
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
                list.GetItem(i).X - 5, list.GetItem(i).Y - 5, 10, 10); //Рисуем все точки

                e.Graphics.DrawString(i.ToString(), drawFont, drawBrush,
                list.GetItem(i).X + margin, list.GetItem(i).Y + margin);

                if (i > 0 && list.drawLines)
                {
                    e.Graphics.DrawLine(pen, list.GetItem(i).X, list.GetItem(i).Y,
                        list.GetItem(i - 1).X, list.GetItem(i - 1).Y);
                }
            }
        }
    }
}
