using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200020B RID: 523
	public abstract class ClanMemberPartyRoleModel : MBGameModel<ClanMemberPartyRoleModel>
	{
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600205C RID: 8284
		public abstract int MaximumPartyRoleAssignmentCount { get; }

		// Token: 0x0600205D RID: 8285
		public abstract IEnumerable<PartyRole> GetAssignablePartyRoles();

		// Token: 0x0600205E RID: 8286
		public abstract SkillObject GetRelevantSkillForPartyRole(PartyRole role);

		// Token: 0x0600205F RID: 8287
		public abstract bool IsHeroAssignableForPartyRole(Hero hero, PartyRole role, MobileParty party);

		// Token: 0x06002060 RID: 8288
		public abstract bool DoesHeroHaveEnoughSkillForPartyRole(Hero hero, PartyRole role, MobileParty party);

		// Token: 0x06002061 RID: 8289
		public abstract bool IsHeroAssignableForPartyRoleInParty(PartyRole role, Hero hero, MobileParty party);
	}
}
