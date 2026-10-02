using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x0200038C RID: 908
	public class MerchantArmyOfPoachersIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x060035B5 RID: 13749 RVA: 0x000DB8C6 File Offset: 0x000D9AC6
		private void engage_poachers_consequence(MenuCallbackArgs args)
		{
			MerchantArmyOfPoachersIssueBehavior.Instance.StartQuestBattle();
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060035B6 RID: 13750 RVA: 0x000DB8D4 File Offset: 0x000D9AD4
		private static MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest Instance
		{
			get
			{
				MerchantArmyOfPoachersIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<MerchantArmyOfPoachersIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest merchantArmyOfPoachersIssueQuest;
						if ((merchantArmyOfPoachersIssueQuest = enumerator.Current as MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest) != null)
						{
							campaignBehavior._cachedQuest = merchantArmyOfPoachersIssueQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000DB96C File Offset: 0x000D9B6C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000DB99C File Offset: 0x000D9B9C
		private bool poachers_menu_back_condition(MenuCallbackArgs args)
		{
			return Hero.MainHero.IsWounded;
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000DB9A8 File Offset: 0x000D9BA8
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("army_of_poachers_village", "{=eaQxeRh6}A boy runs out of the village and asks you to talk to the leader of the poachers. The villagers want to avoid a fight outside their homes.", new OnInitDelegate(this.army_of_poachers_village_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("army_of_poachers_village", "engage_the_poachers", "{=xF7he8fZ}Fight the poachers", new GameMenuOption.OnConditionDelegate(this.engage_poachers_condition), new GameMenuOption.OnConsequenceDelegate(this.engage_poachers_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("army_of_poachers_village", "talk_to_the_poachers", "{=wwJGE28v}Negotiate with the poachers", new GameMenuOption.OnConditionDelegate(this.talk_to_leader_of_poachers_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_leader_of_poachers_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("army_of_poachers_village", "back_poachers", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(this.poachers_menu_back_condition), new GameMenuOption.OnConsequenceDelegate(this.poachers_menu_back_consequence), false, -1, false, null);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000DBA68 File Offset: 0x000D9C68
		private void army_of_poachers_village_on_init(MenuCallbackArgs args)
		{
			if (MerchantArmyOfPoachersIssueBehavior.Instance != null && MerchantArmyOfPoachersIssueBehavior.Instance.IsOngoing)
			{
				args.MenuContext.SetBackgroundMeshName(MerchantArmyOfPoachersIssueBehavior.Instance._questVillage.Settlement.SettlementComponent.WaitMeshName);
				if (MerchantArmyOfPoachersIssueBehavior.Instance._poachersParty == null && !Hero.MainHero.IsWounded)
				{
					MerchantArmyOfPoachersIssueBehavior.Instance.CreatePoachersParty();
				}
				if (MerchantArmyOfPoachersIssueBehavior.Instance._isReadyToBeFinalized && PlayerEncounter.Current != null)
				{
					bool flag = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide;
					PlayerEncounter.Update();
					if (PlayerEncounter.Current == null)
					{
						MerchantArmyOfPoachersIssueBehavior.Instance._isReadyToBeFinalized = false;
						if (flag)
						{
							MerchantArmyOfPoachersIssueBehavior.Instance.QuestSuccessWithPlayerDefeatedPoachers();
						}
						else
						{
							MerchantArmyOfPoachersIssueBehavior.Instance.QuestFailWithPlayerDefeatedAgainstPoachers();
						}
					}
					else if (PlayerEncounter.Battle.WinningSide == BattleSideEnum.None)
					{
						PlayerEncounter.LeaveEncounter = true;
						PlayerEncounter.Update();
						MerchantArmyOfPoachersIssueBehavior.Instance.QuestFailWithPlayerDefeatedAgainstPoachers();
					}
					else if (flag && PlayerEncounter.Current != null && Game.Current.GameStateManager.ActiveState is MapState)
					{
						PlayerEncounter.Finish(true);
						MerchantArmyOfPoachersIssueBehavior.Instance.QuestSuccessWithPlayerDefeatedPoachers();
					}
				}
				if (MerchantArmyOfPoachersIssueBehavior.Instance != null && MerchantArmyOfPoachersIssueBehavior.Instance._talkedToPoachersBattleWillStart)
				{
					MerchantArmyOfPoachersIssueBehavior.Instance.StartQuestBattle();
				}
			}
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000DBBA7 File Offset: 0x000D9DA7
		private bool engage_poachers_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			if (Hero.MainHero.IsWounded)
			{
				args.Tooltip = new TextObject("{=gEHEQazX}You're heavily wounded and not fit for the fight. Come back when you're ready.", null);
				args.IsEnabled = false;
			}
			return true;
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000DBBD5 File Offset: 0x000D9DD5
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000DBBD7 File Offset: 0x000D9DD7
		private bool talk_to_leader_of_poachers_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			if (Hero.MainHero.IsWounded)
			{
				args.Tooltip = new TextObject("{=gEHEQazX}You're heavily wounded and not fit for the fight. Come back when you're ready.", null);
				args.IsEnabled = false;
			}
			return true;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000DBC06 File Offset: 0x000D9E06
		private void poachers_menu_back_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000DBC14 File Offset: 0x000D9E14
		private bool ConditionsHold(Hero issueGiver, out Village questVillage)
		{
			questVillage = null;
			if (issueGiver.CurrentSettlement != null)
			{
				questVillage = issueGiver.CurrentSettlement.BoundVillages.GetRandomElementWithPredicate<Village>((Village x) => !x.Settlement.IsUnderRaid && !x.Settlement.IsRaided);
				if (questVillage != null && issueGiver.IsMerchant && issueGiver.GetTraitLevel(DefaultTraits.Mercy) + issueGiver.GetTraitLevel(DefaultTraits.Honor) < 0)
				{
					Town town = issueGiver.CurrentSettlement.Town;
					if (town != null && town.Security <= (float)60)
					{
						return SettlementHelper.FindNearestHideoutToSettlement(questVillage.Settlement, MobileParty.NavigationType.Default, (Settlement x) => x.IsActive) != null;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000DBCDC File Offset: 0x000D9EDC
		private void talk_to_leader_of_poachers_consequence(MenuCallbackArgs args)
		{
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(MerchantArmyOfPoachersIssueBehavior.Instance._poachersParty.Party), MerchantArmyOfPoachersIssueBehavior.Instance._poachersParty.Party, false, false, false, false, false, false));
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000DBD34 File Offset: 0x000D9F34
		public void OnCheckForIssue(Hero hero)
		{
			Village village;
			if (this.ConditionsHold(hero, out village))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue), IssueBase.IssueFrequency.Common, village));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000DBD9C File Offset: 0x000D9F9C
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue(issueOwner, potentialIssueData.RelatedObject as Village);
		}

		// Token: 0x04000F1C RID: 3868
		private const IssueBase.IssueFrequency ArmyOfPoachersIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x04000F1D RID: 3869
		private MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest _cachedQuest;

		// Token: 0x02000772 RID: 1906
		public class MerchantArmyOfPoachersIssue : IssueBase
		{
			// Token: 0x060060B4 RID: 24756 RVA: 0x001C2923 File Offset: 0x001C0B23
			internal static void AutoGeneratedStaticCollectObjectsMerchantArmyOfPoachersIssue(object o, List<object> collectedObjects)
			{
				((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060060B5 RID: 24757 RVA: 0x001C2931 File Offset: 0x001C0B31
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._questVillage);
			}

			// Token: 0x060060B6 RID: 24758 RVA: 0x001C2946 File Offset: 0x001C0B46
			internal static object AutoGeneratedGetMemberValue_questVillage(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue)o)._questVillage;
			}

			// Token: 0x1700130A RID: 4874
			// (get) Token: 0x060060B7 RID: 24759 RVA: 0x001C2953 File Offset: 0x001C0B53
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 12 + MathF.Ceiling(28f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700130B RID: 4875
			// (get) Token: 0x060060B8 RID: 24760 RVA: 0x001C2969 File Offset: 0x001C0B69
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700130C RID: 4876
			// (get) Token: 0x060060B9 RID: 24761 RVA: 0x001C297E File Offset: 0x001C0B7E
			protected override int RewardGold
			{
				get
				{
					return (int)(500f + 3000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700130D RID: 4877
			// (get) Token: 0x060060BA RID: 24762 RVA: 0x001C2993 File Offset: 0x001C0B93
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x1700130E RID: 4878
			// (get) Token: 0x060060BB RID: 24763 RVA: 0x001C2997 File Offset: 0x001C0B97
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=Jk3mDlU6}Yeah... I've got some problems. A few years ago, I needed hides for my tannery and I hired some hunters. I didn't ask too many questions about where they came by the skins they sold me. Well, that was a bit of mistake. Now they've banded together as a gang and are trying to muscle me out of the leather business.[ib:closed2][if:convo_thinking]", null);
				}
			}

			// Token: 0x1700130F RID: 4879
			// (get) Token: 0x060060BC RID: 24764 RVA: 0x001C29A4 File Offset: 0x001C0BA4
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=apuNQC2W}What can I do for you?", null);
				}
			}

			// Token: 0x17001310 RID: 4880
			// (get) Token: 0x060060BD RID: 24765 RVA: 0x001C29B1 File Offset: 0x001C0BB1
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=LbTETjZu}I want you to crush them. Go to {VILLAGE} and give them a lesson they won't forget.[ib:closed2][if:convo_grave]", null);
					textObject.SetTextVariable("VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17001311 RID: 4881
			// (get) Token: 0x060060BE RID: 24766 RVA: 0x001C29DA File Offset: 0x001C0BDA
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=2ELhox6C}If you don't want to get involved in this yourself, leave one of your capable companions and {NUMBER_OF_TROOPS} men for some days.[ib:closed][if:convo_grave]", null);
					textObject.SetTextVariable("NUMBER_OF_TROOPS", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x17001312 RID: 4882
			// (get) Token: 0x060060BF RID: 24767 RVA: 0x001C29F9 File Offset: 0x001C0BF9
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=b6naGx6H}I'll rid you of those poachers myself.", null);
				}
			}

			// Token: 0x17001313 RID: 4883
			// (get) Token: 0x060060C0 RID: 24768 RVA: 0x001C2A06 File Offset: 0x001C0C06
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=lA14Ubal}I can send a companion to hunt these poachers.", null);
				}
			}

			// Token: 0x17001314 RID: 4884
			// (get) Token: 0x060060C1 RID: 24769 RVA: 0x001C2A13 File Offset: 0x001C0C13
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=Xmtlrrmf}Thank you.[ib:normal][if:convo_normal]  Don't forget to warn your men. These poachers are not ordinary bandits. Good luck.", null);
				}
			}

			// Token: 0x17001315 RID: 4885
			// (get) Token: 0x060060C2 RID: 24770 RVA: 0x001C2A20 File Offset: 0x001C0C20
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=51ahPi69}I understand that your men are still chasing those poachers. I realize that this mess might take a little time to clean up.[ib:normal2][if:convo_grave]", null);
				}
			}

			// Token: 0x17001316 RID: 4886
			// (get) Token: 0x060060C3 RID: 24771 RVA: 0x001C2A2D File Offset: 0x001C0C2D
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001317 RID: 4887
			// (get) Token: 0x060060C4 RID: 24772 RVA: 0x001C2A30 File Offset: 0x001C0C30
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001318 RID: 4888
			// (get) Token: 0x060060C5 RID: 24773 RVA: 0x001C2A34 File Offset: 0x001C0C34
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=428B377z}{ISSUE_GIVER.LINK}, a merchant of {QUEST_GIVER_SETTLEMENT}, told you that the poachers {?ISSUE_GIVER.GENDER}she{?}he{\\?} hired are now out of control. You asked {COMPANION.LINK} to take {NEEDED_MEN_COUNT} of your men to go to {QUEST_VILLAGE} and kill the poachers. They should rejoin your party in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("QUEST_VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17001319 RID: 4889
			// (get) Token: 0x060060C6 RID: 24774 RVA: 0x001C2AE1 File Offset: 0x001C0CE1
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=iHFo2kjz}Army of Poachers", null);
				}
			}

			// Token: 0x1700131A RID: 4890
			// (get) Token: 0x060060C7 RID: 24775 RVA: 0x001C2AEE File Offset: 0x001C0CEE
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=NCC4VUOc}{ISSUE_GIVER.LINK} wants you to get rid of the poachers who once worked for {?ISSUE_GIVER.GENDER}her{?}him{\\?} but are now out of control.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, null, false);
					return textObject;
				}
			}

			// Token: 0x060060C8 RID: 24776 RVA: 0x001C2B13 File Offset: 0x001C0D13
			public MerchantArmyOfPoachersIssue(Hero issueOwner, Village questVillage)
				: base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this._questVillage = questVillage;
			}

			// Token: 0x060060C9 RID: 24777 RVA: 0x001C2B2D File Offset: 0x001C0D2D
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return 0.2f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -1f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementLoyalty)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x060060CA RID: 24778 RVA: 0x001C2B6C File Offset: 0x001C0D6C
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x060060CB RID: 24779 RVA: 0x001C2B7D File Offset: 0x001C0D7D
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x060060CC RID: 24780 RVA: 0x001C2B8C File Offset: 0x001C0D8C
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				int skillValue = hero.GetSkillValue(DefaultSkills.Bow);
				int skillValue2 = hero.GetSkillValue(DefaultSkills.Crossbow);
				int skillValue3 = hero.GetSkillValue(DefaultSkills.Throwing);
				if (skillValue >= skillValue2 && skillValue >= skillValue3)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.Bow, 150);
				}
				return new ValueTuple<SkillObject, int>((skillValue2 >= skillValue3) ? DefaultSkills.Crossbow : DefaultSkills.Throwing, 150);
			}

			// Token: 0x060060CD RID: 24781 RVA: 0x001C2BEF File Offset: 0x001C0DEF
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x060060CE RID: 24782 RVA: 0x001C2C09 File Offset: 0x001C0E09
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x060060CF RID: 24783 RVA: 0x001C2C0C File Offset: 0x001C0E0C
			public override bool IssueStayAliveConditions()
			{
				return !this._questVillage.Settlement.IsUnderRaid && !this._questVillage.Settlement.IsRaided && base.IssueOwner.CurrentSettlement.Town.Security <= 90f;
			}

			// Token: 0x060060D0 RID: 24784 RVA: 0x001C2C60 File Offset: 0x001C0E60
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				skill = null;
				relationHero = null;
				requiredGold = 0;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 15)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x060060D1 RID: 24785 RVA: 0x001C2CD1 File Offset: 0x001C0ED1
			protected override void OnGameLoad()
			{
			}

			// Token: 0x060060D2 RID: 24786 RVA: 0x001C2CD3 File Offset: 0x001C0ED3
			protected override void HourlyTick()
			{
			}

			// Token: 0x060060D3 RID: 24787 RVA: 0x001C2CD5 File Offset: 0x001C0ED5
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(20f), this._questVillage, base.IssueDifficultyMultiplier, this.RewardGold);
			}

			// Token: 0x060060D4 RID: 24788 RVA: 0x001C2CFF File Offset: 0x001C0EFF
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x1700131B RID: 4891
			// (get) Token: 0x060060D5 RID: 24789 RVA: 0x001C2D01 File Offset: 0x001C0F01
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x060060D6 RID: 24790 RVA: 0x001C2D16 File Offset: 0x001C0F16
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueOwner.AddPower(30f);
				base.IssueOwner.CurrentSettlement.Town.Prosperity += 50f;
			}

			// Token: 0x060060D7 RID: 24791 RVA: 0x001C2D50 File Offset: 0x001C0F50
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
				base.IssueOwner.AddPower(-50f);
				base.IssueOwner.CurrentSettlement.Town.Prosperity -= 30f;
				base.IssueOwner.CurrentSettlement.Town.Security -= 5f;
				TraitLevelingHelper.OnIssueFailed(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
			}

			// Token: 0x04001EF3 RID: 7923
			private const int AlternativeSolutionTroopTierRequirement = 2;

			// Token: 0x04001EF4 RID: 7924
			private const int CompanionRequiredSkillLevel = 150;

			// Token: 0x04001EF5 RID: 7925
			private const int MinimumRequiredMenCount = 15;

			// Token: 0x04001EF6 RID: 7926
			private const int IssueDuration = 15;

			// Token: 0x04001EF7 RID: 7927
			private const int QuestTimeLimit = 20;

			// Token: 0x04001EF8 RID: 7928
			[SaveableField(10)]
			private Village _questVillage;
		}

		// Token: 0x02000773 RID: 1907
		public class MerchantArmyOfPoachersIssueQuest : QuestBase
		{
			// Token: 0x060060D8 RID: 24792 RVA: 0x001C2DD7 File Offset: 0x001C0FD7
			internal static void AutoGeneratedStaticCollectObjectsMerchantArmyOfPoachersIssueQuest(object o, List<object> collectedObjects)
			{
				((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060060D9 RID: 24793 RVA: 0x001C2DE5 File Offset: 0x001C0FE5
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._poachersParty);
				collectedObjects.Add(this._questVillage);
			}

			// Token: 0x060060DA RID: 24794 RVA: 0x001C2E06 File Offset: 0x001C1006
			internal static object AutoGeneratedGetMemberValue_poachersParty(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._poachersParty;
			}

			// Token: 0x060060DB RID: 24795 RVA: 0x001C2E13 File Offset: 0x001C1013
			internal static object AutoGeneratedGetMemberValue_questVillage(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._questVillage;
			}

			// Token: 0x060060DC RID: 24796 RVA: 0x001C2E20 File Offset: 0x001C1020
			internal static object AutoGeneratedGetMemberValue_talkedToPoachersBattleWillStart(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._talkedToPoachersBattleWillStart;
			}

			// Token: 0x060060DD RID: 24797 RVA: 0x001C2E32 File Offset: 0x001C1032
			internal static object AutoGeneratedGetMemberValue_isReadyToBeFinalized(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._isReadyToBeFinalized;
			}

			// Token: 0x060060DE RID: 24798 RVA: 0x001C2E44 File Offset: 0x001C1044
			internal static object AutoGeneratedGetMemberValue_persuasionTriedOnce(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._persuasionTriedOnce;
			}

			// Token: 0x060060DF RID: 24799 RVA: 0x001C2E56 File Offset: 0x001C1056
			internal static object AutoGeneratedGetMemberValue_difficultyMultiplier(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._difficultyMultiplier;
			}

			// Token: 0x060060E0 RID: 24800 RVA: 0x001C2E68 File Offset: 0x001C1068
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._rewardGold;
			}

			// Token: 0x1700131C RID: 4892
			// (get) Token: 0x060060E1 RID: 24801 RVA: 0x001C2E7A File Offset: 0x001C107A
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=iHFo2kjz}Army of Poachers", null);
				}
			}

			// Token: 0x1700131D RID: 4893
			// (get) Token: 0x060060E2 RID: 24802 RVA: 0x001C2E87 File Offset: 0x001C1087
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700131E RID: 4894
			// (get) Token: 0x060060E3 RID: 24803 RVA: 0x001C2E8C File Offset: 0x001C108C
			private TextObject QuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=fk4ewfQh}{QUEST_GIVER.LINK}, a merchant of {SETTLEMENT}, told you that the poachers {?QUEST_GIVER.GENDER}she{?}he{\\?} hired before are now out of control. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to go to {VILLAGE} around midnight and kill the poachers.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700131F RID: 4895
			// (get) Token: 0x060060E4 RID: 24804 RVA: 0x001C2EF6 File Offset: 0x001C10F6
			private TextObject QuestCanceledTargetVillageRaidedQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=etYq1Tky}{VILLAGE} was raided and the poachers scattered.", null);
					textObject.SetTextVariable("VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17001320 RID: 4896
			// (get) Token: 0x060060E5 RID: 24805 RVA: 0x001C2F20 File Offset: 0x001C1120
			private TextObject QuestCanceledWarDeclared
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001321 RID: 4897
			// (get) Token: 0x060060E6 RID: 24806 RVA: 0x001C2F54 File Offset: 0x001C1154
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001322 RID: 4898
			// (get) Token: 0x060060E7 RID: 24807 RVA: 0x001C2F88 File Offset: 0x001C1188
			private TextObject QuestFailedAfterTalkingWithProachers
			{
				get
				{
					TextObject textObject = new TextObject("{=PIukmFYA}You decided not to get involved and left the village. You have failed to help {QUEST_GIVER.LINK} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001323 RID: 4899
			// (get) Token: 0x060060E8 RID: 24808 RVA: 0x001C2FBA File Offset: 0x001C11BA
			private TextObject QuestSuccessPlayerComesToAnAgreementWithPoachersQuestLogText
			{
				get
				{
					return new TextObject("{=qPfJpwGa}You have persuaded the poachers to leave the district.", null);
				}
			}

			// Token: 0x17001324 RID: 4900
			// (get) Token: 0x060060E9 RID: 24809 RVA: 0x001C2FC8 File Offset: 0x001C11C8
			private TextObject QuestFailWithPlayerDefeatedAgainstPoachersQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=p8Kfl5u6}You lost the battle against the poachers and failed to help {QUEST_GIVER.LINK} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001325 RID: 4901
			// (get) Token: 0x060060EA RID: 24810 RVA: 0x001C2FFC File Offset: 0x001C11FC
			private TextObject QuestSuccessWithPlayerDefeatedPoachersQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=8gNqLqFl}You have defeated the poachers and helped {QUEST_GIVER.LINK} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001326 RID: 4902
			// (get) Token: 0x060060EB RID: 24811 RVA: 0x001C302E File Offset: 0x001C122E
			private TextObject QuestFailedWithTimeOutLogText
			{
				get
				{
					return new TextObject("{=HX7E09XJ}You failed to complete the quest in time.", null);
				}
			}

			// Token: 0x060060EC RID: 24812 RVA: 0x001C303B File Offset: 0x001C123B
			public MerchantArmyOfPoachersIssueQuest(string questId, Hero giverHero, CampaignTime duration, Village questVillage, float difficultyMultiplier, int rewardGold)
				: base(questId, giverHero, duration, rewardGold)
			{
				this._questVillage = questVillage;
				this._talkedToPoachersBattleWillStart = false;
				this._isReadyToBeFinalized = false;
				this._difficultyMultiplier = difficultyMultiplier;
				this._rewardGold = rewardGold;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x060060ED RID: 24813 RVA: 0x001C307C File Offset: 0x001C127C
			private bool SetStartDialogOnCondition()
			{
				if (this._poachersParty != null && CharacterObject.OneToOneConversationCharacter == ConversationHelper.GetConversationCharacterPartyLeader(this._poachersParty.Party))
				{
					MBTextManager.SetTextVariable("POACHER_PARTY_START_LINE", "{=j9MBwnWI}Well...Are you working for that merchant in the town ? So it's all fine when the rich folk trade in poached skins, but if we do it, armed men come to hunt us down.", false);
					if (this._persuasionTriedOnce)
					{
						MBTextManager.SetTextVariable("POACHER_PARTY_START_LINE", "{=Nn06TSq9}Anything else to say?", false);
					}
					return true;
				}
				return false;
			}

			// Token: 0x060060EE RID: 24814 RVA: 0x001C30D4 File Offset: 0x001C12D4
			private DialogFlow GetPoacherPartyDialogFlow()
			{
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=!}{POACHER_PARTY_START_LINE}", null, null, null, null).Condition(() => this.SetStartDialogOnCondition())
					.Consequence(delegate
					{
						this._task = this.GetPersuasionTask();
					})
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=afbLOXbb}Maybe we can come to an agreement.", null, null, null)
					.Condition(() => !this._persuasionTriedOnce)
					.Consequence(delegate
					{
						this._persuasionTriedOnce = true;
					})
					.GotoDialogState("start_poachers_persuasion")
					.PlayerOption("{=mvw1ayGt}I'm here to do the job I agreed to do, outlaw. Give up or die.", null, null, null)
					.NpcLine("{=hOVr77fd}You will never see the sunrise again![ib:warrior][if:convo_furious]", null, null, null, null)
					.Consequence(delegate
					{
						this._talkedToPoachersBattleWillStart = true;
					})
					.CloseDialog()
					.PlayerOption("{=VJYEoOAc}Well... You have a point. Go on. We won't bother you any more.", null, null, null)
					.NpcLine("{=wglTyBbx}Thank you, friend. Go in peace.[ib:normal][if:convo_approving]", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.GameMenuManager.SetNextMenu("village");
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.QuestFailedAfterTalkingWithPoachers;
					})
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x060060EF RID: 24815 RVA: 0x001C31D4 File Offset: 0x001C13D4
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				dialog.AddDialogLine("poachers_persuasion_check_accepted", "start_poachers_persuasion", "poachers_persuasion_start_reservation", "{=6P1ruzsC}Maybe...", new ConversationSentence.OnConditionDelegate(this.persuasion_start_with_poachers_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_poachers_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("poachers_persuasion_rejected", "poachers_persuasion_start_reservation", "start", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.persuasion_failed_with_poachers_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_rejected_with_poachers_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("poachers_persuasion_attempt", "poachers_persuasion_start_reservation", "poachers_persuasion_select_option", "{=wM77S68a}What's there to discuss?", () => !this.persuasion_failed_with_poachers_on_condition(), null, this, 100, null, null, null);
				dialog.AddDialogLine("poachers_persuasion_success", "poachers_persuasion_start_reservation", "close_window", "{=JQKCPllJ}You've made your point.", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.persuasion_complete_with_poachers_on_consequence), this, 200, null, null, null);
				string text = "poachers_persuasion_select_option_1";
				string text2 = "poachers_persuasion_select_option";
				string text3 = "poachers_persuasion_selected_option_response";
				string text4 = "{=!}{POACHERS_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate onConditionDelegate = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(text, text2, text3, text4, onConditionDelegate, onConsequenceDelegate, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text5 = "poachers_persuasion_select_option_2";
				string text6 = "poachers_persuasion_select_option";
				string text7 = "poachers_persuasion_selected_option_response";
				string text8 = "{=!}{POACHERS_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate onConditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_2_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_2);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(text5, text6, text7, text8, onConditionDelegate2, onConsequenceDelegate2, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text9 = "poachers_persuasion_select_option_3";
				string text10 = "poachers_persuasion_select_option";
				string text11 = "poachers_persuasion_selected_option_response";
				string text12 = "{=!}{POACHERS_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate onConditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_3_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_3);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(text9, text10, text11, text12, onConditionDelegate3, onConsequenceDelegate3, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text13 = "poachers_persuasion_select_option_4";
				string text14 = "poachers_persuasion_select_option";
				string text15 = "poachers_persuasion_selected_option_response";
				string text16 = "{=!}{POACHERS_PERSUADE_ATTEMPT_4}";
				ConversationSentence.OnConditionDelegate onConditionDelegate4 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_4_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate4 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_4_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_4);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_4_on_condition);
				dialog.AddPlayerLine(text13, text14, text15, text16, onConditionDelegate4, onConsequenceDelegate4, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text17 = "poachers_persuasion_select_option_5";
				string text18 = "poachers_persuasion_select_option";
				string text19 = "poachers_persuasion_selected_option_response";
				string text20 = "{=!}{POACHERS_PERSUADE_ATTEMPT_5}";
				ConversationSentence.OnConditionDelegate onConditionDelegate5 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_5_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate5 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_5_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_5);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_5_on_condition);
				dialog.AddPlayerLine(text17, text18, text19, text20, onConditionDelegate5, onConsequenceDelegate5, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				dialog.AddDialogLine("poachers_persuasion_select_option_reaction", "poachers_persuasion_selected_option_response", "poachers_persuasion_start_reservation", "{=!}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x060060F0 RID: 24816 RVA: 0x001C349A File Offset: 0x001C169A
			private void persuasion_start_with_poachers_on_consequence()
			{
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.MediumHard);
			}

			// Token: 0x060060F1 RID: 24817 RVA: 0x001C34C0 File Offset: 0x001C16C0
			private bool persuasion_start_with_poachers_on_condition()
			{
				return this._poachersParty != null && CharacterObject.OneToOneConversationCharacter == ConversationHelper.GetConversationCharacterPartyLeader(this._poachersParty.Party);
			}

			// Token: 0x060060F2 RID: 24818 RVA: 0x001C34E4 File Offset: 0x001C16E4
			private PersuasionTask GetPersuasionTask()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=l7Jt5tvt}This is how I earn my living, and all your clever talk doesn't make it any different. Leave now!", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=wM77S68a}What's there to discuss?", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=cQCs72U7}You're not bad people. You can easily ply your trade somewhere else, somewhere safe.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=bioyMrUD}You are just a bunch of hunters. You don't stand a chance against us!", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Mercy, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=FO1oruNy}You talk about poor folk, but you think the people here like their village turned into a nest of outlaws?", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				TextObject textObject = new TextObject("{=S0NeQdLp}You had an agreement with {QUEST_GIVER.NAME}. Your word is your bond, no matter which side of the law you're on.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				PersuasionOptionArgs persuasionOptionArgs4 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Honor, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, textObject, null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs4);
				TextObject textObject2 = new TextObject("{=brW4pjPQ}Flee while you can. An army is already on its way here to hang you all.", null);
				PersuasionOptionArgs persuasionOptionArgs5 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Hard, true, textObject2, null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs5);
				return persuasionTask;
			}

			// Token: 0x060060F3 RID: 24819 RVA: 0x001C361C File Offset: 0x001C181C
			private bool poachers_persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				if (item == PersuasionOptionResult.CriticalFailure)
				{
					this._task.BlockAllOptions();
				}
				return true;
			}

			// Token: 0x060060F4 RID: 24820 RVA: 0x001C365C File Offset: 0x001C185C
			private void poachers_persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.MediumHard);
				float num;
				float num2;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out num, out num2, difficulty);
				this._task.ApplyEffects(num, num2);
			}

			// Token: 0x060060F5 RID: 24821 RVA: 0x001C36B8 File Offset: 0x001C18B8
			private bool poachers_persuasion_select_option_1_on_condition()
			{
				if (this._task.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(0).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060060F6 RID: 24822 RVA: 0x001C3738 File Offset: 0x001C1938
			private bool poachers_persuasion_select_option_2_on_condition()
			{
				if (this._task.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(1).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060060F7 RID: 24823 RVA: 0x001C37B8 File Offset: 0x001C19B8
			private bool poachers_persuasion_select_option_3_on_condition()
			{
				if (this._task.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(2).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060060F8 RID: 24824 RVA: 0x001C3838 File Offset: 0x001C1A38
			private bool poachers_persuasion_select_option_4_on_condition()
			{
				if (this._task.Options.Count > 3)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(3), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(3).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_4", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060060F9 RID: 24825 RVA: 0x001C38B8 File Offset: 0x001C1AB8
			private bool poachers_persuasion_select_option_5_on_condition()
			{
				if (this._task.Options.Count > 4)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(4), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(4).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_5", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060060FA RID: 24826 RVA: 0x001C3938 File Offset: 0x001C1B38
			private void poachers_persuasion_select_option_1_on_consequence()
			{
				if (this._task.Options.Count > 0)
				{
					this._task.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x060060FB RID: 24827 RVA: 0x001C3964 File Offset: 0x001C1B64
			private void poachers_persuasion_select_option_2_on_consequence()
			{
				if (this._task.Options.Count > 1)
				{
					this._task.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x060060FC RID: 24828 RVA: 0x001C3990 File Offset: 0x001C1B90
			private void poachers_persuasion_select_option_3_on_consequence()
			{
				if (this._task.Options.Count > 2)
				{
					this._task.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x060060FD RID: 24829 RVA: 0x001C39BC File Offset: 0x001C1BBC
			private void poachers_persuasion_select_option_4_on_consequence()
			{
				if (this._task.Options.Count > 3)
				{
					this._task.Options[3].BlockTheOption(true);
				}
			}

			// Token: 0x060060FE RID: 24830 RVA: 0x001C39E8 File Offset: 0x001C1BE8
			private void poachers_persuasion_select_option_5_on_consequence()
			{
				if (this._task.Options.Count > 4)
				{
					this._task.Options[4].BlockTheOption(true);
				}
			}

			// Token: 0x060060FF RID: 24831 RVA: 0x001C3A14 File Offset: 0x001C1C14
			private bool persuasion_failed_with_poachers_on_condition()
			{
				if (this._task.Options.All<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._task.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x06006100 RID: 24832 RVA: 0x001C3A72 File Offset: 0x001C1C72
			private PersuasionOptionArgs poachers_persuasion_setup_option_1()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(0);
			}

			// Token: 0x06006101 RID: 24833 RVA: 0x001C3A85 File Offset: 0x001C1C85
			private PersuasionOptionArgs poachers_persuasion_setup_option_2()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(1);
			}

			// Token: 0x06006102 RID: 24834 RVA: 0x001C3A98 File Offset: 0x001C1C98
			private PersuasionOptionArgs poachers_persuasion_setup_option_3()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(2);
			}

			// Token: 0x06006103 RID: 24835 RVA: 0x001C3AAB File Offset: 0x001C1CAB
			private PersuasionOptionArgs poachers_persuasion_setup_option_4()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(3);
			}

			// Token: 0x06006104 RID: 24836 RVA: 0x001C3ABE File Offset: 0x001C1CBE
			private PersuasionOptionArgs poachers_persuasion_setup_option_5()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(4);
			}

			// Token: 0x06006105 RID: 24837 RVA: 0x001C3AD4 File Offset: 0x001C1CD4
			private bool poachers_persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 0)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x06006106 RID: 24838 RVA: 0x001C3B3C File Offset: 0x001C1D3C
			private bool poachers_persuasion_clickable_option_2_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 1)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked;
				}
				return false;
			}

			// Token: 0x06006107 RID: 24839 RVA: 0x001C3BA4 File Offset: 0x001C1DA4
			private bool poachers_persuasion_clickable_option_3_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 2)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked;
				}
				return false;
			}

			// Token: 0x06006108 RID: 24840 RVA: 0x001C3C0C File Offset: 0x001C1E0C
			private bool poachers_persuasion_clickable_option_4_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 3)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(3).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(3).IsBlocked;
				}
				return false;
			}

			// Token: 0x06006109 RID: 24841 RVA: 0x001C3C74 File Offset: 0x001C1E74
			private bool poachers_persuasion_clickable_option_5_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 4)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(4).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(4).IsBlocked;
				}
				return false;
			}

			// Token: 0x0600610A RID: 24842 RVA: 0x001C3CDB File Offset: 0x001C1EDB
			private void persuasion_rejected_with_poachers_on_consequence()
			{
				PlayerEncounter.LeaveEncounter = false;
				ConversationManager.EndPersuasion();
			}

			// Token: 0x0600610B RID: 24843 RVA: 0x001C3CE8 File Offset: 0x001C1EE8
			private void persuasion_complete_with_poachers_on_consequence()
			{
				PlayerEncounter.LeaveEncounter = true;
				ConversationManager.EndPersuasion();
				Campaign.Current.GameMenuManager.SetNextMenu("village");
				Campaign.Current.ConversationManager.ConversationEndOneShot += this.QuestSuccessPlayerComesToAnAgreementWithPoachers;
			}

			// Token: 0x0600610C RID: 24844 RVA: 0x001C3D24 File Offset: 0x001C1F24
			internal void StartQuestBattle()
			{
				PlayerEncounter.RestartPlayerEncounter(PartyBase.MainParty, this._poachersParty.Party, false, false);
				PlayerEncounter.StartBattle();
				PlayerEncounter.Update();
				this._talkedToPoachersBattleWillStart = false;
				MapEvent.PlayerMapEvent.AttackerSide.RemoveNearbyPartiesFromPlayerMapEvent();
				MapEvent.PlayerMapEvent.DefenderSide.RemoveNearbyPartiesFromPlayerMapEvent();
				GameMenu.ActivateGameMenu("army_of_poachers_village");
				CampaignMission.OpenBattleMission(this._questVillage.Settlement.LocationComplex.GetScene("village_center", 1), false, "land_raid");
				this._isReadyToBeFinalized = true;
			}

			// Token: 0x0600610D RID: 24845 RVA: 0x001C3DB0 File Offset: 0x001C1FB0
			private bool DialogCondition()
			{
				return Hero.OneToOneConversationHero == base.QuestGiver;
			}

			// Token: 0x0600610E RID: 24846 RVA: 0x001C3DC0 File Offset: 0x001C1FC0
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=IefM6uAy}Thank you. You'll be paid well. Also you can keep their illegally obtained leather.[ib:normal2][if:convo_bemused]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition))
					.NpcLine(new TextObject("{=NC2VGafO}They skin their beasts in the woods, then go into the village after midnight to stash the hides. The villagers are terrified of them, I believe. If you go into the village late at night, you should be able to track them down.[ib:normal][if:convo_thinking]", null), null, null, null, null)
					.NpcLine(new TextObject("{=3pkVKMnA}Most poachers would probably run if they were surprised by armed men. But these ones are bold and desperate. Be ready for a fight.[ib:normal2][if:convo_undecided_closed]", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=QNV1b5s5}Are those poachers still in business?[ib:normal2][if:convo_undecided_open]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition))
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=JhJBBWab}They will be gone soon.", null), null, null, null)
					.NpcLine(new TextObject("{=gjGb044I}I hope they will be...[ib:normal2][if:convo_dismayed]", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=Gu3jF88V}Any night battle can easily go wrong. I need more time to prepare.", null), null, null, null)
					.NpcLine(new TextObject("{=2EiC1YyZ}Well, if they get wind of what you're up to, things could go very wrong for me. Do be quick.[ib:nervous2][if:convo_dismayed]", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions();
				this.QuestCharacterDialogFlow = this.GetPoacherPartyDialogFlow();
			}

			// Token: 0x0600610F RID: 24847 RVA: 0x001C3EEC File Offset: 0x001C20EC
			internal void CreatePoachersParty()
			{
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.Default, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefaultQ<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				this._poachersParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._questVillage.Settlement.GatePosition, 1f, null, new TextObject("{=WQa1R55u}Poachers Party", null), clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), null, "", "", 0f, false);
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("leather");
				int num = MathF.Ceiling(this._difficultyMultiplier * 5f) + MBRandom.RandomInt(0, 2);
				this._poachersParty.ItemRoster.AddToCounts(@object, num * 2);
				CharacterObject characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "poacher");
				int num2 = 10 + MathF.Ceiling(40f * this._difficultyMultiplier);
				this._poachersParty.MemberRoster.AddToCounts(characterObject, num2, false, 0, 0, true, -1);
				this._poachersParty.SetPartyUsedByQuest(true);
				this._poachersParty.Ai.DisableAi();
				EnterSettlementAction.ApplyForParty(this._poachersParty, Settlement.CurrentSettlement);
			}

			// Token: 0x06006110 RID: 24848 RVA: 0x001C4054 File Offset: 0x001C2254
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this.QuestStartedLogText, false);
				base.AddTrackedObject(this._questVillage.Settlement);
			}

			// Token: 0x06006111 RID: 24849 RVA: 0x001C407C File Offset: 0x001C227C
			internal void QuestFailedAfterTalkingWithPoachers()
			{
				base.AddLog(this.QuestFailedAfterTalkingWithProachers, false);
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50),
					new Tuple<TraitObject, int>(DefaultTraits.Mercy, 20)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-50f);
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 30f;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06006112 RID: 24850 RVA: 0x001C4128 File Offset: 0x001C2328
			internal void QuestSuccessPlayerComesToAnAgreementWithPoachers()
			{
				base.AddLog(this.QuestSuccessPlayerComesToAnAgreementWithPoachersQuestLogText, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 10),
					new Tuple<TraitObject, int>(DefaultTraits.Mercy, 50)
				});
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				this.RelationshipChangeWithQuestGiver = 5;
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				base.QuestGiver.AddPower(30f);
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				base.QuestGiver.CurrentSettlement.Town.Prosperity += 50f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06006113 RID: 24851 RVA: 0x001C41F4 File Offset: 0x001C23F4
			internal void QuestFailWithPlayerDefeatedAgainstPoachers()
			{
				base.AddLog(this.QuestFailWithPlayerDefeatedAgainstPoachersQuestLogText, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-50f);
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 30f;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06006114 RID: 24852 RVA: 0x001C4290 File Offset: 0x001C2490
			internal void QuestSuccessWithPlayerDefeatedPoachers()
			{
				base.AddLog(this.QuestSuccessWithPlayerDefeatedPoachersQuestLogText, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				this.RelationshipChangeWithQuestGiver = 5;
				base.QuestGiver.AddPower(30f);
				base.QuestGiver.CurrentSettlement.Town.Prosperity += 50f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06006115 RID: 24853 RVA: 0x001C431C File Offset: 0x001C251C
			protected override void OnTimedOut()
			{
				base.AddLog(this.QuestFailedWithTimeOutLogText, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-50f);
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 30f;
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
			}

			// Token: 0x06006116 RID: 24854 RVA: 0x001C43B1 File Offset: 0x001C25B1
			private void QuestCanceledTargetVillageRaided()
			{
				base.AddLog(this.QuestCanceledTargetVillageRaidedQuestLogText, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x06006117 RID: 24855 RVA: 0x001C43C8 File Offset: 0x001C25C8
			protected override void RegisterEvents()
			{
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventCheck));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.MapEventStarted));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.GameMenuOpened));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.CanHeroBecomePrisonerEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.OnCanHeroBecomePrisonerInfoIsRequested));
			}

			// Token: 0x06006118 RID: 24856 RVA: 0x001C445F File Offset: 0x001C265F
			private void OnCanHeroBecomePrisonerInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == Hero.MainHero && this._isReadyToBeFinalized)
				{
					result = false;
				}
			}

			// Token: 0x06006119 RID: 24857 RVA: 0x001C4474 File Offset: 0x001C2674
			protected override void HourlyTick()
			{
				if (PlayerEncounter.Current != null && PlayerEncounter.Current.IsPlayerWaiting && PlayerEncounter.EncounterSettlement == this._questVillage.Settlement && CampaignTime.Now.IsNightTime && !this._isReadyToBeFinalized && base.IsOngoing)
				{
					EnterSettlementAction.ApplyForParty(MobileParty.MainParty, this._questVillage.Settlement);
					GameMenu.SwitchToMenu("army_of_poachers_village");
				}
			}

			// Token: 0x0600611A RID: 24858 RVA: 0x001C44E4 File Offset: 0x001C26E4
			private void GameMenuOpened(MenuCallbackArgs obj)
			{
				if (obj.MenuContext.GameMenu.StringId == "village" && CampaignTime.Now.IsNightTime && Settlement.CurrentSettlement == this._questVillage.Settlement && !this._isReadyToBeFinalized)
				{
					GameMenu.SwitchToMenu("army_of_poachers_village");
				}
				if (obj.MenuContext.GameMenu.StringId == "army_of_poachers_village" && this._isReadyToBeFinalized && MapEvent.PlayerMapEvent != null && MapEvent.PlayerMapEvent.HasWinner && this._poachersParty != null)
				{
					this._poachersParty.IsVisible = false;
				}
			}

			// Token: 0x0600611B RID: 24859 RVA: 0x001C458B File Offset: 0x001C278B
			private void MapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				this.MapEventCheck(mapEvent);
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x0600611C RID: 24860 RVA: 0x001C45A5 File Offset: 0x001C27A5
			private void MapEventCheck(MapEvent mapEvent)
			{
				if (mapEvent.IsRaid && mapEvent.MapEventSettlement == this._questVillage.Settlement)
				{
					this.QuestCanceledTargetVillageRaided();
				}
			}

			// Token: 0x0600611D RID: 24861 RVA: 0x001C45C8 File Offset: 0x001C27C8
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclared);
				}
			}

			// Token: 0x0600611E RID: 24862 RVA: 0x001C45F7 File Offset: 0x001C27F7
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclared, false);
			}

			// Token: 0x0600611F RID: 24863 RVA: 0x001C4610 File Offset: 0x001C2810
			protected override void OnFinalize()
			{
				if (this._poachersParty != null && this._poachersParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._poachersParty);
				}
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
				}
				if (Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu.StringId == "army_of_poachers_village")
				{
					PlayerEncounter.Finish(true);
				}
			}

			// Token: 0x06006120 RID: 24864 RVA: 0x001C4687 File Offset: 0x001C2887
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x04001EF9 RID: 7929
			[SaveableField(10)]
			internal MobileParty _poachersParty;

			// Token: 0x04001EFA RID: 7930
			[SaveableField(20)]
			internal Village _questVillage;

			// Token: 0x04001EFB RID: 7931
			[SaveableField(30)]
			internal bool _talkedToPoachersBattleWillStart;

			// Token: 0x04001EFC RID: 7932
			[SaveableField(40)]
			internal bool _isReadyToBeFinalized;

			// Token: 0x04001EFD RID: 7933
			[SaveableField(50)]
			internal bool _persuasionTriedOnce;

			// Token: 0x04001EFE RID: 7934
			[SaveableField(60)]
			internal float _difficultyMultiplier;

			// Token: 0x04001EFF RID: 7935
			[SaveableField(70)]
			internal int _rewardGold;

			// Token: 0x04001F00 RID: 7936
			private PersuasionTask _task;

			// Token: 0x04001F01 RID: 7937
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.MediumHard;
		}

		// Token: 0x02000774 RID: 1908
		public class MerchantArmyOfPoachersIssueBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006128 RID: 24872 RVA: 0x001C46FE File Offset: 0x001C28FE
			public MerchantArmyOfPoachersIssueBehaviorTypeDefiner()
				: base(800000)
			{
			}

			// Token: 0x06006129 RID: 24873 RVA: 0x001C470B File Offset: 0x001C290B
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue), 1, null);
				base.AddClassDefinition(typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest), 2, null);
			}
		}
	}
}
