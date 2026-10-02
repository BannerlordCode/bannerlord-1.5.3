using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000406 RID: 1030
	public class DiscardItemsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060040B3 RID: 16563 RVA: 0x0011C899 File Offset: 0x0011AA99
		public override void RegisterEvents()
		{
			CampaignEvents.OnItemsDiscardedByPlayerEvent.AddNonSerializedListener(this, new Action<ItemRoster>(this.OnItemsDiscardedByPlayer));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnHourlyTickParty));
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0011C8C9 File Offset: 0x0011AAC9
		private void OnHourlyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.IsLordParty && !mobileParty.IsMainParty && mobileParty.LeaderHero != null)
			{
				this.HandlePartyInventory(mobileParty.Party);
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0011C8F0 File Offset: 0x0011AAF0
		private void OnItemsDiscardedByPlayer(ItemRoster roster)
		{
			int xpBonusForDiscardingItems = Campaign.Current.Models.ItemDiscardModel.GetXpBonusForDiscardingItems(roster);
			if ((float)xpBonusForDiscardingItems > 0f)
			{
				MobilePartyHelper.PartyAddSharedXp(MobileParty.MainParty, (float)xpBonusForDiscardingItems);
			}
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0011C928 File Offset: 0x0011AB28
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0011C92C File Offset: 0x0011AB2C
		private void HandlePartyInventory(PartyBase party)
		{
			if (party.IsMobile && party.MobileParty.IsLordParty && !party.MobileParty.IsMainParty && !party.MobileParty.IsCurrentlyAtSea)
			{
				int num = party.ItemRoster.NumberOfLivestockAnimals + party.ItemRoster.NumberOfPackAnimals + MathF.Max(0, party.ItemRoster.NumberOfMounts - party.NumberOfMenWithHorse);
				if (num > party.MemberRoster.TotalManCount)
				{
					this.DiscardAnimalsCausingHerdingPenalty(party.MobileParty, num - MathF.Max(0, party.ItemRoster.NumberOfMounts - party.NumberOfMenWithHorse));
				}
				if (party.MobileParty.TotalWeightCarried > (float)party.MobileParty.InventoryCapacity)
				{
					this.DiscardOverburdeningItemsForParty(party.MobileParty, party.MobileParty.TotalWeightCarried - (float)party.MobileParty.InventoryCapacity);
				}
			}
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0011CA18 File Offset: 0x0011AC18
		private void DiscardAnimalsCausingHerdingPenalty(MobileParty mobileParty, int amount)
		{
			int num = amount;
			int num2 = mobileParty.ItemRoster.Count - 1;
			while (num2 >= 0 && num > 0)
			{
				if (mobileParty.ItemRoster[num2].EquipmentElement.Item.IsAnimal)
				{
					this.DiscardAnimal(mobileParty, mobileParty.ItemRoster[num2], ref num);
				}
				num2--;
			}
			int num3 = mobileParty.ItemRoster.Count - 1;
			while (num3 >= 0 && num > 0)
			{
				if (mobileParty.ItemRoster[num3].EquipmentElement.Item.IsMountable && mobileParty.ItemRoster[num3].EquipmentElement.Item.HorseComponent.IsPackAnimal)
				{
					this.DiscardAnimal(mobileParty, mobileParty.ItemRoster[num3], ref num);
				}
				num3--;
			}
			int num4 = mobileParty.ItemRoster.Count - 1;
			while (num4 >= 0 && num > 0)
			{
				if (mobileParty.ItemRoster[num4].EquipmentElement.Item.IsMountable)
				{
					this.DiscardAnimal(mobileParty, mobileParty.ItemRoster[num4], ref num);
				}
				num4--;
			}
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x0011CB58 File Offset: 0x0011AD58
		private void DiscardOverburdeningItemsForParty(MobileParty mobileParty, float totalWeightToDiscard)
		{
			int num = (int)(mobileParty.FoodChange * -20f);
			float num2 = totalWeightToDiscard;
			for (int i = mobileParty.ItemRoster.Count - 1; i >= 0; i--)
			{
				if (num2 <= 0f)
				{
					return;
				}
				if (num > 0 && mobileParty.ItemRoster[i].EquipmentElement.Item.IsFood)
				{
					if (mobileParty.ItemRoster[i].Amount > num)
					{
						int num3 = mobileParty.ItemRoster[i].Amount - num;
						num = 0;
						this.DiscardNecessaryAmountOfItems(mobileParty, mobileParty.ItemRoster[i], ref num2, num3);
					}
					else
					{
						num -= mobileParty.ItemRoster[i].Amount;
					}
				}
				else
				{
					this.DiscardNecessaryAmountOfItems(mobileParty, mobileParty.ItemRoster[i], ref num2, int.MaxValue);
				}
			}
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0011CC4C File Offset: 0x0011AE4C
		private void DiscardNecessaryAmountOfItems(MobileParty mobileParty, ItemRosterElement itemRosterElement, ref float weightLeftToDiscard, int discardLimit = 2147483647)
		{
			float equipmentElementWeight = itemRosterElement.EquipmentElement.GetEquipmentElementWeight();
			int num = MBMath.ClampInt((itemRosterElement.GetRosterElementWeight() <= weightLeftToDiscard) ? itemRosterElement.Amount : MathF.Ceiling(weightLeftToDiscard / equipmentElementWeight), 0, discardLimit);
			weightLeftToDiscard -= equipmentElementWeight * (float)num;
			mobileParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num);
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0011CCB0 File Offset: 0x0011AEB0
		private void DiscardAnimal(MobileParty mobileParty, ItemRosterElement itemRosterElement, ref int numberOfAnimalsToDiscard)
		{
			int num = ((itemRosterElement.Amount > numberOfAnimalsToDiscard) ? numberOfAnimalsToDiscard : itemRosterElement.Amount);
			numberOfAnimalsToDiscard -= num;
			mobileParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num);
		}
	}
}
