using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorHand : IToolBehavior
    {
        private bool isDownOnPoint; //мы нажали по точке
        private bool isDownOnShape; //мы нажали по фигуре
        private int _tempSelectedPoint = 0;
        private Guid _tempSelectedShape;
        //TListOfShape shapeList; // Список фигур
        //TListOfPoints _list;

        TShapeList _mainList;
        Graphics _g;

        public void Enter(TShapeList mainList, Graphics graphics)
        {
            Console.WriteLine("Enter Hand behavior");
            //shapeList = ShapeList; //Инициализируем список фигур
            _mainList = mainList;
            _g = graphics;
        }


        public void Exit()
        {
            Console.WriteLine("Exit Hand behavior");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {
            var multiSelect = Options.multiSelect;
            if (e.Button == MouseButtons.Right) //при правом клике снимаем выделение фигур
            {
                multiSelect = false;
                _mainList.Deselect();
            }

            if (e.Button == MouseButtons.Left)
            {
                var isSelect = false; //была ли выбрана фигура при клике
                if (!multiSelect) _mainList.Deselect();
                var i = 0;
                foreach (var shape in _mainList)
                {
                    if (Math.Abs(mousePos.X - shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                            Math.Abs(mousePos.Y - shape.GetCenterPoint().Y) < 15)   //фигуры
                    {
                        isSelect = true;
                        if (Control.ModifierKeys == Keys.Control) //если зажат Ctrl выделяется текущая фигура
                        {
                            multiSelect = true; //выбрано несколько фигур одновременно
                            _mainList.AddToSelect(shape.ID);
                            isDownOnShape = true; //Выбрана фигура
                            shape.Select();
                        }
                        else
                        {
                            if (!shape.isSelect) _mainList.Deselect();
                            shape.Select();
                            _mainList.AddToSelect(shape.ID);
                            _tempSelectedShape = shape.ID;

                            isDownOnShape = true; //Выбрана фигура
                            Cursor.Current = Cursors.Hand;
                        }
                    }
                    var j = 0;
                    foreach (var point in shape)
                    {
                        if (Math.Abs(mousePos.X - point.X) < 10 && //Проверяем наличие курсора в область точки
                            Math.Abs(mousePos.Y - point.Y) < 10)   //фигуры
                        {
                            point.Select();
                            _tempSelectedPoint = j;
                            _tempSelectedShape = shape.ID; // Сохраняем ключ фигуры
                            isDownOnPoint = true;
                            isDownOnShape = true;
                        }
                        j++;
                    }
                    i++;
                }
                if (!isSelect && Control.ModifierKeys != Keys.Control)
                {
                    _mainList.Deselect();
                    multiSelect = false;
                }
            }
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            var multiSelect = Options.multiSelect;
            bool isPointSelected = false; //Выбрана ли точка в фигуре
            //                Перемещение                    //
            if (isDownOnPoint && !isDownOnShape) //Выбрана точка
            {
                _mainList._list[_tempSelectedShape].GetItem(_tempSelectedPoint).SetPoint(mousePos.X, mousePos.Y); //Перемещаем выбранную точку
                Cursor.Current = Cursors.Hand;
            }
            else if (isDownOnShape && !isDownOnPoint) //Выбрана фигура
            {
                //!
                if (!multiSelect)
                    _mainList._list[_tempSelectedShape].Moving(mousePos);
                else
                    _mainList.MovingSelected(mousePos);
                //!
                //Перемещаем выбранную фигуру
                Cursor.Current = Cursors.Hand;
            }
            else if (isDownOnShape && isDownOnPoint) //Выбрана точка внутри фигуры
            {
                _mainList._list[_tempSelectedShape].Points.SetItem(_tempSelectedPoint, mousePos); //Перемещаем выбранную точку
                Cursor.Current = Cursors.Hand;
            }
            //                Подсветка точек и фигур при наведении          //

            foreach (var shape in _mainList)
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

                //Отрисовка
                foreach (var shape1 in _mainList) //При наведении отрисовывается квадрат в центре фигуры
                {
                    if (mousePos != null && Math.Abs(mousePos.X - shape1.GetCenterPoint().X) < 15 &&
                        Math.Abs(mousePos.Y - shape1.GetCenterPoint().Y) < 15)
                    {
                        _g.DrawRectangle(new Pen(Color.Red), shape1.GetCenterPoint().X - 5,
                        shape1.GetCenterPoint().Y - 5, 10, 10);
                    }
                    else
                    {
                        _g.DrawRectangle(new Pen(Color.Gray), shape1.GetCenterPoint().X - 5,
                        shape1.GetCenterPoint().Y - 5, 10, 10);
                    }
                    foreach (var point in shape1)
                    {
                        if (shape1.isSelect)
                            _g.FillEllipse(new SolidBrush(Color.Red), point.X - 4, point.Y - 4, 8, 8);
                        else
                            _g.FillEllipse(new SolidBrush(Color.Black), point.X - 4, point.Y - 4, 8, 8);
                    }
                }
            }
        }

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
            _mainList.isMouseUp = false;
            isDownOnPoint = false;
            isDownOnShape = false;
            //_mainList._list[_tempSelectedShape].Deselect();
            Cursor.Current = Cursors.Default;
        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {
            //var shapes = _mainList.GetShapes();
            //var rects = _mainList.GetRectangles();

        }

    }
}
