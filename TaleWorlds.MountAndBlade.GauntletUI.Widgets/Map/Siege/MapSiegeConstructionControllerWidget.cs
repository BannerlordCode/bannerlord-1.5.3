using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011E RID: 286
	public class MapSiegeConstructionControllerWidget : Widget
	{
		// Token: 0x06000F38 RID: 3896 RVA: 0x0002A314 File Offset: 0x00028514
		public MapSiegeConstructionControllerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0002A320 File Offset: 0x00028520
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num;
			if (this._currentWidget != null)
			{
				base.PositionXOffset = MathF.Clamp(this._currentWidget.PositionXOffset + this._currentWidget.Size.X * base._inverseScaleToUse, 0f, base.EventManager.PageSize.X - base.Size.X);
				base.PositionYOffset = MathF.Clamp(this._currentWidget.PositionYOffset, 175f, base.EventManager.PageSize.Y - base.Size.Y - 70f);
				num = this._currentWidget.ReadOnlyBrush.GlobalAlphaFactor;
			}
			else
			{
				base.PositionXOffset = -1000f;
				base.PositionYOffset = -1000f;
				num = 0f;
			}
			base.IsEnabled = num >= 0.95f;
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0002A412 File Offset: 0x00028612
		public void SetCurrentPOIWidget(MapSiegePOIBrushWidget widget)
		{
			if (widget == null || widget == this._currentWidget)
			{
				this._currentWidget = null;
				return;
			}
			this._currentWidget = (widget.IsPlayerSidePOI ? widget : null);
		}

		// Token: 0x040006F7 RID: 1783
		private MapSiegePOIBrushWidget _currentWidget;
	}
}
