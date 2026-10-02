using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010B RID: 267
	public class DefaultClanMemberPartyRoleModel : ClanMemberPartyRoleModel
	{
		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x0006F4EB File Offset: 0x0006D6EB
		public override int MaximumPartyRoleAssignmentCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0006F4EE File Offset: 0x0006D6EE
		public override IEnumerable<PartyRole> GetAssignablePartyRoles()
		{
			yield return PartyRole.Quartermaster;
			yield return PartyRole.Scout;
			yield return PartyRole.Surgeon;
			yield return PartyRole.Engineer;
			yield break;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0006F4F8 File Offset: 0x0006D6F8
		public override SkillObject GetRelevantSkillForPartyRole(PartyRole role)
		{
			if (role == PartyRole.Engineer)
			{
				return DefaultSkills.Engineering;
			}
			if (role == PartyRole.Quartermaster)
			{
				return DefaultSkills.Steward;
			}
			if (role == PartyRole.Scout)
			{
				return DefaultSkills.Scouting;
			}
			if (role == PartyRole.Surgeon)
			{
				return DefaultSkills.Medicine;
			}
			Debug.FailedAssert(string.Format("Undefined clan role relevant skill {0}", role), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultClanMemberRoleModel.cs", "GetRelevantSkillForPartyRole", 43);
			return null;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0006F551 File Offset: 0x0006D751
		public override bool IsHeroAssignableForPartyRole(Hero hero, PartyRole role, MobileParty party)
		{
			return Campaign.Current.Models.ClanMemberPartyRoleModel.DoesHeroHaveEnoughSkillForPartyRole(hero, role, party) && hero.CanBeGovernorOrHavePartyRole();
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0006F574 File Offset: 0x0006D774
		public override bool DoesHeroHaveEnoughSkillForPartyRole(Hero hero, PartyRole role, MobileParty party)
		{
			if (party.GetHeroPartyRoles(hero).Contains(role))
			{
				return true;
			}
			if (role == PartyRole.Engineer || role == PartyRole.Quartermaster || role == PartyRole.Scout || role == PartyRole.Surgeon)
			{
				return Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(role, hero, party);
			}
			if (role == PartyRole.None)
			{
				return true;
			}
			Debug.FailedAssert(string.Format("Undefined clan role is asked if assignable {0}", role), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultClanMemberRoleModel.cs", "DoesHeroHaveEnoughSkillForPartyRole", 73);
			return false;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0006F5E3 File Offset: 0x0006D7E3
		public override bool IsHeroAssignableForPartyRoleInParty(PartyRole role, Hero hero, MobileParty party)
		{
			return hero.PartyBelongedTo == party && hero != party.GetRoleHolder(role) && hero.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(role)) >= 0;
		}
	}
}
