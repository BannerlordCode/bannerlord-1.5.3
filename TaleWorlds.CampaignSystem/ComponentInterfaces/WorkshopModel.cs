using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F8 RID: 504
	public abstract class WorkshopModel : MBGameModel<WorkshopModel>
	{
		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06001FD4 RID: 8148
		public abstract int DaysForPlayerSaveWorkshopFromBankruptcy { get; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06001FD5 RID: 8149
		public abstract int CapitalLowLimit { get; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06001FD6 RID: 8150
		public abstract int InitialCapital { get; }

		// Token: 0x06001FD7 RID: 8151
		public abstract int GetMaxWorkshopCountForClanTier(int tier);

		// Token: 0x06001FD8 RID: 8152
		public abstract int GetCostForPlayer(Workshop workshop);

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001FD9 RID: 8153
		public abstract int DailyExpense { get; }

		// Token: 0x06001FDA RID: 8154
		public abstract int GetCostForNotable(Workshop workshop);

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001FDB RID: 8155
		public abstract int WarehouseCapacity { get; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001FDC RID: 8156
		public abstract int DefaultWorkshopCountInSettlement { get; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06001FDD RID: 8157
		public abstract int MaximumWorkshopsPlayerCanHave { get; }

		// Token: 0x06001FDE RID: 8158
		public abstract Hero GetNotableOwnerForWorkshop(Workshop workshop);

		// Token: 0x06001FDF RID: 8159
		public abstract ExplainedNumber GetEffectiveConversionSpeedOfProduction(Workshop workshop, float speed, bool includeDescriptions);

		// Token: 0x06001FE0 RID: 8160
		public abstract int GetConvertProductionCost(WorkshopType workshopType);

		// Token: 0x06001FE1 RID: 8161
		public abstract bool CanPlayerSellWorkshop(Workshop workshop, out TextObject explanation);

		// Token: 0x06001FE2 RID: 8162
		public abstract float GetTradeXpPerWarehouseProduction(EquipmentElement production);
	}
}
