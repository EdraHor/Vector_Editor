using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorErase : IToolBehavior
    {
        private bool _isFocusedPoint = false;
        private bool _isFocusedShape = false;
        private int _tempDeletePos;
        private TPoint _mousePos; //Позиция мыши, меняется каждый MouseMove

        TListOfShape shapeList; // Список фигур
        TListOfPoints _list;
        TShapeList _mainList;
        private Graphics _g;

        public void Enter(TShapeList mainList, Graphics graphics)
        {
            Console.WriteLine("Enter Erase behavior");
            //shapeList = ShapeList;
            _mainList = mainList;
            _g = graphics;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Erase behavior");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (Math.Abs(e.X - _list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - _list.GetItem(i).Y) < 10)
                {
                    _tempDeletePos = i;
                    _isFocusedPoint = true;
                }
            }

            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);
                if (Math.Abs(e.X - Shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                    Math.Abs(e.Y - Shape.GetCenterPoint().Y) < 15)   //фигуры
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        Shape.Points.GetItem(j).Select();
                    }
                    _tempDeletePos = i;
                    _isFocusedShape = true; //Выбрана фигура
                }
            }
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            _mousePos = new TPoint(e.X, e.Y); //Сохранение позиции мыши
            for (int i = 0; i < _list.Count; i++) //Выделение точки при наведении
            {
                if (Math.Abs(e.X - _list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - _list.GetItem(i).Y) < 10)
                {
                    _list.GetItem(i).Select();
                }
                else _list.GetItem(i).Deselect();
            }

            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);
                if (Math.Abs(e.X - Shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                    Math.Abs(e.Y - Shape.GetCenterPoint().Y) < 15)   //фигуры
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        Shape.Points.GetItem(j).Select();
                    }
                }
                else
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        if (!Shape.isSelect) Shape.Points.GetItem(j).Deselect();
                    }
                }
            }
        }

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
            if (_isFocusedPoint) _list.Remove(_tempDeletePos);
            else if (_isFocusedShape) shapeList.Remove(_tempDeletePos);

            _isFocusedPoint = false;
            _isFocusedShape = false;
        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {
            for (int i = 0; i < shapeList.Count; i++) //При наведении отрисовывается квадрат к центре фигуры
            {
                var Shape = shapeList.GetItem(i);
                if (Math.Abs(_mousePos.X - Shape.GetCenterPoint().X) < 15 &&
                    Math.Abs(_mousePos.Y - Shape.GetCenterPoint().Y) < 15)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red), Shape.GetCenterPoint().X - 5,
                        Shape.GetCenterPoint().Y - 5, 10, 10);
                }
            }
        }
    }
}
