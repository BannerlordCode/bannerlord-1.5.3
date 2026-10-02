using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000446 RID: 1094
	public class PartiesSellLootCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600463D RID: 17981 RVA: 0x00155EA3 File Offset: 0x001540A3
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x00155EBC File Offset: 0x001540BC
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x00155EC0 File Offset: 0x001540C0
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (Campaign.Current.GameStarted && mobileParty != null && !FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, settlement.MapFaction) && !mobileParty.IsMainParty && mobileParty.IsLordParty && !mobileParty.IsDisbanding && settlement.IsTown)
			{
				int gold = settlement.SettlementComponent.Gold;
				for (int i = 0; i < mobileParty.ItemRoster.Count; i++)
				{
					ItemRosterElement itemRosterElement = mobileParty.ItemRoster[i];
					ItemObject item = itemRosterElement.EquipmentElement.Item;
					ItemModifier itemModifier = itemRosterElement.EquipmentElement.ItemModifier;
					int amount = itemRosterElement.Amount;
					if (!item.IsFood && (item.ItemType != ItemObject.ItemTypeEnum.Horse || !item.HorseComponent.IsRideable || itemModifier != null || item.HorseComponent.IsPackAnimal))
					{
						int itemPrice = settlement.Town.GetItemPrice(itemRosterElement.EquipmentElement, mobileParty, true);
						int num = ((itemPrice * amount < gold) ? amount : (gold / itemPrice));
						if (num > 0)
						{
							SellItemsAction.Apply(mobileParty.Party, settlement.Party, itemRosterElement, num, settlement);
						}
					}
				}
			}
		}
	}
}
