using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EB RID: 235
	internal class OrderOfBattleFormationClassDropdownListButtonWidget : ButtonWidget
	{
		// Token: 0x06000C25 RID: 3109 RVA: 0x00021989 File Offset: 0x0001FB89
		public OrderOfBattleFormationClassDropdownListButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00021994 File Offset: 0x0001FB94
		private void SetBaseBrush()
		{
			switch (this.FormationClass)
			{
			case 0:
				base.Brush = this.UnsetBrush;
				break;
			case 1:
				base.Brush = this.InfantryBrush;
				break;
			case 2:
				base.Brush = this.RangedBrush;
				break;
			case 3:
				base.Brush = this.CavalryBrush;
				break;
			case 4:
				base.Brush = this.HorseArcherBrush;
				break;
			case 5:
				base.Brush = this.InfantryAndRangedBrush;
				break;
			case 6:
				base.Brush = this.CavalryAndHorseArcherBrush;
				break;
			default:
				base.Brush = this.UnsetBrush;
				break;
			}
			this._hasBaseBrushSet = true;
			this.SetColor();
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00021A47 File Offset: 0x0001FC47
		private void SetColor()
		{
			if (this.IsErrored)
			{
				base.Brush.Color = this.ErroredColor;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00021A62 File Offset: 0x0001FC62
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x00021A6A File Offset: 0x0001FC6A
		[Editor(false)]
		public int FormationClass
		{
			get
			{
				return this._formationClass;
			}
			set
			{
				if (value != this._formationClass || !this._hasBaseBrushSet)
				{
					this._formationClass = value;
					base.OnPropertyChanged(value, "FormationClass");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00021A96 File Offset: 0x0001FC96
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x00021A9E File Offset: 0x0001FC9E
		[Editor(false)]
		public Color ErroredColor
		{
			get
			{
				return this._erroredColor;
			}
			set
			{
				if (value != this._erroredColor)
				{
					this._erroredColor = value;
					base.OnPropertyChanged(value, "ErroredColor");
				}
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x00021AC1 File Offset: 0x0001FCC1
		// (set) Token: 0x06000C2D RID: 3117 RVA: 0x00021AC9 File Offset: 0x0001FCC9
		[Editor(false)]
		public bool IsErrored
		{
			get
			{
				return this._isErrored;
			}
			set
			{
				if (value != this._isErrored)
				{
					this._isErrored = value;
					base.OnPropertyChanged(value, "IsErrored");
					this.SetColor();
				}
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00021AED File Offset: 0x0001FCED
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x00021AF5 File Offset: 0x0001FCF5
		[Editor(false)]
		public Brush UnsetBrush
		{
			get
			{
				return this._unsetBrush;
			}
			set
			{
				if (value != this._unsetBrush)
				{
					this._unsetBrush = value;
					base.OnPropertyChanged<Brush>(value, "UnsetBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00021B19 File Offset: 0x0001FD19
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00021B21 File Offset: 0x0001FD21
		[Editor(false)]
		public Brush InfantryBrush
		{
			get
			{
				return this._infantryBrush;
			}
			set
			{
				if (value != this._infantryBrush)
				{
					this._infantryBrush = value;
					base.OnPropertyChanged<Brush>(value, "InfantryBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00021B45 File Offset: 0x0001FD45
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00021B4D File Offset: 0x0001FD4D
		[Editor(false)]
		public Brush RangedBrush
		{
			get
			{
				return this._rangedBrush;
			}
			set
			{
				if (value != this._rangedBrush)
				{
					this._rangedBrush = value;
					base.OnPropertyChanged<Brush>(value, "RangedBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00021B71 File Offset: 0x0001FD71
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x00021B79 File Offset: 0x0001FD79
		[Editor(false)]
		public Brush CavalryBrush
		{
			get
			{
				return this._cavalryBrush;
			}
			set
			{
				if (value != this._cavalryBrush)
				{
					this._cavalryBrush = value;
					base.OnPropertyChanged<Brush>(value, "CavalryBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00021B9D File Offset: 0x0001FD9D
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x00021BA5 File Offset: 0x0001FDA5
		[Editor(false)]
		public Brush HorseArcherBrush
		{
			get
			{
				return this._horseArcherBrush;
			}
			set
			{
				if (value != this._horseArcherBrush)
				{
					this._horseArcherBrush = value;
					base.OnPropertyChanged<Brush>(value, "HorseArcherBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00021BC9 File Offset: 0x0001FDC9
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x00021BD1 File Offset: 0x0001FDD1
		[Editor(false)]
		public Brush InfantryAndRangedBrush
		{
			get
			{
				return this._infantryAndRangedBrush;
			}
			set
			{
				if (value != this._infantryAndRangedBrush)
				{
					this._infantryAndRangedBrush = value;
					base.OnPropertyChanged<Brush>(value, "InfantryAndRangedBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00021BF5 File Offset: 0x0001FDF5
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00021BFD File Offset: 0x0001FDFD
		[Editor(false)]
		public Brush CavalryAndHorseArcherBrush
		{
			get
			{
				return this._cavalryAndHorseArcherBrush;
			}
			set
			{
				if (value != this._cavalryAndHorseArcherBrush)
				{
					this._cavalryAndHorseArcherBrush = value;
					base.OnPropertyChanged<Brush>(value, "CavalryAndHorseArcherBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x0400057F RID: 1407
		private bool _hasBaseBrushSet;

		// Token: 0x04000580 RID: 1408
		private int _formationClass;

		// Token: 0x04000581 RID: 1409
		private Color _erroredColor;

		// Token: 0x04000582 RID: 1410
		private bool _isErrored;

		// Token: 0x04000583 RID: 1411
		private Brush _unsetBrush;

		// Token: 0x04000584 RID: 1412
		private Brush _infantryBrush;

		// Token: 0x04000585 RID: 1413
		private Brush _rangedBrush;

		// Token: 0x04000586 RID: 1414
		private Brush _cavalryBrush;

		// Token: 0x04000587 RID: 1415
		private Brush _horseArcherBrush;

		// Token: 0x04000588 RID: 1416
		private Brush _infantryAndRangedBrush;

		// Token: 0x04000589 RID: 1417
		private Brush _cavalryAndHorseArcherBrush;
	}
}
