using Server_Common.Formats;

namespace local.models
{
	public class EnemyPreActionPhaseResultModel : PhaseResultModel
	{
		public EnemyPreActionPhaseResultModel(TurnWorkResult data)
			: base(data)
		{
		}

		public override string ToString()
		{
			return $"[敵事前行動フェーズ]: ";
		}
	}
}
