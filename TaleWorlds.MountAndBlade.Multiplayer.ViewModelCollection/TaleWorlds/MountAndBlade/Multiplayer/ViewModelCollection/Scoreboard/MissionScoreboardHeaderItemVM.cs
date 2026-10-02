using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x0200001C RID: 28
	public class MissionScoreboardHeaderItemVM : BindingListStringItem
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00007369 File Offset: 0x00005569
		public MissionScoreboardHeaderItemVM(MissionScoreboardSideVM side, string headerID, string value, bool isAvatarStat, bool isIrregularStat)
			: base(value)
		{
			this._side = side;
			this.HeaderID = headerID;
			this.IsAvatarStat = isAvatarStat;
			this.IsIrregularStat = isIrregularStat;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000739B File Offset: 0x0000559B
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x000073A3 File Offset: 0x000055A3
		[DataSourceProperty]
		public string HeaderID
		{
			get
			{
				return this._headerID;
			}
			set
			{
				if (value != this._headerID)
				{
					this._headerID = value;
					base.OnPropertyChangedWithValue<string>(value, "HeaderID");
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000073C6 File Offset: 0x000055C6
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000073CE File Offset: 0x000055CE
		[DataSourceProperty]
		public bool IsIrregularStat
		{
			get
			{
				return this._isIrregularStat;
			}
			set
			{
				if (value != this._isIrregularStat)
				{
					this._isIrregularStat = value;
					base.OnPropertyChangedWithValue(value, "IsIrregularStat");
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000073EC File Offset: 0x000055EC
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x000073F4 File Offset: 0x000055F4
		[DataSourceProperty]
		public bool IsAvatarStat
		{
			get
			{
				return this._isAvatarStat;
			}
			set
			{
				if (value != this._isAvatarStat)
				{
					this._isAvatarStat = value;
					base.OnPropertyChangedWithValue(value, "IsAvatarStat");
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00007412 File Offset: 0x00005612
		[DataSourceProperty]
		public MissionScoreboardPlayerSortControllerVM PlayerSortController
		{
			get
			{
				return this._side.PlayerSortController;
			}
		}

		// Token: 0x040000E6 RID: 230
		private readonly MissionScoreboardSideVM _side;

		// Token: 0x040000E7 RID: 231
		private string _headerID = "";

		// Token: 0x040000E8 RID: 232
		private bool _isIrregularStat;

		// Token: 0x040000E9 RID: 233
		private bool _isAvatarStat;
	}
}
