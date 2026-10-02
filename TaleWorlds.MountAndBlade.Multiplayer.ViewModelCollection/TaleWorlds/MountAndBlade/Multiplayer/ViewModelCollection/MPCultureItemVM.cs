using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000008 RID: 8
	public class MPCultureItemVM : ViewModel
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000034A0 File Offset: 0x000016A0
		// (set) Token: 0x0600005B RID: 91 RVA: 0x000034A8 File Offset: 0x000016A8
		public BasicCultureObject Culture { get; private set; }

		// Token: 0x0600005C RID: 92 RVA: 0x000034B1 File Offset: 0x000016B1
		public MPCultureItemVM(string cultureCode, Action<MPCultureItemVM> onSelection)
		{
			this._onSelection = onSelection;
			this.CultureCode = cultureCode;
			this.Culture = MBObjectManager.Instance.GetObject<BasicCultureObject>(cultureCode);
			this.RefreshValues();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000034DE File Offset: 0x000016DE
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Hint = new HintViewModel(this.Culture.Name, null);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000034FD File Offset: 0x000016FD
		private void ExecuteSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000350B File Offset: 0x0000170B
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003513 File Offset: 0x00001713
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003530 File Offset: 0x00001730
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00003538 File Offset: 0x00001738
		[DataSourceProperty]
		public string CultureCode
		{
			get
			{
				return this._cultureCode;
			}
			set
			{
				if (value != this._cultureCode)
				{
					this._cultureCode = value;
					base.OnPropertyChanged("CultureCode");
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000355A File Offset: 0x0000175A
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00003562 File Offset: 0x00001762
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
					base.OnPropertyChanged("Hint");
				}
			}
		}

		// Token: 0x04000037 RID: 55
		private Action<MPCultureItemVM> _onSelection;

		// Token: 0x04000039 RID: 57
		private bool _isSelected;

		// Token: 0x0400003A RID: 58
		private string _cultureCode;

		// Token: 0x0400003B RID: 59
		private HintViewModel _hint;
	}
}
