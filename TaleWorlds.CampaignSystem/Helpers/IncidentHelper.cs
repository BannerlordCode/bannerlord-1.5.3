using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x02000018 RID: 24
	public static class IncidentHelper
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000B090 File Offset: 0x00009290
		public static T GetSeededRandomElement<T>(List<T> list, long seed)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			return list[MobileParty.MainParty.RandomIntWithSeed((uint)seed, list.Count)];
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000B0CC File Offset: 0x000092CC
		public static T GetSeededRandomElement<T>(MBList<T> list, long seed)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			return list[MobileParty.MainParty.RandomIntWithSeed((uint)seed, list.Count)];
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000B108 File Offset: 0x00009308
		public static T GetSeededRandomElement<T>(MBReadOnlyList<T> list, long seed)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			return list[MobileParty.MainParty.RandomIntWithSeed((uint)seed, list.Count)];
		}
	}
}
