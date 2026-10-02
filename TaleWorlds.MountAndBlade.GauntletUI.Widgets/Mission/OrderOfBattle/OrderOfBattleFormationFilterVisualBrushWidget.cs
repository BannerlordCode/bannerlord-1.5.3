using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EE RID: 238
	public class OrderOfBattleFormationFilterVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000C47 RID: 3143 RVA: 0x00021D04 File Offset: 0x0001FF04
		public OrderOfBattleFormationFilterVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00021D10 File Offset: 0x0001FF10
		private void SetBaseBrush()
		{
			switch (this.FormationFilter)
			{
			case 0:
				base.Brush = this.UnsetBrush;
				break;
			case 1:
				base.Brush = this.ShieldBrush;
				break;
			case 2:
				base.Brush = this.SpearBrush;
				break;
			case 3:
				base.Brush = this.ThrownBrush;
				break;
			case 4:
				base.Brush = this.HeavyBrush;
				break;
			case 5:
				base.Brush = this.HighTierBrush;
				break;
			case 6:
				base.Brush = this.LowTierBrush;
				break;
			default:
				base.Brush = this.UnsetBrush;
				break;
			}
			this._hasBaseBrushSet = true;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00021DBD File Offset: 0x0001FFBD
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x00021DC5 File Offset: 0x0001FFC5
		[Editor(false)]
		public int FormationFilter
		{
			get
			{
				return this._formationFilter;
			}
			set
			{
				if (value != this._formationFilter || !this._hasBaseBrushSet)
				{
					this._formationFilter = value;
					base.OnPropertyChanged(value, "FormationFilter");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00021DF1 File Offset: 0x0001FFF1
		// (set) Token: 0x06000C4C RID: 3148 RVA: 0x00021DF9 File Offset: 0x0001FFF9
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

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00021E1D File Offset: 0x0002001D
		// (set) Token: 0x06000C4E RID: 3150 RVA: 0x00021E25 File Offset: 0x00020025
		[Editor(false)]
		public Brush SpearBrush
		{
			get
			{
				return this._spearBrush;
			}
			set
			{
				if (value != this._spearBrush)
				{
					this._spearBrush = value;
					base.OnPropertyChanged<Brush>(value, "SpearBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00021E49 File Offset: 0x00020049
		// (set) Token: 0x06000C50 RID: 3152 RVA: 0x00021E51 File Offset: 0x00020051
		[Editor(false)]
		public Brush ShieldBrush
		{
			get
			{
				return this._shieldBrush;
			}
			set
			{
				if (value != this._shieldBrush)
				{
					this._shieldBrush = value;
					base.OnPropertyChanged<Brush>(value, "ShieldBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x00021E75 File Offset: 0x00020075
		// (set) Token: 0x06000C52 RID: 3154 RVA: 0x00021E7D File Offset: 0x0002007D
		[Editor(false)]
		public Brush ThrownBrush
		{
			get
			{
				return this._thrownBrush;
			}
			set
			{
				if (value != this._thrownBrush)
				{
					this._thrownBrush = value;
					base.OnPropertyChanged<Brush>(value, "ThrownBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00021EA1 File Offset: 0x000200A1
		// (set) Token: 0x06000C54 RID: 3156 RVA: 0x00021EA9 File Offset: 0x000200A9
		[Editor(false)]
		public Brush HeavyBrush
		{
			get
			{
				return this._heavyBrush;
			}
			set
			{
				if (value != this._heavyBrush)
				{
					this._heavyBrush = value;
					base.OnPropertyChanged<Brush>(value, "HeavyBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x00021ECD File Offset: 0x000200CD
		// (set) Token: 0x06000C56 RID: 3158 RVA: 0x00021ED5 File Offset: 0x000200D5
		[Editor(false)]
		public Brush HighTierBrush
		{
			get
			{
				return this._highTierBrush;
			}
			set
			{
				if (value != this._highTierBrush)
				{
					this._highTierBrush = value;
					base.OnPropertyChanged<Brush>(value, "HighTierBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00021EF9 File Offset: 0x000200F9
		// (set) Token: 0x06000C58 RID: 3160 RVA: 0x00021F01 File Offset: 0x00020101
		[Editor(false)]
		public Brush LowTierBrush
		{
			get
			{
				return this._lowTierBrush;
			}
			set
			{
				if (value != this._lowTierBrush)
				{
					this._lowTierBrush = value;
					base.OnPropertyChanged<Brush>(value, "LowTierBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x0400058F RID: 1423
		private bool _hasBaseBrushSet;

		// Token: 0x04000590 RID: 1424
		private int _formationFilter;

		// Token: 0x04000591 RID: 1425
		private Brush _unsetBrush;

		// Token: 0x04000592 RID: 1426
		private Brush _spearBrush;

		// Token: 0x04000593 RID: 1427
		private Brush _shieldBrush;

		// Token: 0x04000594 RID: 1428
		private Brush _thrownBrush;

		// Token: 0x04000595 RID: 1429
		private Brush _heavyBrush;

		// Token: 0x04000596 RID: 1430
		private Brush _highTierBrush;

		// Token: 0x04000597 RID: 1431
		private Brush _lowTierBrush;
	}
}
