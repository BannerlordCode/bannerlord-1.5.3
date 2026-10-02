using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000088 RID: 136
	public class StealthMissionController : MissionLogic
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x00023764 File Offset: 0x00021964
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Stealth, true);
			base.Mission.IsInventoryAccessible = !Campaign.Current.IsMainHeroDisguised;
			base.Mission.IsQuestScreenAccessible = true;
			SandBoxHelpers.MissionHelper.SpawnPlayer(false, true, false, false, "");
			Mission.Current.GetMissionBehavior<MissionAgentHandler>().SpawnLocationCharacters(null);
		}
	}
}
