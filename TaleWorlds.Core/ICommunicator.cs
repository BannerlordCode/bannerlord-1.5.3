using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000083 RID: 131
	public interface ICommunicator
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000883 RID: 2179
		VirtualPlayer VirtualPlayer { get; }

		// Token: 0x06000884 RID: 2180
		void OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component);

		// Token: 0x06000885 RID: 2181
		void OnAddComponent(PeerComponent component);

		// Token: 0x06000886 RID: 2182
		void OnRemoveComponent(PeerComponent component);

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000887 RID: 2183
		bool IsNetworkActive { get; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000888 RID: 2184
		bool IsConnectionActive { get; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000889 RID: 2185
		bool IsServerPeer { get; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600088A RID: 2186
		// (set) Token: 0x0600088B RID: 2187
		bool IsSynchronized { get; set; }
	}
}
