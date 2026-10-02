using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.ServiceDiscovery.Client
{
	// Token: 0x02000005 RID: 5
	public static class ServiceAddressManager
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000021F0 File Offset: 0x000003F0
		private static string EnvironmentFilePath
		{
			get
			{
				return Path.Combine(BasePath.Name, "Parameters", "Environment");
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002206 File Offset: 0x00000406
		public static void Initalize()
		{
			ServiceAddressManager.LoadCache();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002210 File Offset: 0x00000410
		public static bool ResolveAddress(string serviceDiscoveryAddress, ref string serviceAddress)
		{
			string text;
			if (ServiceAddress.TryGetAddressName(serviceAddress, out text))
			{
				string text2 = VirtualFolders.GetFileContent(ServiceAddressManager.EnvironmentFilePath, null);
				string environmentVariable = Environment.GetEnvironmentVariable("EnvironmentId");
				if (environmentVariable != null)
				{
					Debug.Print("EnvironmentId set from environment variable:" + environmentVariable, 3, Debug.DebugColor.White, 17592186044416UL);
					text2 = environmentVariable;
				}
				ServiceResolvedAddress serviceResolvedAddress;
				if (ServiceAddressManager.TryGetCachedServiceAddress(text, text2, out serviceResolvedAddress))
				{
					serviceAddress = serviceResolvedAddress.Address;
					return true;
				}
				ServiceResolvedAddress serviceResolvedAddress2;
				if (ServiceAddressManager.TryGetRemoteServiceAddressByTag(serviceDiscoveryAddress, text2, out serviceResolvedAddress2))
				{
					ServiceAddressManager.CacheServiceAddress(text, text2, serviceResolvedAddress2);
					serviceAddress = serviceResolvedAddress2.Address;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002298 File Offset: 0x00000498
		private static bool TryGetRemoteServiceAddress(string remoteServiceDiscoveryAddress, string serviceName, string environmentId, out ServiceResolvedAddress resolvedAddress)
		{
			IDiscoveryService discoveryService = new RemoteDiscoveryService(remoteServiceDiscoveryAddress);
			Func<Task<ServiceAddress[]>> <>9__0;
			for (int i = 0; i < 5; i++)
			{
				Func<Task<ServiceAddress[]>> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = () => discoveryService.ResolveService(serviceName, environmentId));
				}
				Task<ServiceAddress[]> task = Task.Run<ServiceAddress[]>(func);
				task.Wait(30000);
				if (task.IsCompleted && task.Result != null)
				{
					ServiceAddress[] result = task.Result;
					ServiceResolvedAddress serviceResolvedAddress;
					if (result == null)
					{
						serviceResolvedAddress = null;
					}
					else
					{
						ServiceAddress serviceAddress = result.FirstOrDefault<ServiceAddress>();
						if (serviceAddress == null)
						{
							serviceResolvedAddress = null;
						}
						else
						{
							ServiceResolvedAddress[] resolvedAddresses = serviceAddress.ResolvedAddresses;
							serviceResolvedAddress = ((resolvedAddresses != null) ? resolvedAddresses.FirstOrDefault<ServiceResolvedAddress>() : null);
						}
					}
					resolvedAddress = serviceResolvedAddress;
					return resolvedAddress != null;
				}
				Debug.Print(string.Format("Couldn't resolve service address, retry count: {0}", i + 1), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			resolvedAddress = null;
			return false;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002378 File Offset: 0x00000578
		private static bool TryGetRemoteServiceAddressByTag(string remoteServiceDiscoveryAddress, string environmentId, out ServiceResolvedAddress resolvedAddress)
		{
			IDiscoveryService discoveryService = new RemoteDiscoveryService(remoteServiceDiscoveryAddress);
			Func<Task<ServiceResolvedAddress>> <>9__0;
			for (int i = 0; i < 5; i++)
			{
				Func<Task<ServiceResolvedAddress>> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = () => discoveryService.ResolveServiceByTag(environmentId));
				}
				Task<ServiceResolvedAddress> task = Task.Run<ServiceResolvedAddress>(func);
				task.Wait(30000);
				if (task.IsCompleted && task.Result != null)
				{
					resolvedAddress = task.Result;
					return resolvedAddress != null;
				}
				Debug.Print(string.Format("Couldn't resolve service address, retry count: {0}", i + 1), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			resolvedAddress = null;
			return false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002420 File Offset: 0x00000620
		private static bool TryGetCachedServiceAddress(string serviceName, string environmentId, out ServiceResolvedAddress resolvedAddress)
		{
			ServiceAddressManager.CachedServiceAddress cachedServiceAddress = ServiceAddressManager._serviceAddressCache.FirstOrDefault<ServiceAddressManager.CachedServiceAddress>((ServiceAddressManager.CachedServiceAddress address) => address.ServiceName == serviceName && address.EnvironmentId == environmentId);
			if (cachedServiceAddress != null)
			{
				if (DateTime.UtcNow - cachedServiceAddress.SavedAt < TimeSpan.FromDays(7.0))
				{
					resolvedAddress = cachedServiceAddress.ResolvedAddress;
					return true;
				}
				ServiceAddressManager._serviceAddressCache.Remove(cachedServiceAddress);
			}
			resolvedAddress = null;
			return false;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000249A File Offset: 0x0000069A
		private static void CacheServiceAddress(string serviceAddress, string environmentId, ServiceResolvedAddress resolvedAddress)
		{
			if (resolvedAddress != null)
			{
				ServiceAddressManager._serviceAddressCache.Add(new ServiceAddressManager.CachedServiceAddress
				{
					ServiceName = serviceAddress,
					EnvironmentId = environmentId,
					ResolvedAddress = resolvedAddress,
					SavedAt = DateTime.UtcNow
				});
				ServiceAddressManager.SaveCache();
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024D3 File Offset: 0x000006D3
		private static void LoadCache()
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024D5 File Offset: 0x000006D5
		private static void SaveCache()
		{
		}

		// Token: 0x04000006 RID: 6
		private const string ParametersDirectoryName = "Parameters";

		// Token: 0x04000007 RID: 7
		private const string EnvironmentFileName = "Environment";

		// Token: 0x04000008 RID: 8
		private const string CacheDirectoryName = "Data";

		// Token: 0x04000009 RID: 9
		private const string CachedServiceAddressesFileName = "ServiceAddresses.dat";

		// Token: 0x0400000A RID: 10
		private const int ResolveAddressTaskTimeoutDurationInSeconds = 30;

		// Token: 0x0400000B RID: 11
		private const int ServiceAddressExpirationTimeInDays = 7;

		// Token: 0x0400000C RID: 12
		private const int MaxRetryCount = 5;

		// Token: 0x0400000D RID: 13
		private static List<ServiceAddressManager.CachedServiceAddress> _serviceAddressCache = new List<ServiceAddressManager.CachedServiceAddress>();

		// Token: 0x0200000A RID: 10
		[Serializable]
		private class CachedServiceAddress
		{
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000024 RID: 36 RVA: 0x0000299A File Offset: 0x00000B9A
			// (set) Token: 0x06000025 RID: 37 RVA: 0x000029A2 File Offset: 0x00000BA2
			public string ServiceName { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000026 RID: 38 RVA: 0x000029AB File Offset: 0x00000BAB
			// (set) Token: 0x06000027 RID: 39 RVA: 0x000029B3 File Offset: 0x00000BB3
			public string EnvironmentId { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000028 RID: 40 RVA: 0x000029BC File Offset: 0x00000BBC
			// (set) Token: 0x06000029 RID: 41 RVA: 0x000029C4 File Offset: 0x00000BC4
			public ServiceResolvedAddress ResolvedAddress { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600002A RID: 42 RVA: 0x000029CD File Offset: 0x00000BCD
			// (set) Token: 0x0600002B RID: 43 RVA: 0x000029D5 File Offset: 0x00000BD5
			public DateTime SavedAt { get; set; }
		}
	}
}
