using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public abstract class UIFurniture : MonoBehaviour
	{
		public class UIFurnitureModel
		{
			private FurnitureModel mFurnitureModel;

			private DeckModel mDeckModel;

			public UIFurnitureModel(FurnitureModel furnitureModel, DeckModel deckModel)
			{
				mFurnitureModel = furnitureModel;
				mDeckModel = deckModel;
			}

			public DeckModel GetDeck()
			{
				return mDeckModel;
			}

			public DateTime GetDateTime()
			{
				return DateTime.UtcNow.ToLocalTime();
			}

			public FurnitureModel GetFurnitureModel()
			{
				return mFurnitureModel;
			}
		}

		private const string FURNITURE_TEXTURE_PATH = "Textures/Furnitures/{0}/{1}";

		protected UIFurnitureModel mFurnitureModel;

		private void Awake()
		{
			OnAwake();
		}

		private void Start()
		{
			OnStart();
		}

		private void OnDestroy()
		{
			mFurnitureModel = null;
			OnDestroyEvent();
		}

		private void Update()
		{
			OnUpdate();
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnDestroyEvent()
		{
		}

		public void Initialize(UIFurnitureModel uiFurnitureModel)
		{
			mFurnitureModel = uiFurnitureModel;
			OnInitialize(uiFurnitureModel);
		}

		protected virtual void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
		}

		public static Texture LoadTexture(FurnitureModel furnitureModel)
		{
			string path = "Textures/Furnitures/" + FurnitureTypeToString(furnitureModel.Type) + "/" + (furnitureModel.NoInType + 1);
			return Resources.Load(path) as Texture;
		}

		private static string FurnitureTypeToString(FurnitureKinds furnitureType)
		{
			string result = string.Empty;
			switch (furnitureType)
			{
			case FurnitureKinds.Chest:
				result = "Chest";
				break;
			case FurnitureKinds.Desk:
				result = "Desk";
				break;
			case FurnitureKinds.Floor:
				result = "Floor";
				break;
			case FurnitureKinds.Hangings:
				result = "Hangings";
				break;
			case FurnitureKinds.Wall:
				result = "Wall";
				break;
			case FurnitureKinds.Window:
				result = "Window";
				break;
			}
			return result;
		}
	}
}
