using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CE RID: 718
	public interface IAgentVisualCreator
	{
		// Token: 0x060029BB RID: 10683
		IAgentVisual Create(AgentVisualsData data, string name, bool needBatchedVersionForWeaponMeshes, bool forceUseFaceCache);
	}
}
