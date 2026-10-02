using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000060 RID: 96
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class HasTableauCache : Attribute
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x00008C78 File Offset: 0x00006E78
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x00008C80 File Offset: 0x00006E80
		public Type TableauCacheType { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00008C89 File Offset: 0x00006E89
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x00008C91 File Offset: 0x00006E91
		public Type MaterialCacheIDGetType { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00008C9A File Offset: 0x00006E9A
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x00008CA1 File Offset: 0x00006EA1
		internal static Dictionary<Type, MaterialCacheIDGetMethodDelegate> TableauCacheTypes { get; private set; }

		// Token: 0x06000978 RID: 2424 RVA: 0x00008CA9 File Offset: 0x00006EA9
		public HasTableauCache(Type tableauCacheType, Type materialCacheIDGetType)
		{
			this.TableauCacheType = tableauCacheType;
			this.MaterialCacheIDGetType = materialCacheIDGetType;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00008CC0 File Offset: 0x00006EC0
		public static void CollectTableauCacheTypes()
		{
			HasTableauCache.TableauCacheTypes = new Dictionary<Type, MaterialCacheIDGetMethodDelegate>();
			HasTableauCache.CollectTableauCacheTypesFrom(typeof(HasTableauCache).Assembly);
			Assembly[] referencingAssembliesSafe = typeof(HasTableauCache).Assembly.GetReferencingAssembliesSafe(null);
			for (int i = 0; i < referencingAssembliesSafe.Length; i++)
			{
				HasTableauCache.CollectTableauCacheTypesFrom(referencingAssembliesSafe[i]);
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00008D18 File Offset: 0x00006F18
		private static void CollectTableauCacheTypesFrom(Assembly assembly)
		{
			object[] customAttributesSafe = assembly.GetCustomAttributesSafe(typeof(HasTableauCache), true);
			if (customAttributesSafe.Length != 0)
			{
				foreach (HasTableauCache hasTableauCache in customAttributesSafe)
				{
					MethodInfo method = hasTableauCache.MaterialCacheIDGetType.GetMethod("GetMaterialCacheID", BindingFlags.Static | BindingFlags.Public);
					MaterialCacheIDGetMethodDelegate materialCacheIDGetMethodDelegate = (MaterialCacheIDGetMethodDelegate)Delegate.CreateDelegate(typeof(MaterialCacheIDGetMethodDelegate), method);
					HasTableauCache.TableauCacheTypes.Add(hasTableauCache.TableauCacheType, materialCacheIDGetMethodDelegate);
				}
			}
		}
	}
}
