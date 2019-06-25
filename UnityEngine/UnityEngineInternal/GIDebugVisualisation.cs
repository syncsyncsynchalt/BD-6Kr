using System;

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEngineInternal
{
	public sealed class GIDebugVisualisation
	{
		public static bool cycleMode
		{
			get;
		}

		public static bool pauseCycleMode
		{
			get;
		}

		public static GITextureType texType
		{
			get;
			set;
		}

		public static void ResetRuntimeInputTextures() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void PlayCycleMode() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void PauseCycleMode() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void StopCycleMode() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void CycleSkipInstances(int skip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void CycleSkipSystems(int skip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
