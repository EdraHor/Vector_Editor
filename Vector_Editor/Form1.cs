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
            size = new Size(1000, 1000);
            _bitmap = new Bitmap(size.Width, size.Height); //Размер нашего рисунка
            pictureBox1.Image = _bitmap;
            pictureBox1.Size = size;
            _g = Graphics.FromImage(_bitmap);

            InitBahaviors();
            _list = new TLstPointer<TPoint>();
            _shapeList = new TLstShape<TShape>();
            SetBehaviorByDefault();
            this.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            bitmapSizeLabel.Text = "W:  " + _bitmap.Width.ToString() + "  H:" + _bitmap.Height.ToString();
        }
        #region Настройки перья и кисти для отрисовки
            private readonly Pen _pen = new Pen(Color.Black, 2);
            private readonly Font _drawFont = new Font("Arial", 12);
            private readonly SolidBrush _drawBrush = new SolidBrush(Color.Black);
            private readonly int _margin = 4;
        #endregion


        private Dictionary<Type, IToolBehavior> _behaviorsMap; //Словарь хранящий инструменты
        private IToolBehavior _сurrentBehavior; //Текущий инструмент
        private Bitmap _bitmap;
        private TLstPointer<TPoint> _list; //cписок хранящий все точки
        private TLstShape<TShape> _shapeList; //Список хранящий все фигуры
        private Graphics _g; //с помощью него рисуется вся графика на PictureBox
        private Size size;


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

        private Point prevLoc;
        private TPoint mousePosB;
        #region Собития PictureBox
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Refresh(); //Перерисовываем область рисования

            //mousePosLabel.Text = mousePosB.GetString();
            _сurrentBehavior.MouseMove(_g, e); //Выполняем событие внутри инструмента

            if (e.Button == MouseButtons.Middle)//Перемещаем весь PictureBox средней кнопкой мыши
            {
                Cursor.Current = Cursors.NoMove2D;
                pictureBox1.Location = new Point(pictureBox1.Location.X + (e.X - prevLoc.X),
                    pictureBox1.Location.Y + (e.Y - prevLoc.Y));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosB = new TPoint(_bitmap.Width * e.X / pictureBox1.Width, 
                _bitmap.Height * e.Y / pictureBox1.Height);

            _сurrentBehavior.MouseDown(_g, e, mousePosB); //Выполняем событие внутри инструмента
            UpdateUI(); //Перерисовка listBox
            prevLoc = e.Location; //Сохранение позиции мыши при нажатии
            pictureBox1.Refresh(); //Перерисовываем область рисования
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e) //Событие изменения колесика мыши
        {
            //if (e.Delta > 0 || e.Delta < 0) //Масштабирование формы на значение прокрутки колесика мыши
            //{
            //    pictureBox1.Size = new Size(pictureBox1.Width + e.Delta / 10, pictureBox1.Height + e.Delta / 10);
            //}

            if (e.Delta != 0)
            {
                if (e.Delta < 0)
                {
                    //set minimum size to zoom
                    if (pictureBox1.Width < 50)
                        return;
                }
                else if (e.Delta > 0)
                {
                    //set maximum size to zoom
                    if (pictureBox1.Width > 10000) //настроить
                        return;
                }
                pictureBox1.Width += Convert.ToInt32(pictureBox1.Width * e.Delta / 1000);
                pictureBox1.Height += Convert.ToInt32(pictureBox1.Height * e.Delta / 1000);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _сurrentBehavior.MouseUp(_g, e); //Выполняем событие внутри инструмента
            UpdateUI(); //Перерисовка listBox
        }

        int PrevBez = 0;
        private void pictureBox1_Paint(object sender, PaintEventArgs e) //Отрисовка всех точек и фигур
        {
            _сurrentBehavior.Paint(e);

            _g.Clear(DefaultBackColor);

            for (int i = 0; i < _shapeList.Count; i++)  //Перебираем фигуры
            {
                var Shape = _shapeList.GetItem(i).Item;

                for (int j = 0; j < Shape.Count; j++) //Перебираем точки внутри фигуры
                {
                    _g.DrawEllipse(new Pen(Shape.GetItem(j).Color),
                        Shape.GetItem(j).X - 5, Shape.GetItem(j).Y - 5, 10, 10); //рисуем точки

                    _g.DrawString(j.ToString(), _drawFont, _drawBrush, //рисуем номера точек
                        Shape.GetItem(j).X + _margin, Shape.GetItem(j).Y + _margin);

                    var Count = Shape.Count;
                    if (!_shapeList.GetItem(i).isBezier)
                    {
                        _g.DrawLine(_pen, Shape.GetItem(j).X, Shape.GetItem(j).Y, //рисуем линии
                            Shape.GetItem(j - 1).X, Shape.GetItem(j - 1).Y);
                        //Дорисовываем последний отрезок линии
                        _g.DrawLine(_pen, Shape.GetItem(Count - 1).X, Shape.GetItem(Count - 1).Y, //рисуем линии
                        Shape.GetItem(0).X, Shape.GetItem(0).Y);
                    }
                    else if (Shape != null && (Count - 1) % 3 == 0)
                    {
                        PrevBez = Count;
                        _g.DrawBeziers(_pen, _shapeList.GetItem(i).GetArray());
                    }
                    else if (PrevBez != 0 && _shapeList.GetItem(i).isBezier)
                    {
                        _g.DrawBeziers(_pen, _shapeList.GetItem(i).GetArray(PrevBez));
                    }
                }
            }

            for (int i = 0; i < _list.Count; i++) //Перебираем точки
            {
                _g.DrawEllipse(new Pen(_list.GetItem(i).Color),//Рисуем все точки
                _list.GetItem(i).X - 5, _list.GetItem(i).Y - 5, 10, 10);
                _g.FillEllipse(_drawBrush, _list.GetItem(i).X - 5, _list.GetItem(i).Y - 5, 10, 10);

                _g.DrawString(i.ToString(), _drawFont, _drawBrush, //Рисуем номера точек
                    _list.GetItem(i).X + _margin, _list.GetItem(i).Y + _margin);

                if (i > 0 && _list.isDrawLines) //рисуем линии последовательно между точками
                {
                    _g.DrawLine(_pen, _list.GetItem(i).X, _list.GetItem(i).Y,
                        _list.GetItem(i - 1).X, _list.GetItem(i - 1).Y);
                }
            }
        }
        #endregion

        #region Кнопки меню MenuItems
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog(); //создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    var image = new Bitmap(open_dialog.FileName);
                    //вместо pictureBox1 укажите pictureBox, в который нужно загрузить изображение 
                    this.pictureBox1.Size = image.Size;
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate();
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate(); //Перерисовываем область рисования
            _g.Clear(DefaultBackColor);
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
                this._сurrentBehavior.Enter(_shapeList, _list);
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
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            _shapeList.ShapeSides = 2; //Рисуем линию
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            линияToolStripMenuItem.Checked = true; 
            треугольникToolStripMenuItem.Checked = false;
            четырехугольникToolStripMenuItem.Checked = false;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            _shapeList.ShapeSides = 3; //Рисуем треугольник
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            треугольникToolStripMenuItem.Checked = true;
            линияToolStripMenuItem.Checked = false;
            четырехугольникToolStripMenuItem.Checked = false;
        }

        private void четырехугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {
                pictureBox1.Location = new Point(0, 0);
                pictureBox1.Size = size; //настроить!
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) //если в pictureBox есть изображение
            {
                //создание диалогового окна "Сохранить как..", для сохранения изображения
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
