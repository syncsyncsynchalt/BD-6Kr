using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIDeckPracticeBanner : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private UITexture mTexture_LevelUp;

		private UIWidget mWidgetThis;

		private Vector3 mDefaultLocalPosition;

		private Vector3 mLevelUpDefaultLocalPosition;

		private int xMovePos = 20;

		private float xMoveTime = 0.15f;

		private Ease xMoveEase = Ease.OutCirc;

		private Ease mxMoveEase = Ease.OutCirc;

		private float mxMoveTime = 0.5f;

		private int xxMovePos = 40;

		private float xxMoveTime = 0.15f;

		private float xxMoveTimeDelay = 1f;

		private float mxxMoveTime = 0.5f;

		private Vector3 mVector3_Show_From = new Vector3(60f, 0f);

		private Vector3 mVector3_Show_To = new Vector3(60f, 20f);

		private float slotTime = 0.7f;

		private float delay = 0.5f;

		public ShipModel Model
		{
			get;
			private set;
		}

		public float alpha
		{
			get
			{
				if (mWidgetThis != null)
				{
					return mWidgetThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (mWidgetThis != null)
				{
					mWidgetThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 0.0001f;
			mTexture_LevelUp.alpha = 0.0001f;
			mDefaultLocalPosition = base.transform.localPosition;
			mLevelUpDefaultLocalPosition = mTexture_LevelUp.transform.localPosition;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				PlayPractice();
			}
			else if (Input.GetKeyDown(KeyCode.Y))
			{
				PlayPracticeWithLevelUp();
			}
		}

		public void PlayPractice()
		{
			GenerateTweenMoveNormal();
		}

		public void PlayPracticeWithLevelUp()
		{
			GenerateTweenMoveLevelUp();
		}

		private Tween GenerateTweenMoveLevelUp()
		{
			Sequence sequence = DOTween.Sequence();
			Sequence s = sequence;
			Transform transform = base.transform;
			Vector3 localPosition = base.transform.localPosition;
			s.Append(transform.DOLocalMoveX(localPosition.x + (float)xMovePos, xMoveTime).SetEase(xMoveEase).SetId(this));
			Sequence s2 = sequence;
			Transform transform2 = base.transform;
			Vector3 localPosition2 = base.transform.localPosition;
			s2.Append(transform2.DOLocalMoveX(localPosition2.x, mxMoveTime).SetEase(Ease.OutBounce).SetId(this));
			sequence.AppendInterval(xxMoveTimeDelay);
			Sequence s3 = sequence;
			Transform transform3 = base.transform;
			Vector3 localPosition3 = base.transform.localPosition;
			s3.Append(transform3.DOLocalMoveX(localPosition3.x + (float)xxMovePos, xxMoveTime).SetId(this));
			Sequence s4 = sequence;
			Transform transform4 = base.transform;
			Vector3 localPosition4 = base.transform.localPosition;
			s4.Append(transform4.DOLocalMoveX(localPosition4.x, mxxMoveTime).SetEase(Ease.OutBounce).SetId(this));
			sequence.Join(GenerateTweenLevelUp());
			return sequence;
		}

		private Tween GenerateTweenMoveNormal()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			Sequence s = sequence;
			Transform transform = base.transform;
			Vector3 localPosition = base.transform.localPosition;
			s.Append(transform.DOLocalMoveX(localPosition.x + (float)xMovePos, xMoveTime).SetEase(xMoveEase).SetId(this));
			Sequence s2 = sequence;
			Transform transform2 = base.transform;
			Vector3 localPosition2 = base.transform.localPosition;
			s2.Append(transform2.DOLocalMoveX(localPosition2.x, mxMoveTime).SetEase(Ease.OutBounce).SetId(this));
			return sequence;
		}

		private Tween GenerateTweenLevelUp()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			mTexture_LevelUp.alpha = 0f;
			mTexture_LevelUp.transform.localPosition = mVector3_Show_From;
			Tween t = DOVirtual.Float(1f, 0f, slotTime, delegate(float alpha)
			{
				mTexture_LevelUp.alpha = alpha;
			}).SetDelay(delay).SetId(this);
			sequence.Append(mTexture_LevelUp.transform.DOLocalMove(mVector3_Show_To, slotTime).SetId(this));
			sequence.Join(t);
			sequence.OnPlay(delegate
			{
				mTexture_LevelUp.alpha = 1f;
			});
			return sequence;
		}

		public void Initialize(ShipModel model)
		{
			Model = model;
			int texNum = (!Model.IsDamaged()) ? 1 : 2;
			mTexture_Ship.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Ship);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_LevelUp);
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			Model = null;
		}
	}
}
