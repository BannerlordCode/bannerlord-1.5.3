using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003F RID: 63
	public class CampaignEventDispatcher : CampaignEventReceiver
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001F86D File Offset: 0x0001DA6D
		public static CampaignEventDispatcher Instance
		{
			get
			{
				Campaign campaign = Campaign.Current;
				if (campaign == null)
				{
					return null;
				}
				return campaign.CampaignEventDispatcher;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001F87F File Offset: 0x0001DA7F
		internal CampaignEventDispatcher(IEnumerable<CampaignEventReceiver> eventReceivers)
		{
			this._eventReceivers = eventReceivers.ToArray<CampaignEventReceiver>();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001F894 File Offset: 0x0001DA94
		internal void AddCampaignEventReceiver(CampaignEventReceiver receiver)
		{
			CampaignEventReceiver[] array = new CampaignEventReceiver[this._eventReceivers.Length + 1];
			for (int i = 0; i < this._eventReceivers.Length; i++)
			{
				array[i] = this._eventReceivers[i];
			}
			array[this._eventReceivers.Length] = receiver;
			this._eventReceivers = array;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001F8E4 File Offset: 0x0001DAE4
		public override void RemoveListeners(object o)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].RemoveListeners(o);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001F910 File Offset: 0x0001DB10
		public override void OnPlayerBodyPropertiesChanged()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerBodyPropertiesChanged();
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001F93C File Offset: 0x0001DB3C
		public override void OnHeroLevelledUp(Hero hero, bool shouldNotify = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroLevelledUp(hero, shouldNotify);
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001F968 File Offset: 0x0001DB68
		public override void OnHomeHideoutChanged(BanditPartyComponent banditPartyComponent, Hideout oldHomeHideout)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHomeHideoutChanged(banditPartyComponent, oldHomeHideout);
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001F994 File Offset: 0x0001DB94
		public override void OnCharacterCreationIsOver()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterCreationIsOver();
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
		public override void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroGainedSkill(hero, skill, change, shouldNotify);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001F9F0 File Offset: 0x0001DBF0
		public override void OnHeroWounded(Hero woundedHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroWounded(woundedHero);
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		public override void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroRelationChanged(effectiveHero, effectiveHeroGainedRelationWith, relationChange, showNotification, detail, originalHero, originalGainedRelationWith);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001FA54 File Offset: 0x0001DC54
		public override void OnLootDistributedToParty(PartyBase winnerParty, PartyBase defeatedParty, ItemRoster lootedItems)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnLootDistributedToParty(winnerParty, defeatedParty, lootedItems);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001FA84 File Offset: 0x0001DC84
		public override void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroOccupationChanged(hero, oldOccupation);
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001FAB0 File Offset: 0x0001DCB0
		public override void OnBarterAccepted(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBarterAccepted(offererHero, otherHero, barters);
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001FAE0 File Offset: 0x0001DCE0
		public override void OnBarterCanceled(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBarterCanceled(offererHero, otherHero, barters);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001FB10 File Offset: 0x0001DD10
		public override void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroCreated(hero, isBornNaturally);
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001FB3C File Offset: 0x0001DD3C
		public override void OnHeroActivated(Hero hero, Hero.CharacterStates previousState)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroActivated(hero, previousState);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001FB68 File Offset: 0x0001DD68
		public override void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnQuestLogAdded(quest, hideInformation);
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001FB94 File Offset: 0x0001DD94
		public override void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIssueLogAdded(issue, hideInformation);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		public override void OnClanTierChanged(Clan clan, bool shouldNotify = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanTierChanged(clan, shouldNotify);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001FBEC File Offset: 0x0001DDEC
		public override void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail actionDetail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanChangedKingdom(clan, oldKingdom, newKingdom, actionDetail, showNotification);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001FC20 File Offset: 0x0001DE20
		public override void OnClanDefected(Clan clan, Kingdom oldKingdom, Kingdom newKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanDefected(clan, oldKingdom, newKingdom);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001FC50 File Offset: 0x0001DE50
		public override void OnClanCreated(Clan clan, bool isCompanion)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanCreated(clan, isCompanion);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001FC7C File Offset: 0x0001DE7C
		public override void OnHeroJoinedParty(Hero hero, MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroJoinedParty(hero, party);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
		public override void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDecisionAdded(decision, isPlayerInvolved);
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001FCD4 File Offset: 0x0001DED4
		public override void OnKingdomDecisionCancelled(KingdomDecision decision, bool isPlayerInvolved)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDecisionCancelled(decision, isPlayerInvolved);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001FD00 File Offset: 0x0001DF00
		public override void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDecisionConcluded(decision, chosenOutcome, isPlayerInvolved);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001FD30 File Offset: 0x0001DF30
		public override void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroOrPartyTradedGold(giver, recipient, goldAmount, showNotification);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001FD60 File Offset: 0x0001DF60
		public override void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroOrPartyGaveItem(giver, receiver, itemRosterElement, showNotification);
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001FD90 File Offset: 0x0001DF90
		public override void OnBanditPartyRecruited(MobileParty banditParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBanditPartyRecruited(banditParty);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001FDBC File Offset: 0x0001DFBC
		public override void OnArmyCreated(Army army)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyCreated(army);
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001FDE8 File Offset: 0x0001DFE8
		public override void OnPartyAttachedAnotherParty(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyAttachedAnotherParty(mobileParty);
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001FE14 File Offset: 0x0001E014
		public override void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNearbyPartyAddedToPlayerMapEvent(mobileParty);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001FE40 File Offset: 0x0001E040
		public override void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyDispersed(army, reason, isPlayersArmy);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001FE70 File Offset: 0x0001E070
		public override void OnArmyGathered(Army army, IMapPoint gatheringPoint)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyGathered(army, gatheringPoint);
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001FE9C File Offset: 0x0001E09C
		public override void OnPerkOpened(Hero hero, PerkObject perk)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPerkOpened(hero, perk);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001FEC8 File Offset: 0x0001E0C8
		public override void OnPerkReset(Hero hero, PerkObject perk)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPerkReset(hero, perk);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001FEF4 File Offset: 0x0001E0F4
		public override void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerTraitChanged(trait, previousLevel);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001FF20 File Offset: 0x0001E120
		public override void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageStateChanged(village, oldState, newState, raiderParty);
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001FF50 File Offset: 0x0001E150
		public override void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSettlementEntered(party, settlement, hero);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001FF80 File Offset: 0x0001E180
		public override void OnAfterSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAfterSettlementEntered(party, settlement, hero);
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001FFB0 File Offset: 0x0001E1B0
		public override void OnBeforeSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeSettlementEntered(party, settlement, hero);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001FFE0 File Offset: 0x0001E1E0
		public override void OnMercenaryTroopChangedInTown(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMercenaryTroopChangedInTown(town, oldTroopType, newTroopType);
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00020010 File Offset: 0x0001E210
		public override void OnMercenaryNumberChangedInTown(Town town, int oldNumber, int newNumber)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMercenaryNumberChangedInTown(town, oldNumber, newNumber);
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00020040 File Offset: 0x0001E240
		public override void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAlleyOccupiedByPlayer(alley, troops);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0002006C File Offset: 0x0001E26C
		public override void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAlleyOwnerChanged(alley, newOwner, oldOwner);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0002009C File Offset: 0x0001E29C
		public override void OnAlleyClearedByPlayer(Alley alley)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAlleyClearedByPlayer(alley);
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000200C8 File Offset: 0x0001E2C8
		public override void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum romanceLevel)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRomanticStateChanged(hero1, hero2, romanceLevel);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000200F8 File Offset: 0x0001E2F8
		public override void OnBeforeHeroesMarried(Hero hero1, Hero hero2, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeHeroesMarried(hero1, hero2, showNotification);
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00020128 File Offset: 0x0001E328
		public override void OnPlayerEliminatedFromTournament(int round, Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerEliminatedFromTournament(round, town);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00020154 File Offset: 0x0001E354
		public override void OnPlayerStartedTournamentMatch(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerStartedTournamentMatch(town);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00020180 File Offset: 0x0001E380
		public override void OnTournamentStarted(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTournamentStarted(town);
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000201AC File Offset: 0x0001E3AC
		public override void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTournamentFinished(winner, participants, town, prize);
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000201DC File Offset: 0x0001E3DC
		public override void OnTournamentCancelled(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTournamentCancelled(town);
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00020208 File Offset: 0x0001E408
		public override void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWarDeclared(faction1, faction2, declareWarDetail);
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00020238 File Offset: 0x0001E438
		public override void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRulingClanChanged(kingdom, oldRulingClan);
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00020264 File Offset: 0x0001E464
		public override void OnStartBattle(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnStartBattle(attackerParty, defenderParty, subject, showNotification);
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00020294 File Offset: 0x0001E494
		public override void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRebellionFinished(settlement, oldOwnerClan);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000202C0 File Offset: 0x0001E4C0
		public override void TownRebelliousStateChanged(Town town, bool rebelliousState)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TownRebelliousStateChanged(town, rebelliousState);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x000202EC File Offset: 0x0001E4EC
		public override void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan rebelliousClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRebelliousClanDisbandedAtSettlement(settlement, rebelliousClan);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00020318 File Offset: 0x0001E518
		public override void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemsLooted(mobileParty, items);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00020344 File Offset: 0x0001E544
		public override void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyDestroyed(mobileParty, destroyerParty);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00020370 File Offset: 0x0001E570
		public override void OnMobilePartyCreated(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyCreated(party);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0002039C File Offset: 0x0001E59C
		public override void OnMapInteractableCreated(IInteractablePoint interactable)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapInteractableCreated(interactable);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000203C8 File Offset: 0x0001E5C8
		public override void OnMapInteractableDestroyed(IInteractablePoint interactable)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapInteractableDestroyed(interactable);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000203F4 File Offset: 0x0001E5F4
		public override void OnMobilePartyQuestStatusChanged(MobileParty party, bool isUsedByQuest)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyQuestStatusChanged(party, isUsedByQuest);
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00020420 File Offset: 0x0001E620
		public override void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroKilled(victim, killer, detail, showNotification);
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00020450 File Offset: 0x0001E650
		public override void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeHeroKilled(victim, killer, detail, showNotification);
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00020480 File Offset: 0x0001E680
		public override void OnChildEducationCompleted(Hero hero, int age)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnChildEducationCompleted(hero, age);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000204AC File Offset: 0x0001E6AC
		public override void OnHeroComesOfAge(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroComesOfAge(hero);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000204D8 File Offset: 0x0001E6D8
		public override void OnHeroReachesTeenAge(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroReachesTeenAge(hero);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00020504 File Offset: 0x0001E704
		public override void OnHeroGrowsOutOfInfancy(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroGrowsOutOfInfancy(hero);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00020530 File Offset: 0x0001E730
		public override void OnCharacterDefeated(Hero winner, Hero loser)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterDefeated(winner, loser);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0002055C File Offset: 0x0001E75C
		public override void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroPrisonerTaken(capturer, prisoner);
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00020588 File Offset: 0x0001E788
		public override void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroPrisonerReleased(prisoner, party, capturerFaction, detail, showNotification);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000205BC File Offset: 0x0001E7BC
		public override void OnCharacterBecameFugitive(Hero hero, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterBecameFugitive(hero, showNotification);
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000205E8 File Offset: 0x0001E7E8
		public override void OnPlayerLearnsAboutHero(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerLearnsAboutHero(hero);
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00020614 File Offset: 0x0001E814
		public override void OnPlayerMetHero(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerMetHero(hero);
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00020640 File Offset: 0x0001E840
		public override void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRenownGained(hero, gainedRenown, doNotNotify);
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00020670 File Offset: 0x0001E870
		public override void OnCrimeRatingChanged(IFaction kingdom, float deltaCrimeAmount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCrimeRatingChanged(kingdom, deltaCrimeAmount);
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0002069C File Offset: 0x0001E89C
		public override void OnNewCompanionAdded(Hero newCompanion)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewCompanionAdded(newCompanion);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000206C8 File Offset: 0x0001E8C8
		public override void OnAfterMissionStarted(IMission iMission)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAfterMissionStarted(iMission);
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000206F4 File Offset: 0x0001E8F4
		public override void OnGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameMenuOpened(args);
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00020720 File Offset: 0x0001E920
		public override void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMakePeace(side1Faction, side2Faction, detail);
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00020750 File Offset: 0x0001E950
		public override void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDestroyed(destroyedKingdom);
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0002077C File Offset: 0x0001E97C
		public override void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanKingdomBeDiscontinued(kingdom, ref result);
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000207A8 File Offset: 0x0001E9A8
		public override void OnKingdomCreated(Kingdom createdKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomCreated(createdKingdom);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000207D4 File Offset: 0x0001E9D4
		public override void OnVillageBecomeNormal(Village village)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageBecomeNormal(village);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00020800 File Offset: 0x0001EA00
		public override void OnVillageBeingRaided(Village village)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageBeingRaided(village);
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0002082C File Offset: 0x0001EA2C
		public override void OnVillageLooted(Village village)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageLooted(village);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00020858 File Offset: 0x0001EA58
		public override void OnConversationEnded(IEnumerable<CharacterObject> characters)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnConversationEnded(characters);
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00020884 File Offset: 0x0001EA84
		public override void OnAgentJoinedConversation(IAgent agent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAgentJoinedConversation(agent);
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000208B0 File Offset: 0x0001EAB0
		public override void OnMapEventEnded(MapEvent mapEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapEventEnded(mapEvent);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000208DC File Offset: 0x0001EADC
		public override void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapEventStarted(mapEvent, attackerParty, defenderParty);
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0002090C File Offset: 0x0001EB0C
		public override void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonersChangeInSettlement(settlement, prisonerRoster, prisonerHero, takenFromDungeon);
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0002093C File Offset: 0x0001EB3C
		public override void OnMissionStarted(IMission mission)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMissionStarted(mission);
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00020968 File Offset: 0x0001EB68
		public override void OnPlayerBoardGameOver(Hero opposingHero, BoardGameHelper.BoardGameState state)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerBoardGameOver(opposingHero, state);
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00020994 File Offset: 0x0001EB94
		public override void OnRansomOfferedToPlayer(Hero captiveHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRansomOfferedToPlayer(captiveHero);
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000209C0 File Offset: 0x0001EBC0
		public override void OnRansomOfferCancelled(Hero captiveHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRansomOfferCancelled(captiveHero);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000209EC File Offset: 0x0001EBEC
		public override void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount, int tributeDurationInDays)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPeaceOfferedToPlayer(opponentFaction, tributeAmount, tributeDurationInDays);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00020A1C File Offset: 0x0001EC1C
		public override void OnTradeAgreementSigned(Kingdom kingdom, Kingdom other)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTradeAgreementSigned(kingdom, other);
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00020A48 File Offset: 0x0001EC48
		public override void OnPeaceOfferResolved(IFaction opponentFaction)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPeaceOfferResolved(opponentFaction);
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00020A74 File Offset: 0x0001EC74
		public override void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMarriageOfferedToPlayer(suitor, maiden);
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00020AA0 File Offset: 0x0001ECA0
		public override void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMarriageOfferCanceled(suitor, maiden);
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00020ACC File Offset: 0x0001ECCC
		public override void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom offeredKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVassalOrMercenaryServiceOfferedToPlayer(offeredKingdom);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00020AF8 File Offset: 0x0001ECF8
		public override void OnCommonAreaStateChanged(Alley alley, Alley.AreaState oldState, Alley.AreaState newState)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCommonAreaStateChanged(alley, oldState, newState);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00020B28 File Offset: 0x0001ED28
		public override void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVassalOrMercenaryServiceOfferCanceled(offeredKingdom);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00020B54 File Offset: 0x0001ED54
		public override void BeforeMissionOpened()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].BeforeMissionOpened();
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00020B80 File Offset: 0x0001ED80
		public override void OnPartyRemoved(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyRemoved(party);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00020BAC File Offset: 0x0001EDAC
		public override void OnPartySizeChanged(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartySizeChanged(party);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00020BD8 File Offset: 0x0001EDD8
		public override void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSettlementOwnerChanged(settlement, openToClaim, newOwner, oldOwner, capturerHero, detail);
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00020C0C File Offset: 0x0001EE0C
		public override void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGovernorChanged(fortification, oldGovernor, newGovernor);
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00020C3C File Offset: 0x0001EE3C
		public override void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSettlementLeft(party, settlement);
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00020C68 File Offset: 0x0001EE68
		public override void Tick(float dt)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].Tick(dt);
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00020C94 File Offset: 0x0001EE94
		public override void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSessionStart(campaignGameStarter);
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00020CC0 File Offset: 0x0001EEC0
		public override void OnAfterSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAfterSessionStart(campaignGameStarter);
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00020CEC File Offset: 0x0001EEEC
		public override void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewGameCreated(campaignGameStarter);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00020D18 File Offset: 0x0001EF18
		public override void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameEarlyLoaded(campaignGameStarter);
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00020D44 File Offset: 0x0001EF44
		public override void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameLoaded(campaignGameStarter);
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00020D70 File Offset: 0x0001EF70
		public override void OnGameLoadFinished()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameLoadFinished();
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00020D9C File Offset: 0x0001EF9C
		public override void OnPartyJoinedArmy(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyJoinedArmy(mobileParty);
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00020DC8 File Offset: 0x0001EFC8
		public override void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyRemovedFromArmy(mobileParty);
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00020DF4 File Offset: 0x0001EFF4
		public override void OnPlayerArmyLeaderChangedBehavior()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerArmyLeaderChangedBehavior();
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00020E20 File Offset: 0x0001F020
		public override void OnArmyOverlaySetDirty()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyOverlaySetDirty();
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00020E4C File Offset: 0x0001F04C
		public override void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerDesertedBattle(sacrificedMenCount);
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00020E78 File Offset: 0x0001F078
		public override void MissionTick(float dt)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].MissionTick(dt);
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00020EA4 File Offset: 0x0001F0A4
		public override void OnChildConceived(Hero mother)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnChildConceived(mother);
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00020ED0 File Offset: 0x0001F0D0
		public override void OnGivenBirth(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGivenBirth(mother, aliveChildren, stillbornCount);
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00020F00 File Offset: 0x0001F100
		public override void OnUnitRecruited(CharacterObject character, int amount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnUnitRecruited(character, amount);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00020F2C File Offset: 0x0001F12C
		public override void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerBattleEnd(mapEvent);
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00020F58 File Offset: 0x0001F158
		public override void OnMissionEnded(IMission mission)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMissionEnded(mission);
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00020F84 File Offset: 0x0001F184
		public override void TickPartialHourlyAi(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TickPartialHourlyAi(party);
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00020FB0 File Offset: 0x0001F1B0
		public override void QuarterDailyPartyTick(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].QuarterDailyPartyTick(party);
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00020FDC File Offset: 0x0001F1DC
		public override void AiHourlyTick(MobileParty party, PartyThinkParams partyThinkParams)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].AiHourlyTick(party, partyThinkParams);
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00021008 File Offset: 0x0001F208
		public override void HourlyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTick();
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00021034 File Offset: 0x0001F234
		public override void QuarterHourlyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].QuarterHourlyTick();
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00021060 File Offset: 0x0001F260
		public override void HourlyTickParty(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTickParty(mobileParty);
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0002108C File Offset: 0x0001F28C
		public override void HourlyTickSettlement(Settlement settlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTickSettlement(settlement);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000210B8 File Offset: 0x0001F2B8
		public override void HourlyTickClan(Clan clan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTickClan(clan);
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000210E4 File Offset: 0x0001F2E4
		public override void DailyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTick();
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00021110 File Offset: 0x0001F310
		public override void DailyTickParty(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickParty(mobileParty);
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0002113C File Offset: 0x0001F33C
		public override void DailyTickTown(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickTown(town);
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00021168 File Offset: 0x0001F368
		public override void DailyTickSettlement(Settlement settlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickSettlement(settlement);
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00021194 File Offset: 0x0001F394
		public override void DailyTickHero(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickHero(hero);
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000211C0 File Offset: 0x0001F3C0
		public override void DailyTickClan(Clan clan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickClan(clan);
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000211EC File Offset: 0x0001F3EC
		public override void WeeklyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].WeeklyTick();
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00021218 File Offset: 0x0001F418
		public override void CollectAvailableTutorials(ref List<CampaignTutorial> tutorials)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CollectAvailableTutorials(ref tutorials);
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00021244 File Offset: 0x0001F444
		public override void OnTutorialCompleted(string tutorial)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTutorialCompleted(tutorial);
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00021270 File Offset: 0x0001F470
		public override void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].BeforeGameMenuOpened(args);
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0002129C File Offset: 0x0001F49C
		public override void AfterGameMenuInitialized(MenuCallbackArgs args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].AfterGameMenuInitialized(args);
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000212C8 File Offset: 0x0001F4C8
		public override void OnBarterablesRequested(BarterData args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBarterablesRequested(args);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000212F4 File Offset: 0x0001F4F4
		public override void OnPartyVisibilityChanged(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyVisibilityChanged(party);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00021320 File Offset: 0x0001F520
		public override void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCompanionRemoved(companion, detail);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0002134C File Offset: 0x0001F54C
		public override void TrackDetected(Track track)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TrackDetected(track);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00021378 File Offset: 0x0001F578
		public override void TrackLost(Track track)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TrackLost(track);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000213A4 File Offset: 0x0001F5A4
		public override void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].LocationCharactersAreReadyToSpawn(unusedUsablePointCount);
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000213D0 File Offset: 0x0001F5D0
		public override void LocationCharactersSimulated()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].LocationCharactersSimulated();
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000213FC File Offset: 0x0001F5FC
		public override void OnBeforePlayerAgentSpawn(ref MatrixFrame spawnFrame)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforePlayerAgentSpawn(ref spawnFrame);
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00021428 File Offset: 0x0001F628
		public override void OnPlayerAgentSpawned()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerAgentSpawned();
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00021454 File Offset: 0x0001F654
		public override void OnPlayerUpgradedTroops(CharacterObject upgradeFromTroop, CharacterObject upgradeToTroop, int number)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerUpgradedTroops(upgradeFromTroop, upgradeToTroop, number);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00021484 File Offset: 0x0001F684
		public override void OnHeroCombatHit(CharacterObject attackerTroop, CharacterObject attackedTroop, PartyBase party, WeaponComponentData usedWeapon, bool isFatal, int xp)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroCombatHit(attackerTroop, attackedTroop, party, usedWeapon, isFatal, xp);
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000214B8 File Offset: 0x0001F6B8
		public override void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterPortraitPopUpOpened(character);
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000214E4 File Offset: 0x0001F6E4
		public override void OnCharacterPortraitPopUpClosed()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterPortraitPopUpClosed();
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00021510 File Offset: 0x0001F710
		public override void OnPlayerStartTalkFromMenu(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerStartTalkFromMenu(hero);
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0002153C File Offset: 0x0001F73C
		public override void OnGameMenuOptionSelected(GameMenu gameMenu, GameMenuOption gameMenuOption)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameMenuOptionSelected(gameMenu, gameMenuOption);
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00021568 File Offset: 0x0001F768
		public override void OnPlayerStartRecruitment(CharacterObject recruitTroopCharacter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerStartRecruitment(recruitTroopCharacter);
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00021594 File Offset: 0x0001F794
		public override void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforePlayerCharacterChanged(oldPlayer, newPlayer);
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000215C0 File Offset: 0x0001F7C0
		public override void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newPlayerParty, bool isMainPartyChanged)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerCharacterChanged(oldPlayer, newPlayer, newPlayerParty, isMainPartyChanged);
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000215F0 File Offset: 0x0001F7F0
		public override void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanLeaderChanged(oldLeader, newLeader);
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0002161C File Offset: 0x0001F81C
		public override void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeEventStarted(siegeEvent);
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00021648 File Offset: 0x0001F848
		public override void OnPlayerSiegeStarted()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerSiegeStarted();
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00021674 File Offset: 0x0001F874
		public override void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeEventEnded(siegeEvent);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000216A0 File Offset: 0x0001F8A0
		public override void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeAftermathApplied(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000216D4 File Offset: 0x0001F8D4
		public override void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeBombardmentHit(besiegerParty, besiegedSettlement, side, weapon, target);
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00021708 File Offset: 0x0001F908
		public override void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeBombardmentWallHit(besiegerParty, besiegedSettlement, side, weapon, isWallCracked);
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0002173C File Offset: 0x0001F93C
		public override void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeEngineDestroyed(besiegerParty, besiegedSettlement, side, destroyedEngine);
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0002176C File Offset: 0x0001F96C
		public override void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTradeRumorIsTaken(newRumors, sourceSettlement);
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00021798 File Offset: 0x0001F998
		public override void OnCheckForIssue(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCheckForIssue(hero);
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000217C4 File Offset: 0x0001F9C4
		public override void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIssueUpdated(issue, details, issueSolver);
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000217F4 File Offset: 0x0001F9F4
		public override void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTroopsDeserted(mobileParty, desertedTroops);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00021820 File Offset: 0x0001FA20
		public override void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTroopRecruited(recruiterHero, recruitmentSettlement, recruitmentSource, troop, amount);
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00021854 File Offset: 0x0001FA54
		public override void OnTroopGivenToSettlement(Hero giverHero, Settlement recipientSettlement, TroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTroopGivenToSettlement(giverHero, recipientSettlement, roster);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00021884 File Offset: 0x0001FA84
		public override void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement itemRosterElement, int number, Settlement currentSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemSold(receiverParty, payerParty, itemRosterElement, number, currentSettlement);
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000218B8 File Offset: 0x0001FAB8
		public override void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> itemRosterElements)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCaravanTransactionCompleted(caravanParty, town, itemRosterElements);
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000218E8 File Offset: 0x0001FAE8
		public override void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerSold(sellerParty, buyerParty, prisoners);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00021918 File Offset: 0x0001FB18
		public override void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyDisbanded(disbandParty, relatedSettlement);
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00021944 File Offset: 0x0001FB44
		public override void OnPartyDisbandStarted(MobileParty disbandParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyDisbandStarted(disbandParty);
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00021970 File Offset: 0x0001FB70
		public override void OnPartyDisbandCanceled(MobileParty disbandParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyDisbandCanceled(disbandParty);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0002199C File Offset: 0x0001FB9C
		public override void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBuildingLevelChanged(town, building, levelChange);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000219CC File Offset: 0x0001FBCC
		public override void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHideoutSpotted(party, hideoutParty);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000219F8 File Offset: 0x0001FBF8
		public override void OnHideoutDeactivated(Settlement hideout)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHideoutDeactivated(hideout);
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00021A24 File Offset: 0x0001FC24
		public override void OnHeroSharedFoodWithAnother(Hero supporterHero, Hero supportedHero, float influence)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroSharedFoodWithAnother(supporterHero, supportedHero, influence);
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00021A54 File Offset: 0x0001FC54
		public override void OnItemsDiscardedByPlayer(ItemRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemsDiscardedByPlayer(roster);
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00021A80 File Offset: 0x0001FC80
		public override void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerInventoryExchange(purchasedItems, soldItems, isTrading);
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00021AB0 File Offset: 0x0001FCB0
		public override void OnPersuasionProgressCommitted(Tuple<PersuasionOptionArgs, PersuasionOptionResult> progress)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPersuasionProgressCommitted(progress);
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00021ADC File Offset: 0x0001FCDC
		public override void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnQuestCompleted(quest, detail);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00021B08 File Offset: 0x0001FD08
		public override void OnQuestStarted(QuestBase quest)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnQuestStarted(quest);
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00021B34 File Offset: 0x0001FD34
		public override void OnItemProduced(ItemObject itemObject, Settlement settlement, int count)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemProduced(itemObject, settlement, count);
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00021B64 File Offset: 0x0001FD64
		public override void OnItemConsumed(ItemObject itemObject, Settlement settlement, int count)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemConsumed(itemObject, settlement, count);
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00021B94 File Offset: 0x0001FD94
		public override void OnPartyConsumedFood(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyConsumedFood(party);
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00021BC0 File Offset: 0x0001FDC0
		public override void OnNewIssueCreated(IssueBase issue)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewIssueCreated(issue);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00021BEC File Offset: 0x0001FDEC
		public override void OnIssueOwnerChanged(IssueBase issue, Hero oldOwner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIssueOwnerChanged(issue, oldOwner);
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00021C18 File Offset: 0x0001FE18
		public override void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeMainCharacterDied(victim, killer, detail, showNotification);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00021C48 File Offset: 0x0001FE48
		public override void OnGameOver()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameOver();
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00021C74 File Offset: 0x0001FE74
		public override void SiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].SiegeCompleted(siegeSettlement, attackerParty, isWin, battleType);
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00021CA4 File Offset: 0x0001FEA4
		public override void AfterSiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].AfterSiegeCompleted(siegeSettlement, attackerParty, isWin, battleType);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00021CD4 File Offset: 0x0001FED4
		public override void SiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].SiegeEngineBuilt(siegeEvent, side, siegeEngine);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00021D04 File Offset: 0x0001FF04
		public override void RaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].RaidCompleted(winnerSide, raidEvent);
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00021D30 File Offset: 0x0001FF30
		public override void ForceSuppliesCompleted(BattleSideEnum winnerSide, ForceSuppliesEventComponent forceSuppliesEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].ForceSuppliesCompleted(winnerSide, forceSuppliesEvent);
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00021D5C File Offset: 0x0001FF5C
		public override void ForceVolunteersCompleted(BattleSideEnum winnerSide, ForceVolunteersEventComponent forceVolunteersEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].ForceVolunteersCompleted(winnerSide, forceVolunteersEvent);
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00021D88 File Offset: 0x0001FF88
		public override void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHideoutBattleCompleted(winnerSide, hideoutEventComponent, battleEndState);
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00021DB8 File Offset: 0x0001FFB8
		public override void OnClanDestroyed(Clan destroyedClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanDestroyed(destroyedClan);
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00021DE4 File Offset: 0x0001FFE4
		public override void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewItemCrafted(itemObject, overriddenItemModifier, isCraftingOrderItem);
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00021E14 File Offset: 0x00020014
		public override void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWorkshopOwnerChanged(workshop, oldOwner);
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00021E40 File Offset: 0x00020040
		public override void OnWorkshopInitialized(Workshop workshop)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWorkshopInitialized(workshop);
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00021E6C File Offset: 0x0002006C
		public override void OnWorkshopTypeChanged(Workshop workshop)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWorkshopTypeChanged(workshop);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00021E98 File Offset: 0x00020098
		public override void OnMainPartyPrisonerRecruited(FlattenedTroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMainPartyPrisonerRecruited(roster);
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00021EC4 File Offset: 0x000200C4
		public override void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerDonatedToSettlement(donatingParty, donatedPrisoners, donatedSettlement);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00021EF4 File Offset: 0x000200F4
		public override void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement equipmentElement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnEquipmentSmeltedByHero(hero, equipmentElement);
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00021F20 File Offset: 0x00020120
		public override void OnPrisonerTaken(FlattenedTroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerTaken(roster);
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00021F4C File Offset: 0x0002014C
		public override void OnBeforeSave()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeSave();
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00021F78 File Offset: 0x00020178
		public override void OnSaveStarted()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSaveStarted();
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00021FA4 File Offset: 0x000201A4
		public override void OnSaveOver(bool isSuccessful, string saveName)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSaveOver(isSuccessful, saveName);
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00021FD0 File Offset: 0x000201D0
		public override void CollectMetadataEntries(List<KeyValuePair<string, string>> pairs)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CollectMetadataEntries(pairs);
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00021FFC File Offset: 0x000201FC
		public override void OnPrisonerReleased(FlattenedTroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerReleased(roster);
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00022028 File Offset: 0x00020228
		public override void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroChangedClan(hero, oldClan);
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00022054 File Offset: 0x00020254
		public override void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroGetsBusy(hero, heroGetsBusyReason);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00022080 File Offset: 0x00020280
		public override void OnPlayerTradeProfit(int profit)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerTradeProfit(profit);
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000220AC File Offset: 0x000202AC
		public override void CraftingPartUnlocked(CraftingPiece craftingPiece)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CraftingPartUnlocked(craftingPiece);
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000220D8 File Offset: 0x000202D8
		public override void OnClanEarnedGoldFromTribute(Clan receiverClan, IFaction payingFaction)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanEarnedGoldFromTribute(receiverClan, payingFaction);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00022104 File Offset: 0x00020304
		public override void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCollectLootItems(winnerParty, gainedLoots);
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00022130 File Offset: 0x00020330
		public override void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroTeleportationRequested(hero, targetSettlement, targetParty, detail);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00022160 File Offset: 0x00020360
		public override void OnClanInfluenceChanged(Clan clan, float change)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanInfluenceChanged(clan, change);
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0002218C File Offset: 0x0002038C
		public override void OnPlayerPartyKnockedOrKilledTroop(CharacterObject strikedTroop)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerPartyKnockedOrKilledTroop(strikedTroop);
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000221B8 File Offset: 0x000203B8
		public override void OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType incomeType, int incomeAmount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerEarnedGoldFromAsset(incomeType, incomeAmount);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000221E4 File Offset: 0x000203E4
		public override void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyLeaderChangeOfferCanceled(party);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00022210 File Offset: 0x00020410
		public override void OnPartyLeaderChanged(MobileParty mobileParty, Hero oldLeader)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyLeaderChanged(mobileParty, oldLeader);
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0002223C File Offset: 0x0002043C
		public override void OnMainPartyStarving()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMainPartyStarving();
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00022268 File Offset: 0x00020468
		public override void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerJoinedTournament(town, isParticipant);
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00022294 File Offset: 0x00020494
		public override void OnCraftingOrderCompleted(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCraftingOrderCompleted(town, craftingOrder, craftedItem, completerHero);
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000222C4 File Offset: 0x000204C4
		public override void OnItemsRefined(Hero hero, Crafting.RefiningFormula refineFormula)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemsRefined(hero, refineFormula);
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000222F0 File Offset: 0x000204F0
		public override void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapEventContinuityNeedsUpdate(faction);
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002231C File Offset: 0x0002051C
		public override void OnHeirSelectionRequested(Dictionary<Hero, int> heirApparents)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeirSelectionRequested(heirApparents);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00022348 File Offset: 0x00020548
		public override void OnHeirSelectionOver(Hero selectedHeir)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeirSelectionOver(selectedHeir);
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00022374 File Offset: 0x00020574
		public override void OnCharacterCreationInitialized(CharacterCreationManager characterCreationManager)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterCreationInitialized(characterCreationManager);
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000223A0 File Offset: 0x000205A0
		public override void OnShipDestroyed(PartyBase owner, Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnShipDestroyed(owner, ship, detail);
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000223D0 File Offset: 0x000205D0
		public override void OnPartyLeftArmy(MobileParty party, Army army)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyLeftArmy(party, army);
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000223FC File Offset: 0x000205FC
		public override void OnShipOwnerChanged(Ship ship, PartyBase oldOwner, ChangeShipOwnerAction.ShipOwnerChangeDetail changeDetail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnShipOwnerChanged(ship, oldOwner, changeDetail);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0002242C File Offset: 0x0002062C
		public override void CanHaveUnlockedUpgradePiece(Ship ship, ChangeShipOwnerAction.ShipOwnerChangeDetail detail, ref bool canHaveUpgradePiece)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHaveUnlockedUpgradePiece(ship, detail, ref canHaveUpgradePiece);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0002245C File Offset: 0x0002065C
		public override void OnShipRepaired(Ship ship, Settlement repairPort)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnShipRepaired(ship, repairPort);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00022488 File Offset: 0x00020688
		public override void OnFigureheadUnlocked(Figurehead figurehead)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnFigureheadUnlocked(figurehead);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000224B4 File Offset: 0x000206B4
		public override void OnPartyAddedToMapEvent(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyAddedToMapEvent(party);
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000224E0 File Offset: 0x000206E0
		public override void OnIncidentResolved(Incident incident)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIncidentResolved(incident);
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0002250C File Offset: 0x0002070C
		public override void OnMobilePartyNavigationStateChanged(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyNavigationStateChanged(mobileParty);
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00022538 File Offset: 0x00020738
		public override void OnMobilePartyJoinedToSiegeEvent(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyJoinedToSiegeEvent(mobileParty);
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00022564 File Offset: 0x00020764
		public override void OnMobilePartyLeftSiegeEvent(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyLeftSiegeEvent(mobileParty);
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00022590 File Offset: 0x00020790
		public override void OnBlockadeActivated(SiegeEvent siegeEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBlockadeActivated(siegeEvent);
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000225BC File Offset: 0x000207BC
		public override void OnBlockadeDeactivated(SiegeEvent siegeEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBlockadeDeactivated(siegeEvent);
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000225E8 File Offset: 0x000207E8
		public override void OnMercenaryServiceStarted(Clan mercenaryClan, StartMercenaryServiceAction.StartMercenaryServiceActionDetails details)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMercenaryServiceStarted(mercenaryClan, details);
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00022614 File Offset: 0x00020814
		public override void OnMercenaryServiceEnded(Clan mercenaryClan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMercenaryServiceEnded(mercenaryClan, details);
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00022640 File Offset: 0x00020840
		public override void OnAllianceStarted(Kingdom kingdom1, Kingdom kingdom2)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAllianceStarted(kingdom1, kingdom2);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0002266C File Offset: 0x0002086C
		public override void OnAllianceEnded(Kingdom kingdom1, Kingdom kingdom2)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAllianceEnded(kingdom1, kingdom2);
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00022698 File Offset: 0x00020898
		public override void OnCallToWarAgreementStarted(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCallToWarAgreementStarted(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000226C8 File Offset: 0x000208C8
		public override void OnCallToWarAgreementEnded(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCallToWarAgreementEnded(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000226F8 File Offset: 0x000208F8
		public override void OnPartyEncounter(PartyBase attacker, PartyBase defender)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyEncounter(attacker, defender);
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00022724 File Offset: 0x00020924
		public override void OnBloodFeudStateChanged(Clan clan, Hero executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBloodFeudStateChanged(clan, executedHero, detail);
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00022754 File Offset: 0x00020954
		public override void OnDeathMarkAdded(Hero hero, Hero killerHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnDeathMarkAdded(hero, killerHero);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00022780 File Offset: 0x00020980
		public override void CanHeroLeadParty(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroLeadParty(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000227B4 File Offset: 0x000209B4
		public override void CanHeroMarry(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroMarry(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000227E8 File Offset: 0x000209E8
		public override void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroEquipmentBeChanged(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0002281C File Offset: 0x00020A1C
		public override void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanBeGovernorOrHavePartyRole(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00022850 File Offset: 0x00020A50
		public override void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroDie(hero, causeOfDeath, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00022884 File Offset: 0x00020A84
		public override void CanHeroBeReleased(Hero prisoner, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroBeReleased(prisoner, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000228B8 File Offset: 0x00020AB8
		public override void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroBecomePrisoner(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000228EC File Offset: 0x00020AEC
		public override void CanPlayerMeetWithHeroAfterConversation(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanPlayerMeetWithHeroAfterConversation(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00022920 File Offset: 0x00020B20
		public override void CanMoveToSettlement(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanMoveToSettlement(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00022954 File Offset: 0x00020B54
		public override void CanHaveCampaignIssues(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHaveCampaignIssues(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00022988 File Offset: 0x00020B88
		public override void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].IsSettlementBusy(settlement, asker, ref priority);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000229B8 File Offset: 0x00020BB8
		public override void OnHeroUnregistered(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroUnregistered(hero);
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000229E4 File Offset: 0x00020BE4
		public override void OnShipCreated(Ship ship, Settlement createdSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnShipCreated(ship, createdSettlement);
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00022A10 File Offset: 0x00020C10
		public override void OnConfigChanged()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnConfigChanged();
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00022A3C File Offset: 0x00020C3C
		public override void OnMobilePartyRaftStateChanged(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyRaftStateChanged(mobileParty);
			}
		}

		// Token: 0x0400018F RID: 399
		private CampaignEventReceiver[] _eventReceivers;
	}
}
