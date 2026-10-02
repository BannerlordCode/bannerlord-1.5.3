using System;
using System.Collections.Generic;
using SandBox.Objects.Cinematics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Hideout
{
	// Token: 0x02000095 RID: 149
	public class HideoutCinematicController : MissionLogic
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000613 RID: 1555 RVA: 0x00029140 File Offset: 0x00027340
		// (remove) Token: 0x06000614 RID: 1556 RVA: 0x00029178 File Offset: 0x00027378
		public event Action OnCinematicFinished;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000615 RID: 1557 RVA: 0x000291B0 File Offset: 0x000273B0
		// (remove) Token: 0x06000616 RID: 1558 RVA: 0x000291E8 File Offset: 0x000273E8
		public event Action<HideoutCinematicController.HideoutCinematicState> OnCinematicStateChanged;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000617 RID: 1559 RVA: 0x00029220 File Offset: 0x00027420
		// (remove) Token: 0x06000618 RID: 1560 RVA: 0x00029258 File Offset: 0x00027458
		public event Action<HideoutCinematicController.HideoutCinematicState, float> OnCinematicTransition;

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0002928D File Offset: 0x0002748D
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x00029295 File Offset: 0x00027495
		public HideoutCinematicController.HideoutCinematicState State { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0002929E File Offset: 0x0002749E
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x000292A6 File Offset: 0x000274A6
		public bool InStateTransition { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000292AF File Offset: 0x000274AF
		public bool IsCinematicActive
		{
			get
			{
				return this.State > HideoutCinematicController.HideoutCinematicState.None;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000292BA File Offset: 0x000274BA
		public float CinematicDuration
		{
			get
			{
				return this._cinematicDuration;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000292C2 File Offset: 0x000274C2
		public float TransitionDuration
		{
			get
			{
				return this._transitionDuration;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000292CA File Offset: 0x000274CA
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Logic;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000292D0 File Offset: 0x000274D0
		public HideoutCinematicController()
		{
			this.State = HideoutCinematicController.HideoutCinematicState.None;
			this.InStateTransition = false;
			this._isBehaviorInit = false;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0002933C File Offset: 0x0002753C
		public void StartCinematic(HideoutCinematicController.OnInitialFadeOutFinished initialFadeOutFinished, Action cinematicFinishedCallback, float transitionDuration = 0.4f, float stateDuration = 0.2f, float cinematicDuration = 8f, bool forceDismountAgents = false)
		{
			if (this._isBehaviorInit && this.State == HideoutCinematicController.HideoutCinematicState.None)
			{
				this.OnCinematicFinished += cinematicFinishedCallback;
				this._initialFadeOutFinished = initialFadeOutFinished;
				this._preCinematicPhase = HideoutCinematicController.HideoutPreCinematicPhase.InitializeFormations;
				this._postCinematicPhase = HideoutCinematicController.HideoutPostCinematicPhase.MoveAgents;
				this._transitionDuration = transitionDuration;
				this._stateDuration = stateDuration;
				this._cinematicDuration = cinematicDuration;
				this._remainingCinematicDuration = this._cinematicDuration;
				this.BeginStateTransition(HideoutCinematicController.HideoutCinematicState.InitialFadeOut);
				return;
			}
			if (!this._isBehaviorInit)
			{
				Debug.FailedAssert("Hideout cinematic controller is not initialized.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutCinematicController.cs", "StartCinematic", 178);
				return;
			}
			if (this.State != HideoutCinematicController.HideoutCinematicState.None)
			{
				Debug.FailedAssert("There is already an ongoing cinematic.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutCinematicController.cs", "StartCinematic", 182);
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000293E4 File Offset: 0x000275E4
		public void GetBossStandingEyePosition(out Vec3 eyePosition)
		{
			Agent agent = this._bossAgentInfo.Agent;
			if (((agent != null) ? agent.Monster : null) != null)
			{
				eyePosition = this._bossAgentInfo.InitialFrame.origin + Vec3.Up * (this._bossAgentInfo.Agent.AgentScale * this._bossAgentInfo.Agent.Monster.StandingEyeHeight);
				return;
			}
			eyePosition = Vec3.Zero;
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutCinematicController.cs", "GetBossStandingEyePosition", 195);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0002947C File Offset: 0x0002767C
		public void GetPlayerStandingEyePosition(out Vec3 eyePosition)
		{
			Agent agent = this._playerAgentInfo.Agent;
			if (((agent != null) ? agent.Monster : null) != null)
			{
				eyePosition = this._playerAgentInfo.InitialFrame.origin + Vec3.Up * (this._playerAgentInfo.Agent.AgentScale * this._playerAgentInfo.Agent.Monster.StandingEyeHeight);
				return;
			}
			eyePosition = Vec3.Zero;
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutCinematicController.cs", "GetPlayerStandingEyePosition", 208);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00029514 File Offset: 0x00027714
		public MatrixFrame GetBanditsInitialFrame()
		{
			MatrixFrame matrixFrame;
			this._hideoutBossFightBehavior.GetBanditsInitialFrame(out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00029530 File Offset: 0x00027730
		public void GetScenePrefabParameters(out float innerRadius, out float outerRadius, out float walkDistance)
		{
			innerRadius = 0f;
			outerRadius = 0f;
			walkDistance = 0f;
			if (this._hideoutBossFightBehavior != null)
			{
				innerRadius = this._hideoutBossFightBehavior.InnerRadius;
				outerRadius = this._hideoutBossFightBehavior.OuterRadius;
				walkDistance = this._hideoutBossFightBehavior.WalkDistance;
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00029584 File Offset: 0x00027784
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("hideout_boss_fight");
			this._hideoutBossFightBehavior = ((gameEntity != null) ? gameEntity.GetFirstScriptOfType<HideoutBossFightBehavior>() : null);
			this._isBehaviorInit = gameEntity != null && this._hideoutBossFightBehavior != null;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000295DC File Offset: 0x000277DC
		public override void OnMissionTick(float dt)
		{
			if (this._isBehaviorInit && this.IsCinematicActive)
			{
				if (this.InStateTransition)
				{
					this.TickStateTransition(dt);
					return;
				}
				switch (this.State)
				{
				case HideoutCinematicController.HideoutCinematicState.InitialFadeOut:
					if (this.TickInitialFadeOut(dt))
					{
						this.BeginStateTransition(HideoutCinematicController.HideoutCinematicState.PreCinematic);
						return;
					}
					break;
				case HideoutCinematicController.HideoutCinematicState.PreCinematic:
					if (this.TickPreCinematic(dt))
					{
						this.BeginStateTransition(HideoutCinematicController.HideoutCinematicState.Cinematic);
						return;
					}
					break;
				case HideoutCinematicController.HideoutCinematicState.Cinematic:
					if (this.TickCinematic(dt))
					{
						this.BeginStateTransition(HideoutCinematicController.HideoutCinematicState.PostCinematic);
						return;
					}
					break;
				case HideoutCinematicController.HideoutCinematicState.PostCinematic:
					if (this.TickPostCinematic(dt))
					{
						this.BeginStateTransition(HideoutCinematicController.HideoutCinematicState.Completed);
						return;
					}
					break;
				case HideoutCinematicController.HideoutCinematicState.Completed:
				{
					Action onCinematicFinished = this.OnCinematicFinished;
					if (onCinematicFinished != null)
					{
						onCinematicFinished();
					}
					this.OnCinematicFinished = null;
					this.OnCinematicStateChanged = null;
					this.OnCinematicTransition = null;
					this.State = HideoutCinematicController.HideoutCinematicState.None;
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000296A4 File Offset: 0x000278A4
		private void TickStateTransition(float dt)
		{
			this._remainingTransitionDuration -= dt;
			if (this._remainingTransitionDuration <= 0f)
			{
				this.InStateTransition = false;
				Action<HideoutCinematicController.HideoutCinematicState> onCinematicStateChanged = this.OnCinematicStateChanged;
				if (onCinematicStateChanged != null)
				{
					onCinematicStateChanged(this.State);
				}
				this._remainingStateDuration = this._stateDuration;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000296F8 File Offset: 0x000278F8
		private bool TickInitialFadeOut(float dt)
		{
			this._remainingStateDuration -= dt;
			if (this._remainingStateDuration <= 0f)
			{
				Agent agent = null;
				Agent agent2 = null;
				List<Agent> list = null;
				List<Agent> list2 = null;
				float num = 0.25f;
				float num2 = 0.20943952f;
				HideoutCinematicController.OnInitialFadeOutFinished initialFadeOutFinished = this._initialFadeOutFinished;
				if (initialFadeOutFinished != null)
				{
					initialFadeOutFinished(ref agent, ref list, ref agent2, ref list2, ref num, ref num2);
				}
				this.ComputeAgentFrames(agent, list, agent2, list2, num, num2);
			}
			return this._remainingStateDuration <= 0f;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00029774 File Offset: 0x00027974
		private bool TickPreCinematic(float dt)
		{
			Scene scene = base.Mission.Scene;
			this._remainingStateDuration -= dt;
			switch (this._preCinematicPhase)
			{
			case HideoutCinematicController.HideoutPreCinematicPhase.InitializeFormations:
			{
				this._playerAgentInfo.Agent.Controller = AgentControllerType.AI;
				bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
				base.Mission.IsTeleportingAgents = true;
				MatrixFrame matrixFrame;
				this._hideoutBossFightBehavior.GetAlliesInitialFrame(out matrixFrame);
				foreach (Formation formation in base.Mission.Teams.Attacker.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						WorldPosition worldPosition = new WorldPosition(scene, matrixFrame.origin);
						formation.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
					}
				}
				MatrixFrame matrixFrame2;
				this._hideoutBossFightBehavior.GetBanditsInitialFrame(out matrixFrame2);
				foreach (Formation formation2 in base.Mission.Teams.Defender.FormationsIncludingEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						WorldPosition worldPosition2 = new WorldPosition(scene, matrixFrame2.origin);
						formation2.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition2));
					}
				}
				foreach (HideoutCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo in this._hideoutAgentsInfo)
				{
					Agent agent = hideoutCinematicAgentInfo.Agent;
					Vec3 f = hideoutCinematicAgentInfo.InitialFrame.rotation.f;
					agent.LookDirection = f;
					Agent agent2 = agent;
					Vec2 vec = f.AsVec2;
					vec = vec.Normalized();
					agent2.SetMovementDirection(in vec);
				}
				base.Mission.IsTeleportingAgents = isTeleportingAgents;
				this._preCinematicPhase = HideoutCinematicController.HideoutPreCinematicPhase.StopFormations;
				break;
			}
			case HideoutCinematicController.HideoutPreCinematicPhase.StopFormations:
				foreach (Formation formation3 in base.Mission.Teams.Attacker.FormationsIncludingEmpty)
				{
					if (formation3.CountOfUnits > 0)
					{
						formation3.SetMovementOrder(MovementOrder.MovementOrderStop);
					}
				}
				foreach (Formation formation4 in base.Mission.Teams.Defender.FormationsIncludingEmpty)
				{
					if (formation4.CountOfUnits > 0)
					{
						formation4.SetMovementOrder(MovementOrder.MovementOrderStop);
					}
				}
				this._preCinematicPhase = HideoutCinematicController.HideoutPreCinematicPhase.InitializeAgents;
				break;
			case HideoutCinematicController.HideoutPreCinematicPhase.InitializeAgents:
			{
				bool isTeleportingAgents2 = base.Mission.IsTeleportingAgents;
				base.Mission.IsTeleportingAgents = true;
				this._cachedAgentFormations = new List<Formation>();
				foreach (HideoutCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo2 in this._hideoutAgentsInfo)
				{
					Agent agent3 = hideoutCinematicAgentInfo2.Agent;
					this._cachedAgentFormations.Add(agent3.Formation);
					agent3.Formation = null;
					MatrixFrame initialFrame = hideoutCinematicAgentInfo2.InitialFrame;
					WorldPosition worldPosition3 = new WorldPosition(scene, initialFrame.origin);
					Vec3 f2 = initialFrame.rotation.f;
					agent3.TeleportToPosition(worldPosition3.GetGroundVec3());
					agent3.LookDirection = f2;
					Agent agent4 = agent3;
					Vec2 vec = f2.AsVec2;
					vec = vec.Normalized();
					agent4.SetMovementDirection(in vec);
				}
				base.Mission.IsTeleportingAgents = isTeleportingAgents2;
				this._preCinematicPhase = HideoutCinematicController.HideoutPreCinematicPhase.MoveAgents;
				break;
			}
			case HideoutCinematicController.HideoutPreCinematicPhase.MoveAgents:
				foreach (HideoutCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo3 in this._hideoutAgentsInfo)
				{
					Agent agent5 = hideoutCinematicAgentInfo3.Agent;
					MatrixFrame targetFrame = hideoutCinematicAgentInfo3.TargetFrame;
					WorldPosition worldPosition4 = new WorldPosition(scene, targetFrame.origin);
					agent5.SetMaximumSpeedLimit(0.65f, false);
					Agent agent6 = agent5;
					Vec2 vec = targetFrame.rotation.f.AsVec2;
					agent6.SetScriptedPositionAndDirection(ref worldPosition4, vec.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
				}
				this._preCinematicPhase = HideoutCinematicController.HideoutPreCinematicPhase.Completed;
				break;
			}
			return this._preCinematicPhase == HideoutCinematicController.HideoutPreCinematicPhase.Completed && this._remainingStateDuration <= 0f;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00029BEC File Offset: 0x00027DEC
		private bool TickCinematic(float dt)
		{
			this._remainingCinematicDuration -= dt;
			this._remainingStateDuration -= dt;
			return this._remainingCinematicDuration <= 0f && this._remainingStateDuration <= 0f;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00029C28 File Offset: 0x00027E28
		private bool TickPostCinematic(float dt)
		{
			this._remainingStateDuration -= dt;
			HideoutCinematicController.HideoutPostCinematicPhase postCinematicPhase = this._postCinematicPhase;
			if (postCinematicPhase != HideoutCinematicController.HideoutPostCinematicPhase.MoveAgents)
			{
				if (postCinematicPhase == HideoutCinematicController.HideoutPostCinematicPhase.FinalizeAgents)
				{
					foreach (HideoutCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo in this._hideoutAgentsInfo)
					{
						Agent agent = hideoutCinematicAgentInfo.Agent;
						agent.DisableScriptedMovement();
						agent.SetMaximumSpeedLimit(-1f, false);
					}
					this._postCinematicPhase = HideoutCinematicController.HideoutPostCinematicPhase.Completed;
				}
			}
			else
			{
				int num = 0;
				foreach (HideoutCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo2 in this._hideoutAgentsInfo)
				{
					Agent agent2 = hideoutCinematicAgentInfo2.Agent;
					if (!hideoutCinematicAgentInfo2.HasReachedTarget(0.5f))
					{
						MatrixFrame targetFrame = hideoutCinematicAgentInfo2.TargetFrame;
						WorldPosition worldPosition = new WorldPosition(base.Mission.Scene, targetFrame.origin);
						agent2.TeleportToPosition(worldPosition.GetGroundVec3());
						Agent agent3 = agent2;
						Vec2 vec = targetFrame.rotation.f.AsVec2;
						vec = vec.Normalized();
						agent3.SetMovementDirection(in vec);
					}
					agent2.Formation = this._cachedAgentFormations[num];
					num++;
				}
				this._postCinematicPhase = HideoutCinematicController.HideoutPostCinematicPhase.FinalizeAgents;
			}
			return this._postCinematicPhase == HideoutCinematicController.HideoutPostCinematicPhase.Completed && this._remainingStateDuration <= 0f;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00029DA0 File Offset: 0x00027FA0
		private void BeginStateTransition(HideoutCinematicController.HideoutCinematicState nextState)
		{
			this.State = nextState;
			this._remainingTransitionDuration = this._transitionDuration;
			this.InStateTransition = true;
			Action<HideoutCinematicController.HideoutCinematicState, float> onCinematicTransition = this.OnCinematicTransition;
			if (onCinematicTransition == null)
			{
				return;
			}
			onCinematicTransition(this.State, this._remainingTransitionDuration);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00029DD8 File Offset: 0x00027FD8
		private bool CheckNavMeshValidity(ref Vec3 initial, ref Vec3 target)
		{
			Scene scene = base.Mission.Scene;
			bool flag = false;
			bool flag2 = scene.GetNavigationMeshForPosition(in initial) != UIntPtr.Zero;
			bool flag3 = scene.GetNavigationMeshForPosition(in target) != UIntPtr.Zero;
			if (flag2 && flag3)
			{
				WorldPosition worldPosition = new WorldPosition(scene, initial);
				WorldPosition worldPosition2 = new WorldPosition(scene, target);
				flag = scene.DoesPathExistBetweenPositions(worldPosition, worldPosition2);
			}
			return flag;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00029E44 File Offset: 0x00028044
		private void ComputeAgentFrames(Agent playerAgent, List<Agent> playerCompanions, Agent bossAgent, List<Agent> bossCompanions, float placementPerturbation, float placementAngle)
		{
			this._hideoutAgentsInfo = new List<HideoutCinematicController.HideoutCinematicAgentInfo>();
			MatrixFrame matrixFrame;
			MatrixFrame matrixFrame2;
			this._hideoutBossFightBehavior.GetPlayerFrames(out matrixFrame, out matrixFrame2, placementPerturbation);
			this._playerAgentInfo = new HideoutCinematicController.HideoutCinematicAgentInfo(playerAgent, HideoutCinematicController.HideoutAgentType.Player, in matrixFrame, in matrixFrame2);
			this._hideoutAgentsInfo.Add(this._playerAgentInfo);
			List<MatrixFrame> list;
			List<MatrixFrame> list2;
			this._hideoutBossFightBehavior.GetAllyFrames(out list, out list2, playerCompanions.Count, placementAngle, placementPerturbation);
			for (int i = 0; i < playerCompanions.Count; i++)
			{
				matrixFrame = list[i];
				matrixFrame2 = list2[i];
				this._hideoutAgentsInfo.Add(new HideoutCinematicController.HideoutCinematicAgentInfo(playerCompanions[i], HideoutCinematicController.HideoutAgentType.Ally, in matrixFrame, in matrixFrame2));
			}
			this._hideoutBossFightBehavior.GetBossFrames(out matrixFrame, out matrixFrame2, placementPerturbation);
			this._bossAgentInfo = new HideoutCinematicController.HideoutCinematicAgentInfo(bossAgent, HideoutCinematicController.HideoutAgentType.Boss, in matrixFrame, in matrixFrame2);
			this._hideoutAgentsInfo.Add(this._bossAgentInfo);
			this._hideoutBossFightBehavior.GetBanditFrames(out list, out list2, bossCompanions.Count, placementAngle, placementPerturbation);
			for (int j = 0; j < bossCompanions.Count; j++)
			{
				matrixFrame = list[j];
				matrixFrame2 = list2[j];
				this._hideoutAgentsInfo.Add(new HideoutCinematicController.HideoutCinematicAgentInfo(bossCompanions[j], HideoutCinematicController.HideoutAgentType.Bandit, in matrixFrame, in matrixFrame2));
			}
		}

		// Token: 0x04000333 RID: 819
		private const float AgentTargetProximityThreshold = 0.5f;

		// Token: 0x04000334 RID: 820
		private const float AgentMaxSpeedCinematicOverride = 0.65f;

		// Token: 0x04000335 RID: 821
		public const string HideoutSceneEntityTag = "hideout_boss_fight";

		// Token: 0x04000336 RID: 822
		public const float DefaultTransitionDuration = 0.4f;

		// Token: 0x04000337 RID: 823
		public const float DefaultStateDuration = 0.2f;

		// Token: 0x04000338 RID: 824
		public const float DefaultCinematicDuration = 8f;

		// Token: 0x04000339 RID: 825
		public const float DefaultPlacementPerturbation = 0.25f;

		// Token: 0x0400033A RID: 826
		public const float DefaultPlacementAngle = 0.20943952f;

		// Token: 0x0400033B RID: 827
		private HideoutCinematicController.OnInitialFadeOutFinished _initialFadeOutFinished;

		// Token: 0x0400033C RID: 828
		private float _cinematicDuration = 8f;

		// Token: 0x0400033D RID: 829
		private float _stateDuration = 0.2f;

		// Token: 0x0400033E RID: 830
		private float _transitionDuration = 0.4f;

		// Token: 0x0400033F RID: 831
		private float _remainingCinematicDuration = 8f;

		// Token: 0x04000340 RID: 832
		private float _remainingStateDuration = 0.2f;

		// Token: 0x04000341 RID: 833
		private float _remainingTransitionDuration = 0.4f;

		// Token: 0x04000342 RID: 834
		private List<Formation> _cachedAgentFormations;

		// Token: 0x04000343 RID: 835
		private List<HideoutCinematicController.HideoutCinematicAgentInfo> _hideoutAgentsInfo;

		// Token: 0x04000344 RID: 836
		private HideoutCinematicController.HideoutCinematicAgentInfo _bossAgentInfo;

		// Token: 0x04000345 RID: 837
		private HideoutCinematicController.HideoutCinematicAgentInfo _playerAgentInfo;

		// Token: 0x04000346 RID: 838
		private bool _isBehaviorInit;

		// Token: 0x04000347 RID: 839
		private HideoutCinematicController.HideoutPreCinematicPhase _preCinematicPhase;

		// Token: 0x04000348 RID: 840
		private HideoutCinematicController.HideoutPostCinematicPhase _postCinematicPhase;

		// Token: 0x04000349 RID: 841
		private HideoutBossFightBehavior _hideoutBossFightBehavior;

		// Token: 0x020001A5 RID: 421
		// (Invoke) Token: 0x06000F42 RID: 3906
		public delegate void OnInitialFadeOutFinished(ref Agent playerAgent, ref List<Agent> playerCompanions, ref Agent bossAgent, ref List<Agent> bossCompanions, ref float placementPerturbation, ref float placementAngle);

		// Token: 0x020001A6 RID: 422
		// (Invoke) Token: 0x06000F46 RID: 3910
		public delegate void OnHideoutCinematicFinished();

		// Token: 0x020001A7 RID: 423
		public readonly struct HideoutCinematicAgentInfo
		{
			// Token: 0x06000F49 RID: 3913 RVA: 0x00068027 File Offset: 0x00066227
			public HideoutCinematicAgentInfo(Agent agent, HideoutCinematicController.HideoutAgentType type, in MatrixFrame initialFrame, in MatrixFrame targetFrame)
			{
				this.Agent = agent;
				this.InitialFrame = initialFrame;
				this.TargetFrame = targetFrame;
				this.Type = type;
			}

			// Token: 0x06000F4A RID: 3914 RVA: 0x00068050 File Offset: 0x00066250
			public bool HasReachedTarget(float proximityThreshold = 0.5f)
			{
				return this.Agent.Position.Distance(this.TargetFrame.origin) <= proximityThreshold;
			}

			// Token: 0x040007B7 RID: 1975
			public readonly Agent Agent;

			// Token: 0x040007B8 RID: 1976
			public readonly MatrixFrame InitialFrame;

			// Token: 0x040007B9 RID: 1977
			public readonly MatrixFrame TargetFrame;

			// Token: 0x040007BA RID: 1978
			public readonly HideoutCinematicController.HideoutAgentType Type;
		}

		// Token: 0x020001A8 RID: 424
		public enum HideoutCinematicState
		{
			// Token: 0x040007BC RID: 1980
			None,
			// Token: 0x040007BD RID: 1981
			InitialFadeOut,
			// Token: 0x040007BE RID: 1982
			PreCinematic,
			// Token: 0x040007BF RID: 1983
			Cinematic,
			// Token: 0x040007C0 RID: 1984
			PostCinematic,
			// Token: 0x040007C1 RID: 1985
			Completed
		}

		// Token: 0x020001A9 RID: 425
		public enum HideoutAgentType
		{
			// Token: 0x040007C3 RID: 1987
			Player,
			// Token: 0x040007C4 RID: 1988
			Boss,
			// Token: 0x040007C5 RID: 1989
			Ally,
			// Token: 0x040007C6 RID: 1990
			Bandit
		}

		// Token: 0x020001AA RID: 426
		public enum HideoutPreCinematicPhase
		{
			// Token: 0x040007C8 RID: 1992
			NotStarted,
			// Token: 0x040007C9 RID: 1993
			InitializeFormations,
			// Token: 0x040007CA RID: 1994
			StopFormations,
			// Token: 0x040007CB RID: 1995
			InitializeAgents,
			// Token: 0x040007CC RID: 1996
			MoveAgents,
			// Token: 0x040007CD RID: 1997
			Completed
		}

		// Token: 0x020001AB RID: 427
		public enum HideoutPostCinematicPhase
		{
			// Token: 0x040007CF RID: 1999
			NotStarted,
			// Token: 0x040007D0 RID: 2000
			MoveAgents,
			// Token: 0x040007D1 RID: 2001
			FinalizeAgents,
			// Token: 0x040007D2 RID: 2002
			Completed
		}
	}
}
