using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Source.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034B RID: 843
	public class CastleGate : UsableMachine, IPointDefendable, ICastleKeyPosition, ITargetable
	{
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002F7D RID: 12157 RVA: 0x000B8F80 File Offset: 0x000B7180
		// (set) Token: 0x06002F7E RID: 12158 RVA: 0x000B8F88 File Offset: 0x000B7188
		public TacticalPosition MiddlePosition { get; private set; }

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002F7F RID: 12159 RVA: 0x000B8F91 File Offset: 0x000B7191
		private static int BatteringRamHitSoundIdCache
		{
			get
			{
				if (CastleGate._batteringRamHitSoundId == -1)
				{
					CastleGate._batteringRamHitSoundId = SoundEvent.GetEventIdFromString("event:/mission/siege/door/hit");
				}
				return CastleGate._batteringRamHitSoundId;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002F80 RID: 12160 RVA: 0x000B8FAF File Offset: 0x000B71AF
		// (set) Token: 0x06002F81 RID: 12161 RVA: 0x000B8FB7 File Offset: 0x000B71B7
		public TacticalPosition WaitPosition { get; private set; }

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002F82 RID: 12162 RVA: 0x000B8FC0 File Offset: 0x000B71C0
		public override FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.Gate;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002F83 RID: 12163 RVA: 0x000B8FC3 File Offset: 0x000B71C3
		// (set) Token: 0x06002F84 RID: 12164 RVA: 0x000B8FCB File Offset: 0x000B71CB
		public CastleGate.GateState State { get; private set; }

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002F85 RID: 12165 RVA: 0x000B8FD4 File Offset: 0x000B71D4
		public bool IsGateOpen
		{
			get
			{
				return this.State == CastleGate.GateState.Open || base.IsDestroyed;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002F86 RID: 12166 RVA: 0x000B8FE6 File Offset: 0x000B71E6
		// (set) Token: 0x06002F87 RID: 12167 RVA: 0x000B8FEE File Offset: 0x000B71EE
		public IPrimarySiegeWeapon AttackerSiegeWeapon { get; set; }

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002F88 RID: 12168 RVA: 0x000B8FF7 File Offset: 0x000B71F7
		// (set) Token: 0x06002F89 RID: 12169 RVA: 0x000B8FFF File Offset: 0x000B71FF
		public IEnumerable<DefencePoint> DefencePoints { get; protected set; }

		// Token: 0x06002F8A RID: 12170 RVA: 0x000B9008 File Offset: 0x000B7208
		public CastleGate()
		{
			this._attackOnlyDoorColliders = new List<GameEntity>();
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000B90C8 File Offset: 0x000B72C8
		public Vec3 GetPosition()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000B90E3 File Offset: 0x000B72E3
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (side != BattleSideEnum.Attacker)
			{
				return OrderType.Use;
			}
			return OrderType.AttackEntity;
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x000B90F8 File Offset: 0x000B72F8
		// (set) Token: 0x06002F8E RID: 12174 RVA: 0x000B9100 File Offset: 0x000B7300
		public FormationAI.BehaviorSide DefenseSide { get; private set; }

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x000B9109 File Offset: 0x000B7309
		public WorldFrame MiddleFrame
		{
			get
			{
				return this._middleFrame;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x000B9111 File Offset: 0x000B7311
		public WorldFrame DefenseWaitFrame
		{
			get
			{
				return this._defenseWaitFrame;
			}
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000B911C File Offset: 0x000B731C
		protected internal override void OnInit()
		{
			base.OnInit();
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.OnNextDestructionState += this.OnNextDestructionState;
				this.DestructibleComponentOnMissionReset = new Action(firstScriptOfType.OnMissionReset);
				if (!GameNetwork.IsClientOrReplay)
				{
					firstScriptOfType.OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnDestroyed);
					firstScriptOfType.OnHitTaken += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnHitTaken);
					DestructableComponent destructableComponent = firstScriptOfType;
					destructableComponent.OnCalculateDestructionStateIndex = (Func<int, int, int, int>)Delegate.Combine(destructableComponent.OnCalculateDestructionStateIndex, new Func<int, int, int, int>(this.OnCalculateDestructionStateIndex));
				}
				firstScriptOfType.BattleSide = BattleSideEnum.Defender;
			}
			this.CollectGameEntities(true);
			base.GameEntity.SetAnimationSoundActivation(true);
			if (GameNetwork.IsClientOrReplay)
			{
				return;
			}
			this._queueManager = base.GameEntity.GetFirstScriptOfType<LadderQueueManager>();
			if (this._queueManager == null)
			{
				WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity ce) => ce.HasScriptOfType<LadderQueueManager>());
				if (weakGameEntity.IsValid)
				{
					this._queueManager = weakGameEntity.GetFirstScriptOfType<LadderQueueManager>();
				}
			}
			if (this._queueManager != null)
			{
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin.y = identity.origin.y - 2f;
				identity.rotation.RotateAboutSide(-1.5707964f);
				identity.rotation.RotateAboutForward(3.1415927f);
				this._queueManager.Initialize(this._queueManager.ManagedNavigationFaceId, identity, -identity.rotation.u, BattleSideEnum.Defender, 15, 0.62831855f, 3f, 2.2f, 0f, 0f, false, 1f, 2.1474836E+09f, 5f, false, -2, -2, int.MaxValue, 15);
				this._queueManager.Activate();
			}
			string sideTag = this.SideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this.DefenseSide = FormationAI.BehaviorSide.BehaviorSideNotSet;
					}
					else
					{
						this.DefenseSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this.DefenseSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this.DefenseSide = FormationAI.BehaviorSide.Left;
			}
			List<WeakGameEntity> list = base.GameEntity.CollectChildrenEntitiesWithTag("middle_pos");
			if (list.Count > 0)
			{
				WeakGameEntity weakGameEntity2 = list.FirstOrDefault<WeakGameEntity>();
				this.MiddlePosition = weakGameEntity2.GetFirstScriptOfType<TacticalPosition>();
				MatrixFrame globalFrame = weakGameEntity2.GetGlobalFrame();
				this._middleFrame = new WorldFrame(globalFrame.rotation, globalFrame.origin.ToWorldPosition());
				this._middleFrame.Origin.GetGroundVec3();
			}
			else
			{
				MatrixFrame globalFrame2 = base.GameEntity.GetGlobalFrame();
				this._middleFrame = new WorldFrame(globalFrame2.rotation, globalFrame2.origin.ToWorldPosition());
			}
			List<WeakGameEntity> list2 = base.GameEntity.CollectChildrenEntitiesWithTag("wait_pos");
			if (list2.Count > 0)
			{
				WeakGameEntity weakGameEntity3 = list2.FirstOrDefault<WeakGameEntity>();
				this.WaitPosition = weakGameEntity3.GetFirstScriptOfType<TacticalPosition>();
				MatrixFrame globalFrame3 = weakGameEntity3.GetGlobalFrame();
				this._defenseWaitFrame = new WorldFrame(globalFrame3.rotation, globalFrame3.origin.ToWorldPosition());
				this._defenseWaitFrame.Origin.GetGroundVec3();
			}
			else
			{
				this._defenseWaitFrame = this._middleFrame;
			}
			this._openingAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.OpeningAnimationName);
			this._closingAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.ClosingAnimationName);
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.OnCheckForProblems();
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000B9490 File Offset: 0x000B7690
		public void SetUsableTeam(Team team)
		{
			using (List<StandingPoint>.Enumerator enumerator = base.StandingPoints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StandingPointWithTeamLimit standingPointWithTeamLimit;
					if ((standingPointWithTeamLimit = enumerator.Current as StandingPointWithTeamLimit) != null)
					{
						standingPointWithTeamLimit.UsableTeam = team;
					}
				}
			}
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000B94EC File Offset: 0x000B76EC
		public override void AfterMissionStart()
		{
			this._afterMissionStartTriggered = true;
			base.AfterMissionStart();
			this.SetInitialStateOfGate();
			this.InitializeExtraColliderPositions();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetAutoOpenState(Mission.Current.IsSallyOutBattle);
			}
			if (this.OwningTeam == CastleGate.DoorOwnership.Attackers)
			{
				this.SetUsableTeam(Mission.Current.AttackerTeam);
			}
			else if (this.OwningTeam == CastleGate.DoorOwnership.Defenders)
			{
				this.SetUsableTeam(Mission.Current.DefenderTeam);
			}
			this._pathChecker = new AgentPathNavMeshChecker(Mission.Current, base.GameEntity.GetGlobalFrame(), 2f, this.NavigationMeshId, BattleSideEnum.Defender, AgentPathNavMeshChecker.Direction.BothDirections, 14f, 3f);
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000B9594 File Offset: 0x000B7794
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.OnNextDestructionState -= this.OnNextDestructionState;
				if (!GameNetwork.IsClientOrReplay)
				{
					firstScriptOfType.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnDestroyed);
					firstScriptOfType.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnHitTaken);
					DestructableComponent destructableComponent = firstScriptOfType;
					destructableComponent.OnCalculateDestructionStateIndex = (Func<int, int, int, int>)Delegate.Remove(destructableComponent.OnCalculateDestructionStateIndex, new Func<int, int, int, int>(this.OnCalculateDestructionStateIndex));
				}
			}
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000B961C File Offset: 0x000B781C
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			if (base.GameEntity.HasTag("outer_gate") && base.GameEntity.HasTag("inner_gate"))
			{
				MBDebug.ShowWarning("Castle gate has both the outer gate tag and the inner gate tag.");
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000B9663 File Offset: 0x000B7863
		protected internal override void OnMissionReset()
		{
			Action destructibleComponentOnMissionReset = this.DestructibleComponentOnMissionReset;
			if (destructibleComponentOnMissionReset != null)
			{
				destructibleComponentOnMissionReset();
			}
			this.CollectGameEntities(false);
			base.OnMissionReset();
			this.SetInitialStateOfGate();
			this._previousAnimationProgress = -1f;
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000B9694 File Offset: 0x000B7894
		private void SetInitialStateOfGate()
		{
			if (!GameNetwork.IsClientOrReplay && this.NavigationMeshIdToDisableOnOpen != -1)
			{
				this._openNavMeshIdDisabled = false;
				base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, true);
			}
			if (!this._civilianMission)
			{
				this._doorSkeleton.SetAnimationAtChannel(this._closingAnimationIndex, 0, 1f, -1f, 0f);
				this._doorSkeleton.SetAnimationParameterAtChannel(0, 0.99f);
				this._doorSkeleton.Freeze(false);
				this.State = CastleGate.GateState.Closed;
				return;
			}
			this.OpenDoor();
			if (this._doorSkeleton != null)
			{
				this._door.SetAnimationChannelParameterSynched(0, 1f);
			}
			this.SetGateNavMeshState(true);
			base.SetDisabled(true);
			DestructableComponent firstScriptOfType = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			if (firstScriptOfType == null)
			{
				return;
			}
			firstScriptOfType.SetDisabled(false);
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000B9765 File Offset: 0x000B7965
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=6wZUG0ev}Gate", null);
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000B9774 File Offset: 0x000B7974
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			if (!this.IsDeactivated)
			{
				TextObject textObject = new TextObject(usableGameObject.GameEntity.HasTag("open") ? "{=5oozsaIb}{KEY} Open" : "{=TJj71hPO}{KEY} Close", null);
				textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				return textObject;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000B97D8 File Offset: 0x000B79D8
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new CastleGateAI(this);
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000B97E0 File Offset: 0x000B79E0
		public void OpenDoorAndDisableGateForCivilianMission()
		{
			this._civilianMission = true;
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000B97EC File Offset: 0x000B79EC
		public void OpenDoor()
		{
			if (!base.IsDisabled)
			{
				this.State = CastleGate.GateState.Open;
				if (!this.AutoOpen)
				{
					this.SetGateNavMeshState(true);
				}
				else
				{
					this.SetGateNavMeshStateForEnemies(true);
				}
				int animationIndexAtChannel = this._doorSkeleton.GetAnimationIndexAtChannel(0);
				float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
				this._door.SetAnimationAtChannelSynched(this._openingAnimationIndex, 0, 1f);
				if (animationIndexAtChannel == this._closingAnimationIndex)
				{
					this._door.SetAnimationChannelParameterSynched(0, 1f - animationParameterAtChannel);
				}
				SynchedMissionObject plank = this._plank;
				if (plank == null)
				{
					return;
				}
				plank.SetVisibleSynched(false, false);
			}
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000B9880 File Offset: 0x000B7A80
		public void CloseDoor()
		{
			if (!base.IsDisabled)
			{
				this.State = CastleGate.GateState.Closed;
				if (!this.AutoOpen)
				{
					this.SetGateNavMeshState(false);
				}
				else
				{
					this.SetGateNavMeshStateForEnemies(false);
				}
				int animationIndexAtChannel = this._doorSkeleton.GetAnimationIndexAtChannel(0);
				float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
				this._door.SetAnimationAtChannelSynched(this._closingAnimationIndex, 0, 1f);
				if (animationIndexAtChannel == this._openingAnimationIndex)
				{
					this._door.SetAnimationChannelParameterSynched(0, 1f - animationParameterAtChannel);
				}
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000B9900 File Offset: 0x000B7B00
		private void UpdateDoorBodies(bool updateAnyway)
		{
			if (this._attackOnlyDoorColliders.Count == 2)
			{
				float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
				if (this._previousAnimationProgress != animationParameterAtChannel || updateAnyway)
				{
					this._previousAnimationProgress = animationParameterAtChannel;
					MatrixFrame matrixFrame = this._doorSkeleton.GetBoneEntitialFrameWithIndex(this._leftDoorBoneIndex);
					MatrixFrame matrixFrame2 = this._doorSkeleton.GetBoneEntitialFrameWithIndex(this._rightDoorBoneIndex);
					this._attackOnlyDoorColliders[0].SetFrame(ref matrixFrame2, true);
					this._attackOnlyDoorColliders[1].SetFrame(ref matrixFrame, true);
					GameEntity agentColliderLeft = this._agentColliderLeft;
					if (agentColliderLeft != null)
					{
						agentColliderLeft.SetFrame(ref matrixFrame, true);
					}
					GameEntity agentColliderRight = this._agentColliderRight;
					if (agentColliderRight != null)
					{
						agentColliderRight.SetFrame(ref matrixFrame2, true);
					}
					if (this._extraColliderLeft != null && this._extraColliderRight != null)
					{
						if (this.State == CastleGate.GateState.Closed)
						{
							if (!this._leftExtraColliderDisabled)
							{
								this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
								this._leftExtraColliderDisabled = true;
							}
							if (!this._rightExtraColliderDisabled)
							{
								this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
								this._rightExtraColliderDisabled = true;
								return;
							}
						}
						else
						{
							float num = (matrixFrame2.origin - matrixFrame.origin).Length * 0.5f;
							float num2 = Vec3.DotProduct(matrixFrame2.rotation.s, Vec3.Side) / (matrixFrame2.rotation.s.Length * 1f);
							float num3 = MathF.Sqrt(1f - num2 * num2);
							float num4 = num * 1.1f;
							float num5 = MBMath.Map(num2, 0.3f, 1f, 0f, 1f) * (num * 0.2f);
							this._extraColliderLeft.SetLocalPosition(matrixFrame.origin - new Vec3(num4 - num + num5, num * num3, 0f, -1f));
							this._extraColliderRight.SetLocalPosition(matrixFrame2.origin - new Vec3(-(num4 - num) - num5, num * num3, 0f, -1f));
							float num6;
							if (num2 < 0f)
							{
								num6 = num;
								num6 += num * -num2;
							}
							else
							{
								num6 = num - num * num2;
							}
							num6 = (num4 - num6) / num;
							if (num6 <= 0.0001f)
							{
								if (!this._leftExtraColliderDisabled)
								{
									this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
									this._leftExtraColliderDisabled = true;
								}
							}
							else
							{
								if (this._leftExtraColliderDisabled)
								{
									this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag & ~BodyFlags.Disabled);
									this._leftExtraColliderDisabled = false;
								}
								matrixFrame = this._extraColliderLeft.GetFrame();
								matrixFrame.rotation.Orthonormalize();
								matrixFrame.origin -= new Vec3(num4 - num4 * num6, 0f, 0f, -1f);
								this._extraColliderLeft.SetFrame(ref matrixFrame, true);
							}
							matrixFrame2 = this._extraColliderRight.GetFrame();
							matrixFrame2.rotation.Orthonormalize();
							float num7;
							if (num2 < 0f)
							{
								num7 = num;
								num7 += num * -num2;
							}
							else
							{
								num7 = num - num * num2;
							}
							num7 = (num4 - num7) / num;
							if (num7 > 0.0001f)
							{
								if (this._rightExtraColliderDisabled)
								{
									this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag & ~BodyFlags.Disabled);
									this._rightExtraColliderDisabled = false;
								}
								matrixFrame2.origin += new Vec3(num4 - num4 * num7, 0f, 0f, -1f);
								this._extraColliderRight.SetFrame(ref matrixFrame2, true);
								return;
							}
							if (!this._rightExtraColliderDisabled)
							{
								this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
								this._rightExtraColliderDisabled = true;
								return;
							}
						}
					}
				}
			}
			else if (this._attackOnlyDoorColliders.Count == 1)
			{
				MatrixFrame boneEntitialFrameWithName = this._doorSkeleton.GetBoneEntitialFrameWithName(this.RightDoorBoneName);
				this._attackOnlyDoorColliders[0].SetFrame(ref boneEntitialFrameWithName, true);
				GameEntity agentColliderRight2 = this._agentColliderRight;
				if (agentColliderRight2 == null)
				{
					return;
				}
				agentColliderRight2.SetFrame(ref boneEntitialFrameWithName, true);
			}
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000B9D40 File Offset: 0x000B7F40
		private void SetGateNavMeshState(bool isEnabled)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshId, isEnabled);
				if (this._queueManager != null)
				{
					this._queueManager.Activate();
					base.Scene.SetAbilityOfFacesWithId(this._queueManager.ManagedNavigationFaceId, isEnabled);
				}
			}
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000B9D94 File Offset: 0x000B7F94
		private void SetGateNavMeshStateForEnemies(bool isEnabled)
		{
			Team attackerTeam = Mission.Current.AttackerTeam;
			if (attackerTeam != null)
			{
				foreach (Agent agent in attackerTeam.ActiveAgents)
				{
					if (agent.IsAIControlled)
					{
						agent.SetAgentExcludeStateForFaceGroupId(this.NavigationMeshId, !isEnabled);
					}
				}
			}
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000B9E08 File Offset: 0x000B8008
		public void SetAutoOpenState(bool isEnabled)
		{
			this.AutoOpen = isEnabled;
			if (this.AutoOpen)
			{
				this.SetGateNavMeshState(true);
				this.SetGateNavMeshStateForEnemies(this.State == CastleGate.GateState.Open);
				return;
			}
			if (this.State == CastleGate.GateState.Open)
			{
				this.CloseDoor();
			}
			else
			{
				this.SetGateNavMeshState(false);
			}
			this.SetGateNavMeshStateForEnemies(true);
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x000B9E5C File Offset: 0x000B805C
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000B9E88 File Offset: 0x000B8088
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!base.GameEntity.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay && this.NavigationMeshIdToDisableOnOpen != -1)
			{
				if (this._openNavMeshIdDisabled)
				{
					if (base.IsDestroyed)
					{
						base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, true);
						this._openNavMeshIdDisabled = false;
					}
					else if (this.State == CastleGate.GateState.Closed)
					{
						int animationIndexAtChannel = this._doorSkeleton.GetAnimationIndexAtChannel(0);
						float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
						if (animationIndexAtChannel != this._closingAnimationIndex || animationParameterAtChannel > 0.4f)
						{
							base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, true);
							this._openNavMeshIdDisabled = false;
						}
					}
				}
				else if (this.State == CastleGate.GateState.Open && !base.IsDestroyed)
				{
					base.Scene.SetAbilityOfFacesWithId(this.NavigationMeshIdToDisableOnOpen, false);
					this._openNavMeshIdDisabled = true;
				}
			}
			if (this._afterMissionStartTriggered)
			{
				this.UpdateDoorBodies(false);
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this.ServerTick(dt);
			}
			if (base.Ai.HasActionCompleted)
			{
				bool flag = false;
				for (int i = 0; i < base.StandingPoints.Count; i++)
				{
					if (base.StandingPoints[i].HasUser || base.StandingPoints[i].HasAIMovingTo)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					bool flag2 = false;
					for (int j = 0; j < base.UserFormations.Count; j++)
					{
						if (base.UserFormations[j].CountOfDetachableNonPlayerUnits > 0)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						((CastleGateAI)base.Ai).ResetInitialGateState(this.State);
					}
				}
			}
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000BA02C File Offset: 0x000B822C
		protected override bool IsAgentOnInconvenientNavmesh(Agent agent, StandingPoint standingPoint)
		{
			if (Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege)
			{
				return false;
			}
			int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = agent.Team.TeamAI as TeamAISiegeComponent) != null && currentNavigationFaceId % 10 != 1)
			{
				if (base.GameEntity.HasTag("inner_gate"))
				{
					return true;
				}
				if (base.GameEntity.HasTag("outer_gate"))
				{
					CastleGate innerGate = teamAISiegeComponent.InnerGate;
					if (innerGate != null)
					{
						Vec3 vec = base.GameEntity.GlobalPosition - agent.Position;
						Vec3 vec2 = innerGate.GameEntity.GlobalPosition - agent.Position;
						if (vec.AsVec2.DotProduct(vec2.AsVec2) > 0f)
						{
							return true;
						}
					}
				}
				foreach (int num in (Mission.Current.DefenderTeam.TeamAI as TeamAISiegeDefender).DifficultNavmeshIDs)
				{
					if (currentNavigationFaceId == num)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000BA160 File Offset: 0x000B8360
		private void ServerTick(float dt)
		{
			if (!this.IsDeactivated)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.HasUser)
					{
						WeakGameEntity weakGameEntity = standingPoint.GameEntity;
						if (weakGameEntity.HasTag("open"))
						{
							this.OpenDoor();
							if (this.AutoOpen)
							{
								this.SetAutoOpenState(false);
							}
						}
						else
						{
							this.CloseDoor();
							if (Mission.Current.IsSallyOutBattle)
							{
								this.SetAutoOpenState(true);
							}
						}
					}
				}
				if (this.AutoOpen && this._pathChecker != null)
				{
					this._pathChecker.Tick(dt);
					if (this._pathChecker.HasAgentsUsingPath())
					{
						if (this.State != CastleGate.GateState.Open)
						{
							this.OpenDoor();
						}
					}
					else if (this.State != CastleGate.GateState.Closed)
					{
						this.CloseDoor();
					}
				}
				if (this._doorSkeleton != null && !base.IsDestroyed)
				{
					float animationParameterAtChannel = this._doorSkeleton.GetAnimationParameterAtChannel(0);
					foreach (StandingPoint standingPoint2 in base.StandingPoints)
					{
						bool flag;
						if (animationParameterAtChannel >= 1f)
						{
							WeakGameEntity weakGameEntity = standingPoint2.GameEntity;
							flag = weakGameEntity.HasTag((this.State == CastleGate.GateState.Open) ? "open" : "close");
						}
						else
						{
							flag = true;
						}
						bool flag2 = flag;
						standingPoint2.SetIsDeactivatedSynched(flag2);
					}
					if (animationParameterAtChannel >= 1f && this.State == CastleGate.GateState.Open)
					{
						if (this._extraColliderRight != null)
						{
							this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
							this._rightExtraColliderDisabled = true;
						}
						if (this._extraColliderLeft != null)
						{
							this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
							this._leftExtraColliderDisabled = true;
						}
					}
					if (this._plank != null && this.State == CastleGate.GateState.Closed && animationParameterAtChannel > 0.9f)
					{
						this._plank.SetVisibleSynched(true, false);
					}
				}
			}
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000BA37C File Offset: 0x000B857C
		public TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			targetFlags |= TargetFlags.IsStructure;
			if (base.IsDestroyed)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (DebugSiegeBehavior.DebugAttackState == DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBattlements)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000BA3AD File Offset: 0x000B85AD
		public float GetTargetValue(List<Vec3> weaponPos)
		{
			return 10f;
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000BA3B4 File Offset: 0x000B85B4
		public WeakGameEntity GetTargetEntity()
		{
			return base.GameEntity;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000BA3BC File Offset: 0x000B85BC
		public BattleSideEnum GetSide()
		{
			return BattleSideEnum.Defender;
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000BA3BF File Offset: 0x000B85BF
		public Vec3 GetTargetGlobalVelocity()
		{
			return Vec3.Zero;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000BA3C8 File Offset: 0x000B85C8
		public bool IsDestructable()
		{
			return base.GameEntity.HasScriptOfType<DestructableComponent>();
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000BA3E3 File Offset: 0x000B85E3
		public WeakGameEntity Entity()
		{
			return base.GameEntity;
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000BA3EC File Offset: 0x000B85EC
		public ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax()
		{
			return base.GameEntity.ComputeGlobalPhysicsBoundingBoxMinMax();
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000BA408 File Offset: 0x000B8608
		protected void CollectGameEntities(bool calledFromOnInit)
		{
			this.CollectDynamicGameEntities(calledFromOnInit);
			if (!GameNetwork.IsClientOrReplay)
			{
				List<WeakGameEntity> list = base.GameEntity.CollectChildrenEntitiesWithTag("plank");
				if (list.Count > 0)
				{
					this._plank = list.FirstOrDefault<WeakGameEntity>().GetFirstScriptOfType<SynchedMissionObject>();
				}
			}
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000BA454 File Offset: 0x000B8654
		protected void OnNextDestructionState()
		{
			this.CollectDynamicGameEntities(false);
			this.UpdateDoorBodies(true);
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000BA464 File Offset: 0x000B8664
		protected void CollectDynamicGameEntities(bool calledFromOnInit)
		{
			this._attackOnlyDoorColliders.Clear();
			List<WeakGameEntity> list;
			if (calledFromOnInit)
			{
				list = base.GameEntity.CollectChildrenEntitiesWithTag("gate").ToList<WeakGameEntity>();
				this._leftExtraColliderDisabled = false;
				this._rightExtraColliderDisabled = false;
				this._agentColliderLeft = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("collider_agent_l"));
				this._agentColliderRight = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("collider_agent_r"));
			}
			else
			{
				list = (from x in base.GameEntity.CollectChildrenEntitiesWithTag("gate")
					where x.IsVisibleIncludeParents()
					select x).ToList<WeakGameEntity>();
			}
			if (list.Count == 0)
			{
				return;
			}
			if (list.Count > 1)
			{
				int num = int.MinValue;
				int num2 = int.MaxValue;
				WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
				WeakGameEntity weakGameEntity2 = WeakGameEntity.Invalid;
				foreach (WeakGameEntity weakGameEntity3 in list)
				{
					int num3 = int.Parse(weakGameEntity3.Tags.FirstOrDefault<string>((string x) => x.Contains("state_")).Split(new char[] { '_' }).Last<string>());
					if (num3 > num)
					{
						num = num3;
						weakGameEntity = weakGameEntity3;
					}
					if (num3 < num2)
					{
						num2 = num3;
						weakGameEntity2 = weakGameEntity3;
					}
				}
				this._door = (calledFromOnInit ? weakGameEntity2.GetFirstScriptOfType<SynchedMissionObject>() : weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>());
			}
			else
			{
				this._door = list[0].GetFirstScriptOfType<SynchedMissionObject>();
			}
			this._doorSkeleton = this._door.GameEntity.Skeleton;
			WeakGameEntity weakGameEntity4 = this._door.GameEntity.CollectChildrenEntitiesWithTag("collider_r").FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity4.IsValid)
			{
				this._attackOnlyDoorColliders.Add(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity4));
			}
			WeakGameEntity weakGameEntity5 = this._door.GameEntity.CollectChildrenEntitiesWithTag("collider_l").FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity5.IsValid)
			{
				this._attackOnlyDoorColliders.Add(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity5));
			}
			if (!weakGameEntity4.IsValid || !weakGameEntity5.IsValid)
			{
				GameEntity agentColliderLeft = this._agentColliderLeft;
				if (agentColliderLeft != null)
				{
					agentColliderLeft.SetVisibilityExcludeParents(false);
				}
				GameEntity agentColliderRight = this._agentColliderRight;
				if (agentColliderRight != null)
				{
					agentColliderRight.SetVisibilityExcludeParents(false);
				}
			}
			WeakGameEntity weakGameEntity6 = this._door.GameEntity.CollectChildrenEntitiesWithTag(this.ExtraCollisionObjectTagLeft).FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity6.IsValid)
			{
				if (!this.ActivateExtraColliders)
				{
					weakGameEntity6.RemovePhysics(false);
				}
				else
				{
					if (!calledFromOnInit)
					{
						MatrixFrame matrixFrame = ((this._extraColliderLeft != null) ? this._extraColliderLeft.GetFrame() : this._doorSkeleton.GetBoneEntitialFrameWithName(this.LeftDoorBoneName));
						this._extraColliderLeft = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity6);
						this._extraColliderLeft.SetFrame(ref matrixFrame, true);
					}
					else
					{
						this._extraColliderLeft = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity6);
					}
					if (this._leftExtraColliderDisabled)
					{
						this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag | BodyFlags.Disabled);
					}
					else
					{
						this._extraColliderLeft.SetBodyFlags(this._extraColliderLeft.BodyFlag & ~BodyFlags.Disabled);
					}
				}
			}
			WeakGameEntity weakGameEntity7 = this._door.GameEntity.CollectChildrenEntitiesWithTag(this.ExtraCollisionObjectTagRight).FirstOrDefault<WeakGameEntity>();
			if (weakGameEntity7.IsValid)
			{
				if (!this.ActivateExtraColliders)
				{
					weakGameEntity7.RemovePhysics(false);
				}
				else
				{
					if (!calledFromOnInit)
					{
						MatrixFrame matrixFrame2 = ((this._extraColliderRight != null) ? this._extraColliderRight.GetFrame() : this._doorSkeleton.GetBoneEntitialFrameWithName(this.RightDoorBoneName));
						this._extraColliderRight = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity7);
						this._extraColliderRight.SetFrame(ref matrixFrame2, true);
					}
					else
					{
						this._extraColliderRight = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity7);
					}
					if (this._rightExtraColliderDisabled)
					{
						this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag | BodyFlags.Disabled);
					}
					else
					{
						this._extraColliderRight.SetBodyFlags(this._extraColliderRight.BodyFlag & ~BodyFlags.Disabled);
					}
				}
			}
			if (this._door != null && this._doorSkeleton != null)
			{
				this._leftDoorBoneIndex = Skeleton.GetBoneIndexFromName(this._doorSkeleton.GetName(), this.LeftDoorBoneName);
				this._rightDoorBoneIndex = Skeleton.GetBoneIndexFromName(this._doorSkeleton.GetName(), this.RightDoorBoneName);
			}
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000BA8E4 File Offset: 0x000B8AE4
		private void InitializeExtraColliderPositions()
		{
			if (this._extraColliderLeft != null)
			{
				MatrixFrame boneEntitialFrameWithName = this._doorSkeleton.GetBoneEntitialFrameWithName(this.LeftDoorBoneName);
				this._extraColliderLeft.SetFrame(ref boneEntitialFrameWithName, true);
				this._extraColliderLeft.SetVisibilityExcludeParents(true);
			}
			if (this._extraColliderRight != null)
			{
				MatrixFrame boneEntitialFrameWithName2 = this._doorSkeleton.GetBoneEntitialFrameWithName(this.RightDoorBoneName);
				this._extraColliderRight.SetFrame(ref boneEntitialFrameWithName2, true);
				this._extraColliderRight.SetVisibilityExcludeParents(true);
			}
			this.UpdateDoorBodies(true);
			foreach (GameEntity gameEntity in this._attackOnlyDoorColliders)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
			if (this._agentColliderLeft != null)
			{
				this._agentColliderLeft.SetVisibilityExcludeParents(true);
			}
			if (this._agentColliderRight != null)
			{
				this._agentColliderRight.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000BA9E4 File Offset: 0x000B8BE4
		private void OnHitTaken(DestructableComponent hitComponent, Agent hitterAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			if (!GameNetwork.IsClientOrReplay && inflictedDamage >= 200 && this.State == CastleGate.GateState.Closed && attackerScriptComponentBehavior is BatteringRam)
			{
				SynchedMissionObject plank = this._plank;
				if (plank != null)
				{
					plank.SetAnimationAtChannelSynched(this.PlankHitAnimationName, 0, 1f);
				}
				this._door.SetAnimationAtChannelSynched(this.HitAnimationName, 0, 1f);
				Mission.Current.MakeSound(CastleGate.BatteringRamHitSoundIdCache, base.GameEntity.GlobalPosition, false, true, -1, -1);
			}
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000BAA68 File Offset: 0x000B8C68
		private void OnDestroyed(DestructableComponent destroyedComponent, Agent destroyerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				SynchedMissionObject plank = this._plank;
				if (plank != null)
				{
					plank.SetVisibleSynched(false, false);
				}
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.SetIsDeactivatedSynched(true);
				}
				if (attackerScriptComponentBehavior is BatteringRam)
				{
					this._door.SetAnimationAtChannelSynched(this.DestroyAnimationName, 0, 1f);
				}
				this.SetGateNavMeshState(true);
			}
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000BAAFC File Offset: 0x000B8CFC
		private int OnCalculateDestructionStateIndex(int destructionStateIndex, int inflictedDamage, int destructionStateCount)
		{
			if (inflictedDamage < 200)
			{
				return destructionStateIndex;
			}
			return MathF.Min(destructionStateIndex, destructionStateCount - 1);
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000BAB14 File Offset: 0x000B8D14
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (base.GameEntity.HasTag("outer_gate") && base.GameEntity.HasTag("inner_gate"))
			{
				MBEditor.AddEntityWarning(base.GameEntity, "This castle gate has both outer and inner tag at the same time.");
				flag = true;
			}
			if (base.GameEntity.CollectChildrenEntitiesWithTag("wait_pos").Count != 1)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "There must be one entity with wait position tag under castle gate.");
				flag = true;
			}
			if (base.GameEntity.HasTag("outer_gate"))
			{
				uint visibilityMask = base.GameEntity.GetVisibilityLevelMaskIncludingParents();
				WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.HasTag("middle_pos") && x.GetVisibilityLevelMaskIncludingParents() == visibilityMask);
				if (weakGameEntity.IsValid)
				{
					WeakGameEntity weakGameEntity2 = base.Scene.FindWeakEntitiesWithTag("inner_gate").FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.GetVisibilityLevelMaskIncludingParents() == visibilityMask);
					if (weakGameEntity2 != null)
					{
						if (weakGameEntity2.HasScriptOfType<CastleGate>())
						{
							Vec2 vec = weakGameEntity2.GlobalPosition.AsVec2 - weakGameEntity.GlobalPosition.AsVec2;
							Vec2 vec2 = base.GameEntity.GlobalPosition.AsVec2 - weakGameEntity.GlobalPosition.AsVec2;
							if (Vec2.DotProduct(vec, vec2) <= 0f)
							{
								MBEditor.AddEntityWarning(base.GameEntity, "Outer gate's middle position must not be between outer and inner gate.");
								flag = true;
							}
						}
						else
						{
							MBEditor.AddEntityWarning(base.GameEntity, weakGameEntity2.Name + " this entity has inner gate tag but doesn't have castle gate script.");
							flag = true;
						}
					}
					else
					{
						MBEditor.AddEntityWarning(base.GameEntity, "There is no entity with inner gate tag.");
						flag = true;
					}
				}
				else
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Outer gate doesn't have any middle positions");
					flag = true;
				}
			}
			Vec3 scaleVector = base.GameEntity.GetGlobalFrame().rotation.GetScaleVector();
			if (MathF.Abs(scaleVector.x - scaleVector.y) > 1E-05f || MathF.Abs(scaleVector.x - scaleVector.z) > 1E-05f || MathF.Abs(scaleVector.y - scaleVector.z) > 1E-05f)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "$$$ Non uniform scale on CastleGate at scene " + base.GameEntity.Scene.GetName());
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000BAD7D File Offset: 0x000B8F7D
		public Vec3 GetTargetingOffset()
		{
			return Vec3.Zero;
		}

		// Token: 0x04001332 RID: 4914
		public const string OuterGateTag = "outer_gate";

		// Token: 0x04001333 RID: 4915
		public const string InnerGateTag = "inner_gate";

		// Token: 0x04001334 RID: 4916
		private const float ExtraColliderScaleFactor = 1.1f;

		// Token: 0x04001335 RID: 4917
		private const string LeftDoorBodyTag = "collider_l";

		// Token: 0x04001336 RID: 4918
		private const string RightDoorBodyTag = "collider_r";

		// Token: 0x04001337 RID: 4919
		private const string RightDoorAgentOnlyBodyTag = "collider_agent_r";

		// Token: 0x04001338 RID: 4920
		private const string OpenTag = "open";

		// Token: 0x04001339 RID: 4921
		private const string CloseTag = "close";

		// Token: 0x0400133A RID: 4922
		private const string MiddlePositionTag = "middle_pos";

		// Token: 0x0400133B RID: 4923
		private const string WaitPositionTag = "wait_pos";

		// Token: 0x0400133C RID: 4924
		private const string LeftDoorAgentOnlyBodyTag = "collider_agent_l";

		// Token: 0x0400133D RID: 4925
		private const int HeavyBlowDamageLimit = 200;

		// Token: 0x0400133F RID: 4927
		private static int _batteringRamHitSoundId = -1;

		// Token: 0x04001341 RID: 4929
		public CastleGate.DoorOwnership OwningTeam;

		// Token: 0x04001342 RID: 4930
		public string OpeningAnimationName = "castle_gate_a_opening";

		// Token: 0x04001343 RID: 4931
		public string ClosingAnimationName = "castle_gate_a_closing";

		// Token: 0x04001344 RID: 4932
		public string HitAnimationName = "castle_gate_a_hit";

		// Token: 0x04001345 RID: 4933
		public string PlankHitAnimationName = "castle_gate_a_plank_hit";

		// Token: 0x04001346 RID: 4934
		public string HitMeleeAnimationName = "castle_gate_a_hit_melee";

		// Token: 0x04001347 RID: 4935
		public string DestroyAnimationName = "castle_gate_a_break";

		// Token: 0x04001348 RID: 4936
		public int NavigationMeshId = 1000;

		// Token: 0x04001349 RID: 4937
		public int NavigationMeshIdToDisableOnOpen = -1;

		// Token: 0x0400134A RID: 4938
		public string LeftDoorBoneName = "bn_bottom_l";

		// Token: 0x0400134B RID: 4939
		public string RightDoorBoneName = "bn_bottom_r";

		// Token: 0x0400134C RID: 4940
		public string ExtraCollisionObjectTagRight = "extra_collider_r";

		// Token: 0x0400134D RID: 4941
		public string ExtraCollisionObjectTagLeft = "extra_collider_l";

		// Token: 0x0400134E RID: 4942
		private int _openingAnimationIndex = -1;

		// Token: 0x0400134F RID: 4943
		private int _closingAnimationIndex = -1;

		// Token: 0x04001350 RID: 4944
		private bool _leftExtraColliderDisabled;

		// Token: 0x04001351 RID: 4945
		private bool _rightExtraColliderDisabled;

		// Token: 0x04001352 RID: 4946
		private bool _civilianMission;

		// Token: 0x04001353 RID: 4947
		public bool ActivateExtraColliders = true;

		// Token: 0x04001354 RID: 4948
		public string SideTag;

		// Token: 0x04001356 RID: 4950
		private bool _openNavMeshIdDisabled;

		// Token: 0x04001357 RID: 4951
		private SynchedMissionObject _door;

		// Token: 0x04001358 RID: 4952
		private Skeleton _doorSkeleton;

		// Token: 0x04001359 RID: 4953
		private GameEntity _extraColliderRight;

		// Token: 0x0400135A RID: 4954
		private GameEntity _extraColliderLeft;

		// Token: 0x0400135B RID: 4955
		private readonly List<GameEntity> _attackOnlyDoorColliders;

		// Token: 0x0400135C RID: 4956
		private float _previousAnimationProgress = -1f;

		// Token: 0x0400135D RID: 4957
		private GameEntity _agentColliderRight;

		// Token: 0x0400135E RID: 4958
		private GameEntity _agentColliderLeft;

		// Token: 0x0400135F RID: 4959
		private LadderQueueManager _queueManager;

		// Token: 0x04001360 RID: 4960
		private bool _afterMissionStartTriggered;

		// Token: 0x04001361 RID: 4961
		private sbyte _rightDoorBoneIndex;

		// Token: 0x04001362 RID: 4962
		private sbyte _leftDoorBoneIndex;

		// Token: 0x04001365 RID: 4965
		private AgentPathNavMeshChecker _pathChecker;

		// Token: 0x04001366 RID: 4966
		public bool AutoOpen;

		// Token: 0x04001367 RID: 4967
		private SynchedMissionObject _plank;

		// Token: 0x04001369 RID: 4969
		private WorldFrame _middleFrame;

		// Token: 0x0400136A RID: 4970
		private WorldFrame _defenseWaitFrame;

		// Token: 0x0400136B RID: 4971
		private Action DestructibleComponentOnMissionReset;

		// Token: 0x0200061C RID: 1564
		public enum DoorOwnership
		{
			// Token: 0x040020E5 RID: 8421
			Defenders,
			// Token: 0x040020E6 RID: 8422
			Attackers
		}

		// Token: 0x0200061D RID: 1565
		public enum GateState
		{
			// Token: 0x040020E8 RID: 8424
			Open,
			// Token: 0x040020E9 RID: 8425
			Closed
		}
	}
}
