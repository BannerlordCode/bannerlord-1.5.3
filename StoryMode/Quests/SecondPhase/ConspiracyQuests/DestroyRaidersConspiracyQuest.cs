using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.SecondPhase.ConspiracyQuests
{
	// Token: 0x0200002B RID: 43
	public class DestroyRaidersConspiracyQuest : ConspiracyQuestBase
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000CB90 File Offset: 0x0000AD90
		private float RaiderPartyPlayerEncounterRadius
		{
			get
			{
				return Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius * 3f;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=DfiACGay}Destroy Raiders", null);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000CBB9 File Offset: 0x0000ADB9
		public override float ConspiracyStrengthDecreaseAmount
		{
			get
			{
				return 50f;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
		private int RegularRaiderPartyTroopCount
		{
			get
			{
				return 17 + MathF.Ceiling(23f * Campaign.Current.Models.IssueModel.GetIssueDifficultyMultiplier());
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
		private int SpecialRaiderPartyTroopCount
		{
			get
			{
				return 33 + MathF.Ceiling(37f * Campaign.Current.Models.IssueModel.GetIssueDifficultyMultiplier());
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000CC08 File Offset: 0x0000AE08
		public override TextObject StartLog
		{
			get
			{
				TextObject textObject = new TextObject("{=Dr63pCHt}{MENTOR.LINK} has sent you a message about bandit attacks near {TARGET_SETTLEMENT}, and advises you to go there and eliminate them all before their actions turn the locals against your movement. ", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.QuestGiver.CharacterObject, textObject, false);
				textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000CC54 File Offset: 0x0000AE54
		public override TextObject StartMessageLogFromMentor
		{
			get
			{
				TextObject textObject = new TextObject("{=V5K8RpAa}{MENTOR.LINK}'s message: “Greetings, {PLAYER.NAME}. We have a new problem. I've had reports from my agents of unusual bandit activity near {TARGET_SETTLEMENT}. They appear to be raiding and killing travellers {?IS_EMPIRE}under the protection of the Empire{?}who aren't under the protection of the Empire{\\?}, and leaving the others alone. This seems very much like the work of {NEMESIS_MENTOR.LINK}, to terrorize local merchants so that no one will stand up for our cause. I advise you to wipe these bandits out as quickly as possible. That would send a good message, both to our allies and our enemies.”", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.QuestGiver.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, textObject, false);
				bool isOnImperialQuestLine = StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine;
				StringHelpers.SetCharacterProperties("NEMESIS_MENTOR", isOnImperialQuestLine ? StoryModeHeroes.AntiImperialMentor.CharacterObject : StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("IS_IMPERIAL", isOnImperialQuestLine ? 1 : 0);
				textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000CD00 File Offset: 0x0000AF00
		public override TextObject SideNotificationText
		{
			get
			{
				TextObject textObject = new TextObject("{=T7OTmJUp}{MENTOR.LINK} has a message for you", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.QuestGiver.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000CD32 File Offset: 0x0000AF32
		private TextObject _destroyRaidersQuestSucceededLogText
		{
			get
			{
				TextObject textObject = new TextObject("{=qg05CSZb}You have defeated all the raiders near {TARGET_SETTLEMENT}. Many people now hope you can bring peace and prosperity back to the region.", null);
				textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000CD56 File Offset: 0x0000AF56
		private TextObject _destroyRaidersQuestFailedOnTimedOutLogText
		{
			get
			{
				return new TextObject("{=DaBN0O7N}You have failed to defeat all raider parties in time. Many of the locals feel that you've brought misfortune upon them, and want nothing to do with you.", null);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000CD63 File Offset: 0x0000AF63
		private TextObject _destroyRaidersQuestFailedOnPlayerDefeatedByRaidersLogText
		{
			get
			{
				return new TextObject("{=mN60B07k}You have lost the battle against raiders and failed to defeat conspiracy forces. Many of the locals feel that you've brought misfortune upon them, and want nothing to do with you.", null);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000CD70 File Offset: 0x0000AF70
		private TextObject _destroyRaidersRegularPartiesProgress
		{
			get
			{
				TextObject textObject = new TextObject("{=dbLb3krw}Hunt the gangs of {RAIDER_NAME}", null);
				textObject.SetTextVariable("RAIDER_NAME", this._banditFaction.Name);
				return textObject;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000CD94 File Offset: 0x0000AF94
		private TextObject _destroyRaidersSpecialPartyProgress
		{
			get
			{
				return new TextObject("{=QVkuaezc}Hunt the conspiracy war party", null);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000CDA1 File Offset: 0x0000AFA1
		private TextObject _destroyRaidersRegularProgressNotification
		{
			get
			{
				TextObject textObject = new TextObject("{=US0VAHiE}You have eliminated a {RAIDER_NAME} party.", null);
				textObject.SetTextVariable("RAIDER_NAME", this._banditFaction.Name);
				return textObject;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000CDC5 File Offset: 0x0000AFC5
		private TextObject _destroyRaidersRegularProgressCompletedNotification
		{
			get
			{
				TextObject textObject = new TextObject("{=LfH7VXDH}You have eliminated all {RAIDER_NAME} gangs in the vicinity.", null);
				textObject.SetTextVariable("RAIDER_NAME", this._banditFaction.Name);
				return textObject;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000CDE9 File Offset: 0x0000AFE9
		private TextObject _destroyRaidersSpecialPartyInformationQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=agrsO3qQ}Due to your successful skirmishes against {RAIDER_NAME}, a conspiracy war party is now patrolling around {SETTLEMENT}.", null);
				textObject.SetTextVariable("RAIDER_NAME", this._banditFaction.Name);
				textObject.SetTextVariable("SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000CE24 File Offset: 0x0000B024
		private TextObject _destroyRaidersSpecialPartySpawnNotification
		{
			get
			{
				TextObject textObject = new TextObject("{=QOVLkdTp}A conspiracy war party is now patrolling around {SETTLEMENT}.", null);
				textObject.SetTextVariable("SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000CE48 File Offset: 0x0000B048
		public DestroyRaidersConspiracyQuest(string questId, Hero questGiver)
			: base(questId, questGiver)
		{
			this._regularRaiderParties = new List<MobileParty>(3);
			this._directedRaidersToEngagePlayer = new List<MobileParty>(3);
			this._targetSettlement = this.DetermineTargetSettlement();
			this._banditFaction = this.GetBanditTypeForSettlement(this._targetSettlement);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000CE88 File Offset: 0x0000B088
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(this.GetConspiracyCaptainDialogue(), this);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
		protected override void RegisterEvents()
		{
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroTakenPrisoner));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.MobilePartyDestroyed));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000CF0C File Offset: 0x0000B10C
		private void OnGameMenuOpened(MenuCallbackArgs menuCallbackArgs)
		{
			if (menuCallbackArgs.MenuContext.GameMenu.StringId == "prisoner_wait")
			{
				PartyBase captorParty = PlayerCaptivity.CaptorParty;
				if (captorParty != null && captorParty.IsMobile && (this._regularRaiderParties.Contains(PlayerCaptivity.CaptorParty.MobileParty) || this._specialRaiderParty == PlayerCaptivity.CaptorParty.MobileParty))
				{
					this.OnQuestFailedByDefeat();
				}
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000CF78 File Offset: 0x0000B178
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
			this.DetermineClosestHideouts();
			if (this._specialRaiderParty != null && this._specialRaiderParty.IsActive && this._specialRaiderParty.MapEventSide == null)
			{
				this._specialRaiderParty.ActualClan = StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan;
			}
			if (this._directedRaidersToEngagePlayer == null)
			{
				this._directedRaidersToEngagePlayer = new List<MobileParty>(3);
				return;
			}
			if (this._directedRaidersToEngagePlayer.Count > this._regularRaiderParties.Count)
			{
				this._directedRaidersToEngagePlayer = new List<MobileParty>(3);
				using (List<MobileParty>.Enumerator enumerator = this._regularRaiderParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MobileParty mobileParty = enumerator.Current;
						this.SetDefaultRaiderAi(mobileParty);
					}
					return;
				}
			}
			foreach (MobileParty mobileParty2 in this._regularRaiderParties)
			{
				this.CheckRaiderPartyPlayerEncounter(mobileParty2);
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000D094 File Offset: 0x0000B294
		protected override void OnStartQuest()
		{
			base.OnStartQuest();
			string text = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? "conspiracy_commander_antiempire" : "conspiracy_commander_empire");
			this._conspiracyCaptainCharacter = Game.Current.ObjectManager.GetObject<CharacterObject>(text);
			this.InitializeRaiders();
			this._regularPartiesProgressTracker = base.AddDiscreteLog(this._destroyRaidersRegularPartiesProgress, TextObject.GetEmpty(), 0, 3, null, false);
			this.SetDialogs();
			base.InitializeQuestOnCreation();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D108 File Offset: 0x0000B308
		private Settlement DetermineTargetSettlement()
		{
			Settlement settlement = null;
			Settlement centralSettlement = StoryModeHeroes.ImperialMentor.HomeSettlement;
			if (!Clan.PlayerClan.Settlements.IsEmpty<Settlement>())
			{
				settlement = Clan.PlayerClan.Settlements.GetRandomElementWithPredicate<Settlement>(delegate(Settlement t)
				{
					if (t.IsTown || t.IsCastle)
					{
						MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition = t.GatePosition;
						CampaignVec2 gatePosition2 = centralSettlement.GatePosition;
						return mapDistanceModel.PathExistBetweenPoints(in gatePosition, in gatePosition2, MobileParty.NavigationType.Default);
					}
					return false;
				});
			}
			else
			{
				MBList<Settlement> mblist = StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom.Settlements.Where<Settlement>((Settlement t) => t.IsTown || t.IsCastle).ToMBList<Settlement>();
				if (!mblist.IsEmpty<Settlement>())
				{
					settlement = mblist.GetRandomElementWithPredicate<Settlement>(delegate(Settlement t)
					{
						MapDistanceModel mapDistanceModel2 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition3 = t.GatePosition;
						CampaignVec2 gatePosition4 = centralSettlement.GatePosition;
						return mapDistanceModel2.PathExistBetweenPoints(in gatePosition3, in gatePosition4, MobileParty.NavigationType.Default);
					});
				}
			}
			if (settlement == null)
			{
				Debug.FailedAssert("Destroy raiders conspiracy quest settlement is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Quests\\SecondPhase\\ConspiracyQuests\\DestroyRaidersConspiracyQuest.cs", "DetermineTargetSettlement", 311);
				settlement = Settlement.All.GetRandomElementWithPredicate<Settlement>(delegate(Settlement t)
				{
					if (t.IsTown || t.IsCastle)
					{
						MapDistanceModel mapDistanceModel3 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition5 = t.GatePosition;
						CampaignVec2 gatePosition6 = centralSettlement.GatePosition;
						return mapDistanceModel3.PathExistBetweenPoints(in gatePosition5, in gatePosition6, MobileParty.NavigationType.Default);
					}
					return false;
				});
			}
			return settlement;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D1EC File Offset: 0x0000B3EC
		private void InitializeRaiders()
		{
			List<Settlement> list = this.DetermineClosestHideouts();
			for (int i = 0; i < 3; i++)
			{
				this.SpawnRaiderPartyAtHideout(list.ElementAt<Settlement>(i), false);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D21C File Offset: 0x0000B41C
		private List<Settlement> DetermineClosestHideouts()
		{
			MapDistanceModel model = Campaign.Current.Models.MapDistanceModel;
			Settlement centralSettlement = StoryModeHeroes.ImperialMentor.HomeSettlement;
			List<Settlement> list = (from x in Hideout.All.Where<Hideout>(delegate(Hideout t)
				{
					MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
					CampaignVec2 gatePosition = t.Settlement.GatePosition;
					CampaignVec2 gatePosition2 = centralSettlement.GatePosition;
					return mapDistanceModel.PathExistBetweenPoints(in gatePosition, in gatePosition2, MobileParty.NavigationType.Default);
				})
				select x.Settlement into t
				orderby model.GetDistance(this._targetSettlement, t, false, false, MobileParty.NavigationType.Default)
				select t).Take<Settlement>(3).ToList<Settlement>();
			this._closestHideout = list[0];
			return list;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D2C0 File Offset: 0x0000B4C0
		private void SpawnRaiderPartyAtHideout(Settlement hideout, bool isSpecialParty = false)
		{
			PartyTemplateObject partyTemplateObject;
			int num;
			TextObject textObject;
			if (isSpecialParty)
			{
				partyTemplateObject = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("conspiracy_anti_imperial_special_raider_party_template") : Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("conspiracy_imperial_special_raider_party_template"));
				num = this.SpecialRaiderPartyTroopCount;
				textObject = new TextObject("{=GW7Zg3IP}Conspiracy War Party", null);
			}
			else
			{
				partyTemplateObject = this._banditFaction.DefaultPartyTemplate;
				num = this.RegularRaiderPartyTroopCount;
				textObject = this._banditFaction.Name;
			}
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty(string.Concat(new object[]
			{
				"destroy_raiders_conspiracy_quest_",
				this._banditFaction.Name,
				"_",
				CampaignTime.Now.ElapsedSecondsUntilNow
			}), StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan, hideout.Hideout, false, partyTemplateObject, hideout.GatePosition);
			mobileParty.Party.SetCustomName(textObject);
			mobileParty.MemberRoster.Clear();
			mobileParty.SetPartyUsedByQuest(true);
			this.SetDefaultRaiderAi(mobileParty);
			if (isSpecialParty)
			{
				this._specialRaiderParty = mobileParty;
				mobileParty.MemberRoster.AddToCounts(this._conspiracyCaptainCharacter, 1, true, 0, 0, true, -1);
				mobileParty.ItemRoster.Clear();
				mobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("vlandia_horse"), num / 2);
				MBInformationManager.AddQuickInformation(this._destroyRaidersSpecialPartySpawnNotification, 0, null, null, "");
			}
			else
			{
				this._regularRaiderParties.Add(mobileParty);
			}
			base.DistributeConspiracyRaiderTroopsByLevel(partyTemplateObject, mobileParty.Party, num);
			base.AddTrackedObject(mobileParty);
			mobileParty.InitializePartyTrade(QuestHelper.CalculateInitialGoldForBanditQuestParty(mobileParty));
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000D459 File Offset: 0x0000B659
		private void SetDefaultRaiderAi(MobileParty raiderParty)
		{
			SetPartyAiAction.GetActionForPatrollingAroundSettlement(raiderParty, this._targetSettlement, MobileParty.NavigationType.Default, false, false);
			raiderParty.Ai.CheckPartyNeedsUpdate();
			raiderParty.Ai.SetDoNotMakeNewDecisions(true);
			raiderParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000D48C File Offset: 0x0000B68C
		private Clan GetBanditTypeForSettlement(Settlement settlement)
		{
			Hideout closestHideout = SettlementHelper.FindNearestHideoutToSettlement(settlement, MobileParty.NavigationType.Default, (Settlement x) => x.IsActive);
			if (closestHideout == null)
			{
				return Clan.BanditFactions.GetRandomElementInefficiently<Clan>();
			}
			return Clan.BanditFactions.FirstOrDefault<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		private void MobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (destroyerParty != null && destroyerParty.MobileParty == MobileParty.MainParty)
			{
				if (this._regularRaiderParties.Contains(mobileParty))
				{
					this.OnBanditPartyClearedByPlayer(mobileParty);
					return;
				}
				if (this._specialRaiderParty == mobileParty)
				{
					this.OnSpecialBanditPartyClearedByPlayer();
				}
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D52C File Offset: 0x0000B72C
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.WinningSide != BattleSideEnum.None && mapEvent.DefeatedSide != BattleSideEnum.None && mapEvent.IsPlayerMapEvent && mapEvent.InvolvedParties.Any<PartyBase>((PartyBase t) => t.IsMobile && (this._regularRaiderParties.Contains(t.MobileParty) || t.MobileParty == this._specialRaiderParty)))
			{
				if (PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide)
				{
					using (List<MapEventParty>.Enumerator enumerator = mapEvent.GetMapEventSide(mapEvent.DefeatedSide).Parties.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MapEventParty mapEventParty = enumerator.Current;
							MobileParty mobileParty = mapEventParty.Party.MobileParty;
							if (mobileParty != null && mobileParty.IsActive && mobileParty.Party.NumberOfHealthyMembers > 0 && (this._regularRaiderParties.Contains(mobileParty) || this._specialRaiderParty == mobileParty))
							{
								DestroyPartyAction.Apply(PartyBase.MainParty, mobileParty);
							}
						}
						return;
					}
				}
				PartyBase captorParty = PlayerCaptivity.CaptorParty;
				if (captorParty == null || !captorParty.IsMobile || (!this._regularRaiderParties.Contains(PlayerCaptivity.CaptorParty.MobileParty) && this._specialRaiderParty != PlayerCaptivity.CaptorParty.MobileParty))
				{
					this.OnQuestFailedByDefeat();
				}
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000D66C File Offset: 0x0000B86C
		private void OnSpecialBanditPartyClearedByPlayer()
		{
			if (base.IsTracked(this._specialRaiderParty))
			{
				base.RemoveTrackedObject(this._specialRaiderParty);
			}
			this._specialPartyProgressTracker.UpdateCurrentProgress(1);
			this._specialRaiderParty = null;
			this.OnQuestSucceeded();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
		private void OnBanditPartyClearedByPlayer(MobileParty defeatedParty)
		{
			this._regularRaiderParties.Remove(defeatedParty);
			this._regularPartiesProgressTracker.UpdateCurrentProgress(3 - this._regularRaiderParties.Count);
			if (this._regularPartiesProgressTracker.HasBeenCompleted())
			{
				MBInformationManager.AddQuickInformation(this._destroyRaidersRegularProgressCompletedNotification, 0, null, null, "");
				base.AddLog(this._destroyRaidersSpecialPartyInformationQuestLog, false);
				this._specialPartyProgressTracker = base.AddDiscreteLog(this._destroyRaidersSpecialPartyProgress, TextObject.GetEmpty(), 0, 1, null, false);
				this.SpawnRaiderPartyAtHideout(this._closestHideout, true);
				return;
			}
			if (base.IsTracked(defeatedParty))
			{
				base.RemoveTrackedObject(defeatedParty);
			}
			MBInformationManager.AddQuickInformation(this._destroyRaidersRegularProgressNotification, 0, null, null, "");
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000D750 File Offset: 0x0000B950
		private void OnHeroTakenPrisoner(PartyBase capturer, Hero prisoner)
		{
			if (prisoner.Clan != Clan.PlayerClan && capturer.IsMobile && (this._regularRaiderParties.Contains(capturer.MobileParty) || this._specialRaiderParty == capturer.MobileParty))
			{
				Debug.FailedAssert("Hero has been taken prisoner by conspiracy raider party", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Quests\\SecondPhase\\ConspiracyQuests\\DestroyRaidersConspiracyQuest.cs", "OnHeroTakenPrisoner", 540);
				EndCaptivityAction.ApplyByEscape(prisoner, null, true);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		protected override void HourlyTick()
		{
			foreach (MobileParty mobileParty in this._regularRaiderParties)
			{
				this.CheckRaiderPartyPlayerEncounter(mobileParty);
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D808 File Offset: 0x0000BA08
		private void CheckRaiderPartyPlayerEncounter(MobileParty raiderParty)
		{
			if (raiderParty.Position.DistanceSquared(MobileParty.MainParty.Position) <= this.RaiderPartyPlayerEncounterRadius && raiderParty.Ai.DoNotAttackMainPartyUntil.IsPast && raiderParty.Party.CalculateCurrentStrength() > PartyBase.MainParty.CalculateCurrentStrength() * 1.2f && MobileParty.MainParty.CurrentSettlement == null)
			{
				if (!this._directedRaidersToEngagePlayer.Contains(raiderParty))
				{
					SetPartyAiAction.GetActionForEngagingParty(raiderParty, MobileParty.MainParty, MobileParty.NavigationType.Default, false);
					raiderParty.Ai.CheckPartyNeedsUpdate();
					this._directedRaidersToEngagePlayer.Add(raiderParty);
					return;
				}
			}
			else if (this._directedRaidersToEngagePlayer.Contains(raiderParty))
			{
				this._directedRaidersToEngagePlayer.Remove(raiderParty);
				this.SetDefaultRaiderAi(raiderParty);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		private DialogFlow GetConspiracyCaptainDialogue()
		{
			return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=bzmcPtZ6}We know you. We were told to look out for you. We know what you're planning with {MENTOR.NAME}. You will fail, and you will die.[ib:closed][if:convo_predatory]", null, null, null, null).Condition(delegate
			{
				StringHelpers.SetCharacterProperties("MENTOR", base.QuestGiver.CharacterObject, null, false);
				return CharacterObject.OneToOneConversationCharacter == this._conspiracyCaptainCharacter && this._specialRaiderParty != null && !this._specialPartyProgressTracker.HasBeenCompleted();
			})
				.BeginPlayerOptions(null, false)
				.PlayerOption("{=BrHU0NuE}Maybe. But if we do, you won't live to see it.", null, null, null)
				.Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += this.OnConspiracyCaptainDialogueEnd;
				})
				.NpcLine("{=EoLcoaHM}We'll see...", null, null, null, null)
				.CloseDialog()
				.PlayerOption("{=TLaxmQDF}You'll without a doubt perish by my sword, but today is not the day.", null, null, null)
				.Consequence(delegate
				{
					PlayerEncounter.LeaveEncounter = true;
				})
				.NpcLine("{=9aY0ifwi}We shall meet again...[if:convo_insulted]", null, null, null, null)
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000D986 File Offset: 0x0000BB86
		private void OnConspiracyCaptainDialogueEnd()
		{
			PlayerEncounter.RestartPlayerEncounter(this._specialRaiderParty.Party, PartyBase.MainParty, true, false);
			PlayerEncounter.StartBattle();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		private void OnQuestSucceeded()
		{
			if (this._targetSettlement.OwnerClan != Clan.PlayerClan && !this._targetSettlement.OwnerClan.MapFaction.IsAtWarWith(Clan.PlayerClan.MapFaction))
			{
				ChangeRelationAction.ApplyPlayerRelation(this._targetSettlement.OwnerClan.Leader, 5, true, true);
			}
			Clan.PlayerClan.AddRenown(5f, true);
			this._targetSettlement.Town.Security += 5f;
			this._targetSettlement.Town.Prosperity += 5f;
			base.AddLog(this._destroyRaidersQuestSucceededLogText, false);
			base.CompleteQuestWithSuccess();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		private void OnQuestFailedByDefeat()
		{
			this.OnQuestFailed();
			base.AddLog(this._destroyRaidersQuestFailedOnPlayerDefeatedByRaidersLogText, false);
			base.CompleteQuestWithFail(null);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		private void OnQuestFailed()
		{
			foreach (MobileParty mobileParty in this._regularRaiderParties)
			{
				if (mobileParty.IsActive)
				{
					DestroyPartyAction.Apply(null, mobileParty);
				}
			}
			if (this._specialRaiderParty != null && this._specialRaiderParty.IsActive)
			{
				DestroyPartyAction.Apply(null, this._specialRaiderParty);
			}
			if (this._targetSettlement.OwnerClan != Clan.PlayerClan)
			{
				ChangeRelationAction.ApplyPlayerRelation(this._targetSettlement.OwnerClan.Leader, -5, true, true);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000DB24 File Offset: 0x0000BD24
		protected override void OnTimedOut()
		{
			this.OnQuestFailed();
			base.AddLog(this._destroyRaidersQuestFailedOnTimedOutLogText, false);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000DB3A File Offset: 0x0000BD3A
		internal static void AutoGeneratedStaticCollectObjectsDestroyRaidersConspiracyQuest(object o, List<object> collectedObjects)
		{
			((DestroyRaidersConspiracyQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000DB48 File Offset: 0x0000BD48
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._targetSettlement);
			collectedObjects.Add(this._regularRaiderParties);
			collectedObjects.Add(this._specialRaiderParty);
			collectedObjects.Add(this._regularPartiesProgressTracker);
			collectedObjects.Add(this._specialPartyProgressTracker);
			collectedObjects.Add(this._banditFaction);
			collectedObjects.Add(this._conspiracyCaptainCharacter);
			collectedObjects.Add(this._closestHideout);
			collectedObjects.Add(this._directedRaidersToEngagePlayer);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000DBC8 File Offset: 0x0000BDC8
		internal static object AutoGeneratedGetMemberValue_targetSettlement(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._targetSettlement;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000DBD5 File Offset: 0x0000BDD5
		internal static object AutoGeneratedGetMemberValue_regularRaiderParties(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._regularRaiderParties;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000DBE2 File Offset: 0x0000BDE2
		internal static object AutoGeneratedGetMemberValue_specialRaiderParty(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._specialRaiderParty;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000DBEF File Offset: 0x0000BDEF
		internal static object AutoGeneratedGetMemberValue_regularPartiesProgressTracker(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._regularPartiesProgressTracker;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000DBFC File Offset: 0x0000BDFC
		internal static object AutoGeneratedGetMemberValue_specialPartyProgressTracker(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._specialPartyProgressTracker;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000DC09 File Offset: 0x0000BE09
		internal static object AutoGeneratedGetMemberValue_banditFaction(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._banditFaction;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000DC16 File Offset: 0x0000BE16
		internal static object AutoGeneratedGetMemberValue_conspiracyCaptainCharacter(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._conspiracyCaptainCharacter;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000DC23 File Offset: 0x0000BE23
		internal static object AutoGeneratedGetMemberValue_closestHideout(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._closestHideout;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000DC30 File Offset: 0x0000BE30
		internal static object AutoGeneratedGetMemberValue_directedRaidersToEngagePlayer(object o)
		{
			return ((DestroyRaidersConspiracyQuest)o)._directedRaidersToEngagePlayer;
		}

		// Token: 0x040000BD RID: 189
		private const int QuestSuccededRelationBonus = 5;

		// Token: 0x040000BE RID: 190
		private const int QuestSucceededSecurityBonus = 5;

		// Token: 0x040000BF RID: 191
		private const int QuestSuceededProsperityBonus = 5;

		// Token: 0x040000C0 RID: 192
		private const int QuestSuceededRenownBonus = 5;

		// Token: 0x040000C1 RID: 193
		private const int QuestFailedRelationPenalty = -5;

		// Token: 0x040000C2 RID: 194
		private const int NumberOfRegularRaidersToSpawn = 3;

		// Token: 0x040000C3 RID: 195
		[SaveableField(1)]
		private readonly Settlement _targetSettlement;

		// Token: 0x040000C4 RID: 196
		[SaveableField(2)]
		private readonly List<MobileParty> _regularRaiderParties;

		// Token: 0x040000C5 RID: 197
		[SaveableField(3)]
		private MobileParty _specialRaiderParty;

		// Token: 0x040000C6 RID: 198
		[SaveableField(4)]
		private JournalLog _regularPartiesProgressTracker;

		// Token: 0x040000C7 RID: 199
		[SaveableField(5)]
		private JournalLog _specialPartyProgressTracker;

		// Token: 0x040000C8 RID: 200
		[SaveableField(6)]
		private Clan _banditFaction;

		// Token: 0x040000C9 RID: 201
		[SaveableField(7)]
		private CharacterObject _conspiracyCaptainCharacter;

		// Token: 0x040000CA RID: 202
		[SaveableField(8)]
		private Settlement _closestHideout;

		// Token: 0x040000CB RID: 203
		[SaveableField(9)]
		private List<MobileParty> _directedRaidersToEngagePlayer;
	}
}
