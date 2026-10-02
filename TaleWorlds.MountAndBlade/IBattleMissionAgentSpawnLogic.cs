using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000263 RID: 611
	public interface IBattleMissionAgentSpawnLogic : IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060022D2 RID: 8914
		int TotalSpawnNumber { get; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060022D3 RID: 8915
		int BattleSize { get; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060022D4 RID: 8916
		int NumberOfAgents { get; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060022D5 RID: 8917
		MissionSpawnPhase DefenderActivePhase { get; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060022D6 RID: 8918
		MissionSpawnPhase AttackerActivePhase { get; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060022D7 RID: 8919
		readonly ref MissionSpawnSettings SpawnSettings { get; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060022D8 RID: 8920
		IMissionDeploymentPlan DeploymentPlan { get; }
	}
}
