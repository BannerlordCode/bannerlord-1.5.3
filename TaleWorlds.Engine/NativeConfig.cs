using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.Engine
{
	// Token: 0x02000072 RID: 114
	public static class NativeConfig
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0000A930 File Offset: 0x00008B30
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x0000A937 File Offset: 0x00008B37
		public static bool CheatMode { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0000A93F File Offset: 0x00008B3F
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0000A946 File Offset: 0x00008B46
		public static bool IsDevelopmentMode { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0000A94E File Offset: 0x00008B4E
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0000A955 File Offset: 0x00008B55
		public static bool IsDetailedDebugMode { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0000A95D File Offset: 0x00008B5D
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x0000A964 File Offset: 0x00008B64
		public static bool LocalizationDebugMode { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0000A96C File Offset: 0x00008B6C
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x0000A973 File Offset: 0x00008B73
		public static bool GetUIDebugMode { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0000A97B File Offset: 0x00008B7B
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x0000A982 File Offset: 0x00008B82
		public static bool DisableSound { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0000A98A File Offset: 0x00008B8A
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x0000A991 File Offset: 0x00008B91
		public static bool EnableEditMode { get; private set; }

		// Token: 0x06000A76 RID: 2678 RVA: 0x0000A99C File Offset: 0x00008B9C
		public static void OnConfigChanged()
		{
			NativeConfig.CheatMode = EngineApplicationInterface.IConfig.GetCheatMode();
			NativeConfig.IsDevelopmentMode = EngineApplicationInterface.IConfig.GetDevelopmentMode();
			NativeConfig.IsDetailedDebugMode = EngineApplicationInterface.IConfig.GetDetailedDebugMode();
			NativeConfig.GetUIDebugMode = EngineApplicationInterface.IConfig.GetUIDebugMode();
			NativeConfig.LocalizationDebugMode = EngineApplicationInterface.IConfig.GetLocalizationDebugMode();
			NativeConfig.EnableEditMode = EngineApplicationInterface.IConfig.GetEnableEditMode();
			NativeConfig.DisableSound = EngineApplicationInterface.IConfig.GetDisableSound();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0000AA12 File Offset: 0x00008C12
		public static bool TableauCacheEnabled
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetTableauCacheMode();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0000AA1E File Offset: 0x00008C1E
		public static bool DoLocalizationCheckAtStartup
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDoLocalizationCheckAtStartup();
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0000AA2A File Offset: 0x00008C2A
		public static bool EnableClothSimulation
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetEnableClothSimulation();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0000AA36 File Offset: 0x00008C36
		public static int CharacterDetail
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetCharacterDetail();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0000AA42 File Offset: 0x00008C42
		public static bool InvertMouse
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetInvertMouse();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0000AA4E File Offset: 0x00008C4E
		public static string LastOpenedScene
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetLastOpenedScene();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0000AA5A File Offset: 0x00008C5A
		public static int AutoSaveInMinutes
		{
			get
			{
				return EngineApplicationInterface.IConfig.AutoSaveInMinutes();
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0000AA66 File Offset: 0x00008C66
		public static bool GetUIDoNotUseGeneratedPrefabs
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetUIDoNotUseGeneratedPrefabs();
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0000AA72 File Offset: 0x00008C72
		public static string DebugLoginUsername
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDebugLoginUserName();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0000AA7E File Offset: 0x00008C7E
		public static string DebugLogicPassword
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDebugLoginPassword();
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0000AA8A File Offset: 0x00008C8A
		public static bool DisableGuiMessages
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDisableGuiMessages();
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0000AA96 File Offset: 0x00008C96
		public static NativeOptions.ConfigQuality AutoGFXQuality
		{
			get
			{
				return (NativeOptions.ConfigQuality)EngineApplicationInterface.IConfig.GetAutoGFXQuality();
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0000AAA2 File Offset: 0x00008CA2
		public static void SetAutoConfigWrtHardware()
		{
			EngineApplicationInterface.IConfig.SetAutoConfigWrtHardware();
		}
	}
}
