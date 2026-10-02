using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000EB RID: 235
	public class GameMenuCallbackManager
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00064C2A File Offset: 0x00062E2A
		public static GameMenuCallbackManager Instance
		{
			get
			{
				return Campaign.Current.GameMenuCallbackManager;
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00064C36 File Offset: 0x00062E36
		public GameMenuCallbackManager()
		{
			this.FillInitializationHandlers();
			this.FillEventHandlers();
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00064C4C File Offset: 0x00062E4C
		private void FillInitializationHandlers()
		{
			this._gameMenuInitializationHandlers = new Dictionary<string, GameMenuInitializationHandlerDelegate>();
			Assembly assembly = typeof(GameMenuInitializationHandler).Assembly;
			this.FillInitializationHandlerWith(assembly);
			foreach (Assembly assembly2 in GameMenuCallbackManager.GetAssemblies())
			{
				this.FillInitializationHandlerWith(assembly2);
			}
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00064C9A File Offset: 0x00062E9A
		private static Assembly[] GetAssemblies()
		{
			return typeof(GameMenu).Assembly.GetActiveReferencingGameAssembliesSafe();
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00064CB0 File Offset: 0x00062EB0
		public void OnGameLoad()
		{
			this.FillInitializationHandlers();
			this.FillEventHandlers();
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00064CC0 File Offset: 0x00062EC0
		private void FillInitializationHandlerWith(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(GameMenuInitializationHandler), false);
					if (customAttributesSafe != null && customAttributesSafe.Length != 0)
					{
						foreach (GameMenuInitializationHandler gameMenuInitializationHandler in customAttributesSafe)
						{
							GameMenuInitializationHandlerDelegate gameMenuInitializationHandlerDelegate = Delegate.CreateDelegate(typeof(GameMenuInitializationHandlerDelegate), methodInfo) as GameMenuInitializationHandlerDelegate;
							if (!this._gameMenuInitializationHandlers.ContainsKey(gameMenuInitializationHandler.MenuId))
							{
								this._gameMenuInitializationHandlers.Add(gameMenuInitializationHandler.MenuId, gameMenuInitializationHandlerDelegate);
							}
							else
							{
								this._gameMenuInitializationHandlers[gameMenuInitializationHandler.MenuId] = gameMenuInitializationHandlerDelegate;
							}
						}
					}
				}
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00064DC8 File Offset: 0x00062FC8
		private void FillEventHandlers()
		{
			this._eventHandlers = new Dictionary<string, Dictionary<string, GameMenuEventHandlerDelegate>>();
			Assembly assembly = typeof(GameMenuEventHandler).Assembly;
			this.FillEventHandlersWith(assembly);
			foreach (Assembly assembly2 in GameMenuCallbackManager.GetAssemblies())
			{
				this.FillEventHandlersWith(assembly2);
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00064E18 File Offset: 0x00063018
		private void FillEventHandlersWith(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(GameMenuEventHandler), false);
					if (customAttributesSafe != null && customAttributesSafe.Length != 0)
					{
						foreach (GameMenuEventHandler gameMenuEventHandler in customAttributesSafe)
						{
							GameMenuEventHandlerDelegate gameMenuEventHandlerDelegate = Delegate.CreateDelegate(typeof(GameMenuEventHandlerDelegate), methodInfo) as GameMenuEventHandlerDelegate;
							Dictionary<string, GameMenuEventHandlerDelegate> dictionary;
							if (!this._eventHandlers.TryGetValue(gameMenuEventHandler.MenuId, out dictionary))
							{
								dictionary = new Dictionary<string, GameMenuEventHandlerDelegate>();
								this._eventHandlers.Add(gameMenuEventHandler.MenuId, dictionary);
							}
							if (!dictionary.ContainsKey(gameMenuEventHandler.MenuOptionId))
							{
								dictionary.Add(gameMenuEventHandler.MenuOptionId, gameMenuEventHandlerDelegate);
							}
							else
							{
								dictionary[gameMenuEventHandler.MenuOptionId] = gameMenuEventHandlerDelegate;
							}
						}
					}
				}
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00064F50 File Offset: 0x00063150
		public void InitializeState(string menuId, MenuContext state)
		{
			GameMenuInitializationHandlerDelegate gameMenuInitializationHandlerDelegate = null;
			if (this._gameMenuInitializationHandlers.TryGetValue(menuId, out gameMenuInitializationHandlerDelegate))
			{
				MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(state, null);
				gameMenuInitializationHandlerDelegate(menuCallbackArgs);
			}
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00064F80 File Offset: 0x00063180
		public void OnConsequence(string menuId, GameMenuOption gameMenuOption, MenuContext state)
		{
			Dictionary<string, GameMenuEventHandlerDelegate> dictionary = null;
			if (this._eventHandlers.TryGetValue(menuId, out dictionary))
			{
				GameMenuEventHandlerDelegate gameMenuEventHandlerDelegate = null;
				if (dictionary.TryGetValue(gameMenuOption.IdString, out gameMenuEventHandlerDelegate))
				{
					MenuCallbackArgs menuCallbackArgs = new MenuCallbackArgs(state, gameMenuOption.Text);
					gameMenuEventHandlerDelegate(menuCallbackArgs);
				}
			}
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00064FC5 File Offset: 0x000631C5
		public TextObject GetMenuOptionTooltip(MenuContext menuContext, int menuItemNumber)
		{
			if (menuContext.GameMenu != null)
			{
				return menuContext.GameMenu.GetMenuOptionTooltip(menuItemNumber);
			}
			if (menuContext.GameMenu == null)
			{
				throw new MBMisuseException("Current game menu empty, can not run GetMenuOptionText");
			}
			return null;
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x00064FF0 File Offset: 0x000631F0
		public TextObject GetVirtualMenuOptionTooltip(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null)
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

		// Token: 0x06001603 RID: 5635 RVA: 0x0006505C File Offset: 0x0006325C
		public TextObject GetVirtualMenuOptionText(MenuContext menuContext, int virtualMenuItemIndex)
		{
			if (menuContext.GameMenu != null)
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

		// Token: 0x06001604 RID: 5636 RVA: 0x000650C6 File Offset: 0x000632C6
		public TextObject GetMenuOptionText(MenuContext menuContext, int menuItemNumber)
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

		// Token: 0x04000737 RID: 1847
		private Dictionary<string, GameMenuInitializationHandlerDelegate> _gameMenuInitializationHandlers;

		// Token: 0x04000738 RID: 1848
		private Dictionary<string, Dictionary<string, GameMenuEventHandlerDelegate>> _eventHandlers;
	}
}
