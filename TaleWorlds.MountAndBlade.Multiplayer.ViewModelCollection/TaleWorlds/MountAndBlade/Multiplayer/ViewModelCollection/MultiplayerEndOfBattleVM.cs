using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.MissionRepresentatives;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000011 RID: 17
	public class MultiplayerEndOfBattleVM : ViewModel
	{
		// Token: 0x060000EA RID: 234 RVA: 0x000053BD File Offset: 0x000035BD
		public MultiplayerEndOfBattleVM()
		{
			this._activeDelay = MissionLobbyComponent.PostMatchWaitDuration / 2f;
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.RefreshValues();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000053EC File Offset: 0x000035EC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=GPfkMajw}Battle Ended", null).ToString();
			this.DescriptionText = new TextObject("{=ADPaaX8R}Best Players of This Battle", null).ToString();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005420 File Offset: 0x00003620
		public void OnTick(float dt)
		{
			if (this._isBattleEnded)
			{
				this._activateTimeElapsed += dt;
				if (this._activateTimeElapsed >= this._activeDelay)
				{
					this._isBattleEnded = false;
					this.OnEnabled();
				}
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005454 File Offset: 0x00003654
		private void OnEnabled()
		{
			MissionScoreboardComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			List<MissionPeer> list = new List<MissionPeer>();
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in missionBehavior.Sides.Where<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side != BattleSideEnum.None))
			{
				foreach (MissionPeer missionPeer in missionScoreboardSide.Players)
				{
					list.Add(missionPeer);
				}
			}
			list.Sort((MissionPeer p1, MissionPeer p2) => this.GetPeerScore(p2).CompareTo(this.GetPeerScore(p1)));
			if (list.Count > 0)
			{
				this.HasFirstPlace = true;
				MissionPeer missionPeer2 = list[0];
				this.FirstPlacePlayer = new MPEndOfBattlePlayerVM(missionPeer2, this.GetPeerScore(missionPeer2), 1);
			}
			if (list.Count > 1)
			{
				this.HasSecondPlace = true;
				MissionPeer missionPeer3 = list[1];
				this.SecondPlacePlayer = new MPEndOfBattlePlayerVM(missionPeer3, this.GetPeerScore(missionPeer3), 2);
			}
			if (list.Count > 2)
			{
				this.HasThirdPlace = true;
				MissionPeer missionPeer4 = list[2];
				this.ThirdPlacePlayer = new MPEndOfBattlePlayerVM(missionPeer4, this.GetPeerScore(missionPeer4), 3);
			}
			this.IsEnabled = true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000055B0 File Offset: 0x000037B0
		public void OnBattleEnded()
		{
			this._isBattleEnded = true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000055B9 File Offset: 0x000037B9
		private int GetPeerScore(MissionPeer peer)
		{
			if (peer == null)
			{
				return 0;
			}
			if (this._gameMode.GameType != MultiplayerGameType.Duel)
			{
				return peer.Score;
			}
			DuelMissionRepresentative component = peer.GetComponent<DuelMissionRepresentative>();
			if (component == null)
			{
				return 0;
			}
			return component.Score;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000055E6 File Offset: 0x000037E6
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000055EE File Offset: 0x000037EE
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000560C File Offset: 0x0000380C
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00005614 File Offset: 0x00003814
		[DataSourceProperty]
		public bool HasFirstPlace
		{
			get
			{
				return this._hasFirstPlace;
			}
			set
			{
				if (value != this._hasFirstPlace)
				{
					this._hasFirstPlace = value;
					base.OnPropertyChangedWithValue(value, "HasFirstPlace");
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005632 File Offset: 0x00003832
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x0000563A File Offset: 0x0000383A
		[DataSourceProperty]
		public bool HasSecondPlace
		{
			get
			{
				return this._hasSecondPlace;
			}
			set
			{
				if (value != this._hasSecondPlace)
				{
					this._hasSecondPlace = value;
					base.OnPropertyChangedWithValue(value, "HasSecondPlace");
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005658 File Offset: 0x00003858
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00005660 File Offset: 0x00003860
		[DataSourceProperty]
		public bool HasThirdPlace
		{
			get
			{
				return this._hasThirdPlace;
			}
			set
			{
				if (value != this._hasThirdPlace)
				{
					this._hasThirdPlace = value;
					base.OnPropertyChangedWithValue(value, "HasThirdPlace");
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000567E File Offset: 0x0000387E
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00005686 File Offset: 0x00003886
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000056A9 File Offset: 0x000038A9
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000056B1 File Offset: 0x000038B1
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000056D4 File Offset: 0x000038D4
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000056DC File Offset: 0x000038DC
		[DataSourceProperty]
		public MPEndOfBattlePlayerVM FirstPlacePlayer
		{
			get
			{
				return this._firstPlacePlayer;
			}
			set
			{
				if (value != this._firstPlacePlayer)
				{
					this._firstPlacePlayer = value;
					base.OnPropertyChangedWithValue<MPEndOfBattlePlayerVM>(value, "FirstPlacePlayer");
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000056FA File Offset: 0x000038FA
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00005702 File Offset: 0x00003902
		[DataSourceProperty]
		public MPEndOfBattlePlayerVM SecondPlacePlayer
		{
			get
			{
				return this._secondPlacePlayer;
			}
			set
			{
				if (value != this._secondPlacePlayer)
				{
					this._secondPlacePlayer = value;
					base.OnPropertyChangedWithValue<MPEndOfBattlePlayerVM>(value, "SecondPlacePlayer");
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005720 File Offset: 0x00003920
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005728 File Offset: 0x00003928
		[DataSourceProperty]
		public MPEndOfBattlePlayerVM ThirdPlacePlayer
		{
			get
			{
				return this._thirdPlacePlayer;
			}
			set
			{
				if (value != this._thirdPlacePlayer)
				{
					this._thirdPlacePlayer = value;
					base.OnPropertyChangedWithValue<MPEndOfBattlePlayerVM>(value, "ThirdPlacePlayer");
				}
			}
		}

		// Token: 0x04000087 RID: 135
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x04000088 RID: 136
		private readonly float _activeDelay;

		// Token: 0x04000089 RID: 137
		private bool _isBattleEnded;

		// Token: 0x0400008A RID: 138
		private float _activateTimeElapsed;

		// Token: 0x0400008B RID: 139
		private bool _isEnabled;

		// Token: 0x0400008C RID: 140
		private bool _hasFirstPlace;

		// Token: 0x0400008D RID: 141
		private bool _hasSecondPlace;

		// Token: 0x0400008E RID: 142
		private bool _hasThirdPlace;

		// Token: 0x0400008F RID: 143
		private string _titleText;

		// Token: 0x04000090 RID: 144
		private string _descriptionText;

		// Token: 0x04000091 RID: 145
		private MPEndOfBattlePlayerVM _firstPlacePlayer;

		// Token: 0x04000092 RID: 146
		private MPEndOfBattlePlayerVM _secondPlacePlayer;

		// Token: 0x04000093 RID: 147
		private MPEndOfBattlePlayerVM _thirdPlacePlayer;
	}
}
