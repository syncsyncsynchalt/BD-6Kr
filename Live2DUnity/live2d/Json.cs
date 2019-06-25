using System;
using System.Collections.Generic;
using System.Text;

namespace live2d
{
    public class Json
    {

        public Json(char[] jsonBytes)
        {
        }

        public Value parse()
        {
            return new Value(null);
        }

        public static Value parseFromBytes(char[] jsonBytes)
        {
            return new Value(null);
        }

        public static Value parseFromString(string jsonString)
        {
            return new Value(null);
        }

        public static double strToDouble(char[] str, int len, int pos, int[] ret_endpos)
        {
            return -1;
        }
    }
}
