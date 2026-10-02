using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x02000006 RID: 6
	public class GameVersionVM : ViewModel
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002493 File Offset: 0x00000693
		public GameVersionVM(Func<List<string>> getVersionTexts)
		{
			this.GameVersionTexts = new MBBindingList<BindingListStringItem>();
			this._getVersionTexts = getVersionTexts;
			this.RefreshValues();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000024B4 File Offset: 0x000006B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.GameVersionTexts.Clear();
			Func<List<string>> getVersionTexts = this._getVersionTexts;
			foreach (string text in ((getVersionTexts != null) ? getVersionTexts() : null))
			{
				this.GameVersionTexts.Add(new BindingListStringItem(text));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002530 File Offset: 0x00000730
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002538 File Offset: 0x00000738
		[DataSourceProperty]
		public MBBindingList<BindingListStringItem> GameVersionTexts
		{
			get
			{
				return this._gameVersionTexts;
			}
			set
			{
				if (value != this._gameVersionTexts)
				{
					this._gameVersionTexts = value;
					base.OnPropertyChangedWithValue<MBBindingList<BindingListStringItem>>(value, "GameVersionTexts");
				}
			}
		}

		// Token: 0x0400000F RID: 15
		private readonly Func<List<string>> _getVersionTexts;

		// Token: 0x04000010 RID: 16
		private MBBindingList<BindingListStringItem> _gameVersionTexts;
	}
}
