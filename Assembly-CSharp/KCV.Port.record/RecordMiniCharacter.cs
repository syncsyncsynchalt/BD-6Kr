using UnityEngine;

namespace KCV.Port.record
{
	public class RecordMiniCharacter : MonoBehaviour
	{
		[SerializeField]
		private UIButton _uiBtn;

		[SerializeField]
		private UISprite _uiCharacter1;

		[SerializeField]
		private Animation _anim;

		private bool _isNormal;

		private bool isControl;

		private void Awake()
		{
			_isNormal = true;
			isControl = true;
			if (_uiBtn == null)
			{
				_uiBtn = GetComponent<UIButton>();
			}
			EventDelegate.Add(_uiBtn.onClick, onMiniCharacterEL);
			Util.FindParentToChild(ref _uiCharacter1, base.transform, "Character");
			if ((Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			_anim.Stop();
			_anim.Play("miniCharacter_wait1");
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBtn);
			Mem.Del(ref _uiCharacter1);
			Mem.Del(ref _anim);
		}

		private void onMiniCharacterEL()
		{
			if (isControl)
			{
				isControl = false;
				_isNormal = !_isNormal;
				_uiCharacter1.spriteName = ((!_isNormal) ? "m_2" : "m_1");
				_anim.Stop();
				_anim.Play("miniCharacter_up1");
			}
		}

		private void compAnimation()
		{
			isControl = true;
			_anim.Stop();
			_anim.Play("miniCharacter_wait1");
		}

		private void compWaitAnimation()
		{
			int iLim = XorRandom.GetILim(1, 100);
			_anim.Stop();
			if (iLim >= 75)
			{
				_anim.Play("miniCharacter_wait1");
			}
			else
			{
				_anim.Play("miniCharacter_wait2");
			}
		}
	}
}
