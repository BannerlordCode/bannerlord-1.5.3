using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.CustomBattle
{
	// Token: 0x020000A9 RID: 169
	public static class CustomBattleFactory
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x0002AE50 File Offset: 0x00029050
		public static void RegisterProvider<T>() where T : ICustomBattleProvider, new()
		{
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < CustomBattleFactory._providers.Count; i++)
			{
				if (CustomBattleFactory._providers[i].GetType() == typeFromHandle)
				{
					Debug.FailedAssert("Custom battle provider was already registered: " + typeFromHandle.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\CustomBattle\\CustomBattleFactory.cs", "RegisterProvider", 18);
					return;
				}
			}
			if (typeFromHandle.Name.ToLowerInvariant().Contains("naval"))
			{
				CustomBattleFactory._providers.Insert(0, typeFromHandle);
				return;
			}
			CustomBattleFactory._providers.Add(typeFromHandle);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0002AEE6 File Offset: 0x000290E6
		public static void StartCustomBattleWithProvider<T>() where T : ICustomBattleProvider, new()
		{
			(Activator.CreateInstance(typeof(T)) as ICustomBattleProvider).StartCustomBattle();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002AF01 File Offset: 0x00029101
		public static void StartCustomBattle()
		{
			if (CustomBattleFactory._providers.Count > 0)
			{
				(Activator.CreateInstance(CustomBattleFactory._providers[0]) as ICustomBattleProvider).StartCustomBattle();
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002AF2A File Offset: 0x0002912A
		public static int GetProviderCount()
		{
			return CustomBattleFactory._providers.Count;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002AF38 File Offset: 0x00029138
		public static List<ICustomBattleProvider> CollectProviders()
		{
			List<ICustomBattleProvider> list = new List<ICustomBattleProvider>();
			for (int i = 0; i < CustomBattleFactory._providers.Count; i++)
			{
				ICustomBattleProvider customBattleProvider = Activator.CreateInstance(CustomBattleFactory._providers[i]) as ICustomBattleProvider;
				list.Add(customBattleProvider);
			}
			return list;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0002AF80 File Offset: 0x00029180
		public static ICustomBattleProvider CollectNextProvider(Type currentProviderType)
		{
			int num = (CustomBattleFactory._providers.IndexOf(currentProviderType) + 1) % CustomBattleFactory._providers.Count;
			return Activator.CreateInstance(CustomBattleFactory._providers[num]) as ICustomBattleProvider;
		}

		// Token: 0x0400034C RID: 844
		private static readonly List<Type> _providers = new List<Type>();
	}
}
