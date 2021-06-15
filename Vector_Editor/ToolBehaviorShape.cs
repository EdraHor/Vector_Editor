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
        public void Enter(TLstShape<TShape> ShapeList)
        {
            Console.WriteLine("Enter Shape behavior");

            shapeList = ShapeList; //Список фигур, который хранит в себе список точек
        }
        TLstShape<TShape> shapeList; // Список фигур
        TShape Shape; // Фигура
        bool shapeStart = false; // Режим создания фигур

        Pen pen = new Pen(Color.Black, 2);
        Font drawFont = new Font("Arial", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        int margin = 4;

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            TPoint point = new TPoint(e.X, e.Y); //запоминаем место клика

            if (!shapeList.EqualPoints(point, 10)) //Проверка наличия точек в месте клика
            {
                if (!shapeStart) //Начинает создавать фигуру
                {
                    Shape = new TShape();
                    shapeList.Add(Shape);
                    shapeStart = true;
                    Shape.AddPoint(point);
                }
                else
                {
                    Shape.AddPoint(point);
                    if (Shape.Shape.Count == list.ShapeSides) //Когда мы создаем последнюю точку
                    {
                        shapeStart = false; //Завершаем создание фигуры
                    }
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
