using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x02000021 RID: 33
	public class MissionScoreboardStatItemVM : ViewModel
	{
		// Token: 0x0600021B RID: 539 RVA: 0x000084FA File Offset: 0x000066FA
		public MissionScoreboardStatItemVM(MissionScoreboardPlayerVM belongedPlayer, string headerID, string item)
		{
			this.Item = item;
			this.HeaderID = headerID;
			this.BelongedPlayer = belongedPlayer;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00008522 File Offset: 0x00006722
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000852A File Offset: 0x0000672A
		[DataSourceProperty]
		public string Item
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value != this._item)
				{
					this._item = value;
					base.OnPropertyChangedWithValue<string>(value, "Item");
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000854D File Offset: 0x0000674D
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00008555 File Offset: 0x00006755
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00008578 File Offset: 0x00006778
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00008580 File Offset: 0x00006780
		[DataSourceProperty]
		public MissionScoreboardPlayerVM BelongedPlayer
		{
			get
			{
				return this._belongedPlayer;
			}
			set
			{
				if (value != this._belongedPlayer)
				{
					this._belongedPlayer = value;
					base.OnPropertyChangedWithValue<MissionScoreboardPlayerVM>(value, "BelongedPlayer");
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000859E File Offset: 0x0000679E
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardMVPItemVM> MVPBadges
		{
			get
			{
				return this.BelongedPlayer.MVPBadges;
			}
		}

		// Token: 0x04000120 RID: 288
		private string _item;

		// Token: 0x04000121 RID: 289
		private string _headerID = "";

		// Token: 0x04000122 RID: 290
		private MissionScoreboardPlayerVM _belongedPlayer;
	}
}
