using System;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000037 RID: 55
	public interface IGauntletMapEventVisualHandler
	{
		// Token: 0x0600029B RID: 667
		void OnNewEventStarted(GauntletMapEventVisual newEvent);

		// Token: 0x0600029C RID: 668
		void OnInitialized(GauntletMapEventVisual newEvent);

		// Token: 0x0600029D RID: 669
		void OnEventEnded(GauntletMapEventVisual newEvent);

		// Token: 0x0600029E RID: 670
		void OnEventVisibilityChanged(GauntletMapEventVisual visibilityChangedEvent);
	}
}
