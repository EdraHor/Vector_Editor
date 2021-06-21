﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    class ToolBehaviorErase : IToolBehavior
    {
        private bool _isFocusedPoint = false;
        private bool _isFocusedShape = false;
        private int _deletePos;
        private TPoint _mousePos; //Позиция мыши, меняется каждый MouseMove

        TLstShape<TShape> shapeList; // Список фигур

        public void Enter(TLstShape<TShape> ShapeList)
        {
            Console.WriteLine("Enter Erase behavior");
            shapeList = ShapeList;
        }

        public void Exit()
        {
            Console.WriteLine("Exit Erase behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Math.Abs(e.X - list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).Y) < 10)
                {
                    _deletePos = i;
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
                        Shape.Item.GetItem(j).Select();
                    }
                    _deletePos = i;
                    _isFocusedShape = true; //Выбрана фигура
                }
            }
        }

        public void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            _mousePos = new TPoint(e.X, e.Y); //Сохранение позиции мыши
            for (int i = 0; i < list.Count; i++) //Выделение точки при наведении
            {
                if (Math.Abs(e.X - list.GetItem(i).X) < 10 &&
                    Math.Abs(e.Y - list.GetItem(i).Y) < 10)
                {
                    list.GetItem(i).Select();
                }
                else list.GetItem(i).Deselect();
            }

            for (int i = 0; i < shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = shapeList.GetItem(i);
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
                        if (!Shape.isSelect) Shape.Item.GetItem(j).Deselect();
                    }
                }
            }
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list)
        {
            if (_isFocusedPoint) list.Remove(_deletePos);
            else if (_isFocusedShape) shapeList.Remove(_deletePos);

            _isFocusedPoint = false;
            _isFocusedShape = false;
        }

        public void Paint(PaintEventArgs e, TLstPointer<TPoint> list)
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
