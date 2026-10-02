using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame
{
	// Token: 0x02000044 RID: 68
	public class MPMatchmakingRegionSelectorItemVM : SelectorItemVM
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00014B76 File Offset: 0x00012D76
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x00014B7E File Offset: 0x00012D7E
		public string RegionCode { get; private set; }

		// Token: 0x06000664 RID: 1636 RVA: 0x00014B87 File Offset: 0x00012D87
		public MPMatchmakingRegionSelectorItemVM(string regionCode, TextObject regionName)
			: base(regionName)
		{
			this.RegionCode = regionCode;
			this.IsRegionNone = regionCode == "None";
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00014BA8 File Offset: 0x00012DA8
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00014BB0 File Offset: 0x00012DB0
		[DataSourceProperty]
		public bool IsRegionNone
		{
			get
			{
				return this._isRegionNone;
			}
			set
			{
				if (value != this._isRegionNone)
				{
					this._isRegionNone = value;
					base.OnPropertyChangedWithValue(value, "IsRegionNone");
				}
			}
		}

		// Token: 0x04000302 RID: 770
		private bool _isRegionNone;
	}
}
