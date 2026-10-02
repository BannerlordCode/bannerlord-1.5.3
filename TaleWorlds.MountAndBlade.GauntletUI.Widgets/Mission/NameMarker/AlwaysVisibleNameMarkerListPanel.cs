using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F5 RID: 245
	public class AlwaysVisibleNameMarkerListPanel : ListPanel
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x000229DB File Offset: 0x00020BDB
		private float _normalOpacity
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000229E2 File Offset: 0x00020BE2
		private float _screenCenterOpacity
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x000229E9 File Offset: 0x00020BE9
		private float _stayOnScreenTimeInSeconds
		{
			get
			{
				return 5f;
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000229F0 File Offset: 0x00020BF0
		public AlwaysVisibleNameMarkerListPanel(UIContext context)
			: base(context)
		{
			this._parentScreenWidget = base.EventManager.Root.GetChild(0).GetChild(0);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00022A18 File Offset: 0x00020C18
		protected override void OnLateUpdate(float dt)
		{
			base.ApplyActionToAllChildrenRecursive(delegate(Widget child)
			{
				child.IsVisible = true;
			});
			base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			this.UpdateOpacity();
			if (this._totalDt > this._stayOnScreenTimeInSeconds)
			{
				base.EventFired("Remove", Array.Empty<object>());
			}
			this._totalDt += dt;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00022AC4 File Offset: 0x00020CC4
		private void UpdateOpacity()
		{
			Vec2 vec = new Vec2(base.Context.TwoDimensionContext.Platform.Width / 2f, base.Context.TwoDimensionContext.Platform.Height / 2f);
			Vec2 vec2 = new Vec2(base.ScaledPositionXOffset, base.ScaledPositionYOffset);
			float num = ((vec2.Distance(vec) <= 150f) ? this._screenCenterOpacity : this._normalOpacity);
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00022B4B File Offset: 0x00020D4B
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x00022B53 File Offset: 0x00020D53
		[DataSourceProperty]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x040005BA RID: 1466
		private Widget _parentScreenWidget;

		// Token: 0x040005BB RID: 1467
		private float _totalDt;

		// Token: 0x040005BC RID: 1468
		private Vec2 _position;
	}
}
