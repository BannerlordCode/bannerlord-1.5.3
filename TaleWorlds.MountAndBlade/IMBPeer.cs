using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B4 RID: 436
	[ScriptingInterfaceBase]
	internal interface IMBPeer
	{
		// Token: 0x060018F7 RID: 6391
		[EngineMethod("set_user_data", false, null, false)]
		void SetUserData(int index, MBNetworkPeer data);

		// Token: 0x060018F8 RID: 6392
		[EngineMethod("set_controlled_agent", false, null, false)]
		void SetControlledAgent(int index, UIntPtr missionPointer, int agentIndex);

		// Token: 0x060018F9 RID: 6393
		[EngineMethod("set_team", false, null, false)]
		void SetTeam(int index, int teamIndex);

		// Token: 0x060018FA RID: 6394
		[EngineMethod("is_active", false, null, false)]
		bool IsActive(int index);

		// Token: 0x060018FB RID: 6395
		[EngineMethod("set_is_synchronized", false, null, false)]
		void SetIsSynchronized(int index, bool value);

		// Token: 0x060018FC RID: 6396
		[EngineMethod("get_is_synchronized", false, null, false)]
		bool GetIsSynchronized(int index);

		// Token: 0x060018FD RID: 6397
		[EngineMethod("debug_refresh_disconnect_timeout", false, null, false)]
		void DebugRefreshDisconnectTimeout(int index);

		// Token: 0x060018FE RID: 6398
		[EngineMethod("send_existing_objects", false, null, false)]
		void SendExistingObjects(int index, UIntPtr missionPointer);

		// Token: 0x060018FF RID: 6399
		[EngineMethod("begin_module_event", false, null, false)]
		void BeginModuleEvent(int index, bool isReliable);

		// Token: 0x06001900 RID: 6400
		[EngineMethod("end_module_event", false, null, false)]
		void EndModuleEvent(bool isReliable);

		// Token: 0x06001901 RID: 6401
		[EngineMethod("get_average_ping_in_milliseconds", false, null, false)]
		double GetAveragePingInMilliseconds(int index);

		// Token: 0x06001902 RID: 6402
		[EngineMethod("get_average_loss_percent", false, null, false)]
		double GetAverageLossPercent(int index);

		// Token: 0x06001903 RID: 6403
		[EngineMethod("set_relevant_game_options", false, null, false)]
		void SetRelevantGameOptions(int index, bool sendMeBloodEvents, bool sendMeSoundEvents);

		// Token: 0x06001904 RID: 6404
		[EngineMethod("get_reversed_host", false, null, false)]
		uint GetReversedHost(int index);

		// Token: 0x06001905 RID: 6405
		[EngineMethod("get_host", false, null, false)]
		uint GetHost(int index);

		// Token: 0x06001906 RID: 6406
		[EngineMethod("get_port", false, null, false)]
		ushort GetPort(int index);
	}
}
