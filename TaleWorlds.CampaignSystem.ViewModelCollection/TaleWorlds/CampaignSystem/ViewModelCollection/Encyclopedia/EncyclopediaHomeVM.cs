using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000CF RID: 207
	public class EncyclopediaHomeVM : EncyclopediaPageVM
	{
		// Token: 0x06001364 RID: 4964 RVA: 0x0004E8DC File Offset: 0x0004CADC
		public EncyclopediaHomeVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this.Lists = new MBBindingList<ListTypeVM>();
			foreach (EncyclopediaPage encyclopediaPage in from p in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages()
				orderby p.HomePageOrderIndex
				select p)
			{
				if (encyclopediaPage.IsRelevant())
				{
					this.Lists.Add(new ListTypeVM(encyclopediaPage));
				}
			}
			this.RefreshValues();
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0004E980 File Offset: 0x0004CB80
		public override void Refresh()
		{
			base.Refresh();
			this.RefreshValues();
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0004E990 File Offset: 0x0004CB90
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._baseName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
			this.HomeTitleText = GameTexts.FindText("str_encyclopedia_name", null).ToString();
			this.Lists.ApplyActionOnAllItems(delegate(ListTypeVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0004E9F9 File Offset: 0x0004CBF9
		public override string GetNavigationBarURL()
		{
			return GameTexts.FindText("str_encyclopedia_home", null).ToString() + " \\";
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0004EA15 File Offset: 0x0004CC15
		public override string GetName()
		{
			return this._baseName;
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x0004EA1D File Offset: 0x0004CC1D
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0004EA25 File Offset: 0x0004CC25
		[DataSourceProperty]
		public bool IsListActive
		{
			get
			{
				return this._isListActive;
			}
			set
			{
				if (value != this._isListActive)
				{
					this._isListActive = value;
					base.OnPropertyChangedWithValue(value, "IsListActive");
				}
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0004EA43 File Offset: 0x0004CC43
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0004EA4B File Offset: 0x0004CC4B
		[DataSourceProperty]
		public string HomeTitleText
		{
			get
			{
				return this._homeTitleText;
			}
			set
			{
				if (value != this._homeTitleText)
				{
					this._homeTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "HomeTitleText");
				}
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0004EA6E File Offset: 0x0004CC6E
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x0004EA76 File Offset: 0x0004CC76
		[DataSourceProperty]
		public MBBindingList<ListTypeVM> Lists
		{
			get
			{
				return this._lists;
			}
			set
			{
				if (value != this._lists)
				{
					this._lists = value;
					base.OnPropertyChangedWithValue<MBBindingList<ListTypeVM>>(value, "Lists");
				}
			}
		}

		// Token: 0x040008D6 RID: 2262
		private string _baseName;

		// Token: 0x040008D7 RID: 2263
		private MBBindingList<ListTypeVM> _lists;

		// Token: 0x040008D8 RID: 2264
		private bool _isListActive;

		// Token: 0x040008D9 RID: 2265
		private string _homeTitleText;
	}
}
