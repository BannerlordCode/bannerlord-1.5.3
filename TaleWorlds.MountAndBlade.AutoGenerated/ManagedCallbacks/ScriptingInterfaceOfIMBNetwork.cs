using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001B RID: 27
	internal class ScriptingInterfaceOfIMBNetwork : IMBNetwork
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000CDFE File Offset: 0x0000AFFE
		public int AddNewBotOnServer()
		{
			return ScriptingInterfaceOfIMBNetwork.call_AddNewBotOnServerDelegate();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000CE0A File Offset: 0x0000B00A
		public int AddNewPlayerOnServer(bool serverPlayer)
		{
			return ScriptingInterfaceOfIMBNetwork.call_AddNewPlayerOnServerDelegate(serverPlayer);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000CE17 File Offset: 0x0000B017
		public void AddPeerToDisconnect(int peer)
		{
			ScriptingInterfaceOfIMBNetwork.call_AddPeerToDisconnectDelegate(peer);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000CE24 File Offset: 0x0000B024
		public void BeginBroadcastModuleEvent()
		{
			ScriptingInterfaceOfIMBNetwork.call_BeginBroadcastModuleEventDelegate();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000CE30 File Offset: 0x0000B030
		public void BeginModuleEventAsClient(bool isReliable)
		{
			ScriptingInterfaceOfIMBNetwork.call_BeginModuleEventAsClientDelegate(isReliable);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000CE3D File Offset: 0x0000B03D
		public bool CanAddNewPlayersOnServer(int numPlayers)
		{
			return ScriptingInterfaceOfIMBNetwork.call_CanAddNewPlayersOnServerDelegate(numPlayers);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000CE4A File Offset: 0x0000B04A
		public void ClearReplicationTableStatistics()
		{
			ScriptingInterfaceOfIMBNetwork.call_ClearReplicationTableStatisticsDelegate();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000CE56 File Offset: 0x0000B056
		public double ElapsedTimeSinceLastUdpPacketArrived()
		{
			return ScriptingInterfaceOfIMBNetwork.call_ElapsedTimeSinceLastUdpPacketArrivedDelegate();
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000CE62 File Offset: 0x0000B062
		public void EndBroadcastModuleEvent(int broadcastFlags, int targetPlayer, bool isReliable)
		{
			ScriptingInterfaceOfIMBNetwork.call_EndBroadcastModuleEventDelegate(broadcastFlags, targetPlayer, isReliable);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000CE71 File Offset: 0x0000B071
		public void EndModuleEventAsClient(bool isReliable)
		{
			ScriptingInterfaceOfIMBNetwork.call_EndModuleEventAsClientDelegate(isReliable);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000CE7E File Offset: 0x0000B07E
		public string GetActiveUdpSessionsIpAddress()
		{
			if (ScriptingInterfaceOfIMBNetwork.call_GetActiveUdpSessionsIpAddressDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000CE94 File Offset: 0x0000B094
		public float GetAveragePacketLossRatio()
		{
			return ScriptingInterfaceOfIMBNetwork.call_GetAveragePacketLossRatioDelegate();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
		public void GetDebugUploadsInBits(ref GameNetwork.DebugNetworkPacketStatisticsStruct networkStatisticsStruct, ref GameNetwork.DebugNetworkPositionCompressionStatisticsStruct posStatisticsStruct)
		{
			ScriptingInterfaceOfIMBNetwork.call_GetDebugUploadsInBitsDelegate(ref networkStatisticsStruct, ref posStatisticsStruct);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000CEAE File Offset: 0x0000B0AE
		public bool GetMultiplayerDisabled()
		{
			return ScriptingInterfaceOfIMBNetwork.call_GetMultiplayerDisabledDelegate();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		public void InitializeClientSide(string serverAddress, int port, int sessionKey, int playerIndex)
		{
			byte[] array = null;
			if (serverAddress != null)
			{
				int byteCount = ScriptingInterfaceOfIMBNetwork._utf8.GetByteCount(serverAddress);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBNetwork._utf8.GetBytes(serverAddress, 0, serverAddress.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBNetwork.call_InitializeClientSideDelegate(array, port, sessionKey, playerIndex);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000CF1A File Offset: 0x0000B11A
		public void InitializeServerSide(int port)
		{
			ScriptingInterfaceOfIMBNetwork.call_InitializeServerSideDelegate(port);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000CF27 File Offset: 0x0000B127
		public bool IsDedicatedServer()
		{
			return ScriptingInterfaceOfIMBNetwork.call_IsDedicatedServerDelegate();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000CF33 File Offset: 0x0000B133
		public void PrepareNewUdpSession(int player, int sessionKey)
		{
			ScriptingInterfaceOfIMBNetwork.call_PrepareNewUdpSessionDelegate(player, sessionKey);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000CF41 File Offset: 0x0000B141
		public void PrintDebugStats()
		{
			ScriptingInterfaceOfIMBNetwork.call_PrintDebugStatsDelegate();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000CF4D File Offset: 0x0000B14D
		public void PrintReplicationTableStatistics()
		{
			ScriptingInterfaceOfIMBNetwork.call_PrintReplicationTableStatisticsDelegate();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000CF5C File Offset: 0x0000B15C
		public int ReadByteArrayFromPacket(byte[] buffer, int offset, int bufferCapacity, ref bool bufferReadValid)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(buffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (buffer != null) ? buffer.Length : 0);
			int num = ScriptingInterfaceOfIMBNetwork.call_ReadByteArrayFromPacketDelegate(managedArray, offset, bufferCapacity, ref bufferReadValid);
			pinnedArrayData.Dispose();
			return num;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000CFA1 File Offset: 0x0000B1A1
		public bool ReadFloatFromPacket(ref CompressionInfo.Float compressionInfo, out float output)
		{
			return ScriptingInterfaceOfIMBNetwork.call_ReadFloatFromPacketDelegate(ref compressionInfo, out output);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000CFAF File Offset: 0x0000B1AF
		public bool ReadIntFromPacket(ref CompressionInfo.Integer compressionInfo, out int output)
		{
			return ScriptingInterfaceOfIMBNetwork.call_ReadIntFromPacketDelegate(ref compressionInfo, out output);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000CFBD File Offset: 0x0000B1BD
		public bool ReadLongFromPacket(ref CompressionInfo.LongInteger compressionInfo, out long output)
		{
			return ScriptingInterfaceOfIMBNetwork.call_ReadLongFromPacketDelegate(ref compressionInfo, out output);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000CFCB File Offset: 0x0000B1CB
		public string ReadStringFromPacket(ref bool bufferReadValid)
		{
			if (ScriptingInterfaceOfIMBNetwork.call_ReadStringFromPacketDelegate(ref bufferReadValid) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000CFE2 File Offset: 0x0000B1E2
		public bool ReadUintFromPacket(ref CompressionInfo.UnsignedInteger compressionInfo, out uint output)
		{
			return ScriptingInterfaceOfIMBNetwork.call_ReadUintFromPacketDelegate(ref compressionInfo, out output);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000CFF0 File Offset: 0x0000B1F0
		public bool ReadUlongFromPacket(ref CompressionInfo.UnsignedLongInteger compressionInfo, out ulong output)
		{
			return ScriptingInterfaceOfIMBNetwork.call_ReadUlongFromPacketDelegate(ref compressionInfo, out output);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000CFFE File Offset: 0x0000B1FE
		public void RemoveBotOnServer(int botPlayerIndex)
		{
			ScriptingInterfaceOfIMBNetwork.call_RemoveBotOnServerDelegate(botPlayerIndex);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000D00B File Offset: 0x0000B20B
		public void ResetDebugUploads()
		{
			ScriptingInterfaceOfIMBNetwork.call_ResetDebugUploadsDelegate();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000D017 File Offset: 0x0000B217
		public void ResetDebugVariables()
		{
			ScriptingInterfaceOfIMBNetwork.call_ResetDebugVariablesDelegate();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000D023 File Offset: 0x0000B223
		public void ResetMissionData()
		{
			ScriptingInterfaceOfIMBNetwork.call_ResetMissionDataDelegate();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000D030 File Offset: 0x0000B230
		public void ServerPing(string serverAddress, int port)
		{
			byte[] array = null;
			if (serverAddress != null)
			{
				int byteCount = ScriptingInterfaceOfIMBNetwork._utf8.GetByteCount(serverAddress);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBNetwork._utf8.GetBytes(serverAddress, 0, serverAddress.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBNetwork.call_ServerPingDelegate(array, port);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000D08B File Offset: 0x0000B28B
		public void SetServerBandwidthLimitInMbps(double value)
		{
			ScriptingInterfaceOfIMBNetwork.call_SetServerBandwidthLimitInMbpsDelegate(value);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000D098 File Offset: 0x0000B298
		public void SetServerFrameRate(double limit)
		{
			ScriptingInterfaceOfIMBNetwork.call_SetServerFrameRateDelegate(limit);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000D0A5 File Offset: 0x0000B2A5
		public void SetServerTickRate(double value)
		{
			ScriptingInterfaceOfIMBNetwork.call_SetServerTickRateDelegate(value);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000D0B2 File Offset: 0x0000B2B2
		public void TerminateClientSide()
		{
			ScriptingInterfaceOfIMBNetwork.call_TerminateClientSideDelegate();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000D0BE File Offset: 0x0000B2BE
		public void TerminateServerSide()
		{
			ScriptingInterfaceOfIMBNetwork.call_TerminateServerSideDelegate();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000D0CC File Offset: 0x0000B2CC
		public void WriteByteArrayToPacket(byte[] value, int offset, int size)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(value, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (value != null) ? value.Length : 0);
			ScriptingInterfaceOfIMBNetwork.call_WriteByteArrayToPacketDelegate(managedArray, offset, size);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000D10F File Offset: 0x0000B30F
		public void WriteFloatToPacket(float value, ref CompressionInfo.Float compressionInfo)
		{
			ScriptingInterfaceOfIMBNetwork.call_WriteFloatToPacketDelegate(value, ref compressionInfo);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000D11D File Offset: 0x0000B31D
		public void WriteIntToPacket(int value, ref CompressionInfo.Integer compressionInfo)
		{
			ScriptingInterfaceOfIMBNetwork.call_WriteIntToPacketDelegate(value, ref compressionInfo);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000D12B File Offset: 0x0000B32B
		public void WriteLongToPacket(long value, ref CompressionInfo.LongInteger compressionInfo)
		{
			ScriptingInterfaceOfIMBNetwork.call_WriteLongToPacketDelegate(value, ref compressionInfo);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000D13C File Offset: 0x0000B33C
		public void WriteStringToPacket(string value)
		{
			byte[] array = null;
			if (value != null)
			{
				int byteCount = ScriptingInterfaceOfIMBNetwork._utf8.GetByteCount(value);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBNetwork._utf8.GetBytes(value, 0, value.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBNetwork.call_WriteStringToPacketDelegate(array);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000D196 File Offset: 0x0000B396
		public void WriteUintToPacket(uint value, ref CompressionInfo.UnsignedInteger compressionInfo)
		{
			ScriptingInterfaceOfIMBNetwork.call_WriteUintToPacketDelegate(value, ref compressionInfo);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000D1A4 File Offset: 0x0000B3A4
		public void WriteUlongToPacket(ulong value, ref CompressionInfo.UnsignedLongInteger compressionInfo)
		{
			ScriptingInterfaceOfIMBNetwork.call_WriteUlongToPacketDelegate(value, ref compressionInfo);
		}

		// Token: 0x04000272 RID: 626
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000273 RID: 627
		public static ScriptingInterfaceOfIMBNetwork.AddNewBotOnServerDelegate call_AddNewBotOnServerDelegate;

		// Token: 0x04000274 RID: 628
		public static ScriptingInterfaceOfIMBNetwork.AddNewPlayerOnServerDelegate call_AddNewPlayerOnServerDelegate;

		// Token: 0x04000275 RID: 629
		public static ScriptingInterfaceOfIMBNetwork.AddPeerToDisconnectDelegate call_AddPeerToDisconnectDelegate;

		// Token: 0x04000276 RID: 630
		public static ScriptingInterfaceOfIMBNetwork.BeginBroadcastModuleEventDelegate call_BeginBroadcastModuleEventDelegate;

		// Token: 0x04000277 RID: 631
		public static ScriptingInterfaceOfIMBNetwork.BeginModuleEventAsClientDelegate call_BeginModuleEventAsClientDelegate;

		// Token: 0x04000278 RID: 632
		public static ScriptingInterfaceOfIMBNetwork.CanAddNewPlayersOnServerDelegate call_CanAddNewPlayersOnServerDelegate;

		// Token: 0x04000279 RID: 633
		public static ScriptingInterfaceOfIMBNetwork.ClearReplicationTableStatisticsDelegate call_ClearReplicationTableStatisticsDelegate;

		// Token: 0x0400027A RID: 634
		public static ScriptingInterfaceOfIMBNetwork.ElapsedTimeSinceLastUdpPacketArrivedDelegate call_ElapsedTimeSinceLastUdpPacketArrivedDelegate;

		// Token: 0x0400027B RID: 635
		public static ScriptingInterfaceOfIMBNetwork.EndBroadcastModuleEventDelegate call_EndBroadcastModuleEventDelegate;

		// Token: 0x0400027C RID: 636
		public static ScriptingInterfaceOfIMBNetwork.EndModuleEventAsClientDelegate call_EndModuleEventAsClientDelegate;

		// Token: 0x0400027D RID: 637
		public static ScriptingInterfaceOfIMBNetwork.GetActiveUdpSessionsIpAddressDelegate call_GetActiveUdpSessionsIpAddressDelegate;

		// Token: 0x0400027E RID: 638
		public static ScriptingInterfaceOfIMBNetwork.GetAveragePacketLossRatioDelegate call_GetAveragePacketLossRatioDelegate;

		// Token: 0x0400027F RID: 639
		public static ScriptingInterfaceOfIMBNetwork.GetDebugUploadsInBitsDelegate call_GetDebugUploadsInBitsDelegate;

		// Token: 0x04000280 RID: 640
		public static ScriptingInterfaceOfIMBNetwork.GetMultiplayerDisabledDelegate call_GetMultiplayerDisabledDelegate;

		// Token: 0x04000281 RID: 641
		public static ScriptingInterfaceOfIMBNetwork.InitializeClientSideDelegate call_InitializeClientSideDelegate;

		// Token: 0x04000282 RID: 642
		public static ScriptingInterfaceOfIMBNetwork.InitializeServerSideDelegate call_InitializeServerSideDelegate;

		// Token: 0x04000283 RID: 643
		public static ScriptingInterfaceOfIMBNetwork.IsDedicatedServerDelegate call_IsDedicatedServerDelegate;

		// Token: 0x04000284 RID: 644
		public static ScriptingInterfaceOfIMBNetwork.PrepareNewUdpSessionDelegate call_PrepareNewUdpSessionDelegate;

		// Token: 0x04000285 RID: 645
		public static ScriptingInterfaceOfIMBNetwork.PrintDebugStatsDelegate call_PrintDebugStatsDelegate;

		// Token: 0x04000286 RID: 646
		public static ScriptingInterfaceOfIMBNetwork.PrintReplicationTableStatisticsDelegate call_PrintReplicationTableStatisticsDelegate;

		// Token: 0x04000287 RID: 647
		public static ScriptingInterfaceOfIMBNetwork.ReadByteArrayFromPacketDelegate call_ReadByteArrayFromPacketDelegate;

		// Token: 0x04000288 RID: 648
		public static ScriptingInterfaceOfIMBNetwork.ReadFloatFromPacketDelegate call_ReadFloatFromPacketDelegate;

		// Token: 0x04000289 RID: 649
		public static ScriptingInterfaceOfIMBNetwork.ReadIntFromPacketDelegate call_ReadIntFromPacketDelegate;

		// Token: 0x0400028A RID: 650
		public static ScriptingInterfaceOfIMBNetwork.ReadLongFromPacketDelegate call_ReadLongFromPacketDelegate;

		// Token: 0x0400028B RID: 651
		public static ScriptingInterfaceOfIMBNetwork.ReadStringFromPacketDelegate call_ReadStringFromPacketDelegate;

		// Token: 0x0400028C RID: 652
		public static ScriptingInterfaceOfIMBNetwork.ReadUintFromPacketDelegate call_ReadUintFromPacketDelegate;

		// Token: 0x0400028D RID: 653
		public static ScriptingInterfaceOfIMBNetwork.ReadUlongFromPacketDelegate call_ReadUlongFromPacketDelegate;

		// Token: 0x0400028E RID: 654
		public static ScriptingInterfaceOfIMBNetwork.RemoveBotOnServerDelegate call_RemoveBotOnServerDelegate;

		// Token: 0x0400028F RID: 655
		public static ScriptingInterfaceOfIMBNetwork.ResetDebugUploadsDelegate call_ResetDebugUploadsDelegate;

		// Token: 0x04000290 RID: 656
		public static ScriptingInterfaceOfIMBNetwork.ResetDebugVariablesDelegate call_ResetDebugVariablesDelegate;

		// Token: 0x04000291 RID: 657
		public static ScriptingInterfaceOfIMBNetwork.ResetMissionDataDelegate call_ResetMissionDataDelegate;

		// Token: 0x04000292 RID: 658
		public static ScriptingInterfaceOfIMBNetwork.ServerPingDelegate call_ServerPingDelegate;

		// Token: 0x04000293 RID: 659
		public static ScriptingInterfaceOfIMBNetwork.SetServerBandwidthLimitInMbpsDelegate call_SetServerBandwidthLimitInMbpsDelegate;

		// Token: 0x04000294 RID: 660
		public static ScriptingInterfaceOfIMBNetwork.SetServerFrameRateDelegate call_SetServerFrameRateDelegate;

		// Token: 0x04000295 RID: 661
		public static ScriptingInterfaceOfIMBNetwork.SetServerTickRateDelegate call_SetServerTickRateDelegate;

		// Token: 0x04000296 RID: 662
		public static ScriptingInterfaceOfIMBNetwork.TerminateClientSideDelegate call_TerminateClientSideDelegate;

		// Token: 0x04000297 RID: 663
		public static ScriptingInterfaceOfIMBNetwork.TerminateServerSideDelegate call_TerminateServerSideDelegate;

		// Token: 0x04000298 RID: 664
		public static ScriptingInterfaceOfIMBNetwork.WriteByteArrayToPacketDelegate call_WriteByteArrayToPacketDelegate;

		// Token: 0x04000299 RID: 665
		public static ScriptingInterfaceOfIMBNetwork.WriteFloatToPacketDelegate call_WriteFloatToPacketDelegate;

		// Token: 0x0400029A RID: 666
		public static ScriptingInterfaceOfIMBNetwork.WriteIntToPacketDelegate call_WriteIntToPacketDelegate;

		// Token: 0x0400029B RID: 667
		public static ScriptingInterfaceOfIMBNetwork.WriteLongToPacketDelegate call_WriteLongToPacketDelegate;

		// Token: 0x0400029C RID: 668
		public static ScriptingInterfaceOfIMBNetwork.WriteStringToPacketDelegate call_WriteStringToPacketDelegate;

		// Token: 0x0400029D RID: 669
		public static ScriptingInterfaceOfIMBNetwork.WriteUintToPacketDelegate call_WriteUintToPacketDelegate;

		// Token: 0x0400029E RID: 670
		public static ScriptingInterfaceOfIMBNetwork.WriteUlongToPacketDelegate call_WriteUlongToPacketDelegate;

		// Token: 0x020002D1 RID: 721
		// (Invoke) Token: 0x06000E34 RID: 3636
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddNewBotOnServerDelegate();

		// Token: 0x020002D2 RID: 722
		// (Invoke) Token: 0x06000E38 RID: 3640
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddNewPlayerOnServerDelegate([MarshalAs(UnmanagedType.U1)] bool serverPlayer);

		// Token: 0x020002D3 RID: 723
		// (Invoke) Token: 0x06000E3C RID: 3644
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPeerToDisconnectDelegate(int peer);

		// Token: 0x020002D4 RID: 724
		// (Invoke) Token: 0x06000E40 RID: 3648
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BeginBroadcastModuleEventDelegate();

		// Token: 0x020002D5 RID: 725
		// (Invoke) Token: 0x06000E44 RID: 3652
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BeginModuleEventAsClientDelegate([MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x020002D6 RID: 726
		// (Invoke) Token: 0x06000E48 RID: 3656
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CanAddNewPlayersOnServerDelegate(int numPlayers);

		// Token: 0x020002D7 RID: 727
		// (Invoke) Token: 0x06000E4C RID: 3660
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearReplicationTableStatisticsDelegate();

		// Token: 0x020002D8 RID: 728
		// (Invoke) Token: 0x06000E50 RID: 3664
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate double ElapsedTimeSinceLastUdpPacketArrivedDelegate();

		// Token: 0x020002D9 RID: 729
		// (Invoke) Token: 0x06000E54 RID: 3668
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndBroadcastModuleEventDelegate(int broadcastFlags, int targetPlayer, [MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x020002DA RID: 730
		// (Invoke) Token: 0x06000E58 RID: 3672
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndModuleEventAsClientDelegate([MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x020002DB RID: 731
		// (Invoke) Token: 0x06000E5C RID: 3676
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActiveUdpSessionsIpAddressDelegate();

		// Token: 0x020002DC RID: 732
		// (Invoke) Token: 0x06000E60 RID: 3680
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAveragePacketLossRatioDelegate();

		// Token: 0x020002DD RID: 733
		// (Invoke) Token: 0x06000E64 RID: 3684
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetDebugUploadsInBitsDelegate(ref GameNetwork.DebugNetworkPacketStatisticsStruct networkStatisticsStruct, ref GameNetwork.DebugNetworkPositionCompressionStatisticsStruct posStatisticsStruct);

		// Token: 0x020002DE RID: 734
		// (Invoke) Token: 0x06000E68 RID: 3688
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetMultiplayerDisabledDelegate();

		// Token: 0x020002DF RID: 735
		// (Invoke) Token: 0x06000E6C RID: 3692
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeClientSideDelegate(byte[] serverAddress, int port, int sessionKey, int playerIndex);

		// Token: 0x020002E0 RID: 736
		// (Invoke) Token: 0x06000E70 RID: 3696
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeServerSideDelegate(int port);

		// Token: 0x020002E1 RID: 737
		// (Invoke) Token: 0x06000E74 RID: 3700
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsDedicatedServerDelegate();

		// Token: 0x020002E2 RID: 738
		// (Invoke) Token: 0x06000E78 RID: 3704
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrepareNewUdpSessionDelegate(int player, int sessionKey);

		// Token: 0x020002E3 RID: 739
		// (Invoke) Token: 0x06000E7C RID: 3708
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrintDebugStatsDelegate();

		// Token: 0x020002E4 RID: 740
		// (Invoke) Token: 0x06000E80 RID: 3712
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrintReplicationTableStatisticsDelegate();

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x06000E84 RID: 3716
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int ReadByteArrayFromPacketDelegate(ManagedArray buffer, int offset, int bufferCapacity, [MarshalAs(UnmanagedType.U1)] ref bool bufferReadValid);

		// Token: 0x020002E6 RID: 742
		// (Invoke) Token: 0x06000E88 RID: 3720
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ReadFloatFromPacketDelegate(ref CompressionInfo.Float compressionInfo, out float output);

		// Token: 0x020002E7 RID: 743
		// (Invoke) Token: 0x06000E8C RID: 3724
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ReadIntFromPacketDelegate(ref CompressionInfo.Integer compressionInfo, out int output);

		// Token: 0x020002E8 RID: 744
		// (Invoke) Token: 0x06000E90 RID: 3728
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ReadLongFromPacketDelegate(ref CompressionInfo.LongInteger compressionInfo, out long output);

		// Token: 0x020002E9 RID: 745
		// (Invoke) Token: 0x06000E94 RID: 3732
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int ReadStringFromPacketDelegate([MarshalAs(UnmanagedType.U1)] ref bool bufferReadValid);

		// Token: 0x020002EA RID: 746
		// (Invoke) Token: 0x06000E98 RID: 3736
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ReadUintFromPacketDelegate(ref CompressionInfo.UnsignedInteger compressionInfo, out uint output);

		// Token: 0x020002EB RID: 747
		// (Invoke) Token: 0x06000E9C RID: 3740
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ReadUlongFromPacketDelegate(ref CompressionInfo.UnsignedLongInteger compressionInfo, out ulong output);

		// Token: 0x020002EC RID: 748
		// (Invoke) Token: 0x06000EA0 RID: 3744
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveBotOnServerDelegate(int botPlayerIndex);

		// Token: 0x020002ED RID: 749
		// (Invoke) Token: 0x06000EA4 RID: 3748
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetDebugUploadsDelegate();

		// Token: 0x020002EE RID: 750
		// (Invoke) Token: 0x06000EA8 RID: 3752
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetDebugVariablesDelegate();

		// Token: 0x020002EF RID: 751
		// (Invoke) Token: 0x06000EAC RID: 3756
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetMissionDataDelegate();

		// Token: 0x020002F0 RID: 752
		// (Invoke) Token: 0x06000EB0 RID: 3760
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ServerPingDelegate(byte[] serverAddress, int port);

		// Token: 0x020002F1 RID: 753
		// (Invoke) Token: 0x06000EB4 RID: 3764
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetServerBandwidthLimitInMbpsDelegate(double value);

		// Token: 0x020002F2 RID: 754
		// (Invoke) Token: 0x06000EB8 RID: 3768
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetServerFrameRateDelegate(double limit);

		// Token: 0x020002F3 RID: 755
		// (Invoke) Token: 0x06000EBC RID: 3772
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetServerTickRateDelegate(double value);

		// Token: 0x020002F4 RID: 756
		// (Invoke) Token: 0x06000EC0 RID: 3776
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TerminateClientSideDelegate();

		// Token: 0x020002F5 RID: 757
		// (Invoke) Token: 0x06000EC4 RID: 3780
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TerminateServerSideDelegate();

		// Token: 0x020002F6 RID: 758
		// (Invoke) Token: 0x06000EC8 RID: 3784
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteByteArrayToPacketDelegate(ManagedArray value, int offset, int size);

		// Token: 0x020002F7 RID: 759
		// (Invoke) Token: 0x06000ECC RID: 3788
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteFloatToPacketDelegate(float value, ref CompressionInfo.Float compressionInfo);

		// Token: 0x020002F8 RID: 760
		// (Invoke) Token: 0x06000ED0 RID: 3792
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteIntToPacketDelegate(int value, ref CompressionInfo.Integer compressionInfo);

		// Token: 0x020002F9 RID: 761
		// (Invoke) Token: 0x06000ED4 RID: 3796
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteLongToPacketDelegate(long value, ref CompressionInfo.LongInteger compressionInfo);

		// Token: 0x020002FA RID: 762
		// (Invoke) Token: 0x06000ED8 RID: 3800
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteStringToPacketDelegate(byte[] value);

		// Token: 0x020002FB RID: 763
		// (Invoke) Token: 0x06000EDC RID: 3804
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteUintToPacketDelegate(uint value, ref CompressionInfo.UnsignedInteger compressionInfo);

		// Token: 0x020002FC RID: 764
		// (Invoke) Token: 0x06000EE0 RID: 3808
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteUlongToPacketDelegate(ulong value, ref CompressionInfo.UnsignedLongInteger compressionInfo);
	}
}
