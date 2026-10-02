using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000286 RID: 646
	public class CustomBattleAgentLogic : MissionLogic
	{
		// Token: 0x06002402 RID: 9218 RVA: 0x00080AA0 File Offset: 0x0007ECA0
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			if (affectedAgent.Character != null && ((affectorAgent != null) ? affectorAgent.Character : null) != null && affectedAgent.State == AgentState.Active)
			{
				bool flag = affectedAgent.Health - (float)blow.InflictedDamage < 1f;
				bool flag2 = affectedAgent.Team.Side == affectorAgent.Team.Side;
				IAgentOriginBase origin = affectorAgent.Origin;
				BasicCharacterObject character = affectedAgent.Character;
				Formation formation = affectorAgent.Formation;
				BasicCharacterObject basicCharacterObject;
				if (formation == null)
				{
					basicCharacterObject = null;
				}
				else
				{
					Agent captain = formation.Captain;
					basicCharacterObject = ((captain != null) ? captain.Character : null);
				}
				int inflictedDamage = blow.InflictedDamage;
				bool flag3 = flag;
				bool flag4 = flag2;
				MissionWeapon missionWeapon = affectorWeapon;
				origin.OnScoreHit(character, basicCharacterObject, inflictedDamage, flag3, flag4, missionWeapon.CurrentUsageItem);
			}
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x00080B48 File Offset: 0x0007ED48
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectorAgent == null && affectedAgent.IsMount && agentState == AgentState.Routed)
			{
				return;
			}
			if (affectedAgent.Origin != null)
			{
				if (agentState == AgentState.Unconscious)
				{
					affectedAgent.Origin.SetWounded();
					if (affectedAgent == base.Mission.MainAgent)
					{
						this.BecomeGhost();
						return;
					}
				}
				else
				{
					if (agentState == AgentState.Killed)
					{
						affectedAgent.Origin.SetKilled();
						return;
					}
					affectedAgent.Origin.SetRouted(false);
				}
			}
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x00080BB0 File Offset: 0x0007EDB0
		private void BecomeGhost()
		{
			Agent leader = base.Mission.PlayerEnemyTeam.Leader;
			if (leader != null && leader.IsActive())
			{
				leader.Controller = AgentControllerType.AI;
			}
			Agent mainAgent = base.Mission.MainAgent;
			if (mainAgent != null && mainAgent.IsActive())
			{
				base.Mission.MainAgent.Controller = AgentControllerType.AI;
			}
		}
	}
}
