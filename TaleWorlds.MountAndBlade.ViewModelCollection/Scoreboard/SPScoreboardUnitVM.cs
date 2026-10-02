using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000016 RID: 22
	public class SPScoreboardUnitVM : ViewModel
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x000067A4 File Offset: 0x000049A4
		public SPScoreboardUnitVM(BasicCharacterObject character)
		{
			this.Character = character;
			this.GainedSkills = new MBBindingList<SPScoreboardSkillItemVM>();
			this._skills = new List<SPScoreboardSkillItemVM>();
			this.Score = new SPScoreboardStatsVM(character.Name);
			CharacterCode.CreateFrom(character);
			this.IsHero = character.IsHero;
			this.Score.IsMainHero = character == Game.Current.PlayerTroop;
			this.IsGainedAnySkills = false;
			if (character.IsHero)
			{
				foreach (SkillObject skillObject in Game.Current.ObjectManager.GetObjectTypeList<SkillObject>())
				{
					this._skills.Add(new SPScoreboardSkillItemVM(skillObject, character.GetSkillValue(skillObject)));
				}
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006880 File Offset: 0x00004A80
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Score.RefreshValues();
			this.GainedSkills.ApplyActionOnAllItems(delegate(SPScoreboardSkillItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000068BD File Offset: 0x00004ABD
		private void ExecuteActivateGainedSkills()
		{
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000068BF File Offset: 0x00004ABF
		private void ExecuteDeactivateGainedSkills()
		{
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000068C1 File Offset: 0x00004AC1
		public void UpdateScores(int numberRemaining, int numberDead, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
		{
			this.Score.UpdateScores(numberRemaining, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000068D8 File Offset: 0x00004AD8
		public void UpdateHeroSkills(SkillObject gainedSkill, int currentSkill)
		{
			SPScoreboardSkillItemVM spscoreboardSkillItemVM = this._skills.First<SPScoreboardSkillItemVM>((SPScoreboardSkillItemVM s) => s.Skill == gainedSkill);
			spscoreboardSkillItemVM.UpdateSkill(currentSkill);
			if (!this.GainedSkills.Contains(spscoreboardSkillItemVM))
			{
				this.GainedSkills.Add(spscoreboardSkillItemVM);
			}
			this.IsGainedAnySkills = this.GainedSkills.Count > 0;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000693F File Offset: 0x00004B3F
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00006947 File Offset: 0x00004B47
		[DataSourceProperty]
		public bool IsGainedAnySkills
		{
			get
			{
				return this._isGainedAnySkills;
			}
			set
			{
				if (value != this._isGainedAnySkills)
				{
					this._isGainedAnySkills = value;
					base.OnPropertyChangedWithValue(value, "IsGainedAnySkills");
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00006965 File Offset: 0x00004B65
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000696D File Offset: 0x00004B6D
		[DataSourceProperty]
		public MBBindingList<SPScoreboardSkillItemVM> GainedSkills
		{
			get
			{
				return this._gainedSkills;
			}
			set
			{
				if (value != this._gainedSkills)
				{
					this._gainedSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<SPScoreboardSkillItemVM>>(value, "GainedSkills");
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000698B File Offset: 0x00004B8B
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00006993 File Offset: 0x00004B93
		[DataSourceProperty]
		public bool IsHero
		{
			get
			{
				return this._isHero;
			}
			set
			{
				if (value != this._isHero)
				{
					this._isHero = value;
					base.OnPropertyChangedWithValue(value, "IsHero");
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000069B1 File Offset: 0x00004BB1
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x000069B9 File Offset: 0x00004BB9
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

		// Token: 0x040000CE RID: 206
		public readonly BasicCharacterObject Character;

		// Token: 0x040000CF RID: 207
		private readonly List<SPScoreboardSkillItemVM> _skills;

		// Token: 0x040000D0 RID: 208
		private SPScoreboardStatsVM _score;

		// Token: 0x040000D1 RID: 209
		private bool _isHero;

		// Token: 0x040000D2 RID: 210
		private bool _isGainedAnySkills;

		// Token: 0x040000D3 RID: 211
		private MBBindingList<SPScoreboardSkillItemVM> _gainedSkills;
	}
}
