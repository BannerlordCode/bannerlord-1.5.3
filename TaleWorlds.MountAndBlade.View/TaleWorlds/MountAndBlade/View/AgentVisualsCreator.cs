using System;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200000D RID: 13
	public class AgentVisualsCreator : IAgentVisualCreator
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002527 File Offset: 0x00000727
		public IAgentVisual Create(AgentVisualsData data, string name, bool needBatchedVersionForWeaponMeshes, bool forceUseFaceCache)
		{
			return AgentVisuals.Create(data, name, false, needBatchedVersionForWeaponMeshes, forceUseFaceCache);
		}
	}
}
