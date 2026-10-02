using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003C RID: 60
	public class WidgetInfo
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00010001 File Offset: 0x0000E201
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00010009 File Offset: 0x0000E209
		public string Name { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00010012 File Offset: 0x0000E212
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x0001001A File Offset: 0x0000E21A
		public Type Type { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00010023 File Offset: 0x0000E223
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0001002B File Offset: 0x0000E22B
		public bool GotCustomUpdate { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00010034 File Offset: 0x0000E234
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0001003C File Offset: 0x0000E23C
		public bool GotCustomLateUpdate { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00010045 File Offset: 0x0000E245
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001004D File Offset: 0x0000E24D
		public bool GotCustomParallelUpdate { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x00010056 File Offset: 0x0000E256
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0001005E File Offset: 0x0000E25E
		public bool GotUpdateBrushes { get; private set; }

		// Token: 0x06000403 RID: 1027 RVA: 0x00010068 File Offset: 0x0000E268
		public WidgetInfo(Type type)
		{
			this.Name = type.Name;
			this.Type = type;
			this.GotCustomUpdate = this.IsMethodOverridden("OnUpdate");
			this.GotCustomLateUpdate = this.IsMethodOverridden("OnLateUpdate");
			this.GotCustomParallelUpdate = this.IsMethodOverridden("OnParallelUpdate");
			this.GotUpdateBrushes = this.IsMethodOverridden("UpdateBrushes");
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000100D2 File Offset: 0x0000E2D2
		public static void Refresh()
		{
			WidgetInfo._widgetInfos = new Dictionary<Type, WidgetInfo>();
			WidgetInfo.CollectWidgetTypes();
			TextureProviderFactory.RefreshProviderTypes();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000100E8 File Offset: 0x0000E2E8
		public static WidgetInfo GetWidgetInfo(Type type)
		{
			return WidgetInfo._widgetInfos[type];
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000100F5 File Offset: 0x0000E2F5
		public static WidgetInfo[] GetWidgetInfos()
		{
			return WidgetInfo._widgetInfos.Values.ToArray<WidgetInfo>();
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00010108 File Offset: 0x0000E308
		private static bool CheckAssemblyReferencesThis(Assembly assembly)
		{
			Assembly assembly2 = typeof(Widget).Assembly;
			AssemblyName[] referencedAssembliesSafe = assembly.GetReferencedAssembliesSafe();
			for (int i = 0; i < referencedAssembliesSafe.Length; i++)
			{
				if (referencedAssembliesSafe[i].Name == assembly2.GetName().Name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00010158 File Offset: 0x0000E358
		private static void CollectWidgetTypes()
		{
			new List<Type>();
			Assembly assembly = typeof(Widget).Assembly;
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (WidgetInfo.CheckAssemblyReferencesThis(assembly2) || assembly2 == assembly)
				{
					foreach (Type type in assembly2.GetTypesSafe(null))
					{
						if (typeof(Widget).IsAssignableFrom(type))
						{
							WidgetInfo._widgetInfos.Add(type, new WidgetInfo(type));
						}
					}
				}
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010214 File Offset: 0x0000E414
		private bool IsMethodOverridden(string methodName)
		{
			MethodInfo method = this.Type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag;
			if (method == null)
			{
				flag = false;
			}
			else
			{
				Type type = this.Type;
				Type type2 = this.Type;
				while (type2 != null)
				{
					if (type2.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
					{
						type = type2;
					}
					type2 = type2.BaseType;
				}
				flag = method.DeclaringType != type;
			}
			return flag;
		}

		// Token: 0x040001F4 RID: 500
		private static Dictionary<Type, WidgetInfo> _widgetInfos;
	}
}
