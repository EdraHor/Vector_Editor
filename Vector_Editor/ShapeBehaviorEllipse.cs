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
        TLstShape<TShape> shapeList; // Список фигур
        TShape Shape; // Фигура
        private bool shapeCreating = false; // Режим создания фигур
        private TPoint StartPos;
        TLstPointer<TPoint> _list;

        #region Настройки пера и кисти для отрисовки
        private readonly Pen _pen = new Pen(Color.Black, 2);
        private readonly Font _drawFont = new Font("Arial", 12);
        private readonly SolidBrush _drawBrush = new SolidBrush(Color.Black);
        private readonly int _margin = 4;
        #endregion

        public void Enter(TLstShape<TShape> ShapeList, TLstPointer<TPoint> list)
        {
            Console.WriteLine("Enter Shape behavior Ellipse");

            shapeList = ShapeList; //Список фигур, который хранит в себе список точек
            _list = list;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior Ellipse");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e)
        {
            StartPos = new TPoint(e.X, e.Y); //запоминаем место клика

            if (e.Button == MouseButtons.Left)
            {
                if (!shapeCreating)
                {
                    Shape = new TShape();
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

        public void MouseMove(Graphics graphics, MouseEventArgs e)
        {

        }
    

        public void MouseUp(Graphics graphics, MouseEventArgs e)
        {
        }

        public void Paint(PaintEventArgs e)
        {

        }
    }
}
