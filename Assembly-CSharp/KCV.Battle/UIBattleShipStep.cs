using UnityEngine;

namespace KCV.Battle
{
	public class UIBattleShipStep : MonoBehaviour
	{
		[SerializeField]
		private Animation _anim;

		private void Awake()
		{
			if ((Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			_anim.playAutomatically = false;
			_anim.Stop();
			base.transform.localScale = new Vector3(500f, 500f, 0f);
		}

		private void OnDestroy()
		{
		}

		public void Play()
		{
			_anim.Play("BattleShipStep");
		}

		private void changeOffset(int num)
		{
			switch (num)
			{
			case 0:
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0.5f);
				break;
			case 1:
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
				break;
			case 2:
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0f);
				break;
			case 3:
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0f);
				break;
			}
		}
	}
}
