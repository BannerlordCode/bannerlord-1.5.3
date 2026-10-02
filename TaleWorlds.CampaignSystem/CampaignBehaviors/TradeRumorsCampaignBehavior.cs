using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200046B RID: 1131
	public class TradeRumorsCampaignBehavior : CampaignBehaviorBase, ITradeRumorCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x0016E524 File Offset: 0x0016C724
		public IEnumerable<TradeRumor> TradeRumors
		{
			get
			{
				foreach (TradeRumor tradeRumor in this._tradeRumors)
				{
					if (!tradeRumor.IsExpired())
					{
						yield return tradeRumor;
					}
				}
				List<TradeRumor>.Enumerator enumerator = default(List<TradeRumor>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x0016E534 File Offset: 0x0016C734
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Settlement, CampaignTime>>("_enteredSettlements", ref this._enteredSettlements);
			dataStore.SyncData<List<TradeRumor>>("_tradeRumors", ref this._tradeRumors);
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x0016E55C File Offset: 0x0016C75C
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnTradeRumorIsTakenEvent.AddNonSerializedListener(this, new Action<List<TradeRumor>, Settlement>(this.OnTradeRumorIsTaken));
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x0016E5C5 File Offset: 0x0016C7C5
		public void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
			this.AddTradeRumors(newRumors, sourceSettlement);
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x0016E5D0 File Offset: 0x0016C7D0
		public void AddTradeRumors(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
			bool flag = true;
			foreach (TradeRumor tradeRumor in newRumors)
			{
				foreach (TradeRumor tradeRumor2 in this.TradeRumors)
				{
					if (tradeRumor2.Settlement == tradeRumor.Settlement && tradeRumor2.ItemCategory == tradeRumor.ItemCategory)
					{
						flag = false;
					}
				}
				if (flag)
				{
					this._tradeRumors.Add(tradeRumor);
				}
			}
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x0016E680 File Offset: 0x0016C880
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x0016E682 File Offset: 0x0016C882
		public void DailyTick()
		{
			this.AddDailyTradeRumors(1);
			this.DeleteExpiredRumors();
			this.DeleteExpiredEnteredSettlements();
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x0016E698 File Offset: 0x0016C898
		private void DeleteExpiredEnteredSettlements()
		{
			List<Settlement> list = new List<Settlement>();
			foreach (KeyValuePair<Settlement, CampaignTime> keyValuePair in this._enteredSettlements)
			{
				if (CampaignTime.Now - keyValuePair.Value >= CampaignTime.Days(1f))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (Settlement settlement in list)
			{
				this._enteredSettlements.Remove(settlement);
			}
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x0016E760 File Offset: 0x0016C960
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			bool flag = mobileParty != null && mobileParty.IsCaravan && mobileParty.Party.Owner != null && mobileParty.Party.Owner.Clan == Clan.PlayerClan && Hero.MainHero.GetPerkValue(DefaultPerks.Trade.TravelingRumors);
			if (mobileParty == null || (!mobileParty.IsMainParty && !flag) || !settlement.IsTown)
			{
				return;
			}
			Town town = settlement.Town;
			if (((town != null) ? town.MarketData : null) == null)
			{
				return;
			}
			if (!this._enteredSettlements.ContainsKey(settlement) || (this._enteredSettlements.ContainsKey(settlement) && CampaignTime.Now - this._enteredSettlements[settlement] >= CampaignTime.Days(1f)))
			{
				List<TradeRumor> list = new List<TradeRumor>();
				IEnumerable<TradeRumor> tradeRumors = this._tradeRumors;
				Func<TradeRumor, bool> <>9__0;
				Func<TradeRumor, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (TradeRumor x) => x.Settlement == settlement);
				}
				foreach (TradeRumor tradeRumor in tradeRumors.Where<TradeRumor>(func))
				{
					list.Add(tradeRumor);
				}
				foreach (TradeRumor tradeRumor2 in list)
				{
					this._tradeRumors.Remove(tradeRumor2);
				}
				List<TradeRumor> list2 = new List<TradeRumor>();
				foreach (ItemObject itemObject in Items.AllTradeGoods)
				{
					list2.Add(new TradeRumor(settlement, itemObject, settlement.Town.GetItemPrice(itemObject, null, false), settlement.Town.GetItemPrice(itemObject, null, true), 10));
				}
				this.AddTradeRumors(list2, settlement);
				if (!this._enteredSettlements.ContainsKey(settlement))
				{
					this._enteredSettlements.Add(settlement, CampaignTime.Now);
					return;
				}
				this._enteredSettlements[settlement] = CampaignTime.Now;
			}
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x0016E9D0 File Offset: 0x0016CBD0
		public void DeleteExpiredRumors()
		{
			List<TradeRumor> list = new List<TradeRumor>();
			foreach (TradeRumor tradeRumor in this._tradeRumors.Where<TradeRumor>((TradeRumor x) => x.IsExpired()))
			{
				list.Add(tradeRumor);
			}
			foreach (TradeRumor tradeRumor2 in list)
			{
				this._tradeRumors.Remove(tradeRumor2);
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x0016EA8C File Offset: 0x0016CC8C
		public void AddDailyTradeRumors(int numberOfTradeRumors)
		{
			int num = 0;
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.Type == ItemObject.ItemTypeEnum.Goods || itemObject.Type == ItemObject.ItemTypeEnum.Horse)
				{
					num++;
				}
			}
			int count = Campaign.Current.AllTowns.Count;
			List<TradeRumor> list = new List<TradeRumor>();
			for (int i = 0; i < numberOfTradeRumors; i++)
			{
				int num2 = MBRandom.RandomInt(count);
				int num3 = MBRandom.RandomInt(num);
				foreach (Town town in Campaign.Current.AllTowns)
				{
					num2--;
					if (num2 < 0)
					{
						using (List<ItemObject>.Enumerator enumerator = Items.All.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ItemObject itemObject2 = enumerator.Current;
								if (itemObject2.Type == ItemObject.ItemTypeEnum.Goods || itemObject2.Type == ItemObject.ItemTypeEnum.Horse)
								{
									num3--;
									if (num3 < 0)
									{
										list.Add(new TradeRumor(town.Settlement, itemObject2, town.GetItemPrice(itemObject2, null, false), town.GetItemPrice(itemObject2, null, true), 10 + MBRandom.RandomInt(10)));
										break;
									}
								}
							}
							break;
						}
					}
				}
				if (Hero.MainHero.GetPerkValue(DefaultPerks.Trade.Tollgates))
				{
					foreach (Workshop workshop in Hero.MainHero.OwnedWorkshops)
					{
						foreach (ItemObject itemObject3 in Items.AllTradeGoods)
						{
							list.Add(new TradeRumor(workshop.Settlement, itemObject3, workshop.Settlement.Town.GetItemPrice(itemObject3, null, false), workshop.Settlement.Town.GetItemPrice(itemObject3, null, true), 10));
						}
					}
				}
			}
			this.AddTradeRumors(list, null);
		}

		// Token: 0x0400149A RID: 5274
		private List<TradeRumor> _tradeRumors = new List<TradeRumor>();

		// Token: 0x0400149B RID: 5275
		private Dictionary<Settlement, CampaignTime> _enteredSettlements = new Dictionary<Settlement, CampaignTime>();
	}
}
