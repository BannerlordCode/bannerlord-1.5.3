using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000283 RID: 643
	public class BattlePowerCalculationLogic : MissionLogic, IBattlePowerCalculationLogic, IMissionBehavior
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x00080481 File Offset: 0x0007E681
		// (set) Token: 0x060023F0 RID: 9200 RVA: 0x00080489 File Offset: 0x0007E689
		public bool IsTeamPowersCalculated { get; private set; }

		// Token: 0x060023F1 RID: 9201 RVA: 0x00080494 File Offset: 0x0007E694
		public BattlePowerCalculationLogic()
		{
			this._sidePowerData = new Dictionary<Team, float>[2];
			for (int i = 0; i < 2; i++)
			{
				this._sidePowerData[i] = new Dictionary<Team, float>();
			}
			this.IsTeamPowersCalculated = false;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000804D3 File Offset: 0x0007E6D3
		public float GetTotalTeamPower(Team team)
		{
			if (!this.IsTeamPowersCalculated)
			{
				this.CalculateTeamPowers();
			}
			return this._sidePowerData[(int)team.Side][team];
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000804F8 File Offset: 0x0007E6F8
		private void CalculateTeamPowers()
		{
			Mission.TeamCollection teams = base.Mission.Teams;
			foreach (Team team in teams)
			{
				this._sidePowerData[(int)team.Side].Add(team, 0f);
			}
			IMissionAgentSpawnLogic missionBehavior = base.Mission.GetMissionBehavior<IMissionAgentSpawnLogic>();
			for (int i = 0; i < 2; i++)
			{
				BattleSideEnum battleSideEnum = (BattleSideEnum)i;
				IEnumerable<IAgentOriginBase> allTroopsForSide = missionBehavior.GetAllTroopsForSide(battleSideEnum);
				Dictionary<Team, float> dictionary = this._sidePowerData[i];
				bool flag = base.Mission.PlayerTeam != null && base.Mission.PlayerTeam.Side == battleSideEnum;
				foreach (IAgentOriginBase agentOriginBase in allTroopsForSide)
				{
					Team agentTeam = Mission.GetAgentTeam(agentOriginBase, flag);
					BasicCharacterObject troop = agentOriginBase.Troop;
					Dictionary<Team, float> dictionary2 = dictionary;
					Team team2 = agentTeam;
					dictionary2[team2] += troop.GetPower();
				}
			}
			foreach (Team team3 in teams)
			{
				team3.QuerySystem.Expire();
			}
			this.IsTeamPowersCalculated = true;
		}

		// Token: 0x04000DD4 RID: 3540
		private Dictionary<Team, float>[] _sidePowerData;
	}
}
