using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AC RID: 172
	public class FleeBehavior : AgentBehavior
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00030509 File Offset: 0x0002E709
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x00030514 File Offset: 0x0002E714
		private FleeBehavior.FleeTargetType SelectedFleeTargetType
		{
			get
			{
				return this._selectedFleeTargetType;
			}
			set
			{
				if (value != this._selectedFleeTargetType)
				{
					this._selectedFleeTargetType = value;
					MBActionSet actionSet = base.OwnerAgent.ActionSet;
					ActionIndexCache currentAction = base.OwnerAgent.GetCurrentAction(1);
					if (this._selectedFleeTargetType != FleeBehavior.FleeTargetType.Cover && !actionSet.AreActionsAlternatives(in currentAction, in ActionIndexCache.act_scared_idle_1) && !actionSet.AreActionsAlternatives(in currentAction, in ActionIndexCache.act_scared_reaction_1))
					{
						base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_scared_reaction_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					}
					if (this._selectedFleeTargetType == FleeBehavior.FleeTargetType.Cover)
					{
						this.BeAfraid();
					}
					this._selectedGoal.GoToTarget();
				}
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000305C6 File Offset: 0x0002E7C6
		public FleeBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			this._missionFightHandler = base.Mission.GetMissionBehavior<MissionFightHandler>();
			this._reconsiderFleeTargetTimer = new BasicMissionTimer();
			this._state = FleeBehavior.State.None;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00030604 File Offset: 0x0002E804
		public override void Tick(float dt, bool isSimulation)
		{
			switch (this._state)
			{
			case FleeBehavior.State.None:
				base.OwnerAgent.DisableScriptedMovement();
				base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_scared_reaction_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, MBRandom.RandomFloat, false, -0.2f, 0, true);
				this._selectedGoal = new FleeBehavior.FleeCoverTarget(base.Navigator, base.OwnerAgent);
				this.SelectedFleeTargetType = FleeBehavior.FleeTargetType.Cover;
				return;
			case FleeBehavior.State.Afraid:
				if (this._scareTimer.ElapsedTime > this._scareTime)
				{
					this._state = FleeBehavior.State.LookForPlace;
					this._scareTimer = null;
					return;
				}
				break;
			case FleeBehavior.State.LookForPlace:
				this.LookForPlace();
				return;
			case FleeBehavior.State.Flee:
				this.Flee();
				return;
			case FleeBehavior.State.Complain:
				if (this._complainToGuardTimer != null && this._complainToGuardTimer.ElapsedTime > 2f)
				{
					this._complainToGuardTimer = null;
					base.OwnerAgent.SetActionChannel(0, in ActionIndexCache.act_none, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					base.OwnerAgent.SetLookAgent(null);
					(this._selectedGoal as FleeBehavior.FleeAgentTarget).Savior.SetLookAgent(null);
					AlarmedBehaviorGroup.AlarmAgent((this._selectedGoal as FleeBehavior.FleeAgentTarget).Savior);
					this._state = FleeBehavior.State.LookForPlace;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00030760 File Offset: 0x0002E960
		private Vec3 GetDangerPosition()
		{
			Vec3 vec = Vec3.Zero;
			if (this._missionFightHandler != null)
			{
				IEnumerable<Agent> dangerSources = this._missionFightHandler.GetDangerSources(base.OwnerAgent);
				if (dangerSources.Any<Agent>())
				{
					foreach (Agent agent in dangerSources)
					{
						vec += agent.Position;
					}
					vec /= (float)dangerSources.Count<Agent>();
				}
			}
			return vec;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000307E8 File Offset: 0x0002E9E8
		private bool IsThereDanger()
		{
			return this._missionFightHandler != null && this._missionFightHandler.GetDangerSources(base.OwnerAgent).Any<Agent>();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0003080C File Offset: 0x0002EA0C
		private float GetPathScore(WorldPosition startWorldPos, WorldPosition targetWorldPos)
		{
			float num = 1f;
			NavigationPath navigationPath = new NavigationPath();
			base.Mission.Scene.GetPathBetweenAIFaces(startWorldPos.GetNearestNavMesh(), targetWorldPos.GetNearestNavMesh(), startWorldPos.AsVec2, targetWorldPos.AsVec2, 0f, navigationPath, null);
			Vec2 asVec = this.GetDangerPosition().AsVec2;
			float num2 = MBMath.WrapAngle((asVec - startWorldPos.AsVec2).RotationInRadians);
			float num3 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(MBMath.WrapAngle((navigationPath.Size > 0) ? (navigationPath.PathPoints[0] - startWorldPos.AsVec2).RotationInRadians : (targetWorldPos.AsVec2 - startWorldPos.AsVec2).RotationInRadians), num2)) / 3.1415927f * 1f;
			float num4 = startWorldPos.AsVec2.DistanceSquared(asVec);
			if (navigationPath.Size > 0)
			{
				float num5 = float.MaxValue;
				Vec2 vec = startWorldPos.AsVec2;
				for (int i = 0; i < navigationPath.Size; i++)
				{
					float num6 = Vec2.DistanceToLineSegmentSquared(navigationPath.PathPoints[i], vec, asVec);
					vec = navigationPath.PathPoints[i];
					if (num6 < num5)
					{
						num5 = num6;
					}
				}
				if (num4 > num5 && num5 < 25f)
				{
					num = 1f * (num5 - num4) / 225f;
				}
				else if (num4 > 4f)
				{
					num = 1f * num5 / 225f;
				}
				else
				{
					num = 1f;
				}
			}
			float num7 = 1f * (225f / startWorldPos.AsVec2.DistanceSquared(targetWorldPos.AsVec2));
			return (1f + num3) * (1f + num3) - 2f + num + num7;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000309E4 File Offset: 0x0002EBE4
		private void LookForPlace()
		{
			FleeBehavior.FleeGoalBase fleeGoalBase = new FleeBehavior.FleeCoverTarget(base.Navigator, base.OwnerAgent);
			FleeBehavior.FleeTargetType fleeTargetType = FleeBehavior.FleeTargetType.Cover;
			if (this.IsThereDanger())
			{
				List<ValueTuple<float, Agent>> availableGuardScores = this.GetAvailableGuardScores(5);
				List<ValueTuple<float, Passage>> availablePassageScores = this.GetAvailablePassageScores(10);
				float num = float.MinValue;
				foreach (ValueTuple<float, Passage> valueTuple in availablePassageScores)
				{
					float item = valueTuple.Item1;
					if (item > num)
					{
						num = item;
						fleeTargetType = FleeBehavior.FleeTargetType.Indoor;
						fleeGoalBase = new FleeBehavior.FleePassageTarget(base.Navigator, base.OwnerAgent, valueTuple.Item2);
					}
				}
				foreach (ValueTuple<float, Agent> valueTuple2 in availableGuardScores)
				{
					float item2 = valueTuple2.Item1;
					if (item2 > num)
					{
						num = item2;
						fleeTargetType = FleeBehavior.FleeTargetType.Guard;
						fleeGoalBase = new FleeBehavior.FleeAgentTarget(base.Navigator, base.OwnerAgent, valueTuple2.Item2);
					}
				}
			}
			this._selectedGoal = fleeGoalBase;
			this.SelectedFleeTargetType = fleeTargetType;
			this._state = FleeBehavior.State.Flee;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00030B08 File Offset: 0x0002ED08
		private bool ShouldChangeTarget()
		{
			if (this._selectedFleeTargetType == FleeBehavior.FleeTargetType.Guard)
			{
				WorldPosition worldPosition = (this._selectedGoal as FleeBehavior.FleeAgentTarget).Savior.GetWorldPosition();
				WorldPosition worldPosition2 = base.OwnerAgent.GetWorldPosition();
				return this.GetPathScore(worldPosition2, worldPosition) <= 1f && this.IsThereASafePlaceToEscape();
			}
			if (this._selectedFleeTargetType != FleeBehavior.FleeTargetType.Indoor)
			{
				return true;
			}
			StandingPoint vacantStandingPointForAI = (this._selectedGoal as FleeBehavior.FleePassageTarget).EscapePortal.GetVacantStandingPointForAI(base.OwnerAgent);
			if (vacantStandingPointForAI == null)
			{
				return true;
			}
			WorldPosition worldPosition3 = base.OwnerAgent.GetWorldPosition();
			WorldPosition origin = vacantStandingPointForAI.GetUserFrameForAgent(base.OwnerAgent).Origin;
			return this.GetPathScore(worldPosition3, origin) <= 1f && this.IsThereASafePlaceToEscape();
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00030BBC File Offset: 0x0002EDBC
		private bool IsThereASafePlaceToEscape()
		{
			if (!this.GetAvailablePassageScores(1).Any<ValueTuple<float, Passage>>((ValueTuple<float, Passage> d) => d.Item1 > 1f))
			{
				return this.GetAvailableGuardScores(1).Any<ValueTuple<float, Agent>>((ValueTuple<float, Agent> d) => d.Item1 > 1f);
			}
			return true;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00030C24 File Offset: 0x0002EE24
		private List<ValueTuple<float, Passage>> GetAvailablePassageScores(int maxPaths = 10)
		{
			WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
			List<ValueTuple<float, Passage>> list = new List<ValueTuple<float, Passage>>();
			List<ValueTuple<float, Passage>> list2 = new List<ValueTuple<float, Passage>>();
			List<ValueTuple<WorldPosition, Passage>> list3 = new List<ValueTuple<WorldPosition, Passage>>();
			if (this._missionAgentHandler.TownPassageProps != null)
			{
				foreach (UsableMachine usableMachine in this._missionAgentHandler.TownPassageProps)
				{
					StandingPoint vacantStandingPointForAI = usableMachine.GetVacantStandingPointForAI(base.OwnerAgent);
					Passage passage = usableMachine as Passage;
					if (vacantStandingPointForAI != null && passage != null)
					{
						WorldPosition origin = vacantStandingPointForAI.GetUserFrameForAgent(base.OwnerAgent).Origin;
						list3.Add(new ValueTuple<WorldPosition, Passage>(origin, passage));
					}
				}
			}
			list3 = list3.OrderBy<ValueTuple<WorldPosition, Passage>, float>((ValueTuple<WorldPosition, Passage> a) => base.OwnerAgent.Position.AsVec2.DistanceSquared(a.Item1.AsVec2)).ToList<ValueTuple<WorldPosition, Passage>>();
			foreach (ValueTuple<WorldPosition, Passage> valueTuple in list3)
			{
				WorldPosition item = valueTuple.Item1;
				if (item.IsValid && !(item.GetNearestNavMesh() == UIntPtr.Zero))
				{
					float pathScore = this.GetPathScore(worldPosition, item);
					ValueTuple<float, Passage> valueTuple2 = new ValueTuple<float, Passage>(pathScore, valueTuple.Item2);
					list.Add(valueTuple2);
					if (pathScore > 1f)
					{
						list2.Add(valueTuple2);
					}
					if (list2.Count >= maxPaths)
					{
						break;
					}
				}
			}
			if (list2.Count > 0)
			{
				return list2;
			}
			return list;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00030DA0 File Offset: 0x0002EFA0
		private List<ValueTuple<float, Agent>> GetAvailableGuardScores(int maxGuards = 5)
		{
			WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
			List<ValueTuple<float, Agent>> list = new List<ValueTuple<float, Agent>>();
			List<ValueTuple<float, Agent>> list2 = new List<ValueTuple<float, Agent>>();
			List<Agent> list3 = new List<Agent>();
			foreach (Agent agent in base.OwnerAgent.Team.ActiveAgents)
			{
				CharacterObject characterObject;
				if ((characterObject = agent.Character as CharacterObject) != null && agent.IsAIControlled && agent.CurrentWatchState != Agent.WatchState.Alarmed && (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Guard || characterObject.Occupation == Occupation.PrisonGuard))
				{
					list3.Add(agent);
				}
			}
			list3 = list3.OrderBy<Agent, float>((Agent a) => base.OwnerAgent.Position.DistanceSquared(a.Position)).ToList<Agent>();
			foreach (Agent agent2 in list3)
			{
				WorldPosition worldPosition2 = agent2.GetWorldPosition();
				if (worldPosition2.IsValid)
				{
					float pathScore = this.GetPathScore(worldPosition, worldPosition2);
					ValueTuple<float, Agent> valueTuple = new ValueTuple<float, Agent>(pathScore, agent2);
					list.Add(valueTuple);
					if (pathScore > 1f)
					{
						list2.Add(valueTuple);
					}
					if (list2.Count >= maxGuards)
					{
						break;
					}
				}
			}
			if (list2.Count > 0)
			{
				return list2;
			}
			return list;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00030F0C File Offset: 0x0002F10C
		protected override void OnActivate()
		{
			base.OnActivate();
			this._state = FleeBehavior.State.None;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00030F1C File Offset: 0x0002F11C
		private void Flee()
		{
			if (this._selectedGoal.IsGoalAchievable())
			{
				if (this._selectedGoal.IsGoalAchieved())
				{
					this._selectedGoal.TargetReached();
					FleeBehavior.FleeTargetType selectedFleeTargetType = this.SelectedFleeTargetType;
					if (selectedFleeTargetType == FleeBehavior.FleeTargetType.Guard)
					{
						this._complainToGuardTimer = new BasicMissionTimer();
						this._state = FleeBehavior.State.Complain;
						return;
					}
					if (selectedFleeTargetType == FleeBehavior.FleeTargetType.Cover && this._reconsiderFleeTargetTimer.ElapsedTime > 0.5f)
					{
						this._state = FleeBehavior.State.LookForPlace;
						this._reconsiderFleeTargetTimer.Reset();
						return;
					}
				}
				else
				{
					if (this.SelectedFleeTargetType == FleeBehavior.FleeTargetType.Guard)
					{
						this._selectedGoal.GoToTarget();
					}
					if (this._reconsiderFleeTargetTimer.ElapsedTime > 1f)
					{
						this._reconsiderFleeTargetTimer.Reset();
						if (this.ShouldChangeTarget())
						{
							this._state = FleeBehavior.State.LookForPlace;
							return;
						}
					}
				}
			}
			else
			{
				this._state = FleeBehavior.State.LookForPlace;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00030FE3 File Offset: 0x0002F1E3
		private void BeAfraid()
		{
			this._scareTimer = new BasicMissionTimer();
			this._scareTime = 0.5f + MBRandom.RandomFloat * 0.5f;
			this._state = FleeBehavior.State.Afraid;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0003100E File Offset: 0x0002F20E
		public override string GetDebugInfo()
		{
			return "Flee " + this._state;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00031025 File Offset: 0x0002F225
		public override float GetAvailability(bool isSimulation)
		{
			if (base.Mission.CurrentTime < 3f)
			{
				return 0f;
			}
			if (!MissionFightHandler.IsAgentAggressive(base.OwnerAgent))
			{
				return 0.9f;
			}
			return 0.1f;
		}

		// Token: 0x040003C4 RID: 964
		public const float ScoreThreshold = 1f;

		// Token: 0x040003C5 RID: 965
		public const float DangerDistance = 5f;

		// Token: 0x040003C6 RID: 966
		public const float ImmediateDangerDistance = 2f;

		// Token: 0x040003C7 RID: 967
		public const float DangerDistanceSquared = 25f;

		// Token: 0x040003C8 RID: 968
		public const float ImmediateDangerDistanceSquared = 4f;

		// Token: 0x040003C9 RID: 969
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x040003CA RID: 970
		private readonly MissionFightHandler _missionFightHandler;

		// Token: 0x040003CB RID: 971
		private FleeBehavior.State _state;

		// Token: 0x040003CC RID: 972
		private readonly BasicMissionTimer _reconsiderFleeTargetTimer;

		// Token: 0x040003CD RID: 973
		private const float ReconsiderImmobilizedFleeTargetTime = 0.5f;

		// Token: 0x040003CE RID: 974
		private const float ReconsiderDefaultFleeTargetTime = 1f;

		// Token: 0x040003CF RID: 975
		private FleeBehavior.FleeGoalBase _selectedGoal;

		// Token: 0x040003D0 RID: 976
		private BasicMissionTimer _scareTimer;

		// Token: 0x040003D1 RID: 977
		private float _scareTime;

		// Token: 0x040003D2 RID: 978
		private BasicMissionTimer _complainToGuardTimer;

		// Token: 0x040003D3 RID: 979
		private const float ComplainToGuardTime = 2f;

		// Token: 0x040003D4 RID: 980
		private FleeBehavior.FleeTargetType _selectedFleeTargetType;

		// Token: 0x020001B7 RID: 439
		private abstract class FleeGoalBase
		{
			// Token: 0x06000F71 RID: 3953 RVA: 0x000688DC File Offset: 0x00066ADC
			protected FleeGoalBase(AgentNavigator navigator, Agent ownerAgent)
			{
				this._navigator = navigator;
				this._ownerAgent = ownerAgent;
			}

			// Token: 0x06000F72 RID: 3954
			public abstract void TargetReached();

			// Token: 0x06000F73 RID: 3955
			public abstract void GoToTarget();

			// Token: 0x06000F74 RID: 3956
			public abstract bool IsGoalAchievable();

			// Token: 0x06000F75 RID: 3957
			public abstract bool IsGoalAchieved();

			// Token: 0x040007FA RID: 2042
			protected readonly AgentNavigator _navigator;

			// Token: 0x040007FB RID: 2043
			protected readonly Agent _ownerAgent;
		}

		// Token: 0x020001B8 RID: 440
		private class FleeAgentTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x17000146 RID: 326
			// (get) Token: 0x06000F76 RID: 3958 RVA: 0x000688F2 File Offset: 0x00066AF2
			// (set) Token: 0x06000F77 RID: 3959 RVA: 0x000688FA File Offset: 0x00066AFA
			public Agent Savior { get; private set; }

			// Token: 0x06000F78 RID: 3960 RVA: 0x00068903 File Offset: 0x00066B03
			public FleeAgentTarget(AgentNavigator navigator, Agent ownerAgent, Agent savior)
				: base(navigator, ownerAgent)
			{
				this.Savior = savior;
			}

			// Token: 0x06000F79 RID: 3961 RVA: 0x00068914 File Offset: 0x00066B14
			public override void GoToTarget()
			{
				this._navigator.SetTargetFrame(this.Savior.GetWorldPosition(), this.Savior.Frame.rotation.f.AsVec2.RotationInRadians, 0.2f, 0.02f, Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.NeverSlowDown, false);
			}

			// Token: 0x06000F7A RID: 3962 RVA: 0x0006896C File Offset: 0x00066B6C
			public override bool IsGoalAchievable()
			{
				return this.Savior.GetWorldPosition().GetNearestNavMesh() != UIntPtr.Zero && this._navigator.TargetPosition.IsValid && this.Savior.IsActive() && this.Savior.CurrentWatchState != Agent.WatchState.Alarmed;
			}

			// Token: 0x06000F7B RID: 3963 RVA: 0x000689D0 File Offset: 0x00066BD0
			public override bool IsGoalAchieved()
			{
				return this._navigator.TargetPosition.IsValid && this._navigator.TargetPosition.GetGroundVec3().Distance(this._ownerAgent.Position) <= this._ownerAgent.GetInteractionDistanceToUsable(this.Savior);
			}

			// Token: 0x06000F7C RID: 3964 RVA: 0x00068A30 File Offset: 0x00066C30
			public override void TargetReached()
			{
				this._ownerAgent.SetActionChannel(0, in ActionIndexCache.act_cheer_1, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				this._ownerAgent.SetActionChannel(1, in ActionIndexCache.act_none, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				this._ownerAgent.DisableScriptedMovement();
				this.Savior.DisableScriptedMovement();
				this.Savior.SetLookAgent(this._ownerAgent);
				this._ownerAgent.SetLookAgent(this.Savior);
			}
		}

		// Token: 0x020001B9 RID: 441
		private class FleePassageTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x17000147 RID: 327
			// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00068AE1 File Offset: 0x00066CE1
			// (set) Token: 0x06000F7E RID: 3966 RVA: 0x00068AE9 File Offset: 0x00066CE9
			public Passage EscapePortal { get; private set; }

			// Token: 0x06000F7F RID: 3967 RVA: 0x00068AF2 File Offset: 0x00066CF2
			public FleePassageTarget(AgentNavigator navigator, Agent ownerAgent, Passage escapePortal)
				: base(navigator, ownerAgent)
			{
				this.EscapePortal = escapePortal;
			}

			// Token: 0x06000F80 RID: 3968 RVA: 0x00068B03 File Offset: 0x00066D03
			public override void GoToTarget()
			{
				this._navigator.SetTarget(this.EscapePortal, false, Agent.AIScriptedFrameFlags.None);
			}

			// Token: 0x06000F81 RID: 3969 RVA: 0x00068B18 File Offset: 0x00066D18
			public override bool IsGoalAchievable()
			{
				return this.EscapePortal.GetVacantStandingPointForAI(this._ownerAgent) != null && !this.EscapePortal.IsDestroyed;
			}

			// Token: 0x06000F82 RID: 3970 RVA: 0x00068B40 File Offset: 0x00066D40
			public override bool IsGoalAchieved()
			{
				StandingPoint vacantStandingPointForAI = this.EscapePortal.GetVacantStandingPointForAI(this._ownerAgent);
				return vacantStandingPointForAI != null && vacantStandingPointForAI.IsUsableByAgent(this._ownerAgent);
			}

			// Token: 0x06000F83 RID: 3971 RVA: 0x00068B70 File Offset: 0x00066D70
			public override void TargetReached()
			{
			}
		}

		// Token: 0x020001BA RID: 442
		private class FleePositionTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x17000148 RID: 328
			// (get) Token: 0x06000F84 RID: 3972 RVA: 0x00068B72 File Offset: 0x00066D72
			// (set) Token: 0x06000F85 RID: 3973 RVA: 0x00068B7A File Offset: 0x00066D7A
			public Vec3 Position { get; private set; }

			// Token: 0x06000F86 RID: 3974 RVA: 0x00068B83 File Offset: 0x00066D83
			public FleePositionTarget(AgentNavigator navigator, Agent ownerAgent, Vec3 position)
				: base(navigator, ownerAgent)
			{
				this.Position = position;
			}

			// Token: 0x06000F87 RID: 3975 RVA: 0x00068B94 File Offset: 0x00066D94
			public override void GoToTarget()
			{
			}

			// Token: 0x06000F88 RID: 3976 RVA: 0x00068B98 File Offset: 0x00066D98
			public override bool IsGoalAchievable()
			{
				return this._navigator.TargetPosition.IsValid;
			}

			// Token: 0x06000F89 RID: 3977 RVA: 0x00068BB8 File Offset: 0x00066DB8
			public override bool IsGoalAchieved()
			{
				return this._navigator.TargetPosition.IsValid && this._navigator.IsTargetReached();
			}

			// Token: 0x06000F8A RID: 3978 RVA: 0x00068BE7 File Offset: 0x00066DE7
			public override void TargetReached()
			{
			}
		}

		// Token: 0x020001BB RID: 443
		private class FleeCoverTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x06000F8B RID: 3979 RVA: 0x00068BE9 File Offset: 0x00066DE9
			public FleeCoverTarget(AgentNavigator navigator, Agent ownerAgent)
				: base(navigator, ownerAgent)
			{
			}

			// Token: 0x06000F8C RID: 3980 RVA: 0x00068BF3 File Offset: 0x00066DF3
			public override void GoToTarget()
			{
				this._ownerAgent.DisableScriptedMovement();
			}

			// Token: 0x06000F8D RID: 3981 RVA: 0x00068C00 File Offset: 0x00066E00
			public override bool IsGoalAchievable()
			{
				return true;
			}

			// Token: 0x06000F8E RID: 3982 RVA: 0x00068C03 File Offset: 0x00066E03
			public override bool IsGoalAchieved()
			{
				return true;
			}

			// Token: 0x06000F8F RID: 3983 RVA: 0x00068C06 File Offset: 0x00066E06
			public override void TargetReached()
			{
			}
		}

		// Token: 0x020001BC RID: 444
		private enum State
		{
			// Token: 0x04000800 RID: 2048
			None,
			// Token: 0x04000801 RID: 2049
			Afraid,
			// Token: 0x04000802 RID: 2050
			LookForPlace,
			// Token: 0x04000803 RID: 2051
			Flee,
			// Token: 0x04000804 RID: 2052
			Complain
		}

		// Token: 0x020001BD RID: 445
		private enum FleeTargetType
		{
			// Token: 0x04000806 RID: 2054
			Indoor,
			// Token: 0x04000807 RID: 2055
			Guard,
			// Token: 0x04000808 RID: 2056
			Cover
		}
	}
}
