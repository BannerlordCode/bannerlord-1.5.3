using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004D7 RID: 1239
	public static class EndCaptivityAction
	{
		// Token: 0x06004D81 RID: 19841 RVA: 0x00187C1C File Offset: 0x00185E1C
		private static void ApplyInternal(Hero prisoner, EndCaptivityDetail detail, Hero facilitatior = null, bool showNotification = true)
		{
			PartyBase partyBelongedToAsPrisoner = prisoner.PartyBelongedToAsPrisoner;
			IFaction faction = ((partyBelongedToAsPrisoner != null) ? partyBelongedToAsPrisoner.MapFaction : null);
			if (prisoner == Hero.MainHero)
			{
				PlayerCaptivity.EndCaptivity();
				if (partyBelongedToAsPrisoner != null && partyBelongedToAsPrisoner.IsSettlement)
				{
					MobileParty.MainParty.DisembarkToPosition(partyBelongedToAsPrisoner.Settlement.GatePosition);
				}
				else if (partyBelongedToAsPrisoner != null && partyBelongedToAsPrisoner.IsMobile)
				{
					MobileParty.MainParty.IsCurrentlyAtSea = partyBelongedToAsPrisoner.MobileParty.IsCurrentlyAtSea;
				}
				if (facilitatior != null && detail != EndCaptivityDetail.Death)
				{
					StringHelpers.SetCharacterProperties("FACILITATOR", facilitatior.CharacterObject, null, false);
					MBInformationManager.AddQuickInformation(new TextObject("{=xPuSASof}{FACILITATOR.NAME} paid a ransom and freed you from captivity.", null), 0, null, null, "");
				}
				CampaignEventDispatcher.Instance.OnHeroPrisonerReleased(prisoner, partyBelongedToAsPrisoner, faction, detail, true);
				return;
			}
			if (detail == EndCaptivityDetail.Death)
			{
				prisoner.StayingInSettlement = null;
			}
			if (partyBelongedToAsPrisoner != null && partyBelongedToAsPrisoner.PrisonRoster.Contains(prisoner.CharacterObject))
			{
				partyBelongedToAsPrisoner.PrisonRoster.RemoveTroop(prisoner.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
			}
			if (detail != EndCaptivityDetail.Death)
			{
				if (detail <= EndCaptivityDetail.ReleasedByChoice || detail == EndCaptivityDetail.ReleasedByCompensation)
				{
					prisoner.ChangeState(Hero.CharacterStates.Released);
					if (prisoner.IsPlayerCompanion && detail != EndCaptivityDetail.Ransom)
					{
						MakeHeroFugitiveAction.Apply(prisoner, false);
					}
				}
				else
				{
					MakeHeroFugitiveAction.Apply(prisoner, false);
				}
				Settlement currentSettlement = prisoner.CurrentSettlement;
				if (currentSettlement != null)
				{
					currentSettlement.AddHeroWithoutParty(prisoner);
				}
				CampaignEventDispatcher.Instance.OnHeroPrisonerReleased(prisoner, partyBelongedToAsPrisoner, faction, detail, showNotification);
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x00187D5D File Offset: 0x00185F5D
		public static void ApplyByReleasedAfterBattle(Hero character)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedAfterBattle, null, true);
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x00187D68 File Offset: 0x00185F68
		public static void ApplyByRansom(Hero character, Hero facilitator)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.Ransom, facilitator, true);
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x00187D73 File Offset: 0x00185F73
		public static void ApplyByPeace(Hero character, Hero facilitator = null)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedAfterPeace, facilitator, true);
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x00187D7E File Offset: 0x00185F7E
		public static void ApplyByEscape(Hero character, Hero facilitator = null, bool showNotification = true)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedAfterEscape, facilitator, showNotification);
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x00187D89 File Offset: 0x00185F89
		public static void ApplyByDeath(Hero character)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.Death, null, true);
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x00187D94 File Offset: 0x00185F94
		public static void ApplyByReleasedByChoice(FlattenedTroopRoster troopRoster)
		{
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in troopRoster)
			{
				if (flattenedTroopRosterElement.Troop.IsHero)
				{
					EndCaptivityAction.ApplyInternal(flattenedTroopRosterElement.Troop.HeroObject, EndCaptivityDetail.ReleasedByChoice, null, true);
				}
			}
			CampaignEventDispatcher.Instance.OnPrisonerReleased(troopRoster);
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x00187E04 File Offset: 0x00186004
		public static void ApplyByReleasedByChoice(Hero character, Hero facilitator = null)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedByChoice, facilitator, true);
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x00187E0F File Offset: 0x0018600F
		public static void ApplyByReleasedByCompensation(Hero character)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedByCompensation, null, true);
		}
	}
}
