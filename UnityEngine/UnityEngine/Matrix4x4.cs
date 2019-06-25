using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct Matrix4x4
	{
		public float m00;

		public float m10;

		public float m20;

		public float m30;

		public float m01;

		public float m11;

		public float m21;

		public float m31;

		public float m02;

		public float m12;

		public float m22;

		public float m32;

		public float m03;

		public float m13;

		public float m23;

		public float m33;

		public float this[int row, int column]
		{
			get
			{
				return this[row + column * 4];
			}
			set
			{
				this[row + column * 4] = value;
			}
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return m00;
				case 1:
					return m10;
				case 2:
					return m20;
				case 3:
					return m30;
				case 4:
					return m01;
				case 5:
					return m11;
				case 6:
					return m21;
				case 7:
					return m31;
				case 8:
					return m02;
				case 9:
					return m12;
				case 10:
					return m22;
				case 11:
					return m32;
				case 12:
					return m03;
				case 13:
					return m13;
				case 14:
					return m23;
				case 15:
					return m33;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					m00 = value;
					break;
				case 1:
					m10 = value;
					break;
				case 2:
					m20 = value;
					break;
				case 3:
					m30 = value;
					break;
				case 4:
					m01 = value;
					break;
				case 5:
					m11 = value;
					break;
				case 6:
					m21 = value;
					break;
				case 7:
					m31 = value;
					break;
				case 8:
					m02 = value;
					break;
				case 9:
					m12 = value;
					break;
				case 10:
					m22 = value;
					break;
				case 11:
					m32 = value;
					break;
				case 12:
					m03 = value;
					break;
				case 13:
					m13 = value;
					break;
				case 14:
					m23 = value;
					break;
				case 15:
					m33 = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
		}

		public Matrix4x4 inverse => Inverse(this);

		public Matrix4x4 transpose => Transpose(this);

		public bool isIdentity
		{
			get;
		}

		public static Matrix4x4 zero
		{
			get
			{
				Matrix4x4 result = default(Matrix4x4);
				result.m00 = 0f;
				result.m01 = 0f;
				result.m02 = 0f;
				result.m03 = 0f;
				result.m10 = 0f;
				result.m11 = 0f;
				result.m12 = 0f;
				result.m13 = 0f;
				result.m20 = 0f;
				result.m21 = 0f;
				result.m22 = 0f;
				result.m23 = 0f;
				result.m30 = 0f;
				result.m31 = 0f;
				result.m32 = 0f;
				result.m33 = 0f;
				return result;
			}
		}

		public static Matrix4x4 identity
		{
			get
			{
				Matrix4x4 result = default(Matrix4x4);
				result.m00 = 1f;
				result.m01 = 0f;
				result.m02 = 0f;
				result.m03 = 0f;
				result.m10 = 0f;
				result.m11 = 1f;
				result.m12 = 0f;
				result.m13 = 0f;
				result.m20 = 0f;
				result.m21 = 0f;
				result.m22 = 1f;
				result.m23 = 0f;
				result.m30 = 0f;
				result.m31 = 0f;
				result.m32 = 0f;
				result.m33 = 1f;
				return result;
			}
		}

		public override int GetHashCode()
		{
			return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2) ^ (GetColumn(3).GetHashCode() >> 1);
		}

		public override bool Equals(object other)
		{
			if (!(other is Matrix4x4))
			{
				return false;
			}
			Matrix4x4 matrix4x = (Matrix4x4)other;
			return GetColumn(0).Equals(matrix4x.GetColumn(0)) && GetColumn(1).Equals(matrix4x.GetColumn(1)) && GetColumn(2).Equals(matrix4x.GetColumn(2)) && GetColumn(3).Equals(matrix4x.GetColumn(3));
		}

		public static Matrix4x4 Inverse(Matrix4x4 m)
		{
			return INTERNAL_CALL_Inverse(ref m);
		}

		private static Matrix4x4 INTERNAL_CALL_Inverse(ref Matrix4x4 m) { throw new NotImplementedException("�Ȃɂ���"); }

		public static Matrix4x4 Transpose(Matrix4x4 m)
		{
			return INTERNAL_CALL_Transpose(ref m);
		}

		private static Matrix4x4 INTERNAL_CALL_Transpose(ref Matrix4x4 m) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static bool Invert(Matrix4x4 inMatrix, out Matrix4x4 dest)
		{
			return INTERNAL_CALL_Invert(ref inMatrix, out dest);
		}

		private static bool INTERNAL_CALL_Invert(ref Matrix4x4 inMatrix, out Matrix4x4 dest) { throw new NotImplementedException("�Ȃɂ���"); }

		public Vector4 GetColumn(int i)
		{
			return new Vector4(this[0, i], this[1, i], this[2, i], this[3, i]);
		}

		public Vector4 GetRow(int i)
		{
			return new Vector4(this[i, 0], this[i, 1], this[i, 2], this[i, 3]);
		}

		public void SetColumn(int i, Vector4 v)
		{
			this[0, i] = v.x;
			this[1, i] = v.y;
			this[2, i] = v.z;
			this[3, i] = v.w;
		}

		public void SetRow(int i, Vector4 v)
		{
			this[i, 0] = v.x;
			this[i, 1] = v.y;
			this[i, 2] = v.z;
			this[i, 3] = v.w;
		}

		public Vector3 MultiplyPoint(Vector3 v)
		{
			Vector3 result = default(Vector3);
			result.x = m00 * v.x + m01 * v.y + m02 * v.z + m03;
			result.y = m10 * v.x + m11 * v.y + m12 * v.z + m13;
			result.z = m20 * v.x + m21 * v.y + m22 * v.z + m23;
			float num = m30 * v.x + m31 * v.y + m32 * v.z + m33;
			num = 1f / num;
			result.x *= num;
			result.y *= num;
			result.z *= num;
			return result;
		}

		public Vector3 MultiplyPoint3x4(Vector3 v)
		{
			Vector3 result = default(Vector3);
			result.x = m00 * v.x + m01 * v.y + m02 * v.z + m03;
			result.y = m10 * v.x + m11 * v.y + m12 * v.z + m13;
			result.z = m20 * v.x + m21 * v.y + m22 * v.z + m23;
			return result;
		}

		public Vector3 MultiplyVector(Vector3 v)
		{
			Vector3 result = default(Vector3);
			result.x = m00 * v.x + m01 * v.y + m02 * v.z;
			result.y = m10 * v.x + m11 * v.y + m12 * v.z;
			result.z = m20 * v.x + m21 * v.y + m22 * v.z;
			return result;
		}

		public static Matrix4x4 Scale(Vector3 v)
		{
			Matrix4x4 result = default(Matrix4x4);
			result.m00 = v.x;
			result.m01 = 0f;
			result.m02 = 0f;
			result.m03 = 0f;
			result.m10 = 0f;
			result.m11 = v.y;
			result.m12 = 0f;
			result.m13 = 0f;
			result.m20 = 0f;
			result.m21 = 0f;
			result.m22 = v.z;
			result.m23 = 0f;
			result.m30 = 0f;
			result.m31 = 0f;
			result.m32 = 0f;
			result.m33 = 1f;
			return result;
		}

		public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
		{
			this = TRS(pos, q, s);
		}

		public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
		{
			return INTERNAL_CALL_TRS(ref pos, ref q, ref s);
		}

		private static Matrix4x4 INTERNAL_CALL_TRS(ref Vector3 pos, ref Quaternion q, ref Vector3 s) { throw new NotImplementedException("�Ȃɂ���"); }

		public override string ToString()
		{
			return UnityString.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		public string ToString(string format)
		{
			return UnityString.Format("{0}\t{1}\t{2}\t{3}\n{4}\t{5}\t{6}\t{7}\n{8}\t{9}\t{10}\t{11}\n{12}\t{13}\t{14}\t{15}\n", m00.ToString(format), m01.ToString(format), m02.ToString(format), m03.ToString(format), m10.ToString(format), m11.ToString(format), m12.ToString(format), m13.ToString(format), m20.ToString(format), m21.ToString(format), m22.ToString(format), m23.ToString(format), m30.ToString(format), m31.ToString(format), m32.ToString(format), m33.ToString(format));
		}

		public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar) { throw new NotImplementedException("�Ȃɂ���"); }

		public static Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar) { throw new NotImplementedException("�Ȃɂ���"); }

		public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
		{
			Matrix4x4 result = default(Matrix4x4);
			result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
			result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
			result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
			result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
			result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
			result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
			result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
			result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
			result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
			result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
			result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
			result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
			result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
			result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
			result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
			result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;
			return result;
		}

		public static Vector4 operator *(Matrix4x4 lhs, Vector4 v)
		{
			Vector4 result = default(Vector4);
			result.x = lhs.m00 * v.x + lhs.m01 * v.y + lhs.m02 * v.z + lhs.m03 * v.w;
			result.y = lhs.m10 * v.x + lhs.m11 * v.y + lhs.m12 * v.z + lhs.m13 * v.w;
			result.z = lhs.m20 * v.x + lhs.m21 * v.y + lhs.m22 * v.z + lhs.m23 * v.w;
			result.w = lhs.m30 * v.x + lhs.m31 * v.y + lhs.m32 * v.z + lhs.m33 * v.w;
			return result;
		}

		public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
		{
			return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
		}

		public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
		{
			return !(lhs == rhs);
		}
	}
}
