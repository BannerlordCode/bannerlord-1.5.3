using System;

namespace TaleWorlds.MountAndBlade.MissionSpawnHandlers
{
	// Token: 0x020003D2 RID: 978
	public class CustomMissionSpawnHandler : MissionLogic
	{
		// Token: 0x060036FC RID: 14076 RVA: 0x000E3B79 File Offset: 0x000E1D79
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x000E3B94 File Offset: 0x000E1D94
		protected static MissionSpawnSettings CreateCustomBattleWaveSpawnSettings()
		{
			return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating, MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer, MissionSpawnSettings.ReinforcementSpawnMethod.Wave, 3f, 0f, 0f, 0.5f, 0, 0f, 0f, 1f, 0.75f);
		}

		// Token: 0x040017AA RID: 6058
		protected DefaultBattleMissionAgentSpawnLogic _missionAgentSpawnLogic;
	}
}
