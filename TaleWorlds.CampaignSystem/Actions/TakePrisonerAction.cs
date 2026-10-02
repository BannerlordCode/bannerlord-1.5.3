using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004F3 RID: 1267
	public static class TakePrisonerAction
	{
		// Token: 0x06004DFC RID: 19964 RVA: 0x0018AD34 File Offset: 0x00188F34
		private static void ApplyInternal(PartyBase capturerParty, Hero prisonerCharacter, bool isEventCalled = true)
		{
			if (prisonerCharacter.PartyBelongedTo != null)
			{
				if (prisonerCharacter.PartyBelongedTo.LeaderHero == prisonerCharacter)
				{
					prisonerCharacter.PartyBelongedTo.RemovePartyLeader();
				}
				prisonerCharacter.PartyBelongedTo.MemberRoster.RemoveTroop(prisonerCharacter.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
			}
			prisonerCharacter.CaptivityStartTime = CampaignTime.Now;
			prisonerCharacter.ChangeState(Hero.CharacterStates.Prisoner);
			capturerParty.AddPrisoner(prisonerCharacter.CharacterObject, 1);
			if (prisonerCharacter == Hero.MainHero)
			{
				if (MobileParty.MainParty.IsDisorganized)
				{
					MobileParty.MainParty.SetDisorganized(false);
				}
				PlayerCaptivity.StartCaptivity(capturerParty);
				if (MobileParty.MainParty.IsCurrentlyAtSea)
				{
					for (int i = MobileParty.MainParty.Ships.Count - 1; i >= 0; i--)
					{
						DestroyShipAction.Apply(MobileParty.MainParty.Ships[i]);
					}
				}
			}
			if (prisonerCharacter.StayingInSettlement != null)
			{
				prisonerCharacter.StayingInSettlement = null;
			}
			if (isEventCalled)
			{
				CampaignEventDispatcher.Instance.OnHeroPrisonerTaken(capturerParty, prisonerCharacter);
			}
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x0018AE23 File Offset: 0x00189023
		public static void Apply(PartyBase capturerParty, Hero prisonerCharacter)
		{
			TakePrisonerAction.ApplyInternal(capturerParty, prisonerCharacter, true);
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0018AE30 File Offset: 0x00189030
		public static void ApplyByTakenFromPartyScreen(FlattenedTroopRoster roster)
		{
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in roster)
			{
				if (flattenedTroopRosterElement.Troop.IsHero)
				{
					TakePrisonerAction.ApplyInternal(PartyBase.MainParty, flattenedTroopRosterElement.Troop.HeroObject, true);
				}
			}
			CampaignEventDispatcher.Instance.OnPrisonerTaken(roster);
		}
	}
}
