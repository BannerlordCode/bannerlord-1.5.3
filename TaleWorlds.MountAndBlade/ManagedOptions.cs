using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038D RID: 909
	public static class ManagedOptions
	{
		// Token: 0x06003492 RID: 13458 RVA: 0x000D9324 File Offset: 0x000D7524
		public static float GetConfig(ManagedOptions.ManagedOptionsType type)
		{
			switch (type)
			{
			case ManagedOptions.ManagedOptionsType.Language:
				return (float)LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode).IndexOf(BannerlordConfig.Language);
			case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				return (float)(BannerlordConfig.GyroOverrideForAttackDefend ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
				return (float)BannerlordConfig.DefendDirectionControl;
			case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
				return (float)BannerlordConfig.AttackDirectionControl;
			case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
				return (float)BannerlordConfig.NumberOfCorpses;
			case ManagedOptions.ManagedOptionsType.BattleSize:
				return (float)BannerlordConfig.BattleSize;
			case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
				return (float)BannerlordConfig.ReinforcementWaveCount;
			case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
				return (float)BannerlordConfig.TurnCameraWithHorseInFirstPerson;
			case ManagedOptions.ManagedOptionsType.ShowBlood:
				return (float)(BannerlordConfig.ShowBlood ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				return (float)(BannerlordConfig.DisplayAttackDirection ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				return (float)(BannerlordConfig.DisplayTargetingReticule ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				return (float)BannerlordConfig.AutoSaveInterval;
			case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
				return BannerlordConfig.FriendlyTroopsBannerOpacity;
			case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
				return (float)BannerlordConfig.AlwaysShowFriendlyTroopBannersType;
			case ManagedOptions.ManagedOptionsType.ShowFormationDistances:
				return (float)(BannerlordConfig.ShowFormationDistances ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportDamage:
				return (float)(BannerlordConfig.ReportDamage ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportBark:
				return (float)(BannerlordConfig.ReportBark ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.LockTarget:
				return (float)(BannerlordConfig.LockTarget ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableTutorialHints:
				return (float)(BannerlordConfig.EnableTutorialHints ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportCasualtiesType:
				return (float)BannerlordConfig.KillFeedVisualType;
			case ManagedOptions.ManagedOptionsType.ReportExperience:
				return (float)(BannerlordConfig.ReportExperience ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportPersonalDamage:
				return (float)(BannerlordConfig.ReportPersonalDamage ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.FirstPersonFov:
				return BannerlordConfig.FirstPersonFov;
			case ManagedOptions.ManagedOptionsType.CombatCameraDistance:
				return BannerlordConfig.CombatCameraDistance;
			case ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals:
				return (float)(BannerlordConfig.EnableDamageTakenVisuals ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableVoiceChat:
				return (float)(BannerlordConfig.EnableVoiceChat ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableDeathIcon:
				return (float)(BannerlordConfig.EnableDeathIcon ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons:
				return (float)(BannerlordConfig.EnableNetworkAlertIcons ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ForceVSyncInMenus:
				return (float)(BannerlordConfig.ForceVSyncInMenus ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection:
				return (float)(BannerlordConfig.EnableVerticalAimCorrection ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier:
				return BannerlordConfig.ZoomSensitivityModifier;
			case ManagedOptions.ManagedOptionsType.UIScale:
				return BannerlordConfig.UIScale;
			case ManagedOptions.ManagedOptionsType.CrosshairType:
				return (float)BannerlordConfig.CrosshairType;
			case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				return (float)(BannerlordConfig.EnableGenericAvatars ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableGenericNames:
				return (float)(BannerlordConfig.EnableGenericNames ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.OrderType:
				return (float)BannerlordConfig.OrderType;
			case ManagedOptions.ManagedOptionsType.OrderLayoutType:
				return (float)BannerlordConfig.OrderLayoutType;
			case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
				return (float)BannerlordConfig.AutoTrackAttackedSettlements;
			case ManagedOptions.ManagedOptionsType.StopGameOnFocusLost:
				return (float)(BannerlordConfig.StopGameOnFocusLost ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.SlowDownOnOrder:
				return (float)(BannerlordConfig.SlowDownOnOrder ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideFullServers:
				return (float)(BannerlordConfig.HideFullServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideEmptyServers:
				return (float)(BannerlordConfig.HideEmptyServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HidePasswordProtectedServers:
				return (float)(BannerlordConfig.HidePasswordProtectedServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideUnofficialServers:
				return (float)(BannerlordConfig.HideUnofficialServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideModuleIncompatibleServers:
				return (float)(BannerlordConfig.HideModuleIncompatibleServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideBattleUI:
				return (float)(BannerlordConfig.HideBattleUI ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
				return (float)BannerlordConfig.UnitSpawnPrioritization;
			case ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox:
				return (float)(BannerlordConfig.EnableSingleplayerChatBox ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox:
				return (float)(BannerlordConfig.EnableMultiplayerChatBox ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.VoiceLanguage:
				return (float)LocalizedVoiceManager.GetVoiceLanguageIds().IndexOf(BannerlordConfig.VoiceLanguage);
			case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
				return (float)BannerlordConfig.PlayerReceivedDamageDifficulty;
			case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
				return (float)BannerlordConfig.MapDoubleClickBehavior;
			default:
				Debug.FailedAssert("ManagedOptionsType not found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "GetConfig", 180);
				return 0f;
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000D9658 File Offset: 0x000D7858
		public static float GetDefaultConfig(ManagedOptions.ManagedOptionsType type)
		{
			switch (type)
			{
			case ManagedOptions.ManagedOptionsType.Language:
				return (float)LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode).IndexOf(BannerlordConfig.DefaultLanguage);
			case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				return 0f;
			case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
				return 0f;
			case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
				return 1f;
			case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
				return 3f;
			case ManagedOptions.ManagedOptionsType.BattleSize:
				return 2f;
			case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
				return 3f;
			case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
				return 2f;
			case ManagedOptions.ManagedOptionsType.ShowBlood:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				return 1f;
			case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				return 30f;
			case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
				return 1f;
			case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ShowFormationDistances:
				return 0f;
			case ManagedOptions.ManagedOptionsType.ReportDamage:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportBark:
				return 1f;
			case ManagedOptions.ManagedOptionsType.LockTarget:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableTutorialHints:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportCasualtiesType:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportExperience:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportPersonalDamage:
				return 1f;
			case ManagedOptions.ManagedOptionsType.FirstPersonFov:
				return 65f;
			case ManagedOptions.ManagedOptionsType.CombatCameraDistance:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableVoiceChat:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableDeathIcon:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ForceVSyncInMenus:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier:
				return 0.66666f;
			case ManagedOptions.ManagedOptionsType.UIScale:
				return 1f;
			case ManagedOptions.ManagedOptionsType.CrosshairType:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableGenericNames:
				return 0f;
			case ManagedOptions.ManagedOptionsType.OrderType:
				return 0f;
			case ManagedOptions.ManagedOptionsType.OrderLayoutType:
				return 0f;
			case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
				return 0f;
			case ManagedOptions.ManagedOptionsType.StopGameOnFocusLost:
				return 1f;
			case ManagedOptions.ManagedOptionsType.SlowDownOnOrder:
				return 1f;
			case ManagedOptions.ManagedOptionsType.HideFullServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideEmptyServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HidePasswordProtectedServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideUnofficialServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideModuleIncompatibleServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideBattleUI:
				return 0f;
			case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox:
				return 1f;
			case ManagedOptions.ManagedOptionsType.VoiceLanguage:
				return (float)LocalizedVoiceManager.GetVoiceLanguageIds().IndexOf(BannerlordConfig.VoiceLanguage);
			case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
				return 0f;
			case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
				return 0f;
			default:
				Debug.FailedAssert("ManagedOptionsType not found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "GetDefaultConfig", 293);
				return 0f;
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000D98B1 File Offset: 0x000D7AB1
		[MBCallback(null, true)]
		internal static int GetConfigCount()
		{
			return 52;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000D98B5 File Offset: 0x000D7AB5
		[MBCallback(null, true)]
		internal static float GetConfigValue(int type)
		{
			return ManagedOptions.GetConfig((ManagedOptions.ManagedOptionsType)type);
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000D98C0 File Offset: 0x000D7AC0
		public static void SetConfig(ManagedOptions.ManagedOptionsType type, float value)
		{
			switch (type)
			{
			case ManagedOptions.ManagedOptionsType.Language:
			{
				List<string> list = LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode);
				if (value >= 0f && value < (float)list.Count)
				{
					BannerlordConfig.Language = list[(int)value];
				}
				else
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "SetConfig", 459);
					BannerlordConfig.Language = list[0];
				}
				break;
			}
			case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				BannerlordConfig.GyroOverrideForAttackDefend = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
				BannerlordConfig.DefendDirectionControl = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
				BannerlordConfig.AttackDirectionControl = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
				BannerlordConfig.NumberOfCorpses = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.BattleSize:
				BannerlordConfig.BattleSize = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
				BannerlordConfig.ReinforcementWaveCount = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
				BannerlordConfig.TurnCameraWithHorseInFirstPerson = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ShowBlood:
				BannerlordConfig.ShowBlood = (double)value != 0.0;
				break;
			case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				BannerlordConfig.DisplayAttackDirection = (double)value != 0.0;
				break;
			case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				BannerlordConfig.DisplayTargetingReticule = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				BannerlordConfig.AutoSaveInterval = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
				BannerlordConfig.FriendlyTroopsBannerOpacity = value;
				break;
			case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
				BannerlordConfig.AlwaysShowFriendlyTroopBannersType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ShowFormationDistances:
				BannerlordConfig.ShowFormationDistances = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportDamage:
				BannerlordConfig.ReportDamage = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportBark:
				BannerlordConfig.ReportBark = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.LockTarget:
				BannerlordConfig.LockTarget = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableTutorialHints:
				BannerlordConfig.EnableTutorialHints = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportCasualtiesType:
				BannerlordConfig.KillFeedVisualType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ReportExperience:
				BannerlordConfig.ReportExperience = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportPersonalDamage:
				BannerlordConfig.ReportPersonalDamage = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.FirstPersonFov:
				BannerlordConfig.FirstPersonFov = value;
				break;
			case ManagedOptions.ManagedOptionsType.CombatCameraDistance:
				BannerlordConfig.CombatCameraDistance = value;
				break;
			case ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals:
				BannerlordConfig.EnableDamageTakenVisuals = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableVoiceChat:
				BannerlordConfig.EnableVoiceChat = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableDeathIcon:
				BannerlordConfig.EnableDeathIcon = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons:
				BannerlordConfig.EnableNetworkAlertIcons = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ForceVSyncInMenus:
				BannerlordConfig.ForceVSyncInMenus = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection:
				BannerlordConfig.EnableVerticalAimCorrection = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier:
				BannerlordConfig.ZoomSensitivityModifier = value;
				break;
			case ManagedOptions.ManagedOptionsType.UIScale:
				BannerlordConfig.UIScale = value;
				break;
			case ManagedOptions.ManagedOptionsType.CrosshairType:
				BannerlordConfig.CrosshairType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				BannerlordConfig.EnableGenericAvatars = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableGenericNames:
				BannerlordConfig.EnableGenericNames = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.OrderType:
				BannerlordConfig.OrderType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.OrderLayoutType:
				BannerlordConfig.OrderLayoutType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
				BannerlordConfig.AutoTrackAttackedSettlements = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.StopGameOnFocusLost:
				BannerlordConfig.StopGameOnFocusLost = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.SlowDownOnOrder:
				BannerlordConfig.SlowDownOnOrder = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideFullServers:
				BannerlordConfig.HideFullServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideEmptyServers:
				BannerlordConfig.HideEmptyServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HidePasswordProtectedServers:
				BannerlordConfig.HidePasswordProtectedServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideUnofficialServers:
				BannerlordConfig.HideUnofficialServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideModuleIncompatibleServers:
				BannerlordConfig.HideModuleIncompatibleServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideBattleUI:
				BannerlordConfig.HideBattleUI = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
				BannerlordConfig.UnitSpawnPrioritization = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox:
				BannerlordConfig.EnableSingleplayerChatBox = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox:
				BannerlordConfig.EnableMultiplayerChatBox = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.VoiceLanguage:
			{
				List<string> list = LocalizedVoiceManager.GetVoiceLanguageIds();
				if (value >= 0f && value < (float)list.Count)
				{
					BannerlordConfig.VoiceLanguage = list[(int)value];
				}
				else
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "SetConfig", 477);
					BannerlordConfig.VoiceLanguage = list[0];
				}
				break;
			}
			case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
				BannerlordConfig.PlayerReceivedDamageDifficulty = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
				BannerlordConfig.MapDoubleClickBehavior = (int)value;
				break;
			}
			ManagedOptions.OnManagedOptionChangedDelegate onManagedOptionChanged = ManagedOptions.OnManagedOptionChanged;
			if (onManagedOptionChanged == null)
			{
				return;
			}
			onManagedOptionChanged(type);
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000D9DB0 File Offset: 0x000D7FB0
		public static SaveResult SaveConfig()
		{
			return BannerlordConfig.Save();
		}

		// Token: 0x04001659 RID: 5721
		public static ManagedOptions.OnManagedOptionChangedDelegate OnManagedOptionChanged;

		// Token: 0x02000667 RID: 1639
		public enum ManagedOptionsType
		{
			// Token: 0x040021F9 RID: 8697
			Language,
			// Token: 0x040021FA RID: 8698
			GyroOverrideForAttackDefend,
			// Token: 0x040021FB RID: 8699
			ControlBlockDirection,
			// Token: 0x040021FC RID: 8700
			ControlAttackDirection,
			// Token: 0x040021FD RID: 8701
			NumberOfCorpses,
			// Token: 0x040021FE RID: 8702
			BattleSize,
			// Token: 0x040021FF RID: 8703
			ReinforcementWaveCount,
			// Token: 0x04002200 RID: 8704
			TurnCameraWithHorseInFirstPerson,
			// Token: 0x04002201 RID: 8705
			ShowBlood,
			// Token: 0x04002202 RID: 8706
			ShowAttackDirection,
			// Token: 0x04002203 RID: 8707
			ShowTargetingReticle,
			// Token: 0x04002204 RID: 8708
			AutoSaveInterval,
			// Token: 0x04002205 RID: 8709
			FriendlyTroopsBannerOpacity,
			// Token: 0x04002206 RID: 8710
			AlwaysShowFriendlyTroopBannersType,
			// Token: 0x04002207 RID: 8711
			ShowFormationDistances,
			// Token: 0x04002208 RID: 8712
			ReportDamage,
			// Token: 0x04002209 RID: 8713
			ReportBark,
			// Token: 0x0400220A RID: 8714
			LockTarget,
			// Token: 0x0400220B RID: 8715
			EnableTutorialHints,
			// Token: 0x0400220C RID: 8716
			ReportCasualtiesType,
			// Token: 0x0400220D RID: 8717
			ReportExperience,
			// Token: 0x0400220E RID: 8718
			ReportPersonalDamage,
			// Token: 0x0400220F RID: 8719
			FirstPersonFov,
			// Token: 0x04002210 RID: 8720
			CombatCameraDistance,
			// Token: 0x04002211 RID: 8721
			EnableDamageTakenVisuals,
			// Token: 0x04002212 RID: 8722
			EnableVoiceChat,
			// Token: 0x04002213 RID: 8723
			EnableDeathIcon,
			// Token: 0x04002214 RID: 8724
			EnableNetworkAlertIcons,
			// Token: 0x04002215 RID: 8725
			ForceVSyncInMenus,
			// Token: 0x04002216 RID: 8726
			EnableVerticalAimCorrection,
			// Token: 0x04002217 RID: 8727
			ZoomSensitivityModifier,
			// Token: 0x04002218 RID: 8728
			UIScale,
			// Token: 0x04002219 RID: 8729
			CrosshairType,
			// Token: 0x0400221A RID: 8730
			EnableGenericAvatars,
			// Token: 0x0400221B RID: 8731
			EnableGenericNames,
			// Token: 0x0400221C RID: 8732
			OrderType,
			// Token: 0x0400221D RID: 8733
			OrderLayoutType,
			// Token: 0x0400221E RID: 8734
			AutoTrackAttackedSettlements,
			// Token: 0x0400221F RID: 8735
			StopGameOnFocusLost,
			// Token: 0x04002220 RID: 8736
			SlowDownOnOrder,
			// Token: 0x04002221 RID: 8737
			HideFullServers,
			// Token: 0x04002222 RID: 8738
			HideEmptyServers,
			// Token: 0x04002223 RID: 8739
			HidePasswordProtectedServers,
			// Token: 0x04002224 RID: 8740
			HideUnofficialServers,
			// Token: 0x04002225 RID: 8741
			HideModuleIncompatibleServers,
			// Token: 0x04002226 RID: 8742
			HideBattleUI,
			// Token: 0x04002227 RID: 8743
			UnitSpawnPrioritization,
			// Token: 0x04002228 RID: 8744
			EnableSingleplayerChatBox,
			// Token: 0x04002229 RID: 8745
			EnableMultiplayerChatBox,
			// Token: 0x0400222A RID: 8746
			VoiceLanguage,
			// Token: 0x0400222B RID: 8747
			PlayerReceivedDamageDifficulty,
			// Token: 0x0400222C RID: 8748
			MapDoubleClickBehavior,
			// Token: 0x0400222D RID: 8749
			ManagedOptionTypeCount
		}

		// Token: 0x02000668 RID: 1640
		// (Invoke) Token: 0x0600414F RID: 16719
		public delegate void OnManagedOptionChangedDelegate(ManagedOptions.ManagedOptionsType changedManagedOptionsType);
	}
}
