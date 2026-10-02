using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000106 RID: 262
	public class CraftingItemFlagVM : ItemFlagVM
	{
		// Token: 0x0600178E RID: 6030 RVA: 0x0005B144 File Offset: 0x00059344
		public CraftingItemFlagVM(string iconPath, TextObject hint, bool isDisplayed)
			: base(iconPath, hint)
		{
			this.IsDisplayed = isDisplayed;
			this.IconPath = "SPGeneral\\" + iconPath;
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x0005B166 File Offset: 0x00059366
		// (set) Token: 0x06001790 RID: 6032 RVA: 0x0005B16E File Offset: 0x0005936E
		[DataSourceProperty]
		public bool IsDisplayed
		{
			get
			{
				return this._isDisplayed;
			}
			set
			{
				if (value != this._isDisplayed)
				{
					this._isDisplayed = value;
					base.OnPropertyChangedWithValue(value, "IsDisplayed");
				}
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0005B18C File Offset: 0x0005938C
		// (set) Token: 0x06001792 RID: 6034 RVA: 0x0005B194 File Offset: 0x00059394
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChangedWithValue<string>(value, "IconPath");
				}
			}
		}

		// Token: 0x04000AB7 RID: 2743
		private bool _isDisplayed;

		// Token: 0x04000AB8 RID: 2744
		private string _iconPath;
	}
}
