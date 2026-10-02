using System;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000119 RID: 281
	public class UIntAdvancedStartOption : AdvancedStartOption
	{
		// Token: 0x06000DB5 RID: 3509 RVA: 0x00062A4B File Offset: 0x00060C4B
		public UIntAdvancedStartOption(string stringId, string categoryId, uint minValue, uint maxValue, AdvancedStartOption.AdvancedStartOptionCondition onCondition, uint defaultValue = 0U)
			: base(new AdvancedStartData<uint>(stringId, categoryId, defaultValue), onCondition)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x040005D3 RID: 1491
		public readonly uint MinValue;

		// Token: 0x040005D4 RID: 1492
		public readonly uint MaxValue;
	}
}
