using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000444 RID: 1092
	public class PartiesBuyFoodCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600462C RID: 17964 RVA: 0x00155545 File Offset: 0x00153745
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x00155575 File Offset: 0x00153775
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00155578 File Offset: 0x00153778
		private void TryBuyingFood(MobileParty mobileParty, Settlement settlement)
		{
			if (Campaign.Current.GameStarted && mobileParty.LeaderHero != null && (settlement.IsTown || settlement.IsVillage) && Campaign.Current.Models.MobilePartyFoodConsumptionModel.DoesPartyConsumeFood(mobileParty) && (mobileParty.Army == null || mobileParty.AttachedTo == null || mobileParty.Army.LeaderParty == mobileParty) && (settlement.IsVillage || (mobileParty.MapFaction != null && !mobileParty.MapFaction.IsAtWarWith(settlement.MapFaction))) && settlement.ItemRoster.TotalFood > 0)
			{
				PartyFoodBuyingModel partyFoodBuyingModel = Campaign.Current.Models.PartyFoodBuyingModel;
				float num = (settlement.IsVillage ? partyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromVillage : partyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromTown);
				if (mobileParty.Army == null || (mobileParty.AttachedTo == null && mobileParty.Army.LeaderParty != mobileParty))
				{
					this.BuyFoodInternal(mobileParty, settlement, this.CalculateFoodCountToBuy(mobileParty, num));
					return;
				}
				this.BuyFoodForArmy(mobileParty, settlement, num);
			}
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00155688 File Offset: 0x00153888
		private int CalculateFoodCountToBuy(MobileParty mobileParty, float minimumDaysToLast)
		{
			if (mobileParty.FoodChange.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return 0;
			}
			float num = (float)mobileParty.TotalFoodAtInventory / -mobileParty.FoodChange;
			float num2 = minimumDaysToLast - num;
			if (num2 > 0f)
			{
				return (int)(-mobileParty.FoodChange * num2);
			}
			return 0;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x001556D8 File Offset: 0x001538D8
		private void BuyFoodInternal(MobileParty mobileParty, Settlement settlement, int numberOfFoodItemsNeededToBuy)
		{
			if (!mobileParty.IsMainParty)
			{
				for (int i = 0; i < numberOfFoodItemsNeededToBuy; i++)
				{
					ItemRosterElement itemRosterElement;
					float num;
					Campaign.Current.Models.PartyFoodBuyingModel.FindItemToBuy(mobileParty, settlement, out itemRosterElement, out num);
					if (itemRosterElement.EquipmentElement.Item == null)
					{
						break;
					}
					if (num <= (float)mobileParty.PartyTradeGold)
					{
						SellItemsAction.Apply(settlement.Party, mobileParty.Party, itemRosterElement, 1, null);
					}
					if (itemRosterElement.EquipmentElement.Item.HasHorseComponent && itemRosterElement.EquipmentElement.Item.HorseComponent.IsLiveStock)
					{
						i += itemRosterElement.EquipmentElement.Item.HorseComponent.MeatCount - 1;
					}
				}
			}
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0015579C File Offset: 0x0015399C
		private void BuyFoodForArmy(MobileParty mobileParty, Settlement settlement, float minimumDaysToLast)
		{
			float num = mobileParty.Army.LeaderParty.FoodChange;
			foreach (MobileParty mobileParty2 in mobileParty.Army.LeaderParty.AttachedParties)
			{
				num += mobileParty2.FoodChange;
			}
			List<ValueTuple<int, int>> list = new List<ValueTuple<int, int>>(mobileParty.Army.Parties.Count);
			float num2 = mobileParty.Army.LeaderParty.FoodChange / num;
			int num3 = this.CalculateFoodCountToBuy(mobileParty.Army.LeaderParty, minimumDaysToLast);
			list.Add(new ValueTuple<int, int>((int)((float)settlement.ItemRoster.TotalFood * num2), num3));
			int num4 = num3;
			foreach (MobileParty mobileParty3 in mobileParty.Army.LeaderParty.AttachedParties)
			{
				num2 = mobileParty3.FoodChange / num;
				num3 = this.CalculateFoodCountToBuy(mobileParty3, minimumDaysToLast);
				list.Add(new ValueTuple<int, int>((int)((float)settlement.ItemRoster.TotalFood * num2), num3));
				num4 += num3;
			}
			bool flag = settlement.ItemRoster.TotalFood < num4;
			int num5 = 0;
			foreach (ValueTuple<int, int> valueTuple in list)
			{
				int num6 = (flag ? valueTuple.Item1 : valueTuple.Item2);
				MobileParty mobileParty4 = ((num5 == 0) ? mobileParty.Army.LeaderParty : mobileParty.Army.LeaderParty.AttachedParties[num5 - 1]);
				if (!mobileParty4.IsMainParty)
				{
					this.BuyFoodInternal(mobileParty4, settlement, num6);
				}
				num5++;
			}
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00155990 File Offset: 0x00153B90
		public void HourlyTickParty(MobileParty mobileParty)
		{
			Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty);
			if (currentSettlementOfMobilePartyForAICalculation != null)
			{
				this.TryBuyingFood(mobileParty, currentSettlementOfMobilePartyForAICalculation);
			}
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x001559AF File Offset: 0x00153BAF
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (mobileParty != null)
			{
				this.TryBuyingFood(mobileParty, settlement);
			}
		}
	}
}
