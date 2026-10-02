using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000122 RID: 290
	public class ClanCardSelectionPopupItemPropertyVM : ViewModel
	{
		// Token: 0x06001A66 RID: 6758 RVA: 0x00063F5B File Offset: 0x0006215B
		public ClanCardSelectionPopupItemPropertyVM(in ClanCardSelectionItemPropertyInfo info)
		{
			this._titleText = info.Title;
			this._valueText = info.Value;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00063F7C File Offset: 0x0006217C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleText = this._titleText;
			this.Title = ((titleText != null) ? titleText.ToString() : null) ?? string.Empty;
			TextObject valueText = this._valueText;
			this.Value = ((valueText != null) ? valueText.ToString() : null) ?? string.Empty;
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x00063FD1 File Offset: 0x000621D1
		// (set) Token: 0x06001A69 RID: 6761 RVA: 0x00063FD9 File Offset: 0x000621D9
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

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x00063FFC File Offset: 0x000621FC
		// (set) Token: 0x06001A6B RID: 6763 RVA: 0x00064004 File Offset: 0x00062204
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x04000C17 RID: 3095
		private readonly TextObject _titleText;

		// Token: 0x04000C18 RID: 3096
		private readonly TextObject _valueText;

		// Token: 0x04000C19 RID: 3097
		private string _title;

		// Token: 0x04000C1A RID: 3098
		private string _value;
	}
}
