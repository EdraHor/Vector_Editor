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

        public void Enter(TLstShape<TShape> ShapeList, TLstPointer<TPoint> list) //Switch shapes
        {
            Console.WriteLine("Enter Shape behavior");
            _shapeList = ShapeList; //Список фигур, который хранит в себе список точек
            _list = list;

            switch (_shapeList.ShapeSides)
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

        private TLstPointer<TPoint> _list;
        TLstShape<TShape> _shapeList; // Список фигур

        public void Exit()
        {
            Console.WriteLine("Exit Shape behavior");
        }

        public void MouseDown(Graphics graphics, MouseEventArgs e)
        {
            _сurrentShapeBehavior.MouseDown(graphics, e); //Выполняем событие внутри инструмента
        }
        //Сделать типы фигур так само как и инструменты?
        //Ожидаемые фигуры: *Прямоугольник *Эллипс *Линия (*Звезда *Стрелка)

        public void MouseMove(Graphics graphics, MouseEventArgs e)
        {
            _сurrentShapeBehavior.MouseMove(graphics, e); //Выполняем событие внутри инструмента
        }

        public void MouseUp(Graphics graphics, MouseEventArgs e)
        {
            _сurrentShapeBehavior.MouseUp(graphics, e); //Выполняем событие внутри инструмента
        }

        public void Paint(PaintEventArgs e)
        {
            _сurrentShapeBehavior.Paint(e); //Выполняем событие внутри инструмента
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
                this._сurrentShapeBehavior.Enter(_shapeList, _list);
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
