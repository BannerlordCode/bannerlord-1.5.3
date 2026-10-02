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
	// Token: 0x0200035A RID: 858
	public class SiegeTower : SiegeWeapon, IPathHolder, IPrimarySiegeWeapon, IMoveableSiegeWeapon, ISpawnable
	{
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x000C6279 File Offset: 0x000C4479
		public MissionObject TargetCastlePosition
		{
			get
			{
				return this._targetWallSegment;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x000C6281 File Offset: 0x000C4481
		private WeakGameEntity CleanState
		{
			get
			{
				if (!(this._cleanState == null))
				{
					return this._cleanState.WeakEntity;
				}
				return base.GameEntity;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x000C62A3 File Offset: 0x000C44A3
		// (set) Token: 0x06003126 RID: 12582 RVA: 0x000C62AB File Offset: 0x000C44AB
		public FormationAI.BehaviorSide WeaponSide { get; private set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x000C62B4 File Offset: 0x000C44B4
		// (set) Token: 0x06003128 RID: 12584 RVA: 0x000C62BC File Offset: 0x000C44BC
		public string PathEntity { get; private set; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x000C62C5 File Offset: 0x000C44C5
		public bool EditorGhostEntityMove
		{
			get
			{
				return this.GhostEntityMove;
			}
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000C62CD File Offset: 0x000C44CD
		public bool HasCompletedAction()
		{
			return !base.IsDisabled && this.IsDeactivated && this._hasArrivedAtTarget && !base.IsDestroyed;
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x000C62F2 File Offset: 0x000C44F2
		public float SiegeWeaponPriority
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x000C62F9 File Offset: 0x000C44F9
		public int OverTheWallNavMeshID
		{
			get
			{
				return this.GetGateNavMeshId();
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x000C6301 File Offset: 0x000C4501
		// (set) Token: 0x0600312E RID: 12590 RVA: 0x000C6309 File Offset: 0x000C4509
		public SiegeWeaponMovementComponent MovementComponent { get; private set; }

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x000C6312 File Offset: 0x000C4512
		public bool HoldLadders
		{
			get
			{
				return !this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x000C6322 File Offset: 0x000C4522
		public bool SendLadders
		{
			get
			{
				return this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000C632F File Offset: 0x000C452F
		public int GetGateNavMeshId()
		{
			if (this.GateNavMeshId != 0)
			{
				return this.GateNavMeshId;
			}
			if (this.DynamicNavmeshIdStart == 0)
			{
				return 0;
			}
			return this.DynamicNavmeshIdStart + 3;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000C6354 File Offset: 0x000C4554
		public List<int> CollectGetDifficultNavmeshIDs()
		{
			List<int> list = new List<int>();
			if (!this._hasLadders)
			{
				return list;
			}
			list.Add(this.DynamicNavmeshIdStart + 1);
			list.Add(this.DynamicNavmeshIdStart + 5);
			list.Add(this.DynamicNavmeshIdStart + 6);
			list.Add(this.DynamicNavmeshIdStart + 7);
			return list;
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000C63AC File Offset: 0x000C45AC
		public List<int> CollectGetDifficultNavmeshIDsForAttackers()
		{
			List<int> list = new List<int>();
			if (!this._hasLadders)
			{
				return list;
			}
			list = this.CollectGetDifficultNavmeshIDs();
			list.Add(this.DynamicNavmeshIdStart + 3);
			return list;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000C63E0 File Offset: 0x000C45E0
		public List<int> CollectGetDifficultNavmeshIDsForDefenders()
		{
			List<int> list = new List<int>();
			if (!this._hasLadders)
			{
				return list;
			}
			list = this.CollectGetDifficultNavmeshIDs();
			list.Add(this.DynamicNavmeshIdStart + 2);
			return list;
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x000C6413 File Offset: 0x000C4613
		// (set) Token: 0x06003136 RID: 12598 RVA: 0x000C641C File Offset: 0x000C461C
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
					this.MovementComponent.SetDestinationNavMeshIdState(!this.HasArrivedAtTarget);
				}
				if (this._hasArrivedAtTarget != value)
				{
					this._hasArrivedAtTarget = value;
					if (this._hasArrivedAtTarget)
					{
						this.ActiveWaitStandingPoint = base.WaitStandingPoints[1];
						if (GameNetwork.IsClientOrReplay)
						{
							goto IL_00CA;
						}
						using (List<LadderQueueManager>.Enumerator enumerator = this._queueManagers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								LadderQueueManager ladderQueueManager = enumerator.Current;
								this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, true);
								ladderQueueManager.Activate();
							}
							goto IL_00CA;
						}
					}
					if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() > 0)
					{
						this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
					}
					IL_00CA:
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetSiegeTowerHasArrivedAtTarget(base.Id));
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

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003137 RID: 12599 RVA: 0x000C653C File Offset: 0x000C473C
		// (set) Token: 0x06003138 RID: 12600 RVA: 0x000C6544 File Offset: 0x000C4744
		public SiegeTower.GateState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetSiegeTowerGateState(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
					this._state = value;
					this.OnSiegeTowerGateStateChange();
				}
			}
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x000C6581 File Offset: 0x000C4781
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.IsValid || !gameEntity.HasScriptOfType<UsableMissionObject>() || gameEntity.HasTag("move"))
			{
				return new TextObject("{=aXjlMBiE}Siege Tower", null);
			}
			return new TextObject("{=6wZUG0ev}Gate", null);
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x000C65BC File Offset: 0x000C47BC
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = (usableGameObject.GameEntity.HasTag("move") ? new TextObject("{=rwZAZSvX}{KEY} Move", null) : new TextObject("{=5oozsaIb}{KEY} Open", null));
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x000C6618 File Offset: 0x000C4818
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.HasArrivedAtTarget);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeTowerGateStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this._fallAngularSpeed, CompressionMission.SiegeMachineComponentAngularSpeedCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.MovementComponent.GetTotalDistanceTraveledForPathTracker(), CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000C666B File Offset: 0x000C486B
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
			if (this.HasCompletedAction())
			{
				return OrderType.Use;
			}
			return OrderType.FollowEntity;
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000C668C File Offset: 0x000C488C
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			if (base.UserCountNotInStruckAction > 0)
			{
				targetFlags |= TargetFlags.IsMoving;
			}
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
			if (this.HasCompletedAction() || base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToTower)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags | TargetFlags.IsSiegeTower;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000C66F0 File Offset: 0x000C48F0
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 90f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000C6714 File Offset: 0x000C4914
		public override void Disable()
		{
			base.Disable();
			this.SetAbilityOfFaces(false);
			if (this._queueManagers != null)
			{
				foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, false);
					ladderQueueManager.DeactivateImmediate();
				}
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000C6798 File Offset: 0x000C4998
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.SiegeTower;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000C679F File Offset: 0x000C499F
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new SiegeTowerAI(this);
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x000C67A7 File Offset: 0x000C49A7
		public override bool IsDeactivated
		{
			get
			{
				return (this.MovementComponent.HasArrivedAtTarget && this.State == SiegeTower.GateState.Open) || base.IsDeactivated;
			}
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000C67C8 File Offset: 0x000C49C8
		protected internal override void OnDeploymentStateChanged(bool isDeployed)
		{
			base.OnDeploymentStateChanged(isDeployed);
			if (this._ditchFillDebris != null)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this._ditchFillDebris.SetVisibleSynched(isDeployed, false);
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					if (isDeployed)
					{
						if (this._soilGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilGenericNavMeshID, true);
						}
						if (this._soilNavMeshID1 > 0 && this._groundToSoilNavMeshID1 > 0 && this._ditchNavMeshID1 > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID1, true);
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID1, this._ditchNavMeshID1, this._soilNavMeshID1, false);
						}
						if (this._soilNavMeshID2 > 0 && this._groundToSoilNavMeshID2 > 0 && this._ditchNavMeshID2 > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID2, true);
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID2, this._ditchNavMeshID2, this._soilNavMeshID2, false);
						}
						if (this._groundGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._groundGenericNavMeshID, false);
						}
					}
					else
					{
						if (this._groundGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._groundGenericNavMeshID, true);
						}
						if (this._soilNavMeshID1 > 0 && this._groundToSoilNavMeshID1 > 0 && this._ditchNavMeshID1 > 0)
						{
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID1, this._soilNavMeshID1, this._ditchNavMeshID1, false);
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID1, false);
						}
						if (this._soilNavMeshID2 > 0 && this._groundToSoilNavMeshID2 > 0 && this._ditchNavMeshID2 > 0)
						{
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID2, this._soilNavMeshID2, this._ditchNavMeshID2, false);
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID2, false);
						}
						if (this._soilGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilGenericNavMeshID, false);
						}
					}
				}
			}
			if (this._sameSideSiegeLadders == null)
			{
				this._sameSideSiegeLadders = (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
					where sl.WeaponSide == this.WeaponSide
					select sl).ToList<SiegeLadder>();
			}
			foreach (SiegeLadder siegeLadder in this._sameSideSiegeLadders)
			{
				siegeLadder.GameEntity.SetVisibilityExcludeParents(!isDeployed);
			}
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000C6A6C File Offset: 0x000C4C6C
		protected override void AttachDynamicNavmeshToEntity()
		{
			if (this.NavMeshPrefabName.Length > 0)
			{
				this.DynamicNavmeshIdStart = Mission.Current.GetNextDynamicNavMeshIdStart();
				this.CleanState.Scene.ImportNavigationMeshPrefab(this.NavMeshPrefabName, this.DynamicNavmeshIdStart);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 1, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 2, true, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 4, false, true, false, true, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 5, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 6, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 7, false, false, false, false, true);
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000C6B5B File Offset: 0x000C4D5B
		protected override WeakGameEntity GetEntityToAttachNavMeshFaces()
		{
			return this.CleanState;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x000C6B63 File Offset: 0x000C4D63
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			SiegeWeaponMovementComponent movementComponent = this.MovementComponent;
			if (movementComponent == null)
			{
				return;
			}
			movementComponent.OnRemoved();
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000C6B7C File Offset: 0x000C4D7C
		public override void SetAbilityOfFaces(bool enabled)
		{
			base.SetAbilityOfFaces(enabled);
			if (this._queueManagers != null)
			{
				foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, enabled);
					if (ladderQueueManager.IsDeactivated != !enabled)
					{
						if (enabled)
						{
							ladderQueueManager.Activate();
						}
						else
						{
							ladderQueueManager.DeactivateImmediate();
						}
					}
				}
			}
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000C6C10 File Offset: 0x000C4E10
		protected override float GetDistanceMultiplierOfWeapon(Vec3 weaponPos)
		{
			float minimumDistanceBetweenPositions = this.GetMinimumDistanceBetweenPositions(weaponPos);
			if (minimumDistanceBetweenPositions < 10f)
			{
				return 1f;
			}
			if (minimumDistanceBetweenPositions < 25f)
			{
				return 0.8f;
			}
			return 0.6f;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000C6C48 File Offset: 0x000C4E48
		private bool IsNavmeshOnThisTowerAttackerDifficultNavmeshIDs(int testedNavmeshID)
		{
			return this._hasLadders && (testedNavmeshID == this.DynamicNavmeshIdStart + 1 || testedNavmeshID == this.DynamicNavmeshIdStart + 5 || testedNavmeshID == this.DynamicNavmeshIdStart + 6 || testedNavmeshID == this.DynamicNavmeshIdStart + 7 || testedNavmeshID == this.DynamicNavmeshIdStart + 3);
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000C6C98 File Offset: 0x000C4E98
		protected override bool IsAgentOnInconvenientNavmesh(Agent agent, StandingPoint standingPoint)
		{
			if (Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege)
			{
				return false;
			}
			int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = agent.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				if (teamAISiegeComponent is TeamAISiegeDefender && currentNavigationFaceId % 10 != 1)
				{
					return true;
				}
				foreach (int num in teamAISiegeComponent.DifficultNavmeshIDs)
				{
					if (currentNavigationFaceId == num)
					{
						return standingPoint != this._gateStandingPoint || !this.IsNavmeshOnThisTowerAttackerDifficultNavmeshIDs(currentNavigationFaceId);
					}
				}
				if (teamAISiegeComponent is TeamAISiegeAttacker && currentNavigationFaceId % 10 == 1)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000C6D54 File Offset: 0x000C4F54
		protected internal override void OnInit()
		{
			this._cleanState = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("body"));
			base.OnInit();
			base.DestructionComponent.OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnDestroyed);
			base.DestructionComponent.BattleSide = BattleSideEnum.Attacker;
			this._aiBarriers = base.Scene.FindEntitiesWithTag(this.BarrierTagToRemove).ToList<GameEntity>();
			if (!GameNetwork.IsClientOrReplay && this._soilGenericNavMeshID > 0)
			{
				this.CleanState.Scene.SetAbilityOfFacesWithId(this._soilGenericNavMeshID, false);
			}
			List<SynchedMissionObject> list = this.CleanState.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.GateTag);
			if (list.Count > 0)
			{
				this._gateObject = list[0];
			}
			this.AddRegularMovementComponent();
			List<GameEntity> list2 = base.Scene.FindEntitiesWithTag("breakable_wall").ToList<GameEntity>();
			if (!list2.IsEmpty<GameEntity>())
			{
				float num = 10000000f;
				GameEntity gameEntity = null;
				MatrixFrame targetFrame = this.MovementComponent.GetTargetFrame();
				foreach (GameEntity gameEntity2 in list2)
				{
					float lengthSquared = (gameEntity2.GlobalPosition - targetFrame.origin).LengthSquared;
					if (lengthSquared < num)
					{
						num = lengthSquared;
						gameEntity = gameEntity2;
					}
				}
				list2 = gameEntity.CollectChildrenEntitiesWithTag("destroyed");
				if (list2.Count > 0)
				{
					this._destroyedWallEntity = list2[0];
				}
				list2 = gameEntity.CollectChildrenEntitiesWithTag("non_destroyed");
				if (list2.Count > 0)
				{
					this._nonDestroyedWallEntity = list2[0];
				}
				list2 = gameEntity.CollectChildrenEntitiesWithTag("particle_spawnpoint");
				if (list2.Count > 0)
				{
					this._battlementDestroyedParticle = list2[0];
				}
			}
			list = this.CleanState.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.HandleTag);
			this._handleObject = ((list.Count < 1) ? null : list[0]);
			this._gateHandleIdleAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.GateHandleIdleAnimation);
			this._gateTrembleAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.GateTrembleAnimation);
			this._queueManagers = new List<LadderQueueManager>();
			if (!GameNetwork.IsClientOrReplay)
			{
				List<WeakGameEntity> list3 = this.CleanState.CollectChildrenEntitiesWithTag("ladder");
				if (list3.Count > 0)
				{
					this._hasLadders = true;
					WeakGameEntity weakGameEntity = list3.ElementAt<WeakGameEntity>(list3.Count / 2);
					foreach (WeakGameEntity weakGameEntity2 in list3)
					{
						if (weakGameEntity2.Name.Contains("middle"))
						{
							weakGameEntity = weakGameEntity2;
						}
						else
						{
							LadderQueueManager firstScriptOfType = weakGameEntity2.GetFirstScriptOfType<LadderQueueManager>();
							firstScriptOfType.Initialize(-1, MatrixFrame.Identity, Vec3.Zero, BattleSideEnum.None, int.MaxValue, 1f, 5f, 5f, 5f, 0f, false, 1f, 0f, 0f, false, -1, -1, int.MaxValue, int.MaxValue);
							firstScriptOfType.DeactivateImmediate();
						}
					}
					int num2 = 0;
					int num3 = 1;
					for (int i = base.GameEntity.Name.Length - 1; i >= 0; i--)
					{
						if (char.IsDigit(base.GameEntity.Name[i]))
						{
							num2 += (int)(base.GameEntity.Name[i] - '0') * num3;
							num3 *= 10;
						}
						else if (num2 > 0)
						{
							break;
						}
					}
					LadderQueueManager firstScriptOfType2 = weakGameEntity.GetFirstScriptOfType<LadderQueueManager>();
					if (firstScriptOfType2 != null)
					{
						MatrixFrame identity = MatrixFrame.Identity;
						identity.rotation.RotateAboutSide(1.5707964f);
						identity.rotation.RotateAboutForward(0.3926991f);
						firstScriptOfType2.Initialize(this.DynamicNavmeshIdStart + 5, identity, new Vec3(0f, 0f, 1f, -1f), BattleSideEnum.Attacker, list3.Count * 2, 0.7853982f, 2f, 1f, 4f, 3f, false, 0.8f, (float)num2 * 2f / 5f, 5f, list3.Count > 1, this.DynamicNavmeshIdStart + 6, this.DynamicNavmeshIdStart + 7, num2 * MathF.Round((float)list3.Count * 0.666f), list3.Count + 1);
						this._queueManagers.Add(firstScriptOfType2);
					}
					base.GameEntity.Scene.MarkFacesWithIdAsLadder(5, true);
					base.GameEntity.Scene.MarkFacesWithIdAsLadder(6, true);
					base.GameEntity.Scene.MarkFacesWithIdAsLadder(7, true);
				}
				else
				{
					this._hasLadders = false;
					LadderQueueManager firstScriptOfType3 = this.CleanState.GetFirstScriptOfType<LadderQueueManager>();
					if (firstScriptOfType3 != null)
					{
						MatrixFrame identity2 = MatrixFrame.Identity;
						identity2.origin.y = identity2.origin.y + 4f;
						identity2.rotation.RotateAboutSide(-1.5707964f);
						identity2.rotation.RotateAboutUp(3.1415927f);
						firstScriptOfType3.Initialize(this.DynamicNavmeshIdStart + 2, identity2, new Vec3(0f, -1f, 0f, -1f), BattleSideEnum.Attacker, 15, 0.7853982f, 2f, 1f, 3f, 1f, false, 0.8f, 4f, 5f, false, -2, -2, int.MaxValue, 15);
						this._queueManagers.Add(firstScriptOfType3);
					}
				}
			}
			this._state = SiegeTower.GateState.Closed;
			this._gateOpenSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/siegetower/dooropen");
			this._closedStateRotation = this._gateObject.GameEntity.GetFrame().rotation;
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
				if (!standingPoint.GameEntity.HasTag("move"))
				{
					this._gateStandingPoint = standingPoint;
					standingPoint.IsDeactivated = true;
					MatrixFrame globalFrame = standingPoint.GameEntity.GetGlobalFrame();
					MatrixFrame globalFrame2 = this.CleanState.GetGlobalFrame();
					this._gateStandingPointLocalIKFrame = globalFrame.TransformToLocal(in globalFrame2);
					standingPoint.AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
				}
			}
			if (base.WaitStandingPoints[0].GlobalPosition.z > base.WaitStandingPoints[1].GlobalPosition.z)
			{
				List<GameEntity> waitStandingPoints = base.WaitStandingPoints;
				List<GameEntity> waitStandingPoints2 = base.WaitStandingPoints;
				GameEntity gameEntity3 = base.WaitStandingPoints[1];
				GameEntity gameEntity4 = base.WaitStandingPoints[0];
				waitStandingPoints[0] = gameEntity3;
				waitStandingPoints2[1] = gameEntity4;
				this.ActiveWaitStandingPoint = base.WaitStandingPoints[0];
			}
			IEnumerable<WeakGameEntity> enumerable = from entity in base.Scene.FindWeakEntitiesWithTag(this._targetWallSegmentTag).ToList<WeakGameEntity>()
				where entity.HasScriptOfType<WallSegment>()
				select entity;
			if (!enumerable.IsEmpty<WeakGameEntity>())
			{
				this._targetWallSegment = enumerable.First<WeakGameEntity>().GetFirstScriptOfType<WallSegment>();
				this._targetWallSegment.AttackerSiegeWeapon = this;
			}
			string sideTag = this._sideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this.WeaponSide = FormationAI.BehaviorSide.Middle;
					}
					else
					{
						this.WeaponSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this.WeaponSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this.WeaponSide = FormationAI.BehaviorSide.Left;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.GetGateNavMeshId() != 0)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
				}
				foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, false);
					ladderQueueManager.DeactivateImmediate();
				}
			}
			WeakGameEntity weakGameEntity3 = base.Scene.FindWeakEntitiesWithTag("ditch_filler").FirstOrDefault<WeakGameEntity>((WeakGameEntity df) => df.HasTag(this._sideTag));
			if (weakGameEntity3 != null)
			{
				this._ditchFillDebris = weakGameEntity3.GetFirstScriptOfType<SynchedMissionObject>();
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this._gateObject.GameEntity.AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 3, true, false, false, false, true);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
			Mission.Current.AddToWeaponListForFriendlyFirePreventing(this);
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000C7614 File Offset: 0x000C5814
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000C7644 File Offset: 0x000C5844
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!this.CleanState.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.GameEntity.HasTag("move"))
					{
						standingPoint.SetIsDeactivatedSynched(this.MovementComponent.HasArrivedAtTarget);
					}
					else
					{
						UsableMissionObject usableMissionObject = standingPoint;
						bool flag;
						if (this.MovementComponent.HasArrivedAtTarget && this.State != SiegeTower.GateState.Open)
						{
							if (this.State == SiegeTower.GateState.GateFalling || this.State == SiegeTower.GateState.GateFallingWallDestroyed)
							{
								Agent userAgent = standingPoint.UserAgent;
								flag = userAgent != null && userAgent.IsPlayerControlled;
							}
							else
							{
								flag = false;
							}
						}
						else
						{
							flag = true;
						}
						usableMissionObject.SetIsDeactivatedSynched(flag);
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay && this.MovementComponent.HasArrivedAtTarget && !this.HasArrivedAtTarget)
			{
				this.HasArrivedAtTarget = true;
				this.ActiveWaitStandingPoint = base.WaitStandingPoints[1];
			}
			if (this.HasArrivedAtTarget)
			{
				switch (this.State)
				{
				case SiegeTower.GateState.Closed:
					if (!GameNetwork.IsClientOrReplay && base.UserCountNotInStruckAction > 0)
					{
						this.State = SiegeTower.GateState.GateFalling;
						return;
					}
					break;
				case SiegeTower.GateState.Open:
					break;
				case SiegeTower.GateState.GateFalling:
				{
					MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
					frame.rotation.RotateAboutSide(this._fallAngularSpeed * dt);
					this._gateObject.GameEntity.SetFrame(ref frame, true);
					if (Vec3.DotProduct(frame.rotation.u, this._openStateRotation.f) < 0.025f)
					{
						this.State = SiegeTower.GateState.GateFallingWallDestroyed;
					}
					this._fallAngularSpeed += dt * 2f * MathF.Max(0.3f, 1f - frame.rotation.u.z);
					return;
				}
				case SiegeTower.GateState.GateFallingWallDestroyed:
				{
					MatrixFrame frame2 = this._gateObject.GameEntity.GetFrame();
					frame2.rotation.RotateAboutSide(this._fallAngularSpeed * dt);
					this._gateObject.GameEntity.SetFrame(ref frame2, true);
					float num = Vec3.DotProduct(frame2.rotation.u, this._openStateRotation.f);
					if (this._fallAngularSpeed > 0f && num < 0.05f)
					{
						frame2.rotation = this._openStateRotation;
						this._gateObject.GameEntity.SetFrame(ref frame2, true);
						this._gateObject.GameEntity.Skeleton.SetAnimationAtChannel(this._gateTrembleAnimationIndex, 0, 1f, -1f, 0f);
						SoundEvent gateOpenSound = this._gateOpenSound;
						if (gateOpenSound != null)
						{
							gateOpenSound.Stop();
						}
						if (!GameNetwork.IsClientOrReplay)
						{
							this.State = SiegeTower.GateState.Open;
						}
					}
					this._fallAngularSpeed += dt * 3f * MathF.Max(0.3f, 1f - frame2.rotation.u.z);
					return;
				}
				default:
					Debug.FailedAssert("Invalid gate state.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SiegeTower.cs", "OnTick", 960);
					break;
				}
			}
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000C7970 File Offset: 0x000C5B70
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!this.CleanState.IsVisibleIncludeParents())
			{
				return;
			}
			this.MovementComponent.TickParallelManually(dt);
			if (this._gateStandingPoint.HasUser)
			{
				Agent userAgent = this._gateStandingPoint.UserAgent;
				if (userAgent.IsInBeingStruckAction)
				{
					userAgent.ClearHandInverseKinematics();
					return;
				}
				Agent userAgent2 = this._gateStandingPoint.UserAgent;
				MatrixFrame globalFrame = this.CleanState.GetGlobalFrame();
				userAgent2.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._gateStandingPointLocalIKFrame, in globalFrame, 0f);
			}
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000C79F8 File Offset: 0x000C5BF8
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() > 0)
			{
				this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
			}
			this._state = SiegeTower.GateState.Closed;
			this._hasArrivedAtTarget = false;
			MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
			frame.rotation = this._closedStateRotation;
			SynchedMissionObject handleObject = this._handleObject;
			if (handleObject != null)
			{
				handleObject.GameEntity.Skeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
			}
			this._gateObject.GameEntity.Skeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
			this._gateObject.GameEntity.SetFrame(ref frame, true);
			if (this._destroyedWallEntity != null && this._nonDestroyedWallEntity != null)
			{
				this._nonDestroyedWallEntity.SetVisibilityExcludeParents(false);
				this._destroyedWallEntity.SetVisibilityExcludeParents(true);
			}
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.IsDeactivated = !standingPoint.GameEntity.HasTag("move");
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000C7B60 File Offset: 0x000C5D60
		public void OnDestroyed(DestructableComponent destroyedComponent, Agent destroyerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			bool flag = false;
			MissionWeapon missionWeapon = weapon;
			if (missionWeapon.CurrentUsageItem != null)
			{
				missionWeapon = weapon;
				bool flag2;
				if (missionWeapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.Burning))
				{
					missionWeapon = weapon;
					flag2 = missionWeapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.AffectsArea | WeaponFlags.AffectsAreaBig);
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
			}
			Mission.Current.KillAgentsOnEntity(destroyedComponent.CurrentState, destroyerAgent, flag);
			foreach (GameEntity gameEntity in this._aiBarriers)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000C7C18 File Offset: 0x000C5E18
		public void HighlightPath()
		{
			this.MovementComponent.HighlightPath();
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000C7C28 File Offset: 0x000C5E28
		public void SwitchGhostEntityMovementMode(bool isGhostEnabled)
		{
			if (isGhostEnabled)
			{
				if (!this._isGhostMovementOn)
				{
					base.RemoveComponent(this.MovementComponent);
					this.GhostEntityMove = true;
					this.MovementComponent.GhostEntitySpeedMultiplier *= 3f;
					this.MovementComponent.SetGhostVisibility(true);
				}
				this._isGhostMovementOn = true;
				return;
			}
			if (this._isGhostMovementOn)
			{
				base.RemoveComponent(this.MovementComponent);
				PathLastNodeFixer component = base.GetComponent<PathLastNodeFixer>();
				base.RemoveComponent(component);
				this.AddRegularMovementComponent();
				this.MovementComponent.SetGhostVisibility(false);
			}
			this._isGhostMovementOn = false;
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000C7CBC File Offset: 0x000C5EBC
		public MatrixFrame GetInitialFrame()
		{
			SiegeWeaponMovementComponent movementComponent = this.MovementComponent;
			if (movementComponent == null)
			{
				return this.CleanState.GetGlobalFrame();
			}
			return movementComponent.GetInitialFrame();
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000C7CE8 File Offset: 0x000C5EE8
		private void OnSiegeTowerGateStateChange()
		{
			switch (this.State)
			{
			case SiegeTower.GateState.Closed:
			{
				SynchedMissionObject handleObject = this._handleObject;
				if (handleObject != null)
				{
					handleObject.GameEntity.Skeleton.SetAnimationAtChannel(this._gateHandleIdleAnimationIndex, 0, 1f, -1f, 0f);
				}
				if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() != 0)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
					return;
				}
				break;
			}
			case SiegeTower.GateState.Open:
				if (this._gateObject.GameEntity.Skeleton.GetAnimationIndexAtChannel(0) != this._gateHandleIdleAnimationIndex)
				{
					MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
					frame.rotation = this._openStateRotation;
					this._gateObject.GameEntity.SetFrame(ref frame, true);
					this._gateObject.GameEntity.Skeleton.SetAnimationAtChannel(this._gateTrembleAnimationIndex, 0, 1f, -1f, 0f);
					SoundEvent gateOpenSound = this._gateOpenSound;
					if (gateOpenSound != null)
					{
						gateOpenSound.Stop();
					}
					if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() != 0)
					{
						this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), true);
					}
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), true);
				}
				foreach (GameEntity gameEntity in this._aiBarriers)
				{
					gameEntity.SetVisibilityExcludeParents(false);
				}
				break;
			case SiegeTower.GateState.GateFalling:
				this._fallAngularSpeed = 0f;
				this._gateOpenSound = SoundEvent.CreateEvent(this._gateOpenSoundIndex, base.Scene);
				this._gateOpenSound.PlayInPosition(this._gateObject.GameEntity.GlobalPosition);
				return;
			case SiegeTower.GateState.GateFallingWallDestroyed:
				if (this._destroyedWallEntity != null && this._nonDestroyedWallEntity != null)
				{
					this._fallAngularSpeed *= 0.1f;
					this._nonDestroyedWallEntity.SetVisibilityExcludeParents(false);
					this._destroyedWallEntity.SetVisibilityExcludeParents(true);
					if (this._battlementDestroyedParticle != null)
					{
						Mission.Current.AddParticleSystemBurstByName(this.BattlementDestroyedParticle, this._battlementDestroyedParticle.GetGlobalFrame(), false);
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000C7F60 File Offset: 0x000C6160
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
				MovementSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/siege/siegetower/move"),
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000C7FE4 File Offset: 0x000C61E4
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

		// Token: 0x06003157 RID: 12631 RVA: 0x000C8048 File Offset: 0x000C6248
		private void UpdateGhostEntity()
		{
			WeakGameEntity firstChildEntityWithTag = this.CleanState.GetFirstChildEntityWithTag("ghost_object");
			if (firstChildEntityWithTag.IsValid && firstChildEntityWithTag.ChildCount > 0)
			{
				this.MovementComponent.GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier;
				WeakGameEntity child = firstChildEntityWithTag.GetChild(0);
				MatrixFrame frame = child.GetFrame();
				child.SetFrame(ref frame, true);
			}
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x000C80A8 File Offset: 0x000C62A8
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x000C80B4 File Offset: 0x000C62B4
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			SiegeTower.SiegeTowerRecord siegeTowerRecord = (SiegeTower.SiegeTowerRecord)synchedMissionObjectReadableRecord.Item2;
			this.HasArrivedAtTarget = siegeTowerRecord.HasArrivedAtTarget;
			this._state = (SiegeTower.GateState)siegeTowerRecord.State;
			this._fallAngularSpeed = siegeTowerRecord.FallAngularSpeed;
			if (this._state == SiegeTower.GateState.Open)
			{
				if (this._destroyedWallEntity != null && this._nonDestroyedWallEntity != null)
				{
					this._nonDestroyedWallEntity.SetVisibilityExcludeParents(false);
					this._destroyedWallEntity.SetVisibilityExcludeParents(true);
				}
				MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
				frame.rotation = this._openStateRotation;
				this._gateObject.GameEntity.SetFrame(ref frame, true);
			}
			float num = siegeTowerRecord.TotalDistanceTraveled;
			num += 0.05f;
			this.MovementComponent.SetTotalDistanceTraveledForPathTracker(num);
			this.MovementComponent.SetTargetFrameForPathTracker();
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000C8198 File Offset: 0x000C6398
		public void AssignParametersFromSpawner(string pathEntityName, string targetWallSegment, string sideTag, int soilNavMeshID1, int soilNavMeshID2, int ditchNavMeshID1, int ditchNavMeshID2, int groundToSoilNavMeshID1, int groundToSoilNavMeshID2, int soilGenericNavMeshID, int groundGenericNavMeshID, Mat3 openStateRotation, string barrierTagToRemove)
		{
			this.PathEntity = pathEntityName;
			this._targetWallSegmentTag = targetWallSegment;
			this._sideTag = sideTag;
			this._soilNavMeshID1 = soilNavMeshID1;
			this._soilNavMeshID2 = soilNavMeshID2;
			this._ditchNavMeshID1 = ditchNavMeshID1;
			this._ditchNavMeshID2 = ditchNavMeshID2;
			this._groundToSoilNavMeshID1 = groundToSoilNavMeshID1;
			this._groundToSoilNavMeshID2 = groundToSoilNavMeshID2;
			this._soilGenericNavMeshID = soilGenericNavMeshID;
			this._groundGenericNavMeshID = groundGenericNavMeshID;
			this._openStateRotation = openStateRotation;
			this.BarrierTagToRemove = barrierTagToRemove;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000C820C File Offset: 0x000C640C
		public bool GetNavmeshFaceIds(out List<int> navmeshFaceIds)
		{
			navmeshFaceIds = new List<int>
			{
				this.DynamicNavmeshIdStart + 1,
				this.DynamicNavmeshIdStart + 3,
				this.DynamicNavmeshIdStart + 5,
				this.DynamicNavmeshIdStart + 6,
				this.DynamicNavmeshIdStart + 7
			};
			return true;
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000C8268 File Offset: 0x000C6468
		public void OnFormationFrameChanged(Agent agent, bool hasFrame, WorldPosition frame)
		{
			foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
			{
				ladderQueueManager.OnFormationFrameChanged(agent, hasFrame, frame);
			}
		}

		// Token: 0x04001492 RID: 5266
		private const int LeftLadderNavMeshIdLocal = 5;

		// Token: 0x04001493 RID: 5267
		private const int MiddleLadderNavMeshIdLocal = 6;

		// Token: 0x04001494 RID: 5268
		private const int RightLadderNavMeshIdLocal = 7;

		// Token: 0x04001495 RID: 5269
		private const string BreakableWallTag = "breakable_wall";

		// Token: 0x04001496 RID: 5270
		private const string DestroyedWallTag = "destroyed";

		// Token: 0x04001497 RID: 5271
		private const string NonDestroyedWallTag = "non_destroyed";

		// Token: 0x04001498 RID: 5272
		private const string LadderTag = "ladder";

		// Token: 0x04001499 RID: 5273
		private const string BattlementDestroyedParticleTag = "particle_spawnpoint";

		// Token: 0x0400149A RID: 5274
		public string GateTag = "gate";

		// Token: 0x0400149B RID: 5275
		public string GateOpenTag = "gateOpen";

		// Token: 0x0400149C RID: 5276
		public string HandleTag = "handle";

		// Token: 0x0400149D RID: 5277
		public string GateHandleIdleAnimation = "siegetower_handle_idle";

		// Token: 0x0400149E RID: 5278
		private int _gateHandleIdleAnimationIndex = -1;

		// Token: 0x0400149F RID: 5279
		public string GateTrembleAnimation = "siegetower_door_stop";

		// Token: 0x040014A0 RID: 5280
		private int _gateTrembleAnimationIndex = -1;

		// Token: 0x040014A1 RID: 5281
		public string BattlementDestroyedParticle = "psys_adobe_battlement_destroyed";

		// Token: 0x040014A2 RID: 5282
		private string _targetWallSegmentTag;

		// Token: 0x040014A3 RID: 5283
		public bool GhostEntityMove = true;

		// Token: 0x040014A4 RID: 5284
		public float GhostEntitySpeedMultiplier = 1f;

		// Token: 0x040014A5 RID: 5285
		private string _sideTag;

		// Token: 0x040014A6 RID: 5286
		private bool _hasLadders;

		// Token: 0x040014A7 RID: 5287
		public float WheelDiameter = 1.3f;

		// Token: 0x040014A8 RID: 5288
		public float MinSpeed = 0.5f;

		// Token: 0x040014A9 RID: 5289
		public float MaxSpeed = 1f;

		// Token: 0x040014AA RID: 5290
		public int GateNavMeshId;

		// Token: 0x040014AB RID: 5291
		public int NavMeshIdToDisableOnDestination = -1;

		// Token: 0x040014AC RID: 5292
		private int _soilNavMeshID1;

		// Token: 0x040014AD RID: 5293
		private int _soilNavMeshID2;

		// Token: 0x040014AE RID: 5294
		private int _ditchNavMeshID1;

		// Token: 0x040014AF RID: 5295
		private int _ditchNavMeshID2;

		// Token: 0x040014B0 RID: 5296
		private int _groundToSoilNavMeshID1;

		// Token: 0x040014B1 RID: 5297
		private int _groundToSoilNavMeshID2;

		// Token: 0x040014B2 RID: 5298
		private int _soilGenericNavMeshID;

		// Token: 0x040014B3 RID: 5299
		private int _groundGenericNavMeshID;

		// Token: 0x040014B4 RID: 5300
		public string BarrierTagToRemove = "barrier";

		// Token: 0x040014B5 RID: 5301
		private List<GameEntity> _aiBarriers;

		// Token: 0x040014B6 RID: 5302
		private bool _isGhostMovementOn;

		// Token: 0x040014B7 RID: 5303
		private bool _hasArrivedAtTarget;

		// Token: 0x040014B8 RID: 5304
		private SiegeTower.GateState _state;

		// Token: 0x040014B9 RID: 5305
		private SynchedMissionObject _gateObject;

		// Token: 0x040014BA RID: 5306
		private SynchedMissionObject _handleObject;

		// Token: 0x040014BB RID: 5307
		private SoundEvent _gateOpenSound;

		// Token: 0x040014BC RID: 5308
		private int _gateOpenSoundIndex = -1;

		// Token: 0x040014BD RID: 5309
		private Mat3 _openStateRotation;

		// Token: 0x040014BE RID: 5310
		private Mat3 _closedStateRotation;

		// Token: 0x040014BF RID: 5311
		private float _fallAngularSpeed;

		// Token: 0x040014C0 RID: 5312
		private GameEntity _cleanState;

		// Token: 0x040014C1 RID: 5313
		private GameEntity _destroyedWallEntity;

		// Token: 0x040014C2 RID: 5314
		private GameEntity _nonDestroyedWallEntity;

		// Token: 0x040014C3 RID: 5315
		private GameEntity _battlementDestroyedParticle;

		// Token: 0x040014C4 RID: 5316
		private StandingPoint _gateStandingPoint;

		// Token: 0x040014C5 RID: 5317
		private MatrixFrame _gateStandingPointLocalIKFrame;

		// Token: 0x040014C6 RID: 5318
		private SynchedMissionObject _ditchFillDebris;

		// Token: 0x040014C7 RID: 5319
		private List<LadderQueueManager> _queueManagers;

		// Token: 0x040014C8 RID: 5320
		private WallSegment _targetWallSegment;

		// Token: 0x040014C9 RID: 5321
		private List<SiegeLadder> _sameSideSiegeLadders;

		// Token: 0x0200063B RID: 1595
		[DefineSynchedMissionObjectType(typeof(SiegeTower))]
		public struct SiegeTowerRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AE2 RID: 2786
			// (get) Token: 0x060040E5 RID: 16613 RVA: 0x000FC88B File Offset: 0x000FAA8B
			// (set) Token: 0x060040E6 RID: 16614 RVA: 0x000FC893 File Offset: 0x000FAA93
			public bool HasArrivedAtTarget { get; private set; }

			// Token: 0x17000AE3 RID: 2787
			// (get) Token: 0x060040E7 RID: 16615 RVA: 0x000FC89C File Offset: 0x000FAA9C
			// (set) Token: 0x060040E8 RID: 16616 RVA: 0x000FC8A4 File Offset: 0x000FAAA4
			public int State { get; private set; }

			// Token: 0x17000AE4 RID: 2788
			// (get) Token: 0x060040E9 RID: 16617 RVA: 0x000FC8AD File Offset: 0x000FAAAD
			// (set) Token: 0x060040EA RID: 16618 RVA: 0x000FC8B5 File Offset: 0x000FAAB5
			public float FallAngularSpeed { get; private set; }

			// Token: 0x17000AE5 RID: 2789
			// (get) Token: 0x060040EB RID: 16619 RVA: 0x000FC8BE File Offset: 0x000FAABE
			// (set) Token: 0x060040EC RID: 16620 RVA: 0x000FC8C6 File Offset: 0x000FAAC6
			public float TotalDistanceTraveled { get; private set; }

			// Token: 0x060040ED RID: 16621 RVA: 0x000FC8D0 File Offset: 0x000FAAD0
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.HasArrivedAtTarget = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeTowerGateStateCompressionInfo, ref bufferReadValid);
				this.FallAngularSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.SiegeMachineComponentAngularSpeedCompressionInfo, ref bufferReadValid);
				this.TotalDistanceTraveled = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200063C RID: 1596
		public enum GateState
		{
			// Token: 0x04002160 RID: 8544
			Closed,
			// Token: 0x04002161 RID: 8545
			Open,
			// Token: 0x04002162 RID: 8546
			GateFalling,
			// Token: 0x04002163 RID: 8547
			GateFallingWallDestroyed,
			// Token: 0x04002164 RID: 8548
			NumberOfStates
		}
	}
}
