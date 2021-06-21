using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _g = pictureBox1.CreateGraphics();
            InitBahaviors();
            _list = new TLstPointer<TPoint>();
            _shapeList = new TLstShape<TShape>();
            SetBehaviorByDefault();
        }
        #region Настройки пера и кисти для отрисовки
            private readonly Pen _pen = new Pen(Color.Black, 2);
            private readonly Font _drawFont = new Font("Arial", 12);
            private readonly SolidBrush _drawBrush = new SolidBrush(Color.Black);
            private readonly int _margin = 4;
        #endregion


        private Dictionary<Type, IToolBehavior> _behaviorsMap; //Словарь хранящий инструменты
        private IToolBehavior _сurrentBehavior; //Текущий инструмент
        //public Bitmap buf;
        private TLstPointer<TPoint> _list; //cписок хранящий все точки
        private TLstShape<TShape> _shapeList; //Список хранящий все фигуры
        private Graphics _g; //с помощью него рисуется вся графика на PictureBox


        public void SetBahaviorHand() //Устанавливает режим перемещения точек
        {
            var type = GetBehavior<ToolBehaviorHand>();
            SetBahavior(type);
        }
        public void SetBahaviorPoints() //Устанавливает режим создания точек 
        {
            var type = GetBehavior<ToolBehaviorPoints>();
            SetBahavior(type);
        }
        public void SetBahaviorErase() //Устанавливает режим удаления точек
        {
            var type = GetBehavior<ToolBehaviorErase>();
            SetBahavior(type);
        }
        public void SetBahaviorShape() //Устанавливает режим создания фигур
        {
            var type = GetBehavior<ToolBehaviorShape>();
            SetBahavior(type);
        }

        public void UpdateUI() //Перерисовка списка точек в listBox
        {
            listBox1.Items.Clear(); //очищаем ListBox со списком точек и фигур
            for (int i = 0; i < _list.Count; i++)
            {
                listBox1.Items.Add(_list.GetStringPoint(i));
            }
            for (int i = 0; i < _shapeList.Count; i++) //Перебираем фигуры
            {
                listBox1.Items.Add("Shape #" + i.ToString());
                var Shape = _shapeList.GetItem(i).Item;
                for (int j = 0; j < Shape.Count; j++) //Перебираем точки внутри фигуры
                {
                    listBox1.Items.Add("-----" + Shape.GetStringPoint(j));
                }
            }
        }

        public void UpdateImage() //Перерисовка всего изображения
        {
            UpdateUI();
            pictureBox1.Invalidate();
        }

        #region Собития PictureBox
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _сurrentBehavior.MouseMove(_g, e, _list); //Выполняем событие внутри инструмента
            pictureBox1.Refresh(); //Перерисовываем область рисования
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _сurrentBehavior.MouseDown(_g, e, _list); //Выполняем событие внутри инструмента
            UpdateUI(); //Перерисовка listBox
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _сurrentBehavior.MouseUp(_g, e, _list); //Выполняем событие внутри инструмента
            UpdateUI(); //Перерисовка listBox
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) //Отрисовка всех точек и фигур
        {
            _сurrentBehavior.Paint(e, _list);

            for (int i = 0; i < _shapeList.Count; i++)  //Перебираем фигуры
            {
                var Shape = _shapeList.GetItem(i).Item;

                for (int j = 0; j < Shape.Count; j++) //Перебираем точки внутри фигуры
                {
                    e.Graphics.DrawEllipse(new Pen(Shape.GetItem(j).Color),
                        Shape.GetItem(j).X - 5, Shape.GetItem(j).Y - 5, 10, 10); //рисуем точки

                    e.Graphics.DrawString(j.ToString(), _drawFont, _drawBrush, //рисуем номера точек
                        Shape.GetItem(j).X + _margin, Shape.GetItem(j).Y + _margin);

                    var Count = Shape.Count;
                    e.Graphics.DrawLine(_pen, Shape.GetItem(j).X, Shape.GetItem(j).Y, //рисуем линии
                        Shape.GetItem(j - 1).X, Shape.GetItem(j - 1).Y);
                    if (Shape.Count > 2)
                        e.Graphics.DrawLine(_pen, Shape.GetItem(Count - 1).X, Shape.GetItem(Count - 1).Y, //рисуем линии
                        Shape.GetItem(0).X, Shape.GetItem(0).Y);
                }
            }

            for (int i = 0; i < _list.Count; i++) //Перебираем точки
            {
                e.Graphics.DrawEllipse(new Pen(_list.GetItem(i).Color),//Рисуем все точки
                _list.GetItem(i).X - 5, _list.GetItem(i).Y - 5, 10, 10);
                e.Graphics.FillEllipse(_drawBrush, _list.GetItem(i).X - 5, _list.GetItem(i).Y - 5, 10, 10);

                e.Graphics.DrawString(i.ToString(), _drawFont, _drawBrush, //Рисуем номера точек
                    _list.GetItem(i).X + _margin, _list.GetItem(i).Y + _margin);

                if (i > 0 && _list.isDrawLines) //рисуем линии последовательно между точками
                {
                    e.Graphics.DrawLine(_pen, _list.GetItem(i).X, _list.GetItem(i).Y,
                        _list.GetItem(i - 1).X, _list.GetItem(i - 1).Y);
                }
            }
        }
        #endregion

        #region Кнопки меню MenuItems
        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            _list.Clear();
            _shapeList.Clear();
            UpdateUI();
        }

        private void создатьТочкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBahaviorPoints();
        }

        private void перемещениеТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBahaviorHand();
        }

        private void удалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBahaviorErase();
        }

        private void фигурыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBahaviorShape();
        }
        #endregion

        #region Инициализация инструментов
        private void InitBahaviors() //Инициализируем словарь со всеми инструментами
        {
            _behaviorsMap = new Dictionary<Type, IToolBehavior>
            {
                [typeof(ToolBehaviorPoints)] = new ToolBehaviorPoints(),
                [typeof(ToolBehaviorHand)] = new ToolBehaviorHand(),
                [typeof(ToolBehaviorErase)] = new ToolBehaviorErase(),
                [typeof(ToolBehaviorShape)] = new ToolBehaviorShape()
            };
        }

        private void SetBahavior(IToolBehavior newBehavior) //Устанавливаем конкретный инструмент
        {
            if (this._сurrentBehavior != newBehavior) //Ограничение выбора одного инструмента дважды
            {
                if (this._сurrentBehavior != null) //Выходим из предыдущего инструмента
                    _сurrentBehavior.Exit();

                this._сurrentBehavior = newBehavior; //Входим в новый инструмент
                this._сurrentBehavior.Enter(_shapeList);
            }
            else Console.WriteLine("Этот инструмент сейчас уже используется");
        }

        private void SetBehaviorByDefault() //Устанавливем инструмент по умолчанию
        {
            SetBahaviorPoints();
        }

        private IToolBehavior GetBehavior<T>() where T : IToolBehavior //Получаем инструмент из словаря
        {
            var type = typeof(T);
            return _behaviorsMap[type];
        }
        #endregion

        #region Тестирование
        private void тест1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _list.Clear(); //Удаляем все точки и фигуры в списке

            _list.Add(new TPoint(100, 100)); //Добавляем базовый набор точек
            _list.Add(new TPoint(200, 100));
            _list.Add(new TPoint(200, 200));
            _list.GetItem(0).SetColor(Color.Red); //Изменяем цвет первой точки
            тест1ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _list.Clear(); //Удаляем все точки и фигуры в списке

            _list.Add(new TPoint(100, 100)); //Добавляем базовый набор точек
            _list.Add(new TPoint(200, 100));
            _list.Add(new TPoint(200, 200));
            _list.GetItem(2).SetColor(Color.Blue); //Изменяем цвет третей точки
            тест4ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _list.Clear(); //Удаляем все точки и фигуры в списке

            _list.Add(new TPoint(100, 100)); //Добавляем базовый набор точек
            _list.Add(new TPoint(200, 100));
            _list.Add(new TPoint(200, 200));
            for (int i = 0; i < _list.Count; i++) //Изменяем цвет всех точек
            {
                _list.GetItem(i).SetColor(Color.Aqua);
            }
            тест7ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _list.Clear(); //Удаляем все точки и фигуры в списке

            _list.Add(new TPoint(100, 100)); //Добавляем базовый набор точек
            _list.Add(new TPoint(200, 100));
            _list.Add(new TPoint(200, 200));

            _list.Remove(0); //Удаляем певую точку

            _list.InstanceItem(0, new TPoint(110, 110)); //Добавляем на место первой точки новую
            тест10ToolStripMenuItem.Checked = true;      //смещенную на 10px вправо

            UpdateUI();
        }
        #endregion

        #region Функционал блока B
        private void соеденитьТочкиПоследовательноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_list.isDrawLines) //проверяем включен ли режим
            {
                _list.isDrawLines = true;
                соеденитьТочкиПоследовательноToolStripMenuItem.Checked = true;
            }
            else
            {
                _list.isDrawLines = false;
                соеденитьТочкиПоследовательноToolStripMenuItem.Checked = false;
            }

        }

        private void режимУмногоДобавленияТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_list.isMiddlePoint)//проверяем включен ли режим
            {
                _list.isMiddlePoint = true;
                режимУмногоДобавленияТочекToolStripMenuItem.Checked = true;
            }
            else
            {
                _list.isMiddlePoint = false;
                режимУмногоДобавленияТочекToolStripMenuItem.Checked = false;
            }
        }

        private void поворотИПеремещениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Dialog testDialog = new Form_Dialog(_list, this);

            if (!_list.isTransformAndRotate)//проверяем включен ли режим
            {
                _list.isTransformAndRotate = true;
                поворотИПеремещениеToolStripMenuItem.Checked = true;
                testDialog.Show(); //открываем форму перемещения/поворота
            }
            else
            {
                _list.isTransformAndRotate = false;
                поворотИПеремещениеToolStripMenuItem.Checked = false;
                testDialog.Close(); //закрываем форму перемещения/поворота
            }

        }

        private void добавлениеТочкиВНачалоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_list.isInFirst)
            {
                _list.isInFirst = true;
                добавлениеТочкиВНачалоToolStripMenuItem.Checked = true;
            }
            else
            {
                _list.isInFirst = false;
                добавлениеТочкиВНачалоToolStripMenuItem.Checked = false;
            }
        }
        #endregion

        #region Количество ребер фигур
        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeList.ShapeSides = 2; //Рисуем линию
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            линияToolStripMenuItem.Checked = true; 
            треугольникToolStripMenuItem.Checked = false;
            четырехугольникToolStripMenuItem.Checked = false;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeList.ShapeSides = 3; //Рисуем треугольник
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            треугольникToolStripMenuItem.Checked = true;
            линияToolStripMenuItem.Checked = false;
            четырехугольникToolStripMenuItem.Checked = false;
        }

        private void четырехугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _shapeList.ShapeSides = 4; //Рисуем четырехугольник
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            четырехугольникToolStripMenuItem.Checked = true;
            треугольникToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
        }
        #endregion

        #region Быстрое выделение фигур
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = _shapeList.GetItem(i);
                if (Shape.Count == 2) Shape.Select(); //Выбираем все линии
            }
            UpdateImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = _shapeList.GetItem(i);
                if (Shape.Count == 3) Shape.Select(); //Выбираем все треугольник
            }
            UpdateImage();
        }

        private void button3_Click(object sender, EventArgs e)
        { 
            for (int i = 0; i < _shapeList.Count; i++) //Перебираем фигуры
            {
                var Shape = _shapeList.GetItem(i);
                if (Shape.Count == 4) Shape.Select(); //Выбираем все четырехугольник
            }
            UpdateImage();
        }
        #endregion
    }
}
