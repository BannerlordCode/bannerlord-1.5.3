using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000FA RID: 250
	public class EducationReviewVM : ViewModel
	{
		// Token: 0x06001654 RID: 5716 RVA: 0x00057AC8 File Offset: 0x00055CC8
		public EducationReviewVM(int pageCount)
		{
			this._pageCount = pageCount;
			this.ReviewList = new MBBindingList<EducationReviewItemVM>();
			for (int i = 0; i < this._pageCount - 1; i++)
			{
				this.ReviewList.Add(new EducationReviewItemVM());
			}
			this.RefreshValues();
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00057B38 File Offset: 0x00055D38
		public override void RefreshValues()
		{
			for (int i = 0; i < this.ReviewList.Count; i++)
			{
				this._educationPageTitle.SetTextVariable("NUMBER", i + 1);
				this.ReviewList[i].Title = this._educationPageTitle.ToString();
			}
			this.StageCompleteText = this._stageCompleteTextObject.ToString();
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00057B9C File Offset: 0x00055D9C
		public void SetGainForStage(int pageIndex, string gainText)
		{
			if (pageIndex >= 0 && pageIndex < this._pageCount)
			{
				this.ReviewList[pageIndex].UpdateWith(gainText);
			}
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00057BBD File Offset: 0x00055DBD
		public void SetCurrentPage(int currentPageIndex)
		{
			this.IsEnabled = currentPageIndex == this._pageCount - 1;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x00057BD0 File Offset: 0x00055DD0
		// (set) Token: 0x06001659 RID: 5721 RVA: 0x00057BD8 File Offset: 0x00055DD8
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x00057BF6 File Offset: 0x00055DF6
		// (set) Token: 0x0600165B RID: 5723 RVA: 0x00057BFE File Offset: 0x00055DFE
		[DataSourceProperty]
		public string StageCompleteText
		{
			get
			{
				return this._stageCompleteText;
			}
			set
			{
				if (value != this._stageCompleteText)
				{
					this._stageCompleteText = value;
					base.OnPropertyChangedWithValue<string>(value, "StageCompleteText");
				}
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x00057C21 File Offset: 0x00055E21
		// (set) Token: 0x0600165D RID: 5725 RVA: 0x00057C29 File Offset: 0x00055E29
		[DataSourceProperty]
		public MBBindingList<EducationReviewItemVM> ReviewList
		{
			get
			{
				return this._reviewList;
			}
			set
			{
				if (value != this._reviewList)
				{
					this._reviewList = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationReviewItemVM>>(value, "ReviewList");
				}
			}
		}

		// Token: 0x04000A1A RID: 2586
		private readonly int _pageCount;

		// Token: 0x04000A1B RID: 2587
		private readonly TextObject _educationPageTitle = new TextObject("{=m1Yynagz}Page {NUMBER}", null);

		// Token: 0x04000A1C RID: 2588
		private readonly TextObject _stageCompleteTextObject = new TextObject("{=flxDkoMh}Stage Complete", null);

		// Token: 0x04000A1D RID: 2589
		private MBBindingList<EducationReviewItemVM> _reviewList;

		// Token: 0x04000A1E RID: 2590
		private bool _isEnabled;

		// Token: 0x04000A1F RID: 2591
		private string _stageCompleteText;
	}
}
