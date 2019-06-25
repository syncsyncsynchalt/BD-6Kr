using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class ProdRebellionAreaCheck : MonoBehaviour
	{
		private StrategyCamera strategyCamera;

		[SerializeField]
		private GameObject ArrowPrefab;

		private RebellionArrow ArrowInstance;

		private UITexture EnemyTex;

		private UISprite FromTile;

		private UISprite TargetTile;

		private Vector2 offset;

		private Vector2 stoppos;

		private Vector2 endpos;

		private Vector2 camerapos;

		private void Start()
		{
			strategyCamera = StrategyTaskManager.GetStrategyTop().strategyCamera;
			EnemyTex = ((Component)base.transform.FindChild("Enemy")).GetComponent<UITexture>();
			EnemyTex.mainTexture = ShipUtils.LoadTexture(512, 9);
		}

		public void Play(int fromAreaNo, int targetAreaNo, Action OnFinish)
		{
			StartCoroutine(StartAnimation(fromAreaNo, targetAreaNo, OnFinish));
		}

		private IEnumerator StartAnimation(int fromAreaNo, int targetAreaNo, Action OnFinish)
		{
			FromTile = ((fromAreaNo != -1) ? StrategyTopTaskManager.Instance.TileManager.Tiles[fromAreaNo].getSprite() : null);
			TargetTile = StrategyTopTaskManager.Instance.TileManager.Tiles[targetAreaNo].getSprite();
			CameraMove(targetAreaNo);
			TargetTile.color = Color.red;
			yield return new WaitForSeconds(0.5f);
			ArrowAnimation(fromAreaNo, targetAreaNo);
			StartCoroutine(EnemyCutIn());
			yield return new WaitForSeconds(2.5f);
			ArrowInstance.EndAnimation();
			OnFinish?.Invoke();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void CameraMove(int targetAreaNo)
		{
			strategyCamera.MoveToTargetTile(targetAreaNo);
		}

		private void ArrowAnimation(int fromAreaNo, int targetAreaNo)
		{
			ArrowInstance = Util.Instantiate(ArrowPrefab, StrategyTaskManager.GetMapRoot().gameObject).GetComponent<RebellionArrow>();
			Vector3 fromTile = (!(FromTile != null) || fromAreaNo == 15 || fromAreaNo == 16 || fromAreaNo == 17) ? TargetTile.transform.parent.TransformPoint(TargetTile.transform.localPosition + new Vector3(185f, -106f)) : FromTile.transform.position;
			ArrowInstance.StartAnimation(fromTile, TargetTile.transform.position);
		}

		private IEnumerator EnemyCutIn()
		{
			offset = new Vector2(200f, -182f);
			stoppos = new Vector2(420f, -182f);
			endpos = new Vector2(1120f, -182f);
			camerapos = strategyCamera.transform.localPosition;
			Vector2 StartPos = offset + camerapos;
			stoppos += camerapos;
			endpos += camerapos;
			EnemyTex.transform.localPosition = StartPos;
			EnemyTex.alpha = 0f;
			SoundUtils.PlaySE(SEFIleInfos.BattleCenterLineOpen);
			TweenAlpha.Begin(EnemyTex.gameObject, 0.2f, 1f);
			TweenPosition.Begin(EnemyTex.gameObject, 0.3f, stoppos);
			yield return new WaitForSeconds(0.8f);
			TweenAlpha.Begin(EnemyTex.gameObject, 0.5f, 0f);
			TweenPosition.Begin(EnemyTex.gameObject, 2f, endpos);
		}
	}
}
