using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    interface IShapeBehavior
    {
        void Enter(TShapeList mainList, Graphics graphics);
        void Exit();
        void MouseDown(MouseEventArgs e, TPoint mousePosB);
        void MouseMove(MouseEventArgs e, TPoint mousePosB);
        void MouseUp(MouseEventArgs e, TPoint mousePosB);
        void Paint(PaintEventArgs e, TPoint mousePosB);
    }
}
