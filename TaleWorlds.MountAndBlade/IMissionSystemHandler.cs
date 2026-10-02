using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000249 RID: 585
	public interface IMissionSystemHandler
	{
		// Token: 0x060021EF RID: 8687
		void OnMissionAfterStarting(Mission mission);

		// Token: 0x060021F0 RID: 8688
		void OnMissionLoadingFinished(Mission mission);

		// Token: 0x060021F1 RID: 8689
		void BeforeMissionTick(Mission mission, float realDt);

		// Token: 0x060021F2 RID: 8690
		void AfterMissionTick(Mission mission, float realDt);

		// Token: 0x060021F3 RID: 8691
		void UpdateCamera(Mission mission, float realDt);

		// Token: 0x060021F4 RID: 8692
		bool RenderIsReady();

		// Token: 0x060021F5 RID: 8693
		IEnumerable<MissionBehavior> OnAddBehaviors(IEnumerable<MissionBehavior> behaviors, Mission mission, string missionName, bool addDefaultMissionBehaviors);
	}
}
