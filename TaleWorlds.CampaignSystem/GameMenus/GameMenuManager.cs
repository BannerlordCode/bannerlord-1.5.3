using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000EE RID: 238
	public class GameMenuManager
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x00065161 File Offset: 0x00063361
		// (set) Token: 0x06001610 RID: 5648 RVA: 0x00065169 File Offset: 0x00063369
		public string NextGameMenuId { get; private set; }

		// Token: 0x06001611 RID: 5649 RVA: 0x00065172 File Offset: 0x00063372
		public GameMenuManager()
		{
			this.NextGameMenuId = null;
			this._gameMenus = new Dictionary<string, GameMenu>();
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x000651A0 File Offset: 0x000633A0
		public GameMenu NextMenu
		{
			get
			{
				GameMenu gameMenu;
				this._gameMenus.TryGetValue(this.NextGameMenuId, out gameMenu);
				return gameMenu;
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000651C2 File Offset: 0x000633C2
		public void SetNextMenu(string name)
		{
			this.NextGameMenuId = name;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000651CB File Offset: 0x000633CB
		public void ExitToLast()
		{
			if (Campaign.Current.CurrentMenuContext != null)
			{
				Game.Current.GameStateManager.LastOrDefault<MapState>().ExitMenuMode();
			}
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x000651ED File Offset: 0x000633ED
		internal object GetSelectedRepeatableObject(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.LastSelectedMenuObject;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetSelectedObject");
			}
			return 0;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0006521C File Offset: 0x0006341C
		internal object ObjectGetCurrentRepeatableObject(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.CurrentRepeatableObject;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not return CurrentRepeatableIndex");
			}
			return null;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00065246 File Offset: 0x00063446
		public void SetCurrentRepeatableIndex(MenuContext menuContext, int index)
		{
			if (menuContext.GameMenu != null)
			{
				menuContext.GameMenu.CurrentRepeatableIndex = index;
				return;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run SetCurrentRepeatableIndex");
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00065270 File Offset: 0x00063470
		public bool GetMenuOptionConditionsHold(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				if (Game.Current == null)
				{
					throw new MBNullParameterException("Game");
				}
				return menuContext.GameMenu.GetMenuOptionConditionsHold(Game.Current, menuContext, menuItemNumber);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionConditionsHold");
				}
				return false;
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000652C0 File Offset: 0x000634C0
		public void RefreshMenuOptions(MenuContext menuContext)
		{
			if (menuContext.GameMenu == null)
			{
				Debug.FailedAssert("Current game menu empty, can not run RefreshMenuOptions", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "RefreshMenuOptions", 143);
				return;
			}
			if (Game.Current == null)
			{
				Debug.FailedAssert("Game is null during RefreshMenuOptions", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "RefreshMenuOptions", 148);
				return;
			}
			menuContext.Handler.OnMenuRefresh();
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0006531C File Offset: 0x0006351C
		public void RefreshMenuOptionConditions(MenuContext menuContext)
		{
			if (menuContext.GameMenu == null)
			{
				Debug.FailedAssert("Current game menu empty, can not run RefreshMenuOptionConditions", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "RefreshMenuOptionConditions", 161);
				return;
			}
			if (Game.Current == null)
			{
				Debug.FailedAssert("Game is null during RefreshMenuOptionConditions", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "RefreshMenuOptionConditions", 166);
				return;
			}
			int virtualMenuOptionAmount = Campaign.Current.GameMenuManager.GetVirtualMenuOptionAmount(menuContext);
			for (int i = 0; i < virtualMenuOptionAmount; i++)
			{
				this.GetMenuOptionConditionsHold(menuContext, i);
			}
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00065392 File Offset: 0x00063592
		public string GetMenuOptionIdString(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu == null)
			{
				Debug.FailedAssert("Current game menu empty, can not run GetMenuOptionIdString", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "GetMenuOptionIdString", 183);
				return "";
			}
			return menuContext.GameMenu.GetMenuOptionIdString(menuItemNumber);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x000653C7 File Offset: 0x000635C7
		internal bool GetMenuOptionIsLeave(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionIsLeave(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return false;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x000653F2 File Offset: 0x000635F2
		public void RunConsequencesOfMenuOption(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				if (Game.Current == null)
				{
					throw new MBNullParameterException("Game");
				}
				menuContext.GameMenu.RunMenuOptionConsequence(menuContext, menuItemNumber);
				return;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run RunConsequencesOfMenuOption");
				}
				return;
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0006542F File Offset: 0x0006362F
		internal void SetRepeatObjectList(MenuContext menuContext, IEnumerable<object> list)
		{
			if (menuContext.GameMenu != null)
			{
				menuContext.GameMenu.SetMenuRepeatObjects(list);
				return;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuManager.cs", "SetRepeatObjectList", 237);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00065460 File Offset: 0x00063660
		public TextObject GetVirtualMenuOptionTooltip(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionTooltip(menuContext, 0);
				}
				return this.GetMenuOptionTooltip(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return null;
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x000654D7 File Offset: 0x000636D7
		public GameMenu.MenuOverlayType GetMenuOverlayType(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.OverlayType;
			}
			return GameMenu.MenuOverlayType.SettlementWithCharacters;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x000654F0 File Offset: 0x000636F0
		public TextObject GetVirtualMenuOptionText(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionText(menuContext, 0);
				}
				return this.GetMenuOptionText(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return null;
			}
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00065568 File Offset: 0x00063768
		public GameMenuOption GetVirtualGameMenuOption(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetGameMenuOption");
			}
			int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
			if (virtualMenuItemIndex < num)
			{
				return menuContext.GameMenu.GetGameMenuOption(0);
			}
			return menuContext.GameMenu.GetGameMenuOption(virtualMenuItemIndex + 1 - num);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x000655D0 File Offset: 0x000637D0
		public TextObject GetVirtualMenuOptionText2(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionText2(menuContext, 0);
				}
				return this.GetMenuOptionText2(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return null;
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00065647 File Offset: 0x00063847
		public float GetVirtualMenuProgress(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.Progress;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
			}
			return 0f;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00065675 File Offset: 0x00063875
		public GameMenu.MenuAndOptionType GetVirtualMenuAndOptionType(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.Type;
			}
			return GameMenu.MenuAndOptionType.RegularMenuOption;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0006568C File Offset: 0x0006388C
		public bool GetVirtualMenuIsWaitActive(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.IsWaitActive;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
			}
			return false;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x000656B6 File Offset: 0x000638B6
		public float GetVirtualMenuTargetWaitHours(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.TargetWaitHours;
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
			}
			return 0f;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000656E4 File Offset: 0x000638E4
		public bool GetVirtualMenuOptionIsEnabled(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					return menuContext.GameMenu.MenuOptions.ElementAt<GameMenuOption>(0).IsEnabled;
				}
				return menuContext.GameMenu.MenuOptions.ElementAt<GameMenuOption>(virtualMenuItemIndex + 1 - num).IsEnabled;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return false;
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00065778 File Offset: 0x00063978
		public int GetVirtualMenuOptionAmount(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				int count = menuContext.GameMenu.MenuRepeatObjects.Count;
				int menuItemAmount = menuContext.GameMenu.MenuItemAmount;
				if (count == 0)
				{
					return menuItemAmount;
				}
				return menuItemAmount - 1 + count;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionAmount");
				}
				return 0;
			}
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000657CC File Offset: 0x000639CC
		public bool GetVirtualMenuOptionIsLeave(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionIsLeave(menuContext, 0);
				}
				return this.GetMenuOptionIsLeave(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionText");
				}
				return false;
			}
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00065843 File Offset: 0x00063A43
		public GameMenuOption GetLeaveMenuOption(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetLeaveMenuOption(Game.Current, menuContext);
			}
			return null;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00065860 File Offset: 0x00063A60
		internal void RunConsequenceOfVirtualMenuOption(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					if (menuContext.GameMenu.MenuRepeatObjects.Count > 0)
					{
						menuContext.GameMenu.LastSelectedMenuObject = menuContext.GameMenu.MenuRepeatObjects[virtualMenuItemIndex];
					}
					this.RunConsequencesOfMenuOption(menuContext, 0);
					return;
				}
				this.RunConsequencesOfMenuOption(menuContext, virtualMenuItemIndex + 1 - num);
				return;
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run RunVirtualMenuItemConsequence");
				}
				return;
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x000658F8 File Offset: 0x00063AF8
		public bool GetVirtualMenuOptionConditionsHold(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				int num = ((menuContext.GameMenu.MenuRepeatObjects.Count > 0) ? menuContext.GameMenu.MenuRepeatObjects.Count : 1);
				if (virtualMenuItemIndex < num)
				{
					return this.GetMenuOptionConditionsHold(menuContext, 0);
				}
				return this.GetMenuOptionConditionsHold(menuContext, virtualMenuItemIndex + 1 - num);
			}
			else
			{
				if (menuContext.GameMenu == null)
				{
					throw new MBMisuseException("Current game menu empty, can not run GetVirtualMenuOptionConditionsHold");
				}
				return false;
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0006596F File Offset: 0x00063B6F
		public void OnFrameTick(MenuContext menuContext, float dt)
		{
			if (menuContext.GameMenu != null)
			{
				menuContext.GameMenu.RunOnTick(menuContext, dt);
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x00065986 File Offset: 0x00063B86
		public TextObject GetMenuText(MenuContext menuContext)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetText();
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuText");
			}
			return null;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000659B0 File Offset: 0x00063BB0
		private TextObject GetMenuOptionText(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionText(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return null;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000659DB File Offset: 0x00063BDB
		private TextObject GetMenuOptionText2(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionText2(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return null;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00065A06 File Offset: 0x00063C06
		private TextObject GetMenuOptionTooltip(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null && !menuContext.GameMenu.IsEmpty)
			{
				return menuContext.GameMenu.GetMenuOptionTooltip(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return null;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00065A3E File Offset: 0x00063C3E
		public void AddGameMenu(GameMenu gameMenu)
		{
			this._gameMenus.Add(gameMenu.StringId, gameMenu);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00065A54 File Offset: 0x00063C54
		public void RemoveRelatedGameMenus(object relatedObject)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, GameMenu> keyValuePair in this._gameMenus)
			{
				if (keyValuePair.Value.RelatedObject == relatedObject)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (string text in list)
			{
				this._gameMenus.Remove(text);
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00065B08 File Offset: 0x00063D08
		public void RemoveRelatedGameMenuOptions(object relatedObject)
		{
			foreach (KeyValuePair<string, GameMenu> keyValuePair in this._gameMenus.ToList<KeyValuePair<string, GameMenu>>())
			{
				foreach (GameMenuOption gameMenuOption in keyValuePair.Value.MenuOptions.ToList<GameMenuOption>())
				{
					if (gameMenuOption.RelatedObject == relatedObject)
					{
						keyValuePair.Value.RemoveMenuOption(gameMenuOption);
					}
				}
			}
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00065BB8 File Offset: 0x00063DB8
		internal void UnregisterNonReadyObjects()
		{
			MBList<KeyValuePair<string, GameMenu>> mblist = this._gameMenus.ToMBList<KeyValuePair<string, GameMenu>>();
			for (int i = mblist.Count - 1; i >= 0; i--)
			{
				if (!mblist[i].Value.IsReady)
				{
					this._gameMenus.Remove(mblist[i].Key);
				}
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00065C18 File Offset: 0x00063E18
		public GameMenu GetGameMenu(string menuId)
		{
			GameMenu gameMenu;
			this._gameMenus.TryGetValue(menuId, out gameMenu);
			return gameMenu;
		}

		// Token: 0x0400073D RID: 1853
		private Dictionary<string, GameMenu> _gameMenus;

		// Token: 0x0400073F RID: 1855
		public int PreviouslySelectedGameMenuItem = -1;

		// Token: 0x04000740 RID: 1856
		public Location NextLocation;

		// Token: 0x04000741 RID: 1857
		public Location PreviousLocation;

		// Token: 0x04000742 RID: 1858
		public List<Location> MenuLocations = new List<Location>();

		// Token: 0x04000743 RID: 1859
		public object PreviouslySelectedGameMenuObject;
	}
}
