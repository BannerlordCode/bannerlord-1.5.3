using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x0200000D RID: 13
	public class LauncherInformationVM : ViewModel
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003583 File Offset: 0x00001783
		public LauncherInformationVM()
		{
			LauncherUI.OnAddHintInformation += this.ExecuteEnableHint;
			LauncherUI.OnHideHintInformation += this.ExecuteDisableHint;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000035AD File Offset: 0x000017AD
		private void ExecuteEnableHint(string text)
		{
			this.IsEnabled = true;
			this.Text = text;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000035BD File Offset: 0x000017BD
		private void ExecuteDisableHint()
		{
			this.IsEnabled = false;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000035C6 File Offset: 0x000017C6
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000035CE File Offset: 0x000017CE
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000035EC File Offset: 0x000017EC
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000035F4 File Offset: 0x000017F4
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

		// Token: 0x0400003E RID: 62
		private bool _isEnabled;

		// Token: 0x0400003F RID: 63
		private string _text;
	}
}
