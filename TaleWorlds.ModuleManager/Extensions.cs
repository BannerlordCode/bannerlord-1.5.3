using System;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.ModuleManager
{
	// Token: 0x02000003 RID: 3
	public static class Extensions
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000020EC File Offset: 0x000002EC
		public static Assembly[] GetActiveReferencingGameAssembliesSafe(this Assembly assembly)
		{
			MBList<Assembly> activeGameAssemblies = ModuleHelper.GetActiveGameAssemblies();
			return assembly.GetReferencingAssembliesSafe((Assembly x) => activeGameAssemblies.Contains(x));
		}
	}
}
