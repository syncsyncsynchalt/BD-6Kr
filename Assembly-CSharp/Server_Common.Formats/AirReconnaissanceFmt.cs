using Common.Enum;

namespace Server_Common.Formats
{
	public class AirReconnaissanceFmt
	{
		public MapAirReconnaissanceKind AirKind;

		public MissionResultKinds SearchResult;

		public AirReconnaissanceFmt(MapAirReconnaissanceKind kind, MissionResultKinds result)
		{
			AirKind = kind;
			SearchResult = result;
		}
	}
}
