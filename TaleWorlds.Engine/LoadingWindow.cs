using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000057 RID: 87
	public static class LoadingWindow
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00006FD8 File Offset: 0x000051D8
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x00006FDF File Offset: 0x000051DF
		public static bool IsLoadingWindowActive { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00006FE7 File Offset: 0x000051E7
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x00006FEE File Offset: 0x000051EE
		public static ILoadingWindowManager LoadingWindowManager { get; private set; }

		// Token: 0x060008D0 RID: 2256 RVA: 0x00006FF8 File Offset: 0x000051F8
		public static void InitializeWith<T>() where T : class, ILoadingWindowManager, new()
		{
			LoadingWindow.Destroy();
			LoadingWindow.LoadingWindowManager = new T();
			LoadingWindow.LoadingWindowManager.Initialize();
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.LoadingWindowManager.EnableLoadingWindow();
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00007029 File Offset: 0x00005229
		public static void Destroy()
		{
			ILoadingWindowManager loadingWindowManager = LoadingWindow.LoadingWindowManager;
			if (loadingWindowManager != null)
			{
				loadingWindowManager.DisableLoadingWindow();
			}
			ILoadingWindowManager loadingWindowManager2 = LoadingWindow.LoadingWindowManager;
			if (loadingWindowManager2 != null)
			{
				loadingWindowManager2.Destroy();
			}
			LoadingWindow.LoadingWindowManager = null;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00007051 File Offset: 0x00005251
		public static void DisableGlobalLoadingWindow()
		{
			if (LoadingWindow.LoadingWindowManager == null)
			{
				return;
			}
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.LoadingWindowManager.DisableLoadingWindow();
				Utilities.DisableGlobalLoadingWindow();
				Utilities.OnLoadingWindowDisabled();
			}
			LoadingWindow.IsLoadingWindowActive = false;
			Utilities.DebugSetGlobalLoadingWindowState(false);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00007082 File Offset: 0x00005282
		public static void EnableGlobalLoadingWindow()
		{
			if (LoadingWindow.LoadingWindowManager == null)
			{
				return;
			}
			LoadingWindow.IsLoadingWindowActive = true;
			Utilities.DebugSetGlobalLoadingWindowState(true);
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.LoadingWindowManager.EnableLoadingWindow();
				Utilities.OnLoadingWindowEnabled();
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000070AE File Offset: 0x000052AE
		public static void SetCurrentModeIsMultiplayer(bool isMultiplayer)
		{
			ILoadingWindowManager loadingWindowManager = LoadingWindow.LoadingWindowManager;
			if (loadingWindowManager == null)
			{
				return;
			}
			loadingWindowManager.SetCurrentModeIsMultiplayer(isMultiplayer);
		}
	}
}
