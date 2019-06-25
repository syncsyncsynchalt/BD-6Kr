using Common.Enum;
using Server_Common.Formats;

namespace local.models
{
	public class MapEventAirReconnaissanceModel : MapEventItemModel
	{
		private MapAirReconnaissanceKind _aircraft_type;

		private MissionResultKinds _result;

		public MapAirReconnaissanceKind AircraftType => _aircraft_type;

		public MissionResultKinds SearchResult => _result;

		public MapEventAirReconnaissanceModel(MapItemGetFmt fmt, AirReconnaissanceFmt fmt2)
			: base(fmt)
		{
			_aircraft_type = fmt2.AirKind;
			_result = fmt2.SearchResult;
		}

		public override string ToString()
		{
			return $"[航空偵察] {base.ToString()} 偵察機:{AircraftType} 結果:{SearchResult}";
		}
	}
}
