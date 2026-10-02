using System;

namespace TaleWorlds.CampaignSystem.MapEvents
{
	// Token: 0x02000332 RID: 818
	public interface IMapEventVisual
	{
		// Token: 0x0600317B RID: 12667
		void Initialize(CampaignVec2 position, bool isVisible);

		// Token: 0x0600317C RID: 12668
		void OnMapEventEnd();

		// Token: 0x0600317D RID: 12669
		void SetVisibility(bool isVisible);
	}
}
