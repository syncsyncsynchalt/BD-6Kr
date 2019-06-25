using Server_Common.Formats;

namespace local.models
{
	public abstract class PhaseResultModel
	{
		protected TurnWorkResult _data;

		public PhaseResultModel(TurnWorkResult data)
		{
			_data = data;
		}
	}
}
