using UnityEngine;

namespace KCV.Battle
{
	public class _tami_dev_battle : MonoBehaviour
	{
		private GameObject _go;

		private Animation _ani;

		private UITexture _tex;

		private bool _startUp;

		private void Start()
		{
			_startUp = false;
			startUp();
		}

		private void startUp()
		{
			_go = GameObject.Find("ProdTorpedoCutIn");
			_ani = _go.GetComponent<Animation>();
			_tex = ((Component)_go.transform.FindChild("FriendShip/Panel/Deg30/Anchor/Object2D")).GetComponent<UITexture>();
			_tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(1, 9);
			_tex.MakePixelPerfect();
			_tex = ((Component)_go.transform.FindChild("EnemyShip/Panel/Deg30/Anchor/Object2D")).GetComponent<UITexture>();
			_tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(511, 9);
			_tex.MakePixelPerfect();
			_startUp = true;
		}

		private void Update()
		{
			if (_startUp)
			{
				if (Input.GetKey(KeyCode.Alpha1))
				{
					_ani.Play("ProdTorpedoCutIn");
				}
				if (Input.GetKey(KeyCode.Alpha2))
				{
					_ani.Play("ProdTorpedoCutInFriend");
				}
				if (Input.GetKey(KeyCode.Alpha3))
				{
					_ani.Play("ProdTorpedoCutInEnemy");
				}
			}
		}
	}
}
