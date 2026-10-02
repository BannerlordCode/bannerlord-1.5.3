using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x0200000E RID: 14
	public class LauncherHintVM : ViewModel
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00003617 File Offset: 0x00001817
		public LauncherHintVM(string text)
		{
			this.Text = text;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003626 File Offset: 0x00001826
		public void ExecuteBeginHint()
		{
			if (!string.IsNullOrEmpty(this.Text))
			{
				LauncherUI.AddHintInformation(this.Text);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003640 File Offset: 0x00001840
		public void ExecuteEndHint()
		{
			LauncherUI.HideHintInformation();
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003647 File Offset: 0x00001847
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000364F File Offset: 0x0000184F
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

		// Token: 0x04000040 RID: 64
		private string _text;
	}
}
