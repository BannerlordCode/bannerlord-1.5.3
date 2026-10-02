using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E5 RID: 229
	public class ReloadBarHeightAdjustmentWidget : Widget
	{
		// Token: 0x06000BE7 RID: 3047 RVA: 0x000212EF File Offset: 0x0001F4EF
		public ReloadBarHeightAdjustmentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000212F8 File Offset: 0x0001F4F8
		private void Refresh()
		{
			if (this.FillWidget != null)
			{
				base.ScaledSuggestedHeight = 50f * this.RelativeDurationToMaxDuration * base._scaleToUse;
				this.FillWidget.ScaledSuggestedHeight = base.ScaledSuggestedHeight - (this.FillWidget.MarginBottom + this.FillWidget.MarginTop) * base._scaleToUse;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00021356 File Offset: 0x0001F556
		// (set) Token: 0x06000BEA RID: 3050 RVA: 0x0002135E File Offset: 0x0001F55E
		public float RelativeDurationToMaxDuration
		{
			get
			{
				return this._relativeDurationToMaxDuration;
			}
			set
			{
				if (value != this._relativeDurationToMaxDuration)
				{
					this._relativeDurationToMaxDuration = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00021376 File Offset: 0x0001F576
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x0002137E File Offset: 0x0001F57E
		public Widget FillWidget
		{
			get
			{
				return this._fillWidget;
			}
			set
			{
				if (value != this._fillWidget)
				{
					this._fillWidget = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x04000565 RID: 1381
		private const float _baseHeight = 50f;

		// Token: 0x04000566 RID: 1382
		private float _relativeDurationToMaxDuration;

		// Token: 0x04000567 RID: 1383
		private Widget _fillWidget;
	}
}
