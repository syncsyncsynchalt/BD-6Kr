using System;

namespace GenericOperator
{
	public struct Complex<T> where T : IComparable<T>
	{
		private T re;

		private T im;

		public T Re
		{
			get
			{
				return re;
			}
			set
			{
				re = value;
			}
		}

		public T Im
		{
			get
			{
				return im;
			}
			set
			{
				im = value;
			}
		}

		public Complex(T re, T im)
		{
			this.re = re;
			this.im = im;
		}

		private static T Add(T x, T y)
		{
			return Operator<T>.Add(x, y);
		}

		private static T Sub(T x, T y)
		{
			return Operator<T>.Subtract(x, y);
		}

		private static T Mul(T x, T y)
		{
			return Operator<T>.Multiply(x, y);
		}

		private static T Div(T x, T y)
		{
			return Operator<T>.Divide(x, y);
		}

		private static T Neg(T x)
		{
			return Operator<T>.Negate(x);
		}

		private static T Acc(T x, T y, T z, T w)
		{
			return Operator<T>.ProductSum(x, y, z, w);
		}

		private static T Det(T x, T y, T z, T w)
		{
			return Operator<T>.ProductDifference(x, y, z, w);
		}

		private static T Norm(T x, T y)
		{
			return Operator<T>.ProductSum(x, y, x, y);
		}

		public Complex<T> Inverse()
		{
			T y = Norm(re, im);
			T val = Div(re, y);
			T val2 = Neg(Div(im, y));
			return new Complex<T>(val, val2);
		}

		public override string ToString()
		{
			if (im.CompareTo(default(T)) < 0)
			{
				return $"{re} - i{Neg(im)}";
			}
			return $"{re} + i{im}";
		}

		public static Complex<T>operator +(Complex<T> x, Complex<T> y)
		{
			return new Complex<T>(Add(x.re, y.re), Add(x.im, y.im));
		}

		public static Complex<T>operator +(T x, Complex<T> y)
		{
			return new Complex<T>(Add(x, y.re), y.im);
		}

		public static Complex<T>operator +(Complex<T> x, T y)
		{
			return new Complex<T>(Add(x.re, y), x.im);
		}

		public static Complex<T>operator -(Complex<T> x)
		{
			return new Complex<T>(Neg(x.re), Neg(x.im));
		}

		public static Complex<T>operator -(Complex<T> x, Complex<T> y)
		{
			return new Complex<T>(Sub(x.re, y.re), Sub(x.im, y.im));
		}

		public static Complex<T>operator -(T x, Complex<T> y)
		{
			return new Complex<T>(Sub(x, y.re), y.im);
		}

		public static Complex<T>operator -(Complex<T> x, T y)
		{
			return new Complex<T>(Sub(x.re, y), x.im);
		}

		public static Complex<T>operator *(Complex<T> x, Complex<T> y)
		{
			return new Complex<T>(Det(x.re, y.re, x.im, y.im), Acc(x.re, y.im, x.im, y.re));
		}

		public static Complex<T>operator *(T x, Complex<T> y)
		{
			return new Complex<T>(Mul(x, y.re), Mul(x, y.im));
		}

		public static Complex<T>operator *(Complex<T> x, T y)
		{
			return new Complex<T>(Mul(x.re, y), Mul(x.im, y));
		}

		public static Complex<T>operator /(Complex<T> x, Complex<T> y)
		{
			return x * y.Inverse();
		}

		public static Complex<T>operator /(T x, Complex<T> y)
		{
			return x * y.Inverse();
		}

		public static Complex<T>operator /(Complex<T> x, T y)
		{
			return new Complex<T>(Div(x.re, y), Div(x.im, y));
		}
	}
}
