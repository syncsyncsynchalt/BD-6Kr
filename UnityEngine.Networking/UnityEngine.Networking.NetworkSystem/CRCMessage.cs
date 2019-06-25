namespace UnityEngine.Networking.NetworkSystem
{
	internal class CRCMessage : MessageBase
	{
		public CRCMessageEntry[] scripts;

		public override void Deserialize(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			scripts = new CRCMessageEntry[num];
			for (int i = 0; i < scripts.Length; i++)
			{
				CRCMessageEntry cRCMessageEntry = default(CRCMessageEntry);
				cRCMessageEntry.name = reader.ReadString();
				cRCMessageEntry.channel = reader.ReadByte();
				scripts[i] = cRCMessageEntry;
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((ushort)scripts.Length);
			CRCMessageEntry[] array = scripts;
			for (int i = 0; i < array.Length; i++)
			{
				CRCMessageEntry cRCMessageEntry = array[i];
				writer.Write(cRCMessageEntry.name);
				writer.Write(cRCMessageEntry.channel);
			}
		}
	}
}
