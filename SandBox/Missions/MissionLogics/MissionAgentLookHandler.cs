using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000074 RID: 116
	public class MissionAgentLookHandler : MissionLogic
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x0001DDC0 File Offset: 0x0001BFC0
		public MissionAgentLookHandler()
		{
			this._staticPointList = new List<MissionAgentLookHandler.PointOfInterest>();
			this._checklist = new List<MissionAgentLookHandler.LookInfo>();
			this._selectionDelegate = new MissionAgentLookHandler.SelectionDelegate(this.SelectRandomAccordingToScore);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
		public override void AfterStart()
		{
			this.AddStablePointsOfInterest();
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001DDF8 File Offset: 0x0001BFF8
		private void AddStablePointsOfInterest()
		{
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag("point_of_interest"))
			{
				this._staticPointList.Add(new MissionAgentLookHandler.PointOfInterest(gameEntity.GetGlobalFrame()));
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001DE64 File Offset: 0x0001C064
		private void DebugTick()
		{
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001DE68 File Offset: 0x0001C068
		public override void OnMissionTick(float dt)
		{
			if (Game.Current.IsDevelopmentMode)
			{
				this.DebugTick();
			}
			float currentTime = base.Mission.CurrentTime;
			foreach (MissionAgentLookHandler.LookInfo lookInfo in this._checklist)
			{
				if (lookInfo.Agent.IsActive() && !ConversationMission.ConversationAgents.Contains(lookInfo.Agent) && (!ConversationMission.ConversationAgents.Any<Agent>() || !lookInfo.Agent.IsPlayerControlled))
				{
					if (lookInfo.CheckTimer.Check(currentTime))
					{
						MissionAgentLookHandler.PointOfInterest pointOfInterest = this._selectionDelegate(lookInfo.Agent);
						if (pointOfInterest != null)
						{
							lookInfo.Reset(pointOfInterest, 5f);
						}
						else
						{
							lookInfo.Reset(null, 1f + MBRandom.RandomFloat);
						}
					}
					else if (lookInfo.PointOfInterest != null && (!lookInfo.PointOfInterest.IsActive || !lookInfo.PointOfInterest.IsVisibleFor(lookInfo.Agent)))
					{
						MissionAgentLookHandler.PointOfInterest pointOfInterest2 = this._selectionDelegate(lookInfo.Agent);
						if (pointOfInterest2 != null)
						{
							lookInfo.Reset(pointOfInterest2, 5f + MBRandom.RandomFloat);
						}
						else
						{
							lookInfo.Reset(null, MBRandom.RandomFloat * 5f + 5f);
						}
					}
					else if (lookInfo.PointOfInterest != null)
					{
						Vec3 targetPosition = lookInfo.PointOfInterest.GetTargetPosition();
						lookInfo.Agent.SetLookToPointOfInterest(targetPosition);
					}
				}
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001E000 File Offset: 0x0001C200
		private MissionAgentLookHandler.PointOfInterest SelectFirstNonAgent(Agent agent)
		{
			if (agent.IsAIControlled)
			{
				int num = MBRandom.RandomInt(this._staticPointList.Count);
				int num2 = num;
				MissionAgentLookHandler.PointOfInterest pointOfInterest;
				for (;;)
				{
					pointOfInterest = this._staticPointList[num2];
					if (pointOfInterest.GetScore(agent) > 0f)
					{
						break;
					}
					num2 = ((num2 + 1 == this._staticPointList.Count) ? 0 : (num2 + 1));
					if (num2 == num)
					{
						goto IL_0053;
					}
				}
				return pointOfInterest;
			}
			IL_0053:
			return null;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001E064 File Offset: 0x0001C264
		private MissionAgentLookHandler.PointOfInterest SelectBestOfLimitedNonAgent(Agent agent)
		{
			int num = 3;
			MissionAgentLookHandler.PointOfInterest pointOfInterest = null;
			float num2 = -1f;
			if (agent.IsAIControlled)
			{
				int num3 = MBRandom.RandomInt(this._staticPointList.Count);
				int num4 = num3;
				do
				{
					MissionAgentLookHandler.PointOfInterest pointOfInterest2 = this._staticPointList[num4];
					float score = pointOfInterest2.GetScore(agent);
					if (score > 0f)
					{
						if (score > num2)
						{
							num2 = score;
							pointOfInterest = pointOfInterest2;
						}
						num--;
					}
					num4 = ((num4 + 1 == this._staticPointList.Count) ? 0 : (num4 + 1));
				}
				while (num4 != num3 && num > 0);
			}
			return pointOfInterest;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		private MissionAgentLookHandler.PointOfInterest SelectBest(Agent agent)
		{
			MissionAgentLookHandler.PointOfInterest pointOfInterest = null;
			float num = -1f;
			if (agent.IsAIControlled)
			{
				foreach (MissionAgentLookHandler.PointOfInterest pointOfInterest2 in this._staticPointList)
				{
					float score = pointOfInterest2.GetScore(agent);
					if (score > 0f && score > num)
					{
						num = score;
						pointOfInterest = pointOfInterest2;
					}
				}
				AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(base.Mission, agent.Position.AsVec2, 5f, false);
				while (proximityMapSearchStruct.LastFoundAgent != null)
				{
					MissionAgentLookHandler.PointOfInterest pointOfInterest3 = new MissionAgentLookHandler.PointOfInterest(proximityMapSearchStruct.LastFoundAgent);
					float score2 = pointOfInterest3.GetScore(agent);
					if (score2 > 0f && score2 > num)
					{
						num = score2;
						pointOfInterest = pointOfInterest3;
					}
					AgentProximityMap.FindNext(base.Mission, ref proximityMapSearchStruct);
				}
			}
			return pointOfInterest;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001E1D0 File Offset: 0x0001C3D0
		private MissionAgentLookHandler.PointOfInterest SelectRandomAccordingToScore(Agent agent)
		{
			float num = 0f;
			List<KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>> list = new List<KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>>();
			if (agent.IsAIControlled)
			{
				foreach (MissionAgentLookHandler.PointOfInterest pointOfInterest in this._staticPointList)
				{
					float score = pointOfInterest.GetScore(agent);
					if (score > 0f)
					{
						list.Add(new KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>(score, pointOfInterest));
						num += score;
					}
				}
				AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, agent.Position.AsVec2, 5f, false);
				while (proximityMapSearchStruct.LastFoundAgent != null)
				{
					MissionAgentLookHandler.PointOfInterest pointOfInterest2 = new MissionAgentLookHandler.PointOfInterest(proximityMapSearchStruct.LastFoundAgent);
					float score2 = pointOfInterest2.GetScore(agent);
					if (score2 > 0f)
					{
						list.Add(new KeyValuePair<float, MissionAgentLookHandler.PointOfInterest>(score2, pointOfInterest2));
						num += score2;
					}
					AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			float num2 = MBRandom.RandomFloat * num;
			MissionAgentLookHandler.PointOfInterest pointOfInterest3 = list[list.Count - 1].Value;
			foreach (KeyValuePair<float, MissionAgentLookHandler.PointOfInterest> keyValuePair in list)
			{
				num2 -= keyValuePair.Key;
				if (num2 <= 0f)
				{
					pointOfInterest3 = keyValuePair.Value;
					break;
				}
			}
			return pointOfInterest3;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001E348 File Offset: 0x0001C548
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsHuman)
			{
				this._checklist.Add(new MissionAgentLookHandler.LookInfo(agent, MBRandom.RandomFloat));
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001E368 File Offset: 0x0001C568
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			for (int i = 0; i < this._checklist.Count; i++)
			{
				MissionAgentLookHandler.LookInfo lookInfo = this._checklist[i];
				if (lookInfo.Agent == affectedAgent)
				{
					this._checklist.RemoveAt(i);
					i--;
				}
				else if (lookInfo.PointOfInterest != null && lookInfo.PointOfInterest.IsRelevant(affectedAgent))
				{
					lookInfo.Reset(null, MBRandom.RandomFloat * 2f + 2f);
				}
			}
		}

		// Token: 0x04000279 RID: 633
		private readonly List<MissionAgentLookHandler.PointOfInterest> _staticPointList;

		// Token: 0x0400027A RID: 634
		private readonly List<MissionAgentLookHandler.LookInfo> _checklist;

		// Token: 0x0400027B RID: 635
		private MissionAgentLookHandler.SelectionDelegate _selectionDelegate;

		// Token: 0x02000177 RID: 375
		private class PointOfInterest
		{
			// Token: 0x17000141 RID: 321
			// (get) Token: 0x06000EAF RID: 3759 RVA: 0x000666DF File Offset: 0x000648DF
			public bool IsActive
			{
				get
				{
					return this._agent == null || this._agent.IsActive();
				}
			}

			// Token: 0x06000EB0 RID: 3760 RVA: 0x000666F8 File Offset: 0x000648F8
			public PointOfInterest(Agent agent)
			{
				this._agent = agent;
				this._selectDistance = 5;
				this._releaseDistanceSquare = 36;
				this._ignoreDirection = false;
				CharacterObject characterObject = (CharacterObject)agent.Character;
				if (!agent.IsHuman)
				{
					this._priority = 1;
					return;
				}
				if (characterObject.IsHero)
				{
					this._priority = 5;
					return;
				}
				if (characterObject.Occupation == Occupation.HorseTrader || characterObject.Occupation == Occupation.Weaponsmith || characterObject.Occupation == Occupation.GoodsTrader || characterObject.Occupation == Occupation.Armorer || characterObject.Occupation == Occupation.Blacksmith)
				{
					this._priority = 3;
					return;
				}
				this._priority = 1;
			}

			// Token: 0x06000EB1 RID: 3761 RVA: 0x00066794 File Offset: 0x00064994
			public PointOfInterest(MatrixFrame frame)
			{
				this._frame = frame;
				this._selectDistance = 4;
				this._releaseDistanceSquare = 25;
				this._ignoreDirection = true;
				this._priority = 2;
			}

			// Token: 0x06000EB2 RID: 3762 RVA: 0x000667C0 File Offset: 0x000649C0
			public float GetScore(Agent agent)
			{
				if (agent == this._agent || this.GetBasicPosition().DistanceSquared(agent.Position) > (float)(this._selectDistance * this._selectDistance))
				{
					return -1f;
				}
				Vec3 vec = this.GetTargetPosition() - agent.GetEyeGlobalPosition();
				float num = vec.Normalize();
				if (Vec2.DotProduct(vec.AsVec2, agent.GetMovementDirection()) < 0.7f)
				{
					return -1f;
				}
				float num2 = (float)(this._priority * this._selectDistance) / num;
				if (this.IsMoving())
				{
					num2 *= 5f;
				}
				if (!this._ignoreDirection)
				{
					MatrixFrame matrixFrame = this.GetTargetFrame();
					Vec2 asVec = matrixFrame.rotation.f.AsVec2;
					matrixFrame = agent.Frame;
					float num3 = Vec2.DotProduct(asVec, matrixFrame.rotation.f.AsVec2);
					if (num3 < -0.7f)
					{
						num2 *= 2f;
					}
					else if (MathF.Abs(num3) < 0.1f)
					{
						num2 *= 2f;
					}
				}
				return num2;
			}

			// Token: 0x06000EB3 RID: 3763 RVA: 0x000668C5 File Offset: 0x00064AC5
			public Vec3 GetTargetPosition()
			{
				Agent agent = this._agent;
				if (agent == null)
				{
					return this._frame.origin;
				}
				return agent.GetEyeGlobalPosition();
			}

			// Token: 0x06000EB4 RID: 3764 RVA: 0x000668E2 File Offset: 0x00064AE2
			public Vec3 GetBasicPosition()
			{
				if (this._agent == null)
				{
					return this._frame.origin;
				}
				return this._agent.Position;
			}

			// Token: 0x06000EB5 RID: 3765 RVA: 0x00066904 File Offset: 0x00064B04
			private bool IsMoving()
			{
				return this._agent == null || this._agent.GetCurrentVelocity().LengthSquared > 0.040000003f;
			}

			// Token: 0x06000EB6 RID: 3766 RVA: 0x00066935 File Offset: 0x00064B35
			private MatrixFrame GetTargetFrame()
			{
				if (this._agent == null)
				{
					return this._frame;
				}
				return this._agent.Frame;
			}

			// Token: 0x06000EB7 RID: 3767 RVA: 0x00066954 File Offset: 0x00064B54
			public bool IsVisibleFor(Agent agent)
			{
				Vec3 basicPosition = this.GetBasicPosition();
				Vec3 position = agent.Position;
				if (agent == this._agent || position.DistanceSquared(basicPosition) > (float)this._releaseDistanceSquare)
				{
					return false;
				}
				Vec3 vec = basicPosition - position;
				vec.Normalize();
				return Vec2.DotProduct(vec.AsVec2, agent.GetMovementDirection()) > 0.4f;
			}

			// Token: 0x06000EB8 RID: 3768 RVA: 0x000669B4 File Offset: 0x00064BB4
			public bool IsRelevant(Agent agent)
			{
				return agent == this._agent;
			}

			// Token: 0x0400071A RID: 1818
			public const int MaxSelectDistanceForAgent = 5;

			// Token: 0x0400071B RID: 1819
			public const int MaxSelectDistanceForFrame = 4;

			// Token: 0x0400071C RID: 1820
			private readonly int _selectDistance;

			// Token: 0x0400071D RID: 1821
			private readonly int _releaseDistanceSquare;

			// Token: 0x0400071E RID: 1822
			private readonly Agent _agent;

			// Token: 0x0400071F RID: 1823
			private readonly MatrixFrame _frame;

			// Token: 0x04000720 RID: 1824
			private readonly bool _ignoreDirection;

			// Token: 0x04000721 RID: 1825
			private readonly int _priority;
		}

		// Token: 0x02000178 RID: 376
		private class LookInfo
		{
			// Token: 0x06000EB9 RID: 3769 RVA: 0x000669BF File Offset: 0x00064BBF
			public LookInfo(Agent agent, float checkTime)
			{
				this.Agent = agent;
				this.CheckTimer = new Timer(Mission.Current.CurrentTime, checkTime, true);
			}

			// Token: 0x06000EBA RID: 3770 RVA: 0x000669E8 File Offset: 0x00064BE8
			public void Reset(MissionAgentLookHandler.PointOfInterest pointOfInterest, float duration)
			{
				if (this.PointOfInterest != pointOfInterest)
				{
					this.PointOfInterest = pointOfInterest;
					if (this.PointOfInterest != null)
					{
						this.Agent.SetLookToPointOfInterest(this.PointOfInterest.GetTargetPosition());
					}
					else if (this.Agent.IsActive())
					{
						this.Agent.DisableLookToPointOfInterest();
					}
				}
				this.CheckTimer.Reset(Mission.Current.CurrentTime, duration);
			}

			// Token: 0x04000722 RID: 1826
			public readonly Agent Agent;

			// Token: 0x04000723 RID: 1827
			public MissionAgentLookHandler.PointOfInterest PointOfInterest;

			// Token: 0x04000724 RID: 1828
			public readonly Timer CheckTimer;
		}

		// Token: 0x02000179 RID: 377
		// (Invoke) Token: 0x06000EBC RID: 3772
		private delegate MissionAgentLookHandler.PointOfInterest SelectionDelegate(Agent agent);
	}
}
