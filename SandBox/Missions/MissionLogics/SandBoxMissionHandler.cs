using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000082 RID: 130
	public class SandBoxMissionHandler : MissionLogic
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00022F81 File Offset: 0x00021181
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent != null && affectedAgent.Character != CharacterObject.PlayerCharacter)
			{
				return;
			}
			if (affectedAgent == affectorAgent || affectorAgent == null)
			{
				Campaign.Current.GameMenuManager.SetNextMenu("settlement_player_unconscious");
			}
		}
	}
}
