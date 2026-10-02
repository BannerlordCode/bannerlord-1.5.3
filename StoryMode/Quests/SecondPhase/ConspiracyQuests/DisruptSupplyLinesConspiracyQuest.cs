using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
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
	// Token: 0x0200002C RID: 44
	public class DisruptSupplyLinesConspiracyQuest : ConspiracyQuestBase
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=y150haHv}Disrupt Supply Lines", null);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		public override TextObject SideNotificationText
		{
			get
			{
				TextObject textObject = new TextObject("{=IPP6MKfy}{MENTOR.LINK} notified you about a weapons caravan that will supply conspirators with weapons and armour.", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		public override TextObject StartMessageLogFromMentor
		{
			get
			{
				TextObject textObject = new TextObject("{=01Y1DAqA}{MENTOR.LINK} has sent you a message: As you may know, I receive reports from my spies in marketplaces around here. There is a merchant who I have been following - I know he is connected with {OTHER_MENTOR.LINK}. Now, I hear he has bought up a large supply of weapons and armor in {QUEST_FROM_SETTLEMENT_NAME}, and plans to travel to {QUEST_TO_SETTLEMENT_NAME}. From there it will move onward. I expect that {OTHER_MENTOR.LINK} is arming {?OTHER_MENTOR.GENDER}her{?}his{\\?} allies in the gangs in that area. If the caravan delivers its load, then I expect we will soon find some of our friends stabbed to death in the streets by hired thugs, and the rest of our friends too frightened to acknowledge us. I need you to track it down and destroy it. Try to intercept it on the first leg of its journey, before it gets to {QUEST_TO_SETTLEMENT_NAME}. If you fail, find out the next town to which it is going. It may take some time to find it, and when you do, it will be well guarded. But I trust in your perseverance, your skill and your understanding of how important this is. Good hunting.", null);
				StringHelpers.SetCharacterProperties("OTHER_MENTOR", StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? StoryModeHeroes.AntiImperialMentor.CharacterObject : StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("QUEST_FROM_SETTLEMENT_NAME", this.QuestFromSettlement.EncyclopediaLinkWithName);
				textObject.SetTextVariable("QUEST_TO_SETTLEMENT_NAME", this.QuestToSettlement.EncyclopediaLinkWithName);
				return textObject;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		public override TextObject StartLog
		{
			get
			{
				TextObject textObject = new TextObject("{=ZKdBlAmp}An arms caravan to resupply the conspirators will be soon on its way.{newline}{MENTOR.LINK}'s message:{newline}\"Our spies have learned about an arms caravan that is attempting to bring the conspirators high quality weapons and armor. We know that it will set out on its route from {QUEST_FROM_SETTLEMENT_NAME} to {QUEST_TO_SETTLEMENT_NAME} after {SPAWN_DAYS} days. We will find out and notify you about the new routes that it takes as it progresses.\"", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				textObject.SetTextVariable("QUEST_FROM_SETTLEMENT_NAME", this.QuestFromSettlement.EncyclopediaLinkWithName);
				textObject.SetTextVariable("QUEST_TO_SETTLEMENT_NAME", this.QuestToSettlement.EncyclopediaLinkWithName);
				textObject.SetTextVariable("SPAWN_DAYS", 5);
				return textObject;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000DE0D File Offset: 0x0000C00D
		public override float ConspiracyStrengthDecreaseAmount
		{
			get
			{
				return 75f;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000DE14 File Offset: 0x0000C014
		private TextObject PlayerDefeatedCaravanLog
		{
			get
			{
				TextObject textObject = new TextObject("{=Db63Pe03}You have defeated the caravan and acquired its supplies. {OTHER_MENTOR.LINK}'s allies will not have their weapons. This will give us time and resources to prepare.", null);
				StringHelpers.SetCharacterProperties("MENTOR", base.Mentor.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("OTHER_MENTOR", StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? StoryModeHeroes.AntiImperialMentor.CharacterObject : StoryModeHeroes.ImperialMentor.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000DE7A File Offset: 0x0000C07A
		private TextObject MainHeroFailedToDisrupt
		{
			get
			{
				return new TextObject("{=9aRqqx3U}The caravan has delivered its supplies to the conspirators. A stronger adversary awaits us...", null);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000DE87 File Offset: 0x0000C087
		private TextObject MainHeroLostCombat
		{
			get
			{
				return new TextObject("{=bT9yspaQ}You have lost the battle against the conspiracy's caravan. A stronger adversary awaits us...", null);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000DE94 File Offset: 0x0000C094
		private Settlement QuestFromSettlement
		{
			get
			{
				return this._caravanTargetSettlements[0];
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000DE9E File Offset: 0x0000C09E
		private Settlement QuestToSettlement
		{
			get
			{
				return this._caravanTargetSettlements[this._caravanTargetSettlements.Length - 1];
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000DEB1 File Offset: 0x0000C0B1
		public MobileParty ConspiracyCaravan
		{
			get
			{
				return this._questCaravanMobileParty;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000DEB9 File Offset: 0x0000C0B9
		public int CaravanPartySize
		{
			get
			{
				return 70 + 70 * (int)this.GetQuestDifficultyMultiplier();
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
		public DisruptSupplyLinesConspiracyQuest(string questId, Hero questGiver)
			: base(questId, questGiver)
		{
			this._questStartTime = CampaignTime.Now;
			List<Settlement> list = new List<Settlement>();
			list.Add(this.GetQuestFromSettlement());
			for (int i = 1; i <= 6; i++)
			{
				list.Add(this.GetNextSettlement(list));
			}
			this._caravanTargetSettlements = new Settlement[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				this._caravanTargetSettlements[j] = list[j];
			}
			base.AddTrackedObject(this.QuestFromSettlement);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000DF50 File Offset: 0x0000C150
		private Settlement GetQuestFromSettlement()
		{
			Settlement centralSettlement = StoryModeHeroes.ImperialMentor.HomeSettlement;
			Settlement settlement = SettlementHelper.FindRandomSettlement(delegate(Settlement s)
			{
				if (s.IsTown && s.MapFaction != Clan.PlayerClan.MapFaction)
				{
					MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
					CampaignVec2 gatePosition = s.GatePosition;
					CampaignVec2 gatePosition2 = centralSettlement.GatePosition;
					if (mapDistanceModel.PathExistBetweenPoints(in gatePosition, in gatePosition2, MobileParty.NavigationType.Default))
					{
						if (!StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
						{
							return StoryModeData.IsKingdomImperial(s.OwnerClan.Kingdom);
						}
						return !StoryModeData.IsKingdomImperial(s.OwnerClan.Kingdom);
					}
				}
				return false;
			});
			if (settlement == null)
			{
				settlement = SettlementHelper.FindRandomSettlement(delegate(Settlement s)
				{
					if (s.IsTown)
					{
						MapDistanceModel mapDistanceModel2 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition3 = s.GatePosition;
						CampaignVec2 gatePosition4 = centralSettlement.GatePosition;
						if (mapDistanceModel2.PathExistBetweenPoints(in gatePosition3, in gatePosition4, MobileParty.NavigationType.Default))
						{
							if (!StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
							{
								return StoryModeData.IsKingdomImperial(s.OwnerClan.Kingdom);
							}
							return !StoryModeData.IsKingdomImperial(s.OwnerClan.Kingdom);
						}
					}
					return false;
				});
			}
			if (settlement == null)
			{
				settlement = SettlementHelper.FindRandomSettlement(delegate(Settlement s)
				{
					if (s.IsTown)
					{
						MapDistanceModel mapDistanceModel3 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition5 = s.GatePosition;
						CampaignVec2 gatePosition6 = centralSettlement.GatePosition;
						return mapDistanceModel3.PathExistBetweenPoints(in gatePosition5, in gatePosition6, MobileParty.NavigationType.Default);
					}
					return false;
				});
			}
			return settlement;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000DFB0 File Offset: 0x0000C1B0
		private Settlement GetNextSettlement(List<Settlement> caravanTargetSettlements)
		{
			Settlement centralSettlement = StoryModeHeroes.ImperialMentor.HomeSettlement;
			Settlement settlement = caravanTargetSettlements.Last<Settlement>();
			Town town = SettlementHelper.FindNearestTownToSettlement(settlement, MobileParty.NavigationType.Default, delegate(Settlement s)
			{
				if (!caravanTargetSettlements.Contains(s) && s.MapFaction != Clan.PlayerClan.MapFaction)
				{
					MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
					CampaignVec2 gatePosition = s.GatePosition;
					CampaignVec2 gatePosition2 = centralSettlement.GatePosition;
					if (mapDistanceModel.PathExistBetweenPoints(in gatePosition, in gatePosition2, MobileParty.NavigationType.Default) && (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? (!StoryModeData.IsKingdomImperial(s.OwnerClan.Kingdom)) : StoryModeData.IsKingdomImperial(s.OwnerClan.Kingdom)) && Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, s, false, false, MobileParty.NavigationType.Default) > 100f)
					{
						return Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, s, false, false, MobileParty.NavigationType.Default) < 500f;
					}
				}
				return false;
			});
			Settlement settlement2 = ((town != null) ? town.Settlement : null);
			if (settlement2 == null)
			{
				settlement2 = SettlementHelper.FindRandomSettlement(delegate(Settlement s)
				{
					if (!caravanTargetSettlements.Contains(s) && s.IsTown && s.MapFaction != Clan.PlayerClan.MapFaction)
					{
						MapDistanceModel mapDistanceModel2 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition3 = s.GatePosition;
						CampaignVec2 gatePosition4 = centralSettlement.GatePosition;
						if (mapDistanceModel2.PathExistBetweenPoints(in gatePosition3, in gatePosition4, MobileParty.NavigationType.Default) && Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, s, false, false, MobileParty.NavigationType.Default) > 100f)
						{
							return Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, s, false, false, MobileParty.NavigationType.Default) < 500f;
						}
					}
					return false;
				});
			}
			if (settlement2 == null)
			{
				settlement2 = SettlementHelper.FindRandomSettlement(delegate(Settlement s)
				{
					if (!caravanTargetSettlements.Contains(s) && s.IsTown)
					{
						MapDistanceModel mapDistanceModel3 = Campaign.Current.Models.MapDistanceModel;
						CampaignVec2 gatePosition5 = s.GatePosition;
						CampaignVec2 gatePosition6 = centralSettlement.GatePosition;
						if (mapDistanceModel3.PathExistBetweenPoints(in gatePosition5, in gatePosition6, MobileParty.NavigationType.Default) && Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, s, false, false, MobileParty.NavigationType.Default) > 100f)
						{
							return Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, s, false, false, MobileParty.NavigationType.Default) < 500f;
						}
					}
					return false;
				});
			}
			return settlement2;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000E03C File Offset: 0x0000C23C
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
			if (this._questCaravanMobileParty != null && this._questCaravanMobileParty.IsActive && this._questCaravanMobileParty.MapEventSide == null && this._questCaravanMobileParty.ActualClan != StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan)
			{
				this._questCaravanMobileParty.ActualClan = StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000E0B1 File Offset: 0x0000C2B1
		protected override void HourlyTick()
		{
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000E0B3 File Offset: 0x0000C2B3
		protected override void OnTimedOut()
		{
			MobileParty questCaravanMobileParty = this._questCaravanMobileParty;
			if (questCaravanMobileParty != null && questCaravanMobileParty.IsActive)
			{
				DestroyPartyAction.Apply(null, this._questCaravanMobileParty);
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000E0D8 File Offset: 0x0000C2D8
		protected override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000E12C File Offset: 0x0000C32C
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (this._questCaravanMobileParty != null && this._questCaravanMobileParty == party)
			{
				if (settlement == this.QuestToSettlement)
				{
					DestroyPartyAction.Apply(null, this._questCaravanMobileParty);
					this.FailedToDisrupt();
					return;
				}
				int num = Array.IndexOf<Settlement>(this._caravanTargetSettlements, settlement) + 1;
				SetPartyAiAction.GetActionForVisitingSettlement(this._questCaravanMobileParty, this._caravanTargetSettlements[num], MobileParty.NavigationType.Default, false, false);
				this._questCaravanMobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, 10);
				if (base.IsTracked(settlement))
				{
					base.RemoveTrackedObject(settlement);
				}
				base.AddTrackedObject(this._caravanTargetSettlements[num]);
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (this._questCaravanMobileParty != null && this._questCaravanMobileParty == party)
			{
				this.AddLogForSettlementVisit(settlement);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000E1E0 File Offset: 0x0000C3E0
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent && this._questCaravanMobileParty != null && mapEvent.InvolvedParties.Contains(this._questCaravanMobileParty.Party))
			{
				if (mapEvent.WinningSide == mapEvent.PlayerSide)
				{
					if (this._questCaravanMobileParty.Party.NumberOfHealthyMembers > 0 && this._questCaravanMobileParty.IsActive)
					{
						DestroyPartyAction.Apply(null, this._questCaravanMobileParty);
					}
					this.BattleWon();
					return;
				}
				if (mapEvent.WinningSide != BattleSideEnum.None)
				{
					if (this._questCaravanMobileParty.IsActive)
					{
						DestroyPartyAction.Apply(null, this._questCaravanMobileParty);
					}
					this.BattleLost();
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000E284 File Offset: 0x0000C484
		protected override void DailyTick()
		{
			if (this._questCaravanMobileParty == null && this._questStartTime.ElapsedDaysUntilNow >= 5f)
			{
				this.CreateQuestCaravanParty();
				this.SetDialogs();
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000E2BC File Offset: 0x0000C4BC
		private void AddLogForSettlementVisit(Settlement settlement)
		{
			TextObject textObject = new TextObject("{=SVcr0EJM}Caravan is moving on to {TO_SETTLEMENT_LINK} from {FROM_SETTLEMENT_LINK}.", null);
			int num = Array.IndexOf<Settlement>(this._caravanTargetSettlements, settlement) + 1;
			textObject.SetTextVariable("FROM_SETTLEMENT_LINK", settlement.EncyclopediaLinkWithName);
			textObject.SetTextVariable("TO_SETTLEMENT_LINK", this._caravanTargetSettlements[num].EncyclopediaLinkWithName);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ConspiracyQuestMapNotification(this, textObject));
			base.AddLog(textObject, false);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E330 File Offset: 0x0000C530
		private void CreateQuestCaravanParty()
		{
			PartyTemplateObject partyTemplateObject = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("conspiracy_anti_imperial_special_raider_party_template") : Campaign.Current.ObjectManager.GetObject<PartyTemplateObject>("conspiracy_imperial_special_raider_party_template"));
			Hero hero = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? StoryModeHeroes.AntiImperialMentor : StoryModeHeroes.ImperialMentor);
			string text;
			string text2;
			this.GetAdditionalVisualsForParty(this.QuestFromSettlement.Culture, out text, out text2);
			string[] array = new string[] { "aserai", "battania", "khuzait", "sturgia", "vlandia" };
			Clan conspiracyClan = StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyClan;
			this._questCaravanMobileParty = CustomPartyComponent.CreateCustomPartyWithPartyTemplate(this.QuestFromSettlement.GatePosition, 0f, this.QuestFromSettlement, new TextObject("{=eVzg5Mtl}Conspiracy Caravan", null), conspiracyClan, partyTemplateObject, hero, text, text2, 4f, true);
			this._questCaravanMobileParty.Aggressiveness = 0f;
			this._questCaravanMobileParty.MemberRoster.Clear();
			this._questCaravanMobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, 40);
			this._questCaravanMobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("fish"), 20);
			this._questCaravanMobileParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("butter"), 20);
			base.DistributeConspiracyRaiderTroopsByLevel(partyTemplateObject, this._questCaravanMobileParty.Party, this.CaravanPartySize);
			this._questCaravanMobileParty.IgnoreByOtherPartiesTill(base.QuestDueTime);
			this._questCaravanMobileParty.SetPartyUsedByQuest(true);
			SetPartyAiAction.GetActionForVisitingSettlement(this._questCaravanMobileParty, this._caravanTargetSettlements[1], MobileParty.NavigationType.Default, false, false);
			this._questCaravanMobileParty.Ai.SetDoNotMakeNewDecisions(true);
			base.AddTrackedObject(this._questCaravanMobileParty);
			this._questCaravanMobileParty.IgnoreByOtherPartiesTill(CampaignTime.DaysFromNow(21f));
			this.AddLogForSettlementVisit(this.QuestFromSettlement);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000E530 File Offset: 0x0000C730
		private void GetAdditionalVisualsForParty(CultureObject culture, out string mountStringId, out string harnessStringId)
		{
			if (culture.StringId == "aserai" || culture.StringId == "khuzait")
			{
				mountStringId = "camel";
				harnessStringId = ((MBRandom.RandomFloat > 0.5f) ? "camel_saddle_a" : "camel_saddle_b");
				return;
			}
			mountStringId = "mule";
			harnessStringId = ((MBRandom.RandomFloat > 0.5f) ? "mule_load_a" : ((MBRandom.RandomFloat > 0.5f) ? "mule_load_b" : "mule_load_c"));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
		private float GetQuestDifficultyMultiplier()
		{
			return MBMath.ClampFloat((0f + (float)Clan.PlayerClan.Fiefs.Count * 0.1f + Clan.PlayerClan.CurrentTotalStrength * 0.0008f + Clan.PlayerClan.Renown * 1.5E-05f + (float)Clan.PlayerClan.AliveLords.Count * 0.002f + (float)Clan.PlayerClan.Companions.Count * 0.01f + (float)Clan.PlayerClan.SupporterNotables.Count * 0.001f + (float)Hero.MainHero.OwnedCaravans.Count * 0.01f + (float)PartyBase.MainParty.NumberOfAllMembers * 0.002f + (float)CharacterObject.PlayerCharacter.Level * 0.002f) * 0.975f + MBRandom.RandomFloat * 0.025f, 0.1f, 1f);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		private void BattleWon()
		{
			base.AddLog(this.PlayerDefeatedCaravanLog, false);
			base.CompleteQuestWithSuccess();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000E6BA File Offset: 0x0000C8BA
		private void BattleLost()
		{
			base.AddLog(this.MainHeroLostCombat, false);
			base.CompleteQuestWithFail(null);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000E6D1 File Offset: 0x0000C8D1
		private void FailedToDisrupt()
		{
			base.AddLog(this.MainHeroFailedToDisrupt, false);
			base.CompleteQuestWithFail(null);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000015).NpcLine(new TextObject("{=ch9f3A1e}Greetings, {?PLAYER.GENDER}madam{?}sir{\\?}. Why did you stop our caravan? I trust you are not robbing us.", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.conversation_with_caravan_master_condition))
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=Xx94UrYe}I might be. What are you carrying? Honest goods, or weapons? How about you let us have a look.", null), null, null, null)
				.NpcLine(new TextObject("{=LXGXxKqw}Ah... Well, I suppose we can drop the charade. [ib:hip2][if:convo_nonchalant]I know who sent you, and I suppose you know who sent me. Certainly, you can see my wares, and then you can feel their sharp end in your belly.", null), null, null, null, null)
				.CloseDialog()
				.PlayerOption(new TextObject("{=cEaXehHy}I was just checking on something. You can move along.", null), null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.cancel_encounter_consequence))
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000E79F File Offset: 0x0000C99F
		private bool conversation_with_caravan_master_condition()
		{
			return this._questCaravanMobileParty != null && ConversationHelper.GetConversationCharacterPartyLeader(this._questCaravanMobileParty.Party) == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000E7C2 File Offset: 0x0000C9C2
		private void cancel_encounter_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000E7D1 File Offset: 0x0000C9D1
		internal static void AutoGeneratedStaticCollectObjectsDisruptSupplyLinesConspiracyQuest(object o, List<object> collectedObjects)
		{
			((DisruptSupplyLinesConspiracyQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E7DF File Offset: 0x0000C9DF
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._caravanTargetSettlements);
			collectedObjects.Add(this._questCaravanMobileParty);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._questStartTime, collectedObjects);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E811 File Offset: 0x0000CA11
		internal static object AutoGeneratedGetMemberValue_caravanTargetSettlements(object o)
		{
			return ((DisruptSupplyLinesConspiracyQuest)o)._caravanTargetSettlements;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000E81E File Offset: 0x0000CA1E
		internal static object AutoGeneratedGetMemberValue_questCaravanMobileParty(object o)
		{
			return ((DisruptSupplyLinesConspiracyQuest)o)._questCaravanMobileParty;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000E82B File Offset: 0x0000CA2B
		internal static object AutoGeneratedGetMemberValue_questStartTime(object o)
		{
			return ((DisruptSupplyLinesConspiracyQuest)o)._questStartTime;
		}

		// Token: 0x040000CC RID: 204
		private const int NumberOfSettlementsToVisit = 6;

		// Token: 0x040000CD RID: 205
		private const int SpawnCaravanWaitDaysAfterQuestStarted = 5;

		// Token: 0x040000CE RID: 206
		[SaveableField(1)]
		private readonly Settlement[] _caravanTargetSettlements;

		// Token: 0x040000CF RID: 207
		[SaveableField(2)]
		private MobileParty _questCaravanMobileParty;

		// Token: 0x040000D0 RID: 208
		[SaveableField(3)]
		private readonly CampaignTime _questStartTime;
	}
}
