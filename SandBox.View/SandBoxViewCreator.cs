using System;
using System.Collections.Generic;
using System.Reflection;
using SandBox.AdvancedStartOptions;
using SandBox.View.Map;
using SandBox.View.Menu;
using SandBox.View.Missions;
using SandBox.View.Missions.NameMarkers;
using SandBox.View.Missions.Tournaments;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.ScreenSystem;

namespace SandBox.View
{
	// Token: 0x0200000B RID: 11
	public static class SandBoxViewCreator
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000033A0 File Offset: 0x000015A0
		static SandBoxViewCreator()
		{
			SandBoxViewCreator.CollectTypes();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000033A8 File Offset: 0x000015A8
		private static void CollectTypes()
		{
			SandBoxViewCreator._actualViewTypes = new Dictionary<Type, MBList<Type>>();
			Assembly assembly = typeof(ViewCreatorModule).Assembly;
			Assembly[] referencingAssembliesSafe = assembly.GetReferencingAssembliesSafe(null);
			SandBoxViewCreator.CheckOverridenViews(assembly);
			Assembly[] array = referencingAssembliesSafe;
			for (int i = 0; i < array.Length; i++)
			{
				SandBoxViewCreator.CheckOverridenViews(array[i]);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000033F4 File Offset: 0x000015F4
		private static void CheckOverridenViews(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				if (typeof(MapView).IsAssignableFrom(type) || typeof(MenuView).IsAssignableFrom(type) || typeof(MissionView).IsAssignableFrom(type) || typeof(ScreenBase).IsAssignableFrom(type))
				{
					object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(OverrideView), false);
					if (customAttributesSafe != null && customAttributesSafe.Length == 1)
					{
						OverrideView overrideView = customAttributesSafe[0] as OverrideView;
						if (overrideView != null)
						{
							MBList<Type> mblist;
							if (SandBoxViewCreator._actualViewTypes.TryGetValue(overrideView.BaseType, out mblist))
							{
								mblist.Add(type);
							}
							else
							{
								SandBoxViewCreator._actualViewTypes[overrideView.BaseType] = new MBList<Type> { type };
							}
						}
					}
				}
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000034F0 File Offset: 0x000016F0
		public static ScreenBase CreateSaveLoadScreen(bool isSaving)
		{
			return ViewCreatorManager.CreateScreenView<SaveLoadScreen>(new object[] { isSaving });
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003506 File Offset: 0x00001706
		public static MissionView CreateMissionNameMarkerUIHandler(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionNameMarkerUIHandler>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003517 File Offset: 0x00001717
		public static MissionView CreateMissionConversationView(Mission mission)
		{
			return ViewCreatorManager.CreateMissionView<MissionConversationView>(true, mission, Array.Empty<object>());
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003525 File Offset: 0x00001725
		public static MissionView CreateMissionBarterView()
		{
			return ViewCreatorManager.CreateMissionView<BarterView>(false, null, Array.Empty<object>());
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003533 File Offset: 0x00001733
		public static MissionView CreateMissionAgentAlarmStateView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionAgentAlarmStateView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003544 File Offset: 0x00001744
		public static MissionView CreateMissionMainAgentDetectionView(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionMainAgentDetectionView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003555 File Offset: 0x00001755
		public static MissionView CreateMissionStealthFailCounter(Mission mission = null)
		{
			return ViewCreatorManager.CreateMissionView<MissionStealthFailCounterView>(mission != null, mission, Array.Empty<object>());
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003566 File Offset: 0x00001766
		public static MissionView CreateMissionTournamentView()
		{
			return ViewCreatorManager.CreateMissionView<MissionTournamentView>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003574 File Offset: 0x00001774
		public static MissionView CreateMissionQuestBarView()
		{
			return ViewCreatorManager.CreateMissionView<MissionQuestBarView>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003584 File Offset: 0x00001784
		public static MapView CreateMapView<T>(params object[] parameters) where T : MapView
		{
			Type type = typeof(T);
			MBList<Type> mblist;
			if (SandBoxViewCreator._actualViewTypes.TryGetValue(typeof(T), out mblist))
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
			}
			return Activator.CreateInstance(type, parameters) as MapView;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000035F8 File Offset: 0x000017F8
		public static MenuView CreateMenuView<T>(params object[] parameters) where T : MenuView
		{
			Type type = typeof(T);
			MBList<Type> mblist;
			if (SandBoxViewCreator._actualViewTypes.TryGetValue(typeof(T), out mblist))
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
			}
			return Activator.CreateInstance(type, parameters) as MenuView;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000366B File Offset: 0x0000186B
		public static MissionView CreateBoardGameView()
		{
			return ViewCreatorManager.CreateMissionView<BoardGameView>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003679 File Offset: 0x00001879
		public static MissionView CreateMissionArenaPracticeFightView()
		{
			return ViewCreatorManager.CreateMissionView<MissionArenaPracticeFightView>(false, null, Array.Empty<object>());
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003687 File Offset: 0x00001887
		public static GlobalLayer CreateCampaignAdvancedStartOptions(AdvancedStartOptions startOptions, Action<AdvancedStartOptions> onConfirm, Action onClose)
		{
			return ViewCreatorManager.CreateGlobalLayer<CampaignAdvancedStartingOptionsView>(new object[] { startOptions, onConfirm, onClose });
		}

		// Token: 0x04000009 RID: 9
		private static Dictionary<Type, MBList<Type>> _actualViewTypes;
	}
}
