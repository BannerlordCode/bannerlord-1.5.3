using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F0 RID: 496
	public abstract class SiegeAftermathModel : MBGameModel<SiegeAftermathModel>
	{
		// Token: 0x06001FA6 RID: 8102
		public abstract int GetSiegeAftermathTraitXpChangeForPlayer(TraitObject trait, Settlement devastatedSettlement, SiegeAftermathAction.SiegeAftermath aftermathType);
	}
}
