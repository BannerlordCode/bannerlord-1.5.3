using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000AD RID: 173
	public class NativeOptions
	{
		// Token: 0x06000F86 RID: 3974 RVA: 0x00012738 File Offset: 0x00010938
		public static string GetGFXPresetName(NativeOptions.ConfigQuality presetIndex)
		{
			switch (presetIndex)
			{
			case NativeOptions.ConfigQuality.GFXVeryLow:
				return "1";
			case NativeOptions.ConfigQuality.GFXLow:
				return "2";
			case NativeOptions.ConfigQuality.GFXMedium:
				return "3";
			case NativeOptions.ConfigQuality.GFXHigh:
				return "4";
			case NativeOptions.ConfigQuality.GFXVeryHigh:
				return "5";
			case NativeOptions.ConfigQuality.GFXCustom:
				return "Custom";
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0001278E File Offset: 0x0001098E
		public static bool IsGFXOptionChangeable(NativeOptions.ConfigQuality config)
		{
			return config < NativeOptions.ConfigQuality.GFXCustom;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00012794 File Offset: 0x00010994
		private static void CorrectSelection(List<NativeOptionData> audioOptions)
		{
			foreach (NativeOptionData nativeOptionData in audioOptions)
			{
				if (nativeOptionData.Type == NativeOptions.NativeOptionsType.SoundDevice)
				{
					int num = 0;
					for (int i = 0; i < NativeOptions.GetSoundDeviceCount(); i++)
					{
						if (NativeOptions.GetSoundDeviceName(i) != "")
						{
							num = i;
						}
					}
					if (nativeOptionData.GetValue(false) > (float)num)
					{
						NativeOptions.SetConfig(NativeOptions.NativeOptionsType.SoundDevice, 0f);
						nativeOptionData.SetValue(0f);
					}
				}
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000F89 RID: 3977 RVA: 0x0001282C File Offset: 0x00010A2C
		// (remove) Token: 0x06000F8A RID: 3978 RVA: 0x00012860 File Offset: 0x00010A60
		public static event Action OnNativeOptionsApplied;

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00012894 File Offset: 0x00010A94
		public static List<NativeOptionData> VideoOptions
		{
			get
			{
				if (NativeOptions._videoOptions == null)
				{
					NativeOptions._videoOptions = new List<NativeOptionData>();
					for (NativeOptions.NativeOptionsType nativeOptionsType = NativeOptions.NativeOptionsType.None; nativeOptionsType < NativeOptions.NativeOptionsType.TotalOptions; nativeOptionsType++)
					{
						if (nativeOptionsType - NativeOptions.NativeOptionsType.DisplayMode <= 7 || nativeOptionsType == NativeOptions.NativeOptionsType.SharpenAmount)
						{
							NativeOptions._videoOptions.Add(new NativeNumericOptionData(nativeOptionsType));
						}
					}
				}
				return NativeOptions._videoOptions;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x000128E0 File Offset: 0x00010AE0
		public static List<NativeOptionData> GraphicsOptions
		{
			get
			{
				if (NativeOptions._graphicsOptions == null)
				{
					NativeOptions._graphicsOptions = new List<NativeOptionData>();
					for (NativeOptions.NativeOptionsType nativeOptionsType = NativeOptions.NativeOptionsType.None; nativeOptionsType < NativeOptions.NativeOptionsType.TotalOptions; nativeOptionsType++)
					{
						switch (nativeOptionsType)
						{
						case NativeOptions.NativeOptionsType.MaxSimultaneousSoundEventCount:
						case NativeOptions.NativeOptionsType.OverAll:
						case NativeOptions.NativeOptionsType.ShaderQuality:
						case NativeOptions.NativeOptionsType.TextureBudget:
						case NativeOptions.NativeOptionsType.TextureQuality:
						case NativeOptions.NativeOptionsType.ShadowmapResolution:
						case NativeOptions.NativeOptionsType.ShadowmapType:
						case NativeOptions.NativeOptionsType.ShadowmapFiltering:
						case NativeOptions.NativeOptionsType.ParticleDetail:
						case NativeOptions.NativeOptionsType.ParticleQuality:
						case NativeOptions.NativeOptionsType.FoliageQuality:
						case NativeOptions.NativeOptionsType.CharacterDetail:
						case NativeOptions.NativeOptionsType.EnvironmentDetail:
						case NativeOptions.NativeOptionsType.TerrainQuality:
						case NativeOptions.NativeOptionsType.NumberOfRagDolls:
						case NativeOptions.NativeOptionsType.AnimationSamplingQuality:
						case NativeOptions.NativeOptionsType.Occlusion:
						case NativeOptions.NativeOptionsType.TextureFiltering:
						case NativeOptions.NativeOptionsType.WaterQuality:
						case NativeOptions.NativeOptionsType.SSRQuality:
						case NativeOptions.NativeOptionsType.Antialiasing:
						case NativeOptions.NativeOptionsType.LightingQuality:
						case NativeOptions.NativeOptionsType.DecalQuality:
						case NativeOptions.NativeOptionsType.PhysicsTickRate:
							NativeOptions._graphicsOptions.Add(new NativeSelectionOptionData(nativeOptionsType));
							break;
						case NativeOptions.NativeOptionsType.DLSS:
							if (NativeOptions.GetIsDLSSAvailable())
							{
								NativeOptions._graphicsOptions.Add(new NativeSelectionOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.DepthOfField:
						case NativeOptions.NativeOptionsType.SSR:
						case NativeOptions.NativeOptionsType.ClothSimulation:
						case NativeOptions.NativeOptionsType.InteractiveGrass:
						case NativeOptions.NativeOptionsType.SunShafts:
						case NativeOptions.NativeOptionsType.SSSSS:
						case NativeOptions.NativeOptionsType.Tesselation:
						case NativeOptions.NativeOptionsType.Bloom:
						case NativeOptions.NativeOptionsType.FilmGrain:
						case NativeOptions.NativeOptionsType.MotionBlur:
						case NativeOptions.NativeOptionsType.DynamicResolution:
							NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							break;
						case NativeOptions.NativeOptionsType.PostFXLensFlare:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(62))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXStreaks:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(63))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXChromaticAberration:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(64))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXVignette:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(65))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXHexagonVignette:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(66))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.DynamicResolutionTarget:
							NativeOptions._graphicsOptions.Add(new NativeNumericOptionData(nativeOptionsType));
							break;
						}
					}
				}
				return NativeOptions._graphicsOptions;
			}
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00012B3C File Offset: 0x00010D3C
		public static void ReadRGLConfigFiles()
		{
			EngineApplicationInterface.IConfig.ReadRGLConfigFiles();
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00012B48 File Offset: 0x00010D48
		public static float GetConfig(NativeOptions.NativeOptionsType type)
		{
			return EngineApplicationInterface.IConfig.GetRGLConfig((int)type);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00012B55 File Offset: 0x00010D55
		public static float GetDefaultConfig(NativeOptions.NativeOptionsType type)
		{
			return EngineApplicationInterface.IConfig.GetDefaultRGLConfig((int)type);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00012B62 File Offset: 0x00010D62
		public static float GetDefaultConfigForOverallSettings(NativeOptions.NativeOptionsType type, int config)
		{
			return EngineApplicationInterface.IConfig.GetRGLConfigForDefaultSettings((int)type, config);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00012B70 File Offset: 0x00010D70
		public static int GetGameKeys(int keyType, int i)
		{
			Debug.FailedAssert("This is not implemented. Changed from Exception to not cause crash.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Options\\NativeOptions\\NativeOptions.cs", "GetGameKeys", 330);
			return 0;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00012B8C File Offset: 0x00010D8C
		public static string GetSoundDeviceName(int i)
		{
			return EngineApplicationInterface.IConfig.GetSoundDeviceName(i);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00012B99 File Offset: 0x00010D99
		public static string GetMonitorDeviceName(int i)
		{
			return EngineApplicationInterface.IConfig.GetMonitorDeviceName(i);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00012BA6 File Offset: 0x00010DA6
		public static string GetVideoDeviceName(int i)
		{
			return EngineApplicationInterface.IConfig.GetVideoDeviceName(i);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00012BB3 File Offset: 0x00010DB3
		public static int GetSoundDeviceCount()
		{
			return EngineApplicationInterface.IConfig.GetSoundDeviceCount();
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00012BBF File Offset: 0x00010DBF
		public static int GetMonitorDeviceCount()
		{
			return EngineApplicationInterface.IConfig.GetMonitorDeviceCount();
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00012BCB File Offset: 0x00010DCB
		public static int GetVideoDeviceCount()
		{
			return EngineApplicationInterface.IConfig.GetVideoDeviceCount();
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00012BD7 File Offset: 0x00010DD7
		public static int GetResolutionCount()
		{
			return EngineApplicationInterface.IConfig.GetResolutionCount();
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00012BE3 File Offset: 0x00010DE3
		public static void RefreshOptionsData()
		{
			EngineApplicationInterface.IConfig.RefreshOptionsData();
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00012BEF File Offset: 0x00010DEF
		public static int GetRefreshRateCount()
		{
			return EngineApplicationInterface.IConfig.GetRefreshRateCount();
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00012BFB File Offset: 0x00010DFB
		public static int GetRefreshRateAtIndex(int index)
		{
			return EngineApplicationInterface.IConfig.GetRefreshRateAtIndex(index);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00012C08 File Offset: 0x00010E08
		public static void SetCustomResolution(int width, int height)
		{
			EngineApplicationInterface.IConfig.SetCustomResolution(width, height);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00012C16 File Offset: 0x00010E16
		public static void GetResolution(ref int width, ref int height)
		{
			EngineApplicationInterface.IConfig.GetDesktopResolution(ref width, ref height);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00012C24 File Offset: 0x00010E24
		public static void GetDesktopResolution(ref int width, ref int height)
		{
			EngineApplicationInterface.IConfig.GetDesktopResolution(ref width, ref height);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00012C32 File Offset: 0x00010E32
		public static Vec2 GetResolutionAtIndex(int index)
		{
			return EngineApplicationInterface.IConfig.GetResolutionAtIndex(index);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00012C3F File Offset: 0x00010E3F
		public static int GetDLSSTechnique()
		{
			return EngineApplicationInterface.IConfig.GetDlssTechnique();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00012C4B File Offset: 0x00010E4B
		public static bool Is120HzAvailable()
		{
			return EngineApplicationInterface.IConfig.Is120HzAvailable();
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00012C57 File Offset: 0x00010E57
		public static int GetDLSSOptionCount()
		{
			return EngineApplicationInterface.IConfig.GetDlssOptionCount();
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00012C63 File Offset: 0x00010E63
		public static bool GetIsDLSSAvailable()
		{
			return EngineApplicationInterface.IConfig.IsDlssAvailable();
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00012C6F File Offset: 0x00010E6F
		public static bool CheckGFXSupportStatus(int enumType)
		{
			return EngineApplicationInterface.IConfig.CheckGFXSupportStatus(enumType);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00012C7C File Offset: 0x00010E7C
		public static void SetConfig(NativeOptions.NativeOptionsType type, float value)
		{
			EngineApplicationInterface.IConfig.SetRGLConfig((int)type, value);
			NativeOptions.OnNativeOptionChangedDelegate onNativeOptionChanged = NativeOptions.OnNativeOptionChanged;
			if (onNativeOptionChanged == null)
			{
				return;
			}
			onNativeOptionChanged(type);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00012C9A File Offset: 0x00010E9A
		public static void ApplyConfigChanges(bool resizeWindow)
		{
			EngineApplicationInterface.IConfig.ApplyConfigChanges(resizeWindow);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00012CA7 File Offset: 0x00010EA7
		public static void SetGameKeys(int keyType, int index, int key)
		{
			Debug.FailedAssert("This is not implemented. Changed from Exception to not cause crash.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Options\\NativeOptions\\NativeOptions.cs", "SetGameKeys", 443);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00012CC4 File Offset: 0x00010EC4
		public static void Apply(int texture_budget, int sharpen_amount, int hdr, int dof_mode, int motion_blur, int ssr, int size, int texture_filtering, int trail_amount, int dynamic_resolution_target)
		{
			EngineApplicationInterface.IConfig.Apply(texture_budget, sharpen_amount, hdr, dof_mode, motion_blur, ssr, size, texture_filtering, trail_amount, dynamic_resolution_target);
			Action onNativeOptionsApplied = NativeOptions.OnNativeOptionsApplied;
			if (onNativeOptionsApplied == null)
			{
				return;
			}
			onNativeOptionsApplied();
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00012CFA File Offset: 0x00010EFA
		public static SaveResult SaveConfig()
		{
			return (SaveResult)EngineApplicationInterface.IConfig.SaveRGLConfig();
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00012D06 File Offset: 0x00010F06
		public static void SetBrightness(float gamma)
		{
			EngineApplicationInterface.IConfig.SetBrightness(gamma);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00012D13 File Offset: 0x00010F13
		public static void SetDefaultGameKeys()
		{
			Debug.FailedAssert("This is not implemented. Changed from Exception to not cause crash.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Options\\NativeOptions\\NativeOptions.cs", "SetDefaultGameKeys", 468);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00012D2E File Offset: 0x00010F2E
		public static void SetDefaultGameConfig()
		{
			EngineApplicationInterface.IConfig.SetDefaultGameConfig();
		}

		// Token: 0x04000223 RID: 547
		public static NativeOptions.OnNativeOptionChangedDelegate OnNativeOptionChanged;

		// Token: 0x04000225 RID: 549
		private static List<NativeOptionData> _videoOptions;

		// Token: 0x04000226 RID: 550
		private static List<NativeOptionData> _graphicsOptions;

		// Token: 0x020000E7 RID: 231
		public enum ConfigQuality
		{
			// Token: 0x04000508 RID: 1288
			GFXVeryLow,
			// Token: 0x04000509 RID: 1289
			GFXLow,
			// Token: 0x0400050A RID: 1290
			GFXMedium,
			// Token: 0x0400050B RID: 1291
			GFXHigh,
			// Token: 0x0400050C RID: 1292
			GFXVeryHigh,
			// Token: 0x0400050D RID: 1293
			GFXCustom
		}

		// Token: 0x020000E8 RID: 232
		public enum NativeOptionsType
		{
			// Token: 0x0400050F RID: 1295
			None = -1,
			// Token: 0x04000510 RID: 1296
			MasterVolume,
			// Token: 0x04000511 RID: 1297
			SoundVolume,
			// Token: 0x04000512 RID: 1298
			MusicVolume,
			// Token: 0x04000513 RID: 1299
			VoiceChatVolume,
			// Token: 0x04000514 RID: 1300
			VoiceOverVolume,
			// Token: 0x04000515 RID: 1301
			SoundDevice,
			// Token: 0x04000516 RID: 1302
			MaxSimultaneousSoundEventCount,
			// Token: 0x04000517 RID: 1303
			SoundPreset,
			// Token: 0x04000518 RID: 1304
			KeepSoundInBackground,
			// Token: 0x04000519 RID: 1305
			SoundOcclusion,
			// Token: 0x0400051A RID: 1306
			MouseSensitivity,
			// Token: 0x0400051B RID: 1307
			InvertMouseYAxis,
			// Token: 0x0400051C RID: 1308
			MouseYMovementScale,
			// Token: 0x0400051D RID: 1309
			TrailAmount,
			// Token: 0x0400051E RID: 1310
			EnableVibration,
			// Token: 0x0400051F RID: 1311
			EnableGyroAssistedAim,
			// Token: 0x04000520 RID: 1312
			GyroAimSensitivity,
			// Token: 0x04000521 RID: 1313
			EnableTouchpadMouse,
			// Token: 0x04000522 RID: 1314
			EnableAlternateAiming,
			// Token: 0x04000523 RID: 1315
			DisplayMode,
			// Token: 0x04000524 RID: 1316
			SelectedMonitor,
			// Token: 0x04000525 RID: 1317
			SelectedAdapter,
			// Token: 0x04000526 RID: 1318
			ScreenResolution,
			// Token: 0x04000527 RID: 1319
			RefreshRate,
			// Token: 0x04000528 RID: 1320
			ResolutionScale,
			// Token: 0x04000529 RID: 1321
			FrameLimiter,
			// Token: 0x0400052A RID: 1322
			VSync,
			// Token: 0x0400052B RID: 1323
			Brightness,
			// Token: 0x0400052C RID: 1324
			OverAll,
			// Token: 0x0400052D RID: 1325
			ShaderQuality,
			// Token: 0x0400052E RID: 1326
			TextureBudget,
			// Token: 0x0400052F RID: 1327
			TextureQuality,
			// Token: 0x04000530 RID: 1328
			ShadowmapResolution,
			// Token: 0x04000531 RID: 1329
			ShadowmapType,
			// Token: 0x04000532 RID: 1330
			ShadowmapFiltering,
			// Token: 0x04000533 RID: 1331
			ParticleDetail,
			// Token: 0x04000534 RID: 1332
			ParticleQuality,
			// Token: 0x04000535 RID: 1333
			FoliageQuality,
			// Token: 0x04000536 RID: 1334
			CharacterDetail,
			// Token: 0x04000537 RID: 1335
			EnvironmentDetail,
			// Token: 0x04000538 RID: 1336
			TerrainQuality,
			// Token: 0x04000539 RID: 1337
			NumberOfRagDolls,
			// Token: 0x0400053A RID: 1338
			AnimationSamplingQuality,
			// Token: 0x0400053B RID: 1339
			Occlusion,
			// Token: 0x0400053C RID: 1340
			TextureFiltering,
			// Token: 0x0400053D RID: 1341
			WaterQuality,
			// Token: 0x0400053E RID: 1342
			SSRQuality,
			// Token: 0x0400053F RID: 1343
			Antialiasing,
			// Token: 0x04000540 RID: 1344
			DLSS,
			// Token: 0x04000541 RID: 1345
			LightingQuality,
			// Token: 0x04000542 RID: 1346
			DecalQuality,
			// Token: 0x04000543 RID: 1347
			DepthOfField,
			// Token: 0x04000544 RID: 1348
			SSR,
			// Token: 0x04000545 RID: 1349
			ClothSimulation,
			// Token: 0x04000546 RID: 1350
			InteractiveGrass,
			// Token: 0x04000547 RID: 1351
			SunShafts,
			// Token: 0x04000548 RID: 1352
			SSSSS,
			// Token: 0x04000549 RID: 1353
			Tesselation,
			// Token: 0x0400054A RID: 1354
			Bloom,
			// Token: 0x0400054B RID: 1355
			FilmGrain,
			// Token: 0x0400054C RID: 1356
			MotionBlur,
			// Token: 0x0400054D RID: 1357
			SharpenAmount,
			// Token: 0x0400054E RID: 1358
			PostFXLensFlare,
			// Token: 0x0400054F RID: 1359
			PostFXStreaks,
			// Token: 0x04000550 RID: 1360
			PostFXChromaticAberration,
			// Token: 0x04000551 RID: 1361
			PostFXVignette,
			// Token: 0x04000552 RID: 1362
			PostFXHexagonVignette,
			// Token: 0x04000553 RID: 1363
			BrightnessMin,
			// Token: 0x04000554 RID: 1364
			BrightnessMax,
			// Token: 0x04000555 RID: 1365
			BrightnessCalibrated,
			// Token: 0x04000556 RID: 1366
			ExposureCompensation,
			// Token: 0x04000557 RID: 1367
			DynamicResolution,
			// Token: 0x04000558 RID: 1368
			DynamicResolutionTarget,
			// Token: 0x04000559 RID: 1369
			FSR,
			// Token: 0x0400055A RID: 1370
			PhysicsTickRate,
			// Token: 0x0400055B RID: 1371
			NumOfOptionTypes,
			// Token: 0x0400055C RID: 1372
			TotalOptions
		}

		// Token: 0x020000E9 RID: 233
		// (Invoke) Token: 0x0600108D RID: 4237
		public delegate void OnNativeOptionChangedDelegate(NativeOptions.NativeOptionsType changedNativeOptionsType);
	}
}
