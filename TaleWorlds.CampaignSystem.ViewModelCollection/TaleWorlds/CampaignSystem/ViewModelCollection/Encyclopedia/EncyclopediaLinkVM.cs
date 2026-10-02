using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000D0 RID: 208
	public class EncyclopediaLinkVM : ViewModel
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0004EA94 File Offset: 0x0004CC94
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0004EA9C File Offset: 0x0004CC9C
		[DataSourceProperty]
		public string ActiveLink
		{
			get
			{
				return this._activeLink;
			}
			set
			{
				if (this._activeLink != value)
				{
					this._activeLink = value;
					base.OnPropertyChangedWithValue<string>(value, "ActiveLink");
				}
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0004EABF File Offset: 0x0004CCBF
		public void ExecuteActiveLink()
		{
			if (!string.IsNullOrEmpty(this.ActiveLink))
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.ActiveLink);
			}
		}

		// Token: 0x040008DA RID: 2266
		private string _activeLink;
	}
}
