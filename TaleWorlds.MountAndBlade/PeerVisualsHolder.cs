using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D1 RID: 721
	public class PeerVisualsHolder
	{
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x0009DA5C File Offset: 0x0009BC5C
		// (set) Token: 0x060029DC RID: 10716 RVA: 0x0009DA64 File Offset: 0x0009BC64
		public MissionPeer Peer { get; private set; }

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x0009DA6D File Offset: 0x0009BC6D
		// (set) Token: 0x060029DE RID: 10718 RVA: 0x0009DA75 File Offset: 0x0009BC75
		public int VisualsIndex { get; private set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x0009DA7E File Offset: 0x0009BC7E
		// (set) Token: 0x060029E0 RID: 10720 RVA: 0x0009DA86 File Offset: 0x0009BC86
		public IAgentVisual AgentVisuals { get; private set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060029E1 RID: 10721 RVA: 0x0009DA8F File Offset: 0x0009BC8F
		// (set) Token: 0x060029E2 RID: 10722 RVA: 0x0009DA97 File Offset: 0x0009BC97
		public IAgentVisual MountAgentVisuals { get; private set; }

		// Token: 0x060029E3 RID: 10723 RVA: 0x0009DAA0 File Offset: 0x0009BCA0
		public PeerVisualsHolder(MissionPeer peer, int index, IAgentVisual agentVisuals, IAgentVisual mountVisuals)
		{
			this.Peer = peer;
			this.VisualsIndex = index;
			this.AgentVisuals = agentVisuals;
			this.MountAgentVisuals = mountVisuals;
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x0009DAC5 File Offset: 0x0009BCC5
		public void SetMountVisuals(IAgentVisual mountAgentVisuals)
		{
			this.MountAgentVisuals = mountAgentVisuals;
		}
	}
}
