using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Objects.Siege
{
	// Token: 0x020003D7 RID: 983
	public class AgentPathNavMeshChecker
	{
		// Token: 0x06003711 RID: 14097 RVA: 0x000E42C0 File Offset: 0x000E24C0
		public AgentPathNavMeshChecker(Mission mission, MatrixFrame pathFrameToCheck, float radiusToCheck, int navMeshId, BattleSideEnum teamToCollect, AgentPathNavMeshChecker.Direction directionToCollect, float maxDistanceCheck, float agentMoveTime)
		{
			this._mission = mission;
			this._pathFrameToCheck = pathFrameToCheck;
			this._radiusToCheck = radiusToCheck;
			this._navMeshId = navMeshId;
			this._teamToCollect = teamToCollect;
			this._directionToCollect = directionToCollect;
			this._maxDistanceCheck = maxDistanceCheck;
			this._agentMoveTime = agentMoveTime;
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000E431C File Offset: 0x000E251C
		public void Tick(float dt)
		{
			float currentTime = this._mission.CurrentTime;
			if (this._tickOccasionallyTimer == null || this._tickOccasionallyTimer.Check(currentTime))
			{
				float num = dt;
				if (this._tickOccasionallyTimer != null)
				{
					num = this._tickOccasionallyTimer.ElapsedTime();
				}
				this._tickOccasionallyTimer = new Timer(currentTime, 0.1f + MBRandom.RandomFloat * 0.1f, true);
				this.TickOccasionally(num);
			}
			bool flag = false;
			foreach (Agent agent in this._nearbyAgents)
			{
				Vec3 position = agent.Position;
				if ((this._teamToCollect == BattleSideEnum.None || (agent.Team != null && agent.Team.Side == this._teamToCollect)) && agent.IsAIControlled)
				{
					if (agent.GetCurrentNavigationFaceId() == this._navMeshId)
					{
						flag = true;
						break;
					}
					if (this._isBeingUsed && position.DistanceSquared(this._pathFrameToCheck.origin) < this._radiusToCheck * this._radiusToCheck)
					{
						flag = true;
						break;
					}
					if (agent.MovementVelocity.LengthSquared > 0.01f)
					{
						Vec2 vec;
						if (this._directionToCollect == AgentPathNavMeshChecker.Direction.ForwardOnly)
						{
							vec = this._pathFrameToCheck.rotation.f.AsVec2;
						}
						else if (this._directionToCollect == AgentPathNavMeshChecker.Direction.BackwardOnly)
						{
							vec = -this._pathFrameToCheck.rotation.f.AsVec2;
						}
						else
						{
							vec = Vec2.Zero;
						}
						if (agent.HasPathThroughNavigationFaceIdFromDirection(this._navMeshId, vec))
						{
							float num2 = agent.GetPathDistanceToPoint(ref this._pathFrameToCheck.origin);
							if (num2 >= 100000f)
							{
								num2 = agent.Position.Distance(this._pathFrameToCheck.origin);
							}
							if (num2 < this._radiusToCheck * 2f || num2 / agent.GetMaximumForwardUnlimitedSpeed() < this._agentMoveTime)
							{
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				this._isBeingUsed = true;
				this._setBeingUsedToFalseTimer = null;
			}
			else if (this._setBeingUsedToFalseTimer == null)
			{
				this._setBeingUsedToFalseTimer = new Timer(currentTime, 1f, true);
			}
			if (this._setBeingUsedToFalseTimer != null && this._setBeingUsedToFalseTimer.Check(currentTime))
			{
				this._setBeingUsedToFalseTimer = null;
				this._isBeingUsed = false;
			}
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000E458C File Offset: 0x000E278C
		public void TickOccasionally(float dt)
		{
			this._nearbyAgents = this._mission.GetNearbyAgents(this._pathFrameToCheck.origin.AsVec2, this._maxDistanceCheck, this._nearbyAgents);
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000E45BB File Offset: 0x000E27BB
		public bool HasAgentsUsingPath()
		{
			return this._isBeingUsed;
		}

		// Token: 0x040017B9 RID: 6073
		private BattleSideEnum _teamToCollect;

		// Token: 0x040017BA RID: 6074
		private AgentPathNavMeshChecker.Direction _directionToCollect;

		// Token: 0x040017BB RID: 6075
		private MatrixFrame _pathFrameToCheck;

		// Token: 0x040017BC RID: 6076
		private float _radiusToCheck;

		// Token: 0x040017BD RID: 6077
		private Mission _mission;

		// Token: 0x040017BE RID: 6078
		private int _navMeshId;

		// Token: 0x040017BF RID: 6079
		private Timer _tickOccasionallyTimer;

		// Token: 0x040017C0 RID: 6080
		private MBList<Agent> _nearbyAgents = new MBList<Agent>();

		// Token: 0x040017C1 RID: 6081
		private bool _isBeingUsed;

		// Token: 0x040017C2 RID: 6082
		private Timer _setBeingUsedToFalseTimer;

		// Token: 0x040017C3 RID: 6083
		private float _maxDistanceCheck;

		// Token: 0x040017C4 RID: 6084
		private float _agentMoveTime;

		// Token: 0x0200069A RID: 1690
		public enum Direction
		{
			// Token: 0x04002338 RID: 9016
			ForwardOnly,
			// Token: 0x04002339 RID: 9017
			BackwardOnly,
			// Token: 0x0400233A RID: 9018
			BothDirections
		}
	}
}
