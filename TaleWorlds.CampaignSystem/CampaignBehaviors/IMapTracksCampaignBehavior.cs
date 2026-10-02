using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000422 RID: 1058
	public interface IMapTracksCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06004343 RID: 17219
		MBReadOnlyList<Track> DetectedTracks { get; }

		// Token: 0x06004344 RID: 17220
		void AddTrack(MobileParty target, CampaignVec2 trackPosition, Vec2 trackDirection);

		// Token: 0x06004345 RID: 17221
		void AddMapArrow(TextObject pointerName, CampaignVec2 trackPosition, Vec2 trackDirection, float life);
	}
}
