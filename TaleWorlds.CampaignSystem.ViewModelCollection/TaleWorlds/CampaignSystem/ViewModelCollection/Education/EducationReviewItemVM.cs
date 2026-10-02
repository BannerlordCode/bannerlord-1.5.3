using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000FB RID: 251
	public class EducationReviewItemVM : ViewModel
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x00057C4F File Offset: 0x00055E4F
		public void UpdateWith(string gainText)
		{
			this.GainText = gainText;
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x00057C58 File Offset: 0x00055E58
		// (set) Token: 0x06001661 RID: 5729 RVA: 0x00057C60 File Offset: 0x00055E60
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x00057C83 File Offset: 0x00055E83
		// (set) Token: 0x06001663 RID: 5731 RVA: 0x00057C8B File Offset: 0x00055E8B
		[DataSourceProperty]
		public string GainText
		{
			get
			{
				return this._gainText;
			}
			set
			{
				if (value != this._gainText)
				{
					this._gainText = value;
					base.OnPropertyChangedWithValue<string>(value, "GainText");
				}
			}
		}

		// Token: 0x04000A20 RID: 2592
		private string _title;

		// Token: 0x04000A21 RID: 2593
		private string _gainText;
	}
}
