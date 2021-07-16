using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    public class ToolBehaviorShape : IToolBehavior
    {
        public ToolBehaviorShape()
        {
            InitBahaviors();
        }

        public void Enter(TShapeList mainList, Graphics graphics) //Switch shapes
        {
            Console.WriteLine("Enter Shape behavior");
            //_shapeList = ShapeList; //Список фигур, который хранит в себе список точек
            _mainList = mainList;
            _g = graphics;

            switch (Options.ShapeSides)
            {
                case 1: 
                    break;
                case 2:
                    break;
                case 3: SetBahaviorEllipse();
                    break;
                case 4: SetBahaviorRectangle();
                    break;

                default:
                    break;
            }
        }
        private Dictionary<Type, IShapeBehavior> _behaviorsMap; //Словарь хранящий инструменты
        private IShapeBehavior _сurrentShapeBehavior; //Текущий инструмент

        TShapeList _mainList;
        private TListOfPoints _list;
        TListOfShape _shapeList; // Список фигур ... //// переключает режимы //////
        private Graphics _g;

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior");
        }

        public void MouseDown(MouseEventArgs e, TPoint mousePos)
        {
            _сurrentShapeBehavior.MouseDown(e, mousePos); //Выполняем событие внутри инструмента
        }
        //Сделать типы фигур так само как и инструменты?
        //Ожидаемые фигуры: *Прямоугольник *Эллипс *Линия (*Звезда *Стрелка)

        public void MouseMove(MouseEventArgs e, TPoint mousePos)
        {
            _сurrentShapeBehavior.MouseMove(e, mousePos); //Выполняем событие внутри инструмента
        }

        public void MouseUp(MouseEventArgs e, TPoint mousePos)
        {
            _сurrentShapeBehavior.MouseUp(e, mousePos); //Выполняем событие внутри инструмента
        }

        public void Paint(PaintEventArgs e, TPoint mousePos)
        {
            _сurrentShapeBehavior.Paint(e, mousePos); //Выполняем событие внутри инструмента
        }

        public void SetBahaviorRectangle() //Устанавливает режим перемещения точек
        {
            var type = GetBehavior<ShapeBehaviorRectangle>();
            SetBahavior(type);
        }
        public void SetBahaviorEllipse() //Устанавливает режим перемещения точек
        {
            var type = GetBehavior<ShapeBehaviorEllipse>();
            SetBahavior(type);
        }

        #region Инициализация инструментов
        private void InitBahaviors() //Инициализируем словарь со всеми инструментами
        {
            _behaviorsMap = new Dictionary<Type, IShapeBehavior>
            {
                [typeof(ShapeBehaviorRectangle)] = new ShapeBehaviorRectangle(),
                [typeof(ShapeBehaviorEllipse)] = new ShapeBehaviorEllipse()
            };
        }

        private void SetBahavior(IShapeBehavior newBehavior) //Устанавливаем конкретный инструмент
        {
            if (this._сurrentShapeBehavior != newBehavior) //Ограничение выбора одного инструмента дважды
            {
                if (this._сurrentShapeBehavior != null) //Выходим из предыдущего инструмента
                    _сurrentShapeBehavior.Exit();

                this._сurrentShapeBehavior = newBehavior; //Входим в новый инструмент
                this._сurrentShapeBehavior.Enter(_mainList, _g);
            }
            else Console.WriteLine("Этот инструмент сейчас уже используется");
        }

        private IShapeBehavior GetBehavior<T>() where T : IShapeBehavior //Получаем инструмент из словаря
        {
            var type = typeof(T);
            return _behaviorsMap[type];
        }
        #endregion
    }
}
