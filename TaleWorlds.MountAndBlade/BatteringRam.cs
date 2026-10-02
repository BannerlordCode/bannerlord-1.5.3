using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034A RID: 842
	public class BatteringRam : SiegeWeapon, IPathHolder, IPrimarySiegeWeapon, IMoveableSiegeWeapon, ISpawnable
	{
		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002F4F RID: 12111 RVA: 0x000B7E0B File Offset: 0x000B600B
		// (set) Token: 0x06002F50 RID: 12112 RVA: 0x000B7E13 File Offset: 0x000B6013
		public SiegeWeaponMovementComponent MovementComponent { get; private set; }

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002F51 RID: 12113 RVA: 0x000B7E1C File Offset: 0x000B601C
		public FormationAI.BehaviorSide WeaponSide
		{
			get
			{
				return this._weaponSide;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002F52 RID: 12114 RVA: 0x000B7E24 File Offset: 0x000B6024
		public string PathEntity
		{
			get
			{
				return this._pathEntityName;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002F53 RID: 12115 RVA: 0x000B7E2C File Offset: 0x000B602C
		public bool EditorGhostEntityMove
		{
			get
			{
				return this.GhostEntityMove;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002F54 RID: 12116 RVA: 0x000B7E34 File Offset: 0x000B6034
		// (set) Token: 0x06002F55 RID: 12117 RVA: 0x000B7E3C File Offset: 0x000B603C
		public BatteringRam.RamState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					this._state = value;
				}
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002F56 RID: 12118 RVA: 0x000B7E4E File Offset: 0x000B604E
		public MissionObject TargetCastlePosition
		{
			get
			{
				return this._gate;
			}
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000B7E56 File Offset: 0x000B6056
		public bool HasCompletedAction()
		{
			return this._gate == null || this._gate.IsDestroyed || (this._gate.State == CastleGate.GateState.Open && this.HasArrivedAtTarget);
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002F58 RID: 12120 RVA: 0x000B7E84 File Offset: 0x000B6084
		public float SiegeWeaponPriority
		{
			get
			{
				return 25f;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x000B7E8B File Offset: 0x000B608B
		public int OverTheWallNavMeshID
		{
			get
			{
				return this.GateNavMeshId;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000B7E93 File Offset: 0x000B6093
		public bool HoldLadders
		{
			get
			{
				return !this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000B7EA3 File Offset: 0x000B60A3
		public bool SendLadders
		{
			get
			{
				return this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000B7EB0 File Offset: 0x000B60B0
		// (set) Token: 0x06002F5D RID: 12125 RVA: 0x000B7EB8 File Offset: 0x000B60B8
		public bool HasArrivedAtTarget
		{
			get
			{
				return this._hasArrivedAtTarget;
			}
			set
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.MovementComponent.SetDestinationNavMeshIdState(!value);
				}
				if (this._hasArrivedAtTarget != value)
				{
					this._hasArrivedAtTarget = value;
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetBatteringRamHasArrivedAtTarget(base.Id));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
						return;
					}
					if (GameNetwork.IsClientOrReplay)
					{
						this.MovementComponent.MoveToTargetAsClient();
					}
				}
			}
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000B7F22 File Offset: 0x000B6122
		public override void Disable()
		{
			base.Disable();
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.DisabledNavMeshID != 0)
				{
					base.Scene.SetAbilityOfFacesWithId(this.DisabledNavMeshID, true);
				}
				base.Scene.SetAbilityOfFacesWithId(this.DynamicNavmeshIdStart + 4, false);
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000B7F61 File Offset: 0x000B6161
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.Ram;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000B7F68 File Offset: 0x000B6168
		protected internal override void OnInit()
		{
			base.OnInit();
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.BattleSide = BattleSideEnum.Attacker;
			}
			this._state = BatteringRam.RamState.Stable;
			IEnumerable<WeakGameEntity> enumerable = from ewgt in base.Scene.FindWeakEntitiesWithTag(this._gateTag).ToList<WeakGameEntity>()
				where ewgt.HasScriptOfType<CastleGate>()
				select ewgt;
			if (!enumerable.IsEmpty<WeakGameEntity>())
			{
				this._gate = enumerable.First<WeakGameEntity>().GetFirstScriptOfType<CastleGate>();
				this._gate.AttackerSiegeWeapon = this;
			}
			this.AddRegularMovementComponent();
			this._batteringRamBody = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("body"));
			this._batteringRamBodySkeleton = this._batteringRamBody.Skeleton;
			this._batteringRamBodySkeleton.SetAnimationAtChannel("batteringram_idle", 0, 1f, 0f, 0f);
			this._pullStandingPoints = new List<StandingPoint>();
			this._pullStandingPointLocalIKFrames = new List<MatrixFrame>();
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
					if (standingPoint.GameEntity.HasTag("pull"))
					{
						standingPoint.IsDeactivated = true;
						this._pullStandingPoints.Add(standingPoint);
						this._pullStandingPointLocalIKFrames.Add(standingPoint.GameEntity.GetGlobalFrame().TransformToLocal(in globalFrame));
						standingPoint.AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
					}
				}
			}
			string sideTag = this._sideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this._weaponSide = FormationAI.BehaviorSide.Middle;
					}
					else
					{
						this._weaponSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this._weaponSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this._weaponSide = FormationAI.BehaviorSide.Left;
			}
			this._ditchFillDebris = base.Scene.FindEntitiesWithTag("ditch_filler").FirstOrDefault<GameEntity>((GameEntity df) => df.HasTag(this._sideTag));
			base.SetScriptComponentToTick(this.GetTickRequirement());
			Mission.Current.AddToWeaponListForFriendlyFirePreventing(this);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000B81C8 File Offset: 0x000B63C8
		private void AddRegularMovementComponent()
		{
			this.MovementComponent = new SiegeWeaponMovementComponent
			{
				PathEntityName = this.PathEntity,
				MinSpeed = this.MinSpeed,
				MaxSpeed = this.MaxSpeed,
				MainObject = this,
				WheelDiameter = this.WheelDiameter,
				NavMeshIdToDisableOnDestination = this.NavMeshIdToDisableOnDestination,
				MovementSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/siege/batteringram/move"),
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000B824C File Offset: 0x000B644C
		protected internal override void OnDeploymentStateChanged(bool isDeployed)
		{
			base.OnDeploymentStateChanged(isDeployed);
			if (this._ditchFillDebris != null)
			{
				this._ditchFillDebris.SetVisibilityExcludeParents(isDeployed);
				if (!GameNetwork.IsClientOrReplay)
				{
					if (isDeployed)
					{
						Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID1, true);
						Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID2, true);
						Mission.Current.Scene.SeparateFacesWithId(this._ditchNavMeshID1, this._groundToBridgeNavMeshID1);
						Mission.Current.Scene.SeparateFacesWithId(this._ditchNavMeshID2, this._groundToBridgeNavMeshID2);
						Mission.Current.Scene.MergeFacesWithId(this._bridgeNavMeshID1, this._groundToBridgeNavMeshID1, 0);
						Mission.Current.Scene.MergeFacesWithId(this._bridgeNavMeshID2, this._groundToBridgeNavMeshID2, 0);
						return;
					}
					Mission.Current.Scene.SeparateFacesWithId(this._bridgeNavMeshID1, this._groundToBridgeNavMeshID1);
					Mission.Current.Scene.SeparateFacesWithId(this._bridgeNavMeshID2, this._groundToBridgeNavMeshID2);
					Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID1, false);
					Mission.Current.Scene.SetAbilityOfFacesWithId(this._bridgeNavMeshID2, false);
					Mission.Current.Scene.MergeFacesWithId(this._ditchNavMeshID1, this._groundToBridgeNavMeshID1, 0);
					Mission.Current.Scene.MergeFacesWithId(this._ditchNavMeshID2, this._groundToBridgeNavMeshID2, 0);
				}
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000B83C8 File Offset: 0x000B65C8
		public MatrixFrame GetInitialFrame()
		{
			if (this.MovementComponent != null)
			{
				return this.MovementComponent.GetInitialFrame();
			}
			return base.GameEntity.GetGlobalFrame();
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000B83F8 File Offset: 0x000B65F8
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000B8428 File Offset: 0x000B6628
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			this.MovementComponent.TickParallelManually(dt);
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			for (int i = 0; i < this._pullStandingPoints.Count; i++)
			{
				StandingPoint standingPoint = this._pullStandingPoints[i];
				if (standingPoint.HasUser)
				{
					if (standingPoint.UserAgent.IsInBeingStruckAction)
					{
						standingPoint.UserAgent.ClearHandInverseKinematics();
					}
					else
					{
						Agent userAgent = standingPoint.UserAgent;
						MatrixFrame matrixFrame = this._pullStandingPointLocalIKFrames[i];
						userAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in matrixFrame, in globalFrame, 0f);
					}
				}
			}
			if (this.MovementComponent.HasArrivedAtTarget && !this.IsDeactivated)
			{
				int userCountNotInStruckAction = base.UserCountNotInStruckAction;
				if (userCountNotInStruckAction > 0)
				{
					float animationParameterAtChannel = this._batteringRamBodySkeleton.GetAnimationParameterAtChannel(0);
					this.UpdateHitAnimationWithProgress((userCountNotInStruckAction - 1) / 2, animationParameterAtChannel);
				}
			}
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000B8510 File Offset: 0x000B6710
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.MovementComponent.HasArrivedAtTarget && !this.HasArrivedAtTarget)
				{
					this.HasArrivedAtTarget = true;
					foreach (StandingPoint standingPoint in base.StandingPoints)
					{
						standingPoint.SetIsDeactivatedSynched(standingPoint.GameEntity.HasTag("move"));
					}
					if (this.DisabledNavMeshID != 0)
					{
						base.GameEntity.Scene.SetAbilityOfFacesWithId(this.DisabledNavMeshID, false);
					}
				}
				if (this.MovementComponent.HasArrivedAtTarget)
				{
					if (this._gate == null || this._gate.IsDestroyed || this._gate.IsGateOpen)
					{
						if (!this._isAllStandingPointsDisabled)
						{
							foreach (StandingPoint standingPoint2 in base.StandingPoints)
							{
								standingPoint2.SetIsDeactivatedSynched(true);
							}
							this._isAllStandingPointsDisabled = true;
							return;
						}
					}
					else
					{
						if (this._isAllStandingPointsDisabled && !this.IsDeactivated)
						{
							foreach (StandingPoint standingPoint3 in base.StandingPoints)
							{
								standingPoint3.SetIsDeactivatedSynched(false);
							}
							this._isAllStandingPointsDisabled = false;
						}
						int userCountNotInStruckAction = base.UserCountNotInStruckAction;
						switch (this.State)
						{
						case BatteringRam.RamState.Stable:
							if (userCountNotInStruckAction > 0)
							{
								this.State = BatteringRam.RamState.Hitting;
								base.SetAbilityOfConditionalFaces(false);
								this._usedPower = userCountNotInStruckAction;
								this._storedPower = 0f;
								this.StartHitAnimationWithProgress((userCountNotInStruckAction - 1) / 2, 0f);
								return;
							}
							break;
						case BatteringRam.RamState.Hitting:
						{
							if (userCountNotInStruckAction <= 0 || this._gate == null || this._gate.IsGateOpen)
							{
								this._batteringRamBody.GetFirstScriptOfType<SynchedMissionObject>().SetAnimationAtChannelSynched("batteringram_idle", 0, 1f);
								this.State = BatteringRam.RamState.Stable;
								base.SetAbilityOfConditionalFaces(true);
								return;
							}
							int num = (userCountNotInStruckAction - 1) / 2;
							float animationParameterAtChannel = this._batteringRamBodySkeleton.GetAnimationParameterAtChannel(0);
							if ((this._usedPower - 1) / 2 != num)
							{
								this.StartHitAnimationWithProgress(num, animationParameterAtChannel);
							}
							this._usedPower = userCountNotInStruckAction;
							this._storedPower += (float)this._usedPower * dt;
							float num2 = ((num == 3) ? 0.5f : ((num == 2) ? 0.56f : 0.58f));
							string text = ((num == 3) ? "batteringram_fire" : ((num == 2) ? "batteringram_fire_weak" : "batteringram_fire_weakest"));
							if (animationParameterAtChannel >= num2)
							{
								MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
								float num3 = this._storedPower * this.DamageMultiplier;
								num3 /= animationParameterAtChannel * MBAnimation.GetAnimationDuration(text);
								this._gate.DestructionComponent.TriggerOnHit(base.PilotAgent, (int)num3, globalFrame.origin, globalFrame.rotation.f, in MissionWeapon.Invalid, -1, this);
								this.State = BatteringRam.RamState.AfterHit;
								return;
							}
							break;
						}
						case BatteringRam.RamState.AfterHit:
							if (this._batteringRamBodySkeleton.GetAnimationParameterAtChannel(0) > 0.999f)
							{
								this.State = BatteringRam.RamState.Stable;
								base.SetAbilityOfConditionalFaces(true);
							}
							break;
						default:
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000B8870 File Offset: 0x000B6A70
		private void StartHitAnimationWithProgress(int powerStage, float progress)
		{
			string text = ((powerStage == 2) ? "batteringram_fire" : ((powerStage == 1) ? "batteringram_fire_weak" : "batteringram_fire_weakest"));
			this._batteringRamBody.GetFirstScriptOfType<SynchedMissionObject>().SetAnimationAtChannelSynched(text, 0, 1f);
			if (progress > 0f)
			{
				this._batteringRamBody.GetFirstScriptOfType<SynchedMissionObject>().SetAnimationChannelParameterSynched(0, progress);
			}
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser && standingPoint.GameEntity.HasTag("pull"))
				{
					ActionIndexCache actionCodeForStandingPoint = this.GetActionCodeForStandingPoint(standingPoint, powerStage);
					if (!standingPoint.UserAgent.SetActionChannel(1, in actionCodeForStandingPoint, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, progress, false, -0.2f, 0, true) && standingPoint.UserAgent.Controller == AgentControllerType.AI)
					{
						standingPoint.UserAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
			}
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000B8980 File Offset: 0x000B6B80
		private void UpdateHitAnimationWithProgress(int powerStage, float progress)
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser && standingPoint.GameEntity.HasTag("pull"))
				{
					ActionIndexCache actionCodeForStandingPoint = this.GetActionCodeForStandingPoint(standingPoint, powerStage);
					if (standingPoint.UserAgent.GetCurrentAction(1) == actionCodeForStandingPoint)
					{
						standingPoint.UserAgent.SetCurrentActionProgress(1, progress);
					}
				}
			}
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000B8A14 File Offset: 0x000B6C14
		private ActionIndexCache GetActionCodeForStandingPoint(StandingPoint standingPoint, int powerStage)
		{
			bool flag = standingPoint.GameEntity.HasTag("right");
			ActionIndexCache actionIndexCache = ActionIndexCache.act_none;
			switch (powerStage)
			{
			case 0:
				actionIndexCache = (flag ? ActionIndexCache.act_usage_batteringram_left_slowest : ActionIndexCache.act_usage_batteringram_right_slowest);
				break;
			case 1:
				actionIndexCache = (flag ? ActionIndexCache.act_usage_batteringram_left_slower : ActionIndexCache.act_usage_batteringram_right_slower);
				break;
			case 2:
				actionIndexCache = (flag ? ActionIndexCache.act_usage_batteringram_left : ActionIndexCache.act_usage_batteringram_right);
				break;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\BatteringRam.cs", "GetActionCodeForStandingPoint", 583);
				break;
			}
			return actionIndexCache;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000B8A9F File Offset: 0x000B6C9F
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new BatteringRamAI(this);
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000B8AA8 File Offset: 0x000B6CA8
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this._state = BatteringRam.RamState.Stable;
			if (!GameNetwork.IsClientOrReplay)
			{
				base.SetAbilityOfConditionalFaces(true);
			}
			this._hasArrivedAtTarget = false;
			this._batteringRamBodySkeleton.SetAnimationAtChannel("batteringram_idle", 0, 1f, 0f, 0f);
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.IsDeactivated = !standingPoint.GameEntity.HasTag("move");
			}
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000B8B50 File Offset: 0x000B6D50
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.HasArrivedAtTarget);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.BatteringRamStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.MovementComponent.GetTotalDistanceTraveledForPathTracker(), CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002F6D RID: 12141 RVA: 0x000B8B88 File Offset: 0x000B6D88
		public override bool IsDeactivated
		{
			get
			{
				return this._gate == null || this._gate.IsDestroyed || (this._gate.State == CastleGate.GateState.Open && this.HasArrivedAtTarget) || base.IsDeactivated;
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000B8BBC File Offset: 0x000B6DBC
		public void HighlightPath()
		{
			this.MovementComponent.HighlightPath();
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000B8BCC File Offset: 0x000B6DCC
		public void SwitchGhostEntityMovementMode(bool isGhostEnabled)
		{
			if (isGhostEnabled)
			{
				if (!this._isGhostMovementOn)
				{
					base.RemoveComponent(this.MovementComponent);
					this.SetUpGhostEntity();
					this.GhostEntityMove = true;
					SiegeWeaponMovementComponent component = base.GetComponent<SiegeWeaponMovementComponent>();
					component.GhostEntitySpeedMultiplier *= 3f;
					component.SetGhostVisibility(true);
				}
				this._isGhostMovementOn = true;
				return;
			}
			if (this._isGhostMovementOn)
			{
				base.RemoveComponent(this.MovementComponent);
				PathLastNodeFixer component2 = base.GetComponent<PathLastNodeFixer>();
				base.RemoveComponent(component2);
				this.AddRegularMovementComponent();
				this.MovementComponent.SetGhostVisibility(false);
			}
			this._isGhostMovementOn = false;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000B8C60 File Offset: 0x000B6E60
		private void SetUpGhostEntity()
		{
			PathLastNodeFixer pathLastNodeFixer = new PathLastNodeFixer
			{
				PathHolder = this
			};
			base.AddComponent(pathLastNodeFixer);
			this.MovementComponent = new SiegeWeaponMovementComponent
			{
				PathEntityName = this.PathEntity,
				MainObject = this,
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
			this.MovementComponent.SetupGhostEntity();
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000B8CC2 File Offset: 0x000B6EC2
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=MaBSSg7I}Battering Ram", null);
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000B8CD0 File Offset: 0x000B6ED0
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = (usableGameObject.GameEntity.HasTag("pull") ? new TextObject("{=1cnJtNTt}{KEY} Pull", null) : new TextObject("{=rwZAZSvX}{KEY} Move", null));
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000B8D2C File Offset: 0x000B6F2C
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (side != BattleSideEnum.Attacker)
			{
				return OrderType.AttackEntity;
			}
			if (!this.HasCompletedAction())
			{
				return OrderType.FollowEntity;
			}
			return OrderType.Use;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000B8D4C File Offset: 0x000B6F4C
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			if (base.UserCountNotInStruckAction > 0)
			{
				targetFlags |= TargetFlags.IsMoving;
			}
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToRam)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			if (this.HasCompletedAction() || base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000B8DA8 File Offset: 0x000B6FA8
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 300f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000B8DCC File Offset: 0x000B6FCC
		protected override float GetDistanceMultiplierOfWeapon(Vec3 weaponPos)
		{
			float minimumDistanceBetweenPositions = this.GetMinimumDistanceBetweenPositions(weaponPos);
			if (minimumDistanceBetweenPositions < 100f)
			{
				return 1f;
			}
			if (minimumDistanceBetweenPositions < 625f)
			{
				return 0.8f;
			}
			return 0.6f;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000B8E02 File Offset: 0x000B7002
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000B8E0C File Offset: 0x000B700C
		public void AssignParametersFromSpawner(string gateTag, string sideTag, int bridgeNavMeshID1, int bridgeNavMeshID2, int ditchNavMeshID1, int ditchNavMeshID2, int groundToBridgeNavMeshID1, int groundToBridgeNavMeshID2, string pathEntityName)
		{
			this._gateTag = gateTag;
			this._sideTag = sideTag;
			this._bridgeNavMeshID1 = bridgeNavMeshID1;
			this._bridgeNavMeshID2 = bridgeNavMeshID2;
			this._ditchNavMeshID1 = ditchNavMeshID1;
			this._ditchNavMeshID2 = ditchNavMeshID2;
			this._groundToBridgeNavMeshID1 = groundToBridgeNavMeshID1;
			this._groundToBridgeNavMeshID2 = groundToBridgeNavMeshID2;
			this._pathEntityName = pathEntityName;
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000B8E60 File Offset: 0x000B7060
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			BatteringRam.BatteringRamRecord batteringRamRecord = (BatteringRam.BatteringRamRecord)synchedMissionObjectReadableRecord.Item2;
			this.HasArrivedAtTarget = batteringRamRecord.HasArrivedAtTarget;
			this._state = (BatteringRam.RamState)batteringRamRecord.State;
			float num = batteringRamRecord.TotalDistanceTraveled;
			num += 0.05f;
			this.MovementComponent.SetTotalDistanceTraveledForPathTracker(num);
			this.MovementComponent.SetTargetFrameForPathTracker();
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000B8EC2 File Offset: 0x000B70C2
		public bool GetNavmeshFaceIds(out List<int> navmeshFaceIds)
		{
			navmeshFaceIds = null;
			return false;
		}

		// Token: 0x0400130A RID: 4874
		private string _pathEntityName = "Path";

		// Token: 0x0400130B RID: 4875
		private const string PullStandingPointTag = "pull";

		// Token: 0x0400130C RID: 4876
		private const string RightStandingPointTag = "right";

		// Token: 0x0400130D RID: 4877
		private const string IdleAnimation = "batteringram_idle";

		// Token: 0x0400130E RID: 4878
		private const string KnockAnimation = "batteringram_fire";

		// Token: 0x0400130F RID: 4879
		private const string KnockSlowerAnimation = "batteringram_fire_weak";

		// Token: 0x04001310 RID: 4880
		private const string KnockSlowestAnimation = "batteringram_fire_weakest";

		// Token: 0x04001311 RID: 4881
		private const float KnockAnimationHitProgress = 0.5f;

		// Token: 0x04001312 RID: 4882
		private const float KnockSlowerAnimationHitProgress = 0.56f;

		// Token: 0x04001313 RID: 4883
		private const float KnockSlowestAnimationHitProgress = 0.58f;

		// Token: 0x04001314 RID: 4884
		private string _gateTag = "gate";

		// Token: 0x04001315 RID: 4885
		public bool GhostEntityMove = true;

		// Token: 0x04001316 RID: 4886
		public float GhostEntitySpeedMultiplier = 1f;

		// Token: 0x04001317 RID: 4887
		private string _sideTag;

		// Token: 0x04001318 RID: 4888
		private FormationAI.BehaviorSide _weaponSide;

		// Token: 0x04001319 RID: 4889
		public float WheelDiameter = 1.3f;

		// Token: 0x0400131A RID: 4890
		public int GateNavMeshId = 7;

		// Token: 0x0400131B RID: 4891
		public int DisabledNavMeshID = 8;

		// Token: 0x0400131C RID: 4892
		private int _bridgeNavMeshID1 = 8;

		// Token: 0x0400131D RID: 4893
		private int _bridgeNavMeshID2 = 8;

		// Token: 0x0400131E RID: 4894
		private int _ditchNavMeshID1 = 9;

		// Token: 0x0400131F RID: 4895
		private int _ditchNavMeshID2 = 10;

		// Token: 0x04001320 RID: 4896
		private int _groundToBridgeNavMeshID1 = 12;

		// Token: 0x04001321 RID: 4897
		private int _groundToBridgeNavMeshID2 = 13;

		// Token: 0x04001322 RID: 4898
		public int NavMeshIdToDisableOnDestination = -1;

		// Token: 0x04001323 RID: 4899
		public float MinSpeed = 0.5f;

		// Token: 0x04001324 RID: 4900
		public float MaxSpeed = 1f;

		// Token: 0x04001325 RID: 4901
		public float DamageMultiplier = 10f;

		// Token: 0x04001326 RID: 4902
		private int _usedPower;

		// Token: 0x04001327 RID: 4903
		private float _storedPower;

		// Token: 0x04001328 RID: 4904
		private List<StandingPoint> _pullStandingPoints;

		// Token: 0x04001329 RID: 4905
		private List<MatrixFrame> _pullStandingPointLocalIKFrames;

		// Token: 0x0400132A RID: 4906
		private GameEntity _ditchFillDebris;

		// Token: 0x0400132B RID: 4907
		private GameEntity _batteringRamBody;

		// Token: 0x0400132C RID: 4908
		private Skeleton _batteringRamBodySkeleton;

		// Token: 0x0400132D RID: 4909
		private bool _isGhostMovementOn;

		// Token: 0x0400132E RID: 4910
		private bool _isAllStandingPointsDisabled;

		// Token: 0x0400132F RID: 4911
		private BatteringRam.RamState _state;

		// Token: 0x04001330 RID: 4912
		private CastleGate _gate;

		// Token: 0x04001331 RID: 4913
		private bool _hasArrivedAtTarget;

		// Token: 0x02000619 RID: 1561
		[DefineSynchedMissionObjectType(typeof(BatteringRam))]
		public struct BatteringRamRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000ACE RID: 2766
			// (get) Token: 0x06004074 RID: 16500 RVA: 0x000FC071 File Offset: 0x000FA271
			// (set) Token: 0x06004075 RID: 16501 RVA: 0x000FC079 File Offset: 0x000FA279
			public bool HasArrivedAtTarget { get; private set; }

			// Token: 0x17000ACF RID: 2767
			// (get) Token: 0x06004076 RID: 16502 RVA: 0x000FC082 File Offset: 0x000FA282
			// (set) Token: 0x06004077 RID: 16503 RVA: 0x000FC08A File Offset: 0x000FA28A
			public int State { get; private set; }

			// Token: 0x17000AD0 RID: 2768
			// (get) Token: 0x06004078 RID: 16504 RVA: 0x000FC093 File Offset: 0x000FA293
			// (set) Token: 0x06004079 RID: 16505 RVA: 0x000FC09B File Offset: 0x000FA29B
			public float TotalDistanceTraveled { get; private set; }

			// Token: 0x0600407A RID: 16506 RVA: 0x000FC0A4 File Offset: 0x000FA2A4
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.HasArrivedAtTarget = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.BatteringRamStateCompressionInfo, ref bufferReadValid);
				this.TotalDistanceTraveled = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200061A RID: 1562
		public enum RamState
		{
			// Token: 0x040020DE RID: 8414
			Stable,
			// Token: 0x040020DF RID: 8415
			Hitting,
			// Token: 0x040020E0 RID: 8416
			AfterHit,
			// Token: 0x040020E1 RID: 8417
			NumberOfStates
		}
	}
}
