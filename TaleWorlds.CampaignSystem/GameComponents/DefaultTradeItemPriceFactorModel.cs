using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000167 RID: 359
	public class DefaultTradeItemPriceFactorModel : TradeItemPriceFactorModel
	{
		// Token: 0x06001B83 RID: 7043 RVA: 0x0008DE50 File Offset: 0x0008C050
		public override float GetTradePenalty(ItemObject item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStore, float supply, float demand)
		{
			Settlement settlement = ((merchant != null) ? merchant.Settlement : null);
			float num = 0.06f;
			bool flag = clientParty != null && clientParty.IsCaravan;
			bool flag2;
			if (merchant == null)
			{
				flag2 = false;
			}
			else
			{
				MobileParty mobileParty = merchant.MobileParty;
				bool? flag3 = ((mobileParty != null) ? new bool?(mobileParty.IsCaravan) : null);
				bool flag4 = true;
				flag2 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
			}
			if (clientParty != null && merchant != null && clientParty.MapFaction.IsAtWarWith(merchant.MapFaction))
			{
				num += 0.5f;
			}
			if (!item.IsTradeGood && !item.IsAnimal && !item.HasHorseComponent && !flag && isSelling)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(1.5f + Math.Max(0f, item.Tierf - 1f) * 0.25f, false, null);
				if (item.IsCraftedWeapon && item.IsCraftedByPlayer && clientParty != null)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Crafting.ArtisanSmith, clientParty, true, ref explainedNumber);
				}
				num += explainedNumber.ResultNumber;
			}
			if (item.HasHorseComponent && item.HorseComponent.IsPackAnimal && !flag && isSelling)
			{
				num += 0.8f;
			}
			if (item.HasHorseComponent && item.HorseComponent.IsMount && !flag && isSelling)
			{
				num += 0.8f;
			}
			if (settlement != null && settlement.IsVillage)
			{
				num += (isSelling ? 1f : 0.1f);
			}
			if (flag2)
			{
				if (item.ItemCategory == DefaultItemCategories.PackAnimal && !isSelling)
				{
					num += 2f;
				}
				num += (isSelling ? 1f : 0.1f);
			}
			bool flag5 = clientParty == null;
			if (flag)
			{
				num *= 0.5f;
			}
			else if (flag5)
			{
				num *= 0.2f;
			}
			float num2 = ((clientParty != null) ? Campaign.Current.Models.PartyTradeModel.GetTradePenaltyFactor(clientParty) : 1f);
			num *= num2;
			ExplainedNumber explainedNumber2 = new ExplainedNumber(num, false, null);
			if (clientParty != null)
			{
				if (settlement != null && clientParty.MapFaction == settlement.MapFaction)
				{
					if (settlement.IsVillage)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.VillageNetwork, clientParty, true, ref explainedNumber2);
					}
					else if (settlement.IsTown)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.RumourNetwork, clientParty, true, ref explainedNumber2);
					}
				}
				if (item.IsTradeGood)
				{
					if (isSelling)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.WholeSeller, clientParty, true, ref explainedNumber2);
					}
					if (isSelling && item.IsFood)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.GranaryAccountant, clientParty, true, ref explainedNumber2);
					}
				}
				else if (!item.IsTradeGood && isSelling)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.Appraiser, clientParty, true, ref explainedNumber2);
				}
				if (PartyBaseHelper.HasFeat(clientParty.Party, DefaultCulturalFeats.AseraiTraderFeat))
				{
					explainedNumber2.AddFactor(-0.1f, null);
				}
				if (item.WeaponComponent != null && isSelling)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.ArmsDealer, clientParty, true, ref explainedNumber2);
				}
				if (!isSelling && item.IsFood)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.InsurancePlans, clientParty, false, ref explainedNumber2);
				}
				if (item.HorseComponent != null && item.HorseComponent.IsPackAnimal)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.ArenicosMules, clientParty, false, ref explainedNumber2);
				}
				if (item.IsMountable)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.DeeperSacks, clientParty, false, ref explainedNumber2);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.ArenicosHorses, clientParty, false, ref explainedNumber2);
				}
				if (clientParty.IsMainParty && Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.SmugglerConnections) && ((merchant != null) ? merchant.MapFaction : null) != null && merchant.MapFaction.MainHeroCrimeRating > 0f)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.SmugglerConnections, clientParty, false, ref explainedNumber2);
				}
				if (!isSelling && merchant != null && merchant.IsSettlement && merchant.Settlement.IsVillage)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.DistributedGoods, clientParty, false, ref explainedNumber2);
				}
				if (isSelling && item.HasHorseComponent)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.LocalConnection, clientParty, false, ref explainedNumber2);
				}
				if (isSelling && (item.ItemCategory == DefaultItemCategories.Pottery || item.ItemCategory == DefaultItemCategories.Tools || item.ItemCategory == DefaultItemCategories.Jewelry || item.ItemCategory == DefaultItemCategories.Cotton))
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.TradeyardForeman, clientParty, true, ref explainedNumber2);
				}
				if (!isSelling && (item.ItemCategory == DefaultItemCategories.Clay || item.ItemCategory == DefaultItemCategories.Iron || item.ItemCategory == DefaultItemCategories.Silver || item.ItemCategory == DefaultItemCategories.Cotton))
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.RapidDevelopment, clientParty, false, ref explainedNumber2);
				}
			}
			return explainedNumber2.ResultNumber;
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0008E2A4 File Offset: 0x0008C4A4
		private float GetPriceFactor(ItemObject item, MobileParty tradingParty, PartyBase merchant, float inStoreValue, float supply, float demand, bool isSelling)
		{
			float basePriceFactor = this.GetBasePriceFactor(item.GetItemCategory(), inStoreValue, supply, demand, isSelling, item.Value);
			float tradePenalty = this.GetTradePenalty(item, tradingParty, merchant, isSelling, inStoreValue, supply, demand);
			if (!isSelling)
			{
				return basePriceFactor * (1f + tradePenalty);
			}
			return basePriceFactor * 1f / (1f + tradePenalty);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0008E2FC File Offset: 0x0008C4FC
		public override float GetBasePriceFactor(ItemCategory itemCategory, float inStoreValue, float supply, float demand, bool isSelling, int transferValue)
		{
			if (isSelling)
			{
				inStoreValue += (float)transferValue;
			}
			float num = MathF.Pow(demand / (0.1f * supply + inStoreValue * 0.04f + 2f), itemCategory.IsAnimal ? 0.3f : 0.6f);
			if (itemCategory.IsTradeGood)
			{
				return MathF.Clamp(num, 0.1f, 10f);
			}
			return MathF.Clamp(num, 0.8f, 1.3f);
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0008E370 File Offset: 0x0008C570
		public override int GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand)
		{
			float priceFactor = this.GetPriceFactor(itemRosterElement.Item, clientParty, merchant, inStoreValue, supply, demand, isSelling);
			float num = (float)itemRosterElement.ItemValue * priceFactor;
			int num2 = (isSelling ? MathF.Floor(num) : MathF.Ceiling(num));
			Hero hero = null;
			if (!isSelling && ((merchant != null) ? merchant.MobileParty : null) != null && merchant.MobileParty.IsCaravan && clientParty.HasPerk(DefaultPerks.Trade.SilverTongue, out hero, true))
			{
				num2 = MathF.Ceiling((float)num2 * (1f - DefaultPerks.Trade.SilverTongue.SecondaryBonus));
			}
			return MathF.Max(num2, 1);
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0008E404 File Offset: 0x0008C604
		public override int GetTheoreticalMaxItemMarketValue(ItemObject item)
		{
			if (item.IsTradeGood || item.IsAnimal)
			{
				return MathF.Round((float)item.Value * 10f);
			}
			return MathF.Round((float)item.Value * 1.3f);
		}

		// Token: 0x04000931 RID: 2353
		private const float MinPriceFactor = 0.1f;

		// Token: 0x04000932 RID: 2354
		private const float MaxPriceFactor = 10f;

		// Token: 0x04000933 RID: 2355
		private const float MinPriceFactorNonTrade = 0.8f;

		// Token: 0x04000934 RID: 2356
		private const float MaxPriceFactorNonTrade = 1.3f;

		// Token: 0x04000935 RID: 2357
		private const float HighTradePenaltyBaseValue = 1.5f;

		// Token: 0x04000936 RID: 2358
		private const float PackAnimalTradePenalty = 0.8f;

		// Token: 0x04000937 RID: 2359
		private const float MountTradePenalty = 0.8f;
	}
}
