using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000095 RID: 149
	public class ItemFlagVM : ViewModel
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x000337B7 File Offset: 0x000319B7
		public ItemFlagVM(string iconName, TextObject hint)
		{
			this.Icon = this.GetIconPath(iconName);
			this.Hint = new HintViewModel(hint, null);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000337DC File Offset: 0x000319DC
		private string GetIconPath(string iconName)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetIconPath");
			mbstringBuilder.Append<string>("<img src=\"SPGeneral\\");
			mbstringBuilder.Append<string>(iconName);
			mbstringBuilder.Append<string>("\"/>");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00033829 File Offset: 0x00031A29
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x00033831 File Offset: 0x00031A31
		[DataSourceProperty]
		public string Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				if (value != this._icon)
				{
					this._icon = value;
					base.OnPropertyChangedWithValue<string>(value, "Icon");
				}
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00033854 File Offset: 0x00031A54
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x0003385C File Offset: 0x00031A5C
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

		// Token: 0x04000599 RID: 1433
		private string _icon;

		// Token: 0x0400059A RID: 1434
		private HintViewModel _hint;
	}
}
