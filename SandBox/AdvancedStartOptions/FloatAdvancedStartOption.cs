using System;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.Library;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000115 RID: 277
	public class FloatAdvancedStartOption : AdvancedStartOption
	{
		// Token: 0x06000DA9 RID: 3497 RVA: 0x000627F1 File Offset: 0x000609F1
		public override bool HasValueChanged()
		{
			return !base.GetValue<float>().ApproximatelyEqualsTo(base.GetDefaultValue<float>(), 1E-05f);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0006280C File Offset: 0x00060A0C
		public FloatAdvancedStartOption(string stringId, string categoryId, float minValue, float maxValue, AdvancedStartOption.AdvancedStartOptionCondition onCondition, float defaultValue = 0f)
			: base(new AdvancedStartData<float>(stringId, categoryId, defaultValue), onCondition)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x040005CE RID: 1486
		public readonly float MinValue;

		// Token: 0x040005CF RID: 1487
		public readonly float MaxValue;
	}
}
