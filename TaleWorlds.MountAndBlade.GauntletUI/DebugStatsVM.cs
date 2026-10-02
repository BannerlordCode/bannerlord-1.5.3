using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000B RID: 11
	internal class DebugStatsVM : ViewModel
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00004324 File Offset: 0x00002524
		public DebugStatsVM()
		{
			this.GameVersion = ApplicationVersion.FromParametersFile(null).ToString();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00004351 File Offset: 0x00002551
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00004359 File Offset: 0x00002559
		[DataSourceProperty]
		public string GameVersion
		{
			get
			{
				return this._gameVersion;
			}
			set
			{
				if (value != this._gameVersion)
				{
					this._gameVersion = value;
					base.OnPropertyChangedWithValue<string>(value, "GameVersion");
				}
			}
		}

		// Token: 0x0400003E RID: 62
		private string _gameVersion;
	}
}
