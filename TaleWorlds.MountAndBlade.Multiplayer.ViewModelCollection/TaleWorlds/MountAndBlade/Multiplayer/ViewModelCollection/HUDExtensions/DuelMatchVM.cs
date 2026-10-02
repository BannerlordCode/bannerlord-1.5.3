using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000094 RID: 148
	public class DuelMatchVM : ViewModel
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0002C74C File Offset: 0x0002A94C
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x0002C754 File Offset: 0x0002A954
		public MissionPeer FirstPlayerPeer { get; private set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0002C75D File Offset: 0x0002A95D
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x0002C765 File Offset: 0x0002A965
		public MissionPeer SecondPlayerPeer { get; private set; }

		// Token: 0x06000E6A RID: 3690 RVA: 0x0002C76E File Offset: 0x0002A96E
		public DuelMatchVM()
		{
			this.IsEnabled = false;
			this._duelCountdownText = new TextObject("{=cO2FDHCa}Duel with {OPPONENT_NAME} is starting in {DUEL_REMAINING_TIME} seconds.", null);
			this.RefreshValues();
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0002C794 File Offset: 0x0002A994
		public void OnDuelPrepStarted(MissionPeer opponentPeer, int prepDuration)
		{
			this._prepTimeRemaining = (float)prepDuration;
			GameTexts.SetVariable("OPPONENT_NAME", opponentPeer.DisplayedName);
			this.IsPreparing = true;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0002C7B8 File Offset: 0x0002A9B8
		public void Tick(float dt)
		{
			if (this._prepTimeRemaining > 0f)
			{
				GameTexts.SetVariable("DUEL_REMAINING_TIME", (float)MathF.Ceiling(this._prepTimeRemaining));
				this.CountdownMessage = this._duelCountdownText.ToString();
				this._prepTimeRemaining -= dt;
				return;
			}
			this.IsPreparing = false;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0002C810 File Offset: 0x0002AA10
		public void OnDuelStarted(MissionPeer firstPeer, MissionPeer secondPeer, int arenaType)
		{
			this.FirstPlayerPeer = firstPeer;
			this.SecondPlayerPeer = secondPeer;
			this.FirstPlayerScore = 0;
			this.SecondPlayerScore = 0;
			this.FirstPlayer = new MPPlayerVM(firstPeer);
			this.SecondPlayer = new MPPlayerVM(secondPeer);
			this.FirstPlayer.RefreshDivision(true);
			this.SecondPlayer.RefreshDivision(true);
			this.ArenaType = arenaType;
			this.UpdateScore();
			this.IsEnabled = true;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0002C87D File Offset: 0x0002AA7D
		public void OnDuelEnded()
		{
			this.FirstPlayerPeer = null;
			this.SecondPlayerPeer = null;
			this.IsEnabled = false;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0002C894 File Offset: 0x0002AA94
		public void OnPeerScored(MissionPeer peer)
		{
			if (peer == this.FirstPlayerPeer)
			{
				int num = this.FirstPlayerScore;
				this.FirstPlayerScore = num + 1;
			}
			else if (peer == this.SecondPlayerPeer)
			{
				int num = this.SecondPlayerScore;
				this.SecondPlayerScore = num + 1;
			}
			this.UpdateScore();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0002C8DB File Offset: 0x0002AADB
		public void RefreshNames(bool changeGenericNames = false)
		{
			if (changeGenericNames)
			{
				this.FirstPlayer.Name = this.FirstPlayerPeer.DisplayedName;
				this.SecondPlayer.Name = this.SecondPlayerPeer.DisplayedName;
			}
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0002C90C File Offset: 0x0002AB0C
		private void UpdateScore()
		{
			GameTexts.SetVariable("LEFT", this.FirstPlayerScore);
			GameTexts.SetVariable("RIGHT", this.SecondPlayerScore);
			this.Score = GameTexts.FindText("str_LEFT_dash_RIGHT", null).ToString();
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002C944 File Offset: 0x0002AB44
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x0002C94C File Offset: 0x0002AB4C
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

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0002C96A File Offset: 0x0002AB6A
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0002C972 File Offset: 0x0002AB72
		[DataSourceProperty]
		public bool IsPreparing
		{
			get
			{
				return this._isPreparing;
			}
			set
			{
				if (value != this._isPreparing)
				{
					this._isPreparing = value;
					base.OnPropertyChangedWithValue(value, "IsPreparing");
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0002C990 File Offset: 0x0002AB90
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x0002C998 File Offset: 0x0002AB98
		[DataSourceProperty]
		public string CountdownMessage
		{
			get
			{
				return this._countdownMessage;
			}
			set
			{
				if (value != this._countdownMessage)
				{
					this._countdownMessage = value;
					base.OnPropertyChangedWithValue<string>(value, "CountdownMessage");
				}
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0002C9BB File Offset: 0x0002ABBB
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x0002C9C3 File Offset: 0x0002ABC3
		[DataSourceProperty]
		public string Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue<string>(value, "Score");
				}
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0002C9E6 File Offset: 0x0002ABE6
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x0002C9EE File Offset: 0x0002ABEE
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

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0002CA0C File Offset: 0x0002AC0C
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x0002CA14 File Offset: 0x0002AC14
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

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0002CA32 File Offset: 0x0002AC32
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x0002CA3A File Offset: 0x0002AC3A
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

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0002CA58 File Offset: 0x0002AC58
		// (set) Token: 0x06000E81 RID: 3713 RVA: 0x0002CA60 File Offset: 0x0002AC60
		[DataSourceProperty]
		public MPPlayerVM FirstPlayer
		{
			get
			{
				return this._firstPlayer;
			}
			set
			{
				if (value != this._firstPlayer)
				{
					this._firstPlayer = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "FirstPlayer");
				}
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0002CA7E File Offset: 0x0002AC7E
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x0002CA86 File Offset: 0x0002AC86
		[DataSourceProperty]
		public MPPlayerVM SecondPlayer
		{
			get
			{
				return this._secondPlayer;
			}
			set
			{
				if (value != this._secondPlayer)
				{
					this._secondPlayer = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "SecondPlayer");
				}
			}
		}

		// Token: 0x04000698 RID: 1688
		private float _prepTimeRemaining;

		// Token: 0x04000699 RID: 1689
		private TextObject _duelCountdownText;

		// Token: 0x0400069A RID: 1690
		private bool _isEnabled;

		// Token: 0x0400069B RID: 1691
		private bool _isPreparing;

		// Token: 0x0400069C RID: 1692
		private string _countdownMessage;

		// Token: 0x0400069D RID: 1693
		private string _score;

		// Token: 0x0400069E RID: 1694
		private int _arenaType;

		// Token: 0x0400069F RID: 1695
		private int _firstPlayerScore;

		// Token: 0x040006A0 RID: 1696
		private int _secondPlayerScore;

		// Token: 0x040006A1 RID: 1697
		private MPPlayerVM _firstPlayer;

		// Token: 0x040006A2 RID: 1698
		private MPPlayerVM _secondPlayer;
	}
}
