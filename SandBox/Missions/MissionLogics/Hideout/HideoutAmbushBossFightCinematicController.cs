using System;
using System.Collections.Generic;
using SandBox.Objects.Cinematics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Hideout
{
	// Token: 0x02000093 RID: 147
	public class HideoutAmbushBossFightCinematicController : MissionLogic
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060005C3 RID: 1475 RVA: 0x00025F5C File Offset: 0x0002415C
		// (remove) Token: 0x060005C4 RID: 1476 RVA: 0x00025F94 File Offset: 0x00024194
		public event Action OnCinematicFinished;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060005C5 RID: 1477 RVA: 0x00025FCC File Offset: 0x000241CC
		// (remove) Token: 0x060005C6 RID: 1478 RVA: 0x00026004 File Offset: 0x00024204
		public event Action<HideoutAmbushBossFightCinematicController.HideoutCinematicState> OnCinematicStateChanged;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060005C7 RID: 1479 RVA: 0x0002603C File Offset: 0x0002423C
		// (remove) Token: 0x060005C8 RID: 1480 RVA: 0x00026074 File Offset: 0x00024274
		public event Action<HideoutAmbushBossFightCinematicController.HideoutCinematicState, float> OnCinematicTransition;

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000260A9 File Offset: 0x000242A9
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x000260B1 File Offset: 0x000242B1
		public HideoutAmbushBossFightCinematicController.HideoutCinematicState State { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x000260BA File Offset: 0x000242BA
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x000260C2 File Offset: 0x000242C2
		public bool InStateTransition { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x000260CB File Offset: 0x000242CB
		public bool IsCinematicActive
		{
			get
			{
				return this.State > HideoutAmbushBossFightCinematicController.HideoutCinematicState.None;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000260D6 File Offset: 0x000242D6
		public float CinematicDuration
		{
			get
			{
				return this._cinematicDuration;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000260DE File Offset: 0x000242DE
		public float TransitionDuration
		{
			get
			{
				return this._transitionDuration;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x000260E6 File Offset: 0x000242E6
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Logic;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000260EC File Offset: 0x000242EC
		public HideoutAmbushBossFightCinematicController()
		{
			this.State = HideoutAmbushBossFightCinematicController.HideoutCinematicState.None;
			this.InStateTransition = false;
			this._isBehaviorInit = false;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00026158 File Offset: 0x00024358
		public void StartCinematic(HideoutAmbushBossFightCinematicController.OnInitialFadeOutFinished initialFadeOutFinished, Action cinematicFinishedCallback, float transitionDuration = 0.4f, float stateDuration = 0.2f, float cinematicDuration = 8f, bool forceDismountAgents = false)
		{
			if (this._isBehaviorInit && this.State == HideoutAmbushBossFightCinematicController.HideoutCinematicState.None)
			{
				this.OnCinematicFinished += cinematicFinishedCallback;
				this._initialFadeOutFinished = initialFadeOutFinished;
				this._preCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.InitializeFormations;
				this._postCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase.MoveAgents;
				this._transitionDuration = transitionDuration;
				this._stateDuration = stateDuration;
				this._cinematicDuration = cinematicDuration;
				this._remainingCinematicDuration = this._cinematicDuration;
				this.BeginStateTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState.InitialFadeOut);
				return;
			}
			if (!this._isBehaviorInit)
			{
				Debug.FailedAssert("Hideout cinematic controller is not initialized.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutAmbushBossFightCinematicController.cs", "StartCinematic", 180);
				return;
			}
			if (this.State != HideoutAmbushBossFightCinematicController.HideoutCinematicState.None)
			{
				Debug.FailedAssert("There is already an ongoing cinematic.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutAmbushBossFightCinematicController.cs", "StartCinematic", 184);
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00026200 File Offset: 0x00024400
		public void GetBossStandingEyePosition(out Vec3 eyePosition)
		{
			Agent agent = this._bossAgentInfo.Agent;
			if (((agent != null) ? agent.Monster : null) != null)
			{
				eyePosition = this._bossAgentInfo.InitialFrame.origin + Vec3.Up * (this._bossAgentInfo.Agent.AgentScale * this._bossAgentInfo.Agent.Monster.StandingEyeHeight);
				return;
			}
			eyePosition = Vec3.Zero;
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutAmbushBossFightCinematicController.cs", "GetBossStandingEyePosition", 197);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00026298 File Offset: 0x00024498
		public void GetPlayerStandingEyePosition(out Vec3 eyePosition)
		{
			Agent agent = this._playerAgentInfo.Agent;
			if (((agent != null) ? agent.Monster : null) != null)
			{
				eyePosition = this._playerAgentInfo.InitialFrame.origin + Vec3.Up * (this._playerAgentInfo.Agent.AgentScale * this._playerAgentInfo.Agent.Monster.StandingEyeHeight);
				return;
			}
			eyePosition = Vec3.Zero;
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutAmbushBossFightCinematicController.cs", "GetPlayerStandingEyePosition", 210);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00026330 File Offset: 0x00024530
		public MatrixFrame GetBanditsInitialFrame()
		{
			MatrixFrame matrixFrame;
			this._hideoutBossFightBehavior.GetBanditsInitialFrame(out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0002634C File Offset: 0x0002454C
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

		// Token: 0x060005D7 RID: 1495 RVA: 0x000263A0 File Offset: 0x000245A0
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("hideout_boss_fight");
			this._hideoutBossFightBehavior = ((gameEntity != null) ? gameEntity.GetFirstScriptOfType<HideoutBossFightBehavior>() : null);
			this._isBehaviorInit = gameEntity != null && this._hideoutBossFightBehavior != null;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000263F8 File Offset: 0x000245F8
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
				case HideoutAmbushBossFightCinematicController.HideoutCinematicState.InitialFadeOut:
					if (this.TickInitialFadeOut(dt))
					{
						this.BeginStateTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState.PreCinematic);
						return;
					}
					break;
				case HideoutAmbushBossFightCinematicController.HideoutCinematicState.PreCinematic:
					if (this.TickPreCinematic(dt))
					{
						this.BeginStateTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState.Cinematic);
						return;
					}
					break;
				case HideoutAmbushBossFightCinematicController.HideoutCinematicState.Cinematic:
					if (this.TickCinematic(dt))
					{
						this.BeginStateTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState.PostCinematic);
						return;
					}
					break;
				case HideoutAmbushBossFightCinematicController.HideoutCinematicState.PostCinematic:
					if (this.TickPostCinematic(dt))
					{
						this.BeginStateTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState.Completed);
						return;
					}
					break;
				case HideoutAmbushBossFightCinematicController.HideoutCinematicState.Completed:
				{
					Action onCinematicFinished = this.OnCinematicFinished;
					if (onCinematicFinished != null)
					{
						onCinematicFinished();
					}
					this.OnCinematicFinished = null;
					this.OnCinematicStateChanged = null;
					this.OnCinematicTransition = null;
					this.State = HideoutAmbushBossFightCinematicController.HideoutCinematicState.None;
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000264C0 File Offset: 0x000246C0
		private void TickStateTransition(float dt)
		{
			this._remainingTransitionDuration -= dt;
			if (this._remainingTransitionDuration <= 0f)
			{
				this.InStateTransition = false;
				Action<HideoutAmbushBossFightCinematicController.HideoutCinematicState> onCinematicStateChanged = this.OnCinematicStateChanged;
				if (onCinematicStateChanged != null)
				{
					onCinematicStateChanged(this.State);
				}
				this._remainingStateDuration = this._stateDuration;
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00026514 File Offset: 0x00024714
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
				HideoutAmbushBossFightCinematicController.OnInitialFadeOutFinished initialFadeOutFinished = this._initialFadeOutFinished;
				if (initialFadeOutFinished != null)
				{
					initialFadeOutFinished(ref agent, ref list, ref agent2, ref list2, ref num, ref num2);
				}
				this.ComputeAgentFrames(agent, list, agent2, list2, num, num2);
			}
			return this._remainingStateDuration <= 0f;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00026590 File Offset: 0x00024790
		private bool TickPreCinematic(float dt)
		{
			Scene scene = base.Mission.Scene;
			this._remainingStateDuration -= dt;
			switch (this._preCinematicPhase)
			{
			case HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.InitializeFormations:
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
				foreach (HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo in this._hideoutAgentsInfo)
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
				this._preCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.StopFormations;
				break;
			}
			case HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.StopFormations:
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
				this._preCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.InitializeAgents;
				break;
			case HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.InitializeAgents:
			{
				bool isTeleportingAgents2 = base.Mission.IsTeleportingAgents;
				base.Mission.IsTeleportingAgents = true;
				this._cachedAgentFormations = new List<Formation>();
				foreach (HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo2 in this._hideoutAgentsInfo)
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
				this._preCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.MoveAgents;
				break;
			}
			case HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.MoveAgents:
				foreach (HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo3 in this._hideoutAgentsInfo)
				{
					Agent agent5 = hideoutCinematicAgentInfo3.Agent;
					MatrixFrame targetFrame = hideoutCinematicAgentInfo3.TargetFrame;
					WorldPosition worldPosition4 = new WorldPosition(scene, targetFrame.origin);
					agent5.SetMaximumSpeedLimit(0.65f, false);
					Agent agent6 = agent5;
					Vec2 vec = targetFrame.rotation.f.AsVec2;
					agent6.SetScriptedPositionAndDirection(ref worldPosition4, vec.RotationInRadians, true, Agent.AIScriptedFrameFlags.None);
				}
				this._preCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.Completed;
				break;
			}
			return this._preCinematicPhase == HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase.Completed && this._remainingStateDuration <= 0f;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00026A08 File Offset: 0x00024C08
		private bool TickCinematic(float dt)
		{
			this._remainingCinematicDuration -= dt;
			this._remainingStateDuration -= dt;
			return this._remainingCinematicDuration <= 0f && this._remainingStateDuration <= 0f;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00026A44 File Offset: 0x00024C44
		private bool TickPostCinematic(float dt)
		{
			this._remainingStateDuration -= dt;
			HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase postCinematicPhase = this._postCinematicPhase;
			if (postCinematicPhase != HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase.MoveAgents)
			{
				if (postCinematicPhase == HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase.FinalizeAgents)
				{
					foreach (HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo in this._hideoutAgentsInfo)
					{
						Agent agent = hideoutCinematicAgentInfo.Agent;
						agent.DisableScriptedMovement();
						agent.SetMaximumSpeedLimit(-1f, false);
					}
					this._postCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase.Completed;
				}
			}
			else
			{
				int num = 0;
				foreach (HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo hideoutCinematicAgentInfo2 in this._hideoutAgentsInfo)
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
				this._postCinematicPhase = HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase.FinalizeAgents;
			}
			return this._postCinematicPhase == HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase.Completed && this._remainingStateDuration <= 0f;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00026BBC File Offset: 0x00024DBC
		private void BeginStateTransition(HideoutAmbushBossFightCinematicController.HideoutCinematicState nextState)
		{
			this.State = nextState;
			this._remainingTransitionDuration = this._transitionDuration;
			this.InStateTransition = true;
			Action<HideoutAmbushBossFightCinematicController.HideoutCinematicState, float> onCinematicTransition = this.OnCinematicTransition;
			if (onCinematicTransition == null)
			{
				return;
			}
			onCinematicTransition(this.State, this._remainingTransitionDuration);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00026BF4 File Offset: 0x00024DF4
		private void ComputeAgentFrames(Agent playerAgent, List<Agent> playerCompanions, Agent bossAgent, List<Agent> bossCompanions, float placementPerturbation, float placementAngle)
		{
			this._hideoutAgentsInfo = new List<HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo>();
			MatrixFrame matrixFrame;
			MatrixFrame matrixFrame2;
			this._hideoutBossFightBehavior.GetPlayerFrames(out matrixFrame, out matrixFrame2, placementPerturbation);
			this._playerAgentInfo = new HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo(playerAgent, HideoutAmbushBossFightCinematicController.HideoutAgentType.Player, in matrixFrame, in matrixFrame2);
			this._hideoutAgentsInfo.Add(this._playerAgentInfo);
			List<MatrixFrame> list;
			List<MatrixFrame> list2;
			this.GetAllyFrames(out list, out list2, this._playerAgentInfo.InitialFrame, this._playerAgentInfo.TargetFrame, playerCompanions.Count, placementAngle);
			for (int i = 0; i < playerCompanions.Count; i++)
			{
				matrixFrame = list[i];
				matrixFrame2 = list2[i];
				this._hideoutAgentsInfo.Add(new HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo(playerCompanions[i], HideoutAmbushBossFightCinematicController.HideoutAgentType.Ally, in matrixFrame, in matrixFrame2));
			}
			this._hideoutBossFightBehavior.GetBossFrames(out matrixFrame, out matrixFrame2, placementPerturbation);
			this._bossAgentInfo = new HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo(bossAgent, HideoutAmbushBossFightCinematicController.HideoutAgentType.Boss, in matrixFrame, in matrixFrame2);
			this._hideoutAgentsInfo.Add(this._bossAgentInfo);
			this.GetBanditFrames(out list, out list2, this._bossAgentInfo.InitialFrame, this._bossAgentInfo.TargetFrame, bossCompanions.Count, placementAngle);
			for (int j = 0; j < bossCompanions.Count; j++)
			{
				matrixFrame = list[j];
				matrixFrame2 = list2[j];
				this._hideoutAgentsInfo.Add(new HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo(bossCompanions[j], HideoutAmbushBossFightCinematicController.HideoutAgentType.Bandit, in matrixFrame, in matrixFrame2));
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00026D50 File Offset: 0x00024F50
		public void GetAllyFrames(out List<MatrixFrame> initialFrames, out List<MatrixFrame> targetFrames, MatrixFrame initialPlayerFrame, MatrixFrame targetPlayerFrame, int agentCount, float agentOffsetAngle)
		{
			initialFrames = new List<MatrixFrame>();
			targetFrames = new List<MatrixFrame>();
			MatrixFrame[] array = new MatrixFrame[this.GetSpineTroopCount(agentCount)];
			for (int i = 0; i < array.Length; i++)
			{
				int num = i + 1;
				MatrixFrame[] array2 = array;
				int num2 = i;
				Vec3 vec = new Vec3(initialPlayerFrame.origin.x, initialPlayerFrame.origin.y - 1.3f * (float)num, initialPlayerFrame.origin.z, -1f);
				array2[num2] = new MatrixFrame(in initialPlayerFrame.rotation, in vec);
			}
			for (int j = 0; j < array.Length; j++)
			{
				int num3 = j + 1;
				initialFrames.Add(array[j]);
				int num4 = num3;
				int num5 = num3;
				for (int k = 0; k < num4; k++)
				{
					List<MatrixFrame> list = initialFrames;
					MatrixFrame[] array3 = array;
					int num6 = j;
					Vec3 vec = new Vec3(array[j].origin.x - 1f * (float)(k + 1), array[j].origin.y, array[j].origin.z, -1f);
					list.Add(new MatrixFrame(in array3[num6].rotation, in vec));
				}
				for (int l = 0; l < num5; l++)
				{
					List<MatrixFrame> list2 = initialFrames;
					MatrixFrame[] array4 = array;
					int num7 = j;
					Vec3 vec = new Vec3(array[j].origin.x + 1f * (float)(l + 1), array[j].origin.y, array[j].origin.z, -1f);
					list2.Add(new MatrixFrame(in array4[num7].rotation, in vec));
				}
			}
			foreach (MatrixFrame matrixFrame in initialFrames)
			{
				List<MatrixFrame> list3 = targetFrames;
				Vec3 vec = new Vec3(matrixFrame.origin.x, matrixFrame.origin.y - 0.5f, matrixFrame.origin.z, -1f);
				list3.Add(new MatrixFrame(in matrixFrame.rotation, in vec));
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00026F80 File Offset: 0x00025180
		public int GetSpineTroopCount(int totalTroopCount)
		{
			if (totalTroopCount <= 0)
			{
				return 1;
			}
			int num = -totalTroopCount;
			int num2 = MathF.Ceiling((-2f + MathF.Sqrt((float)(4 - 4 * num))) / 2f);
			if (num2 < 1)
			{
				num2 = 1;
			}
			return num2;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00026FBC File Offset: 0x000251BC
		public void GetBanditFrames(out List<MatrixFrame> initialFrames, out List<MatrixFrame> targetFrames, MatrixFrame initialBossFrame, MatrixFrame targetBossFrame, int agentCount, float agentOffsetAngle)
		{
			initialFrames = new List<MatrixFrame>();
			targetFrames = new List<MatrixFrame>();
			MatrixFrame[] array = new MatrixFrame[this.GetSpineTroopCount(agentCount)];
			for (int i = 0; i < array.Length; i++)
			{
				int num = i + 1;
				MatrixFrame[] array2 = array;
				int num2 = i;
				Vec3 vec = new Vec3(initialBossFrame.origin.x, initialBossFrame.origin.y + 1.2f * (float)num, initialBossFrame.origin.z, -1f);
				array2[num2] = new MatrixFrame(in initialBossFrame.rotation, in vec);
			}
			for (int j = 0; j < array.Length; j++)
			{
				int num3 = j + 1;
				initialFrames.Add(array[j]);
				int num4 = num3;
				int num5 = num3;
				for (int k = 0; k < num4; k++)
				{
					List<MatrixFrame> list = initialFrames;
					MatrixFrame[] array3 = array;
					int num6 = j;
					Vec3 vec = new Vec3(array[j].origin.x - 1f * (float)(k + 1), array[j].origin.y, array[j].origin.z, -1f);
					list.Add(new MatrixFrame(in array3[num6].rotation, in vec));
				}
				for (int l = 0; l < num5; l++)
				{
					List<MatrixFrame> list2 = initialFrames;
					MatrixFrame[] array4 = array;
					int num7 = j;
					Vec3 vec = new Vec3(array[j].origin.x + 1f * (float)(l + 1), array[j].origin.y, array[j].origin.z, -1f);
					list2.Add(new MatrixFrame(in array4[num7].rotation, in vec));
				}
			}
			foreach (MatrixFrame matrixFrame in initialFrames)
			{
				List<MatrixFrame> list3 = targetFrames;
				Vec3 vec = new Vec3(matrixFrame.origin.x, matrixFrame.origin.y - 0.5f, matrixFrame.origin.z, -1f);
				list3.Add(new MatrixFrame(in matrixFrame.rotation, in vec));
			}
		}

		// Token: 0x040002F6 RID: 758
		private const float AgentTargetProximityThreshold = 0.5f;

		// Token: 0x040002F7 RID: 759
		private const float AgentMaxSpeedCinematicOverride = 0.65f;

		// Token: 0x040002F8 RID: 760
		public const string HideoutSceneEntityTag = "hideout_boss_fight";

		// Token: 0x040002F9 RID: 761
		public const float DefaultTransitionDuration = 0.4f;

		// Token: 0x040002FA RID: 762
		public const float DefaultStateDuration = 0.2f;

		// Token: 0x040002FB RID: 763
		public const float DefaultCinematicDuration = 8f;

		// Token: 0x040002FC RID: 764
		public const float DefaultPlacementPerturbation = 0.25f;

		// Token: 0x040002FD RID: 765
		public const float DefaultPlacementAngle = 0.20943952f;

		// Token: 0x040002FE RID: 766
		private HideoutAmbushBossFightCinematicController.OnInitialFadeOutFinished _initialFadeOutFinished;

		// Token: 0x040002FF RID: 767
		private float _cinematicDuration = 8f;

		// Token: 0x04000300 RID: 768
		private float _stateDuration = 0.2f;

		// Token: 0x04000301 RID: 769
		private float _transitionDuration = 0.4f;

		// Token: 0x04000302 RID: 770
		private float _remainingCinematicDuration = 8f;

		// Token: 0x04000303 RID: 771
		private float _remainingStateDuration = 0.2f;

		// Token: 0x04000304 RID: 772
		private float _remainingTransitionDuration = 0.4f;

		// Token: 0x04000305 RID: 773
		private List<Formation> _cachedAgentFormations;

		// Token: 0x04000306 RID: 774
		private List<HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo> _hideoutAgentsInfo;

		// Token: 0x04000307 RID: 775
		private HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo _bossAgentInfo;

		// Token: 0x04000308 RID: 776
		private HideoutAmbushBossFightCinematicController.HideoutCinematicAgentInfo _playerAgentInfo;

		// Token: 0x04000309 RID: 777
		private bool _isBehaviorInit;

		// Token: 0x0400030A RID: 778
		private HideoutAmbushBossFightCinematicController.HideoutPreCinematicPhase _preCinematicPhase;

		// Token: 0x0400030B RID: 779
		private HideoutAmbushBossFightCinematicController.HideoutPostCinematicPhase _postCinematicPhase;

		// Token: 0x0400030C RID: 780
		private HideoutBossFightBehavior _hideoutBossFightBehavior;

		// Token: 0x0200019B RID: 411
		// (Invoke) Token: 0x06000F34 RID: 3892
		public delegate void OnInitialFadeOutFinished(ref Agent playerAgent, ref List<Agent> playerCompanions, ref Agent bossAgent, ref List<Agent> bossCompanions, ref float placementPerturbation, ref float placementAngle);

		// Token: 0x0200019C RID: 412
		// (Invoke) Token: 0x06000F38 RID: 3896
		public delegate void OnHideoutCinematicFinished();

		// Token: 0x0200019D RID: 413
		public readonly struct HideoutCinematicAgentInfo
		{
			// Token: 0x06000F3B RID: 3899 RVA: 0x00067F9B File Offset: 0x0006619B
			public HideoutCinematicAgentInfo(Agent agent, HideoutAmbushBossFightCinematicController.HideoutAgentType type, in MatrixFrame initialFrame, in MatrixFrame targetFrame)
			{
				this.Agent = agent;
				this.InitialFrame = initialFrame;
				this.TargetFrame = targetFrame;
				this.Type = type;
			}

			// Token: 0x06000F3C RID: 3900 RVA: 0x00067FC4 File Offset: 0x000661C4
			public bool HasReachedTarget(float proximityThreshold = 0.5f)
			{
				return this.Agent.Position.Distance(this.TargetFrame.origin) <= proximityThreshold;
			}

			// Token: 0x0400078D RID: 1933
			public readonly Agent Agent;

			// Token: 0x0400078E RID: 1934
			public readonly MatrixFrame InitialFrame;

			// Token: 0x0400078F RID: 1935
			public readonly MatrixFrame TargetFrame;

			// Token: 0x04000790 RID: 1936
			public readonly HideoutAmbushBossFightCinematicController.HideoutAgentType Type;
		}

		// Token: 0x0200019E RID: 414
		public enum HideoutCinematicState
		{
			// Token: 0x04000792 RID: 1938
			None,
			// Token: 0x04000793 RID: 1939
			InitialFadeOut,
			// Token: 0x04000794 RID: 1940
			PreCinematic,
			// Token: 0x04000795 RID: 1941
			Cinematic,
			// Token: 0x04000796 RID: 1942
			PostCinematic,
			// Token: 0x04000797 RID: 1943
			Completed
		}

		// Token: 0x0200019F RID: 415
		public enum HideoutAgentType
		{
			// Token: 0x04000799 RID: 1945
			Player,
			// Token: 0x0400079A RID: 1946
			Boss,
			// Token: 0x0400079B RID: 1947
			Ally,
			// Token: 0x0400079C RID: 1948
			Bandit
		}

		// Token: 0x020001A0 RID: 416
		public enum HideoutPreCinematicPhase
		{
			// Token: 0x0400079E RID: 1950
			NotStarted,
			// Token: 0x0400079F RID: 1951
			InitializeFormations,
			// Token: 0x040007A0 RID: 1952
			StopFormations,
			// Token: 0x040007A1 RID: 1953
			InitializeAgents,
			// Token: 0x040007A2 RID: 1954
			MoveAgents,
			// Token: 0x040007A3 RID: 1955
			Completed
		}

		// Token: 0x020001A1 RID: 417
		public enum HideoutPostCinematicPhase
		{
			// Token: 0x040007A5 RID: 1957
			NotStarted,
			// Token: 0x040007A6 RID: 1958
			MoveAgents,
			// Token: 0x040007A7 RID: 1959
			FinalizeAgents,
			// Token: 0x040007A8 RID: 1960
			Completed
		}
	}
}
