using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x0200030E RID: 782
	// (Invoke) Token: 0x06002D80 RID: 11648
	public delegate Tuple<bool, TextObject> PartyPresentationDoneButtonConditionDelegate(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum);
}
