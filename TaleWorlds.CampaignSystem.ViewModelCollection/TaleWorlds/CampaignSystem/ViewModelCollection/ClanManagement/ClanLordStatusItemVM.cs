using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012D RID: 301
	public class ClanLordStatusItemVM : ViewModel
	{
		// Token: 0x06001B29 RID: 6953 RVA: 0x00065D0C File Offset: 0x00063F0C
		public ClanLordStatusItemVM(ClanLordStatusItemVM.LordStatus status, TextObject hintText)
		{
			this.Type = (int)status;
			this.Hint = new HintViewModel(hintText, null);
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x00065D2F File Offset: 0x00063F2F
		// (set) Token: 0x06001B2B RID: 6955 RVA: 0x00065D37 File Offset: 0x00063F37
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x00065D55 File Offset: 0x00063F55
		// (set) Token: 0x06001B2D RID: 6957 RVA: 0x00065D5D File Offset: 0x00063F5D
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x04000C9F RID: 3231
		private int _type = -1;

		// Token: 0x04000CA0 RID: 3232
		private HintViewModel _hint;

		// Token: 0x0200028C RID: 652
		public enum LordStatus
		{
			// Token: 0x04001329 RID: 4905
			Dead,
			// Token: 0x0400132A RID: 4906
			Married,
			// Token: 0x0400132B RID: 4907
			Pregnant,
			// Token: 0x0400132C RID: 4908
			InBattle,
			// Token: 0x0400132D RID: 4909
			InSiege,
			// Token: 0x0400132E RID: 4910
			Child,
			// Token: 0x0400132F RID: 4911
			Prisoner,
			// Token: 0x04001330 RID: 4912
			Sick
		}
	}
}
