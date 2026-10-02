using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200006C RID: 108
	public class HouseMissionController : MissionLogic
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x0001AAE6 File Offset: 0x00018CE6
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001AAFF File Offset: 0x00018CFF
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = true;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001AB13 File Offset: 0x00018D13
		public override void EarlyStart()
		{
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001AB18 File Offset: 0x00018D18
		public override void AfterStart()
		{
			base.AfterStart();
			base.Mission.SetMissionMode(MissionMode.StartUp, true);
			base.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
			base.Mission.IsQuestScreenAccessible = true;
			SandBoxHelpers.MissionHelper.SpawnPlayer(base.Mission.DoesMissionRequireCivilianEquipment, true, true, false, "");
			this._missionAgentHandler.SpawnLocationCharacters(null);
		}

		// Token: 0x0400025E RID: 606
		private MissionAgentHandler _missionAgentHandler;
	}
}
