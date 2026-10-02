using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AF RID: 431
	[ScriptingInterfaceBase]
	internal interface IMBItem
	{
		// Token: 0x06001831 RID: 6193
		[EngineMethod("get_item_usage_index", false, null, false)]
		int GetItemUsageIndex(string itemusagename);

		// Token: 0x06001832 RID: 6194
		[EngineMethod("get_item_holster_index", false, null, false)]
		int GetItemHolsterIndex(string itemholstername);

		// Token: 0x06001833 RID: 6195
		[EngineMethod("get_item_is_passive_usage", false, null, false)]
		bool GetItemIsPassiveUsage(string itemUsageName);

		// Token: 0x06001834 RID: 6196
		[EngineMethod("get_holster_frame_by_index", false, null, false)]
		void GetHolsterFrameByIndex(int index, ref MatrixFrame outFrame);

		// Token: 0x06001835 RID: 6197
		[EngineMethod("get_item_usage_set_flags", false, null, false)]
		int GetItemUsageSetFlags(string ItemUsageName);

		// Token: 0x06001836 RID: 6198
		[EngineMethod("get_item_usage_reload_action_code", false, null, false)]
		int GetItemUsageReloadActionCode(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection);

		// Token: 0x06001837 RID: 6199
		[EngineMethod("get_item_usage_strike_type", false, null, false)]
		int GetItemUsageStrikeType(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection);

		// Token: 0x06001838 RID: 6200
		[EngineMethod("get_missile_range", false, null, false)]
		float GetMissileRange(float shootSpeed, float zDiff);
	}
}
