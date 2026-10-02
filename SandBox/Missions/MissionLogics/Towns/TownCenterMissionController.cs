using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Towns
{
	// Token: 0x0200008F RID: 143
	public class TownCenterMissionController : MissionLogic
	{
		// Token: 0x060005AE RID: 1454 RVA: 0x000256B6 File Offset: 0x000238B6
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = true;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000256CC File Offset: 0x000238CC
		public override void AfterStart()
		{
			bool isNight = Campaign.Current.IsNight;
			base.Mission.SetMissionMode(MissionMode.StartUp, true);
			base.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
			base.Mission.IsQuestScreenAccessible = true;
			MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, true, false, false, "");
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

		// Token: 0x060005B0 RID: 1456 RVA: 0x0002575E File Offset: 0x0002395E
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}
	}
}
