using System;
using Helpers;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000078 RID: 120
	public class KingdomWarItemVM : KingdomDiplomacyItemVM
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x0002A2FC File Offset: 0x000284FC
		public KingdomWarItemVM(StanceLink war, Action<KingdomWarItemVM> onSelect)
			: base(war.Faction1, war.Faction2)
		{
			this._war = war;
			this._onSelect = onSelect;
			this.IsBehaviorSelectionEnabled = this.Faction1.IsKingdomFaction && this.Faction1.Leader == Hero.MainHero;
			StanceLink stanceWith = this.Faction1.GetStanceWith(this.Faction2);
			this._warProgressOfFaction1 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(this.Faction1, this.Faction2, true);
			this._warProgressOfFaction2 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(this.Faction2, this.Faction1, true);
			this._numberOfTownsCapturedByFaction1 = stanceWith.GetSuccessfulTownSieges(this.Faction1);
			this._numberOfTownsCapturedByFaction2 = stanceWith.GetSuccessfulTownSieges(this.Faction2);
			this._numberOfCastlesCapturedByFaction1 = stanceWith.GetSuccessfulSieges(this.Faction1) - this._numberOfTownsCapturedByFaction1;
			this._numberOfCastlesCapturedByFaction2 = stanceWith.GetSuccessfulSieges(this.Faction2) - this._numberOfTownsCapturedByFaction2;
			this._numberOfRaidsMadeByFaction1 = stanceWith.GetSuccessfulRaids(this.Faction1);
			this._numberOfRaidsMadeByFaction2 = stanceWith.GetSuccessfulRaids(this.Faction2);
			this.RefreshValues();
			this.WarLog = new MBBindingList<KingdomWarLogItemVM>();
			foreach (ValueTuple<LogEntry, IFaction, IFaction> valueTuple in DiplomacyHelper.GetLogsForWar(war))
			{
				LogEntry item = valueTuple.Item1;
				IFaction item2 = valueTuple.Item2;
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = item as IEncyclopediaLog) != null)
				{
					this.WarLog.Add(new KingdomWarLogItemVM(encyclopediaLog, item2));
				}
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0002A4A8 File Offset: 0x000286A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateDiplomacyProperties();
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002A4B6 File Offset: 0x000286B6
		protected override void OnSelect()
		{
			if (base.IsSelected)
			{
				return;
			}
			this.UpdateDiplomacyProperties();
			this._onSelect(this);
			base.IsSelected = true;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0002A4DC File Offset: 0x000286DC
		protected override void UpdateDiplomacyProperties()
		{
			base.UpdateDiplomacyProperties();
			GameTexts.SetVariable("FACTION_1_NAME", this.Faction1.Name.ToString());
			GameTexts.SetVariable("FACTION_2_NAME", this.Faction2.Name.ToString());
			this.WarName = GameTexts.FindText("str_war_faction_versus_faction", null).ToString();
			StanceLink stanceWith = this.Faction1.GetStanceWith(this.Faction2);
			this.Score = stanceWith.GetSuccessfulSieges(this.Faction1) + stanceWith.GetSuccessfulRaids(this.Faction1);
			this.CasualtiesOfFaction1 = stanceWith.GetCasualties(this.Faction1);
			this.CasualtiesOfFaction2 = stanceWith.GetCasualties(this.Faction2);
			int num = MathF.Ceiling(this._war.WarStartDate.ElapsedDaysUntilNow + 0.01f);
			TextObject textObject = GameTexts.FindText("str_for_DAY_days", null);
			textObject.SetTextVariable("DAY", num.ToString());
			textObject.SetTextVariable("DAY_IS_PLURAL", (num > 1) ? 1 : 0);
			this.NumberOfDaysSinceWarBegan = textObject.ToString();
			base.Stats.Add(new KingdomWarComparableStatVM((int)this.Faction1.CurrentTotalStrength, (int)this.Faction2.CurrentTotalStrength, GameTexts.FindText("str_total_strength", null), this._faction1Color, this._faction2Color, 10000, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(stanceWith.GetCasualties(this.Faction2), stanceWith.GetCasualties(this.Faction1), GameTexts.FindText("str_war_casualties_inflicted", null), this._faction1Color, this._faction2Color, 10000, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._numberOfTownsCapturedByFaction1, this._numberOfTownsCapturedByFaction2, GameTexts.FindText("str_war_captured_towns", null), this._faction1Color, this._faction2Color, 25, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._numberOfCastlesCapturedByFaction1, this._numberOfCastlesCapturedByFaction2, GameTexts.FindText("str_war_captured_castles", null), this._faction1Color, this._faction2Color, 25, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._numberOfRaidsMadeByFaction1, this._numberOfRaidsMadeByFaction2, GameTexts.FindText("str_war_successful_raids", null), this._faction1Color, this._faction2Color, 10, null, null));
			int num2 = (int)(this._warProgressOfFaction1.ResultNumber * 100f / this._warProgressOfFaction1.LimitMaxValue);
			int num3 = (int)(this._warProgressOfFaction2.ResultNumber * 100f / this._warProgressOfFaction2.LimitMaxValue);
			int num4 = MathF.Max(0, num2 - num3);
			int num5 = MathF.Max(0, num3 - num2);
			base.Stats.Add(new KingdomWarComparableStatVM(num4, num5, new TextObject("{=8qbkS5D2}War Progress", null), this._faction1Color, this._faction2Color, 100, new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(this._warProgressOfFaction1, this._warProgressOfFaction2, this._warProgressOfFaction1.LimitMaxValue, this.Faction1.Name, this.Faction2.Name)), new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(this._warProgressOfFaction2, this._warProgressOfFaction1, this._warProgressOfFaction2.LimitMaxValue, this.Faction2.Name, this.Faction1.Name))));
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0002A7C4 File Offset: 0x000289C4
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0002A7CC File Offset: 0x000289CC
		[DataSourceProperty]
		public string WarName
		{
			get
			{
				return this._warName;
			}
			set
			{
				if (value != this._warName)
				{
					this._warName = value;
					base.OnPropertyChangedWithValue<string>(value, "WarName");
				}
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0002A7EF File Offset: 0x000289EF
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x0002A7F7 File Offset: 0x000289F7
		[DataSourceProperty]
		public string NumberOfDaysSinceWarBegan
		{
			get
			{
				return this._numberOfDaysSinceWarBegan;
			}
			set
			{
				if (value != this._numberOfDaysSinceWarBegan)
				{
					this._numberOfDaysSinceWarBegan = value;
					base.OnPropertyChangedWithValue<string>(value, "NumberOfDaysSinceWarBegan");
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0002A81A File Offset: 0x00028A1A
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0002A822 File Offset: 0x00028A22
		[DataSourceProperty]
		public bool IsBehaviorSelectionEnabled
		{
			get
			{
				return this._isBehaviorSelectionEnabled;
			}
			set
			{
				if (value != this._isBehaviorSelectionEnabled)
				{
					this._isBehaviorSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBehaviorSelectionEnabled");
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0002A840 File Offset: 0x00028A40
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0002A848 File Offset: 0x00028A48
		[DataSourceProperty]
		public int Score
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
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0002A866 File Offset: 0x00028A66
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0002A86E File Offset: 0x00028A6E
		[DataSourceProperty]
		public int CasualtiesOfFaction1
		{
			get
			{
				return this._casualtiesOfFaction1;
			}
			set
			{
				if (value != this._casualtiesOfFaction1)
				{
					this._casualtiesOfFaction1 = value;
					base.OnPropertyChangedWithValue(value, "CasualtiesOfFaction1");
				}
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0002A88C File Offset: 0x00028A8C
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0002A894 File Offset: 0x00028A94
		[DataSourceProperty]
		public int CasualtiesOfFaction2
		{
			get
			{
				return this._casualtiesOfFaction2;
			}
			set
			{
				if (value != this._casualtiesOfFaction2)
				{
					this._casualtiesOfFaction2 = value;
					base.OnPropertyChangedWithValue(value, "CasualtiesOfFaction2");
				}
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0002A8B2 File Offset: 0x00028AB2
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0002A8BA File Offset: 0x00028ABA
		[DataSourceProperty]
		public MBBindingList<KingdomWarLogItemVM> WarLog
		{
			get
			{
				return this._warLog;
			}
			set
			{
				if (value != this._warLog)
				{
					this._warLog = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarLogItemVM>>(value, "WarLog");
				}
			}
		}

		// Token: 0x04000420 RID: 1056
		private readonly Action<KingdomWarItemVM> _onSelect;

		// Token: 0x04000421 RID: 1057
		private readonly StanceLink _war;

		// Token: 0x04000422 RID: 1058
		private ExplainedNumber _warProgressOfFaction1;

		// Token: 0x04000423 RID: 1059
		private ExplainedNumber _warProgressOfFaction2;

		// Token: 0x04000424 RID: 1060
		private int _numberOfTownsCapturedByFaction1;

		// Token: 0x04000425 RID: 1061
		private int _numberOfTownsCapturedByFaction2;

		// Token: 0x04000426 RID: 1062
		private int _numberOfCastlesCapturedByFaction1;

		// Token: 0x04000427 RID: 1063
		private int _numberOfCastlesCapturedByFaction2;

		// Token: 0x04000428 RID: 1064
		private int _numberOfRaidsMadeByFaction1;

		// Token: 0x04000429 RID: 1065
		private int _numberOfRaidsMadeByFaction2;

		// Token: 0x0400042A RID: 1066
		private string _warName;

		// Token: 0x0400042B RID: 1067
		private string _numberOfDaysSinceWarBegan;

		// Token: 0x0400042C RID: 1068
		private int _score;

		// Token: 0x0400042D RID: 1069
		private bool _isBehaviorSelectionEnabled;

		// Token: 0x0400042E RID: 1070
		private int _casualtiesOfFaction1;

		// Token: 0x0400042F RID: 1071
		private int _casualtiesOfFaction2;

		// Token: 0x04000430 RID: 1072
		private MBBindingList<KingdomWarLogItemVM> _warLog;
	}
}
