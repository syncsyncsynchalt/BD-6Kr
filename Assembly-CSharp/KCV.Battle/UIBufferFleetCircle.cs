using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using local.managers;
using LT.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(Animation))]
	public class UIBufferFleetCircle : InstantiateObject<UIBufferFleetCircle>
	{
		[SerializeField]
		private float _fCircleScale = 200f;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private MeshRenderer _mrBufferCircle;

		[SerializeField]
		private Texture2D _texBufferCircle;

		[SerializeField]
		private List<Texture2D> _listRippleTexture;

		[SerializeField]
		private List<MeshRenderer> _listRippleRenderer;

		private Color _cBaseColor;

		public static UIBufferFleetCircle Instantiate(UIBufferFleetCircle prefab, Transform parent, FleetType iType)
		{
			UIBufferFleetCircle uIBufferFleetCircle = InstantiateObject<UIBufferFleetCircle>.Instantiate(prefab, parent);
			uIBufferFleetCircle.transform.localScale = new Vector3(uIBufferFleetCircle._fCircleScale, 0f, uIBufferFleetCircle._fCircleScale);
			uIBufferFleetCircle.Init(iType);
			return uIBufferFleetCircle;
		}

		private bool Init(FleetType iType)
		{
			ReflectionMaterial();
			SetCircleColor(iType);
			SetCircleSize(iType);
			return true;
		}

		private void OnDestroy()
		{
			Transform meshTrans = ((Component)_mrBufferCircle).transform;
			Mem.DelMeshSafe(ref meshTrans);
			if (_listRippleRenderer != null)
			{
				_listRippleRenderer.ForEach(delegate(MeshRenderer x)
				{
					meshTrans = ((Component)x).transform;
					Mem.DelMeshSafe(ref meshTrans);
				});
			}
			Mem.Del(ref _fCircleScale);
			Mem.Del(ref _material);
			Mem.Del(ref _mrBufferCircle);
			Mem.Del(ref _texBufferCircle);
			Mem.DelListSafe(ref _listRippleTexture);
			Mem.DelListSafe(ref _listRippleRenderer);
			Mem.Del(ref _cBaseColor);
			Mem.Del(ref meshTrans);
		}

		public void PlayRipple()
		{
			GetComponent<Animation>().Play();
			base.transform.LTValue(1f, 0f, GetComponent<Animation>()["ProdBufferFleetCircleRipple"].length).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				UIBufferFleetCircle uIBufferFleetCircle = this;
				_listRippleRenderer.ForEach(delegate(MeshRenderer renderer)
				{
					renderer.material.color = new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(60f), x);
				});
			});
		}

		public void PlayFocusCircleAnimation()
		{
			Color c = _cBaseColor;
			base.transform.LTValue(_cBaseColor.a, Mathe.Rate(0f, 255f, 70f), 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				c.a = x;
				_mrBufferCircle.sharedMaterial.color = c;
			});
		}

		public void SetDefault()
		{
			_mrBufferCircle.sharedMaterial.color = _cBaseColor;
		}

		private void ReflectionMaterial()
		{
			_mrBufferCircle.material = new Material(_material);
			_mrBufferCircle.sharedMaterial.mainTexture = _texBufferCircle;
			if (Application.isPlaying)
			{
				Mem.Del(ref _texBufferCircle);
			}
			int cnt = 0;
			_listRippleRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.material = new Material(_material);
				x.sharedMaterial.mainTexture = _listRippleTexture[cnt];
				_listRippleRenderer[cnt].sharedMaterial.color = new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f));
				cnt++;
			});
			if (Application.isPlaying)
			{
				Mem.DelListSafe(ref _listRippleTexture);
			}
		}

		private void SetCircleSize(FleetType iType)
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			base.transform.localScale = Vector3.one * CalcCircleSize((iType != 0) ? battleManager.ShipCount_e : battleManager.ShipCount_f, (iType != 0) ? battleManager.FormationId_e : battleManager.FormationId_f);
		}

		private float CalcCircleSize(int nVessels, BattleFormationKinds1 iKind)
		{
			if (nVessels <= 3)
			{
				return 150f;
			}
			switch (iKind)
			{
			case BattleFormationKinds1.TanJuu:
			case BattleFormationKinds1.TanOu:
				switch (nVessels)
				{
				case 4:
					return 190f;
				case 5:
					return 230f;
				case 6:
					return 256f;
				}
				break;
			case BattleFormationKinds1.Rinkei:
				switch (nVessels)
				{
				case 4:
				case 5:
					return 150f;
				case 6:
					return 190f;
				}
				break;
			case BattleFormationKinds1.Teikei:
				switch (nVessels)
				{
				case 4:
					return 150f;
				case 5:
					return 200f;
				case 6:
					return 230f;
				}
				break;
			}
			return 150f;
		}

		private void SetCircleColor(FleetType iType)
		{
			Color color = (iType != 0) ? new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f)) : new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f));
			_mrBufferCircle.sharedMaterial.color = color;
			_cBaseColor = color;
		}
	}
}
