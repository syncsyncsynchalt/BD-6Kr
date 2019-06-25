using KCV.Utils;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiBreakAnimation : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _parShipBreak;

		[SerializeField]
		private ParticleSystem _parShipBreak2;

		[SerializeField]
		private UiBreakMaterialIcon _breakMaterial;

		[SerializeField]
		private Animation _anim;

		public bool isAnimationPlayng;

		public void init()
		{
			_breakMaterial = ((Component)base.transform.FindChild("Material1")).GetComponent<UiBreakMaterialIcon>();
			_breakMaterial.init();
			_parShipBreak = ((Component)base.transform.FindChild("Smoke")).GetComponent<ParticleSystem>();
			_parShipBreak2 = ((Component)base.transform.FindChild("Smoke2")).GetComponent<ParticleSystem>();
			((Component)_parShipBreak).SetActive(isActive: false);
			((Component)_parShipBreak2).SetActive(isActive: false);
			_anim = GetComponent<Animation>();
			changeItems();
			_anim.Stop();
		}

		public void startAnimation()
		{
			_anim.Stop();
			_anim.Play("ShipBreak");
			SoundUtils.PlaySE(SEFIleInfos.SE_015);
			_breakMaterial.startAnimation();
			((Component)_parShipBreak).SetActive(isActive: true);
			_parShipBreak.time = 0f;
			_parShipBreak.Play();
			isAnimationPlayng = true;
		}

		public void compShipBreakAnimation()
		{
			_parShipBreak.Stop();
			_parShipBreak.time = 0f;
			((Component)_parShipBreak).SetActive(isActive: false);
			_anim.Stop();
			_breakMaterial.endAnimation();
			isAnimationPlayng = false;
			ArsenalTaskManager._clsList.compBreakAnimation();
		}

		public void startItemAnimation()
		{
			_anim.Stop();
			_anim.Play("ItemBreak");
			SoundUtils.PlaySE(SEFIleInfos.SE_015);
			_breakMaterial.startAnimation();
			((Component)_parShipBreak2).SetActive(isActive: true);
			_parShipBreak2.time = 0f;
			_parShipBreak2.Play();
			isAnimationPlayng = true;
		}

		public void compItemBreakAnimation()
		{
			_parShipBreak2.Stop();
			_parShipBreak2.time = 0f;
			((Component)_parShipBreak2).SetActive(isActive: false);
			changeItems();
			_anim.Stop();
			_breakMaterial.endAnimation();
			isAnimationPlayng = false;
			ArsenalTaskManager._clsList.compBreakAnimation();
		}

		public void changeItems()
		{
			_breakMaterial.GetComponent<UISprite>().spriteName = "icon2_m" + Random.Range(1, 5);
		}

		private void OnDestroy()
		{
			_parShipBreak = null;
			_parShipBreak2 = null;
			_breakMaterial = null;
			_anim = null;
		}
	}
}
