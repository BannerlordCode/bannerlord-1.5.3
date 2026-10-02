using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007D RID: 125
	public class MPArmoryVM : ViewModel
	{
		// Token: 0x06000C73 RID: 3187 RVA: 0x000268EC File Offset: 0x00024AEC
		public MPArmoryVM(Action<BasicCharacterObject> onOpenFacegen, Action<MPArmoryCosmeticItemBaseVM> onItemObtainRequested, Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this._onOpenFacegen = onOpenFacegen;
			this._character = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			this.CanOpenFacegen = true;
			this.ClassFilter = new MPLobbyClassFilterVM(new Action<MPLobbyClassFilterClassItemVM, bool>(this.OnSelectedClassChanged));
			this.HeroPreview = new MPArmoryHeroPreviewVM();
			this.ClassStats = new MPArmoryClassStatsVM();
			this.HeroPerkSelection = new MPArmoryHeroPerkSelectionVM(new Action<HeroPerkVM, MPPerkVM>(this.OnSelectPerk), new Action(this.ForceRefreshCharacter));
			this.Cosmetics = new MPArmoryCosmeticsVM(new Func<List<IReadOnlyPerkObject>>(this.GetSelectedPerks));
			this.InitalizeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0002699C File Offset: 0x00024B9C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.StatsText = new TextObject("{=ffjTMejn}Stats", null).ToString();
			this.CustomizationText = new TextObject("{=sPkRekRL}Customization", null).ToString();
			this.FacegenText = new TextObject("{=RSx1e5Wf}Edit Character", null).ToString();
			this.RefreshManageTauntButtonText();
			this.ClassFilter.RefreshValues();
			this.HeroPreview.RefreshValues();
			this.ClassStats.RefreshValues();
			this.Cosmetics.RefreshValues();
			this.HeroPerkSelection.RefreshValues();
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00026A2E File Offset: 0x00024C2E
		private void RefreshManageTauntButtonText()
		{
			this.ManageTauntsText = (this.IsManagingTaunts ? new TextObject("{=WiNRdfsm}Done", null).ToString() : new TextObject("{=58O7bWrD}Manage Taunts", null).ToString());
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00026A60 File Offset: 0x00024C60
		public override void OnFinalize()
		{
			this.HeroPreview = null;
			this._character = null;
			this.FinalizeCallbacks();
			this.Cosmetics.OnFinalize();
			base.OnFinalize();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00026A88 File Offset: 0x00024C88
		private void InitalizeCallbacks()
		{
			CharacterViewModel.OnCustomAnimationFinished = (Action<CharacterViewModel>)Delegate.Combine(CharacterViewModel.OnCustomAnimationFinished, new Action<CharacterViewModel>(this.OnCharacterCustomAnimationFinished));
			MPArmoryCosmeticsVM.OnCosmeticPreview += this.OnHeroPreviewItemEquipped;
			MPArmoryCosmeticsVM.OnRemoveCosmeticFromPreview += this.RemoveHeroPreviewItem;
			MPArmoryCosmeticsVM.OnTauntAssignmentRefresh += this.OnTauntAssignmentRefresh;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00026AE8 File Offset: 0x00024CE8
		private void FinalizeCallbacks()
		{
			CharacterViewModel.OnCustomAnimationFinished = (Action<CharacterViewModel>)Delegate.Remove(CharacterViewModel.OnCustomAnimationFinished, new Action<CharacterViewModel>(this.OnCharacterCustomAnimationFinished));
			MPArmoryCosmeticsVM.OnCosmeticPreview -= this.OnHeroPreviewItemEquipped;
			MPArmoryCosmeticsVM.OnRemoveCosmeticFromPreview -= this.RemoveHeroPreviewItem;
			MPArmoryCosmeticsVM.OnTauntAssignmentRefresh -= this.OnTauntAssignmentRefresh;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00026B48 File Offset: 0x00024D48
		public void OnTick(float dt)
		{
			if (this._tauntItemToRefreshNextAnimationWith != null)
			{
				MPArmoryCosmeticTauntItemVM tauntItemToRefreshNextAnimationWith = this._tauntItemToRefreshNextAnimationWith;
				Equipment equipment = LobbyTauntHelper.PrepareForTaunt(Equipment.CreateFromEquipmentCode(this.HeroPreview.HeroVisual.EquipmentCode), tauntItemToRefreshNextAnimationWith.TauntCosmeticElement, false);
				EquipmentIndex equipmentIndex;
				EquipmentIndex equipmentIndex2;
				bool flag;
				equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
				this.HeroPreview.HeroVisual.EquipmentCode = equipment.CalculateEquipmentCode();
				this.HeroPreview.HeroVisual.RightHandWieldedEquipmentIndex = (int)equipmentIndex;
				if (!flag)
				{
					this.HeroPreview.HeroVisual.LeftHandWieldedEquipmentIndex = (int)equipmentIndex2;
				}
				this.HeroPreview.HeroVisual.ExecuteStartCustomAnimation(CosmeticsManagerHelper.GetSuitableTauntActionForEquipment(equipment, tauntItemToRefreshNextAnimationWith.TauntCosmeticElement), false, 0.25f);
				this._currentTauntPreviewAnimationSource = this._tauntItemToRefreshNextAnimationWith;
				this._tauntItemToRefreshNextAnimationWith = null;
			}
			if (this.HeroPreview.HeroVisual.IsPlayingCustomAnimations && this._currentTauntPreviewAnimationSource != null)
			{
				float num = MathF.Clamp(this.HeroPreview.HeroVisual.CustomAnimationProgressRatio, 0f, 1f);
				this._currentTauntPreviewAnimationSource.PreviewAnimationRatio = (float)((int)(num * 100f));
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00026C58 File Offset: 0x00024E58
		public void RefreshPlayerData(PlayerData playerData)
		{
			if (this._character != null)
			{
				this._character.UpdatePlayerCharacterBodyProperties(playerData.BodyProperties, playerData.Race, playerData.IsFemale);
				this._character.Age = playerData.BodyProperties.Age;
				this.HeroPreview.SetCharacter(this._character, playerData.BodyProperties.DynamicProperties, playerData.Race, playerData.IsFemale);
				this.ForceRefreshCharacter();
				this.Cosmetics.RefreshPlayerData(playerData);
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00026CE0 File Offset: 0x00024EE0
		public void ForceRefreshCharacter()
		{
			this.OnSelectedClassChanged(this.ClassFilter.SelectedClassItem, true);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00026CF4 File Offset: 0x00024EF4
		private void OnIsEnabledChanged()
		{
			PlayerData playerData = NetworkMain.GameClient.PlayerData;
			if (this.IsEnabled && playerData != null)
			{
				this.RefreshPlayerData(playerData);
			}
			if (this.IsEnabled)
			{
				this.Cosmetics.RefreshCosmeticInfoFromNetwork();
				if (this.IsManagingTaunts)
				{
					this.ExecuteToggleManageTauntsState();
					return;
				}
			}
			else
			{
				MPArmoryHeroPerkSelectionVM heroPerkSelection = this.HeroPerkSelection;
				foreach (HeroPerkVM heroPerkVM in ((heroPerkSelection != null) ? heroPerkSelection.Perks : null))
				{
					BasicTooltipViewModel hint = heroPerkVM.Hint;
					if (hint != null)
					{
						hint.ExecuteEndHint();
					}
				}
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00026D94 File Offset: 0x00024F94
		private void OnSelectedClassChanged(MPLobbyClassFilterClassItemVM selectedClassItem, bool forceUpdate = false)
		{
			if (this.HeroPreview == null || this.ClassStats == null || this.HeroPerkSelection == null)
			{
				return;
			}
			if (this._currentClassItem == selectedClassItem && !forceUpdate)
			{
				return;
			}
			this._currentClassItem = selectedClassItem;
			this.HeroPerkSelection.RefreshPerksListWithHero(selectedClassItem.HeroClass);
			this.HeroPreview.SetCharacterClass(selectedClassItem.HeroClass.HeroCharacter);
			this.HeroPreview.SetCharacterPerks(this.HeroPerkSelection.CurrentSelectedPerks);
			this.ClassStats.RefreshWith(selectedClassItem.HeroClass);
			this.ClassStats.HeroInformation.RefreshWith(this.HeroPerkSelection.CurrentHeroClass, this.HeroPerkSelection.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>());
			this.Cosmetics.RefreshSelectedClass(selectedClassItem.HeroClass, this.HeroPerkSelection.CurrentSelectedPerks);
			this.Cosmetics.RefreshCosmeticInfoFromNetwork();
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00026E92 File Offset: 0x00025092
		public void SetCanOpenFacegen(bool enabled)
		{
			this.CanOpenFacegen = enabled;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00026E9B File Offset: 0x0002509B
		private void ExecuteOpenFaceGen()
		{
			Action<BasicCharacterObject> onOpenFacegen = this._onOpenFacegen;
			if (onOpenFacegen == null)
			{
				return;
			}
			onOpenFacegen(this._character);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00026EB3 File Offset: 0x000250B3
		public void ExecuteClearTauntSelection()
		{
			this.Cosmetics.ClearTauntSelections();
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00026EC0 File Offset: 0x000250C0
		public void ExecuteToggleManageTauntsState()
		{
			this.IsManagingTaunts = !this.IsManagingTaunts;
			if (this.IsManagingTaunts)
			{
				this._canOpenFacegenBeforeTauntState = this.CanOpenFacegen;
				this.Cosmetics.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Taunt);
				this.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM s)
				{
					s.IsEnabled = true;
				});
				this.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM s)
				{
					s.IsFocused = false;
				});
			}
			else
			{
				this.Cosmetics.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Clothing);
				this.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM s)
				{
					s.IsEnabled = false;
				});
				this.Cosmetics.ClearTauntSelections();
			}
			this.CanOpenFacegen = !this.IsManagingTaunts && this._canOpenFacegenBeforeTauntState;
			this.RefreshManageTauntButtonText();
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00026FC0 File Offset: 0x000251C0
		public void ExecuteSelectFocusedSlot()
		{
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.Cosmetics.TauntSlots)
			{
				if (mparmoryCosmeticTauntSlotVM.IsFocused)
				{
					mparmoryCosmeticTauntSlotVM.ExecuteSelect();
					break;
				}
			}
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0002701C File Offset: 0x0002521C
		public void ExecuteEmptyFocusedSlot()
		{
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.Cosmetics.TauntSlots)
			{
				if (mparmoryCosmeticTauntSlotVM.IsFocused)
				{
					MPArmoryCosmeticTauntItemVM assignedTauntItem = mparmoryCosmeticTauntSlotVM.AssignedTauntItem;
					if (assignedTauntItem != null && assignedTauntItem.IsUsed)
					{
						mparmoryCosmeticTauntSlotVM.AssignedTauntItem.ExecuteAction();
					}
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(new bool?(false));
					mparmoryCosmeticTauntSlotVM.SelectKeyVisual.SetForcedVisibility(new bool?(false));
					break;
				}
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000270B4 File Offset: 0x000252B4
		private void OnSelectPerk(HeroPerkVM heroPerk, MPPerkVM candidate)
		{
			if (this.ClassStats.HeroInformation.HeroClass != null && this.HeroPerkSelection.CurrentHeroClass != null)
			{
				List<IReadOnlyPerkObject> currentSelectedPerks = this.HeroPerkSelection.CurrentSelectedPerks;
				if (currentSelectedPerks.Count > 0)
				{
					this.ClassStats.HeroInformation.RefreshWith(this.HeroPerkSelection.CurrentHeroClass, currentSelectedPerks);
					this.HeroPreview.SetCharacterPerks(currentSelectedPerks);
					this.Cosmetics.RefreshSelectedClass(this._currentClassItem.HeroClass, currentSelectedPerks);
				}
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00027134 File Offset: 0x00025334
		private void RemoveHeroPreviewItem(MPArmoryCosmeticItemBaseVM itemVM)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
			if ((mparmoryCosmeticClothingItemVM = itemVM as MPArmoryCosmeticClothingItemVM) != null)
			{
				EquipmentIndex cosmeticEquipmentIndex = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.GetCosmeticEquipmentIndex();
				MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
				if (heroPreview != null)
				{
					heroPreview.HeroVisual.SetEquipment(cosmeticEquipmentIndex, default(EquipmentElement));
				}
				MPArmoryHeroPreviewVM heroPreview2 = this.HeroPreview;
				string text = ((heroPreview2 != null) ? heroPreview2.HeroVisual.EquipmentCode : null);
				if (!string.IsNullOrEmpty(text))
				{
					this._lastValidEquipment = Equipment.CreateFromEquipmentCode(text);
				}
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000271AC File Offset: 0x000253AC
		private void OnHeroPreviewItemEquipped(MPArmoryCosmeticItemBaseVM itemVM)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
			MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
			if ((mparmoryCosmeticClothingItemVM = itemVM as MPArmoryCosmeticClothingItemVM) != null)
			{
				EquipmentElement equipmentElement = mparmoryCosmeticClothingItemVM.EquipmentElement;
				EquipmentIndex cosmeticEquipmentIndex = equipmentElement.Item.GetCosmeticEquipmentIndex();
				MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
				if (heroPreview != null)
				{
					heroPreview.HeroVisual.SetEquipment(cosmeticEquipmentIndex, equipmentElement);
				}
				MPArmoryHeroPreviewVM heroPreview2 = this.HeroPreview;
				string text = ((heroPreview2 != null) ? heroPreview2.HeroVisual.EquipmentCode : null);
				if (!string.IsNullOrEmpty(text))
				{
					this._lastValidEquipment = Equipment.CreateFromEquipmentCode(text);
					return;
				}
			}
			else if ((mparmoryCosmeticTauntItemVM = itemVM as MPArmoryCosmeticTauntItemVM) != null)
			{
				MPArmoryHeroPreviewVM heroPreview3 = this.HeroPreview;
				if (((heroPreview3 != null) ? heroPreview3.HeroVisual : null) != null)
				{
					this.HeroPreview.HeroVisual.ExecuteStopCustomAnimation();
					if (this._lastValidEquipment != null)
					{
						this.HeroPreview.HeroVisual.SetEquipment(this._lastValidEquipment);
					}
				}
				this._tauntItemToRefreshNextAnimationWith = mparmoryCosmeticTauntItemVM;
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00027274 File Offset: 0x00025474
		private void OnCharacterCustomAnimationFinished(CharacterViewModel character)
		{
			if (character == this.HeroPreview.HeroVisual && this._lastValidEquipment != null)
			{
				MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
				if (((heroPreview != null) ? heroPreview.HeroVisual : null) != null)
				{
					this.HeroPreview.HeroVisual.SetEquipment(this._lastValidEquipment);
					this.HeroPreview.HeroVisual.LeftHandWieldedEquipmentIndex = -1;
					this.HeroPreview.HeroVisual.RightHandWieldedEquipmentIndex = -1;
					this._currentTauntPreviewAnimationSource.PreviewAnimationRatio = 0f;
					this._currentTauntPreviewAnimationSource = null;
				}
			}
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000272FA File Offset: 0x000254FA
		private void OnTauntAssignmentRefresh()
		{
			this.IsTauntAssignmentActive = this.Cosmetics.SelectedTauntItem != null || this.Cosmetics.SelectedTauntSlot != null;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00027320 File Offset: 0x00025520
		private void ResetHeroEquipment()
		{
			MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
			if (heroPreview == null)
			{
				return;
			}
			heroPreview.HeroVisual.SetEquipment(new Equipment());
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0002733C File Offset: 0x0002553C
		public static void ApplyPerkEffectsToEquipment(ref Equipment equipment, List<IReadOnlyPerkObject> selectedPerks)
		{
			MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(selectedPerks);
			IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
			if (enumerable != null)
			{
				foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
				{
					equipment[valueTuple.Item1] = valueTuple.Item2;
				}
			}
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000273A8 File Offset: 0x000255A8
		private List<IReadOnlyPerkObject> GetSelectedPerks()
		{
			return this.HeroPerkSelection.CurrentSelectedPerks;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000273B5 File Offset: 0x000255B5
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x000273BD File Offset: 0x000255BD
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.OnIsEnabledChanged();
				}
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x000273E1 File Offset: 0x000255E1
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x000273E9 File Offset: 0x000255E9
		[DataSourceProperty]
		public bool IsManagingTaunts
		{
			get
			{
				return this._isManagingTaunts;
			}
			set
			{
				if (value != this._isManagingTaunts)
				{
					this._isManagingTaunts = value;
					base.OnPropertyChangedWithValue(value, "IsManagingTaunts");
					this.Cosmetics.IsManagingTaunts = value;
				}
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00027413 File Offset: 0x00025613
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x0002741B File Offset: 0x0002561B
		[DataSourceProperty]
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChangedWithValue(value, "IsTauntAssignmentActive");
					if (this._isTauntAssignmentActive)
					{
						Func<string> getExitText = this._getExitText;
						this.TauntAssignmentClickToCloseText = ((getExitText != null) ? getExitText() : null);
					}
				}
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00027459 File Offset: 0x00025659
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00027461 File Offset: 0x00025661
		[DataSourceProperty]
		public bool CanOpenFacegen
		{
			get
			{
				return this._canOpenFacegen;
			}
			set
			{
				if (value != this._canOpenFacegen)
				{
					this._canOpenFacegen = value;
					base.OnPropertyChangedWithValue(value, "CanOpenFacegen");
				}
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0002747F File Offset: 0x0002567F
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x00027487 File Offset: 0x00025687
		[DataSourceProperty]
		public MPLobbyClassFilterVM ClassFilter
		{
			get
			{
				return this._classFilter;
			}
			set
			{
				if (value != this._classFilter)
				{
					this._classFilter = value;
					base.OnPropertyChangedWithValue<MPLobbyClassFilterVM>(value, "ClassFilter");
				}
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x000274A5 File Offset: 0x000256A5
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x000274AD File Offset: 0x000256AD
		[DataSourceProperty]
		public MPArmoryHeroPreviewVM HeroPreview
		{
			get
			{
				return this._heroPreview;
			}
			set
			{
				if (value != this._heroPreview)
				{
					this._heroPreview = value;
					base.OnPropertyChangedWithValue<MPArmoryHeroPreviewVM>(value, "HeroPreview");
				}
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000274CB File Offset: 0x000256CB
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000274D3 File Offset: 0x000256D3
		[DataSourceProperty]
		public MPArmoryClassStatsVM ClassStats
		{
			get
			{
				return this._classStats;
			}
			set
			{
				if (value != this._classStats)
				{
					this._classStats = value;
					base.OnPropertyChangedWithValue<MPArmoryClassStatsVM>(value, "ClassStats");
				}
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x000274F1 File Offset: 0x000256F1
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x000274F9 File Offset: 0x000256F9
		[DataSourceProperty]
		public MPArmoryHeroPerkSelectionVM HeroPerkSelection
		{
			get
			{
				return this._heroPerkSelection;
			}
			set
			{
				if (value != this._heroPerkSelection)
				{
					this._heroPerkSelection = value;
					base.OnPropertyChangedWithValue<MPArmoryHeroPerkSelectionVM>(value, "HeroPerkSelection");
				}
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00027517 File Offset: 0x00025717
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x0002751F File Offset: 0x0002571F
		[DataSourceProperty]
		public MPArmoryCosmeticsVM Cosmetics
		{
			get
			{
				return this._cosmetics;
			}
			set
			{
				if (value != this._cosmetics)
				{
					this._cosmetics = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticsVM>(value, "Cosmetics");
				}
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0002753D File Offset: 0x0002573D
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x00027545 File Offset: 0x00025745
		[DataSourceProperty]
		public string TauntAssignmentClickToCloseText
		{
			get
			{
				return this._tauntAssignmentClickToCloseText;
			}
			set
			{
				if (value != this._tauntAssignmentClickToCloseText)
				{
					this._tauntAssignmentClickToCloseText = value;
					base.OnPropertyChangedWithValue<string>(value, "TauntAssignmentClickToCloseText");
				}
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00027568 File Offset: 0x00025768
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00027570 File Offset: 0x00025770
		[DataSourceProperty]
		public string StatsText
		{
			get
			{
				return this._statsText;
			}
			set
			{
				if (value != this._statsText)
				{
					this._statsText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatsText");
				}
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00027593 File Offset: 0x00025793
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x0002759B File Offset: 0x0002579B
		[DataSourceProperty]
		public string CustomizationText
		{
			get
			{
				return this._customizationText;
			}
			set
			{
				if (value != this._customizationText)
				{
					this._customizationText = value;
					base.OnPropertyChangedWithValue<string>(value, "CustomizationText");
				}
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x000275BE File Offset: 0x000257BE
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x000275C6 File Offset: 0x000257C6
		[DataSourceProperty]
		public string FacegenText
		{
			get
			{
				return this._facegenText;
			}
			set
			{
				if (value != this._facegenText)
				{
					this._facegenText = value;
					base.OnPropertyChangedWithValue<string>(value, "FacegenText");
				}
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x000275E9 File Offset: 0x000257E9
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x000275F1 File Offset: 0x000257F1
		[DataSourceProperty]
		public string ManageTauntsText
		{
			get
			{
				return this._manageTauntsText;
			}
			set
			{
				if (value != this._manageTauntsText)
				{
					this._manageTauntsText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageTauntsText");
				}
			}
		}

		// Token: 0x040005A6 RID: 1446
		private readonly Action<BasicCharacterObject> _onOpenFacegen;

		// Token: 0x040005A7 RID: 1447
		private bool _canOpenFacegenBeforeTauntState;

		// Token: 0x040005A8 RID: 1448
		private BasicCharacterObject _character;

		// Token: 0x040005A9 RID: 1449
		private MPLobbyClassFilterClassItemVM _currentClassItem;

		// Token: 0x040005AA RID: 1450
		private Equipment _lastValidEquipment;

		// Token: 0x040005AB RID: 1451
		private Func<string> _getExitText;

		// Token: 0x040005AC RID: 1452
		private MPArmoryCosmeticTauntItemVM _tauntItemToRefreshNextAnimationWith;

		// Token: 0x040005AD RID: 1453
		private MPArmoryCosmeticTauntItemVM _currentTauntPreviewAnimationSource;

		// Token: 0x040005AE RID: 1454
		private bool _isEnabled;

		// Token: 0x040005AF RID: 1455
		private bool _isManagingTaunts;

		// Token: 0x040005B0 RID: 1456
		private bool _isTauntAssignmentActive;

		// Token: 0x040005B1 RID: 1457
		private bool _canOpenFacegen;

		// Token: 0x040005B2 RID: 1458
		private MPLobbyClassFilterVM _classFilter;

		// Token: 0x040005B3 RID: 1459
		private MPArmoryHeroPreviewVM _heroPreview;

		// Token: 0x040005B4 RID: 1460
		private MPArmoryClassStatsVM _classStats;

		// Token: 0x040005B5 RID: 1461
		private MPArmoryHeroPerkSelectionVM _heroPerkSelection;

		// Token: 0x040005B6 RID: 1462
		private MPArmoryCosmeticsVM _cosmetics;

		// Token: 0x040005B7 RID: 1463
		private string _tauntAssignmentClickToCloseText;

		// Token: 0x040005B8 RID: 1464
		private string _statsText;

		// Token: 0x040005B9 RID: 1465
		private string _customizationText;

		// Token: 0x040005BA RID: 1466
		private string _facegenText;

		// Token: 0x040005BB RID: 1467
		private string _manageTauntsText;
	}
}
