using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ShapeBehaviorRectangle : IShapeBehavior
    {
        TLstShape<TShape> shapeList; // Список фигур
        TShape Shape; // Фигура
        private bool shapeCreating = false; // Режим создания фигур
        private TPoint StartPos;
        TLstPointer<TPoint> _list;

        public void Enter(TLstShape<TShape> ShapeList, TLstPointer<TPoint> list)
        {
            Console.WriteLine("Enter Shape behavior Rectangle");

            shapeList = ShapeList; //Список фигур, который хранит в себе список точек
            _list = list;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior Rectangle");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e)
        {
            StartPos = new TPoint(e.X, e.Y); //запоминаем место клика

            Shape = new TShape();
            shapeList.Add(Shape);
            Shape.AddPoint(StartPos);
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!shapeCreating)
                {
                    shapeCreating = true;
                    Shape.AddPoint(new TPoint(e.X, StartPos.Y));
                    Shape.AddPoint(new TPoint(e.X, e.Y));
                    Shape.AddPoint(new TPoint(StartPos.X, e.Y));
                }
                Shape.SetPoint(1, new TPoint(e.X, StartPos.Y));
                Shape.SetPoint(2, new TPoint(e.X, e.Y));
                Shape.SetPoint(3, new TPoint(StartPos.X, e.Y));
            }
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e)
        {
            shapeCreating = false;
        }

        public void Paint(PaintEventArgs e)
        {

        }
    }
}
