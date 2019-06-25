using local.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Resource
{
	internal class ShipResource
	{
		private StateType mStateType;

		private TextureType mTextureType;

		private Dictionary<int, Texture> mResources;

		public ShipResource(TextureType textureType, StateType stateType)
		{
			mStateType = stateType;
			mTextureType = textureType;
			mResources = new Dictionary<int, Texture>();
		}

		public IEnumerator GenerateLoadAsync(ShipModel[] shipModels)
		{
			foreach (ShipModel shipModel in shipModels)
			{
				string loadPath = GenerateResourcePath(shipModel.GetGraphicsMstId(), mTextureType, mStateType);
				ResourceRequest request = Resources.LoadAsync<Texture>(loadPath);
				if (!request.isDone)
				{
					yield return request;
				}
				SetResource(shipModel.GetGraphicsMstId(), request.asset as Texture);
			}
		}

		public Texture GetResource(int masterId)
		{
			Texture value = null;
			if (mResources.TryGetValue(masterId, out value))
			{
				return value;
			}
			string path = GenerateResourcePath(masterId, mTextureType, mStateType);
			Texture texture = Resources.Load<Texture>(path);
			mResources.Add(masterId, texture);
			return texture;
		}

		public void ReleaseTextures()
		{
			foreach (Texture value in mResources.Values)
			{
				Resources.UnloadAsset(value);
			}
			foreach (int key in mResources.Keys)
			{
				mResources.Remove(key);
			}
			mResources.Clear();
		}

		private void SetResource(int masterId, Texture texture)
		{
			if (!mResources.ContainsKey(masterId))
			{
				mResources.Add(masterId, texture);
			}
		}

		private static string GenerateResourcePath(int masterId, TextureType textureType, StateType stateType)
		{
			int num = FindResourceNo(textureType, stateType);
			return $"Textures/Ships/{masterId}/{num}";
		}

		private static int FindResourceNo(TextureType textureType, StateType stateType)
		{
			switch (textureType)
			{
			case TextureType.Banner:
				switch (stateType)
				{
				case StateType.Normal:
					return 1;
				case StateType.Damaged:
					return 2;
				}
				break;
			case TextureType.Card:
				switch (stateType)
				{
				case StateType.Normal:
					return 3;
				case StateType.Damaged:
					return 4;
				}
				break;
			case TextureType.Full:
				switch (stateType)
				{
				case StateType.Normal:
					return 9;
				case StateType.Damaged:
					return 10;
				}
				break;
			}
			return -1;
		}
	}
}
