using Librarys.Object;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UISprite))]
	[RequireComponent(typeof(BoxCollider))]
	public class UIDialogButton : AbsDialogButton<int>
	{
		private UISprite _uiSprite;

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiSprite);
		}
	}
}
