using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D7 RID: 215
	public class EncyclopediaContentPageVM : EncyclopediaPageVM
	{
		// Token: 0x06001403 RID: 5123 RVA: 0x0005089E File Offset: 0x0004EA9E
		public EncyclopediaContentPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000508C9 File Offset: 0x0004EAC9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PreviousButtonLabel = this._previousButtonLabelText.ToString();
			this.NextButtonLabel = this._nextButtonLabelText.ToString();
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000508F4 File Offset: 0x0004EAF4
		public void InitializeQuickNavigation(EncyclopediaListVM list)
		{
			if (list != null && list.Items != null)
			{
				List<EncyclopediaListItemVM> list2 = list.Items.Where<EncyclopediaListItemVM>((EncyclopediaListItemVM x) => !x.IsFiltered).ToList<EncyclopediaListItemVM>();
				int count = list2.Count;
				int num = list2.FindIndex((EncyclopediaListItemVM x) => x.Object == base.Obj);
				if (count > 1 && num > -1)
				{
					if (num > 0)
					{
						this._previousItem = list2[num - 1];
						this.PreviousButtonHint = new HintViewModel(new TextObject(this._previousItem.Name, null), null);
						this.IsPreviousButtonEnabled = true;
					}
					if (num < count - 1)
					{
						this._nextItem = list2[num + 1];
						this.NextButtonHint = new HintViewModel(new TextObject(this._nextItem.Name, null), null);
						this.IsNextButtonEnabled = true;
					}
				}
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000509D4 File Offset: 0x0004EBD4
		public void ExecuteGoToNextItem()
		{
			if (this._nextItem != null)
			{
				this._nextItem.Execute();
				return;
			}
			Debug.FailedAssert("If the next button is enabled then next item should not be null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Pages\\EncyclopediaContentPageVM.cs", "ExecuteGoToNextItem", 66);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00050A00 File Offset: 0x0004EC00
		public void ExecuteGoToPreviousItem()
		{
			if (this._previousItem != null)
			{
				this._previousItem.Execute();
				return;
			}
			Debug.FailedAssert("If the previous button is enabled then previous item should not be null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Pages\\EncyclopediaContentPageVM.cs", "ExecuteGoToPreviousItem", 78);
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x00050A2C File Offset: 0x0004EC2C
		// (set) Token: 0x06001409 RID: 5129 RVA: 0x00050A34 File Offset: 0x0004EC34
		[DataSourceProperty]
		public bool IsPreviousButtonEnabled
		{
			get
			{
				return this._isPreviousButtonEnabled;
			}
			set
			{
				if (value != this._isPreviousButtonEnabled)
				{
					this._isPreviousButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPreviousButtonEnabled");
				}
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00050A52 File Offset: 0x0004EC52
		// (set) Token: 0x0600140B RID: 5131 RVA: 0x00050A5A File Offset: 0x0004EC5A
		[DataSourceProperty]
		public bool IsNextButtonEnabled
		{
			get
			{
				return this._isNextButtonEnabled;
			}
			set
			{
				if (value != this._isNextButtonEnabled)
				{
					this._isNextButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNextButtonEnabled");
				}
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00050A78 File Offset: 0x0004EC78
		// (set) Token: 0x0600140D RID: 5133 RVA: 0x00050A80 File Offset: 0x0004EC80
		[DataSourceProperty]
		public string PreviousButtonLabel
		{
			get
			{
				return this._previousButtonLabel;
			}
			set
			{
				if (value != this._previousButtonLabel)
				{
					this._previousButtonLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousButtonLabel");
				}
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x00050AA3 File Offset: 0x0004ECA3
		// (set) Token: 0x0600140F RID: 5135 RVA: 0x00050AAB File Offset: 0x0004ECAB
		[DataSourceProperty]
		public string NextButtonLabel
		{
			get
			{
				return this._nextButtonLabel;
			}
			set
			{
				if (value != this._nextButtonLabel)
				{
					this._nextButtonLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "NextButtonLabel");
				}
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x00050ACE File Offset: 0x0004ECCE
		// (set) Token: 0x06001411 RID: 5137 RVA: 0x00050AD6 File Offset: 0x0004ECD6
		[DataSourceProperty]
		public HintViewModel PreviousButtonHint
		{
			get
			{
				return this._previousButtonHint;
			}
			set
			{
				if (value != this._previousButtonHint)
				{
					this._previousButtonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PreviousButtonHint");
				}
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00050AF4 File Offset: 0x0004ECF4
		// (set) Token: 0x06001413 RID: 5139 RVA: 0x00050AFC File Offset: 0x0004ECFC
		[DataSourceProperty]
		public HintViewModel NextButtonHint
		{
			get
			{
				return this._nextButtonHint;
			}
			set
			{
				if (value != this._nextButtonHint)
				{
					this._nextButtonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NextButtonHint");
				}
			}
		}

		// Token: 0x0400091B RID: 2331
		private EncyclopediaListItemVM _previousItem;

		// Token: 0x0400091C RID: 2332
		private EncyclopediaListItemVM _nextItem;

		// Token: 0x0400091D RID: 2333
		private TextObject _previousButtonLabelText = new TextObject("{=zlcMGAbn}Previous Page", null);

		// Token: 0x0400091E RID: 2334
		private TextObject _nextButtonLabelText = new TextObject("{=QFfMd5q3}Next Page", null);

		// Token: 0x0400091F RID: 2335
		private bool _isPreviousButtonEnabled;

		// Token: 0x04000920 RID: 2336
		private bool _isNextButtonEnabled;

		// Token: 0x04000921 RID: 2337
		private string _previousButtonLabel;

		// Token: 0x04000922 RID: 2338
		private string _nextButtonLabel;

		// Token: 0x04000923 RID: 2339
		private HintViewModel _previousButtonHint;

		// Token: 0x04000924 RID: 2340
		private HintViewModel _nextButtonHint;
	}
}
