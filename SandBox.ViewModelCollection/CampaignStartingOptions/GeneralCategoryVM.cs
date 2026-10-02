using System;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.CampaignStartingOptions
{
	// Token: 0x0200005F RID: 95
	public class GeneralCategoryVM : StartingOptionCategoryVM
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x00015FED File Offset: 0x000141ED
		public GeneralCategoryVM(string categoryId, TextObject name)
			: base(categoryId, name)
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00015FF7 File Offset: 0x000141F7
		public override string GetDescription()
		{
			return string.Empty;
		}
	}
}
