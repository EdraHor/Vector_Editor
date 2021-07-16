using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    interface IToolBehavior
    {
        void Enter(TShapeList mainList, Graphics graphics);
        void Exit();
        void MouseDown(MouseEventArgs e, TPoint mousePos);
        void MouseMove(MouseEventArgs e, TPoint mousePos);
        void MouseUp(MouseEventArgs e, TPoint mousePos);
        void Paint(PaintEventArgs e, TPoint mousePos);
    }
}
