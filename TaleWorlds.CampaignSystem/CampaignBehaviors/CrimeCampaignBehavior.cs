using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000401 RID: 1025
	public class CrimeCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004040 RID: 16448 RVA: 0x00119990 File Offset: 0x00117B90
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterGameCreated));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroDeath));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x00119A10 File Offset: 0x00117C10
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x00119A14 File Offset: 0x00117C14
		private void OnDailyTick()
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				float dailyCrimeRatingChange = clan.DailyCrimeRatingChange;
				if (!clan.IsEliminated && !dailyCrimeRatingChange.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					ChangeCrimeRatingAction.Apply(clan, dailyCrimeRatingChange, false);
				}
			}
			foreach (Kingdom kingdom in Kingdom.All)
			{
				float dailyCrimeRatingChange2 = kingdom.DailyCrimeRatingChange;
				if (!kingdom.IsEliminated && !dailyCrimeRatingChange2.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					ChangeCrimeRatingAction.Apply(kingdom, dailyCrimeRatingChange2, false);
				}
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x00119AEC File Offset: 0x00117CEC
		private void OnAfterGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x00119AF8 File Offset: 0x00117CF8
		private void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenu("town_inside_criminal", "{=XgA2JgVR}You are brought to the town square to face judgment.", new OnInitDelegate(CrimeCampaignBehavior.town_inside_criminal_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_punishment", "{=8iDpmu0L}Accept corporal punishment", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_give_punishment_and_money", "{=Xi1wpR2L}Accept corporal punishment and pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_give_your_life", "{=bVi0JKSx}You will be executed. You must face it as bravely as you can", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_influence", "{=1cMS6415}Pay {FINE}{INFLUENCE_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_money", "{=870ZCp1J}Pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_ignore_charges", "{=UQhRKJb9}Ignore the charges", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_ignore_charges_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_ignore_charges_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_discuss_criminal_surrender", "{=lwVwe4qU}You are discussing the terms of your surrender.", new OnInitDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_punishment", "{=8iDpmu0L}Accept corporal punishment", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_give_punishment_and_money", "{=Xi1wpR2L}Accept corporal punishment and pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_give_your_life", "{=VSzwMDJ2}You will be put to death", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_influence", "{=1cMS6415}Pay {FINE}{INFLUENCE_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_money", "{=870ZCp1J}Pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_back_on_consequence), true, -1, false, null);
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x00119D9C File Offset: 0x00117F9C
		private void OnHeroDeath(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim == Hero.MainHero)
			{
				foreach (Clan clan in Clan.NonBanditFactions)
				{
					if (!clan.IsEliminated)
					{
						ChangeCrimeRatingAction.Apply(clan, -clan.MainHeroCrimeRating, true);
					}
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (!kingdom.IsEliminated)
					{
						ChangeCrimeRatingAction.Apply(kingdom, -kingdom.MainHeroCrimeRating, true);
					}
				}
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x00119E54 File Offset: 0x00118054
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if (side1Faction == Hero.MainHero.MapFaction || side2Faction == Hero.MainHero.MapFaction)
			{
				IFaction faction = ((side1Faction == Hero.MainHero.MapFaction) ? side2Faction : side1Faction);
				float num = Campaign.Current.Models.CrimeModel.DeclareWarCrimeRatingThreshold * 0.5f;
				if (faction.MainHeroCrimeRating > num)
				{
					ChangeCrimeRatingAction.Apply(faction, num - faction.MainHeroCrimeRating, true);
				}
			}
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x00119EC0 File Offset: 0x001180C0
		private static bool CanPayCriminalRatingValueWith(IFaction faction, CrimeModel.PaymentMethod paymentMethod)
		{
			if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(Settlement.CurrentSettlement.MapFaction))
			{
				if (paymentMethod == CrimeModel.PaymentMethod.Gold)
				{
					return true;
				}
				if (CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(faction))
				{
					if (paymentMethod == CrimeModel.PaymentMethod.Influence)
					{
						return true;
					}
				}
				else if (paymentMethod == CrimeModel.PaymentMethod.Punishment)
				{
					return true;
				}
			}
			else if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(Settlement.CurrentSettlement.MapFaction))
			{
				if (CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(faction))
				{
					if (paymentMethod == CrimeModel.PaymentMethod.Gold)
					{
						return true;
					}
					if (paymentMethod == CrimeModel.PaymentMethod.Influence)
					{
						return true;
					}
				}
				else
				{
					if (paymentMethod.HasAnyFlag(CrimeModel.PaymentMethod.Execution))
					{
						return Hero.MainHero.Gold < (int)PayForCrimeAction.GetClearCrimeCost(faction, CrimeModel.PaymentMethod.Gold);
					}
					if (paymentMethod.HasAllFlags(CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x00119F68 File Offset: 0x00118168
		private static bool IsCriminalPlayerInSameKingdomOf(IFaction faction)
		{
			Clan clan = faction as Clan;
			return Hero.MainHero.Clan == faction || Hero.MainHero.Clan.Kingdom == faction || (clan != null && Hero.MainHero.Clan.Kingdom == clan.Kingdom);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00119FB9 File Offset: 0x001181B9
		[GameMenuInitializationHandler("town_discuss_criminal_surrender")]
		[GameMenuInitializationHandler("town_inside_criminal")]
		public static void game_menu_town_criminal_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Town.WaitMeshName);
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x00119FD5 File Offset: 0x001181D5
		public static void town_inside_criminal_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x00119FE8 File Offset: 0x001181E8
		public static void town_discuss_criminal_surrender_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x00119FEA File Offset: 0x001181EA
		public static bool criminal_inside_menu_pay_by_punishment_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			args.Tooltip = new TextObject("{=yGvsbUWc}Beware that you may die from punishment!", null);
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Punishment);
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0011A018 File Offset: 0x00118218
		public static void criminal_inside_menu_pay_by_punishment_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Punishment);
			if (Hero.MainHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (Settlement.CurrentSettlement.IsCastle)
					{
						GameMenu.SwitchToMenu("castle_outside");
						return;
					}
					GameMenu.SwitchToMenu("town_outside");
					return;
				}
				else
				{
					PlayerEncounter.Finish(true);
				}
			}
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0011A078 File Offset: 0x00118278
		public static bool criminal_inside_menu_give_money_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			int num = (int)PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			args.Text.SetTextVariable("FINE", num);
			args.Text.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			if (Hero.MainHero.Gold < num)
			{
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0011A0FC File Offset: 0x001182FC
		public static void criminal_inside_menu_give_money_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0011A130 File Offset: 0x00118330
		public static bool criminal_inside_menu_give_influence_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Bribe;
			float clearCrimeCost = PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
			args.Text.SetTextVariable("FINE", clearCrimeCost.ToString("F1"));
			args.Text.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			if (Clan.PlayerClan.Influence < clearCrimeCost)
			{
				args.Tooltip = new TextObject("{=rMagXCrI}You don't have enough influence to get the charges dropped.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0011A1BD File Offset: 0x001183BD
		public static void criminal_inside_menu_give_influence_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x0011A1F0 File Offset: 0x001183F0
		public static bool criminal_inside_menu_give_punishment_and_money_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			int num = (int)PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			args.Text.SetTextVariable("FINE", num);
			args.Text.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			if (Hero.MainHero.Gold < num)
			{
				args.Tooltip = new TextObject("{=ETKyjOkJ}You don't have enough denars to pay the fine.", null);
				args.IsEnabled = false;
			}
			else
			{
				args.Tooltip = new TextObject("{=yGvsbUWc}Beware that you may die from punishment!", null);
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0011A288 File Offset: 0x00118488
		public static void criminal_inside_menu_give_punishment_and_money_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment);
			if (Hero.MainHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (Settlement.CurrentSettlement.IsCastle)
					{
						GameMenu.SwitchToMenu("castle_outside");
						return;
					}
					GameMenu.SwitchToMenu("town_outside");
					return;
				}
				else
				{
					PlayerEncounter.Finish(true);
				}
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0011A2E6 File Offset: 0x001184E6
		public static bool criminal_inside_menu_give_your_life_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Execution);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0011A300 File Offset: 0x00118500
		public static void criminal_inside_menu_give_your_life_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Execution);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0011A312 File Offset: 0x00118512
		public static bool criminal_inside_menu_ignore_charges_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0011A32B File Offset: 0x0011852B
		public static void criminal_inside_menu_ignore_charges_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0011A34E File Offset: 0x0011854E
		public static void town_discuss_criminal_surrender_back_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_guard");
				return;
			}
			GameMenu.SwitchToMenu("town_guard");
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x0011A371 File Offset: 0x00118571
		public static bool town_discuss_criminal_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}
	}
}
