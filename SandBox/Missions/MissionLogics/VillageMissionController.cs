using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200008A RID: 138
	public class VillageMissionController : MissionLogic
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00023E5B File Offset: 0x0002205B
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00023E70 File Offset: 0x00022070
		public override void AfterStart()
		{
			base.AfterStart();
			bool isNight = Campaign.Current.IsNight;
			base.Mission.IsInventoryAccessible = true;
			base.Mission.IsQuestScreenAccessible = true;
			MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, false, false, false, "");
			missionBehavior.SpawnLocationCharacters(null);
			SandBoxHelpers.MissionHelper.SpawnHorses();
			if (!isNight)
			{
				SandBoxHelpers.MissionHelper.SpawnSheeps();
				SandBoxHelpers.MissionHelper.SpawnCows();
				SandBoxHelpers.MissionHelper.SpawnHogs();
				SandBoxHelpers.MissionHelper.SpawnGeese();
				SandBoxHelpers.MissionHelper.SpawnChicken();
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00023EEF File Offset: 0x000220EF
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}
	}
}
