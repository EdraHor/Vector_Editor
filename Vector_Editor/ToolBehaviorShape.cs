using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorShape : IToolBehavior
    {
        public void Enter(List<TLstPointer<TPoint>> ShapeList)
        {
            Console.WriteLine("Enter Shape behavior");

            ShapeLst = ShapeList; //Список фигур, который хранит в себе список точек
        }
        List<TLstPointer<TPoint>> ShapeLst;
        TLstPointer<TPoint> Shape;
        bool shapeStart = false;

        Pen pen = new Pen(Color.Black);
        Font drawFont = new Font("Arial", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        int margin = 4;

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            TPoint point = new TPoint(e.X, e.Y);

            if (!shapeStart) //Начинает создавать фигуру
            {
                Shape = new TLstPointer<TPoint>();
                ShapeLst.Add(Shape);
                shapeStart = true;
                if (!Shape.EqualPoints(point, 10))
                    Shape.Add(point);
            }
            else
            {
                if (!Shape.EqualPoints(point, 10))
                    Shape.Add(point);
                if (Shape.Count == list.ShapeSides) //Когда мы создаем последнюю точку
                {
                    shapeStart = false;
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
                            e.Graphics.DrawLine(pen, item.GetItem(item.Count-1).x, item.GetItem(item.Count-1).y,//рисуем линии
                            item.GetItem(0).x, item.GetItem(0).y);
                    }
                }
            } 
        }
    }
}
