using System.Collections;

namespace ModeProc
{
	public class Mode
	{
		public delegate void ModeRun();

		public delegate IEnumerator ModeChange();

		public string Name;

		public int ModeNo;

		public ModeRun Run;

		public ModeChange Enter;

		public ModeChange Exit;

		public Mode(ModeRun run, ModeChange enter, ModeChange exit)
		{
			Run = run;
			Enter = enter;
			Exit = exit;
		}
	}
}
