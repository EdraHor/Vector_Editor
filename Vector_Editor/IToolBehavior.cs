using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    interface IToolBehavior
    {
        void Enter(TLstShape<TShape> ShapeList, TLstPointer<TPoint> list);
        void Exit();
        void MouseDown(Graphics graphics, MouseEventArgs e, TPoint mousePosB);
        void MouseMove(Graphics graphics, MouseEventArgs e);
        void MouseUp(Graphics graphics, MouseEventArgs e);
        void Paint(PaintEventArgs e);
    }
}
