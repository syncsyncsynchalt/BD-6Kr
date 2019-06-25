using UnityEngine;

namespace KCV.Supply
{
	public class SupplyStorage : MonoBehaviour
	{
		private int STORAGE_LENGTH = 6;

		[SerializeField]
		private UISprite _uiStorage;

		private string[] _spriteName;

		private int _fromIndex;

		private int _toIndex;

		private float _animeTimer;

		private bool _isAnimate;

		public float ChangeTime;

		public void init(int num, float changeTime)
		{
			_fromIndex = num;
			_toIndex = 0;
			_animeTimer = 0f;
			_isAnimate = false;
			ChangeTime = changeTime;
			_spriteName = new string[STORAGE_LENGTH];
			for (int i = 0; i < STORAGE_LENGTH; i++)
			{
				_spriteName[i] = "sizai_gauge" + i;
			}
			Util.FindParentToChild(ref _uiStorage, base.transform, "Storage");
			_uiStorage.spriteName = _spriteName[_fromIndex];
		}

		private void Update()
		{
			if (!_isAnimate)
			{
				return;
			}
			if (_fromIndex < _toIndex)
			{
				_animeTimer += Time.deltaTime;
				if (_animeTimer >= ChangeTime)
				{
					_fromIndex++;
					_animeTimer = 0f;
					_uiStorage.spriteName = _spriteName[_fromIndex];
				}
			}
			else
			{
				_animeTimer = 0f;
				_isAnimate = false;
			}
		}

		public void UpdateStorage(int num)
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < STORAGE_LENGTH)
				{
					if (num == num2)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			_toIndex = num2;
			_isAnimate = true;
		}

		public void Stop()
		{
			_isAnimate = false;
		}

		public void End()
		{
			_isAnimate = false;
			_fromIndex = _toIndex;
			_uiStorage.spriteName = _spriteName[_fromIndex];
		}
	}
}
