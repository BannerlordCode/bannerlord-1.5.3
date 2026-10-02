using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x020000A4 RID: 164
	public class GameMenuVM : ViewModel
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x00040037 File Offset: 0x0003E237
		// (set) Token: 0x06000F74 RID: 3956 RVA: 0x0004003F File Offset: 0x0003E23F
		public MenuContext MenuContext { get; private set; }

		// Token: 0x06000F75 RID: 3957 RVA: 0x00040048 File Offset: 0x0003E248
		public GameMenuVM(MenuContext menuContext)
		{
			this._gameMenuManager = Campaign.Current.GameMenuManager;
			this._gameMenuItemPool = new GameMenuVM.GameMenuItemPool<GameMenuItemVM>(10);
			this._progressItemPool = new GameMenuVM.GameMenuItemPool<GameMenuItemProgressVM>(10);
			this._newOptionsCache = new List<GameMenuItemVM.GameMenuItemCreationData>();
			this._cachedItemComparer = new GameMenuVM.GameMenuItemComparer();
			this._menuTextAttributeStrings = new Dictionary<string, string>();
			this._menuTextAttributes = new Dictionary<string, object>();
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.MenuContext = menuContext;
			this.MenuId = menuContext.GameMenu.StringId;
			this.Background = menuContext.CurrentBackgroundMeshName;
			this.ItemList = new MBBindingList<GameMenuItemVM>();
			this.ProgressItemList = new MBBindingList<GameMenuItemProgressVM>();
			this.PlunderItems = new MBBindingList<GameMenuPlunderItemVM>();
			this.IsInSiegeMode = PlayerSiege.PlayerSiegeEvent != null;
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0004013C File Offset: 0x0003E33C
		public override void RefreshValues()
		{
			if (this._isIdle)
			{
				return;
			}
			base.RefreshValues();
			this.ItemList.ApplyActionOnAllItems(delegate(GameMenuItemVM x)
			{
				x.RefreshValues();
			});
			this.ProgressItemList.ApplyActionOnAllItems(delegate(GameMenuItemProgressVM x)
			{
				x.RefreshValues();
			});
			this.Refresh(true);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000401B3 File Offset: 0x0003E3B3
		public void SetIdleMode(bool isIdle)
		{
			this._isIdle = isIdle;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000401BC File Offset: 0x0003E3BC
		public void Refresh(bool forceUpdateItems)
		{
			TextObject menuTitle = this.MenuContext.GameMenu.MenuTitle;
			this.TitleText = ((menuTitle != null) ? menuTitle.ToString() : null);
			this.MenuId = this.MenuContext.GameMenu.StringId;
			GameMenu gameMenu = this.MenuContext.GameMenu;
			this.IsEncounterMenu = gameMenu != null && gameMenu.OverlayType == GameMenu.MenuOverlayType.Encounter;
			this.Background = (string.IsNullOrEmpty(this.MenuContext.CurrentBackgroundMeshName) ? "wait_guards_stop" : this.MenuContext.CurrentBackgroundMeshName);
			if (forceUpdateItems)
			{
				this._newOptionsCache.Clear();
				int virtualMenuOptionAmount = this._gameMenuManager.GetVirtualMenuOptionAmount(this.MenuContext);
				for (int i = this.ProgressItemList.Count - 1; i >= 0; i--)
				{
					this._progressItemPool.Release(this.ProgressItemList[i]);
					this.ProgressItemList.RemoveAt(i);
				}
				for (int j = 0; j < virtualMenuOptionAmount; j++)
				{
					this._gameMenuManager.SetCurrentRepeatableIndex(this.MenuContext, j);
					if (this._gameMenuManager.GetVirtualMenuOptionConditionsHold(this.MenuContext, j))
					{
						TextObject textObject;
						TextObject textObject2;
						if (this._gameMenuManager.GetVirtualGameMenuOption(this.MenuContext, j).IsRepeatable)
						{
							textObject = new TextObject(this._gameMenuManager.GetVirtualMenuOptionText(this.MenuContext, j).ToString(), null);
							textObject2 = new TextObject(this._gameMenuManager.GetVirtualMenuOptionText2(this.MenuContext, j).ToString(), null);
						}
						else
						{
							textObject = this._gameMenuManager.GetVirtualMenuOptionText(this.MenuContext, j);
							textObject2 = this._gameMenuManager.GetVirtualMenuOptionText2(this.MenuContext, j);
						}
						TextObject virtualMenuOptionTooltip = this._gameMenuManager.GetVirtualMenuOptionTooltip(this.MenuContext, j);
						TextObject textObject3 = textObject;
						TextObject textObject4 = textObject2;
						TextObject textObject5 = virtualMenuOptionTooltip;
						GameMenu.MenuAndOptionType virtualMenuAndOptionType = this._gameMenuManager.GetVirtualMenuAndOptionType(this.MenuContext);
						GameMenuOption virtualGameMenuOption = this._gameMenuManager.GetVirtualGameMenuOption(this.MenuContext, j);
						GameKey gameKey = ((this._gameMenuManager.GetLeaveMenuOption(this.MenuContext) == virtualGameMenuOption) ? this._leaveKey : null);
						GameMenuOption.IssueQuestFlags optionQuestData = virtualGameMenuOption.OptionQuestData;
						GameMenuItemVM.GameMenuItemCreationData gameMenuItemCreationData = new GameMenuItemVM.GameMenuItemCreationData(this.MenuContext, j, textObject3, textObject4.IsEmpty() ? textObject3 : textObject4, textObject5, virtualMenuAndOptionType, optionQuestData, virtualGameMenuOption, gameKey);
						this._newOptionsCache.Add(gameMenuItemCreationData);
						if (virtualMenuAndOptionType == GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption || virtualMenuAndOptionType == GameMenu.MenuAndOptionType.WaitMenuShowProgressAndHoursOption)
						{
							GameMenuItemProgressVM gameMenuItemProgressVM = this._progressItemPool.Get();
							gameMenuItemProgressVM.InitializeWith(this.MenuContext, j);
							this.ProgressItemList.Add(gameMenuItemProgressVM);
						}
					}
				}
				for (int k = this.ItemList.Count - 1; k >= 0; k--)
				{
					GameMenuItemVM gameMenuItemVM = this.ItemList[k];
					if (gameMenuItemVM.GameMenuOption.IsRepeatable)
					{
						this.ItemList.RemoveAt(k);
						this._gameMenuItemPool.Release(gameMenuItemVM);
					}
					else
					{
						bool flag = true;
						for (int l = this._newOptionsCache.Count - 1; l >= 0; l--)
						{
							GameMenuItemVM.GameMenuItemCreationData gameMenuItemCreationData2 = this._newOptionsCache[l];
							if (gameMenuItemCreationData2.OptionID == gameMenuItemVM.OptionID)
							{
								flag = false;
								gameMenuItemVM.InitializeWith(in gameMenuItemCreationData2);
								if (!string.IsNullOrEmpty(this._latestTutorialElementID))
								{
									gameMenuItemVM.IsHighlightEnabled = gameMenuItemCreationData2.OptionID == this._latestTutorialElementID;
								}
								this._newOptionsCache.RemoveAt(l);
								break;
							}
						}
						if (flag)
						{
							this.ItemList.RemoveAt(k);
							this._gameMenuItemPool.Release(gameMenuItemVM);
						}
					}
				}
				for (int m = 0; m < this._newOptionsCache.Count; m++)
				{
					GameMenuItemVM.GameMenuItemCreationData gameMenuItemCreationData3 = this._newOptionsCache[m];
					GameMenuItemVM gameMenuItemVM2 = this._gameMenuItemPool.Get();
					gameMenuItemVM2.InitializeWith(in gameMenuItemCreationData3);
					if (!string.IsNullOrEmpty(this._latestTutorialElementID))
					{
						gameMenuItemVM2.IsHighlightEnabled = gameMenuItemCreationData3.OptionID == this._latestTutorialElementID;
					}
					this.ItemList.Add(gameMenuItemVM2);
				}
				this.ItemList.Sort(this._cachedItemComparer);
			}
			this.RefreshPlunderStatus();
			this._requireContextTextUpdate = true;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000405C8 File Offset: 0x0003E7C8
		private void RefreshPlunderStatus()
		{
			if (Campaign.Current.Models.EncounterGameMenuModel.IsPlunderMenu(this.MenuContext.GameMenu.StringId))
			{
				if (!this._plunderEventRegistered)
				{
					this.PlunderItems.Clear();
					CampaignEvents.ItemsLooted.AddNonSerializedListener(this, new Action<MobileParty, ItemRoster>(this.OnItemsPlundered));
					MBReadOnlyList<ItemRosterElement> plunderItems = this._viewDataTracker.GetPlunderItems();
					if (plunderItems != null)
					{
						for (int i = 0; i < plunderItems.Count; i++)
						{
							ItemRosterElement itemRosterElement = plunderItems[i];
							this.AddPlunderedItem(itemRosterElement);
						}
					}
					this._plunderEventRegistered = true;
					return;
				}
			}
			else if (this._plunderEventRegistered)
			{
				this.PlunderItems.Clear();
				CampaignEvents.ItemsLooted.ClearListeners(this);
				this._plunderEventRegistered = false;
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00040684 File Offset: 0x0003E884
		public void OnFrameTick()
		{
			this.IsInSiegeMode = PlayerSiege.PlayerSiegeEvent != null;
			if (this._requireContextTextUpdate)
			{
				this._menuText = this._gameMenuManager.GetMenuText(this.MenuContext);
				this.ContextText = this._menuText.ToString();
				this._menuTextAttributes.Clear();
				this._menuTextAttributeStrings.Clear();
				TextObject menuText = this._menuText;
				if (((menuText != null) ? menuText.Attributes : null) != null)
				{
					foreach (KeyValuePair<string, object> keyValuePair in this._menuText.Attributes)
					{
						this._menuTextAttributes[keyValuePair.Key] = keyValuePair.Value;
						this._menuTextAttributeStrings[keyValuePair.Key] = keyValuePair.Value.ToString();
					}
				}
				this._requireContextTextUpdate = false;
			}
			for (int i = 0; i < this.ItemList.Count; i++)
			{
				this.ItemList[i].Refresh();
			}
			for (int j = 0; j < this.ProgressItemList.Count; j++)
			{
				this.ProgressItemList[j].OnTick();
			}
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				this.IsNight = Campaign.Current.IsNight;
			}
			this._requireContextTextUpdate = this.IsMenuTextChanged();
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000407F8 File Offset: 0x0003E9F8
		private bool IsMenuTextChanged()
		{
			GameMenuManager gameMenuManager = this._gameMenuManager;
			TextObject textObject = ((gameMenuManager != null) ? gameMenuManager.GetMenuText(this.MenuContext) : null);
			if (this._menuText != textObject)
			{
				return true;
			}
			int count = this._menuTextAttributes.Count;
			TextObject menuText = this._menuText;
			int? num;
			if (menuText == null)
			{
				num = null;
			}
			else
			{
				Dictionary<string, object> attributes = menuText.Attributes;
				num = ((attributes != null) ? new int?(attributes.Count) : null);
			}
			int? num2 = num;
			if (!((count == num2.GetValueOrDefault()) & (num2 != null)))
			{
				return true;
			}
			foreach (KeyValuePair<string, object> keyValuePair in this._menuTextAttributes)
			{
				string key = keyValuePair.Key;
				object obj = null;
				object obj2 = this._menuTextAttributes[key];
				TextObject menuText2 = this._menuText;
				if (menuText2 == null || !menuText2.Attributes.TryGetValue(key, out obj))
				{
					return true;
				}
				if (obj2 != obj)
				{
					return true;
				}
				if (this._menuTextAttributeStrings[key] != obj.ToString())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00040934 File Offset: 0x0003EB34
		public void UpdateMenuContext(MenuContext newMenuContext)
		{
			this.MenuContext = newMenuContext;
			this.ItemList.Clear();
			this.ProgressItemList.Clear();
			this.Refresh(true);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0004095C File Offset: 0x0003EB5C
		public override void OnFinalize()
		{
			CampaignEvents.ItemsLooted.ClearListeners(this);
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this._gameMenuManager = null;
			this.MenuContext = null;
			this.ItemList.ApplyActionOnAllItems(delegate(GameMenuItemVM x)
			{
				x.OnFinalize();
			});
			this.ItemList.Clear();
			this.ItemList = null;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000409D9 File Offset: 0x0003EBD9
		public void SetLeaveHotKey(GameKey gameKey)
		{
			this._leaveKey = gameKey;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000409E4 File Offset: 0x0003EBE4
		private void OnItemsPlundered(MobileParty mobileParty, ItemRoster newItems)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				for (int i = 0; i < newItems.Count; i++)
				{
					ItemRosterElement itemRosterElement = newItems[i];
					this.AddPlunderedItem(itemRosterElement);
				}
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00040A1C File Offset: 0x0003EC1C
		private void AddPlunderedItem(ItemRosterElement item)
		{
			int num = this.PlunderItems.FindIndex<GameMenuPlunderItemVM>((GameMenuPlunderItemVM x) => x.Item.IsEqualTo(item.EquipmentElement));
			if (num != -1)
			{
				this.PlunderItems[num].Amount += item.Amount;
				return;
			}
			this.PlunderItems.Add(new GameMenuPlunderItemVM(item.EquipmentElement, item.Amount));
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00040A9C File Offset: 0x0003EC9C
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x00040AAE File Offset: 0x0003ECAE
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x00040AB6 File Offset: 0x0003ECB6
		[DataSourceProperty]
		public bool IsNight
		{
			get
			{
				return this._isNight;
			}
			set
			{
				if (value != this._isNight)
				{
					this._isNight = value;
					base.OnPropertyChangedWithValue(value, "IsNight");
				}
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x00040AD4 File Offset: 0x0003ECD4
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x00040ADC File Offset: 0x0003ECDC
		[DataSourceProperty]
		public bool IsInSiegeMode
		{
			get
			{
				return this._isInSiegeMode;
			}
			set
			{
				if (value != this._isInSiegeMode)
				{
					this._isInSiegeMode = value;
					base.OnPropertyChangedWithValue(value, "IsInSiegeMode");
				}
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00040AFA File Offset: 0x0003ECFA
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x00040B02 File Offset: 0x0003ED02
		[DataSourceProperty]
		public bool IsEncounterMenu
		{
			get
			{
				return this._isEncounterMenu;
			}
			set
			{
				if (value != this._isEncounterMenu)
				{
					this._isEncounterMenu = value;
					base.OnPropertyChangedWithValue(value, "IsEncounterMenu");
				}
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00040B20 File Offset: 0x0003ED20
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x00040B28 File Offset: 0x0003ED28
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00040B4B File Offset: 0x0003ED4B
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00040B53 File Offset: 0x0003ED53
		[DataSourceProperty]
		public string ContextText
		{
			get
			{
				return this._contextText;
			}
			set
			{
				if (value != this._contextText)
				{
					this._contextText = value;
					base.OnPropertyChangedWithValue<string>(value, "ContextText");
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00040B76 File Offset: 0x0003ED76
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x00040B7E File Offset: 0x0003ED7E
		[DataSourceProperty]
		public MBBindingList<GameMenuItemVM> ItemList
		{
			get
			{
				return this._itemList;
			}
			set
			{
				if (value != this._itemList)
				{
					this._itemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuItemVM>>(value, "ItemList");
				}
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x00040B9C File Offset: 0x0003ED9C
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x00040BA4 File Offset: 0x0003EDA4
		[DataSourceProperty]
		public MBBindingList<GameMenuItemProgressVM> ProgressItemList
		{
			get
			{
				return this._progressItemList;
			}
			set
			{
				if (value != this._progressItemList)
				{
					this._progressItemList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuItemProgressVM>>(value, "ProgressItemList");
				}
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x00040BC2 File Offset: 0x0003EDC2
		// (set) Token: 0x06000F91 RID: 3985 RVA: 0x00040BCA File Offset: 0x0003EDCA
		[DataSourceProperty]
		public string Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChangedWithValue<string>(value, "Background");
					this.BackgroundCopy = value;
				}
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00040BF4 File Offset: 0x0003EDF4
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x00040BFC File Offset: 0x0003EDFC
		[DataSourceProperty]
		public string BackgroundCopy
		{
			get
			{
				return this._backgroundCopy;
			}
			set
			{
				if (value != this._backgroundCopy)
				{
					this._backgroundCopy = value;
					base.OnPropertyChangedWithValue<string>(value, "BackgroundCopy");
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x00040C1F File Offset: 0x0003EE1F
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x00040C27 File Offset: 0x0003EE27
		[DataSourceProperty]
		public string MenuId
		{
			get
			{
				return this._menuId;
			}
			set
			{
				if (value != this._menuId)
				{
					this._menuId = value;
					base.OnPropertyChangedWithValue<string>(value, "MenuId");
				}
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x00040C4A File Offset: 0x0003EE4A
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x00040C52 File Offset: 0x0003EE52
		[DataSourceProperty]
		public MBBindingList<GameMenuPlunderItemVM> PlunderItems
		{
			get
			{
				return this._plunderItems;
			}
			set
			{
				if (value != this._plunderItems)
				{
					this._plunderItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuPlunderItemVM>>(value, "PlunderItems");
				}
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00040C70 File Offset: 0x0003EE70
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				this._latestTutorialElementID = obj.NewNotificationElementID;
			}
			if (this._latestTutorialElementID != null)
			{
				if (this._latestTutorialElementID != string.Empty)
				{
					if (this._latestTutorialElementID == "town_backstreet" && !this._isTavernButtonHighlightApplied)
					{
						this._isTavernButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "town_backstreet" && this._isTavernButtonHighlightApplied)
					{
						this._isTavernButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("town_backstreet", false);
					}
					if (this._latestTutorialElementID == "sell_all_prisoners" && !this._isSellPrisonerButtonHighlightApplied)
					{
						this._isSellPrisonerButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "sell_all_prisoners" && this._isSellPrisonerButtonHighlightApplied)
					{
						this._isSellPrisonerButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("sell_all_prisoners", false);
					}
					if (this._latestTutorialElementID == "storymode_tutorial_village_buy" && !this._isShopButtonHighlightApplied)
					{
						this._isShopButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "storymode_tutorial_village_buy" && this._isShopButtonHighlightApplied)
					{
						this._isShopButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_buy", false);
					}
					if (this._latestTutorialElementID == "storymode_tutorial_village_recruit" && !this._isRecruitButtonHighlightApplied)
					{
						this._isRecruitButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "storymode_tutorial_village_recruit" && this._isRecruitButtonHighlightApplied)
					{
						this._isRecruitButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_recruit", false);
					}
					if (this._latestTutorialElementID == "hostile_action" && !this._isHostileActionButtonHighlightApplied)
					{
						this._isHostileActionButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "hostile_action" && this._isHostileActionButtonHighlightApplied)
					{
						this._isHostileActionButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("hostile_action", false);
					}
					if (this._latestTutorialElementID == "town_besiege" && !this._isTownBesiegeButtonHighlightApplied)
					{
						this._isTownBesiegeButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "town_besiege" && this._isTownBesiegeButtonHighlightApplied)
					{
						this._isTownBesiegeButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("town_besiege", false);
					}
					if (this._latestTutorialElementID == "storymode_tutorial_village_enter" && !this._isEnterTutorialVillageButtonHighlightApplied)
					{
						this._isEnterTutorialVillageButtonHighlightApplied = this.SetGameMenuButtonHighlightState(this._latestTutorialElementID, true);
						return;
					}
					if (this._latestTutorialElementID != "storymode_tutorial_village_enter" && this._isEnterTutorialVillageButtonHighlightApplied)
					{
						this._isEnterTutorialVillageButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_enter", false);
						return;
					}
				}
				else
				{
					if (this._isTavernButtonHighlightApplied)
					{
						this._isTavernButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("town_backstreet", false);
					}
					if (this._isSellPrisonerButtonHighlightApplied)
					{
						this._isSellPrisonerButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("sell_all_prisoners", false);
					}
					if (this._isShopButtonHighlightApplied)
					{
						this._isShopButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_buy", false);
					}
					if (this._isRecruitButtonHighlightApplied)
					{
						this._isRecruitButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_recruit", false);
					}
					if (this._isHostileActionButtonHighlightApplied)
					{
						this._isHostileActionButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("hostile_action", false);
					}
					if (this._isTownBesiegeButtonHighlightApplied)
					{
						this._isTownBesiegeButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("town_besiege", false);
					}
					if (this._isEnterTutorialVillageButtonHighlightApplied)
					{
						this._isEnterTutorialVillageButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_enter", false);
						return;
					}
				}
			}
			else
			{
				if (this._isTavernButtonHighlightApplied)
				{
					this._isTavernButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("town_backstreet", false);
				}
				if (this._isSellPrisonerButtonHighlightApplied)
				{
					this._isSellPrisonerButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("sell_all_prisoners", false);
				}
				if (this._isShopButtonHighlightApplied)
				{
					this._isShopButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_buy", false);
				}
				if (this._isRecruitButtonHighlightApplied)
				{
					this._isRecruitButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_recruit", false);
				}
				if (this._isHostileActionButtonHighlightApplied)
				{
					this._isHostileActionButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("hostile_action", false);
				}
				if (this._isTownBesiegeButtonHighlightApplied)
				{
					this._isTownBesiegeButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("town_besiege", false);
				}
				if (this._isEnterTutorialVillageButtonHighlightApplied)
				{
					this._isEnterTutorialVillageButtonHighlightApplied = !this.SetGameMenuButtonHighlightState("storymode_tutorial_village_enter", false);
				}
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000410F0 File Offset: 0x0003F2F0
		private bool SetGameMenuButtonHighlightState(string buttonID, bool state)
		{
			for (int i = 0; i < this.ItemList.Count; i++)
			{
				GameMenuItemVM gameMenuItemVM = this.ItemList[i];
				if (gameMenuItemVM.OptionID == buttonID)
				{
					gameMenuItemVM.IsHighlightEnabled = state;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040006F6 RID: 1782
		private bool _isIdle;

		// Token: 0x040006F7 RID: 1783
		private bool _plunderEventRegistered;

		// Token: 0x040006F8 RID: 1784
		private GameMenuManager _gameMenuManager;

		// Token: 0x040006F9 RID: 1785
		private GameKey _leaveKey;

		// Token: 0x040006FA RID: 1786
		private Dictionary<string, string> _menuTextAttributeStrings;

		// Token: 0x040006FB RID: 1787
		private Dictionary<string, object> _menuTextAttributes;

		// Token: 0x040006FC RID: 1788
		private TextObject _menuText = TextObject.GetEmpty();

		// Token: 0x040006FD RID: 1789
		private GameMenuVM.GameMenuItemComparer _cachedItemComparer;

		// Token: 0x040006FE RID: 1790
		private IViewDataTracker _viewDataTracker;

		// Token: 0x04000700 RID: 1792
		private GameMenuVM.GameMenuItemPool<GameMenuItemVM> _gameMenuItemPool;

		// Token: 0x04000701 RID: 1793
		private GameMenuVM.GameMenuItemPool<GameMenuItemProgressVM> _progressItemPool;

		// Token: 0x04000702 RID: 1794
		private List<GameMenuItemVM.GameMenuItemCreationData> _newOptionsCache;

		// Token: 0x04000703 RID: 1795
		private MBBindingList<GameMenuItemVM> _itemList;

		// Token: 0x04000704 RID: 1796
		private MBBindingList<GameMenuItemProgressVM> _progressItemList;

		// Token: 0x04000705 RID: 1797
		private string _titleText;

		// Token: 0x04000706 RID: 1798
		private string _contextText;

		// Token: 0x04000707 RID: 1799
		private string _background;

		// Token: 0x04000708 RID: 1800
		private string _backgroundCopy;

		// Token: 0x04000709 RID: 1801
		private string _menuId;

		// Token: 0x0400070A RID: 1802
		private bool _isNight;

		// Token: 0x0400070B RID: 1803
		private bool _isInSiegeMode;

		// Token: 0x0400070C RID: 1804
		private bool _isEncounterMenu;

		// Token: 0x0400070D RID: 1805
		private MBBindingList<GameMenuPlunderItemVM> _plunderItems;

		// Token: 0x0400070E RID: 1806
		private string _latestTutorialElementID;

		// Token: 0x0400070F RID: 1807
		private bool _isTavernButtonHighlightApplied;

		// Token: 0x04000710 RID: 1808
		private bool _isSellPrisonerButtonHighlightApplied;

		// Token: 0x04000711 RID: 1809
		private bool _isShopButtonHighlightApplied;

		// Token: 0x04000712 RID: 1810
		private bool _isRecruitButtonHighlightApplied;

		// Token: 0x04000713 RID: 1811
		private bool _isHostileActionButtonHighlightApplied;

		// Token: 0x04000714 RID: 1812
		private bool _isTownBesiegeButtonHighlightApplied;

		// Token: 0x04000715 RID: 1813
		private bool _isEnterTutorialVillageButtonHighlightApplied;

		// Token: 0x04000716 RID: 1814
		private bool _requireContextTextUpdate;

		// Token: 0x02000217 RID: 535
		private class GameMenuItemPool<TItem> where TItem : class, new()
		{
			// Token: 0x06002586 RID: 9606 RVA: 0x00082062 File Offset: 0x00080262
			public GameMenuItemPool(int initialCapacity)
			{
				this._pool = new List<TItem>(initialCapacity);
			}

			// Token: 0x06002587 RID: 9607 RVA: 0x00082078 File Offset: 0x00080278
			public TItem Get()
			{
				TItem titem;
				if (this._pool.Count > 0)
				{
					titem = this._pool[this._pool.Count - 1];
					this._pool.RemoveAt(this._pool.Count - 1);
				}
				else
				{
					titem = new TItem();
				}
				return titem;
			}

			// Token: 0x06002588 RID: 9608 RVA: 0x000820CD File Offset: 0x000802CD
			public void Release(TItem item)
			{
				this._pool.Add(item);
			}

			// Token: 0x04001203 RID: 4611
			private readonly List<TItem> _pool;
		}

		// Token: 0x02000218 RID: 536
		private class GameMenuItemComparer : IComparer<GameMenuItemVM>
		{
			// Token: 0x06002589 RID: 9609 RVA: 0x000820DB File Offset: 0x000802DB
			public int Compare(GameMenuItemVM x, GameMenuItemVM y)
			{
				return x.Index.CompareTo(y.Index);
			}
		}
	}
}
