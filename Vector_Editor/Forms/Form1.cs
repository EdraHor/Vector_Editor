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
            Options.PictureBox = pictureBox1;
            Options.Bitmap = _bitmap;
            Options.ListOfShapes = _mainList;
            Options.Pen = _pen;
            Options.Brush = _drawBrush;
            panel6.BackColor = Options.Brush.Color;
            panel7.BackColor = Options.Pen.Color;
            numericUpDown1.Value = (decimal)Options.Pen.Width;
        }
        TShapeList _mainList; 
        #region Настройки перья и кисти для отрисовки
            private readonly Pen _pen = new Pen(Color.Black, 3);
            private readonly Font _drawFont = new Font("Arial", 12);
            private readonly SolidBrush _drawBrush = new SolidBrush(Color.Chocolate);
            private readonly int _margin = 4;
        #endregion


        private Dictionary<Type, IToolBehavior> _behaviorsMap; //Словарь хранящий инструменты
        private IToolBehavior _сurrentBehavior; //Текущий инструмент
        private Bitmap _bitmap;
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
            _g.Clear(DefaultBackColor);
            foreach (var shape in _mainList)
            {
                shape.Draw(_g);
            }
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
            UpdateImage();
        }

        int PrevBez = 0; //Для того чтобы кривая Безье рисовалась всегда (обычно при (count % 3) + 1)
        private void pictureBox1_Paint(object sender, PaintEventArgs e) //Отрисовка всех точек и фигур
        {
            _сurrentBehavior.Paint(e, _mousePos);

            _g.Clear(DefaultBackColor);

            foreach (var shape in _mainList)
            {
                shape.Draw(_g);
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
            _mainList.Clear();
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
            _mainList.Clear(); //Удаляем все точки и фигуры в списке

            TPolygon poly = new TPolygon();
            poly.AddPoint(new TPoint(100, 100)); //Добавляем базовый набор точек
            poly.AddPoint(new TPoint(200, 100));
            poly.AddPoint(new TPoint(200, 200));
            poly.GetItem(0).SetColor(Color.Red); //Изменяем цвет первой точки
            _mainList.AddPolygon(poly);
            тест1ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainList.Clear(); //Удаляем все точки и фигуры в списке

            TPolygon poly = new TPolygon();
            poly.AddPoint(new TPoint(100, 100)); //Добавляем базовый набор точек
            poly.AddPoint(new TPoint(200, 100));
            poly.AddPoint(new TPoint(200, 200));
            poly.GetItem(2).SetColor(Color.Blue); //Изменяем цвет третей точки
            тест4ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainList.Clear(); //Удаляем все точки и фигуры в списке

            TPolygon poly = new TPolygon();
            poly.AddPoint(new TPoint(100, 100)); //Добавляем базовый набор точек
            poly.AddPoint(new TPoint(200, 100));
            poly.AddPoint(new TPoint(200, 200));
            foreach (var point in poly)
            {
                point.Select();
            }
            тест7ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainList.Clear(); //Удаляем все точки и фигуры в списке

            TPolygon poly = new TPolygon();
            poly.AddPoint(new TPoint(100, 100)); //Добавляем базовый набор точек
            poly.AddPoint(new TPoint(200, 100));
            poly.AddPoint(new TPoint(200, 200));

            poly.RemovePoint(0); //Удаляем певую точку

            poly.Points.InstanceItem(0, new TPoint(110, 110)); //Добавляем на место первой точки новую
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
            Form_Dialog testDialog = new Form_Dialog(this);

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
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            Options.ShapeSides = 3; //Рисуем треугольник
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
        }

        private void четырехугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBehaviorByDefault(); //для того чтобы сменить тип фигуры сбрасываем инструмент
            Options.ShapeSides = 4; //Рисуем четырехугольник
            SetBahaviorShape(); //Устанавливаем режим рисования фигур
        }
        #endregion

        #region Быстрое выделение фигур
        private void button1_Click(object sender, EventArgs e) //Полигоны
        {
            foreach (var key in _mainList.polygonKeys)
            {
                _mainList.AddToSelect(key);
                _mainList._list[key].Select();
            }
            Options.multiSelect = true;
            UpdateImage();
        }

        private void button2_Click(object sender, EventArgs e) //Прямоугольники
        {
            foreach (var key in _mainList.rectangleKeys)
            {
                _mainList.AddToSelect(key);
                _mainList._list[key].Select();
            }
            Options.multiSelect = true;
            UpdateImage();
        }

        private void button3_Click(object sender, EventArgs e) //Все фигуры
        {
            foreach (var shape in _mainList)
            {
                _mainList.AddToSelect(shape.ID);
                shape.Select();
            }
            Options.multiSelect = true;
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

        private void buttonRotateLeft_Click(object sender, EventArgs e)
        {
            int angle = Convert.ToInt32(InputAngle.Text);
            _mainList.RotateAt(_mainList.GetCenterSelected(), -angle);
            UpdateImage();
        }

        private void buttonRotateRight_Click(object sender, EventArgs e)
        {
            int angle = Convert.ToInt32(InputAngle.Text);
            _mainList.RotateAt(_mainList.GetCenterSelected(), angle);
            UpdateImage();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            _mainList.CopySelected();
            UpdateImage();
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            _mainList.Paste();
            UpdateImage();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (var key in _mainList.selectedKeys)
            {
                _mainList._list.Remove(key);
            }
            UpdateImage();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            panel6.BackColor = colorDialog1.Color;
            Options.Brush.Color = colorDialog1.Color;
            UpdateImage();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Options.Pen.Width = (float)numericUpDown1.Value;
            UpdateImage();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            panel7.BackColor = colorDialog2.Color;
            Options.Pen.Color = colorDialog2.Color;
            UpdateImage();
        }
    }
}
