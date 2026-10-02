using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200046A RID: 1130
	public class TradeCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004923 RID: 18723 RVA: 0x0016E20E File Offset: 0x0016C40E
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeMarkets();
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x0016E218 File Offset: 0x0016C418
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, new Action<Town>(this.DailyTickTown));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x0016E284 File Offset: 0x0016C484
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter campaignGameStarter, int i)
		{
			if (i == 2)
			{
				this.InitializeTrade();
			}
			if (i % 10 == 0)
			{
				foreach (Town town in Campaign.Current.AllTowns)
				{
					this.UpdateMarketStores(town);
				}
			}
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0016E2EC File Offset: 0x0016C4EC
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			foreach (Town town in Town.AllTowns)
			{
				this.UpdateMarketStores(town);
			}
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x0016E340 File Offset: 0x0016C540
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<ItemCategory, float>>("_numberOfTotalItemsAtGameWorld", ref this._numberOfTotalItemsAtGameWorld);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x0016E354 File Offset: 0x0016C554
		private void InitializeTrade()
		{
			this._numberOfTotalItemsAtGameWorld = new Dictionary<ItemCategory, float>();
			Campaign.Current.Settlements.Where<Settlement>((Settlement settlement) => settlement.IsTown).ToList<Settlement>();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.CharacterObject.Occupation == Occupation.Lord && hero.Clan != Clan.PlayerClan)
				{
					Clan clan = hero.Clan;
					int num;
					if (((clan != null) ? clan.Leader : null) == hero)
					{
						num = 50000 + 10000 * hero.Clan.Tier + ((hero == hero.MapFaction.Leader) ? 50000 : 0);
					}
					else
					{
						num = 10000;
					}
					GiveGoldAction.ApplyBetweenCharacters(null, hero, num, false);
				}
			}
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x0016E454 File Offset: 0x0016C654
		public void DailyTickTown(Town town)
		{
			this.UpdateMarketStores(town);
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x0016E45D File Offset: 0x0016C65D
		private void UpdateMarketStores(Town town)
		{
			town.MarketData.UpdateStores();
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x0016E46C File Offset: 0x0016C66C
		private void InitializeMarkets()
		{
			foreach (Town town in Town.AllTowns)
			{
				foreach (ItemCategory itemCategory in ItemCategories.All)
				{
					if (itemCategory.IsValid)
					{
						town.MarketData.AddDemand(itemCategory, 3f);
						town.MarketData.AddSupply(itemCategory, 2f);
					}
				}
			}
		}

		// Token: 0x04001497 RID: 5271
		private Dictionary<ItemCategory, float> _numberOfTotalItemsAtGameWorld;

		// Token: 0x04001498 RID: 5272
		public const float MaximumTaxRatioForVillages = 1f;

		// Token: 0x04001499 RID: 5273
		public const float MaximumTaxRatioForTowns = 0.5f;

		// Token: 0x020008AA RID: 2218
		public enum TradeGoodType
		{
			// Token: 0x040025B8 RID: 9656
			Grain,
			// Token: 0x040025B9 RID: 9657
			Wood,
			// Token: 0x040025BA RID: 9658
			Meat,
			// Token: 0x040025BB RID: 9659
			Wool,
			// Token: 0x040025BC RID: 9660
			Cheese,
			// Token: 0x040025BD RID: 9661
			Iron,
			// Token: 0x040025BE RID: 9662
			Salt,
			// Token: 0x040025BF RID: 9663
			Spice,
			// Token: 0x040025C0 RID: 9664
			Raw_Silk,
			// Token: 0x040025C1 RID: 9665
			Fish,
			// Token: 0x040025C2 RID: 9666
			Flax,
			// Token: 0x040025C3 RID: 9667
			Grape,
			// Token: 0x040025C4 RID: 9668
			Hides,
			// Token: 0x040025C5 RID: 9669
			Clay,
			// Token: 0x040025C6 RID: 9670
			Date_Fruit,
			// Token: 0x040025C7 RID: 9671
			Bread,
			// Token: 0x040025C8 RID: 9672
			Beer,
			// Token: 0x040025C9 RID: 9673
			Wine,
			// Token: 0x040025CA RID: 9674
			Tools,
			// Token: 0x040025CB RID: 9675
			Pottery,
			// Token: 0x040025CC RID: 9676
			Cloth,
			// Token: 0x040025CD RID: 9677
			Linen,
			// Token: 0x040025CE RID: 9678
			Leather,
			// Token: 0x040025CF RID: 9679
			Velvet,
			// Token: 0x040025D0 RID: 9680
			Saddle_Horse,
			// Token: 0x040025D1 RID: 9681
			Steppe_Horse,
			// Token: 0x040025D2 RID: 9682
			Hunter,
			// Token: 0x040025D3 RID: 9683
			Desert_Horse,
			// Token: 0x040025D4 RID: 9684
			Charger,
			// Token: 0x040025D5 RID: 9685
			War_Horse,
			// Token: 0x040025D6 RID: 9686
			Steppe_Charger,
			// Token: 0x040025D7 RID: 9687
			Desert_War_Horse,
			// Token: 0x040025D8 RID: 9688
			Unknown,
			// Token: 0x040025D9 RID: 9689
			NumberOfTradeItems
		}
	}
}
