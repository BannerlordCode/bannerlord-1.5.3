using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F0 RID: 240
	public class OrderOfBattleFormationMarkerBrushWidget : BrushWidget
	{
		// Token: 0x06000C6C RID: 3180 RVA: 0x00022162 File Offset: 0x00020362
		public OrderOfBattleFormationMarkerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0002216C File Offset: 0x0002036C
		protected override void OnUpdate(float dt)
		{
			base.IsVisible = this.IsAvailable && this.WSign > 0;
			if (base.IsVisible)
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000221E1 File Offset: 0x000203E1
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x000221E9 File Offset: 0x000203E9
		[Editor(false)]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0002220C File Offset: 0x0002040C
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x00022214 File Offset: 0x00020414
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
				}
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00022232 File Offset: 0x00020432
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x0002223A File Offset: 0x0002043A
		[Editor(false)]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChanged(value, "IsTracked");
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00022258 File Offset: 0x00020458
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x00022260 File Offset: 0x00020460
		[Editor(false)]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (this._wSign != value)
				{
					this._wSign = value;
					base.OnPropertyChanged(value, "WSign");
				}
			}
		}

		// Token: 0x040005A0 RID: 1440
		private Vec2 _position;

		// Token: 0x040005A1 RID: 1441
		private bool _isAvailable;

		// Token: 0x040005A2 RID: 1442
		private bool _isTracked;

		// Token: 0x040005A3 RID: 1443
		private int _wSign;
	}
}
