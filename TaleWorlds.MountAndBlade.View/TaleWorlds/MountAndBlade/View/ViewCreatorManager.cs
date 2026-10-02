using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000025 RID: 37
	public static class ViewCreatorManager
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00007A39 File Offset: 0x00005C39
		static ViewCreatorManager()
		{
			ViewCreatorManager.CollectTypes();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007A60 File Offset: 0x00005C60
		internal static void CollectTypes()
		{
			ViewCreatorManager._viewCreators.Clear();
			ViewCreatorManager._actualViewTypes.Clear();
			ViewCreatorManager._defaultTypes.Clear();
			Assembly[] referencingAssembliesSafe = typeof(ViewCreatorModule).Assembly.GetReferencingAssembliesSafe(null);
			Assembly assembly = typeof(ViewCreatorModule).Assembly;
			ViewCreatorManager.CheckAssemblyScreens(assembly);
			Assembly[] array = referencingAssembliesSafe;
			for (int i = 0; i < array.Length; i++)
			{
				ViewCreatorManager.CheckAssemblyScreens(array[i]);
			}
			ViewCreatorManager.CollectDefaults(assembly);
			array = referencingAssembliesSafe;
			for (int i = 0; i < array.Length; i++)
			{
				ViewCreatorManager.CollectDefaults(array[i]);
			}
			array = referencingAssembliesSafe;
			for (int i = 0; i < array.Length; i++)
			{
				ViewCreatorManager.CheckOverridenViews(array[i]);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007B08 File Offset: 0x00005D08
		private static void CheckAssemblyScreens(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(ViewCreatorModule), false);
				if (customAttributesSafe != null && customAttributesSafe.Length == 1 && customAttributesSafe[0] is ViewCreatorModule)
				{
					foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						ViewMethod viewMethod = methodInfo.GetCustomAttributesSafe(typeof(ViewMethod), false)[0] as ViewMethod;
						if (viewMethod != null)
						{
							MBList<MethodInfo> mblist;
							if (ViewCreatorManager._viewCreators.TryGetValue(viewMethod.Name, out mblist))
							{
								mblist.Add(methodInfo);
							}
							else
							{
								ViewCreatorManager._viewCreators.Add(viewMethod.Name, new MBList<MethodInfo> { methodInfo });
							}
						}
					}
				}
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007C04 File Offset: 0x00005E04
		internal static IEnumerable<MissionBehavior> CreateDefaultMissionBehaviors(Mission mission)
		{
			List<MissionBehavior> list = new List<MissionBehavior>();
			foreach (Type type in ViewCreatorManager._defaultTypes)
			{
				Type type2 = null;
				MBList<Type> mblist;
				if (ViewCreatorManager._actualViewTypes.TryGetValue(type, out mblist))
				{
					MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
					for (int i = mblist.Count - 1; i >= 0; i--)
					{
						if (activeGameAssemblies.Contains(mblist[i].Assembly))
						{
							type2 = mblist[i];
							break;
						}
					}
				}
				if (type2 == null && !type.IsAbstract)
				{
					type2 = type;
				}
				if (type2 != null)
				{
					MissionBehavior missionBehavior = Activator.CreateInstance(type2) as MissionBehavior;
					list.Add(missionBehavior);
				}
				else
				{
					Debug.FailedAssert(string.Format("Failed to initialize default mission view type: {0}", type), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\ViewCreatorManager.cs", "CreateDefaultMissionBehaviors", 129);
				}
			}
			return list;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007D04 File Offset: 0x00005F04
		internal static IEnumerable<MissionBehavior> CollectMissionBehaviors(string missionName, Mission mission, IEnumerable<MissionBehavior> behaviors)
		{
			List<MissionBehavior> list = new List<MissionBehavior>();
			MBList<MethodInfo> mblist;
			if (ViewCreatorManager._viewCreators.TryGetValue(missionName, out mblist))
			{
				MethodInfo methodInfo = null;
				MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
				for (int i = mblist.Count - 1; i >= 0; i--)
				{
					if (activeGameAssemblies.Contains(mblist[i].DeclaringType.Assembly))
					{
						methodInfo = mblist[i];
						break;
					}
				}
				if (methodInfo != null)
				{
					MissionBehavior[] array = methodInfo.Invoke(null, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new object[] { mission }, null) as MissionBehavior[];
					list.AddRange(array);
				}
				else
				{
					Debug.FailedAssert("Failed to invoke view creator method for: " + missionName, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\ViewCreatorManager.cs", "CollectMissionBehaviors", 170);
				}
			}
			return behaviors.Concat<MissionBehavior>(list);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007DC4 File Offset: 0x00005FC4
		public static ScreenBase CreateScreenView<T>() where T : ScreenBase, new()
		{
			MBList<Type> mblist;
			if (ViewCreatorManager._actualViewTypes.TryGetValue(typeof(T), out mblist))
			{
				MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
				Type type = null;
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
					return Activator.CreateInstance(type) as ScreenBase;
				}
			}
			return new T();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007E44 File Offset: 0x00006044
		public static GlobalLayer CreateGlobalLayer<T>(params object[] parameters) where T : GlobalLayer, new()
		{
			MBList<Type> mblist;
			if (ViewCreatorManager._actualViewTypes.TryGetValue(typeof(T), out mblist))
			{
				MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
				Type type = null;
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
					return Activator.CreateInstance(type, parameters) as GlobalLayer;
				}
			}
			return new T();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007EC4 File Offset: 0x000060C4
		public static ScreenBase CreateScreenView<T>(params object[] parameters) where T : ScreenBase
		{
			Type type = typeof(T);
			MBList<Type> mblist;
			if (ViewCreatorManager._actualViewTypes.TryGetValue(typeof(T), out mblist))
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
			return Activator.CreateInstance(type, parameters) as ScreenBase;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00007F38 File Offset: 0x00006138
		public static MissionView CreateMissionView<T>(bool isNetwork = false, Mission mission = null, params object[] parameters) where T : MissionView, new()
		{
			Type type = null;
			MBList<Type> mblist;
			if (ViewCreatorManager._actualViewTypes.TryGetValue(typeof(T), out mblist))
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
				return Activator.CreateInstance(type, parameters) as MissionView;
			}
			return new T();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007FB0 File Offset: 0x000061B0
		public static MissionView CreateMissionViewWithArgs<T>(params object[] parameters) where T : MissionView
		{
			Type type = typeof(T);
			MBList<Type> mblist;
			if (ViewCreatorManager._actualViewTypes.TryGetValue(typeof(T), out mblist))
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
			return Activator.CreateInstance(type, parameters) as MissionView;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00008024 File Offset: 0x00006224
		private static void CheckOverridenViews(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				if (typeof(MissionView).IsAssignableFrom(type) || typeof(ScreenBase).IsAssignableFrom(type) || typeof(GlobalLayer).IsAssignableFrom(type))
				{
					object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(OverrideView), false);
					if (customAttributesSafe != null && customAttributesSafe.Length == 1)
					{
						OverrideView overrideView = customAttributesSafe[0] as OverrideView;
						if (overrideView != null)
						{
							MBList<Type> mblist;
							if (ViewCreatorManager._actualViewTypes.TryGetValue(overrideView.BaseType, out mblist))
							{
								mblist.Add(type);
							}
							else
							{
								ViewCreatorManager._actualViewTypes[overrideView.BaseType] = new MBList<Type> { type };
							}
						}
					}
				}
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008110 File Offset: 0x00006310
		private static void CollectDefaults(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				if (typeof(MissionBehavior).IsAssignableFrom(type) && type.GetCustomAttributesSafe(typeof(DefaultView), false).Length == 1)
				{
					ViewCreatorManager._defaultTypes.Add(type);
				}
			}
		}

		// Token: 0x04000049 RID: 73
		private static Dictionary<string, MBList<MethodInfo>> _viewCreators = new Dictionary<string, MBList<MethodInfo>>();

		// Token: 0x0400004A RID: 74
		private static Dictionary<Type, MBList<Type>> _actualViewTypes = new Dictionary<Type, MBList<Type>>();

		// Token: 0x0400004B RID: 75
		private static HashSet<Type> _defaultTypes = new HashSet<Type>();
	}
}
