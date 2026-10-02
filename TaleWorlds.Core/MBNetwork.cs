using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000B1 RID: 177
	public static class MBNetwork
	{
		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0001E786 File Offset: 0x0001C986
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x0001E78D File Offset: 0x0001C98D
		public static INetworkCommunication NetworkViewCommunication { get; private set; }

		// Token: 0x06000957 RID: 2391 RVA: 0x0001E795 File Offset: 0x0001C995
		public static void Initialize(INetworkCommunication networkCommunication)
		{
			MBNetwork.NetworkViewCommunication = networkCommunication;
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0001E79D File Offset: 0x0001C99D
		public static VirtualPlayer MyPeer
		{
			get
			{
				if (MBNetwork.NetworkViewCommunication != null)
				{
					return MBNetwork.NetworkViewCommunication.MyPeer;
				}
				return null;
			}
		}
	}
}
