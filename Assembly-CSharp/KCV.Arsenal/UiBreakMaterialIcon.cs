using UnityEngine;

namespace KCV.Arsenal
{
	public class UiBreakMaterialIcon : MonoBehaviour
	{
		[SerializeField]
		private Animation _anim;

		private string[] animeType;

		public void init()
		{
			base.transform.gameObject.SetActive(false);
			animeType = new string[5];
			animeType[0] = "ShipMaterial1";
			animeType[1] = "ShipMaterial2";
			animeType[2] = "ShipMaterial3";
			animeType[3] = "ShipMaterial4";
			animeType[4] = "ShipMaterial5";
			_anim = GetComponent<Animation>();
			_anim.Stop();
		}

		public void startAnimation()
		{
			base.transform.gameObject.SetActive(true);
			changeItems();
			_anim.Stop();
			_anim.Play(animeType[Random.Range(0, 5)]);
		}

		public void endAnimation()
		{
			_anim.Stop();
			base.transform.gameObject.SetActive(false);
		}

		public void compMaterialAnimation()
		{
			changeItems();
			_anim.Stop();
			_anim.Play(animeType[Random.Range(0, 5)]);
		}

		public void changeItems()
		{
			((Component)_anim).GetComponent<UISprite>().spriteName = "icon2_m" + Random.Range(1, 5);
		}

		private void OnDestroy()
		{
			_anim = null;
			animeType = null;
		}
	}
}
