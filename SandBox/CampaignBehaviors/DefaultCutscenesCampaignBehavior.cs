using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000DA RID: 218
	public class DefaultCutscenesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x00049490 File Offset: 0x00047690
		public override void RegisterEvents()
		{
			CampaignEvents.BeforeHeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(DefaultCutscenesCampaignBehavior.OnHeroesMarried));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnd));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomCreated));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			CampaignEvents.OnBeforeMainCharacterDiedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnBeforeMainCharacterDied));
			CampaignEvents.OnMercenaryServiceEndedEvent.AddNonSerializedListener(this, new Action<Clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails>(this.OnMercenaryServiceEnded));
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0004956C File Offset: 0x0004776C
		private void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			SceneNotificationData sceneNotificationData = null;
			if (victim == Hero.MainHero)
			{
				MobileParty partyBelongedTo = victim.PartyBelongedTo;
				if (partyBelongedTo != null && partyBelongedTo.IsCurrentlyAtSea)
				{
					sceneNotificationData = new NavalDeathSceneNotificationItem(victim, CampaignTime.Now, detail);
				}
				else if (detail == KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge)
				{
					sceneNotificationData = new DeathOldAgeSceneNotificationItem(victim);
				}
				else if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
				{
					if (this._heroWonLastMapEVent)
					{
						bool flag = !victim.CompanionsInParty.Any<Hero>();
						List<CharacterObject> list = new List<CharacterObject>();
						DefaultCutscenesCampaignBehavior.FillAllyCharacters(flag, ref list);
						sceneNotificationData = new MainHeroBattleVictoryDeathNotificationItem(victim, list);
					}
					else
					{
						sceneNotificationData = new MainHeroBattleDeathNotificationItem(victim, this._lastEnemyCulture);
					}
				}
				else if (detail == KillCharacterAction.KillCharacterActionDetail.Executed)
				{
					TextObject textObject = new TextObject("{=uYjEknNX}{VICTIM.NAME}'s execution by {EXECUTER.NAME}", null);
					textObject.SetCharacterProperties("VICTIM", victim.CharacterObject, false);
					textObject.SetCharacterProperties("EXECUTER", killer.CharacterObject, false);
					sceneNotificationData = HeroExecutionSceneNotificationData.CreateForInformingPlayer(killer, victim, CampaignTime.Now, SceneNotificationData.RelevantContextType.Map, null, false, false, true, false, null);
				}
			}
			if (sceneNotificationData != null)
			{
				MBInformationManager.ShowSceneNotification(sceneNotificationData);
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0004964C File Offset: 0x0004784C
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			KingSelectionKingdomDecision.KingSelectionDecisionOutcome kingSelectionDecisionOutcome;
			if ((kingSelectionDecisionOutcome = chosenOutcome as KingSelectionKingdomDecision.KingSelectionDecisionOutcome) != null && isPlayerInvolved && kingSelectionDecisionOutcome.King == Hero.MainHero)
			{
				MBInformationManager.ShowSceneNotification(new BecomeKingSceneNotificationItem(kingSelectionDecisionOutcome.King));
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00049688 File Offset: 0x00047888
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (showNotification)
			{
				SceneNotificationData sceneNotificationData = null;
				if (clan == Clan.PlayerClan && detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom)
				{
					sceneNotificationData = new JoinKingdomSceneNotificationItem(clan, newKingdom);
				}
				else if (Clan.PlayerClan.Kingdom == newKingdom && detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection)
				{
					sceneNotificationData = new JoinKingdomSceneNotificationItem(clan, newKingdom);
				}
				if (sceneNotificationData != null)
				{
					MBInformationManager.ShowSceneNotification(sceneNotificationData);
				}
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x000496D5 File Offset: 0x000478D5
		private void OnMercenaryServiceEnded(Clan clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails detail)
		{
			if (clan.Kingdom != null && clan.Kingdom == Clan.PlayerClan.Kingdom && detail == EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByBecomingVassal)
			{
				MBInformationManager.ShowSceneNotification(new JoinKingdomSceneNotificationItem(clan, clan.Kingdom));
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00049708 File Offset: 0x00047908
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			if (!kingdom.IsRebelClan)
			{
				if (kingdom.Leader == Hero.MainHero)
				{
					MBInformationManager.ShowSceneNotification(Campaign.Current.Models.CutsceneSelectionModel.GetKingdomDestroyedSceneNotification(kingdom));
					return;
				}
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new KingdomDestroyedMapNotification(kingdom, CampaignTime.Now));
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0004975F File Offset: 0x0004795F
		private void OnKingdomCreated(Kingdom kingdom)
		{
			if (Hero.MainHero.Clan.Kingdom == kingdom)
			{
				MBInformationManager.ShowSceneNotification(new KingdomCreatedSceneNotificationItem(kingdom));
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00049780 File Offset: 0x00047980
		private void OnHeroComesOfAge(Hero hero)
		{
			Hero mother = hero.Mother;
			if (((mother != null) ? mother.Clan : null) != Clan.PlayerClan)
			{
				Hero father = hero.Father;
				if (((father != null) ? father.Clan : null) != Clan.PlayerClan)
				{
					return;
				}
			}
			Hero mentorHeroForComeOfAge = this.GetMentorHeroForComeOfAge(hero);
			TextObject textObject = new TextObject("{=t4KwQOB7}{HERO.NAME} is now of age.", null);
			textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new HeirComeOfAgeMapNotification(hero, mentorHeroForComeOfAge, textObject, CampaignTime.Now));
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00049804 File Offset: 0x00047A04
		private void OnMapEventEnd(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent && mapEvent.HasWinner)
			{
				this._heroWonLastMapEVent = mapEvent.WinningSide != BattleSideEnum.None && mapEvent.WinningSide == mapEvent.PlayerSide;
				this._lastEnemyCulture = ((mapEvent.PlayerSide == BattleSideEnum.Attacker) ? mapEvent.DefenderSide.MapFaction.Culture : mapEvent.AttackerSide.MapFaction.Culture);
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00049872 File Offset: 0x00047A72
		private static void OnHeroesMarried(Hero firstHero, Hero secondHero, bool showNotification)
		{
			if (firstHero == Hero.MainHero || secondHero == Hero.MainHero)
			{
				Hero hero = (firstHero.IsFemale ? secondHero : firstHero);
				MBInformationManager.ShowSceneNotification(new MarriageSceneNotificationItem(hero, hero.Spouse, CampaignTime.Now, SceneNotificationData.RelevantContextType.Any));
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x000498A6 File Offset: 0x00047AA6
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000498A8 File Offset: 0x00047AA8
		private static void FillAllyCharacters(bool noCompanions, ref List<CharacterObject> allyCharacters)
		{
			if (noCompanions)
			{
				allyCharacters.Add(Hero.MainHero.MapFaction.Culture.RangedEliteMilitiaTroop);
				return;
			}
			List<CharacterObject> list = (from c in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where c.Character != CharacterObject.PlayerCharacter && c.Character.IsHero
				select c into t
				select t.Character).ToList<CharacterObject>();
			allyCharacters.AddRange(list.Take<CharacterObject>(3));
			int count = allyCharacters.Count;
			for (int i = 0; i < 3 - count; i++)
			{
				allyCharacters.Add(Hero.AllAliveHeroes.GetRandomElement<Hero>().CharacterObject);
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0004996C File Offset: 0x00047B6C
		private Hero GetMentorHeroForComeOfAge(Hero hero)
		{
			Hero hero2 = Hero.MainHero;
			if (hero.IsFemale)
			{
				if (hero.Mother != null && hero.Mother.IsAlive)
				{
					hero2 = hero.Mother;
				}
				else if (hero.Father != null && hero.Father.IsAlive)
				{
					hero2 = hero.Father;
				}
			}
			else if (hero.Father != null && hero.Father.IsAlive)
			{
				hero2 = hero.Father;
			}
			else if (hero.Mother != null && hero.Mother.IsAlive)
			{
				hero2 = hero.Mother;
			}
			if (hero.Mother == Hero.MainHero || hero.Father == Hero.MainHero)
			{
				hero2 = Hero.MainHero;
			}
			return hero2;
		}

		// Token: 0x04000497 RID: 1175
		private bool _heroWonLastMapEVent;

		// Token: 0x04000498 RID: 1176
		private CultureObject _lastEnemyCulture;
	}
}
