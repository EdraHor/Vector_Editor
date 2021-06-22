using System;
using System.Windows.Forms;

namespace Vector_Editor
{
    public partial class Form_Dialog : Form
    {
        public Form_Dialog(TLstPointer<TPoint> list, Form1 form)
        {
            InitializeComponent();
            this.list = list; //Инициализируем список точек
            MainForm = form; //Инициплизируем главную форму
        }

        public TLstPointer<TPoint> list;
        public Form1 MainForm;

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            int angle = Convert.ToInt32(InputAngle.Text);
            int posX = Convert.ToInt32(InputPosX.Text);
            int posY = Convert.ToInt32(InputPosY.Text);

            list.RotateAt(list.GetCenterPoint(), angle); //Вращаем
            list.TransformAt(posX, -posY); //Перемещаем
            MainForm.UpdateImage(); //Обновляем форму
        }
    }
}

