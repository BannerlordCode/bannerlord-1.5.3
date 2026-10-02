using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection
{
	// Token: 0x0200000D RID: 13
	public class PowerLevelComparer : ViewModel
	{
		// Token: 0x0600009D RID: 157 RVA: 0x000030D8 File Offset: 0x000012D8
		public PowerLevelComparer(double defenderPower, double attackerPower)
		{
			this._totalStrength = defenderPower + attackerPower;
			this._totalInitialStrength = this._totalStrength;
			this.InitialDefenderBattlePowerValue = defenderPower;
			this.InitialAttackerBattlePowerValue = attackerPower;
			this.InitialDefenderBattlePower = defenderPower / this._totalStrength;
			this.InitialAttackerBattlePower = attackerPower / this._totalStrength;
			this.Update(defenderPower, attackerPower);
			this.Hint = new HintViewModel(GameTexts.FindText("str_power_levels", null), null);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003166 File Offset: 0x00001366
		public void SetColors(string defenderColor, string attackerColor)
		{
			this.DefenderColor = defenderColor;
			this.AttackerColor = attackerColor;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003176 File Offset: 0x00001376
		public void Update(double defenderPower, double attackerPower)
		{
			this.Update(defenderPower, attackerPower, this.InitialDefenderBattlePowerValue, this.InitialAttackerBattlePowerValue);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000318C File Offset: 0x0000138C
		public void Update(double defenderPower, double attackerPower, double initialDefenderPower, double initialAttackerPower)
		{
			this._totalStrength = defenderPower + attackerPower;
			this._totalInitialStrength = initialDefenderPower + initialAttackerPower;
			this.InitialDefenderBattlePower = initialDefenderPower / (initialDefenderPower + initialAttackerPower);
			this.InitialAttackerBattlePower = initialAttackerPower / (initialDefenderPower + initialAttackerPower);
			this.InitialDefenderBattlePowerValue = initialDefenderPower;
			this.InitialAttackerBattlePowerValue = initialAttackerPower;
			this.DefenderBattlePower = defenderPower / this._totalStrength;
			this.AttackerBattlePower = attackerPower / this._totalStrength;
			this.DefenderBattlePowerValue = defenderPower;
			this.AttackerBattlePowerValue = attackerPower;
			this.DefenderRelativePower = ((initialDefenderPower == 0.0) ? 0f : ((float)(defenderPower / initialDefenderPower)));
			this.AttackerRelativePower = ((initialAttackerPower == 0.0) ? 0f : ((float)(attackerPower / initialAttackerPower)));
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000323A File Offset: 0x0000143A
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003242 File Offset: 0x00001442
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003260 File Offset: 0x00001460
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003268 File Offset: 0x00001468
		[DataSourceProperty]
		public double DefenderBattlePower
		{
			get
			{
				return this._defenderBattlePower;
			}
			set
			{
				if (value != this._defenderBattlePower)
				{
					this._defenderBattlePower = value;
					base.OnPropertyChangedWithValue(value, "DefenderBattlePower");
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003286 File Offset: 0x00001486
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000328E File Offset: 0x0000148E
		[DataSourceProperty]
		public double DefenderBattlePowerValue
		{
			get
			{
				return this._defenderBattlePowerValue;
			}
			set
			{
				if (value != this._defenderBattlePowerValue)
				{
					this._defenderBattlePowerValue = value;
					base.OnPropertyChangedWithValue(value, "DefenderBattlePowerValue");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000032AC File Offset: 0x000014AC
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000032B4 File Offset: 0x000014B4
		[DataSourceProperty]
		public double AttackerBattlePower
		{
			get
			{
				return this._attackerBattlePower;
			}
			set
			{
				if (value != this._attackerBattlePower)
				{
					this._attackerBattlePower = value;
					base.OnPropertyChangedWithValue(value, "AttackerBattlePower");
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000032D2 File Offset: 0x000014D2
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000032DA File Offset: 0x000014DA
		[DataSourceProperty]
		public double AttackerBattlePowerValue
		{
			get
			{
				return this._attackerBattlePowerValue;
			}
			set
			{
				if (value != this._attackerBattlePowerValue)
				{
					this._attackerBattlePowerValue = value;
					base.OnPropertyChangedWithValue(value, "AttackerBattlePowerValue");
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000032F8 File Offset: 0x000014F8
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003300 File Offset: 0x00001500
		[DataSourceProperty]
		public double InitialDefenderBattlePower
		{
			get
			{
				return this._initialDefenderBattlePower;
			}
			set
			{
				if (value != this._initialDefenderBattlePower)
				{
					this._initialDefenderBattlePower = value;
					base.OnPropertyChangedWithValue(value, "InitialDefenderBattlePower");
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000331E File Offset: 0x0000151E
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003326 File Offset: 0x00001526
		[DataSourceProperty]
		public double InitialAttackerBattlePower
		{
			get
			{
				return this._initialAttackerBattlePower;
			}
			set
			{
				if (value != this._initialAttackerBattlePower)
				{
					this._initialAttackerBattlePower = value;
					base.OnPropertyChangedWithValue(value, "InitialAttackerBattlePower");
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003344 File Offset: 0x00001544
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000334C File Offset: 0x0000154C
		[DataSourceProperty]
		public double InitialDefenderBattlePowerValue
		{
			get
			{
				return this._initialDefenderBattlePowerValue;
			}
			set
			{
				if (value != this._initialDefenderBattlePowerValue)
				{
					this._initialDefenderBattlePowerValue = value;
					base.OnPropertyChangedWithValue(value, "InitialDefenderBattlePowerValue");
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000336A File Offset: 0x0000156A
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003372 File Offset: 0x00001572
		[DataSourceProperty]
		public double InitialAttackerBattlePowerValue
		{
			get
			{
				return this._initialAttackerBattlePowerValue;
			}
			set
			{
				if (value != this._initialAttackerBattlePowerValue)
				{
					this._initialAttackerBattlePowerValue = value;
					base.OnPropertyChangedWithValue(value, "InitialAttackerBattlePowerValue");
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003390 File Offset: 0x00001590
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003398 File Offset: 0x00001598
		[DataSourceProperty]
		public float DefenderRelativePower
		{
			get
			{
				return this._defenderRelativePower;
			}
			set
			{
				if (value != this._defenderRelativePower)
				{
					this._defenderRelativePower = value;
					base.OnPropertyChangedWithValue(value, "DefenderRelativePower");
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000033B6 File Offset: 0x000015B6
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000033BE File Offset: 0x000015BE
		[DataSourceProperty]
		public float AttackerRelativePower
		{
			get
			{
				return this._attackerRelativePower;
			}
			set
			{
				if (value != this._attackerRelativePower)
				{
					this._attackerRelativePower = value;
					base.OnPropertyChangedWithValue(value, "AttackerRelativePower");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000033DC File Offset: 0x000015DC
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000033E4 File Offset: 0x000015E4
		[DataSourceProperty]
		public string DefenderColor
		{
			get
			{
				return this._defenderColor;
			}
			set
			{
				if (value != this._defenderColor)
				{
					this._defenderColor = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderColor");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003407 File Offset: 0x00001607
		// (set) Token: 0x060000BA RID: 186 RVA: 0x0000340F File Offset: 0x0000160F
		[DataSourceProperty]
		public string AttackerColor
		{
			get
			{
				return this._attackerColor;
			}
			set
			{
				if (value != this._attackerColor)
				{
					this._attackerColor = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerColor");
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003432 File Offset: 0x00001632
		// (set) Token: 0x060000BC RID: 188 RVA: 0x0000343A File Offset: 0x0000163A
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x04000043 RID: 67
		private double _totalStrength;

		// Token: 0x04000044 RID: 68
		private double _totalInitialStrength;

		// Token: 0x04000045 RID: 69
		private double _defenderBattlePower;

		// Token: 0x04000046 RID: 70
		private double _attackerBattlePower;

		// Token: 0x04000047 RID: 71
		private double _defenderBattlePowerValue;

		// Token: 0x04000048 RID: 72
		private double _attackerBattlePowerValue;

		// Token: 0x04000049 RID: 73
		private double _initialDefenderBattlePower;

		// Token: 0x0400004A RID: 74
		private double _initialAttackerBattlePower;

		// Token: 0x0400004B RID: 75
		private double _initialDefenderBattlePowerValue;

		// Token: 0x0400004C RID: 76
		private double _initialAttackerBattlePowerValue;

		// Token: 0x0400004D RID: 77
		private float _defenderRelativePower;

		// Token: 0x0400004E RID: 78
		private float _attackerRelativePower;

		// Token: 0x0400004F RID: 79
		private string _defenderColor = "#5E8C23FF";

		// Token: 0x04000050 RID: 80
		private string _attackerColor = "#A0341EFF";

		// Token: 0x04000051 RID: 81
		private bool _isEnabled = true;

		// Token: 0x04000052 RID: 82
		private HintViewModel _hint;
	}
}
