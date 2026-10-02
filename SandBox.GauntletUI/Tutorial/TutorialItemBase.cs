using System;
using System.Collections.Generic;
using SandBox.View.Map;
using SandBox.ViewModelCollection.MapSiege;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;

namespace SandBox.GauntletUI.Tutorial
{
	// Token: 0x02000018 RID: 24
	public abstract class TutorialItemBase
	{
		// Token: 0x06000140 RID: 320
		public abstract bool IsConditionsMetForCompletion();

		// Token: 0x06000141 RID: 321
		public abstract bool IsConditionsMetForActivation();

		// Token: 0x06000142 RID: 322
		public abstract TutorialContexts GetTutorialsRelevantContext();

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000A5CD File Offset: 0x000087CD
		// (set) Token: 0x06000143 RID: 323 RVA: 0x0000A5C4 File Offset: 0x000087C4
		public TutorialItemVM.ItemPlacements Placement { get; protected set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000A5DE File Offset: 0x000087DE
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000A5D5 File Offset: 0x000087D5
		public bool MouseRequired { get; protected set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000A5EF File Offset: 0x000087EF
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000A5E6 File Offset: 0x000087E6
		public string HighlightedVisualElementID { get; protected set; }

		// Token: 0x06000149 RID: 329 RVA: 0x0000A5F7 File Offset: 0x000087F7
		protected virtual string GetCustomTutorialElementHighlightID()
		{
			return "";
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000A5FE File Offset: 0x000087FE
		public virtual void OnDeactivate()
		{
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000A600 File Offset: 0x00008800
		public virtual bool IsConditionsMetForVisibility()
		{
			return this.GetTutorialsRelevantContext() != TutorialContexts.Mission || !BannerlordConfig.HideBattleUI;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A615 File Offset: 0x00008815
		public virtual void OnInventoryTransferItem(InventoryTransferItemEvent obj)
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A617 File Offset: 0x00008817
		public virtual void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A619 File Offset: 0x00008819
		public virtual void OnInventoryFilterChanged(InventoryFilterChangedEvent obj)
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A61B File Offset: 0x0000881B
		public virtual void OnPerkSelectedByPlayer(PerkSelectedByPlayerEvent obj)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A61D File Offset: 0x0000881D
		public virtual void OnFocusAddedByPlayer(FocusAddedByPlayerEvent obj)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A61F File Offset: 0x0000881F
		public virtual void OnGameMenuOpened(MenuCallbackArgs obj)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A621 File Offset: 0x00008821
		public virtual void OnMainMapCameraMove(MapScreen.MainMapCameraMoveEvent obj)
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A623 File Offset: 0x00008823
		public virtual void OnCharacterPortraitPopUpOpened(CharacterObject obj)
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A625 File Offset: 0x00008825
		public virtual void OnPlayerStartTalkFromMenuOverlay(Hero obj)
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A627 File Offset: 0x00008827
		public virtual void OnGameMenuOptionSelected(GameMenuOption obj)
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A629 File Offset: 0x00008829
		public virtual void OnPlayerStartRecruitment(CharacterObject obj)
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A62B File Offset: 0x0000882B
		public virtual void OnNewCompanionAdded(Hero obj)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A62D File Offset: 0x0000882D
		public virtual void OnPlayerRecruitedUnit(CharacterObject obj, int count)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000A62F File Offset: 0x0000882F
		public virtual void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000A631 File Offset: 0x00008831
		public virtual void OnMissionNameMarkerToggled(MissionNameMarkerToggleEvent obj)
		{
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000A633 File Offset: 0x00008833
		public virtual void OnPlayerToggleTrackSettlementFromEncyclopedia(PlayerToggleTrackSettlementFromEncyclopediaEvent obj)
		{
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000A635 File Offset: 0x00008835
		public virtual void OnInventoryEquipmentTypeChange(InventoryEquipmentTypeChangedEvent obj)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000A637 File Offset: 0x00008837
		public virtual void OnArmyCohesionByPlayerBoosted(ArmyCohesionBoostedByPlayerEvent obj)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A639 File Offset: 0x00008839
		public virtual void OnPartyAddedToArmyByPlayer(PartyAddedToArmyByPlayerEvent obj)
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A63B File Offset: 0x0000883B
		public virtual void OnPlayerStartEngineConstruction(PlayerStartEngineConstructionEvent obj)
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A63D File Offset: 0x0000883D
		public virtual void OnPlayerUpgradeTroop(CharacterObject arg1, CharacterObject arg2, int arg3)
		{
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A63F File Offset: 0x0000883F
		public virtual void OnPlayerMoveTroop(PlayerMoveTroopEvent obj)
		{
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A641 File Offset: 0x00008841
		public virtual void OnPerkSelectionToggle(PerkSelectionToggleEvent obj)
		{
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A643 File Offset: 0x00008843
		public virtual void OnPlayerInspectedPartySpeed(PlayerInspectedPartySpeedEvent obj)
		{
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000A645 File Offset: 0x00008845
		public virtual void OnPlayerMovementFlagChanged(MissionPlayerMovementFlagsChangeEvent obj)
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A647 File Offset: 0x00008847
		public virtual void OnPlayerToggledUpgradePopup(PlayerToggledUpgradePopupEvent obj)
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000A649 File Offset: 0x00008849
		public virtual void OnOrderOfBattleHeroAssignedToFormation(OrderOfBattleHeroAssignedToFormationEvent obj)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A64B File Offset: 0x0000884B
		public virtual void OnOrderOfBattleFormationClassChanged(OrderOfBattleFormationClassChangedEvent obj)
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000A64D File Offset: 0x0000884D
		public virtual void OnOrderOfBattleFormationWeightChanged(OrderOfBattleFormationWeightChangedEvent obj)
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A64F File Offset: 0x0000884F
		public virtual void OnCraftingWeaponClassSelectionOpened(CraftingWeaponClassSelectionOpenedEvent obj)
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A651 File Offset: 0x00008851
		public virtual void OnCraftingOnWeaponResultPopupOpened(CraftingWeaponResultPopupToggledEvent obj)
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A653 File Offset: 0x00008853
		public virtual void OnCraftingOrderTabOpened(CraftingOrderTabOpenedEvent obj)
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A655 File Offset: 0x00008855
		public virtual void OnCraftingOrderSelectionOpened(CraftingOrderSelectionOpenedEvent obj)
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A657 File Offset: 0x00008857
		public virtual void OnInventoryItemInspected(InventoryItemInspectedEvent obj)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A659 File Offset: 0x00008859
		public virtual void OnCrimeValueInspectedInSettlementOverlay(CrimeValueInspectedInSettlementOverlayEvent obj)
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A65B File Offset: 0x0000885B
		public virtual void OnClanRoleAssignedThroughClanScreen(ClanRoleAssignedThroughClanScreenEvent obj)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A65D File Offset: 0x0000885D
		public virtual void OnPlayerSelectedAKingdomDecisionOption(PlayerSelectedAKingdomDecisionOptionEvent obj)
		{
		}
	}
}
