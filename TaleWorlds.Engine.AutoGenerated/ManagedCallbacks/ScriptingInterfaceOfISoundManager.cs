using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000029 RID: 41
	internal class ScriptingInterfaceOfISoundManager : ISoundManager
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0001910F File Offset: 0x0001730F
		public void AddSoundClientWithId(ulong client_id)
		{
			ScriptingInterfaceOfISoundManager.call_AddSoundClientWithIdDelegate(client_id);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001911C File Offset: 0x0001731C
		public void AddXBOXRemoteUser(ulong XUID, ulong deviceID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			ScriptingInterfaceOfISoundManager.call_AddXBOXRemoteUserDelegate(XUID, deviceID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00019133 File Offset: 0x00017333
		public void ApplyPushToTalk(bool pushed)
		{
			ScriptingInterfaceOfISoundManager.call_ApplyPushToTalkDelegate(pushed);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00019140 File Offset: 0x00017340
		public void ClearDataToBeSent()
		{
			ScriptingInterfaceOfISoundManager.call_ClearDataToBeSentDelegate();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001914C File Offset: 0x0001734C
		public void ClearXBOXSoundManager()
		{
			ScriptingInterfaceOfISoundManager.call_ClearXBOXSoundManagerDelegate();
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00019158 File Offset: 0x00017358
		public void CompressData(ulong clientID, byte[] buffer, int length, byte[] compressedBuffer, ref int compressedBufferLength)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(buffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (buffer != null) ? buffer.Length : 0);
			PinnedArrayData<byte> pinnedArrayData2 = new PinnedArrayData<byte>(compressedBuffer, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			ManagedArray managedArray2 = new ManagedArray(pointer2, (compressedBuffer != null) ? compressedBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_CompressDataDelegate(clientID, managedArray, length, managedArray2, ref compressedBufferLength);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000191CD File Offset: 0x000173CD
		public void CreateVoiceEvent()
		{
			ScriptingInterfaceOfISoundManager.call_CreateVoiceEventDelegate();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000191DC File Offset: 0x000173DC
		public void DecompressData(ulong clientID, byte[] compressedBuffer, int compressedBufferLength, byte[] decompressedBuffer, ref int decompressedBufferLength)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(compressedBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (compressedBuffer != null) ? compressedBuffer.Length : 0);
			PinnedArrayData<byte> pinnedArrayData2 = new PinnedArrayData<byte>(decompressedBuffer, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			ManagedArray managedArray2 = new ManagedArray(pointer2, (decompressedBuffer != null) ? decompressedBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_DecompressDataDelegate(clientID, managedArray, compressedBufferLength, managedArray2, ref decompressedBufferLength);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00019251 File Offset: 0x00017451
		public void DeleteSoundClientWithId(ulong client_id)
		{
			ScriptingInterfaceOfISoundManager.call_DeleteSoundClientWithIdDelegate(client_id);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001925E File Offset: 0x0001745E
		public void DestroyVoiceEvent(int id)
		{
			ScriptingInterfaceOfISoundManager.call_DestroyVoiceEventDelegate(id);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001926B File Offset: 0x0001746B
		public void FinalizeVoicePlayEvent()
		{
			ScriptingInterfaceOfISoundManager.call_FinalizeVoicePlayEventDelegate();
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00019277 File Offset: 0x00017477
		public void GetAttenuationPosition(out Vec3 result)
		{
			ScriptingInterfaceOfISoundManager.call_GetAttenuationPositionDelegate(out result);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00019284 File Offset: 0x00017484
		public bool GetDataToBeSentAt(int index, byte[] buffer, ulong[] receivers, ref bool transportGuaranteed)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(buffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (buffer != null) ? buffer.Length : 0);
			PinnedArrayData<ulong> pinnedArrayData2 = new PinnedArrayData<ulong>(receivers, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			bool flag = ScriptingInterfaceOfISoundManager.call_GetDataToBeSentAtDelegate(index, managedArray, pointer2, ref transportGuaranteed);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
			return flag;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000192E4 File Offset: 0x000174E4
		public int GetGlobalIndexOfEvent(string eventFullName)
		{
			byte[] array = null;
			if (eventFullName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(eventFullName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(eventFullName, 0, eventFullName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISoundManager.call_GetGlobalIndexOfEventDelegate(array);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001933E File Offset: 0x0001753E
		public void GetListenerFrame(out MatrixFrame result)
		{
			ScriptingInterfaceOfISoundManager.call_GetListenerFrameDelegate(out result);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001934B File Offset: 0x0001754B
		public void GetSizeOfDataToBeSentAt(int index, ref uint byte_count, ref uint numReceivers)
		{
			ScriptingInterfaceOfISoundManager.call_GetSizeOfDataToBeSentAtDelegate(index, ref byte_count, ref numReceivers);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001935C File Offset: 0x0001755C
		public void GetVoiceData(byte[] voiceBuffer, int chunkSize, ref int readBytesLength)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(voiceBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (voiceBuffer != null) ? voiceBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_GetVoiceDataDelegate(managedArray, chunkSize, ref readBytesLength);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001939F File Offset: 0x0001759F
		public void HandleStateChanges()
		{
			ScriptingInterfaceOfISoundManager.call_HandleStateChangesDelegate();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000193AB File Offset: 0x000175AB
		public void InitializeVoicePlayEvent()
		{
			ScriptingInterfaceOfISoundManager.call_InitializeVoicePlayEventDelegate();
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000193B7 File Offset: 0x000175B7
		public void InitializeXBOXSoundManager()
		{
			ScriptingInterfaceOfISoundManager.call_InitializeXBOXSoundManagerDelegate();
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000193C4 File Offset: 0x000175C4
		public void LoadEventFileAux(string soundBankName, bool decompressSamples)
		{
			byte[] array = null;
			if (soundBankName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(soundBankName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(soundBankName, 0, soundBankName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_LoadEventFileAuxDelegate(array, decompressSamples);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00019420 File Offset: 0x00017620
		public void PauseBus(string busName)
		{
			byte[] array = null;
			if (busName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(busName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(busName, 0, busName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_PauseBusDelegate(array);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001947C File Offset: 0x0001767C
		public void ProcessDataToBeReceived(ulong senderDeviceID, byte[] data, uint dataSize)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (data != null) ? data.Length : 0);
			ScriptingInterfaceOfISoundManager.call_ProcessDataToBeReceivedDelegate(senderDeviceID, managedArray, dataSize);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000194BF File Offset: 0x000176BF
		public void ProcessDataToBeSent(ref int numData)
		{
			ScriptingInterfaceOfISoundManager.call_ProcessDataToBeSentDelegate(ref numData);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000194CC File Offset: 0x000176CC
		public void RemoveXBOXRemoteUser(ulong XUID)
		{
			ScriptingInterfaceOfISoundManager.call_RemoveXBOXRemoteUserDelegate(XUID);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000194D9 File Offset: 0x000176D9
		public void Reset()
		{
			ScriptingInterfaceOfISoundManager.call_ResetDelegate();
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000194E8 File Offset: 0x000176E8
		public void SetGlobalParameter(string parameterName, float value)
		{
			byte[] array = null;
			if (parameterName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(parameterName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(parameterName, 0, parameterName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_SetGlobalParameterDelegate(array, value);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00019543 File Offset: 0x00017743
		public void SetListenerFrame(ref MatrixFrame frame, ref Vec3 attenuationPosition)
		{
			ScriptingInterfaceOfISoundManager.call_SetListenerFrameDelegate(ref frame, ref attenuationPosition);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00019554 File Offset: 0x00017754
		public void SetState(string stateGroup, string state)
		{
			byte[] array = null;
			if (stateGroup != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(stateGroup);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(stateGroup, 0, stateGroup.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (state != null)
			{
				int byteCount2 = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(state);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(state, 0, state.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_SetStateDelegate(array, array2);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000195F4 File Offset: 0x000177F4
		public bool StartOneShotEvent(string eventFullName, Vec3 position)
		{
			byte[] array = null;
			if (eventFullName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(eventFullName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(eventFullName, 0, eventFullName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISoundManager.call_StartOneShotEventDelegate(array, position);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001964F File Offset: 0x0001784F
		public bool StartOneShotEventWithIndex(int index, Vec3 position)
		{
			return ScriptingInterfaceOfISoundManager.call_StartOneShotEventWithIndexDelegate(index, position);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00019660 File Offset: 0x00017860
		public bool StartOneShotEventWithParam(string eventFullName, Vec3 position, string paramName, float paramValue)
		{
			byte[] array = null;
			if (eventFullName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(eventFullName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(eventFullName, 0, eventFullName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (paramName != null)
			{
				int byteCount2 = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(paramName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(paramName, 0, paramName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfISoundManager.call_StartOneShotEventWithParamDelegate(array, position, array2, paramValue);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00019700 File Offset: 0x00017900
		public void StartVoiceRecord()
		{
			ScriptingInterfaceOfISoundManager.call_StartVoiceRecordDelegate();
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001970C File Offset: 0x0001790C
		public void StopVoiceRecord()
		{
			ScriptingInterfaceOfISoundManager.call_StopVoiceRecordDelegate();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00019718 File Offset: 0x00017918
		public void UnpauseBus(string busName)
		{
			byte[] array = null;
			if (busName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(busName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(busName, 0, busName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_UnpauseBusDelegate(array);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00019774 File Offset: 0x00017974
		public void UpdateVoiceToPlay(byte[] voiceBuffer, int length, int index)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(voiceBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (voiceBuffer != null) ? voiceBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_UpdateVoiceToPlayDelegate(managedArray, length, index);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000197B7 File Offset: 0x000179B7
		public void UpdateXBOXChatCommunicationFlags(ulong XUID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			ScriptingInterfaceOfISoundManager.call_UpdateXBOXChatCommunicationFlagsDelegate(XUID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000197CC File Offset: 0x000179CC
		public void UpdateXBOXLocalUser()
		{
			ScriptingInterfaceOfISoundManager.call_UpdateXBOXLocalUserDelegate();
		}

		// Token: 0x04000526 RID: 1318
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000527 RID: 1319
		public static ScriptingInterfaceOfISoundManager.AddSoundClientWithIdDelegate call_AddSoundClientWithIdDelegate;

		// Token: 0x04000528 RID: 1320
		public static ScriptingInterfaceOfISoundManager.AddXBOXRemoteUserDelegate call_AddXBOXRemoteUserDelegate;

		// Token: 0x04000529 RID: 1321
		public static ScriptingInterfaceOfISoundManager.ApplyPushToTalkDelegate call_ApplyPushToTalkDelegate;

		// Token: 0x0400052A RID: 1322
		public static ScriptingInterfaceOfISoundManager.ClearDataToBeSentDelegate call_ClearDataToBeSentDelegate;

		// Token: 0x0400052B RID: 1323
		public static ScriptingInterfaceOfISoundManager.ClearXBOXSoundManagerDelegate call_ClearXBOXSoundManagerDelegate;

		// Token: 0x0400052C RID: 1324
		public static ScriptingInterfaceOfISoundManager.CompressDataDelegate call_CompressDataDelegate;

		// Token: 0x0400052D RID: 1325
		public static ScriptingInterfaceOfISoundManager.CreateVoiceEventDelegate call_CreateVoiceEventDelegate;

		// Token: 0x0400052E RID: 1326
		public static ScriptingInterfaceOfISoundManager.DecompressDataDelegate call_DecompressDataDelegate;

		// Token: 0x0400052F RID: 1327
		public static ScriptingInterfaceOfISoundManager.DeleteSoundClientWithIdDelegate call_DeleteSoundClientWithIdDelegate;

		// Token: 0x04000530 RID: 1328
		public static ScriptingInterfaceOfISoundManager.DestroyVoiceEventDelegate call_DestroyVoiceEventDelegate;

		// Token: 0x04000531 RID: 1329
		public static ScriptingInterfaceOfISoundManager.FinalizeVoicePlayEventDelegate call_FinalizeVoicePlayEventDelegate;

		// Token: 0x04000532 RID: 1330
		public static ScriptingInterfaceOfISoundManager.GetAttenuationPositionDelegate call_GetAttenuationPositionDelegate;

		// Token: 0x04000533 RID: 1331
		public static ScriptingInterfaceOfISoundManager.GetDataToBeSentAtDelegate call_GetDataToBeSentAtDelegate;

		// Token: 0x04000534 RID: 1332
		public static ScriptingInterfaceOfISoundManager.GetGlobalIndexOfEventDelegate call_GetGlobalIndexOfEventDelegate;

		// Token: 0x04000535 RID: 1333
		public static ScriptingInterfaceOfISoundManager.GetListenerFrameDelegate call_GetListenerFrameDelegate;

		// Token: 0x04000536 RID: 1334
		public static ScriptingInterfaceOfISoundManager.GetSizeOfDataToBeSentAtDelegate call_GetSizeOfDataToBeSentAtDelegate;

		// Token: 0x04000537 RID: 1335
		public static ScriptingInterfaceOfISoundManager.GetVoiceDataDelegate call_GetVoiceDataDelegate;

		// Token: 0x04000538 RID: 1336
		public static ScriptingInterfaceOfISoundManager.HandleStateChangesDelegate call_HandleStateChangesDelegate;

		// Token: 0x04000539 RID: 1337
		public static ScriptingInterfaceOfISoundManager.InitializeVoicePlayEventDelegate call_InitializeVoicePlayEventDelegate;

		// Token: 0x0400053A RID: 1338
		public static ScriptingInterfaceOfISoundManager.InitializeXBOXSoundManagerDelegate call_InitializeXBOXSoundManagerDelegate;

		// Token: 0x0400053B RID: 1339
		public static ScriptingInterfaceOfISoundManager.LoadEventFileAuxDelegate call_LoadEventFileAuxDelegate;

		// Token: 0x0400053C RID: 1340
		public static ScriptingInterfaceOfISoundManager.PauseBusDelegate call_PauseBusDelegate;

		// Token: 0x0400053D RID: 1341
		public static ScriptingInterfaceOfISoundManager.ProcessDataToBeReceivedDelegate call_ProcessDataToBeReceivedDelegate;

		// Token: 0x0400053E RID: 1342
		public static ScriptingInterfaceOfISoundManager.ProcessDataToBeSentDelegate call_ProcessDataToBeSentDelegate;

		// Token: 0x0400053F RID: 1343
		public static ScriptingInterfaceOfISoundManager.RemoveXBOXRemoteUserDelegate call_RemoveXBOXRemoteUserDelegate;

		// Token: 0x04000540 RID: 1344
		public static ScriptingInterfaceOfISoundManager.ResetDelegate call_ResetDelegate;

		// Token: 0x04000541 RID: 1345
		public static ScriptingInterfaceOfISoundManager.SetGlobalParameterDelegate call_SetGlobalParameterDelegate;

		// Token: 0x04000542 RID: 1346
		public static ScriptingInterfaceOfISoundManager.SetListenerFrameDelegate call_SetListenerFrameDelegate;

		// Token: 0x04000543 RID: 1347
		public static ScriptingInterfaceOfISoundManager.SetStateDelegate call_SetStateDelegate;

		// Token: 0x04000544 RID: 1348
		public static ScriptingInterfaceOfISoundManager.StartOneShotEventDelegate call_StartOneShotEventDelegate;

		// Token: 0x04000545 RID: 1349
		public static ScriptingInterfaceOfISoundManager.StartOneShotEventWithIndexDelegate call_StartOneShotEventWithIndexDelegate;

		// Token: 0x04000546 RID: 1350
		public static ScriptingInterfaceOfISoundManager.StartOneShotEventWithParamDelegate call_StartOneShotEventWithParamDelegate;

		// Token: 0x04000547 RID: 1351
		public static ScriptingInterfaceOfISoundManager.StartVoiceRecordDelegate call_StartVoiceRecordDelegate;

		// Token: 0x04000548 RID: 1352
		public static ScriptingInterfaceOfISoundManager.StopVoiceRecordDelegate call_StopVoiceRecordDelegate;

		// Token: 0x04000549 RID: 1353
		public static ScriptingInterfaceOfISoundManager.UnpauseBusDelegate call_UnpauseBusDelegate;

		// Token: 0x0400054A RID: 1354
		public static ScriptingInterfaceOfISoundManager.UpdateVoiceToPlayDelegate call_UpdateVoiceToPlayDelegate;

		// Token: 0x0400054B RID: 1355
		public static ScriptingInterfaceOfISoundManager.UpdateXBOXChatCommunicationFlagsDelegate call_UpdateXBOXChatCommunicationFlagsDelegate;

		// Token: 0x0400054C RID: 1356
		public static ScriptingInterfaceOfISoundManager.UpdateXBOXLocalUserDelegate call_UpdateXBOXLocalUserDelegate;

		// Token: 0x0200058D RID: 1421
		// (Invoke) Token: 0x06001C5F RID: 7263
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSoundClientWithIdDelegate(ulong client_id);

		// Token: 0x0200058E RID: 1422
		// (Invoke) Token: 0x06001C63 RID: 7267
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddXBOXRemoteUserDelegate(ulong XUID, ulong deviceID, [MarshalAs(UnmanagedType.U1)] bool canSendMicSound, [MarshalAs(UnmanagedType.U1)] bool canSendTextSound, [MarshalAs(UnmanagedType.U1)] bool canSendText, [MarshalAs(UnmanagedType.U1)] bool canReceiveSound, [MarshalAs(UnmanagedType.U1)] bool canReceiveText);

		// Token: 0x0200058F RID: 1423
		// (Invoke) Token: 0x06001C67 RID: 7271
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyPushToTalkDelegate([MarshalAs(UnmanagedType.U1)] bool pushed);

		// Token: 0x02000590 RID: 1424
		// (Invoke) Token: 0x06001C6B RID: 7275
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearDataToBeSentDelegate();

		// Token: 0x02000591 RID: 1425
		// (Invoke) Token: 0x06001C6F RID: 7279
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearXBOXSoundManagerDelegate();

		// Token: 0x02000592 RID: 1426
		// (Invoke) Token: 0x06001C73 RID: 7283
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CompressDataDelegate(ulong clientID, ManagedArray buffer, int length, ManagedArray compressedBuffer, ref int compressedBufferLength);

		// Token: 0x02000593 RID: 1427
		// (Invoke) Token: 0x06001C77 RID: 7287
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CreateVoiceEventDelegate();

		// Token: 0x02000594 RID: 1428
		// (Invoke) Token: 0x06001C7B RID: 7291
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DecompressDataDelegate(ulong clientID, ManagedArray compressedBuffer, int compressedBufferLength, ManagedArray decompressedBuffer, ref int decompressedBufferLength);

		// Token: 0x02000595 RID: 1429
		// (Invoke) Token: 0x06001C7F RID: 7295
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeleteSoundClientWithIdDelegate(ulong client_id);

		// Token: 0x02000596 RID: 1430
		// (Invoke) Token: 0x06001C83 RID: 7299
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DestroyVoiceEventDelegate(int id);

		// Token: 0x02000597 RID: 1431
		// (Invoke) Token: 0x06001C87 RID: 7303
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeVoicePlayEventDelegate();

		// Token: 0x02000598 RID: 1432
		// (Invoke) Token: 0x06001C8B RID: 7307
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetAttenuationPositionDelegate(out Vec3 result);

		// Token: 0x02000599 RID: 1433
		// (Invoke) Token: 0x06001C8F RID: 7311
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetDataToBeSentAtDelegate(int index, ManagedArray buffer, IntPtr receivers, [MarshalAs(UnmanagedType.U1)] ref bool transportGuaranteed);

		// Token: 0x0200059A RID: 1434
		// (Invoke) Token: 0x06001C93 RID: 7315
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetGlobalIndexOfEventDelegate(byte[] eventFullName);

		// Token: 0x0200059B RID: 1435
		// (Invoke) Token: 0x06001C97 RID: 7319
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetListenerFrameDelegate(out MatrixFrame result);

		// Token: 0x0200059C RID: 1436
		// (Invoke) Token: 0x06001C9B RID: 7323
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSizeOfDataToBeSentAtDelegate(int index, ref uint byte_count, ref uint numReceivers);

		// Token: 0x0200059D RID: 1437
		// (Invoke) Token: 0x06001C9F RID: 7327
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetVoiceDataDelegate(ManagedArray voiceBuffer, int chunkSize, ref int readBytesLength);

		// Token: 0x0200059E RID: 1438
		// (Invoke) Token: 0x06001CA3 RID: 7331
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void HandleStateChangesDelegate();

		// Token: 0x0200059F RID: 1439
		// (Invoke) Token: 0x06001CA7 RID: 7335
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeVoicePlayEventDelegate();

		// Token: 0x020005A0 RID: 1440
		// (Invoke) Token: 0x06001CAB RID: 7339
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeXBOXSoundManagerDelegate();

		// Token: 0x020005A1 RID: 1441
		// (Invoke) Token: 0x06001CAF RID: 7343
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadEventFileAuxDelegate(byte[] soundBankName, [MarshalAs(UnmanagedType.U1)] bool decompressSamples);

		// Token: 0x020005A2 RID: 1442
		// (Invoke) Token: 0x06001CB3 RID: 7347
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseBusDelegate(byte[] busName);

		// Token: 0x020005A3 RID: 1443
		// (Invoke) Token: 0x06001CB7 RID: 7351
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProcessDataToBeReceivedDelegate(ulong senderDeviceID, ManagedArray data, uint dataSize);

		// Token: 0x020005A4 RID: 1444
		// (Invoke) Token: 0x06001CBB RID: 7355
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProcessDataToBeSentDelegate(ref int numData);

		// Token: 0x020005A5 RID: 1445
		// (Invoke) Token: 0x06001CBF RID: 7359
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveXBOXRemoteUserDelegate(ulong XUID);

		// Token: 0x020005A6 RID: 1446
		// (Invoke) Token: 0x06001CC3 RID: 7363
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetDelegate();

		// Token: 0x020005A7 RID: 1447
		// (Invoke) Token: 0x06001CC7 RID: 7367
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGlobalParameterDelegate(byte[] parameterName, float value);

		// Token: 0x020005A8 RID: 1448
		// (Invoke) Token: 0x06001CCB RID: 7371
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetListenerFrameDelegate(ref MatrixFrame frame, ref Vec3 attenuationPosition);

		// Token: 0x020005A9 RID: 1449
		// (Invoke) Token: 0x06001CCF RID: 7375
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetStateDelegate(byte[] stateGroup, byte[] state);

		// Token: 0x020005AA RID: 1450
		// (Invoke) Token: 0x06001CD3 RID: 7379
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartOneShotEventDelegate(byte[] eventFullName, Vec3 position);

		// Token: 0x020005AB RID: 1451
		// (Invoke) Token: 0x06001CD7 RID: 7383
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartOneShotEventWithIndexDelegate(int index, Vec3 position);

		// Token: 0x020005AC RID: 1452
		// (Invoke) Token: 0x06001CDB RID: 7387
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartOneShotEventWithParamDelegate(byte[] eventFullName, Vec3 position, byte[] paramName, float paramValue);

		// Token: 0x020005AD RID: 1453
		// (Invoke) Token: 0x06001CDF RID: 7391
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartVoiceRecordDelegate();

		// Token: 0x020005AE RID: 1454
		// (Invoke) Token: 0x06001CE3 RID: 7395
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StopVoiceRecordDelegate();

		// Token: 0x020005AF RID: 1455
		// (Invoke) Token: 0x06001CE7 RID: 7399
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnpauseBusDelegate(byte[] busName);

		// Token: 0x020005B0 RID: 1456
		// (Invoke) Token: 0x06001CEB RID: 7403
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateVoiceToPlayDelegate(ManagedArray voiceBuffer, int length, int index);

		// Token: 0x020005B1 RID: 1457
		// (Invoke) Token: 0x06001CEF RID: 7407
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateXBOXChatCommunicationFlagsDelegate(ulong XUID, [MarshalAs(UnmanagedType.U1)] bool canSendMicSound, [MarshalAs(UnmanagedType.U1)] bool canSendTextSound, [MarshalAs(UnmanagedType.U1)] bool canSendText, [MarshalAs(UnmanagedType.U1)] bool canReceiveSound, [MarshalAs(UnmanagedType.U1)] bool canReceiveText);

		// Token: 0x020005B2 RID: 1458
		// (Invoke) Token: 0x06001CF3 RID: 7411
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateXBOXLocalUserDelegate();
	}
}
