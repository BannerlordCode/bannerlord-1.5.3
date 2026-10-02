using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000015 RID: 21
	public class StringBasedVisibilityWidget : Widget
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006C21 File Offset: 0x00004E21
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00006C29 File Offset: 0x00004E29
		public StringBasedVisibilityWidget.WatchTypes WatchType { get; set; }

		// Token: 0x06000132 RID: 306 RVA: 0x00006C32 File Offset: 0x00004E32
		public StringBasedVisibilityWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00006C3B File Offset: 0x00004E3B
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00006C44 File Offset: 0x00004E44
		[Editor(false)]
		public string FirstString
		{
			get
			{
				return this._firstString;
			}
			set
			{
				if (this._firstString != value)
				{
					this._firstString = value;
					base.OnPropertyChanged<string>(value, "FirstString");
					StringBasedVisibilityWidget.WatchTypes watchType = this.WatchType;
					if (watchType == StringBasedVisibilityWidget.WatchTypes.Equal)
					{
						base.IsVisible = string.Equals(value, this.SecondString, StringComparison.OrdinalIgnoreCase);
						return;
					}
					if (watchType != StringBasedVisibilityWidget.WatchTypes.NotEqual)
					{
						return;
					}
					base.IsVisible = !string.Equals(value, this.SecondString, StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006CAB File Offset: 0x00004EAB
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00006CB4 File Offset: 0x00004EB4
		[Editor(false)]
		public string SecondString
		{
			get
			{
				return this._secondString;
			}
			set
			{
				if (this._secondString != value)
				{
					this._secondString = value;
					base.OnPropertyChanged<string>(value, "SecondString");
					StringBasedVisibilityWidget.WatchTypes watchType = this.WatchType;
					if (watchType == StringBasedVisibilityWidget.WatchTypes.Equal)
					{
						base.IsVisible = string.Equals(value, this.FirstString, StringComparison.OrdinalIgnoreCase);
						return;
					}
					if (watchType != StringBasedVisibilityWidget.WatchTypes.NotEqual)
					{
						return;
					}
					base.IsVisible = !string.Equals(value, this.FirstString, StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		// Token: 0x0400008F RID: 143
		private string _firstString;

		// Token: 0x04000090 RID: 144
		private string _secondString;

		// Token: 0x02000021 RID: 33
		public enum WatchTypes
		{
			// Token: 0x040000D3 RID: 211
			Equal,
			// Token: 0x040000D4 RID: 212
			NotEqual
		}
	}
}
