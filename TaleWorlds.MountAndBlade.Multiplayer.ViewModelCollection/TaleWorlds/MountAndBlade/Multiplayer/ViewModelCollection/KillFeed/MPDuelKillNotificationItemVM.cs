using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed
{
	// Token: 0x02000089 RID: 137
	public class MPDuelKillNotificationItemVM : ViewModel
	{
		// Token: 0x06000D7B RID: 3451 RVA: 0x00029874 File Offset: 0x00027A74
		public MPDuelKillNotificationItemVM(MissionPeer firstPlayerPeer, MissionPeer secondPlayerPeer, int firstPlayerScore, int secondPlayerScore, TroopType arenaTroopType, Action<MPDuelKillNotificationItemVM> onRemove)
		{
			this._onRemove = onRemove;
			this.ArenaType = (int)arenaTroopType;
			this.FirstPlayerScore = firstPlayerScore;
			this.SecondPlayerScore = secondPlayerScore;
			int intValue = MultiplayerOptions.OptionType.MinScoreToWinDuel.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.IsEndOfDuel = this.FirstPlayerScore == intValue || this.SecondPlayerScore == intValue;
			this.InitProperties(firstPlayerPeer, secondPlayerPeer);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000298D4 File Offset: 0x00027AD4
		public void InitProperties(MissionPeer firstPlayerPeer, MissionPeer secondPlayerPeer)
		{
			TargetIconType peerIconType = this.GetPeerIconType(firstPlayerPeer);
			this.FirstPlayerName = firstPlayerPeer.DisplayedName;
			this.FirstPlayerCompassElement = new MPTeammateCompassTargetVM(peerIconType, Color.White.ToUnsignedInteger(), Color.White.ToUnsignedInteger(), Banner.CreateOneColoredEmptyBanner(0), false);
			TargetIconType peerIconType2 = this.GetPeerIconType(secondPlayerPeer);
			this.SecondPlayerName = secondPlayerPeer.DisplayedName;
			this.SecondPlayerCompassElement = new MPTeammateCompassTargetVM(peerIconType2, Color.White.ToUnsignedInteger(), Color.White.ToUnsignedInteger(), Banner.CreateOneColoredEmptyBanner(0), false);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00029964 File Offset: 0x00027B64
		private TargetIconType GetPeerIconType(MissionPeer peer)
		{
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(peer, false);
			if (mpheroClassForPeer != null)
			{
				return mpheroClassForPeer.IconType;
			}
			return TargetIconType.None;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00029984 File Offset: 0x00027B84
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00029992 File Offset: 0x00027B92
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0002999A File Offset: 0x00027B9A
		[DataSourceProperty]
		public bool IsEndOfDuel
		{
			get
			{
				return this._isEndOfDuel;
			}
			set
			{
				if (value != this._isEndOfDuel)
				{
					this._isEndOfDuel = value;
					base.OnPropertyChangedWithValue(value, "IsEndOfDuel");
				}
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x000299B8 File Offset: 0x00027BB8
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x000299C0 File Offset: 0x00027BC0
		[DataSourceProperty]
		public int ArenaType
		{
			get
			{
				return this._arenaType;
			}
			set
			{
				if (value != this._arenaType)
				{
					this._arenaType = value;
					base.OnPropertyChangedWithValue(value, "ArenaType");
				}
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x000299DE File Offset: 0x00027BDE
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x000299E6 File Offset: 0x00027BE6
		[DataSourceProperty]
		public int FirstPlayerScore
		{
			get
			{
				return this._firstPlayerScore;
			}
			set
			{
				if (value != this._firstPlayerScore)
				{
					this._firstPlayerScore = value;
					base.OnPropertyChangedWithValue(value, "FirstPlayerScore");
				}
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00029A04 File Offset: 0x00027C04
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x00029A0C File Offset: 0x00027C0C
		[DataSourceProperty]
		public int SecondPlayerScore
		{
			get
			{
				return this._secondPlayerScore;
			}
			set
			{
				if (value != this._secondPlayerScore)
				{
					this._secondPlayerScore = value;
					base.OnPropertyChangedWithValue(value, "SecondPlayerScore");
				}
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00029A2A File Offset: 0x00027C2A
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x00029A32 File Offset: 0x00027C32
		[DataSourceProperty]
		public string FirstPlayerName
		{
			get
			{
				return this._firstPlayerName;
			}
			set
			{
				if (value != this._firstPlayerName)
				{
					this._firstPlayerName = value;
					base.OnPropertyChangedWithValue<string>(value, "FirstPlayerName");
				}
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00029A55 File Offset: 0x00027C55
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x00029A5D File Offset: 0x00027C5D
		[DataSourceProperty]
		public string SecondPlayerName
		{
			get
			{
				return this._secondPlayerName;
			}
			set
			{
				if (value != this._secondPlayerName)
				{
					this._secondPlayerName = value;
					base.OnPropertyChangedWithValue<string>(value, "SecondPlayerName");
				}
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00029A80 File Offset: 0x00027C80
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00029A88 File Offset: 0x00027C88
		[DataSourceProperty]
		public MPTeammateCompassTargetVM FirstPlayerCompassElement
		{
			get
			{
				return this._firstPlayerCompassElement;
			}
			set
			{
				if (value != this._firstPlayerCompassElement)
				{
					this._firstPlayerCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "FirstPlayerCompassElement");
				}
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00029AA6 File Offset: 0x00027CA6
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x00029AAE File Offset: 0x00027CAE
		[DataSourceProperty]
		public MPTeammateCompassTargetVM SecondPlayerCompassElement
		{
			get
			{
				return this._secondPlayerCompassElement;
			}
			set
			{
				if (value != this._secondPlayerCompassElement)
				{
					this._secondPlayerCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "SecondPlayerCompassElement");
				}
			}
		}

		// Token: 0x04000627 RID: 1575
		private Action<MPDuelKillNotificationItemVM> _onRemove;

		// Token: 0x04000628 RID: 1576
		private bool _isEndOfDuel;

		// Token: 0x04000629 RID: 1577
		private int _arenaType;

		// Token: 0x0400062A RID: 1578
		private int _firstPlayerScore;

		// Token: 0x0400062B RID: 1579
		private int _secondPlayerScore;

		// Token: 0x0400062C RID: 1580
		private string _firstPlayerName;

		// Token: 0x0400062D RID: 1581
		private string _secondPlayerName;

		// Token: 0x0400062E RID: 1582
		private MPTeammateCompassTargetVM _firstPlayerCompassElement;

		// Token: 0x0400062F RID: 1583
		private MPTeammateCompassTargetVM _secondPlayerCompassElement;
	}
}
