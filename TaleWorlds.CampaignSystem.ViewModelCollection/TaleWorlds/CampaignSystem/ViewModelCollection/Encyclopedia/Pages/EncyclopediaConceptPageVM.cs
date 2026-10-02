using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D6 RID: 214
	[EncyclopediaViewModel(typeof(Concept))]
	public class EncyclopediaConceptPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x060013F8 RID: 5112 RVA: 0x000506CC File Offset: 0x0004E8CC
		public EncyclopediaConceptPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._concept = base.Obj as Concept;
			Concept.SetConceptTextLinks();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._concept);
			this.RefreshValues();
			this.Refresh();
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00050722 File Offset: 0x0004E922
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = this._concept.Title.ToString();
			this.DescriptionText = this._concept.Description.ToString();
			base.UpdateBookmarkHintText();
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0005075C File Offset: 0x0004E95C
		public override void Refresh()
		{
			base.IsLoadingOver = false;
			base.IsLoadingOver = true;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0005076C File Offset: 0x0004E96C
		public override string GetName()
		{
			return this._concept.Title.ToString();
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0005077E File Offset: 0x0004E97E
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00050790 File Offset: 0x0004E990
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Concept", GameTexts.FindText("str_encyclopedia_concepts", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x000507F8 File Offset: 0x0004E9F8
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._concept);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._concept);
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00050848 File Offset: 0x0004EA48
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x00050850 File Offset: 0x0004EA50
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x00050873 File Offset: 0x0004EA73
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x0005087B File Offset: 0x0004EA7B
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x04000918 RID: 2328
		private Concept _concept;

		// Token: 0x04000919 RID: 2329
		private string _titleText;

		// Token: 0x0400091A RID: 2330
		private string _descriptionText;
	}
}
