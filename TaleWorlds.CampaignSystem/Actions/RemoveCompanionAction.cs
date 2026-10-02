using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004EA RID: 1258
	public static class RemoveCompanionAction
	{
		// Token: 0x06004DD6 RID: 19926 RVA: 0x00189AB4 File Offset: 0x00187CB4
		private static void ApplyInternal(Clan clan, Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			MobileParty partyBelongedTo = companion.PartyBelongedTo;
			PartyBase partyBase = ((partyBelongedTo != null) ? partyBelongedTo.Party : null);
			companion.CompanionOf = null;
			if (partyBase != null && partyBase.IsMobile && detail != RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord)
			{
				bool flag = partyBase.LeaderHero == companion;
				partyBase.MemberRoster.AddToCounts(companion.CharacterObject, -1, false, 0, 0, true, -1);
				if (flag)
				{
					partyBase.MobileParty.SetMoveModeHold();
					partyBase.MobileParty.Ai.RethinkAtNextHourlyTick = true;
					if (partyBase.MemberRoster.Count == 0)
					{
						DestroyPartyAction.Apply(null, partyBase.MobileParty);
					}
					else
					{
						DisbandPartyAction.StartDisband(partyBase.MobileParty);
					}
				}
			}
			if (detail == RemoveCompanionAction.RemoveCompanionDetail.Fire)
			{
				if (companion.PartyBelongedToAsPrisoner != null)
				{
					EndCaptivityAction.ApplyByEscape(companion, null, true);
				}
				else
				{
					MakeHeroFugitiveAction.Apply(companion, false);
				}
				if (companion.IsWanderer)
				{
					companion.ResetEquipments();
				}
			}
			if (companion.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(companion);
			}
			CampaignEventDispatcher.Instance.OnCompanionRemoved(companion, detail);
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x00189B93 File Offset: 0x00187D93
		public static void ApplyByFire(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.Fire);
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x00189B9D File Offset: 0x00187D9D
		public static void ApplyAfterQuest(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.AfterQuest);
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x00189BA7 File Offset: 0x00187DA7
		public static void ApplyByDeath(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.Death);
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x00189BB1 File Offset: 0x00187DB1
		public static void ApplyByByTurningToLord(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord);
		}

		// Token: 0x020008EE RID: 2286
		public enum RemoveCompanionDetail
		{
			// Token: 0x040026B1 RID: 9905
			Fire,
			// Token: 0x040026B2 RID: 9906
			Death,
			// Token: 0x040026B3 RID: 9907
			AfterQuest,
			// Token: 0x040026B4 RID: 9908
			ByTurningToLord
		}
	}
}
