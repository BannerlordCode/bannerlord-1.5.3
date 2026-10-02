using System;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000205 RID: 517
	public class DefaultMissionDifficultyModel : MissionDifficultyModel
	{
		// Token: 0x06001E2C RID: 7724 RVA: 0x000671A8 File Offset: 0x000653A8
		public override float GetDamageMultiplierOfCombatDifficulty(Agent victimAgent, Agent attackerAgent = null)
		{
			float num = 1f;
			victimAgent = (victimAgent.IsMount ? victimAgent.RiderAgent : victimAgent);
			if (victimAgent != null)
			{
				if (victimAgent.IsMainAgent)
				{
					num = Mission.Current.DamageToPlayerMultiplier;
				}
				else
				{
					Mission mission = Mission.Current;
					Agent agent = ((mission != null) ? mission.MainAgent : null);
					if (agent != null && victimAgent.IsFriendOf(agent))
					{
						if (attackerAgent != null && attackerAgent == agent)
						{
							num = Mission.Current.DamageFromPlayerToFriendsMultiplier;
						}
						else
						{
							num = Mission.Current.DamageToFriendsMultiplier;
						}
					}
				}
			}
			return num;
		}
	}
}
