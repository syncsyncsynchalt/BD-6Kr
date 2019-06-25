using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
{
	public enum ShipTexType
	{
		BANNER = 1,
		BANNER_D,
		CARD,
		CARD_D,
		CHARA_HALF,
		CHARA_HALF_D,
		CHARA_SMALL,
		CHARA_SMALL_D,
		CHARA,
		CHARA_D,
		CHARA_FACE,
		CHARA_FACE_D,
		CHARA_NAME,
		BANNER_LONG,
		BANNER_LONG_D
	}

	public abstract class BaseResource
	{
		public abstract void ClearAll();
	}

	[Serializable]
	public class ShipTextureResources : BaseResource
	{
		[Serializable]
		public class SingleShipDictionary : SerializableDictionary<int, Texture2D>
		{
		}

		[Serializable]
		public class ShipsTextureDictionary : SerializableDictionary<int, SingleShipDictionary>
		{
		}

		[SerializeField]
		private ShipsTextureDictionary _dicShipsTexture;

		public ShipTextureResources()
		{
			_dicShipsTexture = new ShipsTextureDictionary();
		}

		public Texture2D Load(int shipID, int texNum)
		{
			Texture2D texture2D = LoadResource(shipID, texNum);
			if (texture2D == null)
			{
				DebugUtils.Warning($"Texture Load Warning : Ship Texture is Not Found (ShipID:{shipID} - TexNum:{texNum})");
				return null;
			}
			return texture2D;
		}

		public override void ClearAll()
		{
			_dicShipsTexture.Clear();
		}

		public void Clear(ShipModel_Battle[] shipsModel)
		{
			for (int i = 0; i < shipsModel.Length; i++)
			{
				if (_dicShipsTexture.ContainsKey(shipsModel[i].MstId))
				{
					_dicShipsTexture[shipsModel[i].MstId].Clear();
				}
			}
		}

		private Texture2D LoadResource(int shipID, int texNum)
		{
			return LoadResourceOrAssetBundle($"Textures/Ships/{shipID}/{texNum}") as Texture2D;
		}
	}

	[Serializable]
	public class SlotItemTextureResource : BaseResource
	{
		[Serializable]
		public class SingleSlotItemDictionary : SerializableDictionary<int, Texture2D>
		{
		}

		[Serializable]
		public class SlotItemsTextureDictionary : SerializableDictionary<int, SingleSlotItemDictionary>
		{
		}

		[SerializeField]
		private SlotItemsTextureDictionary _dicSlotItemTexture;

		public SlotItemTextureResource()
		{
			_dicSlotItemTexture = new SlotItemsTextureDictionary();
		}

		public Texture2D Load(int slotItemID, int texNum)
		{
			Texture2D texture2D = _loadResource(slotItemID, texNum);
			if (texture2D == null)
			{
				return null;
			}
			return texture2D;
		}

		public override void ClearAll()
		{
			_dicSlotItemTexture.Clear();
		}

		private Texture2D _loadResource(int slotItemID, int texnum)
		{
			return Resources.Load($"Textures/SlotItems/{slotItemID}/{texnum}") as Texture2D;
		}
	}

	[Serializable]
	public class ShipVoiceResources : BaseResource
	{
		[Serializable]
		public class SingleShipDictionary : SerializableDictionary<int, AudioClip>
		{
		}

		[Serializable]
		public class ShipsVoiceDictionary : SerializableDictionary<int, SingleShipDictionary>
		{
		}

		[SerializeField]
		private ShipsVoiceDictionary _dicShipsVoice;

		public ShipVoiceResources()
		{
			_dicShipsVoice = new ShipsVoiceDictionary();
		}

		public AudioClip Load(int shipID, int voiceNum)
		{
			AudioClip val = LoadResource(shipID, voiceNum);
			if ((UnityEngine.Object)val == null)
			{
				return null;
			}
			return val;
		}

		public override void ClearAll()
		{
			_dicShipsVoice.Clear();
		}

		private AudioClip LoadResource(int shipID, int voiceNum)
		{
			return Resources.Load($"Sounds/Voice/kc{shipID}/{voiceNum}") as AudioClip;
		}
	}

	[Serializable]
	public class FurnitureResource : BaseResource
	{
		[Serializable]
		public class SingleFurnitureDictionary : SerializableDictionary<int, Texture2D>
		{
		}

		[Serializable]
		public class FurnituresTextureDictionary : SerializableDictionary<FurnitureKinds, SingleFurnitureDictionary>
		{
		}

		[SerializeField]
		private FurnituresTextureDictionary _dicFurnitureTexture;

		public FurnitureResource()
		{
			_dicFurnitureTexture = new FurnituresTextureDictionary();
		}

		public Texture2D LoadInteriorStoreFurniture(FurnitureKinds iType, int furnitureNumber)
		{
			return Resources.Load($"Textures/InteriorStore/Furnitures/{iType}/{furnitureNumber}") as Texture2D;
		}

		public Texture2D Load(FurnitureKinds iType, int furnitureNum)
		{
			if (_dicFurnitureTexture.ContainsKey(iType))
			{
				if (_dicFurnitureTexture[iType].ContainsKey(furnitureNum))
				{
					if (_dicFurnitureTexture[iType][furnitureNum] != null)
					{
						return _dicFurnitureTexture[iType][furnitureNum];
					}
					Texture2D texture2D = LoadResource(iType, furnitureNum);
					if (texture2D == null)
					{
						return null;
					}
					_dicFurnitureTexture[iType][furnitureNum] = texture2D;
				}
				else
				{
					Texture2D texture2D = LoadResource(iType, furnitureNum);
					if (texture2D == null)
					{
						return null;
					}
					_dicFurnitureTexture[iType].Add(furnitureNum, texture2D);
				}
			}
			else
			{
				Texture2D texture2D = LoadResource(iType, furnitureNum);
				if (texture2D == null)
				{
					return null;
				}
				SingleFurnitureDictionary singleFurnitureDictionary = new SingleFurnitureDictionary();
				singleFurnitureDictionary.Add(furnitureNum, texture2D);
				_dicFurnitureTexture.Add(iType, singleFurnitureDictionary);
			}
			return _dicFurnitureTexture[iType][furnitureNum];
		}

		public override void ClearAll()
		{
			_dicFurnitureTexture.Clear();
		}

		private Texture2D LoadResource(FurnitureKinds iType, int furNum)
		{
			return Resources.Load($"Textures/Furnitures/{iType}/{furNum}") as Texture2D;
		}
	}

	[Serializable]
	public class ShaderResources : BaseResource
	{
		private List<string> SHADER_NAME = new List<string>
		{
			"CC2/Grayscale",
			"CC2/Transparent Colored",
			"Skybox/6 Sided",
			"FX/Water",
			"FX/Flare",
			"Particles/Additive",
			"Particles/Alpha Blended",
			"Sprites/Default",
			"Unlit/Transparent Packed",
			"Unlit/Transparent Colored",
			"KCV/Water"
		};

		private List<Shader> _listShader;

		private bool _isInit;

		public List<Shader> shaderList => _listShader;

		public void Load()
		{
			if (!_isInit)
			{
				_listShader = new List<Shader>();
				foreach (string item in SHADER_NAME)
				{
					_listShader.Add(Shader.Find(item));
				}
				_isInit = true;
			}
		}

		public override void ClearAll()
		{
			_isInit = false;
			SHADER_NAME.Clear();
			SHADER_NAME = null;
			_listShader.Clear();
			_listShader = null;
		}
	}

	private const string SHIP_TEXTURE_PATH = "Textures/Ships/{0}/{1}";

	private const string SHIP_VOICE_PATH = "Sounds/Voice/kc{0}/{1}";

	private const string SLOTITEM_TEXTURE_PATH = "Textures/SlotItems/{0}/{1}";

	private const string FURNITURE_TEXTURE_PATH = "Textures/Furnitures/{0}/{1}";

	private const string STRATEGY_MAP_STAGE_PATH = "Textures/Strategy/MapSelectGraph/stage{0}-{1}";

	private const string FURNITURE_STORE_TEXTURE_PATH = "Textures/InteriorStore/Furnitures/{0}/{1}";

	private const string SHIP_TYPE_ICON_TEXTURE = "Textures/Common/Ship/TypeIcon/{0}-{1}";

	private const string BGM_PATH = "Sounds/BGM/{0}";

	private const int SHIP_CTRL_MAX = 0;

	private const int SLOTITEM_CTRL_MAX = 0;

	public static readonly Dictionary<int, Vector2> SHIP_TEXTURE_SIZE = new Dictionary<int, Vector2>
	{
		{
			1,
			new Vector2(360f, 90f)
		},
		{
			2,
			new Vector2(360f, 90f)
		},
		{
			3,
			new Vector2(218f, 300f)
		},
		{
			4,
			new Vector2(218f, 300f)
		}
	};

	public static readonly Dictionary<int, Vector2> SLOTITEM_TEXTURE_SIZE = new Dictionary<int, Vector2>
	{
		{
			1,
			new Vector2(260f, 260f)
		},
		{
			2,
			new Vector2(287f, 430f)
		},
		{
			3,
			new Vector2(287f, 430f)
		},
		{
			4,
			new Vector2(287f, 430f)
		},
		{
			6,
			new Vector2(300f, 300f)
		},
		{
			7,
			new Vector2(300f, 300f)
		}
	};

	[SerializeField]
	private ShipVoiceResources _clsShipVoice = new ShipVoiceResources();

	[SerializeField]
	private ShipTextureResources _clsShipTexture = new ShipTextureResources();

	[SerializeField]
	private SlotItemTextureResource _clsSlotItemTexture = new SlotItemTextureResource();

	[SerializeField]
	private FurnitureResource _clsFurniture = new FurnitureResource();

	[SerializeField]
	private ShaderResources _clsShader = new ShaderResources();

	public ShipVoiceResources ShipVoice => _clsShipVoice;

	public ShipTextureResources ShipTexture => _clsShipTexture;

	public SlotItemTextureResource SlotItemTexture => _clsSlotItemTexture;

	public FurnitureResource Furniture => _clsFurniture;

	public ShaderResources shader => _clsShader;

	public static ResourceRequest LoadStageCoverAsync(int areaId, int mapId)
	{
		return Resources.LoadAsync($"Textures/Strategy/MapSelectGraph/stage{areaId}-{mapId}", typeof(Texture2D));
	}

	public static Texture2D LoadStageCover(int areaId, int mapId)
	{
		return LoadResourceOrAssetBundle($"Textures/Strategy/MapSelectGraph/stage{areaId}-{mapId}") as Texture2D;
	}

	public static Texture LoadShipTypeIcon(ShipModelMst shipModel)
	{
		int num;
		switch (shipModel.Rare)
		{
		case 7:
		case 8:
			num = 4;
			break;
		case 6:
			num = 3;
			break;
		case 4:
		case 5:
			num = 2;
			break;
		default:
			num = 1;
			break;
		}
		int shipType = shipModel.ShipType;
		int num2 = (shipType != 8) ? shipModel.ShipType : 9;
		if (num == -1)
		{
			return null;
		}
		if (shipModel == null)
		{
			return null;
		}
		return Resources.Load($"Textures/Common/Ship/TypeIcon/{num2}-{num}") as Texture;
	}

	protected override void Awake()
	{
		base.Awake();
		_clsShader.Load();
	}

	private void OnDestroy()
	{
		AllRelease();
	}

	public void AllRelease()
	{
		_clsShipTexture.ClearAll();
		_clsFurniture.ClearAll();
		_clsSlotItemTexture.ClearAll();
		_clsShipVoice.ClearAll();
		_clsShader.ClearAll();
	}

	public static UnityEngine.Object LoadResourceOrAssetBundle(string filePath)
	{
		if (File.Exists(ABDataPath.AssetBundlePath + "/" + filePath + ".unity3d"))
		{
			AssetBundle val = AssetBundle.CreateFromFile(ABDataPath.AssetBundlePath + "/" + filePath + ".unity3d");
			string fileName = Path.GetFileName(filePath);
			UnityEngine.Object @object = val.LoadAsset(fileName);
			if (@object == null)
			{
				@object = val.LoadAsset(fileName + ".bytes");
			}
			val.Unload(false);
			return @object;
		}
		return Resources.Load(filePath);
	}
}
