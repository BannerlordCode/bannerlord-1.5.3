using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BA RID: 186
	[MenuOverlay("ArmyMenuOverlay")]
	public class ArmyMenuOverlayVM : GameMenuOverlay
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x00047452 File Offset: 0x00045652
		private Army ArmyToUse
		{
			get
			{
				MobileParty mainParty = MobileParty.MainParty;
				Army army;
				if ((army = ((mainParty != null) ? mainParty.Army : null)) == null)
				{
					MobileParty mainParty2 = MobileParty.MainParty;
					if (mainParty2 == null)
					{
						return null;
					}
					MobileParty targetParty = mainParty2.TargetParty;
					if (targetParty == null)
					{
						return null;
					}
					army = targetParty.Army;
				}
				return army;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00047484 File Offset: 0x00045684
		public ArmyMenuOverlayVM()
		{
			this.PartyList = new MBBindingList<GameMenuPartyItemVM>();
			base.CurrentOverlayType = 2;
			base.IsInitializationOver = false;
			this.CohesionHint = new BasicTooltipViewModel();
			this.ManCountHint = new BasicTooltipViewModel();
			this.FoodHint = new BasicTooltipViewModel();
			this.TutorialNotification = new ElementNotificationVM();
			this.ManageArmyHint = new HintViewModel();
			this.Refresh();
			this._contextMenuItem = null;
			CampaignEvents.ArmyOverlaySetDirtyEvent.AddNonSerializedListener(this, new Action(this.Refresh));
			CampaignEvents.PartyAttachedAnotherParty.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyAttachedAnotherParty));
			CampaignEvents.OnTroopRecruitedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, Hero, CharacterObject, int>(this.OnTroopRecruited));
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this._cohesionConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_army_cohesion");
			base.IsInitializationOver = true;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0004758B File Offset: 0x0004578B
		public override void RefreshValues()
		{
			base.RefreshValues();
			ElementNotificationVM tutorialNotification = this.TutorialNotification;
			if (tutorialNotification != null)
			{
				tutorialNotification.RefreshValues();
			}
			this.Refresh();
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000475AC File Offset: 0x000457AC
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.ArmyOverlaySetDirtyEvent.ClearListeners(this);
			CampaignEvents.PartyAttachedAnotherParty.ClearListeners(this);
			CampaignEvents.OnTroopRecruitedEvent.ClearListeners(this);
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000475FC File Offset: 0x000457FC
		protected override void ExecuteOnSetAsActiveContextMenuItem(GameMenuPartyItemVM troop)
		{
			base.ExecuteOnSetAsActiveContextMenuItem(troop);
			base.ContextList.Clear();
			MobileParty mobileParty = this._contextMenuItem.Party.MobileParty;
			if (((mobileParty != null) ? mobileParty.Army : null) != null && ArmyMenuOverlayVM.GetIsPlayerArmyLeader(this._contextMenuItem.Party.MobileParty.Army) && this._contextMenuItem.Party.MapEvent == null && this._contextMenuItem.Party != this._contextMenuItem.Party.MobileParty.Army.LeaderParty.Party)
			{
				TextObject textObject;
				bool mapScreenActionIsEnabledWithReason = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject);
				base.ContextList.Add(new StringItemWithEnabledAndHintVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", GameMenuOverlay.MenuOverlayContextList.ArmyDismiss.ToString()).ToString(), mapScreenActionIsEnabledWithReason, GameMenuOverlay.MenuOverlayContextList.ArmyDismiss, textObject));
			}
			float getEncounterJoiningRadius = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
			MapEvent mapEvent = MobileParty.MainParty.MapEvent;
			CampaignVec2 campaignVec = ((mapEvent != null) ? mapEvent.Position : MobileParty.MainParty.Position);
			MobileParty mobileParty2 = troop.Party.MobileParty;
			float? num = ((mobileParty2 != null) ? new float?(mobileParty2.Position.DistanceSquared(campaignVec)) : null);
			float num2 = getEncounterJoiningRadius * getEncounterJoiningRadius;
			bool flag = (num.GetValueOrDefault() < num2) & (num != null);
			bool flag2 = troop.Party.MobileParty.MapEvent == MobileParty.MainParty.MapEvent;
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			bool flag3;
			if (encounteredParty == null)
			{
				flag3 = false;
			}
			else
			{
				IFaction mapFaction = encounteredParty.MapFaction;
				bool? flag4 = ((mapFaction != null) ? new bool?(mapFaction.IsAtWarWith(Hero.MainHero.MapFaction)) : null);
				bool flag5 = true;
				flag3 = (flag4.GetValueOrDefault() == flag5) & (flag4 != null);
			}
			bool flag6 = flag3;
			if (this._contextMenuItem.Party.LeaderHero != null && flag && flag2 && !flag6 && this._contextMenuItem.Party != PartyBase.MainParty)
			{
				PlayerEncounter playerEncounter = PlayerEncounter.Current;
				if (((playerEncounter != null) ? playerEncounter.BattleSimulation : null) == null)
				{
					base.ContextList.Add(new StringItemWithEnabledAndHintVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", GameMenuOverlay.MenuOverlayContextList.DonateTroops.ToString()).ToString(), true, GameMenuOverlay.MenuOverlayContextList.DonateTroops, null));
					if (MobileParty.MainParty.CurrentSettlement == null && LocationComplex.Current == null)
					{
						base.ContextList.Add(new StringItemWithEnabledAndHintVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", GameMenuOverlay.MenuOverlayContextList.ConverseWithLeader.ToString()).ToString(), true, GameMenuOverlay.MenuOverlayContextList.ConverseWithLeader, null));
					}
				}
			}
			base.ContextList.Add(new StringItemWithEnabledAndHintVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", GameMenuOverlay.MenuOverlayContextList.Encyclopedia.ToString()).ToString(), true, GameMenuOverlay.MenuOverlayContextList.Encyclopedia, null));
			CharacterObject characterObject;
			if ((characterObject = this._contextMenuItem.Character) == null)
			{
				Hero leaderHero = this._contextMenuItem.Party.LeaderHero;
				characterObject = ((leaderHero != null) ? leaderHero.CharacterObject : null);
			}
			CharacterObject characterObject2 = characterObject;
			if (characterObject2 == null)
			{
				Debug.FailedAssert("ArmyMenuOverlayVM.ExecuteOnSetAsActiveContextMenuItem called on party with no leader hero: " + this._contextMenuItem.Party.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\ArmyMenuOverlayVM.cs", "ExecuteOnSetAsActiveContextMenuItem", 124);
				return;
			}
			CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpOpened(characterObject2);
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00047968 File Offset: 0x00045B68
		public override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			TextObject textObject;
			this.CanManageArmy = CampaignUIHelper.GetCanManageCurrentArmyWithReason(out textObject);
			this.ManageArmyHint.HintText = textObject;
			for (int i = 0; i < this.PartyList.Count; i++)
			{
				this.PartyList[i].RefreshQuestStatus();
			}
			if (this._isVisualsDirty)
			{
				this.RefreshVisualsOfItems();
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000479D1 File Offset: 0x00045BD1
		public sealed override void Refresh()
		{
			if (this.ArmyToUse != null)
			{
				base.IsInitializationOver = false;
				this.UpdateLists();
				this.UpdateProperties();
				base.IsInitializationOver = true;
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000479F8 File Offset: 0x00045BF8
		private void UpdateProperties()
		{
			MBTextManager.SetTextVariable("newline", "\n", false);
			Army army = this.ArmyToUse;
			if (army == null)
			{
				Debug.FailedAssert("Army is null but trying to update army overlay properties", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\ArmyMenuOverlayVM.cs", "UpdateProperties", 169);
				return;
			}
			float num = army.LeaderParty.Food;
			foreach (MobileParty mobileParty in army.LeaderParty.AttachedParties)
			{
				num += mobileParty.Food;
			}
			this.Food = (int)num;
			this.Cohesion = (int)army.Cohesion;
			this.ManCountText = CampaignUIHelper.GetPartyNameplateText(army.LeaderParty, true);
			this.FoodHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetArmyFoodTooltip(army));
			this.CohesionHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetArmyCohesionTooltip(army));
			this.ManCountHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetArmyManCountTooltip(army));
			this.IsCohesionWarningEnabled = army.Cohesion <= 30f;
			this.IsPlayerArmyLeader = ArmyMenuOverlayVM.GetIsPlayerArmyLeader(army);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00047B50 File Offset: 0x00045D50
		private void UpdateLists()
		{
			Army armyToUse = this.ArmyToUse;
			if (armyToUse == null)
			{
				Debug.FailedAssert("Army is null but trying to update army overlay lists", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\ArmyMenuOverlayVM.cs", "UpdateLists", 198);
				return;
			}
			for (int i = this.PartyList.Count - 1; i >= 0; i--)
			{
				GameMenuPartyItemVM partyVM = this.PartyList[i];
				if (!armyToUse.Parties.Any<MobileParty>((MobileParty p) => p.Party == partyVM.Party))
				{
					this.PartyList.RemoveAt(i);
				}
			}
			for (int j = 0; j < armyToUse.Parties.Count; j++)
			{
				MobileParty party = armyToUse.Parties[j];
				if (!this.PartyList.Any<GameMenuPartyItemVM>((GameMenuPartyItemVM p) => p.Party == party.Party))
				{
					bool flag = party == armyToUse.LeaderParty;
					GameMenuPartyItemVM gameMenuPartyItemVM = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), party.Party, true)
					{
						IsLeader = flag
					};
					if (flag)
					{
						this.PartyList.Insert(0, gameMenuPartyItemVM);
					}
					else
					{
						this.PartyList.Add(gameMenuPartyItemVM);
					}
				}
			}
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM2 in this.PartyList)
			{
				gameMenuPartyItemVM2.RefreshProperties();
			}
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00047CC4 File Offset: 0x00045EC4
		public void ExecuteOpenArmyManagement()
		{
			Army armyToUse = this.ArmyToUse;
			if (armyToUse != null && ArmyMenuOverlayVM.GetIsPlayerArmyLeader(armyToUse))
			{
				Action openArmyManagement = this.OpenArmyManagement;
				if (openArmyManagement == null)
				{
					return;
				}
				openArmyManagement();
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00047CF3 File Offset: 0x00045EF3
		private void ExecuteCohesionLink()
		{
			if (this._cohesionConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._cohesionConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Cohesion encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\ArmyMenuOverlayVM.cs", "ExecuteCohesionLink", 257);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00047D34 File Offset: 0x00045F34
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
				}
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00047D94 File Offset: 0x00045F94
		private void RefreshVisualsOfItems()
		{
			for (int i = 0; i < this.PartyList.Count; i++)
			{
				this.PartyList[i].RefreshVisual();
			}
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00047DC8 File Offset: 0x00045FC8
		private void OnPartyAttachedAnotherParty(MobileParty party)
		{
			MobileParty attachedTo = party.AttachedTo;
			if (((attachedTo != null) ? attachedTo.Army : null) != null && party.AttachedTo.Army == MobileParty.MainParty.Army)
			{
				this._isVisualsDirty = true;
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00047DFC File Offset: 0x00045FFC
		private void OnTroopRecruited(Hero recruiterHero, Settlement settlement, Hero troopSource, CharacterObject troop, int number)
		{
			if (((recruiterHero != null) ? recruiterHero.PartyBelongedTo : null) != null && recruiterHero.IsPartyLeader)
			{
				for (int i = 0; i < this.PartyList.Count; i++)
				{
					if (this.PartyList[i].Party == recruiterHero.PartyBelongedTo.Party)
					{
						this.PartyList[i].RefreshProperties();
						return;
					}
				}
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00047E65 File Offset: 0x00046065
		private static bool GetIsPlayerArmyLeader(Army army)
		{
			return army.LeaderParty == MobileParty.MainParty;
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x00047E74 File Offset: 0x00046074
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x00047E7C File Offset: 0x0004607C
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x00047E9A File Offset: 0x0004609A
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x00047EA2 File Offset: 0x000460A2
		[DataSourceProperty]
		public HintViewModel ManageArmyHint
		{
			get
			{
				return this._manageArmyHint;
			}
			set
			{
				if (value != this._manageArmyHint)
				{
					this._manageArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ManageArmyHint");
				}
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x00047EC0 File Offset: 0x000460C0
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00047EC8 File Offset: 0x000460C8
		[DataSourceProperty]
		public int Cohesion
		{
			get
			{
				return this._cohesion;
			}
			set
			{
				if (value != this._cohesion)
				{
					this._cohesion = value;
					base.OnPropertyChangedWithValue(value, "Cohesion");
				}
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00047EE6 File Offset: 0x000460E6
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00047EEE File Offset: 0x000460EE
		[DataSourceProperty]
		public bool IsCohesionWarningEnabled
		{
			get
			{
				return this._isCohesionWarningEnabled;
			}
			set
			{
				if (value != this._isCohesionWarningEnabled)
				{
					this._isCohesionWarningEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCohesionWarningEnabled");
				}
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00047F0C File Offset: 0x0004610C
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00047F14 File Offset: 0x00046114
		[DataSourceProperty]
		public bool CanManageArmy
		{
			get
			{
				return this._canManageArmy;
			}
			set
			{
				if (value != this._canManageArmy)
				{
					this._canManageArmy = value;
					base.OnPropertyChangedWithValue(value, "CanManageArmy");
				}
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00047F32 File Offset: 0x00046132
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00047F3A File Offset: 0x0004613A
		[DataSourceProperty]
		public bool IsPlayerArmyLeader
		{
			get
			{
				return this._isPlayerArmyLeader;
			}
			set
			{
				if (value != this._isPlayerArmyLeader)
				{
					this._isPlayerArmyLeader = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerArmyLeader");
				}
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00047F58 File Offset: 0x00046158
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x00047F60 File Offset: 0x00046160
		[DataSourceProperty]
		public string ManCountText
		{
			get
			{
				return this._manCountText;
			}
			set
			{
				if (value != this._manCountText)
				{
					this._manCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManCountText");
				}
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x00047F83 File Offset: 0x00046183
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x00047F8B File Offset: 0x0004618B
		[DataSourceProperty]
		public int Food
		{
			get
			{
				return this._food;
			}
			set
			{
				if (value != this._food)
				{
					this._food = value;
					base.OnPropertyChangedWithValue(value, "Food");
				}
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x00047FA9 File Offset: 0x000461A9
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x00047FB1 File Offset: 0x000461B1
		[DataSourceProperty]
		public MBBindingList<GameMenuPartyItemVM> PartyList
		{
			get
			{
				return this._partyList;
			}
			set
			{
				if (value != this._partyList)
				{
					this._partyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuPartyItemVM>>(value, "PartyList");
				}
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x00047FCF File Offset: 0x000461CF
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x00047FD7 File Offset: 0x000461D7
		[DataSourceProperty]
		public BasicTooltipViewModel CohesionHint
		{
			get
			{
				return this._cohesionHint;
			}
			set
			{
				if (value != this._cohesionHint)
				{
					this._cohesionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CohesionHint");
				}
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x00047FF5 File Offset: 0x000461F5
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x00047FFD File Offset: 0x000461FD
		[DataSourceProperty]
		public BasicTooltipViewModel ManCountHint
		{
			get
			{
				return this._manCountHint;
			}
			set
			{
				if (value != this._manCountHint)
				{
					this._manCountHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ManCountHint");
				}
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0004801B File Offset: 0x0004621B
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x00048023 File Offset: 0x00046223
		[DataSourceProperty]
		public BasicTooltipViewModel FoodHint
		{
			get
			{
				return this._foodHint;
			}
			set
			{
				if (value != this._foodHint)
				{
					this._foodHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "FoodHint");
				}
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x00048041 File Offset: 0x00046241
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> IssueList
		{
			get
			{
				if (this._issueList == null)
				{
					this._issueList = new MBBindingList<StringItemWithHintVM>();
				}
				return this._issueList;
			}
		}

		// Token: 0x04000820 RID: 2080
		private const float CohesionWarningMin = 30f;

		// Token: 0x04000821 RID: 2081
		public Action OpenArmyManagement;

		// Token: 0x04000822 RID: 2082
		private readonly Concept _cohesionConceptObj;

		// Token: 0x04000823 RID: 2083
		private string _latestTutorialElementID;

		// Token: 0x04000824 RID: 2084
		private bool _isVisualsDirty;

		// Token: 0x04000825 RID: 2085
		private MBBindingList<GameMenuPartyItemVM> _partyList;

		// Token: 0x04000826 RID: 2086
		private string _manCountText;

		// Token: 0x04000827 RID: 2087
		private int _cohesion;

		// Token: 0x04000828 RID: 2088
		private int _food;

		// Token: 0x04000829 RID: 2089
		private bool _isCohesionWarningEnabled;

		// Token: 0x0400082A RID: 2090
		private bool _isPlayerArmyLeader;

		// Token: 0x0400082B RID: 2091
		private bool _canManageArmy;

		// Token: 0x0400082C RID: 2092
		private HintViewModel _manageArmyHint;

		// Token: 0x0400082D RID: 2093
		public ElementNotificationVM _tutorialNotification;

		// Token: 0x0400082E RID: 2094
		private BasicTooltipViewModel _cohesionHint;

		// Token: 0x0400082F RID: 2095
		private BasicTooltipViewModel _manCountHint;

		// Token: 0x04000830 RID: 2096
		private BasicTooltipViewModel _foodHint;

		// Token: 0x04000831 RID: 2097
		private MBBindingList<StringItemWithHintVM> _issueList;
	}
}
