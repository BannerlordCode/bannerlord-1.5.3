using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EA RID: 234
	public class OrderOfBattleFormationClassBrushWidget : BrushWidget
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x000216F1 File Offset: 0x0001F8F1
		public OrderOfBattleFormationClassBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000216FC File Offset: 0x0001F8FC
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

		// Token: 0x06000C10 RID: 3088 RVA: 0x000217AF File Offset: 0x0001F9AF
		private void SetColor()
		{
			if (this.IsErrored)
			{
				base.Brush.Color = this.ErroredColor;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x000217CA File Offset: 0x0001F9CA
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x000217D2 File Offset: 0x0001F9D2
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

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x000217FE File Offset: 0x0001F9FE
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00021806 File Offset: 0x0001FA06
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

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00021829 File Offset: 0x0001FA29
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x00021831 File Offset: 0x0001FA31
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

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00021855 File Offset: 0x0001FA55
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x0002185D File Offset: 0x0001FA5D
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

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00021881 File Offset: 0x0001FA81
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x00021889 File Offset: 0x0001FA89
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

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x000218AD File Offset: 0x0001FAAD
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x000218B5 File Offset: 0x0001FAB5
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

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x000218D9 File Offset: 0x0001FAD9
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x000218E1 File Offset: 0x0001FAE1
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

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00021905 File Offset: 0x0001FB05
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x0002190D File Offset: 0x0001FB0D
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

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00021931 File Offset: 0x0001FB31
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x00021939 File Offset: 0x0001FB39
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

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0002195D File Offset: 0x0001FB5D
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00021965 File Offset: 0x0001FB65
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

		// Token: 0x04000574 RID: 1396
		private bool _hasBaseBrushSet;

		// Token: 0x04000575 RID: 1397
		private int _formationClass;

		// Token: 0x04000576 RID: 1398
		private Color _erroredColor;

		// Token: 0x04000577 RID: 1399
		private bool _isErrored;

		// Token: 0x04000578 RID: 1400
		private Brush _unsetBrush;

		// Token: 0x04000579 RID: 1401
		private Brush _infantryBrush;

		// Token: 0x0400057A RID: 1402
		private Brush _rangedBrush;

		// Token: 0x0400057B RID: 1403
		private Brush _cavalryBrush;

		// Token: 0x0400057C RID: 1404
		private Brush _horseArcherBrush;

		// Token: 0x0400057D RID: 1405
		private Brush _infantryAndRangedBrush;

		// Token: 0x0400057E RID: 1406
		private Brush _cavalryAndHorseArcherBrush;
	}
}
