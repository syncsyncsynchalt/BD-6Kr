using KCV.View.Scroll;

namespace KCV.Remodel
{
	public abstract class AbstractUIRemodelListChild<T> : UIScrollListChild<T> where T : class
	{
		public override void Show()
		{
			base.Show();
			this.SetActive(isActive: true);
		}

		public override void Hide()
		{
			base.Hide();
			this.SetActive(isActive: false);
		}
	}
}
