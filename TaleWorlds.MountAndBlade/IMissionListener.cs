using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000265 RID: 613
	public interface IMissionListener
	{
		// Token: 0x060022E2 RID: 8930
		void OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType);

		// Token: 0x060022E3 RID: 8931
		void OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType);

		// Token: 0x060022E4 RID: 8932
		void OnEndMission();

		// Token: 0x060022E5 RID: 8933
		void OnMissionModeChange(MissionMode oldMissionMode, bool atStart);

		// Token: 0x060022E6 RID: 8934
		void OnConversationCharacterChanged();

		// Token: 0x060022E7 RID: 8935
		void OnResetMission();

		// Token: 0x060022E8 RID: 8936
		void OnDeploymentPlanMade(Team team, bool isFirstPlan);
	}
}
