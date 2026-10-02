using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000402 RID: 1026
	public class DefaultLogsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600405B RID: 16475 RVA: 0x0011A384 File Offset: 0x00118584
		public override void RegisterEvents()
		{
			CampaignEvents.AlleyOwnerChanged.AddNonSerializedListener(this, new Action<Alley, Hero, Hero>(this.OnAlleyOwnerChanged));
			CampaignEvents.ArmyGathered.AddNonSerializedListener(this, new Action<Army, IMapPoint>(this.OnArmyGathered));
			CampaignEvents.BattleStarted.AddNonSerializedListener(this, new Action<PartyBase, PartyBase, object, bool>(this.OnBattleStarted));
			CampaignEvents.CharacterBecameFugitiveEvent.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnCharacterBecameFugitive));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.ClanChangedKingdom));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnPrisonerTaken));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.OnHeroesMarried));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnArmyCreated));
			CampaignEvents.OnTradeAgreementSignedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnTradeAgreementSigned));
			CampaignEvents.RebellionFinished.AddNonSerializedListener(this, new Action<Settlement, Clan>(this.OnRebellionFinished));
			CampaignEvents.KingdomDecisionAdded.AddNonSerializedListener(this, new Action<KingdomDecision, bool>(this.OnKingdomDecisionAdded));
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.PlayerTraitChangedEvent.AddNonSerializedListener(this, new Action<TraitObject, int>(this.OnPlayerTraitChanged));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			CampaignEvents.OnSiegeAftermathAppliedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>>(this.OnSiegeAftermathApplied));
			CampaignEvents.OnAllianceStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceStartedEvent));
			CampaignEvents.OnAllianceEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom>(this.OnAllianceEndedEvent));
			CampaignEvents.OnCallToWarAgreementStartedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom, Kingdom>(this.OnCallToWarAgreementStarted));
			CampaignEvents.OnCallToWarAgreementEndedEvent.AddNonSerializedListener(this, new Action<Kingdom, Kingdom, Kingdom>(this.OnCallToWarAgreementEnded));
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0011A5A2 File Offset: 0x001187A2
		private void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			LogEntry.AddLogEntry(new SiegeAftermathLogEntry(attackerParty, partyContributions.Keys, settlement, aftermathType));
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0011A5B8 File Offset: 0x001187B8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0011A5BA File Offset: 0x001187BA
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMobileParty, bool isMainPartyChanged)
		{
			LogEntry.AddLogEntry(new PlayerCharacterChangedLogEntry(oldPlayer, newPlayer));
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0011A5C8 File Offset: 0x001187C8
		private void OnPrisonerTaken(PartyBase party, Hero hero)
		{
			LogEntry.AddLogEntry(new TakePrisonerLogEntry(party, hero));
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0011A5D6 File Offset: 0x001187D6
		private void OnHeroPrisonerReleased(Hero hero, PartyBase party, IFaction captuererFaction, EndCaptivityDetail detail, bool showNotification)
		{
			if (showNotification)
			{
				LogEntry.AddLogEntry(new EndCaptivityLogEntry(hero, captuererFaction, detail));
			}
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x0011A5EA File Offset: 0x001187EA
		private void OnCommonAreaFightOccured(MobileParty attackerParty, MobileParty defenderParty, Hero attackerHero, Settlement settlement)
		{
			LogEntry.AddLogEntry(new CommonAreaFightLogEntry(attackerParty, defenderParty, attackerHero, settlement));
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0011A5FB File Offset: 0x001187FB
		private void ClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotifications)
		{
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary)
			{
				LogEntry.AddLogEntry(new MercenaryClanChangedKingdomLogEntry(clan, oldKingdom, newKingdom));
			}
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0011A613 File Offset: 0x00118813
		private void OnCharacterBecameFugitive(Hero hero, bool showNotification)
		{
			LogEntry.AddLogEntry(new CharacterBecameFugitiveLogEntry(hero));
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0011A620 File Offset: 0x00118820
		private void OnBattleStarted(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
			if (showNotification)
			{
				LogEntry.AddLogEntry(new BattleStartedLogEntry(attackerParty, defenderParty, subject));
			}
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0011A634 File Offset: 0x00118834
		public void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			if (isPlayersArmy)
			{
				ArmyDispersionLogEntry armyDispersionLogEntry = new ArmyDispersionLogEntry(army, reason);
				LogEntry.AddLogEntry(armyDispersionLogEntry);
				if (army.LeaderParty.MapFaction == Hero.MainHero.MapFaction && army.Parties.IndexOf(MobileParty.MainParty) < 0)
				{
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ArmyDispersionMapNotification(army, reason, armyDispersionLogEntry.GetEncyclopediaText()));
				}
			}
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0011A698 File Offset: 0x00118898
		private void OnArmyGathered(Army army, IMapPoint gatheringPoint)
		{
			LogEntry.AddLogEntry(new GatherArmyLogEntry(army, gatheringPoint));
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0011A6A8 File Offset: 0x001188A8
		private void OnArmyCreated(Army army)
		{
			ArmyCreationLogEntry armyCreationLogEntry = new ArmyCreationLogEntry(army);
			LogEntry.AddLogEntry(armyCreationLogEntry);
			if (army.LeaderParty.MapFaction == MobileParty.MainParty.MapFaction && army.LeaderParty != MobileParty.MainParty)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ArmyCreationMapNotification(army, armyCreationLogEntry.GetEncyclopediaText()));
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0011A701 File Offset: 0x00118901
		private void OnTradeAgreementSigned(Kingdom kingdom1, Kingdom kingdom2)
		{
			LogEntry.AddLogEntry(new TradeAgreementLogEntry(kingdom1, kingdom2));
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0011A710 File Offset: 0x00118910
		private void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			RebellionStartedLogEntry rebellionStartedLogEntry = new RebellionStartedLogEntry(settlement, oldOwnerClan);
			LogEntry.AddLogEntry(rebellionStartedLogEntry);
			if (oldOwnerClan == Clan.PlayerClan)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementRebellionMapNotification(settlement, rebellionStartedLogEntry.GetNotificationText()));
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0011A750 File Offset: 0x00118950
		private void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			LogEntry.AddLogEntry(new KingdomDecisionAddedLogEntry(decision, isPlayerInvolved));
			if (decision.NotifyPlayer && isPlayerInvolved && !decision.IsEnforced)
			{
				TextObject textObject = (decision.DetermineChooser().Leader.IsHumanPlayerCharacter ? decision.GetChooseTitle() : decision.GetSupportTitle());
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new KingdomDecisionMapNotification(decision.Kingdom, decision, textObject));
			}
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0011A7B8 File Offset: 0x001189B8
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			KingdomDecisionConcludedLogEntry kingdomDecisionConcludedLogEntry = new KingdomDecisionConcludedLogEntry(decision, chosenOutcome, isPlayerInvolved);
			LogEntry.AddLogEntry(kingdomDecisionConcludedLogEntry);
			if (decision.Kingdom == Hero.MainHero.MapFaction && decision.NotifyPlayer && !decision.IsEnforced && !isPlayerInvolved)
			{
				MBInformationManager.AddQuickInformation(kingdomDecisionConcludedLogEntry.GetNotificationText(), 0, null, null, "event:/ui/notification/kingdom_decision");
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new KingdomDecisionMapNotification(decision.Kingdom, decision, kingdomDecisionConcludedLogEntry.GetNotificationText()));
			}
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0011A82D File Offset: 0x00118A2D
		private void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			if (Campaign.Current.GameStarted)
			{
				LogEntry.AddLogEntry(new ChangeAlleyOwnerLogEntry(alley, newOwner, oldOwner));
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0011A848 File Offset: 0x00118A48
		private void OnHeroesMarried(Hero marriedHero, Hero marriedTo, bool showNotification)
		{
			CharacterMarriedLogEntry characterMarriedLogEntry = new CharacterMarriedLogEntry(marriedHero, marriedTo);
			LogEntry.AddLogEntry(characterMarriedLogEntry);
			if (marriedHero.Clan == Clan.PlayerClan || marriedTo.Clan == Clan.PlayerClan)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MarriageMapNotification(marriedHero, marriedTo, characterMarriedLogEntry.GetEncyclopediaText(), CampaignTime.Now));
			}
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0011A8A0 File Offset: 0x00118AA0
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			BesiegeSettlementLogEntry besiegeSettlementLogEntry = new BesiegeSettlementLogEntry(siegeEvent.BesiegerCamp.LeaderParty, siegeEvent.BesiegedSettlement);
			LogEntry.AddLogEntry(besiegeSettlementLogEntry);
			if (siegeEvent.BesiegedSettlement.OwnerClan == Clan.PlayerClan)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementUnderSiegeMapNotification(siegeEvent, besiegeSettlementLogEntry.GetEncyclopediaText()));
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0011A8F7 File Offset: 0x00118AF7
		private void OnTournamentFinished(CharacterObject character, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			if (character.IsHero)
			{
				LogEntry.AddLogEntry(new TournamentWonLogEntry(character.HeroObject, town, participants));
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0011A914 File Offset: 0x00118B14
		private void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
			int traitLevel = Hero.MainHero.GetTraitLevel(trait);
			TextObject traitChangedText = DefaultLogsCampaignBehavior.GetTraitChangedText(trait, traitLevel, previousLevel);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new TraitChangedMapNotification(trait, traitLevel != 0, previousLevel, traitChangedText));
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0011A951 File Offset: 0x00118B51
		private void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			LogEntry.AddLogEntry(new EndCallToWarAgreementLogEntry(callingKingdom, calledKingdom, kingdomToCallToWarAgainst));
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0011A960 File Offset: 0x00118B60
		private void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			LogEntry.AddLogEntry(new StartCallToWarAgreementLogEntry(callingKingdom, calledKingdom, kingdomToCallToWarAgainst));
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0011A96F File Offset: 0x00118B6F
		private void OnAllianceEndedEvent(Kingdom kingdom1, Kingdom kingdom2)
		{
			LogEntry.AddLogEntry(new EndAllianceLogEntry(kingdom1, kingdom2));
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0011A97D File Offset: 0x00118B7D
		private void OnAllianceStartedEvent(Kingdom kingdom1, Kingdom kingdom2)
		{
			LogEntry.AddLogEntry(new StartAllianceLogEntry(kingdom1, kingdom2));
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0011A98C File Offset: 0x00118B8C
		private static TextObject GetTraitChangedText(TraitObject traitObject, int level, int previousLevel)
		{
			TextObject textObject;
			TextObject textObject2;
			if (level != 0)
			{
				textObject = GameTexts.FindText("str_trait_name_" + traitObject.StringId.ToLower(), (level + MathF.Abs(traitObject.MinValue)).ToString());
				textObject2 = GameTexts.FindText("str_trait_gained_text", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_trait_name_" + traitObject.StringId.ToLower(), (previousLevel + MathF.Abs(traitObject.MinValue)).ToString());
				textObject2 = GameTexts.FindText("str_trait_lost_text", null);
			}
			textObject2.SetCharacterProperties("HERO", Hero.MainHero.CharacterObject, false);
			textObject2.SetTextVariable("TRAIT_NAME", textObject);
			return textObject2;
		}
	}
}
