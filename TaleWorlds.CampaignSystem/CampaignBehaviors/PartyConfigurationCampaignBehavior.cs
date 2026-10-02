using System;
using TaleWorlds.CampaignSystem.Actions;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000448 RID: 1096
	public class PartyConfigurationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004646 RID: 17990 RVA: 0x001562F8 File Offset: 0x001544F8
		public override void RegisterEvents()
		{
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
			CampaignEvents.OnBeforePlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnBeforePlayerCharacterChanged));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0015634A File Offset: 0x0015454A
		private void OnCompanionRemoved(Hero hero, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (hero != null)
			{
				hero.ResetPartyConfiguration();
			}
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x00156355 File Offset: 0x00154555
		private void OnBeforePlayerCharacterChanged(Hero _, Hero hero)
		{
			if (hero != null)
			{
				hero.ResetPartyConfiguration();
			}
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x00156360 File Offset: 0x00154560
		private void OnHeroChangedClan(Hero hero, Clan clan)
		{
			if (clan == Clan.PlayerClan)
			{
				hero.ResetPartyConfiguration();
			}
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x00156370 File Offset: 0x00154570
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
