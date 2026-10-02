using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B3 RID: 435
	[ScriptingInterfaceBase]
	internal interface IMBNetwork
	{
		// Token: 0x060018CB RID: 6347
		[EngineMethod("get_multiplayer_disabled", false, null, false)]
		bool GetMultiplayerDisabled();

		// Token: 0x060018CC RID: 6348
		[EngineMethod("is_dedicated_server", false, null, false)]
		bool IsDedicatedServer();

		// Token: 0x060018CD RID: 6349
		[EngineMethod("initialize_server_side", false, null, false)]
		void InitializeServerSide(int port);

		// Token: 0x060018CE RID: 6350
		[EngineMethod("initialize_client_side", false, null, false)]
		void InitializeClientSide(string serverAddress, int port, int sessionKey, int playerIndex);

		// Token: 0x060018CF RID: 6351
		[EngineMethod("terminate_server_side", false, null, false)]
		void TerminateServerSide();

		// Token: 0x060018D0 RID: 6352
		[EngineMethod("terminate_client_side", false, null, false)]
		void TerminateClientSide();

		// Token: 0x060018D1 RID: 6353
		[EngineMethod("server_ping", false, null, false)]
		void ServerPing(string serverAddress, int port);

		// Token: 0x060018D2 RID: 6354
		[EngineMethod("add_peer_to_disconnect", false, null, false)]
		void AddPeerToDisconnect(int peer);

		// Token: 0x060018D3 RID: 6355
		[EngineMethod("prepare_new_udp_session", false, null, false)]
		void PrepareNewUdpSession(int player, int sessionKey);

		// Token: 0x060018D4 RID: 6356
		[EngineMethod("get_active_udp_sessions_ip_address", false, null, false)]
		string GetActiveUdpSessionsIpAddress();

		// Token: 0x060018D5 RID: 6357
		[EngineMethod("can_add_new_players_on_server", false, null, false)]
		bool CanAddNewPlayersOnServer(int numPlayers);

		// Token: 0x060018D6 RID: 6358
		[EngineMethod("add_new_player_on_server", false, null, false)]
		int AddNewPlayerOnServer(bool serverPlayer);

		// Token: 0x060018D7 RID: 6359
		[EngineMethod("add_new_bot_on_server", false, null, false)]
		int AddNewBotOnServer();

		// Token: 0x060018D8 RID: 6360
		[EngineMethod("remove_bot_on_server", false, null, false)]
		void RemoveBotOnServer(int botPlayerIndex);

		// Token: 0x060018D9 RID: 6361
		[EngineMethod("reset_mission_data", false, null, false)]
		void ResetMissionData();

		// Token: 0x060018DA RID: 6362
		[EngineMethod("begin_broadcast_module_event", false, null, false)]
		void BeginBroadcastModuleEvent();

		// Token: 0x060018DB RID: 6363
		[EngineMethod("end_broadcast_module_event", false, null, false)]
		void EndBroadcastModuleEvent(int broadcastFlags, int targetPlayer, bool isReliable);

		// Token: 0x060018DC RID: 6364
		[EngineMethod("elapsed_time_since_last_udp_packet_arrived", false, null, false)]
		double ElapsedTimeSinceLastUdpPacketArrived();

		// Token: 0x060018DD RID: 6365
		[EngineMethod("begin_module_event_as_client", false, null, false)]
		void BeginModuleEventAsClient(bool isReliable);

		// Token: 0x060018DE RID: 6366
		[EngineMethod("end_module_event_as_client", false, null, false)]
		void EndModuleEventAsClient(bool isReliable);

		// Token: 0x060018DF RID: 6367
		[EngineMethod("read_int_from_packet", false, null, false)]
		bool ReadIntFromPacket(ref CompressionInfo.Integer compressionInfo, out int output);

		// Token: 0x060018E0 RID: 6368
		[EngineMethod("read_uint_from_packet", false, null, false)]
		bool ReadUintFromPacket(ref CompressionInfo.UnsignedInteger compressionInfo, out uint output);

		// Token: 0x060018E1 RID: 6369
		[EngineMethod("read_long_from_packet", false, null, false)]
		bool ReadLongFromPacket(ref CompressionInfo.LongInteger compressionInfo, out long output);

		// Token: 0x060018E2 RID: 6370
		[EngineMethod("read_ulong_from_packet", false, null, false)]
		bool ReadUlongFromPacket(ref CompressionInfo.UnsignedLongInteger compressionInfo, out ulong output);

		// Token: 0x060018E3 RID: 6371
		[EngineMethod("read_float_from_packet", false, null, false)]
		bool ReadFloatFromPacket(ref CompressionInfo.Float compressionInfo, out float output);

		// Token: 0x060018E4 RID: 6372
		[EngineMethod("read_string_from_packet", false, null, false)]
		string ReadStringFromPacket(ref bool bufferReadValid);

		// Token: 0x060018E5 RID: 6373
		[EngineMethod("write_int_to_packet", false, null, false)]
		void WriteIntToPacket(int value, ref CompressionInfo.Integer compressionInfo);

		// Token: 0x060018E6 RID: 6374
		[EngineMethod("write_uint_to_packet", false, null, false)]
		void WriteUintToPacket(uint value, ref CompressionInfo.UnsignedInteger compressionInfo);

		// Token: 0x060018E7 RID: 6375
		[EngineMethod("write_long_to_packet", false, null, false)]
		void WriteLongToPacket(long value, ref CompressionInfo.LongInteger compressionInfo);

		// Token: 0x060018E8 RID: 6376
		[EngineMethod("write_ulong_to_packet", false, null, false)]
		void WriteUlongToPacket(ulong value, ref CompressionInfo.UnsignedLongInteger compressionInfo);

		// Token: 0x060018E9 RID: 6377
		[EngineMethod("write_float_to_packet", false, null, false)]
		void WriteFloatToPacket(float value, ref CompressionInfo.Float compressionInfo);

		// Token: 0x060018EA RID: 6378
		[EngineMethod("write_string_to_packet", false, null, false)]
		void WriteStringToPacket(string value);

		// Token: 0x060018EB RID: 6379
		[EngineMethod("read_byte_array_from_packet", false, null, false)]
		int ReadByteArrayFromPacket(byte[] buffer, int offset, int bufferCapacity, ref bool bufferReadValid);

		// Token: 0x060018EC RID: 6380
		[EngineMethod("write_byte_array_to_packet", false, null, false)]
		void WriteByteArrayToPacket(byte[] value, int offset, int size);

		// Token: 0x060018ED RID: 6381
		[EngineMethod("set_server_bandwidth_limit_in_mbps", false, null, false)]
		void SetServerBandwidthLimitInMbps(double value);

		// Token: 0x060018EE RID: 6382
		[EngineMethod("set_server_tick_rate", false, null, false)]
		void SetServerTickRate(double value);

		// Token: 0x060018EF RID: 6383
		[EngineMethod("reset_debug_variables", false, null, false)]
		void ResetDebugVariables();

		// Token: 0x060018F0 RID: 6384
		[EngineMethod("print_debug_stats", false, null, false)]
		void PrintDebugStats();

		// Token: 0x060018F1 RID: 6385
		[EngineMethod("get_average_packet_loss_ratio", false, null, false)]
		float GetAveragePacketLossRatio();

		// Token: 0x060018F2 RID: 6386
		[EngineMethod("get_debug_uploads_in_bits", false, null, false)]
		void GetDebugUploadsInBits(ref GameNetwork.DebugNetworkPacketStatisticsStruct networkStatisticsStruct, ref GameNetwork.DebugNetworkPositionCompressionStatisticsStruct posStatisticsStruct);

		// Token: 0x060018F3 RID: 6387
		[EngineMethod("reset_debug_uploads", false, null, false)]
		void ResetDebugUploads();

		// Token: 0x060018F4 RID: 6388
		[EngineMethod("print_replication_table_statistics", false, null, false)]
		void PrintReplicationTableStatistics();

		// Token: 0x060018F5 RID: 6389
		[EngineMethod("clear_replication_table_statistics", false, null, false)]
		void ClearReplicationTableStatistics();

		// Token: 0x060018F6 RID: 6390
		[EngineMethod("set_server_frame_rate", false, null, false)]
		void SetServerFrameRate(double limit);
	}
}
