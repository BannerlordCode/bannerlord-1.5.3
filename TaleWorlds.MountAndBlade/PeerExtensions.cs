using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000312 RID: 786
	public static class PeerExtensions
	{
		// Token: 0x06002D3C RID: 11580 RVA: 0x000AEAA5 File Offset: 0x000ACCA5
		public static void SendExistingObjects(this NetworkCommunicator peer, Mission mission)
		{
			MBAPI.IMBPeer.SendExistingObjects(peer.Index, mission.Pointer);
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000AEABD File Offset: 0x000ACCBD
		public static VirtualPlayer GetPeer(this PeerComponent peerComponent)
		{
			return peerComponent.Peer;
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000AEAC5 File Offset: 0x000ACCC5
		public static NetworkCommunicator GetNetworkPeer(this PeerComponent peerComponent)
		{
			return peerComponent.Peer.Communicator as NetworkCommunicator;
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000AEAD7 File Offset: 0x000ACCD7
		public static T GetComponent<T>(this NetworkCommunicator networkPeer) where T : PeerComponent
		{
			return networkPeer.VirtualPlayer.GetComponent<T>();
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000AEAE4 File Offset: 0x000ACCE4
		public static void RemoveComponent<T>(this NetworkCommunicator networkPeer, bool synched = true) where T : PeerComponent
		{
			networkPeer.VirtualPlayer.RemoveComponent<T>(true);
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000AEAF2 File Offset: 0x000ACCF2
		public static void RemoveComponent(this NetworkCommunicator networkPeer, PeerComponent component)
		{
			networkPeer.VirtualPlayer.RemoveComponent(component);
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000AEB00 File Offset: 0x000ACD00
		public static PeerComponent GetComponent(this NetworkCommunicator networkPeer, uint componentId)
		{
			return networkPeer.VirtualPlayer.GetComponent(componentId);
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000AEB0E File Offset: 0x000ACD0E
		public static void AddComponent(this NetworkCommunicator networkPeer, Type peerComponentType)
		{
			networkPeer.VirtualPlayer.AddComponent(peerComponentType);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000AEB1D File Offset: 0x000ACD1D
		public static void AddComponent(this NetworkCommunicator networkPeer, uint componentId)
		{
			networkPeer.VirtualPlayer.AddComponent(componentId);
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000AEB2C File Offset: 0x000ACD2C
		public static T AddComponent<T>(this NetworkCommunicator networkPeer) where T : PeerComponent, new()
		{
			if (networkPeer.GetComponent<T>() != null)
			{
				return networkPeer.TellClientToAddComponent<T>();
			}
			return networkPeer.VirtualPlayer.AddComponent<T>();
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000AEB50 File Offset: 0x000ACD50
		public static T TellClientToAddComponent<T>(this NetworkCommunicator networkPeer) where T : PeerComponent, new()
		{
			T component = networkPeer.GetComponent<T>();
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new AddPeerComponent(networkPeer, component.TypeId));
			GameNetwork.EndModuleEventAsServer();
			return component;
		}
	}
}
