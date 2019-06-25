using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AvatarBuilder
	{
		public static Avatar BuildHumanAvatar(GameObject go, HumanDescription monoHumanDescription)
		{
			if (go == null)
			{
				throw new NullReferenceException();
			}
			return BuildHumanAvatarMono(go, monoHumanDescription);
		}

		private static Avatar BuildHumanAvatarMono(GameObject go, HumanDescription monoHumanDescription)
		{
			return INTERNAL_CALL_BuildHumanAvatarMono(go, ref monoHumanDescription);
		}

		private static Avatar INTERNAL_CALL_BuildHumanAvatarMono(GameObject go, ref HumanDescription monoHumanDescription) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Avatar BuildGenericAvatar(GameObject go, string rootMotionTransformName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
