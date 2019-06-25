namespace UnityEngine.Networking.NetworkSystem
{
	public class PeerInfoMessage : MessageBase
	{
		public int connectionId;

		public string address;

		public int port;

		public bool isHost;

		public bool isYou;

		public override void Deserialize(NetworkReader reader)
		{
			connectionId = (int)reader.ReadPackedUInt32();
			address = reader.ReadString();
			port = (int)reader.ReadPackedUInt32();
			isHost = reader.ReadBoolean();
			isYou = reader.ReadBoolean();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)connectionId);
			writer.Write(address);
			writer.WritePackedUInt32((uint)port);
			writer.Write(isHost);
			writer.Write(isYou);
		}
	}
}
