using System;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000311 RID: 785
	// (Invoke) Token: 0x06002D8C RID: 11660
	public delegate void PartyScreenClosedDelegate(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel);
}
