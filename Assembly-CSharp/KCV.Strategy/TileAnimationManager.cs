using Common.Enum;
using local.models;
using Server_Common.Formats;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationManager : MonoBehaviour
	{
		private UITexture textBG;

		private UITexture text;

		private Camera cam;

		private GameObject tankInit;

		private GameObject[] tankers;

		private GameObject[] tiles;

		private UISprite[] tileSprites;

		private TileAnimationAttack taa;

		private bool waitingForTiles;

		private bool attacking;

		private bool on;

		private float timer;

		private int curTile;

		private bool tankerAnim;

		[SerializeField]
		private GameObject tileAnimPrefab;

		private GameObject tileAnimInstance;

		public bool isFinished;

		private void Start()
		{
			tileAnimInstance = Object.Instantiate(tileAnimPrefab);
			tileAnimInstance.name = "TileAnimations";
			tileAnimInstance.transform.parent = StrategyTopTaskManager.Instance.UIModel.OverView;
			tileAnimInstance.transform.localScale = Vector3.one;
			tileAnimInstance.transform.localPosition = Vector3.zero;
			cam = StrategyTopTaskManager.Instance.UIModel.MapCamera.myCamera;
			waitingForTiles = true;
			attacking = false;
			on = false;
			isFinished = false;
			curTile = 0;
			tankerAnim = false;
			textBG = GameObject.Find("/StrategyTaskManager/OverView/TileAnimations/TextBG").GetComponent<UITexture>();
			textBG.transform.localScale = new Vector3(1f, 0f, 1f);
			textBG.alpha = 0f;
			text = GameObject.Find("/StrategyTaskManager/OverView/TileAnimations/Text").GetComponent<UITexture>();
			taa = GameObject.Find("/StrategyTaskManager/Map Root/TileAnimationAttack").GetComponent<TileAnimationAttack>();
			taa.SetActive(isActive: false);
			tileAnimInstance.SetActive(false);
			tiles = new GameObject[17];
			GetTiles();
		}

		public void Initialize(RadingResultData d, MapAreaModel m, bool isFirst)
		{
			if (d == null)
			{
				isFinished = true;
				return;
			}
			curTile = d.AreaId - 1;
			taa.SetActive(isActive: true);
			tileAnimInstance.SetActive(true);
			taa.Initialize(d, m);
			StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(delegate
			{
				StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(d.AreaId);
				StartAnimation(m.GetEscortDeck().GetFlagShip() != null, d.AttackKind, isFirst);
			}, isCharacterExit: true, isPanelOff: true);
		}

		public void StartAnimation(bool friendly, RadingKind type, bool isFirst)
		{
			float num = 0f;
			if (isFirst)
			{
				textBG.alpha = 1f;
				iTween.ScaleTo(textBG.gameObject, iTween.Hash("scale", Vector3.one, "time", 0.5f, "easeType", iTween.EaseType.easeOutQuad));
				iTween.MoveTo(text.gameObject, iTween.Hash("position", new Vector3(0f, 0f, 0f), "islocal", true, "time", 1, "delay", 0.4f, "easeType", iTween.EaseType.easeOutExpo));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.5f, "delay", 0.4f, "onupdate", "TextAlpha", "onupdatetarget", base.gameObject));
				iTween.MoveTo(text.gameObject, iTween.Hash("position", new Vector3(680f, 0f, 0f), "islocal", true, "time", 1, "delay", 1.4f, "easeType", iTween.EaseType.easeInExpo));
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "delay", 1.9f, "onupdate", "TextAlpha", "onupdatetarget", base.gameObject));
				iTween.ScaleTo(textBG.gameObject, iTween.Hash("scale", new Vector3(1f, 0f, 1f), "time", 0.5f, "delay", 2, "easeType", iTween.EaseType.easeInQuad, "oncomplete", "TextBGAlphaOff", "oncompletetarget", base.gameObject));
				num = 2.4f;
			}
			StartCoroutine(StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(curTile + 1, 1f));
			iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0.5f, "time", 0.2f, "delay", num, "onupdate", "TileColor", "onupdatetarget", base.gameObject));
			this.DelayAction(num, delegate
			{
				StartCoroutine(Attack(3.4f, friendly, type));
			});
		}

		public void TextAlpha(float f)
		{
			text.alpha = f;
		}

		public void TextBGAlpha(float f)
		{
			textBG.alpha = f;
		}

		public void TextBGAlphaOff()
		{
			textBG.alpha = 0f;
		}

		public void TileColor(float f)
		{
			tileSprites[curTile].color = new Color(1f, f, f, 1f);
		}

		public IEnumerator Attack(float delay, bool friendly, RadingKind type)
		{
			attacking = true;
			taa.transform.localPosition = tiles[curTile].transform.parent.localPosition + tiles[curTile].transform.localPosition + new Vector3(-6f, 6f, 0f);
			taa.Attack(friendly, type);
			while (!taa.isFinished)
			{
				yield return new WaitForEndOfFrame();
			}
			tileSprites[curTile].color = Color.white;
			attacking = false;
			on = false;
			text.transform.localPosition = new Vector3(-680f, 0f, 0f);
			isFinished = true;
			TweenAlpha ta = TweenAlpha.Begin(taa.gameObject, 0.2f, 0f);
			ta.onFinished.Clear();
			ta.SetOnFinished(delegate
			{
				this.taa.SetActive(isActive: false);
				this.tileAnimInstance.SetActive(false);
			});
		}

		private void GetTiles()
		{
			tileSprites = new UISprite[17];
			for (int i = 0; i < 17; i++)
			{
				tiles[i] = StrategyTopTaskManager.Instance.TileManager.Tiles[i + 1].gameObject;
				tileSprites[i] = StrategyTopTaskManager.Instance.TileManager.Tiles[i + 1].getSprite();
			}
		}
	}
}
