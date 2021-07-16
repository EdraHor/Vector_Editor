using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ShapeBehaviorRectangle : IShapeBehavior
    {
        //TListOfShape shapeList; // Список фигур
        TFigure Rectangle; // Фигура
        private bool shapeCreating = false; // Режим создания фигур
        private TPoint StartPos;
        //TListOfPoints _list;
        TShapeList _mainList;
        private Graphics _g;

        public void Enter(TShapeList mainList, Graphics graphics)
        {
            Console.WriteLine("Enter Shape behavior Rectangle");

            //shapeList = ShapeList; //Список фигур, который хранит в себе список точек
            //_list = list;
            _mainList = mainList;
            _g = graphics;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior Rectangle");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {
            StartPos = mousePos; //запоминаем место клика

            Rectangle = new TRectangle();
            _mainList.AddRectangle(Rectangle);
            //_mainList.GetRectangles().Add(Shape);
            //shapeList.Add(Shape);
            Rectangle.AddPoint(StartPos);
            //_g.DrawPath()

            Matrix mx;
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!shapeCreating)
                {
                    shapeCreating = true;
                    Rectangle.AddPoint(new TPoint(mousePos.X, StartPos.Y));
                    Rectangle.AddPoint(mousePos);
                    Rectangle.AddPoint(new TPoint(StartPos.X, mousePos.Y));
                }
                Rectangle.SetPoint(1, new TPoint(mousePos.X, StartPos.Y));
                Rectangle.SetPoint(2, mousePos);
                Rectangle.SetPoint(3, new TPoint(StartPos.X, mousePos.Y));
            }
        }

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
            shapeCreating = false;
        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {

        }
    }
}
