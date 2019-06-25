using Server_Common.Formats;
using System.Collections.Generic;

namespace local.models
{
	public class TurnResultModel : PhaseResultModel
	{
		public List<RadingResultData> RadingResult => _data.RadingResult;

		public TurnResultModel(TurnWorkResult data)
			: base(data)
		{
		}

		public override string ToString()
		{
			string str = $"[タ\u30fcン終了フェ\u30fcズ]: \n";
			str += $"=通商破壊=\n";
			if (RadingResult == null)
			{
				str += "なし";
			}
			else
			{
				for (int i = 0; i < RadingResult.Count; i++)
				{
					RadingResultData radingResultData = RadingResult[i];
					str += $"[通商破壊結果]\n海域{radingResultData.AreaId} 攻撃種別:{radingResultData.AttackKind} 輸送船{radingResultData.BeforeNum}隻(移動中の船を除く)から{radingResultData.BreakNum}隻ロスト\n";
					str += $"海上護衛艦隊の対潜/対空能力:{radingResultData.DeckAttackPow}\n";
					for (int j = 0; j < radingResultData.RadingDamage.Count; j++)
					{
						str += $"\t海上護衛艦隊(MemId:{radingResultData.RadingDamage[j].Rid})は{radingResultData.RadingDamage[j].Damage}のダメ\u30fcジ({radingResultData.RadingDamage[j].DamageState})";
					}
				}
			}
			return str;
		}
	}
}
