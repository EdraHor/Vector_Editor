using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ShapeBehaviorEllipse : IShapeBehavior
    {
        TListOfShape shapeList; // Список фигур
        TFigure Shape; // Фигура
        private bool shapeCreating = false; // Режим создания фигур
        private TPoint StartPos;
        TListOfPoints _list;

        TShapeList _mainList;
        private Graphics _g;

        #region Настройки пера и кисти для отрисовки
        private readonly Pen _pen = new Pen(Color.Black, 2);
        private readonly Font _drawFont = new Font("Arial", 12);
        private readonly SolidBrush _drawBrush = new SolidBrush(Color.Black);
        #endregion

        public void Enter(TShapeList mainList, Graphics graphics)
        {
            Console.WriteLine("Enter Shape behavior Ellipse");

            //shapeList = ShapeList; //Список фигур, который хранит в себе список точек
            _mainList = mainList;
            _g = graphics;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior Ellipse");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {
            StartPos = new TPoint(e.X, e.Y); //запоминаем место клика

            if (e.Button == MouseButtons.Left)
            {
                if (!shapeCreating)
                {
                    Shape = new TRectangle();
                    Shape.isBezier = true;
                    shapeList.Add(Shape);
                    shapeCreating = true;
                }
                Shape.AddPoint(StartPos);
            }
            if (e.Button == MouseButtons.Right)
            {
                shapeCreating = false;
            }
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {

        }
    

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {

        }
    }
}
