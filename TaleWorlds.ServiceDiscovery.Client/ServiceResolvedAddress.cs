using System;

namespace TaleWorlds.ServiceDiscovery.Client
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class ServiceResolvedAddress
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000024E3 File Offset: 0x000006E3
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000024EB File Offset: 0x000006EB
		public string Address { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000024F4 File Offset: 0x000006F4
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000024FC File Offset: 0x000006FC
		public string[] Tags { get; private set; }

		// Token: 0x0600001D RID: 29 RVA: 0x00002505 File Offset: 0x00000705
		public ServiceResolvedAddress(string address, string[] tags)
		{
			this.Address = address;
			this.Tags = tags;
		}
	}
}
