using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000029 RID: 41
	public class StringItemWithHintVM : ViewModel
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x00005D5F File Offset: 0x00003F5F
		public StringItemWithHintVM(string text, TextObject hint)
		{
			this.Text = text;
			this.Hint = new HintViewModel(hint, null);
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00005D7B File Offset: 0x00003F7B
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00005D83 File Offset: 0x00003F83
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00005DA6 File Offset: 0x00003FA6
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00005DAE File Offset: 0x00003FAE
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
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040000BA RID: 186
		private string _text;

		// Token: 0x040000BB RID: 187
		private HintViewModel _hint;
	}
}
