using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000128 RID: 296
	public class DefaultInventoryCapacityModel : InventoryCapacityModel
	{
		// Token: 0x060018D2 RID: 6354 RVA: 0x00078B65 File Offset: 0x00076D65
		public override int GetItemAverageWeight()
		{
			return 10;
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00078B69 File Offset: 0x00076D69
		public override float GetItemEffectiveWeight(EquipmentElement equipmentElement, MobileParty mobileParty, bool isCurrentlyAtSea, out TextObject description)
		{
			if (equipmentElement.Item.HasHorseComponent)
			{
				description = DefaultInventoryCapacityModel._textMountsAndPackAnimals;
				return 0f;
			}
			description = DefaultInventoryCapacityModel._textItems;
			return equipmentElement.GetEquipmentElementWeight();
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00078B98 File Offset: 0x00076D98
		public override ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false, int additionalTroops = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			PartyBase party = mobileParty.Party;
			int num = party.NumberOfMounts;
			int num2 = party.NumberOfHealthyMembers;
			int num3 = party.NumberOfPackAnimals;
			if (includeFollowers)
			{
				foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
				{
					num += mobileParty2.Party.NumberOfMounts;
					num2 += mobileParty2.Party.NumberOfHealthyMembers;
					num3 += mobileParty2.Party.NumberOfPackAnimals;
				}
			}
			Hero hero = null;
			if (mobileParty.HasPerk(DefaultPerks.Steward.ArenicosHorses, out hero, false))
			{
				int num4 = MathF.Round((float)num2 * DefaultPerks.Steward.ArenicosHorses.PrimaryBonus);
				num2 += num4;
			}
			Hero hero2 = null;
			if (mobileParty.HasPerk(DefaultPerks.Steward.ForcedLabor, out hero2, false))
			{
				int totalHealthyCount = party.PrisonRoster.TotalHealthyCount;
				num2 += totalHealthyCount;
			}
			explainedNumber.Add(10f, DefaultInventoryCapacityModel._textBase, null);
			explainedNumber.Add((float)num2 * 2f * 10f, DefaultInventoryCapacityModel._textTroops, null);
			if (!isCurrentlyAtSea)
			{
				explainedNumber.Add((float)num * 2f * 10f, DefaultInventoryCapacityModel._textSpareMounts, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber((float)num3 * 10f * 10f, false, null);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.BeastWhisperer, mobileParty, false, ref explainedNumber2);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.DeeperSacks, mobileParty, true, ref explainedNumber2);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.ArenicosMules, mobileParty, true, ref explainedNumber2);
				explainedNumber.Add(explainedNumber2.ResultNumber, DefaultInventoryCapacityModel._textPackAnimals, null);
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.CaravanMaster, mobileParty, true, ref explainedNumber);
			}
			explainedNumber.LimitMin(10f);
			return explainedNumber;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00078D58 File Offset: 0x00076F58
		public override ExplainedNumber CalculateTotalWeightCarried(MobileParty mobileParty, bool isCurrentlyAtSea, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, DefaultInventoryCapacityModel._textItems);
			InventoryCapacityModel inventoryCapacityModel = Campaign.Current.Models.InventoryCapacityModel;
			foreach (ItemRosterElement itemRosterElement in mobileParty.ItemRoster)
			{
				TextObject textObject;
				float itemEffectiveWeight = inventoryCapacityModel.GetItemEffectiveWeight(itemRosterElement.EquipmentElement, mobileParty, isCurrentlyAtSea, out textObject);
				explainedNumber.Add(itemEffectiveWeight * (float)itemRosterElement.Amount, textObject, null);
			}
			return explainedNumber;
		}

		// Token: 0x0400081C RID: 2076
		private const int _itemAverageWeight = 10;

		// Token: 0x0400081D RID: 2077
		private const float TroopsFactor = 2f;

		// Token: 0x0400081E RID: 2078
		private const float SpareMountsFactor = 2f;

		// Token: 0x0400081F RID: 2079
		private const float PackAnimalsFactor = 10f;

		// Token: 0x04000820 RID: 2080
		private static readonly TextObject _textTroops = new TextObject("{=5k4dxUEJ}Troops", null);

		// Token: 0x04000821 RID: 2081
		private static readonly TextObject _textBase = new TextObject("{=basevalue}Base", null);

		// Token: 0x04000822 RID: 2082
		private static readonly TextObject _textSpareMounts = new TextObject("{=rCiKbsyW}Spare Mounts", null);

		// Token: 0x04000823 RID: 2083
		private static readonly TextObject _textPackAnimals = new TextObject("{=dI1AOyqh}Pack Animals", null);

		// Token: 0x04000824 RID: 2084
		private static readonly TextObject _textMountsAndPackAnimals = new TextObject("{=Sb1MKbvP}Mounts and Pack Animals", null);

		// Token: 0x04000825 RID: 2085
		private static readonly TextObject _textLiveStocksAnimals = new TextObject("{=KxUgSAKi}Live Stock Animals", null);

		// Token: 0x04000826 RID: 2086
		private static readonly TextObject _textItems = new TextObject("{=U7er3V9s}Items", null);
	}
}
