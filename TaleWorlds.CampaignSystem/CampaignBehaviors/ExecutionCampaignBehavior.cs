using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200040D RID: 1037
	public class ExecutionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004202 RID: 16898 RVA: 0x0012B058 File Offset: 0x00129258
		private float CalculatePlayerExecutionProbability(Hero executor)
		{
			if (executor.Clan.HasBloodFeudWithPlayer)
			{
				return 1f;
			}
			int traitLevel = executor.GetTraitLevel(DefaultTraits.Mercy);
			float num = (float)traitLevel * 10f - 50f;
			float num2 = (float)executor.Clan.GetRelationWithClan(Clan.PlayerClan);
			float num3;
			if (num2 >= num)
			{
				num3 = 0f;
			}
			else
			{
				num3 = MathF.Max((-num2 * 0.3f - (float)traitLevel * 5f) * 0.01f, 0f);
			}
			return num3;
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0012B0DC File Offset: 0x001292DC
		private float CalculatePlayerClanMemberExecutionProbability(Hero clanMember, Hero executor)
		{
			int traitLevel = executor.GetTraitLevel(DefaultTraits.Mercy);
			float elapsedDaysUntilNow = clanMember.CaptivityStartTime.ElapsedDaysUntilNow;
			float num2;
			if (executor.Clan.HasBloodFeudWithPlayer)
			{
				float num = MBMath.ClampFloat(elapsedDaysUntilNow * 0.1f, 0f, 1f);
				num2 = num * num;
			}
			else
			{
				float num3 = (float)executor.Clan.GetRelationWithClan(Clan.PlayerClan);
				if (num3 > -50f)
				{
					num2 = 0f;
				}
				else
				{
					num2 = MathF.Max((-num3 * 0.3f - (float)traitLevel * 5f) * 0.01f, 0f);
				}
			}
			return num2;
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x0012B17C File Offset: 0x0012937C
		public static int GetBloodFeudStartRelationPenaltyToOtherClan(Hero dyingHero, Clan otherClan)
		{
			if (otherClan.GetRelationWithClan(dyingHero.Clan) >= Campaign.Current.Models.DiplomacyModel.MaxNeutralRelationLimit)
			{
				if (dyingHero.GetTraitLevel(DefaultTraits.Honor) >= 0 && !dyingHero.Clan.IsRebelClan && dyingHero.Clan.IsNoble)
				{
					return -45;
				}
				return -30;
			}
			else
			{
				if (otherClan.Kingdom != dyingHero.Clan.Kingdom)
				{
					return 0;
				}
				if (dyingHero.GetTraitLevel(DefaultTraits.Honor) >= 0 && !dyingHero.Clan.IsRebelClan && dyingHero.Clan.IsNoble)
				{
					return -50;
				}
				return -25;
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x0012B21C File Offset: 0x0012941C
		private static int GetBloodMoneyForPlayerToPayAgainstClan(Clan clanWithFeud)
		{
			int num = 5000;
			if (clanWithFeud.BloodFeudExecutionsDoneCount < clanWithFeud.BloodFeudExecutionsReceivedCount)
			{
				num += 50000 * (clanWithFeud.BloodFeudExecutionsReceivedCount - clanWithFeud.BloodFeudExecutionsDoneCount);
			}
			return num;
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x0012B254 File Offset: 0x00129454
		public override void RegisterEvents()
		{
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroTakenPrisoner));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail, bool>(this.OnHeroPrisonerReleased));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.OnBloodFeudStateChangedEvent.AddNonSerializedListener(this, new Action<Clan, Hero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail>(this.OnBloodFeudStateChanged));
			CampaignEvents.QuarterHourlyTickEvent.AddNonSerializedListener(this, new Action(this.QuarterHourlyTick));
			CampaignEvents.OnDeathMarkAddedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnDeathMarkAdded));
			CampaignEvents.PrisonersChangeInSettlement.AddNonSerializedListener(this, new Action<Settlement, FlattenedTroopRoster, Hero, bool>(this.OnPrisonersChangedInSettlement));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.CanHeroBeReleasedEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanHeroBeReleased));
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
			CampaignEvents.OnPrisonerDonatedToSettlementEvent.AddNonSerializedListener(this, new Action<MobileParty, FlattenedTroopRoster, Settlement>(this.OnPrisonerDonatedToSettlement));
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x0012B3BC File Offset: 0x001295BC
		private void OnPrisonerDonatedToSettlement(MobileParty oldOwner, FlattenedTroopRoster prisonerRoster, Settlement toSettlement)
		{
			if (oldOwner.ActualClan.HasBloodFeudWithPlayer)
			{
				foreach (CharacterObject characterObject in prisonerRoster.Troops)
				{
					if (characterObject.IsHero && characterObject.HeroObject != Hero.MainHero && characterObject.HeroObject.Clan == Clan.PlayerClan && !this._pendingClanMemberSettlementExecutions.ContainsKey(characterObject.HeroObject))
					{
						this.StartClanMemberExecutionAtSettlement(characterObject.HeroObject, oldOwner.ActualClan);
					}
				}
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0012B45C File Offset: 0x0012965C
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (detail == KillCharacterAction.KillCharacterActionDetail.Executed && victim.Clan != null && killer.Clan != null)
			{
				if (killer == Hero.MainHero)
				{
					this.OnPlayerExecutedHero(victim);
				}
				else if (victim.Clan == Clan.PlayerClan)
				{
					this.OnPlayerClanMemberExecuted(victim, killer);
				}
				else if (killer.Clan == Clan.PlayerClan && victim.Clan.HasBloodFeudWithPlayer)
				{
					this.OnPlayerMemberExecutedAHero(victim, killer);
				}
				if (victim.Clan.HasBloodFeudWithPlayer)
				{
					victim.Clan.BloodFeudExecutionsReceivedCount++;
				}
				else if (killer.Clan.HasBloodFeudWithPlayer)
				{
					killer.Clan.BloodFeudExecutionsDoneCount++;
				}
			}
			if (this._heroesPendingMapEventEndToBeExecuted.Contains(victim))
			{
				this._heroesPendingMapEventEndToBeExecuted.Remove(victim);
			}
			if (this._pendingClanMemberSettlementExecutions.ContainsKey(victim))
			{
				this._pendingClanMemberSettlementExecutions.Remove(victim);
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x0012B548 File Offset: 0x00129748
		private void OnPlayerExecutedHero(Hero victim)
		{
			if (!victim.Clan.HasBloodFeudWithPlayer)
			{
				ChangeBloodFeudStateAction.StartBloodFeudWithClanByPlayerExecutingAHero(victim.Clan, victim);
				TraitLevelingHelper.OnBloodFeudStarted(victim);
			}
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x0012B56C File Offset: 0x0012976C
		private void OnPlayerClanMemberExecuted(Hero executedHero, Hero executor)
		{
			if (!executor.Clan.HasBloodFeudWithPlayer)
			{
				ChangeBloodFeudStateAction.StartBloodFeudWithClanByAIExecutingPlayerRelative(executor.Clan, executedHero);
			}
			TextObject textObject = new TextObject("{=xwMNKupG}The {EXECUTOR_CLAN} pursues their feud against you by executing your {RELATION} {CLAN_MEMBER.NAME}.", null);
			StringHelpers.SetCharacterProperties("CLAN_MEMBER", executedHero.CharacterObject, textObject, false);
			textObject.SetTextVariable("EXECUTOR_CLAN", executor.Clan.Name);
			textObject.SetTextVariable("RELATION", ConversationHelper.GetHeroRelationToHeroTextShort(executedHero, Hero.MainHero, false));
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudClanMemberGotExecutedMapNotification(executedHero, executor, CampaignTime.Now, textObject));
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x0012B5FC File Offset: 0x001297FC
		private void OnPlayerMemberExecutedAHero(Hero heroToExecute, Hero clanMember)
		{
			TextObject textObject = new TextObject("{=9OTgBLHx}{CLAN_MEMBER} ordered the execution of {LORD}, continuing the blood feud between the {PLAYER_CLAN} and the {OTHER_CLAN}.", null);
			textObject.SetTextVariable("LORD", heroToExecute.Name);
			textObject.SetTextVariable("CLAN_MEMBER", clanMember.Name);
			textObject.SetTextVariable("PLAYER_CLAN", Clan.PlayerClan.Name);
			textObject.SetTextVariable("OTHER_CLAN", heroToExecute.Clan.Name);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudClanMemberExecutedLordMapNotification(heroToExecute, clanMember, CampaignTime.Now, textObject));
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0012B684 File Offset: 0x00129884
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (effectiveHero.Clan != null && effectiveHeroGainedRelationWith.Clan != null && (effectiveHero.Clan == Clan.PlayerClan || effectiveHeroGainedRelationWith.Clan == Clan.PlayerClan) && effectiveHero.Clan.GetRelationWithClan(effectiveHeroGainedRelationWith.Clan) >= 0)
			{
				if (effectiveHero.Clan.HasBloodFeudWithPlayer)
				{
					ChangeBloodFeudStateAction.SettleBloodFeudByRelationIncrease(effectiveHero.Clan);
					return;
				}
				if (effectiveHeroGainedRelationWith.Clan.HasBloodFeudWithPlayer)
				{
					ChangeBloodFeudStateAction.SettleBloodFeudByRelationIncrease(effectiveHeroGainedRelationWith.Clan);
				}
			}
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x0012B700 File Offset: 0x00129900
		private void OnBloodFeudStateChanged(Clan clanWithFeud, Hero executedHero, ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail detail)
		{
			if (clanWithFeud.HasBloodFeudWithPlayer)
			{
				TextObject textObject;
				if (detail == ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.StartedByPlayerExecuteAHero)
				{
					textObject = new TextObject("{=3RvhG1HN}You have started a blood feud with the {CLAN}.", null);
				}
				else
				{
					textObject = new TextObject("{=99k8PGvq}The {CLAN} have started a blood feud with your clan, the {PLAYER_CLAN}.", null);
					textObject.SetTextVariable("PLAYER_CLAN", Clan.PlayerClan.Name);
				}
				textObject.SetTextVariable("CLAN", clanWithFeud.Name);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudStartedMapNotification(textObject));
				ChangeRelationAction.SetRelationBetweenHeroes(Clan.PlayerClan.Leader, clanWithFeud.Leader, Campaign.Current.Models.DiplomacyModel.MinRelationLimit, true);
				int num = 0;
				if (clanWithFeud.Kingdom != null)
				{
					foreach (Clan clan in Clan.All)
					{
						if (clan != Clan.PlayerClan)
						{
							int bloodFeudStartRelationPenaltyToOtherClan = ExecutionCampaignBehavior.GetBloodFeudStartRelationPenaltyToOtherClan(executedHero, clan);
							if (bloodFeudStartRelationPenaltyToOtherClan != 0)
							{
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Clan.PlayerClan.Leader, clan.Leader, bloodFeudStartRelationPenaltyToOtherClan, false);
								num++;
							}
						}
					}
				}
				if (num > 0)
				{
					TextObject textObject2 = new TextObject("{=oqO9kjeW}The execution has hurt your relations with {COUNT} {?IS_PLURAL}clans{?}clan{\\?}.", null);
					MBTextManager.SetTextVariable("IS_PLURAL", (num > 1) ? 1 : 0);
					textObject2.SetTextVariable("COUNT", num);
					MBInformationManager.AddQuickInformation(textObject2, 0, null, null, "");
				}
			}
			else
			{
				TextObject textObject3 = new TextObject("{=Px3DDMvV}You have decided not to pursue your blood feud with the {CLAN}, and it has ended.", null);
				if (detail == ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.SettledByRansomPayment)
				{
					textObject3 = new TextObject("{=fsfeamMz}Your blood feud with the {CLAN} has ended, as you paid them money to settle it.", null);
				}
				else if (detail == ChangeBloodFeudStateAction.ChangeBloodFeudActionDetail.SettledByRelationIncrease)
				{
					textObject3 = new TextObject("{=iNPcIqUj}Your blood feud with the {CLAN} has ended after relations between your clans improved.", null);
				}
				textObject3.SetTextVariable("CLAN", clanWithFeud.Name);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudEndedMapNotification(textObject3));
			}
			this.UpdateAllPlayerClanMemberExecutionStates();
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x0012B8B8 File Offset: 0x00129AB8
		private void OnHeroTakenPrisoner(PartyBase capturer, Hero prisoner)
		{
			if (capturer != null && capturer.LeaderHero != null)
			{
				if (prisoner == Hero.MainHero)
				{
					if (MBRandom.RandomFloat <= this.CalculatePlayerExecutionProbability(capturer.LeaderHero))
					{
						capturer.LeaderHero.SetHasMet();
						this._isMainHeroExecuted = true;
						return;
					}
				}
				else if (capturer.LeaderHero.Clan == Clan.PlayerClan)
				{
					if (capturer.LeaderHero != Hero.MainHero && prisoner.Clan.HasBloodFeudWithPlayer)
					{
						this._heroesPendingMapEventEndToBeExecuted.Add(prisoner);
						return;
					}
				}
				else if (prisoner.Clan == Clan.PlayerClan && capturer.LeaderHero.Clan.HasBloodFeudWithPlayer)
				{
					this.ShowClanMemberCapturedByFeudedClanNotification(prisoner, capturer.LeaderHero.Clan);
				}
			}
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x0012B970 File Offset: 0x00129B70
		private void ShowClanMemberCapturedByFeudedClanNotification(Hero clanMember, Clan feudedClan)
		{
			TextObject textObject = new TextObject("{=tOGIagn0}The {FEUDED_CLAN} has captured your {RELATION} {CLAN_MEMBER.NAME}. Unless {?CLAN_MEMBER.GENDER}she{?}he{\\?} is freed, they are likely to execute {?CLAN_MEMBER.GENDER}her{?}him{\\?} within a few days.", null);
			StringHelpers.SetCharacterProperties("CLAN_MEMBER", clanMember.CharacterObject, textObject, false);
			textObject.SetTextVariable("FEUDED_CLAN", feudedClan.Name);
			textObject.SetTextVariable("RELATION", ConversationHelper.GetHeroRelationToHeroTextShort(clanMember, Hero.MainHero, false));
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudClanMemberCapturedMapNotification(clanMember, textObject));
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x0012B9DC File Offset: 0x00129BDC
		private void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail, bool showNotification = true)
		{
			this.UpdateAllPlayerClanMemberExecutionStates();
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x0012B9E4 File Offset: 0x00129BE4
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			this.UpdateAllPlayerClanMemberExecutionStates();
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x0012B9EC File Offset: 0x00129BEC
		private void DailyTickHero(Hero hero)
		{
			if (this._pendingClanMemberSettlementExecutions.ContainsKey(hero))
			{
				ValueTuple<Clan, CampaignTime> valueTuple = this._pendingClanMemberSettlementExecutions[hero];
				if (valueTuple.Item2.IsPast)
				{
					Hero leader = valueTuple.Item1.Leader;
					KillCharacterAction.ApplyByExecution(hero, leader, true, false);
					return;
				}
			}
			else if (hero != Hero.MainHero && hero.Clan == Clan.PlayerClan && hero.PartyBelongedToAsPrisoner != null)
			{
				Hero hero2 = (hero.PartyBelongedToAsPrisoner.IsMobile ? hero.PartyBelongedToAsPrisoner.LeaderHero : hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan.Leader);
				if (hero2 != null && MBRandom.RandomFloat <= this.CalculatePlayerClanMemberExecutionProbability(hero, hero2))
				{
					KillCharacterAction.ApplyByExecution(hero, hero2, true, false);
				}
			}
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x0012BAA0 File Offset: 0x00129CA0
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (this._isMainHeroExecuted && !Campaign.Current.ConversationManager.IsConversationInProgress)
			{
				CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(Hero.MainHero.PartyBelongedToAsPrisoner.LeaderHero.CharacterObject, null, false, false, false, false, false, false));
			}
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0012BAFC File Offset: 0x00129CFC
		private void OnDeathMarkAdded(Hero victim, Hero killer)
		{
			if (victim.DeathMark == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent)
			{
				this._heroesPendingMapEventEndToBeExecuted.Add(victim);
				if (killer == Hero.MainHero)
				{
					this.OnPlayerExecutedHero(victim);
				}
			}
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0012BB22 File Offset: 0x00129D22
		private void OnClanDestroyed(Clan destroyedClan)
		{
			this.UpdateAllPlayerClanMemberExecutionStates();
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x0012BB2C File Offset: 0x00129D2C
		private void UpdateAllPlayerClanMemberExecutionStates()
		{
			foreach (Hero hero in Clan.PlayerClan.Heroes)
			{
				if (hero != Hero.MainHero)
				{
					this.UpdateClanMemberExecutionState(hero);
				}
			}
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x0012BB8C File Offset: 0x00129D8C
		private void QuarterHourlyTick()
		{
			foreach (Hero hero in this._heroesPendingMapEventEndToBeExecuted)
			{
				if (hero.DeathMark == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent && hero.DeathMarkKillerHero != null && (hero.PartyBelongedToAsPrisoner == null || hero.PartyBelongedToAsPrisoner.MapEvent != null))
				{
					KillCharacterAction.ApplyByExecution(hero, hero.DeathMarkKillerHero, true, false);
					break;
				}
				if (hero.IsPrisoner && hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.LeaderHero != null && hero.PartyBelongedToAsPrisoner.MapEvent == null)
				{
					Hero leaderHero = hero.PartyBelongedToAsPrisoner.LeaderHero;
					if (leaderHero.Clan == Clan.PlayerClan && MBRandom.RandomFloat <= 0.125f)
					{
						KillCharacterAction.ApplyByExecution(hero, leaderHero, true, false);
						break;
					}
				}
			}
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x0012BC70 File Offset: 0x00129E70
		private void StartClanMemberExecutionAtSettlement(Hero clanMember, Clan clanWithFeud)
		{
			CampaignTime campaignTime = CampaignTime.DaysFromNow(8f);
			this._pendingClanMemberSettlementExecutions.Add(clanMember, new ValueTuple<Clan, CampaignTime>(clanWithFeud, campaignTime));
			Settlement settlement = clanMember.PartyBelongedToAsPrisoner.Settlement;
			string heroRelationToHeroTextShort = ConversationHelper.GetHeroRelationToHeroTextShort(clanMember, Hero.MainHero, false);
			TextObject textObject = new TextObject("{=8IP5qoyq}{CLAN_MEMBER.NAME}, your {RELATION}, is held prisoner by the {OTHER_CLAN} in {SETTLEMENT}. As your two clans have a blood feud, they plan to execute {?CLAN_MEMBER.GENDER}her{?}him{\\?} in {DAYS} {?DAYS > 1}days{?}day{\\?}.", null);
			StringHelpers.SetCharacterProperties("CLAN_MEMBER", clanMember.CharacterObject, textObject, false);
			textObject.SetTextVariable("SETTLEMENT", settlement.Name);
			textObject.SetTextVariable("DAYS", campaignTime.RemainingDaysFromNow, 2);
			textObject.SetTextVariable("OTHER_CLAN", clanWithFeud.Name);
			textObject.SetTextVariable("RELATION", heroRelationToHeroTextShort);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudClanMemberCapturedMapNotification(clanMember, campaignTime, settlement, textObject));
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x0012BD30 File Offset: 0x00129F30
		private void CancelClanMemberExecutionAtSettlement(Hero clanMember)
		{
			Clan item = this._pendingClanMemberSettlementExecutions[clanMember].Item1;
			this._pendingClanMemberSettlementExecutions.Remove(clanMember);
			TextObject textObject = new TextObject("{=m3R1ubdZ}Your {RELATION} {CLAN_MEMBER.NAME} is no longer held prisoner by the {FEUDED_CLAN}, and is no longer at risk of being executed as part of your blood feud with them.", null);
			StringHelpers.SetCharacterProperties("CLAN_MEMBER", clanMember.CharacterObject, textObject, false);
			textObject.SetTextVariable("FEUDED_CLAN", item.Name);
			textObject.SetTextVariable("RELATION", ConversationHelper.GetHeroRelationToHeroTextShort(clanMember, Hero.MainHero, false));
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new BloodFeudClanMemberExecuteCancelledMapNotification(clanMember, textObject));
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x0012BDBC File Offset: 0x00129FBC
		public void UpdateClanMemberExecutionState(Hero clanMember)
		{
			Clan clan = null;
			ValueTuple<Clan, CampaignTime> valueTuple;
			if (this._pendingClanMemberSettlementExecutions.TryGetValue(clanMember, out valueTuple))
			{
				clan = valueTuple.Item1;
			}
			Clan clan2;
			if (this.GetClanMemberCurrentExecutionData(clanMember, out clan2))
			{
				if (clan == null)
				{
					this.StartClanMemberExecutionAtSettlement(clanMember, clan2);
					return;
				}
				if (clan != clan2)
				{
					this.CancelClanMemberExecutionAtSettlement(clanMember);
					this.StartClanMemberExecutionAtSettlement(clanMember, clan2);
					return;
				}
			}
			else if (clan != null)
			{
				this.CancelClanMemberExecutionAtSettlement(clanMember);
			}
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x0012BE18 File Offset: 0x0012A018
		public bool GetClanMemberCurrentExecutionData(Hero hero, out Clan currentExecutorClan)
		{
			ValueTuple<Clan, CampaignTime> valueTuple;
			if (this._pendingClanMemberSettlementExecutions.TryGetValue(hero, out valueTuple) && valueTuple.Item1.HasBloodFeudWithPlayer)
			{
				PartyBase partyBelongedToAsPrisoner = hero.PartyBelongedToAsPrisoner;
				IFaction faction;
				if (partyBelongedToAsPrisoner == null)
				{
					faction = null;
				}
				else
				{
					Settlement settlement = partyBelongedToAsPrisoner.Settlement;
					faction = ((settlement != null) ? settlement.OwnerClan.MapFaction : null);
				}
				if (faction == valueTuple.Item1.MapFaction && !valueTuple.Item1.IsEliminated)
				{
					currentExecutorClan = valueTuple.Item1;
					return true;
				}
			}
			if (hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.Settlement != null)
			{
				currentExecutorClan = hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan;
				if (currentExecutorClan.HasBloodFeudWithPlayer)
				{
					return true;
				}
			}
			currentExecutorClan = null;
			return false;
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0012BEBF File Offset: 0x0012A0BF
		private void OnPrisonersChangedInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
			this.UpdateAllPlayerClanMemberExecutionStates();
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x0012BEC8 File Offset: 0x0012A0C8
		private void CanHeroBeReleased(Hero hero, ref bool result)
		{
			if (this._pendingClanMemberSettlementExecutions.ContainsKey(hero))
			{
				result = false;
				return;
			}
			if (this._heroesPendingMapEventEndToBeExecuted.Contains(hero))
			{
				result = false;
				return;
			}
			if (hero.Clan == Clan.PlayerClan && hero.IsPrisoner && hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.LeaderHero != null && hero.PartyBelongedToAsPrisoner.MobileParty.ActualClan.HasBloodFeudWithPlayer)
			{
				result = false;
			}
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x0012BF44 File Offset: 0x0012A144
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("lord_executes_player", "start", "close_window", "{=!}{EXECUTE_TEXT}", new ConversationSentence.OnConditionDelegate(this.player_is_executed_option_condition), delegate
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += this.player_is_executed_option_consequence;
			}, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_talk_blood_feud_info", "ransom_broker_talk", "ransom_broker_blood_feud_info", "{=FmL3Jixd}As you deal with captives, do you know anything about blood feuds?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_blood_feud_info_1", "ransom_broker_blood_feud_info", "ransom_broker_pretalk", "{=CaQqcqAU}Well, if one of the great families of this land kills a member of a different clan, the victim's kin may declare a feud. Both sides will then usually execute any prisoners from the other clan that they catch, without incurring the usual disapproval. Sometimes we can broker an end to such feuds, though it is costly.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_talk_end_blood_feud", "ransom_broker_talk", "ransom_broker_end_feud_ask", "{=bDKb7ACL}I’m involved in a blood feud, and I would like to end it.", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_has_active_feuds_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_end_feud_ask", "ransom_broker_end_feud_ask", "ransom_broker_end_feud_select_clan", "{=0oD6hGf4}Which clan do you want to end your feud with?", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_collect_feud_clans_on_consequence), 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("ransom_broker_end_feud_clan", "ransom_broker_end_feud_select_clan", "ransom_broker_end_feud_confirm", "{=!}{CLAN}", "{=ijTpwdn1}I am thinking of a different clan", "ransom_broker_end_feud_ask", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_feud_clan_option_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_feud_clan_selected_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_end_feud_cancel", "ransom_broker_end_feud_select_clan", "ransom_broker_pretalk", "{=mdNRYlfS}Nevermind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_end_feud_confirm_free", "ransom_broker_end_feud_confirm", "ransom_broker_pretalk", "{=TSJHk5YJ}Consider it done.", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_end_feud_no_cost_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_end_feud_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("ransom_broker_end_feud_confirm_cost", "ransom_broker_end_feud_confirm", "ransom_broker_end_feud_pay", "{=!}{FEUD_END_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_end_feud_cost_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("ransom_broker_end_feud_pay_accept", "ransom_broker_end_feud_pay", "ransom_broker_end_feud_paid", "{=0rKfapmF}Yes. Here is the money. Let us end the killing.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_end_feud_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.conversation_ransom_broker_end_feud_pay_clickable_condition), null);
			campaignGameStarter.AddPlayerLine("ransom_broker_end_feud_pay_decline", "ransom_broker_end_feud_pay", "ransom_broker_pretalk", "{=nykOrXhv}I cannot afford that right now.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("ransom_broker_end_feud_paid", "ransom_broker_end_feud_paid", "ransom_broker_pretalk", "{=TSJHk5YJ}Consider it done.", null, null, 100, null);
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x0012C15C File Offset: 0x0012A35C
		private bool conversation_ransom_broker_has_active_feuds_on_condition()
		{
			foreach (Clan clan in Clan.All)
			{
				if (clan.HasBloodFeudWithPlayer && !clan.IsEliminated)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x0012C1C0 File Offset: 0x0012A3C0
		private void conversation_ransom_broker_collect_feud_clans_on_consequence()
		{
			List<Clan> list = new List<Clan>();
			foreach (Clan clan in Clan.All)
			{
				if (clan.HasBloodFeudWithPlayer && !clan.IsEliminated)
				{
					list.Add(clan);
				}
			}
			ConversationSentence.SetObjectsToRepeatOver(list, 5);
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0012C230 File Offset: 0x0012A430
		private bool conversation_ransom_broker_feud_clan_option_on_condition()
		{
			Clan clan = ConversationSentence.CurrentProcessedRepeatObject as Clan;
			if (clan != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("CLAN", clan.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x0012C264 File Offset: 0x0012A464
		private void conversation_ransom_broker_feud_clan_selected_on_consequence()
		{
			this._selectedBloodFeudClan = ConversationSentence.SelectedRepeatObject as Clan;
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0012C276 File Offset: 0x0012A476
		private bool conversation_ransom_broker_end_feud_no_cost_on_condition()
		{
			return this._selectedBloodFeudClan != null && (float)ExecutionCampaignBehavior.GetBloodMoneyForPlayerToPayAgainstClan(this._selectedBloodFeudClan) <= 0f;
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0012C298 File Offset: 0x0012A498
		private bool conversation_ransom_broker_end_feud_cost_on_condition()
		{
			if (this._selectedBloodFeudClan == null)
			{
				return false;
			}
			TextObject textObject = TextObject.GetEmpty();
			int bloodMoneyForPlayerToPayAgainstClan = ExecutionCampaignBehavior.GetBloodMoneyForPlayerToPayAgainstClan(this._selectedBloodFeudClan);
			if (this._selectedBloodFeudClan.BloodFeudExecutionsDoneCount > this._selectedBloodFeudClan.BloodFeudExecutionsReceivedCount)
			{
				textObject = new TextObject("{=CwUg1jS5}Well, they've killed more of you than you have of them, so it probably will not cost you so much. I think I could settle things for {BLOOD_MONEY_COST}{GOLD_ICON} denars. Do you agree?", null);
			}
			else if (this._selectedBloodFeudClan.BloodFeudExecutionsDoneCount < this._selectedBloodFeudClan.BloodFeudExecutionsReceivedCount)
			{
				textObject = new TextObject("{=Wgfb1nSf}That can be arranged, but you've killed more of them then they of you, and a family like {CLAN} won't have it said that it reckons its blood cheaply. For {BLOOD_MONEY_COST}{GOLD_ICON}, I think I could arrange a settlement. Do you agree to pay?", null);
			}
			else
			{
				textObject = new TextObject("{=eqaVrGK4}You both seem to have spilled the same amount of blood. Honor is satisfied. For {BLOOD_MONEY_COST}{GOLD_ICON}, I think I could arrange a settlement. Do you agree to pay?", null);
			}
			textObject.SetTextVariable("BLOOD_MONEY_COST", bloodMoneyForPlayerToPayAgainstClan);
			textObject.SetTextVariable("CLAN", this._selectedBloodFeudClan.Name);
			MBTextManager.SetTextVariable("FEUD_END_TEXT", textObject, false);
			return true;
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x0012C34C File Offset: 0x0012A54C
		private bool conversation_ransom_broker_end_feud_pay_clickable_condition(out TextObject explanation)
		{
			int bloodMoneyForPlayerToPayAgainstClan = ExecutionCampaignBehavior.GetBloodMoneyForPlayerToPayAgainstClan(this._selectedBloodFeudClan);
			if (Hero.MainHero.Gold < bloodMoneyForPlayerToPayAgainstClan)
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x0012C388 File Offset: 0x0012A588
		private void conversation_ransom_broker_end_feud_on_consequence()
		{
			if (this._selectedBloodFeudClan != null && this._selectedBloodFeudClan.HasBloodFeudWithPlayer)
			{
				int bloodMoneyForPlayerToPayAgainstClan = ExecutionCampaignBehavior.GetBloodMoneyForPlayerToPayAgainstClan(this._selectedBloodFeudClan);
				if (bloodMoneyForPlayerToPayAgainstClan > 0)
				{
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, bloodMoneyForPlayerToPayAgainstClan, false);
				}
				ChangeBloodFeudStateAction.SettleBloodFeudByRansomPayment(this._selectedBloodFeudClan);
				ChangeRelationAction.SetRelationBetweenHeroes(Clan.PlayerClan.Leader, this._selectedBloodFeudClan.Leader, -49, true);
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x0012C3F0 File Offset: 0x0012A5F0
		private bool player_is_executed_option_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.IsLord && this._isMainHeroExecuted)
			{
				TextObject textObject = new TextObject("{=y2dJ4jTd}Even if henceforth there is only blood between your kin and mine, I will show you no mercy. Off with your head.", null);
				if (Hero.OneToOneConversationHero.Clan.HasBloodFeudWithPlayer)
				{
					textObject = new TextObject("{=rMeHWobg}{PLAYER.NAME}. You owe us a debt of blood, and for that your life is forfeit.", null);
					if (Hero.MainHero.GetTraitLevel(DefaultTraits.Calculating) < 0 || Hero.MainHero.GetTraitLevel(DefaultTraits.Mercy) < 0 || Hero.MainHero.GetTraitLevel(DefaultTraits.Honor) < 0)
					{
						textObject = new TextObject("{=PF3Tao8I}Well, {PLAYER.NAME}. Are you expecting mercy? You shall receive the same kind of mercy that you have shown my kin. Off with your head!", null);
					}
					StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, textObject, false);
					MBTextManager.SetTextVariable("EXECUTE_TEXT", textObject, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x0012C4B2 File Offset: 0x0012A6B2
		private void player_is_executed_option_consequence()
		{
			this._isMainHeroExecuted = false;
			KillCharacterAction.ApplyByExecution(Hero.MainHero, Hero.MainHero.PartyBelongedToAsPrisoner.LeaderHero, true, false);
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x0012C4D6 File Offset: 0x0012A6D6
		public override void SyncData(IDataStore store)
		{
			store.SyncData<Dictionary<Hero, ValueTuple<Clan, CampaignTime>>>("_pendingClanMemberSettlementExecutions", ref this._pendingClanMemberSettlementExecutions);
			store.SyncData<List<Hero>>("_heroesPendingMapEventEndToBeExecuted", ref this._heroesPendingMapEventEndToBeExecuted);
			store.SyncData<bool>("_isMainHeroExecuted", ref this._isMainHeroExecuted);
		}

		// Token: 0x040013C8 RID: 5064
		[TupleElementNames(new string[] { "CapturerClan", "ExecutionDate" })]
		private Dictionary<Hero, ValueTuple<Clan, CampaignTime>> _pendingClanMemberSettlementExecutions = new Dictionary<Hero, ValueTuple<Clan, CampaignTime>>();

		// Token: 0x040013C9 RID: 5065
		private List<Hero> _heroesPendingMapEventEndToBeExecuted = new List<Hero>();

		// Token: 0x040013CA RID: 5066
		private bool _isMainHeroExecuted;

		// Token: 0x040013CB RID: 5067
		private Clan _selectedBloodFeudClan;

		// Token: 0x040013CC RID: 5068
		private const int PlayerExecutionRelationThreshold = -50;

		// Token: 0x040013CD RID: 5069
		private const int BloodFeudEndRelationLevel = 0;

		// Token: 0x040013CE RID: 5070
		private const int DaysTillBloodFeudExecutionInSettlement = 8;
	}
}
