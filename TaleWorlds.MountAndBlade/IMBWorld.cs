using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BB RID: 443
	[ScriptingInterfaceBase]
	internal interface IMBWorld
	{
		// Token: 0x06001927 RID: 6439
		[EngineMethod("get_global_time", false, null, false)]
		float GetGlobalTime(MBCommon.TimeType timeType);

		// Token: 0x06001928 RID: 6440
		[EngineMethod("get_last_messages", false, null, false)]
		string GetLastMessages();

		// Token: 0x06001929 RID: 6441
		[EngineMethod("get_game_type", false, null, false)]
		int GetGameType();

		// Token: 0x0600192A RID: 6442
		[EngineMethod("set_game_type", false, null, false)]
		void SetGameType(int gameType);

		// Token: 0x0600192B RID: 6443
		[EngineMethod("pause_game", false, null, false)]
		void PauseGame();

		// Token: 0x0600192C RID: 6444
		[EngineMethod("unpause_game", false, null, false)]
		void UnpauseGame();

		// Token: 0x0600192D RID: 6445
		[EngineMethod("set_mesh_used", false, null, false)]
		void SetMeshUsed(string meshName);

		// Token: 0x0600192E RID: 6446
		[EngineMethod("set_material_used", false, null, false)]
		void SetMaterialUsed(string materialName);

		// Token: 0x0600192F RID: 6447
		[EngineMethod("set_body_used", false, null, false)]
		void SetBodyUsed(string bodyName);

		// Token: 0x06001930 RID: 6448
		[EngineMethod("fix_skeletons", false, null, false)]
		void FixSkeletons();

		// Token: 0x06001931 RID: 6449
		[EngineMethod("check_resource_modifications", false, null, false)]
		void CheckResourceModifications();
	}
}
