using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard
{
	// Token: 0x020000B3 RID: 179
	public class TournamentLeaderboardEntryItemVM : ViewModel
	{
		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00044B72 File Offset: 0x00042D72
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x00044B7A File Offset: 0x00042D7A
		public int Rank { get; private set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x00044B83 File Offset: 0x00042D83
		// (set) Token: 0x060010EE RID: 4334 RVA: 0x00044B8B File Offset: 0x00042D8B
		public float PrizeValue { get; private set; }

		// Token: 0x060010EF RID: 4335 RVA: 0x00044B94 File Offset: 0x00042D94
		public TournamentLeaderboardEntryItemVM(Hero hero, int victories, int placement)
		{
			this._heroObj = hero;
			this.PrizeStr = "-";
			this.Rank = placement;
			this.PlacementOnLeaderboard = placement;
			this.IsChampion = placement == 1;
			this.Victories = victories;
			float num;
			if (float.TryParse(this.PrizeStr, out num))
			{
				this.PrizeValue = num;
			}
			this.IsMainHero = hero == TaleWorlds.CampaignSystem.Hero.MainHero;
			this.Hero = new HeroVM(hero, false);
			this.ChampionRewardsHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTournamentChampionRewardsTooltip(hero, null));
			this.RefreshValues();
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00044C44 File Offset: 0x00042E44
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._heroObj.Name.ToString();
			GameTexts.SetVariable("RANK", this.Rank);
			this.RankText = GameTexts.FindText("str_leaderboard_rank", null).ToString();
			HeroVM hero = this.Hero;
			if (hero == null)
			{
				return;
			}
			hero.RefreshValues();
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00044CA3 File Offset: 0x00042EA3
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x00044CAB File Offset: 0x00042EAB
		[DataSourceProperty]
		public BasicTooltipViewModel ChampionRewardsHint
		{
			get
			{
				return this._championRewardsHint;
			}
			set
			{
				if (value != this._championRewardsHint)
				{
					this._championRewardsHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ChampionRewardsHint");
				}
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00044CC9 File Offset: 0x00042EC9
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x00044CD1 File Offset: 0x00042ED1
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00044CF4 File Offset: 0x00042EF4
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x00044CFC File Offset: 0x00042EFC
		[DataSourceProperty]
		public string RankText
		{
			get
			{
				return this._rankText;
			}
			set
			{
				if (value != this._rankText)
				{
					this._rankText = value;
					base.OnPropertyChangedWithValue<string>(value, "RankText");
				}
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00044D1F File Offset: 0x00042F1F
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x00044D27 File Offset: 0x00042F27
		[DataSourceProperty]
		public int Victories
		{
			get
			{
				return this._victories;
			}
			set
			{
				if (value != this._victories)
				{
					this._victories = value;
					base.OnPropertyChangedWithValue(value, "Victories");
				}
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x00044D45 File Offset: 0x00042F45
		// (set) Token: 0x060010FA RID: 4346 RVA: 0x00044D4D File Offset: 0x00042F4D
		[DataSourceProperty]
		public bool IsChampion
		{
			get
			{
				return this._isChampion;
			}
			set
			{
				if (value != this._isChampion)
				{
					this._isChampion = value;
					base.OnPropertyChangedWithValue(value, "IsChampion");
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x00044D6B File Offset: 0x00042F6B
		// (set) Token: 0x060010FC RID: 4348 RVA: 0x00044D73 File Offset: 0x00042F73
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x00044D91 File Offset: 0x00042F91
		// (set) Token: 0x060010FE RID: 4350 RVA: 0x00044D99 File Offset: 0x00042F99
		[DataSourceProperty]
		public HeroVM Hero
		{
			get
			{
				return this._hero;
			}
			set
			{
				if (value != this._hero)
				{
					this._hero = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Hero");
				}
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x00044DB7 File Offset: 0x00042FB7
		// (set) Token: 0x06001100 RID: 4352 RVA: 0x00044DBF File Offset: 0x00042FBF
		[DataSourceProperty]
		public string PrizeStr
		{
			get
			{
				return this._prizeStr;
			}
			set
			{
				if (value != this._prizeStr)
				{
					this._prizeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "PrizeStr");
				}
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00044DE2 File Offset: 0x00042FE2
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x00044DEA File Offset: 0x00042FEA
		[DataSourceProperty]
		public int PlacementOnLeaderboard
		{
			get
			{
				return this._placementOnLeaderboard;
			}
			set
			{
				if (value != this._placementOnLeaderboard)
				{
					this._placementOnLeaderboard = value;
					base.OnPropertyChangedWithValue(value, "PlacementOnLeaderboard");
				}
			}
		}

		// Token: 0x040007AE RID: 1966
		private readonly Hero _heroObj;

		// Token: 0x040007AF RID: 1967
		private int _placementOnLeaderboard;

		// Token: 0x040007B0 RID: 1968
		private int _victories;

		// Token: 0x040007B1 RID: 1969
		private bool _isMainHero;

		// Token: 0x040007B2 RID: 1970
		private bool _isChampion;

		// Token: 0x040007B3 RID: 1971
		private string _name;

		// Token: 0x040007B4 RID: 1972
		private string _rankText;

		// Token: 0x040007B5 RID: 1973
		private string _prizeStr;

		// Token: 0x040007B6 RID: 1974
		private HeroVM _hero;

		// Token: 0x040007B7 RID: 1975
		private BasicTooltipViewModel _championRewardsHint;
	}
}
