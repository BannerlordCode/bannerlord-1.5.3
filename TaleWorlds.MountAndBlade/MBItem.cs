using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D2 RID: 466
	public static class MBItem
	{
		// Token: 0x06001C0B RID: 7179 RVA: 0x000612DE File Offset: 0x0005F4DE
		public static int GetItemUsageIndex(string itemUsageName)
		{
			return MBAPI.IMBItem.GetItemUsageIndex(itemUsageName);
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000612EB File Offset: 0x0005F4EB
		public static int GetItemHolsterIndex(string itemHolsterName)
		{
			return MBAPI.IMBItem.GetItemHolsterIndex(itemHolsterName);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000612F8 File Offset: 0x0005F4F8
		public static bool GetItemIsPassiveUsage(string itemUsageName)
		{
			return MBAPI.IMBItem.GetItemIsPassiveUsage(itemUsageName);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x00061308 File Offset: 0x0005F508
		public static MatrixFrame GetHolsterFrameByIndex(int index)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBItem.GetHolsterFrameByIndex(index, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x0006132B File Offset: 0x0005F52B
		public static ItemObject.ItemUsageSetFlags GetItemUsageSetFlags(string ItemUsageName)
		{
			return (ItemObject.ItemUsageSetFlags)MBAPI.IMBItem.GetItemUsageSetFlags(ItemUsageName);
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00061338 File Offset: 0x0005F538
		public static ActionIndexCache GetItemUsageReloadActionCode(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection)
		{
			return new ActionIndexCache(MBAPI.IMBItem.GetItemUsageReloadActionCode(itemUsageName, usageDirection, isMounted, leftHandUsageSetIndex, isLeftStance, isLowLookDirection));
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00061351 File Offset: 0x0005F551
		public static int GetItemUsageStrikeType(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection)
		{
			return MBAPI.IMBItem.GetItemUsageStrikeType(itemUsageName, usageDirection, isMounted, leftHandUsageSetIndex, isLeftStance, isLowLookDirection);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x00061365 File Offset: 0x0005F565
		public static float GetMissileRange(float shotSpeed, float zDiff)
		{
			return MBAPI.IMBItem.GetMissileRange(shotSpeed, zDiff);
		}
	}
}
