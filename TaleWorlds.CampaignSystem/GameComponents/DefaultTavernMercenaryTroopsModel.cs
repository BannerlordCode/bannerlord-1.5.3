using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000164 RID: 356
	public class DefaultTavernMercenaryTroopsModel : TavernMercenaryTroopsModel
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001B60 RID: 7008 RVA: 0x0008CDDC File Offset: 0x0008AFDC
		public override float RegularMercenariesSpawnChance
		{
			get
			{
				return 0.7f;
			}
		}
	}
}
