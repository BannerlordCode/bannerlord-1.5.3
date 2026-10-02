using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.BattleScore;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x0200000F RID: 15
	public abstract class ScoreboardBaseVM : ViewModel
	{
		// Token: 0x060000CF RID: 207 RVA: 0x000045A4 File Offset: 0x000027A4
		public ScoreboardBaseVM(BattleScoreContext scoreboardContext)
		{
			this.ScoreboardContext = scoreboardContext;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000045C4 File Offset: 0x000027C4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.KillHint = new HintViewModel(GameTexts.FindText("str_battle_result_score_sort_button", "0"), null);
			this.DeadHint = new HintViewModel(GameTexts.FindText("str_battle_result_score_sort_button", "1"), null);
			this.WoundedHint = new HintViewModel(GameTexts.FindText("str_battle_result_score_sort_button", "2"), null);
			this.RoutedHint = new HintViewModel(GameTexts.FindText("str_battle_result_score_sort_button", "3"), null);
			this.RemainingHint = new HintViewModel(GameTexts.FindText("str_battle_result_score_sort_button", "4"), null);
			this.UpgradeHint = new HintViewModel(GameTexts.FindText("str_battle_result_score_sort_button", "5"), null);
			this.UpdateQuitText();
			GameTexts.SetVariable("KEY", Game.Current.GameTextManager.GetHotKeyGameText("Generic", 4));
			this._retreatInquiryData = new InquiryData("", GameTexts.FindText("str_can_not_retreat", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null);
			SPScoreboardSideVM attackers = this.Attackers;
			if (attackers != null)
			{
				attackers.RefreshValues();
			}
			SPScoreboardSideVM defenders = this.Defenders;
			if (defenders != null)
			{
				defenders.RefreshValues();
			}
			this.ShowScoreboardText = new TextObject("{=5Ixsvn3s}Toggle scoreboard", null).ToString();
			this.FastForwardText = new TextObject("{=HH7LDwlK}Toggle Fast Forward", null).ToString();
			this.MoraleText = GameTexts.FindText("str_morale", null).ToString();
			InputKeyItemVM showMouseKey = this.ShowMouseKey;
			if (showMouseKey != null)
			{
				showMouseKey.RefreshValues();
			}
			InputKeyItemVM showScoreboardKey = this.ShowScoreboardKey;
			if (showScoreboardKey != null)
			{
				showScoreboardKey.RefreshValues();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.RefreshValues();
			}
			InputKeyItemVM fastForwardKey = this.FastForwardKey;
			if (fastForwardKey != null)
			{
				fastForwardKey.RefreshValues();
			}
			InputKeyItemVM pauseInputKey = this.PauseInputKey;
			if (pauseInputKey == null)
			{
				return;
			}
			pauseInputKey.RefreshValues();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000479C File Offset: 0x0000299C
		public void OnMainHeroDeath()
		{
			this.IsMainCharacterDead = true;
			IMissionScreen missionScreen = this._missionScreen;
			if (missionScreen == null)
			{
				return;
			}
			missionScreen.SetOrderFlagVisibility(false);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000047B6 File Offset: 0x000029B6
		public void OnTakenControlOfAnotherAgent()
		{
			this.IsMainCharacterDead = false;
			IMissionScreen missionScreen = this._missionScreen;
			if (missionScreen == null)
			{
				return;
			}
			missionScreen.SetOrderFlagVisibility(false);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000047D0 File Offset: 0x000029D0
		public virtual void Initialize(IMissionScreen missionScreen, Mission mission, Action releaseSimulationSources, Action<bool> onToggle)
		{
			this.OnToggle = onToggle;
			this._missionScreen = missionScreen;
			this._mission = mission;
			this._releaseSimulationSources = releaseSimulationSources;
			this.BattleResult = "";
			this.BattleResultIndex = -1;
			this.IsOver = false;
			this.ShowScoreboard = false;
			Action<bool> onToggle2 = this.OnToggle;
			if (onToggle2 != null)
			{
				onToggle2(false);
			}
			if (mission != null)
			{
				this._battleEndLogic = this._mission.GetMissionBehavior<BattleEndLogic>();
			}
			this.NeutralTroops = new SPScoreboardSideVM(null, null, false, false);
			this.PowerComparer = new PowerLevelComparer(1.0, 1.0);
			this.RefreshValues();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004874 File Offset: 0x00002A74
		protected virtual void UpdateQuitText()
		{
			if (this.IsOver)
			{
				this.QuitText = GameTexts.FindText("str_done", null).ToString();
				return;
			}
			if (this.IsMainCharacterDead && !this.IsSimulation)
			{
				this.QuitText = GameTexts.FindText("str_end_battle", null).ToString();
				return;
			}
			this.QuitText = GameTexts.FindText("str_retreat", null).ToString();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000048DD File Offset: 0x00002ADD
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000048E0 File Offset: 0x00002AE0
		public void Tick(float dt)
		{
			this.PowerComparer.IsEnabled = this.ScoreboardContext.IsPowerComparisonRelevant;
			this.IsPowerComparerEnabled = this.PowerComparer.IsEnabled && !BannerlordConfig.HideBattleUI && !MBCommon.IsPaused;
			this.OnTick(dt);
		}

		// Token: 0x060000D7 RID: 215
		protected abstract void OnTick(float dt);

		// Token: 0x060000D8 RID: 216 RVA: 0x0000492F File Offset: 0x00002B2F
		protected SPScoreboardSideVM GetSide(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Defender)
			{
				return this.Defenders;
			}
			if (side == BattleSideEnum.Attacker)
			{
				return this.Attackers;
			}
			return this.NeutralTroops;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000494C File Offset: 0x00002B4C
		public void SetMouseState(bool visible)
		{
			this._mouseState = (visible ? ScoreboardBaseVM.MouseState.Visible : ScoreboardBaseVM.MouseState.NotVisible);
			this.IsMouseEnabled = visible;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004964 File Offset: 0x00002B64
		public static string GetFormattedTimeTextFromSeconds(int seconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			string text = "";
			if (timeSpan.Hours > 0)
			{
				text += string.Format("{0:D2}{1}:", timeSpan.Hours, ScoreboardBaseVM._hourAbbrString);
			}
			text += string.Format("{0:D2}{1}:", timeSpan.Minutes, ScoreboardBaseVM._minuteAbbrString);
			return text + string.Format("{0:D2}{1}", timeSpan.Seconds, ScoreboardBaseVM._secondAbbrString);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000049F0 File Offset: 0x00002BF0
		protected float GetBattleMoraleOfSide(BattleSideEnum side)
		{
			if (Mission.Current == null)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 0;
			bool flag = false;
			for (int i = 0; i < Mission.Current.Teams.Count; i++)
			{
				Team team = Mission.Current.Teams[i];
				if (team.Side == side)
				{
					for (int j = 0; j < team.ActiveAgents.Count; j++)
					{
						Agent agent = team.ActiveAgents[j];
						if (agent.IsHuman)
						{
							if (agent.IsAIControlled)
							{
								num2++;
								num += agent.GetMorale();
							}
							else
							{
								flag = true;
							}
						}
					}
				}
			}
			if (num2 > 0)
			{
				return MBMath.ClampFloat(num / (float)num2, 0f, 100f);
			}
			if (flag)
			{
				return 50f;
			}
			return 0f;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004AC4 File Offset: 0x00002CC4
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM showMouseKey = this.ShowMouseKey;
			if (showMouseKey != null)
			{
				showMouseKey.OnFinalize();
			}
			InputKeyItemVM showScoreboardKey = this.ShowScoreboardKey;
			if (showScoreboardKey != null)
			{
				showScoreboardKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM fastForwardKey = this.FastForwardKey;
			if (fastForwardKey != null)
			{
				fastForwardKey.OnFinalize();
			}
			InputKeyItemVM pauseInputKey = this.PauseInputKey;
			if (pauseInputKey == null)
			{
				return;
			}
			pauseInputKey.OnFinalize();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004B2B File Offset: 0x00002D2B
		public virtual void ExecuteShowScoreboardAction()
		{
			this.ShowScoreboard = !this.ShowScoreboard;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004B3C File Offset: 0x00002D3C
		public virtual void ExecutePlayAction()
		{
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004B3E File Offset: 0x00002D3E
		public virtual void ExecuteFastForwardAction()
		{
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004B40 File Offset: 0x00002D40
		public virtual void ExecutePauseSimulationAction()
		{
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004B42 File Offset: 0x00002D42
		public virtual void ExecuteEndSimulationAction()
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004B44 File Offset: 0x00002D44
		public virtual void ExecuteQuitAction()
		{
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004B46 File Offset: 0x00002D46
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004B4E File Offset: 0x00002D4E
		protected int MissionTimeInSeconds
		{
			get
			{
				return this._missionTimeInSeconds;
			}
			set
			{
				if (value != this._missionTimeInSeconds)
				{
					this._missionTimeInSeconds = value;
					this.MissionTimeStr = ScoreboardBaseVM.GetFormattedTimeTextFromSeconds(this._missionTimeInSeconds);
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004B71 File Offset: 0x00002D71
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004B79 File Offset: 0x00002D79
		[DataSourceProperty]
		public string MissionTimeStr
		{
			get
			{
				return this._missionTimeStr;
			}
			set
			{
				if (value != this._missionTimeStr)
				{
					this._missionTimeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "MissionTimeStr");
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004B9C File Offset: 0x00002D9C
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00004BA4 File Offset: 0x00002DA4
		[DataSourceProperty]
		public bool IsPowerComparerEnabled
		{
			get
			{
				return this._isPowerComparerEnabled;
			}
			set
			{
				if (value != this._isPowerComparerEnabled)
				{
					this._isPowerComparerEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPowerComparerEnabled");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004BC2 File Offset: 0x00002DC2
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00004BCA File Offset: 0x00002DCA
		[DataSourceProperty]
		public string QuitText
		{
			get
			{
				return this._quitText;
			}
			set
			{
				if (value != this._quitText)
				{
					this._quitText = value;
					base.OnPropertyChangedWithValue<string>(value, "QuitText");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004BED File Offset: 0x00002DED
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00004BF5 File Offset: 0x00002DF5
		[DataSourceProperty]
		public string ShowScoreboardText
		{
			get
			{
				return this._showScoreboardText;
			}
			set
			{
				if (value != this._showScoreboardText)
				{
					this._showScoreboardText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShowScoreboardText");
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004C18 File Offset: 0x00002E18
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00004C20 File Offset: 0x00002E20
		[DataSourceProperty]
		public string FastForwardText
		{
			get
			{
				return this._fastForwardText;
			}
			set
			{
				if (value != this._fastForwardText)
				{
					this._fastForwardText = value;
					base.OnPropertyChangedWithValue<string>(value, "FastForwardText");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004C43 File Offset: 0x00002E43
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004C4B File Offset: 0x00002E4B
		[DataSourceProperty]
		public string MoraleText
		{
			get
			{
				return this._moraleText;
			}
			set
			{
				if (value != this._moraleText)
				{
					this._moraleText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoraleText");
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004C6E File Offset: 0x00002E6E
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004C76 File Offset: 0x00002E76
		[DataSourceProperty]
		public SPScoreboardSideVM Attackers
		{
			get
			{
				return this._attackers;
			}
			set
			{
				if (value != this._attackers)
				{
					this._attackers = value;
					base.OnPropertyChangedWithValue<SPScoreboardSideVM>(value, "Attackers");
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004C94 File Offset: 0x00002E94
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004C9C File Offset: 0x00002E9C
		[DataSourceProperty]
		public SPScoreboardSideVM Defenders
		{
			get
			{
				return this._defenders;
			}
			set
			{
				if (value != this._defenders)
				{
					this._defenders = value;
					base.OnPropertyChangedWithValue<SPScoreboardSideVM>(value, "Defenders");
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004CBA File Offset: 0x00002EBA
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004CC2 File Offset: 0x00002EC2
		[DataSourceProperty]
		public SPScoreboardSideVM NeutralTroops
		{
			get
			{
				return this._neutralTroops;
			}
			set
			{
				if (value != this._neutralTroops)
				{
					this._neutralTroops = value;
					base.OnPropertyChangedWithValue<SPScoreboardSideVM>(value, "NeutralTroops");
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004CE0 File Offset: 0x00002EE0
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004CE8 File Offset: 0x00002EE8
		[DataSourceProperty]
		public HintViewModel KillHint
		{
			get
			{
				return this._killHint;
			}
			set
			{
				if (value != this._killHint)
				{
					this._killHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "KillHint");
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004D06 File Offset: 0x00002F06
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004D0E File Offset: 0x00002F0E
		[DataSourceProperty]
		public HintViewModel DeadHint
		{
			get
			{
				return this._deadHint;
			}
			set
			{
				if (value != this._deadHint)
				{
					this._deadHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DeadHint");
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004D2C File Offset: 0x00002F2C
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004D34 File Offset: 0x00002F34
		[DataSourceProperty]
		public HintViewModel UpgradeHint
		{
			get
			{
				return this._upgradeHint;
			}
			set
			{
				if (value != this._upgradeHint)
				{
					this._upgradeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UpgradeHint");
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004D52 File Offset: 0x00002F52
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004D5A File Offset: 0x00002F5A
		[DataSourceProperty]
		public HintViewModel WoundedHint
		{
			get
			{
				return this._woundedHint;
			}
			set
			{
				if (value != this._woundedHint)
				{
					this._woundedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "WoundedHint");
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004D78 File Offset: 0x00002F78
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004D80 File Offset: 0x00002F80
		[DataSourceProperty]
		public HintViewModel RoutedHint
		{
			get
			{
				return this._routedHint;
			}
			set
			{
				if (value != this._routedHint)
				{
					this._routedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RoutedHint");
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00004D9E File Offset: 0x00002F9E
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00004DA6 File Offset: 0x00002FA6
		[DataSourceProperty]
		public HintViewModel RemainingHint
		{
			get
			{
				return this._remainingHint;
			}
			set
			{
				if (value != this._remainingHint)
				{
					this._remainingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RemainingHint");
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004DC4 File Offset: 0x00002FC4
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004DCC File Offset: 0x00002FCC
		[DataSourceProperty]
		public int BattleResultIndex
		{
			get
			{
				return this._battleResultIndex;
			}
			set
			{
				if (value != this._battleResultIndex)
				{
					this._battleResultIndex = value;
					base.OnPropertyChangedWithValue(value, "BattleResultIndex");
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004DEA File Offset: 0x00002FEA
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004DF2 File Offset: 0x00002FF2
		[DataSourceProperty]
		public string BattleResult
		{
			get
			{
				return this._battleResult;
			}
			set
			{
				if (value != this._battleResult)
				{
					this._battleResult = value;
					base.OnPropertyChangedWithValue<string>(value, "BattleResult");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004E15 File Offset: 0x00003015
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00004E1D File Offset: 0x0000301D
		[DataSourceProperty]
		public bool IsMouseEnabled
		{
			get
			{
				return this._isMouseEnabled;
			}
			set
			{
				if (value != this._isMouseEnabled)
				{
					this._isMouseEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMouseEnabled");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004E3B File Offset: 0x0000303B
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00004E43 File Offset: 0x00003043
		[DataSourceProperty]
		public bool IsOver
		{
			get
			{
				return this._isOver;
			}
			set
			{
				if (value != this._isOver)
				{
					this._isOver = value;
					base.OnPropertyChangedWithValue(value, "IsOver");
					this.UpdateQuitText();
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004E67 File Offset: 0x00003067
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00004E6F File Offset: 0x0000306F
		[DataSourceProperty]
		public string SimulationResult
		{
			get
			{
				return this._simulationResult;
			}
			set
			{
				if (value != this._simulationResult)
				{
					this._simulationResult = value;
					base.OnPropertyChangedWithValue<string>(value, "SimulationResult");
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004E92 File Offset: 0x00003092
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004E9A File Offset: 0x0000309A
		[DataSourceProperty]
		public bool IsMainCharacterDead
		{
			get
			{
				return this._isMainCharacterDead;
			}
			set
			{
				if (value != this._isMainCharacterDead)
				{
					this._isMainCharacterDead = value;
					base.OnPropertyChangedWithValue(value, "IsMainCharacterDead");
					this.UpdateQuitText();
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004EBE File Offset: 0x000030BE
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00004EC6 File Offset: 0x000030C6
		[DataSourceProperty]
		public bool ShowScoreboard
		{
			get
			{
				return this._showScoreboard;
			}
			set
			{
				if (value != this._showScoreboard)
				{
					this._showScoreboard = value;
					base.OnPropertyChangedWithValue(value, "ShowScoreboard");
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004EE4 File Offset: 0x000030E4
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00004EEC File Offset: 0x000030EC
		[DataSourceProperty]
		public bool IsSimulation
		{
			get
			{
				return this._isSimulation;
			}
			set
			{
				if (value != this._isSimulation)
				{
					this._isSimulation = value;
					base.OnPropertyChangedWithValue(value, "IsSimulation");
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004F0A File Offset: 0x0000310A
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00004F12 File Offset: 0x00003112
		[DataSourceProperty]
		public bool IsNavalBattle
		{
			get
			{
				return this._isNavalBattle;
			}
			set
			{
				if (value != this._isNavalBattle)
				{
					this._isNavalBattle = value;
					base.OnPropertyChangedWithValue(value, "IsNavalBattle");
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00004F30 File Offset: 0x00003130
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00004F38 File Offset: 0x00003138
		[DataSourceProperty]
		public bool IsFastForwarding
		{
			get
			{
				return this._isFastForwarding;
			}
			set
			{
				if (value != this._isFastForwarding)
				{
					this._isFastForwarding = value;
					base.OnPropertyChangedWithValue(value, "IsFastForwarding");
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00004F56 File Offset: 0x00003156
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00004F5E File Offset: 0x0000315E
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00004F7C File Offset: 0x0000317C
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00004F84 File Offset: 0x00003184
		[DataSourceProperty]
		public PowerLevelComparer PowerComparer
		{
			get
			{
				return this._powerComparer;
			}
			set
			{
				if (value != this._powerComparer)
				{
					this._powerComparer = value;
					base.OnPropertyChangedWithValue<PowerLevelComparer>(value, "PowerComparer");
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004FA4 File Offset: 0x000031A4
		public virtual void SetShortcuts(ScoreboardHotkeys shortcuts)
		{
			this.ShowMouseKey = InputKeyItemVM.CreateFromGameKey(shortcuts.ShowMouseHotkey, false);
			this.ShowScoreboardKey = InputKeyItemVM.CreateFromGameKey(shortcuts.ShowScoreboardHotkey, false);
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(shortcuts.DoneInputKey, true);
			this.FastForwardKey = InputKeyItemVM.CreateFromHotKey(shortcuts.FastForwardKey, true);
			this.PauseInputKey = InputKeyItemVM.CreateFromHotKey(shortcuts.PauseInputKey, true);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000500B File Offset: 0x0000320B
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00005013 File Offset: 0x00003213
		[DataSourceProperty]
		public InputKeyItemVM ShowMouseKey
		{
			get
			{
				return this._showMouseKey;
			}
			set
			{
				if (value != this._showMouseKey)
				{
					this._showMouseKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShowMouseKey");
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00005031 File Offset: 0x00003231
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00005039 File Offset: 0x00003239
		[DataSourceProperty]
		public InputKeyItemVM ShowScoreboardKey
		{
			get
			{
				return this._showScoreboardKey;
			}
			set
			{
				if (value != this._showScoreboardKey)
				{
					this._showScoreboardKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShowScoreboardKey");
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005057 File Offset: 0x00003257
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000505F File Offset: 0x0000325F
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000507D File Offset: 0x0000327D
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00005085 File Offset: 0x00003285
		[DataSourceProperty]
		public InputKeyItemVM FastForwardKey
		{
			get
			{
				return this._fastForwardKey;
			}
			set
			{
				if (value != this._fastForwardKey)
				{
					this._fastForwardKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "FastForwardKey");
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000050A3 File Offset: 0x000032A3
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000050AB File Offset: 0x000032AB
		[DataSourceProperty]
		public InputKeyItemVM PauseInputKey
		{
			get
			{
				return this._pauseInputKey;
			}
			set
			{
				if (value != this._pauseInputKey)
				{
					this._pauseInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PauseInputKey");
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000050C9 File Offset: 0x000032C9
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000050CC File Offset: 0x000032CC
		[DataSourceProperty]
		public virtual MBBindingList<BattleResultVM> BattleResults
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0400005B RID: 91
		protected Action OnFastForwardIncreaseSpeed;

		// Token: 0x0400005C RID: 92
		protected Action OnFastForwardDecreaseSpeed;

		// Token: 0x0400005D RID: 93
		protected Action OnFastForwardResetSpeed;

		// Token: 0x0400005E RID: 94
		private static readonly TextObject _hourAbbrString = GameTexts.FindText("str_hour_abbr", null);

		// Token: 0x0400005F RID: 95
		private static readonly TextObject _minuteAbbrString = GameTexts.FindText("str_minute_abbr", null);

		// Token: 0x04000060 RID: 96
		private static readonly TextObject _secondAbbrString = GameTexts.FindText("str_second_abbr", null);

		// Token: 0x04000061 RID: 97
		protected BattleSideEnum PlayerSide;

		// Token: 0x04000062 RID: 98
		protected IMissionScreen _missionScreen;

		// Token: 0x04000063 RID: 99
		protected Mission _mission;

		// Token: 0x04000064 RID: 100
		protected BattleEndLogic _battleEndLogic;

		// Token: 0x04000065 RID: 101
		protected InquiryData _retreatInquiryData;

		// Token: 0x04000066 RID: 102
		protected Action _releaseSimulationSources;

		// Token: 0x04000067 RID: 103
		protected Action<bool> OnToggle;

		// Token: 0x04000068 RID: 104
		private ScoreboardBaseVM.MouseState _mouseState;

		// Token: 0x04000069 RID: 105
		protected const float MissionEndScoreboardDelayTime = 1.5f;

		// Token: 0x0400006A RID: 106
		protected BattleScoreContext ScoreboardContext;

		// Token: 0x0400006B RID: 107
		private string _quitText;

		// Token: 0x0400006C RID: 108
		private string _showScoreboardText;

		// Token: 0x0400006D RID: 109
		private string _fastForwardText;

		// Token: 0x0400006E RID: 110
		private string _moraleText;

		// Token: 0x0400006F RID: 111
		private bool _isFastForwarding;

		// Token: 0x04000070 RID: 112
		private bool _isPaused;

		// Token: 0x04000071 RID: 113
		private bool _isMainCharacterDead;

		// Token: 0x04000072 RID: 114
		private bool _showScoreboard;

		// Token: 0x04000073 RID: 115
		private bool _isSimulation = true;

		// Token: 0x04000074 RID: 116
		private bool _isNavalBattle;

		// Token: 0x04000075 RID: 117
		private bool _isMouseEnabled;

		// Token: 0x04000076 RID: 118
		private PowerLevelComparer _powerComparer;

		// Token: 0x04000077 RID: 119
		private bool _isOver;

		// Token: 0x04000078 RID: 120
		private string _battleResult;

		// Token: 0x04000079 RID: 121
		private int _battleResultIndex = -1;

		// Token: 0x0400007A RID: 122
		private HintViewModel _killHint;

		// Token: 0x0400007B RID: 123
		private HintViewModel _upgradeHint;

		// Token: 0x0400007C RID: 124
		private HintViewModel _deadHint;

		// Token: 0x0400007D RID: 125
		private HintViewModel _woundedHint;

		// Token: 0x0400007E RID: 126
		private HintViewModel _routedHint;

		// Token: 0x0400007F RID: 127
		private HintViewModel _remainingHint;

		// Token: 0x04000080 RID: 128
		private SPScoreboardSideVM _attackers;

		// Token: 0x04000081 RID: 129
		private SPScoreboardSideVM _defenders;

		// Token: 0x04000082 RID: 130
		private SPScoreboardSideVM _neutralTroops;

		// Token: 0x04000083 RID: 131
		private bool _isPowerComparerEnabled;

		// Token: 0x04000084 RID: 132
		private int _missionTimeInSeconds;

		// Token: 0x04000085 RID: 133
		private string _missionTimeStr;

		// Token: 0x04000086 RID: 134
		private string _simulationResult;

		// Token: 0x04000087 RID: 135
		private InputKeyItemVM _showMouseKey;

		// Token: 0x04000088 RID: 136
		private InputKeyItemVM _showScoreboardKey;

		// Token: 0x04000089 RID: 137
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400008A RID: 138
		private InputKeyItemVM _fastForwardKey;

		// Token: 0x0400008B RID: 139
		private InputKeyItemVM _pauseInputKey;

		// Token: 0x0200008E RID: 142
		internal enum MouseState
		{
			// Token: 0x04000562 RID: 1378
			NotVisible,
			// Token: 0x04000563 RID: 1379
			Visible
		}

		// Token: 0x0200008F RID: 143
		public enum Categories
		{
			// Token: 0x04000565 RID: 1381
			Party,
			// Token: 0x04000566 RID: 1382
			Tactical,
			// Token: 0x04000567 RID: 1383
			NumOfCategories
		}

		// Token: 0x02000090 RID: 144
		protected enum BattleResultType
		{
			// Token: 0x04000569 RID: 1385
			NotOver = -1,
			// Token: 0x0400056A RID: 1386
			Defeat,
			// Token: 0x0400056B RID: 1387
			Victory,
			// Token: 0x0400056C RID: 1388
			Retreat
		}
	}
}
