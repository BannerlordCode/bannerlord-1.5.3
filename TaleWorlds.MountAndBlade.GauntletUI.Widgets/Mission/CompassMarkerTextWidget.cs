using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DB RID: 219
	public class CompassMarkerTextWidget : TextWidget
	{
		// Token: 0x06000B2D RID: 2861 RVA: 0x0001F729 File Offset: 0x0001D929
		public CompassMarkerTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0001F732 File Offset: 0x0001D932
		private void UpdateBrush()
		{
			if (this.PrimaryBrush != null && this.SecondaryBrush != null)
			{
				base.Brush = (this.IsPrimary ? this.PrimaryBrush : this.SecondaryBrush);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x0001F760 File Offset: 0x0001D960
		// (set) Token: 0x06000B30 RID: 2864 RVA: 0x0001F768 File Offset: 0x0001D968
		public bool IsPrimary
		{
			get
			{
				return this._isPrimary;
			}
			set
			{
				if (this._isPrimary != value)
				{
					this._isPrimary = value;
					base.OnPropertyChanged(value, "IsPrimary");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0001F78C File Offset: 0x0001D98C
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x0001F794 File Offset: 0x0001D994
		public float Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (Math.Abs(this._position - value) > 1E-45f)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0001F7BD File Offset: 0x0001D9BD
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x0001F7C5 File Offset: 0x0001D9C5
		public Brush PrimaryBrush
		{
			get
			{
				return this._primaryBrush;
			}
			set
			{
				if (this._primaryBrush != value)
				{
					this._primaryBrush = value;
					base.OnPropertyChanged<Brush>(value, "PrimaryBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0001F7E9 File Offset: 0x0001D9E9
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x0001F7F1 File Offset: 0x0001D9F1
		public Brush SecondaryBrush
		{
			get
			{
				return this._secondaryBrush;
			}
			set
			{
				if (this._secondaryBrush != value)
				{
					this._secondaryBrush = value;
					base.OnPropertyChanged<Brush>(value, "SecondaryBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x04000510 RID: 1296
		private bool _isPrimary;

		// Token: 0x04000511 RID: 1297
		private float _position;

		// Token: 0x04000512 RID: 1298
		private Brush _primaryBrush;

		// Token: 0x04000513 RID: 1299
		private Brush _secondaryBrush;
	}
}
