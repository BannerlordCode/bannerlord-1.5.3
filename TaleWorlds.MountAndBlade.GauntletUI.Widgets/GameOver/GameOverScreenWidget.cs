using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameOver
{
	// Token: 0x02000155 RID: 341
	public class GameOverScreenWidget : Widget
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x000327C6 File Offset: 0x000309C6
		// (set) Token: 0x0600121E RID: 4638 RVA: 0x000327CE File Offset: 0x000309CE
		public BrushWidget ConceptVisualWidget { get; set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x000327D7 File Offset: 0x000309D7
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x000327DF File Offset: 0x000309DF
		public BrushWidget BannerBrushWidget { get; set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x000327E8 File Offset: 0x000309E8
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x000327F0 File Offset: 0x000309F0
		public BrushWidget BannerFrameBrushWidget1 { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x000327F9 File Offset: 0x000309F9
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00032801 File Offset: 0x00030A01
		public BrushWidget BannerFrameBrushWidget2 { get; set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x0003280A File Offset: 0x00030A0A
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x00032812 File Offset: 0x00030A12
		public string GameOverReason { get; set; }

		// Token: 0x06001227 RID: 4647 RVA: 0x0003281B File Offset: 0x00030A1B
		public GameOverScreenWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnManualLateUpdate), 4);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00032840 File Offset: 0x00030A40
		private void OnManualLateUpdate(float obj)
		{
			if (this.ConceptVisualWidget != null)
			{
				this.ConceptVisualWidget.Brush = base.Context.GetBrush("GameOver.Mask." + this.GameOverReason);
			}
			if (this.BannerBrushWidget != null)
			{
				this.BannerBrushWidget.Brush = base.Context.GetBrush("GameOver.Banner." + this.GameOverReason);
			}
			if (this.BannerFrameBrushWidget1 != null)
			{
				this.BannerFrameBrushWidget1.Brush = base.Context.GetBrush("GameOver.Banner.Frame." + this.GameOverReason);
			}
			if (this.BannerFrameBrushWidget2 != null)
			{
				this.BannerFrameBrushWidget2.Brush = base.Context.GetBrush("GameOver.Banner.Frame." + this.GameOverReason);
			}
		}
	}
}
