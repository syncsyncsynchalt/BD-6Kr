using System.Collections;

public static class RetentionData
{
	private static Hashtable _hData;

	public static void SetData(Hashtable data)
	{
		if (data != null)
		{
			if (_hData != null)
			{
				_hData.Clear();
			}
			else
			{
				_hData = new Hashtable();
			}
			_hData = data;
		}
	}

	public static Hashtable GetData()
	{
		if (_hData != null)
		{
			return _hData;
		}
		return null;
	}

	public static void Release()
	{
		if (_hData != null)
		{
			_hData.Clear();
		}
		_hData = null;
	}
}
