using System;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000116 RID: 278
	public class IntAdvancedStartOption : AdvancedStartOption
	{
		// Token: 0x06000DAB RID: 3499 RVA: 0x0006282E File Offset: 0x00060A2E
		public IntAdvancedStartOption(string stringId, string categoryId, int minValue, int maxValue, AdvancedStartOption.AdvancedStartOptionCondition onCondition, int defaultValue = 0)
			: base(new AdvancedStartData<int>(stringId, categoryId, defaultValue), onCondition)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x040005D0 RID: 1488
		public readonly int MinValue;

		// Token: 0x040005D1 RID: 1489
		public readonly int MaxValue;
	}
}
