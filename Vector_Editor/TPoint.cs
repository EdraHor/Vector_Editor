using System;
using System.Drawing;
using System.Numerics;

namespace Vector_Editor
{
    public class TPoint
    {
        public Point _point;
        public int X { get => _point.X; set => _point.X = X; } //При вызове X,Y ссылаются на координаты Point
        public int Y { get => _point.Y; set => _point.Y = Y; } //

        public Color Color { get; private set; }

        public TPoint() { Color = Color.Black; } //Конструкторы под разные типы входящих данных
        public TPoint(int X, int Y) 
        {
            _point.X = X;
            _point.Y = Y;
            Color = Color.Black;
        }
        public TPoint(double X, double Y)
        {
            _point.X = (int)Math.Round(X);
            _point.Y = (int)Math.Round(Y);
            Color = Color.Black;
        }
        public TPoint(string X, string Y)
        {
            _point.X = Convert.ToInt32(X);
            _point.Y = Convert.ToInt32(Y);
            Color = Color.Black;
        }
        public TPoint(Vector2 vector)
        {
            _point.X = (int)vector.X;
            _point.Y = (int)vector.Y;
            Color = Color.Black;             //end
        }

        public void SetPoint(int X, int Y)//Установка координат точки
        {
            _point.X = X;
            _point.Y = Y;
        }

        public string GetString() //Получение точек в виде строки
        {
            return "x: " + _point.X + "; y: " + _point.Y;
        }
        public void Select()
        {
            Color = Color.Red;
        }
        public void Deselect()
        {
            Color = Color.Black;
        }
        public void SetColor(Color color)
        {
            Color = color;
        }
    }
}
