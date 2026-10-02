using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000416 RID: 1046
	public class HeroKnownInformationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600429F RID: 17055 RVA: 0x00130240 File Offset: 0x0012E440
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnDailyTickHero));
			CampaignEvents.ConversationEnded.AddNonSerializedListener(this, new Action<IEnumerable<CharacterObject>>(this.ConversationEnded));
			CampaignEvents.OnAgentJoinedConversationEvent.AddNonSerializedListener(this, new Action<IAgent>(this.OnAgentJoinedConversation));
			CampaignEvents.OnPlayerMetHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnPlayerMetHero));
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.OnHeroesMarried));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinishedEvent));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			CampaignEvents.OnPlayerLearnsAboutHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnPlayerLearnsAboutHero));
			CampaignEvents.NearbyPartyAddedToPlayerMapEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnNearbyPartyAddedToPlayerMapEvent));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuChanged));
			CampaignEvents.AfterMissionStarted.AddNonSerializedListener(this, new Action<IMission>(this.OnAfterMissionStarted));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.PartyAttachedAnotherParty.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyAttachedAnotherParty));
			CampaignEvents.OnPlayerJoinedTournamentEvent.AddNonSerializedListener(this, new Action<Town, bool>(this.OnPlayerJoinedTournament));
			CampaignEvents.OnMarriageOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferedToPlayer));
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x001303BD File Offset: 0x0012E5BD
		private void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
			if (suitor.Clan == Clan.PlayerClan)
			{
				maiden.IsKnownToPlayer = true;
				return;
			}
			suitor.IsKnownToPlayer = true;
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x001303DC File Offset: 0x0012E5DC
		private void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
			foreach (CharacterObject characterObject in Campaign.Current.TournamentManager.GetTournamentGame(town).GetParticipantCharacters(town.Settlement, false))
			{
				if (characterObject.IsHero && !characterObject.HeroObject.IsKnownToPlayer)
				{
					characterObject.HeroObject.IsKnownToPlayer = true;
				}
			}
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x00130460 File Offset: 0x0012E660
		private void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
			if (mobileParty.LeaderHero != null)
			{
				mobileParty.LeaderHero.IsKnownToPlayer = true;
			}
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x00130478 File Offset: 0x0012E678
		private void OnPartyAttachedAnotherParty(MobileParty party)
		{
			if (party == MobileParty.MainParty)
			{
				if (party.AttachedTo.LeaderHero != null)
				{
					party.AttachedTo.LeaderHero.IsKnownToPlayer = true;
				}
				using (List<MobileParty>.Enumerator enumerator = party.AttachedTo.AttachedParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MobileParty mobileParty = enumerator.Current;
						if (mobileParty.LeaderHero != null)
						{
							mobileParty.LeaderHero.IsKnownToPlayer = true;
						}
					}
					return;
				}
			}
			if ((party.AttachedTo == MobileParty.MainParty || party.AttachedTo == MobileParty.MainParty.AttachedTo) && party.LeaderHero != null)
			{
				party.LeaderHero.IsKnownToPlayer = true;
			}
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x00130538 File Offset: 0x0012E738
		private void OnPartyAttachedToAnotherParty(MobileParty mobileParty)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				if (mobileParty.AttachedTo.LeaderHero != null)
				{
					mobileParty.AttachedTo.LeaderHero.IsKnownToPlayer = true;
				}
				using (List<MobileParty>.Enumerator enumerator = mobileParty.AttachedTo.AttachedParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MobileParty mobileParty2 = enumerator.Current;
						if (mobileParty2.LeaderHero != null)
						{
							mobileParty2.LeaderHero.IsKnownToPlayer = true;
						}
					}
					return;
				}
			}
			if ((mobileParty.AttachedTo == MobileParty.MainParty || mobileParty.AttachedTo == MobileParty.MainParty.AttachedTo) && mobileParty.LeaderHero != null)
			{
				mobileParty.LeaderHero.IsKnownToPlayer = true;
			}
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x001305F8 File Offset: 0x0012E7F8
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (MapEvent.PlayerMapEvent == mapEvent)
			{
				foreach (PartyBase partyBase in mapEvent.InvolvedParties)
				{
					if (partyBase.LeaderHero != null)
					{
						partyBase.LeaderHero.IsKnownToPlayer = true;
					}
				}
			}
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x0013065C File Offset: 0x0012E85C
		private void OnPlayerLearnsAboutHero(Hero hero)
		{
			this.UpdateHeroLocation(hero);
			if (hero.Clan != Clan.PlayerClan)
			{
				TextObject textObject = new TextObject("{=oSghSUxp}You've learned about {?IS_RULER}{RULER_NAME_AND_TITLE}{?}{HERO.NAME}{\\?}.", null);
				textObject.SetTextVariable("IS_RULER", hero.IsKingdomLeader ? 1 : 0);
				if (hero.IsKingdomLeader)
				{
					TextObject textObject2 = GameTexts.FindText("str_faction_ruler_name_with_title", hero.MapFaction.Culture.StringId);
					textObject2.SetCharacterProperties("RULER", hero.CharacterObject, false);
					textObject.SetTextVariable("RULER_NAME_AND_TITLE", textObject2);
				}
				else
				{
					textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
				}
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
			}
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x0013070A File Offset: 0x0012E90A
		private void OnAfterMissionStarted(IMission mission)
		{
			if (CampaignMission.Current.Location != null)
			{
				this.LearnAboutLocationCharacters(CampaignMission.Current.Location);
			}
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x00130728 File Offset: 0x0012E928
		private void OnGameMenuChanged(MenuCallbackArgs args)
		{
			foreach (Location location in Campaign.Current.GameMenuManager.MenuLocations)
			{
				this.LearnAboutLocationCharacters(location);
			}
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x00130784 File Offset: 0x0012E984
		private void LearnAboutLocationCharacters(Location location)
		{
			foreach (LocationCharacter locationCharacter in location.GetCharacterList())
			{
				if (locationCharacter.Character.IsHero && locationCharacter.Character.HeroObject.CurrentSettlement == Settlement.CurrentSettlement)
				{
					locationCharacter.Character.HeroObject.IsKnownToPlayer = true;
				}
			}
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x00130800 File Offset: 0x0012EA00
		private void OnPlayerMetHero(Hero hero)
		{
			hero.IsKnownToPlayer = true;
			if (hero.IsNotable)
			{
				Settlement currentSettlement = hero.CurrentSettlement;
				if (currentSettlement != null && currentSettlement.IsTown)
				{
					TraitEffectObject calculatingNotableRelationEffect = DefaultPersonalityTraitEffects.CalculatingNotableRelationEffect;
					float traitEffectBonus = TraitEffectHelper.GetTraitEffectBonus(Hero.MainHero, calculatingNotableRelationEffect);
					if (traitEffectBonus != 0f)
					{
						ChangeRelationAction.ApplyPlayerRelation(hero, (int)traitEffectBonus, false, false);
					}
				}
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00130854 File Offset: 0x0012EA54
		private void OnDailyTickHero(Hero hero)
		{
			this.UpdateHeroLocation(hero);
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00130860 File Offset: 0x0012EA60
		private void OnAgentJoinedConversation(IAgent agent)
		{
			CharacterObject characterObject = (CharacterObject)agent.Character;
			if (characterObject.IsHero)
			{
				this.UpdateHeroLocation(characterObject.HeroObject);
				characterObject.HeroObject.IsKnownToPlayer = true;
			}
			MobileParty conversationParty = MobileParty.ConversationParty;
			Hero hero;
			if (conversationParty == null)
			{
				hero = null;
			}
			else
			{
				CaravanPartyComponent caravanPartyComponent = conversationParty.CaravanPartyComponent;
				hero = ((caravanPartyComponent != null) ? caravanPartyComponent.Owner : null);
			}
			Hero hero2 = hero;
			if (hero2 != null)
			{
				hero2.IsKnownToPlayer = true;
			}
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x001308C4 File Offset: 0x0012EAC4
		private void UpdateHeroLocation(Hero hero)
		{
			if (hero.IsKnownToPlayer)
			{
				if (hero.IsActive || hero.IsPrisoner)
				{
					Settlement closestSettlement = HeroHelper.GetClosestSettlement(hero);
					if (closestSettlement != null)
					{
						hero.UpdateLastKnownClosestSettlement(closestSettlement);
						return;
					}
				}
			}
			else
			{
				hero.UpdateLastKnownClosestSettlement(null);
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x00130904 File Offset: 0x0012EB04
		private void OnCharacterCreationIsOver(int index)
		{
			if (index == 1)
			{
				foreach (Hero hero in Hero.AllAliveHeroes)
				{
					this.UpdateHeroLocation(hero);
				}
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x0013095C File Offset: 0x0012EB5C
		private void OnGameLoadFinishedEvent()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("e1.8.1.0", 0))
			{
				foreach (Hero hero in Clan.PlayerClan.Heroes)
				{
					hero.SetHasMet();
				}
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (hero2.LastKnownClosestSettlement == null)
					{
						this.UpdateHeroLocation(hero2);
					}
					if (hero2.HasMet)
					{
						hero2.IsKnownToPlayer = true;
					}
				}
			}
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x00130A2C File Offset: 0x0012EC2C
		private void OnHeroesMarried(Hero hero1, Hero hero2, bool showNotification)
		{
			if (hero1 == Hero.MainHero)
			{
				hero2.SetHasMet();
			}
			if (hero2 == Hero.MainHero)
			{
				hero1.SetHasMet();
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00130A4A File Offset: 0x0012EC4A
		private void OnHeroCreated(Hero hero, bool isBornNaturally)
		{
			if (hero.Clan == Clan.PlayerClan)
			{
				hero.SetHasMet();
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00130A60 File Offset: 0x0012EC60
		private void ConversationEnded(IEnumerable<CharacterObject> conversationCharacters)
		{
			foreach (CharacterObject characterObject in conversationCharacters)
			{
				if (characterObject.IsHero)
				{
					bool flag = true;
					CampaignEventDispatcher.Instance.CanPlayerMeetWithHeroAfterConversation(characterObject.HeroObject, ref flag);
					if (flag)
					{
						characterObject.HeroObject.SetHasMet();
					}
				}
			}
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00130ACC File Offset: 0x0012ECCC
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
