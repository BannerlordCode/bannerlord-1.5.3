using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000010 RID: 16
	public class SPScoreboardPartyVM : ViewModel
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005100 File Offset: 0x00003300
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005108 File Offset: 0x00003308
		public IBattleCombatant BattleCombatant { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005111 File Offset: 0x00003311
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00005119 File Offset: 0x00003319
		public float CurrentPower { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00005122 File Offset: 0x00003322
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000512A File Offset: 0x0000332A
		public float InitialPower { get; private set; }

		// Token: 0x0600012F RID: 303 RVA: 0x00005133 File Offset: 0x00003333
		public SPScoreboardPartyVM(IBattleCombatant battleCombatant)
		{
			this.BattleCombatant = battleCombatant;
			this.Members = new MBBindingList<SPScoreboardUnitVM>();
			this.Score = new SPScoreboardStatsVM((battleCombatant != null) ? battleCombatant.Name : new TextObject("{=qnxJYAs7}Party", null));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000516E File Offset: 0x0000336E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Score.RefreshValues();
			this.Members.ApplyActionOnAllItems(delegate(SPScoreboardUnitVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000051AB File Offset: 0x000033AB
		public void UpdateScores(BasicCharacterObject character, int numberRemaining, int numberDead, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
		{
			this.GetUnitAddIfNotExists(character).UpdateScores(numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			SPScoreboardStatsVM score = this.Score;
			if (score != null)
			{
				score.UpdateScores(numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			}
			this.RefreshPower();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000051E4 File Offset: 0x000033E4
		public void UpdateHeroSkills(BasicCharacterObject heroCharacter, SkillObject upgradedSkill)
		{
			this.GetUnitAddIfNotExists(heroCharacter).UpdateHeroSkills(upgradedSkill, heroCharacter.GetSkillValue(upgradedSkill));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000051FC File Offset: 0x000033FC
		public SPScoreboardUnitVM GetUnitAddIfNotExists(BasicCharacterObject character)
		{
			if (character == Game.Current.PlayerTroop)
			{
				this.Score.IsMainParty = true;
			}
			SPScoreboardUnitVM spscoreboardUnitVM = this.Members.FirstOrDefault<SPScoreboardUnitVM>((SPScoreboardUnitVM p) => p.Character == character);
			if (spscoreboardUnitVM == null)
			{
				spscoreboardUnitVM = new SPScoreboardUnitVM(character);
				this.Members.Add(spscoreboardUnitVM);
				this.Members.Sort(new SPScoreboardSortControllerVM.ItemMemberComparer());
			}
			return spscoreboardUnitVM;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005278 File Offset: 0x00003478
		public SPScoreboardUnitVM GetUnit(BasicCharacterObject character)
		{
			return this.Members.FirstOrDefault<SPScoreboardUnitVM>((SPScoreboardUnitVM p) => p.Character == character);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000052AC File Offset: 0x000034AC
		internal SPScoreboardStatsVM RemoveUnit(BasicCharacterObject troop)
		{
			SPScoreboardUnitVM spscoreboardUnitVM = this.Members.FirstOrDefault<SPScoreboardUnitVM>((SPScoreboardUnitVM m) => m.Character == troop);
			SPScoreboardStatsVM spscoreboardStatsVM = null;
			if (spscoreboardUnitVM != null)
			{
				spscoreboardStatsVM = spscoreboardUnitVM.Score.GetScoreForOneAliveMember();
				this.UpdateScores(troop, -spscoreboardStatsVM.Remaining, -spscoreboardStatsVM.Dead, -spscoreboardStatsVM.Wounded, -spscoreboardStatsVM.Routed, -spscoreboardStatsVM.Kill, -spscoreboardStatsVM.ReadyToUpgrade);
				if (!spscoreboardUnitVM.Score.IsAnyStatRelevant())
				{
					this.Members.Remove(spscoreboardUnitVM);
					if (troop == Game.Current.PlayerTroop)
					{
						this.Score.IsMainParty = false;
					}
				}
			}
			return spscoreboardStatsVM;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005360 File Offset: 0x00003560
		internal void AddUnit(BasicCharacterObject unit, SPScoreboardStatsVM scoreToBringOver)
		{
			this.Score.UpdateScores(scoreToBringOver.Remaining, scoreToBringOver.Dead, scoreToBringOver.Wounded, scoreToBringOver.Routed, scoreToBringOver.Kill, scoreToBringOver.ReadyToUpgrade);
			SPScoreboardUnitVM spscoreboardUnitVM = this.Members.FirstOrDefault<SPScoreboardUnitVM>((SPScoreboardUnitVM m) => m.Character == unit);
			if (spscoreboardUnitVM == null)
			{
				spscoreboardUnitVM = new SPScoreboardUnitVM(unit);
				this.Members.Add(spscoreboardUnitVM);
				if (unit == Game.Current.PlayerTroop)
				{
					this.Score.IsMainParty = true;
				}
			}
			spscoreboardUnitVM.Score.UpdateScores(scoreToBringOver.Remaining, scoreToBringOver.Dead, scoreToBringOver.Wounded, scoreToBringOver.Routed, scoreToBringOver.Kill, scoreToBringOver.ReadyToUpgrade);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000542C File Offset: 0x0000362C
		private void RefreshPower()
		{
			this.CurrentPower = 0f;
			this.InitialPower = 0f;
			foreach (SPScoreboardUnitVM spscoreboardUnitVM in this.Members)
			{
				this.CurrentPower += (float)spscoreboardUnitVM.Score.Remaining * spscoreboardUnitVM.Character.GetPower();
				this.InitialPower += (float)(spscoreboardUnitVM.Score.Dead + spscoreboardUnitVM.Score.Routed + spscoreboardUnitVM.Score.Wounded + spscoreboardUnitVM.Score.Remaining) * spscoreboardUnitVM.Character.GetPower();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000054F8 File Offset: 0x000036F8
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00005500 File Offset: 0x00003700
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

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000551E File Offset: 0x0000371E
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00005526 File Offset: 0x00003726
		[DataSourceProperty]
		public MBBindingList<SPScoreboardUnitVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardUnitVM>>(value, "Members");
				}
			}
		}

		// Token: 0x0400008F RID: 143
		private MBBindingList<SPScoreboardUnitVM> _members;

		// Token: 0x04000090 RID: 144
		private SPScoreboardStatsVM _score;
	}
}
