using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorErase : IToolBehavior
    {
        private bool _isFocusedPoint = false;
        private bool _isFocusedShape = false;
        private Guid _tempDeleteFigure;
        private int _tempDeletePoint;
        private TPoint _mousePos; //Позиция мыши, меняется каждый MouseMove

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
            if (e.Button == MouseButtons.Left)
            {
                foreach (var shape in _mainList)
                {
                    var p = 0;
                    //сохраняем точку для удаления
                    foreach (var point in shape)
                    {
                        if (Math.Abs(mousePos.X - point.X) < 10 &&
                            Math.Abs(mousePos.Y - point.Y) < 10)
                        {
                            _tempDeleteFigure = shape.ID;
                            _tempDeletePoint = p;
                            //shape.RemovePoint(_tempDeletePoint);
                            _isFocusedPoint = true; //Выбрана точка

                        }
                        p++;
                    }
                    //сохраняем фигуру для удаления
                    if (Math.Abs(mousePos.X - shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                        Math.Abs(mousePos.Y - shape.GetCenterPoint().Y) < 15)   //фигуры
                    {
                        foreach (var point in shape)
                        {
                            point.Select();
                        }
                        _tempDeleteFigure = shape.ID;
                        _isFocusedShape = true; //Выбрана фигура
                    }
                }
            }
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            bool isPointSelected = false; //Выбрана ли точка в фигуре
            //выделение точек
            foreach (var shape in _mainList) //Выделение точки при наведении
            {
                foreach (var point in shape)
                {
                    if (Math.Abs(mousePos.X - point.X) < 10 && //Проверяем наличие курсора в область точки
                        Math.Abs(mousePos.Y - point.Y) < 10)   //фигуры
                    {
                        point.Select();
                        isPointSelected = true;
                    }
                    else
                    {
                        if (!shape.isSelect) point.Deselect();
                    }
                }
                if (Math.Abs(mousePos.X - shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                    Math.Abs(mousePos.Y - shape.GetCenterPoint().Y) < 15)   //фигуры
                {
                    foreach (var point in shape)
                    {
                        point.Select();
                    }
                }
                else
                {
                    foreach (var point in shape)
                    {
                        if (!isPointSelected && !shape.isSelect) point.Deselect();
                    }
                }

                //Отрисовка центров фигур
                if (mousePos != null && Math.Abs(mousePos.X - shape.GetCenterPoint().X) < 15 &&
                    Math.Abs(mousePos.Y - shape.GetCenterPoint().Y) < 15)
                {
                    _g.DrawRectangle(new Pen(Color.Red), shape.GetCenterPoint().X - 5,
                    shape.GetCenterPoint().Y - 5, 10, 10);
                }
                else
                {
                    _g.DrawRectangle(new Pen(Color.Gray), shape.GetCenterPoint().X - 5,
                    shape.GetCenterPoint().Y - 5, 10, 10);
                }
                foreach (var point in shape)
                {
                    _g.FillEllipse(new SolidBrush(point.Color), point.X - 4, point.Y - 4, 8, 8);
                }
        }
    }
        
    

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
            if (_isFocusedPoint) _mainList._list[_tempDeleteFigure].RemovePoint(_tempDeletePoint);
            else if (_isFocusedShape) _mainList.Remove(_tempDeleteFigure);

            _isFocusedPoint = false;
            _isFocusedShape = false;
        }
    

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {
            
        }
    }
}
