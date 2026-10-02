using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x0200002F RID: 47
	public static class MissionNameMarkerFactory
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003CF RID: 975 RVA: 0x0001074C File Offset: 0x0000E94C
		// (remove) Token: 0x060003D0 RID: 976 RVA: 0x00010780 File Offset: 0x0000E980
		public static event Action OnProvidersChanged;

		// Token: 0x060003D2 RID: 978 RVA: 0x000107E8 File Offset: 0x0000E9E8
		public static MissionNameMarkerFactory.INameMarkerProviderContext PushContext(string name, bool addDefaultProviders)
		{
			MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext = new MissionNameMarkerFactory.NameMarkerProviderContext(false, name, new Action(MissionNameMarkerFactory.FireProvidersChangedEvent));
			if (addDefaultProviders)
			{
				MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext2 = MissionNameMarkerFactory.DefaultContext as MissionNameMarkerFactory.NameMarkerProviderContext;
				for (int i = 0; i < nameMarkerProviderContext2.ProviderTypes.Count; i++)
				{
					nameMarkerProviderContext.AddProvider(nameMarkerProviderContext2.ProviderTypes[i]);
				}
			}
			MissionNameMarkerFactory._registeredContexts.Add(nameMarkerProviderContext);
			return nameMarkerProviderContext;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001084C File Offset: 0x0000EA4C
		public static void PopContext(string contextId)
		{
			for (int i = 0; i < MissionNameMarkerFactory._registeredContexts.Count; i++)
			{
				if (MissionNameMarkerFactory._registeredContexts[i].Id == contextId)
				{
					MissionNameMarkerFactory.PopContext(MissionNameMarkerFactory._registeredContexts[i]);
					return;
				}
			}
			Debug.FailedAssert("Trying to pop a name marker context that was not pushed: " + contextId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "PopContext", 54);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000108B3 File Offset: 0x0000EAB3
		public static void PopContext(MissionNameMarkerFactory.INameMarkerProviderContext context)
		{
			if (context.IsDefaultContext)
			{
				Debug.FailedAssert("Default name marker context cannot be removed", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "PopContext", 61);
				return;
			}
			MissionNameMarkerFactory._registeredContexts.Remove(context);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000108E0 File Offset: 0x0000EAE0
		private static void FireProvidersChangedEvent()
		{
			Action onProvidersChanged = MissionNameMarkerFactory.OnProvidersChanged;
			if (onProvidersChanged == null)
			{
				return;
			}
			onProvidersChanged();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000108F4 File Offset: 0x0000EAF4
		public static List<MissionNameMarkerProvider> CollectProviders()
		{
			List<MissionNameMarkerProvider> list = new List<MissionNameMarkerProvider>();
			MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext = MissionNameMarkerFactory._registeredContexts[MissionNameMarkerFactory._registeredContexts.Count - 1] as MissionNameMarkerFactory.NameMarkerProviderContext;
			for (int i = 0; i < nameMarkerProviderContext.ProviderTypes.Count; i++)
			{
				MissionNameMarkerProvider missionNameMarkerProvider = Activator.CreateInstance(nameMarkerProviderContext.ProviderTypes[i]) as MissionNameMarkerProvider;
				list.Add(missionNameMarkerProvider);
			}
			return list;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00010958 File Offset: 0x0000EB58
		public static void UpdateProviders(MissionNameMarkerProvider[] existingProviders, out List<MissionNameMarkerProvider> addedProviders, out List<MissionNameMarkerProvider> removedProviders)
		{
			addedProviders = new List<MissionNameMarkerProvider>();
			removedProviders = new List<MissionNameMarkerProvider>();
			MissionNameMarkerFactory.NameMarkerProviderContext nameMarkerProviderContext = MissionNameMarkerFactory._registeredContexts[MissionNameMarkerFactory._registeredContexts.Count - 1] as MissionNameMarkerFactory.NameMarkerProviderContext;
			for (int i = 0; i < existingProviders.Length; i++)
			{
				bool flag = true;
				MissionNameMarkerProvider missionNameMarkerProvider = existingProviders[i];
				for (int j = 0; j < nameMarkerProviderContext.ProviderTypes.Count; j++)
				{
					Type type = nameMarkerProviderContext.ProviderTypes[j];
					if (missionNameMarkerProvider.GetType() == type)
					{
						flag = false;
					}
				}
				if (flag)
				{
					removedProviders.Add(missionNameMarkerProvider);
				}
			}
			for (int k = 0; k < nameMarkerProviderContext.ProviderTypes.Count; k++)
			{
				bool flag2 = true;
				Type type2 = nameMarkerProviderContext.ProviderTypes[k];
				for (int l = 0; l < existingProviders.Length; l++)
				{
					if (existingProviders[l].GetType() == type2)
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					MissionNameMarkerProvider missionNameMarkerProvider2 = Activator.CreateInstance(type2) as MissionNameMarkerProvider;
					addedProviders.Add(missionNameMarkerProvider2);
				}
			}
		}

		// Token: 0x040001F4 RID: 500
		public static readonly MissionNameMarkerFactory.INameMarkerProviderContext DefaultContext = new MissionNameMarkerFactory.NameMarkerProviderContext(true, "DefaultNameMarkerContext", new Action(MissionNameMarkerFactory.FireProvidersChangedEvent));

		// Token: 0x040001F6 RID: 502
		private static List<MissionNameMarkerFactory.INameMarkerProviderContext> _registeredContexts = new List<MissionNameMarkerFactory.INameMarkerProviderContext> { MissionNameMarkerFactory.DefaultContext };

		// Token: 0x020000A1 RID: 161
		public interface INameMarkerProviderContext
		{
			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x0600071B RID: 1819
			string Id { get; }

			// Token: 0x170001FA RID: 506
			// (get) Token: 0x0600071C RID: 1820
			bool IsDefaultContext { get; }

			// Token: 0x0600071D RID: 1821
			void AddProvider<T>() where T : MissionNameMarkerProvider, new();

			// Token: 0x0600071E RID: 1822
			void RemoveProvider<T>() where T : MissionNameMarkerProvider, new();
		}

		// Token: 0x020000A2 RID: 162
		private class NameMarkerProviderContext : MissionNameMarkerFactory.INameMarkerProviderContext
		{
			// Token: 0x170001FB RID: 507
			// (get) Token: 0x0600071F RID: 1823 RVA: 0x000186D2 File Offset: 0x000168D2
			// (set) Token: 0x06000720 RID: 1824 RVA: 0x000186DA File Offset: 0x000168DA
			public string Id { get; private set; }

			// Token: 0x170001FC RID: 508
			// (get) Token: 0x06000721 RID: 1825 RVA: 0x000186E3 File Offset: 0x000168E3
			// (set) Token: 0x06000722 RID: 1826 RVA: 0x000186EB File Offset: 0x000168EB
			public bool IsDefaultContext { get; private set; }

			// Token: 0x170001FD RID: 509
			// (get) Token: 0x06000723 RID: 1827 RVA: 0x000186F4 File Offset: 0x000168F4
			// (set) Token: 0x06000724 RID: 1828 RVA: 0x000186FC File Offset: 0x000168FC
			public List<Type> ProviderTypes { get; private set; }

			// Token: 0x06000725 RID: 1829 RVA: 0x00018705 File Offset: 0x00016905
			public NameMarkerProviderContext(bool isDefault, string id, Action onProvidersChanged)
			{
				this._onProvidersChanged = onProvidersChanged;
				this.IsDefaultContext = isDefault;
				this.Id = id;
				this.ProviderTypes = new List<Type>();
			}

			// Token: 0x06000726 RID: 1830 RVA: 0x0001872D File Offset: 0x0001692D
			public void AddProvider<T>() where T : MissionNameMarkerProvider, new()
			{
				this.AddProvider(typeof(T));
			}

			// Token: 0x06000727 RID: 1831 RVA: 0x0001873F File Offset: 0x0001693F
			public void RemoveProvider<T>() where T : MissionNameMarkerProvider, new()
			{
				this.RemoveProvider(typeof(T));
			}

			// Token: 0x06000728 RID: 1832 RVA: 0x00018754 File Offset: 0x00016954
			public void AddProvider(Type tProvider)
			{
				for (int i = 0; i < this.ProviderTypes.Count; i++)
				{
					if (this.ProviderTypes[i] == tProvider)
					{
						Debug.FailedAssert("Provider of type: " + tProvider.Name + " was already added to name marker context: " + this.Id, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "AddProvider", 182);
						return;
					}
				}
				this.ProviderTypes.Add(tProvider);
				Action onProvidersChanged = this._onProvidersChanged;
				if (onProvidersChanged == null)
				{
					return;
				}
				onProvidersChanged();
			}

			// Token: 0x06000729 RID: 1833 RVA: 0x000187D8 File Offset: 0x000169D8
			public void RemoveProvider(Type tProvider)
			{
				int i = 0;
				while (i < this.ProviderTypes.Count)
				{
					if (this.ProviderTypes[i] == tProvider)
					{
						this.ProviderTypes.Remove(tProvider);
						Action onProvidersChanged = this._onProvidersChanged;
						if (onProvidersChanged == null)
						{
							return;
						}
						onProvidersChanged();
						return;
					}
					else
					{
						i++;
					}
				}
				Debug.FailedAssert("Provider of type: " + tProvider.Name + " was not added to name marker context: " + this.Id, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Missions\\NameMarker\\MissionNameMarkerFactory.cs", "RemoveProvider", 203);
			}

			// Token: 0x040003F7 RID: 1015
			private Action _onProvidersChanged;
		}
	}
}
