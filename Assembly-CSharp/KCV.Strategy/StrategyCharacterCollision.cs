using KCV.Utils;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyCharacterCollision : MonoBehaviour
	{
		[SerializeField]
		private UITexture liveTex;

		[SerializeField]
		private UIFlagShip UIflagShip;

		private bool failed;

		[SerializeField]
		private ParticleSystem heartUpPar;

		[SerializeField]
		private ParticleSystem heartDownPar;

		[SerializeField]
		private new Camera camera;

		private void Start()
		{
			if (UIflagShip != null)
			{
				UIflagShip.SetOnBackTouchCallBack(HeartAction);
			}
		}

		public void SetCollisionHight(int height)
		{
			BoxCollider2D component = GetComponent<BoxCollider2D>();
			BoxCollider2D boxCollider2D = component;
			Vector2 size = component.size;
			boxCollider2D.size = new Vector2(size.x, height);
		}

		public void OnClick()
		{
			StrategyShipCharacter component = UIflagShip.GetComponent<StrategyShipCharacter>();
			if (!(component == null) && component.shipModel != null)
			{
				int num = UIflagShip.TouchedPartnerShip(component.shipModel);
				ShipUtils.PlayShipVoice(component.shipModel, num);
				int lov = component.shipModel.Lov;
				component.shipModel.LovAction(0, num);
				bool isLovUp = component.shipModel.Lov - lov > 0;
				bool isLovDown = component.shipModel.Lov - lov < 0;
				PlayMotion(component, isLovUp, isLovDown);
				SingletonMonoBehaviour<Live2DModel>.Instance.Play();
			}
		}

		public void ResetTouchCount()
		{
			UIflagShip.ResetClickedCount();
		}

		private void PlayMotion(StrategyShipCharacter character, bool isLovUp, bool isLovDown)
		{
			HeartAction(isLovUp, isLovDown, isBackTouch: false);
			if (UIflagShip.getClickedCount() == 0)
			{
				if (character.shipModel.IsMarriage())
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Secret);
				}
				else
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Port);
				}
			}
			else if (UIflagShip.getClickedCount() < 4)
			{
				if (character.shipModel.Lov >= 700)
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion((Live2DModel.MotionType)Random.Range(6, 8));
				}
				else if (character.shipModel.Lov >= 500)
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Love1);
				}
				else
				{
					SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Port);
				}
			}
			else if (character.shipModel.Lov >= 25 || character.shipModel.IsMarriage())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Port);
			}
			else if (character.shipModel.Lov <= 10)
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion((Live2DModel.MotionType)Random.Range(4, 6));
			}
			else
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Dislike2);
			}
		}

		public void HeartAction(bool isLovUp, bool isLovDown, bool isBackTouch)
		{
			if (isLovUp)
			{
				if (isBackTouch)
				{
					Vector3 localPosition = base.transform.parent.parent.localPosition;
					float y = localPosition.y;
					((Component)heartUpPar).transform.localPosition = new Vector3(0f, 0f - y, 0f);
				}
				else if (camera != null)
				{
					((Component)heartUpPar).transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
				}
				heartUpPar.Stop();
				heartUpPar.Clear();
				heartUpPar.Play();
			}
			else if (isLovDown)
			{
				if (isBackTouch)
				{
					Vector3 localPosition2 = base.transform.parent.parent.localPosition;
					float y2 = localPosition2.y;
					((Component)heartDownPar).transform.localPosition = new Vector3(0f, 0f - y2, 0f);
				}
				else if (camera != null)
				{
					((Component)heartDownPar).transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
				}
				heartDownPar.Stop();
				heartDownPar.Clear();
				heartDownPar.Play();
			}
		}

		public void SetEnableBackTouch(bool isEnable)
		{
			UIflagShip.isEnableBackTouch = isEnable;
		}

		private void OnDestroy()
		{
			liveTex = null;
			UIflagShip = null;
			heartUpPar = null;
			heartDownPar = null;
			camera = null;
		}
	}
}
