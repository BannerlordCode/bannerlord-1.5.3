using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000D2 RID: 210
	public class EncyclopediaSearchResultVM : ViewModel
	{
		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0004F5F2 File Offset: 0x0004D7F2
		// (set) Token: 0x060013A9 RID: 5033 RVA: 0x0004F5FA File Offset: 0x0004D7FA
		public string OrgNameText { get; private set; }

		// Token: 0x060013AA RID: 5034 RVA: 0x0004F604 File Offset: 0x0004D804
		public EncyclopediaSearchResultVM(EncyclopediaListItem source, string searchedText, int matchStartIndex)
		{
			this.MatchStartIndex = matchStartIndex;
			this.LinkId = source.Id;
			this.PageType = source.TypeName;
			this.OrgNameText = source.Name;
			this._nameText = source.Name;
			this.UpdateSearchedText(searchedText);
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0004F660 File Offset: 0x0004D860
		public void UpdateSearchedText(string searchedText)
		{
			this._searchedText = searchedText;
			if (string.IsNullOrEmpty(this.OrgNameText))
			{
				return;
			}
			int num = this.OrgNameText.IndexOf(this._searchedText, StringComparison.InvariantCultureIgnoreCase);
			if (num < 0)
			{
				return;
			}
			int num2 = MBMath.ClampInt(this._searchedText.Length, 0, this.OrgNameText.Length - num);
			if (num2 == 0)
			{
				return;
			}
			string text = this.OrgNameText.Substring(num, num2);
			if (!string.IsNullOrEmpty(text))
			{
				this.NameText = this.OrgNameText.Replace(text, "<a>" + text + "</a>");
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0004F6F5 File Offset: 0x0004D8F5
		public void Execute()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.PageType, this.LinkId);
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x0004F712 File Offset: 0x0004D912
		// (set) Token: 0x060013AE RID: 5038 RVA: 0x0004F71A File Offset: 0x0004D91A
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (this._nameText != value)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x040008EF RID: 2287
		private string _searchedText;

		// Token: 0x040008F1 RID: 2289
		public readonly int MatchStartIndex;

		// Token: 0x040008F2 RID: 2290
		public string LinkId = "";

		// Token: 0x040008F3 RID: 2291
		public string PageType;

		// Token: 0x040008F4 RID: 2292
		public string _nameText;
	}
}
