using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vector_Editor
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //buf = new Bitmap(pictureBox1.Width, pictureBox1.Height);  // с размерами
            //g = Graphics.FromImage(buf);
            g = pictureBox1.CreateGraphics();
            InitBahaviors();
            SetBehaviorByDefault();

            list = new TLstPointer<TPoint>();
            ShapeLst = new List<TLstPointer<TPoint>>();
        }

        private Dictionary<Type, IToolBehavior> BehaviorsMap; //Словарь хранящий инструменты
        private IToolBehavior CurrentBehavior; //Текущий инструмент
        //public Bitmap buf;
        TLstPointer<TPoint> list;
        List<TLstPointer<TPoint>> ShapeLst;
        public Graphics g;
        public bool isDraw = false;
        public bool IsCreating = true;
        public bool IsMoving = false;


        public void SetBahaviorHand()
        {
            var type = GetBehavior<ToolBehaviorHand>();
            SetBahavior(type);
        }
        public void SetBahaviorPoints()
        {
            var type = GetBehavior<ToolBehaviorPoints>();
            SetBahavior(type);
        }
        public void SetBahaviorErase()
        {
            var type = GetBehavior<ToolBehaviorErase>();
            SetBahavior(type);
        }
        public void SetBahaviorShape()
        {
            var type = GetBehavior<ToolBehaviorShape>();
            SetBahavior(type);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            CurrentBehavior.MouseMove(g, e, list);
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            CurrentBehavior.MouseDown(g, e, list);
            UpdateUI();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            CurrentBehavior.MouseUp(g, e, list);
            UpdateUI();
        }

        public void UpdateUI()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                listBox1.Items.Add(list.GetStringPoint(i));
            }
            foreach (var item in ShapeLst) //Перебираем фигуры
            {
                listBox1.Items.Add("Shape" + item.ToString());
                for (int i = 0; i < item.Count; i++) //Перебираем точки внутри фигуры
                {
                    listBox1.Items.Add("-----"+item.GetStringPoint(i));
                }
            }
        }

        public void UpdateImage()
        {
            UpdateUI();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            CurrentBehavior.Paint(e, list);
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            list.Clear();
            ShapeLst.Clear();
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




        private void InitBahaviors() //Инициализируем словарь со всеми инструментами
        {
            this.BehaviorsMap = new Dictionary<Type, IToolBehavior>();

            this.BehaviorsMap[typeof(ToolBehaviorPoints)] = new ToolBehaviorPoints();
            this.BehaviorsMap[typeof(ToolBehaviorHand)] = new ToolBehaviorHand();
            this.BehaviorsMap[typeof(ToolBehaviorErase)] = new ToolBehaviorErase();
            this.BehaviorsMap[typeof(ToolBehaviorShape)] = new ToolBehaviorShape();
        }

        private void SetBahavior(IToolBehavior newBehavior)
        {
            if (this.CurrentBehavior != newBehavior) //Ограничение выбора одного инструмента дважды
            {
                if (this.CurrentBehavior != null) //Выходим из предыдущего инструмента
                    CurrentBehavior.Exit();

                this.CurrentBehavior = newBehavior; //Входим в новый инструмент
                this.CurrentBehavior.Enter(ShapeLst);
            }
            else Console.WriteLine("Этот инструмент сейчас уже используется");
        }

        private void SetBehaviorByDefault()
        {
            SetBahaviorPoints();
        }

        private IToolBehavior GetBehavior<T>() where T : IToolBehavior //Получаем состояние из словаря
        {
            var type = typeof(T);
            return BehaviorsMap[type];
        }

        private void тест1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Clear();

            list.Add(new TPoint(100, 100));
            list.Add(new TPoint(200, 100));
            list.Add(new TPoint(200, 200));
            list.GetItem(0).color = Color.Blue;
            тест1ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Clear();

            list.Add(new TPoint(100, 100));
            list.Add(new TPoint(200, 100));
            list.Add(new TPoint(200, 200));
            list.GetItem(2).color = Color.Pink;
            тест4ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Clear();

            list.Add(new TPoint(100, 100));
            list.Add(new TPoint(200, 100));
            list.Add(new TPoint(200, 200));
            for (int i = 0; i < list.Count; i++)
            {
                list.GetItem(i).color = Color.Aqua;
            }
            тест7ToolStripMenuItem.Checked = true;
            UpdateUI();
        }

        private void тест10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Clear();

            list.Add(new TPoint(100, 100));
            list.Add(new TPoint(200, 100));
            list.Add(new TPoint(200, 200));

            list.Remove(0);

            list.InstanceItem(0, new TPoint(110, 110));
            тест10ToolStripMenuItem.Checked = true;

            UpdateUI();
        }

        private void соеденитьТочкиПоследовательноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!list.drawLines)
            {
                list.drawLines = true;
                соеденитьТочкиПоследовательноToolStripMenuItem.Checked = true;
            }
            else
            {
                list.drawLines = false;
                соеденитьТочкиПоследовательноToolStripMenuItem.Checked = false;
            }

        }

        private void режимУмногоДобавленияТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!list.smartPoint)
            {
                list.smartPoint = true;
                режимУмногоДобавленияТочекToolStripMenuItem.Checked = true;
            }
            else
            {
                list.smartPoint = false;
                режимУмногоДобавленияТочекToolStripMenuItem.Checked = false;
            }
        }

        private void поворотИПеремещениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Dialog testDialog = new Form_Dialog(list, this);

            if (!list.transformAndRotate)
            {
                list.transformAndRotate = true;
                поворотИПеремещениеToolStripMenuItem.Checked = true;
                testDialog.Show();
            }
            else
            {
                list.transformAndRotate = false;
                поворотИПеремещениеToolStripMenuItem.Checked = false;
                testDialog.Close();
            }

        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.ShapeSides = 2;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.ShapeSides = 3;
        }
    }
}
