using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorHand : IToolBehavior
    {
        bool isDownOnPoint;
        bool isDownOnShape;
        int selectedPoint = 0;
        Pen pen = new Pen(Color.Black);
        Font drawFont = new Font("Arial", 12);
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        int margin = 4;
        TLstShape<TShape> shapeList; // Список фигур

        public void Enter(TLstShape<TShape> ShapeList)
        {
            Console.WriteLine("Enter Hand behavior");
            shapeList = ShapeList;
        }


        public void Exit()
        {
            Console.WriteLine("Exit Hand behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Math.Abs(e.X - list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).Y) < 10)
                {
                    list.GetItem(i).color = Color.Red;
                    selectedPoint = i;
                    isDownOnPoint = true;
                    Cursor.Current = Cursors.Hand;
                }
            }
            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);
                if (Math.Abs(e.X - Shape.GetCenterPoint().X) < 15 &&
                    Math.Abs(e.Y - Shape.GetCenterPoint().Y) < 15)
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        Shape.Shape.GetItem(j).color = Color.Red;
                    }
                    selectedPoint = i;
                    isDownOnShape = true;
                    Cursor.Current = Cursors.Hand;
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            if (isDownOnPoint) //Выбрана точка
            {
                list.GetItem(selectedPoint).SetPoint(e.X, e.Y); //Перемещаем выбранную точку
                Cursor.Current = Cursors.Hand;
            }
            else if (isDownOnShape) //Выбрана фигура
            {
                shapeList.GetItem(selectedPoint).Moving(new TPoint(e.X,e.Y)); //Перемещаем выбранную фигуру
                Cursor.Current = Cursors.Hand;
            }
            for (int i = 0; i < list.Count; i++) //Перебираем точки
            {
                if (Math.Abs(e.X - list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).Y) < 10)
                {
                    list.GetItem(i).color = Color.Red;
                }
                else
                {
                    list.GetItem(i).color = Color.Black;
                }
            }
            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);
                if (Math.Abs(e.X - Shape.GetCenterPoint().X) < 15 &&
                    Math.Abs(e.Y - Shape.GetCenterPoint().Y) < 15)
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        Shape.Shape.GetItem(j).color = Color.Red;
                    }
                }
                else
                {
                    for (int j = 0; j < Shape.Count; j++)
                    {
                        Shape.Shape.GetItem(j).color = Color.Black;
                    }
                }
            }
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            if (isDownOnPoint)
            {
                isDownOnPoint = false;
                Cursor.Current = Cursors.Default;
            }
            else if (isDownOnShape)
            {
                isDownOnShape = false;
                Cursor.Current = Cursors.Default;
            }
        }

        public void Paint(PaintEventArgs e, TLstPointer<TPoint> list)
        {

        }
    }
}
