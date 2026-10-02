using System;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000025 RID: 37
	internal sealed class DebugAgentScaleOnNetworkTestComponent : UdpNetworkComponent
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x00007FE4 File Offset: 0x000061E4
		public override void OnUdpNetworkHandlerTick(float dt)
		{
			if (GameNetwork.IsServer)
			{
				float totalMissionTime = MBCommon.GetTotalMissionTime();
				if (this._lastTestSendTime < totalMissionTime + 10f)
				{
					AgentReadOnlyList agents = Mission.Current.Agents;
					int count = agents.Count;
					this._lastTestSendTime = totalMissionTime;
					int num = (int)(new Random().NextDouble() * (double)count);
					Agent agent = agents[num];
					if (agent.IsActive())
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new DebugAgentScaleOnNetworkTest(agent.Index, agent.AgentScale));
						GameNetwork.EndBroadcastModuleEventUnreliable(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008065 File Offset: 0x00006265
		public DebugAgentScaleOnNetworkTestComponent()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				DebugAgentScaleOnNetworkTestComponent.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000807A File Offset: 0x0000627A
		public override void OnUdpNetworkHandlerClose()
		{
			base.OnUdpNetworkHandlerClose();
			if (GameNetwork.IsClientOrReplay)
			{
				DebugAgentScaleOnNetworkTestComponent.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008090 File Offset: 0x00006290
		private static void HandleServerMessageDebugAgentScaleOnNetworkTest(DebugAgentScaleOnNetworkTest message)
		{
			Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(message.AgentToTestIndex, true);
			if (agentFromIndex != null && agentFromIndex.IsActive())
			{
				CompressionMission.DebugScaleValueCompressionInfo.GetPrecision();
				float agentScale = agentFromIndex.AgentScale;
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000080C7 File Offset: 0x000062C7
		private static void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			new GameNetwork.NetworkMessageHandlerRegisterer(mode).Register<DebugAgentScaleOnNetworkTest>(new GameNetworkMessage.ServerMessageHandlerDelegate<DebugAgentScaleOnNetworkTest>(DebugAgentScaleOnNetworkTestComponent.HandleServerMessageDebugAgentScaleOnNetworkTest));
		}

		// Token: 0x0400006A RID: 106
		private float _lastTestSendTime;
	}
}
