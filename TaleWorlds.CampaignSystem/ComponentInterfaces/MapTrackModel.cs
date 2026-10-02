using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C3 RID: 451
	public abstract class MapTrackModel : MBGameModel<MapTrackModel>
	{
		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001E5F RID: 7775
		public abstract float MaxTrackLife { get; }

		// Token: 0x06001E60 RID: 7776
		public abstract float GetSkipTrackChance(MobileParty mobileParty);

		// Token: 0x06001E61 RID: 7777
		public abstract float GetMaxTrackSpottingDistanceForMainParty();

		// Token: 0x06001E62 RID: 7778
		public abstract bool CanPartyLeaveTrack(MobileParty mobileParty);

		// Token: 0x06001E63 RID: 7779
		public abstract float GetTrackDetectionDifficultyForMainParty(Track track, float trackSpottingDistance);

		// Token: 0x06001E64 RID: 7780
		public abstract float GetSkillFromTrackDetected(Track track);

		// Token: 0x06001E65 RID: 7781
		public abstract int GetTrackLife(MobileParty mobileParty);

		// Token: 0x06001E66 RID: 7782
		public abstract TextObject TrackTitle(Track track);

		// Token: 0x06001E67 RID: 7783
		public abstract IEnumerable<ValueTuple<TextObject, string>> GetTrackDescription(Track track);

		// Token: 0x06001E68 RID: 7784
		public abstract uint GetTrackColor(Track track);

		// Token: 0x06001E69 RID: 7785
		public abstract float GetTrackScale(Track track);
	}
}
