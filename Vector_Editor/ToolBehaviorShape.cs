using System;
using System.Drawing;
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
        private bool shapeCreating = false; // Режим создания фигур

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            TPoint point = new TPoint(e.X, e.Y); //запоминаем место клика

            if (!shapeList.EqualPoints(point, 10)) //Проверка наличия точек в месте клика
            {
                if (!shapeCreating) //Начинает создавать фигуру
                {
                    Shape = new TShape();
                    shapeList.Add(Shape);
                    shapeCreating = true;
                    Shape.AddPoint(point);
                }
                else //добавляем новые точки в фигуре
                {
                    Shape.AddPoint(point);
                    if (Shape.Item.Count == shapeList.ShapeSides) //Когда мы создаем последнюю точку
                    {
                        shapeCreating = false; //Завершаем создание фигуры
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
