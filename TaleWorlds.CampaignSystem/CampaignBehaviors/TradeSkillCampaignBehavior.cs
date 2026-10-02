using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200046C RID: 1132
	public class TradeSkillCampaignBehavior : CampaignBehaviorBase, IPlayerTradeBehavior
	{
		// Token: 0x06004939 RID: 18745 RVA: 0x0016ED0A File Offset: 0x0016CF0A
		public override void RegisterEvents()
		{
			CampaignEvents.PlayerInventoryExchangeEvent.AddNonSerializedListener(this, new Action<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool>(this.PlayerInventoryUpdated));
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x0016ED24 File Offset: 0x0016CF24
		private void RecordPurchases(ItemRosterElement itemRosterElement, int totalPrice)
		{
			TradeSkillCampaignBehavior.ItemTradeData itemTradeData;
			if (!this.ItemsTradeData.TryGetValue(itemRosterElement.EquipmentElement.Item, out itemTradeData))
			{
				itemTradeData = default(TradeSkillCampaignBehavior.ItemTradeData);
			}
			int num = itemTradeData.NumItemsPurchased + itemRosterElement.Amount;
			float num2 = (itemTradeData.AveragePrice * (float)itemTradeData.NumItemsPurchased + (float)totalPrice) / MathF.Max(0.0001f, (float)num);
			this.ItemsTradeData[itemRosterElement.EquipmentElement.Item] = new TradeSkillCampaignBehavior.ItemTradeData(num2, num);
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x0016EDA8 File Offset: 0x0016CFA8
		private int RecordSales(ItemRosterElement itemRosterElement, int totalPrice, bool isTrading)
		{
			int num = 0;
			TradeSkillCampaignBehavior.ItemTradeData itemTradeData;
			if (this.ItemsTradeData.TryGetValue(itemRosterElement.EquipmentElement.Item, out itemTradeData))
			{
				if (isTrading)
				{
					int num2 = MathF.Min(itemTradeData.NumItemsPurchased, itemRosterElement.Amount);
					int num3 = itemTradeData.NumItemsPurchased - num2;
					float num4 = (float)num2 * itemTradeData.AveragePrice;
					float num5 = (float)totalPrice / MathF.Max(0.001f, (float)itemRosterElement.Amount);
					int num6 = MathF.Round((float)num2 * num5);
					num = MathF.Max(0, num6 - MathF.Floor(num4));
					if (num3 == 0)
					{
						this.ItemsTradeData.Remove(itemRosterElement.EquipmentElement.Item);
					}
					else
					{
						this.ItemsTradeData[itemRosterElement.EquipmentElement.Item] = new TradeSkillCampaignBehavior.ItemTradeData(itemTradeData.AveragePrice, num3);
					}
				}
				else
				{
					int num7 = MobileParty.MainParty.ItemRoster.FindIndexOfElement(itemRosterElement.EquipmentElement);
					if (num7 == -1)
					{
						this.ItemsTradeData.Remove(itemRosterElement.EquipmentElement.Item);
					}
					else
					{
						int amount = MobileParty.MainParty.ItemRoster.GetElementCopyAtIndex(num7).Amount;
						if (itemTradeData.NumItemsPurchased > amount)
						{
							this.ItemsTradeData[itemRosterElement.EquipmentElement.Item] = new TradeSkillCampaignBehavior.ItemTradeData(itemTradeData.AveragePrice, amount);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x0016EF14 File Offset: 0x0016D114
		private int GetAveragePriceForItem(ItemRosterElement itemRosterElement)
		{
			TradeSkillCampaignBehavior.ItemTradeData itemTradeData;
			if (!this.ItemsTradeData.TryGetValue(itemRosterElement.EquipmentElement.Item, out itemTradeData))
			{
				return 0;
			}
			return MathF.Round(itemTradeData.AveragePrice);
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x0016EF4C File Offset: 0x0016D14C
		private void PlayerInventoryUpdated(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			int num = 0;
			if (isTrading)
			{
				foreach (ValueTuple<ItemRosterElement, int> valueTuple in purchasedItems)
				{
					this.ProcessPurchases(valueTuple.Item1, valueTuple.Item2);
				}
			}
			foreach (ValueTuple<ItemRosterElement, int> valueTuple2 in soldItems)
			{
				num += this.ProcessSales(valueTuple2.Item1, valueTuple2.Item2, isTrading);
			}
			if (isTrading)
			{
				SkillLevelingManager.OnTradeProfitMade(PartyBase.MainParty, num);
				CampaignEventDispatcher.Instance.OnPlayerTradeProfit(num);
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x0016F010 File Offset: 0x0016D210
		private int ProcessSales(ItemRosterElement itemRosterElement, int totalPrice, bool isTrading)
		{
			if (itemRosterElement.EquipmentElement.ItemModifier == null)
			{
				return this.RecordSales(itemRosterElement, totalPrice, isTrading);
			}
			return 0;
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x0016F03C File Offset: 0x0016D23C
		private void ProcessPurchases(ItemRosterElement itemRosterElement, int totalPrice)
		{
			if (itemRosterElement.EquipmentElement.ItemModifier == null)
			{
				this.RecordPurchases(itemRosterElement, totalPrice);
			}
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x0016F062 File Offset: 0x0016D262
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData>>("ItemsTradeData", ref this.ItemsTradeData);
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x0016F078 File Offset: 0x0016D278
		public int GetProjectedProfit(ItemRosterElement itemRosterElement, int itemCost)
		{
			if (itemRosterElement.EquipmentElement.ItemModifier != null)
			{
				return 0;
			}
			int averagePriceForItem = this.GetAveragePriceForItem(itemRosterElement);
			return itemCost - averagePriceForItem;
		}

		// Token: 0x0400149C RID: 5276
		private Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData> ItemsTradeData = new Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData>();

		// Token: 0x020008AF RID: 2223
		public class TradeSkillCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006C0D RID: 27661 RVA: 0x001DBA47 File Offset: 0x001D9C47
			public TradeSkillCampaignBehaviorTypeDefiner()
				: base(150794)
			{
			}

			// Token: 0x06006C0E RID: 27662 RVA: 0x001DBA54 File Offset: 0x001D9C54
			protected override void DefineStructTypes()
			{
				base.AddStructDefinition(typeof(TradeSkillCampaignBehavior.ItemTradeData), 10, null);
			}

			// Token: 0x06006C0F RID: 27663 RVA: 0x001DBA69 File Offset: 0x001D9C69
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, TradeSkillCampaignBehavior.ItemTradeData>));
			}
		}

		// Token: 0x020008B0 RID: 2224
		internal struct ItemTradeData
		{
			// Token: 0x06006C10 RID: 27664 RVA: 0x001DBA7B File Offset: 0x001D9C7B
			public ItemTradeData(float averagePrice, int numItemsPurchased)
			{
				this.AveragePrice = averagePrice;
				this.NumItemsPurchased = numItemsPurchased;
			}

			// Token: 0x06006C11 RID: 27665 RVA: 0x001DBA8C File Offset: 0x001D9C8C
			public static void AutoGeneratedStaticCollectObjectsItemTradeData(object o, List<object> collectedObjects)
			{
				((TradeSkillCampaignBehavior.ItemTradeData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006C12 RID: 27666 RVA: 0x001DBAA8 File Offset: 0x001D9CA8
			private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x06006C13 RID: 27667 RVA: 0x001DBAAA File Offset: 0x001D9CAA
			internal static object AutoGeneratedGetMemberValueAveragePrice(object o)
			{
				return ((TradeSkillCampaignBehavior.ItemTradeData)o).AveragePrice;
			}

			// Token: 0x06006C14 RID: 27668 RVA: 0x001DBABC File Offset: 0x001D9CBC
			internal static object AutoGeneratedGetMemberValueNumItemsPurchased(object o)
			{
				return ((TradeSkillCampaignBehavior.ItemTradeData)o).NumItemsPurchased;
			}

			// Token: 0x040025E5 RID: 9701
			[SaveableField(10)]
			public readonly float AveragePrice;

			// Token: 0x040025E6 RID: 9702
			[SaveableField(20)]
			public readonly int NumItemsPurchased;
		}
	}
}
