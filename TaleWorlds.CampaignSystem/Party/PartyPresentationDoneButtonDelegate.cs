using System;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x0200030D RID: 781
	// (Invoke) Token: 0x06002D7C RID: 11644
	public delegate bool PartyPresentationDoneButtonDelegate(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null);
}
