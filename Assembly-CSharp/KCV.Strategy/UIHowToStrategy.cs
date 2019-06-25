using local.models;
using UnityEngine;

namespace KCV.Strategy
{
	public class UIHowToStrategy : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl StickKey;

		private float time;

		private bool isShow;

		private bool isForce;

		[SerializeField]
		private Transform Items1;

		[SerializeField]
		private Transform Items2;

		[SerializeField]
		private GameObject FooterMenu;

		private static readonly Vector3 ShowPos = new Vector3(-482f, -258f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-482f, -291f, 0f);

		private static readonly Vector3 FooterShowPos = new Vector3(-485f, -214f, 0f);

		private static readonly Vector3 FooterHidePos = new Vector3(-485f, -242f, 0f);

		private static readonly Vector3 ShowPos2 = new Vector3(-3487f, -40f, 0f);

		private static readonly Vector3 HidePos2 = new Vector3(-3487f, -70f, 0f);

		private Vector3 NextPos;

		private SettingModel model;

		private Coroutine cor;

		private void Awake()
		{
			base.transform.localPositionY(-291f);
			model = new SettingModel();
		}

		private void Update()
		{
			if (key == null || !key.IsRun)
			{
				return;
			}
			time += Time.deltaTime;
			if (key.IsAnyKey || StickKey.IsUpdateIndex)
			{
				time = 0f;
				if (isShow)
				{
					Hide();
				}
			}
			else if (5f < time && !isShow)
			{
				Show();
			}
		}

		public void SetKeyController(KeyControl key, KeyControl stickkey)
		{
			this.key = key;
			StickKey = stickkey;
			if (key == null && isShow)
			{
				Hide();
			}
		}

		public void isForceShow()
		{
			isForce = true;
			base.transform.localPosition = HidePos2;
			NextPos = HidePos2;
			Show();
		}

		public void isForceHide()
		{
			isForce = false;
			Hide();
		}

		public void Show()
		{
			if (model.GuideDisplay)
			{
				Vector3 vector = (!isForce) ? ShowPos : ShowPos2;
				Reset(vector);
				Util.MoveTo(base.gameObject, 0.4f, vector, iTween.EaseType.easeInSine);
				Util.MoveTo(FooterMenu, 0.4f, FooterShowPos, iTween.EaseType.easeInSine);
				isShow = true;
				if (cor != null)
				{
					StopCoroutine(cor);
				}
			}
		}

		public void Hide()
		{
			if (!isForce && isShow)
			{
				Vector3 vector = (!(NextPos == ShowPos2)) ? HidePos : HidePos2;
				Reset(vector);
				Util.MoveTo(base.gameObject, 0.4f, vector, iTween.EaseType.easeInSine);
				Util.MoveTo(FooterMenu, 0.4f, FooterHidePos, iTween.EaseType.easeInSine);
				isShow = false;
				if (cor != null)
				{
					StopCoroutine(cor);
				}
			}
		}

		private void Reset(Vector3 NewNextPos)
		{
			iTween.Stop(base.gameObject);
			if (NewNextPos == ShowPos && NextPos != HidePos)
			{
				base.transform.localPosition = HidePos;
			}
			else if (NewNextPos == ShowPos2 && NextPos != HidePos2)
			{
				base.transform.localPosition = HidePos2;
			}
			NextPos = NewNextPos;
			if (NextPos == ShowPos || NextPos == ShowPos2)
			{
				Items1.SetActive(!isForce);
				Items2.SetActive(isForce);
			}
		}

		private void OnDestroy()
		{
			key = null;
			StickKey = null;
			Items1 = null;
			Items2 = null;
			FooterMenu = null;
			model = null;
			cor = null;
		}
	}
}
