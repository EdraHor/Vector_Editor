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
        //TListOfShape shapeList; // Список фигур
        //TListOfPoints _list;
        private TPoint MousePos;

        TShapeList _mainList;
        Graphics _g;

        public void Enter(TShapeList mainList, Graphics graphics)
        {
            Console.WriteLine("Enter Hand behavior");
            //shapeList = ShapeList; //Инициализируем список фигур
            _mainList = mainList;
            _g = graphics;
            //_list = list;
        }


        public void Exit()
        {
            Console.WriteLine("Exit Hand behavior");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {
            var i = 0;
            //foreach (var points in _mainList.GetPoints())
            //{
            //    if (Math.Abs(e.X - points.X) < 10 && //Проверяем наличие курсора в область точки
            //        Math.Abs(e.Y - points.Y) < 10)
            //    {
            //        points.Select();
            //        _tempSelectedPoint = i;
            //        isDownOnPoint = true; //Выбрана точка
            //        Cursor.Current = Cursors.Hand;
            //    }
            //    i++;
            //}
            //i = 0;
            //var j = 0; 
            //foreach (var shapes in _mainList.GetShapes())
            //{
            //    if (e.Button == MouseButtons.Right) shapes.Deselect();
            //    foreach (var points in shapes)
            //    {
            //        if (Math.Abs(e.X - points.X) < 10 && //Проверяем наличие курсора в область точки
            //            Math.Abs(e.Y - points.Y) < 10)   //фигуры
            //        {
            //            points.Select();
            //            _tempSelectedPoint = j;
            //            _tempSelectedShape = i;
            //            isDownOnPoint = true;
            //            isDownOnShape = true;

            //        }
            //        if (Math.Abs(e.X - shapes.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
            //            Math.Abs(e.Y - shapes.GetCenterPoint().Y) < 15)   //фигуры
            //        {
            //            points.Select();

            //            _tempSelectedShape = i;
            //            isDownOnShape = true; //Выбрана фигура
            //            Cursor.Current = Cursors.Hand;
            //            if (Control.ModifierKeys == Keys.Control) //если зажат Ctrl выделяется текущая фигура
            //            {
            //                shapes.Select();
            //            }
            //        }
            //        j++;
            //    }
            //    i++;
            //}
        }

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            //MousePos = new TPoint(e.X, e.Y); //Сохранение позиции мыши
            //bool isPointSelected = false; //Выбрана ли точка в фигуре
            ////var points = _mainList.GetPoints();
            ////var shapes = _mainList.GetShapes();
            ////                Перемещение                    //
            //if (isDownOnPoint && !isDownOnShape) //Выбрана точка
            //{
            //    points.GetItem(_tempSelectedPoint).SetPoint(e.X, e.Y); //Перемещаем выбранную точку
            //    Cursor.Current = Cursors.Hand;
            //}
            //else if (isDownOnShape && !isDownOnPoint) //Выбрана фигура
            //{
            //    //!
            //    shapes.GetItem(_tempSelectedShape).Moving(MousePos);
            //    foreach (var shape in shapes)
            //    {
            //        if (shape.isSelect) shapes.MovingSelected(MousePos);
            //    }
            //    //!
            //     //Перемещаем выбранную фигуру
            //    Cursor.Current = Cursors.Hand;
            //}
            //else if (isDownOnShape && isDownOnPoint) //Выбрана точка внутри фигуры
            //{
            //    shapes.GetItem(_tempSelectedShape).Points.SetItem(_tempSelectedPoint, MousePos); //Перемещаем выбранную точку
            //    Cursor.Current = Cursors.Hand;
            //}
            ////                Подсветка точек и фигур при наведении          //

            //foreach (var point in points)
            //{
            //    if (Math.Abs(e.X - point.X) < 10 && //Проверяем наличие курсора в область точки
            //        Math.Abs(e.Y - point.Y) < 10)
            //    {
            //        point.Select();
            //    }
            //    else
            //    {
            //        point.Deselect();
            //    }
            //}
            //foreach (var shape in shapes)
            //{
            //    foreach (var point in shape)
            //    {
            //        if (Math.Abs(e.X - point.X) < 10 && //Проверяем наличие курсора в область точки
            //            Math.Abs(e.Y - point.Y) < 10)   //фигуры
            //        {
            //            point.Select();
            //            isPointSelected = true;
            //        }
            //        else
            //        {
            //            if (!shape.isSelect) point.Deselect();
            //        }
            //    }
            //    if (Math.Abs(e.X - shape.GetCenterPoint().X) < 15 && //Проверяем наличие курсора в области центра
            //        Math.Abs(e.Y - shape.GetCenterPoint().Y) < 15)   //фигуры
            //    {
            //        foreach (var point in shape)
            //        {
            //            point.Select();
            //        }
            //    }
            //    else
            //    {
            //        foreach (var point in shape)
            //        {
            //            if (!isPointSelected && !shape.isSelect) point.Deselect();
            //        }
            //    }
            //}
        }

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
                isDownOnPoint = false;
                isDownOnShape = false;
                Cursor.Current = Cursors.Default;
        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {
            //var shapes = _mainList.GetShapes();
            //var rects = _mainList.GetRectangles();
            //foreach (var shape in shapes) //При наведении отрисовывается квадрат к центре фигуры
            //{
            //    if (MousePos != null && Math.Abs(MousePos.X - shape.GetCenterPoint().X) < 15 &&
            //        Math.Abs(MousePos.Y - shape.GetCenterPoint().Y) < 15)
            //    {
            //        e.Graphics.DrawRectangle(new Pen(Color.Red), shape.GetCenterPoint().X - 5,
            //        shape.GetCenterPoint().Y - 5, 10, 10);
            //    }
            //    else
            //    {
            //        e.Graphics.DrawRectangle(new Pen(Color.Gray), shape.GetCenterPoint().X - 5,
            //        shape.GetCenterPoint().Y - 5, 10, 10);
            //    }
            //}
            //foreach (var shape in rects) //При наведении отрисовывается квадрат к центре фигуры
            //{
            //    if (MousePos != null && Math.Abs(MousePos.X - shape.GetCenterPoint().X) < 15 &&
            //        Math.Abs(MousePos.Y - shape.GetCenterPoint().Y) < 15)
            //    {
            //        e.Graphics.DrawRectangle(new Pen(Color.Red), shape.GetCenterPoint().X - 5,
            //        shape.GetCenterPoint().Y - 5, 10, 10);
            //    }
            //    else
            //    {
            //        e.Graphics.DrawRectangle(new Pen(Color.Gray), shape.GetCenterPoint().X - 5,
            //        shape.GetCenterPoint().Y - 5, 10, 10);
            //    }
            //}
        }

    }
}
