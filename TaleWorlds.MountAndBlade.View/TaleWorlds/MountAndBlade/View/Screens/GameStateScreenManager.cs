using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000056 RID: 86
	public class GameStateScreenManager : IGameStateManagerListener
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x00012008 File Offset: 0x00010208
		private GameStateManager GameStateManager
		{
			get
			{
				return GameStateManager.Current;
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0001200F File Offset: 0x0001020F
		public GameStateScreenManager()
		{
			this._screenTypes = new Dictionary<Type, MBList<Type>>();
			this.CollectTypes();
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00012048 File Offset: 0x00010248
		internal void CollectTypes()
		{
			this._screenTypes.Clear();
			Assembly assembly = typeof(GameStateScreen).Assembly;
			Assembly[] referencingAssembliesSafe = assembly.GetReferencingAssembliesSafe(null);
			this.CheckAssemblyScreens(assembly);
			foreach (Assembly assembly2 in referencingAssembliesSafe)
			{
				this.CheckAssemblyScreens(assembly2);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00012098 File Offset: 0x00010298
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ForceVSyncInMenus)
			{
				if (!BannerlordConfig.ForceVSyncInMenus)
				{
					Utilities.SetForceVsync(false);
					return;
				}
				if (this.GameStateManager.ActiveState.IsMenuState)
				{
					Utilities.SetForceVsync(true);
				}
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000120C8 File Offset: 0x000102C8
		private void CheckAssemblyScreens(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(GameStateScreen), false);
				if (customAttributesSafe != null && customAttributesSafe.Length != 0)
				{
					foreach (GameStateScreen gameStateScreen in customAttributesSafe)
					{
						if (this._screenTypes.ContainsKey(gameStateScreen.GameStateType))
						{
							this._screenTypes[gameStateScreen.GameStateType].Add(type);
						}
						else
						{
							this._screenTypes.Add(gameStateScreen.GameStateType, new MBList<Type> { type });
						}
					}
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000121A0 File Offset: 0x000103A0
		public ScreenBase CreateScreen(GameState state)
		{
			Type type = null;
			MBList<Type> mblist;
			if (this._screenTypes.TryGetValue(state.GetType(), out mblist))
			{
				MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
				for (int i = mblist.Count - 1; i >= 0; i--)
				{
					if (activeGameAssemblies.Contains(mblist[i].Assembly))
					{
						type = mblist[i];
						break;
					}
				}
				if (type != null)
				{
					return Activator.CreateInstance(type, new object[] { state }) as ScreenBase;
				}
				Debug.FailedAssert(string.Format("Failed to create game state screen for state: {0}", state.GetType()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Screens\\GameStateScreenManager.cs", "CreateScreen", 108);
			}
			return null;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0001223C File Offset: 0x0001043C
		public void BuildScreens()
		{
			int num = 0;
			foreach (GameState gameState in this.GameStateManager.GameStates)
			{
				ScreenBase screenBase = this.CreateScreen(gameState);
				gameState.RegisterListener(screenBase as IGameStateListener);
				if (screenBase != null)
				{
					if (num == 0)
					{
						ScreenManager.CleanAndPushScreen(screenBase);
					}
					else
					{
						ScreenManager.PushScreen(screenBase);
					}
				}
				num++;
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000122B8 File Offset: 0x000104B8
		void IGameStateManagerListener.OnCreateState(GameState gameState)
		{
			ScreenBase screenBase = this.CreateScreen(gameState);
			if (screenBase == null)
			{
				Debug.FailedAssert(string.Format("Create screen for {0} returned null.", gameState.GetName()), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Screens\\GameStateScreenManager.cs", "OnCreateState", 145);
			}
			gameState.RegisterListener(screenBase as IGameStateListener);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00012304 File Offset: 0x00010504
		void IGameStateManagerListener.OnPushState(GameState gameState, bool isTopGameState)
		{
			if (!gameState.IsMenuState)
			{
				Utilities.ClearOldResourcesAndObjects();
			}
			if (gameState.IsMenuState && BannerlordConfig.ForceVSyncInMenus)
			{
				Utilities.SetForceVsync(true);
			}
			else if (!gameState.IsMenuState)
			{
				Utilities.SetForceVsync(false);
			}
			ScreenBase listenerOfType;
			if ((listenerOfType = gameState.GetListenerOfType<ScreenBase>()) != null)
			{
				if (isTopGameState)
				{
					ScreenManager.CleanAndPushScreen(listenerOfType);
				}
				else
				{
					ScreenManager.PushScreen(listenerOfType);
				}
			}
			ThumbnailCacheManager.Current.ClearUnusedCache();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00012368 File Offset: 0x00010568
		void IGameStateManagerListener.OnPopState(GameState gameState)
		{
			if (gameState.IsMenuState && BannerlordConfig.ForceVSyncInMenus)
			{
				Utilities.SetForceVsync(false);
			}
			if (this.GameStateManager.ActiveState != null && this.GameStateManager.ActiveState.IsMenuState && BannerlordConfig.ForceVSyncInMenus)
			{
				Utilities.SetForceVsync(true);
			}
			ScreenManager.PopScreen();
			if (!gameState.IsMenuState)
			{
				Utilities.ClearOldResourcesAndObjects();
			}
			ThumbnailCacheManager.Current.ClearUnusedCache();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000123D2 File Offset: 0x000105D2
		void IGameStateManagerListener.OnCleanStates()
		{
			ScreenManager.CleanScreens();
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000123D9 File Offset: 0x000105D9
		void IGameStateManagerListener.OnSavedGameLoadFinished()
		{
			this.BuildScreens();
		}

		// Token: 0x04000169 RID: 361
		private Dictionary<Type, MBList<Type>> _screenTypes;
	}
}
