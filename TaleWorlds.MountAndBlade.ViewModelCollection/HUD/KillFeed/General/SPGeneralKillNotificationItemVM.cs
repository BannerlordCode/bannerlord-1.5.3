using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.General
{
	// Token: 0x0200005F RID: 95
	public class SPGeneralKillNotificationItemVM : ViewModel
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x0001A79C File Offset: 0x0001899C
		public SPGeneralKillNotificationItemVM(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning, Action<SPGeneralKillNotificationItemVM> onRemove)
		{
			this._affectedAgent = affectedAgent;
			this._affectorAgent = affectorAgent;
			this._onRemove = onRemove;
			this._showNames = BannerlordConfig.KillFeedVisualType == 0;
			this.InitProperties(this._affectedAgent, this._affectorAgent, isHeadshot, isSuicide, isDrowning);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001A828 File Offset: 0x00018A28
		private void InitProperties(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning)
		{
			if (!this._showNames)
			{
				if (affectorAgent == null)
				{
					goto IL_004D;
				}
				BasicCharacterObject character = affectorAgent.Character;
				bool? flag = ((character != null) ? new bool?(character.IsHero) : null);
				bool flag2 = true;
				if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
				{
					goto IL_004D;
				}
			}
			this.MurdererName = affectorAgent.Name;
			IL_004D:
			this.MurdererType = SPGeneralKillNotificationItemVM.GetAgentType(affectorAgent);
			if (!this._showNames)
			{
				BasicCharacterObject character2 = affectedAgent.Character;
				if (character2 == null || !character2.IsHero)
				{
					goto IL_0081;
				}
			}
			this.VictimName = affectedAgent.Name;
			IL_0081:
			this.VictimType = SPGeneralKillNotificationItemVM.GetAgentType(affectedAgent);
			this.IsUnconscious = affectedAgent.State == AgentState.Unconscious;
			this.IsHeadshot = isHeadshot;
			this.IsSuicide = isSuicide;
			this.IsDrowning = isDrowning;
			Team team = affectedAgent.Team;
			Color color;
			if (team != null && team.IsValid)
			{
				if (affectedAgent.Team.IsPlayerAlly)
				{
					color = this._enemyColor;
				}
				else
				{
					color = this._friendlyColor;
				}
			}
			else
			{
				color = Color.FromUint(4284111450U);
			}
			this.BackgroundColor = color;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001A930 File Offset: 0x00018B30
		private static string GetAgentType(Agent agent)
		{
			if (((agent != null) ? agent.Character : null) == null)
			{
				return "None";
			}
			switch (agent.Character.DefaultFormationGroup)
			{
			case 0:
				return "Infantry_Light";
			case 1:
				return "Archer_Light";
			case 2:
				return "Cavalry_Light";
			case 3:
				return "HorseArcher_Light";
			case 4:
			case 5:
				return "Infantry_Heavy";
			case 6:
				return "Cavalry_Light";
			case 7:
				return "Cavalry_Heavy";
			case 8:
			case 9:
			case 10:
				return "Infantry_Heavy";
			default:
				return "None";
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001A9C6 File Offset: 0x00018BC6
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001A9D4 File Offset: 0x00018BD4
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0001A9DC File Offset: 0x00018BDC
		[DataSourceProperty]
		public string MurdererName
		{
			get
			{
				return this._murdererName;
			}
			set
			{
				if (value != this._murdererName)
				{
					this._murdererName = value;
					base.OnPropertyChangedWithValue<string>(value, "MurdererName");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001A9FF File Offset: 0x00018BFF
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0001AA07 File Offset: 0x00018C07
		[DataSourceProperty]
		public string MurdererType
		{
			get
			{
				return this._murdererType;
			}
			set
			{
				if (value != this._murdererType)
				{
					this._murdererType = value;
					base.OnPropertyChangedWithValue<string>(value, "MurdererType");
				}
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001AA2A File Offset: 0x00018C2A
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x0001AA32 File Offset: 0x00018C32
		[DataSourceProperty]
		public string VictimName
		{
			get
			{
				return this._victimName;
			}
			set
			{
				if (value != this._victimName)
				{
					this._victimName = value;
					base.OnPropertyChangedWithValue<string>(value, "VictimName");
				}
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001AA55 File Offset: 0x00018C55
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0001AA5D File Offset: 0x00018C5D
		[DataSourceProperty]
		public string VictimType
		{
			get
			{
				return this._victimType;
			}
			set
			{
				if (value != this._victimType)
				{
					this._victimType = value;
					base.OnPropertyChangedWithValue<string>(value, "VictimType");
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0001AA80 File Offset: 0x00018C80
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0001AA88 File Offset: 0x00018C88
		[DataSourceProperty]
		public bool IsUnconscious
		{
			get
			{
				return this._isUnconscious;
			}
			set
			{
				if (value != this._isUnconscious)
				{
					this._isUnconscious = value;
					base.OnPropertyChangedWithValue(value, "IsUnconscious");
				}
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0001AAA6 File Offset: 0x00018CA6
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0001AAAE File Offset: 0x00018CAE
		[DataSourceProperty]
		public bool IsHeadshot
		{
			get
			{
				return this._isHeadshot;
			}
			set
			{
				if (value != this._isHeadshot)
				{
					this._isHeadshot = value;
					base.OnPropertyChangedWithValue(value, "IsHeadshot");
				}
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0001AACC File Offset: 0x00018CCC
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0001AAD4 File Offset: 0x00018CD4
		[DataSourceProperty]
		public bool IsSuicide
		{
			get
			{
				return this._isSuicide;
			}
			set
			{
				if (value != this._isSuicide)
				{
					this._isSuicide = value;
					base.OnPropertyChangedWithValue(value, "IsSuicide");
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0001AAF2 File Offset: 0x00018CF2
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0001AAFA File Offset: 0x00018CFA
		[DataSourceProperty]
		public bool IsDrowning
		{
			get
			{
				return this._isDrowning;
			}
			set
			{
				if (value != this._isDrowning)
				{
					this._isDrowning = value;
					base.OnPropertyChangedWithValue(value, "IsDrowning");
				}
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0001AB18 File Offset: 0x00018D18
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0001AB20 File Offset: 0x00018D20
		[DataSourceProperty]
		public Color BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				if (value != this._backgroundColor)
				{
					this._backgroundColor = value;
					base.OnPropertyChangedWithValue(value, "BackgroundColor");
				}
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0001AB43 File Offset: 0x00018D43
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0001AB4B File Offset: 0x00018D4B
		[DataSourceProperty]
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChangedWithValue(value, "IsPaused");
				}
			}
		}

		// Token: 0x04000353 RID: 851
		private readonly Color _friendlyColor = new Color(0.54296875f, 0.77734375f, 0.421875f, 1f);

		// Token: 0x04000354 RID: 852
		private readonly Color _enemyColor = new Color(0.953125f, 0.48828125f, 0.42578125f, 1f);

		// Token: 0x04000355 RID: 853
		private readonly Agent _affectedAgent;

		// Token: 0x04000356 RID: 854
		private readonly Agent _affectorAgent;

		// Token: 0x04000357 RID: 855
		private readonly Action<SPGeneralKillNotificationItemVM> _onRemove;

		// Token: 0x04000358 RID: 856
		private readonly bool _showNames;

		// Token: 0x04000359 RID: 857
		private string _murdererName;

		// Token: 0x0400035A RID: 858
		private string _murdererType;

		// Token: 0x0400035B RID: 859
		private string _victimName;

		// Token: 0x0400035C RID: 860
		private string _victimType;

		// Token: 0x0400035D RID: 861
		private bool _isUnconscious;

		// Token: 0x0400035E RID: 862
		private bool _isHeadshot;

		// Token: 0x0400035F RID: 863
		private bool _isSuicide;

		// Token: 0x04000360 RID: 864
		private bool _isDrowning;

		// Token: 0x04000361 RID: 865
		private Color _backgroundColor;

		// Token: 0x04000362 RID: 866
		private bool _isPaused;
	}
}
