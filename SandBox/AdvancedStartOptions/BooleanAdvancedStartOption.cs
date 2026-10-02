using System;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;

namespace SandBox.AdvancedStartOptions
{
	// Token: 0x02000114 RID: 276
	public class BooleanAdvancedStartOption : AdvancedStartOption
	{
		// Token: 0x06000DA8 RID: 3496 RVA: 0x000627DF File Offset: 0x000609DF
		public BooleanAdvancedStartOption(string stringId, string categoryId, AdvancedStartOption.AdvancedStartOptionCondition onCondition, bool defaultValue = false)
			: base(new AdvancedStartData<bool>(stringId, categoryId, defaultValue), onCondition)
		{
		}
	}
}
