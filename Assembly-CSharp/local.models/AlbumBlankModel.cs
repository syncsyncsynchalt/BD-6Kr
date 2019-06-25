namespace local.models
{
	public class AlbumBlankModel : IAlbumModel
	{
		private int _id;

		public int Id => _id;

		public AlbumBlankModel(int id)
		{
			_id = id;
		}

		public override string ToString()
		{
			return ToString(detail: false);
		}

		public string ToString(bool detail)
		{
			return $"図鑑ID:{Id} --- ";
		}
	}
}
