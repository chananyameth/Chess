using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public enum PlayerColor
    {
        Black = 1,
        White = -1,
        None = 0
    }
    public enum Params
    {
        hatzracha = 0,
        pawn_at_end = 1,
        derech_hilucho = 2,
        d_h_0 = 3,  // derech hilucho bit 1
        d_h_1 = 4,  // derech hilucho bit 2
        d_h_2 = 5,  // derech hilucho bit 3

        count
    };
    public static class Tools
    {
        //X:
        public const int None = -1;
        public const int A = 0;
        public const int B = 1;
        public const int C = 2;
        public const int D = 3;
        public const int E = 4;
        public const int F = 5;
        public const int G = 6;
        public const int H = 7;

        //Y:
        public const int one = 7;
        public const int two = 6;
        public const int three = 5;
        public const int four = 4;
        public const int five = 3;
        public const int six = 2;
        public const int seven = 1;
        public const int eight = 0;

        public static bool inBoard(Point p)
        {
            //couldn't be any other wrong number (see Point.x.set, Point.y.set)
            if (p.x == None || p.y == None)
                return false;

            return true;
        }
        public static bool inBoard(int x, int y)
        {
            if (x < A || H < x)
                return false;
            if (y < eight || one < y)
                return false;

            return true;
        }
    }
}
