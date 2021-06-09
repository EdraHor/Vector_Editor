using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vector_Editor
{
    public static class TLst
    {
        //метод поиска ближних точек на позиции в списке
        //Односвязный список на указателях с метками начала и конца списка
        //Передавать наш список в другие области программы и убрать static
        //Добавить TPosition

        //public List<TPoint> data = new List<TPoint>();
        public const int DataSize = 100;
        public static TPoint[] Data;
        private static int FirstEinList;
        private static int LastEinList;
        private static int CurrentPoint;


        static TLst()
        {
            CurrentPoint = 1;
            Data = new TPoint[DataSize];
            FirstEinList = 1;
            LastEinList = DataSize;
            ResetAllPoints();
        }

        public static void ResetAllPoints()
        {
            CurrentPoint = 1;
            TPoint point = new TPoint();
            point.x = 0; point.y = 0;
            for (int i = 0; i < DataSize; i++)
            {
                Data[i] = point;
                Data[i].Active = false;
            }
        }

        public static int GetCurrentPoint()
        {
            return CurrentPoint;
        }

        public static string GetStringPoint(int DataItem)
            {
                return "(" + DataItem.ToString() + ") x: " + Data[DataItem].x.ToString()
                + "; y: " + Data[DataItem].y.ToString();
        }

        public static int GetDataSize()
        {
            return Data.Length;
        }

        public static void AddPoint(TPoint point)
        {
            point.Active = true;
            Data[CurrentPoint] = point;
            CurrentPoint++;
        }

        public static bool EqualsPoints(TPoint point)
        {
            foreach (var item in TLst.Data)
            {
                if (Math.Abs(point.x - item.GetPointX()) < 10 &&
                    Math.Abs(point.y - item.GetPointY()) < 10)
                {
                    return true;
                }
            }
            return false;
        }

        public static void InstancePoint(int position)
        {

        }
    
        public static void DeletePoint(int position)
        {
            if (position < FirstEinList || position > DataSize)
                throw new IndexOutOfRangeException("Invalid Position"); ;

            Data[position].SetDisable();
            CurrentPoint--;
            for (int i = position; i < CurrentPoint; i++)
            {
                Data[i] = Data[i + 1];
            }
        }
    }
}
