using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class HumanTrait
	{
		public static int MuscleCount
		{
			get;
		}

		public static string[] MuscleName
		{
			get;
		}

		public static int BoneCount
		{
			get;
		}

		public static string[] BoneName
		{
			get;
		}

		public static int RequiredBoneCount
		{
			get;
		}

		public static int MuscleFromBone(int i, int dofIndex) { throw new NotImplementedException("�Ȃɂ���"); }

		public static int BoneFromMuscle(int i) { throw new NotImplementedException("�Ȃɂ���"); }

		public static bool RequiredBone(int i) { throw new NotImplementedException("�Ȃɂ���"); }

		internal static bool HasCollider(Avatar avatar, int i) { throw new NotImplementedException("�Ȃɂ���"); }

		public static float GetMuscleDefaultMin(int i) { throw new NotImplementedException("�Ȃɂ���"); }

		public static float GetMuscleDefaultMax(int i) { throw new NotImplementedException("�Ȃɂ���"); }

		public static int GetParentBone(int i) { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
