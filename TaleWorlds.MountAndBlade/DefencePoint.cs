using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000336 RID: 822
	public class DefencePoint : ScriptComponentBehavior
	{
		// Token: 0x06002EA3 RID: 11939 RVA: 0x000B45DF File Offset: 0x000B27DF
		public void AddDefender(Agent defender)
		{
			this.defenders.Add(defender);
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000B45ED File Offset: 0x000B27ED
		public bool RemoveDefender(Agent defender)
		{
			return this.defenders.Remove(defender);
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000B45FB File Offset: 0x000B27FB
		public IEnumerable<Agent> Defenders
		{
			get
			{
				return this.defenders;
			}
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000B4604 File Offset: 0x000B2804
		public void PurgeInactiveDefenders()
		{
			foreach (Agent agent in this.defenders.Where<Agent>((Agent d) => !d.IsActive()).ToList<Agent>())
			{
				this.RemoveDefender(agent);
			}
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000B4684 File Offset: 0x000B2884
		private MatrixFrame GetPosition(int index)
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 f = globalFrame.rotation.f;
			f.Normalize();
			globalFrame.origin -= f * (float)index * ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius) * 2f * 1.5f;
			return globalFrame;
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000B46F4 File Offset: 0x000B28F4
		public MatrixFrame GetVacantPosition(Agent a)
		{
			Mission mission = Mission.Current;
			Team team = mission.Teams.First<Team>((Team t) => t.Side == this.Side);
			for (int i = 0; i < 100; i++)
			{
				MatrixFrame position = this.GetPosition(i);
				Agent closestAllyAgent = mission.GetClosestAllyAgent(team, position.origin, ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius));
				if (closestAllyAgent == null || closestAllyAgent == a)
				{
					return position;
				}
			}
			Debug.FailedAssert("Couldn't find a vacant position", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\DefencePoint.cs", "GetVacantPosition", 73);
			return MatrixFrame.Identity;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000B4774 File Offset: 0x000B2974
		public int CountOccupiedDefenderPositions()
		{
			Mission mission = Mission.Current;
			Team team = mission.Teams.First<Team>((Team t) => t.Side == this.Side);
			for (int i = 0; i < 100; i++)
			{
				MatrixFrame position = this.GetPosition(i);
				if (mission.GetClosestAllyAgent(team, position.origin, ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius)) == null)
				{
					return i;
				}
			}
			return 100;
		}

		// Token: 0x04001277 RID: 4727
		private List<Agent> defenders = new List<Agent>();

		// Token: 0x04001278 RID: 4728
		public BattleSideEnum Side;
	}
}
