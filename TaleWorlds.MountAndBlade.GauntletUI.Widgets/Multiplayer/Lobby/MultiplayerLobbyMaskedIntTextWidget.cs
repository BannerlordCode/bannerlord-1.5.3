using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A8 RID: 168
	public class MultiplayerLobbyMaskedIntTextWidget : TextWidget
	{
		// Token: 0x060008F4 RID: 2292 RVA: 0x00019CB7 File Offset: 0x00017EB7
		public MultiplayerLobbyMaskedIntTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00019CC0 File Offset: 0x00017EC0
		private void IntValueUpdated()
		{
			if (this.IntValue == this.MaskedIntValue)
			{
				base.Text = this.MaskText;
				return;
			}
			base.IntText = this.IntValue;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00019CE9 File Offset: 0x00017EE9
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x00019CF1 File Offset: 0x00017EF1
		[Editor(false)]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (this._intValue != value)
				{
					this._intValue = value;
					base.OnPropertyChanged(value, "IntValue");
					this.IntValueUpdated();
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00019D15 File Offset: 0x00017F15
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x00019D1D File Offset: 0x00017F1D
		[Editor(false)]
		public int MaskedIntValue
		{
			get
			{
				return this._maskedIntValue;
			}
			set
			{
				if (this._maskedIntValue != value)
				{
					this._maskedIntValue = value;
					base.OnPropertyChanged(value, "MaskedIntValue");
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00019D3B File Offset: 0x00017F3B
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00019D43 File Offset: 0x00017F43
		[Editor(false)]
		public string MaskText
		{
			get
			{
				return this._maskText;
			}
			set
			{
				if (this._maskText != value)
				{
					this._maskText = value;
					base.OnPropertyChanged<string>(value, "MaskText");
				}
			}
		}

		// Token: 0x0400040D RID: 1037
		private int _intValue;

		// Token: 0x0400040E RID: 1038
		private int _maskedIntValue;

		// Token: 0x0400040F RID: 1039
		private string _maskText;
	}
}
