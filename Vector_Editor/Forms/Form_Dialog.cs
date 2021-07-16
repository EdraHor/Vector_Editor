using System;
using System.Windows.Forms;

namespace Vector_Editor
{
    public partial class Form_Dialog : Form
    {
        public Form_Dialog(TListOfPoints list, Form1 form)
        {
            InitializeComponent();
            _list = list; //Инициализируем список точек
            MainForm = form; //Инициплизируем главную форму
        }

        public TListOfPoints _list;
        public Form1 MainForm;

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            int angle = Convert.ToInt32(InputAngle.Text);
            int posX = Convert.ToInt32(InputPosX.Text);
            int posY = Convert.ToInt32(InputPosY.Text);

            _list.RotateAt(_list.GetCenterPoint(), angle); //Вращаем
            _list.TransformAt(posX, -posY); //Перемещаем
            MainForm.UpdateImage(); //Обновляем форму
        }
    }
}

