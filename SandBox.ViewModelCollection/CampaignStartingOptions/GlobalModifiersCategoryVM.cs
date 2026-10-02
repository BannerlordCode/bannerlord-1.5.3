using System;
using System.Text;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.CampaignStartingOptions
{
	// Token: 0x02000060 RID: 96
	public class GlobalModifiersCategoryVM : StartingOptionCategoryVM
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x00015FFE File Offset: 0x000141FE
		public GlobalModifiersCategoryVM(string categoryId, TextObject name)
			: base(categoryId, name)
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00016008 File Offset: 0x00014208
		public override string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < base.Options.Count; i++)
			{
				StartingOptionVM startingOptionVM = base.Options[i];
				if (StartingOptionCategoryVM.IsOptionRelevant(startingOptionVM))
				{
					int optionType = startingOptionVM.OptionType;
					if (optionType != 0)
					{
						if (optionType == 1)
						{
							StartingOptionCategoryVM.AppendEntry(stringBuilder, startingOptionVM.Name + ": " + startingOptionVM.ValueAsString, "\n");
						}
					}
					else if (startingOptionVM.ValueAsBoolean)
					{
						StartingOptionCategoryVM.AppendEntry(stringBuilder, startingOptionVM.Name, "\n");
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
