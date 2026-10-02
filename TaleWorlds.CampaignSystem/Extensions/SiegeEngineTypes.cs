using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000177 RID: 375
	public static class SiegeEngineTypes
	{
		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x0009050D File Offset: 0x0008E70D
		public static MBReadOnlyList<SiegeEngineType> All
		{
			get
			{
				return Campaign.Current.AllSiegeEngineTypes;
			}
		}
	}
}
