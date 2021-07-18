using System.Drawing;
using System;

namespace Vector_Editor
{
    class TRectangle : TFigure//, ICloneable
    {

        public TRectangle()
        {
            isClosed = true;
        }
        public TRectangle(TFigure rect)
        {
            isClosed = true;
            Points = rect.Points;
        }
        public TRectangle(TListOfPoints ShapePoints)
        {
            isClosed = true;
            Points = new TListOfPoints();
            for (int i = 0; i < ShapePoints.Count; i++)
            {
                Points.Add(ShapePoints.GetItem(i));
            }
        }


        public override TFigure Clone()
        {
            var figure = base.Clone();
            var rect = new TRectangle();
            return figure;
        }

        public override void Draw(Graphics g)
        {
            g.FillPolygon(Options.Brush, GetArray());
            var i = 0;
            foreach (var item in Points)
            {
                g.DrawLine(Options.Pen, item.ToPoint(), GetItem(i - 1).ToPoint());
                i++;
            }
            g.DrawLine(Options.Pen, GetItem(Count - 1).ToPoint(), GetItem(0).ToPoint());
        }
    }
}
