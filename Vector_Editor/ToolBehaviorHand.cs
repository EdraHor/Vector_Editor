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
        private int _tempSelectedShape = 0;
        TLstShape<TShape> shapeList; // Список фигур
        TLstPointer<TPoint> _list;
        private TPoint MousePos;

        public void Enter(TLstShape<TShape> ShapeList, TLstPointer<TPoint> list)
        {
            Console.WriteLine("Enter Hand behavior");
            shapeList = ShapeList; //Инициализируем список фигур
            _list = list;
        }


        public void Exit()
        {
            Console.WriteLine("Exit Hand behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TPoint mousePosB)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (Math.Abs(e.X - _list.GetItem(i).X) < 10 && //Проверяем наличие курсора в область точки
                    Math.Abs(e.Y - _list.GetItem(i).Y) < 10)
                {
                    _list.GetItem(i).Select();
                    _tempSelectedPoint = i;
                    isDownOnPoint = true; //Выбрана точка
                    Cursor.Current = Cursors.Hand;
                }
            }
            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);

                for (int j = 0; j < Shape.Count; j++)
                {
                    if (Math.Abs(e.X - Shape.Item.GetItem(j).X) < 10 && //Проверяем наличие курсора в область точки
                        Math.Abs(e.Y - Shape.Item.GetItem(j).Y) < 10)   //фигуры
                    {
                        Shape.Item.GetItem(i).Select();
                        _tempSelectedPoint = j;
                        _tempSelectedShape = i;
                        isDownOnPoint = true;
                        isDownOnShape = true;
                        if (Control.ModifierKeys == Keys.Control) //если зажат Ctrl
                        {
                            Shape.isSelect = true;
                            Shape.Select();
                        }
                    }
                    if (Math.Abs(e.X - Shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                        Math.Abs(e.Y - Shape.GetCenterPoint().Y) < 15)   //фигуры
                    {
                        Shape.Item.GetItem(j).Select();

                        _tempSelectedShape = i;
                        isDownOnShape = true; //Выбрана фигура
                        Cursor.Current = Cursors.Hand;
                        if (Control.ModifierKeys == Keys.Control) //если зажат Ctrl выделяется текущая фигура
                        {
                            Shape.Select();
                        }
                    }
                }
            if (e.Button == MouseButtons.Right) Shape.Deselect();
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e)
        {
            MousePos = new TPoint(e.X, e.Y); //Сохранение позиции мыши
            bool isPointSelected = false; //Выбрана ли точка в фигуре
            //                Перемещение                    //
            if (isDownOnPoint && !isDownOnShape) //Выбрана точка
            {
                _list.GetItem(_tempSelectedPoint).SetPoint(e.X, e.Y); //Перемещаем выбранную точку
                Cursor.Current = Cursors.Hand;
            }
            else if (isDownOnShape && !isDownOnPoint) //Выбрана фигура
            {
                for (int i = 0; i < shapeList.Count; i++)
                {
                    shapeList.GetItem(_tempSelectedShape).Moving(MousePos);

                    if (shapeList.GetItem(i).isSelect) shapeList.MovingSelected(MousePos);
                }
                 //Перемещаем выбранную фигуру
                Cursor.Current = Cursors.Hand;
            }
            else if (isDownOnShape && isDownOnPoint) //Выбрана точка внутри фигуры
            {
                shapeList.GetItem(_tempSelectedShape).Item.SetItem(_tempSelectedPoint, MousePos); //Перемещаем выбранную точку
                Cursor.Current = Cursors.Hand;
            }
            //                Подсветка точек и фигур при наведении          //

            for (int i = 0; i < _list.Count; i++) //Перебираем точки
            {
                if (Math.Abs(e.X - _list.GetItem(i).X) < 10 && //Проверяем наличие курсора в область точки
                    Math.Abs(e.Y - _list.GetItem(i).Y) < 10)
                {
                    _list.GetItem(i).Select();
                }
                else
                {
                    _list.GetItem(i).Deselect();
                }
            }
            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);
                for (int j = 0; j < Shape.Count; j++) //Перебор точек фигуры
                {
                    if (Math.Abs(e.X - Shape.Item.GetItem(j).X) < 10 && //Проверяем наличие курсора в область точки
                        Math.Abs(e.Y - Shape.Item.GetItem(j).Y) < 10)   //фигуры
                    {
                        Shape.Item.GetItem(j).Select();
                        isPointSelected = true;
                    }
                    else
                    {
                        if (!Shape.isSelect) Shape.Item.GetItem(j).Deselect();
                    }
                } 

                //Перебор фигур по их центрам
                if (Math.Abs(e.X - Shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
                    Math.Abs(e.Y - Shape.GetCenterPoint().Y) < 15)   //фигуры
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        Shape.Item.GetItem(j).Select();
                    }
                }
                else
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        if (!isPointSelected && !Shape.isSelect) Shape.Item.GetItem(j).Deselect();
                    }
                }
            }
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e)
        {
                isDownOnPoint = false;
                isDownOnShape = false;
                Cursor.Current = Cursors.Default;
        }

        public void Paint(PaintEventArgs e)
        {
            for (int i = 0; i < shapeList.Count; i++) //При наведении отрисовывается квадрат к центре фигуры
            {
                var Shape = shapeList.GetItem(i);
                if (MousePos != null && Math.Abs(MousePos.X - Shape.GetCenterPoint().X) < 15 &&
                    Math.Abs(MousePos.Y - Shape.GetCenterPoint().Y) < 15)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red), Shape.GetCenterPoint().X - 5,
                    Shape.GetCenterPoint().Y - 5, 10, 10);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Gray), Shape.GetCenterPoint().X - 5,
                    Shape.GetCenterPoint().Y - 5, 10, 10);
                }
            }
        }

    }
}
