using System.Drawing;

namespace Vector_Editor
{
    class TRectangle : TFigure
    {
        public TRectangle()
        {

        }
        public TRectangle(TListOfPoints ShapePoints)
        {
            Points = new TListOfPoints();
            for (int i = 0; i < ShapePoints.Count; i++)
            {
                Points.Add(ShapePoints.GetItem(i));
            }
        }
        public override void Draw(Graphics g)
        {
            var i = 0;
            foreach (var item in Points)
            {
                g.DrawLine(new Pen(Color.Black), item.ToPoint(), GetItem(i - 1).ToPoint());
                i++;
            }
            g.DrawLine(new Pen(Color.Black), GetItem(Count - 1).ToPoint(), GetItem(0).ToPoint());
        }
    }
}
