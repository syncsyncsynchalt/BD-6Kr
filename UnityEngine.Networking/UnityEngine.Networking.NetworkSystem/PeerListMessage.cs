namespace UnityEngine.Networking.NetworkSystem
{
	public class PeerListMessage : MessageBase
	{
		public PeerInfoMessage[] peers;

		public override void Deserialize(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			peers = new PeerInfoMessage[num];
			for (int i = 0; i < peers.Length; i++)
			{
				PeerInfoMessage peerInfoMessage = new PeerInfoMessage();
				peerInfoMessage.Deserialize(reader);
				peers[i] = peerInfoMessage;
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((ushort)peers.Length);
			PeerInfoMessage[] array = peers;
			foreach (PeerInfoMessage peerInfoMessage in array)
			{
				peerInfoMessage.Serialize(writer);
			}
		}
	}
}
