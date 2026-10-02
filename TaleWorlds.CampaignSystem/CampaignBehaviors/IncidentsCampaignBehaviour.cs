using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000424 RID: 1060
	public class IncidentsCampaignBehaviour : CampaignBehaviorBase, INonReadyObjectHandler
	{
		// Token: 0x0600434C RID: 17228 RVA: 0x001337A4 File Offset: 0x001319A4
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.GameMenuOptionSelectedEvent.AddNonSerializedListener(this, new Action<GameMenu, GameMenuOption>(this.OnGameMenuOptionSelected));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.ConversationEnded.AddNonSerializedListener(this, new Action<IEnumerable<CharacterObject>>(this.ConversationEnded));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnHeirSelectionOverEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeirSelectionOver));
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x00133880 File Offset: 0x00131A80
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			Campaign.Current.IncidentManager.InitializeOnNewGame();
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x00133891 File Offset: 0x00131A91
		private void OnHeirSelectionOver(Hero obj)
		{
			Campaign.Current.IncidentManager.OnPlayerCharacterChange();
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x001338A2 File Offset: 0x00131AA2
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (mobileParty == MobileParty.MainParty && Campaign.Current.CurrentMenuContext == null)
			{
				this._canInvokeSettlementEvent = true;
			}
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x001338BF File Offset: 0x00131ABF
		private void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				this._canInvokeSettlementEvent = false;
			}
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x001338D0 File Offset: 0x00131AD0
		void INonReadyObjectHandler.OnBeforeNonReadyObjectsDeleted()
		{
			this.InitializeIncidents();
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x001338D8 File Offset: 0x00131AD8
		private void ConversationEnded(IEnumerable<CharacterObject> conversationCharacters)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.LeaveEncounter && MobileParty.MainParty.CurrentSettlement == null && !MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.RandomFloatWithSeed((uint)CampaignTime.Now.ToMinutes) < Campaign.Current.Models.IncidentModel.GetIncidentTriggerGlobalProbability())
			{
				this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "LeavingEncounter" }));
			}
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x00133954 File Offset: 0x00131B54
		private void OnMapEventEnded(MapEvent evt)
		{
			if (!evt.IsPlayerMapEvent || evt.IsNavalMapEvent)
			{
				return;
			}
			if (!evt.HasWinner || evt.DefeatedSide == evt.PlayerSide)
			{
				return;
			}
			if ((evt.IsFieldBattle || evt.IsHideoutBattle) && !evt.AttackerSide.IsSurrendered && !evt.DefenderSide.IsSurrendered && MobileParty.MainParty.RandomFloatWithSeed((uint)CampaignTime.Now.ToMinutes) < Campaign.Current.Models.IncidentModel.GetIncidentTriggerGlobalProbability())
			{
				this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "LeavingBattle" }));
			}
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x001339FC File Offset: 0x00131BFC
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (!this._canInvokeSettlementEvent)
			{
				return;
			}
			if (MobileParty.MainParty.RandomFloatWithSeed((uint)CampaignTime.Now.ToMinutes) < Campaign.Current.Models.IncidentModel.GetIncidentTriggerGlobalProbability())
			{
				if (args.MenuContext.GameMenu.StringId == "town")
				{
					this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "EnteringTown" }));
					this._canInvokeSettlementEvent = false;
				}
				if (args.MenuContext.GameMenu.StringId == "village")
				{
					this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "EnteringVillage" }));
					this._canInvokeSettlementEvent = false;
				}
				if (args.MenuContext.GameMenu.StringId == "castle")
				{
					this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "EnteringCastle" }));
					this._canInvokeSettlementEvent = false;
				}
			}
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00133AF8 File Offset: 0x00131CF8
		private void OnGameMenuOptionSelected(GameMenu gameMenu, GameMenuOption option)
		{
			if (MobileParty.MainParty.RandomFloatWithSeed((uint)CampaignTime.Now.ToMinutes) < Campaign.Current.Models.IncidentModel.GetIncidentTriggerGlobalProbability())
			{
				if (gameMenu.StringId == "town" && option.IdString == "town_leave")
				{
					this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "LeavingTown", "LeavingTown" }));
				}
				if (gameMenu.StringId == "castle" && option.IdString == "leave")
				{
					this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "LeavingCastle", "LeavingCastle" }));
				}
				if (gameMenu.StringId == "village" && option.IdString == "leave")
				{
					this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "LeavingVillage", "LeavingVillage" }));
				}
			}
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x00133C05 File Offset: 0x00131E05
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x00133C08 File Offset: 0x00131E08
		private void OnHourlyTick()
		{
			float num = MobileParty.MainParty.RandomFloatWithSeed((uint)CampaignTime.Now.ToMinutes);
			if (MobileParty.MainParty.SiegeEvent != null && num < Campaign.Current.Models.IncidentModel.GetIncidentTriggerProbabilityDuringSiege())
			{
				this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "DuringSiege" }));
				return;
			}
			if (Campaign.Current.CurrentMenuContext != null && (Campaign.Current.CurrentMenuContext.GameMenu.StringId == "town_wait_menus" || Campaign.Current.CurrentMenuContext.GameMenu.StringId == "village_wait_menus") && num < Campaign.Current.Models.IncidentModel.GetIncidentTriggerProbabilityDuringWait())
			{
				this.TryInvokeIncident(new Incident.IncidentTrigger(new string[] { "WaitingInSettlement" }));
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x00133CE8 File Offset: 0x00131EE8
		private void TryInvokeIncident(Incident.IncidentTrigger trigger)
		{
			if ((trigger.HasTrigger("EnteringCastle") || trigger.HasTrigger("EnteringTown") || trigger.HasTrigger("EnteringVillage")) && (MobileParty.MainParty.CurrentSettlement == null || MobileParty.MainParty.CurrentSettlement.IsSettlementBusy(this)))
			{
				return;
			}
			if ((trigger.HasTrigger("LeavingTown") || trigger.HasTrigger("LeavingVillage") || trigger.HasTrigger("LeavingCastle")) && MobileParty.MainParty.LastVisitedSettlement.IsSettlementBusy(this))
			{
				return;
			}
			if (Campaign.Current.ConversationManager.IsConversationFlowActive)
			{
				return;
			}
			Campaign.Current.IncidentManager.TryInvokeIncident(trigger);
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x00133D97 File Offset: 0x00131F97
		private Incident RegisterIncident(string id, string title, string description, string triggerId, string type, CampaignTime cooldown, Func<TextObject, bool> condition)
		{
			return this.RegisterIncident(id, title, description, new string[] { triggerId }, type, cooldown, condition);
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x00133DB3 File Offset: 0x00131FB3
		private Incident RegisterIncident(string id, string title, string description, string[] triggerIds, string type, CampaignTime cooldown, Func<TextObject, bool> condition)
		{
			Incident incident = Game.Current.ObjectManager.RegisterPresumedObject<Incident>(new Incident(id));
			incident.Initialize(title, description, new Incident.IncidentTrigger(triggerIds), type, cooldown, condition);
			return incident;
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x00133DE0 File Offset: 0x00131FE0
		private void InitializeIncidents()
		{
			Incident incident = this.RegisterIncident("incident_rooster_theft", "{=0EG0iB9p}Rooster theft", "{=8zFwYZeQ}As you ride off, a villager runs after you accusing one of your men of stealing a rooster. A few squawks silence the culprit's attempt to deny the crime. Honor requires you to uphold the laws of the land, but your men expect a generous {RANK} to value those who shed blood for {PRONOUN} over those who do not.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Days(60f), delegate(TextObject description)
			{
				description.SetTextVariable("RANK", Hero.MainHero.IsLord ? "lord" : "captain");
				description.SetTextVariable("PRONOUN", Hero.MainHero.IsFemale ? "her" : "him");
				return PartyBase.MainParty.MemberRoster.TotalRegulars >= 25;
			});
			string text169 = "{=0k4T7dzu}Return the rooster and have your man whipped.";
			List<IncidentEffect> list = new List<IncidentEffect>();
			list.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -50));
			list.Add(CampaignIncidentEffects.MoraleChange(-5f));
			list.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 10, null));
			incident.AddOption(text169, list, null, null);
			string text2 = "{=Ppzvp7Mu}Tell the peasant that the rooster is just part of the debt they owe their protectors.";
			List<IncidentEffect> list2 = new List<IncidentEffect>();
			list2.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -10, null));
			incident.AddOption(text2, list2, null, null);
			string text3 = "{=NdcsYWrF}Throw the peasant a few silvers in compensation.";
			List<IncidentEffect> list3 = new List<IncidentEffect>();
			list3.Add(CampaignIncidentEffects.GoldChange(() => -10));
			list3.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list3.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			incident.AddOption(text3, list3, null, null);
			Incident incident2 = this.RegisterIncident("incident_suitable_boy", "{=7y4Fbx8K}A Suitable Boy", "{=jB4tmY9W}As you leave the village, a young woman approaches you carrying a months-old baby, surrounded by her relatives. She points at one of your men and says that he was the father, and that he should stay here and wed her. The lad, a strong fellow but not good with words, is too flustered to speak. You can't exactly recall if you passed through here a year ago or not.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => PartyBase.MainParty.MemberRoster.TotalRegulars >= 30);
			Incident incident3 = incident2;
			string text4 = "{=ThnC13fF}Tell the lad to search his conscience and do what is right, under the eyes of Heaven";
			List<IncidentEffect> list4 = new List<IncidentEffect>();
			list4.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			List<IncidentEffect> list5 = list4;
			IncidentEffect[] array = new IncidentEffect[2];
			array[0] = CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 1, false);
			array[1] = CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 10, null);
			list5.Add(CampaignIncidentEffects.Group(array).WithChance(0.5f));
			incident3.AddOption(text4, list4, null, null);
			Incident incident4 = incident2;
			string text5 = "{=bjDjwN9r}Congratulate the lad on finding a wife and throw the couple a purse of coins as a wedding gift";
			List<IncidentEffect> list6 = new List<IncidentEffect>();
			list6.Add(CampaignIncidentEffects.GoldChange(() => -50));
			list6.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list6.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 1, false));
			incident4.AddOption(text5, list6, null, null);
			Incident incident5 = incident2;
			string text6 = "{=sb9zWc3I}Give your blessing to the marriage and hurl huge handfuls of coins to the villagers, declaring that this will be a day that all shall remember.";
			List<IncidentEffect> list7 = new List<IncidentEffect>();
			list7.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list7.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list7.Add(CampaignIncidentEffects.GoldChange(() => -1000));
			list7.Add(CampaignIncidentEffects.RenownChange(5f));
			incident5.AddOption(text6, list7, null, null);
			incident2.AddOption("{=eP8MrIEE}Tell the villagers to get themselves a plough horse instead of trying to poach one of your men", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.MoraleChange(5f)
			}, null, null);
			Incident incident6 = this.RegisterIncident("incident_spoiled_food_supplies", "{=5aPGeeDf}Spoiled Food Supplies", "{=RsWbJJS7}A delegation of your troops approach you. They show you a heel of bread from your stores, crawling with maggots. A piece of dried meat is streaked with mold. Your quartermaster appears to have packed the goods carelessly, allowing moisture and pests to get inside.", "LeavingVillage", "FoodConsumption", CampaignTime.Days(60f), (TextObject description) => PartyBase.MainParty.MemberRoster.TotalRegulars >= 10);
			incident6.AddOption("{=p0gqO74k}Money is tight and you've eaten worse in your time. Tell the lads to think of it as seasoning", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.WoundTroopsRandomly(0.05f).WithChance(0.5f)
			}, null, null);
			string text7 = "{=DcaV3yt9}Order your men to throw away the spoiled food";
			List<IncidentEffect> list8 = new List<IncidentEffect>();
			list8.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list8.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSpoiledFoodItem|18_18), () => -2));
			incident6.AddOption(text7, list8, null, null);
			incident6.AddOption("{=YbqueBpd}Punish your quartermaster for storing it improperly, and take the money to replace it out of his pay. Discipline must prevail", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.MoraleChange(5f)
			}, null, null);
			incident6.AddOption("{=fxfInKgI}Wolf down the food yourself and smack your lips, telling the men that they are lucky to have such fine rations.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 50),
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.HealthChance(-10).WithChance(0.5f)
			}, null, null);
			Incident incident7 = this.RegisterIncident("incident_desperate_times", "{=GNdltUJj}Desperate Times", "{=CZamxmfc}As you pass through the town gates, you are thronged by a group of refugees from the countryside. They are gaunt, with sunken eyes. They say they were unable to sow their fields this year for fear of bandits, and were forced to eat their seed grain. They ask for any scraps you can offer.", "LeavingTown", "PlightOfCivilians", CampaignTime.Days(60f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement != null && MobileParty.MainParty.LastVisitedSettlement.IsTown)
				{
					return MobileParty.MainParty.LastVisitedSettlement.Town.Villages.Any<Village>((Village x) => x.VillageState == Village.VillageStates.Looted);
				}
				return false;
			});
			string text8 = "{=BjUtGJg6}Break out your food and allow them to fill their bellies before your depart";
			List<IncidentEffect> list9 = new List<IncidentEffect>();
			list9.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list9.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetDesperateTimesFoodItem|18_22), () => -1));
			incident7.AddOption(text8, list9, null, null);
			string text9 = "{=JO6D30FU}Give them sufficient money to replace their seed grain, even though the townspeople may complain that it raises prices";
			List<IncidentEffect> list10 = new List<IncidentEffect>();
			list10.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list10.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200));
			list10.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -5, null));
			incident7.AddOption(text9, list10, null, null);
			incident7.AddOption("{=BslqBNQj}Tell them that you cannot help", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100) }, null, null);
			Incident incident8 = this.RegisterIncident("incident_no_country_for_the_afflicted", "{=rflKyqOT}No Country for the Afflicted", "{=NORgVfas}As you prepare to leave, a delegation of townspeople approach you. A group of lepers has encamped before the walls, and they beg passers-by for alms. The townspeople are worried that they might spread the disease and, at any rate, are frightening away potential customers.", "LeavingTown", "PlightOfCivilians", CampaignTime.Days(60f), (TextObject description) => true);
			string text10 = "{=yfsqkUcb}Tell the townspeople that Heaven wishes to test their compassion, and that they should treat the lepers with kindness.";
			List<IncidentEffect> list11 = new List<IncidentEffect>();
			list11.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200));
			list11.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -5, null));
			list11.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, -10));
			incident8.AddOption(text10, list11, null, null);
			string text11 = "{=z6vCG954}Give the lepers some money and food on the condition that they try to find a more welcoming town somewhere else";
			List<IncidentEffect> list12 = new List<IncidentEffect>();
			list12.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list12.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list12.Add(CampaignIncidentEffects.GoldChange(() => -100));
			incident8.AddOption(text11, list12, null, null);
			string text12 = "{=egSS7oyV}Drive the lepers away, telling them they must have sinned grievously to be so afflicted";
			List<IncidentEffect> list13 = new List<IncidentEffect>();
			list13.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list13.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 10, null));
			incident8.AddOption(text12, list13, null, null);
			incident8.AddOption("{=zcMJEpHC}Don't get involved", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100) }, null, null);
			Incident incident9 = this.RegisterIncident("incident_veterans_demand_privileges", "{=j8Aq3lRW}Veterans demand privileges", "{=FbKc1AwE}Several of your newer recruits approach you. Your {HIGH_TIER_TROOPS} have been demanding the freshest cuts of meat and highest quality bread from your stores, letting them make do with gristle and stale crusts. The veterans say that this is the privilege of experience, and that it will motivate your recruits to improve themselves.", new string[] { "LeavingCastle", "LeavingVillage", "LeavingTown" }, "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				CharacterObject characterObject = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVeteranMaxTierTroop|18_35();
				if (characterObject == null)
				{
					return false;
				}
				description.SetTextVariable("HIGH_TIER_TROOPS", characterObject.Name);
				return PartyBase.MainParty.ItemRoster.Any<ItemRosterElement>((ItemRosterElement x) => !x.IsEmpty && x.EquipmentElement.Item == DefaultItems.Meat);
			});
			string text13 = "{=56hS9BRG}Admonish the {HIGH_TIER_TROOPS} for their indiscipline, even though they will be humiliated and may even desert";
			List<IncidentEffect> list14 = new List<IncidentEffect>();
			list14.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list14.Add(CampaignIncidentEffects.MoraleChange(5f));
			list14.Add(CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVeteranMaxTierTroop|18_35), -1, null, null).WithChance(0.5f));
			incident9.AddOption(text13, list14, delegate(TextObject text)
			{
				CharacterObject characterObject2 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVeteranMaxTierTroop|18_35();
				text.SetTextVariable("HIGH_TIER_TROOPS", characterObject2.Name);
				return true;
			}, null);
			incident9.AddOption("{=EAu32UBG}Tell your recruits that your veterans have indeed earned the right to the best food", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100)
			}, null, null);
			string text14 = "{=37YTPzfG}Tell your men that they have all earned the right to your esteem, and break out an extra ration of {FOOD_OR_DRINK_ITEM} to show your appreciation";
			List<IncidentEffect> list15 = new List<IncidentEffect>();
			list15.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list15.Add(CampaignIncidentEffects.MoraleChange(5f));
			list15.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVeteranFoodItem|18_34), () => -1));
			incident9.AddOption(text14, list15, delegate(TextObject text)
			{
				ItemObject itemObject = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVeteranFoodItem|18_34();
				text.SetTextVariable("FOOD_OR_DRINK_ITEM", itemObject.Name);
				return true;
			}, null);
			incident9.AddOption("{=USdND5SY}You propose a compromise - a slightly better ration for veteran troops - knowing it will avoid a fight but come across as indecisive.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100)
			}, null, null);
			Incident incident10 = this.RegisterIncident("incident_an_affair_of_honor", "{=gUKUisS6}An Affair of Honor", "{=akvIjedh}Two of your cavalrymen request permission to fight a duel. Both had been wooing a merchant's daughter in {TOWN}, and one slighted the other in front of her. Some commanders encourage their men to duel as a boost to their fiery sense of pride, but others think that such fights erode their discipline as soldiers and, of course, carry the risk of injury.", "LeavingVillage", "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				bool flag = (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
					where x.Character.Tier >= 4 && x.Character.HasMount() && !x.Character.IsHero
					select x).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) >= 5;
				bool flag2 = MobileParty.MainParty.MemberRoster.GetTroopRoster().Count<TroopRosterElement>((TroopRosterElement x) => x.Character.Tier >= 4 && x.Character.HasMount() && !x.Character.IsHero) >= 2;
				if (!flag || !flag2)
				{
					return false;
				}
				description.SetTextVariable("TOWN", MobileParty.MainParty.LastVisitedSettlement.Name);
				return true;
			});
			incident10.AddOption("{=24pUil64}You like that fiery spirit! Let them fight until one falls and cannot rise", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.PartyExperienceChance(200),
				CampaignIncidentEffects.WoundTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetFirstNobleTroop|18_44), 1).WithChance(0.5f),
				CampaignIncidentEffects.KillTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSecondNobleTroop|18_45), 1).WithChance(0.5f)
			}, null, null);
			incident10.AddOption("{=37lEhqtv}Let them fight until the first drop of blood is shed", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.PartyExperienceChance(100),
				CampaignIncidentEffects.WoundTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetFirstNobleTroop|18_44), 1).WithChance(0.5f),
				CampaignIncidentEffects.WoundTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSecondNobleTroop|18_45), 1).WithChance(0.5f)
			}, null, null);
			incident10.AddOption("{=cexRoZPF}You will allow no such foolishness. Save their anger for the enemy", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			incident10.AddOption("{=ZYopfLDg}Tell your men to throw a die, leaving the matter to fate.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100) }, null, null);
			Incident incident11 = this.RegisterIncident("incident_troops_fight_over_insult", "{=4mC7AIPk}Troops Fight over Insult", "{=HpkqVXQ3}A fight breaks out in your camp. A high-born {CAVALRYMAN} demanded a {FOOTMAN} water his horse, even though the {FOOTMAN} is a veteran nearly twice the {CAVALRYMAN}'s age. The two then exchanged insults, and now the {FOOTMAN} demands a duel while the {CAVALRYMAN}, who is only willing to duel his social equal, demands the {FOOTMAN} be told to watch his place.", "LeavingVillage", "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				CharacterObject characterObject3 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetInsultCavalryman|18_54();
				CharacterObject characterObject4 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetInsultFootman|18_55();
				if (characterObject4 == null || characterObject3 == null)
				{
					return false;
				}
				description.SetTextVariable("CAVALRYMAN", characterObject3.Name);
				description.SetTextVariable("FOOTMAN", characterObject4.Name);
				return MobileParty.MainParty.MemberRoster.GetTroopRoster().Any<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__CavalryTroopsPredicate|18_52)) && MobileParty.MainParty.MemberRoster.GetTroopRoster().Any<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__FootmenTroopsPredicate|18_53));
			});
			Incident incident12 = incident11;
			string text15 = "{=QGcozvvK}Only blood can wipe out an insult! Let the two have it out, until one falls and cannot rise";
			list4 = new List<IncidentEffect>();
			list4.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 200));
			list4.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			List<IncidentEffect> list16 = list4;
			ValueTuple<IncidentEffect, float>[] array2 = new ValueTuple<IncidentEffect, float>[4];
			array2[0] = new ValueTuple<IncidentEffect, float>(CampaignIncidentEffects.KillTroopsRandomly(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__FootmenTroopsPredicate|18_53), () => 1, false), 0.25f);
			array2[1] = new ValueTuple<IncidentEffect, float>(CampaignIncidentEffects.WoundTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetInsultCavalryman|18_54), 1), 0.25f);
			array2[2] = new ValueTuple<IncidentEffect, float>(CampaignIncidentEffects.KillTroopsRandomly(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__CavalryTroopsPredicate|18_52), () => 1, false), 0.25f);
			array2[3] = new ValueTuple<IncidentEffect, float>(CampaignIncidentEffects.WoundTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetInsultFootman|18_55), 1), 0.25f);
			list16.Add(CampaignIncidentEffects.Select(array2));
			list4.Add(CampaignIncidentEffects.PartyExperienceChance(200));
			incident12.AddOption(text15, list4, null, null);
			Incident incident13 = incident11;
			string text16 = "{=HbQDtv60}Reprimand the cavalryman, even though he is fiery and might desert";
			List<IncidentEffect> list17 = new List<IncidentEffect>();
			list17.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100));
			list17.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list17.Add(CampaignIncidentEffects.MoraleChange(5f));
			list17.Add(CampaignIncidentEffects.KillTroopsRandomly(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__CavalryTroopsPredicate|18_52), () => 1, false).WithChance(0.25f));
			incident13.AddOption(text16, list17, null, null);
			incident11.AddOption("{=7YCEpRij}Reprimand the footman, who you know will grumble but will have the sense to swallow his pride", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident11.AddOption("{=OGDApTnl}Tell them both to stop behaving like fools", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			Incident incident14 = this.RegisterIncident("incident_arrow_proofing", "{=JKO2Rmlr}Arrow-proofing", "{=ZCqZvb7K}As you leave the village, one of your soldiers proudly shows you a magic oil he bought from a peddlar, who told him that, if he annoints himself with it, it will prevent him from being wounded in battle. ", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Days(60f), (TextObject description) => PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident14.AddOption("{=CNYKy7q3}Congratulate the soldier on his wise purchase, and tell him that you expect to see him in the thick of the fray in the next battle", new List<IncidentEffect>
			{
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			incident14.AddOption("{=CXZxDvry}Tell your soldiers they should rely on their own strength instead of magic", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100) }, null, null);
			incident14.AddOption("{=NHwt84uk}Chastise your man for being so gullible", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			Incident incident15 = this.RegisterIncident("incident_purebred_horse", "{=eWh4BDkx}Purebred Horse", "{=J9HLJpk7}As you leave the village, one of your cavalry recruits proudly presents to you a new horse he purchased. Maybe it's not the fastest beast on four legs, he says, but it's strong and spirited - he saw it fighting another of the trader's horses. You run an eye over the beast and see that your man was sold a mule, its ears cleverly cropped to resemble a horse's. The creature is strong enough but far too stubborn to serve as a cavalry mount. You know that your man will be the laughingstock of your company for weeks.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.ItemRoster.Any<ItemRosterElement>((ItemRosterElement x) => x.EquipmentElement.Item.HasHorseComponent));
			string text17 = "{=DHcl6JU5}Give him one of your own horses, add the mule to your pack train, and tell him to talk to you before he makes any more purchases";
			List<IncidentEffect> list18 = new List<IncidentEffect>();
			list18.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetPurebredHorseItem|18_61), () => -1));
			list18.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("mule"), () => 1));
			list18.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			incident15.AddOption(text17, list18, null, null);
			string text18 = "{=3uHIXyDk}Your men could use a laugh. Let the recruit show off his prize to his mates and take the humiliation, even if it's enough to drive him to desert";
			List<IncidentEffect> list19 = new List<IncidentEffect>();
			list19.Add(CampaignIncidentEffects.MoraleChange(5f));
			list19.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => x.Character.HasMount() && !x.Character.IsHero && !x.Character.IsHero, () => 1, false).WithChance(0.25f));
			incident15.AddOption(text18, list19, null, null);
			incident15.AddOption("{=HXVVJYQr}Tell him curtly what he did and that he needs to go get his old mount back from the trader, even if it costs him a few denars", new List<IncidentEffect>
			{
				CampaignIncidentEffects.MoraleChange(-2f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100)
			}, null, null);
			Incident incident16 = this.RegisterIncident("incident_veteran_mentor", "{=1jmYaNxu}Veteran Mentor", "{=CObK3sDa}As you leave the village, a greybeard walks up to you carrying a cudgel. Although he's too old to fight, he says, he was a warrior once and claims to know a trick move, a way of suddenly closing with your opponent and tripping them with a spear, which he offers to teach to your troops for a price. He shows you his battle-scars, which are indeed impressive.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => true);
			incident16.AddOption("{=aME3jqrO}Tell him that tricks don't win battles, but bravery does.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.MoraleChange(5f)
			}, null, null);
			string text19 = "{=43GeTG0K}Accept, even if it costs you time, recognizing that a warrior who survived so many battles probably knows a thing or two.";
			List<IncidentEffect> list20 = new List<IncidentEffect>();
			list20.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list20.Add(CampaignIncidentEffects.PartyExperienceChance(200));
			list20.Add(CampaignIncidentEffects.DisorganizeParty());
			incident16.AddOption(text19, list20, null, null);
			string text20 = "{=JvUb1wr4}Accept, and tell him to demonstrate his technique with a sword on one of your men, so that the lesson sticks.";
			List<IncidentEffect> list21 = new List<IncidentEffect>();
			list21.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list21.Add(CampaignIncidentEffects.PartyExperienceChance(400));
			list21.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list21.Add(CampaignIncidentEffects.WoundTroopsRandomly(1));
			incident16.AddOption(text20, list21, null, null);
			Incident incident17 = this.RegisterIncident("incident_horsefly_plague", "{=S4YyXUPU}Horsefly Plague", "{=NJ0BBaaC}One of your cavalry commanders reported that some horses seem mysteriously weak. You see their listless eyes and swollen bellies and recognize it - swamp fever. This plague is spread by horseflies, and it will infect other animals if the horse is not killed", "LeavingVillage", "AnimalIllness", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.ItemRoster.Where<ItemRosterElement>((ItemRosterElement x) => x.EquipmentElement.Item.HasHorseComponent).Sum<ItemRosterElement>((ItemRosterElement x) => x.Amount) >= 4);
			incident17.AddOption("{=SIVBGe7H}Isolate the infected horses immediately to prevent the spread ", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50),
				CampaignIncidentEffects.ChangeItemsAmount(new Func<List<ItemObject>>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHorseflyPlagueHorseItems|18_73), -1),
				CampaignIncidentEffects.ChangeItemsAmount(new Func<List<ItemObject>>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHorseflyPlagueHorseItems|18_73), -3).WithChance(0.3f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			incident17.AddOption("{=A4voELZr}Order the disposal of the infected horses to avoid further contamination.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.ChangeItemsAmount(new Func<List<ItemObject>>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHorseflyPlagueHorseItems|18_73), -2)
			}, null, null);
			incident17.AddOption("{=ZCWziaI4}Ignore the outbreak and continue marching", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.ChangeItemsAmount(new Func<List<ItemObject>>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHorseflyPlagueHorseItems|18_73), -1),
				CampaignIncidentEffects.ChangeItemsAmount(new Func<List<ItemObject>>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHorseflyPlagueHorseItems|18_73), -3).WithChance(0.8f)
			}, null, null);
			Incident incident18 = this.RegisterIncident("incident_glanders_outbreak", "{=QjWhMbL1}Glanders Outbreak", "{=xH4KUzVS}As you leave town, you notice one of your horses has a swollen head and a bloody nose. You recognize this disease. It can spread to your men, and not only this animal is at risk but every other animal that drank from the same trough.", "LeavingVillage", "AnimalIllness", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopsToDemotePredicate|18_79)).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) >= 5);
			incident18.AddOption("{=AIABabCI}Kill the horse and the two other horses it shared a stable with.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopsToDemotePredicate|18_79), new Func<CharacterObject, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemoteToPredicate|18_80), 3, true),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100)
			}, null, null);
			Incident incident19 = incident18;
			string text21 = "{=IBIbetoC}Kill the horse alone, hoping for the best.";
			list4 = new List<IncidentEffect>();
			list4.Add(CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopsToDemotePredicate|18_79), new Func<CharacterObject, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemoteToPredicate|18_80), 1, true));
			List<IncidentEffect> list22 = list4;
			IncidentEffect[] array3 = new IncidentEffect[2];
			array3[0] = CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopsToDemotePredicate|18_79), new Func<CharacterObject, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemoteToPredicate|18_80), 3, true);
			array3[1] = CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 2, false);
			list22.Add(CampaignIncidentEffects.Group(array3).WithChance(0.25f));
			incident19.AddOption(text21, list4, null, null);
			Incident incident20 = incident18;
			string text22 = "{=bWz49lZu}Allow the cavalryman to care for the animal, even though it puts him and others at risk.";
			list4 = new List<IncidentEffect>();
			List<IncidentEffect> list23 = list4;
			IncidentEffect incidentEffect = CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100);
			IncidentEffect[] array4 = new IncidentEffect[3];
			array4[0] = CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopsToDemotePredicate|18_79), new Func<CharacterObject, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemoteToPredicate|18_80), 3, true);
			array4[1] = CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 2, false);
			array4[2] = CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -200);
			list23.Add(CampaignIncidentEffects.Select(incidentEffect, CampaignIncidentEffects.Group(array4).WithChance(0.25f), 0.5f));
			incident20.AddOption(text22, list4, null, null);
			Incident incident21 = this.RegisterIncident("incident_colicky_horses", "{=k4kbVxTL}Colicky Horses", "{=Bi8fKaGw}As you leave town, one of your cavalrymen finds his mount lying on the ground, whining in pain and refusing to get up. You're sure that he was careless and fed the animal too quickly after your arrival, as he was eager to get to the town's tavern.", "LeavingTown", "AnimalIllness", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemotePredicate|18_87)).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) >= 15);
			incident21.AddOption("{=bNTKEu9p}Provide what care you can, even though it will slow you down and there is no sure chance of success", new List<IncidentEffect>
			{
				CampaignIncidentEffects.DisorganizeParty(),
				CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemotePredicate|18_87), new Func<CharacterObject, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemoteToPredicate|18_88), 1, true).WithChance(0.5f)
			}, null, null);
			incident21.AddOption("{=9yFSbR36}Whip the responsible cavalryman for his negligence, and let him buy his own remount.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			incident21.AddOption("{=qRZpuLae}End the animal's misery quickly, and tell the horseman that you will not trust another poor beast to his care.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemotePredicate|18_87), new Func<CharacterObject, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__TroopToDemoteToPredicate|18_88), 1, true)
			}, null, null);
			Incident incident22 = this.RegisterIncident("incident_camp_fever", "{=l2rWAm4N}Camp Fever", "{=Q5MXehRY}As you break camp, you find that two of your men are bathed in sweat and cannot rouse themselves. They are confused, and one is starting to show pock-marks on his chest. You recognize the symptoms as camp fever, and fear it will spread quickly. Their companions want to tend to them, but you know that if you don't quarantine the men the disease is likely to spread. ", "LeavingEncounter", "Illness", CampaignTime.Days(60f), (TextObject description) => !MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.MemberRoster.TotalRegulars >= 50);
			incident22.AddOption("{=htMJ1uiy}Load the sick onto a cart and give them water, but otherwise let their comrades have as little contact with them as possible. ", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.WoundTroopsRandomlyWithChanceOfDeath(2, 0.25f)
			}, null, null);
			string text23 = "{=4v8LpVb8}Send some men to fetch a doctor and medicine, accepting that it will delay your march";
			List<IncidentEffect> list24 = new List<IncidentEffect>();
			list24.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list24.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list24.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100));
			list24.Add(CampaignIncidentEffects.DisorganizeParty());
			incident22.AddOption(text23, list24, null, null);
			string text24 = "{=c5uy0bb9}Let your men treat the sick as you march";
			List<IncidentEffect> list25 = new List<IncidentEffect>();
			list25.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 1, false).WithChance(0.1f));
			list25.Add(CampaignIncidentEffects.WoundTroopsRandomly(2).WithChance(0.6f));
			incident22.AddOption(text24, list25, null, null);
			Incident incident23 = this.RegisterIncident("incident_bloody_flux", "{=LEHxD9ik}Bloody Flux", "{=3FwbFdbw}As you inspect your siege camp, you come across disgusting evidence that one or more of your men has contracted the bloody flux. It will pass from man to man as they share food and water, and you know what it can do to a besieging army if left unchecked.", "DuringSiege", "Illness", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalRegulars >= 70);
			incident23.AddOption("{=MSARXv6p}Identify the sick and make them camp outside your palisades, tending to themselves.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.WoundTroopsRandomlyWithChanceOfDeath(0.05f, 0.5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			incident23.AddOption("{=Qnb8wrL0}Treat your men but trust in strict preventative measures, like digging latrines outside camp and forcing your men to go far afield for water, that they are likely to resent.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.WoundTroopsRandomlyWithChanceOfDeath(3, 0.25f),
				CampaignIncidentEffects.WoundTroopsRandomlyWithChanceOfDeath(5, 0.25f).WithChance(0.5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			incident23.AddOption("{=zdPb0vtU}Treat the sick and make no additional demands on your men, trusting in luck to see you through this.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.WoundTroopsRandomlyWithChanceOfDeath(0.1f, 0.3f)
			}, null, null);
			Incident incident24 = this.RegisterIncident("incident_lice_and_fleas", "{=CX8WdvvR}Lice and Fleas", "{=b4BaOUKn}Lice are part of military life but several of your men are absolutely teaming with the vermin. Perhaps they are especially virulent in these parts, or perhaps they thrive in this weather. You fear that they will spread camp fever or other diseases.", "LeavingEncounter", "Illness", CampaignTime.Days(60f), (TextObject description) => !MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.MemberRoster.TotalRegulars >= 45);
			incident24.AddOption("{=ybnqAVbP}Force the afflicted to shave every strand of their hair and beard", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident24.AddOption("{=PAW2b2Y2}Require your men to go through the laborious process of picking clean their bodies and clothes", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			incident24.AddOption("{=irLNaJ3m}Tell your men to give themselves a good scratch and keep marching", new List<IncidentEffect>
			{
				CampaignIncidentEffects.WoundTroopsRandomly(3).WithChance(0.5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100)
			}, null, null);
			Incident incident25 = this.RegisterIncident("incident_saint_of_the_woods", "{=SqpOlZhl}Saint of the Woods", "{=RrHS8JOx}As you depart the scene of your victory, a man dressed in white robes emerges from a nearby wood. He tells you that you owe your victory to the intervention of Hageos Tristamos, a holy man from a century ago who assists those who fight in a noble cause, and asks that you donate half the loot to the local shrine in gratitude, where it will be used to feed the poor.", "LeavingBattle", "Illness", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= 3);
			string text25 = "{=4dIYUMkA}Donate the spoils of battle to the saint's shrine";
			List<IncidentEffect> list26 = new List<IncidentEffect>();
			list26.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list26.Add(CampaignIncidentEffects.GoldChange(() => -300));
			incident25.AddOption(text25, list26, null, null);
			string text26 = "{=Ioc2z1Ap}Declare that you owe your victory to no saint but to your men, and give them the silver";
			List<IncidentEffect> list27 = new List<IncidentEffect>();
			list27.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list27.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list27.Add(CampaignIncidentEffects.MoraleChange(3f));
			list27.Add(CampaignIncidentEffects.GoldChange(() => -300));
			incident25.AddOption(text26, list27, null, null);
			string text27 = "{=2auYPXJ8}Declare that you owe your victory to your own right arm, and keep the silver";
			List<IncidentEffect> list28 = new List<IncidentEffect>();
			list28.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200));
			list28.Add(CampaignIncidentEffects.GoldChange(() => 300));
			incident25.AddOption(text27, list28, null, null);
			string text28 = "{=NLbvOuFt}Fall on your knees in tears, repent of your sins, and leave a huge donation";
			List<IncidentEffect> list29 = new List<IncidentEffect>();
			list29.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list29.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list29.Add(CampaignIncidentEffects.RenownChange(5f));
			list29.Add(CampaignIncidentEffects.GoldChange(() => -500));
			incident25.AddOption(text28, list29, null, null);
			Incident incident26 = this.RegisterIncident("incident_servants_of_mercy", "{=FYvrShj3}Servants of Mercy", "{=t0iKT1VJ}As the dust of battle settles, you see a group of women tending to the wounded. They explain that they are the Servants of Hope, mendicant sisters pledged to assist the afflicted in the hope of winning mercy from Heaven. They offer to accompany your party for a few days and tend to your men's wounds, asking nothing in return.", "LeavingBattle", "Illness", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= 3);
			incident26.AddOption("{=5ByLt1QP}Tell the Sisters that they will slow you down, and your men can heal on their own.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			incident26.AddOption("{=STLwVkdO}Accept the Sisters' help", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.DisorganizeParty(),
				CampaignIncidentEffects.HealTroopsRandomly(3)
			}, null, null);
			Incident incident27 = this.RegisterIncident("incident_right_of_pillage", "{=p0bMRIum}Right of Pillage", "{=NWJ87xjB}As you prepare to march forth from newly captured {TOWN_NAME}, your men stop you. They shed their blood for your wars and you denied them their customary right to sack it. They say that it you will not let them enrich themselves from the enemy, then you should enrich them from your purse", new string[] { "LeavingTown", "LeavingCastle" }, "PartyCampLife", CampaignTime.Days(60f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				BesiegeSettlementLogEntry besiegeSettlementLogEntry = IncidentsCampaignBehaviour.<InitializeIncidents>g__FindLastBesiegeSettlementLogEntry|18_103();
				if (besiegeSettlementLogEntry == null)
				{
					return false;
				}
				SiegeAftermathLogEntry siegeAftermathLogEntry = IncidentsCampaignBehaviour.<InitializeIncidents>g__FindLastSiegeAftermathLogEntry|18_104(besiegeSettlementLogEntry);
				if (siegeAftermathLogEntry == null)
				{
					return false;
				}
				if (siegeAftermathLogEntry.GameTime < besiegeSettlementLogEntry.GameTime)
				{
					return false;
				}
				description.SetTextVariable("TOWN_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				return PartyBase.MainParty.MemberRoster.TotalRegulars >= 5;
			});
			string text29 = "{=lm37mXGH}Let your men return and sack the town while it is still defenseless";
			List<IncidentEffect> list30 = new List<IncidentEffect>();
			list30.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				BesiegeSettlementLogEntry besiegeSettlementLogEntry2 = IncidentsCampaignBehaviour.<InitializeIncidents>g__FindLastBesiegeSettlementLogEntry|18_103();
				if (((besiegeSettlementLogEntry2 != null) ? besiegeSettlementLogEntry2.OwnerClanBeforeBesiege : null) == null || besiegeSettlementLogEntry2.OwnerClanBeforeBesiege.IsEliminated)
				{
					return new List<TextObject>();
				}
				SiegeAftermathAction.ApplyAftermath(MobileParty.MainParty, MobileParty.MainParty.LastVisitedSettlement, SiegeAftermathAction.SiegeAftermath.Pillage, besiegeSettlementLogEntry2.OwnerClanBeforeBesiege, new Dictionary<MobileParty, float>());
				return new List<TextObject>();
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=6QuPiTsY}Consequences of a town sacked", null), IncidentHintType.Effect)));
			incident27.AddOption(text29, list30, null, null);
			string text30 = "{=MLIyVwfx}Pay them yourself";
			List<IncidentEffect> list31 = new List<IncidentEffect>();
			list31.Add(CampaignIncidentEffects.GoldChange(() => -1000));
			list31.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list31.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list31.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			incident27.AddOption(text30, list31, null, null);
			incident27.AddOption("{=HG7WWmX8}Punish the troublemakers for sewing mutiny", new List<IncidentEffect> { CampaignIncidentEffects.MoraleChange(-20f) }, null, null);
			incident27.AddOption("{=tK6IkgCK}Tell your men that Heaven expects them to show mercy to the innocent", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-10f)
			}, null, null);
			Incident incident28 = this.RegisterIncident("incident_donative_demand", "{=p8ESONLB}Donative Demand", "{=qaGbN6wX}It has not escaped your men’s notice that you have accumulated a fair amount of money in your treasury. A delegation approaches you and suggests that this would be a good time for the traditional donative, as Calradian emperors of old were known to give their legions.", new string[] { "LeavingCastle", "LeavingVillage", "LeavingTown" }, "PartyCampLife", CampaignTime.Days(60f), (TextObject description) => Hero.MainHero.Gold >= 5000 && MobileParty.MainParty.MemberRoster.TotalRegulars >= 20);
			incident28.AddOption("{=Z7RAahk3}Convince your men that the money would be better spent on supplies and recruits to ensure their survival", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-2f)
			}, null, null);
			incident28.AddOption("{=YOhKn6YV}Tell your men that they can do as they wish with their wages, but you are not in the habit of subsidizing wine vendors, women of questionable virtue and gambling sharks more than necessary", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-2f)
			}, null, null);
			string text31 = "{=ocsX04bn}\"I am a river of wealth unto you, my brave warriors!\"";
			List<IncidentEffect> list32 = new List<IncidentEffect>();
			list32.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list32.Add(CampaignIncidentEffects.MoraleChange(5f));
			list32.Add(CampaignIncidentEffects.GoldChange(() => -500));
			incident28.AddOption(text31, list32, null, null);
			incident28.AddOption("{=NrmCF6qb}Erupt in a towering rage, so they will think twice about bothering you again", new List<IncidentEffect>
			{
				CampaignIncidentEffects.MoraleChange(-5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100)
			}, null, null);
			Incident incident29 = this.RegisterIncident("incident_vedmaks_treasure", "{=6bF0LqJo}Vedmak's Treasure", "{=tfckGAIA}As you prepare to leave, one of your men says the he was approached by a villager. There is a particularly ill-tempered vedmak on the edge of town who makes his living not from healing or love-potions but from black magic, and who threatens to curse villagers unless they pay him. They asked your men to drive the vedmak away and burn his hut, and told them that they can have his ill-gotten gains.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null && MobileParty.MainParty.LastVisitedSettlement.MapFaction.StringId == "sturgia" && MobileParty.MainParty.LastVisitedSettlement.IsVillage && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident29.AddOption("{=byocyGXf}Convince your men that those who anger with vedmaks are likely to see their nose shrivel up or their fingers fall off", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100)
			}, null, null);
			string text32 = "{=8gEBPabt}Let them go and demand they split their haul with you";
			List<IncidentEffect> list33 = new List<IncidentEffect>();
			list33.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 5, null));
			list33.Add(CampaignIncidentEffects.MoraleChange(5f));
			list33.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list33.Add(CampaignIncidentEffects.GoldChange(() => 200));
			incident29.AddOption(text32, list33, null, null);
			incident29.AddOption("{=3zP80NBJ}Tell them that village disputes are a matter for a proper inquest by the authorities", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			Incident incident30 = this.RegisterIncident("incident_the_deer_departed", "{=IG19KaLK}The Deer Departed", "{=oxBlBLWF}You noticed that the deer around this Khuzait village are unusually unafraid of humans. One of your men manages to shoot one. The villagers approach him afterwards and beg him not to eat the animal. Their former shaman transformed himself into a deer some years ago, they say, and while they don't know if the animal your man killed is him or not, they want to perform the burial rights properly, just to be sure.", "LeavingVillage", "HuntingForaging", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				Settlement lastVisitedSettlement = MobileParty.MainParty.LastVisitedSettlement;
				if (((lastVisitedSettlement != null) ? lastVisitedSettlement.MapFaction.StringId : null) == "khuzait")
				{
					return MobileParty.MainParty.MemberRoster.GetTroopRoster().Any<TroopRosterElement>((TroopRosterElement x) => !x.Character.IsHero && x.Character.IsRanged);
				}
				return false;
			});
			string text33 = "{=xJ0FKCtA}Let the villagers take the deer, even though your men scoff at your superstition";
			List<IncidentEffect> list34 = new List<IncidentEffect>();
			list34.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list34.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list34.Add(CampaignIncidentEffects.MoraleChange(-5f));
			list34.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 5, null));
			incident30.AddOption(text33, list34, null, null);
			string text34 = "{=p8yBnCG2}Let your men enjoy their meal, but commend the villagers for making a good attempt to get their hands on a piece of perfectly good meat";
			List<IncidentEffect> list35 = new List<IncidentEffect>();
			list35.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list35.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list35.Add(CampaignIncidentEffects.MoraleChange(5f));
			list35.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("meat"), () => 3));
			list35.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -5, null));
			incident30.AddOption(text34, list35, null, null);
			string text35 = "{=0wRiev5G}Show genuine remorse and participate in the burial rites for the deer, just in case, even if your troops make comments behind your back";
			List<IncidentEffect> list36 = new List<IncidentEffect>();
			list36.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list36.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list36.Add(CampaignIncidentEffects.MoraleChange(-5f));
			list36.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 20, null));
			incident30.AddOption(text35, list36, null, null);
			Incident incident31 = this.RegisterIncident("incident_wedding_celebration", "{=G16Nlaaq}Wedding Celebration", "{=dkwLMXDz}As you break camp, several of your veterans come forward, somewhat sheepishly. They push one of their number forward and say that he married a young lass at the last village that you visited. They ask if they can share one of the amphorae of wine that they've noticed you are carrying in celebration.", "LeavingEncounter", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => !MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.MemberRoster.TotalRegulars >= 10);
			string text36 = "{=Edn4syjZ}Indulge your men, even though you have little doubt that the wedding is an invention";
			List<IncidentEffect> list37 = new List<IncidentEffect>();
			list37.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list37.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list37.Add(CampaignIncidentEffects.MoraleChange(5f));
			list37.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("wine"), () => -1));
			incident31.AddOption(text36, list37, null, null);
			incident31.AddOption("{=u514BSH6}Tell them that you will not reward deceit", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			incident31.AddOption("{=axNOvMcw}Pour all your wine onto the ground, telling them that you cannot tolerate the presence of anything that erodes their morals and discipline", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.MoraleChange(5f)
			}, null, null);
			Incident incident32 = this.RegisterIncident("incident_kannic_splendors", "{=msK9baMp}Kannic Splendors", "{=a1cuuevH}One of your men comes up to you excitedly. A peddler in the marketplace sold him a map to a treasure buried by a long-dead Kannic sorceror in a wadi near here. He wants a bit of time to check it out and promises you a fifth share of anything he finds. You've heard tales of gold found in ancient tombs near here, but in this case you'd give a hundred to one odds that this leads to either a grave that was pillaged centuries ago or someone's underground irrigation cistern, and at any rate the locals won't appreciate your man smashing things up with pick and shovel.", "LeavingTown", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				Settlement lastVisitedSettlement2 = MobileParty.MainParty.LastVisitedSettlement;
				return ((lastVisitedSettlement2 != null) ? lastVisitedSettlement2.MapFaction.StringId : null) == "aserai" && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5;
			});
			incident32.AddOption("{=12bIzNkB}Tell him that no one sells maps to treasures that they could just as easily dig up themselves", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			string text37 = "{=bi8UfCdg}Indulge your man, and wait for his to complete his errand";
			List<IncidentEffect> list38 = new List<IncidentEffect>();
			list38.Add(CampaignIncidentEffects.DisorganizeParty());
			list38.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list38.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -10, null));
			list38.Add(CampaignIncidentEffects.GoldChange(() => 1000).WithChance(0.01f));
			incident32.AddOption(text37, list38, null, null);
			incident32.AddOption("{=KQhA1bQq}Tell him that the Kannic sorcerors sealed jinn into their treasure chambers with the power to turn a man's skin inside-out, and you couldn't possibly let him go", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100)
			}, null, null);
			Incident incident33 = this.RegisterIncident("incident_solider_is_wanted_criminal", "{=eN5vqUuF}Soldier is Wanted Criminal", "{=z4RgNBkq}On your way out of the village, a delegation led by the elder approaches you. They point to a {TROOP_TYPE} in your ranks - a good warrior, but none too outgoing. They say he used to farm in this village, but fled after he killed his neighbor in a quarrel over boundary stones. The accused snaps back with a sudden vehemence - his victim was a cheat, and deserved what he got. Under the law, murder requires the death penalty, though custom allows blood money to be paid instead.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				CharacterObject characterObject5 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetWantedCriminalTroop|18_129();
				if (characterObject5 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOP_TYPE", characterObject5.Name);
				return (MobileParty.MainParty.LastVisitedSettlement.MapFaction.StringId == "empire" || MobileParty.MainParty.LastVisitedSettlement.MapFaction.StringId == "vlandia") && MobileParty.MainParty.MemberRoster.TotalRegulars >= 40;
			});
			string text38 = "{=HXj3itpQ}Your {TROOP_TYPE} confessed. There is no option but to let the law take its course";
			List<IncidentEffect> list39 = new List<IncidentEffect>();
			list39.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 300));
			list39.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list39.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => x.Character == IncidentsCampaignBehaviour.<InitializeIncidents>g__GetWantedCriminalTroop|18_129(), () => 1, false));
			list39.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 10, null));
			incident33.AddOption(text38, list39, delegate(TextObject text)
			{
				CharacterObject characterObject6 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetWantedCriminalTroop|18_129();
				text.SetTextVariable("TROOP_TYPE", characterObject6.Name);
				return true;
			}, null);
			string text39 = "{=XvgESxLA}Tell the villagers that they will have to accept blood money, even though they would prefer vengeance";
			List<IncidentEffect> list40 = new List<IncidentEffect>();
			list40.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list40.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list40.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list40.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -5, null));
			list40.Add(CampaignIncidentEffects.GoldChange(() => -300));
			incident33.AddOption(text39, list40, null, null);
			string text40 = "{=OjPetQa7}Tell the villagers that they will need to wait until the next life for justice, because you need your fighter in this one";
			List<IncidentEffect> list41 = new List<IncidentEffect>();
			list41.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list41.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list41.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -10, null));
			incident33.AddOption(text40, list41, null, null);
			Incident incident34 = this.RegisterIncident("incident_soldier_in_debt", "{=GDHHAyQ5}Soldier in Debt", "{=brV8zgmJ}As you leave town, a merchant approaches you. He shows you a document indicating that one of your {TROOP_TYPE} took out a loan, to be repaid on departure with a quarter part as interest. Your man shrugs, calls the merchant a thief for loaning at such ruinous rates, and says he lost the money gambling anyway.", "LeavingTown", "TroopSettlementRelation", CampaignTime.Days(60f), delegate(TextObject description)
			{
				CharacterObject characterObject7 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetSoldierInDebtTroop|18_139();
				if (characterObject7 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOP_TYPE", characterObject7.Name);
				return Hero.MainHero.Clan.Kingdom != null && !Hero.MainHero.Clan.IsUnderMercenaryService;
			});
			incident34.AddOption("{=z0yA6wmV}Force your soldier to repay the loan with full interest", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			string text41 = "{=qeWTPZge}Tell the merchant that he is lucky not to be chased out of camp for gouging your men";
			List<IncidentEffect> list42 = new List<IncidentEffect>();
			list42.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list42.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list42.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -5, null));
			incident34.AddOption(text41, list42, null, null);
			string text42 = "{=KMb0CHWI}Pay back the loan out of your own purse, even if you might get a reputation for being easily manipulated";
			List<IncidentEffect> list43 = new List<IncidentEffect>();
			list43.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list43.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list43.Add(CampaignIncidentEffects.GoldChange(() => -100));
			list43.Add(CampaignIncidentEffects.MoraleChange(5f));
			incident34.AddOption(text42, list43, null, null);
			Incident incident35 = this.RegisterIncident("incident_apples_from_heaven", "{=DCy85lqp}Apples from Heaven", "{=OBdQ2q3d}As you enter the village, some of your men tell you that they've found a group of what look like wild apple trees in full bloom by a stream. They'd fetch a fine price in the village market. It doesn't look like anyone owns the land, but you know that once they're discovered a half-dozen claimants will press title.", "EnteringVillage", "HuntingForaging", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalRegulars >= 5);
			incident35.AddOption("{=n6gqNNJ0}Tell your men to help themselves.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100)
			}, null, null);
			incident35.AddOption("{=mezu0Pej}Inform the villagers of your discovery", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			Incident incident36 = this.RegisterIncident("incident_tempting_berries", "{=o7kYGTjF}Tempting Berries", "{=OmYT9wnV}Your party stumbles upon some bushes with ripe berries near a village, allowing for a refreshing break during the journey. You haven't seen berries quite like these before, but they do look tasty.", "LeavingVillage", "HuntingForaging", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalRegulars >= 5);
			incident36.AddOption("{=xSoKTeLL}Allow your men to collect some berries from the bushes", new List<IncidentEffect> { CampaignIncidentEffects.Select(CampaignIncidentEffects.Group(new IncidentEffect[]
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.MoraleChange(5f)
			}), CampaignIncidentEffects.WoundTroopsRandomly(3), 0.2f) }, null, null);
			incident36.AddOption("{=E8IpjMAw}The bushes may belong to someone. Leave them to the local farmers", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident36.AddOption("{=y0Ior5zV}If they are ripe why aren't they are already eaten by animals or collected? They might be poisonous. Avoid them", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.MoraleChange(-1f)
			}, null, null);
			Incident incident37 = this.RegisterIncident("incident_hunters_paradise", "{=zQ2rtAQE}Hunter's Paradise", "{=fjWw8D1x}Your men spot several deer on the outskirts of the village, a rare thing in a land where meat and hides are valuable. The villagers tell you that there was an unusual crop of acorns this year, while fear of bandits has prevented them from hunting as they normally would. They invite you to shoot a few deer, as there will be plenty to go around anyway and the beasts are trampling the crops.", "EnteringVillage", "HuntingForaging", CampaignTime.Years(1000f), (TextObject description) => PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			string text43 = "{=E659QFHp}Free meat, a bit of fun and practice for your men. Sound the hunting horn.";
			List<IncidentEffect> list44 = new List<IncidentEffect>();
			list44.Add(CampaignIncidentEffects.DisorganizeParty());
			list44.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list44.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("meat"), () => 3));
			list44.Add(CampaignIncidentEffects.PartyExperienceChance(200));
			list44.Add(CampaignIncidentEffects.WoundTroopsRandomly(2).WithChance(0.5f));
			incident37.AddOption(text43, list44, null, null);
			string text44 = "{=jON8A2UY}A bunch of loud amateur hunters will scare the game and probably injure themselves. Let your scout bag an animal or two, while the rest of your party marches ahead.";
			List<IncidentEffect> list45 = new List<IncidentEffect>();
			list45.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("meat"), () => 2));
			list45.Add(CampaignIncidentEffects.DisorganizeParty());
			incident37.AddOption(text44, list45, null, null);
			incident37.AddOption("{=5rjUJaW4}Hunting requires patience. You've got real battles to fight.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100) }, null, null);
			Incident incident38 = this.RegisterIncident("incident_boar_in_a_thicket", "{=Fa0J7a6M}Boar in a Thicket", "{=sQA7Z3Pp}As you set out from the village, a local hunter tells you that he can lead you to a nest made by a particularly large boar in a nearby thicket. The animal is a threat to the village and too fierce for him, he says. It already killed two of his dogs. Your men could probably get it and claim a meal of fresh pork, though not without risk.", "LeavingVillage", "HuntingForaging", CampaignTime.Days(60f), (TextObject description) => PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			Incident incident39 = incident38;
			string text45 = "{=chZrxxe1}The hunt is on! Surround the thicket with spearmen and drive the boar out";
			list4 = new List<IncidentEffect>();
			List<IncidentEffect> list46 = list4;
			IncidentEffect[] array5 = new IncidentEffect[3];
			array5[0] = CampaignIncidentEffects.MoraleChange(5f);
			array5[1] = CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("meat"), () => 3);
			array5[2] = CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100);
			list46.Add(CampaignIncidentEffects.Select(CampaignIncidentEffects.Group(array5), CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 1, false), 0.75f));
			incident39.AddOption(text45, list4, null, null);
			incident38.AddOption("{=Hl2EsIob}Leave the animal be", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100) }, null, null);
			Incident incident40 = this.RegisterIncident("incident_broken_wagon", "{=8FobMTCH}Broken-down Wagon", "{=FJzoOKIU}You come across a trader whose wagon has gotten stuck in the road after his draught horse went lame. He is desperate to sell his wares before they spoil, and offers them at a bargain price.", "LeavingEncounter", "TroopSettlementRelation", CampaignTime.Days(60f), delegate(TextObject description)
			{
				if (!MobileParty.MainParty.IsCurrentlyAtSea)
				{
					return MobileParty.MainParty.ItemRoster.Any<ItemRosterElement>((ItemRosterElement x) => x.EquipmentElement.Item.HasHorseComponent || x.EquipmentElement.Item.StringId == "mule");
				}
				return false;
			});
			string text46 = "{=ywaqOBG6}Give some draught animal to him so that he can continue on his way.";
			List<IncidentEffect> list47 = new List<IncidentEffect>();
			list47.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list47.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetBrokenWagonDraughtAnimalItem|18_156), () => -1));
			incident40.AddOption(text46, list47, null, null);
			string text47 = "{=1bobtmxl}Sell him your cheapest horse so that he can continue.";
			List<IncidentEffect> list48 = new List<IncidentEffect>();
			list48.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list48.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list48.Add(CampaignIncidentEffects.GoldChange(() => 200));
			list48.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetBrokenWagonCheapestHorseItem|18_157), () => -1));
			incident40.AddOption(text47, list48, null, null);
			string text48 = "{=buCYa33Q}Buy his wares at bargain price.";
			List<IncidentEffect> list49 = new List<IncidentEffect>();
			list49.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list49.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list49.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("cotton"), () => 1));
			list49.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("wine"), () => 1));
			list49.Add(CampaignIncidentEffects.GoldChange(() => -100));
			incident40.AddOption(text48, list49, null, null);
			incident40.AddOption("{=wIakrAb}Explain to him that you cannot help.", new List<IncidentEffect>(), null, null);
			Incident incident41 = this.RegisterIncident("incident_honor_the_slain_foe", "{=bGxsp5Le}Honor the Slain Foe", "{=lFaBQPxk}Your {CULTURE_NAME} troops ask if they can take charge of the {CULTURE_BURIAL_STYLE} of the {CULTURE_NAME} in the enemy's ranks, declaring that they feel a duty to their kin even if they fought in a bad cause.", "LeavingBattle", "PostBattle", CampaignTime.Days(60f), delegate(TextObject description)
			{
				TroopRosterElement troopRosterElement = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetHonorTheSlainFoeRandomTroop|18_170();
				if (troopRosterElement.Character == null)
				{
					return false;
				}
				description.SetTextVariable("CULTURE_NAME", troopRosterElement.Character.Culture.Name);
				string stringId = troopRosterElement.Character.Culture.StringId;
				if (stringId == "nord" || stringId == "battania" || stringId == "khuzait")
				{
					description.SetTextVariable("CULTURE_BURIAL_STYLE", "{=LntPXkbN}cremation");
				}
				else
				{
					description.SetTextVariable("CULTURE_BURIAL_STYLE", "{=AbdmPy2k}burial");
				}
				return true;
			});
			incident41.AddOption("{=9mHMjIqd}Refuse, knowing that separate rituals will breed suspicion among the different cultures in your ranks", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident41.AddOption("{=ssAwOnse}Grant their request, as there is no shame in honoring a valiant foe", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.MoraleChange(10f)
			}, null, null);
			Incident incident42 = this.RegisterIncident("incident_local_hero", "{=lfYEujfb}Local Hero", "{=9ab6HwCS}As you prepare to leave the village, the elder approaches you. His nephew is to be married, and it would be a great honor if a warrior of your renown would bless the couple. Your men suppress grins and jostle each other, and you know they are looking forward to the chance to get royally drunk.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Days(60f), (TextObject description) => Hero.MainHero.Clan.Renown >= 500f && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident42.AddOption("{=Huu4DySE}Accept the invitation and let the men have their fun", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			string text49 = "{=LamSdYjF}Accept the invitation, offering a handsome gift to the couple";
			List<IncidentEffect> list50 = new List<IncidentEffect>();
			list50.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list50.Add(CampaignIncidentEffects.GoldChange(() => -100));
			list50.Add(CampaignIncidentEffects.MoraleChange(10f));
			list50.Add(CampaignIncidentEffects.DisorganizeParty());
			incident42.AddOption(text49, list50, null, null);
			incident42.AddOption("{=Pw3qq9nw}Apologize, even though it will disappoint your men", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			Incident incident43 = this.RegisterIncident("incident_false_money", "{=Cba9zsDh}False Money", "{=FKkG15p7}As you leave the village, a trader comes up to you and says the \"silver\" denars that some of your men just gave him are just polished iron. You can indeed see some specks of rust. He demands that you pay him in full. It's entirely possible that your men tried to pass off some false coinage, but the trader's \"evidence\" could equally easily have come from his own stock.", "LeavingTown", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalRegulars >= 25);
			incident43.AddOption("{=ZZXWjvQG}Have your men turn out their pockets. If there are any other tin coins there, assume they are at fault.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident43.AddOption("{=lnF2ci30}Tell the merchant that once he accepts currency, it is his fault and his responsibility", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			Incident incident44 = this.RegisterIncident("incident_coin_clipping", "{=IFtXJbfK}Coin Clipping", "{=7fpGI4Jb}Shortly after the loot is distributed, you come across several of your men rubbing the edges of the coins that you paid them against a rock. You recognize what they're doing - clipping, or shaving off a bit of the silver to melt down in the hope that no one will notice.", "LeavingBattle", "PartyCampLife", CampaignTime.Days(60f), (TextObject description) => Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) >= 40 && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident44.AddOption("{=D750cNUH}Turn a blind eye, even though the practice is considered little better than counterfeiting", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100) }, null, null);
			incident44.AddOption("{=EM8bWb1b}Demand that your men stop, and inform any merchants in the next town that they should look very carefully at whatever coins your men give them.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			string text50 = "{=rRVEjUWH}Show them a few tricks from your rogue's repertoire";
			List<IncidentEffect> list51 = new List<IncidentEffect>();
			list51.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -200));
			list51.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list51.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list51.Add(CampaignIncidentEffects.GoldChange(() => 200));
			list51.Add(CampaignIncidentEffects.SkillChange(DefaultSkills.Roguery, 100f));
			incident44.AddOption(text50, list51, null, null);
			Incident incident45 = this.RegisterIncident("incident_ice_march", "{=5LOG5afj}Ice March", "{=rm4ApCIW}Now that the fighting has ended you notice that the weather has turned bitterly cold, and a wind is picking up. Soon after you return to the march, you notice that several of your men have fallen behind. Your men are shivering in the wind, however, and are in no mood to wait. You can march on and hope they catch up to you, or set a slower pace.", "LeavingEncounter", "HardTravel", CampaignTime.Days(60f), delegate(TextObject description)
			{
				MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(MobileParty.MainParty.Position.ToVec2());
				return !MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.MemberRoster.TotalRegulars >= 30 && (weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain || weatherEventInPosition == MapWeatherModel.WeatherEvent.LightRain);
			});
			string text51 = "{=bXoYv9a2}Continue at a normal marching pace";
			List<IncidentEffect> list52 = new List<IncidentEffect>();
			list52.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list52.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => !x.Character.IsHero, () => (int)(0.05f * (float)MobileParty.MainParty.MemberRoster.TotalRegulars), false).WithChance(0.5f));
			incident45.AddOption(text51, list52, null, null);
			incident45.AddOption("{=gxGlWxVC}Slow down until the stragglers can catch up", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200),
				CampaignIncidentEffects.MoraleChange(-2f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			Incident incident46 = this.RegisterIncident("incident_sandstorm_warning", "{=cSa3IFw5}Sandstorm Warning", "{=yJMpjiP7}As you set out again after the battle, you feel a few scattered drops of rain. Then there is a rush of wind, and a wall of dust engulfs you. The blowing grains sting your face, and you can see but a few arms' lengths ahead. You know that it is a simple matter for men to get lost in the notorious sandstorms of the Nahhas, only to be found later dead of thirst or buried in a dune.", "LeavingEncounter", "HardTravel", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.Position.IsValid() && (!MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.MemberRoster.TotalRegulars >= 20) && Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.Position.Face) == TerrainType.Desert);
			incident46.AddOption("{=vzOfEiKx}Wait briefly in the lee of a dune until the wind eases up, then press cautiously ahead", new List<IncidentEffect>
			{
				CampaignIncidentEffects.DisorganizeParty(),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100)
			}, null, null);
			string text52 = "{=HZwt5rVy}Keep marching, even though there is a good chance some of your men might be separated.";
			List<IncidentEffect> list53 = new List<IncidentEffect>();
			list53.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list53.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => !x.Character.IsHero, () => (int)(0.05f * (float)MobileParty.MainParty.MemberRoster.TotalRegulars), false).WithChance(0.5f));
			incident46.AddOption(text52, list53, null, null);
			Incident incident47 = this.RegisterIncident("incident_vintage_of_victory", "{=1cgOQB5K}Vintage of Victory", "{=ZoIXaxj9}Among the loot from your last battle you discover a vat of fine wine from {WINE_SETTLEMENT_NAME}. You know that you could sell it for a considerable sum, but your men just risked their lives and don't seem inclined to pocket silver tomorrow for a drink today.", "LeavingBattle", "PostBattle", CampaignTime.Days(60f), delegate(TextObject description)
			{
				Village seededRandomElement = IncidentHelper.GetSeededRandomElement<Village>(Village.All.Where<Village>((Village x) => x.VillageType == DefaultVillageTypes.VineYard).ToList<Village>(), Campaign.Current.IncidentManager.ActiveIncidentSeed);
				if (seededRandomElement == null)
				{
					return false;
				}
				description.SetTextVariable("WINE_SETTLEMENT_NAME", seededRandomElement.Name);
				return MobileParty.MainParty.MemberRoster.TotalManCount >= 20;
			});
			string text53 = "{=hj27QkT3}Keep it to sell, telling your men that the money will be spent on keeping them safe";
			List<IncidentEffect> list54 = new List<IncidentEffect>();
			list54.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list54.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list54.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("wine"), () => 1));
			incident47.AddOption(text53, list54, null, null);
			incident47.AddOption("{=SdQEKdeo}Smash the top off the amphorae and let them drink their fill.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.MoraleChange(2f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			Incident incident48 = this.RegisterIncident("incident_no_time_to_mourn", "{=GL0jmbbJ}No Time to Mourn", "{=Qrj5hN2k}It is time to bury your dead, but the ground is frozen solid and the wood from nearby trees is too wet to make a proper pyre. You can give the bodies a hasty burial in a nearby gully, even though wolves or other animals may find them, or take the bodies with you to bury them as soon as you can.", "LeavingBattle", "PostBattle", CampaignTime.Days(60f), delegate(TextObject description)
			{
				PlayerBattleEndedLogEntry playerBattleEndedLogEntry = Campaign.Current.LogEntryHistory.FindLastGameActionLog<PlayerBattleEndedLogEntry>((PlayerBattleEndedLogEntry x) => true);
				return playerBattleEndedLogEntry != null && playerBattleEndedLogEntry.PlayerCasualties > 0 && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5;
			});
			incident48.AddOption("{=2GMj6ZaW}Carry the bodies with you until you find dry wood or softer ground", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.MoraleChange(2f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			incident48.AddOption("{=0CCgjMTG}Cover the bodies as best you can and hope for the best", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-2f)
			}, null, null);
			Incident incident49 = this.RegisterIncident("incident_no_mood_for_mercy", "{=X8dgb937}No Mood for Mercy", "{=uXendo01}It was a hard-fought battle, and among your fallen is a young good-natured {TROOP_TYPE} who kept his comrades' spirits up during hard times. Your men’s faces are grim as they go out to collect the wounded of both sides. You can tell that, if they have their way, not many injured enemies will be brought back to your camp still alive as prisoners.", "LeavingBattle", "PostBattle", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				CharacterObject characterObject8 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetNoMoodForMercyRandomTroop|18_190();
				if (characterObject8 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOP_TYPE", characterObject8.Name);
				PlayerBattleEndedLogEntry playerBattleEndedLogEntry2 = Campaign.Current.LogEntryHistory.FindLastGameActionLog<PlayerBattleEndedLogEntry>((PlayerBattleEndedLogEntry x) => true);
				return playerBattleEndedLogEntry2 != null && playerBattleEndedLogEntry2.PlayerCasualties > 0;
			});
			incident49.AddOption("{=5SWqBZSh}Demand that your men respect the customs of war and tend to those who gave quarter", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.MoraleChange(-15f)
			}, null, null);
			string text54 = "{=3CaDxb8R}Remind your men that war is war, the {TROOP_TYPE} knew the risks, and prisoners bring ransoms";
			List<IncidentEffect> list55 = new List<IncidentEffect>();
			list55.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list55.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list55.Add(CampaignIncidentEffects.MoraleChange(-5f));
			incident49.AddOption(text54, list55, delegate(TextObject text)
			{
				CharacterObject characterObject9 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetNoMoodForMercyRandomTroop|18_190();
				text.SetTextVariable("TROOP_TYPE", characterObject9.Name);
				return true;
			}, null);
			string text55 = "{=mBkCqpEj}Tell your men that they can avenge their comrade as they see fit";
			List<IncidentEffect> list56 = new List<IncidentEffect>();
			list56.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list56.Add(CampaignIncidentEffects.MoraleChange(10f));
			list56.Add(CampaignIncidentEffects.RemovePrisonersRandomlyWithPredicate((TroopRosterElement x) => true, 3));
			incident49.AddOption(text55, list56, null, null);
			Incident incident50 = this.RegisterIncident("incident_misplaced_vengeance", "{=fiesVorG}Misplaced Vengeance", "{=i0SwEBdf}Throughout your visit to {VILLAGE}, the villagers were worried that the enemy who sacked {LOOTED_VILLAGE} might be sending patrols their way. As you leave, you find out that their fears were not misplaced. The first thing you see is a flight of ravens in the sky. Your men begin to mutter to themselves, but soon they stumble across a sight that silences them - a pile of burned corpses, stacked by a crossroad, left by the {ENEMY} as their calling card. You press on, but you soon notice that your men are casting an eye towards your prisoners. They may not have been directly involved in what happened in {LOOTED_VILLAGE}, but you doubt your men care much.", "LeavingVillage", "PostBattle", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				if (!MobileParty.MainParty.LastVisitedSettlement.IsVillage)
				{
					return false;
				}
				Village village = MobileParty.MainParty.LastVisitedSettlement.Village;
				Village lootedVillage = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetMisplacedVengeanceLootedVillage|18_196();
				if (lootedVillage == null)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE", village.Name);
				description.SetTextVariable("LOOTED_VILLAGE", lootedVillage.Name);
				description.SetTextVariable("ENEMY", lootedVillage.Settlement.MapFaction.Name);
				if (MobileParty.MainParty.LastVisitedSettlement.MapFaction == Hero.MainHero.MapFaction && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5)
				{
					return (from x in MobileParty.MainParty.PrisonRoster.GetTroopRoster()
						where x.Character.Culture == lootedVillage.Settlement.Culture && !x.Character.IsHero
						select x).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) > 5;
				}
				return false;
			});
			string text56 = "{=SfO3uwdX}Take a brief walk to clear your head, and when you come back to find the prisoners gone, do not ask questions";
			List<IncidentEffect> list57 = new List<IncidentEffect>();
			list57.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list57.Add(CampaignIncidentEffects.MoraleChange(5f));
			list57.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				Village lootedVillage = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetMisplacedVengeanceLootedVillage|18_196();
				CampaignIncidentEffects.RemovePrisonersRandomlyWithPredicate(delegate(TroopRosterElement x)
				{
					CultureObject culture = x.Character.Culture;
					Village lootedVillage3 = lootedVillage;
					return culture == ((lootedVillage3 != null) ? lootedVillage3.Settlement.Culture : null);
				}, 5).Consequence();
				TextObject textObject = new TextObject("{=vyH9slae}Lost {AMOUNT} {?AMOUNT > 1}prisoners{?}prisoner{\\?} of {CULTURE} culture", null).SetTextVariable("AMOUNT", 5);
				string text170 = "CULTURE";
				Village lootedVillage2 = lootedVillage;
				TextObject textObject2 = textObject.SetTextVariable(text170, (lootedVillage2 != null) ? lootedVillage2.Settlement.Culture.Name : null);
				return new List<TextObject> { textObject2 };
			}, delegate(IncidentEffect effect)
			{
				Village village2 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetMisplacedVengeanceLootedVillage|18_196();
				return new IncidentHint(new TextObject("{=gejYla80}Lose {AMOUNT} {?AMOUNT > 1}prisoners{?}prisoner{\\?} of {CULTURE} culture", null).SetTextVariable("AMOUNT", 5).SetTextVariable("CULTURE", (village2 != null) ? village2.Settlement.Culture.Name : null), IncidentHintType.Effect);
			}));
			incident50.AddOption(text56, list57, null, null);
			incident50.AddOption("{=qljKyPrR}Tell your men that the prisoners must be kept safe and that you will hold them responsible for anything that happens", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-2f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			Incident incident51 = this.RegisterIncident("incident_sleeping_sentry", "{=qFa4Oik0}Sleeping Sentry", "{=gQigaCkD}One of your {RECRUIT} fell asleep on guard. He was new to your company and not popular, and your men are angry. He put their lives at risk, and they demand he be turned over to them to face the ancient Calradic penalty - to run the gauntlet of men with sticks who will beat him, an ordeal that frequently ends in death.", "LeavingEncounter", "PartyCampLife", CampaignTime.Days(60f), delegate(TextObject description)
			{
				CharacterObject characterObject10 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSleepingSentryTroop|18_203();
				if (characterObject10 == null)
				{
					return false;
				}
				description.SetTextVariable("RECRUIT", characterObject10.Name);
				return !MobileParty.MainParty.IsCurrentlyAtSea && MobileParty.MainParty.MemberRoster.TotalRegulars >= 35;
			});
			string text57 = "{=oIknbpZl}Force the recruit to run the gauntlet";
			List<IncidentEffect> list58 = new List<IncidentEffect>();
			list58.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list58.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list58.Add(CampaignIncidentEffects.MoraleChange(5f));
			list58.Add(CampaignIncidentEffects.Select(CampaignIncidentEffects.WoundTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSleepingSentryTroop|18_203), 1), CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => x.Character.Tier <= 2, () => 1, false), 0.5f));
			incident51.AddOption(text57, list58, null, null);
			incident51.AddOption("{=0lqAKHfU}Tell your men to be forgiving, as they were once recruits themselves", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.MoraleChange(-10f)
			}, null, null);
			Incident incident52 = this.RegisterIncident("incident_trade_proposal", "{=IaUhqDw6}Trade Proposal", "{=sU4t3v9r}A passing trader offers to buy {GOOD} from your party at well over the price that it would fetch in the nearest town. No doubt some other merchant has fed him bad information on prices, either to put a rival out of business or as a joke.", "LeavingEncounter", "Profit", CampaignTime.Days(60f), delegate(TextObject description)
			{
				ItemRosterElement itemRosterElement = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetTradeProposalGood|18_208();
				if (itemRosterElement.EquipmentElement.Item == null)
				{
					return false;
				}
				description.SetTextVariable("GOOD", itemRosterElement.EquipmentElement.Item.Name);
				if (!MobileParty.MainParty.IsCurrentlyAtSea)
				{
					return MobileParty.MainParty.ItemRoster.Any<ItemRosterElement>((ItemRosterElement x) => x.Amount > 0);
				}
				return false;
			});
			string text58 = "{=b9S5bNbq}As a merchant he should know his trade. Keep silent and take the offer";
			List<IncidentEffect> list59 = new List<IncidentEffect>();
			list59.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list59.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				ItemRosterElement itemRosterElement2 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetTradeProposalGood|18_208();
				MobileParty.MainParty.ItemRoster.AddToCounts(itemRosterElement2.EquipmentElement, -1);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, itemRosterElement2.EquipmentElement.ItemValue * 2, false);
				TextObject textObject3 = new TextObject("{=JmG1KrTw}Lost 1 {GOOD} and gained {AMOUNT}{GOLD_ICON}", null);
				textObject3.SetTextVariable("GOOD", itemRosterElement2.EquipmentElement.Item.Name);
				textObject3.SetTextVariable("AMOUNT", itemRosterElement2.EquipmentElement.ItemValue * 2);
				return new List<TextObject> { textObject3 };
			}, delegate(IncidentEffect effect)
			{
				ItemRosterElement itemRosterElement3 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetTradeProposalGood|18_208();
				TextObject textObject4 = new TextObject("{=baqpnA0F}Lose 1 {GOOD}, gain {AMOUNT}{GOLD_ICON}", null);
				textObject4.SetTextVariable("GOOD", itemRosterElement3.EquipmentElement.Item.Name);
				textObject4.SetTextVariable("AMOUNT", itemRosterElement3.EquipmentElement.ItemValue * 2);
				return new IncidentHint(textObject4, IncidentHintType.Effect);
			}));
			incident52.AddOption(text58, list59, null, null);
			incident52.AddOption("{=0Wwwa77J}Tell the trader that you have no wish to profit from his ignorance", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100)
			}, null, null);
			string text59 = "{=QWHwvRtC}As a fool and his money are soon parted, you'll do him a favor by saving him some time. Haggle for more";
			List<IncidentEffect> list60 = new List<IncidentEffect>();
			list60.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list60.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				ItemRosterElement itemRosterElement4 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetTradeProposalGood|18_208();
				MobileParty.MainParty.ItemRoster.AddToCounts(itemRosterElement4.EquipmentElement, -1);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, itemRosterElement4.EquipmentElement.ItemValue * 3, false);
				TextObject textObject5 = new TextObject("{=JmG1KrTw}Lost 1 {GOOD} and gained {AMOUNT}{GOLD_ICON}", null);
				textObject5.SetTextVariable("GOOD", itemRosterElement4.EquipmentElement.Item.Name);
				textObject5.SetTextVariable("AMOUNT", itemRosterElement4.EquipmentElement.ItemValue * 3);
				return new List<TextObject> { textObject5 };
			}, delegate(IncidentEffect effect)
			{
				ItemRosterElement itemRosterElement5 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetTradeProposalGood|18_208();
				TextObject textObject6 = new TextObject("{=baqpnA0F}Lose 1 {GOOD}, gain {AMOUNT}{GOLD_ICON}", null);
				textObject6.SetTextVariable("GOOD", itemRosterElement5.EquipmentElement.Item.Name);
				textObject6.SetTextVariable("AMOUNT", itemRosterElement5.EquipmentElement.ItemValue * 3);
				return new IncidentHint(textObject6, IncidentHintType.Effect);
			}));
			incident52.AddOption(text59, list60, null, null);
			Incident incident53 = this.RegisterIncident("incident_singing_for_supper", "{=9uaAUCas}Singing for Supper", "{=iV57q3Tm}A strolling minstrel offers to compose an epic ballad about your heroic deeds, potentially increasing your renown for a little amount of coin. Do you accept his offer, and if so, do you offer him any guidance on what to sing?", "LeavingTown", "DreamsSongsAndSigns", CampaignTime.Days(60f), (TextObject description) => true);
			string text60 = "{=AxGaZNry}Let him do it and reward him generously, but tell him to describe your losses and faults as well";
			List<IncidentEffect> list61 = new List<IncidentEffect>();
			list61.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200));
			list61.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list61.Add(CampaignIncidentEffects.RenownChange(20f));
			list61.Add(CampaignIncidentEffects.InfluenceChange(-100f));
			incident53.AddOption(text60, list61, null, null);
			incident53.AddOption("{=m35ezdZG}You don't need a bard to recount your heroic deeds", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100) }, null, null);
			string text61 = "{=bJBLAnrc}Shower him with gold and tell him that he should feel free to embellish your story as he sees fit";
			List<IncidentEffect> list62 = new List<IncidentEffect>();
			list62.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list62.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list62.Add(CampaignIncidentEffects.GoldChange(() => -1000));
			list62.Add(CampaignIncidentEffects.RenownChange(20f));
			list62.Add(CampaignIncidentEffects.InfluenceChange(200f));
			incident53.AddOption(text61, list62, null, null);
			string text62 = "{=HbOFGhTG}Tragic tales are always the most popular. Ensure the bard does not leave out the daemon who sits on your shoulder, your bouts of madness, and the witch who foretold your terrible but heroic death";
			List<IncidentEffect> list63 = new List<IncidentEffect>();
			list63.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -300));
			list63.Add(CampaignIncidentEffects.RenownChange(30f));
			list63.Add(CampaignIncidentEffects.GoldChange(() => -200));
			incident53.AddOption(text62, list63, null, null);
			Incident incident54 = this.RegisterIncident("incident_love_marriage", "{=QpkXLmjE}Love Marriage", "{=Tq4O8vic}One of your {KNIGHT} asks you for a favor. His daughter plans to marry a man from the back alleys of this town with a reputation as a no-good layabout. He has refused permission but asks you to do what you can to insure the two do not elope.", "LeavingTown", "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				CharacterObject character = IncidentHelper.GetSeededRandomElement<TroopRosterElement>(MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__LoveMarriageTroopPredicate|18_220)).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
				if (character == null)
				{
					return false;
				}
				description.SetTextVariable("KNIGHT", character.Name);
				return MobileParty.MainParty.LastVisitedSettlement.IsFortification && MobileParty.MainParty.LastVisitedSettlement.OwnerClan == Hero.MainHero.Clan;
			});
			string text63 = "{=uppCchb0}Let them marry. Love must triumph over a father's pride";
			List<IncidentEffect> list64 = new List<IncidentEffect>();
			list64.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200));
			list64.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list64.Add(CampaignIncidentEffects.KillTroopsRandomly(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__LoveMarriageTroopPredicate|18_220), () => 1, false).WithChance(0.5f));
			incident54.AddOption(text63, list64, null, null);
			incident54.AddOption("{=oQmuxCSk}Do your best to explain to the girl that marriages are affairs of the head, not of the heart, and she had better think of her future and that of her children and not elope.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 200).WithChance(0.5f)
			}, null, null);
			incident54.AddOption("{=GMQROhTM}Order your troops to give the upstart a sound beating. Who does he think he is, the hero of some foolish romantic poem?", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50),
				CampaignIncidentEffects.MoraleChange(5f)
			}, null, null);
			Incident incident55 = this.RegisterIncident("incident_riddles_on_the_road", "{=bBd0jAvo}Riddles on the Road", "{=S2PV4Qeb}As you ride out of the village, an old man in rags comes up to you and grasps the halter of your horse. \"You must tell me!\" he barks. \"What is the tale told by the sword?\" Your men gather in to see if you will humor this seemingly mad hermit.", "LeavingVillage", "DreamsSongsAndSigns", CampaignTime.Years(1000f), (TextObject description) => PartyBase.MainParty.MemberRoster.TotalRegulars >= 10);
			incident55.AddOption("{=2dW3cBQy}That steel is forged from iron, and long hard marches breed tough warriors.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident55.AddOption("{=PHo98vjr}That a sword is all you need, because if you own one, you can take whatever else you need from those without.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 200),
				CampaignIncidentEffects.RenownChange(10f)
			}, null, null);
			incident55.AddOption("{=xhuGmF9r}That the force of the blade lies only in the hand that wields it.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100) }, null, null);
			incident55.AddOption("{=yMoBOgxx}\"You tell me,\" you say, with a mad laugh, as you deal him a slice that draws blood.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.RenownChange(20f)
			}, null, null);
			Incident incident56 = this.RegisterIncident("incident_deadly_reputation", "{=XeX5x8jw}A Deadly Reputation", "{=GpUBP10Q}As you leave the village, a merchant who clearly wants your approval shouts out, \"{LOCAL_KING} has slain his thousands, but {PLAYER} his tens of thousands!\"", "LeavingVillage", "DreamsSongsAndSigns", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || MobileParty.MainParty.LastVisitedSettlement.MapFaction.Leader == null)
				{
					return false;
				}
				description.SetTextVariable("LOCAL_KING", MobileParty.MainParty.LastVisitedSettlement.MapFaction.Leader.Name);
				description.SetTextVariable("PLAYER", Hero.MainHero.Name);
				return Hero.MainHero.Clan.Kingdom != null && Hero.MainHero.Clan.Kingdom.Leader != Hero.MainHero;
			});
			string text64 = "{=sxH5NHba}\"I have, and it is but an appetite-whetter before the main meal.\"";
			List<IncidentEffect> list65 = new List<IncidentEffect>();
			list65.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list65.Add(CampaignIncidentEffects.RenownChange(20f));
			list65.Add(CampaignIncidentEffects.HeroRelationChange(() => Hero.MainHero.Clan.Kingdom.Leader, -1));
			incident56.AddOption(text64, list65, null, null);
			string text65 = "{=acBdTQxp}\"All I have done has been in the service of {KING}. I am but his will made flesh.\"";
			List<IncidentEffect> list66 = new List<IncidentEffect>();
			list66.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list66.Add(CampaignIncidentEffects.HeroRelationChange(() => Hero.MainHero.Clan.Kingdom.Leader, 5));
			incident56.AddOption(text65, list66, delegate(TextObject text)
			{
				text.SetTextVariable("KING", Hero.MainHero.Clan.Kingdom.Leader.Name);
				return true;
			}, null);
			incident56.AddOption("{=naVH8uSO}\"Any blood I have shed may have been necessary but I hope that Heaven will forgive me.\"", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100) }, null, null);
			incident56.AddOption("{=iR3ZuTSW}Acknowledge him by howling like an animal and swirling your weapon about, as mad behavior makes for good stories.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -200),
				CampaignIncidentEffects.RenownChange(20f)
			}, null, null);
			Incident incident57 = this.RegisterIncident("incident_intriguing_rumors", "{=iYfbN2yb}Intriguing Rumors", "{=aqZyQ3Ca}A man, dressed as a nobleman's servant, approaches you. \"I hear that you and {RIVALLORD.NAME} aren't on the best of terms. Well, what can you expect from a man who can't manage his own family? I was recently employed at his household, and know that his wife was unusually close to her groom... I can spread the word, if you make it worth my while.\"", "LeavingTown", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				Hero hero = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetIntriguingRumorsRivalLord|18_228();
				if (hero == null)
				{
					return false;
				}
				description.SetCharacterProperties("RIVALLORD", hero.CharacterObject, false);
				return true;
			});
			incident57.AddOption("{=Gjd6nP29}You will not sully the good name of anyone with an obviously false rumor", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100) }, null, null);
			string text66 = "{=jE9aKaYd}People will believe a noble's former servant. This man's favor is worth a purse of silver to you.";
			List<IncidentEffect> list67 = new List<IncidentEffect>();
			list67.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list67.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list67.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list67.Add(CampaignIncidentEffects.InfluenceChange(10f));
			list67.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				Hero hero2 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetIntriguingRumorsRivalLord|18_228();
				if (hero2 != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, hero2, -5, true);
					hero2.AddInfluenceWithKingdom(-10f);
					TextObject textObject7 = new TextObject("{=WkIylUOa}{RIVALLORD.NAME} lost {AMOUNT} influence.", null);
					textObject7.SetCharacterProperties("RIVALLORD", hero2.CharacterObject, false);
					textObject7.SetTextVariable("AMOUNT", 10);
					return new List<TextObject> { textObject7 };
				}
				return new List<TextObject>();
			}, delegate(IncidentEffect effect)
			{
				Hero hero3 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetIntriguingRumorsRivalLord|18_228();
				TextObject textObject8 = new TextObject("{=NQKDUIGg}{RIVALLORD.NAME} loses {AMOUNT} influence", null);
				textObject8.SetCharacterProperties("RIVALLORD", hero3.CharacterObject, false);
				textObject8.SetTextVariable("AMOUNT", 10);
				return new IncidentHint(textObject8, IncidentHintType.Effect);
			}));
			incident57.AddOption(text66, list67, null, null);
			string text67 = "{=bZUMOAjU}Crack a few jokes to your men at {RIVALLORD.NAME}'s expense, but otherwise do nothing";
			List<IncidentEffect> list68 = new List<IncidentEffect>();
			list68.Add(CampaignIncidentEffects.MoraleChange(5f));
			incident57.AddOption(text67, list68, delegate(TextObject text)
			{
				Hero hero4 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetIntriguingRumorsRivalLord|18_228();
				text.SetCharacterProperties("RIVALLORD", hero4.CharacterObject, false);
				return true;
			}, null);
			Incident incident58 = this.RegisterIncident("incident_charitable_acts_recognition", "{=pjy9Z4bt}Charitable Acts Recognition", "{=vaez5WXH}As you leave town, you see a much larger number of beggars gathered than you would expect. Some of their limps look unconvincing. Your reputation for compassion and generosity may have gotten around.", "LeavingTown", "PlightOfCivilians", CampaignTime.Days(60f), (TextObject description) => Hero.MainHero.GetTraitLevel(DefaultTraits.Generosity) > 1 || Hero.MainHero.GetTraitLevel(DefaultTraits.Mercy) > 1);
			string text68 = "{=DFIPsN1W}If they come to you in need, it is not for you to question them. Have your men distribute alms as usual.";
			List<IncidentEffect> list69 = new List<IncidentEffect>();
			list69.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list69.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200));
			list69.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			incident58.AddOption(text68, list69, null, null);
			string text69 = "{=CKKIJZcB}Have some of your men boil up a soup. Keep a watchful eye on all the beggars, especially those who move too vigorously to the front of the line, and serve only those who seem genuinely too frail to work.";
			List<IncidentEffect> list70 = new List<IncidentEffect>();
			list70.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list70.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list70.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list70.Add(CampaignIncidentEffects.DisorganizeParty());
			incident58.AddOption(text69, list70, null, null);
			incident58.AddOption("{=T1pwrLN9}Tell them that you are no chicken to be plucked. You will reward neither liars nor those who keep company with liars.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			Incident incident59 = this.RegisterIncident("incident_the_pavillon", "{=rG4b9Tft}The Pavillon", "{=qBsa5KC5}As you leave town you come across an encampment of traveling entertainers. You see tattered but colorful tents, you hear wailing but enticing music, and smell the scent of cheap incense and meat that is probably slightly spoiled but spiced so that you would never notice. A group of women in some noble's thrown-away silks, their faces made up skillfully with charcoal and sheep's blood, beckon to your men. What do you do?", "LeavingTown", "TroopSettlementRelation", CampaignTime.Days(60f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				CultureObject culture2 = MobileParty.MainParty.LastVisitedSettlement.Culture;
				return PartyBase.MainParty.MemberRoster.TotalRegulars >= 5 && culture2 != null && (culture2.StringId == "aserai" || culture2.StringId == "khuzait" || culture2.StringId == "empire_s");
			});
			incident59.AddOption("{=lPAC4x1e}Tell your men to steer clear of such dubious enticements", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident59.AddOption("{=xYuNxo3v}Let your men have some fun, but tell them to watch their purses and stay out of fights", new List<IncidentEffect>
			{
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.WoundTroopsRandomly(1).WithChance(0.5f),
				CampaignIncidentEffects.DisorganizeParty()
			}, null, null);
			string text70 = "{=mmlk2tXh}The fun is on you! Hurl a fistful of silver at the entertainers and tell them your men deserve nothing but the best";
			List<IncidentEffect> list71 = new List<IncidentEffect>();
			list71.Add(CampaignIncidentEffects.MoraleChange(5f));
			list71.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list71.Add(CampaignIncidentEffects.GoldChange(() => -100));
			incident59.AddOption(text70, list71, null, null);
			string text71 = "{=lzK4PmYL}Rob the entertainers. No doubt their wealth is ill-gotten, and to whom will they complain?";
			List<IncidentEffect> list72 = new List<IncidentEffect>();
			list72.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list72.Add(CampaignIncidentEffects.GoldChange(() => 200));
			incident59.AddOption(text71, list72, null, null);
			Incident incident60 = this.RegisterIncident("incident_abundance_of_troublemakers", "{=GFGuSmN9}Abundance of troublemakers", "{=mZir8Ogn}As you prepare to leave, the village elder approaches you. There's a gang of four lads who are constantly picking fights with the other villagers, he says. Even their own families don't want them around for fear they'll end up killing someone and cause a blood feud. He begs you to take them into your ranks, where they can learn a bit of discipline.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null);
			string text72 = "{=ePstZcmu}Accept the recruits. The first chance you get, find fault with their gear and give them a sound thrashing, so they'll know to mend their ways.";
			List<IncidentEffect> list73 = new List<IncidentEffect>();
			list73.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list73.Add(CampaignIncidentEffects.ChangeTroopAmount(() => MobileParty.MainParty.LastVisitedSettlement.Culture.BasicTroop, 4, null, null));
			incident60.AddOption(text72, list73, null, null);
			string text73 = "{=Dogg1b9a}Take the recruits. Try not to be too hard on them at first, even though they'll probably corrode your party's discipline.";
			List<IncidentEffect> list74 = new List<IncidentEffect>();
			list74.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list74.Add(CampaignIncidentEffects.MoraleChange(-10f));
			list74.Add(CampaignIncidentEffects.ChangeTroopAmount(() => MobileParty.MainParty.LastVisitedSettlement.Culture.BasicTroop, 4, null, null));
			incident60.AddOption(text73, list74, null, null);
			incident60.AddOption("{=EIbmlJRP}Tell the elder he'll need to find some other way to handle his troublemakers.", new List<IncidentEffect>(), null, null);
			Incident incident61 = this.RegisterIncident("incident_ennoble_a_fighter", "{=wLozeWS9}Ennoble a fighter", "{=FPnUWAj1}After the battle, one of your {TROOPS} from your party approaches you, dreamed of riding with your cavalry as a {NOBLE_TIER}. He has the money to buy his own horse, and he certainly distinguished himself in the fight. You know however that this will go down badly - with the high-born cavalry, who prefer to mingle with their own class, but probably even more so with his comrades, who will think he scorns them.", "LeavingBattle", "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				CharacterObject characterObject11 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetRegularTroop|18_244();
				if (characterObject11 == null)
				{
					return false;
				}
				CharacterObject characterObject12 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetNobleTroop|18_245(characterObject11.Culture);
				if (characterObject12 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOPS", characterObject11.Name);
				description.SetTextVariable("NOBLE_TIER", characterObject12.Name);
				return true;
			});
			Incident incident62 = incident61;
			string text74 = "{=HpAbeiKP}Courage and skill deserve a reward, even if the result is not what you get";
			List<IncidentEffect> list75 = new List<IncidentEffect>();
			list75.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list75.Add(CampaignIncidentEffects.MoraleChange(-10f));
			list75.Add(CampaignIncidentEffects.UpgradeTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetRegularTroop|18_244), new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetNobleTroopForUpgrade|18_246), 1, () => Campaign.Current.IncidentManager.ActiveIncidentSeed));
			incident62.AddOption(text74, list75, null, null);
			Incident incident63 = incident61;
			string text75 = "{=oL1qlaxH}Explain to him your reservations, even though you can tell he won't like to hear them, and let him make up his own mind";
			list4 = new List<IncidentEffect>();
			list4.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list4.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			List<IncidentEffect> list76 = list4;
			IncidentEffect[] array6 = new IncidentEffect[2];
			array6[0] = CampaignIncidentEffects.MoraleChange(-5f);
			array6[1] = CampaignIncidentEffects.UpgradeTroop(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetRegularTroop|18_244), new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetNobleTroopForUpgrade|18_246), 1, () => Campaign.Current.IncidentManager.ActiveIncidentSeed);
			list76.Add(CampaignIncidentEffects.Select(CampaignIncidentEffects.Group(array6), CampaignIncidentEffects.Custom(null, () => new List<TextObject>(), (IncidentEffect effect) => new IncidentHint(new TextObject("{=lobJVVWT}Nothing happens", null), IncidentHintType.Effect)), 0.5f));
			incident63.AddOption(text75, list4, null, null);
			incident61.AddOption("{=uJYOhEx8}Commend him, but tell him to stick with his comrades", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			Incident incident64 = this.RegisterIncident("incident_family_claims_bandit", "{=6IA1S2XU}Family Claims Bandit", "{=BG8Ph5La}As you leave {VILLAGE_NAME}, a woman approaches you and grasps your horse's halter. Among your prisoners is her son, she says. You captured him when he was part of a gang of bandits, but she tells you tearfully that he is a good boy who fell in with bad company. She swears she'll ensure that he doesn't stray again from the correct path.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				return MobileParty.MainParty.PrisonRoster.TotalRegulars > 0 && MobileParty.MainParty.PrisonRoster.GetTroopRoster().Any<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__BanditPrisonerPredicate|18_254));
			});
			incident64.AddOption("{=QDbl4lB7}Release your captive, even if some will mutter that you have a soft heart and a soft head", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.RemovePrisonersRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__BanditPrisonerPredicate|18_254), 1)
			}, null, null);
			incident64.AddOption("{=9yrWWbHF}Tell the woman that her son shall face the same penalties as any other bandit you caught", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100)
			}, null, null);
			string text76 = "{=6sAI5uOm}Tell the village that they must pay the lad's ransom, so that he will know there is no profit in banditry";
			List<IncidentEffect> list77 = new List<IncidentEffect>();
			list77.Add(CampaignIncidentEffects.GoldChange(() => 100));
			list77.Add(CampaignIncidentEffects.RemovePrisonersRandomlyWithPredicate(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__BanditPrisonerPredicate|18_254), 1));
			list77.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list77.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50));
			incident64.AddOption(text76, list77, null, null);
			Incident incident65 = this.RegisterIncident("incident_turn_pike", "{=kl13BJZB}Turnpike", "{=VauaVUNS}You notice a footpath leading through your estate which could be quickly improved to be used by wagons and be a shortcut between the main trade roads. You could charge tolls and turn it into a turnpike, though the locals would resent the increased traffic and fees on their customary route.", new string[] { "LeavingCastle", "LeavingTown" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsFortification || MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Clan.PlayerClan)
				{
					return false;
				}
				Building building = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTurnPikeBuilding|18_257();
				return building != null && building.CurrentLevel == building.BuildingType.StartLevel;
			});
			string text77 = "{=aF4Q5TJc}Widen the road for wagons but charge tolls to use it";
			List<IncidentEffect> list78 = new List<IncidentEffect>();
			list78.Add(CampaignIncidentEffects.BuildingLevelChange(new Func<Building>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTurnPikeBuilding|18_257), () => 1));
			list78.Add(CampaignIncidentEffects.TownBoundVillageRelationChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, -10));
			list78.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			incident65.AddOption(text77, list78, null, null);
			string text78 = "{=xxVE2bw1}Improve the road, but charge no tolls";
			List<IncidentEffect> list79 = new List<IncidentEffect>();
			list79.Add(CampaignIncidentEffects.BuildingLevelChange(new Func<Building>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTurnPikeBuilding|18_257), () => 1));
			list79.Add(CampaignIncidentEffects.TownBoundVillageRelationChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 10));
			list79.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list79.Add(CampaignIncidentEffects.GoldChange(() => -1000));
			incident65.AddOption(text78, list79, null, null);
			incident65.AddOption("{=wcbbBjcD}Let it be", new List<IncidentEffect>(), null, null);
			Incident incident66 = this.RegisterIncident("incident_endebted_farmers", "{=Vbs9aocS}Endebted Farmers", "{=cRMxKTa8}Several freeholding farmers in your fief have fallen into debt through what appears to be a combination of bad luck and risky investments - a plough horse that died, a crop ruined by frost. They are unable to pay their taxes, and ask for a reprieve for several years.", new string[] { "LeavingCastle", "LeavingTown" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsFortification || MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Clan.PlayerClan)
				{
					return false;
				}
				Building building2 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetEndebtedFarmersBuilding|18_265();
				return building2 != null && building2.CurrentLevel == building2.BuildingType.StartLevel;
			});
			string text79 = "{=OM6tkdD7}You know an opportunity when you see it. Seize their lands. They can farm as your tenants";
			List<IncidentEffect> list80 = new List<IncidentEffect>();
			list80.Add(CampaignIncidentEffects.BuildingLevelChange(new Func<Building>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetEndebtedFarmersBuilding|18_265), () => 1));
			list80.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list80.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			incident66.AddOption(text79, list80, null, null);
			incident66.AddOption("{=jeWayyGY}A reprieve is fine, but you tack on a bit of interest so that you're not overwhelmed with hard luck stories", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100) }, null, null);
			string text80 = "{=WHWokYrT}Call in a favor with a moneylender in town to reduce their debts so they can pay their taxes";
			List<IncidentEffect> list81 = new List<IncidentEffect>();
			list81.Add(CampaignIncidentEffects.GoldChange(() => 1000));
			list81.Add(CampaignIncidentEffects.InfluenceChange(-20f));
			incident66.AddOption(text80, list81, null, null);
			string text81 = "{=3QobDImA}Help them get back on their feet, even if some will call say that you are too trusting";
			List<IncidentEffect> list82 = new List<IncidentEffect>();
			list82.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200));
			list82.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list82.Add(CampaignIncidentEffects.GoldChange(() => -200));
			incident66.AddOption(text81, list82, null, null);
			Incident incident67 = this.RegisterIncident("incident_fertility_festival", "{=o5LpNEIa}Fertility Festival", "{=lfJ6OHgX}The village has seen an unexpected rise in pregnancies. Some of your villagers ask your permission to hold a festival in honor of Agalea, protectress of motherhood. Agalea has never been recognized as a saint by the imperial Senate, and many consider her veneration to be a primitive Palaic holder. Her festivals tend to be rather raucous, befitting the theme of fertility, and also rather expensive. It would certainly set tongues wagging in the district. Do you grant permission?", "LeavingVillage", "FiefManagement", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null && (MobileParty.MainParty.LastVisitedSettlement.IsVillage && MobileParty.MainParty.LastVisitedSettlement.OwnerClan == Hero.MainHero.Clan) && MobileParty.MainParty.LastVisitedSettlement.Culture.StringId == "empire");
			string text82 = "{=EFUNx0cv}A vat of wine for you to dance in honor of the lady Agalea!";
			List<IncidentEffect> list83 = new List<IncidentEffect>();
			list83.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list83.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list83.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list83.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 10, null));
			list83.Add(CampaignIncidentEffects.InfluenceChange(-100f));
			list83.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 20));
			list83.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -10));
			incident67.AddOption(text82, list83, null, null);
			string text83 = "{=NWNIVZF0}Give the villagers a stern lecture on the importance of sound doctrine and lawful worship";
			List<IncidentEffect> list84 = new List<IncidentEffect>();
			list84.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list84.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list84.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -10, null));
			list84.Add(CampaignIncidentEffects.InfluenceChange(100f));
			list84.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 5));
			incident67.AddOption(text83, list84, null, null);
			string text84 = "{=J9BsFeSS}Tell the villagers to keep things discrete";
			List<IncidentEffect> list85 = new List<IncidentEffect>();
			list85.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 10));
			list85.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -5));
			incident67.AddOption(text84, list85, null, null);
			Incident incident68 = this.RegisterIncident("incident_migrant_influx", "{=OY5LLcxv}Migrant Influx", "{=2KaXj3Qa}The depredations of the {NEARBY_ENEMY_FACTION} has driven many farmers from their homes. Those who have land usually return to it, but many of the poorer are seeking a new place to live. You speak to a few encamped on the outskirts of your village. There is no land for them. They seem eager to work as tenants, but there is always a possibility of tensions with the more established village families.", "LeavingVillage", "FiefManagement", CampaignTime.Days(60f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				if (MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Hero.MainHero.Clan)
				{
					return false;
				}
				Kingdom kingdom = Kingdom.All.FirstOrDefault<Kingdom>((Kingdom k) => k.IsAtWarWith(Hero.MainHero.MapFaction));
				if (kingdom == null)
				{
					return false;
				}
				description.SetTextVariable("NEARBY_ENEMY_FACTION", kingdom.Name);
				return MobileParty.MainParty.LastVisitedSettlement.IsVillage && MobileParty.MainParty.LastVisitedSettlement.OwnerClan == Hero.MainHero.Clan;
			});
			string text85 = "{=8JqCDWRd}Welcome them, and give them a donation to purchase small plots of land.";
			List<IncidentEffect> list86 = new List<IncidentEffect>();
			list86.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list86.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list86.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 20));
			list86.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, 10).WithChance(0.25f));
			incident68.AddOption(text85, list86, null, null);
			string text86 = "{=U4UJn9X4}Let them stay. Tell both newcomers and longtime residents to get along, and hope for the best.";
			List<IncidentEffect> list87 = new List<IncidentEffect>();
			list87.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 25));
			list87.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -10).WithChance(0.5f));
			incident68.AddOption(text86, list87, null, null);
			incident68.AddOption("{=o4aQ2UEL}Tell them to make their homes someplace else.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100) }, null, null);
			Incident incident69 = this.RegisterIncident("incident_nomads_wish_to_settle", "{=5vN2NZ8R}Nomads Wish to Settle", "{=U4Ybz08z}A desperate band of nomads has arrived at your village, seeking a place to settle down. Their herds were ravaged by an epidemic, they say, and they are ready to take up farming or work as laborers. Otherwise, they must starve or give themselves to another tribe as servants. The villagers are skeptical. Nomads are too lazy to be farmers, they say, preferring horse theft and feuds.", "LeavingVillage", "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				Village village3 = MobileParty.MainParty.LastVisitedSettlement.Village;
				bool flag3 = new string[] { "khuzait", "aserai", "empire_s" }.Contains(village3.Settlement.Culture.StringId);
				bool flag4 = Hero.MainHero.Clan.Fiefs.Contains(village3.Bound.Town);
				return flag3 && flag4;
			});
			string text87 = "{=iSZCkpt3}Welcome them with open arms and a small donation to buy plots of land for their homes";
			List<IncidentEffect> list88 = new List<IncidentEffect>();
			list88.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list88.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list88.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 20));
			list88.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -10).WithChance(0.25f));
			incident69.AddOption(text87, list88, null, null);
			string text88 = "{=taVbaFFW}Sternly instruct them to respect the traditions of settled life, even though the cold welcome might scare some away";
			List<IncidentEffect> list89 = new List<IncidentEffect>();
			list89.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list89.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list89.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 10));
			incident69.AddOption(text88, list89, null, null);
			incident69.AddOption("{=jUlHTiSU}Agree with the villagers that nomads are more trouble than they are worth.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100) }, null, null);
			Incident incident70 = this.RegisterIncident("incident_conflict_over_commons_both_yours", "{=uqB1fKFY}Conflict Over Commons", "{=raLLq10c}Grazing land has grown scarce near {TOWN_OR_CASTLE_NAME}, and a heath between {POORER_VILLAGE} and {RICHER_VILLAGE}, covered with thistle and formerly of little value, has become a point of contention. {POORER_VILLAGE} claims that they have been taking goats there for generations, but {RICHER_VILLAGE} has produced a document showing legal title.", new string[] { "LeavingCastle", "LeavingTown" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (!MobileParty.MainParty.LastVisitedSettlement.IsFortification || MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Hero.MainHero.Clan)
				{
					return false;
				}
				List<Village> list170 = MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).ToList<Village>();
				Village village4 = list170.LastOrDefault<Village>();
				Village village5 = list170.FirstOrDefault<Village>();
				if (village4 == null || village5 == null || village4 == village5)
				{
					return false;
				}
				float num = Village.All.Sum<Village>((Village x) => x.Hearth) / (float)Village.All.Count;
				if (village4.Hearth < num || village5.Hearth < num)
				{
					return false;
				}
				description.SetTextVariable("TOWN_OR_CASTLE_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				description.SetTextVariable("RICHER_VILLAGE", village4.Name);
				description.SetTextVariable("POORER_VILLAGE", village5.Name);
				return true;
			});
			string text89 = "{=cVzpV96W}The laws of the realm are clear: no matter how many years go by, the original owner retains the land unless it is legally sold or transferred.";
			List<IncidentEffect> list90 = new List<IncidentEffect>();
			list90.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200));
			list90.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).LastOrDefault<Village>(), 5));
			list90.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).FirstOrDefault<Village>(), -10));
			incident70.AddOption(text89, list90, null, null);
			string text90 = "{=3zEtGAII}You agree with the ancient Palaic principle: the lands belong to the spirits of the earth, not to humans, and go to those who make the best use of them.";
			List<IncidentEffect> list91 = new List<IncidentEffect>();
			list91.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list91.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list91.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).LastOrDefault<Village>(), -5));
			list91.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).FirstOrDefault<Village>(), 10));
			incident70.AddOption(text90, list91, null, null);
			string text91 = "{=wlVCpQHS}Have them split it.";
			List<IncidentEffect> list92 = new List<IncidentEffect>();
			list92.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -50));
			list92.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).FirstOrDefault<Village>(), 5));
			incident70.AddOption(text91, list92, null, null);
			string text92 = "{=YWZFOX4S}Let them know that your decision is for sale at a price";
			List<IncidentEffect> list93 = new List<IncidentEffect>();
			list93.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list93.Add(CampaignIncidentEffects.GoldChange(() => 1000));
			list93.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).LastOrDefault<Village>(), 5));
			list93.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town.Villages.OrderBy<Village, float>((Village x) => x.Hearth).FirstOrDefault<Village>(), -10));
			incident70.AddOption(text92, list93, null, null);
			Incident incident71 = this.RegisterIncident("incident_conflict_over_commons", "{=uqB1fKFY}Conflict Over Commons", "{=jGafCYMe}Villagers from your fief of {VILLAGE} have started going to a marsh on the road to {VILLAGE2} to gather reeds for baskets, supplementing their earnings. Hunters from {VILLAGE2}, owned by {LORD.NAME}, say they've been disturbing the local waterfowl. Neither of you has title to the marsh, but {VILLAGE2} says that its people started going there first. Your tenants say it's not their problem if they've figured out a better way to make use of the resources.", new string[] { "LeavingCastle", "LeavingTown" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (!MobileParty.MainParty.LastVisitedSettlement.IsFortification || MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Hero.MainHero.Clan)
				{
					return false;
				}
				Village village6 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVillageOne|18_310();
				Village village7 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetOtherVillage|18_311();
				if (((village7 != null) ? village7.Settlement.Owner : null) == null)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE", village6.Name);
				description.SetTextVariable("VILLAGE2", village7.Name);
				description.SetCharacterProperties("LORD", village7.Settlement.Owner.CharacterObject, false);
				return true;
			});
			string text93 = "{=8gTYsXZM}Continue to send your reed gatherers there, and have them bring staves and slings to defend their rights";
			List<IncidentEffect> list94 = new List<IncidentEffect>();
			list94.Add(CampaignIncidentEffects.SettlementRelationChange(() => IncidentsCampaignBehaviour.<InitializeIncidents>g__GetOtherVillage|18_311().Settlement, -10, null));
			list94.Add(CampaignIncidentEffects.HeroRelationChange(() => IncidentsCampaignBehaviour.<InitializeIncidents>g__GetOtherVillage|18_311().Settlement.OwnerClan.Leader, -10));
			list94.Add(CampaignIncidentEffects.VillageHearthChange(new Func<Village>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVillageOne|18_310), -5).WithChance(0.25f));
			list94.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 20));
			list94.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			incident71.AddOption(text93, list94, null, null);
			string text94 = "{=0xiAZr96}Tell {NOBLE.NAME} that your people will get their reeds elsewhere and stop frightening off the game.";
			List<IncidentEffect> list95 = new List<IncidentEffect>();
			list95.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 5));
			list95.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100));
			list95.Add(CampaignIncidentEffects.HeroRelationChange(() => IncidentsCampaignBehaviour.<InitializeIncidents>g__GetOtherVillage|18_311().Settlement.OwnerClan.Leader, 20));
			incident71.AddOption(text94, list95, delegate(TextObject text)
			{
				Village village8 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetOtherVillage|18_311();
				text.SetCharacterProperties("NOBLE", village8.Settlement.OwnerClan.Leader.CharacterObject, false);
				return true;
			}, null);
			Incident incident72 = this.RegisterIncident("incident_educational_advancements", "{=RZJ0f8qo}Educational Advancements", "{=2OQlwt7x}Villagers say that a wanderer has recently arrived and offered to teach their children to read. He is not only well-educated, but he has a gift for holding the attention of restless 10-year-olds. However, he seems to have spent time with one of the rebel bands that were common around here ten years back, and occasionally speaks of a coming reign of justice brought by Heaven, in a way that doesn't seem like one of his ironic jokes.", "LeavingVillage", "FiefManagement", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null && Hero.MainHero.Clan.Fiefs.Contains(MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town));
			string text95 = "{=Wuga4ZqG}Let him stay. It's good for children to read, and it's not like they take their elders seriously anyway";
			List<IncidentEffect> list96 = new List<IncidentEffect>();
			list96.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -5));
			list96.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, 10));
			list96.Add(CampaignIncidentEffects.InfluenceChange(20f));
			incident72.AddOption(text95, list96, null, null);
			incident72.AddOption("{=zEPpgYaf}Reading won't help a farmboy plough, but might goad him to do foolish things that get him killed", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100) }, null, null);
			string text96 = "{=3OerG5Ik}Take the teacher aside, and tell him the elder will pay him a handsome stipend that will stop abruptly if he breathes a word about politics or theology.";
			List<IncidentEffect> list97 = new List<IncidentEffect>();
			list97.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list97.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, 10));
			list97.Add(CampaignIncidentEffects.GoldChange(() => -200));
			incident72.AddOption(text96, list97, null, null);
			Incident incident73 = this.RegisterIncident("incident_gold_rush", "{=8jG2vlOu}Gold Rush", "{=eQz8b4TH}The village is buzzing with a poorly kept secret. Following a rare but fierce rainstorm in the dry hills, fishermen have begun to find small flakes of gold in a local stream. If word gets out, there will be a huge influx of fortune-seekers into the area. The gold will last a few months, and it's not clear how much of it will benefit the village or its owner.", "LeavingVillage", "FiefManagement", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null && Hero.MainHero.Clan.Fiefs.Contains(MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town));
			string text97 = "{=mEqk8UCG}This region could use more people. Let word get out.";
			List<IncidentEffect> list98 = new List<IncidentEffect>();
			list98.Add(CampaignIncidentEffects.VillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Village, 20));
			list98.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -10));
			list98.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list98.Add(CampaignIncidentEffects.InfestNearbyHideout(() => MobileParty.MainParty.LastVisitedSettlement));
			incident73.AddOption(text97, list98, null, null);
			string text98 = "{=aNGQJJkM}As liege of the village, the gold belongs to you. Post guards along the river to stop unauthorized prospecting";
			List<IncidentEffect> list99 = new List<IncidentEffect>();
			list99.Add(CampaignIncidentEffects.GoldChange(() => 1000));
			list99.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -10, null));
			list99.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200));
			incident73.AddOption(text98, list99, null, null);
			string text99 = "{=cECwyvjY}Encourage the villagers to mine it stealthily, ensuring their cooperation by allowing them to keep it";
			List<IncidentEffect> list100 = new List<IncidentEffect>();
			list100.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list100.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, 100));
			list100.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Town, -5));
			incident73.AddOption(text99, list100, null, null);
			Incident incident74 = this.RegisterIncident("incident_wages_of_war_and_diseases", "{=7q53TLg7}Wages of war and diseases", "{=FZXYHC7s}Between the wars and an epidemic, the villages near {FIEF_NAME} are experiencing a shortage of people, especially young men. Fields are going fallow for lack of hands to plough them, especially those cleared only in recent generations where the title is in dispute. As liege, you can redistribute this unused land, but you will need to find labor.", new string[] { "LeavingCastle", "LeavingTown" }, "FiefManagement", CampaignTime.Days(60f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsFortification || MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Clan.PlayerClan)
				{
					return false;
				}
				Building building3 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetWagesOfWarAndDiseasesBuilding|18_330();
				if (building3 == null)
				{
					return false;
				}
				float num2 = Village.All.Average<Village>((Village x) => x.Hearth);
				if (MobileParty.MainParty.LastVisitedSettlement.BoundVillages.Average<Village>((Village x) => x.Hearth) > num2)
				{
					return false;
				}
				description.SetTextVariable("FIEF_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				return building3.CurrentLevel == building3.BuildingType.StartLevel;
			});
			string text100 = "{=wnvUsbSs}Seize the land as your own, attracting desperate peasants from surrounding districts with exploitative sharecropping agreements";
			List<IncidentEffect> list101 = new List<IncidentEffect>();
			list101.Add(CampaignIncidentEffects.BuildingLevelChange(new Func<Building>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetWagesOfWarAndDiseasesBuilding|18_330), () => 1));
			list101.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list101.Add(CampaignIncidentEffects.TownBoundVillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 10));
			incident74.AddOption(text100, list101, null, null);
			string text101 = "{=8jCcqBSV}Distribute the land to some of your veterans as a reward for their service";
			List<IncidentEffect> list102 = new List<IncidentEffect>();
			list102.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list102.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => !x.Character.IsHero, () => 10, false));
			list102.Add(CampaignIncidentEffects.TownBoundVillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 20));
			incident74.AddOption(text101, list102, null, null);
			string text102 = "{=QSEZfwVk}Distribute money to the region's farmers so they can hire agricultural laborers or provide attractive dowries for their daughters ";
			List<IncidentEffect> list103 = new List<IncidentEffect>();
			list103.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200));
			list103.Add(CampaignIncidentEffects.TownBoundVillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 30));
			list103.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list103.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 10));
			incident74.AddOption(text102, list103, null, null);
			Incident incident75 = this.RegisterIncident("incident_successful_harvest", "{=MaaFv4Po}Successful Harvest", "{=evcKUaab}An early thaw and a warm spring has ensured a bumper crop, which the prayers of the villagers and the efforts of the local cats have protected from rats and birds. Such bounties occur rarely. What will you do?", new string[] { "LeavingTown", "LeavingCastle" }, "FiefManagement", CampaignTime.Days(60f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null && MobileParty.MainParty.LastVisitedSettlement.OwnerClan == Hero.MainHero.Clan && (CampaignTime.Now.GetSeasonOfYear == CampaignTime.Seasons.Summer || CampaignTime.Now.GetSeasonOfYear == CampaignTime.Seasons.Autumn));
			string text103 = "{=HT1mQuCI}Feast! Thank Heaven and the spirits of the land, and make merry";
			List<IncidentEffect> list104 = new List<IncidentEffect>();
			list104.Add(CampaignIncidentEffects.MoraleChange(10f));
			list104.Add(CampaignIncidentEffects.TownBoundVillageRelationChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 5));
			list104.Add(CampaignIncidentEffects.TownBoundVillageHearthChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 10));
			list104.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 5));
			list104.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list104.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			incident75.AddOption(text103, list104, null, null);
			string text104 = "{=c5vobNeb}Encourage the farmers to sell the crops and make much-needed improvements to irrigation and roads";
			List<IncidentEffect> list105 = new List<IncidentEffect>();
			list105.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list105.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 20));
			incident75.AddOption(text104, list105, null, null);
			string text105 = "{=ZaBiC5XV}Go through your accounts to see who might owe you back taxes";
			List<IncidentEffect> list106 = new List<IncidentEffect>();
			list106.Add(CampaignIncidentEffects.GoldChange(() => 1000));
			list106.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200));
			incident75.AddOption(text105, list106, null, null);
			Incident incident76 = this.RegisterIncident("incident_haunted_dreams", "{=99w595qn}Haunted Dreams", "{=yXXbY1bb}You lie down for a well-deserved nap, but you slip into an uneasy dream. You are sailing on a wind-tossed sea. A wave crashes on your bow, sending spray across the decks. You look closely, and see that it is not water but blood sloshing around across your feet. You stare out into the sea, and the breakers take on the forms of men you have slain in battle.", "WaitingInSettlement", "DreamsSongsAndSigns", CampaignTime.Years(1000f), (TextObject description) => Hero.MainHero.Clan.Renown >= 200f && Campaign.Current.GetCampaignBehavior<IStatisticsCampaignBehavior>().GetNumberOfTroopsKnockedOrKilledByPlayer() > 0);
			incident76.AddOption("{=sxrBpIUk}Have I killed without good reason? Heaven wishes me to repent! I shall choose the peaceful path. When there is one, that is...", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100)
			}, null, null);
			incident76.AddOption("{=a0MWe8GO}To where do we sail on this sea of blood? To glory, that is where!", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100)
			}, null, null);
			incident76.AddOption("{=B1cyEaQA}Dreams are but a temporary madness. Heaven does not speak to us in this way.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100) }, null, null);
			Incident incident77 = this.RegisterIncident("incident_market_manipulation", "{=tGwbM59L}Market Manipulation", "{=5OuDl2Po}As you come into town, the merchant {MERCHANT_NAME} makes you an offer. Local merchants are on edge awaiting an incoming caravan from {PLACE}. If you claim that you found its wreckage, looted by bandits, the prices of goods would rise considerably. He offers to pay you 1000 denars, in addition to whatever extra you can make by selling your own wares once the prices rise.", "EnteringTown", "Profit", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.CurrentSettlement == null)
				{
					return false;
				}
				Town randomElement = Town.AllTowns.GetRandomElement<Town>();
				Hero hero5 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetMarketManipulationMerchant|18_348();
				if (hero5 == null)
				{
					return false;
				}
				description.SetTextVariable("MERCHANT_NAME", hero5.Name);
				description.SetTextVariable("PLACE", randomElement.Name);
				return Hero.MainHero.GetSkillValue(DefaultSkills.Trade) >= 20;
			});
			string text106 = "{=SLASexva}Accept, but leave enough room for doubt in your account that it won't look too bad if the caravan turns up.";
			List<IncidentEffect> list107 = new List<IncidentEffect>();
			list107.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list107.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list107.Add(CampaignIncidentEffects.GoldChange(() => 1000));
			list107.Add(CampaignIncidentEffects.SkillChange(DefaultSkills.Trade, 50f));
			list107.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				TownMarketData marketData = Settlement.CurrentSettlement.Town.MarketData;
				foreach (ItemCategory itemCategory in ItemCategories.All.Where<ItemCategory>((ItemCategory x) => x.IsTradeGood))
				{
					marketData.SetDemand(itemCategory, marketData.GetDemand(itemCategory) * 1.1f);
				}
				return new List<TextObject> { new TextObject("{=OI4jkYuY}Demands of trade goods raised by {AMOUNT}% in {TOWN}", null).SetTextVariable("AMOUNT", 10).SetTextVariable("TOWN", Settlement.CurrentSettlement.Name) };
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=uiomHbfS}Raise demand of trade goods by {AMOUNT}% in {TOWN}", null).SetTextVariable("AMOUNT", 10).SetTextVariable("TOWN", Settlement.CurrentSettlement.Name), IncidentHintType.Effect)));
			incident77.AddOption(text106, list107, null, null);
			string text107 = "{=EyHD7gjy}Say you'll swear up and down that you saw the wreckage, damaging your reputation, but demand 2000 denars instead.";
			List<IncidentEffect> list108 = new List<IncidentEffect>();
			list108.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -200));
			list108.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -200));
			list108.Add(CampaignIncidentEffects.GoldChange(() => 2000));
			list108.Add(CampaignIncidentEffects.SkillChange(DefaultSkills.Trade, 100f));
			list108.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				TownMarketData marketData2 = Settlement.CurrentSettlement.Town.MarketData;
				foreach (ItemCategory itemCategory2 in ItemCategories.All.Where<ItemCategory>((ItemCategory x) => x.IsTradeGood))
				{
					marketData2.SetDemand(itemCategory2, marketData2.GetDemand(itemCategory2) * 1.1f);
				}
				return new List<TextObject> { new TextObject("{=OI4jkYuY}Demands of trade goods raised by {AMOUNT}% in {TOWN}", null).SetTextVariable("AMOUNT", 10).SetTextVariable("TOWN", Settlement.CurrentSettlement.Name) };
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=uiomHbfS}Raise demand of trade goods by {AMOUNT}% in {TOWN}", null).SetTextVariable("AMOUNT", 10).SetTextVariable("TOWN", Settlement.CurrentSettlement.Name), IncidentHintType.Effect)));
			incident77.AddOption(text107, list108, null, null);
			string text108 = "{=aMMxVIpa}Erupt in anger for all to see, and curse {MERCHANT} for taking you for a base liar.";
			List<IncidentEffect> list109 = new List<IncidentEffect>();
			list109.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list109.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list109.Add(CampaignIncidentEffects.SettlementRelationChange(() => Settlement.CurrentSettlement, 10, null));
			list109.Add(CampaignIncidentEffects.HeroRelationChange(new Func<Hero>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetMarketManipulationMerchant|18_348), -20));
			incident77.AddOption(text108, list109, delegate(TextObject text)
			{
				Hero hero6 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetMarketManipulationMerchant|18_348();
				text.SetTextVariable("MERCHANT", hero6.Name);
				return true;
			}, null);
			incident77.AddOption("{=UbZMlyk6}Take a pass on the matter.", new List<IncidentEffect>(), null, null);
			Incident incident78 = this.RegisterIncident("incident_gnostic_boys", "{=hewpozXg}Gnostic Boys", "{=H7Co1wqC}Two of your {NOBLE_TIER} have lately been causing a stir in the camp with their irreverent religious preaching. They propound that this world is a illusion, created by a being called the Demiurge for its own amusement, and humans are but game-pieces. You think this may be the Marcorionist doctrine, declared heretical by the imperial Senate some years ago. It is clearly disturbing your more traditional-minded troops and is probably also not good for your party's reputation.", "LeavingVillage", "DreamsSongsAndSigns", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsVillage || MobileParty.MainParty.LastVisitedSettlement.Village.Bound.IsCastle)
				{
					return false;
				}
				CharacterObject characterObject13 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetGnosticBoysNobleTroop|18_361();
				if (characterObject13 == null)
				{
					return false;
				}
				description.SetTextVariable("NOBLE_TIER", characterObject13.Name);
				return true;
			});
			incident78.AddOption("{=WyuxrGqC}Tell them, in a towering fury, that if they do not wish to be burned then they had best either leave your party or cease this sacrilege", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.MoraleChange(20f),
				CampaignIncidentEffects.InfluenceChange(50f),
				CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetGnosticBoysNobleTroop|18_361), -2, null, null)
			}, null, null);
			string text109 = "{=4WZ5CWm8}Take them aside and tell them that while nihilism is fine for a boozy salon in {TOWN_NAME}, it's a bit much for men who must every day put their lives on the line.";
			List<IncidentEffect> list110 = new List<IncidentEffect>();
			list110.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list110.Add(CampaignIncidentEffects.MoraleChange(-20f).WithChance(0.5f));
			incident78.AddOption(text109, list110, delegate(TextObject text)
			{
				text.SetTextVariable("TOWN_NAME", MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Name);
				return true;
			}, null);
			incident78.AddOption("{=vzFoybv0}Tell your party that, if this is true, then men are free to choose their own purpose in this world - and anyone who stands with you should make his purpose to help those in need.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.InfluenceChange(-50f)
			}, null, null);
			incident78.AddOption("{=e07rQCcq}Tell the men that this is splendid news for brave men like themselves, as surely nothing would amuse the Demiurge more than a good fight.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.MoraleChange(20f),
				CampaignIncidentEffects.InfluenceChange(-50f)
			}, null, null);
			Incident incident79 = this.RegisterIncident("incident_speculative_investment", "{=12bmA5qD}Speculative Investment", "{=vPNDzUAF}You strike up a conversation with a trader in town, who is desperate for news of the wars. When you tell him about the looting of {VILLAGE_NAME}, he makes a proposition. The price of grain will almost certainly rise, and he intends to buy up food now to sell it later when the hunger hits. He needs money, and he will sell you {OTHER_GOOD} at far below their market value - 1000 denars. He asks you also to keep this information to yourself.", "LeavingTown", "Profit", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				VillageStateChangedLogEntry villageStateChangedLogEntry = Campaign.Current.LogEntryHistory.FindLastGameActionLog<VillageStateChangedLogEntry>((VillageStateChangedLogEntry x) => x.NewState == Village.VillageStates.Looted && x.Village.IsProducing(DefaultItems.Grain));
				if (villageStateChangedLogEntry == null)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE_NAME", villageStateChangedLogEntry.Village.Name);
				ItemObject itemObject2 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSpeculativeInvestmentOtherGood|18_365();
				description.SetTextVariable("OTHER_GOOD", itemObject2.Name);
				return Hero.MainHero.GetSkillValue(DefaultSkills.Trade) >= 20;
			});
			string text110 = "{=aUui6FYp}All's fair in trade and war. You accept the deal.";
			List<IncidentEffect> list111 = new List<IncidentEffect>();
			list111.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50));
			list111.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -50));
			list111.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list111.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSpeculativeInvestmentOtherGood|18_365), delegate
			{
				ItemObject itemObject3 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSpeculativeInvestmentOtherGood|18_365();
				return (int)Math.Round((double)(1000f / (float)itemObject3.Value));
			}));
			incident79.AddOption(text110, list111, null, null);
			incident79.AddOption("{=2xvqOUJh}Tell him quietly that you will not profit off of the misery of others.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100) }, null, null);
			incident79.AddOption("{=hnMUxRyt}Denounce him angrily in public as a food hoarder for all to hear, so that word gets around of your compassion and mercy.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -150),
				CampaignIncidentEffects.RenownChange(5f)
			}, null, null);
			Incident incident80 = this.RegisterIncident("incident_mad_dog", "{=iuAMSSTY}Mad Dog", "{=nyZpbVbv}Your men are very proud of their leader - perhaps too proud. Every time they go into town, they spread exaggerated tales of your recklessness in battle. You imagine that this could win you a few admiring recruits, but more and more you've caught a look that suggests fear or distrust, as though you're a wild animal or a demon.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), (TextObject description) => Hero.MainHero.Clan.Renown >= 300f && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident80.AddOption("{=bbH4PqtD}Let the tales spread! Sanity and caution do not inspire legend", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -300),
				CampaignIncidentEffects.RenownChange(10f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetMadDogTroop|18_370), 1, null, null)
			}, null, null);
			incident80.AddOption("{=Qbdym8vH}In a towering rage, tell your men that you will not tolerate lies of any sort.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100)
			}, null, null);
			incident80.AddOption("{=EzpB6hJG}Try to play down the image of a madman by being as friendly as possible on your next visit to town.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100)
			}, null, null);
			Incident incident81 = this.RegisterIncident("incident_besiegers_blues", "{=uTsdJCSU}Besiegers' Blues", "{=IxgJotDk}At first this siege gave your men a welcome respite from the hardships of the march, but now inactivity and boredom has started to take a toll. Even dice and gossip have lost their charms, and you're starting to see quarrels and lapses in discipline.", "DuringSiege", "Siege", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.MemberRoster.TotalRegulars >= 40)
				{
					SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
					object obj;
					if (playerSiegeEvent == null)
					{
						obj = null;
					}
					else
					{
						SiegeEvent.SiegeEnginesContainer siegeEngines = playerSiegeEvent.BesiegerCamp.SiegeEngines;
						obj = ((siegeEngines != null) ? siegeEngines.SiegePreparations : null);
					}
					return obj != null;
				}
				return false;
			});
			incident81.AddOption("{=LUap9uP0}Drill your men for the upcoming assault, even though the ever-present risk of injury might put several out of commission when they're needed most.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.PartyExperienceChance(100),
				CampaignIncidentEffects.WoundTroopsRandomly(0.05f).WithChance(0.5f)
			}, null, null);
			string text111 = "{=GTPPtara}March about the camp, searching for specks of rust on swords or sentries who aren't fully attentive to their watch. Give any violators hard, pointless tasks to teach them to keep their standards up.";
			List<IncidentEffect> list112 = new List<IncidentEffect>();
			list112.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list112.Add(CampaignIncidentEffects.SiegeProgressChange(() => PlayerSiege.PlayerSiegeEvent.BesiegerCamp.SiegeEngines.SiegePreparations.Progress * 0.2f));
			incident81.AddOption(text111, list112, null, null);
			incident81.AddOption("{=bp8kKb1A}Let the men enjoy their rest.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.MoraleChange(20f)
			}, null, null);
			Incident incident82 = this.RegisterIncident("incident_swaggering_shield_brothers", "{=KtJYTHVi}Swaggering Shield-Brothers", "{=YCPF4qyz}You are approached by a pair of Skolderbroda. They are anxious to fight for you and ask only that you pay them a token sum. They say you that a warlord like you deserves the services of men such as themselves - praise to you, but a slight to the rest of your party. One of your troops tells you that he's sure that these men must have broken their vows to their company, or why else would they be so eager to join you?", "LeavingVillage", "PartyCampLife", CampaignTime.Years(1000f), (TextObject description) => Hero.MainHero.Clan.Renown >= 150f && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident82.AddOption("{=Kqr9pVCU}Take them into your company. Your troops will need to bear any insult.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSkolderbrotvaTroop|18_374), 2, null, null),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200),
				CampaignIncidentEffects.MoraleChange(-5f)
			}, null, null);
			incident82.AddOption("{=78VrrDRM}Tell them that you believe them to be oathbreakers, and as such have no place in your ranks.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100) }, null, null);
			string text112 = "{=1b9oYu6v}Tell them they can join you and be as arrogant as they like - so long as they beat one of your men in a duel to first blood.";
			list4 = new List<IncidentEffect>();
			list4.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			List<IncidentEffect> list113 = list4;
			IncidentEffect[] array7 = new IncidentEffect[2];
			array7[0] = CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSkolderbrotvaTroop|18_374), 2, null, null);
			array7[1] = CampaignIncidentEffects.WoundTroop(() => (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				orderby x.Character.Tier descending
				select x).FirstOrDefault<TroopRosterElement>((TroopRosterElement x) => !x.Character.IsHero).Character, 1);
			list113.Add(CampaignIncidentEffects.Group(array7).WithChance(0.5f));
			incident82.AddOption(text112, list4, null, null);
			Incident incident83 = this.RegisterIncident("incident_ill_conceived_plans", "{=VSLbywBT}Ill-Conceived Plans", "{=K8DJmY4L}Several of your {TROOP_TYPE} have an idea for the next battle. They suggest that they be hidden in a supply wagon and left for the enemy to discover and drag into camp, whereupon they will leap out and wreak havoc. They are very proud of their scheme, even though you can think of half a dozen reasons it won't work.", "LeavingVillage", "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.MemberRoster.TotalRegulars < 10)
				{
					return false;
				}
				CharacterObject character2 = IncidentHelper.GetSeededRandomElement<TroopRosterElement>((from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
					where !x.Character.IsHero
					select x).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
				if (character2 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOP_TYPE", character2.Name);
				return true;
			});
			incident83.AddOption("{=X68PZrYv}Tell them that every people in Calradia has its own legend about warriors concealed in unlikely places, and you can't imagine them getting away with it.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			string text113 = "{=Gsazqfhj}Let them test their plan. Buy a hide, sew the men into it with their weapons, and gather the company to watch in amusement as they struggle to free themselves.";
			List<IncidentEffect> list114 = new List<IncidentEffect>();
			list114.Add(CampaignIncidentEffects.GoldChange(() => -50));
			list114.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50));
			list114.Add(CampaignIncidentEffects.DisorganizeParty());
			list114.Add(CampaignIncidentEffects.PartyExperienceChance(100));
			incident83.AddOption(text113, list114, null, null);
			incident83.AddOption("{=YhxsQjGM}Nod quickly as though you weren't really listening and move on to some other task.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -50) }, null, null);
			Incident incident84 = this.RegisterIncident("incident_heat_and_dust", "{=HoNCyyfW}Heat and Dust", "{=42bGMh4X}Your men were not expecting to spend so much time in the desert. They complain that baggage that they might gladly carry themselves in normal times becomes a crushing burden when the blazing sun is high in the sky. They have located a number of pack animals in the markets and ask you to buy them to lighten their own loads.", "LeavingVillage", "HardTravel", CampaignTime.Years(1000f), (TextObject description) => Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.Position.Face) == TerrainType.Desert && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			string text114 = "{=ACzeec4e}Agree to purchase the camels.";
			List<IncidentEffect> list115 = new List<IncidentEffect>();
			list115.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 50));
			list115.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("camel"), () => 4));
			list115.Add(CampaignIncidentEffects.GoldChange(() => -1000));
			incident84.AddOption(text114, list115, null, null);
			incident84.AddOption("{=uD8su80f}Tell them that you need the money for other purposes.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200) }, null, null);
			Incident incident85 = this.RegisterIncident("incident_preemptive_work", "{=Zd3jBBun}Preemptive Work", "{=3IphvZTK}In your absence, several families in your district have been hard at work draining a marsh and turning it into a field. The survey records in your treasury however indicate clearly that the marsh is part of your domain. They almost certainly did not know this, but you are entitled to claim the field or charge them for working it.", new string[] { "EnteringTown", "EnteringCastle" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsFortification || MobileParty.MainParty.LastVisitedSettlement.OwnerClan != Clan.PlayerClan)
				{
					return false;
				}
				Settlement lastVisitedSettlement3 = MobileParty.MainParty.LastVisitedSettlement;
				bool flag5 = false;
				for (float num3 = -3f; num3 <= 3f; num3 += 0.25f)
				{
					for (float num4 = -3f; num4 <= 3f; num4 += 0.25f)
					{
						if (num3 * num3 + num4 * num4 <= 9f)
						{
							CampaignVec2 campaignVec = lastVisitedSettlement3.Position + new Vec2(num3, num4);
							if (campaignVec.Face.IsValid() && Campaign.Current.MapSceneWrapper.GetFaceTerrainType(campaignVec.Face) == TerrainType.Desert)
							{
								flag5 = true;
								break;
							}
						}
					}
					if (flag5)
					{
						break;
					}
				}
				if (!flag5)
				{
					return false;
				}
				Building building4 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetPreemptiveWorkBuilding|18_387();
				return building4 != null && building4.CurrentLevel == building4.BuildingType.StartLevel;
			});
			string text115 = "{=9e8I99t3}Transfer ownership to the farmers to encourage such enterprise, even if others may take advantage of you in the future.";
			List<IncidentEffect> list116 = new List<IncidentEffect>();
			list116.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list116.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -50));
			list116.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 20));
			incident85.AddOption(text115, list116, null, null);
			string text116 = "{=j0HmgDOA}Maintain your claim to the land, but allow the farmers to make use of it for only a token rent.";
			List<IncidentEffect> list117 = new List<IncidentEffect>();
			list117.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list117.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50));
			list117.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 15));
			list117.Add(CampaignIncidentEffects.GoldChange(() => 500));
			incident85.AddOption(text116, list117, null, null);
			string text117 = "{=xcPJPtaT}Demand the land for yourself.";
			List<IncidentEffect> list118 = new List<IncidentEffect>();
			list118.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list118.Add(CampaignIncidentEffects.BuildingLevelChange(new Func<Building>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetPreemptiveWorkBuilding|18_387), () => 1));
			list118.Add(CampaignIncidentEffects.TownBoundVillageRelationChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, -20));
			incident85.AddOption(text117, list118, null, null);
			Incident incident86 = this.RegisterIncident("incident_through_proper_channels", "{=S12S4pmL}Through the proper channels", "{=3eOOT66E}One of your tenants has a proposal for you. He proposes to dig a channel known as a qanah into a hillside, where the water table is high, which will allow him to flood some low-lying ground and turn it into an oasis garden. The barren hillside is your property, though at present it yields no income of any kind.", new string[] { "EnteringTown", "EnteringCastle" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.CurrentSettlement == null)
				{
					return false;
				}
				Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
				bool flag6 = false;
				for (float num5 = -3f; num5 <= 3f; num5 += 0.25f)
				{
					for (float num6 = -3f; num6 <= 3f; num6 += 0.25f)
					{
						if (num5 * num5 + num6 * num6 <= 9f)
						{
							CampaignVec2 campaignVec2 = currentSettlement.Position + new Vec2(num5, num6);
							if (campaignVec2.Face.IsValid() && Campaign.Current.MapSceneWrapper.GetFaceTerrainType(campaignVec2.Face) == TerrainType.Desert)
							{
								flag6 = true;
								break;
							}
						}
					}
					if (flag6)
					{
						break;
					}
				}
				return flag6 && MobileParty.MainParty.CurrentSettlement.IsFortification && Hero.MainHero.Clan == MobileParty.MainParty.CurrentSettlement.OwnerClan;
			});
			string text118 = "{=7f06ovrO}Applaud his plan, and tell him that you will charge him nothing to sink a channel through your property.";
			List<IncidentEffect> list119 = new List<IncidentEffect>();
			list119.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list119.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.CurrentSettlement.Town, 20));
			incident86.AddOption(text118, list119, null, null);
			string text119 = "{=iB2g7XBt}Insist he purchase the hillside from your at market rate.";
			List<IncidentEffect> list120 = new List<IncidentEffect>();
			list120.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list120.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.CurrentSettlement.Town, 15));
			list120.Add(CampaignIncidentEffects.GoldChange(() => 300));
			incident86.AddOption(text119, list120, null, null);
			string text120 = "{=P8W5OQB9}Wait until he digs the channel and clears the fields, then charge him heavily to use the water without which his effort will be for nothing.";
			List<IncidentEffect> list121 = new List<IncidentEffect>();
			list121.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -200));
			list121.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50));
			list121.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.CurrentSettlement.Town, 10));
			list121.Add(CampaignIncidentEffects.GoldChange(() => 500));
			incident86.AddOption(text120, list121, null, null);
			Incident incident87 = this.RegisterIncident("incident_dodgy_spice", "{=5OzX71s3}Dodgy Spice", "{=QEP3ntdb}Merchants are savvy to most attempts to pass off most types of shoddy goods as top quality, but one of your {TROOP.NAME} knows a few tricks that aren't in the usual rogue's repetoire. He suggests buying a selection of spices from the market, then sprinkling a bit of pomegranate rind in with the saffron, some cassia bark with the cinnamon, and a lead compound known as litharge with the pepper.", "LeavingTown", "Profit", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (Hero.MainHero.GetSkillValue(DefaultSkills.Trade) < 10)
				{
					return false;
				}
				TroopRosterElement troopRosterElement2 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetRandomTroop|18_401();
				if (troopRosterElement2.Character == null)
				{
					return false;
				}
				description.SetCharacterProperties("TROOP", troopRosterElement2.Character, false);
				return true;
			});
			string text121 = "{=lrysxcAE}A bit of lead never hurt anyone! Try them all, and split the profits with your {TROOP.NAME}.";
			List<IncidentEffect> list122 = new List<IncidentEffect>();
			list122.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list122.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -300));
			list122.Add(CampaignIncidentEffects.GoldChange(() => -100));
			list122.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("salt"), () => 1));
			incident87.AddOption(text121, list122, delegate(TextObject text)
			{
				TroopRosterElement troopRosterElement3 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetRandomTroop|18_401();
				text.SetCharacterProperties("TROOP", troopRosterElement3.Character, false);
				return true;
			}, null);
			string text122 = "{=tDK8b1J8}You vaguely recall hearing that lead might be poisonous, but you'll try the others.";
			List<IncidentEffect> list123 = new List<IncidentEffect>();
			list123.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list123.Add(CampaignIncidentEffects.GoldChange(() => -150));
			list123.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("salt"), () => 1));
			incident87.AddOption(text122, list123, null, null);
			incident87.AddOption("{=MfbK2Sqa}Tell your man to save his ingenuity for battle, not trade.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -50)
			}, null, null);
			string text123 = "{=a5gLRsa4}Thank your man bruskly and try his trick, but pay him nothing.";
			List<IncidentEffect> list124 = new List<IncidentEffect>();
			list124.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -200));
			list124.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list124.Add(CampaignIncidentEffects.ChangeItemAmount(() => Game.Current.ObjectManager.GetObject<ItemObject>("salt"), () => 1));
			incident87.AddOption(text123, list124, null, null);
			Incident incident88 = this.RegisterIncident("incident_sorrows_of_war", "{=FL8rUSWo}Sorrows of War", "{=a939QfLJ}As you leave {TOWN_NAME}, a young woman throws herself in front of your horse. She raises her face to the sky, tears at her hair, and wails that her young husband signed up to fight for you and was slain in battle. \"You took him from me!\" she screams. It is not clear what she wants.", "LeavingTown", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				PlayerBattleEndedLogEntry playerBattleEndedLogEntry3 = Campaign.Current.LogEntryHistory.FindLastGameActionLog<PlayerBattleEndedLogEntry>((PlayerBattleEndedLogEntry x) => true);
				if (playerBattleEndedLogEntry3 == null)
				{
					return false;
				}
				if (!playerBattleEndedLogEntry3.HasHeavyCausality)
				{
					return false;
				}
				description.SetTextVariable("TOWN_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				return true;
			});
			string text124 = "{=8uabQDxp}Tell her, as soberly as you can, that her husband gave his life for a noble cause. Hand her a purse of 100 denars, and tell her that you are sure that he would be happy if she marries again, as long as he remains in her memory.";
			List<IncidentEffect> list125 = new List<IncidentEffect>();
			list125.Add(CampaignIncidentEffects.GoldChange(() => -100));
			list125.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 50));
			incident88.AddOption(text124, list125, null, null);
			incident88.AddOption("{=kZJvgtE3}Dismount, burst into tears, take her hands in yours and tell her that every life lost in your service tears at your heart, and that you want nothing more but for these wars to end.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -200),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100)
			}, null, null);
			incident88.AddOption("{=52l7VuWI}Tell her that Heaven expects her to bear her fate with dignity.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100) }, null, null);
			string text125 = "{=Sz28bZOI}Have your purser speak to her at length about what she might need, as you keep your distance. ";
			List<IncidentEffect> list126 = new List<IncidentEffect>();
			list126.Add(CampaignIncidentEffects.DisorganizeParty());
			list126.Add(CampaignIncidentEffects.GoldChange(() => -50));
			incident88.AddOption(text125, list126, null, null);
			Incident incident89 = this.RegisterIncident("incident_souvenirs", "{=LAMmAUvt}Souvenirs", "{=PqzLYt6i}Some time recently - you're not sure exactly when - your men started a new post-battle tradition - going about the field collecting grisly trophies from slain enemies. Now you can't help noticing these prizes dangling from their belts or the halters of their horses.", "LeavingBattle", "PostBattle", CampaignTime.Years(1000f), (TextObject description) => Settlement.CurrentSettlement != null && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5);
			incident89.AddOption("{=1zsWhG2N}Tell your men as best you can that, while you respect their spirit, this will frighten local farmers who don't understand warriors' ways.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.MoraleChange(-20f)
			}, null, null);
			string text126 = "{=VHlwd8na}Take a lively interest in each of these ornaments, asking your men at which battle and from which fallen foe they were taken.";
			List<IncidentEffect> list127 = new List<IncidentEffect>();
			list127.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list127.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list127.Add(CampaignIncidentEffects.MoraleChange(20f));
			list127.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				List<Settlement> list171 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetNearbyVillages|18_417();
				List<TextObject> list172 = new List<TextObject>();
				foreach (Settlement settlement in list171)
				{
					ChangeRelationAction.ApplyPlayerRelation(settlement.Notables.FirstOrDefault<Hero>(), -5, true, true);
					TextObject textObject9 = new TextObject("{=8IzNumMa}{?AMOUNT > 0}Increased{?}Decreased{\\?} relationship with {SETTLEMENT} by {ABS(AMOUNT)}.", null);
					textObject9.SetTextVariable("AMOUNT", -5);
					textObject9.SetTextVariable("SETTLEMENT", settlement.Name);
					list172.Add(textObject9);
				}
				return list172;
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=4o7R829M}-5 relations with three surrounding villages", null), IncidentHintType.Effect)));
			incident89.AddOption(text126, list127, null, null);
			incident89.AddOption("{=xoH5LaY0}Harangue your men for their barbarism, tell them to honor rather than mock their fallen foes.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			string text127 = "{=SLAFtiET}Pretend you saw nothing.";
			List<IncidentEffect> list128 = new List<IncidentEffect>();
			list128.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				List<Settlement> list173 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetNearbyVillages|18_417();
				List<TextObject> list174 = new List<TextObject>();
				foreach (Settlement settlement2 in list173)
				{
					ChangeRelationAction.ApplyPlayerRelation(settlement2.Notables.FirstOrDefault<Hero>(), -5, true, true);
					TextObject textObject10 = new TextObject("{=8IzNumMa}{?AMOUNT > 0}Increased{?}Decreased{\\?} relationship with {SETTLEMENT} by {ABS(AMOUNT)}.", null);
					textObject10.SetTextVariable("AMOUNT", -5);
					textObject10.SetTextVariable("SETTLEMENT", settlement2.Name);
					list174.Add(textObject10);
				}
				return list174;
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=4o7R829M}-5 relations with three surrounding villages", null), IncidentHintType.Effect)));
			incident89.AddOption(text127, list128, null, null);
			Incident incident90 = this.RegisterIncident("incident_back_taxes", "{=NFwuiSCw}Back taxes", "{=YAnTtrLs}Your reputation as a skilled trader generally does you good, but there is always a downside. When you arrive in {TOWN}, the market inspector descends from his tower to present you with a list of fees that you supposedly accumulated over the years, based on decrees that go back several centuries. The decrees may be genuine, but in all your talk with merchants, you never heard of anyone ever paying them.", "EnteringTown", "Profit", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.CurrentSettlement == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN", MobileParty.MainParty.CurrentSettlement.Name);
				return Hero.MainHero.GetSkillValue(DefaultSkills.Trade) >= 40;
			});
			string text128 = "{=RPyFwR6I}Erupt in fury, telling the inspector that there is no way you will be fleeced in this way, legally or illegally.";
			List<IncidentEffect> list129 = new List<IncidentEffect>();
			list129.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list129.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list129.Add(CampaignIncidentEffects.RenownChange(5f));
			list129.Add(CampaignIncidentEffects.CrimeRatingChange(() => MobileParty.MainParty.CurrentSettlement.MapFaction, -10f));
			incident90.AddOption(text128, list129, null, null);
			string text129 = "{=vzA2ZJog}Pay, but say for all to hear that selective application of obscure laws is little better than theft.";
			List<IncidentEffect> list130 = new List<IncidentEffect>();
			list130.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list130.Add(CampaignIncidentEffects.GoldChange(() => -300));
			incident90.AddOption(text129, list130, null, null);
			string text130 = "{=ogP9E5Hx}Reason with the inspector over a splendid dinner and an amphora of the finest wine that is of course on you.";
			List<IncidentEffect> list131 = new List<IncidentEffect>();
			list131.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -50));
			list131.Add(CampaignIncidentEffects.InfluenceChange(50f));
			list131.Add(CampaignIncidentEffects.GoldChange(() => -100));
			incident90.AddOption(text130, list131, null, null);
			string text131 = "{=9aYLFpC9}Just hand over the money to make the problem go away.";
			List<IncidentEffect> list132 = new List<IncidentEffect>();
			list132.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list132.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100));
			incident90.AddOption(text131, list132, null, null);
			Incident incident91 = this.RegisterIncident("incident_third_party_arbitration", "{=trfafbJW}Third-Party Arbitration", "{=XQU3Tobe}Knowing your reputation as a trader, two merchants in {TOWN_NAME} are asking you to arbitrate a dispute between them. The two had drawn up a contract wherein each partner had a half-share in an incoming shipment of wine. When it arrived, however, the sea captain demanded twice the price. The wealthier of the two partners was at the docks. He took the initiative and purchased the goods, deciding to take a loss for the sake of his reputation as a reliable supplier to the local taverns, but the second is poorer and wants out of the deal.", "EnteringTown", "Profit", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.CurrentSettlement == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN_NAME", MobileParty.MainParty.CurrentSettlement.Name);
				return Hero.MainHero.GetSkillValue(DefaultSkills.Trade) >= 35;
			});
			incident91.AddOption("{=nbshAaHo}The letter of the contract would favor the wealthier partner. Merchants should be careful what they sign.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -50),
				CampaignIncidentEffects.InfluenceChange(50f)
			}, null, null);
			incident91.AddOption("{=bsC5zEoI}Never mind the letter of the contract, the reasonable, decent move for the richer merchant would have been to consult with his partner.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -50),
				CampaignIncidentEffects.InfluenceChange(50f)
			}, null, null);
			incident91.AddOption("{=Ib04QQJY}Tell them that the whole thing gives you a headache.", new List<IncidentEffect>(), null, null);
			string text132 = "{=0ZP5fDBX}Let the wealthier merchant know that your decision is for sale.";
			List<IncidentEffect> list133 = new List<IncidentEffect>();
			list133.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -300));
			list133.Add(CampaignIncidentEffects.GoldChange(() => 1000));
			incident91.AddOption(text132, list133, null, null);
			Incident incident92 = this.RegisterIncident("incident_tipping_the_scales", "{=OXzJbvd8}Tipping the scales", "{=fhjyEgld}Your troops are taking a bit longer than usual to get ready, and you've had time to relax a bit and watch goings-on at the market. One commodity vendor in particular has caught your attention. He always uses one set of weights for the sacks that he is buying from local farmers, and another for goods that he sells. You have little doubt that one or both of these sets is a false measure, allowing him to defraud his customers.", "LeavingTown", "Profit", CampaignTime.Years(1000f), (TextObject description) => Hero.MainHero.GetSkillValue(DefaultSkills.Trade) >= 30);
			incident92.AddOption("{=wjpmx20I}This isn't your affair, and you don't know if other merchants or corrupt officials may be in on it. Walk away.", new List<IncidentEffect>(), null, null);
			string text133 = "{=Uidhqph1}Denounce the merchant loudly, even if you run the risk of making enemies among the town elite.";
			List<IncidentEffect> list134 = new List<IncidentEffect>();
			list134.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list134.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -20, null).WithChance(0.5f));
			incident92.AddOption(text133, list134, null, null);
			string text134 = "{=MGfXReQC}Walk up to the merchant, and insist he use the lighter weights to sell you one measure of his goods. Then come back later and return them, insisting he use the heavier weights.";
			List<IncidentEffect> list135 = new List<IncidentEffect>();
			list135.Add(CampaignIncidentEffects.GoldChange(() => 200));
			list135.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -50));
			list135.Add(CampaignIncidentEffects.SkillChange(DefaultSkills.Roguery, 50f));
			incident92.AddOption(text134, list135, null, null);
			Incident incident93 = this.RegisterIncident("incident_injury_accident", "{=m9MbuMlw}Injury Accident", "{=1jAyaivZ}As you pass through the village streets, a dog dashes out from among some spectators and bites at the heels of one of your {CAVALRYMAN}'s horses, panicking it. The mount rears and gallops down the street, trampling a vegetable vendor who was unable to get out of its way. She will probably walk with a limp hereafter, and her family demands compensation.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				TroopRosterElement troopRosterElement4 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetRandomCavalryTroop|18_434();
				if (troopRosterElement4.Character == null)
				{
					return false;
				}
				description.SetTextVariable("CAVALRYMAN", troopRosterElement4.Character.Name);
				return MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__CavalryTroopPredicate|18_433)).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) >= 10;
			});
			string text135 = "{=jwsexTeh}No horseman can control a mount that is under attack from a dog. Refuse.";
			List<IncidentEffect> list136 = new List<IncidentEffect>();
			list136.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list136.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -20, null));
			incident93.AddOption(text135, list136, null, null);
			string text136 = "{=5HsJkbZz}You brought your men into their village, and you must take responsibility.";
			List<IncidentEffect> list137 = new List<IncidentEffect>();
			list137.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list137.Add(CampaignIncidentEffects.GoldChange(() => -100));
			incident93.AddOption(text136, list137, null, null);
			string text137 = "{=te1A19Xe}Tell your {CAVALRYMAN} that he can sort it out.";
			List<IncidentEffect> list138 = new List<IncidentEffect>();
			list138.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			incident93.AddOption(text137, list138, delegate(TextObject text)
			{
				TroopRosterElement troopRosterElement5 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetRandomCavalryTroop|18_434();
				text.SetTextVariable("CAVALRYMAN", troopRosterElement5.Character.Name);
				return true;
			}, null);
			Incident incident94 = this.RegisterIncident("incident_water_supplies", "{=xYjLaJxA}Water supplies", "{=EbNZg7jp}In hot climes, often the most vulnerable part of a fortress is its water supply. Most depend on wells but those can run dry. After days spent in front of {FORTRESS_NAME}, you've caught glimpses of the defenders slipping into the hills. You explore where they've gone, and eventually you come across a rudimentary cistern. You suspect that they must be refreshing their water here.", "DuringSiege", "Siege", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.BesiegedSettlement == null)
				{
					return false;
				}
				description.SetTextVariable("FORTRESS_NAME", MobileParty.MainParty.BesiegedSettlement.Name);
				if (!MobileParty.MainParty.Position.IsValid())
				{
					return false;
				}
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.Position.Face);
				return faceTerrainType == TerrainType.Steppe || faceTerrainType == TerrainType.Desert;
			});
			string text138 = "{=57VuY9ln}Poison it! Some women and children of {FORTRESS_NAME} may well perish along with the defenders, but such is war.";
			List<IncidentEffect> list139 = new List<IncidentEffect>();
			list139.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list139.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				MobileParty garrisonParty = MobileParty.MainParty.BesiegedSettlement.Town.GarrisonParty;
				int num7 = (int)((float)garrisonParty.MemberRoster.TotalRegulars * 0.1f);
				List<TroopRosterElement> list175 = (from x in garrisonParty.MemberRoster.GetTroopRoster()
					where !x.Character.IsHero && x.Number > 0
					select x).ToList<TroopRosterElement>();
				while (num7 > 0 && list175.Count > 0)
				{
					int num8 = MBRandom.RandomInt(list175.Count);
					TroopRosterElement troopRosterElement6 = list175[num8];
					int num9 = MBRandom.RandomInt(1, Math.Min(num7, troopRosterElement6.Number) + 1);
					garrisonParty.MemberRoster.AddToCounts(troopRosterElement6.Character, -num9, false, 0, 0, true, -1);
					troopRosterElement6.Number -= num9;
					list175[num8] = troopRosterElement6;
					if (troopRosterElement6.Number == 0)
					{
						list175.RemoveAt(num8);
					}
					num7 -= num9;
				}
				return new List<TextObject>
				{
					new TextObject("{=pS3PKbYB}10% of the garrison perished.", null)
				};
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=gyRuGBMF}10% of the garrison perishes", null), IncidentHintType.Effect)));
			list139.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.BesiegedSettlement.Town, -20));
			incident94.AddOption(text138, list139, delegate(TextObject text)
			{
				text.SetTextVariable("FORTRESS_NAME", MobileParty.MainParty.BesiegedSettlement.Name);
				return true;
			}, null);
			string text139 = "{=bGaD3LoG}The best blows are struck not to the body but to the spirit. Let the defenders know you found it by diverting it to a pool where your men can bathe on the hottest, thirstiest days of the siege.";
			List<IncidentEffect> list140 = new List<IncidentEffect>();
			list140.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list140.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list140.Add(CampaignIncidentEffects.SiegeProgressChange(() => 0.1f));
			incident94.AddOption(text139, list140, null, null);
			incident94.AddOption("{=iiwhVNBb}You respect the defenders' cunning. Simply observe how they slip out of the fortress, by ropes or hidden gates, and learn from it.", new List<IncidentEffect> { CampaignIncidentEffects.SkillChange(DefaultSkills.Engineering, 100f) }, null, null);
			Incident incident95 = this.RegisterIncident("incident_mining", "{=O4iUPSXs}Mining", "{=0DSopB7F}After observing the terrain around {FORTRESS_NAME}, you think conditions might be appropriate for mining - digging a tunnel under a wall, then collapsing it. This can open a breach but is time-consuming and risky. If you encounter rock underground, you may find all your efforts go to waste. If the ground is too soft, the tunnel may collapse and bury your men. Most troops are reluctant to spend time in dark, confined tunnels unless they are paid extra, or forced.", "DuringSiege", "Siege", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.BesiegedSettlement == null)
				{
					return false;
				}
				description.SetTextVariable("FORTRESS_NAME", MobileParty.MainParty.BesiegedSettlement.Name);
				return MobileParty.MainParty.BesiegedSettlement != null && Hero.MainHero.GetSkillValue(DefaultSkills.Engineering) >= 30;
			});
			Incident incident96 = incident95;
			string text140 = "{=SR6inO3d}Double the pay of any troops willing to dig the mine, and tunnel carefully.";
			list4 = new List<IncidentEffect>();
			list4.Add(CampaignIncidentEffects.GoldChange(() => -500));
			List<IncidentEffect> list141 = list4;
			IncidentEffect incidentEffect2 = CampaignIncidentEffects.Group(new IncidentEffect[]
			{
				CampaignIncidentEffects.BreachSiegeWall(1),
				CampaignIncidentEffects.MoraleChange(5f),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.RenownChange(5f),
				CampaignIncidentEffects.SkillChange(DefaultSkills.Engineering, 50f)
			});
			IncidentEffect[] array8 = new IncidentEffect[2];
			array8[0] = CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100);
			array8[1] = CampaignIncidentEffects.SiegeProgressChange(() => -0.2f);
			list141.Add(CampaignIncidentEffects.Select(incidentEffect2, CampaignIncidentEffects.Group(array8), 0.6f));
			incident96.AddOption(text140, list4, null, null);
			Incident incident97 = incident95;
			string text141 = "{=kntd1zYo}Tunnel quickly, threatening to execute anyone who refuses for insubordination";
			list4 = new List<IncidentEffect>();
			List<IncidentEffect> list142 = list4;
			IncidentEffect[] array9 = new IncidentEffect[4];
			array9[0] = CampaignIncidentEffects.SiegeProgressChange(() => 0.2f);
			array9[1] = CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100);
			array9[2] = CampaignIncidentEffects.RenownChange(5f);
			array9[3] = CampaignIncidentEffects.SkillChange(DefaultSkills.Engineering, 50f);
			IncidentEffect incidentEffect3 = CampaignIncidentEffects.Group(array9);
			IncidentEffect[] array10 = new IncidentEffect[3];
			array10[0] = CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100);
			array10[1] = CampaignIncidentEffects.SiegeProgressChange(() => -0.1f);
			array10[2] = CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 10, false);
			list142.Add(CampaignIncidentEffects.Select(incidentEffect3, CampaignIncidentEffects.Group(array10), 0.6f));
			incident97.AddOption(text141, list4, null, null);
			incident95.AddOption("{=lCumAw9j}Rely on the usual methods", new List<IncidentEffect>(), null, null);
			Incident incident98 = this.RegisterIncident("incident_at_the_breach", "{=OaCU1zfR}At the Breach", "{=rpvUQUmq}You've been studying the walls of {FORTRESS_NAME} for days, and you see what appears to be a crack in one place - possibly the result of an old earthquake, or just negligence. It's hard to hit with your catapults and the ground is too rocky to mine, but you reckon troops with pickaxes by themselves could bring it down - assuming they were willing to brave the storm of arrows and rocks that would no doubt come pouring from the battlements above.", "DuringSiege", "Siege", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.BesiegedSettlement == null)
				{
					return false;
				}
				description.SetTextVariable("FORTRESS_NAME", MobileParty.MainParty.BesiegedSettlement.Name);
				return MobileParty.MainParty.BesiegedSettlement != null && Hero.MainHero.GetSkillValue(DefaultSkills.Engineering) >= 20;
			});
			string text142 = "{=PJYgbG2h}\"A purse of silver for every stone brought to me from that wall!\"";
			List<IncidentEffect> list143 = new List<IncidentEffect>();
			list143.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list143.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list143.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => true, () => 10, false));
			list143.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list143.Add(CampaignIncidentEffects.SiegeProgressChange(() => 0.2f).WithChance(0.8f));
			incident98.AddOption(text142, list143, null, null);
			string text143 = "{=ffJrKMFm}Order your recruits to swarm the base of the wall and bring it down. They can be replaced.";
			List<IncidentEffect> list144 = new List<IncidentEffect>();
			list144.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list144.Add(CampaignIncidentEffects.KillTroopsRandomlyOrderedByTier((TroopRosterElement x) => true, () => 10));
			list144.Add(CampaignIncidentEffects.BreachSiegeWall(1));
			incident98.AddOption(text143, list144, null, null);
			incident98.AddOption("{=wJ3Nh230}A breach is not worth the losses.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100)
			}, null, null);
			Incident incident99 = this.RegisterIncident("incident_failed_inspection", "{=KBh8fb89}Failed Inspection", "{=jUBTrdbE}You take a quick look over your men before you ride out, and you are shocked to notice the state of their equipment. In particular, several of the men have frayed bridles and shield straps that look like they might snap in battle. They are supposed to oil and replace their leather at their own expense, and clearly have not been doing so.", new string[] { "LeavingCastle", "LeavingVillage", "LeavingTown" }, "PartyCampLife", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.MemberRoster.TotalRegulars >= 40);
			incident99.AddOption("{=fyJ6aDOQ}Burst into a righteous fury and hand out extra punishments, so that no one will ever again dare neglect their equipment", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -200),
				CampaignIncidentEffects.PartyExperienceChance(100)
			}, null, null);
			incident99.AddOption("{=PZTyrsag}Sternly lecture them the risks of a strap snapping in battle. Have them do all their preparations for march over again, from the beginning.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.PartyExperienceChance(20),
				CampaignIncidentEffects.DisorganizeParty(),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50)
			}, null, null);
			string text144 = "{=fQscCIxs}Buy them the best equipment you can from the market to replace their wares.";
			List<IncidentEffect> list145 = new List<IncidentEffect>();
			list145.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list145.Add(CampaignIncidentEffects.GoldChange(() => -200));
			incident99.AddOption(text144, list145, null, null);
			string text145 = "{=SLAFtiET}Pretend you saw nothing.";
			List<IncidentEffect> list146 = new List<IncidentEffect>();
			list146.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list146.Add(CampaignIncidentEffects.DemoteTroopsRandomlyWithPredicate((TroopRosterElement x) => !x.Character.IsHero, (CharacterObject x) => true, 2, false));
			incident99.AddOption(text145, list146, null, null);
			Incident incident100 = this.RegisterIncident("incident_the_quiet_life", "{=3M54uDKZ}The Quiet Life", "{=IaOfWrNF}As you pass through your lands, you see several of your {TROOP_NAME} casting envious eyes over the farmers in the fields. They are tired of wandering from place to place, they say, and hope to put down roots somewhere and start a family. Their contract of enlistment is not yet up but it occurs to you that if they settled in your estates, it would be a boon to your local militia.", new string[] { "EnteringTown", "EnteringCastle" }, "FiefManagement", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.CurrentSettlement == null)
				{
					return false;
				}
				CharacterObject characterObject14 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTheQuietLifeTroop|18_465();
				if (characterObject14 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOP_NAME", characterObject14.Name);
				return MobileParty.MainParty.CurrentSettlement.IsFortification && Hero.MainHero.Clan == MobileParty.MainParty.CurrentSettlement.OwnerClan && MobileParty.MainParty.MemberRoster.TotalRegulars >= 60;
			});
			string text146 = "{=SFUrdvHo}Grant them their wish, and encourage them to purchase land near {FORTRESS_NAME}.";
			List<IncidentEffect> list147 = new List<IncidentEffect>();
			list147.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list147.Add(CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTheQuietLifeTroop|18_465), -3, null, null));
			list147.Add(CampaignIncidentEffects.SettlementMilitiaChange(() => MobileParty.MainParty.CurrentSettlement, 10));
			incident100.AddOption(text146, list147, delegate(TextObject text)
			{
				text.SetTextVariable("FORTRESS_NAME", MobileParty.MainParty.CurrentSettlement.Name);
				return true;
			}, null);
			string text147 = "{=3WFuf7OC}Give them money to purchase land near your fief in {VILLAGE_NAME}, so long as they take charge of training the local militia.";
			List<IncidentEffect> list148 = new List<IncidentEffect>();
			list148.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list148.Add(CampaignIncidentEffects.GoldChange(() => -300));
			list148.Add(CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTheQuietLifeTroop|18_465), -3, null, null));
			list148.Add(CampaignIncidentEffects.SettlementMilitiaChange(() => MobileParty.MainParty.CurrentSettlement, 15));
			incident100.AddOption(text147, list148, delegate(TextObject text)
			{
				Village randomElement2 = MobileParty.MainParty.CurrentSettlement.Town.Villages.GetRandomElement<Village>();
				text.SetTextVariable("VILLAGE_NAME", randomElement2.Name);
				return true;
			}, null);
			string text148 = "{=YIsU82Jd}Tell them they can go, but only if they forfeit back pay as compensation to you for losing them.";
			List<IncidentEffect> list149 = new List<IncidentEffect>();
			list149.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list149.Add(CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTheQuietLifeTroop|18_465), -3, null, null));
			list149.Add(CampaignIncidentEffects.GoldChange(() => 300));
			incident100.AddOption(text148, list149, null, null);
			incident100.AddOption("{=KUb0DRxw}Refuse them permission.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100) }, null, null);
			Incident incident101 = this.RegisterIncident("incident_job_offer", "{=xEktDm2z}Job Offer", "{=guEjfYhO}One of your {TROOP_NAME} had some drinks with the arena master in {TOWN} and left the tavern with a job offer - work as a trainer and occasional combatant. The signing bonus is generous. He is reluctant to leave you, and you'd be reluctant to lose a valuable soldier, but he'd be in a fine position to sing your praises.", "LeavingTown", "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				CharacterObject characterObject15 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobOfferTroop|18_474();
				if (characterObject15 == null)
				{
					return false;
				}
				description.SetTextVariable("TROOP_NAME", characterObject15.Name);
				description.SetTextVariable("TOWN", MobileParty.MainParty.LastVisitedSettlement.Name);
				return MobileParty.MainParty.MemberRoster.TotalRegulars >= 30 && Hero.MainHero.Clan.Renown >= 200f;
			});
			incident101.AddOption("{=0awbcZag}Give him your blessing and encourage him to bore everyone to tears with his old war stories.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.RenownChange(10f),
				CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobOfferTroop|18_474), -1, null, null)
			}, null, null);
			incident101.AddOption("{=Qb3axqdZ}Persuade him to stay. The arena is fine for practice but true glory is not won with blunted weapons.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100)
			}, null, null);
			string text149 = "{=klVc5afL}Let him go if he splits his signing bonus with you.";
			List<IncidentEffect> list150 = new List<IncidentEffect>();
			list150.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list150.Add(CampaignIncidentEffects.RenownChange(5f));
			list150.Add(CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobOfferTroop|18_474), -1, null, null));
			list150.Add(CampaignIncidentEffects.GoldChange(() => 200));
			incident101.AddOption(text149, list150, null, null);
			Incident incident102 = this.RegisterIncident("incident_secession_of_the_plebs", "{=VvwnbRx4}Secession of the Plebs", "{=GgYzaakh}Trade in {TOWN_NAME} has been booming, and prices and wages have been going up. The workers at your {WORKSHOP} have taken notice. Your superviser reports that they have all refused to come in to work until they receive a pay increase. He notes that there remain plenty of hungry mouths and willing hands in town, but that the current workers might cause trouble if they are replaced.", "EnteringTown", "Workshop", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				Workshop workshop = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSecessionOfThePlebsWorkshop|18_478();
				if (workshop == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN_NAME", workshop.Settlement.Name);
				description.SetTextVariable("WORKSHOP", workshop.Name);
				return true;
			});
			incident102.AddOption("{=5krpAZu6}Chide them gently for walking off the job before coming to you first, because of course you'll up their wages.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200),
				CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSecessionOfThePlebsWorkshop|18_478), -0.1f)
			}, null, null);
			incident102.AddOption("{=zM2vbhrt}Sit down with them and negotiate a mutual understanding.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetSecessionOfThePlebsWorkshop|18_478), -0.05f)
			}, null, null);
			string text150 = "{=aTU2CgIU}Fire the lot of them, hire different workers, and post some troops outside to protect your shop.";
			List<IncidentEffect> list151 = new List<IncidentEffect>();
			list151.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list151.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list151.Add(CampaignIncidentEffects.KillTroopsRandomly((TroopRosterElement x) => !x.Character.IsHero, () => 2, false));
			incident102.AddOption(text150, list151, null, null);
			Incident incident103 = this.RegisterIncident("incident_occupational_safety", "{=l8rpMldr}Occupational Safety", "{=Eih6lrcN}Business is good at your {WORKSHOP} in {TOWN}, but in the rush to keep up with demand the normal precautions are not being taken. {INJURY_TEXT} He will live, but is unlikely to be able to do skilled work, and the rest of the workers are worried about future accidents.", "EnteringTown", "Workshop", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (!MobileParty.MainParty.CurrentSettlement.IsTown)
				{
					return false;
				}
				WorkshopType workshopType = WorkshopType.Find("brewery");
				WorkshopType workshopType2 = WorkshopType.Find("smithy");
				Workshop workshop2 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetOccupationalSafetyWorkshop|18_485();
				if (workshop2 == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN", workshop2.Settlement.Name);
				description.SetTextVariable("WORKSHOP", workshop2.Name);
				string text171 = string.Empty;
				if (workshop2.WorkshopType == workshopType)
				{
					text171 = "{=JcoSd4oi}One man burned his hand in the oven.";
				}
				else if (workshop2.WorkshopType == workshopType2)
				{
					text171 = "{=lVnw8K2N}One man crushed his hand in the forge.";
				}
				description.SetTextVariable("INJURY_TEXT", text171);
				return true;
			});
			string text151 = "{=JdlbkM8C}Pay him a pension and order your supervisors to take more care, even if it slows down production.";
			List<IncidentEffect> list152 = new List<IncidentEffect>();
			list152.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list152.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100));
			list152.Add(CampaignIncidentEffects.GoldChange(() => -100));
			list152.Add(CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetOccupationalSafetyWorkshop|18_485), -0.1f));
			incident103.AddOption(text151, list152, null, null);
			string text152 = "{=OlvD0u1O}You're not going to pay for another man's clumsiness. Let work recommence at the same pace.";
			List<IncidentEffect> list153 = new List<IncidentEffect>();
			list153.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list153.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.CurrentSettlement.Town, -10));
			incident103.AddOption(text152, list153, null, null);
			incident103.AddOption("{=k4aNfNHg}As on the battlefield, so on the workshop floor. Let each team work at its own pace, receiving bonuses for output, but also taking responsibiliy for injuries.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -200),
				CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetOccupationalSafetyWorkshop|18_485), 0.05f)
			}, null, null);
			Incident incident104 = this.RegisterIncident("incident_hand_in_the_strongbox", "{=OFuTQ6t8}Hand in the Strongbox", "{=82Fno8Vr}Your clerk at your {WORKSHOP} in {TOWN} has caught one of your foremen skimming from the funds you use to purchase raw materials. Luckily, he did not take much, and he readily confessed. How do you prevent this from happening in the future?", "EnteringTown", "Workshop", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				Workshop workshop3 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHandInTheStrongboxWorkshop|18_490();
				if (workshop3 == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN", workshop3.Settlement.Name);
				description.SetTextVariable("WORKSHOP", workshop3.Name);
				return true;
			});
			incident104.AddOption("{=47hUs9yg}You make an example of the foreman. You hand him over to the town authorities and encourage them to apply the full penalty - branding as a thief and exile from the town.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100),
				CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHandInTheStrongboxWorkshop|18_490), -0.1f).WithChance(0.3f)
			}, null, null);
			incident104.AddOption("{=n2mOvIPt}You appeal to your men's better natures. You plea for leniency for your foreman, and beg them not to take advantage.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100),
				CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHandInTheStrongboxWorkshop|18_490), -0.05f)
			}, null, null);
			incident104.AddOption("{=cyZG4qHm}You institute a complex scheme of bookkeeping, even though it will make purchases more onerous and slow down production.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100),
				CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetHandInTheStrongboxWorkshop|18_490), -0.05f)
			}, null, null);
			incident104.AddOption("{=G8eaX10B}You treat all your workers as potential thieves, having them searched coming and going from the workshop.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100) }, null, null);
			Incident incident105 = this.RegisterIncident("incident_jobs_for_the_lads", "{=0avcvATJ}Jobs for the Lads", "{=cJdoDJ4M}One prominent resident of {TOWN}, {ARTISAN_NAME}, approaches you. Your {WORKSHOP} has been hiring workers who have recently migrated in from the countryside. He requests that you fire them and take on some local boys from the back alleys. If you refuse, he won't be responsible if the carts carrying your shipments get swarmed by traffic going through tight gates, slowing down production.", "EnteringTown", "Workshop", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (!MobileParty.MainParty.CurrentSettlement.IsTown)
				{
					return false;
				}
				Workshop workshop4 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsWorkshop|18_495();
				if (workshop4 == null)
				{
					return false;
				}
				Hero hero7 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsArtisan|18_496();
				if (hero7 == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN", workshop4.Settlement.Name);
				description.SetTextVariable("WORKSHOP", workshop4.Name);
				description.SetTextVariable("ARTISAN_NAME", hero7.Name);
				return true;
			});
			incident105.AddOption("{=bdUzdzIe}Sadly bid farewell to the migrants and wish them well in future endeavors.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.HeroRelationChange(new Func<Hero>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsArtisan|18_496), 10)
			}, null, null);
			string text153 = "{=qoSObD62}Tell {ARTISAN_NAME} to do his worst - you're not firing men who've served you well.";
			List<IncidentEffect> list154 = new List<IncidentEffect>();
			list154.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list154.Add(CampaignIncidentEffects.WorkshopProfitabilityChange(new Func<Workshop>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsWorkshop|18_495), -0.05f));
			incident105.AddOption(text153, list154, delegate(TextObject text)
			{
				Hero hero8 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsArtisan|18_496();
				text.SetTextVariable("ARTISAN_NAME", hero8.Name);
				return true;
			}, null);
			string text154 = "{=TbF5Kc4x}Ask {ARTISAN_NAME} if you can make a sizeable contribution to his guild's feast and funeral fund.";
			List<IncidentEffect> list155 = new List<IncidentEffect>();
			list155.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list155.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list155.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list155.Add(CampaignIncidentEffects.HeroRelationChange(new Func<Hero>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsArtisan|18_496), 10));
			incident105.AddOption(text154, list155, delegate(TextObject text)
			{
				Hero hero9 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetJobsForTheLadsArtisan|18_496();
				text.SetTextVariable("ARTISAN_NAME", hero9.Name);
				return true;
			}, null);
			Incident incident106 = this.RegisterIncident("incident_respect_for_the_slain", "{=EilA5dlq}Respect for the Slain", "{=pNVKWAKo}Throughout your visit to {VILLAGE}, one of your older {TROOP_TYPE} seems moody and downcast. As you march out, he suddenly stops, stares at a roadside stall, then breaks ranks from your party and starts shouting at the vendor. The story quickly comes tumbling out. He served in one of the Empire's old legions in a battle here against the {NEAREST_OTHER_CULTURE}, and the vendor is selling scraps of metal that were clearly looted from the graves of the dead.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				if (MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__HighTierTroopsPredicate|18_503)).ToList<TroopRosterElement>()
					.Sum<TroopRosterElement>((TroopRosterElement x) => x.Number) < 10)
				{
					return false;
				}
				CharacterObject characterObject16 = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetRespectForTheSlainSelectedTroop|18_504();
				if (characterObject16 == null)
				{
					return false;
				}
				CultureObject cultureObject = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetRespectForTheSlainNearestOtherCulture|18_505();
				if (cultureObject == null)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE", MobileParty.MainParty.LastVisitedSettlement.Name);
				description.SetTextVariable("TROOP_TYPE", characterObject16.Name);
				description.SetTextVariable("NEAREST_OTHER_CULTURE", cultureObject.Name);
				return true;
			});
			string text155 = "{=N2Sb9nS6}Purchase what old helmets and shield bosses you can and allow your man to build a small memorial on the battlefield, even though you know it will soon be looted afresh.";
			List<IncidentEffect> list156 = new List<IncidentEffect>();
			list156.Add(CampaignIncidentEffects.GoldChange(() => -200));
			list156.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list156.Add(CampaignIncidentEffects.DisorganizeParty());
			incident106.AddOption(text155, list156, null, null);
			incident106.AddOption("{=fqBRonK9}Do you not also not loot whatever you can from the dead at your victories? Tell your man to save his anger for wrongs done to the living.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			string text156 = "{=WGmKib15}Erupt in fury at the vendor and accuse him of grave desecration. It's unlikely that the thefts will stop, but your men will be pleased.";
			List<IncidentEffect> list157 = new List<IncidentEffect>();
			list157.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list157.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 50));
			list157.Add(CampaignIncidentEffects.RenownChange(2f));
			list157.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -5, null));
			incident106.AddOption(text156, list157, null, null);
			Incident incident107 = this.RegisterIncident("incident_just_rewards", "{=YwstWeDQ}Just Rewards", "{=Jo5OjwAU}Your men fought truly heroically in the last battle. As they help their wounded comrades back to camp and gather the slain, their weary faces sometimes look to you with expectation. If ever there was a time to give them some extra recognition of their valor and sacrifice, this would be it.", "LeavingBattle", "PartyCampLife", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				PlayerBattleEndedLogEntry playerBattleEndedLogEntry4 = Campaign.Current.LogEntryHistory.FindLastGameActionLog<PlayerBattleEndedLogEntry>((PlayerBattleEndedLogEntry x) => true);
				return playerBattleEndedLogEntry4 != null && playerBattleEndedLogEntry4.IsAgainstGreatOdds && Hero.MainHero.Gold >= 2000 && PartyBase.MainParty.MemberRoster.TotalRegulars >= 5;
			});
			string text157 = "{=Wenvx1OE}Reach into your treasury and grab great fistfuls of silver denars, hurling it to your men and praising them by name for their deeds.";
			List<IncidentEffect> list158 = new List<IncidentEffect>();
			list158.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list158.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 200));
			list158.Add(CampaignIncidentEffects.GoldChange(() => -(int)((float)Hero.MainHero.Gold * 0.5f)));
			list158.Add(CampaignIncidentEffects.RenownChange(5f));
			incident107.AddOption(text157, list158, null, null);
			string text158 = "{=w3a39PVT}Give them an extra week's pay as a bonus, telling them they deserve far more but it is all you can afford.";
			List<IncidentEffect> list159 = new List<IncidentEffect>();
			list159.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			list159.Add(CampaignIncidentEffects.GoldChange(() => MobileParty.MainParty.TotalWage * -7));
			incident107.AddOption(text158, list159, null, null);
			incident107.AddOption("{=WnlD7awi}Gruffly tell them that you were pleased they did their duty.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100) }, null, null);
			Incident incident108 = this.RegisterIncident("incident_letter_of_reference", "{=q0HIXEal}Letter of Reference", "{=D7ZcX8XR}As you pass through the market, {NOTABLE.NAME} requests a minute of your time. {?NOTABLE.GENDER}Her{?}His{\\?} cousin is planning on doing some business in {TOWN}, and {?NOTABLE.GENDER}she{?}he{\\?} would like a letter of reference from you. You know next to nothing about the lad, although {NOTABLE.NAME} swears that he is honest and conscientious. You find you are receiving a number of these types of requests as your fame spreads.", "LeavingTown", "Profit", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				Hero hero10 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetLetterOfReferenceNotable|18_516();
				if (hero10 == null || Hero.MainHero.Clan.Renown < 200f)
				{
					return false;
				}
				description.SetCharacterProperties("NOTABLE", hero10.CharacterObject, false);
				description.SetTextVariable("TOWN", hero10.CurrentSettlement.Name);
				return true;
			});
			incident108.AddOption("{=z15Ux7sh}Write truthfully that you don't know much about the bearer of the letter, but he comes from good family.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100) }, null, null);
			incident108.AddOption("{=aZwaFhA6}Write effusively about the lad's qualities.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100),
				CampaignIncidentEffects.HeroRelationChange(new Func<Hero>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetLetterOfReferenceNotable|18_516), 20)
			}, null, null);
			string text159 = "{=8rEX8rjv}Tell {NOTABLE.NAME} that you will write whatever {?NOTABLE.GENDER}she{?}he{\\?} wants, for the proper price.";
			List<IncidentEffect> list160 = new List<IncidentEffect>();
			list160.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list160.Add(CampaignIncidentEffects.GoldChange(() => 300));
			incident108.AddOption(text159, list160, delegate(TextObject text)
			{
				Hero hero11 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetLetterOfReferenceNotable|18_516();
				text.SetCharacterProperties("NOTABLE", hero11.CharacterObject, false);
				return true;
			}, null);
			string text160 = "{=DOPdMPla}Upbraid {NOTABLE.NAME} in the street for all to hear for daring to presume that you would recommend anyone that you do not know.";
			List<IncidentEffect> list161 = new List<IncidentEffect>();
			list161.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 200));
			list161.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100));
			list161.Add(CampaignIncidentEffects.HeroRelationChange(new Func<Hero>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetLetterOfReferenceNotable|18_516), -10));
			incident108.AddOption(text160, list161, delegate(TextObject text)
			{
				Hero hero12 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetLetterOfReferenceNotable|18_516();
				text.SetCharacterProperties("NOTABLE", hero12.CharacterObject, false);
				return true;
			}, null);
			Incident incident109 = this.RegisterIncident("incident_tiller_and_wanderer", "{=OqlihYNW}The Tiller and the Wanderer", "{=nuqL3OAF}As you are leaving your fief you stumble across an altercation. A group of nomads, who pass by annually on their migration from summer to winter pastures, have allowed their herds to graze in your tenants' crops. Your troops far outnumber the nomads and your tenants are urging you to teach them a lesson, but of course their kin might retaliate at a later date.", new string[] { "LeavingTown", "LeavingCastle" }, "FiefManagement", CampaignTime.Years(1000f), (TextObject description) => MobileParty.MainParty.LastVisitedSettlement != null && (MobileParty.MainParty.LastVisitedSettlement.IsFortification && MobileParty.MainParty.LastVisitedSettlement.OwnerClan == Hero.MainHero.Clan) && (MobileParty.MainParty.LastVisitedSettlement.Culture.StringId == "aserai" || MobileParty.MainParty.LastVisitedSettlement.Culture.StringId == "khuzait"));
			string text161 = "{=fI8Bd468}Seize the nomads' herds to compensate you and your tenants for their crops.";
			List<IncidentEffect> list162 = new List<IncidentEffect>();
			list162.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTillerAndWandererHorseItem|18_522), () => 3));
			list162.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list162.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list162.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, 10));
			list162.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 20, null));
			list162.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, -10));
			list162.Add(CampaignIncidentEffects.Custom(null, delegate
			{
				Clan clan = IncidentsCampaignBehaviour.<InitializeIncidents>g__GetTillerAndWandererBanditPartyClan|18_523();
				for (int i = 0; i < 3; i++)
				{
					Hideout hideout = SettlementHelper.FindNearestHideoutToSettlement(MobileParty.MainParty.LastVisitedSettlement, MobileParty.NavigationType.Default, null);
					CampaignVec2 campaignVec3 = NavigationHelper.FindPointAroundPosition(MobileParty.MainParty.LastVisitedSettlement.GatePosition, MobileParty.NavigationType.Default, 5f, 0f, true, false);
					MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("incident_tiller_and_wanderer_bandit_revenge_" + i, clan, hideout, false, null, campaignVec3);
					mobileParty.MemberRoster.AddToCounts(clan.Culture.BanditBandit, MobileParty.MainParty.RandomIntWithSeed((uint)Campaign.Current.IncidentManager.ActiveIncidentSeed, 5, 17), false, 0, 0, true, -1);
					mobileParty.InitializePartyTrade(200);
					mobileParty.SetCustomHomeSettlement(hideout.Settlement);
					mobileParty.Party.SetVisualAsDirty();
				}
				return new List<TextObject>
				{
					new TextObject("{=qxE0YAmo}3 bandit parties spawned nearby.", null)
				};
			}, (IncidentEffect effect) => new IncidentHint(new TextObject("{=ayfKQixq}3 bandit parties spawn nearby", null), IncidentHintType.Effect)));
			incident109.AddOption(text161, list162, null, null);
			string text162 = "{=ciQPfxEC}Demand that the nomads pay fair compensation for damaged crops, even though you know this will not settle the problem for long.";
			List<IncidentEffect> list163 = new List<IncidentEffect>();
			list163.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list163.Add(CampaignIncidentEffects.TownSecurityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, -5));
			list163.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, 10, null));
			incident109.AddOption(text162, list163, null, null);
			string text163 = "{=fkQj3TIP}Tell the nomads that proud warriors such as themselves need not worry what these dirt-diggers think. Share a meal with them, and see if some will join you.";
			List<IncidentEffect> list164 = new List<IncidentEffect>();
			list164.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list164.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list164.Add(CampaignIncidentEffects.ChangeTroopAmount(new Func<CharacterObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetTillerAndWandererTroop|18_524), 4, null, null));
			list164.Add(CampaignIncidentEffects.SettlementRelationChange(() => MobileParty.MainParty.LastVisitedSettlement, -10, null));
			list164.Add(CampaignIncidentEffects.TownProsperityChange(() => MobileParty.MainParty.LastVisitedSettlement.Town, -10));
			incident109.AddOption(text163, list164, null, null);
			Incident incident110 = this.RegisterIncident("incident_cries_in_the_mist", "{=ELXomm1C}Cries in the Mist", "{=SAXnFQLq}The people of {VILLAGE_NAME} warned you to take care outside their village. When the mist gathers, The woods will be haunted by spirits whose shriek foretells death. Now the gloom is gathering, and you hear mutterings from your men. You are certain that they will soon be hearing wailing and seeing apparitions, real or imagined. The horses are becoming twitchy too, picking up on the mood of the men, and accidents are likely to happen.", "LeavingVillage", "DreamsSongsAndSigns", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsVillage)
				{
					return false;
				}
				Village village9 = MobileParty.MainParty.LastVisitedSettlement.Village;
				bool flag7 = false;
				for (float num10 = -3f; num10 <= 3f; num10 += 0.25f)
				{
					for (float num11 = -3f; num11 <= 3f; num11 += 0.25f)
					{
						if (num10 * num10 + num11 * num11 <= 9f)
						{
							CampaignVec2 campaignVec4 = new CampaignVec2(village9.Settlement.Position.ToVec2() + new Vec2(num10, num11), true);
							if (campaignVec4.Face.IsValid() && Campaign.Current.MapSceneWrapper.GetFaceTerrainType(campaignVec4.Face) == TerrainType.Forest)
							{
								flag7 = true;
								break;
							}
						}
					}
					if (flag7)
					{
						break;
					}
				}
				if (!flag7)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE_NAME", village9.Name);
				return true;
			});
			string text164 = "{=uD05twgi}Tell your men to ignore the superstitions of villagers, and press on.";
			List<IncidentEffect> list165 = new List<IncidentEffect>();
			list165.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100));
			list165.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100));
			list165.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100));
			list165.Add(CampaignIncidentEffects.MoraleChange(-20f));
			list165.Add(CampaignIncidentEffects.WoundTroopsRandomly((TroopRosterElement x) => x.Character.IsMounted, () => Math.Min(MobileParty.MainParty.RandomIntWithSeed((uint)Campaign.Current.IncidentManager.ActiveIncidentSeed, 1, 6), (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where x.Character.IsMounted
				select x).Sum<TroopRosterElement>((TroopRosterElement x) => x.Number)), true).WithChance(0.5f));
			incident110.AddOption(text164, list165, null, null);
			incident110.AddOption("{=BxpJKMYq}Dispel the spirits in the approved orthodox Calradian way, by loudly calling on Heaven to ward off the ghosts of Below.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Valor, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100)
			}, null, null);
			incident110.AddOption("{=4Ef7xG1o}Propitiate the spirits as the villagers advised, by slaughtering a black cockerel that they sold one of your men.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -100)
			}, null, null);
			Incident incident111 = this.RegisterIncident("incident_feeling_the_bite", "{=0yKJ0J0N}Feeling the Bite", "{=D9T64ejr}As you leave {VILLAGE_NAME}, several of the villagers approach you. They warn you that stagnant ponds from the last rain have yet to fully dry and have bred mosquitos. They offer to sell you a concoction of oils including rosemary, catmint and other herbs that they say will ward off all flying vermin.", "LeavingVillage", "HardTravel", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null)
				{
					return false;
				}
				CampaignTime.Seasons getSeasonOfYear = CampaignTime.Now.GetSeasonOfYear;
				bool flag8 = getSeasonOfYear == CampaignTime.Seasons.Spring || getSeasonOfYear == CampaignTime.Seasons.Summer;
				description.SetTextVariable("VILLAGE_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				return flag8;
			});
			string text165 = "{=zGVNqFnh}Buy vials of ointment for every member of your party, at the risk of looking gullible.";
			List<IncidentEffect> list166 = new List<IncidentEffect>();
			list166.Add(CampaignIncidentEffects.GoldChange(() => -100));
			list166.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, -50));
			list166.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100));
			incident111.AddOption(text165, list166, null, null);
			incident111.AddOption("{=qCoqaa70}Have your men wrap themselves up as best they can and press on.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -50) }, null, null);
			incident111.AddOption("{=VccCV5dX}Take circuitous routs to avoid the ponds.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.DisorganizeParty(),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 100)
			}, null, null);
			Incident incident112 = this.RegisterIncident("incident_nearly_a_gift_horse", "{=N19m9qmd}Nearly a Gift Horse", "{=WR7NCetE}As you leave {VILLAGE_NAME}, a scraggly young man waves to attract your attention. He leads a magnificent {RARE_HORSE}, and says that he will sell it to you for a mere 500 denars. He claims he found it wandering on a heath, almost dead from exhaustion and neglect, and he nursed it back to health. He won't go into town because he says he would be unfairly accused of theft.", "LeavingVillage", "TroopSettlementRelation", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (MobileParty.MainParty.LastVisitedSettlement == null || !MobileParty.MainParty.LastVisitedSettlement.IsVillage)
				{
					return false;
				}
				ItemObject itemObject4 = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetNearlyAGiftHorseHorse|18_546();
				if (itemObject4 == null)
				{
					return false;
				}
				description.SetTextVariable("VILLAGE_NAME", MobileParty.MainParty.LastVisitedSettlement.Name);
				description.SetTextVariable("RARE_HORSE", itemObject4.Name);
				description.SetTextVariable("TOWN_NAME", MobileParty.MainParty.LastVisitedSettlement.Village.Bound.Name);
				return true;
			});
			string text166 = "{=YVETzHXK}Plausible enough, and you have no time to check the provenance of every bargain you find. Buy the horse!";
			List<IncidentEffect> list167 = new List<IncidentEffect>();
			list167.Add(CampaignIncidentEffects.GoldChange(() => -500));
			list167.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -50));
			list167.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetNearlyAGiftHorseHorse|18_546), () => 1));
			incident112.AddOption(text166, list167, null, null);
			string text167 = "{=B9HD9PNw}A likely story... Place the man under arrest and send word to the authorities of {TOWN_NAME}, who have the time to sort things out.";
			List<IncidentEffect> list168 = new List<IncidentEffect>();
			list168.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, 100));
			list168.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -50));
			list168.Add(CampaignIncidentEffects.DisorganizeParty());
			incident112.AddOption(text167, list168, delegate(TextObject text)
			{
				Settlement lastVisitedSettlement4 = MobileParty.MainParty.LastVisitedSettlement;
				text.SetTextVariable("TOWN_NAME", lastVisitedSettlement4.Village.Bound.Name);
				return true;
			}, null);
			incident112.AddOption("{=CyM5bAdB}You're not sure you trust the seller, but you're not handing over to officials who might just hang him and seize the horse themselves. Go on your way.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, 100) }, null, null);
			string text168 = "{=AwBJzppu}Take the horse without paying. Tell the seller that thieves have no right to complain of theft.";
			List<IncidentEffect> list169 = new List<IncidentEffect>();
			list169.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Mercy, -100));
			list169.Add(CampaignIncidentEffects.TraitChange(DefaultTraits.Honor, -100));
			list169.Add(CampaignIncidentEffects.ChangeItemAmount(new Func<ItemObject>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetNearlyAGiftHorseHorse|18_546), () => 1));
			incident112.AddOption(text168, list169, null, null);
			Incident incident113 = this.RegisterIncident("incident_hammer_of_the_sun", "{=R80p9Ywt}Hammer of the Sun", "{=th5MgyAZ}As you enter {TOWN_NAME}, one of your {ASERAI_TROOPS} approaches you. Many of his comrades are not used to the heat of the desert, he says. They march after drinking wine through the evening but are not careful to properly take water throughout the day, as the sun beats down on them. He has seen men collapse and die from such carelessness and ignorance.", "EnteringTown", "HardTravel", CampaignTime.Years(1000f), delegate(TextObject description)
			{
				if (!MobileParty.MainParty.Position.IsValid())
				{
					return false;
				}
				if (Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.Position.Face) != TerrainType.Desert || MobileParty.MainParty.MemberRoster.TotalRegulars < 50)
				{
					return false;
				}
				Settlement currentSettlement2 = MobileParty.MainParty.CurrentSettlement;
				MBList<TroopRosterElement> troopRoster = MobileParty.MainParty.MemberRoster.GetTroopRoster();
				if (troopRoster.Count<TroopRosterElement>((TroopRosterElement x) => x.Character.Culture.StringId != "aserai" && !x.Character.IsHero) < 10)
				{
					return false;
				}
				CharacterObject character3 = IncidentHelper.GetSeededRandomElement<TroopRosterElement>(troopRoster.Where<TroopRosterElement>((TroopRosterElement x) => x.Character.Culture.StringId == "aserai" && !x.Character.IsHero).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
				if (character3 == null)
				{
					return false;
				}
				description.SetTextVariable("TOWN_NAME", currentSettlement2.Name);
				description.SetTextVariable("ASERAI_TROOPS", character3.Name);
				return true;
			});
			incident113.AddOption("{=lbSsRg8w}Your men will resent any curbs on their celebrations, even for their own health. Warn them but take no other precautions.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 100),
				CampaignIncidentEffects.KillTroopsRandomlyByChance(0.01f)
			}, null, null);
			incident113.AddOption("{=3Uub2e17}Tell your men that there shall be no wine tonight, and none henceforth until they learn how to handle themselves in the desert.", new List<IncidentEffect> { CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, -100) }, null, null);
			incident113.AddOption("{=cG9pkbqo}Let them men celebrate, but move slowly in the morning, pausing to make sure that each man is drinking from his water-skin.", new List<IncidentEffect>
			{
				CampaignIncidentEffects.TraitChange(DefaultTraits.Generosity, 50),
				CampaignIncidentEffects.TraitChange(DefaultTraits.Calculating, 50),
				CampaignIncidentEffects.DisorganizeParty(),
				CampaignIncidentEffects.WoundTroopsRandomlyByChance(0.01f)
			}, null, null);
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x0013D204 File Offset: 0x0013B404
		[CompilerGenerated]
		internal static BesiegeSettlementLogEntry <InitializeIncidents>g__FindLastBesiegeSettlementLogEntry|18_103()
		{
			return Campaign.Current.LogEntryHistory.FindLastGameActionLog<BesiegeSettlementLogEntry>((BesiegeSettlementLogEntry x) => x.BesiegerHero == Hero.MainHero && x.Settlement == MobileParty.MainParty.LastVisitedSettlement);
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0013D234 File Offset: 0x0013B434
		[CompilerGenerated]
		internal static SiegeAftermathLogEntry <InitializeIncidents>g__FindLastSiegeAftermathLogEntry|18_104(BesiegeSettlementLogEntry siegeLog)
		{
			return Campaign.Current.LogEntryHistory.FindLastGameActionLog<SiegeAftermathLogEntry>((SiegeAftermathLogEntry x) => x.PlayerWasInvolved && x.SiegeAftermath == SiegeAftermathAction.SiegeAftermath.ShowMercy && x.CapturedSettlement == siegeLog.Settlement);
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0013D26C File Offset: 0x0013B46C
		[CompilerGenerated]
		internal static CharacterObject <InitializeIncidents>g__GetWantedCriminalTroop|18_129()
		{
			List<TroopRosterElement> list = (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where !x.Character.IsHero
				select x).ToList<TroopRosterElement>();
			if (list.Count == 0)
			{
				return null;
			}
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>(list, Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0013D2D6 File Offset: 0x0013B4D6
		[CompilerGenerated]
		internal static CharacterObject <InitializeIncidents>g__GetSoldierInDebtTroop|18_139()
		{
			return MobileParty.MainParty.MemberRoster.GetTroopRoster().GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement x) => !x.Character.IsHero).Character;
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0013D310 File Offset: 0x0013B510
		[CompilerGenerated]
		internal static TroopRosterElement <InitializeIncidents>g__GetHonorTheSlainFoeRandomTroop|18_170()
		{
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>((from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where !x.Character.IsHero
				select x).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed);
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x0013D36C File Offset: 0x0013B56C
		[CompilerGenerated]
		internal static CharacterObject <InitializeIncidents>g__GetNoMoodForMercyRandomTroop|18_190()
		{
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>((from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where !x.Character.IsHero
				select x).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0013D3CC File Offset: 0x0013B5CC
		[CompilerGenerated]
		internal static Village <InitializeIncidents>g__GetMisplacedVengeanceLootedVillage|18_196()
		{
			Village village = null;
			float num = float.MaxValue;
			foreach (Village village2 in Village.All)
			{
				if (village2.VillageState == Village.VillageStates.Looted && Hero.MainHero.MapFaction.IsAtWarWith(village2.Settlement.MapFaction))
				{
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty.LastVisitedSettlement.Village.Settlement, village2.Settlement, false, false, MobileParty.NavigationType.All);
					if (distance <= num && distance <= 1000f)
					{
						village = village2;
						num = distance;
					}
				}
			}
			return village;
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0013D48C File Offset: 0x0013B68C
		[CompilerGenerated]
		internal static ItemRosterElement <InitializeIncidents>g__GetTradeProposalGood|18_208()
		{
			return IncidentHelper.GetSeededRandomElement<ItemRosterElement>(MobileParty.MainParty.ItemRoster.Where<ItemRosterElement>((ItemRosterElement x) => x.EquipmentElement.Item.IsTradeGood).ToList<ItemRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed);
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0013D4E0 File Offset: 0x0013B6E0
		[CompilerGenerated]
		internal static Hero <InitializeIncidents>g__GetIntriguingRumorsRivalLord|18_228()
		{
			Kingdom kingdom = Hero.MainHero.Clan.Kingdom;
			List<Hero> list;
			if (kingdom == null)
			{
				list = null;
			}
			else
			{
				list = kingdom.AliveLords.Where<Hero>((Hero lord) => lord != Hero.MainHero && Hero.MainHero.GetRelation(lord) < 0).ToList<Hero>();
			}
			return IncidentHelper.GetSeededRandomElement<Hero>(list, Campaign.Current.IncidentManager.ActiveIncidentSeed);
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x0013D548 File Offset: 0x0013B748
		[CompilerGenerated]
		internal static CharacterObject <InitializeIncidents>g__GetNobleTroop|18_245(CultureObject culture)
		{
			List<TroopRosterElement> list = (from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where !x.Character.IsHero && x.Character.Tier >= 4 && x.Character.IsMounted && x.Number > 0 && x.Character.Culture == culture && CharacterHelper.GetTroopTree(culture.EliteBasicTroop, -1f, float.MaxValue).Contains(x.Character)
				select x).ToList<TroopRosterElement>();
			if (list.Count == 0)
			{
				return null;
			}
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>(list, Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0013D5AC File Offset: 0x0013B7AC
		[CompilerGenerated]
		internal static Village <InitializeIncidents>g__GetOtherVillage|18_311()
		{
			MBReadOnlyList<Village> villages = MobileParty.MainParty.LastVisitedSettlement.Town.Villages;
			Village village = IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__GetVillageOne|18_310();
			Village village2 = null;
			float num = float.MaxValue;
			foreach (Village village3 in Village.All)
			{
				if (!villages.Contains(village3))
				{
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(village.Settlement, village3.Settlement, false, false, MobileParty.NavigationType.Default);
					if (distance < num && village3.Settlement.OwnerClan != Clan.PlayerClan)
					{
						village2 = village3;
						num = distance;
					}
				}
			}
			return village2;
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0013D670 File Offset: 0x0013B870
		[CompilerGenerated]
		internal static TroopRosterElement <InitializeIncidents>g__GetRandomTroop|18_401()
		{
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>((from x in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where !x.Character.IsHero
				select x).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed);
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x0013D6CC File Offset: 0x0013B8CC
		[CompilerGenerated]
		internal static List<Settlement> <InitializeIncidents>g__GetNearbyVillages|18_417()
		{
			return Settlement.All.Where<Settlement>((Settlement s) => s.IsVillage).OrderBy<Settlement, float>(delegate(Settlement s)
			{
				float num;
				return Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, s, false, MobileParty.NavigationType.All, out num);
			}).Take<Settlement>(3)
				.ToList<Settlement>();
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0013D731 File Offset: 0x0013B931
		[CompilerGenerated]
		internal static TroopRosterElement <InitializeIncidents>g__GetRandomCavalryTroop|18_434()
		{
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>(MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__CavalryTroopPredicate|18_433)).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed);
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0013D770 File Offset: 0x0013B970
		[CompilerGenerated]
		internal static CharacterObject <InitializeIncidents>g__GetRespectForTheSlainSelectedTroop|18_504()
		{
			return IncidentHelper.GetSeededRandomElement<TroopRosterElement>(MobileParty.MainParty.MemberRoster.GetTroopRoster().Where<TroopRosterElement>(new Func<TroopRosterElement, bool>(IncidentsCampaignBehaviour.<>c.<>9.<InitializeIncidents>g__HighTierTroopsPredicate|18_503)).ToList<TroopRosterElement>(), Campaign.Current.IncidentManager.ActiveIncidentSeed).Character;
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x0013D7C0 File Offset: 0x0013B9C0
		[CompilerGenerated]
		internal static CultureObject <InitializeIncidents>g__GetRespectForTheSlainNearestOtherCulture|18_505()
		{
			Settlement settlement = (from s in Settlement.All
				where s.Culture != MobileParty.MainParty.LastVisitedSettlement.Culture
				orderby Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty.LastVisitedSettlement, s, false, false, MobileParty.NavigationType.All)
				select s).FirstOrDefault<Settlement>();
			if (settlement == null)
			{
				return null;
			}
			return settlement.Culture;
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0013D82C File Offset: 0x0013BA2C
		[CompilerGenerated]
		internal static Clan <InitializeIncidents>g__GetTillerAndWandererBanditPartyClan|18_523()
		{
			if (MobileParty.MainParty.RandomFloatWithSeed((uint)Campaign.Current.IncidentManager.ActiveIncidentSeed) >= 0.5f)
			{
				return Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "steppe_bandits");
			}
			return Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "desert_bandits");
		}

		// Token: 0x040013EB RID: 5099
		private bool _canInvokeSettlementEvent;

		// Token: 0x02000855 RID: 2133
		public static class IncidentType
		{
			// Token: 0x04002209 RID: 8713
			public const string TroopSettlementRelation = "TroopSettlementRelation";

			// Token: 0x0400220A RID: 8714
			public const string FoodConsumption = "FoodConsumption";

			// Token: 0x0400220B RID: 8715
			public const string PlightOfCivilians = "PlightOfCivilians";

			// Token: 0x0400220C RID: 8716
			public const string PartyCampLife = "PartyCampLife";

			// Token: 0x0400220D RID: 8717
			public const string AnimalIllness = "AnimalIllness";

			// Token: 0x0400220E RID: 8718
			public const string Illness = "Illness";

			// Token: 0x0400220F RID: 8719
			public const string HuntingForaging = "HuntingForaging";

			// Token: 0x04002210 RID: 8720
			public const string PostBattle = "PostBattle";

			// Token: 0x04002211 RID: 8721
			public const string HardTravel = "HardTravel";

			// Token: 0x04002212 RID: 8722
			public const string Profit = "Profit";

			// Token: 0x04002213 RID: 8723
			public const string DreamsSongsAndSigns = "DreamsSongsAndSigns";

			// Token: 0x04002214 RID: 8724
			public const string FiefManagement = "FiefManagement";

			// Token: 0x04002215 RID: 8725
			public const string Siege = "Siege";

			// Token: 0x04002216 RID: 8726
			public const string Workshop = "Workshop";
		}

		// Token: 0x02000856 RID: 2134
		public static class IncidentTrigger
		{
			// Token: 0x04002217 RID: 8727
			public const string LeavingVillage = "LeavingVillage";

			// Token: 0x04002218 RID: 8728
			public const string LeavingTown = "LeavingTown";

			// Token: 0x04002219 RID: 8729
			public const string LeavingCastle = "LeavingCastle";

			// Token: 0x0400221A RID: 8730
			public const string LeavingEncounter = "LeavingEncounter";

			// Token: 0x0400221B RID: 8731
			public const string LeavingBattle = "LeavingBattle";

			// Token: 0x0400221C RID: 8732
			public const string EnteringVillage = "EnteringVillage";

			// Token: 0x0400221D RID: 8733
			public const string EnteringTown = "EnteringTown";

			// Token: 0x0400221E RID: 8734
			public const string EnteringCastle = "EnteringCastle";

			// Token: 0x0400221F RID: 8735
			public const string WaitingInSettlement = "WaitingInSettlement";

			// Token: 0x04002220 RID: 8736
			public const string DuringSiege = "DuringSiege";
		}
	}
}
