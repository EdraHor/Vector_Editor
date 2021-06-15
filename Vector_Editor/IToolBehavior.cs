using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Vector_Editor
{
    interface IToolBehavior
    {
        void Enter(TLstShape<TShape> ShapeList);
        void Exit();
        void MouseDown(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list);
        void MouseMove(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list);
        void MouseUp(Graphics graphics, MouseEventArgs e, TLstPointer<TPoint> list);
        void Paint(PaintEventArgs e, TLstPointer<TPoint> list);
    }
}
