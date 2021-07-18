using System.Drawing;
using System.Windows.Forms;

namespace Vector_Editor
{
    public static class Options
    {
        //Хранит пареаметры и типы для доступа во всем проекте
        public static short ShapeSides;
        public static bool isDrawLines = false;
        public static bool isMiddlePoint = false;
        public static bool isTransformAndRotate = false;
        public static bool isInFirst = false;
        public static Bitmap Bitmap;
        public static PictureBox PictureBox;
        public static TShapeList ListOfShapes;
        public static SolidBrush Brush;
        public static Pen Pen;
        public static bool multiSelect = false;


    }
}
