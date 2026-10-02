using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000151 RID: 337
	public class DefaultSettlementEconomyModel : SettlementEconomyModel
	{
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x00086016 File Offset: 0x00084216
		private DefaultSettlementEconomyModel.CategoryValues CategoryValuesCache
		{
			get
			{
				if (this._categoryValues == null)
				{
					this._categoryValues = new DefaultSettlementEconomyModel.CategoryValues();
				}
				return this._categoryValues;
			}
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00086034 File Offset: 0x00084234
		public override ValueTuple<float, float> GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand)
		{
			float num = oldSupply * 0.85f + dailySupply * 0.15f;
			float num2 = oldDemand * 0.85f + dailyDemand * 0.15f;
			num = MathF.Max(0.1f, num);
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00086078 File Offset: 0x00084278
		public override float GetDailyDemandForCategory(Town town, ItemCategory category, int extraProsperity)
		{
			float num = MathF.Max(0f, town.Prosperity + (float)extraProsperity);
			float num2 = MathF.Max(0f, town.Prosperity - 3000f);
			float num3 = category.BaseDemand * num;
			float num4 = category.LuxuryDemand * num2;
			float num5 = num3 + num4;
			if (category.BaseDemand < 1E-08f)
			{
				num5 = num * 0.01f;
			}
			return num5;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x000860DC File Offset: 0x000842DC
		public override int GetTownGoldChange(Town town)
		{
			float num = 10000f + town.Prosperity * 12f - (float)town.Gold;
			return MathF.Round(0.25f * num);
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00086110 File Offset: 0x00084310
		public override float CalculateDailySettlementBudgetForItemCategory(Town town, float demand, ItemCategory category)
		{
			return demand * MathF.Pow(town.GetItemCategoryPriceIndex(category), 0.3f);
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00086125 File Offset: 0x00084325
		public override float GetDemandChangeFromValue(float purchaseValue)
		{
			return purchaseValue * 0.15f;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0008612E File Offset: 0x0008432E
		public override float GetEstimatedDemandForCategory(Town town, ItemData itemData, ItemCategory category)
		{
			return Campaign.Current.Models.SettlementEconomyModel.GetDailyDemandForCategory(town, category, 1000);
		}

		// Token: 0x040008B5 RID: 2229
		private DefaultSettlementEconomyModel.CategoryValues _categoryValues;

		// Token: 0x040008B6 RID: 2230
		private const int ProsperityLuxuryTreshold = 3000;

		// Token: 0x040008B7 RID: 2231
		private const float dailyChangeFactor = 0.15f;

		// Token: 0x040008B8 RID: 2232
		private const float oneMinusDailyChangeFactor = 0.85f;

		// Token: 0x020005CD RID: 1485
		private class CategoryValues
		{
			// Token: 0x06005189 RID: 20873 RVA: 0x001909C4 File Offset: 0x0018EBC4
			public CategoryValues()
			{
				this.PriceDict = new Dictionary<ItemCategory, int>();
				foreach (ItemObject itemObject in Items.All)
				{
					this.PriceDict[itemObject.GetItemCategory()] = itemObject.Value;
				}
			}

			// Token: 0x0600518A RID: 20874 RVA: 0x00190A38 File Offset: 0x0018EC38
			public int GetValueOfCategory(ItemCategory category)
			{
				int num = 1;
				this.PriceDict.TryGetValue(category, out num);
				return num;
			}

			// Token: 0x04001918 RID: 6424
			public Dictionary<ItemCategory, int> PriceDict;
		}
	}
}
