using UnityEngine;

namespace KCV.Album
{
	[RequireComponent(typeof(Animation))]
	public class UIAlbumMiniCharacter : MonoBehaviour
	{
		private UIButton _uiBtn;

		private UISprite _uiCharacter1;

		private UISprite _uiCharacter2;

		private UISprite _uiShadow;

		private Animation _anim;

		private Vector3 mVector3_MushiDefaultPosition;

		private Vector3 mVector3_MushiDefaultScale;

		private Vector3 mVector3_ShadowDefaultScale;

		private Quaternion mQuaternion_MushiDefaultRotation;

		private bool isControl;

		private void OnEnable()
		{
			_uiCharacter2.transform.localScale = mVector3_MushiDefaultScale;
			_uiCharacter2.transform.localPosition = mVector3_MushiDefaultPosition;
			_uiCharacter2.transform.localRotation = mQuaternion_MushiDefaultRotation;
			_uiShadow.transform.localScale = mVector3_ShadowDefaultScale;
			_uiShadow.alpha = 1f;
			isControl = true;
			_anim.Stop();
			_anim.Play("AlbumChara_Wait2");
		}

		private void OnDisable()
		{
			isControl = true;
			_anim.Stop();
		}

		private void Awake()
		{
			if (_uiBtn == null)
			{
				_uiBtn = GetComponent<UIButton>();
			}
			EventDelegate.Add(_uiBtn.onClick, onMiniCharacterEL);
			Util.FindParentToChild(ref _uiCharacter1, base.transform, "Character1");
			Util.FindParentToChild(ref _uiCharacter2, base.transform, "Character2");
			Util.FindParentToChild(ref _uiShadow, base.transform, "Shadow");
			mVector3_MushiDefaultPosition = _uiCharacter2.transform.localPosition;
			mVector3_MushiDefaultScale = _uiCharacter2.transform.localScale;
			mVector3_ShadowDefaultScale = _uiShadow.transform.localScale;
			mQuaternion_MushiDefaultRotation = _uiCharacter2.transform.localRotation;
			if ((Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
		}

		private void OnDestroy()
		{
			_uiBtn = null;
			_uiCharacter1 = null;
			_uiCharacter2 = null;
			_uiShadow = null;
			_anim = null;
		}

		private void onMiniCharacterEL()
		{
			if (!isControl)
			{
				return;
			}
			isControl = false;
			_anim.Stop();
			int iLim = XorRandom.GetILim(1, 100);
			if (iLim >= 85)
			{
				XorRandom.GetILim(1, 5);
				if (iLim >= 3)
				{
					_anim.Play("AlbumChara_Up1");
				}
				else
				{
					_anim.Play("AlbumChara_Up2");
				}
				return;
			}
			XorRandom.GetILim(1, 100);
			if (iLim >= 50)
			{
				_anim.Play("AlbumChara_Normal1");
			}
			else if (iLim >= 20)
			{
				_anim.Play("AlbumChara_Normal2");
			}
			else
			{
				_anim.Play("AlbumChara_Normal3");
			}
		}

		private void startInCharacterAnimate()
		{
		}

		private void animationFinished()
		{
			_anim.Stop();
			int iLim = XorRandom.GetILim(1, 2);
			if (iLim == 1)
			{
				_anim.Play("AlbumChara_In1");
			}
			else
			{
				_anim.Play("AlbumChara_In2");
			}
		}

		private void compAnimation()
		{
			isControl = true;
			_anim.Stop();
			_anim.Play("AlbumChara_Wait2");
		}

		private void compWaitAnimation()
		{
			int iLim = XorRandom.GetILim(1, 100);
			_anim.Stop();
			if (iLim >= 75)
			{
				_anim.Play("AlbumChara_Wait3");
			}
			else
			{
				_anim.Play("AlbumChara_Wait2");
			}
		}
	}
}
