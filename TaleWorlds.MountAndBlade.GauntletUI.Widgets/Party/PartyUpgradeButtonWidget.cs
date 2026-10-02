using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006C RID: 108
	public class PartyUpgradeButtonWidget : ButtonWidget
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x00011A3F File Offset: 0x0000FC3F
		public PartyUpgradeButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00011A48 File Offset: 0x0000FC48
		private void UpdateVisual()
		{
			if (this.ImageIdentifierWidget == null || this.UnavailableBrush == null || this.InsufficientBrush == null)
			{
				return;
			}
			if (!this.IsAvailable)
			{
				this.ImageIdentifierWidget.Brush.GlobalColor = new Color(1f, 1f, 1f, 1f);
				this.ImageIdentifierWidget.Brush.SaturationFactor = -100f;
				this._marinerTroopBrush.SetState("Disabled");
				base.UpdateChildrenStates = false;
				base.IsEnabled = true;
				base.Brush = this.UnavailableBrush;
				return;
			}
			if (this.IsAvailable && this.IsInsufficient)
			{
				this.ImageIdentifierWidget.Brush.GlobalColor = new Color(0.9f, 0.5f, 0.5f, 1f);
				this.ImageIdentifierWidget.Brush.SaturationFactor = -150f;
				this._marinerTroopBrush.SetState("Disabled");
				base.UpdateChildrenStates = false;
				base.IsEnabled = true;
				base.Brush = this.InsufficientBrush;
				return;
			}
			this.ImageIdentifierWidget.Brush.GlobalColor = new Color(1f, 1f, 1f, 1f);
			this.ImageIdentifierWidget.Brush.SaturationFactor = 0f;
			this._marinerTroopBrush.SetState("Default");
			base.UpdateChildrenStates = true;
			base.IsEnabled = true;
			base.Brush = this.DefaultBrush;
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00011BC0 File Offset: 0x0000FDC0
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		[Editor(false)]
		public ImageIdentifierWidget ImageIdentifierWidget
		{
			get
			{
				return this._imageIdentifierWidget;
			}
			set
			{
				if (this._imageIdentifierWidget != value)
				{
					this._imageIdentifierWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "ImageIdentifierWidget");
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00011BE6 File Offset: 0x0000FDE6
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x00011BEE File Offset: 0x0000FDEE
		[Editor(false)]
		public Brush DefaultBrush
		{
			get
			{
				return this._defaultBrush;
			}
			set
			{
				if (this._defaultBrush != value)
				{
					this._defaultBrush = value;
					base.OnPropertyChanged<Brush>(value, "DefaultBrush");
				}
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00011C0C File Offset: 0x0000FE0C
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00011C14 File Offset: 0x0000FE14
		[Editor(false)]
		public BrushWidget MarinerTroopBrush
		{
			get
			{
				return this._marinerTroopBrush;
			}
			set
			{
				if (this._marinerTroopBrush != value)
				{
					this._marinerTroopBrush = value;
					base.OnPropertyChanged<BrushWidget>(value, "MarinerTroopBrush");
				}
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00011C32 File Offset: 0x0000FE32
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x00011C3A File Offset: 0x0000FE3A
		[Editor(false)]
		public Brush UnavailableBrush
		{
			get
			{
				return this._unavailableBrush;
			}
			set
			{
				if (this._unavailableBrush != value)
				{
					this._unavailableBrush = value;
					base.OnPropertyChanged<Brush>(value, "UnavailableBrush");
				}
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00011C58 File Offset: 0x0000FE58
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00011C60 File Offset: 0x0000FE60
		[Editor(false)]
		public Brush InsufficientBrush
		{
			get
			{
				return this._insufficientBrush;
			}
			set
			{
				if (this._insufficientBrush != value)
				{
					this._insufficientBrush = value;
					base.OnPropertyChanged<Brush>(value, "InsufficientBrush");
				}
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00011C7E File Offset: 0x0000FE7E
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x00011C86 File Offset: 0x0000FE86
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (this._isAvailable != value)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
				}
				this.UpdateVisual();
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x00011CAA File Offset: 0x0000FEAA
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x00011CB2 File Offset: 0x0000FEB2
		[Editor(false)]
		public bool IsInsufficient
		{
			get
			{
				return this._isInsufficient;
			}
			set
			{
				if (this._isInsufficient != value)
				{
					this._isInsufficient = value;
					base.OnPropertyChanged(value, "IsInsufficient");
				}
				this.UpdateVisual();
			}
		}

		// Token: 0x04000287 RID: 647
		private ImageIdentifierWidget _imageIdentifierWidget;

		// Token: 0x04000288 RID: 648
		private Brush _defaultBrush;

		// Token: 0x04000289 RID: 649
		private Brush _unavailableBrush;

		// Token: 0x0400028A RID: 650
		private Brush _insufficientBrush;

		// Token: 0x0400028B RID: 651
		private BrushWidget _marinerTroopBrush;

		// Token: 0x0400028C RID: 652
		private bool _isAvailable;

		// Token: 0x0400028D RID: 653
		private bool _isInsufficient;
	}
}
