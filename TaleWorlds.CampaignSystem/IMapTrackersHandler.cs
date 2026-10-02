using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A5 RID: 165
	public interface IMapTrackersHandler
	{
		// Token: 0x06001392 RID: 5010
		void OnTrackerAdded(ITrackableCampaignObject trackable);

		// Token: 0x06001393 RID: 5011
		void OnTrackerRemoved(ITrackableCampaignObject trackable);

		// Token: 0x06001394 RID: 5012
		void ResetTrackers();
	}
}
