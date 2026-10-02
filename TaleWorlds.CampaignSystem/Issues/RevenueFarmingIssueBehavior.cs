using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000390 RID: 912
	public class RevenueFarmingIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060035DF RID: 13791 RVA: 0x000DC43A File Offset: 0x000DA63A
		private float IncidentChance
		{
			get
			{
				return (100f - RevenueFarmingIssueBehavior.Instance.TargetSettlement.Town.Loyalty * 0.8f) * 0.01f;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060035E0 RID: 13792 RVA: 0x000DC464 File Offset: 0x000DA664
		private static RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest Instance
		{
			get
			{
				RevenueFarmingIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<RevenueFarmingIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest revenueFarmingIssueQuest;
						if ((revenueFarmingIssueQuest = enumerator.Current as RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest) != null)
						{
							campaignBehavior._cachedQuest = revenueFarmingIssueQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000DC4FC File Offset: 0x000DA6FC
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunchedEvent));
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000DC550 File Offset: 0x000DA750
		private void OnAfterSessionLaunchedEvent(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenuOption("town_guard", "talk_to_steward_for_revenue_town", "{=voXpzZdH}Hand over the revenue", new GameMenuOption.OnConditionDelegate(this.talk_to_steward_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_steward_on_consequence), false, 2, false, null);
			gameStarter.AddGameMenuOption("town", "talk_to_steward_for_revenue_town", "{=voXpzZdH}Hand over the revenue", new GameMenuOption.OnConditionDelegate(this.talk_to_steward_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_steward_on_consequence), false, 8, false, null);
			gameStarter.AddGameMenuOption("castle_guard", "talk_to_steward_for_revenue_castle", "{=voXpzZdH}Hand over the revenue", new GameMenuOption.OnConditionDelegate(this.talk_to_steward_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_steward_on_consequence), false, 2, false, null);
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000DC5F0 File Offset: 0x000DA7F0
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenuOption("village", "revenue_farming_quest_collect_tax_menu_button", "{=mcrjFxDQ}Collect revenue", new GameMenuOption.OnConditionDelegate(this.collect_revenue_menu_condition), new GameMenuOption.OnConsequenceDelegate(this.collect_revenue_menu_consequence), false, 4, false, null);
			gameStarter.AddWaitGameMenu("village_collect_revenue", "{=p6swAFWn}Your men started collecting the revenues...", new OnInitDelegate(this.collecting_menu_on_init), null, null, new OnTickDelegate(this.collection_menu_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameMenu.MenuOverlayType.None, 10f, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("village_collect_revenue", "village_collect_revenue_back", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_consequence), true, -1, false, null);
			this.AddVillageEvents(gameStarter);
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000DC69C File Offset: 0x000DA89C
		private bool talk_to_steward_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
			if (RevenueFarmingIssueBehavior.Instance != null)
			{
				if (Hero.MainHero.Gold < RevenueFarmingIssueBehavior.Instance._totalRequestedDenars)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=QOWyEJrm}You don't have enough denars.", null);
				}
				if (!RevenueFarmingIssueBehavior.Instance._allRevenuesAreCollected)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=QrAowQ5f}You have to collect the revenues first.", null);
				}
				return Settlement.CurrentSettlement.OwnerClan == RevenueFarmingIssueBehavior.Instance.QuestGiver.Clan;
			}
			return false;
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000DC72E File Offset: 0x000DA92E
		private void talk_to_steward_on_consequence(MenuCallbackArgs args)
		{
			RevenueFarmingIssueBehavior.Instance.RevenuesAreDeliveredToSteward();
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000DC75C File Offset: 0x000DA95C
		private void AddVillageEvents(CampaignGameStarter gameStarter)
		{
			this._villageEvents = new List<RevenueFarmingIssueBehavior.VillageEvent>();
			string text = "{=RabC7Wzm}The headman tells you that most of the villagers can't afford the rest of the tax. They offer crops and other goods as payment in kind.";
			TextObject textObject = new TextObject("{=5hgc03yZ}While your men were collecting revenues, a headman came and told you that most of the villagers couldn't afford to pay what they owe. They offered to pay the rest with their products.", null);
			List<RevenueFarmingIssueBehavior.VillageEventOptionData> list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=XVzQ7MXQ}Refuse the offer, break into their homes and collect all rents and taxes by force.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 10)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				TraitLevelingHelper.OnIssueSolvedThroughQuest(RevenueFarmingIssueBehavior.Instance.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Mercy, -50)
				});
				int num = MBRandom.RandomInt(2, 4);
				TextObject textObject2 = new TextObject("{=3vFxRKja}You refused his offer and decided to collect the rest of the revenues by force. Your action upset the village notables and made villagers angry. Some villagers tried to resist. In the brawl, {WOUNDED_COUNT} of your men got wounded.", null);
				textObject2.SetTextVariable("WOUNDED_COUNT", num);
				RevenueFarmingIssueBehavior.Instance.AddLog(textObject2, false);
				this.ChangeRelationWithNotables(-5);
				int num2 = MBRandom.RandomInt(2, 4);
				MobileParty.MainParty.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(num2);
				TextObject textObject3 = new TextObject("{=o27lTMD4}Some villagers tried to resist. In the brawl, {WOUNDED_NUMBER} of your men got wounded.", null);
				textObject3.SetTextVariable("WOUNDED_NUMBER", num2);
				MBInformationManager.AddQuickInformation(textObject3, 0, null, null, "");
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=buKXELE3}Accept the offer.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Trade;
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
				int num3 = (int)((float)revenueVillage.TargetAmount * 0.5f / (float)revenueVillage.Village.VillageType.PrimaryProduction.Value);
				TextObject textObject4 = new TextObject("{=wZfbYfoH}They will give you {PRODUCT_COUNT} {.%}{?(PRODUCT_COUNT > 1)}{PLURAL(PRODUCT)}{?}{PRODUCT}{\\?}{.%}.", null);
				textObject4.SetTextVariable("PRODUCT", revenueVillage.Village.VillageType.PrimaryProduction.Name);
				textObject4.SetTextVariable("PRODUCT_COUNT", num3);
				args.Tooltip = textObject4;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				int num4;
				this.GiveVillageGoods(out num4);
				TextObject textObject5 = new TextObject("{=b5InObbq}You accepted the headman's offer. The village's notables and villagers were happy with your decision and they gave you {PRODUCT_COUNT} {.%}{?(PRODUCT_COUNT > 1)}{PLURAL(PRODUCT)}{?}{PRODUCT}{\\?}{.%}.", null);
				textObject5.SetTextVariable("PRODUCT", RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().Village.VillageType.PrimaryProduction.Name);
				textObject5.SetTextVariable("PRODUCT_COUNT", num4);
				RevenueFarmingIssueBehavior.Instance.AddLog(textObject5, false);
				this.ChangeRelationWithNotables(1);
				this.CompleteCurrentRevenueCollection(true);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=jULnw6F1}Leave the village, telling the villagers that they are exempted from payment this year.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=a3WpsFTM}You decided to exempt the rest of the villagers from payment and left the village. The village's notables and farmers were grateful to you.", null), false);
				this.ChangeRelationWithNotables(3);
				this.CompleteCurrentRevenueCollection(true);
			}, false));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("offer_goods_and_troops", text, textObject, list));
			text = "{=tVYLzFwu}Suddenly a brawl starts between your troops and some of the village youth.";
			textObject = new TextObject("{=vKaeKPJ5}Revenue collection was interrupted by a sudden brawl between your troops and young men of the village.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=eJegI0iX}Order the rest of your troops to put the village youth to flight.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Mission;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 10)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=Zx1ZEl6Q}You ordered your troops to fight back. In the heat of the brawl, one of the young men was struck on the head and killed. His death greatly upset the villagers.", null), false);
				MBInformationManager.AddQuickInformation(new TextObject("{=xfEVlh7v}Your men beat some of youngsters to the death.", null), 0, null, null, "");
				this.ChangeRelationWithNotables(-5);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=Z6IoX4MH}Order your troops to try not to hurt the youth and try to separate the two sides.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 10)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				int num5 = MBRandom.RandomInt(6, 10);
				TextObject textObject6 = new TextObject("{=YRocrk78}You ordered your troops to disengage. When the dust settled, {WOUNDED} of them had been injured. But the village notables understood that you wanted a peaceful solution.", null);
				textObject6.SetTextVariable("WOUNDED", num5);
				RevenueFarmingIssueBehavior.Instance.AddLog(textObject6, false);
				TextObject textObject7 = new TextObject("{=00Qvwelb}{WOUNDED_NUMBER} of your men got wounded while they were trying to separate the two sides.", null);
				textObject7.SetTextVariable("WOUNDED_NUMBER", num5);
				MBInformationManager.AddQuickInformation(textObject7, 0, null, null, "");
				MobileParty.MainParty.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(num5);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=Xl5JTBJE}Leave the village, telling them you've collected enough.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=T0feOigD}You decided to stop collecting revenues and leave the village. You told the villagers that they had paid enough, and they were grateful to you.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("brawl_breaks_out", text, textObject, list));
			text = "{=cOlZvnal}A landlord says that his retainers, who help keep order in the village, have gone unpaid and are starting to get mutinous. He says that if you can't help him out with a small sum of money to pay them while you collect the revenues from the rest of the village, he can't guarantee that things will go peacefully.";
			textObject = new TextObject("{=HK4pwetq}A few hours after the revenue collection started, a landlord came and told you that his retainers were getting mutinuous. He asked you for some money to pay them their back wages.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=0p0jXXIa}Reject the landlord's request for money and collect revenues as before.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				this.ChangeRelationWithNotables(-5);
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=bS7IAgJS}You told the notable that this was not your affair. A few hours later, the mutineers ambushed and killed some of your men on the outskirts of the village, and the revenues stolen. Your men completed collecting revenues from the village, but lost it all to an ambush.", null), false);
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().CollectedAmount, true);
					TextObject textObject8 = GameTexts.FindText("str_quest_collect_debt_quest_gold_removed", null);
					textObject8.SetTextVariable("GOLD_AMOUNT", RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().CollectedAmount);
					InformationManager.DisplayMessage(new InformationMessage(textObject8.ToString(), "event:/ui/notification/coins_negative"));
					this.CompleteCurrentRevenueCollection(false);
					int num6 = MBRandom.RandomInt(2, 5);
					TextObject textObject9 = new TextObject("{=mosHZG3b}The mutineers ambushed and killed {KILLED_NUMBER} of your men.", null);
					textObject9.SetTextVariable("KILLED_NUMBER", num6);
					MBInformationManager.AddQuickInformation(textObject9, 0, null, null, "");
					MobileParty.MainParty.MemberRoster.RemoveNumberOfNonHeroTroopsRandomly(num6);
					return;
				}
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=KQ8AU8Bz}You told the notable that this was not your affair. He did not like to hear this.", null), false);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=EmJDw5xP}Give the landlord a small bribe for his men, and continue to collect revenues with their help.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Trade;
				int num7 = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().TargetAmount / 3;
				if (Hero.MainHero.Gold < num7)
				{
					args.Tooltip = new TextObject("{=m6uSOtE4}You don't have enough money.", null);
					args.IsEnabled = false;
				}
				else
				{
					TextObject textObject10 = new TextObject("{=hCavIm4G}You will pay {AMOUNT}{GOLD_ICON} denars.", null);
					textObject10.SetTextVariable("AMOUNT", num7);
					args.Tooltip = textObject10;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=kp19y5Hh}You paid off the landlords' retainers to forestall a mutiny. The village notables were grateful to you.", null), false);
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().TargetAmount / 3, false);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=DhrjR8bs}Announce that the villagers have paid enough, and leave the village.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=1yCeyK4I}You declared that the village had paid enough, and departed.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("landlord_asks_for_money", text, textObject, list));
			text = "{=pBII35Fa}As your man were collecting the tax, the headman shouted out to you across the fields that there has been an outbreak of the flux in the village. He warns you, for your own good, to stay away.";
			textObject = new TextObject("{=fn59IIUf}As your man were collecting the tax, the headman shouted out to you that the village had seen an outbreak of the flux, and that you should stay away.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=CbapENnw}Tell your men that the headman is probably lying, and to go ahead and collect the revenues.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=9AyDNhQj}You told your men to ignore the warning. Several hours after you left, some of your men started experiencing chills, then diarrhea. This does not appear to be a particularly virulent strain, as no one died, but about half your men are incapacitated.", null), false);
					int num8 = MobileParty.MainParty.MemberRoster.TotalHealthyCount / 2;
					TextObject textObject11 = new TextObject("{=rmmZayCT}{WOUNDED_COUNT} of your men got wounded because of illness.", null);
					textObject11.SetTextVariable("WOUNDED_COUNT", num8);
					MBInformationManager.AddQuickInformation(textObject11, 0, null, null, "");
					MobileParty.MainParty.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(num8);
				}
				else
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=obhcbsQT}You told your men to ignore the warning.", null), false);
				}
				this.ChangeRelationWithNotables(-4);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=iE5vWYj2}Tell your men to be careful, and to touch nothing in a house where anyone has been sick.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage2 = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=b3GrvocA}You told your men to go carefully, but still collect the revenues. The village notables seemed upset with your decision.", null), false);
				revenueVillage2.SetAdditionalProgress(0.35f);
				this.ChangeRelationWithNotables(-2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=YZZ4zjxU}Tell the villagers that, in light of their hardship, they are forgiven what they owe.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=JSI0FVZ1}You decided to forgive the villagers' back payments. The headman thanked you, as the villagers were already suffering.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("village_is_under_quarantine", text, textObject, list));
			text = "{=yPkHn74X}When you enter the village commons, you find a crowd of villagers has gathered to resist you. They call the rents and taxes owed 'unlawful' and refuse to pay them at all. They pelt your men with rotten vegetables.";
			textObject = new TextObject("{=yPkHn74X}When you enter the village commons, you find a crowd of villagers has gathered to resist you. They call the rents and taxes owed 'unlawful' and refuse to pay them at all. They pelt your men with rotten vegetables.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=aZ9bME9C}Order your men to break up the crowd by force", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount <= 9)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=ztY2Nf0N}You ordered your men to break up the crowd. There was some scuffling, and some of your men were wounded.", null), false);
					int num9 = MBRandom.RandomInt(6, 8);
					TextObject textObject12 = new TextObject("{=xJwo7eBh}{WOUNDED_NUMBER} of your men got wounded while they were breaking up the crowd.", null);
					textObject12.SetTextVariable("WOUNDED_NUMBER", num9);
					MBInformationManager.AddQuickInformation(textObject12, 0, null, null, "");
					MobileParty.MainParty.MemberRoster.WoundNumberOfNonHeroTroopsRandomly(num9);
				}
				else
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=ObYvBt0e}You ordered your men to break up the crowd. The attempt was successful and your men continued collecting taxes as usual.", null), false);
				}
				this.ChangeRelationWithNotables(-5);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=4MPhLYcT}Bargain with the group, agreeing to forgive the debts of the poorest villagers", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().SetAdditionalProgress(0.5f);
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=54RyKzPJ}After your attempts to bargain, a deal has been made to forgive the debts of the poorest villagers.", null), false);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=tZw45isr}Tell the villagers that they made their point and that you're leaving", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=6TYsIQav}After observing the villagers' hardships, you called back your men so as not put any more burden on them.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("refuse_to_pay_what_they_owe", text, textObject, list));
			text = "{=Tl4yagLi}The headman tells you that some households have suffered particularly hard this year from crop failures and bandit depredations. He asks you to forgive their back payments entirely. He hints that they are so desperate that they might resist by force.";
			textObject = new TextObject("{=Tl4yagLi}The headman tells you that some households have suffered particularly hard this year from crop failures and bandit depredations. He asks you to forgive their back payments entirely. He hints that they are so desperate that they might resist by force.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=agMtRiru}Refuse to exempt anyone", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=VsriS0iI}You refused to exempt anyone, but the residents attacked and killed some of your troops who were separated from the rest, and then ran away.", null), false);
					int num10 = MBRandom.RandomInt(2, 4);
					TextObject textObject13 = new TextObject("{=MGD8Ka2o}The residents attacked and killed {KILLED_NUMBER} of your troops who were separated from the rest.", null);
					textObject13.SetTextVariable("KILLED_NUMBER", num10);
					MBInformationManager.AddQuickInformation(textObject13, 0, null, null, "");
					MobileParty.MainParty.MemberRoster.RemoveNumberOfNonHeroTroopsRandomly(num10);
				}
				else
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=Rz1kkvbK}You refused to exempt anyone. Fortunately the villagers were sufficiently cowed by your men, and did not raise a hand.", null), false);
				}
				this.ChangeRelationWithNotables(-5);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=WDp5EAl3}Agree to exempt the poor households", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().SetAdditionalProgress(0.35f);
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=o70h6xqb}You showed mercy and exempted the poor households from the tax collection", null), false);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=aMleZjlG}Tell the villagers that they have all paid enough, and depart", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=rsQhD7sC}You thought that the villagers have paid enough, so departed from the settlement", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("relief_for_the_poorest", text, textObject, list));
			foreach (RevenueFarmingIssueBehavior.VillageEvent villageEvent in this._villageEvents)
			{
				this.AddVillageEventMenus(villageEvent, gameStarter);
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000DCD18 File Offset: 0x000DAF18
		private void AddVillageEventMenus(RevenueFarmingIssueBehavior.VillageEvent villageEvent, CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu(villageEvent.Id, villageEvent.MainEventText, null, GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			for (int i = 0; i < villageEvent.OptionConditionsAndConsequences.Count; i++)
			{
				RevenueFarmingIssueBehavior.VillageEventOptionData villageEventOptionData = villageEvent.OptionConditionsAndConsequences[i];
				gameStarter.AddGameMenuOption(villageEvent.Id, "Id_option" + i, villageEventOptionData.Text, villageEventOptionData.OnCondition, villageEventOptionData.OnConsequence, villageEventOptionData.IsLeave, -1, false, null);
			}
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000DCD98 File Offset: 0x000DAF98
		private bool collect_revenue_menu_condition(MenuCallbackArgs args)
		{
			if (RevenueFarmingIssueBehavior.Instance == null || !RevenueFarmingIssueBehavior.Instance.IsOngoing || (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty))
			{
				return false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.RevenueVillages.FirstOrDefault<RevenueFarmingIssueBehavior.RevenueVillage>((RevenueFarmingIssueBehavior.RevenueVillage x) => x.Village == Settlement.CurrentSettlement.Village);
			if (revenueVillage != null && !revenueVillage.GetIsCompleted())
			{
				bool flag = MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 20;
				TextObject textObject = new TextObject("{=CfCsGTfb}Villagers are not taking you seriously, as you do not have enough soldiers to carry out the process. At least 20 men are needed to continue.", null);
				return MenuHelper.SetOptionProperties(args, flag, !flag, textObject);
			}
			return false;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x000DCE5C File Offset: 0x000DB05C
		private bool leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x000DCE67 File Offset: 0x000DB067
		private void leave_consequence(MenuCallbackArgs args)
		{
			RevenueFarmingIssueBehavior.Instance.CollectingRevenues = false;
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000DCE80 File Offset: 0x000DB080
		private void collecting_menu_on_init(MenuCallbackArgs args)
		{
			if (RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().CollectedAmount == 0)
			{
				TextObject textObject = new TextObject("{=VktwHCN6}Your men have started to collect the tax of {VILLAGE}", null);
				textObject.SetTextVariable("VILLAGE", Settlement.CurrentSettlement.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
			RevenueFarmingIssueBehavior.Instance.CollectingRevenues = true;
			args.MenuContext.GameMenu.StartWait();
			RevenueFarmingIssueBehavior.UpdateCollectionMenuProgress(args);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000DCEED File Offset: 0x000DB0ED
		private void collection_menu_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			RevenueFarmingIssueBehavior.UpdateCollectionMenuProgress(args);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000DCEF8 File Offset: 0x000DB0F8
		private static void UpdateCollectionMenuProgress(MenuCallbackArgs args)
		{
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(revenueVillage.CollectProgress);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000DCF26 File Offset: 0x000DB126
		private void collect_revenue_menu_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_collect_revenue");
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000DCF3D File Offset: 0x000DB13D
		[GameMenuInitializationHandler("village_collect_revenue")]
		private static void village_collect_revenue_game_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000DCF64 File Offset: 0x000DB164
		[GameMenuInitializationHandler("offer_goods_and_troops")]
		[GameMenuInitializationHandler("brawl_breaks_out")]
		[GameMenuInitializationHandler("landlord_asks_for_money")]
		[GameMenuInitializationHandler("village_is_under_quarantine")]
		[GameMenuInitializationHandler("refuse_to_pay_what_they_owe")]
		[GameMenuInitializationHandler("relief_for_the_poorest")]
		private static void village_event_common_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000DCF80 File Offset: 0x000DB180
		private void ChangeRelationWithNotables(int relation)
		{
			foreach (Hero hero in Settlement.CurrentSettlement.Notables)
			{
				hero.SetHasMet();
				ChangeRelationAction.ApplyPlayerRelation(hero, relation, false, false);
			}
			TextObject textObject;
			if (relation > 0)
			{
				textObject = new TextObject("{=IwS1qeq9}Your relation is increased by {MAGNITUDE} with village notables.", null);
			}
			else
			{
				textObject = new TextObject("{=r5Netxy0}Your relation is decreased by {MAGNITUDE} with village notables.", null);
			}
			textObject.SetTextVariable("MAGNITUDE", relation);
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000DD018 File Offset: 0x000DB218
		private void CompleteCurrentRevenueCollection(bool addLog = true)
		{
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
			RevenueFarmingIssueBehavior.Instance.SetVillageAsCompleted(revenueVillage, addLog);
			if (RevenueFarmingIssueBehavior.Instance.IsTracked(revenueVillage.Village.Settlement))
			{
				RevenueFarmingIssueBehavior.Instance.RemoveTrackedObject(revenueVillage.Village.Settlement);
			}
			RevenueFarmingIssueBehavior.Instance.CollectingRevenues = false;
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000DD07C File Offset: 0x000DB27C
		private void GiveVillageGoods(out int amount)
		{
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
			amount = (int)((float)revenueVillage.TargetAmount * 0.5f / (float)revenueVillage.Village.VillageType.PrimaryProduction.Value);
			MobileParty.MainParty.ItemRoster.AddToCounts(revenueVillage.Village.VillageType.PrimaryProduction, amount);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000DD0E0 File Offset: 0x000DB2E0
		public void OnVillageEventWithIdSpawned(string Id)
		{
			RevenueFarmingIssueBehavior.VillageEvent villageEvent = this._villageEvents.FirstOrDefault<RevenueFarmingIssueBehavior.VillageEvent>((RevenueFarmingIssueBehavior.VillageEvent x) => x.Id == Id);
			RevenueFarmingIssueBehavior.Instance.AddLog(villageEvent.MainLog, false);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x000DD124 File Offset: 0x000DB324
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x000DD128 File Offset: 0x000DB328
		private bool ConditionsHold(Hero issueGiver, out Settlement targetSettlement)
		{
			targetSettlement = null;
			if (issueGiver.IsLord && issueGiver.Clan.Leader == issueGiver && issueGiver.GetTraitLevel(DefaultTraits.Mercy) <= 0 && issueGiver.Clan.Settlements.Count > 0)
			{
				targetSettlement = issueGiver.Clan.Settlements.Where<Settlement>((Settlement x) => x.IsTown).GetRandomElementInefficiently<Settlement>();
			}
			return targetSettlement != null;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000DD1AC File Offset: 0x000DB3AC
		public void OnCheckForIssue(Hero hero)
		{
			Settlement settlement;
			if (this.ConditionsHold(hero, out settlement))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssue), IssueBase.IssueFrequency.VeryCommon, settlement));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000DD214 File Offset: 0x000DB414
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new RevenueFarmingIssueBehavior.RevenueFarmingIssue(issueOwner, potentialIssueData.RelatedObject as Settlement);
		}

		// Token: 0x04000F25 RID: 3877
		private const int CollectAllVillageTaxesAfterHours = 10;

		// Token: 0x04000F26 RID: 3878
		private const IssueBase.IssueFrequency RevenueFarmingIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000F27 RID: 3879
		private List<RevenueFarmingIssueBehavior.VillageEvent> _villageEvents;

		// Token: 0x04000F28 RID: 3880
		private RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest _cachedQuest;

		// Token: 0x0200077F RID: 1919
		public class RevenueFarmingIssue : IssueBase
		{
			// Token: 0x06006210 RID: 25104 RVA: 0x001C74E3 File Offset: 0x001C56E3
			internal static void AutoGeneratedStaticCollectObjectsRevenueFarmingIssue(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.RevenueFarmingIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006211 RID: 25105 RVA: 0x001C74F1 File Offset: 0x001C56F1
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetSettlement);
			}

			// Token: 0x06006212 RID: 25106 RVA: 0x001C7506 File Offset: 0x001C5706
			internal static object AutoGeneratedGetMemberValue_targetSettlement(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssue)o)._targetSettlement;
			}

			// Token: 0x17001377 RID: 4983
			// (get) Token: 0x06006213 RID: 25107 RVA: 0x001C7513 File Offset: 0x001C5713
			protected override int RewardGold
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17001378 RID: 4984
			// (get) Token: 0x06006214 RID: 25108 RVA: 0x001C7518 File Offset: 0x001C5718
			protected int TotalRequestedDenars
			{
				get
				{
					int num = 0;
					foreach (Village village in this._targetSettlement.BoundVillages)
					{
						if (!village.Settlement.IsRaided && !village.Settlement.IsUnderRaid)
						{
							num += (int)(village.Hearth * 4f);
						}
					}
					return num / 3;
				}
			}

			// Token: 0x17001379 RID: 4985
			// (get) Token: 0x06006215 RID: 25109 RVA: 0x001C7598 File Offset: 0x001C5798
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=j5fS9zaa}Yes, there is something. I have been on campaign for much of this year, and I have not been able to go around to my estates collecting the rents that are owed to me and the taxes that are owed to the realm. I need some help collecting these revenues.[ib:confident3][if:convo_nonchalant]", null);
				}
			}

			// Token: 0x1700137A RID: 4986
			// (get) Token: 0x06006216 RID: 25110 RVA: 0x001C75A5 File Offset: 0x001C57A5
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=AXy26AFb}Maybe I can help. What are the terms of agreement.", null);
				}
			}

			// Token: 0x1700137B RID: 4987
			// (get) Token: 0x06006217 RID: 25111 RVA: 0x001C75B2 File Offset: 0x001C57B2
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=F540oIed}I can designate you as my official revenue farmer, and give you a list of everyone's holdings and how much they owe. All you need to do is visit all my villages and collect what you can. I don't expect you to be able to get every denar. Some of the people around here are genuinely hard - up, but they'll all try to get out of paying. Let's just keep it simple: I will take {TOTAL_REQUESTED_DENARS}{GOLD_ICON} denars and you can keep whatever else you can squeeze out of them. Are you interested?[if:convo_calm_friendly]", null);
					textObject.SetTextVariable("TOTAL_REQUESTED_DENARS", this.TotalRequestedDenars);
					return textObject;
				}
			}

			// Token: 0x1700137C RID: 4988
			// (get) Token: 0x06006218 RID: 25112 RVA: 0x001C75D1 File Offset: 0x001C57D1
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=dAmK7rKG}All right. I will visit your villages and collect your rent.", null);
				}
			}

			// Token: 0x1700137D RID: 4989
			// (get) Token: 0x06006219 RID: 25113 RVA: 0x001C75DE File Offset: 0x001C57DE
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700137E RID: 4990
			// (get) Token: 0x0600621A RID: 25114 RVA: 0x001C75E1 File Offset: 0x001C57E1
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700137F RID: 4991
			// (get) Token: 0x0600621B RID: 25115 RVA: 0x001C75E4 File Offset: 0x001C57E4
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=zqrn2beP}Revenue Farming", null);
				}
			}

			// Token: 0x17001380 RID: 4992
			// (get) Token: 0x0600621C RID: 25116 RVA: 0x001C75F1 File Offset: 0x001C57F1
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=U8izV2lM}A {?ISSUE_GIVER.GENDER}lady{?}lord{\\?} is looking for someone to collect back rents that {?ISSUE_GIVER.GENDER}she{?}he{\\?} says are owed to {?ISSUE_GIVER.GENDER}her{?}him{\\?}.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, null, false);
					return textObject;
				}
			}

			// Token: 0x0600621D RID: 25117 RVA: 0x001C7616 File Offset: 0x001C5816
			public RevenueFarmingIssue(Hero issueOwner, Settlement targetSettlement)
				: base(issueOwner, CampaignTime.DaysFromNow(20f))
			{
				this._targetSettlement = targetSettlement;
			}

			// Token: 0x0600621E RID: 25118 RVA: 0x001C7630 File Offset: 0x001C5830
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x0600621F RID: 25119 RVA: 0x001C7645 File Offset: 0x001C5845
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x06006220 RID: 25120 RVA: 0x001C7648 File Offset: 0x001C5848
			public override bool IssueStayAliveConditions()
			{
				if (this._targetSettlement.OwnerClan == base.IssueOwner.Clan)
				{
					return this._targetSettlement.BoundVillages.Any<Village>((Village x) => !x.Settlement.IsRaided && !x.Settlement.IsUnderRaid);
				}
				return false;
			}

			// Token: 0x06006221 RID: 25121 RVA: 0x001C76A0 File Offset: 0x001C58A0
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				flags = IssueBase.PreconditionFlags.None;
				relationHero = null;
				requiredGold = 0;
				skill = null;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (FactionManager.IsAtWarAgainstFaction(issueGiver.MapFaction, Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 40)
				{
					flags |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (issueGiver.MapFaction.Leader == Hero.MainHero)
				{
					flags |= IssueBase.PreconditionFlags.MainHeroIsKingdomLeader;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06006222 RID: 25122 RVA: 0x001C772D File Offset: 0x001C592D
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06006223 RID: 25123 RVA: 0x001C772F File Offset: 0x001C592F
			protected override void HourlyTick()
			{
			}

			// Token: 0x06006224 RID: 25124 RVA: 0x001C7734 File Offset: 0x001C5934
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				List<RevenueFarmingIssueBehavior.RevenueVillage> list = new List<RevenueFarmingIssueBehavior.RevenueVillage>();
				foreach (Village village in this._targetSettlement.BoundVillages)
				{
					if (!village.Settlement.IsUnderRaid && !village.Settlement.IsRaided)
					{
						list.Add(new RevenueFarmingIssueBehavior.RevenueVillage(village, (int)(village.Hearth * 4f)));
					}
				}
				return new RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(20f), list);
			}

			// Token: 0x06006225 RID: 25125 RVA: 0x001C77D8 File Offset: 0x001C59D8
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001F3B RID: 7995
			private const int IssueAndQuestDuration = 20;

			// Token: 0x04001F3C RID: 7996
			private const int MinimumRequiredMenCount = 40;

			// Token: 0x04001F3D RID: 7997
			[SaveableField(1)]
			private Settlement _targetSettlement;
		}

		// Token: 0x02000780 RID: 1920
		public class RevenueFarmingIssueQuest : QuestBase
		{
			// Token: 0x06006226 RID: 25126 RVA: 0x001C77DA File Offset: 0x001C59DA
			internal static void AutoGeneratedStaticCollectObjectsRevenueFarmingIssueQuest(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006227 RID: 25127 RVA: 0x001C77E8 File Offset: 0x001C59E8
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._revenueVillages);
				collectedObjects.Add(this._currentVillageEvents);
				collectedObjects.Add(this._questProgressLog);
			}

			// Token: 0x06006228 RID: 25128 RVA: 0x001C7815 File Offset: 0x001C5A15
			internal static object AutoGeneratedGetMemberValue_totalRequestedDenars(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._totalRequestedDenars;
			}

			// Token: 0x06006229 RID: 25129 RVA: 0x001C7827 File Offset: 0x001C5A27
			internal static object AutoGeneratedGetMemberValueCollectingRevenues(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o).CollectingRevenues;
			}

			// Token: 0x0600622A RID: 25130 RVA: 0x001C7839 File Offset: 0x001C5A39
			internal static object AutoGeneratedGetMemberValue_allRevenuesAreCollected(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._allRevenuesAreCollected;
			}

			// Token: 0x0600622B RID: 25131 RVA: 0x001C784B File Offset: 0x001C5A4B
			internal static object AutoGeneratedGetMemberValue_revenueVillages(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._revenueVillages;
			}

			// Token: 0x0600622C RID: 25132 RVA: 0x001C7858 File Offset: 0x001C5A58
			internal static object AutoGeneratedGetMemberValue_currentVillageEvents(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._currentVillageEvents;
			}

			// Token: 0x0600622D RID: 25133 RVA: 0x001C7865 File Offset: 0x001C5A65
			internal static object AutoGeneratedGetMemberValue_questProgressLog(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._questProgressLog;
			}

			// Token: 0x17001381 RID: 4993
			// (get) Token: 0x0600622E RID: 25134 RVA: 0x001C7872 File Offset: 0x001C5A72
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=zqrn2beP}Revenue Farming", null);
				}
			}

			// Token: 0x17001382 RID: 4994
			// (get) Token: 0x0600622F RID: 25135 RVA: 0x001C787F File Offset: 0x001C5A7F
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001383 RID: 4995
			// (get) Token: 0x06006230 RID: 25136 RVA: 0x001C7882 File Offset: 0x001C5A82
			public List<RevenueFarmingIssueBehavior.RevenueVillage> RevenueVillages
			{
				get
				{
					return this._revenueVillages;
				}
			}

			// Token: 0x17001384 RID: 4996
			// (get) Token: 0x06006231 RID: 25137 RVA: 0x001C788A File Offset: 0x001C5A8A
			// (set) Token: 0x06006232 RID: 25138 RVA: 0x001C7892 File Offset: 0x001C5A92
			public Settlement TargetSettlement { get; private set; }

			// Token: 0x17001385 RID: 4997
			// (get) Token: 0x06006233 RID: 25139 RVA: 0x001C789C File Offset: 0x001C5A9C
			private TextObject QuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=b0WQfzNb}{QUEST_GIVER.LINK} the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} of {TARGET_SETTLEMENT} told you that {?QUEST_GIVER.GENDER}she{?}he{\\?} wanted to grant revenue collection rights to a commander of good reputation who has enough men to suppress any resistance. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to visit all the villages that are bound to {TARGET_SETTLEMENT} and collect taxes and rents. You have agreed to collect the revenues after paying {QUEST_GIVER.LINK}'s share, {REQUESTED_DENARS}{GOLD_ICON} denars.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this.TargetSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("REQUESTED_DENARS", this._totalRequestedDenars);
					return textObject;
				}
			}

			// Token: 0x17001386 RID: 4998
			// (get) Token: 0x06006234 RID: 25140 RVA: 0x001C78F8 File Offset: 0x001C5AF8
			private TextObject QuestCanceledWarDeclaredLog
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001387 RID: 4999
			// (get) Token: 0x06006235 RID: 25141 RVA: 0x001C792C File Offset: 0x001C5B2C
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001388 RID: 5000
			// (get) Token: 0x06006236 RID: 25142 RVA: 0x001C7960 File Offset: 0x001C5B60
			private TextObject QuestSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=CEQhyvzj}You have completed the collection of revenues and paid {QUEST_GIVER.LINK} a fix sum in advance, as agreed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001389 RID: 5001
			// (get) Token: 0x06006237 RID: 25143 RVA: 0x001C7994 File Offset: 0x001C5B94
			private TextObject QuestBetrayedLog
			{
				get
				{
					TextObject textObject = new TextObject("{=5ky3voFY}You have rejected handing over the revenue to the {QUEST_GIVER.LINK}. The {?QUEST_GIVER.GENDER}lady{?}lord{\\?} is deeply disappointed in you.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700138A RID: 5002
			// (get) Token: 0x06006238 RID: 25144 RVA: 0x001C79C8 File Offset: 0x001C5BC8
			private TextObject QuestFailedWithTimeOutLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=RNdrvZJQ}You have failed to bring the revenues to the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} in time.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700138B RID: 5003
			// (get) Token: 0x06006239 RID: 25145 RVA: 0x001C79FC File Offset: 0x001C5BFC
			private TextObject AllRevenuesAreCollectedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=ywlzjQfN}{QUEST_GIVER.LINK} wants {TOTAL_REQUESTED_DENARS}{GOLD_ICON} that you have collected from {?QUEST_GIVER.GENDER}her{?}his{\\?} fiefs. You can either give the denars to the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} yourself, or hand them over to a steward of the {?QUEST_GIVER.GENDER}lady{?}lord{\\?}, which can be found in the castles and towns that belong to the {?QUEST_GIVER.GENDER}lady{?}lord{\\?}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TOTAL_REQUESTED_DENARS", this._totalRequestedDenars);
					return textObject;
				}
			}

			// Token: 0x0600623A RID: 25146 RVA: 0x001C7A40 File Offset: 0x001C5C40
			public RevenueFarmingIssueQuest(string questId, Hero giverHero, CampaignTime duration, List<RevenueFarmingIssueBehavior.RevenueVillage> revenueVillages)
				: base(questId, giverHero, duration, 0)
			{
				this._revenueVillages = revenueVillages;
				this.TargetSettlement = this._revenueVillages[0].Village.Bound;
				foreach (RevenueFarmingIssueBehavior.VillageEvent villageEvent in Campaign.Current.GetCampaignBehavior<RevenueFarmingIssueBehavior>()._villageEvents)
				{
					this._currentVillageEvents.Add(villageEvent.Id, false);
				}
				foreach (RevenueFarmingIssueBehavior.RevenueVillage revenueVillage in revenueVillages)
				{
					this._totalRequestedDenars += revenueVillage.TargetAmount / 3;
				}
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600623B RID: 25147 RVA: 0x001C7B38 File Offset: 0x001C5D38
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this._questProgressLog = base.AddDiscreteLog(this.QuestStartedLogText, new TextObject("{=bC5aMfG0}Villages", null), 0, this._revenueVillages.Count, null, true);
				foreach (RevenueFarmingIssueBehavior.RevenueVillage revenueVillage in this._revenueVillages)
				{
					base.AddTrackedObject(revenueVillage.Village.Settlement);
				}
			}

			// Token: 0x0600623C RID: 25148 RVA: 0x001C7BC8 File Offset: 0x001C5DC8
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=PXigJyMs}Excellent. You are acting in my name now. Try to be polite but you have every right to use force if they don't cough up what's owed. Good luck.[ib:confident2][if:convo_bored2]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=tthBNejU}Have you collected the revenues?[if:convo_undecided_open]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=wErSpkjy}I'm still working on it.", null), null, null, null)
					.NpcLine(new TextObject("{=BI1UnHaB}Good, good. This takes time, I know, but don't keep me waiting too long.[if:convo_mocking_aristocratic]", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(MapEventHelper.OnConversationEnd))
					.CloseDialog()
					.PlayerOption(new TextObject("{=ORl6qiOj}Yes, here is your share.", null), null, null, null)
					.Condition(() => this._allRevenuesAreCollected)
					.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.TurnQuestInClickableCondition))
					.NpcLine(new TextObject("{=MKYzHFKB}Thank you for your help.[if:convo_delighted]", null), null, null, null, null)
					.Consequence(delegate
					{
						this.QuestCompletedWithSuccess();
						MapEventHelper.OnConversationEnd();
					})
					.CloseDialog()
					.PlayerOption(new TextObject("{=kj3WQY1V}Maybe I should keep this to myself.", null), null, null, null)
					.Condition(() => this._allRevenuesAreCollected)
					.NpcLine(new TextObject("{=82aiVoV9}You will regret this in the long run...[ib:closed2][if:convo_angry]", null), null, null, null, null)
					.Consequence(delegate
					{
						this.QuestCompletedWithBetray();
						MapEventHelper.OnConversationEnd();
					})
					.CloseDialog()
					.PlayerOption(new TextObject("{=G5tyQj6N}Not yet.", null), null, null, null)
					.NpcLine(new TextObject("{=UXCjNTjF}Hurry up. I don't have that much time.[if:convo_annoyed]", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(MapEventHelper.OnConversationEnd))
					.CloseDialog()
					.EndPlayerOptions();
			}

			// Token: 0x0600623D RID: 25149 RVA: 0x001C7D8C File Offset: 0x001C5F8C
			private bool TurnQuestInClickableCondition(out TextObject explanation)
			{
				if (Hero.MainHero.Gold < RevenueFarmingIssueBehavior.Instance._totalRequestedDenars)
				{
					explanation = new TextObject("{=QOWyEJrm}You don't have enough denars.", null);
					return false;
				}
				explanation = null;
				return true;
			}

			// Token: 0x0600623E RID: 25150 RVA: 0x001C7DB8 File Offset: 0x001C5FB8
			protected override void OnBeforeTimedOut(ref bool completeWithSuccess, ref bool doNotResolveTheQuest)
			{
				this.RelationshipChangeWithQuestGiver = -5;
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
				if (Hero.MainHero.Gold >= this._totalRequestedDenars)
				{
					this.ShowQuestResolvePopUp();
					doNotResolveTheQuest = true;
				}
			}

			// Token: 0x0600623F RID: 25151 RVA: 0x001C7E08 File Offset: 0x001C6008
			protected override void OnTimedOut()
			{
				base.AddLog(this.QuestFailedWithTimeOutLogText, false);
			}

			// Token: 0x06006240 RID: 25152 RVA: 0x001C7E18 File Offset: 0x001C6018
			protected override void RegisterEvents()
			{
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaid));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
				CampaignEvents.PlayerDesertedBattleEvent.AddNonSerializedListener(this, new Action<int>(this.OnPlayerDeserted));
			}

			// Token: 0x06006241 RID: 25153 RVA: 0x001C7EC6 File Offset: 0x001C60C6
			private void OnPlayerDeserted(int count)
			{
				RevenueFarmingIssueBehavior.Instance.CollectingRevenues = false;
			}

			// Token: 0x06006242 RID: 25154 RVA: 0x001C7ED3 File Offset: 0x001C60D3
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if (mapEvent.IsPlayerMapEvent && mapEvent.HasWinner && mapEvent.IsFieldBattle)
				{
					RevenueFarmingIssueBehavior.Instance.CollectingRevenues = false;
				}
			}

			// Token: 0x06006243 RID: 25155 RVA: 0x001C7EF8 File Offset: 0x001C60F8
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == this.TargetSettlement && oldOwner == base.QuestGiver)
				{
					TextObject textObject = new TextObject("{=1m68Nsze}{QUEST_GIVER.LINK} has lost {SETTLEMENT} and your agreement with {?QUEST_GIVER.GENDER}her{?}him{\\?} has been canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this.TargetSettlement.EncyclopediaLinkWithName);
					base.CompleteQuestWithCancel(textObject);
				}
			}

			// Token: 0x06006244 RID: 25156 RVA: 0x001C7F5A File Offset: 0x001C615A
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06006245 RID: 25157 RVA: 0x001C7F70 File Offset: 0x001C6170
			private void OnVillageRaid(Village village)
			{
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = this._revenueVillages.FirstOrDefault<RevenueFarmingIssueBehavior.RevenueVillage>((RevenueFarmingIssueBehavior.RevenueVillage x) => x.Village.Id == village.Id);
				if (revenueVillage != null && !revenueVillage.IsRaided)
				{
					TextObject textObject = new TextObject("{=k8U0928J}{VILLAGE} has been raided. {QUEST_GIVER.LINK} asks you to exempt them, but still wants you to collect {AMOUNT}{GOLD_ICON} denars from rest of {?QUEST_GIVER.GENDER}her{?}his{\\?} villages.", null);
					textObject.SetTextVariable("VILLAGE", village.Settlement.EncyclopediaLinkWithName);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					this._totalRequestedDenars -= revenueVillage.TargetAmount / 3;
					textObject.SetTextVariable("AMOUNT", this._totalRequestedDenars);
					revenueVillage.SetDone();
					revenueVillage.IsRaided = true;
					base.AddLog(textObject, false);
					this._questProgressLog.UpdateCurrentProgress(this._questProgressLog.CurrentProgress + 1);
					if (this.CollectingRevenues)
					{
						this.CollectingRevenues = false;
					}
					if (this._revenueVillages.All<RevenueFarmingIssueBehavior.RevenueVillage>((RevenueFarmingIssueBehavior.RevenueVillage x) => x.IsRaided))
					{
						TextObject textObject2 = new TextObject("{=44f1ff0q}All the villages of {QUEST_GIVER.LINK} has been raided and your agreement with {?QUEST_GIVER.GENDER}her{?}him{\\?} has been canceled.", null);
						base.CompleteQuestWithCancel(textObject2);
					}
				}
			}

			// Token: 0x06006246 RID: 25158 RVA: 0x001C8098 File Offset: 0x001C6298
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					if (!this._allRevenuesAreCollected)
					{
						if (this._revenueVillages.All<RevenueFarmingIssueBehavior.RevenueVillage>((RevenueFarmingIssueBehavior.RevenueVillage x) => x.GetIsCompleted()))
						{
							this.OnAllRevenuesAreCollected();
						}
					}
					if (this.CollectingRevenues)
					{
						this.ProgressRevenueCollectionForVillage();
					}
				}
			}

			// Token: 0x06006247 RID: 25159 RVA: 0x001C80F5 File Offset: 0x001C62F5
			private void OnAllRevenuesAreCollected()
			{
				this._allRevenuesAreCollected = true;
				base.AddLog(this.AllRevenuesAreCollectedLogText, false);
			}

			// Token: 0x06006248 RID: 25160 RVA: 0x001C810C File Offset: 0x001C630C
			public void RevenuesAreDeliveredToSteward()
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=RCa0DpAo}You have handed over the revenue to the steward", null), 0, null, null, "");
				this.QuestCompletedWithSuccess();
			}

			// Token: 0x06006249 RID: 25161 RVA: 0x001C812C File Offset: 0x001C632C
			private void ShowQuestResolvePopUp()
			{
				TextObject textObject = new TextObject("{=I9GYdYZx}{?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.NAME} wants {TOTAL_REQUESTED_DENARS}{GOLD_ICON} denars that you have collected from {?QUEST_GIVER.GENDER}her{?}his{\\?} fiefs. {?QUEST_GIVER.GENDER}She{?}He{\\?} has sent {?QUEST_GIVER.GENDER}her{?}his{\\?} steward to you to collect it. If you refuse this will be counted as a crime and {?QUEST_GIVER.GENDER}her{?}his{\\?} faction may declare war on you.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				textObject.SetTextVariable("TOTAL_REQUESTED_DENARS", this._totalRequestedDenars);
				InformationManager.ShowInquiry(new InquiryData(this.Title.ToString(), textObject.ToString(), true, true, new TextObject("{=plZVwdlL}Send the revenue", null).ToString(), new TextObject("{=asa9HaIQ}Keep the revenue", null).ToString(), new Action(this.QuestCompletedWithSuccess), new Action(this.QuestCompletedWithBetray), "", 0f, null, null, null), false, false);
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				if (this.CollectingRevenues)
				{
					this.CollectingRevenues = false;
				}
			}

			// Token: 0x0600624A RID: 25162 RVA: 0x001C81F0 File Offset: 0x001C63F0
			private void QuestCompletedWithSuccess()
			{
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, this._totalRequestedDenars, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 30)
				});
				this.RelationshipChangeWithQuestGiver = 5;
				base.AddLog(this.QuestSuccessLog, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600624B RID: 25163 RVA: 0x001C824C File Offset: 0x001C644C
			private void QuestCompletedWithBetray()
			{
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				this.RelationshipChangeWithQuestGiver = -15;
				base.AddLog(this.QuestBetrayedLog, false);
				base.CompleteQuestWithBetrayal(null);
				ChangeCrimeRatingAction.Apply(base.QuestGiver.MapFaction, 45f, true);
			}

			// Token: 0x0600624C RID: 25164 RVA: 0x001C82AC File Offset: 0x001C64AC
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclaredLog);
				}
			}

			// Token: 0x0600624D RID: 25165 RVA: 0x001C82D6 File Offset: 0x001C64D6
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclaredLog, false);
			}

			// Token: 0x0600624E RID: 25166 RVA: 0x001C82F0 File Offset: 0x001C64F0
			protected override void OnFinalize()
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (this._currentVillageEvents.Any<KeyValuePair<string, bool>>((KeyValuePair<string, bool> x) => x.Key == Campaign.Current.CurrentMenuContext.GameMenu.StringId) || Campaign.Current.CurrentMenuContext.GameMenu.StringId == "village_collect_revenue")
					{
						if (Game.Current.GameStateManager.ActiveState is MapState)
						{
							PlayerEncounter.Finish(true);
							return;
						}
						GameMenu.SwitchToMenu("village_outside");
					}
				}
			}

			// Token: 0x0600624F RID: 25167 RVA: 0x001C837C File Offset: 0x001C657C
			protected override void InitializeQuestOnGameLoad()
			{
				this.TargetSettlement = this._revenueVillages[0].Village.Bound;
				this.SetDialogs();
			}

			// Token: 0x06006250 RID: 25168 RVA: 0x001C83A0 File Offset: 0x001C65A0
			public RevenueFarmingIssueBehavior.RevenueVillage FindCurrentRevenueVillage()
			{
				foreach (RevenueFarmingIssueBehavior.RevenueVillage revenueVillage in this._revenueVillages)
				{
					if (revenueVillage.Village.Id == Settlement.CurrentSettlement.Village.Id)
					{
						return revenueVillage;
					}
				}
				return null;
			}

			// Token: 0x06006251 RID: 25169 RVA: 0x001C8414 File Offset: 0x001C6614
			private void ProgressRevenueCollectionForVillage()
			{
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = this.FindCurrentRevenueVillage();
				if (!revenueVillage.EventOccurred && revenueVillage.CollectProgress >= 0.3f)
				{
					RevenueFarmingIssueBehavior behavior = Campaign.Current.GetCampaignBehavior<RevenueFarmingIssueBehavior>();
					KeyValuePair<string, bool> randomElementInefficiently = this._currentVillageEvents.Where<KeyValuePair<string, bool>>((KeyValuePair<string, bool> x) => !x.Value && behavior._villageEvents.Any<RevenueFarmingIssueBehavior.VillageEvent>((RevenueFarmingIssueBehavior.VillageEvent y) => y.Id == x.Key)).GetRandomElementInefficiently<KeyValuePair<string, bool>>();
					this._currentVillageEvents[randomElementInefficiently.Key] = true;
					behavior.OnVillageEventWithIdSpawned(randomElementInefficiently.Key);
					revenueVillage.EventOccurred = true;
					GameMenu.SwitchToMenu(randomElementInefficiently.Key);
					Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
					return;
				}
				revenueVillage.CollectedAmount += revenueVillage.HourlyGain;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, revenueVillage.HourlyGain, false);
				if (revenueVillage.GetIsCompleted())
				{
					this.SetVillageAsCompleted(revenueVillage, true);
				}
			}

			// Token: 0x06006252 RID: 25170 RVA: 0x001C84EC File Offset: 0x001C66EC
			public void SetVillageAsCompleted(RevenueFarmingIssueBehavior.RevenueVillage village, bool addLog = true)
			{
				this.CollectingRevenues = false;
				village.SetDone();
				base.RemoveTrackedObject(village.Village.Settlement);
				GameMenu.SwitchToMenu("village");
				this._questProgressLog.UpdateCurrentProgress(this._questProgressLog.CurrentProgress + 1);
				if (addLog)
				{
					TextObject textObject = new TextObject("{=mQqN8Fg0}Your men have collected {TOTAL_COLLECTED_FROM_VILLAGE}{GOLD_ICON} denars and completed the revenue collection from {VILLAGE}.", null);
					textObject.SetTextVariable("TOTAL_COLLECTED_FROM_VILLAGE", village.CollectedAmount);
					textObject.SetTextVariable("VILLAGE", village.Village.Settlement.EncyclopediaLinkWithName);
					base.AddLog(textObject, false);
				}
				if (!this._allRevenuesAreCollected)
				{
					if (this._revenueVillages.All<RevenueFarmingIssueBehavior.RevenueVillage>((RevenueFarmingIssueBehavior.RevenueVillage x) => x.GetIsCompleted()))
					{
						this.OnAllRevenuesAreCollected();
					}
				}
			}

			// Token: 0x04001F3E RID: 7998
			[SaveableField(10)]
			internal int _totalRequestedDenars;

			// Token: 0x04001F3F RID: 7999
			[SaveableField(20)]
			private readonly List<RevenueFarmingIssueBehavior.RevenueVillage> _revenueVillages;

			// Token: 0x04001F41 RID: 8001
			[SaveableField(30)]
			public bool CollectingRevenues;

			// Token: 0x04001F42 RID: 8002
			[SaveableField(40)]
			private readonly Dictionary<string, bool> _currentVillageEvents = new Dictionary<string, bool>();

			// Token: 0x04001F43 RID: 8003
			[SaveableField(50)]
			internal bool _allRevenuesAreCollected;

			// Token: 0x04001F44 RID: 8004
			[SaveableField(60)]
			private JournalLog _questProgressLog;
		}

		// Token: 0x02000781 RID: 1921
		public class VillageEvent
		{
			// Token: 0x06006259 RID: 25177 RVA: 0x001C8601 File Offset: 0x001C6801
			internal static void AutoGeneratedStaticCollectObjectsVillageEvent(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.VillageEvent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600625A RID: 25178 RVA: 0x001C860F File Offset: 0x001C680F
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x0600625B RID: 25179 RVA: 0x001C8611 File Offset: 0x001C6811
			public VillageEvent(string id, string mainEventText, TextObject mainLog, List<RevenueFarmingIssueBehavior.VillageEventOptionData> optionConditionsAndConsequences)
			{
				this.Id = id;
				this.MainEventText = mainEventText;
				this.MainLog = mainLog;
				this.OptionConditionsAndConsequences = optionConditionsAndConsequences;
			}

			// Token: 0x04001F45 RID: 8005
			public readonly string Id;

			// Token: 0x04001F46 RID: 8006
			public readonly string MainEventText;

			// Token: 0x04001F47 RID: 8007
			public TextObject MainLog;

			// Token: 0x04001F48 RID: 8008
			public List<RevenueFarmingIssueBehavior.VillageEventOptionData> OptionConditionsAndConsequences;
		}

		// Token: 0x02000782 RID: 1922
		public class RevenueVillage
		{
			// Token: 0x0600625C RID: 25180 RVA: 0x001C8636 File Offset: 0x001C6836
			internal static void AutoGeneratedStaticCollectObjectsRevenueVillage(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.RevenueVillage)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600625D RID: 25181 RVA: 0x001C8644 File Offset: 0x001C6844
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Village);
			}

			// Token: 0x0600625E RID: 25182 RVA: 0x001C8652 File Offset: 0x001C6852
			internal static object AutoGeneratedGetMemberValueVillage(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).Village;
			}

			// Token: 0x0600625F RID: 25183 RVA: 0x001C865F File Offset: 0x001C685F
			internal static object AutoGeneratedGetMemberValueTargetAmount(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).TargetAmount;
			}

			// Token: 0x06006260 RID: 25184 RVA: 0x001C8671 File Offset: 0x001C6871
			internal static object AutoGeneratedGetMemberValueCollectedAmount(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).CollectedAmount;
			}

			// Token: 0x06006261 RID: 25185 RVA: 0x001C8683 File Offset: 0x001C6883
			internal static object AutoGeneratedGetMemberValueHourlyGain(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).HourlyGain;
			}

			// Token: 0x06006262 RID: 25186 RVA: 0x001C8695 File Offset: 0x001C6895
			internal static object AutoGeneratedGetMemberValueEventOccurred(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).EventOccurred;
			}

			// Token: 0x06006263 RID: 25187 RVA: 0x001C86A7 File Offset: 0x001C68A7
			internal static object AutoGeneratedGetMemberValueIsRaided(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).IsRaided;
			}

			// Token: 0x06006264 RID: 25188 RVA: 0x001C86B9 File Offset: 0x001C68B9
			internal static object AutoGeneratedGetMemberValue_isDone(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o)._isDone;
			}

			// Token: 0x06006265 RID: 25189 RVA: 0x001C86CB File Offset: 0x001C68CB
			internal static object AutoGeneratedGetMemberValue_customProgress(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o)._customProgress;
			}

			// Token: 0x1700138C RID: 5004
			// (get) Token: 0x06006266 RID: 25190 RVA: 0x001C86DD File Offset: 0x001C68DD
			public float CollectProgress
			{
				get
				{
					return ((this.CollectedAmount == 0) ? 0f : ((float)this.CollectedAmount / (float)this.TargetAmount)) + this._customProgress;
				}
			}

			// Token: 0x06006267 RID: 25191 RVA: 0x001C8704 File Offset: 0x001C6904
			public void SetDone()
			{
				this._isDone = true;
			}

			// Token: 0x06006268 RID: 25192 RVA: 0x001C8710 File Offset: 0x001C6910
			public RevenueVillage(Village village, int targetAmount)
			{
				this.Village = village;
				this.TargetAmount = targetAmount;
				this.CollectedAmount = 0;
				this.HourlyGain = targetAmount / 10;
				this._isDone = false;
				this.EventOccurred = false;
				this.IsRaided = false;
				this._customProgress = 0f;
			}

			// Token: 0x06006269 RID: 25193 RVA: 0x001C8762 File Offset: 0x001C6962
			public void SetAdditionalProgress(float progress)
			{
				this._customProgress = progress;
			}

			// Token: 0x0600626A RID: 25194 RVA: 0x001C876B File Offset: 0x001C696B
			public bool GetIsCompleted()
			{
				return this._isDone || this.CollectProgress >= 1f || this.CollectedAmount >= this.TargetAmount;
			}

			// Token: 0x04001F49 RID: 8009
			[SaveableField(1)]
			public readonly Village Village;

			// Token: 0x04001F4A RID: 8010
			[SaveableField(2)]
			public readonly int TargetAmount;

			// Token: 0x04001F4B RID: 8011
			[SaveableField(3)]
			public int CollectedAmount;

			// Token: 0x04001F4C RID: 8012
			[SaveableField(4)]
			public int HourlyGain;

			// Token: 0x04001F4D RID: 8013
			[SaveableField(5)]
			private bool _isDone;

			// Token: 0x04001F4E RID: 8014
			[SaveableField(6)]
			public bool EventOccurred;

			// Token: 0x04001F4F RID: 8015
			[SaveableField(7)]
			public bool IsRaided;

			// Token: 0x04001F50 RID: 8016
			[SaveableField(8)]
			private float _customProgress;
		}

		// Token: 0x02000783 RID: 1923
		public class RevenueFarmingIssueBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600626B RID: 25195 RVA: 0x001C8795 File Offset: 0x001C6995
			public RevenueFarmingIssueBehaviorTypeDefiner()
				: base(850000)
			{
			}

			// Token: 0x0600626C RID: 25196 RVA: 0x001C87A4 File Offset: 0x001C69A4
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssue), 1, null);
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest), 2, null);
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.VillageEvent), 3, null);
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.RevenueVillage), 4, null);
			}

			// Token: 0x0600626D RID: 25197 RVA: 0x001C87F9 File Offset: 0x001C69F9
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<RevenueFarmingIssueBehavior.RevenueVillage>));
				base.ConstructContainerDefinition(typeof(List<RevenueFarmingIssueBehavior.VillageEvent>));
				base.ConstructContainerDefinition(typeof(Dictionary<string, bool>));
			}
		}

		// Token: 0x02000784 RID: 1924
		public struct VillageEventOptionData
		{
			// Token: 0x0600626E RID: 25198 RVA: 0x001C882B File Offset: 0x001C6A2B
			public VillageEventOptionData(string text, GameMenuOption.OnConditionDelegate onCondition, GameMenuOption.OnConsequenceDelegate onConsequence, bool isLeave = false)
			{
				this.Text = text;
				this.OnCondition = onCondition;
				this.OnConsequence = onConsequence;
				this.IsLeave = isLeave;
			}

			// Token: 0x04001F51 RID: 8017
			public readonly string Text;

			// Token: 0x04001F52 RID: 8018
			public readonly GameMenuOption.OnConditionDelegate OnCondition;

			// Token: 0x04001F53 RID: 8019
			public readonly GameMenuOption.OnConsequenceDelegate OnConsequence;

			// Token: 0x04001F54 RID: 8020
			public readonly bool IsLeave;
		}
	}
}
