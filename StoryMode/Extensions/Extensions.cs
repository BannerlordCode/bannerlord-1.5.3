using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.Extensions
{
	// Token: 0x02000058 RID: 88
	public static class Extensions
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0002018C File Offset: 0x0001E38C
		public static bool IsTrainingField(this Settlement settlement)
		{
			return settlement.SettlementComponent is TrainingField;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0002019C File Offset: 0x0001E39C
		public static TrainingField TrainingField(this Settlement settlement)
		{
			return settlement.SettlementComponent as TrainingField;
		}
	}
}
