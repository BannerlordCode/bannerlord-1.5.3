using System;
using TaleWorlds.CampaignSystem.MapEvents;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000B5 RID: 181
	public interface IMapEventVisualCreator
	{
		// Token: 0x06001408 RID: 5128
		IMapEventVisual CreateMapEventVisual(MapEvent mapEvent);
	}
}
