namespace UnityEngine
{
	public struct WebCamDevice
	{
		internal string m_Name;

		internal int m_Flags;

		public string name => m_Name;

		public bool isFrontFacing => (m_Flags & 1) == 1;
	}
}
