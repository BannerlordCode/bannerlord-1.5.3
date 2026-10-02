using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A6 RID: 166
	public class AlternativeUsageItemOptionVM : SelectorItemVM
	{
		// Token: 0x0600100C RID: 4108 RVA: 0x000321BA File Offset: 0x000303BA
		public AlternativeUsageItemOptionVM(string usageType, TextObject s, TextObject hint, SelectorVM<AlternativeUsageItemOptionVM> parentSelector, int index)
			: base(s, hint)
		{
			this.UsageType = usageType;
			this._index = index;
			this._parentSelector = parentSelector;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x000321DB File Offset: 0x000303DB
		private void ExecuteSelection()
		{
			this._parentSelector.SelectedIndex = this._index;
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x000321EE File Offset: 0x000303EE
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x000321F6 File Offset: 0x000303F6
		[DataSourceProperty]
		public string UsageType
		{
			get
			{
				return this._usageType;
			}
			set
			{
				if (value != this._usageType)
				{
					this._usageType = value;
					base.OnPropertyChangedWithValue<string>(value, "UsageType");
				}
			}
		}

		// Token: 0x04000784 RID: 1924
		private int _index;

		// Token: 0x04000785 RID: 1925
		private SelectorVM<AlternativeUsageItemOptionVM> _parentSelector;

		// Token: 0x04000786 RID: 1926
		private string _usageType;
	}
}
