using Common.Enum;
using LT.Tweening;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UISprite))]
	public class UISelectCommandInfo : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiIcon;

		[SerializeField]
		private UISprite _uiText;

		private UISprite _uiBackground;

		private BattleCommand _iCommand = BattleCommand.None;

		private UISprite background => this.GetComponentThis(ref _uiBackground);

		public BattleCommand command
		{
			get
			{
				return _iCommand;
			}
			private set
			{
				_iCommand = value;
				_uiIcon.spriteName = $"command_{(int)value}_icon";
				_uiText.spriteName = $"command_{(int)value}_txt";
				_uiIcon.MakePixelPerfect();
				_uiText.MakePixelPerfect();
				if (_iCommand != BattleCommand.None)
				{
					_uiText.alpha = 0f;
					_uiIcon.alpha = 0f;
					_uiText.transform.LTCancel();
					_uiText.transform.LTValue(_uiText.alpha, 1f, 0.2f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
					{
						_uiText.alpha = x;
						_uiIcon.alpha = x;
					});
				}
				else
				{
					_uiText.transform.LTCancel();
					_uiText.transform.LTValue(_uiText.alpha, 0f, 0.2f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
					{
						_uiText.alpha = x;
						_uiIcon.alpha = x;
					});
				}
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiIcon);
			Mem.Del(ref _uiText);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _iCommand);
		}

		public void ClearInfo()
		{
			command = BattleCommand.None;
		}

		public void SetInfo(BattleCommand iCommand)
		{
			command = iCommand;
		}
	}
}
