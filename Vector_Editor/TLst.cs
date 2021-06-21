using System;

namespace Vector_Editor
{
    public static class TLst
    {
        //метод поиска ближних точек на позиции в списке
        //Односвязный список на указателях с метками начала и конца списка
        //Передавать наш список в другие области программы и убрать static
        //Добавить TPosition

        //public List<TPoint> data = new List<TPoint>();
        public const int SIZE = 100;
        public static TPoint[] Data;
        private static int _firstEinList;
        private static int _currentPoint;


        static TLst()
        {
            _currentPoint = 1;
            Data = new TPoint[SIZE];
            _firstEinList = 1;
            ResetAllPoints();
        }

        public static void ResetAllPoints()
        {
            _currentPoint = 1;
            TPoint point = new TPoint(0,0);
            for (int i = 0; i < SIZE; i++)
            {
                Data[i] = point;
                //Data[i].Active = false;
            }
        }

        public static int GetCurrentPoint()
        {
            return _currentPoint;
        }

        public static string GetStringPoint(int DataItem)
            {
                return "(" + DataItem.ToString() + ") x: " + Data[DataItem].X.ToString()
                + "; y: " + Data[DataItem].Y.ToString();
        }

        public static int GetDataSize()
        {
            return Data.Length;
        }

        public static void AddPoint(TPoint point)
        {
            //point.Active = true;
            Data[_currentPoint] = point;
            _currentPoint++;
        }

        public static bool EqualsPoints(TPoint point)
        {
            foreach (var item in TLst.Data)
            {
                if (Math.Abs(point.X - item.X) < 10 &&
                    Math.Abs(point.Y - item.Y) < 10)
                {
                    return true;
                }
            }
            return false;
        }
    
        public static void DeletePoint(int position)
        {
            if (position < _firstEinList || position > SIZE)
                throw new IndexOutOfRangeException("Invalid Position"); ;

            _currentPoint--;
            for (int i = position; i < _currentPoint; i++)
            {
                Data[i] = Data[i + 1];
            }
        }
    }
}
