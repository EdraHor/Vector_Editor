using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorHand : IToolBehavior
    {
        bool isDown;
        int selectedPoint = 0;
        Pen pen = new Pen(Color.Black);
        Font drawFont = new Font("Arial", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        int margin = 4;

        public void Enter(List<TLstPointer<TPoint>> ShapeList)
        {
            Console.WriteLine("Enter Hand behavior");
            ShapeLst = ShapeList;
        }
        List<TLstPointer<TPoint>> ShapeLst;

        public void Exit()
        {
            Console.WriteLine("Exit Hand behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Math.Abs(e.X - list.GetItem(i).x) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).y) < 10)
                {
                    list.GetItem(i).color = Color.Red;
                    selectedPoint = i;
                    isDown = true;
                }
            }
            foreach (var item in ShapeLst) //Перебираем фигуры
            {
                for (int i = 0; i < item.Count; i++) //Перебираем точки внутри фигуры
                {
                    if (Math.Abs(e.X - item.GetItem(i).x) < 10 &&
                        Math.Abs(e.Y - item.GetItem(i).y) < 10)
                    {
                        item.GetItem(i).color = Color.Red;
                        selectedPoint = i;
                        isDown = true;
                    }
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            if (isDown) list.GetItem(selectedPoint).SetPoint(e.X, e.Y);
            for (int i = 0; i < list.Count; i++)
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
            if (isDown)
            {
                list.GetItem(selectedPoint).color = Color.Black;
                isDown = false;
            }
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

            foreach (var item in ShapeLst) //Перебираем фигуры
            {
                for (int i = 0; i < item.Count; i++) //Перебираем точки внутри фигуры
                {
                    e.Graphics.DrawEllipse(new Pen(item.GetItem(i).color),
                        item.GetItem(i).x - 5, item.GetItem(i).y - 5, 10, 10); //рисуем точки

                    e.Graphics.DrawString(i.ToString(), drawFont, drawBrush, //рисуем номера точек
                        item.GetItem(i).x + margin, item.GetItem(i).y + margin);

                    if (item.Count == list.ShapeSides)
                    {
                        e.Graphics.DrawLine(pen, item.GetItem(i).x, item.GetItem(i).y,//рисуем линии
                            item.GetItem(i - 1).x, item.GetItem(i - 1).y);
                        if (item.Count > 2)
                            e.Graphics.DrawLine(pen, item.GetItem(item.Count - 1).x, item.GetItem(item.Count - 1).y,//рисуем линии
                            item.GetItem(0).x, item.GetItem(0).y);
                    }
                }
            }
        }
    }
}
