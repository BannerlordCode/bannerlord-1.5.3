using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004F4 RID: 1268
	public static class TeleportHeroAction
	{
		// Token: 0x06004DFF RID: 19967 RVA: 0x0018AEA4 File Offset: 0x001890A4
		private static void ApplyInternal(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			CampaignEventDispatcher.Instance.OnHeroTeleportationRequested(hero, targetSettlement, targetParty, detail);
			switch (detail)
			{
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToSettlement:
				if (targetSettlement != null)
				{
					if (!hero.IsActive)
					{
						hero.ChangeState(Hero.CharacterStates.Active);
					}
					if (hero.CurrentSettlement != null)
					{
						LeaveSettlementAction.ApplyForCharacterOnly(hero);
					}
					if (hero.PartyBelongedTo != null)
					{
						if (!hero.PartyBelongedTo.IsActive || hero.PartyBelongedTo.IsCurrentlyEngagingParty || hero.PartyBelongedTo.MapEvent != null)
						{
							return;
						}
						hero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
					EnterSettlementAction.ApplyForCharacterOnly(hero, targetSettlement);
					return;
				}
				break;
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToParty:
				if (hero.IsTraveling)
				{
					hero.ChangeState(Hero.CharacterStates.Active);
				}
				AddHeroToPartyAction.Apply(hero, targetParty, true);
				return;
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToPartyAsPartyLeader:
				if (hero.IsTraveling)
				{
					hero.ChangeState(Hero.CharacterStates.Active);
				}
				AddHeroToPartyAction.Apply(hero, targetParty, true);
				targetParty.ChangePartyLeader(hero);
				targetParty.PartyComponent.ClearCachedName();
				targetParty.Party.SetCustomName(null);
				targetParty.Party.SetVisualAsDirty();
				if (targetParty.IsDisbanding)
				{
					DisbandPartyAction.CancelDisband(targetParty);
				}
				if (targetParty.Ai.DoNotMakeNewDecisions)
				{
					targetParty.Ai.SetDoNotMakeNewDecisions(false);
					return;
				}
				break;
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlement:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToParty:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlementAsGovernor:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader:
				if (detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlement && hero.CurrentSettlement == targetSettlement)
				{
					TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, targetSettlement);
					return;
				}
				if (hero.GovernorOf != null)
				{
					ChangeGovernorAction.RemoveGovernorOf(hero);
				}
				if (hero.CurrentSettlement != null && hero.CurrentSettlement != targetSettlement)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(hero);
				}
				if (hero.PartyBelongedTo != null)
				{
					if (!hero.PartyBelongedTo.IsActive || (hero.PartyBelongedTo.IsCurrentlyEngagingParty && hero.PartyBelongedTo.MapEvent != null))
					{
						return;
					}
					hero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
				}
				if (detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader)
				{
					TextObject textObject = new TextObject("{=ithcVNfA}{CLAN_NAME}{.o} Party", null);
					textObject.SetTextVariable("CLAN_NAME", (targetParty.ActualClan != null) ? targetParty.ActualClan.Name : CampaignData.NeutralFactionName);
					targetParty.Party.SetCustomName(textObject);
				}
				hero.ChangeState(Hero.CharacterStates.Traveling);
				break;
			default:
				return;
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x0018B0B9 File Offset: 0x001892B9
		public static void ApplyImmediateTeleportToSettlement(Hero heroToBeMoved, Settlement targetSettlement)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, targetSettlement, null, TeleportHeroAction.TeleportationDetail.ImmediateTeleportToSettlement);
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x0018B0C4 File Offset: 0x001892C4
		public static void ApplyImmediateTeleportToParty(Hero heroToBeMoved, MobileParty party)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, null, party, TeleportHeroAction.TeleportationDetail.ImmediateTeleportToParty);
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x0018B0CF File Offset: 0x001892CF
		public static void ApplyImmediateTeleportToPartyAsPartyLeader(Hero heroToBeMoved, MobileParty party)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, null, party, TeleportHeroAction.TeleportationDetail.ImmediateTeleportToPartyAsPartyLeader);
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x0018B0DA File Offset: 0x001892DA
		public static void ApplyDelayedTeleportToSettlement(Hero heroToBeMoved, Settlement targetSettlement)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, targetSettlement, null, TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlement);
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0018B0E5 File Offset: 0x001892E5
		public static void ApplyDelayedTeleportToParty(Hero heroToBeMoved, MobileParty party)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, null, party, TeleportHeroAction.TeleportationDetail.DelayedTeleportToParty);
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x0018B0F0 File Offset: 0x001892F0
		public static void ApplyDelayedTeleportToSettlementAsGovernor(Hero heroToBeMoved, Settlement targetSettlement)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, targetSettlement, null, TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlementAsGovernor);
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x0018B0FB File Offset: 0x001892FB
		public static void ApplyDelayedTeleportToPartyAsPartyLeader(Hero heroToBeMoved, MobileParty party)
		{
			TeleportHeroAction.ApplyInternal(heroToBeMoved, null, party, TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader);
		}

		// Token: 0x020008F4 RID: 2292
		public enum TeleportationDetail
		{
			// Token: 0x040026CC RID: 9932
			ImmediateTeleportToSettlement,
			// Token: 0x040026CD RID: 9933
			ImmediateTeleportToParty,
			// Token: 0x040026CE RID: 9934
			ImmediateTeleportToPartyAsPartyLeader,
			// Token: 0x040026CF RID: 9935
			DelayedTeleportToSettlement,
			// Token: 0x040026D0 RID: 9936
			DelayedTeleportToParty,
			// Token: 0x040026D1 RID: 9937
			DelayedTeleportToSettlementAsGovernor,
			// Token: 0x040026D2 RID: 9938
			DelayedTeleportToPartyAsPartyLeader
		}
	}
}
