using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000027 RID: 39
	internal sealed class NetworkStatusReplicationComponent : UdpNetworkComponent
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00008470 File Offset: 0x00006670
		public override void OnUdpNetworkHandlerTick(float dt)
		{
			if (GameNetwork.IsServer)
			{
				float totalMissionTime = MBCommon.GetTotalMissionTime();
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (networkCommunicator.IsSynchronized)
					{
						while (this._peerData.Count <= networkCommunicator.Index)
						{
							NetworkStatusReplicationComponent.NetworkStatusData networkStatusData = new NetworkStatusReplicationComponent.NetworkStatusData();
							this._peerData.Add(networkStatusData);
						}
						double num = networkCommunicator.RefreshAndGetAveragePingInMilliseconds();
						NetworkStatusReplicationComponent.NetworkStatusData networkStatusData2 = this._peerData[networkCommunicator.Index];
						bool flag = networkStatusData2.NextPingForceSendTime <= totalMissionTime;
						if (flag || networkStatusData2.NextPingTrySendTime <= totalMissionTime)
						{
							int num2 = MathF.Round(num);
							if (flag || networkStatusData2.LastSentPingValue != num2)
							{
								networkStatusData2.LastSentPingValue = num2;
								networkStatusData2.NextPingForceSendTime = totalMissionTime + 10f + MBRandom.RandomFloatRanged(1.5f, 2.5f);
								GameNetwork.BeginBroadcastModuleEvent();
								GameNetwork.WriteMessage(new PingReplication(networkCommunicator, num2));
								GameNetwork.EndBroadcastModuleEventUnreliable(GameNetwork.EventBroadcastFlags.None, null);
							}
							networkStatusData2.NextPingTrySendTime = totalMissionTime + MBRandom.RandomFloatRanged(1.5f, 2.5f);
						}
						if (!networkCommunicator.IsServerPeer && networkStatusData2.NextLossTrySendTime <= totalMissionTime)
						{
							networkStatusData2.NextLossTrySendTime = totalMissionTime + MBRandom.RandomFloatRanged(1.5f, 2.5f);
							int num3 = (int)networkCommunicator.RefreshAndGetAverageLossPercent();
							if (networkStatusData2.LastSentLossValue != num3)
							{
								networkStatusData2.LastSentLossValue = num3;
								GameNetwork.BeginModuleEventAsServer(networkCommunicator);
								GameNetwork.WriteMessage(new LossReplicationMessage(num3));
								GameNetwork.EndModuleEventAsServer();
							}
						}
					}
				}
				if (this._nextPerformanceStateTrySendTime <= totalMissionTime)
				{
					this._nextPerformanceStateTrySendTime = totalMissionTime + MBRandom.RandomFloatRanged(1.5f, 2.5f);
					ServerPerformanceState serverPerformanceState = this.GetServerPerformanceState();
					if (serverPerformanceState != this._lastSentPerformanceState)
					{
						this._lastSentPerformanceState = serverPerformanceState;
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new ServerPerformanceStateReplicationMessage(serverPerformanceState));
						GameNetwork.EndBroadcastModuleEventUnreliable(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008668 File Offset: 0x00006868
		public NetworkStatusReplicationComponent()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				NetworkStatusReplicationComponent.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008688 File Offset: 0x00006888
		public override void OnUdpNetworkHandlerClose()
		{
			base.OnUdpNetworkHandlerClose();
			if (GameNetwork.IsClientOrReplay)
			{
				NetworkStatusReplicationComponent.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000869D File Offset: 0x0000689D
		private static void HandleServerMessagePingReplication(PingReplication message)
		{
			NetworkCommunicator peer = message.Peer;
			if (peer == null)
			{
				return;
			}
			peer.SetAveragePingInMillisecondsAsClient((double)message.PingValue);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000086B6 File Offset: 0x000068B6
		private static void HandleServerMessageLossReplication(LossReplicationMessage message)
		{
			if (GameNetwork.IsMyPeerReady)
			{
				GameNetwork.MyPeer.SetAverageLossPercentAsClient((double)message.LossValue);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000086D0 File Offset: 0x000068D0
		private static void HandleServerMessageServerPerformanceStateReplication(ServerPerformanceStateReplicationMessage message)
		{
			if (GameNetwork.IsMyPeerReady)
			{
				GameNetwork.MyPeer.SetServerPerformanceProblemStateAsClient(message.ServerPerformanceProblemState);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000086E9 File Offset: 0x000068E9
		private static void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			networkMessageHandlerRegisterer.Register<PingReplication>(new GameNetworkMessage.ServerMessageHandlerDelegate<PingReplication>(NetworkStatusReplicationComponent.HandleServerMessagePingReplication));
			networkMessageHandlerRegisterer.Register<LossReplicationMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<LossReplicationMessage>(NetworkStatusReplicationComponent.HandleServerMessageLossReplication));
			networkMessageHandlerRegisterer.Register<ServerPerformanceStateReplicationMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<ServerPerformanceStateReplicationMessage>(NetworkStatusReplicationComponent.HandleServerMessageServerPerformanceStateReplication));
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008728 File Offset: 0x00006928
		private ServerPerformanceState GetServerPerformanceState()
		{
			if (Mission.Current == null)
			{
				return ServerPerformanceState.High;
			}
			float averageFps = Mission.Current.GetAverageFps();
			if (averageFps >= 50f)
			{
				return ServerPerformanceState.High;
			}
			if (averageFps >= 30f)
			{
				return ServerPerformanceState.Medium;
			}
			return ServerPerformanceState.Low;
		}

		// Token: 0x0400006E RID: 110
		private List<NetworkStatusReplicationComponent.NetworkStatusData> _peerData = new List<NetworkStatusReplicationComponent.NetworkStatusData>();

		// Token: 0x0400006F RID: 111
		private float _nextPerformanceStateTrySendTime;

		// Token: 0x04000070 RID: 112
		private ServerPerformanceState _lastSentPerformanceState;

		// Token: 0x020000A2 RID: 162
		private class NetworkStatusData
		{
			// Token: 0x040001A0 RID: 416
			public float NextPingForceSendTime;

			// Token: 0x040001A1 RID: 417
			public float NextPingTrySendTime;

			// Token: 0x040001A2 RID: 418
			public int LastSentPingValue = -1;

			// Token: 0x040001A3 RID: 419
			public float NextLossTrySendTime;

			// Token: 0x040001A4 RID: 420
			public int LastSentLossValue;
		}
	}
}
