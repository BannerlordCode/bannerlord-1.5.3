using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000028 RID: 40
	public class PartyCompositionVM : ViewModel
	{
		// Token: 0x0600032C RID: 812 RVA: 0x000165F0 File Offset: 0x000147F0
		public PartyCompositionVM()
		{
			this.InfantryHint = new HintViewModel(new TextObject("{=1Bm1Wk1v}Infantry", null), null);
			this.RangedHint = new HintViewModel(new TextObject("{=bIiBytSB}Archers", null), null);
			this.CavalryHint = new HintViewModel(new TextObject("{=YVGtcLHF}Cavalry", null), null);
			this.HorseArcherHint = new HintViewModel(new TextObject("{=I1CMeL9R}Mounted Archers", null), null);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00016660 File Offset: 0x00014860
		public void OnTroopRemoved(FormationClass formationClass, int count)
		{
			if (this.IsInfantry(formationClass))
			{
				this.InfantryCount -= count;
			}
			if (this.IsRanged(formationClass))
			{
				this.RangedCount -= count;
			}
			if (this.IsCavalry(formationClass))
			{
				this.CavalryCount -= count;
			}
			if (this.IsHorseArcher(formationClass))
			{
				this.HorseArcherCount -= count;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000166CC File Offset: 0x000148CC
		public void OnTroopAdded(FormationClass formationClass, int count)
		{
			if (this.IsInfantry(formationClass))
			{
				this.InfantryCount += count;
			}
			if (this.IsRanged(formationClass))
			{
				this.RangedCount += count;
			}
			if (this.IsCavalry(formationClass))
			{
				this.CavalryCount += count;
			}
			if (this.IsHorseArcher(formationClass))
			{
				this.HorseArcherCount += count;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00016738 File Offset: 0x00014938
		public void RefreshCounts(MBBindingList<PartyCharacterVM> list)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				TroopRosterElement troop = list[i].Troop;
				FormationClass defaultFormationClass = list[i].Troop.Character.DefaultFormationClass;
				if (this.IsInfantry(defaultFormationClass))
				{
					num += troop.Number;
				}
				if (this.IsRanged(defaultFormationClass))
				{
					num2 += troop.Number;
				}
				if (this.IsCavalry(defaultFormationClass))
				{
					num3 += troop.Number;
				}
				if (this.IsHorseArcher(defaultFormationClass))
				{
					num4 += troop.Number;
				}
			}
			this.InfantryCount = num;
			this.RangedCount = num2;
			this.CavalryCount = num3;
			this.HorseArcherCount = num4;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000167F9 File Offset: 0x000149F9
		private bool IsInfantry(FormationClass formationClass)
		{
			return formationClass == FormationClass.Infantry || formationClass == FormationClass.HeavyInfantry || formationClass == FormationClass.NumberOfDefaultFormations;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00016808 File Offset: 0x00014A08
		private bool IsRanged(FormationClass formationClass)
		{
			return formationClass == FormationClass.Ranged;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001680E File Offset: 0x00014A0E
		private bool IsCavalry(FormationClass formationClass)
		{
			return formationClass == FormationClass.Cavalry || formationClass == FormationClass.LightCavalry || formationClass == FormationClass.HeavyCavalry;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001681E File Offset: 0x00014A1E
		private bool IsHorseArcher(FormationClass formationClass)
		{
			return formationClass == FormationClass.HorseArcher;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00016824 File Offset: 0x00014A24
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0001682C File Offset: 0x00014A2C
		[DataSourceProperty]
		public int InfantryCount
		{
			get
			{
				return this._infantryCount;
			}
			set
			{
				if (value != this._infantryCount)
				{
					this._infantryCount = value;
					base.OnPropertyChangedWithValue(value, "InfantryCount");
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0001684A File Offset: 0x00014A4A
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00016852 File Offset: 0x00014A52
		[DataSourceProperty]
		public int RangedCount
		{
			get
			{
				return this._rangedCount;
			}
			set
			{
				if (value != this._rangedCount)
				{
					this._rangedCount = value;
					base.OnPropertyChangedWithValue(value, "RangedCount");
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00016870 File Offset: 0x00014A70
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00016878 File Offset: 0x00014A78
		[DataSourceProperty]
		public int CavalryCount
		{
			get
			{
				return this._cavalryCount;
			}
			set
			{
				if (value != this._cavalryCount)
				{
					this._cavalryCount = value;
					base.OnPropertyChangedWithValue(value, "CavalryCount");
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00016896 File Offset: 0x00014A96
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0001689E File Offset: 0x00014A9E
		[DataSourceProperty]
		public int HorseArcherCount
		{
			get
			{
				return this._horseArcherCount;
			}
			set
			{
				if (value != this._horseArcherCount)
				{
					this._horseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "HorseArcherCount");
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000168BC File Offset: 0x00014ABC
		// (set) Token: 0x0600033D RID: 829 RVA: 0x000168C4 File Offset: 0x00014AC4
		[DataSourceProperty]
		public HintViewModel InfantryHint
		{
			get
			{
				return this._infantryHint;
			}
			set
			{
				if (value != this._infantryHint)
				{
					this._infantryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InfantryHint");
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600033E RID: 830 RVA: 0x000168E2 File Offset: 0x00014AE2
		// (set) Token: 0x0600033F RID: 831 RVA: 0x000168EA File Offset: 0x00014AEA
		[DataSourceProperty]
		public HintViewModel RangedHint
		{
			get
			{
				return this._rangedHint;
			}
			set
			{
				if (value != this._rangedHint)
				{
					this._rangedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RangedHint");
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00016908 File Offset: 0x00014B08
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00016910 File Offset: 0x00014B10
		[DataSourceProperty]
		public HintViewModel CavalryHint
		{
			get
			{
				return this._cavalryHint;
			}
			set
			{
				if (value != this._cavalryHint)
				{
					this._cavalryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CavalryHint");
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0001692E File Offset: 0x00014B2E
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00016936 File Offset: 0x00014B36
		[DataSourceProperty]
		public HintViewModel HorseArcherHint
		{
			get
			{
				return this._horseArcherHint;
			}
			set
			{
				if (value != this._horseArcherHint)
				{
					this._horseArcherHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HorseArcherHint");
				}
			}
		}

		// Token: 0x0400016C RID: 364
		private int _infantryCount;

		// Token: 0x0400016D RID: 365
		private int _rangedCount;

		// Token: 0x0400016E RID: 366
		private int _cavalryCount;

		// Token: 0x0400016F RID: 367
		private int _horseArcherCount;

		// Token: 0x04000170 RID: 368
		private HintViewModel _infantryHint;

		// Token: 0x04000171 RID: 369
		private HintViewModel _rangedHint;

		// Token: 0x04000172 RID: 370
		private HintViewModel _cavalryHint;

		// Token: 0x04000173 RID: 371
		private HintViewModel _horseArcherHint;
	}
}
