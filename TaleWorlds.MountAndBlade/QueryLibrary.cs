using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000180 RID: 384
	public static class QueryLibrary
	{
		// Token: 0x06001457 RID: 5207 RVA: 0x0004A803 File Offset: 0x00048A03
		public static bool IsInfantry(Agent a)
		{
			return !a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0004A818 File Offset: 0x00048A18
		public static bool IsInfantryWithoutBanner(Agent a)
		{
			return a.Banner == null && !a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0004A835 File Offset: 0x00048A35
		public static bool HasShield(Agent a)
		{
			return a.HasShieldCached;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0004A83D File Offset: 0x00048A3D
		public static bool IsRanged(Agent a)
		{
			return !a.HasMount && a.IsRangedCached;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0004A84F File Offset: 0x00048A4F
		public static bool IsRangedWithoutBanner(Agent a)
		{
			return a.Banner == null && !a.HasMount && a.IsRangedCached;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004A869 File Offset: 0x00048A69
		public static bool IsCavalry(Agent a)
		{
			return a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0004A87E File Offset: 0x00048A7E
		public static bool IsCavalryWithoutBanner(Agent a)
		{
			return a.Banner == null && a.HasMount && !a.IsRangedCached;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0004A89B File Offset: 0x00048A9B
		public static bool IsRangedCavalry(Agent a)
		{
			return a.HasMount && a.IsRangedCached;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004A8AD File Offset: 0x00048AAD
		public static bool IsRangedCavalryWithoutBanner(Agent a)
		{
			return a.Banner == null && a.HasMount && a.IsRangedCached;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004A8C7 File Offset: 0x00048AC7
		public static bool HasSpear(Agent a)
		{
			return a.HasSpearCached;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004A8CF File Offset: 0x00048ACF
		public static bool HasThrown(Agent a)
		{
			return a.HasThrownCached;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004A8D7 File Offset: 0x00048AD7
		public static bool IsHeavy(Agent a)
		{
			return MissionGameModels.Current.AgentStatCalculateModel.HasHeavyArmor(a);
		}
	}
}
