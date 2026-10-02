using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000321 RID: 801
	internal class MBNetworkPeer : DotNetObject
	{
		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002DDE RID: 11742 RVA: 0x000B27C4 File Offset: 0x000B09C4
		public NetworkCommunicator NetworkPeer { get; }

		// Token: 0x06002DDF RID: 11743 RVA: 0x000B27CC File Offset: 0x000B09CC
		internal MBNetworkPeer(NetworkCommunicator networkPeer)
		{
			this.NetworkPeer = networkPeer;
		}
	}
}
