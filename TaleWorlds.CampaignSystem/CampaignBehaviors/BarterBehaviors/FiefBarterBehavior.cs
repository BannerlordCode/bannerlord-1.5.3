using System;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors
{
	// Token: 0x02000489 RID: 1161
	public class FiefBarterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B05 RID: 19205 RVA: 0x0017A8C3 File Offset: 0x00178AC3
		public override void RegisterEvents()
		{
			CampaignEvents.BarterablesRequested.AddNonSerializedListener(this, new Action<BarterData>(this.CheckForBarters));
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x0017A8DC File Offset: 0x00178ADC
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x0017A8E0 File Offset: 0x00178AE0
		public void CheckForBarters(BarterData args)
		{
			if (args.OffererHero != null && args.OtherHero != null && args.OffererHero.GetPerkValue(DefaultPerks.Trade.EverythingHasAPrice) && (!args.OtherHero.Clan.IsMinorFaction || args.OtherHero.Clan == Clan.PlayerClan) && !args.OtherHero.Clan.IsUnderMercenaryService && !args.OffererHero.Clan.IsUnderMercenaryService)
			{
				foreach (Town town in Town.AllFiefs)
				{
					Clan ownerClan = town.OwnerClan;
					if (((ownerClan != null) ? ownerClan.Leader : null) == args.OffererHero)
					{
						Barterable barterable = new FiefBarterable(town.Settlement, args.OffererHero, args.OtherHero);
						args.AddBarterable<FiefBarterGroup>(barterable, false);
					}
					else
					{
						Clan ownerClan2 = town.OwnerClan;
						if (((ownerClan2 != null) ? ownerClan2.Leader : null) == args.OtherHero)
						{
							Barterable barterable2 = new FiefBarterable(town.Settlement, args.OtherHero, args.OffererHero);
							args.AddBarterable<FiefBarterGroup>(barterable2, false);
						}
					}
				}
			}
		}
	}
}
