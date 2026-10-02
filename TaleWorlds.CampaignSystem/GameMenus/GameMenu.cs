using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E8 RID: 232
	public class GameMenu
	{
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x00064461 File Offset: 0x00062661
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x00064469 File Offset: 0x00062669
		public GameMenu.MenuAndOptionType Type { get; private set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x00064472 File Offset: 0x00062672
		// (set) Token: 0x060015AE RID: 5550 RVA: 0x0006447A File Offset: 0x0006267A
		public string StringId { get; private set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x00064483 File Offset: 0x00062683
		// (set) Token: 0x060015B0 RID: 5552 RVA: 0x0006448B File Offset: 0x0006268B
		public object RelatedObject { get; private set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060015B1 RID: 5553 RVA: 0x00064494 File Offset: 0x00062694
		// (set) Token: 0x060015B2 RID: 5554 RVA: 0x0006449C File Offset: 0x0006269C
		public TextObject MenuTitle { get; private set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060015B3 RID: 5555 RVA: 0x000644A5 File Offset: 0x000626A5
		// (set) Token: 0x060015B4 RID: 5556 RVA: 0x000644AD File Offset: 0x000626AD
		public GameMenu.MenuOverlayType OverlayType { get; private set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x000644B6 File Offset: 0x000626B6
		// (set) Token: 0x060015B6 RID: 5558 RVA: 0x000644BE File Offset: 0x000626BE
		public bool IsReady { get; private set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x000644C7 File Offset: 0x000626C7
		public int MenuItemAmount
		{
			get
			{
				return this._menuItems.Count;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x000644D4 File Offset: 0x000626D4
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x000644DC File Offset: 0x000626DC
		public List<object> MenuRepeatObjects { get; private set; } = new List<object>();

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x000644E5 File Offset: 0x000626E5
		public object CurrentRepeatableObject
		{
			get
			{
				if (this.MenuRepeatObjects.Count <= this.CurrentRepeatableIndex)
				{
					return null;
				}
				return this.MenuRepeatObjects[this.CurrentRepeatableIndex];
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x0006450D File Offset: 0x0006270D
		// (set) Token: 0x060015BC RID: 5564 RVA: 0x00064515 File Offset: 0x00062715
		public bool IsWaitMenu { get; private set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x0006451E File Offset: 0x0006271E
		// (set) Token: 0x060015BE RID: 5566 RVA: 0x00064526 File Offset: 0x00062726
		public bool IsWaitActive { get; private set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x0006452F File Offset: 0x0006272F
		public bool IsEmpty
		{
			get
			{
				return this.MenuRepeatObjects.Count == 0 && this.MenuItemAmount == 0;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x00064549 File Offset: 0x00062749
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x00064551 File Offset: 0x00062751
		public float Progress { get; private set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x0006455A File Offset: 0x0006275A
		// (set) Token: 0x060015C3 RID: 5571 RVA: 0x00064562 File Offset: 0x00062762
		public float TargetWaitHours { get; private set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0006456B File Offset: 0x0006276B
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x00064573 File Offset: 0x00062773
		public OnTickDelegate OnTick { get; private set; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x0006457C File Offset: 0x0006277C
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x00064584 File Offset: 0x00062784
		public OnConditionDelegate OnCondition { get; private set; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x0006458D File Offset: 0x0006278D
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00064595 File Offset: 0x00062795
		public OnConsequenceDelegate OnConsequence { get; private set; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x0006459E File Offset: 0x0006279E
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x000645A6 File Offset: 0x000627A6
		public int CurrentRepeatableIndex { get; set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x000645AF File Offset: 0x000627AF
		public IEnumerable<GameMenuOption> MenuOptions
		{
			get
			{
				return this._menuItems;
			}
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x000645B7 File Offset: 0x000627B7
		internal GameMenu(string idString)
		{
			this.StringId = idString;
			this._menuItems = new List<GameMenuOption>();
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x000645DC File Offset: 0x000627DC
		internal void Initialize(TextObject text, OnInitDelegate initDelegate, GameMenu.MenuOverlayType overlay, GameMenu.MenuFlags flags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.CurrentRepeatableIndex = 0;
			this.LastSelectedMenuObject = null;
			this._defaultText = text;
			this.OnInit = initDelegate;
			this.OverlayType = overlay;
			this.AutoSelectFirst = (flags & GameMenu.MenuFlags.AutoSelectFirst) > GameMenu.MenuFlags.None;
			this.RelatedObject = relatedObject;
			this.IsReady = true;
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00064628 File Offset: 0x00062828
		internal void Initialize(TextObject text, OnInitDelegate initDelegate, OnConditionDelegate condition, OnConsequenceDelegate consequence, OnTickDelegate tick, GameMenu.MenuAndOptionType type, GameMenu.MenuOverlayType overlay, float targetWaitHours = 0f, GameMenu.MenuFlags flags = GameMenu.MenuFlags.None, object relatedObject = null)
		{
			this.CurrentRepeatableIndex = 0;
			this.LastSelectedMenuObject = null;
			this._defaultText = text;
			this.OnInit = initDelegate;
			this.OverlayType = overlay;
			this.AutoSelectFirst = (flags & GameMenu.MenuFlags.AutoSelectFirst) > GameMenu.MenuFlags.None;
			this.RelatedObject = relatedObject;
			this.OnConsequence = consequence;
			this.OnCondition = condition;
			this.Type = type;
			this.OnTick = tick;
			this.TargetWaitHours = targetWaitHours;
			this.IsWaitMenu = type > GameMenu.MenuAndOptionType.RegularMenuOption;
			this.IsReady = true;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x000646A7 File Offset: 0x000628A7
		public void SetMenuRepeatObjects(IEnumerable<object> list)
		{
			this.MenuRepeatObjects = list.ToList<object>();
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x000646B5 File Offset: 0x000628B5
		private void AddOption(GameMenuOption newOption, int index = -1)
		{
			if (index >= 0 && this._menuItems.Count >= index)
			{
				this._menuItems.Insert(index, newOption);
				return;
			}
			this._menuItems.Add(newOption);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x000646E3 File Offset: 0x000628E3
		public bool GetMenuOptionConditionsHold(Game game, MenuContext menuContext, int menuItemNumber)
		{
			if (this.IsWaitMenu)
			{
				return this._menuItems[menuItemNumber].GetConditionsHold(game, menuContext) && this.RunWaitMenuCondition(menuContext);
			}
			return this._menuItems[menuItemNumber].GetConditionsHold(game, menuContext);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0006471F File Offset: 0x0006291F
		public TextObject GetMenuOptionText(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].Text;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00064732 File Offset: 0x00062932
		public GameMenuOption GetGameMenuOption(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber];
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00064740 File Offset: 0x00062940
		public TextObject GetMenuOptionText2(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].Text2;
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00064753 File Offset: 0x00062953
		public string GetMenuOptionIdString(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].IdString;
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00064766 File Offset: 0x00062966
		public TextObject GetMenuOptionTooltip(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].Tooltip;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00064779 File Offset: 0x00062979
		public bool GetMenuOptionIsLeave(int menuItemNumber)
		{
			return this._menuItems[menuItemNumber].IsLeave;
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0006478C File Offset: 0x0006298C
		public void SetProgressOfWaitingInMenu(float progress)
		{
			this.Progress = progress;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00064795 File Offset: 0x00062995
		public void SetTargetedWaitingTimeAndInitialProgress(float targetedWaitingTime, float initialProgress)
		{
			this.TargetWaitHours = targetedWaitingTime;
			this.SetProgressOfWaitingInMenu(initialProgress);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000647A8 File Offset: 0x000629A8
		public GameMenuOption GetLeaveMenuOption(Game game, MenuContext menuContext)
		{
			int leaveMenuOptionIndex = this.GetLeaveMenuOptionIndex(game, menuContext);
			if (leaveMenuOptionIndex < 0)
			{
				return null;
			}
			return this._menuItems[leaveMenuOptionIndex];
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x000647D0 File Offset: 0x000629D0
		public int GetLeaveMenuOptionIndex(Game game, MenuContext menuContext)
		{
			for (int i = 0; i < this._menuItems.Count; i++)
			{
				if (this._menuItems[i].IsLeave && this._menuItems[i].IsEnabled && this._menuItems[i].GetConditionsHold(game, menuContext))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00064834 File Offset: 0x00062A34
		public void RunOnTick(MenuContext menuContext, float dt)
		{
			if (this.IsWaitMenu && this.IsWaitActive)
			{
				if (this.OnTick != null)
				{
					MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
					this.OnTick(menuCallbackArgs, CampaignTime.Now - this._previousTickTime);
					this._previousTickTime = CampaignTime.Now;
				}
				if (this.Progress >= 1f)
				{
					this.EndWait();
					this.RunWaitMenuConsequence(menuContext);
				}
			}
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x000648A8 File Offset: 0x00062AA8
		public bool RunWaitMenuCondition(MenuContext menuContext)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
				bool flag = this.OnCondition(menuCallbackArgs);
				if (flag && !this.IsWaitActive)
				{
					menuContext.GameMenu.StartWait();
				}
				return flag;
			}
			return true;
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x000648F0 File Offset: 0x00062AF0
		public void RunWaitMenuConsequence(MenuContext menuContext)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
				this.OnConsequence(menuCallbackArgs);
			}
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00064920 File Offset: 0x00062B20
		public void RunMenuOptionConsequence(MenuContext menuContext, int menuItemNumber)
		{
			if (menuItemNumber >= this._menuItems.Count || menuItemNumber < 0)
			{
				Debug.FailedAssert("menuItemNumber out of bounds", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenu.cs", "RunMenuOptionConsequence", 269);
				menuItemNumber = this._menuItems.Count - 1;
			}
			GameMenuOption gameMenuOption = this._menuItems[menuItemNumber];
			if (gameMenuOption.IsLeave && this.IsWaitMenu)
			{
				this.EndWait();
			}
			gameMenuOption.RunConsequence(menuContext);
			if (Campaign.Current != null)
			{
				CampaignEventDispatcher.Instance.OnGameMenuOptionSelected(this, gameMenuOption);
			}
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000649A4 File Offset: 0x00062BA4
		public void StartWait()
		{
			this._previousTickTime = CampaignTime.Now;
			this.IsWaitActive = true;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppableFastForward;
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x000649C3 File Offset: 0x00062BC3
		public void EndWait()
		{
			this.IsWaitActive = false;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x000649D7 File Offset: 0x00062BD7
		private void ResetVariablesOnInit()
		{
			this.Progress = 0f;
			this.CurrentRepeatableIndex = 0;
			this.MenuRepeatObjects.Clear();
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x000649F8 File Offset: 0x00062BF8
		public void RunOnInit(Game game, MenuContext menuContext)
		{
			this.ResetVariablesOnInit();
			MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
			if (this.OnInit != null)
			{
				Debug.Print("[GAME MENU] " + menuContext.GameMenu.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
				this.OnInit(menuCallbackArgs);
				this.MenuTitle = menuCallbackArgs.MenuTitle;
			}
			CampaignEventDispatcher.Instance.OnGameMenuOpened(menuCallbackArgs);
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00064A6C File Offset: 0x00062C6C
		public void PreInit(MenuContext menuContext)
		{
			MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
			CampaignEventDispatcher.Instance.BeforeGameMenuOpened(menuCallbackArgs);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00064A94 File Offset: 0x00062C94
		public void AfterInit(MenuContext menuContext)
		{
			MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(menuContext, this.MenuTitle);
			CampaignEventDispatcher.Instance.AfterGameMenuInitialized(menuCallbackArgs);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00064AB9 File Offset: 0x00062CB9
		public TextObject GetText()
		{
			return this._defaultText;
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00064AC1 File Offset: 0x00062CC1
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x00064AC9 File Offset: 0x00062CC9
		public bool AutoSelectFirst { get; private set; }

		// Token: 0x060015EA RID: 5610 RVA: 0x00064AD4 File Offset: 0x00062CD4
		public static void ActivateGameMenu(string menuId)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			if (Campaign.Current.CurrentMenuContext == null)
			{
				Campaign.Current.GameMenuManager.SetNextMenu(menuId);
				MapState mapState = Game.Current.GameStateManager.LastOrDefault<MapState>();
				if (mapState != null)
				{
					mapState.EnterMenuMode();
				}
				bool flag;
				if (mapState == null)
				{
					flag = null != null;
				}
				else
				{
					MenuContext menuContext = mapState.MenuContext;
					flag = ((menuContext != null) ? menuContext.GameMenu : null) != null;
				}
				if (flag)
				{
					GameMenu gameMenu = mapState.MenuContext.GameMenu;
					if (gameMenu != null && gameMenu.IsWaitMenu)
					{
						mapState.MenuContext.GameMenu.StartWait();
						return;
					}
				}
			}
			else
			{
				GameMenu.SwitchToMenu(menuId);
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00064B6C File Offset: 0x00062D6C
		public static void SwitchToMenu(string menuId)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
			if (currentMenuContext != null)
			{
				currentMenuContext.SwitchToMenu(menuId);
				if (currentMenuContext.GameMenu.IsWaitMenu && Campaign.Current.TimeControlMode == CampaignTimeControlMode.Stop)
				{
					currentMenuContext.GameMenu.StartWait();
					return;
				}
			}
			else
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenu.cs", "SwitchToMenu", 390);
			}
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00064BD7 File Offset: 0x00062DD7
		public static void ExitToLast()
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.GameMenuManager.ExitToLast();
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00064BF4 File Offset: 0x00062DF4
		internal void AddOption(string optionId, TextObject optionText, GameMenuOption.OnConditionDelegate condition, GameMenuOption.OnConsequenceDelegate consequence, int index = -1, bool isLeave = false, bool isRepeatable = false, object relatedObject = null)
		{
			this.AddOption(new GameMenuOption(GameMenu.MenuAndOptionType.RegularMenuOption, optionId, optionText, optionText, condition, consequence, isLeave, isRepeatable, relatedObject), index);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00064C1B File Offset: 0x00062E1B
		internal void RemoveMenuOption(GameMenuOption option)
		{
			this._menuItems.Remove(option);
		}

		// Token: 0x04000722 RID: 1826
		private TextObject _defaultText;

		// Token: 0x04000728 RID: 1832
		public OnInitDelegate OnInit;

		// Token: 0x0400072B RID: 1835
		public object LastSelectedMenuObject;

		// Token: 0x04000733 RID: 1843
		private CampaignTime _previousTickTime;

		// Token: 0x04000734 RID: 1844
		private readonly List<GameMenuOption> _menuItems;

		// Token: 0x0200058E RID: 1422
		public enum MenuOverlayType
		{
			// Token: 0x04001822 RID: 6178
			None,
			// Token: 0x04001823 RID: 6179
			SettlementWithParties,
			// Token: 0x04001824 RID: 6180
			SettlementWithCharacters,
			// Token: 0x04001825 RID: 6181
			SettlementWithBoth,
			// Token: 0x04001826 RID: 6182
			Encounter
		}

		// Token: 0x0200058F RID: 1423
		public enum MenuFlags
		{
			// Token: 0x04001828 RID: 6184
			None,
			// Token: 0x04001829 RID: 6185
			AutoSelectFirst
		}

		// Token: 0x02000590 RID: 1424
		public enum MenuAndOptionType
		{
			// Token: 0x0400182B RID: 6187
			RegularMenuOption,
			// Token: 0x0400182C RID: 6188
			WaitMenuShowProgressAndHoursOption,
			// Token: 0x0400182D RID: 6189
			WaitMenuShowOnlyProgressOption,
			// Token: 0x0400182E RID: 6190
			WaitMenuHideProgressAndHoursOption
		}
	}
}
