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
            _g = Graphics.FromImage(_bitmap);

            _mainList = new TShapeList();
            pictureBox1.Image = _bitmap;
            pictureBox1.Size = size;
            
            this.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            bitmapSizeLabel.Text = "W:  " + _bitmap.Width.ToString() + "  H:" + _bitmap.Height.ToString();

            InitBahaviors();
            SetBehaviorByDefault();
        }
        TShapeList _mainList;

        #region Настройки перья и кисти для отрисовки
            private readonly Pen _pen = new Pen(Color.Black, 2);
            private readonly Font _drawFont = new Font("Arial", 12);
            private readonly SolidBrush _drawBrush = new SolidBrush(Color.Black);
            private readonly int _margin = 4;
        #endregion


        private Dictionary<Type, IToolBehavior> _behaviorsMap; //Словарь хранящий инструменты
        private IToolBehavior _сurrentBehavior; //Текущий инструмент
        private Bitmap _bitmap;
        private TListOfPoints _list; //cписок хранящий все точки
        private TListOfShape _shapeList; //Список хранящий все фигуры
        private Graphics _g; //с помощью него рисуется вся графика на PictureBox
        private Size size;
        private TPoint _mousePos;


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
            var i = 0;

            foreach (var shape in _mainList)//Перебираем фигуры
            {
                listBox1.Items.Add("Shape #" + i.ToString());

                foreach (var point in shape)//Перебираем точки внутри фигуры
                {
                    if (point != null)
                        listBox1.Items.Add("-----" + point.GetString());
                }
                i++;
            }
        }

        public void UpdateImage() //Перерисовка всего изображения
        {
            UpdateUI();
            pictureBox1.Invalidate();
        }

        private Point prevLoc;
        #region Собития PictureBox
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _сurrentBehavior.MouseMove(e, _mousePos); //Выполняем событие внутри инструмента
            pictureBox1.Refresh(); //Перерисовываем область рисования

            _mousePos = new TPoint(_bitmap.Width * e.X / pictureBox1.Width,
            _bitmap.Height * e.Y / pictureBox1.Height);
            mousePosLabel.Text = _mousePos.GetString();

            if (e.Button == MouseButtons.Middle)//Перемещаем весь PictureBox средней кнопкой мыши
            {
                Cursor.Current = Cursors.NoMove2D;
                pictureBox1.Location = new Point(pictureBox1.Location.X + (e.X - prevLoc.X),
                pictureBox1.Location.Y + (e.Y - prevLoc.Y));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _mousePos = new TPoint(_bitmap.Width * e.X / pictureBox1.Width,
                _bitmap.Height * e.Y / pictureBox1.Height);

            _сurrentBehavior.MouseDown(e, _mousePos); //Выполняем событие внутри инструмента

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
            _сurrentBehavior.MouseUp(e, _mousePos); //Выполняем событие внутри инструмента
            UpdateUI(); //Перерисовка listBox
        }

        int PrevBez = 0; //Для того чтобы кривая Безье рисовалась всегда (обычно при (count % 3) + 1)
        private void pictureBox1_Paint(object sender, PaintEventArgs e) //Отрисовка всех точек и фигур
        {
            _сurrentBehavior.Paint(e, _mousePos);

            _g.Clear(DefaultBackColor);

            foreach (var item in _mainList._list.Values)
            {
                item.Draw(_g);
            }
            //foreach (var shapes in _mainList.GetRectangles())
            //{
            //    var j = 0;
            //    var Count = shapes.Count;
            //    foreach (var points in shapes)
            //    {
            //        var prev = shapes.GetItem(j - 1);
            //        _g.DrawEllipse(new Pen(points.Color),
            //            points.X - 5, points.Y - 5, 10, 10); //рисуем точки

            //        _g.DrawString(j.ToString(), _drawFont, _drawBrush, //рисуем номера точек
            //            points.X + _margin, points.Y + _margin);

            //        _g.DrawLine(_pen, points.X, points.Y, //рисуем линии
            //            shapes.GetItem(j - 1).X, shapes.GetItem(j - 1).Y);
            //        //Дорисовываем последний отрезок линии
            //        _g.DrawLine(_pen, shapes.GetItem(Count - 1).X, shapes.GetItem(Count - 1).Y, //рисуем линии
            //            shapes.GetItem(0).X, shapes.GetItem(0).Y);
            //        j++;
            //    }
                
            //}
            //var o = 0;
            //foreach (var points in _mainList.GetPoints())
            //{
            //    var list = _mainList.GetPoints();
            //    _g.DrawEllipse(new Pen(points.Color),//Рисуем все точки
            //        points.X - 5, points.Y - 5, 10, 10);
            //    _g.FillEllipse(_drawBrush, points.X - 5, points.Y - 5, 10, 10);

            //    _g.DrawString(o.ToString(), _drawFont, _drawBrush, //Рисуем номера точек
            //        points.X + _margin, points.Y + _margin);

            //    if (o > 0 && _list.isDrawLines) //рисуем линии последовательно между точками
            //    {
            //        _g.DrawLine(_pen, points.X, points.Y,
            //            list.GetItem(o - 1).X, list.GetItem(o - 1).Y);
            //    }
            //    o++;
            //}

            //var i = 0;
            //foreach (var shape in _mainList.GetShapes())  //Перебираем фигуры
            //{
            //    var j = 0;
            //    foreach (var point in shape) //Перебираем точки внутри фигуры
            //    {
            //        _g.DrawEllipse(new Pen(point.Color),
            //            point.X - 5, point.Y - 5, 10, 10); //рисуем точки

            //        _g.DrawString(j.ToString(), _drawFont, _drawBrush, //рисуем номера точек
            //            point.X + _margin, point.Y + _margin);

            //        var Count = shape.Count;
            //        if (!shape.isBezier)
            //        {
            //            _g.DrawLine(_pen, point.X, point.Y, //рисуем линии
            //                shape.GetItem(j - 1).X, shape.GetItem(j - 1).Y);
            //            //Дорисовываем последний отрезок линии
            //            _g.DrawLine(_pen, shape.GetItem(Count - 1).X, shape.GetItem(Count - 1).Y, //рисуем линии
            //            shape.GetItem(0).X, shape.GetItem(0).Y);
            //        }
            //        else if (shape != null && (Count - 1) % 3 == 0)
            //        {
            //            PrevBez = Count;
            //            _g.DrawBeziers(_pen, _shapeList.GetItem(i).GetArray());
            //        }
            //        else if (PrevBez != 0 && _shapeList.GetItem(i).isBezier)
            //        {
            //            _g.DrawBeziers(_pen, _shapeList.GetItem(i).GetArray(PrevBez));
            //        }
            //        j++;
            //    }
            //    i++;
            //}

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
                savedialog.Filter = "JPEG (*.JPG)|*.jpg|BMP (*.BMP)|*.BMP|GIF (*.GIF)|*.GIF|PNG (*.PNG)|*.PNG|Все файлы (*.*)|*.*";
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                this._сurrentBehavior.Enter(_mainList, _g);
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
            if (!Options.isDrawLines) //проверяем включен ли режим
            {
                Options.isDrawLines = true;
                соеденитьТочкиПоследовательноToolStripMenuItem.Checked = true;
            }
            else
            {
                Options.isDrawLines = false;
                соеденитьТочкиПоследовательноToolStripMenuItem.Checked = false;
            }

        }

        private void режимУмногоДобавленияТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Options.isMiddlePoint)//проверяем включен ли режим
            {
                Options.isMiddlePoint = true;
                режимУмногоДобавленияТочекToolStripMenuItem.Checked = true;
            }
            else
            {
                Options.isMiddlePoint = false;
                режимУмногоДобавленияТочекToolStripMenuItem.Checked = false;
            }
        }

        private void поворотИПеремещениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Dialog testDialog = new Form_Dialog(_list, this);

            if (!Options.isTransformAndRotate)//проверяем включен ли режим
            {
                Options.isTransformAndRotate = true;
                поворотИПеремещениеToolStripMenuItem.Checked = true;
                testDialog.Show(); //открываем форму перемещения/поворота
            }
            else
            {
                Options.isTransformAndRotate = false;
                поворотИПеремещениеToolStripMenuItem.Checked = false;
                testDialog.Close(); //закрываем форму перемещения/поворота
            }

        }

        private void добавлениеТочкиВНачалоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Options.isInFirst)
            {
                Options.isInFirst = true;
                добавлениеТочкиВНачалоToolStripMenuItem.Checked = true;
            }
            else
            {
                Options.isInFirst = false;
                добавлениеТочкиВНачалоToolStripMenuItem.Checked = false;
            }
        }
        #endregion

        #region Количество ребер фигур
        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            Options.ShapeSides = 2; //Рисуем линию
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            линияToolStripMenuItem.Checked = true; 
            треугольникToolStripMenuItem.Checked = false;
            четырехугольникToolStripMenuItem.Checked = false;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            Options.ShapeSides = 3; //Рисуем треугольник
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
            треугольникToolStripMenuItem.Checked = true;
            линияToolStripMenuItem.Checked = false;
            четырехугольникToolStripMenuItem.Checked = false;
        }

        private void четырехугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            Options.ShapeSides = 4; //Рисуем четырехугольник
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
    }
}
