using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000085 RID: 133
	public class SandBoxSiegeMissionSpawnHandler : SandBoxMissionSpawnHandler
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x00023064 File Offset: 0x00021264
		public override void AfterStart()
		{
			int numberOfInvolvedMen = this._mapEvent.GetNumberOfInvolvedMen(BattleSideEnum.Defender);
			int numberOfInvolvedMen2 = this._mapEvent.GetNumberOfInvolvedMen(BattleSideEnum.Attacker);
			int num = numberOfInvolvedMen;
			int num2 = numberOfInvolvedMen2;
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, false);
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, false);
			MissionSpawnSettings missionSpawnSettings = SandBoxMissionSpawnHandler.CreateSandBoxBattleWaveSpawnSettings();
			this._missionAgentSpawnLogic.InitWithSinglePhase(numberOfInvolvedMen, numberOfInvolvedMen2, num, num2, false, false, in missionSpawnSettings);
		}
	}
}
