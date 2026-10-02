using System;

namespace TaleWorlds.ServiceDiscovery.Client
{
	// Token: 0x02000004 RID: 4
	public class ServiceAddress
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002145 File Offset: 0x00000345
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000214D File Offset: 0x0000034D
		public string Service { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002156 File Offset: 0x00000356
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000215E File Offset: 0x0000035E
		public ServiceResolvedAddress[] ResolvedAddresses { get; private set; }

		// Token: 0x0600000C RID: 12 RVA: 0x00002167 File Offset: 0x00000367
		public ServiceAddress(string service, ServiceResolvedAddress[] resolvedAddresses)
		{
			this.ResolvedAddresses = resolvedAddresses;
			this.Service = service;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002180 File Offset: 0x00000380
		public static bool IsServiceAddress(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				string text = address.ToLower();
				if (text.StartsWith("service://") && text.EndsWith('/'.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021BE File Offset: 0x000003BE
		public static bool TryGetAddressName(string serviceAddress, out string addressName)
		{
			if (ServiceAddress.IsServiceAddress(serviceAddress))
			{
				addressName = serviceAddress.Substring("service://".Length).Trim(new char[] { '/' });
				return true;
			}
			addressName = null;
			return false;
		}

		// Token: 0x04000002 RID: 2
		private const string Prefix = "service://";

		// Token: 0x04000003 RID: 3
		private const char Suffix = '/';
	}
}
