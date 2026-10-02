using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000168 RID: 360
	public class CraftingCardHighlightBrushWidget : BrushWidget
	{
		// Token: 0x06001324 RID: 4900 RVA: 0x00034A48 File Offset: 0x00032C48
		public CraftingCardHighlightBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00034A58 File Offset: 0x00032C58
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			if (this._firstFrame && base.IsVisible)
			{
				this._firstFrame = false;
				return;
			}
			if (!this._playingAnimation && !this._firstFrame)
			{
				base.BrushRenderer.RestartAnimation();
				this._playingAnimation = true;
			}
		}

		// Token: 0x040008B9 RID: 2233
		private bool _playingAnimation;

		// Token: 0x040008BA RID: 2234
		private bool _firstFrame = true;
	}
}
