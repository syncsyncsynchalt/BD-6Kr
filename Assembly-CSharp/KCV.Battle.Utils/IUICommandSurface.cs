using LT.Tweening;

namespace KCV.Battle.Utils
{
	public interface IUICommandSurface
	{
		int index
		{
			get;
		}

		UIWidget widget
		{
			get;
		}

		LTDescr Magnify();

		LTDescr Reduction();
	}
}
