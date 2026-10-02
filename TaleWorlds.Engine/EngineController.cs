using System;
using TaleWorlds.Engine.InputSystem;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x02000043 RID: 67
	public static class EngineController
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060006C8 RID: 1736 RVA: 0x00003C6C File Offset: 0x00001E6C
		// (remove) Token: 0x060006C9 RID: 1737 RVA: 0x00003CA0 File Offset: 0x00001EA0
		public static event Action ConfigChange;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006CA RID: 1738 RVA: 0x00003CD4 File Offset: 0x00001ED4
		// (remove) Token: 0x060006CB RID: 1739 RVA: 0x00003D08 File Offset: 0x00001F08
		public static event Action<bool> OnConstrainedStateChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060006CC RID: 1740 RVA: 0x00003D3C File Offset: 0x00001F3C
		// (remove) Token: 0x060006CD RID: 1741 RVA: 0x00003D70 File Offset: 0x00001F70
		public static event Action OnDLCInstalledCallback;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060006CE RID: 1742 RVA: 0x00003DA4 File Offset: 0x00001FA4
		// (remove) Token: 0x060006CF RID: 1743 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public static event Action OnDLCLoadedCallback;

		// Token: 0x060006D0 RID: 1744 RVA: 0x00003E0B File Offset: 0x0000200B
		internal static void OnApplicationTick(float dt)
		{
			Input.Update();
			Screen.Update();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00003E18 File Offset: 0x00002018
		[EngineCallback(null, false)]
		internal static void Initialize()
		{
			IInputContext inputContext = null;
			Input.Initialize(new EngineInputManager(), inputContext);
			Common.PlatformFileHelper = new PlatformFileHelperPC(Utilities.GetApplicationName());
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00003E41 File Offset: 0x00002041
		[EngineCallback(null, false)]
		internal static void OnConfigChange()
		{
			NativeConfig.OnConfigChanged();
			if (EngineController.ConfigChange != null)
			{
				EngineController.ConfigChange();
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00003E59 File Offset: 0x00002059
		[EngineCallback(null, false)]
		internal static void OnConstrainedStateChange(bool isConstrained)
		{
			Action<bool> onConstrainedStateChanged = EngineController.OnConstrainedStateChanged;
			if (onConstrainedStateChanged == null)
			{
				return;
			}
			onConstrainedStateChanged(isConstrained);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00003E6B File Offset: 0x0000206B
		[EngineCallback(null, false)]
		internal static void OnDLCInstalled()
		{
			Action onDLCInstalledCallback = EngineController.OnDLCInstalledCallback;
			if (onDLCInstalledCallback == null)
			{
				return;
			}
			onDLCInstalledCallback();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00003E7C File Offset: 0x0000207C
		[EngineCallback(null, false)]
		internal static void OnDLCLoaded()
		{
			Action onDLCLoadedCallback = EngineController.OnDLCLoadedCallback;
			if (onDLCLoadedCallback == null)
			{
				return;
			}
			onDLCLoadedCallback();
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00003E90 File Offset: 0x00002090
		[EngineCallback(null, false)]
		public static string GetVersionStr()
		{
			return ApplicationVersion.FromParametersFile(null).ToString();
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00003EB4 File Offset: 0x000020B4
		[EngineCallback(null, false)]
		public static string GetApplicationPlatformName()
		{
			return ApplicationPlatform.CurrentPlatform.ToString();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00003ED4 File Offset: 0x000020D4
		[EngineCallback(null, false)]
		public static string GetModulesVersionStr()
		{
			string text = "";
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules(null))
			{
				text = string.Concat(new object[] { text, moduleInfo.Name, "#", moduleInfo.Version, "\n" });
			}
			return text;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00003F60 File Offset: 0x00002160
		[EngineCallback(null, false)]
		internal static void OnControllerDisconnection()
		{
			ScreenManager.OnControllerDisconnect();
		}
	}
}
