using System;

namespace live2d
{
	public class UtMath
	{
		public const double M_PI = Math.PI;

		private static Random random = new Random();

		public static double DEG_TO_RAD_D = Math.PI / 180.0;

		public static float DEG_TO_RAD_F = (float)Math.PI / 180f;

		public static double RAD_TO_DEG_D = 180.0 / Math.PI;

		public static float RAD_TO_DEG_F = 57.29578f;

		public static float PI_F = (float)Math.PI;

		private static readonly double[] sintable = new double[128]
		{
			0.0,
			0.012368,
			0.024734,
			0.037097,
			0.049454,
			0.061803,
			0.074143,
			0.086471,
			0.098786,
			0.111087,
			0.12337,
			0.135634,
			0.147877,
			0.160098,
			0.172295,
			0.184465,
			0.196606,
			0.208718,
			0.220798,
			0.232844,
			0.244854,
			0.256827,
			0.268761,
			0.280654,
			0.292503,
			0.304308,
			0.316066,
			0.327776,
			0.339436,
			0.351044,
			0.362598,
			0.374097,
			0.385538,
			0.396921,
			0.408243,
			0.419502,
			0.430697,
			0.441826,
			0.452888,
			0.463881,
			0.474802,
			0.485651,
			0.496425,
			0.507124,
			0.517745,
			0.528287,
			0.538748,
			0.549126,
			0.559421,
			0.56963,
			0.579752,
			0.589785,
			0.599728,
			0.609579,
			0.619337,
			0.629,
			0.638567,
			0.648036,
			0.657406,
			0.666676,
			0.675843,
			0.684908,
			0.693867,
			0.70272,
			0.711466,
			0.720103,
			0.72863,
			0.737045,
			0.745348,
			0.753536,
			0.76161,
			0.769566,
			0.777405,
			0.785125,
			0.792725,
			0.800204,
			0.807561,
			0.814793,
			0.821901,
			0.828884,
			0.835739,
			0.842467,
			0.849066,
			0.855535,
			0.861873,
			0.868079,
			0.874153,
			0.880093,
			0.885898,
			0.891567,
			0.897101,
			0.902497,
			0.907754,
			0.912873,
			0.917853,
			0.922692,
			0.92739,
			0.931946,
			0.936359,
			0.940629,
			0.944755,
			0.948737,
			0.952574,
			0.956265,
			0.959809,
			0.963207,
			0.966457,
			0.96956,
			0.972514,
			0.97532,
			0.977976,
			0.980482,
			0.982839,
			0.985045,
			0.987101,
			0.989006,
			0.990759,
			0.992361,
			0.993811,
			0.995109,
			0.996254,
			0.997248,
			0.998088,
			0.998776,
			0.999312,
			0.999694,
			0.999924,
			1.0
		};

		public static double getAngleNotAbs(float[] v1, float[] v2)
		{
			double q = Math.Atan2(v1[1], v1[0]);
			double q2 = Math.Atan2(v2[1], v2[0]);
			return getAngleDiff(q, q2);
		}

		public static double getAngleDiff(double Q1, double Q2)
		{
			double num;
			for (num = Q1 - Q2; num < -Math.PI; num += Math.PI * 2.0)
			{
			}
			while (num > Math.PI)
			{
				num -= Math.PI * 2.0;
			}
			return num;
		}

		public static double fsin(double x)
		{
			return Math.Sin(x);
		}

		public static float range(float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public static double randDouble()
		{
			return random.NextDouble();
		}

		public static int rand(int max)
		{
			return random.Next(max);
		}

		public float map(float val, float smin, float smax, float dmin, float dmax)
		{
			float num = (val - smin) / (smax - smin);
			return (dmax - dmin) * num + dmin;
		}
	}
}
