using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Options.ManagedOptions;

namespace TaleWorlds.MountAndBlade.Options
{
	// Token: 0x020003A2 RID: 930
	public static class OptionsProvider
	{
		// Token: 0x06003568 RID: 13672 RVA: 0x000DD23B File Offset: 0x000DB43B
		public static OptionCategory GetVideoOptionCategory(bool isMainMenu, Action onBrightnessClick, Action onExposureClick, Action onBenchmarkClick)
		{
			return new OptionCategory(OptionsProvider.GetVideoGeneralOptions(isMainMenu, onBrightnessClick, onExposureClick, onBenchmarkClick), OptionsProvider.GetVideoOptionGroups());
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000DD250 File Offset: 0x000DB450
		private static IEnumerable<IOptionData> GetVideoGeneralOptions(bool isMainMenu, Action onBrightnessClick, Action onExposureClick, Action onBenchmarkClick)
		{
			if (isMainMenu)
			{
				yield return new ActionOptionData("Benchmark", onBenchmarkClick);
			}
			yield return new ActionOptionData(NativeOptions.NativeOptionsType.Brightness, onBrightnessClick);
			yield return new ActionOptionData(NativeOptions.NativeOptionsType.ExposureCompensation, onExposureClick);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SelectedMonitor);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SelectedAdapter);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.DisplayMode);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ScreenResolution);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.RefreshRate);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.VSync);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ForceVSyncInMenus);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.FrameLimiter);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.SharpenAmount);
			yield break;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000DD275 File Offset: 0x000DB475
		private static IEnumerable<IOptionData> GetPerformanceGeneralOptions(bool isMultiplayer)
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.OverAll);
			yield break;
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000DD27E File Offset: 0x000DB47E
		private static IEnumerable<OptionGroup> GetVideoOptionGroups()
		{
			return null;
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000DD281 File Offset: 0x000DB481
		public static OptionCategory GetPerformanceOptionCategory(bool isMultiplayer)
		{
			return new OptionCategory(OptionsProvider.GetPerformanceGeneralOptions(isMultiplayer), OptionsProvider.GetPerformanceOptionGroups(isMultiplayer));
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000DD294 File Offset: 0x000DB494
		private static IEnumerable<OptionGroup> GetPerformanceOptionGroups(bool isMultiplayer)
		{
			yield return new OptionGroup(new TextObject("{=sRTd3RI5}Graphics", null), OptionsProvider.GetPerformanceGraphicsOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=vDMe8SCV}Resolution Scaling", null), OptionsProvider.GetPerformanceResolutionScalingOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=2zcrC0h1}Gameplay", null), OptionsProvider.GetPerformanceGameplayOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=xebFLnH2}Audio", null), OptionsProvider.GetPerformanceAudioOptions());
			yield break;
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000DD2A4 File Offset: 0x000DB4A4
		public static IEnumerable<IOptionData> GetPerformanceGraphicsOptions(bool isMultiplayer)
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.Antialiasing);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShaderQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TextureBudget);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TextureQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TextureFiltering);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.CharacterDetail);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShadowmapResolution);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShadowmapType);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShadowmapFiltering);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ParticleDetail);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ParticleQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.FoliageQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TerrainQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.EnvironmentDetail);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.Occlusion);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.DecalQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.WaterQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SSRQuality);
			if (!isMultiplayer)
			{
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.NumberOfCorpses);
			}
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.NumberOfRagDolls);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.LightingQuality);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.ClothSimulation);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SunShafts);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.Tesselation);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.InteractiveGrass);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SSR);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SSSSS);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.MotionBlur);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.DepthOfField);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.Bloom);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.FilmGrain);
			if (NativeOptions.CheckGFXSupportStatus(65))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXVignette);
			}
			if (NativeOptions.CheckGFXSupportStatus(64))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXChromaticAberration);
			}
			if (NativeOptions.CheckGFXSupportStatus(62))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXLensFlare);
			}
			if (NativeOptions.CheckGFXSupportStatus(66))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXHexagonVignette);
			}
			if (NativeOptions.CheckGFXSupportStatus(63))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXStreaks);
			}
			yield break;
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000DD2B4 File Offset: 0x000DB4B4
		public static IEnumerable<IOptionData> GetPerformanceResolutionScalingOptions(bool isMultiplayer)
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.DLSS);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.ResolutionScale);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.DynamicResolution);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.DynamicResolutionTarget);
			yield break;
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000DD2BD File Offset: 0x000DB4BD
		public static IEnumerable<IOptionData> GetPerformanceGameplayOptions(bool isMultiplayer)
		{
			if (!isMultiplayer)
			{
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.BattleSize);
				yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.PhysicsTickRate);
			}
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.AnimationSamplingQuality);
			yield break;
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000DD2CD File Offset: 0x000DB4CD
		public static IEnumerable<IOptionData> GetPerformanceAudioOptions()
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.MaxSimultaneousSoundEventCount);
			yield break;
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000DD2D6 File Offset: 0x000DB4D6
		private static IEnumerable<IOptionData> GetAudioGeneralOptions(bool isMultiplayer)
		{
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MasterVolume);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.SoundVolume);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MusicVolume);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.VoiceOverVolume);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SoundPreset);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SoundDevice);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.KeepSoundInBackground);
			if (isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableVoiceChat);
			}
			else
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SoundOcclusion);
			}
			yield break;
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000DD2E6 File Offset: 0x000DB4E6
		public static OptionCategory GetAudioOptionCategory(bool isMultiplayer)
		{
			return new OptionCategory(OptionsProvider.GetAudioGeneralOptions(isMultiplayer), OptionsProvider.GetAudioOptionGroups(isMultiplayer));
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000DD2F9 File Offset: 0x000DB4F9
		private static IEnumerable<OptionGroup> GetAudioOptionGroups(bool isMultiplayer)
		{
			return null;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000DD2FC File Offset: 0x000DB4FC
		public static OptionCategory GetGameplayOptionCategory(bool isMainMenu, bool isMultiplayer)
		{
			return new OptionCategory(OptionsProvider.GetGameplayGeneralOptions(isMultiplayer), OptionsProvider.GetGameplayOptionGroups(isMainMenu, isMultiplayer));
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000DD310 File Offset: 0x000DB510
		private static IEnumerable<IOptionData> GetGameplayGeneralOptions(bool isMultiplayer)
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.Language);
			if (!isMultiplayer)
			{
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.VoiceLanguage);
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty);
			}
			yield break;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000DD320 File Offset: 0x000DB520
		private static IEnumerable<OptionGroup> GetGameplayOptionGroups(bool isMainMenu, bool isMultiplayer)
		{
			yield return new OptionGroup(new TextObject("{=m9KoYCv5}Controls", null), OptionsProvider.GetGameplayControlsOptions(isMainMenu, isMultiplayer));
			yield return new OptionGroup(new TextObject("{=uZ6q4Qs2}Visuals", null), OptionsProvider.GetGameplayVisualOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=gAfbULHM}Camera", null), OptionsProvider.GetGameplayCameraOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=WRMyiiYJ}User Interface", null), OptionsProvider.GetGameplayUIOptions(isMultiplayer));
			if (!isMultiplayer)
			{
				yield return new OptionGroup(new TextObject("{=ys9baYiQ}Campaign", null), OptionsProvider.GetGameplayCampaignOptions());
			}
			yield break;
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000DD337 File Offset: 0x000DB537
		private static IEnumerable<IOptionData> GetGameplayControlsOptions(bool isMainMenu, bool isMultiplayer)
		{
			bool isDualSense = Input.ControllerType.IsPlaystation();
			if (isDualSense)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend);
			}
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ControlBlockDirection);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ControlAttackDirection);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MouseYMovementScale);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MouseSensitivity);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.InvertMouseYAxis);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableVibration);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableAlternateAiming);
			if (isDualSense)
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableTouchpadMouse);
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableGyroAssistedAim);
				yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.GyroAimSensitivity);
			}
			if (!isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.LockTarget);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.SlowDownOnOrder);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.StopGameOnFocusLost);
			}
			yield break;
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000DD347 File Offset: 0x000DB547
		private static IEnumerable<IOptionData> GetGameplayVisualOptions(bool isMultiplayer)
		{
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.TrailAmount);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType);
			if (!isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowFormationDistances);
			}
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowBlood);
			yield break;
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000DD357 File Offset: 0x000DB557
		private static IEnumerable<IOptionData> GetGameplayCameraOptions(bool isMultiplayer)
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.FirstPersonFov);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.CombatCameraDistance);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier);
			yield break;
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000DD360 File Offset: 0x000DB560
		private static IEnumerable<IOptionData> GetGameplayUIOptions(bool isMultiplayer)
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.CrosshairType);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.OrderType);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.OrderLayoutType);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ReportCasualtiesType);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.UIScale);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowAttackDirection);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowTargetingReticle);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportDamage);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportBark);
			if (!isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportExperience);
			}
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportPersonalDamage);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals);
			if (isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableDeathIcon);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableGenericAvatars);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableGenericNames);
			}
			else
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.HideBattleUI);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableTutorialHints);
			}
			yield break;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000DD370 File Offset: 0x000DB570
		private static IEnumerable<IOptionData> GetGameplayCampaignOptions()
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.AutoSaveInterval);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ReinforcementWaveCount);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior);
			yield break;
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000DD379 File Offset: 0x000DB579
		public static IEnumerable<string> GetGameKeyCategoriesList(bool isMultiplayer)
		{
			yield return GameKeyMainCategories.ActionCategory;
			yield return GameKeyMainCategories.OrderMenuCategory;
			if (!isMultiplayer)
			{
				yield return GameKeyMainCategories.ShipControlsCategory;
				yield return GameKeyMainCategories.CampaignMapCategory;
				yield return GameKeyMainCategories.MenuShortcutCategory;
				yield return GameKeyMainCategories.PhotoModeCategory;
			}
			else
			{
				yield return GameKeyMainCategories.PollCategory;
			}
			yield return GameKeyMainCategories.ChatCategory;
			yield break;
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000DD389 File Offset: 0x000DB589
		public static IEnumerable<int> GetHiddenGameKeys(bool isNavalModuleActive)
		{
			if (!isNavalModuleActive)
			{
				yield return 45;
			}
			yield break;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000DD399 File Offset: 0x000DB599
		public static OptionCategory GetControllerOptionCategory()
		{
			return new OptionCategory(OptionsProvider.GetControllerBaseOptions(), OptionsProvider.GetControllerOptionGroups());
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000DD3AA File Offset: 0x000DB5AA
		private static IEnumerable<IOptionData> GetControllerBaseOptions()
		{
			return null;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000DD3AD File Offset: 0x000DB5AD
		private static IEnumerable<OptionGroup> GetControllerOptionGroups()
		{
			return null;
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000DD3B0 File Offset: 0x000DB5B0
		public static Dictionary<NativeOptions.NativeOptionsType, float[]> GetDefaultNativeOptions()
		{
			if (OptionsProvider._defaultNativeOptions == null)
			{
				OptionsProvider._defaultNativeOptions = new Dictionary<NativeOptions.NativeOptionsType, float[]>();
				foreach (NativeOptionData nativeOptionData in NativeOptions.VideoOptions.Union<NativeOptionData>(NativeOptions.GraphicsOptions))
				{
					float[] array = new float[OptionsProvider._overallConfigCount];
					bool flag = false;
					for (int i = 0; i < OptionsProvider._overallConfigCount; i++)
					{
						array[i] = NativeOptions.GetDefaultConfigForOverallSettings(nativeOptionData.Type, i);
						if (array[i] < 0f)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						OptionsProvider._defaultNativeOptions[nativeOptionData.Type] = array;
					}
				}
			}
			return OptionsProvider._defaultNativeOptions;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000DD470 File Offset: 0x000DB670
		public static Dictionary<ManagedOptions.ManagedOptionsType, float[]> GetDefaultManagedOptions()
		{
			if (OptionsProvider._defaultManagedOptions == null)
			{
				OptionsProvider._defaultManagedOptions = new Dictionary<ManagedOptions.ManagedOptionsType, float[]>();
				float[] array = new float[OptionsProvider._overallConfigCount];
				for (int i = 0; i < OptionsProvider._overallConfigCount; i++)
				{
					array[i] = (float)i;
				}
				OptionsProvider._defaultManagedOptions.Add(ManagedOptions.ManagedOptionsType.BattleSize, array);
				array = new float[OptionsProvider._overallConfigCount];
				for (int j = 0; j < OptionsProvider._overallConfigCount; j++)
				{
					array[j] = (float)j;
				}
				OptionsProvider._defaultManagedOptions.Add(ManagedOptions.ManagedOptionsType.NumberOfCorpses, array);
			}
			return OptionsProvider._defaultManagedOptions;
		}

		// Token: 0x040016D0 RID: 5840
		private static readonly int _overallConfigCount = NativeSelectionOptionData.GetOptionsLimit(NativeOptions.NativeOptionsType.OverAll) - 1;

		// Token: 0x040016D1 RID: 5841
		private static Dictionary<NativeOptions.NativeOptionsType, float[]> _defaultNativeOptions;

		// Token: 0x040016D2 RID: 5842
		private static Dictionary<ManagedOptions.ManagedOptionsType, float[]> _defaultManagedOptions;
	}
}
