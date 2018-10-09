using System;
using System.Collections.Generic;
using System.Text;

namespace DrNet
{
    public static class DrNetFastHashCode
    {
        public static int CombineHashCodes(int left, int right)
        {
            return ((left << 5) + left) ^ right;
        }

        public  static int CombineHashCodes(int h1, int h2, int h3)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), h3);
        }

        public  static int CombineHashCodes(int h1, int h2, int h3, int h4)
        {
            return CombineHashCodes(CombineHashCodes(CombineHashCodes(h1, h2), h3), h4);
        }
    }
}
