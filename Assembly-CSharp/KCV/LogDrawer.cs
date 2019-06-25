namespace KCV
{
	public class LogDrawer : SingletonMonoBehaviour<LogDrawer>
	{
		private UILabel myLabel;

		private int lineCount;

		protected override void Awake()
		{
			base.Awake();
			SingletonMonoBehaviour<LogDrawer>.instance = this;
			myLabel = GetComponent<UILabel>();
			myLabel.text = string.Empty;
			lineCount = 0;
		}

		private void Start()
		{
		}

		public void addDebugText(string s)
		{
			UILabel uILabel = myLabel;
			uILabel.text = uILabel.text + s + "\n";
			lineCount++;
			if (lineCount > 20)
			{
				int count = myLabel.text.IndexOf("\n", 0) + 1;
				myLabel.text = myLabel.text.Remove(0, count);
			}
		}

		public new static bool exist()
		{
			if (SingletonMonoBehaviour<LogDrawer>.instance != null)
			{
				return true;
			}
			return false;
		}
	}
}
