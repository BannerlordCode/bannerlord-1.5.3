using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000009 RID: 9
	public class MPDeathCardVM : ViewModel
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00003580 File Offset: 0x00001780
		public MPDeathCardVM(MultiplayerGameType gameType)
		{
			this.KillCountsEnabled = gameType != MultiplayerGameType.Captain;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000035F5 File Offset: 0x000017F5
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.YouText = GameTexts.FindText("str_death_card_you", null).ToString();
			this.Deactivate();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000361C File Offset: 0x0000181C
		public void OnMainAgentRemoved(Agent affectorAgent, KillingBlow blow)
		{
			this.ResetProperties();
			if (affectorAgent != null && affectorAgent == Agent.Main)
			{
				this.TitleText = this._killedSelfText.ToString();
				this.IsSelfInflicted = true;
			}
			else if (affectorAgent != null && affectorAgent.IsMount && affectorAgent.RiderAgent == null)
			{
				this._killedByStrayHorse.SetTextVariable("MOUNT_NAME", affectorAgent.NameTextObject);
				this.TitleText = this._killedByStrayHorse.ToString();
				this.IsSelfInflicted = true;
			}
			else
			{
				this.IsSelfInflicted = false;
				this.TitleText = this._killedByText.ToString();
			}
			Team team = ((affectorAgent != null) ? affectorAgent.Team : null);
			Agent main = Agent.Main;
			this.KillerText = ((team == ((main != null) ? main.Team : null)) ? this._allyText.ToString() : this._enemyText.ToString());
			if (this.IsSelfInflicted)
			{
				this.PlayerProperties = new MPPlayerVM(GameNetwork.MyPeer.GetComponent<MissionPeer>());
				this.PlayerProperties.RefreshDivision(false);
			}
			else
			{
				this.KillerName = ((affectorAgent != null) ? affectorAgent.Name : null) ?? "";
				if (blow.WeaponItemKind >= 0)
				{
					this.UsedWeaponName = ItemObject.GetItemFromWeaponKind(blow.WeaponItemKind).Name.ToString();
				}
				else
				{
					this.UsedWeaponName = new TextObject("{=GAZ5QLZi}Unarmed", null).ToString();
				}
				bool isServerOrRecorder = GameNetwork.IsServerOrRecorder;
				if (((affectorAgent != null) ? affectorAgent.MissionPeer : null) != null)
				{
					this.PlayerProperties = new MPPlayerVM(affectorAgent.MissionPeer);
					this.PlayerProperties.RefreshDivision(false);
					this.NumOfTimesPlayerKilled = Agent.Main.MissionPeer.GetNumberOfTimesPeerKilledPeer(affectorAgent.MissionPeer);
					this.NumOfTimesPlayerGotKilled = affectorAgent.MissionPeer.GetNumberOfTimesPeerKilledPeer(Agent.Main.MissionPeer) + (isServerOrRecorder ? 0 : 1);
				}
				else if (((affectorAgent != null) ? affectorAgent.OwningAgentMissionPeer : null) != null)
				{
					this.PlayerProperties = new MPPlayerVM(affectorAgent.OwningAgentMissionPeer);
					this.PlayerProperties.RefreshDivision(false);
				}
				else
				{
					this.PlayerProperties = new MPPlayerVM(affectorAgent);
				}
			}
			this.IsActive = true;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003823 File Offset: 0x00001A23
		private void ResetProperties()
		{
			this.IsActive = false;
			this.TitleText = "";
			this.UsedWeaponName = "";
			this.BodyPartHit = -1;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003849 File Offset: 0x00001A49
		public void Deactivate()
		{
			this.IsActive = false;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003852 File Offset: 0x00001A52
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000385A File Offset: 0x00001A5A
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003878 File Offset: 0x00001A78
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00003880 File Offset: 0x00001A80
		[DataSourceProperty]
		public bool IsSelfInflicted
		{
			get
			{
				return this._isSelfInflicted;
			}
			set
			{
				if (value != this._isSelfInflicted)
				{
					this._isSelfInflicted = value;
					base.OnPropertyChangedWithValue(value, "IsSelfInflicted");
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000389E File Offset: 0x00001A9E
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000038A6 File Offset: 0x00001AA6
		[DataSourceProperty]
		public bool KillCountsEnabled
		{
			get
			{
				return this._killCountsEnabled;
			}
			set
			{
				if (value != this._killCountsEnabled)
				{
					this._killCountsEnabled = value;
					base.OnPropertyChangedWithValue(value, "KillCountsEnabled");
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000038C4 File Offset: 0x00001AC4
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000038CC File Offset: 0x00001ACC
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000038EF File Offset: 0x00001AEF
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000038F7 File Offset: 0x00001AF7
		[DataSourceProperty]
		public string UsedWeaponName
		{
			get
			{
				return this._usedWeaponName;
			}
			set
			{
				if (value != this._usedWeaponName)
				{
					this._usedWeaponName = value;
					base.OnPropertyChangedWithValue<string>(value, "UsedWeaponName");
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000391A File Offset: 0x00001B1A
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003922 File Offset: 0x00001B22
		[DataSourceProperty]
		public string KillerName
		{
			get
			{
				return this._killerName;
			}
			set
			{
				if (value != this._killerName)
				{
					this._killerName = value;
					base.OnPropertyChangedWithValue<string>(value, "KillerName");
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003945 File Offset: 0x00001B45
		// (set) Token: 0x06000077 RID: 119 RVA: 0x0000394D File Offset: 0x00001B4D
		[DataSourceProperty]
		public string KillerText
		{
			get
			{
				return this._killerText;
			}
			set
			{
				if (value != this._killerText)
				{
					this._killerText = value;
					base.OnPropertyChangedWithValue<string>(value, "KillerText");
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003970 File Offset: 0x00001B70
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00003978 File Offset: 0x00001B78
		[DataSourceProperty]
		public string YouText
		{
			get
			{
				return this._youText;
			}
			set
			{
				if (value != this._youText)
				{
					this._youText = value;
					base.OnPropertyChangedWithValue<string>(value, "YouText");
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000399B File Offset: 0x00001B9B
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000039A3 File Offset: 0x00001BA3
		[DataSourceProperty]
		public MPPlayerVM PlayerProperties
		{
			get
			{
				return this._playerProperties;
			}
			set
			{
				if (value != this._playerProperties)
				{
					this._playerProperties = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "PlayerProperties");
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000039C1 File Offset: 0x00001BC1
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000039C9 File Offset: 0x00001BC9
		[DataSourceProperty]
		public int BodyPartHit
		{
			get
			{
				return this._bodyPartHit;
			}
			set
			{
				if (value != this._bodyPartHit)
				{
					this._bodyPartHit = value;
					base.OnPropertyChangedWithValue(value, "BodyPartHit");
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000039E7 File Offset: 0x00001BE7
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000039EF File Offset: 0x00001BEF
		[DataSourceProperty]
		public int NumOfTimesPlayerKilled
		{
			get
			{
				return this._numOfTimesPlayerKilled;
			}
			set
			{
				if (value != this._numOfTimesPlayerKilled)
				{
					this._numOfTimesPlayerKilled = value;
					base.OnPropertyChangedWithValue(value, "NumOfTimesPlayerKilled");
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003A0D File Offset: 0x00001C0D
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00003A15 File Offset: 0x00001C15
		[DataSourceProperty]
		public int NumOfTimesPlayerGotKilled
		{
			get
			{
				return this._numOfTimesPlayerGotKilled;
			}
			set
			{
				if (value != this._numOfTimesPlayerGotKilled)
				{
					this._numOfTimesPlayerGotKilled = value;
					base.OnPropertyChangedWithValue(value, "NumOfTimesPlayerGotKilled");
				}
			}
		}

		// Token: 0x0400003C RID: 60
		private readonly TextObject _killedByStrayHorse = GameTexts.FindText("str_killed_by_stray_horse", null);

		// Token: 0x0400003D RID: 61
		private readonly TextObject _killedSelfText = GameTexts.FindText("str_killed_self", null);

		// Token: 0x0400003E RID: 62
		private readonly TextObject _killedByText = GameTexts.FindText("str_killed_by", null);

		// Token: 0x0400003F RID: 63
		private readonly TextObject _enemyText = GameTexts.FindText("str_death_card_enemy", null);

		// Token: 0x04000040 RID: 64
		private readonly TextObject _allyText = GameTexts.FindText("str_death_card_ally", null);

		// Token: 0x04000041 RID: 65
		private bool _isActive;

		// Token: 0x04000042 RID: 66
		private bool _isSelfInflicted;

		// Token: 0x04000043 RID: 67
		private bool _killCountsEnabled;

		// Token: 0x04000044 RID: 68
		private int _numOfTimesPlayerKilled;

		// Token: 0x04000045 RID: 69
		private int _numOfTimesPlayerGotKilled;

		// Token: 0x04000046 RID: 70
		private string _titleText;

		// Token: 0x04000047 RID: 71
		private string _usedWeaponName;

		// Token: 0x04000048 RID: 72
		private string _killerName;

		// Token: 0x04000049 RID: 73
		private string _killerText;

		// Token: 0x0400004A RID: 74
		private string _youText;

		// Token: 0x0400004B RID: 75
		private MPPlayerVM _playerProperties;

		// Token: 0x0400004C RID: 76
		private int _bodyPartHit;
	}
}
