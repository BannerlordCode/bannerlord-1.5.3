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
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000382 RID: 898
	public class LandLordCompanyOfTroubleIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x0600356C RID: 13676 RVA: 0x000DA480 File Offset: 0x000D8680
		private static LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest Instance
		{
			get
			{
				LandLordCompanyOfTroubleIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<LandLordCompanyOfTroubleIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest landLordCompanyOfTroubleIssueQuest;
						if ((landLordCompanyOfTroubleIssueQuest = enumerator.Current as LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest) != null)
						{
							campaignBehavior._cachedQuest = landLordCompanyOfTroubleIssueQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000DA518 File Offset: 0x000D8718
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000DA548 File Offset: 0x000D8748
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("company_of_trouble_menu", "", new OnInitDelegate(this.company_of_trouble_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000DA56C File Offset: 0x000D876C
		private void company_of_trouble_menu_on_init(MenuCallbackArgs args)
		{
			if (LandLordCompanyOfTroubleIssueBehavior.Instance != null)
			{
				if (LandLordCompanyOfTroubleIssueBehavior.Instance._checkForBattleResults)
				{
					bool flag = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide;
					PlayerEncounter.Finish(true);
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty != null && LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty.IsActive)
					{
						DestroyPartyAction.Apply(null, LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty);
					}
					LandLordCompanyOfTroubleIssueBehavior.Instance._checkForBattleResults = false;
					if (flag)
					{
						LandLordCompanyOfTroubleIssueBehavior.Instance.QuestSuccessWithPlayerDefeatedCompany();
						return;
					}
					LandLordCompanyOfTroubleIssueBehavior.Instance.QuestFailWithPlayerDefeatedAgainstCompany();
					return;
				}
				else
				{
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._triggerCompanyOfTroubleConversation)
					{
						CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(LandLordCompanyOfTroubleIssueBehavior.Instance._troubleCharacterObject, PartyBase.MainParty, false, false, false, false, false, false));
						LandLordCompanyOfTroubleIssueBehavior.Instance._triggerCompanyOfTroubleConversation = false;
						return;
					}
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._battleWillStart)
					{
						PlayerEncounter.Start();
						PlayerEncounter.Current.SetupFields(PartyBase.MainParty, LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty.Party);
						PlayerEncounter.StartBattle();
						IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
						CampaignVec2 position = MobileParty.MainParty.Position;
						MapPatchData mapPatchAtPosition = mapSceneWrapper.GetMapPatchAtPosition(in position);
						CampaignMission.OpenBattleMission(Campaign.Current.Models.SceneModel.GetBattleSceneForMapPatch(mapPatchAtPosition, PlayerEncounter.IsNavalEncounter()), false, "");
						LandLordCompanyOfTroubleIssueBehavior.Instance._battleWillStart = false;
						LandLordCompanyOfTroubleIssueBehavior.Instance._checkForBattleResults = true;
						return;
					}
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._companyLeftQuestWillFail)
					{
						LandLordCompanyOfTroubleIssueBehavior.Instance.CompanyLeftQuestFail();
					}
				}
			}
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000DA6EF File Offset: 0x000D88EF
		[GameMenuInitializationHandler("company_of_trouble_menu")]
		public static void company_of_trouble_menu_game_menu_on_init_background(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_ambush");
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000DA704 File Offset: 0x000D8904
		public void OnCheckForIssue(Hero hero)
		{
			if (hero.IsLord && hero.Clan != Clan.PlayerClan && hero.PartyBelongedTo != null && !hero.IsMinorFactionHero && hero.GetTraitLevel(DefaultTraits.Mercy) <= 0)
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000DA792 File Offset: 0x000D8992
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue(issueOwner);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000DA79A File Offset: 0x000D899A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000F09 RID: 3849
		private const IssueBase.IssueFrequency LandLordCompanyOfTroubleIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000F0A RID: 3850
		private LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest _cachedQuest;

		// Token: 0x04000F0B RID: 3851
		private const int IssueDuration = 25;

		// Token: 0x02000751 RID: 1873
		public class LandLordCompanyOfTroubleIssue : IssueBase
		{
			// Token: 0x06005D06 RID: 23814 RVA: 0x001B35D9 File Offset: 0x001B17D9
			internal static void AutoGeneratedStaticCollectObjectsLandLordCompanyOfTroubleIssue(object o, List<object> collectedObjects)
			{
				((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005D07 RID: 23815 RVA: 0x001B35E7 File Offset: 0x001B17E7
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x170011E3 RID: 4579
			// (get) Token: 0x06005D08 RID: 23816 RVA: 0x001B35F0 File Offset: 0x001B17F0
			private int CompanyTroopCount
			{
				get
				{
					return 5 + (int)(base.IssueDifficultyMultiplier * 30f);
				}
			}

			// Token: 0x170011E4 RID: 4580
			// (get) Token: 0x06005D09 RID: 23817 RVA: 0x001B3601 File Offset: 0x001B1801
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011E5 RID: 4581
			// (get) Token: 0x06005D0A RID: 23818 RVA: 0x001B3604 File Offset: 0x001B1804
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011E6 RID: 4582
			// (get) Token: 0x06005D0B RID: 23819 RVA: 0x001B3607 File Offset: 0x001B1807
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=wrpsJM2u}Yes... I hired a band of mercenaries for a campaign some time back. But... normally mercenaries have their own peculiar kind of honor. You pay them, they fight for you, you don't, they go somewhere else. But these ones have made it pretty clear that if I don't keep renewing the contract, they'll turn bandit. I can't afford that right now.[if:convo_thinking][ib:closed]", null);
				}
			}

			// Token: 0x170011E7 RID: 4583
			// (get) Token: 0x06005D0C RID: 23820 RVA: 0x001B3614 File Offset: 0x001B1814
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=VlbCFDWu}What do you want from me?", null);
				}
			}

			// Token: 0x170011E8 RID: 4584
			// (get) Token: 0x06005D0D RID: 23821 RVA: 0x001B3621 File Offset: 0x001B1821
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=wxDbPiNH}Well, you have the reputation of being able to manage ruffians. Maybe you can take them off my hands, find some other lord who has more need of them and more denars to pay them. I've paid their contract for a few months. I can give you a small reward and if you can find a buyer, you can transfer the rest of the contract to him and pocket the down payment.[if:convo_innocent_smile]", null);
				}
			}

			// Token: 0x170011E9 RID: 4585
			// (get) Token: 0x06005D0E RID: 23822 RVA: 0x001B362E File Offset: 0x001B182E
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=6bvJSIqh}Yes. I can find a new lord to take them on.", null);
				}
			}

			// Token: 0x170011EA RID: 4586
			// (get) Token: 0x06005D0F RID: 23823 RVA: 0x001B363B File Offset: 0x001B183B
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=PV7RHgUl}Company of Trouble", null);
				}
			}

			// Token: 0x170011EB RID: 4587
			// (get) Token: 0x06005D10 RID: 23824 RVA: 0x001B3648 File Offset: 0x001B1848
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=zw7a9eIt}{ISSUE_GIVER.NAME} wants you to take {?ISSUE_GIVER.GENDER}her{?}his{\\?} mercenaries and transfer them to another lord before they cause any trouble.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011EC RID: 4588
			// (get) Token: 0x06005D11 RID: 23825 RVA: 0x001B367C File Offset: 0x001B187C
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=I022Z9Ub}Heh. {QUEST_GIVER.NAME} got in deeper than {?QUEST_GIVER.GENDER}she{?}he{\\?} could handle with those mercenaries.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06005D12 RID: 23826 RVA: 0x001B36AE File Offset: 0x001B18AE
			public LandLordCompanyOfTroubleIssue(Hero issueOwner)
				: base(issueOwner, CampaignTime.DaysFromNow(25f))
			{
			}

			// Token: 0x06005D13 RID: 23827 RVA: 0x001B36C1 File Offset: 0x001B18C1
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x06005D14 RID: 23828 RVA: 0x001B36D6 File Offset: 0x001B18D6
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005D15 RID: 23829 RVA: 0x001B36D8 File Offset: 0x001B18D8
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005D16 RID: 23830 RVA: 0x001B36DA File Offset: 0x001B18DA
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest(questId, base.IssueOwner, CampaignTime.Never, this.CompanyTroopCount);
			}

			// Token: 0x06005D17 RID: 23831 RVA: 0x001B36F3 File Offset: 0x001B18F3
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06005D18 RID: 23832 RVA: 0x001B36F8 File Offset: 0x001B18F8
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
				if (Clan.PlayerClan.Tier < 1)
				{
					flag |= IssueBase.PreconditionFlags.ClanTier;
				}
				if (MobileParty.MainParty.MemberRoster.TotalManCount < this.CompanyTroopCount)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (MobileParty.MainParty.MemberRoster.TotalManCount + this.CompanyTroopCount > PartyBase.MainParty.PartySizeLimit)
				{
					flag |= IssueBase.PreconditionFlags.PartySizeLimit;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005D19 RID: 23833 RVA: 0x001B37B0 File Offset: 0x001B19B0
			public override bool IssueStayAliveConditions()
			{
				return base.IssueOwner.Clan != Clan.PlayerClan;
			}

			// Token: 0x06005D1A RID: 23834 RVA: 0x001B37C7 File Offset: 0x001B19C7
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}
		}

		// Token: 0x02000752 RID: 1874
		public class LandLordCompanyOfTroubleIssueQuest : QuestBase
		{
			// Token: 0x06005D1B RID: 23835 RVA: 0x001B37C9 File Offset: 0x001B19C9
			internal static void AutoGeneratedStaticCollectObjectsLandLordCompanyOfTroubleIssueQuest(object o, List<object> collectedObjects)
			{
				((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005D1C RID: 23836 RVA: 0x001B37D7 File Offset: 0x001B19D7
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._companyOfTroubleParty);
				collectedObjects.Add(this._persuationTriedHeroesList);
			}

			// Token: 0x06005D1D RID: 23837 RVA: 0x001B37F8 File Offset: 0x001B19F8
			internal static object AutoGeneratedGetMemberValue_companyOfTroubleParty(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._companyOfTroubleParty;
			}

			// Token: 0x06005D1E RID: 23838 RVA: 0x001B3805 File Offset: 0x001B1A05
			internal static object AutoGeneratedGetMemberValue_battleWillStart(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._battleWillStart;
			}

			// Token: 0x06005D1F RID: 23839 RVA: 0x001B3817 File Offset: 0x001B1A17
			internal static object AutoGeneratedGetMemberValue_triggerCompanyOfTroubleConversation(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._triggerCompanyOfTroubleConversation;
			}

			// Token: 0x06005D20 RID: 23840 RVA: 0x001B3829 File Offset: 0x001B1A29
			internal static object AutoGeneratedGetMemberValue_thieveryCount(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._thieveryCount;
			}

			// Token: 0x06005D21 RID: 23841 RVA: 0x001B383B File Offset: 0x001B1A3B
			internal static object AutoGeneratedGetMemberValue_demandGold(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._demandGold;
			}

			// Token: 0x06005D22 RID: 23842 RVA: 0x001B384D File Offset: 0x001B1A4D
			internal static object AutoGeneratedGetMemberValue_persuationTriedHeroesList(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._persuationTriedHeroesList;
			}

			// Token: 0x170011ED RID: 4589
			// (get) Token: 0x06005D23 RID: 23843 RVA: 0x001B385A File Offset: 0x001B1A5A
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170011EE RID: 4590
			// (get) Token: 0x06005D24 RID: 23844 RVA: 0x001B385D File Offset: 0x001B1A5D
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=PV7RHgUl}Company of Trouble", null);
				}
			}

			// Token: 0x170011EF RID: 4591
			// (get) Token: 0x06005D25 RID: 23845 RVA: 0x001B386C File Offset: 0x001B1A6C
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=8nS3QgD7}{QUEST_GIVER.LINK} is a {?QUEST_GIVER.GENDER}lady{?}lord{\\?} who told you that {?QUEST_GIVER.GENDER}she{?}he{\\?} wants to sell {?QUEST_GIVER.GENDER}her{?}his{\\?} mercenaries to another lord's service. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you sell them for {?QUEST_GIVER.GENDER}her{?}him{\\?} without causing any trouble.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011F0 RID: 4592
			// (get) Token: 0x06005D26 RID: 23846 RVA: 0x001B38A0 File Offset: 0x001B1AA0
			private TextObject QuestSuccessPlayerSoldCompany
			{
				get
				{
					TextObject textObject = new TextObject("{=34MdCd6u}You have sold the mercenaries to another lord as you promised. {QUEST_GIVER.LINK} is grateful and sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011F1 RID: 4593
			// (get) Token: 0x06005D27 RID: 23847 RVA: 0x001B38D4 File Offset: 0x001B1AD4
			private TextObject AllCompanyDiedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=RrTAX7QE}You got the troublesome mercenaries killed off. You get no extra money for the contract, but you did get rid of them as you promised. {QUEST_GIVER.LINK} is grateful and sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011F2 RID: 4594
			// (get) Token: 0x06005D28 RID: 23848 RVA: 0x001B3906 File Offset: 0x001B1B06
			private TextObject PlayerDefeatedAgainstCompany
			{
				get
				{
					return new TextObject("{=7naLQmq1}You have lost the battle against the mercenaries. You have failed to get rid of them as you promised. Now they've turned bandit and are starting to plunder the countryside", null);
				}
			}

			// Token: 0x170011F3 RID: 4595
			// (get) Token: 0x06005D29 RID: 23849 RVA: 0x001B3913 File Offset: 0x001B1B13
			private TextObject QuestFailCompanyLeft
			{
				get
				{
					return new TextObject("{=k9SksaXg}The mercenaries left your party, as you failed to get rid of them as you promised. Now the mercenaries have turned bandit and start to plunder countryside.", null);
				}
			}

			// Token: 0x170011F4 RID: 4596
			// (get) Token: 0x06005D2A RID: 23850 RVA: 0x001B3920 File Offset: 0x001B1B20
			private TextObject QuestCanceledWarDeclared
			{
				get
				{
					TextObject textObject = new TextObject("{=ItueKmqd}Your clan is now at war with the {QUEST_GIVER_SETTLEMENT_FACTION}. You contract with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT_FACTION", base.QuestGiver.MapFaction.InformalName);
					return textObject;
				}
			}

			// Token: 0x170011F5 RID: 4597
			// (get) Token: 0x06005D2B RID: 23851 RVA: 0x001B3970 File Offset: 0x001B1B70
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06005D2C RID: 23852 RVA: 0x001B39A4 File Offset: 0x001B1BA4
			public LandLordCompanyOfTroubleIssueQuest(string questId, Hero questGiver, CampaignTime duration, int companyTroopCount)
				: base(questId, questGiver, duration, 500)
			{
				this._troubleCharacterObject = MBObjectManager.Instance.GetObject<CharacterObject>("company_of_trouble_character");
				this._persuationTriedHeroesList = new List<Hero>();
				this._troubleCharacterObject.SetTransferableInPartyScreen(false);
				this._troubleCharacterObject.SetTransferableInHideouts(false);
				this._companyTroopCount = companyTroopCount;
				this._tasks = new PersuasionTask[3];
				this._battleWillStart = false;
				this._thieveryCount = 0;
				this._demandGold = 0;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06005D2D RID: 23853 RVA: 0x001B3A2C File Offset: 0x001B1C2C
			protected override void InitializeQuestOnGameLoad()
			{
				this._troubleCharacterObject = MBObjectManager.Instance.GetObject<CharacterObject>("company_of_trouble_character");
				this._troubleCharacterObject.SetTransferableInPartyScreen(false);
				this._troubleCharacterObject.SetTransferableInHideouts(false);
				this._tasks = new PersuasionTask[3];
				this.UpdateCompanyTroopCount();
				this.SetDialogs();
			}

			// Token: 0x06005D2E RID: 23854 RVA: 0x001B3A80 File Offset: 0x001B1C80
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=T6d7wtJX}Very well. I'll tell them to join your party. Good luck.[if:convo_mocking_aristocratic][ib:hip]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=bWpLYiEg}Did you ever find a way to handle those mercenaries?[if:convo_astonished]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
					})
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=XzK4niIb}I'll find an employer soon.", null), null, null, null)
					.NpcLine(new TextObject("{=rOBRabQz}Good. I'm waiting for your good news.[if:convo_mocking_aristocratic]", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=Zb3EdxDT}That kind of lord is hard to find.", null), null, null, null)
					.NpcLine(new TextObject("{=yOfrb9Lu}Don't wait too long. These are dangerous men. Be careful.[if:convo_nonchalant]", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCompanyDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetOtherLordsDialogFlow(), this);
			}

			// Token: 0x06005D2F RID: 23855 RVA: 0x001B3BCC File Offset: 0x001B1DCC
			private DialogFlow GetOtherLordsDialogFlow()
			{
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("hero_main_options", 700).BeginPlayerOptions(null, false).PlayerOption(new TextObject("{=2E7s4L9R}Do you need mercenaries? I have a contract that I can transfer to you for {DEMAND_GOLD} denars.", null), null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.PersuasionDialogForLordGeneralCondition))
					.BeginNpcOptions(null, false)
					.NpcOption(new TextObject("{=ZR4RJdYS}Hmm, that sounds interesting...[if:convo_thinking]", null), new ConversationSentence.OnConditionDelegate(this.PersuasionDialogSpecialCondition), null, null, null, null)
					.GotoDialogState("company_of_trouble_persuasion")
					.NpcOption(new TextObject("{=pmrjUNEz}As it happens, I already have a mercenary contract that I wish to sell. So, no thank you.[if:convo_calm_friendly]", null), new ConversationSentence.OnConditionDelegate(this.HasSameIssue), null, null, null, null)
					.GotoDialogState("hero_main_options")
					.NpcOption(new TextObject("{=bw0hEPN6}You already bought their contract from our clan. Why would I want to buy them back?[if:convo_confused_normal]", null), new ConversationSentence.OnConditionDelegate(this.IsSameClanMember), null, null, null, null)
					.GotoDialogState("hero_main_options")
					.NpcOption(new TextObject("{=64bH4bUo}No, thank you. But perhaps one of the other lords of our clan would be interested.[if:convo_undecided_closed]", null), () => !this.HasMobileParty(), null, null, null, null)
					.GotoDialogState("hero_main_options")
					.NpcOption(new TextObject("{=Zs6L1aBL}I'm sorry. I don't need mercenaries right now.[if:convo_normal]", null), null, null, null, null, null)
					.GotoDialogState("hero_main_options")
					.EndNpcOptions()
					.EndPlayerOptions();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x06005D30 RID: 23856 RVA: 0x001B3CF4 File Offset: 0x001B1EF4
			private bool PersuasionDialogSpecialCondition()
			{
				return !this.IsSameClanMember() && !this.HasSameIssue() && this.HasMobileParty() && !this.InSameSettlement();
			}

			// Token: 0x06005D31 RID: 23857 RVA: 0x001B3D19 File Offset: 0x001B1F19
			private bool HasMobileParty()
			{
				return Hero.OneToOneConversationHero.PartyBelongedTo != null;
			}

			// Token: 0x06005D32 RID: 23858 RVA: 0x001B3D28 File Offset: 0x001B1F28
			private bool IsSameClanMember()
			{
				return Hero.OneToOneConversationHero.Clan == base.QuestGiver.Clan;
			}

			// Token: 0x06005D33 RID: 23859 RVA: 0x001B3D41 File Offset: 0x001B1F41
			private bool InSameSettlement()
			{
				return Hero.OneToOneConversationHero.CurrentSettlement != null && base.QuestGiver.CurrentSettlement != null && Hero.OneToOneConversationHero.CurrentSettlement == base.QuestGiver.CurrentSettlement;
			}

			// Token: 0x06005D34 RID: 23860 RVA: 0x001B3D75 File Offset: 0x001B1F75
			private bool HasSameIssue()
			{
				IssueBase issue = Hero.OneToOneConversationHero.Issue;
				return ((issue != null) ? issue.GetType() : null) == base.GetType();
			}

			// Token: 0x06005D35 RID: 23861 RVA: 0x001B3D98 File Offset: 0x001B1F98
			private bool PersuasionDialogForLordGeneralCondition()
			{
				if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.IsLord && Hero.OneToOneConversationHero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && Hero.OneToOneConversationHero != base.QuestGiver && !Hero.OneToOneConversationHero.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction) && Hero.OneToOneConversationHero.Clan != Clan.PlayerClan && !this._persuationTriedHeroesList.Contains(Hero.OneToOneConversationHero))
				{
					this.UpdateCompanyTroopCount();
					this._demandGold = 1000 + this._companyTroopCount * 150;
					MBTextManager.SetTextVariable("DEMAND_GOLD", this._demandGold);
					this._tasks[0] = this.GetPersuasionTask1();
					this._tasks[1] = this.GetPersuasionTask2();
					this._tasks[2] = this.GetPersuasionTask3();
					this._selectedTask = this._tasks.GetRandomElement<PersuasionTask>();
					return true;
				}
				return false;
			}

			// Token: 0x06005D36 RID: 23862 RVA: 0x001B3E98 File Offset: 0x001B2098
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				dialog.AddDialogLine("company_of_trouble_persuasion_check_accepted", "company_of_trouble_persuasion", "company_of_trouble_persuasion_start_reservation", "{=GCH6RgIQ}How tough are they?", new ConversationSentence.OnConditionDelegate(this.persuasion_start_with_company_of_trouble_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_company_of_trouble_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_rejected", "company_of_trouble_persuasion_start_reservation", "hero_main_options", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.persuasion_failed_with_company_of_trouble_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_rejected_with_company_of_trouble_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_attempt", "company_of_trouble_persuasion_start_reservation", "company_of_trouble_persuasion_select_option", "{=K0Qtl5RZ}Tell me about the details...", () => !this.persuasion_failed_with_company_of_trouble_on_condition(), null, this, 100, null, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_success", "company_of_trouble_persuasion_start_reservation", "close_window", "{=QlECaaHt}Hmm...They can be useful.", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.persuasion_complete_with_company_of_trouble_on_consequence), this, 200, null, null, null);
				string text = "company_of_trouble_persuasion_select_option_1";
				string text2 = "company_of_trouble_persuasion_select_option";
				string text3 = "company_of_trouble_persuasion_selected_option_response";
				string text4 = "{=0AUZvSAq}{COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate onConditionDelegate = new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.company_of_trouble_persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.company_of_trouble_persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(text, text2, text3, text4, onConditionDelegate, onConsequenceDelegate, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text5 = "company_of_trouble_persuasion_select_option_2";
				string text6 = "company_of_trouble_persuasion_select_option";
				string text7 = "company_of_trouble_persuasion_selected_option_response";
				string text8 = "{=GG1W8qGd}{COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate onConditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_select_option_2_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.company_of_trouble_persuasion_setup_option_2);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.company_of_trouble_persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(text5, text6, text7, text8, onConditionDelegate2, onConsequenceDelegate2, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text9 = "company_of_trouble_persuasion_select_option_3";
				string text10 = "company_of_trouble_persuasion_select_option";
				string text11 = "company_of_trouble_persuasion_selected_option_response";
				string text12 = "{=kFs940kp}{COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate onConditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_select_option_3_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.company_of_trouble_persuasion_setup_option_3);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.company_of_trouble_persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(text9, text10, text11, text12, onConditionDelegate3, onConsequenceDelegate3, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_select_option_reaction", "company_of_trouble_persuasion_selected_option_response", "company_of_trouble_persuasion_start_reservation", "{=D0xDRqvm}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x06005D37 RID: 23863 RVA: 0x001B40B6 File Offset: 0x001B22B6
			private void persuasion_start_with_company_of_trouble_on_consequence()
			{
				this._persuationTriedHeroesList.Add(Hero.OneToOneConversationHero);
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.Hard);
			}

			// Token: 0x06005D38 RID: 23864 RVA: 0x001B40EC File Offset: 0x001B22EC
			private bool persuasion_start_with_company_of_trouble_on_condition()
			{
				return !this._persuationTriedHeroesList.Contains(Hero.OneToOneConversationHero);
			}

			// Token: 0x06005D39 RID: 23865 RVA: 0x001B4104 File Offset: 0x001B2304
			private PersuasionTask GetPersuasionTask1()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=1V9GeKr8}Fah...I don't need more men. Thank you.", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=EvAubSxs}What kind of troops do they make?", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Trade, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=sqMUtasn}Cheap, disposable and effective. What you say?", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Tactics, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=Pcgqs9aX}Here's a quick run down of their training...", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=WvQDatMJ}I won't kid you, they're mean bastards, but that's good if you can manage them.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				return persuasionTask;
			}

			// Token: 0x06005D3A RID: 23866 RVA: 0x001B41C8 File Offset: 0x001B23C8
			private PersuasionTask GetPersuasionTask2()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=UP0pMGDR}There are enough bandits around here already. I don't need more on retainer.", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=zR356YDY}I have to say, they seem more like bandits than soldiers.", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=JI6Q9pQ7}Bandits can kill as well as any other kind of troops, if used correctly.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Trade, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=SqceZdzH}Of course. That's why they're cheap. You get what you pay for. ", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Scouting, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=NWLH02KL}Bandits are good in the wilderness, having been both predator and prey.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				return persuasionTask;
			}

			// Token: 0x06005D3B RID: 23867 RVA: 0x001B428C File Offset: 0x001B248C
			private PersuasionTask GetPersuasionTask3()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=97pacK2l}Fah... I don't need more men. Thank you.", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=A2ju7YTZ}I don't know... They look treacherous.", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Tactics, DefaultTraits.Mercy, TraitEffect.Negative, PersuasionArgumentStrength.Easy, false, new TextObject("{=z1mdQhDB}Of course. Send them in ahead of your other troops. If they die, you don't need to pay them.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=jWavM9AD}You've been around in the world. You know that mercenaries aren't saints.", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Generosity, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=sLjGguGy}Sure, they're bastards. But they'll be loyal bastards if you treat them well.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				return persuasionTask;
			}

			// Token: 0x06005D3C RID: 23868 RVA: 0x001B4350 File Offset: 0x001B2550
			private bool company_of_trouble_persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				if (item == PersuasionOptionResult.CriticalFailure)
				{
					this._selectedTask.BlockAllOptions();
				}
				return true;
			}

			// Token: 0x06005D3D RID: 23869 RVA: 0x001B4390 File Offset: 0x001B2590
			private void company_of_trouble_persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.Hard);
				float num;
				float num2;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out num, out num2, difficulty);
				this._selectedTask.ApplyEffects(num, num2);
			}

			// Token: 0x06005D3E RID: 23870 RVA: 0x001B43EC File Offset: 0x001B25EC
			private bool company_of_trouble_persuasion_select_option_1_on_condition()
			{
				if (this._selectedTask.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(0).Line);
					MBTextManager.SetTextVariable("COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005D3F RID: 23871 RVA: 0x001B446C File Offset: 0x001B266C
			private bool company_of_trouble_persuasion_select_option_2_on_condition()
			{
				if (this._selectedTask.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(1).Line);
					MBTextManager.SetTextVariable("COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005D40 RID: 23872 RVA: 0x001B44EC File Offset: 0x001B26EC
			private bool company_of_trouble_persuasion_select_option_3_on_condition()
			{
				if (this._selectedTask.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(2).Line);
					MBTextManager.SetTextVariable("COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005D41 RID: 23873 RVA: 0x001B456C File Offset: 0x001B276C
			private void company_of_trouble_persuasion_select_option_1_on_consequence()
			{
				if (this._selectedTask.Options.Count > 0)
				{
					this._selectedTask.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x06005D42 RID: 23874 RVA: 0x001B4598 File Offset: 0x001B2798
			private void company_of_trouble_persuasion_select_option_2_on_consequence()
			{
				if (this._selectedTask.Options.Count > 1)
				{
					this._selectedTask.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x06005D43 RID: 23875 RVA: 0x001B45C4 File Offset: 0x001B27C4
			private void company_of_trouble_persuasion_select_option_3_on_consequence()
			{
				if (this._selectedTask.Options.Count > 2)
				{
					this._selectedTask.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x06005D44 RID: 23876 RVA: 0x001B45F0 File Offset: 0x001B27F0
			private bool persuasion_failed_with_company_of_trouble_on_condition()
			{
				if (this._selectedTask.Options.All<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._selectedTask.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005D45 RID: 23877 RVA: 0x001B464E File Offset: 0x001B284E
			private PersuasionOptionArgs company_of_trouble_persuasion_setup_option_1()
			{
				return this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(0);
			}

			// Token: 0x06005D46 RID: 23878 RVA: 0x001B4661 File Offset: 0x001B2861
			private PersuasionOptionArgs company_of_trouble_persuasion_setup_option_2()
			{
				return this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(1);
			}

			// Token: 0x06005D47 RID: 23879 RVA: 0x001B4674 File Offset: 0x001B2874
			private PersuasionOptionArgs company_of_trouble_persuasion_setup_option_3()
			{
				return this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(2);
			}

			// Token: 0x06005D48 RID: 23880 RVA: 0x001B4688 File Offset: 0x001B2888
			private bool company_of_trouble_persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._selectedTask.Options.Count > 0)
				{
					hintText = (this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked ? hintText : null);
					return !this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005D49 RID: 23881 RVA: 0x001B46F0 File Offset: 0x001B28F0
			private bool company_of_trouble_persuasion_clickable_option_2_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._selectedTask.Options.Count > 1)
				{
					hintText = (this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked ? hintText : null);
					return !this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005D4A RID: 23882 RVA: 0x001B4758 File Offset: 0x001B2958
			private bool company_of_trouble_persuasion_clickable_option_3_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._selectedTask.Options.Count > 2)
				{
					hintText = (this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked ? hintText : null);
					return !this._selectedTask.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005D4B RID: 23883 RVA: 0x001B47BF File Offset: 0x001B29BF
			private void persuasion_rejected_with_company_of_trouble_on_consequence()
			{
				if (PlayerEncounter.Current != null)
				{
					PlayerEncounter.LeaveEncounter = true;
				}
				ConversationManager.EndPersuasion();
			}

			// Token: 0x06005D4C RID: 23884 RVA: 0x001B47D4 File Offset: 0x001B29D4
			private void persuasion_complete_with_company_of_trouble_on_consequence()
			{
				if (PlayerEncounter.Current != null)
				{
					PlayerEncounter.LeaveEncounter = true;
				}
				ConversationManager.EndPersuasion();
				this.UpdateCompanyTroopCount();
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._demandGold, false);
				this.RelationshipChangeWithQuestGiver = 5;
				base.AddLog(this.QuestSuccessPlayerSoldCompany, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06005D4D RID: 23885 RVA: 0x001B4848 File Offset: 0x001B2A48
			private DialogFlow GetCompanyDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=8TCev3Qs}So, captain. We expect a bit of looting and plundering as compensation, in addition to the wages. You don't seem like you're going to provide it to us. So, farewell.[if:innocent_smile][ib:hip]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.CompanyDialogFromCondition))
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=1aaoSpNf}Your contract with the {QUEST_GIVER.NAME} is still in force. I can't let you go without {?QUEST_GIVER.GENDER}her{?}his{\\?} permission.", null), null, null, null)
					.NpcLine(new TextObject("{=oI5H6Xo8}Don't think we won't fight you if you try and stop us.[if:convo_mocking_aristocratic]", null), null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=hIFazIcK}So be it!", null), null, null, null)
					.NpcLine(new TextObject("{=KKeRi477}All right, lads. Let's kill the boss.[if:convo_predatory][ib:aggressive]", null), null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.CreateCompanyEnemyParty;
					})
					.CloseDialog()
					.PlayerOption(new TextObject("{=bm7UcuQj}No! There is no need to fight. I don't want any bloodshed... Just leave.", null), null, null, null)
					.NpcLine(new TextObject("{=1vnaskLR}It was a pleasure to work with you, chief. Farewell...[if:convo_nonchalant][ib:normal2]", null), null, null, null, null)
					.Consequence(delegate
					{
						this._companyLeftQuestWillFail = true;
					})
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog()
					.PlayerOption(new TextObject("{=hj4vfgxk}As you wish! Good luck. ", null), null, null, null)
					.NpcLine(new TextObject("{=1vnaskLR}It was a pleasure to work with you, chief. Farewell...[if:convo_nonchalant][ib:normal2]", null), null, null, null, null)
					.Consequence(delegate
					{
						this._companyLeftQuestWillFail = true;
					})
					.CloseDialog()
					.EndPlayerOptions();
			}

			// Token: 0x06005D4E RID: 23886 RVA: 0x001B4981 File Offset: 0x001B2B81
			private bool CompanyDialogFromCondition()
			{
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
				return this._troubleCharacterObject == CharacterObject.OneToOneConversationCharacter;
			}

			// Token: 0x06005D4F RID: 23887 RVA: 0x001B49A8 File Offset: 0x001B2BA8
			private void CreateCompanyEnemyParty()
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				Settlement settlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsHideout);
				this._companyOfTroubleParty = BanditPartyComponent.CreateBanditParty("company_of_trouble_" + base.StringId, settlement.OwnerClan, settlement.Hideout, false, null, MobileParty.MainParty.Position);
				this._companyOfTroubleParty.MemberRoster.AddToCounts(this._troubleCharacterObject, this._companyTroopCount, false, 0, 0, true, -1);
				TextObject textObject = new TextObject("{=PV7RHgUl}Company of Trouble", null);
				this._companyOfTroubleParty.Party.SetCustomName(textObject);
				this._companyOfTroubleParty.InitializePartyTrade(QuestHelper.CalculateInitialGoldForBanditQuestParty(this._companyOfTroubleParty));
				this._companyOfTroubleParty.SetPartyUsedByQuest(true);
				this._battleWillStart = true;
			}

			// Token: 0x06005D50 RID: 23888 RVA: 0x001B4A98 File Offset: 0x001B2C98
			internal void CompanyLeftQuestFail()
			{
				this.RelationshipChangeWithQuestGiver = -2;
				this.UpdateCompanyTroopCount();
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				base.AddLog(this.QuestFailCompanyLeft, false);
				base.CompleteQuestWithFail(null);
				this._companyLeftQuestWillFail = false;
				GameMenu.ExitToLast();
			}

			// Token: 0x06005D51 RID: 23889 RVA: 0x001B4AF8 File Offset: 0x001B2CF8
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this.PlayerStartsQuestLogText, false);
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, this._companyTroopCount, false, 0, 0, true, -1);
				MBInformationManager.AddQuickInformation(new TextObject("{=jGIxKb99}Mercenaries have joined your party.", null), 0, null, null, "");
			}

			// Token: 0x06005D52 RID: 23890 RVA: 0x001B4B54 File Offset: 0x001B2D54
			protected override void RegisterEvents()
			{
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			}

			// Token: 0x06005D53 RID: 23891 RVA: 0x001B4BBD File Offset: 0x001B2DBD
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06005D54 RID: 23892 RVA: 0x001B4BD0 File Offset: 0x001B2DD0
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclared);
				}
			}

			// Token: 0x06005D55 RID: 23893 RVA: 0x001B4BFA File Offset: 0x001B2DFA
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclared, false);
			}

			// Token: 0x06005D56 RID: 23894 RVA: 0x001B4C14 File Offset: 0x001B2E14
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if ((mapEvent.IsPlayerMapEvent || mapEvent.IsPlayerSimulation) && !this._checkForBattleResults)
				{
					this.UpdateCompanyTroopCount();
					if (this._companyTroopCount == 0)
					{
						base.AddLog(this.AllCompanyDiedLogText, false);
						this.RelationshipChangeWithQuestGiver = 5;
						GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
						base.CompleteQuestWithSuccess();
					}
				}
			}

			// Token: 0x06005D57 RID: 23895 RVA: 0x001B4C74 File Offset: 0x001B2E74
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					this.UpdateCompanyTroopCount();
					if (MobileParty.MainParty.MemberRoster.TotalManCount - this._companyTroopCount <= this._companyTroopCount && MapEvent.PlayerMapEvent == null && Settlement.CurrentSettlement == null && PlayerEncounter.Current == null && !Hero.MainHero.IsWounded && !MobileParty.MainParty.IsCurrentlyAtSea)
					{
						this._triggerCompanyOfTroubleConversation = true;
						GameMenu.ActivateGameMenu("company_of_trouble_menu");
					}
				}
			}

			// Token: 0x06005D58 RID: 23896 RVA: 0x001B4CEC File Offset: 0x001B2EEC
			private void TryToStealItemFromPlayer()
			{
				bool flag = false;
				for (int i = 0; i < MobileParty.MainParty.ItemRoster.Count; i++)
				{
					ItemRosterElement itemRosterElement = MobileParty.MainParty.ItemRoster[i];
					ItemObject item = itemRosterElement.EquipmentElement.Item;
					if (!itemRosterElement.IsEmpty && item.IsFood)
					{
						MobileParty.MainParty.ItemRoster.AddToCounts(item, -1);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (this._thieveryCount == 0 || this._thieveryCount == 1)
					{
						InformationManager.ShowInquiry(new InquiryData(this.Title.ToString(), (this._thieveryCount == 0) ? new TextObject("{=OKpwA8Az}Your men have noticed some of the goods in the baggage train are missing.", null).ToString() : new TextObject("{=acu1wTeq}Your men are sure of that some of the goods were stolen from the baggage train.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), true, false);
					}
					else
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=xlm8oYhM}Your men reported that some of the goods were stolen from the baggage train.", null), 0, null, null, "");
					}
					this._thieveryCount++;
				}
			}

			// Token: 0x06005D59 RID: 23897 RVA: 0x001B4E04 File Offset: 0x001B3004
			protected override void DailyTick()
			{
				if (MBRandom.RandomFloat > 0.5f)
				{
					this.TryToStealItemFromPlayer();
				}
			}

			// Token: 0x06005D5A RID: 23898 RVA: 0x001B4E18 File Offset: 0x001B3018
			private void UpdateCompanyTroopCount()
			{
				bool flag = false;
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == this._troubleCharacterObject)
					{
						flag = true;
						this._companyTroopCount = troopRosterElement.Number;
						break;
					}
				}
				if (!flag)
				{
					this._companyTroopCount = 0;
				}
			}

			// Token: 0x06005D5B RID: 23899 RVA: 0x001B4E98 File Offset: 0x001B3098
			internal void QuestSuccessWithPlayerDefeatedCompany()
			{
				base.AddLog(this.AllCompanyDiedLogText, false);
				this.RelationshipChangeWithQuestGiver = 5;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06005D5C RID: 23900 RVA: 0x001B4EC7 File Offset: 0x001B30C7
			internal void QuestFailWithPlayerDefeatedAgainstCompany()
			{
				this.RelationshipChangeWithQuestGiver = -2;
				base.AddLog(this.PlayerDefeatedAgainstCompany, false);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06005D5D RID: 23901 RVA: 0x001B4EE6 File Offset: 0x001B30E6
			protected override void OnFinalize()
			{
				this.UpdateCompanyTroopCount();
				if (this._companyTroopCount > 0)
				{
					MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				}
			}

			// Token: 0x04001E56 RID: 7766
			private const string TroubleCharacterObjectStringId = "company_of_trouble_character";

			// Token: 0x04001E57 RID: 7767
			private int _companyTroopCount;

			// Token: 0x04001E58 RID: 7768
			[SaveableField(20)]
			internal MobileParty _companyOfTroubleParty;

			// Token: 0x04001E59 RID: 7769
			[SaveableField(30)]
			internal bool _battleWillStart;

			// Token: 0x04001E5A RID: 7770
			internal bool _checkForBattleResults;

			// Token: 0x04001E5B RID: 7771
			[SaveableField(40)]
			private int _thieveryCount;

			// Token: 0x04001E5C RID: 7772
			[SaveableField(80)]
			internal bool _triggerCompanyOfTroubleConversation;

			// Token: 0x04001E5D RID: 7773
			[SaveableField(50)]
			private int _demandGold;

			// Token: 0x04001E5E RID: 7774
			internal CharacterObject _troubleCharacterObject;

			// Token: 0x04001E5F RID: 7775
			private PersuasionTask[] _tasks;

			// Token: 0x04001E60 RID: 7776
			private PersuasionTask _selectedTask;

			// Token: 0x04001E61 RID: 7777
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.Hard;

			// Token: 0x04001E62 RID: 7778
			[SaveableField(70)]
			private List<Hero> _persuationTriedHeroesList;

			// Token: 0x04001E63 RID: 7779
			internal bool _companyLeftQuestWillFail;
		}

		// Token: 0x02000753 RID: 1875
		public class LandLordCompanyOfTroubleIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005D65 RID: 23909 RVA: 0x001B4F7C File Offset: 0x001B317C
			public LandLordCompanyOfTroubleIssueTypeDefiner()
				: base(4800000)
			{
			}

			// Token: 0x06005D66 RID: 23910 RVA: 0x001B4F89 File Offset: 0x001B3189
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue), 1, null);
				base.AddClassDefinition(typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest), 2, null);
			}
		}
	}
}
