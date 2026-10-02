using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006E RID: 110
	public class IndoorMissionController : MissionLogic
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x0001AB80 File Offset: 0x00018D80
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = true;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001AB94 File Offset: 0x00018D94
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001ABB0 File Offset: 0x00018DB0
		public override void AfterStart()
		{
			base.AfterStart();
			base.Mission.SetMissionMode(MissionMode.StartUp, true);
			base.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
			base.Mission.IsQuestScreenAccessible = true;
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, true, false, false, "");
			this._missionAgentHandler.SpawnLocationCharacters(null);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001AC18 File Offset: 0x00018E18
		public override void OnAfterMissionLoadingFinished()
		{
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
		}

		// Token: 0x0400025F RID: 607
		private MissionAgentHandler _missionAgentHandler;
	}
}
