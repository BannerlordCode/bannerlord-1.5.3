using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000012 RID: 18
	public class SPScoreboardSideVM : ViewModel
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00005780 File Offset: 0x00003980
		public SPScoreboardSideVM(TextObject name, Banner sideFlag, bool isSimulation, bool isPlayerSide)
		{
			SPScoreboardSideVM <>4__this = this;
			this.Parties = new MBBindingList<SPScoreboardPartyVM>();
			this.Ships = new MBBindingList<SPScoreboardShipVM>();
			this.Score = new SPScoreboardStatsVM(name);
			this.IsPlayerSide = isPlayerSide;
			MBBindingList<SPScoreboardPartyVM> parties = this.Parties;
			this.SortController = new SPScoreboardSortControllerVM(ref parties);
			this.Parties = parties;
			if (sideFlag != null)
			{
				this.BannerVisual = new BannerImageIdentifierVM(sideFlag, true);
				this.BannerVisualSmall = new BannerImageIdentifierVM(sideFlag, false);
			}
			this.MoraleHint = new BasicTooltipViewModel(() => <>4__this.GetMoraleHintStr(isSimulation));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005820 File Offset: 0x00003A20
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Score.RefreshValues();
			this.Parties.ApplyActionOnAllItems(delegate(SPScoreboardPartyVM x)
			{
				x.RefreshValues();
			});
			this.Ships.ApplyActionOnAllItems(delegate(SPScoreboardShipVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005894 File Offset: 0x00003A94
		private string GetMoraleHintStr(bool isSimulation)
		{
			return GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).SetTextVariable("LEFT", isSimulation ? GameTexts.FindText("str_morale", null).ToString() : new TextObject("{=trPyg7mr}Battle Morale", null).ToString()).SetTextVariable("RIGHT", MathF.Round(this.Morale).ToString())
				.ToString();
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000058FD File Offset: 0x00003AFD
		public void UpdateScores(IBattleCombatant battleCombatant, bool isPlayerParty, BasicCharacterObject character, int numberRemaining, int numberDead, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
		{
			this.GetPartyAddIfNotExists(battleCombatant, isPlayerParty).UpdateScores(character, numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			this.Score.UpdateScores(numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			this.RefreshPower();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005936 File Offset: 0x00003B36
		public void UpdateHeroSkills(IBattleCombatant battleCombatant, bool isPlayerParty, BasicCharacterObject heroCharacter, SkillObject upgradedSkill)
		{
			this.GetPartyAddIfNotExists(battleCombatant, isPlayerParty).UpdateHeroSkills(heroCharacter, upgradedSkill);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005948 File Offset: 0x00003B48
		public SPScoreboardPartyVM GetPartyAddIfNotExists(IBattleCombatant battleCombatant, bool isPlayerParty)
		{
			SPScoreboardPartyVM spscoreboardPartyVM = this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant);
			if (spscoreboardPartyVM == null)
			{
				spscoreboardPartyVM = new SPScoreboardPartyVM(battleCombatant);
				if (isPlayerParty)
				{
					this.Parties.Insert(0, spscoreboardPartyVM);
				}
				else
				{
					this.Parties.Add(spscoreboardPartyVM);
				}
			}
			return spscoreboardPartyVM;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000059A8 File Offset: 0x00003BA8
		public SPScoreboardPartyVM GetParty(IBattleCombatant battleCombatant)
		{
			return this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000059DC File Offset: 0x00003BDC
		public SPScoreboardStatsVM RemoveTroop(IBattleCombatant battleCombatant, BasicCharacterObject troop)
		{
			SPScoreboardPartyVM spscoreboardPartyVM = this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant);
			SPScoreboardStatsVM spscoreboardStatsVM = spscoreboardPartyVM.RemoveUnit(troop);
			if (spscoreboardPartyVM.Members.Count == 0)
			{
				this.Parties.Remove(spscoreboardPartyVM);
			}
			this.Score.UpdateScores(-spscoreboardStatsVM.Remaining, -spscoreboardStatsVM.Dead, -spscoreboardStatsVM.Wounded, -spscoreboardStatsVM.Routed, -spscoreboardStatsVM.Kill, -spscoreboardStatsVM.ReadyToUpgrade);
			return spscoreboardStatsVM;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005A68 File Offset: 0x00003C68
		public void AddTroop(IBattleCombatant battleCombatant, BasicCharacterObject currentTroop, SPScoreboardStatsVM scoreToBringOver)
		{
			this.Parties.FirstOrDefault<SPScoreboardPartyVM>((SPScoreboardPartyVM p) => p.BattleCombatant == battleCombatant).AddUnit(currentTroop, scoreToBringOver);
			this.Score.UpdateScores(scoreToBringOver.Remaining, scoreToBringOver.Dead, scoreToBringOver.Wounded, scoreToBringOver.Routed, scoreToBringOver.Kill, scoreToBringOver.ReadyToUpgrade);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005AD0 File Offset: 0x00003CD0
		public SPScoreboardShipVM GetShipAddIfNotExists(IShipOrigin ship, string shipType, IBattleCombatant owner, TeamSideEnum teamSideEnum, int formationIndex)
		{
			SPScoreboardShipVM spscoreboardShipVM = this.Ships.FirstOrDefault<SPScoreboardShipVM>((SPScoreboardShipVM p) => p.Ship == ship);
			if (spscoreboardShipVM == null)
			{
				spscoreboardShipVM = new SPScoreboardShipVM(ship, shipType, owner, teamSideEnum, formationIndex);
				this.Ships.Add(spscoreboardShipVM);
			}
			spscoreboardShipVM.FormationIndex = formationIndex;
			return spscoreboardShipVM;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005B2C File Offset: 0x00003D2C
		private void RefreshPower()
		{
			this.CurrentPower = 0f;
			this.InitialPower = 0f;
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in this._parties)
			{
				this.InitialPower += spscoreboardPartyVM.InitialPower;
				this.CurrentPower += spscoreboardPartyVM.CurrentPower;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005BB0 File Offset: 0x00003DB0
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005BB8 File Offset: 0x00003DB8
		public float CurrentPower { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005BC1 File Offset: 0x00003DC1
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00005BC9 File Offset: 0x00003DC9
		public float InitialPower { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005BD2 File Offset: 0x00003DD2
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00005BDA File Offset: 0x00003DDA
		[DataSourceProperty]
		public BannerImageIdentifierVM BannerVisual
		{
			get
			{
				return this._bannerVisual;
			}
			set
			{
				if (value != this._bannerVisual)
				{
					this._bannerVisual = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "BannerVisual");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00005BF8 File Offset: 0x00003DF8
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00005C00 File Offset: 0x00003E00
		[DataSourceProperty]
		public BannerImageIdentifierVM BannerVisualSmall
		{
			get
			{
				return this._bannerVisualSmall;
			}
			set
			{
				if (value != this._bannerVisualSmall)
				{
					this._bannerVisualSmall = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "BannerVisualSmall");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00005C1E File Offset: 0x00003E1E
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00005C26 File Offset: 0x00003E26
		[DataSourceProperty]
		public SPScoreboardStatsVM Score
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
					base.OnPropertyChangedWithValue<SPScoreboardStatsVM>(value, "Score");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00005C44 File Offset: 0x00003E44
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00005C4C File Offset: 0x00003E4C
		[DataSourceProperty]
		public MBBindingList<SPScoreboardPartyVM> Parties
		{
			get
			{
				return this._parties;
			}
			set
			{
				if (value != this._parties)
				{
					this._parties = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardPartyVM>>(value, "Parties");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00005C6A File Offset: 0x00003E6A
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00005C72 File Offset: 0x00003E72
		[DataSourceProperty]
		public MBBindingList<SPScoreboardShipVM> Ships
		{
			get
			{
				return this._ships;
			}
			set
			{
				if (value != this._ships)
				{
					this._ships = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardShipVM>>(value, "Ships");
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00005C90 File Offset: 0x00003E90
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00005C98 File Offset: 0x00003E98
		[DataSourceProperty]
		public SPScoreboardSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChanged("SortController");
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00005CB5 File Offset: 0x00003EB5
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00005CBD File Offset: 0x00003EBD
		[DataSourceProperty]
		public float Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				if (value != this._morale)
				{
					this._morale = value;
					base.OnPropertyChangedWithValue(value, "Morale");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00005CDB File Offset: 0x00003EDB
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00005CE3 File Offset: 0x00003EE3
		[DataSourceProperty]
		public BasicTooltipViewModel MoraleHint
		{
			get
			{
				return this._moraleHint;
			}
			set
			{
				if (value != this._moraleHint)
				{
					this._moraleHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "MoraleHint");
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00005D01 File Offset: 0x00003F01
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00005D09 File Offset: 0x00003F09
		[DataSourceProperty]
		public bool IsPlayerSide
		{
			get
			{
				return this._isPlayerSide;
			}
			set
			{
				if (value != this._isPlayerSide)
				{
					this._isPlayerSide = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerSide");
				}
			}
		}

		// Token: 0x040000A1 RID: 161
		private MBBindingList<SPScoreboardPartyVM> _parties;

		// Token: 0x040000A2 RID: 162
		private MBBindingList<SPScoreboardShipVM> _ships;

		// Token: 0x040000A3 RID: 163
		private SPScoreboardStatsVM _score;

		// Token: 0x040000A4 RID: 164
		private BannerImageIdentifierVM _bannerVisual;

		// Token: 0x040000A5 RID: 165
		private BannerImageIdentifierVM _bannerVisualSmall;

		// Token: 0x040000A6 RID: 166
		private SPScoreboardSortControllerVM _sortController;

		// Token: 0x040000A7 RID: 167
		private float _morale;

		// Token: 0x040000A8 RID: 168
		private BasicTooltipViewModel _moraleHint;

		// Token: 0x040000A9 RID: 169
		private bool _isPlayerSide;
	}
}
