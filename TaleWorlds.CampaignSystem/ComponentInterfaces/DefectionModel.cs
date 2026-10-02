using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A4 RID: 420
	public abstract class DefectionModel : MBGameModel<DefaultDefectionModel>
	{
		// Token: 0x06001D24 RID: 7460
		public abstract bool CanHeroDefectToFaction(Hero hero, Kingdom kingdom);
	}
}
