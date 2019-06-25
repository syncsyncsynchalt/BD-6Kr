using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class TusTss
	{
		private struct TusTssData
		{
			public int dataSize;

			private IntPtr _data;

			public byte[] data
			{
				get
				{
					byte[] array = new byte[dataSize];
					Marshal.Copy(_data, array, 0, dataSize);
					return array;
				}
			}
		}

		public struct TusRetrievedVariable
		{
			public long lastChangedDate;

			public long variable;

			public long oldVariable;

			private int npIDSize;

			private IntPtr _ownerNpID;

			private IntPtr _lastChangeAuthorNpID;

			public int hasData;

			public byte[] ownerNpID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_ownerNpID, array, 0, npIDSize);
					return array;
				}
			}

			public byte[] lastChangeAuthorNpID
			{
				get
				{
					byte[] array = new byte[npIDSize];
					Marshal.Copy(_lastChangeAuthorNpID, array, 0, npIDSize);
					return array;
				}
			}
		}

		public struct TusVariable
		{
			public int slotID;

			public long value;

			public TusVariable(int _slotID, long _value)
			{
				slotID = _slotID;
				value = _value;
			}
		}

		public static event Messages.EventHandler OnTusDataSet;

		public static event Messages.EventHandler OnTusDataRecieved;

		public static event Messages.EventHandler OnTusVariablesSet;

		public static event Messages.EventHandler OnTusVariablesModified;

		public static event Messages.EventHandler OnTusVariablesRecieved;

		public static event Messages.EventHandler OnTssDataRecieved;

		public static event Messages.EventHandler OnTssNoData;

		public static event Messages.EventHandler OnTusTssError;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTUSIsBusy();

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTusTssGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxTusTssGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSClearVariablesToSet();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSAddVariableToSet(int slot, long value);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSSetVariables();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSSetVariablesForUser(byte[] npID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTUSSetVariablesForVirtualUser(string onlineID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSModifyVariables();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSModifyVariablesForUser(byte[] npID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTUSModifyVariablesForVirtualUser(string onlineID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSClearVariablesToGet();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSAddVariableToGet(int slot);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSRequestVariables();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSRequestVariablesForUser(byte[] npID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTUSRequestVariablesForVirtualUser(string onlineID);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxTUSGetVariableCount();

		[DllImport("UnityNpToolkit")]
		private static extern long PrxTUSGetVariableValue(int index);

		[DllImport("UnityNpToolkit")]
		private static extern long PrxTUSGetVariable(int index, out TusRetrievedVariable variable);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSSetData(int slotID, byte[] data, int dataSize);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSRequestData(int slotID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSSetDataForUser(int slotID, byte[] data, int dataSize, byte[] npID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTUSSetDataForVirtualUser(int slotID, byte[] data, int dataSize, string onlineID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSRequestDataForUser(int slotID, byte[] npID);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxTUSRequestDataForVirtualUser(int slotID, string onlineID);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTUSGetData(out TusTssData data);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxTSSIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTSSRequestData();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTSSRequestDataFromSlot(int slot);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTSSRequestSlotStatus(int slot);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxTSSGetData(out TusTssData data);

		public static bool IsTusBusy()
		{
			return PrxTUSIsBusy();
		}

		public static ErrorCode SetTusVariables(TusVariable[] variables)
		{
			if (variables.Length > 0)
			{
				PrxTUSClearVariablesToSet();
				for (int i = 0; i < variables.Length; i++)
				{
					PrxTUSAddVariableToSet(variables[i].slotID, variables[i].value);
				}
				return PrxTUSSetVariables();
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode SetTusVariablesForUser(byte[] npID, TusVariable[] variables)
		{
			if (variables.Length > 0)
			{
				PrxTUSClearVariablesToSet();
				for (int i = 0; i < variables.Length; i++)
				{
					PrxTUSAddVariableToSet(variables[i].slotID, variables[i].value);
				}
				return PrxTUSSetVariablesForUser(npID);
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode SetTusVariablesForVirtualUser(string onlineID, TusVariable[] variables)
		{
			if (variables.Length > 0)
			{
				PrxTUSClearVariablesToSet();
				for (int i = 0; i < variables.Length; i++)
				{
					PrxTUSAddVariableToSet(variables[i].slotID, variables[i].value);
				}
				return PrxTUSSetVariablesForVirtualUser(onlineID);
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode ModifyTusVariables(TusVariable[] variables)
		{
			if (variables.Length > 0)
			{
				PrxTUSClearVariablesToSet();
				for (int i = 0; i < variables.Length; i++)
				{
					PrxTUSAddVariableToSet(variables[i].slotID, variables[i].value);
				}
				return PrxTUSModifyVariables();
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode ModifyTusVariablesForUser(byte[] npID, TusVariable[] variables)
		{
			if (variables.Length > 0)
			{
				PrxTUSClearVariablesToSet();
				for (int i = 0; i < variables.Length; i++)
				{
					PrxTUSAddVariableToSet(variables[i].slotID, variables[i].value);
				}
				return PrxTUSModifyVariablesForUser(npID);
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode ModifyTusVariablesForVirtualUser(string onlineID, TusVariable[] variables)
		{
			if (variables.Length > 0)
			{
				PrxTUSClearVariablesToSet();
				for (int i = 0; i < variables.Length; i++)
				{
					PrxTUSAddVariableToSet(variables[i].slotID, variables[i].value);
				}
				return PrxTUSModifyVariablesForVirtualUser(onlineID);
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode RequestTusVariables(int[] slotIDs)
		{
			if (slotIDs.Length > 0)
			{
				PrxTUSClearVariablesToGet();
				for (int i = 0; i < slotIDs.Length; i++)
				{
					PrxTUSAddVariableToGet(slotIDs[i]);
				}
				return PrxTUSRequestVariables();
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode RequestTusVariablesForUser(byte[] npID, int[] slotIDs)
		{
			if (slotIDs.Length > 0)
			{
				PrxTUSClearVariablesToGet();
				for (int i = 0; i < slotIDs.Length; i++)
				{
					PrxTUSAddVariableToGet(slotIDs[i]);
				}
				return PrxTUSRequestVariablesForUser(npID);
			}
			return ErrorCode.NP_OK;
		}

		public static ErrorCode RequestTusVariablesForVirtualUser(string onlineID, int[] slotIDs)
		{
			if (slotIDs.Length > 0)
			{
				PrxTUSClearVariablesToGet();
				for (int i = 0; i < slotIDs.Length; i++)
				{
					PrxTUSAddVariableToGet(slotIDs[i]);
				}
				return PrxTUSRequestVariablesForVirtualUser(onlineID);
			}
			return ErrorCode.NP_OK;
		}

		public static long[] GetTusVariablesValue()
		{
			int num = PrxTUSGetVariableCount();
			long[] array = new long[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = PrxTUSGetVariableValue(i);
			}
			return array;
		}

		public static TusRetrievedVariable[] GetTusVariables()
		{
			int num = PrxTUSGetVariableCount();
			TusRetrievedVariable[] array = new TusRetrievedVariable[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = default(TusRetrievedVariable);
				PrxTUSGetVariable(i, out array[i]);
			}
			return array;
		}

		public static ErrorCode SetTusData(int slotID, byte[] data)
		{
			return PrxTUSSetData(slotID, data, data.Length);
		}

		public static ErrorCode RequestTusData(int slotID)
		{
			return PrxTUSRequestData(slotID);
		}

		public static ErrorCode SetTusDataForUser(int slotID, byte[] data, byte[] npID)
		{
			return PrxTUSSetDataForUser(slotID, data, data.Length, npID);
		}

		public static ErrorCode SetTusDataForVirtualUser(int slotID, byte[] data, string onlineID)
		{
			return PrxTUSSetDataForVirtualUser(slotID, data, data.Length, onlineID);
		}

		public static ErrorCode RequestTusDataForUser(int slotID, byte[] npID)
		{
			return PrxTUSRequestDataForUser(slotID, npID);
		}

		public static ErrorCode RequestTusDataForVirtualUser(int slotID, string onlineID)
		{
			return PrxTUSRequestDataForVirtualUser(slotID, onlineID);
		}

		public static byte[] GetTusData()
		{
			TusTssData data = default(TusTssData);
			if (PrxTUSGetData(out data) == ErrorCode.NP_OK)
			{
				return data.data;
			}
			return null;
		}

		public static bool IsTssBusy()
		{
			return PrxTSSIsBusy();
		}

		public static ErrorCode RequestTssData()
		{
			return PrxTSSRequestData();
		}

		public static ErrorCode RequestTssDataFromSlot(int slot)
		{
			return PrxTSSRequestDataFromSlot(slot);
		}

		public static byte[] GetTssData()
		{
			TusTssData data = default(TusTssData);
			if (PrxTSSGetData(out data) == ErrorCode.NP_OK)
			{
				return data.data;
			}
			return null;
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_TUSDataSet:
				if (TusTss.OnTusDataSet != null)
				{
					TusTss.OnTusDataSet(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TUSDataReceived:
				if (TusTss.OnTusDataRecieved != null)
				{
					TusTss.OnTusDataRecieved(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TUSVariablesSet:
				if (TusTss.OnTusVariablesSet != null)
				{
					TusTss.OnTusVariablesSet(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TUSVariablesModified:
				if (TusTss.OnTusVariablesModified != null)
				{
					TusTss.OnTusVariablesModified(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TUSVariablesReceived:
				if (TusTss.OnTusVariablesRecieved != null)
				{
					TusTss.OnTusVariablesRecieved(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TSSDataReceived:
				if (TusTss.OnTssDataRecieved != null)
				{
					TusTss.OnTssDataRecieved(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TSSNoData:
				if (TusTss.OnTssNoData != null)
				{
					TusTss.OnTssNoData(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_TusTssError:
				if (TusTss.OnTusTssError != null)
				{
					TusTss.OnTusTssError(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
