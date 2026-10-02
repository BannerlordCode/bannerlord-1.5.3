using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000327 RID: 807
	public class NetworkCommunication : INetworkCommunication
	{
		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x000B31A1 File Offset: 0x000B13A1
		VirtualPlayer INetworkCommunication.MyPeer
		{
			get
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				if (myPeer == null)
				{
					return null;
				}
				return myPeer.VirtualPlayer;
			}
		}
	}
}
