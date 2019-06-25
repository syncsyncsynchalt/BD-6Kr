using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class ChannelBuffer : IDisposable
	{
		private const int k_MaxFreePacketCount = 512;

		private const int k_MaxPendingPacketCount = 16;

		private const int k_PacketHeaderReserveSize = 100;

		private NetworkConnection m_Connection;

		private ChannelPacket m_CurrentPacket;

		private float m_LastFlushTime;

		private byte m_ChannelId;

		private int m_MaxPacketSize;

		private bool m_IsReliable;

		private bool m_IsBroken;

		private int m_MaxPendingPacketCount;

		private List<ChannelPacket> m_PendingPackets;

		private static List<ChannelPacket> s_FreePackets;

		internal static int pendingPacketCount;

		public float maxDelay = 0.01f;

		private float m_LastBufferedMessageCountTimer = Time.time;

		private static NetworkWriter s_SendWriter = new NetworkWriter();

		private bool m_Disposed;

		public int numMsgsOut
		{
			get;
			private set;
		}

		public int numBufferedMsgsOut
		{
			get;
			private set;
		}

		public int numBytesOut
		{
			get;
			private set;
		}

		public int numMsgsIn
		{
			get;
			private set;
		}

		public int numBytesIn
		{
			get;
			private set;
		}

		public int numBufferedPerSecond
		{
			get;
			private set;
		}

		public int lastBufferedPerSecond
		{
			get;
			private set;
		}

		public ChannelBuffer(NetworkConnection conn, int bufferSize, byte cid, bool isReliable)
		{
			m_Connection = conn;
			m_MaxPacketSize = bufferSize - 100;
			m_CurrentPacket = new ChannelPacket(m_MaxPacketSize, isReliable);
			m_ChannelId = cid;
			m_MaxPendingPacketCount = 16;
			m_IsReliable = isReliable;
			if (isReliable)
			{
				m_PendingPackets = new List<ChannelPacket>();
				if (s_FreePackets == null)
				{
					s_FreePackets = new List<ChannelPacket>();
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!m_Disposed && disposing && m_PendingPackets != null)
			{
				foreach (ChannelPacket pendingPacket in m_PendingPackets)
				{
					pendingPacketCount--;
					if (s_FreePackets.Count < 512)
					{
						s_FreePackets.Add(pendingPacket);
					}
				}
				m_PendingPackets.Clear();
			}
			m_Disposed = true;
		}

		public bool SetOption(ChannelOption option, int value)
		{
			if (option == ChannelOption.MaxPendingBuffers)
			{
				if (!m_IsReliable)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Cannot set MaxPendingBuffers on unreliable channel " + m_ChannelId);
					}
					return false;
				}
				if (value < 0 || value >= 512)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Invalid MaxPendingBuffers for channel " + m_ChannelId + ". Must be greater than zero and less than " + 512);
					}
					return false;
				}
				m_MaxPendingPacketCount = value;
				return true;
			}
			return false;
		}

		public void CheckInternalBuffer()
		{
			if (Time.time - m_LastFlushTime > maxDelay && !m_CurrentPacket.IsEmpty())
			{
				SendInternalBuffer();
				m_LastFlushTime = Time.time;
			}
			if (Time.time - m_LastBufferedMessageCountTimer > 1f)
			{
				lastBufferedPerSecond = numBufferedPerSecond;
				numBufferedPerSecond = 0;
				m_LastBufferedMessageCountTimer = Time.time;
			}
		}

		public bool SendWriter(NetworkWriter writer)
		{
			return SendBytes(writer.AsArraySegment().Array, writer.AsArraySegment().Count);
		}

		public bool Send(short msgType, MessageBase msg)
		{
			s_SendWriter.StartMessage(msgType);
			msg.Serialize(s_SendWriter);
			s_SendWriter.FinishMessage();
			numMsgsOut++;
			return SendWriter(s_SendWriter);
		}

		internal bool SendBytes(byte[] bytes, int bytesToSend)
		{
			if (bytesToSend <= 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ChannelBuffer:SendBytes cannot send zero bytes");
				}
				return false;
			}
			if (bytesToSend > m_MaxPacketSize)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Failed to send big message of " + bytesToSend + " bytes. The maximum is " + m_MaxPacketSize + " bytes on this channel.");
				}
				return false;
			}
			if (!m_CurrentPacket.HasSpace(bytesToSend))
			{
				if (m_IsReliable)
				{
					if (m_PendingPackets.Count == 0)
					{
						if (!m_CurrentPacket.SendToTransport(m_Connection, m_ChannelId))
						{
							QueuePacket();
						}
						m_CurrentPacket.Write(bytes, bytesToSend);
						return true;
					}
					if (m_PendingPackets.Count >= m_MaxPendingPacketCount)
					{
						if (!m_IsBroken && LogFilter.logError)
						{
							Debug.LogError("ChannelBuffer buffer limit of " + m_PendingPackets.Count + " packets reached.");
						}
						m_IsBroken = true;
						return false;
					}
					QueuePacket();
					m_CurrentPacket.Write(bytes, bytesToSend);
					return true;
				}
				if (!m_CurrentPacket.SendToTransport(m_Connection, m_ChannelId))
				{
					if (LogFilter.logError)
					{
						Debug.Log("ChannelBuffer SendBytes no space on unreliable channel " + m_ChannelId);
					}
					return false;
				}
				m_CurrentPacket.Write(bytes, bytesToSend);
				return true;
			}
			m_CurrentPacket.Write(bytes, bytesToSend);
			if (maxDelay == 0f)
			{
				return SendInternalBuffer();
			}
			return true;
		}

		private void QueuePacket()
		{
			pendingPacketCount++;
			m_PendingPackets.Add(m_CurrentPacket);
			m_CurrentPacket = AllocPacket();
		}

		private ChannelPacket AllocPacket()
		{
			if (s_FreePackets.Count == 0)
			{
				return new ChannelPacket(m_MaxPacketSize, m_IsReliable);
			}
			ChannelPacket result = s_FreePackets[0];
			s_FreePackets.RemoveAt(0);
			result.Reset();
			return result;
		}

		private static void FreePacket(ChannelPacket packet)
		{
			if (s_FreePackets.Count < 512)
			{
				s_FreePackets.Add(packet);
			}
		}

		public bool SendInternalBuffer()
		{
			if (m_IsReliable && m_PendingPackets.Count > 0)
			{
				while (m_PendingPackets.Count > 0)
				{
					ChannelPacket packet = m_PendingPackets[0];
					if (!packet.SendToTransport(m_Connection, m_ChannelId))
					{
						break;
					}
					pendingPacketCount--;
					m_PendingPackets.RemoveAt(0);
					FreePacket(packet);
					if (m_IsBroken && m_PendingPackets.Count < m_MaxPendingPacketCount / 2)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("ChannelBuffer recovered from overflow but data was lost.");
						}
						m_IsBroken = false;
					}
				}
				return true;
			}
			return m_CurrentPacket.SendToTransport(m_Connection, m_ChannelId);
		}
	}
}
