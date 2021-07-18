using System;
using System.Collections.Generic;
using System.Drawing;

namespace Vector_Editor
{
    public class TPolygon : TFigure//, ICloneable
    {
        public TPolygon()
        {
        }

        public TPolygon(TFigure poly)
        {
            Points = poly.Points;
        }

        public TPolygon(TListOfPoints ShapePoints)
        {
            Points = new TListOfPoints();
            for (int i = 0; i < ShapePoints.Count; i++)
            {
                Points.Add(ShapePoints.GetItem(i));
            }
        }

        public override TFigure Clone()
        {
            var poly = new TPolygon();
            var figure = base.Clone();
            return figure;
        }

        public override void Draw(Graphics g)
        {
            var i = 0;
            if (!isClosed) //если полигон еще не закончен, то мы не ресуем последнюю линию
                foreach (var item in Points)
                {
                    g.DrawLine(Options.Pen, item.ToPoint(), GetItem(i - 1).ToPoint());
                    i++;
                }
            else
            if (isClosed)
            {
                g.FillPolygon(Options.Brush, GetArray());
                g.DrawPolygon(Options.Pen, GetArray());
            }
        }
    }
}
