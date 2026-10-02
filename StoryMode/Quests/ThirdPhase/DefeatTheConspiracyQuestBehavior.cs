using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.ThirdPhase
{
	// Token: 0x02000025 RID: 37
	public class DefeatTheConspiracyQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x0000A833 File Offset: 0x00008A33
		public bool IsMobilePartyCreatedForQuest(MobileParty mobileParty)
		{
			return this._partiesCreatedForQuest.Contains(mobileParty);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000A841 File Offset: 0x00008A41
		private TextObject _empireUnitedText
		{
			get
			{
				return new TextObject("{=zTYd6Qai}The Empire has been united under the {FACTION}!", null);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A84E File Offset: 0x00008A4E
		private TextObject _empireDefeatedText
		{
			get
			{
				return new TextObject("{=rCX81DDR}The Empire has been destroyed!", null);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A85C File Offset: 0x00008A5C
		public override void RegisterEvents()
		{
			StoryModeEvents.OnConspiracyActivatedEvent.AddNonSerializedListener(this, new Action(this.OnConspiracyActivated));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A8AE File Offset: 0x00008AAE
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._partiesCreatedForQuest.Contains(mobileParty))
			{
				this._partiesCreatedForQuest.Remove(mobileParty);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000A8CB File Offset: 0x00008ACB
		private void OnConspiracyActivated()
		{
			this.InitializeFinalPhase();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000A8D4 File Offset: 0x00008AD4
		private void HourlyTick()
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null && StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.IsEmpty<Kingdom>() && !this._hasBeenFinalized)
			{
				this._hasBeenFinalized = true;
				object obj = new TextObject("{=R4Gqskgq}Victory", null);
				TextObject textObject = (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? this._empireUnitedText : this._empireDefeatedText);
				textObject.SetTextVariable("FACTION", StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom.Name);
				TextObject textObject2 = new TextObject("{=DM6luo3c}Continue", null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, textObject2.ToString(), "", delegate
				{
					string text;
					string text2;
					string text3;
					if (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
					{
						text = "imperial_outro";
						text2 = "imperial_outro";
						text3 = "imperial_outro";
					}
					else
					{
						text = "anti_imperial_outro";
						text2 = "anti_imperial_outro";
						text3 = "anti_imperial_outro";
					}
					Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
					DefeatTheConspiracyQuestBehavior.PlayOutroCinematic(text, text2, text3, new Action(this.ShowGameStatistics));
				}, null, "", 0f, null, null, null), true, false);
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A9BC File Offset: 0x00008BBC
		protected void InitializeFinalPhase()
		{
			bool isOnImperialQuestLine = StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine;
			List<Kingdom> list = Kingdom.All.Where<Kingdom>((Kingdom t) => StoryModeData.IsKingdomImperial(t) && t != StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom && !t.IsEliminated).ToList<Kingdom>();
			List<Kingdom> list2 = Kingdom.All.Where<Kingdom>((Kingdom t) => !StoryModeData.IsKingdomImperial(t) && t != StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom && !t.IsEliminated).ToList<Kingdom>();
			List<DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest> list3 = new List<DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest>();
			List<Kingdom> list4;
			List<Kingdom> list5;
			if (isOnImperialQuestLine)
			{
				if (list2.IsEmpty<Kingdom>())
				{
					Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom t) => !StoryModeData.IsKingdomImperial(t));
					randomElementWithPredicate.ReactivateKingdom();
					list2.Add(randomElementWithPredicate);
				}
				list4 = list2.OrderByDescending<Kingdom, float>((Kingdom t) => t.CurrentTotalStrength).Take<Kingdom>(3).ToList<Kingdom>();
				list5 = new List<Kingdom> { StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom };
				for (int i = 0; i < list2.Count; i++)
				{
					for (int j = i + 1; j < list2.Count; j++)
					{
						if (!DiplomacyHelper.IsSameFactionAndNotEliminated(list2[i], list2[j]))
						{
							MakePeaceAction.Apply(list2[i], list2[j]);
						}
					}
				}
			}
			else
			{
				if (list.IsEmpty<Kingdom>())
				{
					Kingdom randomElementWithPredicate2 = Kingdom.All.GetRandomElementWithPredicate<Kingdom>(new Func<Kingdom, bool>(StoryModeData.IsKingdomImperial));
					randomElementWithPredicate2.ReactivateKingdom();
					list.Add(randomElementWithPredicate2);
				}
				list4 = list.ToList<Kingdom>();
				list5 = new List<Kingdom> { StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom };
				for (int k2 = 0; k2 < list.Count; k2++)
				{
					for (int l = k2 + 1; l < list.Count; l++)
					{
						if (!DiplomacyHelper.IsSameFactionAndNotEliminated(list[k2], list[l]))
						{
							MakePeaceAction.Apply(list[k2], list[l]);
						}
					}
				}
			}
			foreach (Kingdom kingdom5 in list5)
			{
				StoryModeManager.Current.MainStoryLine.ThirdPhase.AddAllyKingdom(kingdom5);
			}
			int num = 0;
			foreach (Kingdom kingdom2 in list4)
			{
				StoryModeManager.Current.MainStoryLine.ThirdPhase.AddOppositionKingdom(kingdom2);
				DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest defeatTheConspiracyQuest = new DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest("defeat_the_conspiracy_quest_" + num, kingdom2);
				num++;
				defeatTheConspiracyQuest.StartQuest();
				list3.Add(defeatTheConspiracyQuest);
			}
			Dictionary<Kingdom, int> dictionary = new Dictionary<Kingdom, int>();
			float conspiracyStrength = StoryModeManager.Current.MainStoryLine.SecondPhase.ConspiracyStrength;
			List<float> list6 = new List<float> { 0.5f, 0.3f, 0.2f };
			int num2 = 0;
			for (int m = 0; m < list6.Count; m++)
			{
				if (num2 > list4.Count - 1)
				{
					num2 = 0;
				}
				Kingdom kingdom3 = list4[num2];
				if (!dictionary.ContainsKey(kingdom3))
				{
					dictionary.Add(kingdom3, 0);
				}
				Dictionary<Kingdom, int> dictionary2 = dictionary;
				Kingdom kingdom4 = kingdom3;
				dictionary2[kingdom4] += (int)(conspiracyStrength * list6[m]);
				num2++;
			}
			foreach (KeyValuePair<Kingdom, int> keyValuePair in dictionary)
			{
				Kingdom kingdom = keyValuePair.Key;
				MBList<MobileParty> mblist = new MBList<MobileParty>();
				foreach (WarPartyComponent warPartyComponent in kingdom.WarPartyComponents)
				{
					if (warPartyComponent.Leader != null)
					{
						mblist.Add(warPartyComponent.MobileParty);
					}
				}
				MBList<CharacterObject> mblist2 = CharacterHelper.GetTroopTree(kingdom.Culture.BasicTroop, 3f, 6f).ToMBList<CharacterObject>();
				int num3 = keyValuePair.Value / 2;
				int num4 = num3 / 200;
				List<Hero> list7 = new List<Hero>();
				Clan clan = null;
				Func<Settlement, bool> <>9__9;
				Func<Hero, bool> <>9__8;
				for (int n = 0; n < num4; n++)
				{
					bool flag = false;
					if (clan == null || clan.AliveLords.Count >= clan.WarPartyLimit)
					{
						DefeatTheConspiracyQuestBehavior.<>c__DisplayClass16_1 CS$<>8__locals2 = new DefeatTheConspiracyQuestBehavior.<>c__DisplayClass16_1();
						DefeatTheConspiracyQuestBehavior.<>c__DisplayClass16_1 CS$<>8__locals3 = CS$<>8__locals2;
						NameGenerator nameGenerator = NameGenerator.Current;
						CultureObject culture = kingdom.Culture;
						MBReadOnlyList<Settlement> all = Settlement.All;
						Func<Settlement, bool> func;
						if ((func = <>9__9) == null)
						{
							func = (<>9__9 = (Settlement t) => t.Culture == kingdom.Culture && t.IsVillage);
						}
						CS$<>8__locals3.clanName = nameGenerator.GenerateClanName(culture, all.GetRandomElementWithPredicate<Settlement>(func));
						clan = Clan.CreateClan(string.Concat(new object[]
						{
							"main_storyline_clan_",
							CS$<>8__locals2.clanName.ToString(),
							"_",
							Clan.All.Count<Clan>((Clan t) => t.Name == CS$<>8__locals2.clanName)
						}));
						clan.ChangeClanName(CS$<>8__locals2.clanName, CS$<>8__locals2.clanName);
						clan.Culture = kingdom.Culture;
						clan.Banner = Banner.CreateRandomClanBanner(-1);
						clan.SetInitialHomeSettlement(kingdom.InitialHomeSettlement);
						clan.IsNoble = true;
						flag = true;
						clan.CalculateMidSettlement();
					}
					MBList<Settlement> mblist3 = kingdom.Settlements.Where<Settlement>((Settlement t) => !t.IsUnderSiege && !t.IsUnderRaid).ToMBList<Settlement>();
					Settlement settlement = null;
					if (!mblist3.IsEmpty<Settlement>())
					{
						settlement = mblist3.GetRandomElementWithPredicate<Settlement>((Settlement t) => t.IsTown);
					}
					if (settlement == null)
					{
						settlement = kingdom.InitialHomeSettlement;
					}
					Hero hero;
					if (mblist.Count <= 0)
					{
						MBReadOnlyList<Hero> allAliveHeroes = Hero.AllAliveHeroes;
						Func<Hero, bool> func2;
						if ((func2 = <>9__8) == null)
						{
							func2 = (<>9__8 = (Hero t) => t.Occupation == Occupation.Lord && t.Culture == kingdom.Culture);
						}
						hero = allAliveHeroes.GetRandomElementWithPredicate<Hero>(func2);
					}
					else
					{
						hero = mblist.GetRandomElement<MobileParty>().LeaderHero;
					}
					Hero hero2 = HeroCreator.CreateSpecialHero(hero.CharacterObject, settlement, clan, null, -1);
					GiveGoldAction.ApplyBetweenCharacters(null, hero2, 200000, true);
					hero2.ChangeState(Hero.CharacterStates.Active);
					if (clan.Leader == null)
					{
						clan.SetLeader(hero2);
					}
					if (clan.Kingdom != kingdom)
					{
						ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, default(CampaignTime), false);
					}
					if (clan.Kingdom.RulingClan == null || clan.Kingdom.RulingClan.IsEliminated)
					{
						ChangeRulingClanAction.Apply(clan.Kingdom, clan);
					}
					MobileParty mobileParty;
					if (settlement != null)
					{
						hero2.BornSettlement = settlement;
						EnterSettlementAction.ApplyForCharacterOnly(hero2, settlement);
						mobileParty = MobilePartyHelper.CreateNewClanMobileParty(hero2, clan);
						this._partiesCreatedForQuest.Add(mobileParty);
					}
					else
					{
						Clan rulingClan = kingdom.RulingClan;
						CampaignVec2 campaignVec = (((rulingClan != null) ? rulingClan.FactionMidSettlement : null) ?? kingdom.FactionMidSettlement).GatePosition;
						if (!NavigationHelper.IsPositionValidForNavigationType(campaignVec, MobileParty.NavigationType.Default))
						{
							campaignVec = Campaign.Current.MapSceneWrapper.GetAccessiblePointNearPosition(in campaignVec, Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default));
						}
						mobileParty = MobilePartyHelper.SpawnLordParty(hero2, campaignVec, 5f);
						this._partiesCreatedForQuest.Add(mobileParty);
						hero2.BornSettlement = Settlement.All.GetRandomElementWithPredicate<Settlement>((Settlement t) => t.IsTown || t.IsVillage);
					}
					mobileParty.MemberRoster.AddToCounts(mblist2.GetRandomElement<CharacterObject>(), 200, false, 0, 0, true, -1);
					mobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, 100);
					mobileParty.ItemRoster.AddToCounts(DefaultItems.Meat, 50);
					mobileParty.LordPartyComponent.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit);
					list7.Add(hero2);
					if (flag)
					{
						CampaignEventDispatcher.Instance.OnClanCreated(clan, false);
					}
				}
				if (mblist.IsEmpty<MobileParty>())
				{
					mblist.AddRange(list7.Select<Hero, MobileParty>((Hero t) => t.PartyBelongedTo));
				}
				if (!mblist.IsEmpty<MobileParty>())
				{
					int num5 = keyValuePair.Value - num3;
					int num6 = num3 - list7.Count * 200;
					int num7 = num5 + num6;
					int num8 = num7 / mblist.Count;
					int num9 = num7 % mblist.Count;
					if (num8 > 0)
					{
						foreach (MobileParty mobileParty2 in mblist)
						{
							for (int num10 = 0; num10 < num8; num10++)
							{
								mobileParty2.MemberRoster.AddToCounts(mblist2.GetRandomElement<CharacterObject>(), 1, false, 0, 0, true, -1);
							}
						}
					}
					if (num9 > 0)
					{
						MobileParty randomElement = mblist.GetRandomElement<MobileParty>();
						for (int num11 = 0; num11 < num9; num11++)
						{
							randomElement.MemberRoster.AddToCounts(mblist2.GetRandomElement<CharacterObject>(), 1, false, 0, 0, true, -1);
						}
					}
				}
			}
			for (int num12 = 0; num12 < list4.Count; num12++)
			{
				foreach (Clan clan2 in list4[num12].Clans)
				{
					clan2.UpdateCurrentStrength();
				}
				if (!list4[num12].IsAtWarWith(StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom))
				{
					ChangeRelationAction.ApplyPlayerRelation(list4[num12].Leader, -10, true, true);
					DeclareWarAction.ApplyByPlayerHostility(list4[num12], StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom);
				}
			}
			foreach (DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest defeatTheConspiracyQuest2 in list3)
			{
				defeatTheConspiracyQuest2.CalculateReinforcedWarScore();
			}
			Hero leader = list4[list4.IndexOfMax<Kingdom>((Kingdom k) => (int)k.CurrentTotalStrength)].Leader;
			SceneNotificationData sceneNotificationData;
			if (StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine)
			{
				sceneNotificationData = new AntiEmpireConspiracyBeginsSceneNotificationItem(leader, StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.ToList<Kingdom>());
			}
			else
			{
				sceneNotificationData = new ProEmpireConspiracyBeginsSceneNotificationItem(leader);
			}
			MBInformationManager.ShowSceneNotification(sceneNotificationData);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000B510 File Offset: 0x00009710
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<bool>("_hasBeenFinalized", ref this._hasBeenFinalized);
			dataStore.SyncData<List<MobileParty>>("_partiesCreatedForQuest", ref this._partiesCreatedForQuest);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B538 File Offset: 0x00009738
		private static void PlayOutroCinematic(string videoFile, string audioFile, string subtitleFile, Action onVideoFinished)
		{
			VideoPlaybackState videoPlaybackState = Game.Current.GameStateManager.CreateState<VideoPlaybackState>();
			string text = ModuleHelper.GetModuleFullPath("SandBox") + "Videos/CampaignOutro/";
			string text2 = text + videoFile + ".ivf";
			string text3 = text + audioFile + ".ogg";
			string text4 = text + subtitleFile;
			videoPlaybackState.SetStartingParameters(text2, text3, text4, 30f, true);
			videoPlaybackState.SetOnVideoFinisedDelegate(onVideoFinished);
			Game.Current.GameStateManager.PushState(videoPlaybackState, 0);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000B5B4 File Offset: 0x000097B4
		private void ShowGameStatistics()
		{
			GameOverState gameOverState = Game.Current.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.Victory });
			Game.Current.GameStateManager.PopState(0);
			Game.Current.GameStateManager.PushState(gameOverState, 0);
		}

		// Token: 0x040000AD RID: 173
		private const int TroopCountPerNewLord = 200;

		// Token: 0x040000AE RID: 174
		private bool _hasBeenFinalized;

		// Token: 0x040000AF RID: 175
		private List<MobileParty> _partiesCreatedForQuest = new List<MobileParty>();

		// Token: 0x040000B0 RID: 176
		public const int TroopLimitPerNewClanParty = 600;

		// Token: 0x02000063 RID: 99
		internal class OppositionData
		{
			// Token: 0x060005A9 RID: 1449 RVA: 0x000203AD File Offset: 0x0001E5AD
			internal static void AutoGeneratedStaticCollectObjectsOppositionData(object o, List<object> collectedObjects)
			{
				((DefeatTheConspiracyQuestBehavior.OppositionData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060005AA RID: 1450 RVA: 0x000203BB File Offset: 0x0001E5BB
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.QuestLog);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.LastPeaceOfferDate, collectedObjects);
			}

			// Token: 0x060005AB RID: 1451 RVA: 0x000203DA File Offset: 0x0001E5DA
			internal static object AutoGeneratedGetMemberValueInitialWarScore(object o)
			{
				return ((DefeatTheConspiracyQuestBehavior.OppositionData)o).InitialWarScore;
			}

			// Token: 0x060005AC RID: 1452 RVA: 0x000203EC File Offset: 0x0001E5EC
			internal static object AutoGeneratedGetMemberValueReinforcedWarScore(object o)
			{
				return ((DefeatTheConspiracyQuestBehavior.OppositionData)o).ReinforcedWarScore;
			}

			// Token: 0x060005AD RID: 1453 RVA: 0x000203FE File Offset: 0x0001E5FE
			internal static object AutoGeneratedGetMemberValueQuestLog(object o)
			{
				return ((DefeatTheConspiracyQuestBehavior.OppositionData)o).QuestLog;
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x0002040B File Offset: 0x0001E60B
			internal static object AutoGeneratedGetMemberValueLastPeaceOfferDate(object o)
			{
				return ((DefeatTheConspiracyQuestBehavior.OppositionData)o).LastPeaceOfferDate;
			}

			// Token: 0x0400020D RID: 525
			[SaveableField(10)]
			public float InitialWarScore;

			// Token: 0x0400020E RID: 526
			[SaveableField(20)]
			public float ReinforcedWarScore;

			// Token: 0x0400020F RID: 527
			[SaveableField(30)]
			public JournalLog QuestLog;

			// Token: 0x04000210 RID: 528
			[SaveableField(40)]
			public CampaignTime LastPeaceOfferDate = CampaignTime.Zero;
		}

		// Token: 0x02000064 RID: 100
		public class DefeatTheConspiracyQuestBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060005B0 RID: 1456 RVA: 0x00020430 File Offset: 0x0001E630
			public DefeatTheConspiracyQuestBehaviorTypeDefiner()
				: base(16000)
			{
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x0002043D File Offset: 0x0001E63D
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(DefeatTheConspiracyQuestBehavior.OppositionData), 1, null);
				base.AddClassDefinition(typeof(DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest), 2, null);
			}
		}

		// Token: 0x02000065 RID: 101
		public class DefeatTheConspiracyQuest : StoryModeQuestBase
		{
			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00020463 File Offset: 0x0001E663
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=Dfmg8stq}Eliminate {FACTION}", null);
					textObject.SetTextVariable("FACTION", this._oppositionKingdom.Name);
					return textObject;
				}
			}

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00020487 File Offset: 0x0001E687
			private TextObject _questCanceledLogText
			{
				get
				{
					return new TextObject("{=tVlZTOst}You have chosen a different path.", null);
				}
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00020494 File Offset: 0x0001E694
			private TextObject _defeatOpposingKingdomsQuestLogText
			{
				get
				{
					return new TextObject("{=ib2TKPUa}The ruler of the {FACTION} is leading the alliance against you. Defeat their armies or force them to make peace by capturing their settlements and destroying their parties to achieve victory.", null);
				}
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000204A1 File Offset: 0x0001E6A1
			private TextObject _defeatOpposingKingdomsProgressLogText
			{
				get
				{
					return new TextObject("{=3Io5vmOk}War Progress with the {FACTION}", null);
				}
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x060005B6 RID: 1462 RVA: 0x000204AE File Offset: 0x0001E6AE
			private TextObject _imperialKingdomDefeatedPopUpTitleText
			{
				get
				{
					return new TextObject("{=XWL3XcIq}{FACTION} Defeated", null);
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x060005B7 RID: 1463 RVA: 0x000204BB File Offset: 0x0001E6BB
			private TextObject _imperialKingdomDefeatedPopUpDescriptionText
			{
				get
				{
					return new TextObject("{=4SnDe0rA}The clans of the {FACTION} have defected to surrounding kingdoms as their leader has given up all hopes of restoring the Empire.", null);
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000204C8 File Offset: 0x0001E6C8
			private TextObject _imperialKingdomDefeatedQuestLogText
			{
				get
				{
					return new TextObject("{=OwcgxRXB}Weakened by the war, the {FACTION} has collapsed and its clans have defected to surrounding kingdoms.", null);
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000204D5 File Offset: 0x0001E6D5
			private TextObject _antiImperialKingdomDefeatedPopUpTitleText
			{
				get
				{
					return new TextObject("{=L3Qb6lbp}Peace Offer from the {FACTION}", null);
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x060005BA RID: 1466 RVA: 0x000204E2 File Offset: 0x0001E6E2
			private TextObject _antiImperialKingdomDefeatedPopUpKingDescriptionText
			{
				get
				{
					return new TextObject("{=E87miqTI}Exhausted from the war, the clans of the {FACTION} offer to make peace with the {PLAYER_SUPPORTED_FACTION}.", null);
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x060005BB RID: 1467 RVA: 0x000204EF File Offset: 0x0001E6EF
			private TextObject _antiImperialKingdomDefeatedPopUpSubjectDescriptionText
			{
				get
				{
					return new TextObject("{=hGPdLssq}The ruler of the {PLAYER_SUPPORTED_FACTION} has accepted the peace offered by the war-ravaged {FACTION}.", null);
				}
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x060005BC RID: 1468 RVA: 0x000204FC File Offset: 0x0001E6FC
			private TextObject _antiImperialKingdomDefeatedQuestLogText
			{
				get
				{
					return new TextObject("{=weS3DJKA}Weakened by war, the ruler of the {FACTION} offers to make peace with {PLAYER_FACTION}.", null);
				}
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x060005BD RID: 1469 RVA: 0x00020509 File Offset: 0x0001E709
			private TextObject _playerSupportedKingdomDestroyedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=eH8z6Fws}Despite its Dragon Banner, the {PLAYER_SUPPORTED_KINGDOM} has been destroyed!", null);
					textObject.SetTextVariable("PLAYER_SUPPORTED_KINGDOM", StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x00020536 File Offset: 0x0001E736
			public DefeatTheConspiracyQuest(string questId, Kingdom oppositionKingdom)
				: base(questId, StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine ? StoryModeHeroes.ImperialMentor : StoryModeHeroes.AntiImperialMentor, CampaignTime.Never)
			{
				this._oppositionKingdom = oppositionKingdom;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x00020574 File Offset: 0x0001E774
			protected override void SetDialogs()
			{
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x00020576 File Offset: 0x0001E776
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0002057E File Offset: 0x0001E77E
			protected override void HourlyTick()
			{
				this.UpdateWarProgressWithKingdom();
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x00020588 File Offset: 0x0001E788
			private void UpdateWarProgressWithKingdom()
			{
				float num = this.CalculateWarScoreForKingdom(this._oppositionKingdom);
				float reinforcedWarScore = this._oppositionData.ReinforcedWarScore;
				float num2 = this._oppositionData.InitialWarScore / 2f;
				int num3 = (int)MathF.Clamp((reinforcedWarScore - num) / (reinforcedWarScore - num2) * 100f, -100f, 100f);
				this._oppositionData.QuestLog.UpdateCurrentProgress(num3);
				if (MobileParty.MainParty.MapEvent == null && MobileParty.MainParty.SiegeEvent == null && !Hero.MainHero.IsPrisoner && num <= this._oppositionData.InitialWarScore / 2f && this._oppositionData.LastPeaceOfferDate.ElapsedDaysUntilNow >= (float)CampaignTime.DaysInWeek)
				{
					this._oppositionData.LastPeaceOfferDate = CampaignTime.Now;
					this.InitializeKingdomDefeatedPopUp(this._oppositionKingdom);
				}
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0002065C File Offset: 0x0001E85C
			private void InitializeKingdomDefeatedPopUp(Kingdom kingdom)
			{
				Kingdom playerSupportedKingdom = StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom;
				TextObject textObject;
				TextObject textObject2;
				if (StoryModeData.IsKingdomImperial(kingdom))
				{
					textObject = this._imperialKingdomDefeatedPopUpTitleText;
					textObject.SetTextVariable("FACTION", kingdom.Name);
					textObject2 = this._imperialKingdomDefeatedPopUpDescriptionText;
					textObject2.SetTextVariable("FACTION", kingdom.Name);
					textObject2.SetTextVariable("PLAYER_SUPPORTED_FACTION", playerSupportedKingdom.Name);
					TextObject textObject3 = new TextObject("{=DM6luo3c}Continue", null);
					InformationManager.ShowInquiry(new InquiryData(textObject.ToString(), textObject2.ToString(), true, false, textObject3.ToString(), "", delegate
					{
						this.OnKingdomDefeated(kingdom, true);
					}, null, "", 0f, null, null, null), true, false);
					return;
				}
				textObject = this._antiImperialKingdomDefeatedPopUpTitleText;
				textObject.SetTextVariable("FACTION", kingdom.Name);
				if (playerSupportedKingdom.Leader == Hero.MainHero)
				{
					textObject2 = this._antiImperialKingdomDefeatedPopUpKingDescriptionText;
					textObject2.SetTextVariable("FACTION", kingdom.Name);
					textObject2.SetTextVariable("PLAYER_SUPPORTED_FACTION", playerSupportedKingdom.Name);
					TextObject textObject4 = new TextObject("{=Y94H6XnK}Accept", null);
					TextObject textObject5 = new TextObject("{=cOgmdp9e}Decline", null);
					InformationManager.ShowInquiry(new InquiryData(textObject.ToString(), textObject2.ToString(), true, true, textObject4.ToString(), textObject5.ToString(), delegate
					{
						this.OnKingdomDefeated(kingdom, true);
					}, delegate
					{
						this.OnKingdomDefeated(kingdom, false);
					}, "", 0f, null, null, null), true, false);
					return;
				}
				textObject2 = this._antiImperialKingdomDefeatedPopUpSubjectDescriptionText;
				textObject2.SetTextVariable("FACTION", kingdom.Name);
				textObject2.SetTextVariable("PLAYER_SUPPORTED_FACTION", playerSupportedKingdom.Name);
				TextObject textObject6 = new TextObject("{=DM6luo3c}Continue", null);
				InformationManager.ShowInquiry(new InquiryData(textObject.ToString(), textObject2.ToString(), true, false, textObject6.ToString(), "", delegate
				{
					this.OnKingdomDefeated(kingdom, true);
				}, null, "", 0f, null, null, null), true, false);
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x00020880 File Offset: 0x0001EA80
			private void OnKingdomDefeated(Kingdom kingdom, bool makePeace = true)
			{
				if (this._oppositionKingdom == kingdom)
				{
					Kingdom playerSupportedKingdom = StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom;
					if (StoryModeData.IsKingdomImperial(kingdom))
					{
						StoryModeManager.Current.MainStoryLine.ThirdPhase.RemoveOppositionKingdom(kingdom);
						base.RemoveLog(this._oppositionData.QuestLog);
						TextObject imperialKingdomDefeatedQuestLogText = this._imperialKingdomDefeatedQuestLogText;
						imperialKingdomDefeatedQuestLogText.SetTextVariable("FACTION", kingdom.EncyclopediaLinkWithName);
						base.AddLog(imperialKingdomDefeatedQuestLogText, false);
						Kingdom kingdom2 = null;
						if (!StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.IsEmpty<Kingdom>())
						{
							kingdom2 = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.OrderBy<Kingdom, float>((Kingdom t) => Campaign.Current.Models.MapDistanceModel.GetDistance(kingdom.FactionMidSettlement, t.FactionMidSettlement, false, false, MobileParty.NavigationType.Default)).FirstOrDefault<Kingdom>();
						}
						else
						{
							List<Kingdom> list = Kingdom.All.Where<Kingdom>((Kingdom t) => !StoryModeData.IsKingdomImperial(t) && !t.IsEliminated && t != playerSupportedKingdom).ToList<Kingdom>();
							if (list.IsEmpty<Kingdom>())
							{
								kingdom2 = playerSupportedKingdom;
							}
							else
							{
								kingdom2 = list.OrderBy<Kingdom, float>((Kingdom t) => Campaign.Current.Models.MapDistanceModel.GetDistance(kingdom.FactionMidSettlement, t.FactionMidSettlement, false, false, MobileParty.NavigationType.Default)).FirstOrDefault<Kingdom>();
								bool flag = false;
								foreach (Settlement settlement in new List<Settlement>(kingdom.Settlements))
								{
									if (settlement.SiegeEvent != null)
									{
										if (settlement.SiegeEvent.IsPlayerSiegeEvent && !flag)
										{
											flag = true;
										}
										settlement.SiegeEvent.FinalizeSiegeEvent();
									}
									ChangeOwnerOfSettlementAction.ApplyByLeaveFaction(playerSupportedKingdom.Leader, settlement);
								}
								if (flag)
								{
									GameMenu.ActivateGameMenu("siege_ended_by_last_conspiracy_kingdom_defeat");
								}
							}
						}
						if (kingdom2 != null)
						{
							this.DefectClansOfKingdomToKingdom(kingdom, kingdom2);
						}
						else
						{
							Debug.FailedAssert("Kingdom to defect can't be found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Quests\\ThirdPhase\\DefeatTheConspiracyQuestBehavior.cs", "OnKingdomDefeated", 396);
						}
					}
					else
					{
						StoryModeManager.Current.MainStoryLine.ThirdPhase.RemoveOppositionKingdom(kingdom);
						base.RemoveLog(this._oppositionData.QuestLog);
						TextObject antiImperialKingdomDefeatedQuestLogText = this._antiImperialKingdomDefeatedQuestLogText;
						antiImperialKingdomDefeatedQuestLogText.SetTextVariable("FACTION", kingdom.EncyclopediaLinkWithName);
						antiImperialKingdomDefeatedQuestLogText.SetTextVariable("PLAYER_FACTION", playerSupportedKingdom.EncyclopediaLinkWithName);
						base.AddLog(antiImperialKingdomDefeatedQuestLogText, false);
						if (makePeace)
						{
							MakePeaceAction.Apply(playerSupportedKingdom, kingdom);
						}
						if (StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.IsEmpty<Kingdom>())
						{
							Kingdom playerSupportedKingdom2 = playerSupportedKingdom;
							foreach (Kingdom kingdom3 in Kingdom.All.Where<Kingdom>((Kingdom t) => !t.IsEliminated && StoryModeData.IsKingdomImperial(t)))
							{
								if (kingdom3 != playerSupportedKingdom2)
								{
									this.DefectClansOfKingdomToKingdom(kingdom3, playerSupportedKingdom2);
								}
							}
						}
					}
					base.CompleteQuestWithSuccess();
				}
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x00020BA4 File Offset: 0x0001EDA4
			private void DefectClansOfKingdomToKingdom(Kingdom defectorKingdom, Kingdom targetKingdom)
			{
				foreach (Clan clan in new List<Clan>(defectorKingdom.Clans))
				{
					if (clan == Clan.PlayerClan)
					{
						ChangeKingdomAction.ApplyByLeaveKingdom(Clan.PlayerClan, true);
					}
					else if (clan.IsUnderMercenaryService)
					{
						ChangeKingdomAction.ApplyByJoinFactionAsMercenary(clan, targetKingdom, CampaignTime.Zero, clan.MercenaryAwardMultiplier, false);
					}
					else
					{
						ChangeKingdomAction.ApplyByJoinToKingdom(clan, targetKingdom, default(CampaignTime), false);
					}
				}
				DestroyKingdomAction.Apply(defectorKingdom);
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x00020C40 File Offset: 0x0001EE40
			private void OnKingdomDestroyed(Kingdom kingdom)
			{
				if (StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom == kingdom)
				{
					base.AddLog(this._playerSupportedKingdomDestroyedLogText, false);
					base.CompleteQuestWithFail(null);
					TextObject textObject = new TextObject("{=atnUtXdO}The {KINGDOM_NAME} has been defeated. Your quest to restore the Empire has failed.", null);
					if (!StoryModeManager.Current.MainStoryLine.IsOnAntiImperialQuestLine)
					{
						textObject = new TextObject("{=r48aEAbq}The {KINGDOM_NAME} has been defeated. Your quest to destroy the Empire has failed.", null);
					}
					textObject.SetTextVariable("KINGDOM_NAME", StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom.Name);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x00020CCC File Offset: 0x0001EECC
			private float CalculateWarScoreForKingdom(Kingdom kingdom)
			{
				float num = 0f;
				foreach (Settlement settlement in kingdom.Settlements)
				{
					num += this.GetWarScoreOfSettlement(settlement);
				}
				num += kingdom.CurrentTotalStrength;
				return num;
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x00020D34 File Offset: 0x0001EF34
			private float GetWarScoreOfSettlement(Settlement settlement)
			{
				float num = 0f;
				if (settlement.IsTown)
				{
					num = 3000f;
				}
				else if (settlement.IsCastle)
				{
					num = 1000f;
				}
				return num;
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x00020D68 File Offset: 0x0001EF68
			protected override void RegisterEvents()
			{
				CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
				CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnCampaignQuestCompleted));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.SettlementOwnerChanged));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x00020DD1 File Offset: 0x0001EFD1
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom)
				{
					base.CompleteQuestWithCancel(this._questCanceledLogText);
					StoryModeManager.Current.MainStoryLine.CancelSecondAndThirdPhase();
				}
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x00020E08 File Offset: 0x0001F008
			private void OnCampaignQuestCompleted(QuestBase completedQuest, QuestBase.QuestCompleteDetails detail)
			{
				if (completedQuest != this && completedQuest is DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest)
				{
					this.UpdateWarProgressWithKingdom();
					StoryModeManager.Current.MainStoryLine.ThirdPhase.CompleteThirdPhase(detail);
				}
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x00020E31 File Offset: 0x0001F031
			private void SettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (((newOwner != null && newOwner.MapFaction == this._oppositionKingdom) || (oldOwner != null && oldOwner.MapFaction == this._oppositionKingdom)) && this.GetWarScoreOfSettlement(settlement) > 0f)
				{
					this.UpdateWarProgressWithKingdom();
				}
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x00020E6C File Offset: 0x0001F06C
			protected override void OnStartQuest()
			{
				DefeatTheConspiracyQuestBehavior.OppositionData oppositionData = new DefeatTheConspiracyQuestBehavior.OppositionData();
				oppositionData.InitialWarScore = this.CalculateWarScoreForKingdom(this._oppositionKingdom);
				oppositionData.ReinforcedWarScore = 0f;
				TextObject defeatOpposingKingdomsQuestLogText = this._defeatOpposingKingdomsQuestLogText;
				TextObject defeatOpposingKingdomsProgressLogText = this._defeatOpposingKingdomsProgressLogText;
				defeatOpposingKingdomsQuestLogText.SetTextVariable("FACTION", this._oppositionKingdom.EncyclopediaLinkWithName);
				defeatOpposingKingdomsProgressLogText.SetTextVariable("FACTION", this._oppositionKingdom.EncyclopediaLinkWithName);
				oppositionData.QuestLog = base.AddTwoWayContinuousLog(defeatOpposingKingdomsQuestLogText, defeatOpposingKingdomsProgressLogText, 0, 100, false);
				this._oppositionData = oppositionData;
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x00020EF1 File Offset: 0x0001F0F1
			public void CalculateReinforcedWarScore()
			{
				this._oppositionData.ReinforcedWarScore = this.CalculateWarScoreForKingdom(this._oppositionKingdom);
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x00020F0A File Offset: 0x0001F10A
			internal static void AutoGeneratedStaticCollectObjectsDefeatTheConspiracyQuest(object o, List<object> collectedObjects)
			{
				((DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060005D0 RID: 1488 RVA: 0x00020F18 File Offset: 0x0001F118
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._oppositionKingdom);
				collectedObjects.Add(this._oppositionData);
			}

			// Token: 0x060005D1 RID: 1489 RVA: 0x00020F39 File Offset: 0x0001F139
			internal static object AutoGeneratedGetMemberValue_oppositionKingdom(object o)
			{
				return ((DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest)o)._oppositionKingdom;
			}

			// Token: 0x060005D2 RID: 1490 RVA: 0x00020F46 File Offset: 0x0001F146
			internal static object AutoGeneratedGetMemberValue_oppositionData(object o)
			{
				return ((DefeatTheConspiracyQuestBehavior.DefeatTheConspiracyQuest)o)._oppositionData;
			}

			// Token: 0x04000211 RID: 529
			private const int ProgressTrackerRange = 100;

			// Token: 0x04000212 RID: 530
			[SaveableField(100)]
			private Kingdom _oppositionKingdom;

			// Token: 0x04000213 RID: 531
			[SaveableField(110)]
			private DefeatTheConspiracyQuestBehavior.OppositionData _oppositionData;
		}
	}
}
