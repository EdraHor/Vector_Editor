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
    public partial class Form_Dialog : Form
    {
        public Form_Dialog(TLstPointer<TPoint> list, Form1 form)
        {
            InitializeComponent();
            this.list = list;
            MainForm = form;
        }

        public TLstPointer<TPoint> list;
        public Form1 MainForm;

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            int angle = Convert.ToInt32(InputAngle.Text);
            int posX = Convert.ToInt32(InputPosX.Text);
            int posY = Convert.ToInt32(InputPosY.Text);

            list.RotateAt(list.GetCenterPoint(), angle);
            list.TransformAt(posX, -posY);
            MainForm.UpdateImage();
        }
    }
}
