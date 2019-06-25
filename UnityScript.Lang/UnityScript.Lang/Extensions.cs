using System;

namespace UnityScript.Lang
{
	public static class Extensions
	{
        public static int length(System.Array a)
        {
            throw new NotImplementedException("‚È‚É‚±‚ê");
            return 0;
        }

        //		public static int length => a.Length;

        //		public static int length => s.Length;

        //		public static bool operator ==(char lhs, string rhs)
        //		{
        //			int num = (1 == rhs.Length) ? 1 : 0;
        //			if (num != 0)
        //			{
        //				num = ((lhs == rhs[0]) ? 1 : 0);
        //			}
        //			return (byte)num != 0;
        //		}

        //		public static bool operator ==(string lhs, char rhs)
        //		{
        //			return rhs == lhs;
        //		}

        //		public static bool operator !=(char lhs, string rhs)
        //		{
        //			int num = (1 != rhs.Length) ? 1 : 0;
        //			if (num == 0)
        //			{
        //				num = ((lhs != rhs[0]) ? 1 : 0);
        //			}
        //			return (byte)num != 0;
        //		}

        //		public static bool operator !=(string lhs, char rhs)
        //		{
        //			return rhs != lhs;
        //		}

        //		public static implicit operator bool(Enum e)
        //		{
        //			return ((IConvertible)e).ToInt32((IFormatProvider)null) != 0;
        //		}
    }
}
