using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000313 RID: 787
	// (Invoke) Token: 0x06002D94 RID: 11668
	public delegate bool CanTalkToHeroDelegate(Hero hero, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty, out TextObject cantTalkReason);
}
