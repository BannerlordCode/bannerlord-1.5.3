using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000432 RID: 1074
	public interface IWorkshopWarehouseCampaignBehavior
	{
		// Token: 0x06004404 RID: 17412
		bool IsGettingInputsFromWarehouse(Workshop workshop);

		// Token: 0x06004405 RID: 17413
		void SetIsGettingInputsFromWarehouse(Workshop workshop, bool isActive);

		// Token: 0x06004406 RID: 17414
		float GetStockProductionInWarehouseRatio(Workshop workshop);

		// Token: 0x06004407 RID: 17415
		void SetStockProductionInWarehouseRatio(Workshop workshop, float percentage);

		// Token: 0x06004408 RID: 17416
		float GetWarehouseItemRosterWeight(Settlement settlement);

		// Token: 0x06004409 RID: 17417
		bool IsRawMaterialsSufficientInTownMarket(Workshop workshop);

		// Token: 0x0600440A RID: 17418
		int GetInputCount(Workshop workshop);

		// Token: 0x0600440B RID: 17419
		int GetOutputCount(Workshop workshop);

		// Token: 0x0600440C RID: 17420
		ExplainedNumber GetInputDailyChange(Workshop workshop);

		// Token: 0x0600440D RID: 17421
		ExplainedNumber GetOutputDailyChange(Workshop workshop);
	}
}
