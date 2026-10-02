using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks
{
	// Token: 0x020003CA RID: 970
	internal static class PerkAssemblyCollection
	{
		// Token: 0x060036C5 RID: 14021 RVA: 0x000E25F0 File Offset: 0x000E07F0
		public static List<Type> GetPerkAssemblyTypes()
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Assembly> list2 = new List<Assembly>();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					if (PerkAssemblyCollection.CheckAssemblyForPerks(assembly))
					{
						list2.Add(assembly);
					}
				}
				catch
				{
				}
			}
			foreach (Assembly assembly2 in list2)
			{
				try
				{
					List<Type> typesSafe = assembly2.GetTypesSafe(null);
					list.AddRange(typesSafe);
				}
				catch
				{
				}
			}
			return list;
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000E26AC File Offset: 0x000E08AC
		private static bool CheckAssemblyForPerks(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(MPPerkObject));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssembliesSafe = assembly.GetReferencedAssembliesSafe();
			for (int i = 0; i < referencedAssembliesSafe.Length; i++)
			{
				if (referencedAssembliesSafe[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}
	}
}
