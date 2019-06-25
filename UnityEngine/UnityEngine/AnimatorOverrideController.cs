using System;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AnimatorOverrideController : RuntimeAnimatorController
	{
		public RuntimeAnimatorController runtimeAnimatorController
		{
			get;
			set;
		}

		public AnimationClip this[string name]
		{
			get
			{
				return Internal_GetClipByName(name, returnEffectiveClip: true);
			}
			set
			{
				Internal_SetClipByName(name, value);
			}
		}

		public AnimationClip this[AnimationClip clip]
		{
			get
			{
				return Internal_GetClip(clip, returnEffectiveClip: true);
			}
			set
			{
				Internal_SetClip(clip, value);
			}
		}

		public AnimationClipPair[] clips
		{
			get
			{
				AnimationClip[] originalClips = GetOriginalClips();
				Dictionary<AnimationClip, bool> dictionary = new Dictionary<AnimationClip, bool>(originalClips.Length);
				AnimationClip[] array = originalClips;
				foreach (AnimationClip key in array)
				{
					dictionary[key] = true;
				}
				originalClips = new AnimationClip[dictionary.Count];
				dictionary.Keys.CopyTo(originalClips, 0);
				AnimationClipPair[] array2 = new AnimationClipPair[originalClips.Length];
				for (int j = 0; j < originalClips.Length; j++)
				{
					array2[j] = new AnimationClipPair();
					array2[j].originalClip = originalClips[j];
					array2[j].overrideClip = Internal_GetClip(originalClips[j], returnEffectiveClip: false);
				}
				return array2;
			}
			set
			{
				for (int i = 0; i < value.Length; i++)
				{
					Internal_SetClip(value[i].originalClip, value[i].overrideClip);
				}
			}
		}

		public AnimatorOverrideController()
		{
			Internal_CreateAnimationSet(this);
		}

		private static void Internal_CreateAnimationSet([Writable] AnimatorOverrideController self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private AnimationClip Internal_GetClipByName(string name, bool returnEffectiveClip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_SetClipByName(string name, AnimationClip clip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private AnimationClip Internal_GetClip(AnimationClip originalClip, bool returnEffectiveClip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private AnimationClip[] GetOriginalClips() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private AnimationClip[] GetOverrideClips() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
