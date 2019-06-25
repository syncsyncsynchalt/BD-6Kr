using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class UIStartupHeader : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private List<Animation> _listAnimation;

		private void OnDestroy()
		{
			Mem.Del(ref _uiLabel);
			Mem.Del(ref _uiBackground);
			Mem.DelList(ref _listAnimation);
		}

		public void ClearMessage()
		{
			_uiLabel.text = string.Empty;
		}

		public void SetMessage(string title)
		{
			_uiLabel.text = title;
		}
	}
}
