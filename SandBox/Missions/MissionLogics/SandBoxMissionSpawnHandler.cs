using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000083 RID: 131
	public class SandBoxMissionSpawnHandler : MissionLogic
	{
		// Token: 0x06000532 RID: 1330 RVA: 0x00022FB7 File Offset: 0x000211B7
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			this._mapEvent = MapEvent.PlayerMapEvent;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00022FDC File Offset: 0x000211DC
		protected static MissionSpawnSettings CreateSandBoxBattleWaveSpawnSettings()
		{
			int reinforcementWaveCount = BannerlordConfig.GetReinforcementWaveCount();
			return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating, MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer, MissionSpawnSettings.ReinforcementSpawnMethod.Wave, 3f, 0f, 0f, 0.5f, reinforcementWaveCount, 0f, 0f, 1f, 0.75f);
		}

		// Token: 0x040002C4 RID: 708
		protected DefaultBattleMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x040002C5 RID: 709
		protected MapEvent _mapEvent;
	}
}
