using System;
using TaleWorlds.CampaignSystem.MapEvents;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000B4 RID: 180
	public class VisualCreator
	{
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x0005E1E0 File Offset: 0x0005C3E0
		// (set) Token: 0x06001405 RID: 5125 RVA: 0x0005E1E8 File Offset: 0x0005C3E8
		public IMapEventVisualCreator MapEventVisualCreator { get; set; }

		// Token: 0x06001406 RID: 5126 RVA: 0x0005E1F1 File Offset: 0x0005C3F1
		public IMapEventVisual CreateMapEventVisual(MapEvent mapEvent)
		{
			IMapEventVisualCreator mapEventVisualCreator = this.MapEventVisualCreator;
			if (mapEventVisualCreator == null)
			{
				return null;
			}
			return mapEventVisualCreator.CreateMapEventVisual(mapEvent);
		}
	}
}
