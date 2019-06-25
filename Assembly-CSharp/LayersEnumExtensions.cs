public static class LayersEnumExtensions
{
	public static int IntLayer(this Generics.Layers layer)
	{
		switch (layer)
		{
		case Generics.Layers.Default:
			return 0;
		case Generics.Layers.TransparentFX:
			return 1;
		case Generics.Layers.IgnoreRaycast:
			return 2;
		case Generics.Layers.Water:
			return 3;
		case Generics.Layers.UI:
			return 5;
		case Generics.Layers.Background:
			return 8;
		case Generics.Layers.UI2D:
			return 9;
		case Generics.Layers.UI3D:
			return 10;
		case Generics.Layers.Transition:
			return 11;
		case Generics.Layers.ShipGirl:
			return 12;
		case Generics.Layers.TopMost:
			return 13;
		case Generics.Layers.CutIn:
			return 14;
		case Generics.Layers.SaveData:
			return 15;
		case Generics.Layers.Effects:
			return 16;
		case Generics.Layers.FocusDim:
			return 17;
		case Generics.Layers.UnRefrectEffects:
			return 18;
		case Generics.Layers.SplitWater:
			return 19;
		default:
			return 0;
		}
	}
}
