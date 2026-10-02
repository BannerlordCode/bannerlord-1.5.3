using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.MountAndBlade.Network;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C7 RID: 455
	public sealed class Mission : DotNetObject, IMission
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x00054403 File Offset: 0x00052603
		// (set) Token: 0x06001997 RID: 6551 RVA: 0x0005440B File Offset: 0x0005260B
		internal UIntPtr Pointer { get; private set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001998 RID: 6552 RVA: 0x00054414 File Offset: 0x00052614
		public bool IsFinalized
		{
			get
			{
				return this.Pointer == UIntPtr.Zero;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x00054426 File Offset: 0x00052626
		// (set) Token: 0x0600199A RID: 6554 RVA: 0x00054433 File Offset: 0x00052633
		public static Mission Current
		{
			get
			{
				Mission current = Mission._current;
				return Mission._current;
			}
			private set
			{
				if (value == null)
				{
					Mission current = Mission._current;
				}
				Mission._current = value;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600199B RID: 6555 RVA: 0x00054444 File Offset: 0x00052644
		// (set) Token: 0x0600199C RID: 6556 RVA: 0x0005444C File Offset: 0x0005264C
		private MissionInitializerRecord InitializerRecord { get; set; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x00054455 File Offset: 0x00052655
		public string SceneName
		{
			get
			{
				return this.InitializerRecord.SceneName;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x00054462 File Offset: 0x00052662
		public string SceneLevels
		{
			get
			{
				return this.InitializerRecord.SceneLevels;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x0005446F File Offset: 0x0005266F
		public float DamageToPlayerMultiplier
		{
			get
			{
				return BannerlordConfig.GetDamageToPlayerMultiplier();
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060019A0 RID: 6560 RVA: 0x00054476 File Offset: 0x00052676
		public float DamageToFriendsMultiplier
		{
			get
			{
				return this.InitializerRecord.DamageToFriendsMultiplier;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060019A1 RID: 6561 RVA: 0x00054483 File Offset: 0x00052683
		public float DamageFromPlayerToFriendsMultiplier
		{
			get
			{
				return this.InitializerRecord.DamageFromPlayerToFriendsMultiplier;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00054490 File Offset: 0x00052690
		public bool HasValidTerrainType
		{
			get
			{
				return this.InitializerRecord.TerrainType >= 0;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060019A3 RID: 6563 RVA: 0x000544A3 File Offset: 0x000526A3
		public TerrainType TerrainType
		{
			get
			{
				if (!this.HasValidTerrainType)
				{
					return TerrainType.Water;
				}
				return (TerrainType)this.InitializerRecord.TerrainType;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x000544BB File Offset: 0x000526BB
		// (set) Token: 0x060019A5 RID: 6565 RVA: 0x000544C3 File Offset: 0x000526C3
		public Scene Scene { get; private set; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060019A6 RID: 6566 RVA: 0x000544CC File Offset: 0x000526CC
		// (set) Token: 0x060019A7 RID: 6567 RVA: 0x000544D4 File Offset: 0x000526D4
		public Vec3 CustomCameraTargetLocalOffset { get; private set; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x000544DD File Offset: 0x000526DD
		// (set) Token: 0x060019A9 RID: 6569 RVA: 0x000544E5 File Offset: 0x000526E5
		public Vec3 CustomCameraLocalOffset { get; private set; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x000544EE File Offset: 0x000526EE
		// (set) Token: 0x060019AB RID: 6571 RVA: 0x000544F6 File Offset: 0x000526F6
		public Vec3 CustomCameraLocalOffset2 { get; private set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x000544FF File Offset: 0x000526FF
		// (set) Token: 0x060019AD RID: 6573 RVA: 0x00054507 File Offset: 0x00052707
		public Vec3 CustomCameraGlobalOffset { get; private set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x00054510 File Offset: 0x00052710
		// (set) Token: 0x060019AF RID: 6575 RVA: 0x00054518 File Offset: 0x00052718
		public Vec3 CustomCameraLocalRotationalOffset { get; private set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x00054521 File Offset: 0x00052721
		// (set) Token: 0x060019B1 RID: 6577 RVA: 0x00054529 File Offset: 0x00052729
		public bool CustomCameraIgnoreCollision { get; private set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x00054532 File Offset: 0x00052732
		// (set) Token: 0x060019B3 RID: 6579 RVA: 0x0005453A File Offset: 0x0005273A
		public float CustomCameraFovMultiplier { get; private set; } = 1f;

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x00054543 File Offset: 0x00052743
		// (set) Token: 0x060019B5 RID: 6581 RVA: 0x0005454B File Offset: 0x0005274B
		public float CustomCameraFixedDistance { get; private set; } = float.MinValue;

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x00054554 File Offset: 0x00052754
		// (set) Token: 0x060019B7 RID: 6583 RVA: 0x0005455C File Offset: 0x0005275C
		public float ListenerAndAttenuationPosBlendFactor { get; private set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x00054565 File Offset: 0x00052765
		// (set) Token: 0x060019B9 RID: 6585 RVA: 0x0005456D File Offset: 0x0005276D
		public GameEntity IgnoredEntityForCamera { get; private set; }

		// Token: 0x060019BA RID: 6586 RVA: 0x00054578 File Offset: 0x00052778
		public IEnumerable<WeakGameEntity> GetActiveEntitiesWithScriptComponentOfType<T>()
		{
			return from amo in this._activeMissionObjects
				where amo is T
				select amo.GameEntity;
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000545D3 File Offset: 0x000527D3
		public void AddActiveMissionObject(MissionObject missionObject)
		{
			this._missionObjects.Add(missionObject);
			this._activeMissionObjects.Add(missionObject);
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x000545ED File Offset: 0x000527ED
		public void ActivateMissionObject(MissionObject missionObject)
		{
			this._activeMissionObjects.Add(missionObject);
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000545FB File Offset: 0x000527FB
		public void DeactivateMissionObject(MissionObject missionObject)
		{
			this._activeMissionObjects.Remove(missionObject);
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x0005460A File Offset: 0x0005280A
		public MBReadOnlyList<MissionObject> ActiveMissionObjects
		{
			get
			{
				return this._activeMissionObjects;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x00054612 File Offset: 0x00052812
		public MBReadOnlyList<MissionObject> MissionObjects
		{
			get
			{
				return this._missionObjects;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x0005461A File Offset: 0x0005281A
		public MBReadOnlyList<Mission.DynamicallyCreatedEntity> AddedEntitiesInfo
		{
			get
			{
				return this._addedEntitiesInfo;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x00054622 File Offset: 0x00052822
		// (set) Token: 0x060019C2 RID: 6594 RVA: 0x0005462A File Offset: 0x0005282A
		public Mission.MBBoundaryCollection Boundaries { get; private set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x00054634 File Offset: 0x00052834
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x00054690 File Offset: 0x00052890
		public bool IsMainAgentObjectInteractionEnabled
		{
			get
			{
				switch (this._missionMode)
				{
				case MissionMode.Conversation:
				case MissionMode.Barter:
				case MissionMode.Deployment:
				case MissionMode.Replay:
				case MissionMode.CutScene:
					return false;
				}
				return (this.IsNavalBattle || !this.MissionEnded) && this._isMainAgentObjectInteractionEnabled;
			}
			set
			{
				this._isMainAgentObjectInteractionEnabled = value;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0005469C File Offset: 0x0005289C
		// (set) Token: 0x060019C6 RID: 6598 RVA: 0x000546E6 File Offset: 0x000528E6
		public bool IsMainAgentItemInteractionEnabled
		{
			get
			{
				switch (this._missionMode)
				{
				case MissionMode.Conversation:
				case MissionMode.Barter:
				case MissionMode.Deployment:
				case MissionMode.Replay:
				case MissionMode.CutScene:
					return false;
				}
				return this._isMainAgentItemInteractionEnabled;
			}
			set
			{
				this._isMainAgentItemInteractionEnabled = value;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x000546EF File Offset: 0x000528EF
		// (set) Token: 0x060019C8 RID: 6600 RVA: 0x000546F7 File Offset: 0x000528F7
		public bool IsTeleportingAgents { get; set; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x00054700 File Offset: 0x00052900
		// (set) Token: 0x060019CA RID: 6602 RVA: 0x00054708 File Offset: 0x00052908
		public bool ForceTickOccasionally { get; set; }

		// Token: 0x060019CB RID: 6603 RVA: 0x00054711 File Offset: 0x00052911
		private void FinalizeMission()
		{
			TeamAISiegeComponent.OnMissionFinalize();
			MBAPI.IMBMission.FinalizeMission(this.Pointer);
			this.Pointer = UIntPtr.Zero;
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x00054733 File Offset: 0x00052933
		// (set) Token: 0x060019CD RID: 6605 RVA: 0x00054745 File Offset: 0x00052945
		public Mission.MissionCombatType CombatType
		{
			get
			{
				return (Mission.MissionCombatType)MBAPI.IMBMission.GetCombatType(this.Pointer);
			}
			set
			{
				MBAPI.IMBMission.SetCombatType(this.Pointer, (int)value);
			}
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00054758 File Offset: 0x00052958
		public void SetMissionCombatType(Mission.MissionCombatType missionCombatType)
		{
			MBAPI.IMBMission.SetCombatType(this.Pointer, (int)missionCombatType);
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0005476B File Offset: 0x0005296B
		public MissionMode Mode
		{
			get
			{
				return this._missionMode;
			}
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00054774 File Offset: 0x00052974
		public void ConversationCharacterChanged()
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnConversationCharacterChanged();
			}
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x000547C4 File Offset: 0x000529C4
		public void SetMissionMode(MissionMode newMode, bool atStart)
		{
			if (this._missionMode != newMode)
			{
				MissionMode missionMode = this._missionMode;
				this._missionMode = newMode;
				if (this.CurrentState != Mission.State.Over)
				{
					for (int i = 0; i < this.MissionBehaviors.Count; i++)
					{
						this.MissionBehaviors[i].OnMissionModeChange(missionMode, atStart);
					}
					foreach (IMissionListener missionListener in this._listeners)
					{
						missionListener.OnMissionModeChange(missionMode, atStart);
					}
				}
			}
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00054860 File Offset: 0x00052A60
		private Mission.AgentCreationResult CreateAgentInternal(AgentFlag agentFlags, int forcedAgentIndex, bool isFemale, ref AgentSpawnData spawnData, ref AgentCapsuleData capsuleData, ref AnimationSystemData animationSystemData, int instanceNo)
		{
			return MBAPI.IMBMission.CreateAgent(this.Pointer, (ulong)agentFlags, forcedAgentIndex, isFemale, ref spawnData, ref capsuleData.BodyCap, ref capsuleData.CrouchedBodyCap, ref animationSystemData, instanceNo);
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x00054895 File Offset: 0x00052A95
		public float CurrentTime
		{
			get
			{
				return this._cachedMissionTime;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x0005489D File Offset: 0x00052A9D
		// (set) Token: 0x060019D5 RID: 6613 RVA: 0x000548AF File Offset: 0x00052AAF
		public bool PauseAITick
		{
			get
			{
				return MBAPI.IMBMission.GetPauseAITick(this.Pointer);
			}
			set
			{
				MBAPI.IMBMission.SetPauseAITick(this.Pointer, value);
			}
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x000548C2 File Offset: 0x00052AC2
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void UpdateMissionTimeCache(float curTime)
		{
			this._cachedMissionTime = curTime;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x000548CB File Offset: 0x00052ACB
		public float GetAverageFps()
		{
			return MBAPI.IMBMission.GetAverageFps(this.Pointer);
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x000548DD File Offset: 0x00052ADD
		public bool GetFallAvoidSystemActive()
		{
			return MBAPI.IMBMission.GetFallAvoidSystemActive(this.Pointer);
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x000548EF File Offset: 0x00052AEF
		public void SetFallAvoidSystemActive(bool fallAvoidActive)
		{
			MBAPI.IMBMission.SetFallAvoidSystemActive(this.Pointer, fallAvoidActive);
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00054902 File Offset: 0x00052B02
		public bool IsPositionInsideBoundaries(Vec2 position)
		{
			return MBAPI.IMBMission.IsPositionInsideBoundaries(this.Pointer, position);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00054915 File Offset: 0x00052B15
		public bool IsPositionInsideHardBoundaries(Vec2 position)
		{
			return MBAPI.IMBMission.IsPositionInsideHardBoundaries(this.Pointer, position);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00054928 File Offset: 0x00052B28
		public bool IsPositionInsideAnyBlockerNavMeshFace2D(Vec2 position)
		{
			return MBAPI.IMBMission.IsPositionInsideAnyBlockerNavMeshFace2D(this.Pointer, position);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0005493B File Offset: 0x00052B3B
		public bool IsPositionOnAnyBlockerNavMeshFace(Vec3 position)
		{
			return MBAPI.IMBMission.IsPositionOnAnyBlockerNavMeshFace(this.Pointer, position);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00054950 File Offset: 0x00052B50
		private bool IsFormationUnitPositionAvailableAuxMT(ref WorldPosition formationPosition, ref WorldPosition unitPosition, ref WorldPosition nearestAvailableUnitPosition, float manhattanDistance)
		{
			bool flag;
			using (new TWSharedMutexReadLock(Scene.PhysicsAndRayCastLock))
			{
				flag = MBAPI.IMBMission.IsFormationUnitPositionAvailable(this.Pointer, ref formationPosition, ref unitPosition, ref nearestAvailableUnitPosition, manhattanDistance);
			}
			return flag;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000549A0 File Offset: 0x00052BA0
		public Agent RayCastForClosestAgent(Vec3 sourcePoint, Vec3 targetPoint, int excludedAgentIndex, float rayThickness, out float collisionDistance)
		{
			return MBAPI.IMBMission.RayCastForClosestAgent(this.Pointer, sourcePoint, targetPoint, excludedAgentIndex, rayThickness, out collisionDistance);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000549B9 File Offset: 0x00052BB9
		public Agent RayCastForClosestAgentsLimbs(Vec3 sourcePoint, Vec3 targetPoint, int excludedAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex)
		{
			return MBAPI.IMBMission.RayCastForClosestAgentsLimbs(this.Pointer, sourcePoint, targetPoint, excludedAgentIndex, rayThickness, out collisionDistance, out boneIndex);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000549D4 File Offset: 0x00052BD4
		public bool RayCastForGivenAgentsLimbs(Vec3 sourcePoint, Vec3 rayFinishPoint, int givenAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex)
		{
			return MBAPI.IMBMission.RayCastForGivenAgentsLimbs(this.Pointer, sourcePoint, rayFinishPoint, givenAgentIndex, rayThickness, out collisionDistance, out boneIndex);
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000549EF File Offset: 0x00052BEF
		internal AgentProximityMap.ProximityMapSearchStructInternal ProximityMapBeginSearch(Vec2 searchPos, float searchRadius)
		{
			return MBAPI.IMBMission.ProximityMapBeginSearch(this.Pointer, searchPos, searchRadius);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00054A03 File Offset: 0x00052C03
		internal float ProximityMapMaxSearchRadius()
		{
			return MBAPI.IMBMission.ProximityMapMaxSearchRadius(this.Pointer);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00054A15 File Offset: 0x00052C15
		public float GetBiggestAgentCollisionPadding()
		{
			return MBAPI.IMBMission.GetBiggestAgentCollisionPadding(this.Pointer);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00054A27 File Offset: 0x00052C27
		public void SetMissionCorpseFadeOutTimeInSeconds(float corpseFadeOutTimeInSeconds)
		{
			MBAPI.IMBMission.SetMissionCorpseFadeOutTimeInSeconds(this.Pointer, corpseFadeOutTimeInSeconds);
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00054A3A File Offset: 0x00052C3A
		public void SetOverrideCorpseCount(int overrideCorpseCount)
		{
			MBAPI.IMBMission.SetOverrideCorpseCount(this.Pointer, overrideCorpseCount);
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00054A4D File Offset: 0x00052C4D
		public void SetReportStuckAgentsMode(bool value)
		{
			MBAPI.IMBMission.SetReportStuckAgentsMode(this.Pointer, value);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00054A60 File Offset: 0x00052C60
		internal void BatchFormationUnitPositions(MBArrayList<Vec2i> orderedPositionIndices, MBArrayList<Vec2> orderedLocalPositions, MBList2D<int> availabilityTable, MBList2D<WorldPosition> globalPositionTable, WorldPosition orderPosition, Vec2 direction, int fileCount, int rankCount, bool fastCheckWithSameFaceGroupIdDigit)
		{
			MBAPI.IMBMission.BatchFormationUnitPositions(this.Pointer, orderedPositionIndices.RawArray, orderedLocalPositions.RawArray, availabilityTable.RawArray, globalPositionTable.RawArray, orderPosition, direction, fileCount, rankCount, fastCheckWithSameFaceGroupIdDigit);
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00054AA0 File Offset: 0x00052CA0
		internal void ProximityMapFindNext(ref AgentProximityMap.ProximityMapSearchStructInternal searchStruct)
		{
			MBAPI.IMBMission.ProximityMapFindNext(this.Pointer, ref searchStruct);
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00054AB4 File Offset: 0x00052CB4
		[UsedImplicitly]
		[MBCallback(null, false)]
		public void ResetMission()
		{
			IMissionListener[] array = this._listeners.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnResetMission();
			}
			foreach (Agent agent in this._activeAgents)
			{
				agent.OnRemove();
			}
			foreach (Agent agent2 in this._allAgents)
			{
				agent2.OnDelete();
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnClearScene();
			}
			this.NumOfFormationsSpawnedTeamOne = 0;
			this.NumOfFormationsSpawnedTeamTwo = 0;
			foreach (Team team in this.Teams)
			{
				team.Reset();
			}
			MBAPI.IMBMission.ClearScene(this.Pointer);
			this._activeAgents.Clear();
			this._allAgents.Clear();
			this._mountsWithoutRiders.Clear();
			this.MainAgent = null;
			this.ClearMissiles();
			this._missilesList.Clear();
			this._missilesDictionary.Clear();
			this._agentCount = 0;
			for (int j = 0; j < 2; j++)
			{
				this._initialAgentCountPerSide[j] = 0;
				this._removedAgentCountPerSide[j] = 0;
			}
			this.ResetMissionObjects();
			this.RemoveSpawnedMissionObjects();
			this._activeMissionObjects.Clear();
			this._activeMissionObjects.AddRange(this.MissionObjects);
			this._tickActions.Clear();
			this.Scene.ClearRuntimeDecals();
			PropertyChangedEventHandler onMissionReset = this.OnMissionReset;
			if (onMissionReset == null)
			{
				return;
			}
			onMissionReset(this, null);
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060019EB RID: 6635 RVA: 0x00054CC4 File Offset: 0x00052EC4
		// (remove) Token: 0x060019EC RID: 6636 RVA: 0x00054CFC File Offset: 0x00052EFC
		public event PropertyChangedEventHandler OnMissionReset;

		// Token: 0x060019ED RID: 6637 RVA: 0x00054D34 File Offset: 0x00052F34
		public void Initialize()
		{
			Mission.Current = this;
			this.CurrentState = Mission.State.Initializing;
			this._deploymentPlan = this.GetMissionBehavior<MissionDeploymentPlanningLogic>();
			if (this._deploymentPlan == null)
			{
				this._deploymentPlan = new DefaultMissionDeploymentPlan(this);
			}
			MissionInitializerRecord initializerRecord = this.InitializerRecord;
			MBAPI.IMBMission.InitializeMission(this.Pointer, ref initializerRecord);
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00054D87 File Offset: 0x00052F87
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnSceneCreated(Scene scene)
		{
			this.Scene = scene;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00054D90 File Offset: 0x00052F90
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void TickAgentsAndTeams(float dt, bool tickPaused)
		{
			this.TickAgentsAndTeamsImp(dt, tickPaused);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00054D9A File Offset: 0x00052F9A
		public void TickAgentsAndTeamsAsync(float dt)
		{
			MBAPI.IMBMission.TickAgentsAndTeamsAsync(this.Pointer, dt);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00054DAD File Offset: 0x00052FAD
		internal void Tick(float dt)
		{
			MBAPI.IMBMission.Tick(this.Pointer, dt);
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00054DC0 File Offset: 0x00052FC0
		internal void IdleTick(float dt)
		{
			MBAPI.IMBMission.IdleTick(this.Pointer, dt);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00054DD3 File Offset: 0x00052FD3
		public void MakeSound(int soundIndex, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2)
		{
			MBAPI.IMBMission.MakeSound(this.Pointer, soundIndex, position, soundCanBePredicted, isReliable, relatedAgent1, relatedAgent2);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00054DF0 File Offset: 0x00052FF0
		public void MakeSound(int soundIndex, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2, ref SoundEventParameter parameter)
		{
			MBAPI.IMBMission.MakeSoundWithParameter(this.Pointer, soundIndex, position, soundCanBePredicted, isReliable, relatedAgent1, relatedAgent2, parameter);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00054E1D File Offset: 0x0005301D
		public void MakeSoundOnlyOnRelatedPeer(int soundIndex, Vec3 position, int relatedAgent)
		{
			MBAPI.IMBMission.MakeSoundOnlyOnRelatedPeer(this.Pointer, soundIndex, position, relatedAgent);
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x00054E32 File Offset: 0x00053032
		public void AddDynamicallySpawnedMissionObjectInfo(Mission.DynamicallyCreatedEntity entityInfo)
		{
			this._addedEntitiesInfo.Add(entityInfo);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x00054E40 File Offset: 0x00053040
		private void RemoveDynamicallySpawnedMissionObjectInfo(MissionObjectId id)
		{
			Mission.DynamicallyCreatedEntity dynamicallyCreatedEntity = this._addedEntitiesInfo.FirstOrDefault<Mission.DynamicallyCreatedEntity>((Mission.DynamicallyCreatedEntity x) => x.ObjectId == id);
			if (dynamicallyCreatedEntity != null)
			{
				this._addedEntitiesInfo.Remove(dynamicallyCreatedEntity);
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00054E84 File Offset: 0x00053084
		private int AddMissileAux(int forcedMissileIndex, bool isPrediction, Agent shooterAgent, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, WeakGameEntity gameEntityToIgnore, bool isPrimaryWeaponShot, out GameEntity missileEntity)
		{
			UIntPtr uintPtr;
			int num = MBAPI.IMBMission.AddMissile(this.Pointer, isPrediction, shooterAgent.Index, in weaponData, weaponStatsData, weaponStatsData.Length, damageBonus, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, gameEntityToIgnore.Pointer, forcedMissileIndex, isPrimaryWeaponShot, out uintPtr);
			missileEntity = (isPrediction ? null : new GameEntity(uintPtr));
			return num;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00054EDC File Offset: 0x000530DC
		private int AddMissileSingleUsageAux(int forcedMissileIndex, bool isPrediction, Agent shooterAgent, in WeaponData weaponData, in WeaponStatsData weaponStatsData, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, WeakGameEntity gameEntityToIgnore, bool isPrimaryWeaponShot, out GameEntity missileEntity)
		{
			UIntPtr uintPtr;
			int num = MBAPI.IMBMission.AddMissileSingleUsage(this.Pointer, isPrediction, shooterAgent.Index, in weaponData, in weaponStatsData, damageBonus, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, gameEntityToIgnore.Pointer, forcedMissileIndex, isPrimaryWeaponShot, out uintPtr);
			missileEntity = (isPrediction ? null : new GameEntity(uintPtr));
			return num;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00054F2D File Offset: 0x0005312D
		public Vec3 GetMissileCollisionPoint(Vec3 missileStartingPosition, Vec3 missileDirection, float missileSpeed, in WeaponData weaponData)
		{
			return MBAPI.IMBMission.GetMissileCollisionPoint(this.Pointer, missileStartingPosition, missileDirection, missileSpeed, in weaponData);
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x00054F44 File Offset: 0x00053144
		public void RemoveMissileAsClient(int missileIndex)
		{
			MBAPI.IMBMission.RemoveMissile(this.Pointer, missileIndex);
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00054F57 File Offset: 0x00053157
		public static float GetMissileVerticalAimCorrection(Vec3 vecToTarget, float missileStartingSpeed, ref WeaponStatsData weaponStatsData, float airFrictionConstant)
		{
			return MBAPI.IMBMission.GetMissileVerticalAimCorrection(vecToTarget, missileStartingSpeed, ref weaponStatsData, airFrictionConstant);
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00054F67 File Offset: 0x00053167
		public static float GetMissileRange(float missileStartingSpeed, float heightDifference)
		{
			return MBAPI.IMBMission.GetMissileRange(missileStartingSpeed, heightDifference);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00054F75 File Offset: 0x00053175
		public void PrepareMissileWeaponForDrop(int missileIndex)
		{
			MBAPI.IMBMission.PrepareMissileWeaponForDrop(this.Pointer, missileIndex);
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00054F88 File Offset: 0x00053188
		public void AddParticleSystemBurstByName(string particleSystem, MatrixFrame frame, bool synchThroughNetwork)
		{
			MBAPI.IMBMission.AddParticleSystemBurstByName(this.Pointer, particleSystem, ref frame, synchThroughNetwork);
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x00054F9E File Offset: 0x0005319E
		public bool IsLoadingFinished
		{
			get
			{
				return MBAPI.IMBMission.GetIsLoadingFinished(this.Pointer);
			}
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00054FB0 File Offset: 0x000531B0
		public Vec2 GetClosestBoundaryPosition(Vec2 position)
		{
			return MBAPI.IMBMission.GetClosestBoundaryPosition(this.Pointer, position);
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00054FC4 File Offset: 0x000531C4
		private void ResetMissionObjects()
		{
			for (int i = this._dynamicEntities.Count - 1; i >= 0; i--)
			{
				Mission.DynamicEntityInfo dynamicEntityInfo = this._dynamicEntities[i];
				dynamicEntityInfo.Entity.RemoveEnginePhysics();
				dynamicEntityInfo.Entity.Remove(74);
				this._dynamicEntities.RemoveAt(i);
			}
			foreach (MissionObject missionObject in this.MissionObjects)
			{
				if (missionObject.CreatedAtRuntime)
				{
					break;
				}
				missionObject.OnMissionReset();
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00055068 File Offset: 0x00053268
		private void RemoveSpawnedMissionObjects()
		{
			MissionObject[] array = this._missionObjects.ToArray();
			for (int i = array.Length - 1; i >= 0; i--)
			{
				MissionObject missionObject = array[i];
				if (!missionObject.CreatedAtRuntime)
				{
					break;
				}
				if (missionObject.GameEntity.IsValid)
				{
					missionObject.GameEntity.RemoveAllChildren();
					missionObject.GameEntity.Remove(75);
				}
			}
			this._spawnedItemEntitiesCreatedAtRuntime.Clear();
			this._lastRuntimeMissionObjectIdCount = 0;
			this._emptyRuntimeMissionObjectIds.Clear();
			this._addedEntitiesInfo.Clear();
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000550F4 File Offset: 0x000532F4
		public int GetFreeRuntimeMissionObjectId()
		{
			float currentTime = Mission.Current.CurrentTime;
			int num = -1;
			if (this._emptyRuntimeMissionObjectIds.Count > 0)
			{
				if (currentTime - this._emptyRuntimeMissionObjectIds.Peek().Item2 > 30f || this._lastRuntimeMissionObjectIdCount >= 8191)
				{
					num = this._emptyRuntimeMissionObjectIds.Pop().Item1;
				}
				else
				{
					num = this._lastRuntimeMissionObjectIdCount;
					this._lastRuntimeMissionObjectIdCount++;
				}
			}
			else if (this._lastRuntimeMissionObjectIdCount < 8191)
			{
				num = this._lastRuntimeMissionObjectIdCount;
				this._lastRuntimeMissionObjectIdCount++;
			}
			return num;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0005518F File Offset: 0x0005338F
		private void ReturnRuntimeMissionObjectId(int id)
		{
			this._emptyRuntimeMissionObjectIds.Push(new ValueTuple<int, float>(id, Mission.Current.CurrentTime));
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000551AC File Offset: 0x000533AC
		public int GetFreeSceneMissionObjectId()
		{
			int lastSceneMissionObjectIdCount = this._lastSceneMissionObjectIdCount;
			this._lastSceneMissionObjectIdCount++;
			return lastSceneMissionObjectIdCount;
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x000551C2 File Offset: 0x000533C2
		public void SetCameraFrame(ref MatrixFrame cameraFrame, float zoomFactor)
		{
			this.SetCameraFrame(ref cameraFrame, zoomFactor, ref cameraFrame.origin);
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000551D2 File Offset: 0x000533D2
		public void SetCameraFrame(ref MatrixFrame cameraFrame, float zoomFactor, ref Vec3 attenuationPosition)
		{
			cameraFrame.Fill();
			MBAPI.IMBMission.SetCameraFrame(this.Pointer, ref cameraFrame, zoomFactor, ref attenuationPosition);
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000551ED File Offset: 0x000533ED
		public MatrixFrame GetCameraFrame()
		{
			return MBAPI.IMBMission.GetCameraFrame(this.Pointer);
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x000551FF File Offset: 0x000533FF
		// (set) Token: 0x06001A0B RID: 6667 RVA: 0x00055206 File Offset: 0x00053406
		public bool CameraIsFirstPerson
		{
			get
			{
				return Mission._isCameraFirstPerson;
			}
			set
			{
				if (Mission._isCameraFirstPerson != value)
				{
					Mission._isCameraFirstPerson = value;
					MBAPI.IMBMission.SetCameraIsFirstPerson(value);
					this.ResetFirstThirdPersonView();
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x00055227 File Offset: 0x00053427
		// (set) Token: 0x06001A0D RID: 6669 RVA: 0x0005522E File Offset: 0x0005342E
		public static float CameraAddedDistance
		{
			get
			{
				return BannerlordConfig.CombatCameraDistance;
			}
			set
			{
				if (value != BannerlordConfig.CombatCameraDistance)
				{
					BannerlordConfig.CombatCameraDistance = value;
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0005523E File Offset: 0x0005343E
		public float ClearSceneTimerElapsedTime
		{
			get
			{
				return MBAPI.IMBMission.GetClearSceneTimerElapsedTime(this.Pointer);
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00055250 File Offset: 0x00053450
		public void ResetFirstThirdPersonView()
		{
			MBAPI.IMBMission.ResetFirstThirdPersonView(this.Pointer);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00055262 File Offset: 0x00053462
		public void SetCustomCameraLocalOffset(Vec3 newCameraOffset)
		{
			this.CustomCameraLocalOffset = newCameraOffset;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005526B File Offset: 0x0005346B
		public void SetCustomCameraTargetLocalOffset(Vec3 newTargetLocalOffset)
		{
			this.CustomCameraTargetLocalOffset = newTargetLocalOffset;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00055274 File Offset: 0x00053474
		public void SetCustomCameraLocalOffset2(Vec3 newCameraOffset)
		{
			this.CustomCameraLocalOffset2 = newCameraOffset;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0005527D File Offset: 0x0005347D
		public void SetCustomCameraLocalRotationalOffset(Vec3 newCameraRotationalOffset)
		{
			this.CustomCameraLocalRotationalOffset = newCameraRotationalOffset;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00055286 File Offset: 0x00053486
		public void SetCustomCameraGlobalOffset(Vec3 newCameraOffset)
		{
			this.CustomCameraGlobalOffset = newCameraOffset;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005528F File Offset: 0x0005348F
		public void SetCustomCameraFovMultiplier(float newFovMultiplier)
		{
			this.CustomCameraFovMultiplier = newFovMultiplier;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00055298 File Offset: 0x00053498
		public void SetCustomCameraFixedDistance(float distance)
		{
			this.CustomCameraFixedDistance = distance;
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000552A1 File Offset: 0x000534A1
		public void SetIgnoredEntityForCamera(GameEntity ignoredEntity)
		{
			this.IgnoredEntityForCamera = ignoredEntity;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x000552AA File Offset: 0x000534AA
		public void SetCustomCameraIgnoreCollision(bool ignoreCollision)
		{
			this.CustomCameraIgnoreCollision = ignoreCollision;
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000552B3 File Offset: 0x000534B3
		public void SetListenerAndAttenuationPosBlendFactor(float factor)
		{
			this.ListenerAndAttenuationPosBlendFactor = factor;
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000552BC File Offset: 0x000534BC
		internal void UpdateSceneTimeSpeed()
		{
			if (this.Scene != null)
			{
				float num = 1f;
				int num2 = -1;
				for (int i = 0; i < this._timeSpeedRequests.Count; i++)
				{
					if (this._timeSpeedRequests[i].RequestedTimeSpeed < num)
					{
						num = this._timeSpeedRequests[i].RequestedTimeSpeed;
						num2 = this._timeSpeedRequests[i].RequestID;
					}
				}
				if (!this.Scene.TimeSpeed.ApproximatelyEqualsTo(num, 1E-05f))
				{
					if (num2 != -1)
					{
						Debug.Print(string.Format("Updated mission time speed with request ID:{0}, time speed{1}", num2, num), 0, Debug.DebugColor.White, 17592186044416UL);
					}
					else
					{
						Debug.Print(string.Format("Reverted time speed back to default({0})", num), 0, Debug.DebugColor.White, 17592186044416UL);
					}
					this.Scene.TimeSpeed = num;
				}
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000553AB File Offset: 0x000535AB
		public void AddTimeSpeedRequest(Mission.TimeSpeedRequest request)
		{
			this._timeSpeedRequests.Add(request);
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x000553BC File Offset: 0x000535BC
		[Conditional("_RGL_KEEP_ASSERTS")]
		private void AssertTimeSpeedRequestDoesNotExist(Mission.TimeSpeedRequest request)
		{
			for (int i = 0; i < this._timeSpeedRequests.Count; i++)
			{
				int requestID = this._timeSpeedRequests[i].RequestID;
				int requestID2 = request.RequestID;
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000553FC File Offset: 0x000535FC
		public void RemoveTimeSpeedRequest(int timeSpeedRequestID)
		{
			int num = -1;
			for (int i = 0; i < this._timeSpeedRequests.Count; i++)
			{
				if (this._timeSpeedRequests[i].RequestID == timeSpeedRequestID)
				{
					num = i;
				}
			}
			this._timeSpeedRequests.RemoveAt(num);
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00055448 File Offset: 0x00053648
		public bool GetRequestedTimeSpeed(int timeSpeedRequestID, out float requestedTime)
		{
			foreach (Mission.TimeSpeedRequest timeSpeedRequest in this._timeSpeedRequests)
			{
				if (timeSpeedRequest.RequestID == timeSpeedRequestID)
				{
					requestedTime = timeSpeedRequest.RequestedTimeSpeed;
					return true;
				}
			}
			requestedTime = 0f;
			return false;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000554B8 File Offset: 0x000536B8
		public void ClearAgentActions()
		{
			MBAPI.IMBMission.ClearAgentActions(this.Pointer);
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x000554CA File Offset: 0x000536CA
		public void ClearMissiles()
		{
			MBAPI.IMBMission.ClearMissiles(this.Pointer);
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000554DC File Offset: 0x000536DC
		public void ClearCorpses(bool isMissionReset)
		{
			MBAPI.IMBMission.ClearCorpses(this.Pointer, isMissionReset);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000554EF File Offset: 0x000536EF
		private Agent FindAgentWithIndexAux(int index)
		{
			if (index >= 0)
			{
				return MBAPI.IMBMission.FindAgentWithIndex(this.Pointer, index);
			}
			return null;
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00055508 File Offset: 0x00053708
		private Agent GetClosestEnemyAgent(MBTeam team, Vec3 position, float radius)
		{
			return MBAPI.IMBMission.GetClosestEnemy(this.Pointer, team.Index, position, radius);
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00055522 File Offset: 0x00053722
		private Agent GetClosestAllyAgent(MBTeam team, Vec3 position, float radius)
		{
			return MBAPI.IMBMission.GetClosestAlly(this.Pointer, team.Index, position, radius);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0005553C File Offset: 0x0005373C
		private int GetNearbyEnemyAgentCount(MBTeam team, Vec2 position, float radius)
		{
			int num = 0;
			int num2 = 0;
			MBAPI.IMBMission.GetAgentCountAroundPosition(this.Pointer, team.Index, position, radius, ref num, ref num2);
			return num2;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0005556A File Offset: 0x0005376A
		public bool IsAgentInProximityMap(Agent agent)
		{
			return MBAPI.IMBMission.IsAgentInProximityMap(this.Pointer, agent.Index);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00055584 File Offset: 0x00053784
		public void OnMissionStateActivate()
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissionStateActivated();
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x000555D4 File Offset: 0x000537D4
		public void OnMissionStateDeactivate()
		{
			if (this.MissionBehaviors != null)
			{
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnMissionStateDeactivated();
				}
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0005562C File Offset: 0x0005382C
		public void OnMissionStateFinalize(bool forceClearGPUResources)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissionStateFinalized();
			}
			if (GameNetwork.IsSessionActive && this.GetMissionBehavior<MissionNetworkComponent>() != null)
			{
				this.RemoveMissionBehavior(this.GetMissionBehavior<MissionNetworkComponent>());
			}
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.RemoveMissionBehavior(this.MissionBehaviors[i]);
			}
			this._deploymentPlan = null;
			this.MissionLogics.Clear();
			this.Scene = null;
			Mission.Current = null;
			this.ClearUnreferencedResources(forceClearGPUResources);
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000556E8 File Offset: 0x000538E8
		public void ClearUnreferencedResources(bool forceClearGPUResources)
		{
			Common.MemoryCleanupGC(false);
			if (forceClearGPUResources)
			{
				MBAPI.IMBMission.ClearResources(this.Pointer);
				MBAPI.IMBMission.DefragRenderBuffers();
			}
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0005570D File Offset: 0x0005390D
		public static void DefragRenderBuffers()
		{
			Common.MemoryCleanupGC(false);
			MBAPI.IMBMission.DefragRenderBuffers();
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00055720 File Offset: 0x00053920
		internal void OnEntityHit(WeakGameEntity entity, Agent attackerAgent, AttackCollisionData collisionData, int inflictedDamage, DamageTypes damageType, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ref CombatLogData combatLog)
		{
			bool flag = false;
			float num = (float)inflictedDamage;
			float num2 = -1f;
			float num3 = -1f;
			MissionObject missionObject = null;
			while (entity.IsValid)
			{
				int scriptCount = entity.GetScriptCount();
				for (int i = 0; i < scriptCount; i++)
				{
					MissionObject missionObject2;
					if ((missionObject2 = entity.GetScriptAtIndex(i) as MissionObject) != null)
					{
						bool flag2;
						if (missionObject2.OnHit(attackerAgent, inflictedDamage, impactPosition, impactDirection, in weapon, affectorWeaponSlotOrMissileIndex, null, out flag2, out num, out num2, out num3))
						{
							missionObject = missionObject2;
						}
						flag = flag || flag2;
					}
				}
				if (missionObject != null)
				{
					break;
				}
				entity = entity.Parent;
			}
			combatLog.MissionObjectHit = missionObject;
			if (flag && attackerAgent != null && !attackerAgent.IsMount && !attackerAgent.IsAIControlled)
			{
				combatLog.DamageType = damageType;
				combatLog.InflictedDamage = inflictedDamage;
				combatLog.ModifiedDamage = MathF.Round(num - (float)inflictedDamage);
				if (num2 > 0f)
				{
					combatLog.InflictedFireDamage = (int)num2;
					combatLog.ModifiedFireDamage = (int)num3;
				}
				this.AddCombatLogSafe(attackerAgent, null, combatLog);
			}
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00055818 File Offset: 0x00053A18
		public float GetMainAgentMaxCameraZoom()
		{
			if (this.MainAgent != null)
			{
				return MissionGameModels.Current.AgentStatCalculateModel.GetMaxCameraZoom(this.MainAgent);
			}
			return 1f;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0005583D File Offset: 0x00053A3D
		public WorldPosition GetBestSlopeTowardsDirection(ref WorldPosition centerPosition, float halfSize, ref WorldPosition referencePosition)
		{
			return MBAPI.IMBMission.GetBestSlopeTowardsDirection(this.Pointer, ref centerPosition, halfSize, ref referencePosition);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00055854 File Offset: 0x00053A54
		public WorldPosition GetBestSlopeAngleHeightPosForDefending(WorldPosition enemyPosition, WorldPosition defendingPosition, int sampleSize, float distanceRatioAllowedFromDefendedPos, float distanceSqrdAllowedFromBoundary, float cosinusOfBestSlope, float cosinusOfMaxAcceptedSlope, float minSlopeScore, float maxSlopeScore, float excessiveSlopePenalty, float nearConeCenterRatio, float nearConeCenterBonus, float heightDifferenceCeiling, float maxDisplacementPenalty)
		{
			return MBAPI.IMBMission.GetBestSlopeAngleHeightPosForDefending(this.Pointer, enemyPosition, defendingPosition, sampleSize, distanceRatioAllowedFromDefendedPos, distanceSqrdAllowedFromBoundary, cosinusOfBestSlope, cosinusOfMaxAcceptedSlope, minSlopeScore, maxSlopeScore, excessiveSlopePenalty, nearConeCenterRatio, nearConeCenterBonus, heightDifferenceCeiling, maxDisplacementPenalty);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0005588C File Offset: 0x00053A8C
		public Vec2 GetAveragePositionOfAgents(List<Agent> agents)
		{
			int num = 0;
			Vec2 vec = Vec2.Zero;
			foreach (Agent agent in agents)
			{
				num++;
				vec += agent.Position.AsVec2;
			}
			if (num == 0)
			{
				return Vec2.Invalid;
			}
			return vec * (1f / (float)num);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0005590C File Offset: 0x00053B0C
		private void GetNearbyAgentsAux(Vec2 center, float radius, MBTeam team, Mission.GetNearbyAgentsAuxType type, MBList<Agent> resultList)
		{
			EngineStackArray.StackArray40Int stackArray40Int = default(EngineStackArray.StackArray40Int);
			object getNearbyAgentsAuxLock = Mission.GetNearbyAgentsAuxLock;
			lock (getNearbyAgentsAuxLock)
			{
				int num = 0;
				for (;;)
				{
					int num2 = -1;
					MBAPI.IMBMission.GetNearbyAgentsAux(this.Pointer, center, radius, team.Index, (int)type, num, ref stackArray40Int, ref num2);
					for (int i = 0; i < num2; i++)
					{
						Agent agent = DotNetObject.GetManagedObjectWithId(stackArray40Int[i]) as Agent;
						resultList.Add(agent);
					}
					if (num2 < 40)
					{
						break;
					}
					num += 40;
				}
			}
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x000559B0 File Offset: 0x00053BB0
		private int GetNearbyAgentsCountAux(Vec2 center, float radius, MBTeam team, Mission.GetNearbyAgentsAuxType type)
		{
			int num = 0;
			EngineStackArray.StackArray40Int stackArray40Int = default(EngineStackArray.StackArray40Int);
			object getNearbyAgentsAuxLock = Mission.GetNearbyAgentsAuxLock;
			lock (getNearbyAgentsAuxLock)
			{
				int num2 = 0;
				for (;;)
				{
					int num3 = -1;
					MBAPI.IMBMission.GetNearbyAgentsAux(this.Pointer, center, radius, team.Index, (int)type, num2, ref stackArray40Int, ref num3);
					num += num3;
					if (num3 < 40)
					{
						break;
					}
					num2 += 40;
				}
			}
			return num;
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00055A30 File Offset: 0x00053C30
		public void SetRandomDecideTimeOfAgentsWithIndices(int[] agentIndices, float? minAIReactionTime = null, float? maxAIReactionTime = null)
		{
			if (minAIReactionTime == null || maxAIReactionTime == null)
			{
				maxAIReactionTime = new float?((float)(-1));
				minAIReactionTime = maxAIReactionTime;
			}
			MBAPI.IMBMission.SetRandomDecideTimeOfAgents(this.Pointer, agentIndices.Length, agentIndices, minAIReactionTime.Value, maxAIReactionTime.Value);
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00055A7D File Offset: 0x00053C7D
		public void SetBowMissileSpeedModifier(float modifier)
		{
			MBAPI.IMBMission.SetBowMissileSpeedModifier(this.Pointer, modifier);
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x00055A90 File Offset: 0x00053C90
		public void SetCrossbowMissileSpeedModifier(float modifier)
		{
			MBAPI.IMBMission.SetCrossbowMissileSpeedModifier(this.Pointer, modifier);
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00055AA3 File Offset: 0x00053CA3
		public void SetThrowingMissileSpeedModifier(float modifier)
		{
			MBAPI.IMBMission.SetThrowingMissileSpeedModifier(this.Pointer, modifier);
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00055AB6 File Offset: 0x00053CB6
		public void SetMissileRangeModifier(float modifier)
		{
			MBAPI.IMBMission.SetMissileRangeModifier(this.Pointer, modifier);
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00055AC9 File Offset: 0x00053CC9
		public void SetLastMovementKeyPressed(Agent.MovementControlFlag lastMovementKeyPressed)
		{
			MBAPI.IMBMission.SetLastMovementKeyPressed(this.Pointer, lastMovementKeyPressed);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x00055ADC File Offset: 0x00053CDC
		public Vec2 GetWeightedPointOfEnemies(Agent agent, Vec2 basePoint)
		{
			return MBAPI.IMBMission.GetWeightedPointOfEnemies(this.Pointer, agent.Index, basePoint);
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x00055AF5 File Offset: 0x00053CF5
		public bool GetPathBetweenPositions(ref NavigationData navData)
		{
			return MBAPI.IMBMission.GetNavigationPoints(this.Pointer, ref navData);
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x00055B08 File Offset: 0x00053D08
		public void SetNavigationFaceCostWithIdAroundPosition(int navigationFaceId, Vec3 position, float cost)
		{
			MBAPI.IMBMission.SetNavigationFaceCostWithIdAroundPosition(this.Pointer, navigationFaceId, position, cost);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00055B1D File Offset: 0x00053D1D
		public WorldPosition GetStraightPathToTarget(Vec2 targetPosition, WorldPosition startingPosition, float samplingDistance = 1f, bool stopAtObstacle = true)
		{
			return MBAPI.IMBMission.GetStraightPathToTarget(this.Pointer, targetPosition, startingPosition, samplingDistance, stopAtObstacle);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00055B34 File Offset: 0x00053D34
		public void SkipForwardMissionReplay(float startTime, float endTime)
		{
			MBAPI.IMBMission.SkipForwardMissionReplay(this.Pointer, startTime, endTime);
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00055B48 File Offset: 0x00053D48
		public int GetDebugAgent()
		{
			return MBAPI.IMBMission.GetDebugAgent(this.Pointer);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00055B5A File Offset: 0x00053D5A
		public void AddAiDebugText(string str)
		{
			MBAPI.IMBMission.AddAiDebugText(this.Pointer, str);
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00055B6D File Offset: 0x00053D6D
		public void SetDebugAgent(int index)
		{
			MBAPI.IMBMission.SetDebugAgent(this.Pointer, index);
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00055B80 File Offset: 0x00053D80
		public static float GetFirstPersonFov()
		{
			return BannerlordConfig.FirstPersonFov;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00055B87 File Offset: 0x00053D87
		public float GetWaterLevelAtPosition(Vec2 position, bool useWaterRenderer)
		{
			return MBAPI.IMBMission.GetWaterLevelAtPosition(this.Pointer, position, useWaterRenderer);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00055B9B File Offset: 0x00053D9B
		public float GetWaterLevelAtPositionMT(Vec2 position, bool useWaterRenderer)
		{
			return MBAPI.IMBMission.GetWaterLevelAtPosition(this.Pointer, position, useWaterRenderer);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00055BB0 File Offset: 0x00053DB0
		[UsedImplicitly]
		[MBCallback(null, true)]
		public bool CanPhysicsCollideBetweenTwoEntities(UIntPtr entity0Ptr, UIntPtr entity1Ptr)
		{
			WeakGameEntity weakGameEntity = new WeakGameEntity(entity0Ptr);
			WeakGameEntity weakGameEntity2 = new WeakGameEntity(entity1Ptr);
			BodyFlags bodyFlags = (weakGameEntity.IsValid ? weakGameEntity.BodyFlag : BodyFlags.None);
			BodyFlags bodyFlags2 = (weakGameEntity2.IsValid ? weakGameEntity2.BodyFlag : BodyFlags.None);
			WeakGameEntity weakGameEntity3 = weakGameEntity;
			while (weakGameEntity3.IsValid)
			{
				int scriptCount = weakGameEntity3.GetScriptCount();
				for (int i = 0; i < scriptCount; i++)
				{
					ScriptComponentBehavior scriptAtIndex = weakGameEntity3.GetScriptAtIndex(i);
					if (scriptAtIndex != null && !(weakGameEntity == weakGameEntity2) && !scriptAtIndex.CanPhysicsCollideBetweenTwoEntities(weakGameEntity, bodyFlags, weakGameEntity2, bodyFlags2))
					{
						return false;
					}
				}
				weakGameEntity3 = weakGameEntity3.Parent;
			}
			weakGameEntity3 = weakGameEntity2;
			while (weakGameEntity3.IsValid)
			{
				int scriptCount2 = weakGameEntity3.GetScriptCount();
				for (int j = 0; j < scriptCount2; j++)
				{
					ScriptComponentBehavior scriptAtIndex2 = weakGameEntity3.GetScriptAtIndex(j);
					if (scriptAtIndex2 != null && !(weakGameEntity == weakGameEntity2) && !scriptAtIndex2.CanPhysicsCollideBetweenTwoEntities(weakGameEntity2, bodyFlags2, weakGameEntity, bodyFlags))
					{
						return false;
					}
				}
				weakGameEntity3 = weakGameEntity3.Parent;
			}
			return true;
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001A45 RID: 6725 RVA: 0x00055CA8 File Offset: 0x00053EA8
		// (remove) Token: 0x06001A46 RID: 6726 RVA: 0x00055CE0 File Offset: 0x00053EE0
		public event Mission.OnBeforeAgentRemovedDelegate OnBeforeAgentRemoved;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06001A47 RID: 6727 RVA: 0x00055D18 File Offset: 0x00053F18
		// (remove) Token: 0x06001A48 RID: 6728 RVA: 0x00055D50 File Offset: 0x00053F50
		public event Func<WorldPosition, Team, bool> IsFormationUnitPositionAvailable_AdditionalCondition;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06001A49 RID: 6729 RVA: 0x00055D88 File Offset: 0x00053F88
		// (remove) Token: 0x06001A4A RID: 6730 RVA: 0x00055DC0 File Offset: 0x00053FC0
		public event Func<Agent, bool> CanAgentRout_AdditionalCondition;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001A4B RID: 6731 RVA: 0x00055DF8 File Offset: 0x00053FF8
		// (remove) Token: 0x06001A4C RID: 6732 RVA: 0x00055E30 File Offset: 0x00054030
		public event Mission.OnAddSoundAlarmFactorToAgentsDelegate OnAddSoundAlarmFactorToAgents;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001A4D RID: 6733 RVA: 0x00055E68 File Offset: 0x00054068
		// (remove) Token: 0x06001A4E RID: 6734 RVA: 0x00055EA0 File Offset: 0x000540A0
		public event Func<bool> IsAgentInteractionAllowed_AdditionalCondition;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001A4F RID: 6735 RVA: 0x00055ED8 File Offset: 0x000540D8
		// (remove) Token: 0x06001A50 RID: 6736 RVA: 0x00055F10 File Offset: 0x00054110
		public event Mission.OnMainAgentChangedDelegate OnMainAgentChanged;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001A51 RID: 6737 RVA: 0x00055F48 File Offset: 0x00054148
		// (remove) Token: 0x06001A52 RID: 6738 RVA: 0x00055F80 File Offset: 0x00054180
		public event Mission.OnCameraShakeTriggeredDelegate OnCameraShakeTriggered;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06001A53 RID: 6739 RVA: 0x00055FB8 File Offset: 0x000541B8
		// (remove) Token: 0x06001A54 RID: 6740 RVA: 0x00055FF0 File Offset: 0x000541F0
		public event Mission.ComputeTroopBodyPropertiesDelegate OnComputeTroopBodyProperties;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001A55 RID: 6741 RVA: 0x00056028 File Offset: 0x00054228
		// (remove) Token: 0x06001A56 RID: 6742 RVA: 0x00056060 File Offset: 0x00054260
		public event Func<BattleSideEnum, BasicCharacterObject, FormationClass> GetAgentTroopClass_Override;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001A57 RID: 6743 RVA: 0x00056098 File Offset: 0x00054298
		// (remove) Token: 0x06001A58 RID: 6744 RVA: 0x000560D0 File Offset: 0x000542D0
		public event Action DeploymentFinishedEvent;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001A59 RID: 6745 RVA: 0x00056108 File Offset: 0x00054308
		// (remove) Token: 0x06001A5A RID: 6746 RVA: 0x00056140 File Offset: 0x00054340
		public event Action<Agent, SpawnedItemEntity> OnItemPickUp;

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x00056175 File Offset: 0x00054375
		public MBReadOnlyList<Mission.Missile> MissilesList
		{
			get
			{
				return this._missilesList;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001A5C RID: 6748 RVA: 0x00056180 File Offset: 0x00054380
		// (remove) Token: 0x06001A5D RID: 6749 RVA: 0x000561B8 File Offset: 0x000543B8
		public event Action<Agent, SpawnedItemEntity> OnItemDrop;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001A5E RID: 6750 RVA: 0x000561F0 File Offset: 0x000543F0
		// (remove) Token: 0x06001A5F RID: 6751 RVA: 0x00056228 File Offset: 0x00054428
		public event Action<Formation> FormationCaptainChanged;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001A60 RID: 6752 RVA: 0x00056260 File Offset: 0x00054460
		// (remove) Token: 0x06001A61 RID: 6753 RVA: 0x00056298 File Offset: 0x00054498
		public event Func<Agent, WorldPosition?> GetOverriddenFleePositionForAgent;

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x000562CD File Offset: 0x000544CD
		// (set) Token: 0x06001A63 RID: 6755 RVA: 0x000562D8 File Offset: 0x000544D8
		public bool MissionEnded
		{
			get
			{
				return this._missionEnded;
			}
			private set
			{
				if (!this._missionEnded && value)
				{
					this.MissionIsEnding = true;
					foreach (MissionObject missionObject in this.MissionObjects)
					{
						missionObject.OnMissionEnded();
					}
					this.MissionIsEnding = false;
				}
				this._missionEnded = value;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0005634C File Offset: 0x0005454C
		public MBReadOnlyList<KeyValuePair<Agent, MissionTime>> MountsWithoutRiders
		{
			get
			{
				return this._mountsWithoutRiders;
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001A65 RID: 6757 RVA: 0x00056354 File Offset: 0x00054554
		// (remove) Token: 0x06001A66 RID: 6758 RVA: 0x0005638C File Offset: 0x0005458C
		public event Func<bool> AreOrderGesturesEnabled_AdditionalCondition;

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x000563C1 File Offset: 0x000545C1
		// (set) Token: 0x06001A68 RID: 6760 RVA: 0x000563C9 File Offset: 0x000545C9
		public bool MissionIsEnding { get; private set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x000563D2 File Offset: 0x000545D2
		// (set) Token: 0x06001A6A RID: 6762 RVA: 0x000563DA File Offset: 0x000545DA
		public bool IsDeploymentFinished { get; private set; }

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001A6B RID: 6763 RVA: 0x000563E4 File Offset: 0x000545E4
		// (remove) Token: 0x06001A6C RID: 6764 RVA: 0x0005641C File Offset: 0x0005461C
		public event Func<bool> IsBattleInRetreatEvent;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001A6D RID: 6765 RVA: 0x00056454 File Offset: 0x00054654
		// (remove) Token: 0x06001A6E RID: 6766 RVA: 0x0005648C File Offset: 0x0005468C
		public event Action<int> OnMissileRemovedEvent;

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x000564C1 File Offset: 0x000546C1
		// (set) Token: 0x06001A70 RID: 6768 RVA: 0x000564C9 File Offset: 0x000546C9
		public BattleSideEnum RetreatSide { get; private set; } = BattleSideEnum.None;

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x000564D2 File Offset: 0x000546D2
		// (set) Token: 0x06001A72 RID: 6770 RVA: 0x000564DA File Offset: 0x000546DA
		public bool IsFastForward
		{
			get
			{
				return this._isFastForward;
			}
			private set
			{
				this._isFastForward = value;
				MBAPI.IMBMission.OnFastForwardStateChanged(this.Pointer, this._isFastForward);
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x000564F9 File Offset: 0x000546F9
		// (set) Token: 0x06001A74 RID: 6772 RVA: 0x00056501 File Offset: 0x00054701
		public bool FixedDeltaTimeMode { get; set; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x0005650A File Offset: 0x0005470A
		// (set) Token: 0x06001A76 RID: 6774 RVA: 0x00056512 File Offset: 0x00054712
		public float FixedDeltaTime { get; set; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x0005651B File Offset: 0x0005471B
		// (set) Token: 0x06001A78 RID: 6776 RVA: 0x00056523 File Offset: 0x00054723
		public Mission.State CurrentState { get; private set; }

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x0005652C File Offset: 0x0005472C
		// (set) Token: 0x06001A7A RID: 6778 RVA: 0x00056534 File Offset: 0x00054734
		public Mission.TeamCollection Teams { get; private set; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x0005653D File Offset: 0x0005473D
		public Team AttackerTeam
		{
			get
			{
				return this.Teams.Attacker;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0005654A File Offset: 0x0005474A
		public Team DefenderTeam
		{
			get
			{
				return this.Teams.Defender;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x00056557 File Offset: 0x00054757
		public Team AttackerAllyTeam
		{
			get
			{
				return this.Teams.AttackerAlly;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x00056564 File Offset: 0x00054764
		public Team DefenderAllyTeam
		{
			get
			{
				return this.Teams.DefenderAlly;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x00056571 File Offset: 0x00054771
		// (set) Token: 0x06001A80 RID: 6784 RVA: 0x0005657E File Offset: 0x0005477E
		public Team PlayerTeam
		{
			get
			{
				return this.Teams.Player;
			}
			set
			{
				this.Teams.Player = value;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001A81 RID: 6785 RVA: 0x0005658C File Offset: 0x0005478C
		public Team PlayerEnemyTeam
		{
			get
			{
				return this.Teams.PlayerEnemy;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001A82 RID: 6786 RVA: 0x00056599 File Offset: 0x00054799
		public Team PlayerAllyTeam
		{
			get
			{
				return this.Teams.PlayerAlly;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001A83 RID: 6787 RVA: 0x000565A6 File Offset: 0x000547A6
		// (set) Token: 0x06001A84 RID: 6788 RVA: 0x000565AE File Offset: 0x000547AE
		public Team SpectatorTeam { get; set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001A85 RID: 6789 RVA: 0x000565B7 File Offset: 0x000547B7
		IMissionTeam IMission.PlayerTeam
		{
			get
			{
				return this.PlayerTeam;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x000565BF File Offset: 0x000547BF
		public bool IsMissionEnding
		{
			get
			{
				return this.CurrentState != Mission.State.Over && this.MissionEnded;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x000565D2 File Offset: 0x000547D2
		public List<MissionLogic> MissionLogics { get; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000565DA File Offset: 0x000547DA
		public List<MissionBehavior> MissionBehaviors { get; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x000565E2 File Offset: 0x000547E2
		// (set) Token: 0x06001A8A RID: 6794 RVA: 0x000565EA File Offset: 0x000547EA
		public IInputContext InputManager { get; set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000565F3 File Offset: 0x000547F3
		// (set) Token: 0x06001A8C RID: 6796 RVA: 0x000565FB File Offset: 0x000547FB
		public bool NeedsMemoryCleanup { get; private set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x00056604 File Offset: 0x00054804
		public Agent InitialPlayerAgent
		{
			get
			{
				return this._initialPlayerAgent;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0005660C File Offset: 0x0005480C
		// (set) Token: 0x06001A8F RID: 6799 RVA: 0x00056614 File Offset: 0x00054814
		public Agent MainAgent
		{
			get
			{
				return this._mainAgent;
			}
			set
			{
				Agent mainAgent = this._mainAgent;
				this._mainAgent = value;
				Mission.OnMainAgentChangedDelegate onMainAgentChanged = this.OnMainAgentChanged;
				if (onMainAgentChanged != null)
				{
					onMainAgentChanged(mainAgent);
				}
				if (!GameNetwork.IsClient)
				{
					this.MainAgentServer = this._mainAgent;
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x00056656 File Offset: 0x00054856
		public IMissionDeploymentPlan DeploymentPlan
		{
			get
			{
				return this._deploymentPlan;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x0005665E File Offset: 0x0005485E
		public bool IsBattleSpawnPathSelectorInitialized
		{
			get
			{
				return this._battleSpawnPathSelector != null && this._battleSpawnPathSelector.IsInitialized;
			}
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00056678 File Offset: 0x00054878
		public bool GetDeploymentPlan<T>(out T deploymentPlan) where T : IMissionDeploymentPlan
		{
			deploymentPlan = default(T);
			IMissionDeploymentPlan deploymentPlan2;
			if (this._deploymentPlan != null && (deploymentPlan2 = this._deploymentPlan) is T)
			{
				T t = (T)((object)deploymentPlan2);
				deploymentPlan = t;
			}
			return deploymentPlan != null;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000566C0 File Offset: 0x000548C0
		public float GetRemovedAgentRatioForSide(BattleSideEnum side)
		{
			float num = 0f;
			if (side == BattleSideEnum.NumSides)
			{
				Debug.FailedAssert("Cannot get removed agent count for side. Invalid battle side passed!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetRemovedAgentRatioForSide", 717);
			}
			float num2 = (float)this._initialAgentCountPerSide[(int)side];
			if (num2 > 0f && this._agentCount > 0)
			{
				num = MathF.Min((float)this._removedAgentCountPerSide[(int)side] / num2, 1f);
			}
			return num;
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x00056722 File Offset: 0x00054922
		// (set) Token: 0x06001A95 RID: 6805 RVA: 0x0005672A File Offset: 0x0005492A
		public Agent MainAgentServer { get; set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x00056733 File Offset: 0x00054933
		public bool HasSpawnPath
		{
			get
			{
				return this._battleSpawnPathSelector.IsInitialized;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x00056740 File Offset: 0x00054940
		public bool IsFieldBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.FieldBattle;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x0005674B File Offset: 0x0005494B
		public bool IsSiegeBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x00056756 File Offset: 0x00054956
		public bool IsSallyOutBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00056761 File Offset: 0x00054961
		public bool IsNavalBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.NavalBattle;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0005676C File Offset: 0x0005496C
		public bool IsNavalRaidBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.NavalRaid;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x00056777 File Offset: 0x00054977
		public AgentReadOnlyList AllAgents
		{
			get
			{
				return this._allAgents;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x0005677F File Offset: 0x0005497F
		public AgentReadOnlyList Agents
		{
			get
			{
				return this._activeAgents;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x00056787 File Offset: 0x00054987
		public bool IsInventoryAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsInventoryAccessibleAtMission || this._isScreenAccessAllowed) && this.IsInventoryAccessible;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001A9F RID: 6815 RVA: 0x000567AA File Offset: 0x000549AA
		// (set) Token: 0x06001AA0 RID: 6816 RVA: 0x000567B2 File Offset: 0x000549B2
		public bool IsInventoryAccessible { private get; set; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x000567BB File Offset: 0x000549BB
		// (set) Token: 0x06001AA2 RID: 6818 RVA: 0x000567C3 File Offset: 0x000549C3
		public MissionResult MissionResult { get; private set; }

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x000567CC File Offset: 0x000549CC
		// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x000567D4 File Offset: 0x000549D4
		public MissionFocusableObjectInformationProvider FocusableObjectInformationProvider { get; private set; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x000567DD File Offset: 0x000549DD
		// (set) Token: 0x06001AA6 RID: 6822 RVA: 0x000567E5 File Offset: 0x000549E5
		public bool IsQuestScreenAccessible { private get; set; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x000567EE File Offset: 0x000549EE
		private bool _isScreenAccessAllowed
		{
			get
			{
				return this.Mode != MissionMode.Battle && this.Mode != MissionMode.Deployment && this.Mode != MissionMode.Duel && this.Mode != MissionMode.CutScene;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x0005681A File Offset: 0x00054A1A
		public bool IsQuestScreenAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsQuestScreenAccessibleAtMission || this._isScreenAccessAllowed) && this.IsQuestScreenAccessible;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x0005683D File Offset: 0x00054A3D
		// (set) Token: 0x06001AAA RID: 6826 RVA: 0x00056845 File Offset: 0x00054A45
		public bool IsCharacterWindowAccessible { private get; set; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x0005684E File Offset: 0x00054A4E
		public bool IsCharacterWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsCharacterWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsCharacterWindowAccessible;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x00056871 File Offset: 0x00054A71
		// (set) Token: 0x06001AAD RID: 6829 RVA: 0x00056879 File Offset: 0x00054A79
		public bool IsPartyWindowAccessible { private get; set; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001AAE RID: 6830 RVA: 0x00056882 File Offset: 0x00054A82
		public bool IsPartyWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsPartyWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsPartyWindowAccessible;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x000568A5 File Offset: 0x00054AA5
		// (set) Token: 0x06001AB0 RID: 6832 RVA: 0x000568AD File Offset: 0x00054AAD
		public bool IsKingdomWindowAccessible { private get; set; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x000568B6 File Offset: 0x00054AB6
		public bool IsKingdomWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsKingdomWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsKingdomWindowAccessible;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x000568D9 File Offset: 0x00054AD9
		// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x000568E1 File Offset: 0x00054AE1
		public bool IsClanWindowAccessible { private get; set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x000568EA File Offset: 0x00054AEA
		public bool IsClanWindowAccessAllowed
		{
			get
			{
				return Game.Current.GameType.IsClanWindowAccessibleAtMission && this._isScreenAccessAllowed && this.IsClanWindowAccessible;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0005690D File Offset: 0x00054B0D
		// (set) Token: 0x06001AB6 RID: 6838 RVA: 0x00056915 File Offset: 0x00054B15
		public bool IsEncyclopediaWindowAccessible { private get; set; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0005691E File Offset: 0x00054B1E
		public bool IsEncyclopediaWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsEncyclopediaWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsEncyclopediaWindowAccessible;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x00056941 File Offset: 0x00054B41
		// (set) Token: 0x06001AB9 RID: 6841 RVA: 0x00056949 File Offset: 0x00054B49
		public bool IsBannerWindowAccessible { private get; set; }

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x00056952 File Offset: 0x00054B52
		public bool IsBannerWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsBannerWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsBannerWindowAccessible;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x00056975 File Offset: 0x00054B75
		// (set) Token: 0x06001ABC RID: 6844 RVA: 0x0005697D File Offset: 0x00054B7D
		public bool DoesMissionRequireCivilianEquipment
		{
			get
			{
				return this._doesMissionRequireCivilianEquipment;
			}
			set
			{
				this._doesMissionRequireCivilianEquipment = value;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x00056986 File Offset: 0x00054B86
		// (set) Token: 0x06001ABE RID: 6846 RVA: 0x0005698E File Offset: 0x00054B8E
		public Mission.MissionTeamAITypeEnum MissionTeamAIType { get; set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x00056997 File Offset: 0x00054B97
		private Lazy<MissionRecorder> _recorder
		{
			get
			{
				return new Lazy<MissionRecorder>(() => new MissionRecorder(this));
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x000569AA File Offset: 0x00054BAA
		public MissionRecorder Recorder
		{
			get
			{
				return this._recorder.Value;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000569B7 File Offset: 0x00054BB7
		public bool CanPlayerTakeControlOfAnotherAgentWhenDead
		{
			get
			{
				return this._canPlayerTakeControlOfAnotherAgentWhenDead;
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000569BF File Offset: 0x00054BBF
		public readonly ref List<SiegeWeapon> GetAttackerWeaponsForFriendlyFirePreventing()
		{
			return ref this._attackerWeaponsForFriendlyFirePreventing;
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000569C8 File Offset: 0x00054BC8
		public void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnDeploymentPlanMade(team, isFirstPlan);
			}
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00056A1C File Offset: 0x00054C1C
		public WorldPosition GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(Vec2 directionTowards, WorldPosition originalPosition, ref float positionPenalty)
		{
			return MBAPI.IMBMission.GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(this.Pointer, ref directionTowards, ref originalPosition, ref positionPenalty);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00056A33 File Offset: 0x00054C33
		public int GetNextDynamicNavMeshIdStart()
		{
			int nextDynamicNavMeshIdStart = this._nextDynamicNavMeshIdStart;
			this._nextDynamicNavMeshIdStart += 50;
			return nextDynamicNavMeshIdStart;
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00056A4C File Offset: 0x00054C4C
		public FormationClass GetAgentTroopClass(BattleSideEnum battleSide, BasicCharacterObject agentCharacter)
		{
			if (this.GetAgentTroopClass_Override != null)
			{
				return this.GetAgentTroopClass_Override(battleSide, agentCharacter);
			}
			FormationClass formationClass = agentCharacter.GetFormationClass();
			if (this.IsSiegeBattle || this.IsNavalBattle || this.IsNavalRaidBattle || (this.IsSallyOutBattle && battleSide == BattleSideEnum.Attacker))
			{
				formationClass = formationClass.DismountedClass();
			}
			return formationClass;
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00056AAC File Offset: 0x00054CAC
		[UsedImplicitly]
		[MBCallback(null, false)]
		public WorldPosition GetClosestFleePositionForAgent(Agent agent)
		{
			if (this.GetOverriddenFleePositionForAgent != null)
			{
				WorldPosition? worldPosition = this.GetOverriddenFleePositionForAgent(agent);
				if (worldPosition != null)
				{
					return worldPosition.Value;
				}
			}
			WorldPosition worldPosition2 = agent.GetWorldPosition();
			float maximumForwardUnlimitedSpeed = agent.GetMaximumForwardUnlimitedSpeed();
			Team team = agent.Team;
			BattleSideEnum battleSideEnum = BattleSideEnum.None;
			bool flag = agent.MountAgent != null;
			if (team != null)
			{
				team.UpdateCachedEnemyDataForFleeing();
				battleSideEnum = team.Side;
			}
			MBReadOnlyList<FleePosition> mbreadOnlyList = ((this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut && agent.IsMount) ? this.GetFleePositionsForSide(BattleSideEnum.Attacker) : this.GetFleePositionsForSide(battleSideEnum));
			return this.GetClosestFleePosition(mbreadOnlyList, worldPosition2, maximumForwardUnlimitedSpeed, flag, (team != null) ? team.CachedEnemyDataForFleeing : null);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00056B50 File Offset: 0x00054D50
		public WorldPosition GetClosestFleePositionForFormation(Formation formation)
		{
			WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
			float movementSpeedMaximum = formation.QuerySystem.MovementSpeedMaximum;
			bool flag = formation.QuerySystem.IsCavalryFormation || formation.QuerySystem.IsRangedCavalryFormation;
			Team team = formation.Team;
			team.UpdateCachedEnemyDataForFleeing();
			MBReadOnlyList<FleePosition> fleePositionsForSide = this.GetFleePositionsForSide(team.Side);
			return this.GetClosestFleePosition(fleePositionsForSide, cachedMedianPosition, movementSpeedMaximum, flag, team.CachedEnemyDataForFleeing);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00056BB8 File Offset: 0x00054DB8
		private WorldPosition GetClosestFleePosition(MBReadOnlyList<FleePosition> availableFleePositions, WorldPosition runnerPosition, float runnerSpeed, bool runnerHasMount, MBReadOnlyList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>> chaserData)
		{
			int num = ((chaserData != null) ? chaserData.Count : 0);
			if (availableFleePositions.Count > 0)
			{
				float[] array = new float[availableFleePositions.Count];
				WorldPosition[] array2 = new WorldPosition[availableFleePositions.Count];
				for (int i = 0; i < availableFleePositions.Count; i++)
				{
					array[i] = 1f;
					array2[i] = new WorldPosition(this.Scene, UIntPtr.Zero, availableFleePositions[i].GetClosestPointToEscape(runnerPosition.AsVec2), false);
					array2[i].SetVec2(array2[i].AsVec2 - runnerPosition.AsVec2);
				}
				for (int j = 0; j < num; j++)
				{
					float item = chaserData[j].Item1;
					if (item > 0f)
					{
						ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool> valueTuple = chaserData[j];
						Vec2 asVec = valueTuple.Item2.AsVec2;
						int item2 = chaserData[j].Item3;
						Vec2 vec2;
						if (item2 > 1)
						{
							Vec2 item3 = chaserData[j].Item4;
							Vec2 item4 = chaserData[j].Item5;
							Vec2 vec = runnerPosition.AsVec2;
							vec2 = MBMath.GetClosestPointOnLineSegmentToPoint(in item3, in item4, in vec) - runnerPosition.AsVec2;
						}
						else
						{
							vec2 = asVec - runnerPosition.AsVec2;
						}
						for (int k = 0; k < availableFleePositions.Count; k++)
						{
							Vec2 vec = array2[k].AsVec2;
							float num2 = vec2.DotProduct(vec.Normalized());
							if (num2 > 0f)
							{
								vec = array2[k].AsVec2;
								vec = vec.LeftVec();
								float num3 = MathF.Max(MathF.Abs(vec2.DotProduct(vec.Normalized())) / item, 1f);
								float num4 = MathF.Max(num2 / runnerSpeed, 1f);
								if (num4 > num3)
								{
									float num5 = num4 / num3;
									num5 /= num2;
									array[k] += num5 * (float)item2;
								}
							}
						}
					}
				}
				for (int l = 0; l < availableFleePositions.Count; l++)
				{
					WorldPosition worldPosition = new WorldPosition(this.Scene, UIntPtr.Zero, availableFleePositions[l].GetClosestPointToEscape(runnerPosition.AsVec2), false);
					float num6;
					if (this.Scene.GetPathDistanceBetweenPositions(ref runnerPosition, ref worldPosition, 0f, out num6))
					{
						array[l] *= num6;
					}
					else
					{
						array[l] = float.MaxValue;
					}
				}
				int num7 = -1;
				float num8 = float.MaxValue;
				for (int m = 0; m < availableFleePositions.Count; m++)
				{
					if (num8 > array[m])
					{
						num7 = m;
						num8 = array[m];
					}
				}
				if (num7 >= 0)
				{
					Vec3 closestPointToEscape = availableFleePositions[num7].GetClosestPointToEscape(runnerPosition.AsVec2);
					return new WorldPosition(this.Scene, UIntPtr.Zero, closestPointToEscape, false);
				}
			}
			float[] array3 = new float[4];
			for (int n = 0; n < num; n++)
			{
				ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool> valueTuple = chaserData[n];
				Vec2 asVec2 = valueTuple.Item2.AsVec2;
				int item5 = chaserData[n].Item3;
				Vec2 vec3;
				if (item5 > 1)
				{
					Vec2 item6 = chaserData[n].Item4;
					Vec2 item7 = chaserData[n].Item5;
					Vec2 vec = runnerPosition.AsVec2;
					vec3 = MBMath.GetClosestPointOnLineSegmentToPoint(in item6, in item7, in vec) - runnerPosition.AsVec2;
				}
				else
				{
					vec3 = asVec2 - runnerPosition.AsVec2;
				}
				float num9 = vec3.Length;
				if (chaserData[n].Item6)
				{
					num9 *= 0.5f;
				}
				if (runnerHasMount)
				{
					num9 *= 2f;
				}
				float num10 = MBMath.ClampFloat(1f - (num9 - 40f) / 40f, 0.01f, 1f);
				Vec2 vec4 = vec3.Normalized();
				float num11 = 1.2f;
				float num12 = num10 * (float)item5 * num11;
				float num13 = num12 * MathF.Abs(vec4.x);
				float num14 = num12 * MathF.Abs(vec4.y);
				array3[(vec4.y < 0f) ? 0 : 1] -= num14;
				array3[(vec4.x < 0f) ? 2 : 3] -= num13;
				array3[(vec4.y < 0f) ? 1 : 0] += num14;
				array3[(vec4.x < 0f) ? 3 : 2] += num13;
			}
			float num15 = 0.04f;
			Vec3 vec5;
			Vec3 vec6;
			this.Scene.GetBoundingBox(out vec5, out vec6);
			Vec2 closestBoundaryPosition = this.GetClosestBoundaryPosition(new Vec2(runnerPosition.X, vec5.y));
			Vec2 closestBoundaryPosition2 = this.GetClosestBoundaryPosition(new Vec2(runnerPosition.X, vec6.y));
			Vec2 closestBoundaryPosition3 = this.GetClosestBoundaryPosition(new Vec2(vec5.x, runnerPosition.Y));
			Vec2 closestBoundaryPosition4 = this.GetClosestBoundaryPosition(new Vec2(vec6.x, runnerPosition.Y));
			float num16 = closestBoundaryPosition2.y - closestBoundaryPosition.y;
			float num17 = closestBoundaryPosition4.x - closestBoundaryPosition3.x;
			array3[0] += (num16 - (runnerPosition.Y - closestBoundaryPosition.y)) * num15;
			array3[1] += (num16 - (closestBoundaryPosition2.y - runnerPosition.Y)) * num15;
			array3[2] += (num17 - (runnerPosition.X - closestBoundaryPosition3.x)) * num15;
			array3[3] += (num17 - (closestBoundaryPosition4.x - runnerPosition.X)) * num15;
			Vec2 vec7;
			if (array3[0] >= array3[1] && array3[0] >= array3[2] && array3[0] >= array3[3])
			{
				vec7 = new Vec2(closestBoundaryPosition.x, closestBoundaryPosition.y);
			}
			else if (array3[1] >= array3[2] && array3[1] >= array3[3])
			{
				vec7 = new Vec2(closestBoundaryPosition2.x, closestBoundaryPosition2.y);
			}
			else if (array3[2] >= array3[3])
			{
				vec7 = new Vec2(closestBoundaryPosition3.x, closestBoundaryPosition3.y);
			}
			else
			{
				vec7 = new Vec2(closestBoundaryPosition4.x, closestBoundaryPosition4.y);
			}
			return new WorldPosition(this.Scene, UIntPtr.Zero, new Vec3(vec7, runnerPosition.GetNavMeshZ(), -1f), false);
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x00057216 File Offset: 0x00055416
		// (set) Token: 0x06001ACB RID: 6859 RVA: 0x0005721E File Offset: 0x0005541E
		public MissionTimeTracker MissionTimeTracker { get; private set; }

		// Token: 0x06001ACC RID: 6860 RVA: 0x00057228 File Offset: 0x00055428
		public MBReadOnlyList<FleePosition> GetFleePositionsForSide(BattleSideEnum side)
		{
			if (side == BattleSideEnum.NumSides)
			{
				Debug.FailedAssert("Flee position with invalid battle side field found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetFleePositionsForSide", 1280);
				return null;
			}
			int num = (int)((side == BattleSideEnum.None) ? BattleSideEnum.Defender : (side + 1));
			return this._fleePositions[num];
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x00057267 File Offset: 0x00055467
		public void AddToWeaponListForFriendlyFirePreventing(SiegeWeapon weapon)
		{
			this._attackerWeaponsForFriendlyFirePreventing.Add(weapon);
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x00057278 File Offset: 0x00055478
		public Mission(MissionInitializerRecord rec, MissionState missionState, bool needsMemoryCleanup)
		{
			this.Pointer = MBAPI.IMBMission.CreateMission(this);
			this._spawnedItemEntitiesCreatedAtRuntime = new List<SpawnedItemEntity>();
			this._missionObjects = new MBList<MissionObject>();
			this._activeMissionObjects = new MBList<MissionObject>();
			this._mountsWithoutRiders = new MBList<KeyValuePair<Agent, MissionTime>>();
			this._addedEntitiesInfo = new MBList<Mission.DynamicallyCreatedEntity>();
			this._emptyRuntimeMissionObjectIds = new Stack<ValueTuple<int, float>>();
			this.Boundaries = new Mission.MBBoundaryCollection(this);
			this.InitializerRecord = rec;
			this.CurrentState = Mission.State.NewlyCreated;
			this.IsInventoryAccessible = false;
			this.IsQuestScreenAccessible = true;
			this.IsCharacterWindowAccessible = true;
			this.IsPartyWindowAccessible = true;
			this.IsKingdomWindowAccessible = true;
			this.IsClanWindowAccessible = true;
			this.IsBannerWindowAccessible = false;
			this.IsEncyclopediaWindowAccessible = true;
			this._missilesList = new MBList<Mission.Missile>();
			this._missilesDictionary = new Dictionary<int, Mission.Missile>();
			this._activeAgents = new AgentList(256);
			this._allAgents = new AgentList(256);
			for (int i = 0; i < 3; i++)
			{
				this._fleePositions[i] = new MBList<FleePosition>(32);
			}
			for (int j = 0; j < 2; j++)
			{
				this._initialAgentCountPerSide[j] = 0;
				this._removedAgentCountPerSide[j] = 0;
			}
			this.MissionBehaviors = new List<MissionBehavior>();
			this.MissionLogics = new List<MissionLogic>();
			this._otherMissionBehaviors = new List<MissionBehavior>();
			this._missionState = missionState;
			this._battleSpawnPathSelector = new BattleSpawnPathSelector(this);
			this.Teams = new Mission.TeamCollection(this);
			this.FocusableObjectInformationProvider = new MissionFocusableObjectInformationProvider();
			this.MissionTimeTracker = new MissionTimeTracker();
			this.NeedsMemoryCleanup = needsMemoryCleanup;
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000574E5 File Offset: 0x000556E5
		public void SetCloseProximityWaveSoundsEnabled(bool value)
		{
			MBAPI.IMBMission.SetCloseProximityWaveSoundsEnabled(this.Pointer, value);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000574F8 File Offset: 0x000556F8
		public void ForceDisableOcclusion(bool value)
		{
			MBAPI.IMBMission.ForceDisableOcclusion(this.Pointer, value);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0005750C File Offset: 0x0005570C
		public void AddFleePosition(FleePosition fleePosition)
		{
			BattleSideEnum side = fleePosition.GetSide();
			if (side == BattleSideEnum.NumSides)
			{
				Debug.FailedAssert("Flee position with invalid battle side field found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "AddFleePosition", 1370);
				return;
			}
			if (side == BattleSideEnum.None)
			{
				for (int i = 0; i < this._fleePositions.Length; i++)
				{
					this._fleePositions[i].Add(fleePosition);
				}
				return;
			}
			int num = (int)(side + 1);
			this._fleePositions[num].Add(fleePosition);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x00057578 File Offset: 0x00055778
		private void FreeResources()
		{
			this.MainAgent = null;
			this.Teams.ClearResources();
			this.SpectatorTeam = null;
			this._activeAgents = null;
			this._allAgents = null;
			if (GameNetwork.NetworkPeersValid)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null)
					{
						component.ClearAllVisuals(true);
						networkCommunicator.RemoveComponent(component);
					}
					MissionRepresentativeBase component2 = networkCommunicator.GetComponent<MissionRepresentativeBase>();
					if (component2 != null)
					{
						networkCommunicator.RemoveComponent(component2);
					}
				}
			}
			if (GameNetwork.DisconnectedNetworkPeers != null)
			{
				Debug.Print("DisconnectedNetworkPeers.Clear()", 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.DisconnectedNetworkPeers.Clear();
			}
			this._missionState = null;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00057648 File Offset: 0x00055848
		public void RetreatMission()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				missionLogic.OnRetreatMission();
			}
			if (MBEditor.EditModeEnabled && MBEditor.IsEditModeOn)
			{
				MBEditor.LeaveEditMissionMode();
				return;
			}
			this.EndMission();
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000576B4 File Offset: 0x000558B4
		public void SurrenderMission()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				missionLogic.OnSurrenderMission();
			}
			if (MBEditor.EditModeEnabled && MBEditor.IsEditModeOn)
			{
				MBEditor.LeaveEditMissionMode();
				return;
			}
			this.EndMission();
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00057720 File Offset: 0x00055920
		public bool HasMissionBehavior<T>() where T : MissionBehavior
		{
			return this.GetMissionBehavior<T>() != null;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00057730 File Offset: 0x00055930
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentAddedAsCorpse(Agent affectedAgent, int corpsesToFadeIndex)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				for (int i = 0; i < affectedAgent.GetAttachedWeaponsCount(); i++)
				{
					if (affectedAgent.GetAttachedWeapon(i).Item.ItemFlags.HasAnyFlag(ItemFlags.CanBePickedUpFromCorpse))
					{
						this.SpawnAttachedWeaponOnCorpse(affectedAgent, i, -1);
					}
				}
				affectedAgent.ClearAttachedWeapons();
			}
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00057788 File Offset: 0x00055988
		public SpawnedItemEntity SpawnAttachedWeaponOnCorpse(Agent agent, int attachedWeaponIndex, int forcedSpawnIndex)
		{
			Skeleton skeleton = agent.AgentVisuals.GetSkeleton();
			if (skeleton != null)
			{
				skeleton.ForceUpdateBoneFrames();
			}
			MissionWeapon attachedWeapon = agent.GetAttachedWeapon(attachedWeaponIndex);
			GameEntity attachedWeaponEntity = agent.AgentVisuals.GetAttachedWeaponEntity(attachedWeaponIndex);
			attachedWeaponEntity.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = attachedWeaponEntity.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnAttachedWeaponOnCorpse(agent.Index, attachedWeaponIndex, firstScriptOfType.Id.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			return this.SpawnWeaponAux(attachedWeaponEntity.WeakEntity, attachedWeapon, Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithStaticPhysics, Vec3.Zero, Vec3.Zero, false, true);
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00057839 File Offset: 0x00055A39
		public void AddMountWithoutRider(Agent mount)
		{
			this._mountsWithoutRiders.Add(new KeyValuePair<Agent, MissionTime>(mount, MissionTime.Now));
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00057854 File Offset: 0x00055A54
		public void RemoveMountWithoutRider(Agent mount)
		{
			for (int i = 0; i < this._mountsWithoutRiders.Count; i++)
			{
				if (this._mountsWithoutRiders[i].Key == mount)
				{
					this._mountsWithoutRiders.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0005789C File Offset: 0x00055A9C
		public void UpdateMountReservationsAfterRiderMounts(Agent rider, Agent mount)
		{
			int selectedMountIndex = rider.GetSelectedMountIndex();
			if (selectedMountIndex >= 0 && selectedMountIndex != mount.Index)
			{
				Agent agent = Mission.Current.FindAgentWithIndex(selectedMountIndex);
				if (agent != null)
				{
					rider.HumanAIComponent.UnreserveMount(agent);
				}
			}
			int num = ((mount.CommonAIComponent != null) ? mount.CommonAIComponent.ReservedRiderAgentIndex : (-1));
			if (num >= 0)
			{
				if (num == rider.Index)
				{
					rider.HumanAIComponent.UnreserveMount(mount);
					return;
				}
				Agent agent2 = Mission.Current.FindAgentWithIndex(num);
				if (agent2 != null)
				{
					agent2.HumanAIComponent.UnreserveMount(mount);
				}
			}
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x00057924 File Offset: 0x00055B24
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentDeleted(Agent affectedAgent)
		{
			if (affectedAgent != null)
			{
				affectedAgent.State = AgentState.Deleted;
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnAgentDeleted(affectedAgent);
				}
				this._allAgents.Remove(affectedAgent);
				affectedAgent.OnDelete();
				affectedAgent.SetTeam(null, false);
			}
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0005799C File Offset: 0x00055B9C
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			Mission.OnBeforeAgentRemovedDelegate onBeforeAgentRemoved = this.OnBeforeAgentRemoved;
			if (onBeforeAgentRemoved != null)
			{
				onBeforeAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			}
			affectedAgent.State = agentState;
			if (affectorAgent != null && affectorAgent.Team != affectedAgent.Team)
			{
				affectorAgent.KillCount++;
			}
			Team team = affectedAgent.Team;
			if (team != null)
			{
				team.DeactivateAgent(affectedAgent);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnEarlyAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			}
			foreach (MissionBehavior missionBehavior2 in this.MissionBehaviors)
			{
				missionBehavior2.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			}
			bool flag = this.MainAgent == affectedAgent;
			if (flag)
			{
				affectedAgent.OnMainAgentWieldedItemChange = null;
				this.MainAgent = null;
			}
			if (this._initialPlayerAgent == affectedAgent)
			{
				this._initialPlayerAgent = null;
			}
			affectedAgent.OnAgentWieldedItemChange = null;
			affectedAgent.OnAgentMountedStateChanged = null;
			if (affectedAgent.Team != null && affectedAgent.Team.Side != BattleSideEnum.None)
			{
				this._removedAgentCountPerSide[(int)affectedAgent.Team.Side]++;
			}
			this._activeAgents.Remove(affectedAgent);
			affectedAgent.OnRemove();
			if (affectedAgent.IsMount && affectedAgent.RiderAgent == null)
			{
				this.RemoveMountWithoutRider(affectedAgent);
			}
			if (flag)
			{
				affectedAgent.Team.DelegateCommandToAI();
			}
			if (!GameNetwork.IsClientOrReplay && agentState != AgentState.Routed && affectedAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon))
			{
				EquipmentIndex offhandWieldedItemIndex = affectedAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex == EquipmentIndex.ExtraWeaponSlot)
				{
					WeaponComponentData currentUsageItem = affectedAgent.Equipment[offhandWieldedItemIndex].CurrentUsageItem;
					if (currentUsageItem != null && currentUsageItem.WeaponClass == WeaponClass.Banner)
					{
						affectedAgent.DropItem(EquipmentIndex.ExtraWeaponSlot, WeaponClass.Undefined);
					}
				}
			}
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x00057B78 File Offset: 0x00055D78
		public void OnObjectDisabled(DestructableComponent destructionComponent)
		{
			UsableMachine firstScriptOfType = destructionComponent.GameEntity.GetFirstScriptOfType<UsableMachine>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.Disable();
			}
			if (destructionComponent != null)
			{
				destructionComponent.SetAbilityOfFaces(false);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnObjectDisabled(destructionComponent);
			}
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x00057BEC File Offset: 0x00055DEC
		public bool TryGetMissileVelocityFromMissileIndex(int missileIndex, out Vec3 velocity)
		{
			velocity = Vec3.Invalid;
			Mission.Missile missile;
			if (this._missilesDictionary.TryGetValue(missileIndex, out missile))
			{
				velocity = missile.GetVelocity();
				return true;
			}
			return false;
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x00057C24 File Offset: 0x00055E24
		public MissionObjectId SpawnWeaponAsDropFromMissile(int missileIndex, MissionObject attachedMissionObject, in MatrixFrame attachLocalFrame, Mission.WeaponSpawnFlags spawnFlags, in Vec3 velocity, in Vec3 angularVelocity, int forcedSpawnIndex)
		{
			this.PrepareMissileWeaponForDrop(missileIndex);
			Mission.Missile missile = this._missilesDictionary[missileIndex];
			if (attachedMissionObject != null)
			{
				attachedMissionObject.AddStuckMissile(missile.Entity);
			}
			if (attachedMissionObject != null)
			{
				GameEntity entity = missile.Entity;
				MatrixFrame matrixFrame = attachedMissionObject.GameEntity.GetGlobalFrame();
				matrixFrame = matrixFrame.TransformToParent(in attachLocalFrame);
				entity.SetGlobalFrame(in matrixFrame, true);
			}
			else
			{
				missile.Entity.SetGlobalFrame(in attachLocalFrame, true);
			}
			missile.Entity.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = missile.Entity.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			this.SpawnWeaponAux(missile.Entity.WeakEntity, missile.Weapon, spawnFlags, velocity, angularVelocity, true, false);
			return firstScriptOfType.Id;
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00057CF6 File Offset: 0x00055EF6
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void SpawnWeaponAsDropFromAgent(Agent agent, EquipmentIndex equipmentIndex, ref Vec3 globalVelocity, ref Vec3 globalAngularVelocity, Mission.WeaponSpawnFlags spawnFlags)
		{
			this.SpawnWeaponAsDropFromAgentAux(agent, equipmentIndex, ref globalVelocity, ref globalAngularVelocity, spawnFlags, -1);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00057D08 File Offset: 0x00055F08
		public void SpawnWeaponAsDropFromAgentAux(Agent agent, EquipmentIndex equipmentIndex, ref Vec3 globalVelocity, ref Vec3 globalAngularVelocity, Mission.WeaponSpawnFlags spawnFlags, int forcedSpawnIndex)
		{
			agent.AgentVisuals.GetSkeleton().ForceUpdateBoneFrames();
			agent.PrepareWeaponForDropInEquipmentSlot(equipmentIndex, (spawnFlags & Mission.WeaponSpawnFlags.WithHolster) > Mission.WeaponSpawnFlags.None);
			WeakGameEntity weaponEntityFromEquipmentSlot = agent.GetWeaponEntityFromEquipmentSlot(equipmentIndex);
			weaponEntityFromEquipmentSlot.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = weaponEntityFromEquipmentSlot.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			CompressionMission.SpawnedItemVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalVelocity.x);
			CompressionMission.SpawnedItemVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalVelocity.y);
			CompressionMission.SpawnedItemVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalVelocity.z);
			CompressionMission.SpawnedItemAngularVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalAngularVelocity.x);
			CompressionMission.SpawnedItemAngularVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalAngularVelocity.y);
			CompressionMission.SpawnedItemAngularVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalAngularVelocity.z);
			MissionWeapon missionWeapon = agent.Equipment[equipmentIndex];
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnWeaponAsDropFromAgent(agent.Index, equipmentIndex, globalVelocity, globalAngularVelocity, spawnFlags, firstScriptOfType.Id.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			agent.OnWeaponDrop(equipmentIndex);
			this.SpawnWeaponAux(weaponEntityFromEquipmentSlot, missionWeapon, spawnFlags, globalVelocity, globalAngularVelocity, true, false);
			if (!GameNetwork.IsClientOrReplay)
			{
				for (int i = 0; i < missionWeapon.GetAttachedWeaponsCount(); i++)
				{
					if (missionWeapon.GetAttachedWeapon(i).Item.ItemFlags.HasAnyFlag(ItemFlags.CanBePickedUpFromCorpse))
					{
						this.SpawnAttachedWeaponOnSpawnedWeapon(firstScriptOfType, i, -1);
					}
				}
			}
			Action<Agent, SpawnedItemEntity> onItemDrop = this.OnItemDrop;
			if (onItemDrop == null)
			{
				return;
			}
			onItemDrop(agent, firstScriptOfType);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00057E94 File Offset: 0x00056094
		public void SpawnAttachedWeaponOnSpawnedWeapon(SpawnedItemEntity spawnedWeapon, int attachmentIndex, int forcedSpawnIndex)
		{
			WeakGameEntity child = spawnedWeapon.GameEntity.GetChild(attachmentIndex);
			child.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = child.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			this.SpawnWeaponAux(child, spawnedWeapon.WeaponCopy.GetAttachedWeapon(attachmentIndex), Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithStaticPhysics, Vec3.Zero, Vec3.Zero, false, false);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnAttachedWeaponOnSpawnedWeapon(spawnedWeapon.Id, attachmentIndex, firstScriptOfType.Id.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00057F36 File Offset: 0x00056136
		public GameEntity SpawnWeaponWithNewEntity(ref MissionWeapon weapon, Mission.WeaponSpawnFlags spawnFlags, MatrixFrame frame)
		{
			return this.SpawnWeaponWithNewEntityAux(weapon, spawnFlags, frame, -1, null, false, false);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00057F4C File Offset: 0x0005614C
		public GameEntity SpawnWeaponWithNewEntityAux(MissionWeapon weapon, Mission.WeaponSpawnFlags spawnFlags, MatrixFrame frame, int forcedSpawnIndex, MissionObject attachedMissionObject, bool hasLifeTime, bool spawnedOnACorpse = false)
		{
			GameEntity gameEntity = GameEntityExtensions.Instantiate(this.Scene, weapon, spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster), true);
			gameEntity.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = gameEntity.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			if (attachedMissionObject != null)
			{
				attachedMissionObject.GameEntity.AddChild(gameEntity.WeakEntity, false);
			}
			if (attachedMissionObject != null)
			{
				MatrixFrame matrixFrame = attachedMissionObject.GameEntity.GetGlobalFrame().TransformToParent(in frame);
				if (!matrixFrame.rotation.IsOrthonormal())
				{
					matrixFrame.rotation.Orthonormalize();
				}
				gameEntity.SetGlobalFrame(in matrixFrame, true);
			}
			else
			{
				gameEntity.SetGlobalFrame(in frame, true);
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnWeaponWithNewEntity(weapon, spawnFlags, firstScriptOfType.Id.Id, frame, (attachedMissionObject != null) ? attachedMissionObject.Id : MissionObjectId.Invalid, true, hasLifeTime, spawnedOnACorpse));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				for (int i = 0; i < weapon.GetAttachedWeaponsCount(); i++)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new AttachWeaponToSpawnedWeapon(weapon.GetAttachedWeapon(i), firstScriptOfType.Id, weapon.GetAttachedWeaponFrame(i)));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			Vec3 zero = Vec3.Zero;
			this.SpawnWeaponAux(gameEntity.WeakEntity, weapon, spawnFlags, zero, zero, hasLifeTime, spawnedOnACorpse);
			return gameEntity;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000580B0 File Offset: 0x000562B0
		public void AttachWeaponWithNewEntityToSpawnedWeapon(MissionWeapon weapon, SpawnedItemEntity spawnedItem, MatrixFrame attachLocalFrame)
		{
			GameEntity gameEntity = GameEntityExtensions.Instantiate(this.Scene, weapon, false, true);
			spawnedItem.GameEntity.AddChild(gameEntity.WeakEntity, false);
			gameEntity.SetFrame(ref attachLocalFrame, true);
			spawnedItem.AttachWeaponToWeapon(weapon, ref attachLocalFrame);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x000580F4 File Offset: 0x000562F4
		private SpawnedItemEntity SpawnWeaponAux(WeakGameEntity weaponEntity, MissionWeapon weapon, Mission.WeaponSpawnFlags spawnFlags, Vec3 globalVelocity, Vec3 globalAngularVelocity, bool hasLifeTime, bool spawnedOnACorpse = false)
		{
			SpawnedItemEntity firstScriptOfType = weaponEntity.GetFirstScriptOfType<SpawnedItemEntity>();
			bool flag = weapon.IsBanner();
			MissionWeapon missionWeapon = weapon;
			bool flag2 = !flag && hasLifeTime;
			Mission.WeaponSpawnFlags weaponSpawnFlags = spawnFlags;
			Vec3 vec = (flag ? globalVelocity : Vec3.Zero);
			firstScriptOfType.Initialize(missionWeapon, flag2, weaponSpawnFlags, in vec, spawnedOnACorpse);
			if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics | Mission.WeaponSpawnFlags.WithStaticPhysics))
			{
				BodyFlags bodyFlags = BodyFlags.OnlyCollideWithRaycast | BodyFlags.DroppedItem;
				if (weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp) || spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.CannotBePickedUp))
				{
					bodyFlags |= BodyFlags.DoNotCollideWithRaycast;
				}
				bodyFlags |= BodyFlags.Moveable;
				weaponEntity.AddBodyFlags(bodyFlags, false);
				WeaponData weaponData = weapon.GetWeaponData(true);
				this.RecalculateBody(ref weaponData, weapon.Item.ItemComponent, weapon.Item.WeaponDesign, ref spawnFlags);
				int num = -1;
				if (flag)
				{
					weaponEntity.AddPhysics(weaponData.BaseWeight, weaponData.CenterOfMassShift, weaponData.Shape, globalVelocity, globalAngularVelocity, PhysicsMaterial.GetFromIndex(weaponData.PhysicsMaterialIndex), true, num);
				}
				else if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics | Mission.WeaponSpawnFlags.WithStaticPhysics))
				{
					int num2 = (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster) ? MathF.Max((int)(weapon.IsAnyConsumable() ? weapon.ModifiedMaxAmount : weapon.MaxAmmo), 1) : 1);
					float num3 = weaponData.BaseWeight * (float)num2;
					weaponEntity.AddPhysics(num3, weaponData.CenterOfMassShift, weaponData.Shape, globalVelocity, globalAngularVelocity, PhysicsMaterial.GetFromIndex(weaponData.PhysicsMaterialIndex), spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithStaticPhysics), num);
					if (weaponEntity.Parent != WeakGameEntity.Invalid && spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithStaticPhysics))
					{
						weaponEntity.SetPhysicsMoveToBatched(true);
						weaponEntity.ConvertDynamicBodyToRayCast();
					}
					else
					{
						weaponEntity.SetPhysicsStateOnlyVariable(true, true);
					}
				}
				weaponData.DeinitializeManagedPointers();
			}
			return firstScriptOfType;
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0005828C File Offset: 0x0005648C
		public void OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType)
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnEquipItemsFromSpawnEquipmentBegin(agent, creationType);
			}
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000582E0 File Offset: 0x000564E0
		public void OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnEquipItemsFromSpawnEquipment(agent, creationType);
			}
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00058334 File Offset: 0x00056534
		public static int GetCurrentVolumeGeneratorVersion()
		{
			return MBAPI.IMBMission.GetCurrentVolumeGeneratorVersion();
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00058340 File Offset: 0x00056540
		[CommandLineFunctionality.CommandLineArgumentFunction("flee_enemies", "mission")]
		public static string MakeEnemiesFleeCheat(List<string> strings)
		{
			Game game = Game.Current;
			if (game == null || !game.CheatMode)
			{
				return "Cheat mode is not enabled.";
			}
			if (GameNetwork.IsClientOrReplay)
			{
				return "does not work in multiplayer";
			}
			if (Mission.Current != null && Mission.Current.Agents != null)
			{
				foreach (Agent agent2 in Mission.Current.Agents.Where<Agent>((Agent agent) => agent.IsHuman && agent.IsActive() && agent.Team.IsEnemyOf(Mission.Current.PlayerTeam)))
				{
					CommonAIComponent commonAIComponent = agent2.CommonAIComponent;
					if (commonAIComponent != null)
					{
						commonAIComponent.Panic();
					}
				}
				return "enemies are fleeing";
			}
			return "mission is not available";
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00058404 File Offset: 0x00056604
		[CommandLineFunctionality.CommandLineArgumentFunction("flee_team", "mission")]
		public static string MakeTeamFleeCheat(List<string> strings)
		{
			if (GameNetwork.IsClientOrReplay)
			{
				return "does not work in multiplayer";
			}
			if (Mission.Current == null || Mission.Current.Agents == null)
			{
				return "mission is not available";
			}
			string text = "Usage 1: flee_team [ Attacker | AttackerAlly | Defender | DefenderAlly ]\nUsage 2: flee_team [ Attacker | AttackerAlly | Defender | DefenderAlly ] [FormationNo]";
			if (strings.IsEmpty<string>() || strings[0] == "help")
			{
				return "makes an entire team or a team's formation flee battle.\n" + text;
			}
			if (strings.Count >= 3)
			{
				return "invalid number of parameters.\n" + text;
			}
			string text2 = strings[0];
			Team targetTeam = null;
			string text3 = text2.ToLower();
			if (!(text3 == "attacker"))
			{
				if (!(text3 == "attackerally"))
				{
					if (!(text3 == "defender"))
					{
						if (text3 == "defenderally")
						{
							targetTeam = Mission.Current.DefenderAllyTeam;
						}
					}
					else
					{
						targetTeam = Mission.Current.DefenderTeam;
					}
				}
				else
				{
					targetTeam = Mission.Current.AttackerAllyTeam;
				}
			}
			else
			{
				targetTeam = Mission.Current.AttackerTeam;
			}
			if (targetTeam == null)
			{
				return "given team is not valid";
			}
			Formation targetFormation = null;
			if (strings.Count == 2)
			{
				int num = 8;
				int num2 = int.Parse(strings[1]);
				if (num2 < 0 || num2 >= num)
				{
					return "invalid formation index. formation index should be between [0, " + (num - 1) + "]";
				}
				FormationClass formationClass = (FormationClass)num2;
				targetFormation = targetTeam.GetFormation(formationClass);
			}
			if (targetFormation == null)
			{
				IEnumerable<Agent> agents = Mission.Current.Agents;
				Func<Agent, bool> <>9__0;
				Func<Agent, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (Agent agent) => agent.IsHuman && agent.Team == targetTeam);
				}
				foreach (Agent agent3 in agents.Where<Agent>(func))
				{
					CommonAIComponent commonAIComponent = agent3.CommonAIComponent;
					if (commonAIComponent != null)
					{
						commonAIComponent.Panic();
					}
				}
				return "agents in team: " + text2 + " are fleeing";
			}
			IEnumerable<Agent> agents2 = Mission.Current.Agents;
			Func<Agent, bool> <>9__1;
			Func<Agent, bool> func2;
			if ((func2 = <>9__1) == null)
			{
				func2 = (<>9__1 = (Agent agent) => agent.IsHuman && agent.Formation == targetFormation);
			}
			foreach (Agent agent2 in agents2.Where<Agent>(func2))
			{
				CommonAIComponent commonAIComponent2 = agent2.CommonAIComponent;
				if (commonAIComponent2 != null)
				{
					commonAIComponent2.Panic();
				}
			}
			return string.Concat(new object[]
			{
				"agents in team: ",
				text2,
				" and formation: ",
				(int)targetFormation.FormationIndex,
				" (",
				targetFormation.FormationIndex.ToString(),
				") are fleeing"
			});
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x000586E4 File Offset: 0x000568E4
		public void RecalculateBody(ref WeaponData weaponData, ItemComponent itemComponent, WeaponDesign craftedWeaponData, ref Mission.WeaponSpawnFlags spawnFlags)
		{
			WeaponComponent weaponComponent = (WeaponComponent)itemComponent;
			ItemObject item = weaponComponent.Item;
			if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster))
			{
				weaponData.Shape = (string.IsNullOrEmpty(item.HolsterBodyName) ? null : PhysicsShape.GetFromResource(item.HolsterBodyName, false));
			}
			else
			{
				weaponData.Shape = (string.IsNullOrEmpty(item.BodyName) ? null : PhysicsShape.GetFromResource(item.BodyName, false));
			}
			PhysicsShape physicsShape = weaponData.Shape;
			if (physicsShape == null)
			{
				Debug.FailedAssert("Item has no body! Applying a default body, but this should not happen! Check this!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RecalculateBody", 2210);
				physicsShape = PhysicsShape.GetFromResource("bo_axe_short", false);
			}
			if (!weaponComponent.Item.ItemFlags.HasAnyFlag(ItemFlags.DoNotScaleBodyAccordingToWeaponLength))
			{
				if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster) || !item.RecalculateBody)
				{
					weaponData.Shape = physicsShape;
				}
				else
				{
					PhysicsShape physicsShape2 = physicsShape.CreateCopy();
					weaponData.Shape = physicsShape2;
					float num = (float)weaponComponent.PrimaryWeapon.WeaponLength * 0.01f;
					if (craftedWeaponData != null)
					{
						physicsShape2.Clear();
						physicsShape2.InitDescription();
						float num2 = 0f;
						float num3 = 0f;
						float num4 = 0f;
						for (int i = 0; i < craftedWeaponData.UsedPieces.Length; i++)
						{
							WeaponDesignElement weaponDesignElement = craftedWeaponData.UsedPieces[i];
							if (weaponDesignElement.IsValid)
							{
								float scaledPieceOffset = weaponDesignElement.ScaledPieceOffset;
								float num5 = craftedWeaponData.PiecePivotDistances[i];
								float num6 = num5 + scaledPieceOffset - weaponDesignElement.ScaledDistanceToPreviousPiece;
								float num7 = num5 - scaledPieceOffset + weaponDesignElement.ScaledDistanceToNextPiece;
								num2 = MathF.Min(num6, num2);
								if (num7 > num3)
								{
									num3 = num7;
									num4 = (num7 + num6) * 0.5f;
								}
							}
						}
						WeaponDesignElement weaponDesignElement2 = craftedWeaponData.UsedPieces[2];
						if (weaponDesignElement2.IsValid)
						{
							float scaledPieceOffset2 = weaponDesignElement2.ScaledPieceOffset;
							num2 -= scaledPieceOffset2;
						}
						physicsShape2.AddCapsule(new CapsuleData(0.035f, new Vec3(0f, 0f, craftedWeaponData.CraftedWeaponLength, -1f), new Vec3(0f, 0f, num2, -1f)));
						bool flag = false;
						if (craftedWeaponData.UsedPieces[1].IsValid)
						{
							float num8 = craftedWeaponData.PiecePivotDistances[1];
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(-0.1f, 0f, num8, -1f), new Vec3(0.1f, 0f, num8, -1f)));
							flag = true;
						}
						if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.ThrowingAxe)
						{
							WeaponDesignElement weaponDesignElement3 = craftedWeaponData.UsedPieces[0];
							float num9 = craftedWeaponData.PiecePivotDistances[0];
							float num10 = num9 + weaponDesignElement3.CraftingPiece.Length * 0.8f;
							float num11 = num9 - weaponDesignElement3.CraftingPiece.Length * 0.8f;
							float num12 = num9 + weaponDesignElement3.CraftingPiece.Length;
							float num13 = num9 - weaponDesignElement3.CraftingPiece.Length;
							float bladeWidth = weaponDesignElement3.CraftingPiece.BladeData.BladeWidth;
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(0f, 0f, num10, -1f), new Vec3(-bladeWidth, 0f, num12, -1f)));
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(0f, 0f, num11, -1f), new Vec3(-bladeWidth, 0f, num13, -1f)));
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(-bladeWidth, 0f, num12, -1f), new Vec3(-bladeWidth, 0f, num13, -1f)));
							flag = true;
						}
						if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.Javelin)
						{
							float num14 = craftedWeaponData.PiecePivotDistances[0];
							physicsShape2.AddCapsule(new CapsuleData(0.025f, new Vec3(-0.05f, 0f, num14, -1f), new Vec3(0.05f, 0f, num14, -1f)));
							flag = true;
						}
						if (!flag)
						{
							physicsShape2.AddCapsule(new CapsuleData(0.025f, new Vec3(-0.05f, 0f, num4, -1f), new Vec3(0.05f, 0f, num4, -1f)));
						}
					}
					else
					{
						weaponData.Shape.Prepare();
						int num15 = physicsShape.CapsuleCount();
						if (num15 == 0)
						{
							Debug.FailedAssert("Item has 0 body parts. Applying a default body, but this should not happen! Check this!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RecalculateBody", 2348);
							return;
						}
						switch (weaponComponent.PrimaryWeapon.WeaponClass)
						{
						case WeaponClass.Dagger:
						case WeaponClass.OneHandedSword:
						case WeaponClass.TwoHandedSword:
						case WeaponClass.ThrowingKnife:
						{
							CapsuleData capsuleData = default(CapsuleData);
							physicsShape2.GetCapsule(ref capsuleData, 0);
							float radius = capsuleData.Radius;
							Vec3 p = capsuleData.P1;
							Vec3 p2 = capsuleData.P2;
							physicsShape2.SetCapsule(new CapsuleData(radius, new Vec3(p.x, p.y, p.z * num, -1f), p2), 0);
							break;
						}
						case WeaponClass.OneHandedAxe:
						case WeaponClass.TwoHandedAxe:
						case WeaponClass.Mace:
						case WeaponClass.TwoHandedMace:
						case WeaponClass.OneHandedPolearm:
						case WeaponClass.TwoHandedPolearm:
						case WeaponClass.LowGripPolearm:
						case WeaponClass.Arrow:
						case WeaponClass.Bolt:
						case WeaponClass.Crossbow:
						case WeaponClass.ThrowingAxe:
						case WeaponClass.Javelin:
						case WeaponClass.Banner:
						{
							CapsuleData capsuleData2 = default(CapsuleData);
							physicsShape2.GetCapsule(ref capsuleData2, 0);
							float radius2 = capsuleData2.Radius;
							Vec3 p3 = capsuleData2.P1;
							Vec3 p4 = capsuleData2.P2;
							physicsShape2.SetCapsule(new CapsuleData(radius2, new Vec3(p3.x, p3.y, p3.z * num, -1f), p4), 0);
							for (int j = 1; j < num15; j++)
							{
								CapsuleData capsuleData3 = default(CapsuleData);
								physicsShape2.GetCapsule(ref capsuleData3, j);
								float radius3 = capsuleData3.Radius;
								Vec3 p5 = capsuleData3.P1;
								Vec3 p6 = capsuleData3.P2;
								physicsShape2.SetCapsule(new CapsuleData(radius3, new Vec3(p5.x, p5.y, p5.z * num, -1f), new Vec3(p6.x, p6.y, p6.z * num, -1f)), j);
							}
							break;
						}
						case WeaponClass.SmallShield:
						case WeaponClass.LargeShield:
							Debug.FailedAssert("Shields should not have recalculate body flag.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RecalculateBody", 2422);
							break;
						}
					}
				}
			}
			weaponData.CenterOfMassShift = weaponData.Shape.GetWeaponCenterOfMass();
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00058D8C File Offset: 0x00056F8C
		[UsedImplicitly]
		[MBCallback(null, true)]
		internal void OnFixedTick(float fixedDt)
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnFixedMissionTick(fixedDt);
			}
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00058DC4 File Offset: 0x00056FC4
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnPreTick(float dt)
		{
			this.WaitTickCompletion();
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnPreMissionTick(dt);
			}
			this.TickDebugAgents();
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00058E08 File Offset: 0x00057008
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void ApplySkeletonScaleToAllEquippedItems(string itemName)
		{
			int count = this.Agents.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < 12; j++)
				{
					EquipmentElement equipmentElement = this.Agents[i].SpawnEquipment[j];
					if (!equipmentElement.IsEmpty && equipmentElement.Item.StringId == itemName)
					{
						HorseComponent horseComponent = equipmentElement.Item.HorseComponent;
						if (((horseComponent != null) ? horseComponent.SkeletonScale : null) != null)
						{
							this.Agents[i].AgentVisuals.ApplySkeletonScale(equipmentElement.Item.HorseComponent.SkeletonScale.MountSitBoneScale, equipmentElement.Item.HorseComponent.SkeletonScale.MountRadiusAdder, equipmentElement.Item.HorseComponent.SkeletonScale.BoneIndices, equipmentElement.Item.HorseComponent.SkeletonScale.Scales);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00058F0C File Offset: 0x0005710C
		[CommandLineFunctionality.CommandLineArgumentFunction("set_facial_anim_to_agent", "mission")]
		public static string SetFacialAnimToAgent(List<string> strings)
		{
			Mission mission = Mission.Current;
			if (mission == null)
			{
				return "Mission could not be found";
			}
			if (strings.Count != 2)
			{
				return "Enter agent index and animation name please";
			}
			int num;
			if (int.TryParse(strings[0], out num) && num >= 0)
			{
				foreach (Agent agent in mission.Agents)
				{
					if (agent.Index == num)
					{
						agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.High, strings[1], true);
						return "Done";
					}
				}
			}
			return "Please enter a valid agent index";
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00058FB4 File Offset: 0x000571B4
		private void WaitTickCompletion()
		{
			while (!this.tickCompleted)
			{
				Thread.Sleep(1);
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x00058FC8 File Offset: 0x000571C8
		private void AgentTickMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.AllAgents[i].TickParallel(dt);
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x00058FF4 File Offset: 0x000571F4
		public void TickAgentsAndTeamsImp(float dt, bool tickPaused)
		{
			float num = (tickPaused ? 0f : dt);
			TWParallel.For(0, this.AllAgents.Count, num, new TWParallel.ParallelForWithDtAuxPredicate(this.AgentTickMT), 16);
			foreach (Agent agent in this.AllAgents)
			{
				agent.Tick(num);
			}
			foreach (Team team in this.Teams)
			{
				team.Tick(dt);
			}
			this.tickCompleted = true;
			foreach (MBSubModuleBase mbsubModuleBase in this._cachedSubModuleList)
			{
				mbsubModuleBase.AfterAsyncTickTick(dt);
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000590F8 File Offset: 0x000572F8
		[CommandLineFunctionality.CommandLineArgumentFunction("formation_speed_adjustment_enabled", "ai")]
		public static string EnableSpeedAdjustmentCommand(List<string> strings)
		{
			if (!GameNetwork.IsSessionActive)
			{
				HumanAIComponent.FormationSpeedAdjustmentEnabled = !HumanAIComponent.FormationSpeedAdjustmentEnabled;
				string text = "Speed Adjustment ";
				if (HumanAIComponent.FormationSpeedAdjustmentEnabled)
				{
					text += "enabled";
				}
				else
				{
					text += "disabled";
				}
				return text;
			}
			return "Does not work on multiplayer.";
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00059148 File Offset: 0x00057348
		public void OnTick(float dt, float realDt, bool updateCamera, bool doAsyncAITick)
		{
			this.ApplyGeneratedCombatLogs();
			if (this.InputManager == null)
			{
				this.InputManager = new EmptyInputContext();
			}
			for (int i = 0; i < this._tickActions.Count; i++)
			{
				ValueTuple<Mission.MissionTickAction, Agent, int, int> valueTuple = this._tickActions[i];
				Agent item = valueTuple.Item2;
				if (item.IsActive())
				{
					switch (valueTuple.Item1)
					{
					case Mission.MissionTickAction.TryToSheathWeaponInHand:
						item.TryToSheathWeaponInHand((Agent.HandIndex)valueTuple.Item3, (Agent.WeaponWieldActionType)valueTuple.Item4);
						break;
					case Mission.MissionTickAction.RemoveEquippedWeapon:
						item.RemoveEquippedWeapon((EquipmentIndex)valueTuple.Item3);
						break;
					case Mission.MissionTickAction.TryToWieldWeaponInSlot:
						item.TryToWieldWeaponInSlot((EquipmentIndex)valueTuple.Item3, (Agent.WeaponWieldActionType)valueTuple.Item4, false);
						break;
					case Mission.MissionTickAction.DropItem:
						if (!item.Equipment[valueTuple.Item3].IsEmpty)
						{
							item.DropItem((EquipmentIndex)valueTuple.Item3, WeaponClass.Undefined);
						}
						break;
					case Mission.MissionTickAction.RegisterDrownBlow:
					{
						Blow blow = new Blow(item.Index);
						blow.DamageType = DamageTypes.Blunt;
						blow.BoneIndex = item.Monster.HeadLookDirectionBoneIndex;
						blow.BaseMagnitude = 10f;
						blow.GlobalPosition = item.Position;
						blow.GlobalPosition.z = blow.GlobalPosition.z + item.GetEyeGlobalHeight();
						blow.DamagedPercentage = 1f;
						blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
						blow.SwingDirection = item.LookDirection;
						blow.Direction = blow.SwingDirection;
						blow.InflictedDamage = 10;
						blow.DamageCalculated = true;
						sbyte mainHandItemBoneIndex = item.Monster.MainHandItemBoneIndex;
						AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, item.Velocity, Vec3.Up);
						item.RegisterBlow(blow, in attackCollisionDataForDebugPurpose);
						item.MakeVoice(SkinVoiceManager.VoiceType.Drown, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
						if (item.Controller == AgentControllerType.AI)
						{
							Agent agent = item;
							Vec3 vec = new Vec3(0f, 0f, -20f, -1f);
							agent.AddAcceleration(in vec);
						}
						CombatLogData combatLogData = new CombatLogData(true, true, item.IsMine, false, false, false, true, item.IsMine, item.Health <= 0f, false, false, false, null, true, false, false, 0f);
						combatLogData.InflictedDamage = blow.InflictedDamage;
						combatLogData.IsSpecialDamage = true;
						Mission.Current.AddCombatLogSafe(item, item, combatLogData);
						break;
					}
					case Mission.MissionTickAction.RegisterBurnBlow:
						if (item.Health - 2f > 1f)
						{
							item.Health -= 2f;
						}
						else
						{
							Blow blow2 = new Blow(item.Index);
							blow2.DamageType = DamageTypes.Blunt;
							blow2.BaseMagnitude = item.Health + 1f;
							blow2.InflictedDamage = (int)blow2.BaseMagnitude;
							blow2.GlobalPosition = item.Position;
							blow2.GlobalPosition.z = blow2.GlobalPosition.z + item.GetEyeGlobalHeight();
							blow2.DamagedPercentage = 1f;
							item.Die(blow2, Agent.KillInfo.Invalid);
						}
						break;
					}
				}
			}
			this._tickActions.Clear();
			this.MissionTimeTracker.Tick(dt);
			this.CheckMissionEnd(this.CurrentTime);
			if (this.IsFastForward && this.MissionEnded)
			{
				this.IsFastForward = false;
			}
			if (this.CurrentState == Mission.State.Continuing)
			{
				if (this._inMissionLoadingScreenTimer != null && this._inMissionLoadingScreenTimer.Check(this.CurrentTime))
				{
					this._inMissionLoadingScreenTimer = null;
					Action onLoadingEndedAction = this._onLoadingEndedAction;
					if (onLoadingEndedAction != null)
					{
						onLoadingEndedAction();
					}
					LoadingWindow.DisableGlobalLoadingWindow();
				}
				for (int j = this.MissionBehaviors.Count - 1; j >= 0; j--)
				{
					this.MissionBehaviors[j].OnPreDisplayMissionTick(dt);
				}
				if (!GameNetwork.IsDedicatedServer && updateCamera)
				{
					this._missionState.Handler.UpdateCamera(this, realDt);
				}
				this.tickCompleted = false;
				for (int k = this.MissionBehaviors.Count - 1; k >= 0; k--)
				{
					this.MissionBehaviors[k].OnMissionTick(dt);
				}
				for (int l = this._dynamicEntities.Count - 1; l >= 0; l--)
				{
					Mission.DynamicEntityInfo dynamicEntityInfo = this._dynamicEntities[l];
					if (dynamicEntityInfo.TimerToDisable.Check(this.CurrentTime))
					{
						dynamicEntityInfo.Entity.RemoveEnginePhysics();
						dynamicEntityInfo.Entity.Remove(79);
						this._dynamicEntities.RemoveAt(l);
					}
				}
				this.HandleSpawnedItems();
				DebugNetworkEventStatistics.EndTick(dt);
				if (this.CurrentState == Mission.State.Continuing && this.IsFriendlyMission && !this.IsInPhotoMode)
				{
					if (this.InputManager.IsGameKeyDown(4))
					{
						this.OnEndMissionRequest();
					}
					else
					{
						this._leaveMissionTimer = null;
					}
				}
				if (doAsyncAITick)
				{
					this.TickAgentsAndTeamsAsync(dt);
					return;
				}
				this.TickAgentsAndTeamsImp(dt, false);
			}
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x00059656 File Offset: 0x00057856
		public void AddTickAction(Mission.MissionTickAction action, Agent agent, int param1, int param2)
		{
			this._tickActions.Add(new ValueTuple<Mission.MissionTickAction, Agent, int, int>(action, agent, param1, param2));
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00059670 File Offset: 0x00057870
		public void AddTickActionMT(Mission.MissionTickAction action, Agent agent, int param1, int param2)
		{
			object tickActionsLock = this._tickActionsLock;
			lock (tickActionsLock)
			{
				this._tickActions.Add(new ValueTuple<Mission.MissionTickAction, Agent, int, int>(action, agent, param1, param2));
			}
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000596C0 File Offset: 0x000578C0
		public void RemoveSpawnedItemsAndMissiles()
		{
			this.ClearMissiles();
			this._missilesList.Clear();
			this._missilesDictionary.Clear();
			this.RemoveSpawnedMissionObjects();
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000596E4 File Offset: 0x000578E4
		public void AfterStart()
		{
			this._activeAgents.Clear();
			this._allAgents.Clear();
			this._tickActions.Clear();
			this._cachedSubModuleList = Module.CurrentModule.CollectSubModules();
			foreach (MBSubModuleBase mbsubModuleBase in this._cachedSubModuleList)
			{
				mbsubModuleBase.OnBeforeMissionBehaviorInitialize(this);
			}
			for (int i = 0; i < this.MissionBehaviors.Count; i++)
			{
				this.MissionBehaviors[i].OnBehaviorInitialize();
			}
			foreach (MBSubModuleBase mbsubModuleBase2 in this._cachedSubModuleList)
			{
				mbsubModuleBase2.OnMissionBehaviorInitialize(this);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.EarlyStart();
			}
			this._battleSpawnPathSelector.Initialize();
			this._deploymentPlan.Initialize();
			foreach (MissionBehavior missionBehavior2 in this.MissionBehaviors)
			{
				missionBehavior2.AfterStart();
			}
			foreach (MissionObject missionObject in this.MissionObjects)
			{
				missionObject.AfterMissionStart();
			}
			if (MissionGameModels.Current.ApplyWeatherEffectsModel != null)
			{
				MissionGameModels.Current.ApplyWeatherEffectsModel.ApplyWeatherEffects();
			}
			this.CurrentState = Mission.State.Continuing;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000598C4 File Offset: 0x00057AC4
		public void AfterMissionLoadingFinished()
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAfterMissionLoadingFinished();
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00059914 File Offset: 0x00057B14
		public void OnEndMissionRequest()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				bool flag;
				InquiryData inquiryData = missionLogic.OnEndMissionRequest(out flag);
				if (!flag)
				{
					this._leaveMissionTimer = null;
					return;
				}
				if (inquiryData != null)
				{
					this._leaveMissionTimer = null;
					InformationManager.ShowInquiry(inquiryData, true, false);
					return;
				}
			}
			if (this._leaveMissionTimer != null)
			{
				if (this._leaveMissionTimer.ElapsedTime > 0.6f)
				{
					this._leaveMissionTimer = null;
					this.EndMission();
					return;
				}
			}
			else
			{
				this._leaveMissionTimer = new BasicMissionTimer();
			}
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x000599BC File Offset: 0x00057BBC
		public float GetMissionEndTimeInSeconds()
		{
			return 0.6f;
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000599C3 File Offset: 0x00057BC3
		public float GetMissionEndTimerValue()
		{
			if (this._leaveMissionTimer == null)
			{
				return -1f;
			}
			return this._leaveMissionTimer.ElapsedTime;
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000599E0 File Offset: 0x00057BE0
		private void ApplyGeneratedCombatLogs()
		{
			if (!this._combatLogsCreated.IsEmpty)
			{
				CombatLogData combatLogData;
				while (this._combatLogsCreated.TryDequeue(out combatLogData))
				{
					CombatLogManager.GenerateCombatLog(combatLogData);
				}
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x00059A14 File Offset: 0x00057C14
		public int GetMemberCountOfSide(BattleSideEnum side)
		{
			int num = 0;
			foreach (Team team in this.Teams)
			{
				if (team.Side == side)
				{
					num += team.ActiveAgents.Count;
				}
			}
			return num;
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00059A7C File Offset: 0x00057C7C
		public Path GetInitialSpawnPath()
		{
			return this._battleSpawnPathSelector.InitialPath;
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00059A8C File Offset: 0x00057C8C
		public SpawnPathData GetInitialSpawnPathData(BattleSideEnum battleSide)
		{
			SpawnPathData spawnPathData;
			this._battleSpawnPathSelector.GetInitialPathDataOfSide(battleSide, out spawnPathData);
			return spawnPathData;
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x00059AA9 File Offset: 0x00057CA9
		[return: TupleElementNames(new string[] { "pathData", "startOffset" })]
		public MBReadOnlyList<ValueTuple<SpawnPathData, float>> GetReinforcementPathsDataOfSide(BattleSideEnum battleSide)
		{
			return this._battleSpawnPathSelector.GetReinforcementPathsDataOfSide(battleSide);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00059AB8 File Offset: 0x00057CB8
		public void GetTroopSpawnFrameWithIndex(AgentBuildData buildData, int troopSpawnIndex, int troopSpawnCount, out Vec3 troopSpawnPosition, out Vec2 troopSpawnDirection)
		{
			Formation agentFormation = buildData.AgentFormation;
			BasicCharacterObject agentCharacter = buildData.AgentCharacter;
			troopSpawnPosition = Vec3.Invalid;
			WorldPosition worldPosition;
			Vec2 direction;
			if (buildData.AgentSpawnsIntoOwnFormation)
			{
				worldPosition = agentFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
				direction = agentFormation.Direction;
			}
			else
			{
				IAgentOriginBase agentOrigin = buildData.AgentOrigin;
				bool agentIsReinforcement = buildData.AgentIsReinforcement;
				Team agentTeam = buildData.AgentTeam;
				BattleSideEnum side = agentTeam.Side;
				if (buildData.AgentSpawnsUsingOwnTroopClass)
				{
					FormationClass agentTroopClass = this.GetAgentTroopClass(side, agentCharacter);
					this.GetFormationSpawnFrame(agentTeam, agentTroopClass, agentIsReinforcement, out worldPosition, out direction, true);
				}
				else if (agentCharacter.IsHero && agentOrigin != null && agentOrigin.BattleCombatant != null && agentCharacter == agentOrigin.BattleCombatant.General && this.GetFormationSpawnClass(agentTeam, FormationClass.NumberOfRegularFormations, agentIsReinforcement) == FormationClass.NumberOfRegularFormations)
				{
					this.GetFormationSpawnFrame(agentTeam, FormationClass.NumberOfRegularFormations, agentIsReinforcement, out worldPosition, out direction, true);
				}
				else
				{
					this.GetFormationSpawnFrame(agentTeam, agentFormation.FormationIndex, agentIsReinforcement, out worldPosition, out direction, true);
				}
			}
			bool flag = !buildData.AgentNoHorses && agentFormation.HasAnyMountedUnit;
			WorldPosition? worldPosition2;
			Vec2? vec;
			agentFormation.GetUnitSpawnFrameWithIndex(troopSpawnIndex, in worldPosition, in direction, agentFormation.Width, troopSpawnCount, agentFormation.UnitSpacing, flag, out worldPosition2, out vec);
			if (worldPosition2 != null && buildData.MakeUnitStandOutDistance != 0f)
			{
				worldPosition2.Value.SetVec2(worldPosition2.Value.AsVec2 + vec.Value * buildData.MakeUnitStandOutDistance);
			}
			if (worldPosition2 != null)
			{
				if (worldPosition2.Value.GetNavMesh() == UIntPtr.Zero)
				{
					troopSpawnPosition = this.Scene.GetLastPointOnNavigationMeshFromWorldPositionToDestination(ref worldPosition, worldPosition2.Value.AsVec2);
				}
				else
				{
					troopSpawnPosition = worldPosition2.Value.GetGroundVec3();
				}
			}
			if (!troopSpawnPosition.IsValid)
			{
				troopSpawnPosition = worldPosition.GetGroundVec3();
			}
			troopSpawnDirection = ((vec != null) ? vec.Value : direction);
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00059CAC File Offset: 0x00057EAC
		public void GetFormationSpawnFrame(Team team, FormationClass formationClass, bool isReinforcement, out WorldPosition spawnPosition, out Vec2 spawnDirection, bool useDefaultClassIfNotFound = true)
		{
			IFormationDeploymentPlan formationDeploymentPlan = this._deploymentPlan.GetFormationPlan(team, formationClass, isReinforcement);
			if (!formationDeploymentPlan.HasFrame() && useDefaultClassIfNotFound && !formationClass.IsDefaultFormationClass())
			{
				FormationClass formationClass2 = formationClass.DefaultClass();
				formationDeploymentPlan = this._deploymentPlan.GetFormationPlan(team, formationClass2, isReinforcement);
			}
			if (formationDeploymentPlan.HasFrame())
			{
				spawnPosition = formationDeploymentPlan.CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
				spawnDirection = formationDeploymentPlan.GetDirection();
				return;
			}
			Vec2 asVec;
			MatrixFrame formationsCenterFrameAndExtents = this._deploymentPlan.GetFormationsCenterFrameAndExtents(team, out asVec, true);
			spawnPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, formationsCenterFrameAndExtents.origin, false);
			asVec = formationsCenterFrameAndExtents.rotation.f.AsVec2;
			spawnDirection = asVec.Normalized();
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x00059D6C File Offset: 0x00057F6C
		public WorldFrame GetSpawnPathFrame(BattleSideEnum battleSide, float pathOffset = 0f, float targetOffset = 0f)
		{
			SpawnPathData initialSpawnPathData = this.GetInitialSpawnPathData(battleSide);
			if (initialSpawnPathData.IsValid)
			{
				Vec2 vec;
				Vec2 vec2;
				initialSpawnPathData.GetSpawnPathFrameFacingTarget(pathOffset, targetOffset, false, out vec, out vec2, false, 0.2f);
				Mat3 identity = Mat3.Identity;
				identity.RotateAboutUp(vec2.RotationInRadians);
				WorldPosition worldPosition = new WorldPosition(this.Scene, UIntPtr.Zero, vec.ToVec3(0f), false);
				return new WorldFrame(identity, worldPosition);
			}
			return WorldFrame.Invalid;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00059DE0 File Offset: 0x00057FE0
		private void BuildAgent(Agent agent, AgentBuildData agentBuildData)
		{
			if (agent == null)
			{
				throw new MBNullParameterException("agent");
			}
			agent.Build(agentBuildData);
			if (agent.Controller == AgentControllerType.Player)
			{
				this._initialPlayerAgent = agent;
			}
			if (!agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				EquipmentElement equipmentElement = agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
				if (equipmentElement.Item.HorseComponent.BodyLength != 0)
				{
					agent.SetInitialAgentScale(0.01f * (float)equipmentElement.Item.HorseComponent.BodyLength);
				}
			}
			agent.EquipItemsFromSpawnEquipment(true, (agentBuildData != null && agentBuildData.PrepareImmediately) || agent == Agent.Main, agentBuildData != null && agentBuildData.UseFaceCache, (agentBuildData != null) ? agentBuildData.FaceCacheId : 0);
			agent.InitializeAgentRecord();
			agent.AgentVisuals.BatchLastLodMeshes();
			agent.PreloadForRendering();
			ActionIndexCache currentAction = agent.GetCurrentAction(0);
			if (currentAction != ActionIndexCache.act_none)
			{
				agent.SetActionChannel(0, in currentAction, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, MBRandom.RandomFloat * 0.8f, false, -0.2f, 0, true);
			}
			agent.InitializeComponents();
			if (agent.Controller == AgentControllerType.Player)
			{
				this.ResetFirstThirdPersonView();
			}
			this._activeAgents.Add(agent);
			this._allAgents.Add(agent);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x00059F2C File Offset: 0x0005812C
		private Agent CreateAgent(Monster monster, bool isFemale, int instanceNo, Agent.CreationType creationType, float stepSize, int forcedAgentIndex, int weight, BasicCharacterObject characterObject)
		{
			AnimationSystemData animationSystemData = monster.FillAnimationSystemData(stepSize, false, isFemale);
			AgentCapsuleData agentCapsuleData = monster.FillCapsuleData();
			AgentSpawnData agentSpawnData = monster.FillSpawnData(null);
			Mission.AgentCreationResult agentCreationResult = this.CreateAgentInternal(monster.Flags, forcedAgentIndex, isFemale, ref agentSpawnData, ref agentCapsuleData, ref animationSystemData, instanceNo);
			Agent agent = new Agent(this, agentCreationResult, creationType, monster, this._agentCreationIndex);
			this._agentCreationIndex++;
			agent.Character = characterObject;
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentCreated(agent);
			}
			return agent;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x00059FDC File Offset: 0x000581DC
		public void SetBattleAgentCount(int agentCount)
		{
			if (this._agentCount == 0 || this._agentCount > agentCount)
			{
				this._agentCount = agentCount;
			}
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x00059FF8 File Offset: 0x000581F8
		public Vec2 GetFormationSpawnPosition(Team team, FormationClass formationClass)
		{
			return this._deploymentPlan.GetFormationPlan(team, formationClass, false).CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.None).AsVec2;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0005A021 File Offset: 0x00058221
		public FormationClass GetFormationSpawnClass(Team team, FormationClass formationClass, bool isReinforcement = false)
		{
			return this._deploymentPlan.GetFormationPlan(team, formationClass, isReinforcement).SpawnClass;
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0005A038 File Offset: 0x00058238
		public Equipment DecideAgentSpawnEquipment(AgentBuildData agentBuildData, out ItemObject formationBannerItem)
		{
			formationBannerItem = null;
			Equipment equipment;
			if (agentBuildData.AgentOverridenSpawnEquipment != null)
			{
				equipment = agentBuildData.AgentOverridenSpawnEquipment.Clone(false);
			}
			else if (!agentBuildData.AgentFixedEquipment)
			{
				equipment = Equipment.GetRandomEquipmentElements(agentBuildData.AgentCharacter, !Game.Current.GameType.IsCoreOnlyGameMode, agentBuildData.AgentCivilianEquipment ? Equipment.EquipmentType.Civilian : Equipment.EquipmentType.Battle, agentBuildData.AgentEquipmentSeed);
			}
			else if (agentBuildData.AgentCivilianEquipment)
			{
				equipment = agentBuildData.AgentCharacter.FirstCivilianEquipment.Clone(false);
			}
			else
			{
				equipment = agentBuildData.AgentCharacter.FirstBattleEquipment.Clone(false);
			}
			if (agentBuildData.AgentNoHorses)
			{
				equipment[EquipmentIndex.ArmorItemEndSlot] = default(EquipmentElement);
				equipment[EquipmentIndex.HorseHarness] = default(EquipmentElement);
			}
			if (agentBuildData.AgentNoWeapons)
			{
				equipment[EquipmentIndex.WeaponItemBeginSlot] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon1] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon2] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon3] = default(EquipmentElement);
				equipment[EquipmentIndex.ExtraWeaponSlot] = default(EquipmentElement);
			}
			if (agentBuildData.AgentCharacter.IsHero)
			{
				ItemObject item = equipment[EquipmentIndex.ExtraWeaponSlot].Item;
				if (item != null && item.IsBannerItem && item.BannerComponent != null)
				{
					formationBannerItem = item;
					equipment[EquipmentIndex.ExtraWeaponSlot] = default(EquipmentElement);
				}
				else if (agentBuildData.AgentBannerItem != null)
				{
					formationBannerItem = agentBuildData.AgentBannerItem;
				}
			}
			else if (agentBuildData.AgentBannerItem != null)
			{
				equipment[EquipmentIndex.Weapon1] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon2] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon3] = default(EquipmentElement);
				if (agentBuildData.AgentBannerReplacementWeaponItem != null)
				{
					equipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(agentBuildData.AgentBannerReplacementWeaponItem, null, null, false);
				}
				else
				{
					equipment[EquipmentIndex.WeaponItemBeginSlot] = default(EquipmentElement);
				}
				equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(agentBuildData.AgentBannerItem, null, null, false);
			}
			if (agentBuildData.AgentNoArmor)
			{
				equipment[EquipmentIndex.Gloves] = default(EquipmentElement);
				equipment[EquipmentIndex.Body] = default(EquipmentElement);
				equipment[EquipmentIndex.Cape] = default(EquipmentElement);
				equipment[EquipmentIndex.NumAllWeaponSlots] = default(EquipmentElement);
				equipment[EquipmentIndex.Leg] = default(EquipmentElement);
			}
			for (int i = 0; i < 5; i++)
			{
				if (!equipment[(EquipmentIndex)i].IsEmpty && equipment[(EquipmentIndex)i].Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp))
				{
					equipment[(EquipmentIndex)i] = default(EquipmentElement);
				}
			}
			return equipment;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0005A2CC File Offset: 0x000584CC
		public Agent SpawnAgent(AgentBuildData agentBuildData, bool spawnFromAgentVisuals = false, Equipment agentSpawnEquipment = null, ItemObject formationBannerItem = null)
		{
			this.Scene.WaitWaterRendererCPUSimulation();
			BasicCharacterObject agentCharacter = agentBuildData.AgentCharacter;
			if (agentCharacter == null)
			{
				throw new MBNullParameterException("npcCharacterObject");
			}
			int num = -1;
			if (agentBuildData.AgentIndexOverriden)
			{
				num = agentBuildData.AgentIndex;
			}
			Agent agent = this.CreateAgent(agentBuildData.AgentMonster, agentBuildData.GenderOverriden ? agentBuildData.AgentIsFemale : agentCharacter.IsFemale, 0, Agent.CreationType.FromCharacterObj, agentCharacter.GetStepSize(), num, agentBuildData.AgentMonster.Weight, agentCharacter);
			agent.FormationPositionPreference = agentCharacter.FormationPositionPreference;
			float num2 = (agentBuildData.AgeOverriden ? ((float)agentBuildData.AgentAge) : agentCharacter.Age);
			if (num2 == 0f)
			{
				agentBuildData.Age(29);
			}
			else if (MBBodyProperties.GetMaturityType(num2) < BodyMeshMaturityType.Teenager && (this.Mode == MissionMode.Battle || this.Mode == MissionMode.Duel || this.Mode == MissionMode.Tournament || this.Mode == MissionMode.Stealth))
			{
				agentBuildData.Age(27);
			}
			if (agentBuildData.BodyPropertiesOverriden)
			{
				agent.UpdateBodyProperties(agentBuildData.AgentBodyProperties);
				if (!agentBuildData.AgeOverriden)
				{
					agent.Age = agentCharacter.Age;
				}
			}
			agent.BodyPropertiesSeed = agentBuildData.AgentEquipmentSeed;
			if (agentBuildData.AgeOverriden)
			{
				agent.Age = (float)agentBuildData.AgentAge;
			}
			if (agentBuildData.GenderOverriden)
			{
				agent.IsFemale = agentBuildData.AgentIsFemale;
			}
			agent.SetTeam(agentBuildData.AgentTeam, false);
			agent.SetClothingColor1(agentBuildData.AgentClothingColor1);
			agent.SetClothingColor2(agentBuildData.AgentClothingColor2);
			agent.SetRandomizeColors(agentBuildData.RandomizeColors);
			agent.Origin = agentBuildData.AgentOrigin;
			Formation agentFormation = agentBuildData.AgentFormation;
			if (agentFormation != null && !agentFormation.HasBeenPositioned)
			{
				if (this._deploymentPlan.IsPlanMade(agentFormation.Team))
				{
					this.SetFormationPositioningFromDeploymentPlan(agentFormation, false);
				}
				else
				{
					WorldPosition worldPosition = new WorldPosition(this.Scene.Pointer, UIntPtr.Zero, agentBuildData.AgentInitialPosition.Value, false);
					agentFormation.SetPositioning(new WorldPosition?(worldPosition), null, null);
				}
			}
			if (agentBuildData.AgentInitialPosition == null)
			{
				Team agentTeam = agentBuildData.AgentTeam;
				BattleSideEnum side = agentBuildData.AgentTeam.Side;
				Vec3 vec = Vec3.Invalid;
				Vec2 vec2 = Vec2.Invalid;
				if (agentCharacter == Game.Current.PlayerTroop && this._deploymentPlan.HasPlayerSpawnFrame(side))
				{
					WorldPosition worldPosition2;
					Vec2 vec3;
					this._deploymentPlan.GetPlayerSpawnFrame(side, out worldPosition2, out vec3);
					vec = worldPosition2.GetGroundVec3();
					vec2 = vec3;
				}
				else if (agentFormation != null)
				{
					int num3;
					int num4;
					if (agentBuildData.AgentSpawnsIntoOwnFormation)
					{
						num3 = agentFormation.CountOfUnits;
						num4 = num3 + 1;
					}
					else if (agentBuildData.AgentFormationTroopSpawnIndex >= 0 && agentBuildData.AgentFormationTroopSpawnCount > 0)
					{
						num3 = agentBuildData.AgentFormationTroopSpawnIndex;
						num4 = agentBuildData.AgentFormationTroopSpawnCount;
					}
					else
					{
						num3 = agentFormation.GetNextSpawnIndex();
						num4 = num3 + 1;
					}
					if (num3 >= num4)
					{
						num4 = num3 + 1;
					}
					this.GetTroopSpawnFrameWithIndex(agentBuildData, num3, num4, out vec, out vec2);
				}
				else
				{
					WorldPosition worldPosition3;
					this.GetFormationSpawnFrame(agentTeam, FormationClass.NumberOfAllFormations, agentBuildData.AgentIsReinforcement, out worldPosition3, out vec2, true);
					vec = worldPosition3.GetGroundVec3();
				}
				agentBuildData.InitialPosition(in vec).InitialDirection(in vec2);
			}
			Agent agent2 = agent;
			Vec3 vec4 = agentBuildData.AgentInitialPosition.GetValueOrDefault();
			Vec2 vec5 = agentBuildData.AgentInitialDirection.GetValueOrDefault();
			agent2.SetInitialFrame(in vec4, in vec5, agentBuildData.AgentCanSpawnOutsideOfMissionBoundary);
			if (agentCharacter.BattleEquipments == null && agentCharacter.CivilianEquipments == null)
			{
				Debug.Print("characterObject.AllEquipments is null for \"" + agentCharacter.StringId + "\".", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			if (agentCharacter.BattleEquipments != null)
			{
				if (agentCharacter.BattleEquipments.Any<Equipment>((Equipment eq) => eq == null) && agentCharacter.CivilianEquipments != null)
				{
					if (agentCharacter.CivilianEquipments.Any<Equipment>((Equipment eq) => eq == null))
					{
						Debug.Print("Character with id \"" + agentCharacter.StringId + "\" has a null equipment in its AllEquipments.", 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
			}
			if (agentCharacter.CivilianEquipments == null)
			{
				agentBuildData.CivilianEquipment(false);
			}
			if (agentCharacter.IsHero)
			{
				agentBuildData.FixedEquipment(true);
			}
			if (agentSpawnEquipment == null)
			{
				agentSpawnEquipment = this.DecideAgentSpawnEquipment(agentBuildData, out formationBannerItem);
			}
			if (agentBuildData.AgentCharacter.IsHero)
			{
				if (formationBannerItem != null)
				{
					agent.SetFormationBanner(formationBannerItem);
				}
			}
			else if (agentBuildData.AgentBannerItem != null && agentBuildData.AgentOverridenSpawnMissionEquipment != null)
			{
				agentBuildData.AgentOverridenSpawnMissionEquipment[EquipmentIndex.ExtraWeaponSlot] = new MissionWeapon(agentBuildData.AgentBannerItem, null, agentBuildData.AgentBanner);
			}
			agent.InitializeSpawnEquipment(agentSpawnEquipment);
			agent.InitializeMissionEquipment(agentBuildData.AgentOverridenSpawnMissionEquipment, agentBuildData.AgentBanner);
			if (agent.RandomizeColors)
			{
				agent.Equipment.SetGlossMultipliersOfWeaponsRandomly(agentBuildData.AgentEquipmentSeed);
			}
			Agent agent3 = null;
			ItemObject item = agentSpawnEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
			if (item != null && item.HasHorseComponent && item.HorseComponent.IsRideable)
			{
				int num5 = -1;
				if (agentBuildData.AgentMountIndexOverriden)
				{
					num5 = agentBuildData.AgentMountIndex;
				}
				EquipmentElement equipmentElement = agentSpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
				EquipmentElement equipmentElement2 = agentSpawnEquipment[EquipmentIndex.HorseHarness];
				vec4 = agentBuildData.AgentInitialPosition.GetValueOrDefault();
				vec5 = agentBuildData.AgentInitialDirection.GetValueOrDefault();
				agent3 = this.CreateHorseAgentFromRosterElements(equipmentElement, equipmentElement2, in vec4, in vec5, num5, agentBuildData.AgentMountKey);
				Equipment equipment = new Equipment();
				equipment[EquipmentIndex.ArmorItemEndSlot] = agentSpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
				equipment[EquipmentIndex.HorseHarness] = agentSpawnEquipment[EquipmentIndex.HorseHarness];
				Equipment equipment2 = equipment;
				agent3.InitializeSpawnEquipment(equipment2);
				agent.SetMountAgentBeforeBuild(agent3);
			}
			if (spawnFromAgentVisuals || !GameNetwork.IsClientOrReplay)
			{
				agent.Equipment.CheckLoadedAmmos();
			}
			if (!agentBuildData.BodyPropertiesOverriden)
			{
				BodyProperties bodyProperties;
				if (this.OnComputeTroopBodyProperties != null)
				{
					bodyProperties = this.OnComputeTroopBodyProperties(agentBuildData, agentCharacter, agentSpawnEquipment, agentBuildData.AgentEquipmentSeed);
					agentBuildData.UseFaceCache = !agentCharacter.IsHero;
				}
				else
				{
					bodyProperties = agentCharacter.GetBodyProperties(agentSpawnEquipment, agentBuildData.AgentEquipmentSeed);
				}
				agent.UpdateBodyProperties(bodyProperties);
			}
			if (GameNetwork.IsServerOrRecorder && agent.RiderAgent == null)
			{
				Vec3 valueOrDefault = agentBuildData.AgentInitialPosition.GetValueOrDefault();
				Vec2 valueOrDefault2 = agentBuildData.AgentInitialDirection.GetValueOrDefault();
				if (agent.IsMount)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new CreateFreeMountAgent(agent, valueOrDefault, valueOrDefault2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				else
				{
					bool flag = agentBuildData.AgentMissionPeer != null;
					NetworkCommunicator networkCommunicator;
					if (!flag)
					{
						MissionPeer owningAgentMissionPeer = agentBuildData.OwningAgentMissionPeer;
						networkCommunicator = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.GetNetworkPeer() : null);
					}
					else
					{
						networkCommunicator = agentBuildData.AgentMissionPeer.GetNetworkPeer();
					}
					NetworkCommunicator networkCommunicator2 = networkCommunicator;
					bool flag2 = agent.MountAgent != null && agent.MountAgent.RiderAgent == agent;
					GameNetwork.BeginBroadcastModuleEvent();
					int index = agent.Index;
					BasicCharacterObject character = agent.Character;
					Monster monster = agent.Monster;
					Equipment spawnEquipment = agent.SpawnEquipment;
					MissionEquipment equipment3 = agent.Equipment;
					BodyProperties bodyPropertiesValue = agent.BodyPropertiesValue;
					int bodyPropertiesSeed = agent.BodyPropertiesSeed;
					bool isFemale = agent.IsFemale;
					Team team = agent.Team;
					int num6 = ((team != null) ? team.TeamIndex : (-1));
					Formation formation = agent.Formation;
					int num7 = ((formation != null) ? formation.Index : (-1));
					uint clothingColor = agent.ClothingColor1;
					uint clothingColor2 = agent.ClothingColor2;
					int num8 = (flag2 ? agent.MountAgent.Index : (-1));
					Agent mountAgent = agent.MountAgent;
					GameNetwork.WriteMessage(new CreateAgent(index, character, monster, spawnEquipment, equipment3, bodyPropertiesValue, bodyPropertiesSeed, isFemale, num6, num7, clothingColor, clothingColor2, num8, (mountAgent != null) ? mountAgent.SpawnEquipment : null, flag, valueOrDefault, valueOrDefault2, networkCommunicator2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			MultiplayerMissionAgentVisualSpawnComponent missionBehavior = this.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			if (missionBehavior != null && agentBuildData.AgentMissionPeer != null && agentBuildData.AgentMissionPeer.IsMine && agentBuildData.AgentVisualsIndex == 0)
			{
				missionBehavior.OnMyAgentSpawned();
			}
			if (agent3 != null)
			{
				agent3.SetClothingColor1(agentBuildData.AgentClothingColor1);
				agent3.SetClothingColor2(agentBuildData.AgentClothingColor2);
				this.BuildAgent(agent3, agentBuildData);
				foreach (MissionBehavior missionBehavior2 in this.MissionBehaviors)
				{
					missionBehavior2.OnAgentBuild(agent3, null);
				}
			}
			this.BuildAgent(agent, agentBuildData);
			if (agentBuildData.AgentMissionPeer != null)
			{
				agent.MissionPeer = agentBuildData.AgentMissionPeer;
			}
			if (agentBuildData.OwningAgentMissionPeer != null)
			{
				agent.SetOwningAgentMissionPeer(agentBuildData.OwningAgentMissionPeer);
			}
			foreach (MissionBehavior missionBehavior3 in this.MissionBehaviors)
			{
				Agent agent4 = agent;
				Banner banner;
				if ((banner = agentBuildData.AgentBanner) == null)
				{
					Team agentTeam2 = agentBuildData.AgentTeam;
					banner = ((agentTeam2 != null) ? agentTeam2.Banner : null);
				}
				missionBehavior3.OnAgentBuild(agent4, banner);
			}
			agent.AgentVisuals.CheckResources(true);
			if (agent.IsAIControlled)
			{
				if (agent3 == null)
				{
					AgentFlag agentFlag = agent.GetAgentFlags() & ~AgentFlag.CanRide;
					agent.SetAgentFlags(agentFlag);
				}
				else if (agent.Formation == null)
				{
					agent.SetRidingOrder(RidingOrder.RidingOrderEnum.Mount);
				}
			}
			Mission mission = Mission.Current;
			if (mission != null && mission.IsDeploymentFinished)
			{
				MissionGameModels.Current.AgentStatCalculateModel.InitializeAgentStatsAfterDeploymentFinished(agent);
				MissionGameModels.Current.AgentStatCalculateModel.InitializeMissionEquipmentAfterDeploymentFinished(agent);
			}
			return agent;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0005AB98 File Offset: 0x00058D98
		public void SetInitialAgentCountForSide(BattleSideEnum side, int agentCount)
		{
			if (side >= BattleSideEnum.Defender && side < BattleSideEnum.NumSides)
			{
				this._initialAgentCountPerSide[(int)side] = agentCount;
				return;
			}
			Debug.FailedAssert("Cannot set initial agent count.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "SetInitialAgentCountForSide", 4010);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0005ABD4 File Offset: 0x00058DD4
		public void SetFormationPositioningFromDeploymentPlan(Formation formation, bool isReinforcement = false)
		{
			IFormationDeploymentPlan formationPlan = this._deploymentPlan.GetFormationPlan(formation.Team, formation.FormationIndex, isReinforcement);
			if (formationPlan.HasDimensions)
			{
				formation.SetFormOrder(FormOrder.FormOrderCustom(formationPlan.PlannedWidth), true);
			}
			formation.SetPositioning(new WorldPosition?(formationPlan.CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.None)), new Vec2?(formationPlan.GetDirection()), null);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0005AC3A File Offset: 0x00058E3A
		public Agent SpawnMonster(ItemRosterElement rosterElement, ItemRosterElement harnessRosterElement, in Vec3 initialPosition, in Vec2 initialDirection, int forcedAgentIndex = -1)
		{
			return this.SpawnMonster(rosterElement.EquipmentElement, harnessRosterElement.EquipmentElement, in initialPosition, in initialDirection, forcedAgentIndex);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0005AC58 File Offset: 0x00058E58
		public Agent SpawnMonster(EquipmentElement equipmentElement, EquipmentElement harnessRosterElement, in Vec3 initialPosition, in Vec2 initialDirection, int forcedAgentIndex = -1)
		{
			Agent agent = this.CreateHorseAgentFromRosterElements(equipmentElement, harnessRosterElement, in initialPosition, in initialDirection, forcedAgentIndex, MountCreationKey.GetRandomMountKeyString(equipmentElement.Item, MBRandom.RandomInt()));
			Equipment equipment = new Equipment();
			equipment[EquipmentIndex.ArmorItemEndSlot] = equipmentElement;
			equipment[EquipmentIndex.HorseHarness] = harnessRosterElement;
			Equipment equipment2 = equipment;
			agent.InitializeSpawnEquipment(equipment2);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CreateFreeMountAgent(agent, initialPosition, initialDirection));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.BuildAgent(agent, null);
			return agent;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0005ACD8 File Offset: 0x00058ED8
		public AgentBuildData GetAgentBuildDataToSpawnTroop(IAgentOriginBase troopOrigin, bool isPlayerSide, bool hasFormation, bool spawnWithHorse, bool isReinforcement, int formationTroopCount, int formationTroopIndex, Vec3? initialPosition, Vec2? initialDirection, ItemObject bannerItem = null, FormationClass formationIndex = FormationClass.NumberOfAllFormations, bool useTroopClassForSpawn = false)
		{
			BasicCharacterObject troop = troopOrigin.Troop;
			Team agentTeam = Mission.GetAgentTeam(troopOrigin, isPlayerSide);
			AgentBuildData agentBuildData = new AgentBuildData(troop).Team(agentTeam).Banner(troopOrigin.Banner).ClothingColor1(agentTeam.Color)
				.ClothingColor2(agentTeam.Color2)
				.TroopOrigin(troopOrigin)
				.NoHorses(!spawnWithHorse)
				.CivilianEquipment(this.DoesMissionRequireCivilianEquipment)
				.SpawnsUsingOwnTroopClass(useTroopClassForSpawn);
			if (hasFormation)
			{
				Formation formation;
				if (formationIndex == FormationClass.NumberOfAllFormations)
				{
					formation = agentTeam.GetFormation(this.GetAgentTroopClass(agentTeam.Side, troop));
				}
				else
				{
					formation = agentTeam.GetFormation(formationIndex);
				}
				agentBuildData.Formation(formation);
				agentBuildData.FormationTroopSpawnCount(formationTroopCount).FormationTroopSpawnIndex(formationTroopIndex);
			}
			if (!troop.IsPlayerCharacter)
			{
				agentBuildData.IsReinforcement(isReinforcement);
			}
			if (bannerItem != null)
			{
				if (bannerItem.IsBannerItem && bannerItem.BannerComponent != null)
				{
					agentBuildData.BannerItem(bannerItem);
					ItemObject bannerBearerReplacementWeapon = MissionGameModels.Current.BattleBannerBearersModel.GetBannerBearerReplacementWeapon(troop);
					agentBuildData.BannerReplacementWeaponItem(bannerBearerReplacementWeapon);
				}
				else
				{
					Debug.FailedAssert("Passed banner item with name: " + bannerItem.Name + " is not a proper banner item", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetAgentBuildDataToSpawnTroop", 4113);
					Debug.Print("Invalid banner item: " + bannerItem.Name + " is passed to a troop to be spawned", 0, Debug.DebugColor.Yellow, 17592186044416UL);
				}
			}
			if (initialPosition != null)
			{
				AgentBuildData agentBuildData2 = agentBuildData;
				Vec3 value = initialPosition.Value;
				agentBuildData2.InitialPosition(in value);
				AgentBuildData agentBuildData3 = agentBuildData;
				Vec2 value2 = initialDirection.Value;
				agentBuildData3.InitialDirection(in value2);
			}
			if (spawnWithHorse)
			{
				agentBuildData.MountKey(MountCreationKey.GetRandomMountKeyString(troop.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, troop.GetMountKeySeed()));
			}
			if (isPlayerSide && troop == Game.Current.PlayerTroop)
			{
				agentBuildData.Controller(AgentControllerType.Player);
			}
			return agentBuildData;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0005AE98 File Offset: 0x00059098
		public Agent SpawnTroop(IAgentOriginBase troopOrigin, bool isPlayerSide, bool hasFormation, bool spawnWithHorse, bool isReinforcement, int formationTroopCount, int formationTroopIndex, bool isAlarmed, bool wieldInitialWeapons, Vec3? initialPosition, Vec2? initialDirection, string specialActionSetSuffix = null, ItemObject bannerItem = null, FormationClass formationIndex = FormationClass.NumberOfAllFormations, bool useTroopClassForSpawn = false)
		{
			AgentBuildData agentBuildDataToSpawnTroop = this.GetAgentBuildDataToSpawnTroop(troopOrigin, isPlayerSide, hasFormation, spawnWithHorse, isReinforcement, formationTroopCount, formationTroopIndex, initialPosition, initialDirection, bannerItem, formationIndex, useTroopClassForSpawn);
			return this.SpawnTroopWithAgentBuildData(agentBuildDataToSpawnTroop, isAlarmed, wieldInitialWeapons, specialActionSetSuffix);
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x0005AED0 File Offset: 0x000590D0
		public Agent SpawnTroopWithAgentBuildData(AgentBuildData agentBuildData, bool isAlarmed, bool wieldInitialWeapons, string specialActionSetSuffix = null)
		{
			Agent agent = this.SpawnAgent(agentBuildData, false, null, null);
			if (agent.Character.IsHero)
			{
				agent.SetAgentFlags(agent.GetAgentFlags() | AgentFlag.IsUnique);
			}
			if (agent.IsAIControlled && isAlarmed)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			if (wieldInitialWeapons)
			{
				agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			}
			if (!string.IsNullOrEmpty(specialActionSetSuffix))
			{
				AnimationSystemData animationSystemData = agentBuildData.AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSetWithSuffix(agentBuildData.AgentMonster, agentBuildData.AgentIsFemale, specialActionSetSuffix), agent.Character.GetStepSize(), false);
				agent.SetActionSet(ref animationSystemData);
			}
			return agent;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0005AF60 File Offset: 0x00059160
		public Agent SpawnTroopWithAgentBuildDataAndEquipment(AgentBuildData agentBuildData, Equipment agentSpawnEquipment, ItemObject formationBannerItem, bool isAlarmed, bool wieldInitialWeapons, string specialActionSetSuffix = null)
		{
			Agent agent = this.SpawnAgent(agentBuildData, false, agentSpawnEquipment, formationBannerItem);
			if (agent.Character.IsHero)
			{
				agent.SetAgentFlags(agent.GetAgentFlags() | AgentFlag.IsUnique);
			}
			if (agent.IsAIControlled && isAlarmed)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			if (wieldInitialWeapons)
			{
				agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			}
			if (!string.IsNullOrEmpty(specialActionSetSuffix))
			{
				AnimationSystemData animationSystemData = agentBuildData.AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSetWithSuffix(agentBuildData.AgentMonster, agentBuildData.AgentIsFemale, specialActionSetSuffix), agent.Character.GetStepSize(), false);
				agent.SetActionSet(ref animationSystemData);
			}
			return agent;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0005AFF4 File Offset: 0x000591F4
		public Agent ReplaceBotWithPlayer(Agent botAgent, MissionPeer missionPeer)
		{
			if (!GameNetwork.IsClientOrReplay && botAgent != null)
			{
				if (GameNetwork.IsServer)
				{
					NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
					if (!networkPeer.IsServerPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						NetworkCommunicator networkCommunicator = networkPeer;
						int index = botAgent.Index;
						float health = botAgent.Health;
						Agent mountAgent = botAgent.MountAgent;
						GameNetwork.WriteMessage(new ReplaceBotWithPlayer(networkCommunicator, index, health, (mountAgent != null) ? mountAgent.Health : (-1f)));
						GameNetwork.EndModuleEventAsServer();
					}
				}
				if (botAgent.Formation != null)
				{
					botAgent.Formation.PlayerOwner = botAgent;
				}
				botAgent.SetOwningAgentMissionPeer(null);
				botAgent.MissionPeer = missionPeer;
				botAgent.Formation = missionPeer.ControlledFormation;
				AgentFlag agentFlags = botAgent.GetAgentFlags();
				if (!agentFlags.HasAnyFlag(AgentFlag.CanRide))
				{
					botAgent.SetAgentFlags(agentFlags | AgentFlag.CanRide);
				}
				int botsUnderControlAlive = missionPeer.BotsUnderControlAlive;
				missionPeer.BotsUnderControlAlive = botsUnderControlAlive - 1;
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new BotsControlledChange(missionPeer.GetNetworkPeer(), missionPeer.BotsUnderControlAlive, missionPeer.BotsUnderControlTotal));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				if (botAgent.Formation != null)
				{
					missionPeer.Team.AssignPlayerAsSergeantOfFormation(missionPeer, missionPeer.ControlledFormation.FormationIndex);
				}
				return botAgent;
			}
			return null;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0005B108 File Offset: 0x00059308
		private Agent CreateHorseAgentFromRosterElements(EquipmentElement mount, EquipmentElement mountHarness, in Vec3 initialPosition, in Vec2 initialDirection, int forcedAgentMountIndex, string horseCreationKey)
		{
			HorseComponent horseComponent = mount.Item.HorseComponent;
			Agent agent = this.CreateAgent(horseComponent.Monster, false, 0, Agent.CreationType.FromHorseObj, 1f, forcedAgentMountIndex, (int)mount.Weight, null);
			agent.SetInitialFrame(in initialPosition, in initialDirection, false);
			agent.BaseHealthLimit = (float)mount.GetModifiedMountHitPoints();
			agent.HealthLimit = agent.BaseHealthLimit;
			agent.Health = agent.HealthLimit;
			agent.SetMountInitialValues(mount.GetModifiedItemName(), horseCreationKey);
			return agent;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0005B180 File Offset: 0x00059380
		public void OnAgentInteraction(Agent requesterAgent, Agent targetAgent, sbyte agentBoneIndex)
		{
			if (requesterAgent == Agent.Main && targetAgent.IsMount)
			{
				Agent.Main.Mount(targetAgent);
				return;
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentInteraction(requesterAgent, targetAgent, agentBoneIndex);
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0005B1F0 File Offset: 0x000593F0
		[UsedImplicitly]
		[MBCallback(null, false)]
		public void EndMission()
		{
			Debug.Print("I called EndMission", 0, Debug.DebugColor.White, 17179869184UL);
			this._missionEndTime = -1f;
			this.NextCheckTimeEndMission = -1f;
			this.MissionEnded = true;
			this.CurrentState = Mission.State.EndingNextFrame;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0005B22C File Offset: 0x0005942C
		private void EndMissionInternal()
		{
			MBDebug.Print("I called EndMissionInternal", 0, Debug.DebugColor.White, 17179869184UL);
			this._deploymentPlan.ClearAll();
			IMissionListener[] array = this._listeners.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnEndMission();
			}
			this.StopSoundEvents();
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnEndMissionInternal();
			}
			foreach (Agent agent in this.Agents)
			{
				agent.OnRemove();
			}
			foreach (Agent agent2 in this.AllAgents)
			{
				agent2.OnDelete();
				agent2.Clear();
			}
			this.Teams.Clear();
			this.FocusableObjectInformationProvider.OnFinalize();
			foreach (MissionObject missionObject in this.MissionObjects)
			{
				missionObject.OnEndMission();
			}
			this.CurrentState = Mission.State.Over;
			this.FreeResources();
			this.FinalizeMission();
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0005B3B4 File Offset: 0x000595B4
		private void StopSoundEvents()
		{
			if (this._ambientSoundEvent != null)
			{
				this._ambientSoundEvent.Stop();
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0005B3CC File Offset: 0x000595CC
		public void AddMissionBehavior(MissionBehavior missionBehavior)
		{
			this.MissionBehaviors.Add(missionBehavior);
			missionBehavior.Mission = this;
			MissionBehaviorType behaviorType = missionBehavior.BehaviorType;
			if (behaviorType != MissionBehaviorType.Logic)
			{
				if (behaviorType == MissionBehaviorType.Other)
				{
					this._otherMissionBehaviors.Add(missionBehavior);
				}
			}
			else
			{
				this.MissionLogics.Add(missionBehavior as MissionLogic);
			}
			missionBehavior.OnCreated();
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0005B424 File Offset: 0x00059624
		public T GetMissionBehavior<T>() where T : class, IMissionBehavior
		{
			for (int i = 0; i < this.MissionBehaviors.Count; i++)
			{
				T t;
				if ((t = this.MissionBehaviors[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0005B474 File Offset: 0x00059674
		public void RemoveMissionBehavior(MissionBehavior missionBehavior)
		{
			missionBehavior.OnRemoveBehavior();
			MissionBehaviorType behaviorType = missionBehavior.BehaviorType;
			if (behaviorType != MissionBehaviorType.Logic)
			{
				if (behaviorType != MissionBehaviorType.Other)
				{
					Debug.FailedAssert("Invalid behavior type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RemoveMissionBehavior", 4457);
				}
				else
				{
					this._otherMissionBehaviors.Remove(missionBehavior);
				}
			}
			else
			{
				this.MissionLogics.Remove(missionBehavior as MissionLogic);
			}
			this.MissionBehaviors.Remove(missionBehavior);
			missionBehavior.Mission = null;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0005B4E8 File Offset: 0x000596E8
		public void JoinEnemyTeam()
		{
			if (this.PlayerTeam == this.DefenderTeam)
			{
				Agent leader = this.AttackerTeam.Leader;
				if (leader != null)
				{
					if (this.MainAgent != null && this.MainAgent.IsActive())
					{
						this.MainAgent.Controller = AgentControllerType.AI;
					}
					leader.Controller = AgentControllerType.Player;
					this.PlayerTeam = this.AttackerTeam;
					return;
				}
			}
			else if (this.PlayerTeam == this.AttackerTeam)
			{
				Agent leader2 = this.DefenderTeam.Leader;
				if (leader2 != null)
				{
					if (this.MainAgent != null && this.MainAgent.IsActive())
					{
						this.MainAgent.Controller = AgentControllerType.AI;
					}
					leader2.Controller = AgentControllerType.Player;
					this.PlayerTeam = this.DefenderTeam;
					return;
				}
			}
			else
			{
				Debug.FailedAssert("Player is neither attacker nor defender.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "JoinEnemyTeam", 4501);
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0005B5B8 File Offset: 0x000597B8
		public void OnEndMissionResult()
		{
			MissionLogic[] array = this.MissionLogics.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnBattleEnded();
			}
			this.RetreatMission();
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0005B5F0 File Offset: 0x000597F0
		public bool IsAgentInteractionAllowed()
		{
			if (this.IsAgentInteractionAllowed_AdditionalCondition != null)
			{
				Delegate[] invocationList = this.IsAgentInteractionAllowed_AdditionalCondition.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					object obj;
					if ((obj = invocationList[i].DynamicInvoke(Array.Empty<object>())) is bool && !(bool)obj)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0005B644 File Offset: 0x00059844
		public bool IsOrderGesturesEnabled()
		{
			if (this.AreOrderGesturesEnabled_AdditionalCondition != null)
			{
				Delegate[] invocationList = this.AreOrderGesturesEnabled_AdditionalCondition.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					object obj;
					if ((obj = invocationList[i].DynamicInvoke(Array.Empty<object>())) is bool && !(bool)obj)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0005B698 File Offset: 0x00059898
		public List<EquipmentElement> GetExtraEquipmentElementsForCharacter(BasicCharacterObject character, bool getAllEquipments = false)
		{
			List<EquipmentElement> list = new List<EquipmentElement>();
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				List<EquipmentElement> extraEquipmentElementsForCharacter = missionLogic.GetExtraEquipmentElementsForCharacter(character, getAllEquipments);
				if (extraEquipmentElementsForCharacter != null)
				{
					list.AddRange(extraEquipmentElementsForCharacter);
				}
			}
			return list;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0005B6FC File Offset: 0x000598FC
		private bool CheckMissionEnded()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				MissionResult missionResult = null;
				if (missionLogic.MissionEnded(ref missionResult))
				{
					Debug.Print("CheckMissionEnded::ended", 0, Debug.DebugColor.White, 17592186044416UL);
					this.MissionResult = missionResult;
					this.MissionEnded = true;
					this.MissionResultReady(missionResult);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0005B784 File Offset: 0x00059984
		private void MissionResultReady(MissionResult missionResult)
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				missionLogic.OnMissionResultReady(missionResult);
			}
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0005B7D8 File Offset: 0x000599D8
		private void CheckMissionEnd(float currentTime)
		{
			if (!GameNetwork.IsClient && currentTime > this.NextCheckTimeEndMission)
			{
				if (this.CurrentState == Mission.State.Continuing)
				{
					if (this.MissionEnded)
					{
						return;
					}
					this.NextCheckTimeEndMission += 0.1f;
					this.CheckMissionEnded();
					if (!this.MissionEnded)
					{
						return;
					}
					this._missionEndTime = currentTime + this.MissionCloseTimeAfterFinish;
					this.NextCheckTimeEndMission += 5f;
					using (List<MissionLogic>.Enumerator enumerator = this.MissionLogics.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MissionLogic missionLogic = enumerator.Current;
							missionLogic.ShowBattleResults();
						}
						return;
					}
				}
				if (currentTime > this._missionEndTime)
				{
					this.EndMissionInternal();
					return;
				}
				this.NextCheckTimeEndMission += 5f;
				return;
			}
			else if (this.CurrentState != Mission.State.Continuing && currentTime > this.NextCheckTimeEndMission)
			{
				this.EndMissionInternal();
			}
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0005B8D4 File Offset: 0x00059AD4
		public bool IsPlayerCloseToAnEnemy(float distance = 5f)
		{
			if (this.MainAgent == null)
			{
				return false;
			}
			Vec3 position = this.MainAgent.Position;
			float num = distance * distance;
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(this, position.AsVec2, distance, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (lastFoundAgent != this.MainAgent && lastFoundAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack) && lastFoundAgent.Position.DistanceSquared(position) <= num && (!lastFoundAgent.IsAIControlled || lastFoundAgent.IsAlarmed()) && lastFoundAgent.IsEnemyOf(this.MainAgent) && !lastFoundAgent.IsRetreating())
				{
					return true;
				}
				AgentProximityMap.FindNext(this, ref proximityMapSearchStruct);
			}
			return false;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0005B97C File Offset: 0x00059B7C
		public Vec3 GetRandomPositionAroundPoint(Vec3 center, float minDistance, float maxDistance, bool nearFirst = false)
		{
			Vec3 vec = new Vec3(-1f, 0f, 0f, -1f);
			vec.RotateAboutZ(6.2831855f * MBRandom.RandomFloat);
			float num = maxDistance - minDistance;
			if (nearFirst)
			{
				for (int i = 4; i > 0; i--)
				{
					int num2 = 0;
					while ((float)num2 <= 10f)
					{
						vec.RotateAboutZ(1.2566371f);
						Vec3 vec2 = center + vec * (minDistance + num / (float)i);
						if (this.Scene.GetNavigationMeshForPosition(in vec2) != UIntPtr.Zero)
						{
							return vec2;
						}
						num2++;
					}
				}
			}
			else
			{
				for (int j = 1; j < 5; j++)
				{
					int num3 = 0;
					while ((float)num3 <= 10f)
					{
						vec.RotateAboutZ(1.2566371f);
						Vec3 vec3 = center + vec * (minDistance + num / (float)j);
						if (this.Scene.GetNavigationMeshForPosition(in vec3) != UIntPtr.Zero)
						{
							return vec3;
						}
						num3++;
					}
				}
			}
			return center;
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0005BA84 File Offset: 0x00059C84
		public WorldPosition FindBestDefendingPosition(WorldPosition enemyPosition, WorldPosition defendedPosition)
		{
			return this.GetBestSlopeAngleHeightPosForDefending(enemyPosition, defendedPosition, 10, 0.5f, 4f, 0.5f, 0.70710677f, 0.1f, 1f, 0.7f, 0.5f, 1.2f, 20f, 0.6f);
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0005BAD2 File Offset: 0x00059CD2
		public WorldPosition FindPositionWithBiggestSlopeTowardsDirectionInSquare(ref WorldPosition center, float halfSize, ref WorldPosition referencePosition)
		{
			return this.GetBestSlopeTowardsDirection(ref center, halfSize, ref referencePosition);
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0005BAE0 File Offset: 0x00059CE0
		public Mission.Missile AddCustomMissile(Agent shooterAgent, MissionWeapon missileWeapon, Vec3 position, Vec3 direction, Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, MissionObject missionObjectToIgnore, int forcedMissileIndex = -1)
		{
			WeaponData weaponData = missileWeapon.GetWeaponData(true);
			GameEntity gameEntity;
			int num;
			if (missileWeapon.WeaponsCount == 1)
			{
				WeaponStatsData weaponStatsDataForUsage = missileWeapon.GetWeaponStatsDataForUsage(0);
				num = this.AddMissileSingleUsageAux(forcedMissileIndex, false, shooterAgent, in weaponData, in weaponStatsDataForUsage, 0f, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, (missionObjectToIgnore != null) ? missionObjectToIgnore.GameEntity : WeakGameEntity.Invalid, false, out gameEntity);
			}
			else
			{
				WeaponStatsData[] weaponStatsData = missileWeapon.GetWeaponStatsData();
				num = this.AddMissileAux(forcedMissileIndex, false, shooterAgent, in weaponData, weaponStatsData, 0f, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, (missionObjectToIgnore != null) ? missionObjectToIgnore.GameEntity : WeakGameEntity.Invalid, false, out gameEntity);
			}
			weaponData.DeinitializeManagedPointers();
			Mission.Missile missile = new Mission.Missile(this, num, gameEntity, shooterAgent, missileWeapon, missionObjectToIgnore);
			this._missilesList.Add(missile);
			this._missilesDictionary.Add(num, missile);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CreateMissile(num, shooterAgent.Index, EquipmentIndex.None, missileWeapon, position, direction, speed, orientation, addRigidBody, missionObjectToIgnore.Id, false));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			return missile;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0005BBE8 File Offset: 0x00059DE8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, bool isPrimaryWeaponShot, int forcedMissileIndex)
		{
			bool flag = GameNetwork.IsClient && forcedMissileIndex == -1;
			float num = 0f;
			MissionWeapon missionWeapon;
			if (shooterAgent.Equipment[weaponIndex].CurrentUsageItem != null && shooterAgent.Equipment[weaponIndex].CurrentUsageItem.IsRangedWeapon && shooterAgent.Equipment[weaponIndex].CurrentUsageItem.IsConsumable)
			{
				missionWeapon = shooterAgent.Equipment[weaponIndex];
			}
			else
			{
				missionWeapon = shooterAgent.Equipment[weaponIndex].AmmoWeapon;
				if (shooterAgent.Equipment[weaponIndex].CurrentUsageItem != null)
				{
					num = (float)shooterAgent.Equipment[weaponIndex].GetModifiedThrustDamageForCurrentUsage();
				}
			}
			if (!missionWeapon.IsEmpty)
			{
				missionWeapon.Amount = 1;
				WeaponData weaponData = missionWeapon.GetWeaponData(true);
				Vec3 vec = velocity;
				float num2 = vec.Normalize();
				float num3 = (float)shooterAgent.Equipment[shooterAgent.GetPrimaryWieldedItemIndex()].GetModifiedMissileSpeedForCurrentUsage();
				GameEntity gameEntity;
				int num4;
				if (missionWeapon.WeaponsCount == 1)
				{
					WeaponStatsData weaponStatsDataForUsage = missionWeapon.GetWeaponStatsDataForUsage(0);
					num4 = this.AddMissileSingleUsageAux(forcedMissileIndex, flag, shooterAgent, in weaponData, in weaponStatsDataForUsage, num, ref position, ref vec, ref orientation, num3, num2, hasRigidBody, WeakGameEntity.Invalid, isPrimaryWeaponShot, out gameEntity);
				}
				else
				{
					WeaponStatsData[] weaponStatsData = missionWeapon.GetWeaponStatsData();
					num4 = this.AddMissileAux(forcedMissileIndex, flag, shooterAgent, in weaponData, weaponStatsData, num, ref position, ref vec, ref orientation, num3, num2, hasRigidBody, WeakGameEntity.Invalid, isPrimaryWeaponShot, out gameEntity);
				}
				weaponData.DeinitializeManagedPointers();
				if (!flag)
				{
					Mission.Missile missile = new Mission.Missile(this, num4, gameEntity, shooterAgent, missionWeapon, null);
					this._missilesList.Add(missile);
					this._missilesDictionary.Add(num4, missile);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new CreateMissile(num4, shooterAgent.Index, weaponIndex, MissionWeapon.Invalid, position, vec, num2, orientation, hasRigidBody, MissionObjectId.Invalid, isPrimaryWeaponShot));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
				}
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);
				}
				if (shooterAgent != null)
				{
					shooterAgent.UpdateLastRangedAttackTimeDueToAnAttack(Mission.Current.CurrentTime);
				}
			}
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0005BE2C File Offset: 0x0005A02C
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal AgentState GetAgentState(Agent affectorAgent, Agent agent, DamageTypes damageType, WeaponFlags weaponFlags)
		{
			float num;
			float agentStateProbability = MissionGameModels.Current.AgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(affectorAgent, agent, damageType, weaponFlags, out num);
			AgentState agentState = AgentState.None;
			bool flag = false;
			using (List<MissionBehavior>.Enumerator enumerator = this.MissionBehaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IAgentStateDecider agentStateDecider;
					if ((agentStateDecider = enumerator.Current as IAgentStateDecider) != null)
					{
						agentState = agentStateDecider.GetAgentState(agent, agentStateProbability, out flag);
						break;
					}
				}
			}
			if (agentState == AgentState.None)
			{
				float randomFloat = MBRandom.RandomFloat;
				if (randomFloat < agentStateProbability)
				{
					agentState = AgentState.Killed;
					flag = true;
				}
				else
				{
					agentState = AgentState.Unconscious;
					if (randomFloat > 1f - num)
					{
						flag = true;
					}
				}
			}
			if (flag && affectorAgent != null && affectorAgent.Team != null && agent.Team != null && affectorAgent.Team == agent.Team)
			{
				flag = false;
			}
			for (int i = 0; i < this.MissionBehaviors.Count; i++)
			{
				this.MissionBehaviors[i].OnGetAgentState(agent, flag);
			}
			return agentState;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0005BF24 File Offset: 0x0005A124
		public void OnAgentMount(Agent agent)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentMount(agent);
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0005BF78 File Offset: 0x0005A178
		public void OnAgentDismount(Agent agent)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentDismount(agent);
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0005BFCC File Offset: 0x0005A1CC
		public void OnObjectUsed(Agent userAgent, UsableMissionObject usableGameObject)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnObjectUsed(userAgent, usableGameObject);
			}
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0005C020 File Offset: 0x0005A220
		public void OnObjectStoppedBeingUsed(Agent userAgent, UsableMissionObject usableGameObject)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnObjectStoppedBeingUsed(userAgent, usableGameObject);
			}
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0005C074 File Offset: 0x0005A274
		public void InitializeStartingBehaviors(MissionLogic[] logicBehaviors, MissionBehavior[] otherBehaviors, MissionNetwork[] networkBehaviors)
		{
			foreach (MissionLogic missionLogic in logicBehaviors)
			{
				this.AddMissionBehavior(missionLogic);
			}
			foreach (MissionNetwork missionNetwork in networkBehaviors)
			{
				this.AddMissionBehavior(missionNetwork);
			}
			foreach (MissionBehavior missionBehavior in otherBehaviors)
			{
				this.AddMissionBehavior(missionBehavior);
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0005C0D9 File Offset: 0x0005A2D9
		public Agent GetClosestEnemyAgent(Team team, Vec3 position, float radius)
		{
			return this.GetClosestEnemyAgent(team.MBTeam, position, radius);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0005C0E9 File Offset: 0x0005A2E9
		public Agent GetClosestAllyAgent(Team team, Vec3 position, float radius)
		{
			return this.GetClosestAllyAgent(team.MBTeam, position, radius);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0005C0F9 File Offset: 0x0005A2F9
		public int GetNearbyEnemyAgentCount(Team team, Vec2 position, float radius)
		{
			return this.GetNearbyEnemyAgentCount(team.MBTeam, position, radius);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0005C10C File Offset: 0x0005A30C
		public bool HasAnyAgentsOfSideInRange(Vec3 origin, float radius, BattleSideEnum side)
		{
			Team team = ((side == BattleSideEnum.Attacker) ? this.AttackerTeam : this.DefenderTeam);
			return MBAPI.IMBMission.HasAnyAgentsOfTeamAround(this.Pointer, origin, radius, team.MBTeam.Index);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0005C149 File Offset: 0x0005A349
		public void AddSoundAlarmFactorToAgents(Agent alarmCreatorAgent, in Vec3 soundPosition, float soundLevelSquareRoot)
		{
			Mission.OnAddSoundAlarmFactorToAgentsDelegate onAddSoundAlarmFactorToAgents = this.OnAddSoundAlarmFactorToAgents;
			if (onAddSoundAlarmFactorToAgents == null)
			{
				return;
			}
			onAddSoundAlarmFactorToAgents(alarmCreatorAgent, in soundPosition, soundLevelSquareRoot);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0005C160 File Offset: 0x0005A360
		private void HandleSpawnedItems()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				int num = 0;
				for (int i = this._spawnedItemEntitiesCreatedAtRuntime.Count - 1; i >= 0; i--)
				{
					SpawnedItemEntity spawnedItemEntity = this._spawnedItemEntitiesCreatedAtRuntime[i];
					if (!spawnedItemEntity.IsRemoved)
					{
						if (!spawnedItemEntity.IsDeactivated && !spawnedItemEntity.HasUser && spawnedItemEntity.HasLifeTime && !spawnedItemEntity.HasAIMovingTo && (num > 500 || spawnedItemEntity.IsReadyToBeDeleted()))
						{
							spawnedItemEntity.GameEntity.Remove(80);
						}
						else
						{
							num++;
						}
					}
					if (spawnedItemEntity.IsRemoved)
					{
						this._spawnedItemEntitiesCreatedAtRuntime.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0005C200 File Offset: 0x0005A400
		public bool OnMissionObjectRemoved(MissionObject missionObject, int removeReason)
		{
			if (!GameNetwork.IsClientOrReplay && missionObject.CreatedAtRuntime)
			{
				this.ReturnRuntimeMissionObjectId(missionObject.Id.Id);
				if (GameNetwork.IsServerOrRecorder)
				{
					this.RemoveDynamicallySpawnedMissionObjectInfo(missionObject.Id);
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new RemoveMissionObject(missionObject.Id));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			this._activeMissionObjects.Remove(missionObject);
			return this._missionObjects.Remove(missionObject);
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x0005C278 File Offset: 0x0005A478
		public bool AgentLookingAtAgent(Agent agent1, Agent agent2)
		{
			Vec3 vec = agent2.Position - agent1.Position;
			float num = vec.Normalize();
			float num2 = Vec3.DotProduct(vec, agent1.LookDirection);
			return num2 < 1f && num2 > 0.86f && num < 4f;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0005C2CB File Offset: 0x0005A4CB
		public Agent FindAgentWithIndex(int agentId)
		{
			return this.FindAgentWithIndexAux(agentId);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0005C2D4 File Offset: 0x0005A4D4
		public static Team GetAgentTeam(IAgentOriginBase troopOrigin, bool isPlayerSide)
		{
			if (Mission.Current == null)
			{
				Debug.FailedAssert("Mission current is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetAgentTeam", 5164);
				return null;
			}
			Team team;
			if (isPlayerSide)
			{
				if (Mission.Current.PlayerAllyTeam == null || troopOrigin.IsUnderPlayersCommand || troopOrigin.IsInSameArmyAsPlayer)
				{
					team = Mission.Current.PlayerTeam;
				}
				else
				{
					team = Mission.Current.PlayerAllyTeam;
				}
			}
			else
			{
				team = Mission.Current.PlayerEnemyTeam;
			}
			return team;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0005C348 File Offset: 0x0005A548
		public static Team GetTeam(TeamSideEnum teamSide)
		{
			if (Mission.Current == null)
			{
				Debug.FailedAssert("Mission current is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetTeam", 5199);
				return null;
			}
			switch (teamSide)
			{
			case TeamSideEnum.PlayerTeam:
				return Mission.Current.PlayerTeam;
			case TeamSideEnum.PlayerAllyTeam:
				return Mission.Current.PlayerAllyTeam;
			case TeamSideEnum.EnemyTeam:
				return Mission.Current.PlayerEnemyTeam;
			default:
				return null;
			}
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0005C3B0 File Offset: 0x0005A5B0
		public static IEnumerable<Team> GetTeamsOfSide(BattleSideEnum side)
		{
			return Mission.Current.Teams.Where<Team>((Team t) => t.Side == side);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0005C3E8 File Offset: 0x0005A5E8
		public static float ComputeSpawnPathDeploymentOffset(int troopCount, Path path)
		{
			float totalLength = path.GetTotalLength();
			float num = 200f;
			if (troopCount > 20)
			{
				int num2 = MathF.Max(BannerlordConfig.MaxBattleSize - 20, 1);
				float num3 = (float)Math.Min(troopCount - 20, num2) / (float)num2;
				num += 400f * MathF.Pow(num3, 0.6f);
			}
			num = MathF.Min(totalLength, num);
			return -num / 2f + 1f;
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0005C450 File Offset: 0x0005A650
		public void OnRenderingStarted()
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnRenderingStarted();
			}
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0005C4A0 File Offset: 0x0005A6A0
		public unsafe Agent.MovementBehaviorType GetMovementTypeOfAgents(IEnumerable<Agent> agents)
		{
			float currentTime = Mission.Current.CurrentTime;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Agent agent in agents)
			{
				num++;
				if (agent.IsAIControlled)
				{
					if (!agent.IsRetreating())
					{
						if (agent.Formation == null)
						{
							goto IL_0065;
						}
						MovementOrder movementOrder = *agent.Formation.GetReadonlyMovementOrderReference();
						if (movementOrder.OrderType != OrderType.Retreat)
						{
							goto IL_0065;
						}
					}
					num2++;
				}
				IL_0065:
				if (currentTime - agent.LastMeleeHitTime < 3f)
				{
					num3++;
				}
			}
			if ((float)num2 * 1f / (float)num > 0.3f)
			{
				return Agent.MovementBehaviorType.Flee;
			}
			if (num3 > 0)
			{
				return Agent.MovementBehaviorType.Engaged;
			}
			return Agent.MovementBehaviorType.Idle;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0005C568 File Offset: 0x0005A768
		public void ShowInMissionLoadingScreen(int durationInSecond, Action onLoadingEndedAction)
		{
			this._inMissionLoadingScreenTimer = new Timer(this.CurrentTime, (float)durationInSecond, true);
			this._onLoadingEndedAction = onLoadingEndedAction;
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0005C58C File Offset: 0x0005A78C
		public bool CanAgentRout(Agent agent)
		{
			return (agent.IsRunningAway || (agent.CommonAIComponent != null && agent.CommonAIComponent.IsRetreating) || (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWander) && agent.IsWandering())) && agent.RiderAgent == null && (this.CanAgentRout_AdditionalCondition == null || this.CanAgentRout_AdditionalCondition(agent));
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0005C5F0 File Offset: 0x0005A7F0
		internal bool CanGiveDamageToAgentShield(Agent attacker, WeaponComponentData attackerWeapon, Agent defender)
		{
			return MissionGameModels.Current.AgentApplyDamageModel.CanWeaponIgnoreFriendlyFireChecks(attackerWeapon) || !this.CancelsDamageAndBlocksAttackBecauseOfNonEnemyCase(attacker, defender);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0005C614 File Offset: 0x0005A814
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void MeleeHitCallback(ref AttackCollisionData collisionData, Agent attacker, Agent victim, GameEntity realHitEntity, ref float inOutMomentumRemaining, ref MeleeCollisionReaction colReaction, CrushThroughState crushThroughState, Vec3 blowDir, Vec3 swingDir, ref HitParticleResultData hitParticleResultData, bool crushedThroughWithoutAgentCollision)
		{
			hitParticleResultData.Reset();
			bool flag = collisionData.CollisionResult == CombatCollisionResult.Parried || collisionData.CollisionResult == CombatCollisionResult.Blocked || collisionData.CollisionResult == CombatCollisionResult.ChamberBlocked;
			if (collisionData.IsAlternativeAttack && !flag && victim != null && victim.IsHuman && collisionData.CollisionBoneIndex != -1 && (collisionData.VictimHitBodyPart == BoneBodyPartType.ArmLeft || collisionData.VictimHitBodyPart == BoneBodyPartType.ArmRight) && victim.IsHuman)
			{
				colReaction = MeleeCollisionReaction.ContinueChecking;
			}
			int affectorWeaponSlotOrMissileIndex = collisionData.AffectorWeaponSlotOrMissileIndex;
			MissionWeapon missionWeapon = ((affectorWeaponSlotOrMissileIndex >= 0) ? attacker.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid);
			bool flag2 = this.CancelsDamageAndBlocksAttackBecauseOfNonEnemyCase(attacker, victim);
			if (flag2 && collisionData.StrikeType == 1 && !collisionData.IsAlternativeAttack && !missionWeapon.IsEmpty && attacker.IsActive() && victim.IsActive())
			{
				float z = attacker.GetCurWeaponOffset().z;
				float num = missionWeapon.CurrentUsageItem.GetRealWeaponLength() + z;
				if (num > 0.01f)
				{
					float armLength = attacker.GetArmLength();
					if (MBMath.ClampFloat(collisionData.CollisionDistanceOnWeapon, 0f, num) / num < 0.8f && collisionData.CollisionDistanceOnWeapon < armLength)
					{
						colReaction = MeleeCollisionReaction.ContinueChecking;
					}
				}
			}
			if (colReaction != MeleeCollisionReaction.ContinueChecking)
			{
				bool flag3 = victim != null && victim.CurrentMortalityState == Agent.MortalityState.Invulnerable;
				bool flag4 = victim == null && realHitEntity == null;
				bool flag5;
				if (flag2)
				{
					collisionData.AttackerStunPeriod = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunPeriodAttackerFriendlyFire);
					flag5 = true;
				}
				else
				{
					flag5 = flag3 || flag4 || (flag && !collisionData.AttackBlockedWithShield);
				}
				if (crushThroughState == CrushThroughState.CrushedThisFrame && !collisionData.IsAlternativeAttack)
				{
					Blow blow = default(Blow);
					MissionCombatMechanicsHelper.UpdateMomentumRemaining(ref inOutMomentumRemaining, in blow, in collisionData, attacker, victim, in missionWeapon, true);
				}
				WeaponComponentData weaponComponentData = null;
				CombatLogData combatLogData = default(CombatLogData);
				if (!flag5)
				{
					this.GetAttackCollisionResults(attacker, victim, (realHitEntity != null) ? realHitEntity.WeakEntity : WeakGameEntity.Invalid, inOutMomentumRemaining, in missionWeapon, crushThroughState > CrushThroughState.None, flag5, crushedThroughWithoutAgentCollision, ref collisionData, out weaponComponentData, out combatLogData);
					if (!collisionData.IsAlternativeAttack && attacker.IsDoingPassiveAttack && !GameNetwork.IsSessionActive && ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ReportDamage) > 0f)
					{
						if (attacker.HasMount)
						{
							if (attacker.IsMainAgent)
							{
								InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_delivered_couched_lance_damage", null).ToString(), Color.ConvertStringToColor("#AE4AD9FF")));
							}
							else if (victim != null && victim.IsMainAgent)
							{
								InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_received_couched_lance_damage", null).ToString(), Color.ConvertStringToColor("#D65252FF")));
							}
						}
						else if (attacker.IsMainAgent)
						{
							InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_delivered_braced_polearm_damage", null).ToString(), Color.ConvertStringToColor("#AE4AD9FF")));
						}
						else if (victim != null && victim.IsMainAgent)
						{
							InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_received_braced_polearm_damage", null).ToString(), Color.ConvertStringToColor("#D65252FF")));
						}
					}
					if (collisionData.CollidedWithShieldOnBack && weaponComponentData != null && victim != null && victim.IsMainAgent)
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_hit_shield_on_back", null).ToString(), Color.ConvertStringToColor("#FFFFFFFF")));
					}
				}
				else
				{
					collisionData.InflictedDamage = 0;
					collisionData.BaseMagnitude = 0f;
					collisionData.AbsorbedByArmor = 0;
					collisionData.SelfInflictedDamage = 0;
				}
				if (!crushedThroughWithoutAgentCollision)
				{
					Blow blow2 = this.CreateMeleeBlow(attacker, victim, in collisionData, in missionWeapon, crushThroughState, blowDir, swingDir, flag5);
					if (!flag && ((victim != null && victim.IsActive()) || realHitEntity != null))
					{
						this.RegisterBlow(attacker, victim, (realHitEntity != null) ? realHitEntity.WeakEntity : WeakGameEntity.Invalid, blow2, ref collisionData, in missionWeapon, ref combatLogData);
					}
					MissionCombatMechanicsHelper.UpdateMomentumRemaining(ref inOutMomentumRemaining, in blow2, in collisionData, attacker, victim, in missionWeapon, false);
					bool flag6 = victim != null && victim.Health <= 0f;
					bool flag7 = (blow2.BlowFlag & BlowFlags.ShrugOff) > BlowFlags.None;
					this.DecideAgentHitParticles(attacker, victim, in blow2, in collisionData, ref hitParticleResultData);
					MissionGameModels.Current.AgentApplyDamageModel.DecideWeaponCollisionReaction(in blow2, in collisionData, attacker, victim, in missionWeapon, flag6, flag7, inOutMomentumRemaining, out colReaction);
				}
				else
				{
					colReaction = MeleeCollisionReaction.ContinueChecking;
				}
				foreach (MissionBehavior missionBehavior in Mission.Current.MissionBehaviors)
				{
					missionBehavior.OnMeleeHit(attacker, victim, flag5, collisionData);
				}
			}
			if (collisionData.IsShieldBroken)
			{
				Vec3 vec = collisionData.CollisionGlobalPosition;
				this.AddSoundAlarmFactorToAgents(attacker, in vec, 15f);
				return;
			}
			if (!collisionData.IsMissile && (collisionData.CollisionResult == CombatCollisionResult.HitWorld || collisionData.CollisionResult == CombatCollisionResult.Blocked || collisionData.CollisionResult == CombatCollisionResult.Parried || collisionData.CollisionResult == CombatCollisionResult.ChamberBlocked))
			{
				Vec3 vec = collisionData.CollisionGlobalPosition;
				this.AddSoundAlarmFactorToAgents(attacker, in vec, 8f);
			}
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0005CAD0 File Offset: 0x0005ACD0
		private void DecideAgentHitParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, ref HitParticleResultData hprd)
		{
			if (victim != null && (blow.InflictedDamage > 0 || victim.Health <= 0f))
			{
				BlowWeaponRecord weaponRecord = blow.WeaponRecord;
				bool flag;
				if (weaponRecord.HasWeapon() && !blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.NoBlood))
				{
					AttackCollisionData attackCollisionData = collisionData;
					flag = attackCollisionData.IsAlternativeAttack;
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					MissionGameModels.Current.DamageParticleModel.GetMeleeAttackSweatParticles(attacker, victim, in blow, in collisionData, out hprd);
					return;
				}
				MissionGameModels.Current.DamageParticleModel.GetMeleeAttackBloodParticles(attacker, victim, in blow, in collisionData, out hprd);
			}
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0005CB60 File Offset: 0x0005AD60
		private void RegisterBlow(Agent attacker, Agent victim, WeakGameEntity realHitEntity, Blow b, ref AttackCollisionData collisionData, in MissionWeapon attackerWeapon, ref CombatLogData combatLogData)
		{
			b.VictimBodyPart = collisionData.VictimHitBodyPart;
			if (!collisionData.AttackBlockedWithShield)
			{
				if (collisionData.IsColliderAgent)
				{
					if (b.SelfInflictedDamage > 0 && attacker != null && attacker.IsActive() && attacker.IsFriendOf(victim))
					{
						Blow blow;
						AttackCollisionData attackCollisionData;
						attacker.CreateBlowFromBlowAsReflection(in b, in collisionData, out blow, out attackCollisionData);
						if (victim.IsMount && attacker.MountAgent != null)
						{
							attacker.MountAgent.RegisterBlow(blow, in attackCollisionData);
						}
						else
						{
							attacker.RegisterBlow(blow, in attackCollisionData);
						}
					}
					if (b.InflictedDamage > 0)
					{
						combatLogData.IsFatalDamage = victim != null && victim.Health - (float)b.InflictedDamage < 1f;
						combatLogData.InflictedDamage = b.InflictedDamage - combatLogData.ModifiedDamage;
						this.PrintAttackCollisionResults(attacker, victim, null, ref collisionData, ref combatLogData);
					}
					victim.RegisterBlow(b, in collisionData);
				}
				else if (collisionData.EntityExists)
				{
					MissionWeapon missionWeapon = (b.IsMissile ? this._missilesDictionary[b.WeaponRecord.AffectorWeaponSlotOrMissileIndex].Weapon : ((attacker != null && b.WeaponRecord.HasWeapon()) ? attacker.Equipment[b.WeaponRecord.AffectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid));
					this.OnEntityHit(realHitEntity, attacker, collisionData, b.InflictedDamage, (DamageTypes)collisionData.DamageType, b.GlobalPosition, b.SwingDirection, in missionWeapon, b.WeaponRecord.AffectorWeaponSlotOrMissileIndex, ref combatLogData);
					if (attacker != null && b.SelfInflictedDamage > 0 && attacker.IsActive())
					{
						Blow blow2;
						AttackCollisionData attackCollisionData2;
						attacker.CreateBlowFromBlowAsReflection(in b, in collisionData, out blow2, out attackCollisionData2);
						attacker.RegisterBlow(blow2, in attackCollisionData2);
					}
				}
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnRegisterBlow(attacker, victim, realHitEntity, b, ref collisionData, in attackerWeapon);
			}
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0005CD60 File Offset: 0x0005AF60
		private Blow CreateMissileBlow(Agent attackerAgent, in AttackCollisionData collisionData, in MissionWeapon attackerWeapon, Vec3 missilePosition, Vec3 missileStartingPosition)
		{
			Blow blow = new Blow((attackerAgent != null) ? attackerAgent.Index : (-1));
			MissionWeapon missionWeapon = attackerWeapon;
			blow.BlowFlag = (missionWeapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) ? BlowFlags.KnockDown : BlowFlags.None);
			AttackCollisionData attackCollisionData = collisionData;
			blow.Direction = attackCollisionData.MissileVelocity.NormalizedCopy();
			blow.SwingDirection = blow.Direction;
			attackCollisionData = collisionData;
			blow.GlobalPosition = attackCollisionData.CollisionGlobalPosition;
			attackCollisionData = collisionData;
			blow.BoneIndex = attackCollisionData.CollisionBoneIndex;
			attackCollisionData = collisionData;
			blow.StrikeType = (StrikeType)attackCollisionData.StrikeType;
			attackCollisionData = collisionData;
			blow.DamageType = (DamageTypes)attackCollisionData.DamageType;
			attackCollisionData = collisionData;
			blow.VictimBodyPart = attackCollisionData.VictimHitBodyPart;
			sbyte b;
			if (attackerAgent == null)
			{
				b = -1;
			}
			else
			{
				Monster monster = attackerAgent.Monster;
				missionWeapon = attackerWeapon;
				b = monster.GetBoneToAttachForItemFlags(missionWeapon.Item.ItemFlags);
			}
			sbyte b2 = b;
			missionWeapon = attackerWeapon;
			ItemObject item = missionWeapon.Item;
			missionWeapon = attackerWeapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			attackCollisionData = collisionData;
			int affectorWeaponSlotOrMissileIndex = attackCollisionData.AffectorWeaponSlotOrMissileIndex;
			sbyte b3 = b2;
			attackCollisionData = collisionData;
			blow.WeaponRecord.FillAsMissileBlow(item, currentUsageItem, affectorWeaponSlotOrMissileIndex, b3, missileStartingPosition, missilePosition, attackCollisionData.MissileVelocity);
			blow.BaseMagnitude = collisionData.BaseMagnitude;
			blow.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
			blow.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
			blow.InflictedDamage = collisionData.InflictedDamage;
			blow.SelfInflictedDamage = collisionData.SelfInflictedDamage;
			blow.DamageCalculated = true;
			return blow;
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0005CEFC File Offset: 0x0005B0FC
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal float OnAgentHitBlocked(Agent affectedAgent, Agent affectorAgent, ref AttackCollisionData collisionData, Vec3 blowDirection, Vec3 swingDirection, bool isMissile)
		{
			Blow blow;
			if (isMissile)
			{
				Mission.Missile missile = this._missilesDictionary[collisionData.AffectorWeaponSlotOrMissileIndex];
				MissionWeapon weapon = missile.Weapon;
				blow = this.CreateMissileBlow(affectorAgent, in collisionData, in weapon, missile.GetPosition(), collisionData.MissileStartingPosition);
			}
			else
			{
				int affectorWeaponSlotOrMissileIndex = collisionData.AffectorWeaponSlotOrMissileIndex;
				MissionWeapon missionWeapon = ((affectorWeaponSlotOrMissileIndex >= 0) ? affectorAgent.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid);
				blow = this.CreateMeleeBlow(affectorAgent, affectedAgent, in collisionData, in missionWeapon, CrushThroughState.None, blowDirection, swingDirection, true);
			}
			return this.OnAgentHit(affectedAgent, affectorAgent, in blow, in collisionData, true, 0f);
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0005CF84 File Offset: 0x0005B184
		private Blow CreateMeleeBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, in MissionWeapon attackerWeapon, CrushThroughState crushThroughState, Vec3 blowDirection, Vec3 swingDirection, bool cancelDamage)
		{
			Blow blow = new Blow(attackerAgent.Index);
			AttackCollisionData attackCollisionData = collisionData;
			blow.VictimBodyPart = attackCollisionData.VictimHitBodyPart;
			bool flag = MissionCombatMechanicsHelper.HitWithAnotherBone(in collisionData, attackerAgent, in attackerWeapon);
			attackCollisionData = collisionData;
			MissionWeapon missionWeapon;
			if (attackCollisionData.IsAlternativeAttack)
			{
				missionWeapon = attackerWeapon;
				blow.AttackType = (missionWeapon.IsEmpty ? AgentAttackType.Kick : AgentAttackType.Bash);
			}
			else
			{
				blow.AttackType = AgentAttackType.Standard;
			}
			missionWeapon = attackerWeapon;
			sbyte b;
			if (!missionWeapon.IsEmpty)
			{
				Monster monster = attackerAgent.Monster;
				missionWeapon = attackerWeapon;
				b = monster.GetBoneToAttachForItemFlags(missionWeapon.Item.ItemFlags);
			}
			else
			{
				b = -1;
			}
			sbyte b2 = b;
			missionWeapon = attackerWeapon;
			ItemObject item = missionWeapon.Item;
			missionWeapon = attackerWeapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			attackCollisionData = collisionData;
			blow.WeaponRecord.FillAsMeleeBlow(item, currentUsageItem, attackCollisionData.AffectorWeaponSlotOrMissileIndex, b2);
			attackCollisionData = collisionData;
			blow.StrikeType = (StrikeType)attackCollisionData.StrikeType;
			missionWeapon = attackerWeapon;
			DamageTypes damageTypes;
			if (!missionWeapon.IsEmpty && !flag)
			{
				attackCollisionData = collisionData;
				if (!attackCollisionData.IsAlternativeAttack)
				{
					attackCollisionData = collisionData;
					damageTypes = (DamageTypes)attackCollisionData.DamageType;
					goto IL_0121;
				}
			}
			damageTypes = DamageTypes.Blunt;
			IL_0121:
			blow.DamageType = damageTypes;
			attackCollisionData = collisionData;
			blow.NoIgnore = attackCollisionData.IsAlternativeAttack;
			attackCollisionData = collisionData;
			blow.AttackerStunPeriod = attackCollisionData.AttackerStunPeriod;
			attackCollisionData = collisionData;
			blow.DefenderStunPeriod = attackCollisionData.DefenderStunPeriod;
			blow.BlowFlag = BlowFlags.None;
			attackCollisionData = collisionData;
			blow.GlobalPosition = attackCollisionData.CollisionGlobalPosition;
			attackCollisionData = collisionData;
			blow.BoneIndex = attackCollisionData.CollisionBoneIndex;
			blow.Direction = blowDirection;
			attackCollisionData = collisionData;
			if (attackCollisionData.CollidedWithLastBoneSegment)
			{
				attackCollisionData = collisionData;
				blow.SwingDirection = attackCollisionData.LastBoneSegmentSwingDir;
			}
			else
			{
				blow.SwingDirection = swingDirection;
			}
			if (cancelDamage)
			{
				blow.BaseMagnitude = 0f;
				blow.MovementSpeedDamageModifier = 0f;
				blow.InflictedDamage = 0;
				blow.SelfInflictedDamage = 0;
				blow.AbsorbedByArmor = 0f;
			}
			else
			{
				blow.BaseMagnitude = collisionData.BaseMagnitude;
				blow.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
				blow.InflictedDamage = collisionData.InflictedDamage;
				blow.SelfInflictedDamage = collisionData.SelfInflictedDamage;
				blow.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
			}
			blow.DamageCalculated = true;
			if (crushThroughState != CrushThroughState.None)
			{
				blow.BlowFlag |= BlowFlags.CrushThrough;
			}
			if (blow.StrikeType == StrikeType.Thrust)
			{
				attackCollisionData = collisionData;
				if (!attackCollisionData.ThrustTipHit)
				{
					blow.BlowFlag |= BlowFlags.NonTipThrust;
				}
			}
			attackCollisionData = collisionData;
			if (attackCollisionData.IsColliderAgent)
			{
				if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentShrugOffBlow(victimAgent, in collisionData, in blow))
				{
					blow.BlowFlag |= BlowFlags.ShrugOff;
				}
				if (victimAgent.IsHuman)
				{
					Agent mountAgent = victimAgent.MountAgent;
					if (mountAgent != null)
					{
						if (mountAgent.RiderAgent == victimAgent)
						{
							AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
							missionWeapon = attackerWeapon;
							if (agentApplyDamageModel.DecideAgentDismountedByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
							{
								blow.BlowFlag |= BlowFlags.CanDismount;
							}
						}
					}
					else
					{
						AgentApplyDamageModel agentApplyDamageModel2 = MissionGameModels.Current.AgentApplyDamageModel;
						missionWeapon = attackerWeapon;
						if (agentApplyDamageModel2.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
						{
							blow.BlowFlag |= BlowFlags.KnockBack;
						}
						AgentApplyDamageModel agentApplyDamageModel3 = MissionGameModels.Current.AgentApplyDamageModel;
						missionWeapon = attackerWeapon;
						if (agentApplyDamageModel3.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
						{
							blow.BlowFlag |= BlowFlags.KnockDown;
						}
					}
				}
				else if (victimAgent.IsMount)
				{
					AgentApplyDamageModel agentApplyDamageModel4 = MissionGameModels.Current.AgentApplyDamageModel;
					missionWeapon = attackerWeapon;
					if (agentApplyDamageModel4.DecideMountRearedByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
					{
						blow.BlowFlag |= BlowFlags.MakesRear;
					}
				}
			}
			return blow;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0005D364 File Offset: 0x0005B564
		internal float OnAgentHit(Agent affectedAgent, Agent affectorAgent, in Blow b, in AttackCollisionData collisionData, bool isBlocked, float damagedHp)
		{
			float num = -1f;
			bool flag = false;
			int affectorWeaponSlotOrMissileIndex = b.WeaponRecord.AffectorWeaponSlotOrMissileIndex;
			Blow blow = b;
			bool isMissile = blow.IsMissile;
			int inflictedDamage = b.InflictedDamage;
			blow = b;
			float num2 = (blow.IsMissile ? (b.GlobalPosition - b.WeaponRecord.StartingPosition).Length : 0f);
			MissionWeapon missionWeapon;
			if (isMissile)
			{
				Mission.Missile missile = this._missilesDictionary[affectorWeaponSlotOrMissileIndex];
				missionWeapon = missile.Weapon;
				flag = missile.MissionObjectToIgnore != null;
			}
			else
			{
				missionWeapon = ((affectorAgent != null && affectorWeaponSlotOrMissileIndex >= 0) ? affectorAgent.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid);
			}
			if (affectorAgent != null && isMissile)
			{
				num = this.GetShootDifficulty(affectedAgent, affectorAgent, b.VictimBodyPart == BoneBodyPartType.Head);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentHit(affectedAgent, affectorAgent, in missionWeapon, in b, in collisionData);
				missionBehavior.OnScoreHit(affectedAgent, affectorAgent, missionWeapon.CurrentUsageItem, isBlocked, flag, in b, in collisionData, damagedHp, num2, num);
			}
			foreach (AgentComponent agentComponent in affectedAgent.Components)
			{
				agentComponent.OnHit(affectorAgent, inflictedDamage, in missionWeapon, in b, in collisionData);
			}
			affectedAgent.CheckToDropFlaggedItem();
			return (float)inflictedDamage;
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0005D4E8 File Offset: 0x0005B6E8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void MissileAreaDamageCallback(ref AttackCollisionData collisionDataInput, ref Blow blowInput, Agent alreadyDamagedAgent, Agent shooterAgent, bool isBigExplosion)
		{
			float num = (isBigExplosion ? 2.8f : 1.2f);
			float num2 = (isBigExplosion ? 1.6f : 1f);
			float num3 = 1f;
			if (collisionDataInput.MissileVelocity.LengthSquared < 484f)
			{
				num2 *= 0.8f;
				num3 = 0.5f;
			}
			AttackCollisionData attackCollisionData = collisionDataInput;
			blowInput.VictimBodyPart = collisionDataInput.VictimHitBodyPart;
			List<Agent> list = new List<Agent>();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(this, blowInput.GlobalPosition.AsVec2, num, true);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (lastFoundAgent.CurrentMortalityState != Agent.MortalityState.Invulnerable && lastFoundAgent != shooterAgent && lastFoundAgent != alreadyDamagedAgent)
				{
					list.Add(lastFoundAgent);
				}
				AgentProximityMap.FindNext(this, ref proximityMapSearchStruct);
			}
			foreach (Agent agent in list)
			{
				Blow blow = blowInput;
				blow.DamageCalculated = false;
				attackCollisionData = collisionDataInput;
				float num4 = float.MaxValue;
				sbyte b = -1;
				Skeleton skeleton = agent.AgentVisuals.GetSkeleton();
				sbyte boneCount = skeleton.GetBoneCount();
				MatrixFrame globalFrame = agent.AgentVisuals.GetGlobalFrame();
				for (sbyte b2 = 0; b2 < boneCount; b2 += 1)
				{
					float num5 = globalFrame.TransformToParent(in skeleton.GetBoneEntitialFrame(b2).origin).DistanceSquared(blowInput.GlobalPosition);
					if (num5 < num4)
					{
						b = b2;
						num4 = num5;
					}
				}
				if (num4 <= num * num)
				{
					float num6 = MathF.Sqrt(num4);
					float num7 = 1f;
					if (num6 > num2)
					{
						float num8 = MBMath.Lerp(1f, 3f, (num6 - num2) / (num - num2), 1E-05f);
						num7 = 1f / (num8 * num8);
					}
					num7 *= num3;
					attackCollisionData.SetCollisionBoneIndexForAreaDamage(b);
					MissionWeapon weapon = this._missilesDictionary[attackCollisionData.AffectorWeaponSlotOrMissileIndex].Weapon;
					WeaponComponentData weaponComponentData;
					CombatLogData combatLogData;
					this.GetAttackCollisionResults(shooterAgent, agent, WeakGameEntity.Invalid, 1f, in weapon, false, false, false, ref attackCollisionData, out weaponComponentData, out combatLogData);
					blow.BaseMagnitude = attackCollisionData.BaseMagnitude;
					blow.MovementSpeedDamageModifier = attackCollisionData.MovementSpeedDamageModifier;
					blow.InflictedDamage = attackCollisionData.InflictedDamage;
					blow.SelfInflictedDamage = attackCollisionData.SelfInflictedDamage;
					blow.AbsorbedByArmor = (float)attackCollisionData.AbsorbedByArmor;
					blow.DamageCalculated = true;
					blow.InflictedDamage = MathF.Round((float)blow.InflictedDamage * num7);
					blow.SelfInflictedDamage = MathF.Round((float)blow.SelfInflictedDamage * num7);
					combatLogData.ModifiedDamage = MathF.Round((float)combatLogData.ModifiedDamage * num7);
					this.RegisterBlow(shooterAgent, agent, WeakGameEntity.Invalid, blow, ref attackCollisionData, in weapon, ref combatLogData);
				}
			}
			Mission.OnCameraShakeTriggeredDelegate onCameraShakeTriggered = this.OnCameraShakeTriggered;
			if (onCameraShakeTriggered == null)
			{
				return;
			}
			onCameraShakeTriggered(in blowInput.GlobalPosition, 10f + num * 5f);
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0005D7F4 File Offset: 0x0005B9F4
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnMissileRemoved(int missileIndex)
		{
			this._missilesDictionary.Remove(missileIndex);
			for (int i = 0; i < this._missilesList.Count; i++)
			{
				if (this._missilesList[i].Index == missileIndex)
				{
					this._missilesList.RemoveAt(i);
					break;
				}
			}
			Action<int> onMissileRemovedEvent = this.OnMissileRemovedEvent;
			if (onMissileRemovedEvent == null)
			{
				return;
			}
			onMissileRemovedEvent(missileIndex);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0005D858 File Offset: 0x0005BA58
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal bool MissileHitCallback(out int extraHitParticleIndex, ref AttackCollisionData collisionData, Vec3 missileStartingPosition, Vec3 missilePosition, Vec3 missileAngularVelocity, Vec3 movementVelocity, MatrixFrame attachGlobalFrame, MatrixFrame affectedShieldGlobalFrame, int numDamagedAgents, Agent attacker, Agent victim, GameEntity hitEntity)
		{
			WeakGameEntity weakGameEntity = ((hitEntity != null) ? hitEntity.WeakEntity : WeakGameEntity.Invalid);
			Mission.Missile missile = this._missilesDictionary[collisionData.AffectorWeaponSlotOrMissileIndex];
			MissionWeapon weapon = missile.Weapon;
			WeaponFlags weaponFlags = weapon.CurrentUsageItem.WeaponFlags;
			float num = 1f;
			WeaponComponentData weaponComponentData = null;
			AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
			MissionWeapon missionWeapon = missile.Weapon;
			agentApplyDamageModel.DecideMissileWeaponFlags(attacker, in missionWeapon, ref weaponFlags);
			extraHitParticleIndex = -1;
			Mission.MissileCollisionReaction missileCollisionReaction = Mission.MissileCollisionReaction.Invalid;
			bool flag = !GameNetwork.IsSessionActive;
			bool missileHasPhysics = collisionData.MissileHasPhysics;
			PhysicsMaterial fromIndex = PhysicsMaterial.GetFromIndex(collisionData.PhysicsMaterialIndex);
			object obj = (fromIndex.IsValid ? fromIndex.GetFlags() : PhysicsMaterialFlags.None);
			bool flag2 = (weaponFlags & WeaponFlags.AmmoSticksWhenShot) > (WeaponFlags)0UL;
			object obj2 = obj;
			bool flag3 = (obj2 & 1) == 0;
			bool flag4 = (obj2 & 8) != 0;
			MissionObject missionObject = null;
			if (victim == null && weakGameEntity.IsValid)
			{
				WeakGameEntity weakGameEntity2 = weakGameEntity;
				do
				{
					missionObject = weakGameEntity2.GetFirstScriptOfType<MissionObject>();
					weakGameEntity2 = weakGameEntity2.Parent;
				}
				while (missionObject == null && weakGameEntity2.IsValid);
				weakGameEntity = ((missionObject != null) ? missionObject.GameEntity : WeakGameEntity.Invalid);
			}
			Mission.MissileCollisionReaction missileCollisionReaction2;
			if (flag4)
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.PassThrough;
			}
			else if (weaponFlags.HasAnyFlag(WeaponFlags.Burning))
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.BecomeInvisible;
			}
			else if (!flag3 || !flag2)
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.BounceBack;
			}
			else
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.Stick;
			}
			bool flag5 = false;
			bool flag6 = victim != null && victim.CurrentMortalityState == Agent.MortalityState.Invulnerable;
			if (collisionData.MissileGoneUnderWater || collisionData.MissileGoneOutOfBorder || flag6)
			{
				missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
			}
			else if (victim == null)
			{
				if (weakGameEntity.IsValid)
				{
					CombatLogData combatLogData;
					this.GetAttackCollisionResults(attacker, victim, weakGameEntity, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
					Blow blow = this.CreateMissileBlow(attacker, in collisionData, in weapon, missilePosition, missileStartingPosition);
					this.RegisterBlow(attacker, null, weakGameEntity, blow, ref collisionData, in weapon, ref combatLogData);
				}
				missileCollisionReaction = missileCollisionReaction2;
			}
			else if (collisionData.AttackBlockedWithShield)
			{
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, weakGameEntity, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				if (!collisionData.IsShieldBroken)
				{
					this.MakeSound(ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeStone, collisionData.CollisionGlobalPosition, false, false, -1, -1);
				}
				bool flag7 = false;
				if (weaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield))
				{
					if (!collisionData.IsShieldBroken)
					{
						EquipmentIndex offhandWieldedItemIndex = victim.GetOffhandWieldedItemIndex();
						float num2 = (float)collisionData.InflictedDamage;
						float managedParameter = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ShieldPenetrationOffset);
						float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ShieldPenetrationFactor);
						missionWeapon = victim.Equipment[offhandWieldedItemIndex];
						if (num2 > managedParameter + managedParameter2 * (float)missionWeapon.GetGetModifiedArmorForCurrentUsage())
						{
							flag7 = true;
						}
					}
					else
					{
						flag7 = true;
					}
				}
				else if (victim.State == AgentState.Active && collisionData.IsShieldBroken && MissionGameModels.Current.AgentApplyDamageModel.ShouldMissilePassThroughAfterShieldBreak(attacker, weapon.CurrentUsageItem))
				{
					flag7 = true;
				}
				if (flag7)
				{
					victim.MakeVoice(SkinVoiceManager.VoiceType.Pain, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					num *= 0.4f + MBRandom.RandomFloat * 0.2f;
					missileCollisionReaction = Mission.MissileCollisionReaction.PassThrough;
				}
				else
				{
					missileCollisionReaction = (collisionData.IsShieldBroken ? Mission.MissileCollisionReaction.BecomeInvisible : missileCollisionReaction2);
				}
			}
			else if (collisionData.MissileBlockedWithWeapon)
			{
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, weakGameEntity, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				missileCollisionReaction = Mission.MissileCollisionReaction.BounceBack;
			}
			else
			{
				if (attacker != null && attacker.IsFriendOf(victim))
				{
					if (this.ForceNoFriendlyFire)
					{
						flag5 = true;
					}
					else if (!missileHasPhysics)
					{
						if (flag)
						{
							if (attacker.Controller == AgentControllerType.AI)
							{
								flag5 = true;
							}
						}
						else if ((MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 && MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0) || this.Mode == MissionMode.Duel)
						{
							flag5 = true;
						}
					}
				}
				else if (victim.IsHuman && attacker != null && !attacker.IsEnemyOf(victim))
				{
					flag5 = true;
				}
				else if (flag && attacker != null && attacker.Controller == AgentControllerType.AI && victim.RiderAgent != null && attacker.IsFriendOf(victim.RiderAgent))
				{
					flag5 = true;
				}
				if (flag5)
				{
					if (flag && attacker != null && attacker == Agent.Main && attacker.IsFriendOf(victim))
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_you_hit_a_friendly_troop", null).ToString(), Color.ConvertStringToColor("#D65252FF")));
					}
					missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
				}
				else
				{
					bool flag8 = (weaponFlags & WeaponFlags.MultiplePenetration) > (WeaponFlags)0UL;
					CombatLogData combatLogData;
					this.GetAttackCollisionResults(attacker, victim, WeakGameEntity.Invalid, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
					Blow blow2 = this.CreateMissileBlow(attacker, in collisionData, in weapon, missilePosition, missileStartingPosition);
					if (collisionData.IsColliderAgent && flag8 && numDamagedAgents > 0)
					{
						blow2.InflictedDamage /= numDamagedAgents;
						blow2.SelfInflictedDamage /= numDamagedAgents;
						combatLogData.InflictedDamage = blow2.InflictedDamage - combatLogData.ModifiedDamage;
					}
					if (collisionData.IsColliderAgent)
					{
						if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentShrugOffBlow(victim, in collisionData, in blow2))
						{
							blow2.BlowFlag |= BlowFlags.ShrugOff;
						}
						else if (victim.IsHuman)
						{
							Agent mountAgent = victim.MountAgent;
							if (mountAgent != null)
							{
								if (mountAgent.RiderAgent == victim && MissionGameModels.Current.AgentApplyDamageModel.DecideAgentDismountedByBlow(attacker, victim, in collisionData, weapon.CurrentUsageItem, in blow2))
								{
									blow2.BlowFlag |= BlowFlags.CanDismount;
								}
							}
							else
							{
								if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedBackByBlow(attacker, victim, in collisionData, weapon.CurrentUsageItem, in blow2))
								{
									blow2.BlowFlag |= BlowFlags.KnockBack;
								}
								if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedDownByBlow(attacker, victim, in collisionData, weapon.CurrentUsageItem, in blow2))
								{
									blow2.BlowFlag |= BlowFlags.KnockDown;
								}
							}
						}
					}
					if (victim.State == AgentState.Active)
					{
						this.RegisterBlow(attacker, victim, WeakGameEntity.Invalid, blow2, ref collisionData, in weapon, ref combatLogData);
					}
					extraHitParticleIndex = MissionGameModels.Current.DamageParticleModel.GetMissileAttackParticle(attacker, victim, in blow2, in collisionData);
					if (flag8 && numDamagedAgents < 3)
					{
						missileCollisionReaction = Mission.MissileCollisionReaction.PassThrough;
					}
					else
					{
						missileCollisionReaction = missileCollisionReaction2;
						if (missileCollisionReaction2 == Mission.MissileCollisionReaction.Stick && !collisionData.CollidedWithShieldOnBack)
						{
							bool flag9 = this.CombatType == Mission.MissionCombatType.Combat;
							if (flag9)
							{
								bool flag10 = victim.IsHuman && collisionData.VictimHitBodyPart == BoneBodyPartType.Head;
								flag9 = victim.State != AgentState.Active || !flag10;
							}
							if (flag9)
							{
								float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.MissileMinimumDamageToStick);
								float num3 = 2f * managedParameter3;
								if ((float)blow2.InflictedDamage < managedParameter3 && blow2.AbsorbedByArmor > num3 && !GameNetwork.IsClientOrReplay)
								{
									missileCollisionReaction = Mission.MissileCollisionReaction.BounceBack;
								}
							}
							else
							{
								missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
							}
						}
					}
				}
			}
			if (collisionData.CollidedWithShieldOnBack && weaponComponentData != null && victim != null && victim.IsMainAgent)
			{
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_hit_shield_on_back", null).ToString(), Color.ConvertStringToColor("#FFFFFFFF")));
			}
			bool flag11;
			MatrixFrame matrixFrame;
			if (!collisionData.MissileHasPhysics && missileCollisionReaction == Mission.MissileCollisionReaction.Stick)
			{
				AttackCollisionData attackCollisionData = collisionData;
				missionWeapon = missile.Weapon;
				matrixFrame = this.CalculateAttachedLocalFrame(in attachGlobalFrame, attackCollisionData, missionWeapon.CurrentUsageItem, victim, weakGameEntity, movementVelocity, missileAngularVelocity, affectedShieldGlobalFrame, true, out flag11);
			}
			else
			{
				missionWeapon = missile.Weapon;
				MatrixFrame matrixFrame2 = missionWeapon.CurrentUsageItem.GetMissileStartingFrame();
				missionWeapon = missile.Weapon;
				MatrixFrame matrixFrame3 = missionWeapon.CurrentUsageItem.StickingFrame;
				matrixFrame2 = matrixFrame2.TransformToParent(in matrixFrame3);
				matrixFrame = attachGlobalFrame.TransformToParent(in matrixFrame2);
				missionWeapon = missile.Weapon;
				matrixFrame3 = missionWeapon.CurrentUsageItem.GetMissileStartingFrame();
				matrixFrame = matrixFrame.TransformToParent(in matrixFrame3);
				matrixFrame.origin.z = Math.Max(matrixFrame.origin.z, -100f);
				missionObject = null;
				flag11 = false;
			}
			Vec3 zero = Vec3.Zero;
			Vec3 zero2 = Vec3.Zero;
			if (missileCollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				WeaponFlags weaponFlags2 = weaponFlags & WeaponFlags.AmmoBreakOnBounceBackMask;
				if (weaponFlags2 == WeaponFlags.AmmoCanBreakOnBounceBack)
				{
					Vec3 vec = collisionData.MissileVelocity;
					if (vec.Length > ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BreakableProjectileMinimumBreakSpeed))
					{
						goto IL_0774;
					}
				}
				if (weaponFlags2 != WeaponFlags.AmmoBreaksOnBounceBack)
				{
					missile.CalculateBounceBackVelocity(missileAngularVelocity, collisionData, out zero, out zero2);
					goto IL_07A6;
				}
				IL_0774:
				missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
				if (weapon.Item.ItemType != ItemObject.ItemTypeEnum.SlingStones)
				{
					extraHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_broken_arrow");
				}
			}
			IL_07A6:
			if (missile.ShooterAgent != null && (missileCollisionReaction == Mission.MissileCollisionReaction.Stick || missileCollisionReaction == Mission.MissileCollisionReaction.BounceBack) && (victim == null || collisionData.AttackBlockedWithShield || collisionData.MissileBlockedWithWeapon))
			{
				missionWeapon = missile.Weapon;
				bool flag12;
				if (missionWeapon.CurrentUsageItem.WeaponClass != WeaponClass.Stone)
				{
					missionWeapon = missile.Weapon;
					if (missionWeapon.CurrentUsageItem.WeaponClass != WeaponClass.Boulder)
					{
						missionWeapon = missile.Weapon;
						if (missionWeapon.CurrentUsageItem.WeaponClass != WeaponClass.BallistaStone)
						{
							missionWeapon = missile.Weapon;
							flag12 = missionWeapon.CurrentUsageItem.WeaponClass == WeaponClass.BallistaBoulder;
							goto IL_0837;
						}
					}
				}
				flag12 = true;
				IL_0837:
				float num4;
				if (!flag12)
				{
					missionWeapon = missile.Weapon;
					num4 = (missionWeapon.CurrentUsageItem.IsAmmo ? 7f : 9f);
				}
				else
				{
					num4 = 13.1f;
				}
				float num5 = num4;
				Agent shooterAgent = missile.ShooterAgent;
				Vec3 vec = missile.GetPosition();
				this.AddSoundAlarmFactorToAgents(shooterAgent, in vec, num5);
			}
			this.HandleMissileCollisionReaction(collisionData.AffectorWeaponSlotOrMissileIndex, missileCollisionReaction, matrixFrame, flag11, attacker, victim, collisionData.AttackBlockedWithShield, collisionData.CollisionBoneIndex, missionObject, zero, zero2, -1);
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissileHit(attacker, victim, flag5, collisionData);
			}
			if ((victim != null && collisionData.AttackBlockedWithShield) || collisionData.MissileBlockedWithWeapon)
			{
				victim.UpdateLastRecievedContactTimes(collisionData.IsMissile);
			}
			return missileCollisionReaction != Mission.MissileCollisionReaction.PassThrough;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0005E184 File Offset: 0x0005C384
		public void HandleMissileCollisionReaction(int missileIndex, Mission.MissileCollisionReaction collisionReaction, MatrixFrame attachLocalFrame, bool isAttachedFrameLocal, Agent attackerAgent, Agent attachedAgent, bool attachedToShield, sbyte attachedBoneIndex, MissionObject attachedMissionObject, Vec3 bounceBackVelocity, Vec3 bounceBackAngularVelocity, int forcedSpawnIndex)
		{
			Mission.Missile missile = this._missilesDictionary[missileIndex];
			MissionObjectId missionObjectId = new MissionObjectId(-1, true);
			switch (collisionReaction)
			{
			case Mission.MissileCollisionReaction.Stick:
				missile.Entity.SetVisibilityExcludeParents(true);
				if (attachedAgent != null)
				{
					this.PrepareMissileWeaponForDrop(missileIndex);
					if (attachedToShield)
					{
						EquipmentIndex offhandWieldedItemIndex = attachedAgent.GetOffhandWieldedItemIndex();
						attachedAgent.AttachWeaponToWeapon(offhandWieldedItemIndex, missile.Weapon, missile.Entity, ref attachLocalFrame);
					}
					else
					{
						attachedAgent.AttachWeaponToBone(missile.Weapon, missile.Entity, attachedBoneIndex, ref attachLocalFrame);
					}
				}
				else
				{
					Vec3 zero = Vec3.Zero;
					missionObjectId = this.SpawnWeaponAsDropFromMissile(missileIndex, attachedMissionObject, in attachLocalFrame, Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithStaticPhysics, in zero, in zero, forcedSpawnIndex);
				}
				break;
			case Mission.MissileCollisionReaction.BounceBack:
				missile.Entity.SetVisibilityExcludeParents(true);
				missionObjectId = this.SpawnWeaponAsDropFromMissile(missileIndex, null, in attachLocalFrame, Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithPhysics, in bounceBackVelocity, in bounceBackAngularVelocity, forcedSpawnIndex);
				break;
			case Mission.MissileCollisionReaction.BecomeInvisible:
				missile.Entity.Remove(81);
				break;
			}
			bool flag = collisionReaction != Mission.MissileCollisionReaction.PassThrough;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new HandleMissileCollisionReaction(missileIndex, collisionReaction, attachLocalFrame, isAttachedFrameLocal, attackerAgent.Index, (attachedAgent != null) ? attachedAgent.Index : (-1), attachedToShield, attachedBoneIndex, (attachedMissionObject != null) ? attachedMissionObject.Id : MissionObjectId.Invalid, bounceBackVelocity, bounceBackAngularVelocity, missionObjectId.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			else if (GameNetwork.IsClientOrReplay && flag)
			{
				this.RemoveMissileAsClient(missileIndex);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissileCollisionReaction(collisionReaction, attackerAgent, attachedAgent, attachedBoneIndex);
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0005E324 File Offset: 0x0005C524
		[UsedImplicitly]
		[MBCallback(null, true)]
		internal void MissileCalculatePassbySoundParametersCallbackMT(int missileIndex, ref SoundEventParameter soundEventParameter)
		{
			this._missilesDictionary[missileIndex].CalculatePassbySoundParametersMT(ref soundEventParameter);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0005E338 File Offset: 0x0005C538
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void ChargeDamageCallback(ref AttackCollisionData collisionData, Blow blow, Agent attacker, Agent victim)
		{
			if (victim.CurrentMortalityState != Agent.MortalityState.Invulnerable && (attacker.RiderAgent == null || attacker.IsEnemyOf(victim) || this.IsFriendlyFireAllowedForChargeDamage()))
			{
				WeaponComponentData weaponComponentData;
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, WeakGameEntity.Invalid, 1f, in MissionWeapon.Invalid, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				if (collisionData.CollidedWithShieldOnBack && weaponComponentData != null && victim != null && victim.IsMainAgent)
				{
					InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_hit_shield_on_back", null).ToString(), Color.ConvertStringToColor("#FFFFFFFF")));
				}
				if ((float)collisionData.InflictedDamage > 0f)
				{
					blow.BaseMagnitude = collisionData.BaseMagnitude;
					blow.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
					blow.InflictedDamage = collisionData.InflictedDamage;
					blow.SelfInflictedDamage = collisionData.SelfInflictedDamage;
					blow.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
					blow.DamageCalculated = true;
					if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedBackByBlow(attacker, victim, in collisionData, null, in blow))
					{
						blow.BlowFlag |= BlowFlags.KnockBack;
					}
					else
					{
						blow.BlowFlag &= ~BlowFlags.KnockBack;
					}
					if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedDownByBlow(attacker, victim, in collisionData, null, in blow))
					{
						blow.BlowFlag |= BlowFlags.KnockDown;
					}
					WeakGameEntity invalid = WeakGameEntity.Invalid;
					Blow blow2 = blow;
					MissionWeapon missionWeapon = default(MissionWeapon);
					this.RegisterBlow(attacker, victim, invalid, blow2, ref collisionData, in missionWeapon, ref combatLogData);
				}
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0005E49C File Offset: 0x0005C69C
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void FallDamageCallback(ref AttackCollisionData collisionData, Blow b, Agent attacker, Agent victim)
		{
			if (victim.CurrentMortalityState != Agent.MortalityState.Invulnerable)
			{
				WeaponComponentData weaponComponentData;
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, WeakGameEntity.Invalid, 1f, in MissionWeapon.Invalid, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				b.BaseMagnitude = collisionData.BaseMagnitude;
				b.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
				b.InflictedDamage = collisionData.InflictedDamage;
				b.SelfInflictedDamage = collisionData.SelfInflictedDamage;
				b.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
				b.DamageCalculated = true;
				if (b.InflictedDamage > 0)
				{
					Agent riderAgent = victim.RiderAgent;
					WeakGameEntity invalid = WeakGameEntity.Invalid;
					Blow blow = b;
					MissionWeapon missionWeapon = default(MissionWeapon);
					this.RegisterBlow(attacker, victim, invalid, blow, ref collisionData, in missionWeapon, ref combatLogData);
					if (riderAgent != null)
					{
						this.FallDamageCallback(ref collisionData, b, riderAgent, riderAgent);
					}
				}
			}
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0005E55C File Offset: 0x0005C75C
		public void KillAgentsOnEntity(GameEntity entity, Agent destroyerAgent, bool burnAgents)
		{
			if (entity == null)
			{
				return;
			}
			int num;
			sbyte b;
			if (destroyerAgent != null)
			{
				num = destroyerAgent.Index;
				b = destroyerAgent.Monster.MainHandItemBoneIndex;
			}
			else
			{
				num = -1;
				b = -1;
			}
			Vec3 vec;
			Vec3 vec2;
			entity.GetPhysicsMinMax(true, out vec, out vec2, false);
			Vec2 vec3 = (vec2.AsVec2 + vec.AsVec2) * 0.5f;
			float num2 = (vec2.AsVec2 - vec.AsVec2).Length * 0.5f;
			Blow blow = new Blow(num);
			blow.DamageCalculated = true;
			blow.BaseMagnitude = 2000f;
			blow.InflictedDamage = 2000;
			blow.Direction = new Vec3(0f, 0f, -1f, -1f);
			blow.DamageType = DamageTypes.Blunt;
			blow.BoneIndex = 0;
			blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, 0);
			if (burnAgents)
			{
				blow.WeaponRecord.WeaponFlags = blow.WeaponRecord.WeaponFlags | (WeaponFlags.AffectsArea | WeaponFlags.Burning);
				blow.WeaponRecord.CurrentPosition = blow.GlobalPosition;
				blow.WeaponRecord.StartingPosition = blow.GlobalPosition;
			}
			MatrixFrame globalFrame = entity.GetGlobalFrame();
			Vec3 vec4 = vec3.ToVec3(0f);
			Vec2 asVec = globalFrame.TransformToParent(in vec4).AsVec2;
			List<Agent> list = new List<Agent>();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(this, asVec, num2, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				WeakGameEntity weakGameEntity = lastFoundAgent.GetSteppedEntity();
				while (weakGameEntity.IsValid && !(weakGameEntity == entity))
				{
					weakGameEntity = weakGameEntity.Parent;
				}
				if (weakGameEntity.IsValid)
				{
					list.Add(lastFoundAgent);
				}
				AgentProximityMap.FindNext(this, ref proximityMapSearchStruct);
			}
			foreach (Agent agent in list)
			{
				blow.GlobalPosition = agent.Position;
				AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Abdomen, b, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);
				agent.RegisterBlow(blow, in attackCollisionDataForDebugPurpose);
			}
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x0005E7E8 File Offset: 0x0005C9E8
		public void KillAgentCheat(Agent agent)
		{
			if (!GameNetwork.IsClientOrReplay && this.Mode != MissionMode.CutScene && this.Mode != MissionMode.Conversation && this.Mode != MissionMode.Barter)
			{
				Agent agent2 = this.MainAgent ?? agent;
				Blow blow = new Blow(agent2.Index);
				blow.DamageType = DamageTypes.Blunt;
				blow.BoneIndex = agent.Monster.HeadLookDirectionBoneIndex;
				blow.GlobalPosition = agent.Position;
				blow.GlobalPosition.z = blow.GlobalPosition.z + agent.GetEyeGlobalHeight();
				blow.BaseMagnitude = 2000f;
				blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
				blow.InflictedDamage = 2000;
				blow.SwingDirection = agent.LookDirection;
				if (this.InputManager.IsGameKeyDown(2))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(-1f, 0f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				else if (this.InputManager.IsGameKeyDown(3))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(1f, 0f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				else if (this.InputManager.IsGameKeyDown(1))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				else if (this.InputManager.IsGameKeyDown(0))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(0f, 1f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				blow.Direction = blow.SwingDirection;
				blow.DamageCalculated = true;
				sbyte mainHandItemBoneIndex = agent2.Monster.MainHandItemBoneIndex;
				AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);
				agent.RegisterBlow(blow, in attackCollisionDataForDebugPurpose);
			}
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0005EAA0 File Offset: 0x0005CCA0
		public bool KillCheats(bool killAll, bool killEnemy, bool killHorse, bool killYourself)
		{
			bool flag = false;
			if (!GameNetwork.IsClientOrReplay && this.Mode != MissionMode.CutScene && this.Mode != MissionMode.Conversation && this.Mode != MissionMode.Barter)
			{
				if (killYourself)
				{
					if (this.MainAgent != null)
					{
						if (killHorse)
						{
							if (this.MainAgent.MountAgent != null)
							{
								Agent mountAgent = this.MainAgent.MountAgent;
								this.KillAgentCheat(mountAgent);
								flag = true;
							}
						}
						else
						{
							Agent mainAgent = this.MainAgent;
							this.KillAgentCheat(mainAgent);
							flag = true;
						}
					}
				}
				else
				{
					bool flag2 = false;
					int num = this.Agents.Count - 1;
					while (num >= 0 && !flag2)
					{
						Agent agent = this.Agents[num];
						if (agent != this.MainAgent && agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack) && this.PlayerTeam != null)
						{
							if (killEnemy)
							{
								if (agent.Team != null && agent.Team.IsValid && this.PlayerTeam.IsEnemyOf(agent.Team))
								{
									if (killHorse && agent.HasMount)
									{
										if (agent.MountAgent != null)
										{
											this.KillAgentCheat(agent.MountAgent);
											if (!killAll)
											{
												flag2 = true;
											}
											flag = true;
										}
									}
									else
									{
										this.KillAgentCheat(agent);
										if (!killAll)
										{
											flag2 = true;
										}
										flag = true;
									}
								}
							}
							else if (agent.Team != null && agent.Team.IsValid && this.PlayerTeam.IsFriendOf(agent.Team))
							{
								if (killHorse)
								{
									if (agent.MountAgent != null)
									{
										this.KillAgentCheat(agent.MountAgent);
										if (!killAll)
										{
											flag2 = true;
										}
										flag = true;
									}
								}
								else
								{
									this.KillAgentCheat(agent);
									if (!killAll)
									{
										flag2 = true;
									}
									flag = true;
								}
							}
						}
						num--;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0005EC64 File Offset: 0x0005CE64
		private bool CancelsDamageAndBlocksAttackBecauseOfNonEnemyCase(Agent attacker, Agent victim)
		{
			if (victim == null || attacker == null)
			{
				return false;
			}
			bool flag = !GameNetwork.IsSessionActive || this.ForceNoFriendlyFire || (MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 && MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0) || this.Mode == MissionMode.Duel || attacker.Controller == AgentControllerType.AI;
			bool flag2 = attacker.IsFriendOf(victim);
			return (flag && flag2) || (victim.IsHuman && !flag2 && !attacker.IsEnemyOf(victim));
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0005ECE0 File Offset: 0x0005CEE0
		private bool IsFriendlyFireAllowedForChargeDamage()
		{
			if (!GameNetwork.IsServer)
			{
				return false;
			}
			if (this._doesMissionAllowChargeDamageOnFriendly == null || this._doesMissionAllowChargeDamageOnFriendly == null)
			{
				MissionMultiplayerGameModeBase missionBehavior = this.GetMissionBehavior<MissionMultiplayerGameModeBase>();
				this._doesMissionAllowChargeDamageOnFriendly = new bool?(missionBehavior.IsGameModeAllowChargeDamageOnFriendly);
			}
			return this._doesMissionAllowChargeDamageOnFriendly.Value && (MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0 || MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0005ED50 File Offset: 0x0005CF50
		public bool CanTakeControlOfAgent(Agent agentToTakeControlOf)
		{
			return this._canPlayerTakeControlOfAnotherAgentWhenDead && this.MainAgent == null && agentToTakeControlOf != null && agentToTakeControlOf.IsHuman && agentToTakeControlOf.IsActive() && agentToTakeControlOf.Team != null && agentToTakeControlOf.Team == this.PlayerTeam && !agentToTakeControlOf.IsUsingGameObject && !agentToTakeControlOf.Character.IsHero && agentToTakeControlOf.Health / agentToTakeControlOf.HealthLimit >= 0.25f;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0005EDC4 File Offset: 0x0005CFC4
		public void SetPlayerCanTakeControlOfAnotherAgentWhenDead()
		{
			this._canPlayerTakeControlOfAnotherAgentWhenDead = true;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0005EDCD File Offset: 0x0005CFCD
		public void TakeControlOfAgent(Agent agentToTakeControlOf)
		{
			if (this.IsFastForward)
			{
				this.IsFastForward = false;
			}
			agentToTakeControlOf.Controller = AgentControllerType.Player;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0005EDE5 File Offset: 0x0005CFE5
		public float GetDamageMultiplierOfCombatDifficulty(Agent victimAgent, Agent attackerAgent = null)
		{
			if (MissionGameModels.Current.MissionDifficultyModel != null)
			{
				return MissionGameModels.Current.MissionDifficultyModel.GetDamageMultiplierOfCombatDifficulty(victimAgent, attackerAgent);
			}
			return 1f;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0005EE0C File Offset: 0x0005D00C
		public float GetShootDifficulty(Agent affectedAgent, Agent affectorAgent, bool isHeadShot)
		{
			Vec2 vec = affectedAgent.MovementVelocity - affectorAgent.MovementVelocity;
			Vec3 vec2 = new Vec3(vec.x, vec.y, 0f, -1f);
			Vec3 vec3 = affectedAgent.Position - affectorAgent.Position;
			float num = vec3.Normalize();
			float num2 = vec2.Normalize();
			float length = Vec3.CrossProduct(vec2, vec3).Length;
			float num3 = MBMath.ClampFloat(0.3f * ((4f + num) / 4f) * ((4f + length * num2) / 4f), 1f, 12f);
			if (isHeadShot)
			{
				num3 *= 1.2f;
			}
			return num3;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0005EEC4 File Offset: 0x0005D0C4
		private MatrixFrame CalculateAttachedLocalFrame(in MatrixFrame attachedGlobalFrame, AttackCollisionData collisionData, WeaponComponentData missileWeapon, Agent affectedAgent, WeakGameEntity hitEntity, Vec3 missileMovementVelocity, Vec3 missileRotationSpeed, MatrixFrame shieldGlobalFrame, bool shouldMissilePenetrate, out bool isAttachedFrameLocal)
		{
			isAttachedFrameLocal = false;
			MatrixFrame matrixFrame = attachedGlobalFrame;
			bool isNonZero = missileWeapon.RotationSpeed.IsNonZero;
			bool flag = affectedAgent != null && !collisionData.AttackBlockedWithShield && missileWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.AmmoSticksWhenShot);
			float managedParameter = ManagedParameters.Instance.GetManagedParameter(flag ? (isNonZero ? ManagedParametersEnum.RotatingProjectileMinPenetration : ManagedParametersEnum.ProjectileMinPenetration) : ManagedParametersEnum.ObjectMinPenetration);
			float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(flag ? (isNonZero ? ManagedParametersEnum.RotatingProjectileMaxPenetration : ManagedParametersEnum.ProjectileMaxPenetration) : ManagedParametersEnum.ObjectMaxPenetration);
			Vec3 vec = missileMovementVelocity;
			float num = vec.Normalize();
			float num2 = MBMath.ClampFloat(flag ? ((float)collisionData.InflictedDamage / affectedAgent.HealthLimit) : (num / ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ProjectileMaxPenetrationSpeed)), 0f, 1f);
			if (shouldMissilePenetrate)
			{
				float num3 = managedParameter + (managedParameter2 - managedParameter) * num2;
				matrixFrame.origin += vec * num3;
			}
			MatrixFrame matrixFrame2;
			if (missileRotationSpeed.IsNonZero)
			{
				float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(flag ? ManagedParametersEnum.AgentProjectileNormalWeight : ManagedParametersEnum.ProjectileNormalWeight);
				matrixFrame2 = missileWeapon.GetMissileStartingFrame();
				Vec3 vec2 = matrixFrame2.TransformToParent(in missileRotationSpeed);
				Vec3 vec3 = -collisionData.CollisionGlobalNormal;
				float num4 = vec2.x * vec2.x;
				float num5 = vec2.y * vec2.y;
				float num6 = vec2.z * vec2.z;
				int num7 = ((num4 > num5 && num4 > num6) ? 0 : ((num5 > num6) ? 1 : 2));
				vec3 -= vec3.ProjectOnUnitVector(matrixFrame.rotation[num7]);
				Vec3 vec4 = Vec3.CrossProduct(vec, vec3.NormalizedCopy());
				float num8 = vec4.Normalize();
				matrixFrame.rotation.RotateAboutAnArbitraryVector(in vec4, MathF.Asin(MathF.Clamp(num8, 0f, 1f)) * managedParameter3);
			}
			if (!collisionData.AttackBlockedWithShield && affectedAgent != null)
			{
				float num9 = Vec3.DotProduct(collisionData.CollisionGlobalNormal, vec) + 1f;
				if (num9 > 0.5f)
				{
					matrixFrame.origin -= num9 * 0.1f * collisionData.CollisionGlobalNormal;
				}
			}
			matrixFrame2 = missileWeapon.GetMissileStartingFrame();
			MatrixFrame matrixFrame3 = missileWeapon.StickingFrame;
			matrixFrame2 = matrixFrame2.TransformToParent(in matrixFrame3);
			matrixFrame = matrixFrame.TransformToParent(in matrixFrame2);
			matrixFrame3 = missileWeapon.GetMissileStartingFrame();
			matrixFrame = matrixFrame.TransformToParent(in matrixFrame3);
			if (collisionData.AttackBlockedWithShield)
			{
				matrixFrame = shieldGlobalFrame.TransformToLocal(in matrixFrame);
				isAttachedFrameLocal = true;
			}
			else if (affectedAgent != null)
			{
				if (flag)
				{
					MBAgentVisuals agentVisuals = affectedAgent.AgentVisuals;
					matrixFrame3 = agentVisuals.GetGlobalFrame();
					matrixFrame2 = agentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex(collisionData.CollisionBoneIndex);
					matrixFrame = matrixFrame3.TransformToParent(in matrixFrame2).GetUnitRotFrame(affectedAgent.AgentScale).TransformToLocalNonOrthogonal(in matrixFrame);
					isAttachedFrameLocal = true;
				}
			}
			else if (hitEntity.IsValid)
			{
				if (collisionData.CollisionBoneIndex >= 0)
				{
					matrixFrame = hitEntity.Skeleton.GetBoneEntitialFrameWithIndex(collisionData.CollisionBoneIndex).TransformToLocalNonOrthogonal(in matrixFrame);
					isAttachedFrameLocal = true;
				}
				else
				{
					matrixFrame2 = hitEntity.GetGlobalFrame();
					matrixFrame = matrixFrame2.TransformToLocalNonOrthogonal(in matrixFrame);
					isAttachedFrameLocal = true;
				}
			}
			else
			{
				matrixFrame.origin.z = Math.Max(matrixFrame.origin.z, -100f);
			}
			return matrixFrame;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0005F220 File Offset: 0x0005D420
		[UsedImplicitly]
		[MBCallback(null, true)]
		internal void GetDefendCollisionResults(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, int attackerWeaponSlotIndex, bool isAlternativeAttack, StrikeType strikeType, Agent.UsageDirection attackDirection, float collisionDistanceOnWeapon, float attackProgress, bool attackIsParried, bool isPassiveUsageHit, bool isHeavyAttack, ref float defenderStunPeriod, ref float attackerStunPeriod, ref bool crushedThrough)
		{
			bool flag = false;
			MissionCombatMechanicsHelper.GetDefendCollisionResults(attackerAgent, defenderAgent, collisionResult, attackerWeaponSlotIndex, isAlternativeAttack, strikeType, attackDirection, collisionDistanceOnWeapon, attackProgress, attackIsParried, isPassiveUsageHit, isHeavyAttack, ref defenderStunPeriod, ref attackerStunPeriod, ref crushedThrough, ref flag);
			if ((crushedThrough || flag) && (attackerAgent.CanLogCombatFor || defenderAgent.CanLogCombatFor))
			{
				CombatLogData combatLogData = new CombatLogData(false, attackerAgent.IsHuman, attackerAgent.IsMine, attackerAgent.RiderAgent != null, attackerAgent.RiderAgent != null && attackerAgent.RiderAgent.IsMine, attackerAgent.IsMount, defenderAgent.IsHuman, defenderAgent.IsMine, defenderAgent.Health <= 0f, defenderAgent.HasMount, defenderAgent.RiderAgent != null && defenderAgent.RiderAgent.IsMine, defenderAgent.IsMount, null, defenderAgent.RiderAgent == attackerAgent, crushedThrough, flag, 0f);
				this.AddCombatLogSafe(attackerAgent, defenderAgent, combatLogData);
			}
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0005F304 File Offset: 0x0005D504
		private CombatLogData GetAttackCollisionResults(Agent attackerAgent, Agent victimAgent, WeakGameEntity hitObject, float momentumRemaining, in MissionWeapon attackerWeapon, bool crushedThrough, bool cancelDamage, bool crushedThroughWithoutAgentCollision, ref AttackCollisionData attackCollisionData, out WeaponComponentData shieldOnBack, out CombatLogData combatLog)
		{
			AttackInformation attackInformation = new AttackInformation(attackerAgent, victimAgent, hitObject, in attackCollisionData, in attackerWeapon);
			shieldOnBack = attackInformation.ShieldOnBack;
			int num;
			MissionCombatMechanicsHelper.GetAttackCollisionResults(in attackInformation, crushedThrough, momentumRemaining, cancelDamage, ref attackCollisionData, out combatLog, out num);
			float num2 = (float)attackCollisionData.InflictedDamage;
			if (num2 > 0f)
			{
				float num3 = MissionGameModels.Current.AgentApplyDamageModel.CalculateDamage(in attackInformation, in attackCollisionData, num2);
				combatLog.ModifiedDamage = MathF.Round(num3 - num2);
				attackCollisionData.InflictedDamage = MathF.Round(num3);
			}
			else
			{
				combatLog.ModifiedDamage = 0;
				attackCollisionData.InflictedDamage = 0;
			}
			combatLog.ReflectedDamage = 0;
			if (!attackCollisionData.IsFallDamage && attackInformation.IsFriendlyFire)
			{
				if (!attackInformation.IsAttackerAIControlled && GameNetwork.IsSessionActive)
				{
					int num4 = (attackCollisionData.IsMissile ? MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					attackCollisionData.SelfInflictedDamage = MathF.Round((float)attackCollisionData.InflictedDamage * ((float)num4 * 0.01f));
					attackCollisionData.SelfInflictedDamage = MBMath.ClampInt(attackCollisionData.SelfInflictedDamage, 0, 2000);
					int num5 = (attackCollisionData.IsMissile ? MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					attackCollisionData.InflictedDamage = MathF.Round((float)attackCollisionData.InflictedDamage * ((float)num5 * 0.01f));
					attackCollisionData.InflictedDamage = MBMath.ClampInt(attackCollisionData.InflictedDamage, 0, 2000);
					combatLog.InflictedDamage = attackCollisionData.InflictedDamage;
				}
				combatLog.IsFriendlyFire = true;
				combatLog.ReflectedDamage = attackCollisionData.SelfInflictedDamage;
			}
			if (attackCollisionData.AttackBlockedWithShield && attackCollisionData.InflictedDamage > 0 && (int)attackInformation.VictimShield.HitPoints - attackCollisionData.InflictedDamage <= 0)
			{
				attackCollisionData.IsShieldBroken = true;
			}
			if (!crushedThroughWithoutAgentCollision)
			{
				combatLog.BodyPartHit = attackCollisionData.VictimHitBodyPart;
			}
			return combatLog;
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0005F4DC File Offset: 0x0005D6DC
		private void PrintAttackCollisionResults(Agent attackerAgent, Agent victimAgent, MissionObject missionObjectHit, ref AttackCollisionData attackCollisionData, ref CombatLogData combatLog)
		{
			if (attackCollisionData.IsColliderAgent && !attackCollisionData.AttackBlockedWithShield && attackerAgent != null && (attackerAgent.CanLogCombatFor || victimAgent.CanLogCombatFor) && victimAgent.State == AgentState.Active)
			{
				combatLog.MissionObjectHit = missionObjectHit;
				this.AddCombatLogSafe(attackerAgent, victimAgent, combatLog);
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0005F534 File Offset: 0x0005D734
		public void AddCombatLogSafe(Agent attackerAgent, Agent victimAgent, CombatLogData combatLog)
		{
			MissionObject missionObjectHit = combatLog.MissionObjectHit;
			combatLog.SetVictimAgent(victimAgent);
			if (GameNetwork.IsServerOrRecorder)
			{
				CombatLogNetworkMessage combatLogNetworkMessage = new CombatLogNetworkMessage(attackerAgent.Index, (victimAgent != null) ? victimAgent.Index : (-1), (missionObjectHit != null) ? missionObjectHit.Id : MissionObjectId.Invalid, combatLog);
				object obj = ((attackerAgent == null) ? null : (attackerAgent.IsHuman ? attackerAgent : attackerAgent.RiderAgent));
				object obj2;
				if (obj == null)
				{
					obj2 = null;
				}
				else
				{
					MissionPeer missionPeer = obj.MissionPeer;
					obj2 = ((missionPeer != null) ? missionPeer.Peer.Communicator : null);
				}
				NetworkCommunicator networkCommunicator = obj2 as NetworkCommunicator;
				object obj3 = ((victimAgent == null) ? null : (victimAgent.IsHuman ? victimAgent : victimAgent.RiderAgent));
				object obj4;
				if (obj3 == null)
				{
					obj4 = null;
				}
				else
				{
					MissionPeer missionPeer2 = obj3.MissionPeer;
					obj4 = ((missionPeer2 != null) ? missionPeer2.Peer.Communicator : null);
				}
				NetworkCommunicator networkCommunicator2 = obj4 as NetworkCommunicator;
				if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator);
					GameNetwork.WriteMessage(combatLogNetworkMessage);
					GameNetwork.EndModuleEventAsServer();
				}
				if (networkCommunicator2 != null && !networkCommunicator2.IsServerPeer && networkCommunicator2 != networkCommunicator)
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
					GameNetwork.WriteMessage(combatLogNetworkMessage);
					GameNetwork.EndModuleEventAsServer();
				}
			}
			this._combatLogsCreated.Enqueue(combatLog);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0005F640 File Offset: 0x0005D840
		public MissionObject CreateMissionObjectFromPrefab(string prefab, MatrixFrame frame, bool hasCustomRestOffset, float restOffset, Action<GameEntity> actionAppliedBeforeScriptInitialization)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				GameEntity gameEntity;
				if (hasCustomRestOffset)
				{
					gameEntity = GameEntity.InstantiateWithRestOffset(this.Scene, prefab, true, frame, restOffset, false, "");
				}
				else
				{
					gameEntity = GameEntity.Instantiate(this.Scene, prefab, frame, false);
				}
				actionAppliedBeforeScriptInitialization(gameEntity);
				gameEntity.CallScriptCallbacks(true);
				MissionObject firstScriptOfType = gameEntity.GetFirstScriptOfType<MissionObject>();
				List<MissionObjectId> list = new List<MissionObjectId>();
				using (IEnumerator<GameEntity> enumerator = gameEntity.GetChildren().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionObject firstScriptOfType2;
						if ((firstScriptOfType2 = enumerator.Current.GetFirstScriptOfType<MissionObject>()) != null)
						{
							list.Add(firstScriptOfType2.Id);
						}
					}
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new CreateMissionObject(firstScriptOfType.Id, prefab, frame, list));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					this.AddDynamicallySpawnedMissionObjectInfo(new Mission.DynamicallyCreatedEntity(prefab, firstScriptOfType.Id, frame, ref list));
				}
				return firstScriptOfType;
			}
			return null;
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0005F72C File Offset: 0x0005D92C
		public int GetNearbyAllyAgentsCount(Vec2 center, float radius, Team team)
		{
			return this.GetNearbyAgentsCountAux(center, radius, team.MBTeam, Mission.GetNearbyAgentsAuxType.Friend);
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0005F73D File Offset: 0x0005D93D
		public MBList<Agent> GetNearbyAllyAgents(Vec2 center, float radius, Team team, MBList<Agent> agents)
		{
			agents.Clear();
			this.GetNearbyAgentsAux(center, radius, team.MBTeam, Mission.GetNearbyAgentsAuxType.Friend, agents);
			return agents;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0005F759 File Offset: 0x0005D959
		public MBList<Agent> GetNearbyEnemyAgents(Vec2 center, float radius, Team team, MBList<Agent> agents)
		{
			agents.Clear();
			this.GetNearbyAgentsAux(center, radius, team.MBTeam, Mission.GetNearbyAgentsAuxType.Enemy, agents);
			return agents;
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0005F775 File Offset: 0x0005D975
		public MBList<Agent> GetNearbyAgents(Vec2 center, float radius, MBList<Agent> agents)
		{
			agents.Clear();
			this.GetNearbyAgentsAux(center, radius, MBTeam.InvalidTeam, Mission.GetNearbyAgentsAuxType.All, agents);
			return agents;
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0005F790 File Offset: 0x0005D990
		public bool IsFormationUnitPositionAvailableMT(ref WorldPosition formationPosition, ref WorldPosition unitPosition, ref WorldPosition nearestAvailableUnitPosition, float manhattanDistance, Team team)
		{
			if (!formationPosition.IsValid || formationPosition.GetNavMeshMT() == UIntPtr.Zero || !unitPosition.IsValid || unitPosition.GetNavMeshMT() == UIntPtr.Zero)
			{
				return false;
			}
			if (this.IsFormationUnitPositionAvailable_AdditionalCondition != null && !this.IsFormationUnitPositionAvailable_AdditionalCondition(unitPosition, team))
			{
				return false;
			}
			if (this.Mode == MissionMode.Deployment && this.DeploymentPlan.HasDeploymentBoundaries(team))
			{
				IMissionDeploymentPlan deploymentPlan = this.DeploymentPlan;
				Vec2 asVec = unitPosition.AsVec2;
				if (!deploymentPlan.IsPositionInsideDeploymentBoundaries(team, in asVec))
				{
					return false;
				}
			}
			return this.IsFormationUnitPositionAvailableAuxMT(ref formationPosition, ref unitPosition, ref nearestAvailableUnitPosition, manhattanDistance);
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x0005F830 File Offset: 0x0005DA30
		public bool IsOrderPositionAvailable(in WorldPosition orderPosition, Team team)
		{
			WorldPosition worldPosition = orderPosition;
			if (worldPosition.IsValid)
			{
				worldPosition = orderPosition;
				if (!(worldPosition.GetNavMesh() == UIntPtr.Zero))
				{
					if (this.IsFormationUnitPositionAvailable_AdditionalCondition != null && !this.IsFormationUnitPositionAvailable_AdditionalCondition(orderPosition, team))
					{
						return false;
					}
					worldPosition = orderPosition;
					return this.IsPositionInsideBoundaries(worldPosition.AsVec2);
				}
			}
			return false;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0005F89C File Offset: 0x0005DA9C
		public bool IsFormationUnitPositionAvailable(ref WorldPosition unitPosition, Team team)
		{
			WorldPosition worldPosition = unitPosition;
			float num = 1f;
			WorldPosition invalid = WorldPosition.Invalid;
			return this.IsFormationUnitPositionAvailableMT(ref worldPosition, ref unitPosition, ref invalid, num, team);
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0005F8C9 File Offset: 0x0005DAC9
		public bool HasSceneMapPatch()
		{
			return this.InitializerRecord.SceneHasMapPatch;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0005F8D8 File Offset: 0x0005DAD8
		public bool GetPatchSceneEncounterPosition(out Vec3 position)
		{
			if (this.InitializerRecord.SceneHasMapPatch)
			{
				Vec2 patchCoordinates = this.InitializerRecord.PatchCoordinates;
				float northRotation = this.Scene.GetNorthRotation();
				Vec2 vec;
				Vec2 vec2;
				this.Boundaries.GetOrientedBoundariesBox(out vec, out vec2, northRotation);
				Vec2 side = Vec2.Side;
				side.RotateCCW(northRotation);
				Vec2 vec3 = side.LeftVec();
				Vec2 vec4 = vec2 - vec;
				Vec2 vec5 = vec.x * side + vec.y * vec3 + vec4.x * patchCoordinates.x * side + vec4.y * patchCoordinates.y * vec3;
				position = vec5.ToVec3(this.Scene.GetTerrainHeight(vec5, true));
				return true;
			}
			position = Vec3.Invalid;
			return false;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0005F9BC File Offset: 0x0005DBBC
		public bool GetPatchSceneEncounterDirection(out Vec2 direction)
		{
			if (this.InitializerRecord.SceneHasMapPatch)
			{
				float northRotation = this.Scene.GetNorthRotation();
				direction = this.InitializerRecord.PatchEncounterDir;
				direction.RotateCCW(northRotation);
				return true;
			}
			direction = Vec2.Invalid;
			return false;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0005FA08 File Offset: 0x0005DC08
		private void TickDebugAgents()
		{
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0005FA0C File Offset: 0x0005DC0C
		public void AddTimerToDynamicEntity(GameEntity gameEntity, float timeToKill = 10f)
		{
			Mission.DynamicEntityInfo dynamicEntityInfo = new Mission.DynamicEntityInfo
			{
				Entity = gameEntity,
				TimerToDisable = new Timer(this.CurrentTime, timeToKill, true)
			};
			this._dynamicEntities.Add(dynamicEntityInfo);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0005FA45 File Offset: 0x0005DC45
		public void AddListener(IMissionListener listener)
		{
			this._listeners.Add(listener);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0005FA53 File Offset: 0x0005DC53
		public void RemoveListener(IMissionListener listener)
		{
			this._listeners.Remove(listener);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0005FA64 File Offset: 0x0005DC64
		public void OnAgentFleeing(Agent agent)
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnAgentFleeing(agent);
			}
			agent.OnFleeing();
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0005FAA4 File Offset: 0x0005DCA4
		public void OnAgentPanicked(Agent agent)
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnAgentPanicked(agent);
			}
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0005FADC File Offset: 0x0005DCDC
		public void OnInitialSpawnCompleted(BattleSideEnum battleSide = BattleSideEnum.None)
		{
			if (battleSide == BattleSideEnum.None)
			{
				for (int i = 0; i < 2; i++)
				{
					this.OnBattleSideSpawned((BattleSideEnum)i);
				}
			}
			else
			{
				this.OnBattleSideSpawned(battleSide);
			}
			if (this.IsInitialSpawnCompleted() && !this.HasMissionBehavior<DeploymentMissionController>())
			{
				this.OnDeploymentFinished();
				this.OnAfterDeploymentFinished();
			}
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0005FB28 File Offset: 0x0005DD28
		internal void OnDeploymentFinished()
		{
			this.IsDeploymentFinished = true;
			foreach (Team team in this.Teams)
			{
				if (team.TeamAI != null)
				{
					team.TeamAI.OnDeploymentFinished();
				}
			}
			foreach (MissionObject missionObject in this.ActiveMissionObjects)
			{
				missionObject.OnDeploymentFinished();
			}
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnDeploymentFinished();
			}
			Action deploymentFinishedEvent = this.DeploymentFinishedEvent;
			if (deploymentFinishedEvent == null)
			{
				return;
			}
			deploymentFinishedEvent();
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0005FC08 File Offset: 0x0005DE08
		internal void OnAfterDeploymentFinished()
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnAfterDeploymentFinished();
			}
			foreach (Agent agent in this.Agents)
			{
				AgentStatCalculateModel agentStatCalculateModel = MissionGameModels.Current.AgentStatCalculateModel;
				if (agentStatCalculateModel != null)
				{
					agentStatCalculateModel.InitializeAgentStatsAfterDeploymentFinished(agent);
				}
				AgentStatCalculateModel agentStatCalculateModel2 = MissionGameModels.Current.AgentStatCalculateModel;
				if (agentStatCalculateModel2 != null)
				{
					agentStatCalculateModel2.InitializeMissionEquipmentAfterDeploymentFinished(agent);
				}
			}
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0005FCAC File Offset: 0x0005DEAC
		private void OnBattleSideSpawned(BattleSideEnum side)
		{
			this._spawnedBattleSides[(int)side] = true;
			if (this.MissionBehaviors != null)
			{
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnBattleSideSpawned(side);
				}
			}
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0005FD10 File Offset: 0x0005DF10
		private bool IsInitialSpawnCompleted()
		{
			bool flag = true;
			for (int i = 0; i < this._spawnedBattleSides.Length; i++)
			{
				if (!this._spawnedBattleSides[i])
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0005FD41 File Offset: 0x0005DF41
		public void OnFormationCaptainChanged(Formation formation)
		{
			Action<Formation> formationCaptainChanged = this.FormationCaptainChanged;
			if (formationCaptainChanged == null)
			{
				return;
			}
			formationCaptainChanged(formation);
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0005FD54 File Offset: 0x0005DF54
		public void SetFastForwardingFromUI(bool fastForwarding)
		{
			this.IsFastForward = fastForwarding;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0005FD5D File Offset: 0x0005DF5D
		public bool CheckIfBattleInRetreat()
		{
			Func<bool> isBattleInRetreatEvent = this.IsBattleInRetreatEvent;
			return isBattleInRetreatEvent != null && isBattleInRetreatEvent();
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0005FD70 File Offset: 0x0005DF70
		public void AddSpawnedItemEntityCreatedAtRuntime(SpawnedItemEntity spawnedItemEntity)
		{
			this._spawnedItemEntitiesCreatedAtRuntime.Add(spawnedItemEntity);
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0005FD7E File Offset: 0x0005DF7E
		public void TriggerOnItemPickUpEvent(Agent agent, SpawnedItemEntity spawnedItemEntity)
		{
			Action<Agent, SpawnedItemEntity> onItemPickUp = this.OnItemPickUp;
			if (onItemPickUp == null)
			{
				return;
			}
			onItemPickUp(agent, spawnedItemEntity);
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0005FD94 File Offset: 0x0005DF94
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal static void DebugLogNativeMissionNetworkEvent(int eventEnum, string eventName, int bitCount)
		{
			int num = eventEnum + CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo.GetMaximumValue() + 1;
			DebugNetworkEventStatistics.StartEvent(eventName, num);
			DebugNetworkEventStatistics.AddDataToStatistic(bitCount);
			DebugNetworkEventStatistics.EndEvent();
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0005FDC2 File Offset: 0x0005DFC2
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void PauseMission()
		{
			this._missionState.Paused = true;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0005FDD0 File Offset: 0x0005DFD0
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_n_allies", "mission")]
		public static string KillNAllies(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			int num = 0;
			if (strings.Count > 0 && !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'mission.kill_n_allies [count]";
			}
			if (Mission.Current != null && num > 0)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					if (num <= 0)
					{
						break;
					}
					if (team.IsPlayerTeam)
					{
						foreach (Agent agent in team.ActiveAgents.ToList<Agent>())
						{
							if (agent.IsAIControlled)
							{
								Mission.Current.KillAgentCheat(agent);
								if (--num <= 0)
								{
									break;
								}
							}
						}
					}
				}
				return "n allied agents killed.";
			}
			return "No active mission found or less than 1 agent to kill.";
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0005FEDC File Offset: 0x0005E0DC
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_all_allies", "mission")]
		public static string KillAllAllies(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			if (Mission.Current != null)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.IsPlayerTeam || team.IsPlayerAlly)
					{
						foreach (Agent agent in team.ActiveAgents.ToList<Agent>())
						{
							if (agent.IsAIControlled)
							{
								Mission.Current.KillAgentCheat(agent);
							}
						}
					}
				}
				return "Allied agents killed.";
			}
			return "No active mission found";
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0005FFB4 File Offset: 0x0005E1B4
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_all_enemies", "mission")]
		public static string KillAllEnemies(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			if (Mission.Current != null)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.IsPlayerEnemy)
					{
						foreach (Agent agent in team.ActiveAgents.ToList<Agent>())
						{
							if (agent.IsAIControlled)
							{
								Mission.Current.KillAgentCheat(agent);
							}
						}
					}
				}
				return "Enemy agents killed.";
			}
			return "No active mission found";
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x00060084 File Offset: 0x0005E284
		[CommandLineFunctionality.CommandLineArgumentFunction("toggleDisableDying", "mission")]
		public static string ToggleDisableDying(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			int num = 0;
			if (strings.Count > 0 && !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'toggleDisableDying [index]' or just 'toggleDisableDying' for making all agents invincible.";
			}
			if (Mission.Current == null)
			{
				return "No active mission found";
			}
			if (strings.Count == 0 || num == -1)
			{
				Mission.Current.DisableDying = !Mission.Current.DisableDying;
				if (Mission.Current.DisableDying)
				{
					return "Dying disabled for all";
				}
				return "Dying not disabled for all";
			}
			else
			{
				Agent agent = Mission.Current.FindAgentWithIndex(num);
				if (agent != null)
				{
					agent.ToggleInvulnerable();
					return "Disable Dying for agent " + num.ToString() + ": " + (agent.CurrentMortalityState == Agent.MortalityState.Invulnerable).ToString();
				}
				return "Invalid agent index " + num.ToString();
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x0006015C File Offset: 0x0005E35C
		[CommandLineFunctionality.CommandLineArgumentFunction("toggleDisableDyingTeam", "mission")]
		public static string ToggleDisableDyingTeam(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			int num = 0;
			if (strings.Count > 0 && !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'toggleDisableDyingTeam [team_no]' for making all active agents of a team invincible.";
			}
			int num2 = 0;
			foreach (Agent agent in Mission.Current.AllAgents)
			{
				if (agent.Team != null && agent.Team.MBTeam.Index == num)
				{
					agent.ToggleInvulnerable();
					num2++;
				}
			}
			return string.Concat(new object[]
			{
				"Toggled invulnerability for active agents of team ",
				num.ToString(),
				", agent count: ",
				num2
			});
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00060234 File Offset: 0x0005E434
		[CommandLineFunctionality.CommandLineArgumentFunction("killAgent", "mission")]
		public static string KillAgent(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			if (Mission.Current == null)
			{
				return "Current mission does not exist.";
			}
			int num;
			if (strings.Count == 0 || !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'killAgent [index]'";
			}
			Agent agent = Mission.Current.FindAgentWithIndex(num);
			if (agent == null)
			{
				return "Agent " + num.ToString() + " not found.";
			}
			if (agent.State == AgentState.Active)
			{
				Mission.Current.KillAgentCheat(agent);
				return "Agent " + num.ToString() + " died.";
			}
			return "Agent " + num.ToString() + " already dead.";
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000602E8 File Offset: 0x0005E4E8
		[CommandLineFunctionality.CommandLineArgumentFunction("set_battering_ram_speed", "mission")]
		public static string IncreaseBatteringRamSpeeds(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			float num;
			if (strings.Count == 0 || !float.TryParse(strings[0], out num))
			{
				return "Please enter a speed value";
			}
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				if (missionObject.GameEntity.HasScriptOfType<BatteringRam>())
				{
					missionObject.GameEntity.GetFirstScriptOfType<BatteringRam>().MovementComponent.MaxSpeed = num;
					missionObject.GameEntity.GetFirstScriptOfType<BatteringRam>().MovementComponent.MinSpeed = num;
				}
			}
			return "Battering ram max speed increased.";
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000603B0 File Offset: 0x0005E5B0
		[CommandLineFunctionality.CommandLineArgumentFunction("set_siege_tower_speed", "mission")]
		public static string IncreaseSiegeTowerSpeed(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			float num;
			if (strings.Count == 0 || !float.TryParse(strings[0], out num))
			{
				return "Please enter a speed value";
			}
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				if (missionObject.GameEntity.HasScriptOfType<SiegeTower>())
				{
					missionObject.GameEntity.GetFirstScriptOfType<SiegeTower>().MovementComponent.MaxSpeed = num;
					missionObject.GameEntity.GetFirstScriptOfType<SiegeTower>().MovementComponent.MinSpeed = num;
				}
			}
			return "Siege tower max speed increased.";
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00060478 File Offset: 0x0005E678
		[CommandLineFunctionality.CommandLineArgumentFunction("reload_managed_core_params", "game")]
		public static string LoadParamsDebug(List<string> strings)
		{
			if (!GameNetwork.IsSessionActive)
			{
				ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_core_parameters"));
				return "Managed core parameters reloaded.";
			}
			return "Does not work on multiplayer.";
		}

		// Token: 0x04000898 RID: 2200
		public const int MaxRuntimeMissionObjects = 8191;

		// Token: 0x04000899 RID: 2201
		private static readonly object GetNearbyAgentsAuxLock = new object();

		// Token: 0x0400089D RID: 2205
		private int _lastSceneMissionObjectIdCount;

		// Token: 0x0400089E RID: 2206
		private int _lastRuntimeMissionObjectIdCount;

		// Token: 0x0400089F RID: 2207
		private bool _isMainAgentObjectInteractionEnabled = true;

		// Token: 0x040008AA RID: 2218
		private List<Mission.TimeSpeedRequest> _timeSpeedRequests = new List<Mission.TimeSpeedRequest>();

		// Token: 0x040008AB RID: 2219
		private bool _isMainAgentItemInteractionEnabled = true;

		// Token: 0x040008AC RID: 2220
		private readonly MBList<MissionObject> _activeMissionObjects;

		// Token: 0x040008AD RID: 2221
		private readonly MBList<MissionObject> _missionObjects;

		// Token: 0x040008AE RID: 2222
		private readonly List<SpawnedItemEntity> _spawnedItemEntitiesCreatedAtRuntime;

		// Token: 0x040008AF RID: 2223
		private readonly MBList<Mission.DynamicallyCreatedEntity> _addedEntitiesInfo;

		// Token: 0x040008B0 RID: 2224
		private readonly Stack<ValueTuple<int, float>> _emptyRuntimeMissionObjectIds;

		// Token: 0x040008B1 RID: 2225
		private static bool _isCameraFirstPerson = false;

		// Token: 0x040008B5 RID: 2229
		private MissionMode _missionMode;

		// Token: 0x040008B6 RID: 2230
		private float _cachedMissionTime;

		// Token: 0x040008B8 RID: 2232
		public const int MaxNavMeshId = 1000000;

		// Token: 0x040008B9 RID: 2233
		private const float NavigationMeshHeightLimit = 1.5f;

		// Token: 0x040008BA RID: 2234
		private const float SpeedBonusFactorForSwing = 0.7f;

		// Token: 0x040008BB RID: 2235
		private const float SpeedBonusFactorForThrust = 0.5f;

		// Token: 0x040008BC RID: 2236
		private const float _exitTimeInSeconds = 0.6f;

		// Token: 0x040008BD RID: 2237
		private const int MaxNavMeshPerDynamicObject = 10;

		// Token: 0x040008C8 RID: 2248
		private bool? _doesMissionAllowChargeDamageOnFriendly;

		// Token: 0x040008CA RID: 2250
		private bool _missionEnded;

		// Token: 0x040008CB RID: 2251
		private Dictionary<int, Mission.Missile> _missilesDictionary;

		// Token: 0x040008CC RID: 2252
		private MBList<Mission.Missile> _missilesList;

		// Token: 0x040008CD RID: 2253
		private readonly List<Mission.DynamicEntityInfo> _dynamicEntities = new List<Mission.DynamicEntityInfo>();

		// Token: 0x040008D0 RID: 2256
		public bool DisableDying;

		// Token: 0x040008D2 RID: 2258
		public bool ForceNoFriendlyFire;

		// Token: 0x040008D3 RID: 2259
		public const int MaxDamage = 2000;

		// Token: 0x040008D4 RID: 2260
		public bool IsFriendlyMission = true;

		// Token: 0x040008D5 RID: 2261
		public BasicCultureObject MusicCulture;

		// Token: 0x040008D6 RID: 2262
		private int _nextDynamicNavMeshIdStart = 1000050;

		// Token: 0x040008D7 RID: 2263
		private MissionState _missionState;

		// Token: 0x040008D8 RID: 2264
		private List<IMissionListener> _listeners = new List<IMissionListener>();

		// Token: 0x040008D9 RID: 2265
		private BasicMissionTimer _leaveMissionTimer;

		// Token: 0x040008DA RID: 2266
		private MBReadOnlyList<MBSubModuleBase> _cachedSubModuleList;

		// Token: 0x040008DB RID: 2267
		private readonly MBList<KeyValuePair<Agent, MissionTime>> _mountsWithoutRiders;

		// Token: 0x040008DE RID: 2270
		private List<MissionBehavior> _otherMissionBehaviors;

		// Token: 0x040008DF RID: 2271
		private readonly object _lockHelper = new object();

		// Token: 0x040008E0 RID: 2272
		private AgentList _activeAgents;

		// Token: 0x040008E1 RID: 2273
		private bool[] _spawnedBattleSides = new bool[2];

		// Token: 0x040008E2 RID: 2274
		private IMissionDeploymentPlan _deploymentPlan;

		// Token: 0x040008E4 RID: 2276
		public bool IsOrderMenuOpen;

		// Token: 0x040008E5 RID: 2277
		public bool IsTransferMenuOpen;

		// Token: 0x040008E6 RID: 2278
		public bool IsInPhotoMode;

		// Token: 0x040008E7 RID: 2279
		private Agent _initialPlayerAgent;

		// Token: 0x040008E8 RID: 2280
		private Agent _mainAgent;

		// Token: 0x040008E9 RID: 2281
		private Action _onLoadingEndedAction;

		// Token: 0x040008EA RID: 2282
		private Timer _inMissionLoadingScreenTimer;

		// Token: 0x040008EB RID: 2283
		public bool AllowAiTicking = true;

		// Token: 0x040008EC RID: 2284
		private int _agentCreationIndex;

		// Token: 0x040008ED RID: 2285
		private readonly MBList<FleePosition>[] _fleePositions = new MBList<FleePosition>[3];

		// Token: 0x040008EE RID: 2286
		private bool _doesMissionRequireCivilianEquipment;

		// Token: 0x040008EF RID: 2287
		public IAgentVisualCreator AgentVisualCreator;

		// Token: 0x040008F0 RID: 2288
		private readonly int[] _initialAgentCountPerSide = new int[2];

		// Token: 0x040008F1 RID: 2289
		private readonly int[] _removedAgentCountPerSide = new int[2];

		// Token: 0x040008F4 RID: 2292
		private ConcurrentQueue<CombatLogData> _combatLogsCreated = new ConcurrentQueue<CombatLogData>();

		// Token: 0x040008F5 RID: 2293
		private AgentList _allAgents;

		// Token: 0x040008F6 RID: 2294
		[TupleElementNames(new string[] { "Action", "Agent", "Param1", "Param2" })]
		private MBList<ValueTuple<Mission.MissionTickAction, Agent, int, int>> _tickActions = new MBList<ValueTuple<Mission.MissionTickAction, Agent, int, int>>();

		// Token: 0x040008F7 RID: 2295
		private readonly object _tickActionsLock = new object();

		// Token: 0x040008F8 RID: 2296
		private List<SiegeWeapon> _attackerWeaponsForFriendlyFirePreventing = new List<SiegeWeapon>();

		// Token: 0x040008FA RID: 2298
		private bool _isFastForward;

		// Token: 0x04000902 RID: 2306
		private float _missionEndTime;

		// Token: 0x04000903 RID: 2307
		public float MissionCloseTimeAfterFinish = 30f;

		// Token: 0x04000904 RID: 2308
		private static Mission _current = null;

		// Token: 0x04000907 RID: 2311
		public float NextCheckTimeEndMission = 10f;

		// Token: 0x04000909 RID: 2313
		public int NumOfFormationsSpawnedTeamOne;

		// Token: 0x0400090A RID: 2314
		private SoundEvent _ambientSoundEvent;

		// Token: 0x0400090B RID: 2315
		private readonly BattleSpawnPathSelector _battleSpawnPathSelector;

		// Token: 0x0400090C RID: 2316
		private int _agentCount;

		// Token: 0x0400090D RID: 2317
		public int NumOfFormationsSpawnedTeamTwo;

		// Token: 0x04000919 RID: 2329
		private bool _canPlayerTakeControlOfAnotherAgentWhenDead;

		// Token: 0x0400091B RID: 2331
		private bool tickCompleted = true;

		// Token: 0x020004F5 RID: 1269
		public class MBBoundaryCollection : IDictionary<string, ICollection<Vec2>>, ICollection<KeyValuePair<string, ICollection<Vec2>>>, IEnumerable<KeyValuePair<string, ICollection<Vec2>>>, IEnumerable, INotifyCollectionChanged
		{
			// Token: 0x06003C1A RID: 15386 RVA: 0x000F1DCD File Offset: 0x000EFFCD
			IEnumerator IEnumerable.GetEnumerator()
			{
				int count = this.Count;
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					string boundaryName = MBAPI.IMBMission.GetBoundaryName(this._mission.Pointer, i);
					List<Vec2> boundaryPoints = this.GetBoundaryPoints(boundaryName);
					yield return new KeyValuePair<string, ICollection<Vec2>>(boundaryName, boundaryPoints);
					num = i;
				}
				yield break;
			}

			// Token: 0x06003C1B RID: 15387 RVA: 0x000F1DDC File Offset: 0x000EFFDC
			public IEnumerator<KeyValuePair<string, ICollection<Vec2>>> GetEnumerator()
			{
				int count = this.Count;
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					string boundaryName = MBAPI.IMBMission.GetBoundaryName(this._mission.Pointer, i);
					List<Vec2> boundaryPoints = this.GetBoundaryPoints(boundaryName);
					yield return new KeyValuePair<string, ICollection<Vec2>>(boundaryName, boundaryPoints);
					num = i;
				}
				yield break;
			}

			// Token: 0x17000A55 RID: 2645
			// (get) Token: 0x06003C1C RID: 15388 RVA: 0x000F1DEB File Offset: 0x000EFFEB
			public int Count
			{
				get
				{
					return MBAPI.IMBMission.GetBoundaryCount(this._mission.Pointer);
				}
			}

			// Token: 0x06003C1D RID: 15389 RVA: 0x000F1E02 File Offset: 0x000F0002
			public float GetBoundaryRadius(string name)
			{
				return MBAPI.IMBMission.GetBoundaryRadius(this._mission.Pointer, name);
			}

			// Token: 0x17000A56 RID: 2646
			// (get) Token: 0x06003C1E RID: 15390 RVA: 0x000F1E1A File Offset: 0x000F001A
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06003C1F RID: 15391 RVA: 0x000F1E20 File Offset: 0x000F0020
			public void GetOrientedBoundariesBox(out Vec2 boxMinimum, out Vec2 boxMaximum, float rotationInRadians = 0f)
			{
				Vec2 side = Vec2.Side;
				side.RotateCCW(rotationInRadians);
				Vec2 vec = side.LeftVec();
				boxMinimum = new Vec2(float.MaxValue, float.MaxValue);
				boxMaximum = new Vec2(float.MinValue, float.MinValue);
				foreach (ICollection<Vec2> collection in this.Values)
				{
					foreach (Vec2 vec2 in collection)
					{
						float num = Vec2.DotProduct(vec2, side);
						float num2 = Vec2.DotProduct(vec2, vec);
						boxMinimum.x = ((num < boxMinimum.x) ? num : boxMinimum.x);
						boxMinimum.y = ((num2 < boxMinimum.y) ? num2 : boxMinimum.y);
						boxMaximum.x = ((num > boxMaximum.x) ? num : boxMaximum.x);
						boxMaximum.y = ((num2 > boxMaximum.y) ? num2 : boxMaximum.y);
					}
				}
			}

			// Token: 0x06003C20 RID: 15392 RVA: 0x000F1F58 File Offset: 0x000F0158
			internal MBBoundaryCollection(Mission mission)
			{
				this._mission = mission;
			}

			// Token: 0x06003C21 RID: 15393 RVA: 0x000F1F67 File Offset: 0x000F0167
			public void Add(KeyValuePair<string, ICollection<Vec2>> item)
			{
				this.Add(item.Key, item.Value);
			}

			// Token: 0x06003C22 RID: 15394 RVA: 0x000F1F80 File Offset: 0x000F0180
			public void Clear()
			{
				foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
				{
					this.Remove(keyValuePair.Key);
				}
			}

			// Token: 0x06003C23 RID: 15395 RVA: 0x000F1FD0 File Offset: 0x000F01D0
			public bool Contains(KeyValuePair<string, ICollection<Vec2>> item)
			{
				return this.ContainsKey(item.Key);
			}

			// Token: 0x06003C24 RID: 15396 RVA: 0x000F1FE0 File Offset: 0x000F01E0
			public void CopyTo(KeyValuePair<string, ICollection<Vec2>>[] array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
				{
					array[arrayIndex] = keyValuePair;
					arrayIndex++;
					if (arrayIndex >= array.Length)
					{
						throw new ArgumentException("Not enough size in array.");
					}
				}
			}

			// Token: 0x06003C25 RID: 15397 RVA: 0x000F205C File Offset: 0x000F025C
			public bool Remove(KeyValuePair<string, ICollection<Vec2>> item)
			{
				return this.Remove(item.Key);
			}

			// Token: 0x17000A57 RID: 2647
			// (get) Token: 0x06003C26 RID: 15398 RVA: 0x000F206C File Offset: 0x000F026C
			public ICollection<string> Keys
			{
				get
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
					{
						list.Add(keyValuePair.Key);
					}
					return list;
				}
			}

			// Token: 0x17000A58 RID: 2648
			// (get) Token: 0x06003C27 RID: 15399 RVA: 0x000F20C4 File Offset: 0x000F02C4
			public ICollection<ICollection<Vec2>> Values
			{
				get
				{
					List<ICollection<Vec2>> list = new List<ICollection<Vec2>>();
					foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
					{
						list.Add(keyValuePair.Value);
					}
					return list;
				}
			}

			// Token: 0x17000A59 RID: 2649
			public ICollection<Vec2> this[string name]
			{
				get
				{
					if (name == null)
					{
						throw new ArgumentNullException("name");
					}
					List<Vec2> boundaryPoints = this.GetBoundaryPoints(name);
					if (boundaryPoints.Count == 0)
					{
						throw new KeyNotFoundException();
					}
					return boundaryPoints;
				}
				set
				{
					if (name == null)
					{
						throw new ArgumentNullException("name");
					}
					this.Add(name, value);
				}
			}

			// Token: 0x06003C2A RID: 15402 RVA: 0x000F2166 File Offset: 0x000F0366
			public void Add(string name, ICollection<Vec2> points)
			{
				this.Add(name, points, true);
			}

			// Token: 0x06003C2B RID: 15403 RVA: 0x000F2174 File Offset: 0x000F0374
			public void Add(string name, ICollection<Vec2> points, bool isAllowanceInside)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				if (points == null)
				{
					throw new ArgumentNullException("points");
				}
				if (points.Count < 3)
				{
					throw new ArgumentException("At least three points are required.");
				}
				bool flag = MBAPI.IMBMission.AddBoundary(this._mission.Pointer, name, points.ToArray<Vec2>(), points.Count, isAllowanceInside);
				if (!flag)
				{
					throw new ArgumentException("An element with the same name already exists.");
				}
				if (flag)
				{
					NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
					if (collectionChanged != null)
					{
						collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, name));
					}
				}
				foreach (Team team in Mission.Current.Teams)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						formation.ResetMovementOrderPositionCache();
					}
				}
			}

			// Token: 0x06003C2C RID: 15404 RVA: 0x000F2280 File Offset: 0x000F0480
			public bool ContainsKey(string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				return this.GetBoundaryPoints(name).Count > 0;
			}

			// Token: 0x06003C2D RID: 15405 RVA: 0x000F22A0 File Offset: 0x000F04A0
			public bool Remove(string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				bool flag = MBAPI.IMBMission.RemoveBoundary(this._mission.Pointer, name);
				if (flag)
				{
					NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
					if (collectionChanged != null)
					{
						collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, name));
					}
				}
				foreach (Team team in Mission.Current.Teams)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						formation.ResetMovementOrderPositionCache();
					}
				}
				return flag;
			}

			// Token: 0x06003C2E RID: 15406 RVA: 0x000F2370 File Offset: 0x000F0570
			public bool TryGetValue(string name, out ICollection<Vec2> points)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				points = this.GetBoundaryPoints(name);
				return points.Count > 0;
			}

			// Token: 0x06003C2F RID: 15407 RVA: 0x000F2394 File Offset: 0x000F0594
			private List<Vec2> GetBoundaryPoints(string name)
			{
				List<Vec2> list = new List<Vec2>();
				Vec2[] array = new Vec2[10];
				for (int i = 0; i < 1000; i += 10)
				{
					int num = -1;
					MBAPI.IMBMission.GetBoundaryPoints(this._mission.Pointer, name, i, array, 10, ref num);
					list.AddRange(array.Take<Vec2>(num));
					if (num < 10)
					{
						break;
					}
				}
				return list;
			}

			// Token: 0x140000B0 RID: 176
			// (add) Token: 0x06003C30 RID: 15408 RVA: 0x000F23F4 File Offset: 0x000F05F4
			// (remove) Token: 0x06003C31 RID: 15409 RVA: 0x000F242C File Offset: 0x000F062C
			public event NotifyCollectionChangedEventHandler CollectionChanged;

			// Token: 0x04001CD0 RID: 7376
			private readonly Mission _mission;
		}

		// Token: 0x020004F6 RID: 1270
		private enum GetNearbyAgentsAuxType
		{
			// Token: 0x04001CD3 RID: 7379
			Friend = 1,
			// Token: 0x04001CD4 RID: 7380
			Enemy,
			// Token: 0x04001CD5 RID: 7381
			All
		}

		// Token: 0x020004F7 RID: 1271
		public class DynamicallyCreatedEntity
		{
			// Token: 0x06003C32 RID: 15410 RVA: 0x000F2461 File Offset: 0x000F0661
			public DynamicallyCreatedEntity(string prefab, MissionObjectId objectId, MatrixFrame frame, ref List<MissionObjectId> childObjectIds)
			{
				this.Prefab = prefab;
				this.ObjectId = objectId;
				this.Frame = frame;
				this.ChildObjectIds = childObjectIds;
			}

			// Token: 0x04001CD6 RID: 7382
			public string Prefab;

			// Token: 0x04001CD7 RID: 7383
			public MissionObjectId ObjectId;

			// Token: 0x04001CD8 RID: 7384
			public MatrixFrame Frame;

			// Token: 0x04001CD9 RID: 7385
			public List<MissionObjectId> ChildObjectIds;
		}

		// Token: 0x020004F8 RID: 1272
		[Flags]
		[EngineStruct("Weapon_spawn_flag", true, "wsf", false)]
		public enum WeaponSpawnFlags : uint
		{
			// Token: 0x04001CDB RID: 7387
			None = 0U,
			// Token: 0x04001CDC RID: 7388
			WithHolster = 1U,
			// Token: 0x04001CDD RID: 7389
			WithoutHolster = 2U,
			// Token: 0x04001CDE RID: 7390
			AsMissile = 4U,
			// Token: 0x04001CDF RID: 7391
			WithPhysics = 8U,
			// Token: 0x04001CE0 RID: 7392
			WithStaticPhysics = 16U,
			// Token: 0x04001CE1 RID: 7393
			UseAnimationSpeed = 32U,
			// Token: 0x04001CE2 RID: 7394
			CannotBePickedUp = 64U
		}

		// Token: 0x020004F9 RID: 1273
		[EngineStruct("Mission_combat_type", false, null)]
		public enum MissionCombatType
		{
			// Token: 0x04001CE4 RID: 7396
			Combat,
			// Token: 0x04001CE5 RID: 7397
			ArenaCombat,
			// Token: 0x04001CE6 RID: 7398
			NoCombat
		}

		// Token: 0x020004FA RID: 1274
		public enum BattleSizeType
		{
			// Token: 0x04001CE8 RID: 7400
			Battle,
			// Token: 0x04001CE9 RID: 7401
			Siege,
			// Token: 0x04001CEA RID: 7402
			SallyOut
		}

		// Token: 0x020004FB RID: 1275
		[EngineStruct("Agent_creation_result", false, null)]
		internal struct AgentCreationResult
		{
			// Token: 0x04001CEB RID: 7403
			internal int Index;

			// Token: 0x04001CEC RID: 7404
			internal UIntPtr AgentPtr;

			// Token: 0x04001CED RID: 7405
			internal UIntPtr PositionPtr;

			// Token: 0x04001CEE RID: 7406
			internal UIntPtr IndexPtr;

			// Token: 0x04001CEF RID: 7407
			internal UIntPtr FlagsPtr;

			// Token: 0x04001CF0 RID: 7408
			internal UIntPtr StatePtr;

			// Token: 0x04001CF1 RID: 7409
			internal UIntPtr MovementModePointer;

			// Token: 0x04001CF2 RID: 7410
			internal UIntPtr ControllerPointer;

			// Token: 0x04001CF3 RID: 7411
			internal UIntPtr MovementDirectionPointer;

			// Token: 0x04001CF4 RID: 7412
			internal UIntPtr PrimaryWieldedItemIndexPointer;

			// Token: 0x04001CF5 RID: 7413
			internal UIntPtr OffHandWieldedItemIndexPointer;

			// Token: 0x04001CF6 RID: 7414
			internal UIntPtr Channel0CurrentActionPointer;

			// Token: 0x04001CF7 RID: 7415
			internal UIntPtr Channel1CurrentActionPointer;

			// Token: 0x04001CF8 RID: 7416
			internal UIntPtr MaximumForwardUnlimitedSpeed;
		}

		// Token: 0x020004FC RID: 1276
		public struct TimeSpeedRequest
		{
			// Token: 0x17000A5A RID: 2650
			// (get) Token: 0x06003C33 RID: 15411 RVA: 0x000F2487 File Offset: 0x000F0687
			// (set) Token: 0x06003C34 RID: 15412 RVA: 0x000F248F File Offset: 0x000F068F
			public float RequestedTimeSpeed { get; private set; }

			// Token: 0x17000A5B RID: 2651
			// (get) Token: 0x06003C35 RID: 15413 RVA: 0x000F2498 File Offset: 0x000F0698
			// (set) Token: 0x06003C36 RID: 15414 RVA: 0x000F24A0 File Offset: 0x000F06A0
			public int RequestID { get; private set; }

			// Token: 0x06003C37 RID: 15415 RVA: 0x000F24A9 File Offset: 0x000F06A9
			public TimeSpeedRequest(float requestedTime, int requestID)
			{
				this.RequestedTimeSpeed = requestedTime;
				this.RequestID = requestID;
			}
		}

		// Token: 0x020004FD RID: 1277
		public static class MissionNetworkHelper
		{
			// Token: 0x06003C38 RID: 15416 RVA: 0x000F24BC File Offset: 0x000F06BC
			public static Agent GetAgentFromIndex(int agentIndex, bool canBeNull = false)
			{
				Agent agent = Mission.Current.FindAgentWithIndex(agentIndex);
				if (!canBeNull && agent == null && agentIndex >= 0)
				{
					Debug.Print("Agent with index: " + agentIndex + " could not be found while reading reference from packet.", 0, Debug.DebugColor.White, 17592186044416UL);
					throw new MBNotFoundException("Agent with index: " + agentIndex + " could not be found while reading reference from packet.");
				}
				return agent;
			}

			// Token: 0x06003C39 RID: 15417 RVA: 0x000F2521 File Offset: 0x000F0721
			public static MBTeam GetMBTeamFromTeamIndex(int teamIndex)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				if (teamIndex < 0)
				{
					return MBTeam.InvalidTeam;
				}
				return new MBTeam(Mission.Current, teamIndex);
			}

			// Token: 0x06003C3A RID: 15418 RVA: 0x000F254C File Offset: 0x000F074C
			public static Team GetTeamFromTeamIndex(int teamIndex)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				if (teamIndex < 0)
				{
					return Team.Invalid;
				}
				MBTeam mbteamFromTeamIndex = Mission.MissionNetworkHelper.GetMBTeamFromTeamIndex(teamIndex);
				return Mission.Current.Teams.Find(mbteamFromTeamIndex);
			}

			// Token: 0x06003C3B RID: 15419 RVA: 0x000F258C File Offset: 0x000F078C
			public static MissionObject GetMissionObjectFromMissionObjectId(MissionObjectId missionObjectId)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				if (missionObjectId.Id < 0)
				{
					return null;
				}
				MissionObject missionObject = Mission.Current.MissionObjects.FirstOrDefault<MissionObject>((MissionObject mo) => mo.Id == missionObjectId);
				if (missionObject == null)
				{
					MBDebug.Print(string.Concat(new object[]
					{
						"MissionObject with ID: ",
						missionObjectId.Id,
						" runtime: ",
						missionObjectId.CreatedAtRuntime.ToString(),
						" could not be found."
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				return missionObject;
			}

			// Token: 0x06003C3C RID: 15420 RVA: 0x000F2648 File Offset: 0x000F0848
			public static CombatLogData GetCombatLogDataForCombatLogNetworkMessage(CombatLogNetworkMessage message)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(message.AttackerAgentIndex, false);
				Agent agentFromIndex2 = Mission.MissionNetworkHelper.GetAgentFromIndex(message.VictimAgentIndex, true);
				bool flag = agentFromIndex != null;
				bool flag2 = flag && agentFromIndex.IsHuman;
				bool flag3 = flag && agentFromIndex.IsMine;
				bool flag4 = flag && agentFromIndex.RiderAgent != null;
				bool flag5 = flag4 && agentFromIndex.RiderAgent.IsMine;
				bool flag6 = flag && agentFromIndex.IsMount;
				bool flag7 = agentFromIndex2 != null && agentFromIndex2.Health <= 0f;
				bool flag8 = agentFromIndex != null && ((agentFromIndex2 != null) ? agentFromIndex2.RiderAgent : null) == agentFromIndex;
				bool flag9 = agentFromIndex == agentFromIndex2;
				bool flag10 = flag2;
				bool flag11 = flag3;
				bool flag12 = flag4;
				bool flag13 = flag5;
				bool flag14 = flag6;
				bool flag15 = agentFromIndex2 != null && agentFromIndex2.IsHuman;
				bool flag16 = agentFromIndex2 != null && agentFromIndex2.IsMine;
				bool flag17 = flag7;
				bool flag18 = ((agentFromIndex2 != null) ? agentFromIndex2.RiderAgent : null) != null;
				bool? flag19;
				if (agentFromIndex2 == null)
				{
					flag19 = null;
				}
				else
				{
					Agent riderAgent = agentFromIndex2.RiderAgent;
					flag19 = ((riderAgent != null) ? new bool?(riderAgent.IsMine) : null);
				}
				CombatLogData combatLogData = new CombatLogData(flag9, flag10, flag11, flag12, flag13, flag14, flag15, flag16, flag17, flag18, flag19 ?? false, agentFromIndex2 != null && agentFromIndex2.IsMount, Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(message.MissionObjectHitId), flag8, message.CrushedThrough, message.Chamber, message.Distance);
				combatLogData.DamageType = message.DamageType;
				combatLogData.IsRangedAttack = message.IsRangedAttack;
				combatLogData.IsFriendlyFire = message.IsFriendlyFire;
				combatLogData.IsFatalDamage = message.IsFatalDamage;
				combatLogData.IsSpecialDamage = message.IsSpecialDamage;
				combatLogData.BodyPartHit = message.BodyPartHit;
				combatLogData.HitSpeed = message.HitSpeed;
				combatLogData.InflictedDamage = message.InflictedDamage;
				combatLogData.AbsorbedDamage = message.AbsorbedDamage;
				combatLogData.ModifiedDamage = message.ModifiedDamage;
				combatLogData.ReflectedDamage = message.ReflectedDamage;
				string text;
				if (agentFromIndex2 == null)
				{
					text = null;
				}
				else
				{
					MissionPeer missionPeer = agentFromIndex2.MissionPeer;
					text = ((missionPeer != null) ? missionPeer.DisplayedName : null);
				}
				string text2;
				if ((text2 = text) == null)
				{
					text2 = ((agentFromIndex2 != null) ? agentFromIndex2.Name : null) ?? "";
				}
				combatLogData.VictimAgentName = text2;
				return combatLogData;
			}
		}

		// Token: 0x020004FE RID: 1278
		public class Missile : MBMissile
		{
			// Token: 0x17000A5C RID: 2652
			// (get) Token: 0x06003C3D RID: 15421 RVA: 0x000F2874 File Offset: 0x000F0A74
			// (set) Token: 0x06003C3E RID: 15422 RVA: 0x000F287C File Offset: 0x000F0A7C
			public GameEntity Entity { get; private set; }

			// Token: 0x17000A5D RID: 2653
			// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000F2885 File Offset: 0x000F0A85
			// (set) Token: 0x06003C40 RID: 15424 RVA: 0x000F288D File Offset: 0x000F0A8D
			public MissionWeapon Weapon { get; private set; }

			// Token: 0x17000A5E RID: 2654
			// (get) Token: 0x06003C41 RID: 15425 RVA: 0x000F2896 File Offset: 0x000F0A96
			// (set) Token: 0x06003C42 RID: 15426 RVA: 0x000F289E File Offset: 0x000F0A9E
			public Agent ShooterAgent { get; private set; }

			// Token: 0x17000A5F RID: 2655
			// (get) Token: 0x06003C43 RID: 15427 RVA: 0x000F28A7 File Offset: 0x000F0AA7
			// (set) Token: 0x06003C44 RID: 15428 RVA: 0x000F28AF File Offset: 0x000F0AAF
			public MissionObject MissionObjectToIgnore { get; private set; }

			// Token: 0x17000A60 RID: 2656
			// (get) Token: 0x06003C45 RID: 15429 RVA: 0x000F28B8 File Offset: 0x000F0AB8
			// (set) Token: 0x06003C46 RID: 15430 RVA: 0x000F28C0 File Offset: 0x000F0AC0
			public GameEntity AlreadyHitEntityToIgnore { get; private set; }

			// Token: 0x06003C47 RID: 15431 RVA: 0x000F28C9 File Offset: 0x000F0AC9
			public Missile(Mission mission, int index, GameEntity entity, Agent shooterAgent, MissionWeapon weapon, MissionObject missionObjectToIgnore)
				: base(mission)
			{
				base.Index = index;
				this.Entity = entity;
				this.Weapon = weapon;
				this.ShooterAgent = shooterAgent;
				this.MissionObjectToIgnore = missionObjectToIgnore;
			}

			// Token: 0x06003C48 RID: 15432 RVA: 0x000F28F8 File Offset: 0x000F0AF8
			public void CalculatePassbySoundParametersMT(ref SoundEventParameter soundEventParameter)
			{
				if (this.Weapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield))
				{
					soundEventParameter.Update("impactModifier", 0.3f);
				}
			}

			// Token: 0x06003C49 RID: 15433 RVA: 0x000F2938 File Offset: 0x000F0B38
			public void CalculateBounceBackVelocity(Vec3 rotationSpeed, AttackCollisionData collisionData, out Vec3 velocity, out Vec3 angularVelocity)
			{
				Vec3 missileVelocity = collisionData.MissileVelocity;
				float num = (float)this.Weapon.CurrentUsageItem.WeaponLength * 0.01f * this.Weapon.Item.ScaleFactor;
				PhysicsMaterial fromIndex = PhysicsMaterial.GetFromIndex(collisionData.PhysicsMaterialIndex);
				float num2;
				float num3;
				if (fromIndex.IsValid)
				{
					num2 = fromIndex.GetDynamicFriction();
					num3 = fromIndex.GetRestitution();
				}
				else
				{
					num2 = 0.3f;
					num3 = 0.4f;
				}
				PhysicsMaterial fromName = PhysicsMaterial.GetFromName(this.Weapon.Item.PrimaryWeapon.PhysicsMaterial);
				float num4;
				float num5;
				if (fromName.IsValid)
				{
					num4 = fromName.GetDynamicFriction();
					num5 = fromName.GetRestitution();
				}
				else
				{
					num4 = 0.3f;
					num5 = 0.4f;
				}
				float num6 = (num2 + num4) * 0.5f;
				float num7 = (num3 + num5) * 0.5f;
				Vec3 vec = missileVelocity.Reflect(collisionData.CollisionGlobalNormal);
				float num8 = Vec3.DotProduct(vec, collisionData.CollisionGlobalNormal);
				Vec3 vec2 = collisionData.CollisionGlobalNormal;
				Vec3 vec3 = vec2.RotateAboutAnArbitraryVector(Vec3.CrossProduct(vec, collisionData.CollisionGlobalNormal).NormalizedCopy(), 1.5707964f);
				float num9 = Vec3.DotProduct(vec, vec3);
				velocity = collisionData.CollisionGlobalNormal * (num7 * num8) + vec3 * (num9 * num6);
				velocity += collisionData.CollisionGlobalNormal;
				angularVelocity = -Vec3.CrossProduct(collisionData.CollisionGlobalNormal, velocity);
				float lengthSquared = angularVelocity.LengthSquared;
				float weight = this.Weapon.GetWeight();
				WeaponClass weaponClass = this.Weapon.CurrentUsageItem.WeaponClass;
				float num10;
				if (weaponClass == WeaponClass.Arrow || weaponClass == WeaponClass.Bolt)
				{
					num10 = 0.25f * weight * 0.055f * 0.055f + 0.08333333f * weight * num * num;
				}
				else if (weaponClass == WeaponClass.ThrowingKnife)
				{
					num10 = 0.25f * weight * 0.2f * 0.2f + 0.08333333f * weight * num * num;
					num10 += 0.5f * weight * 0.2f * 0.2f;
					rotationSpeed * num3;
					MatrixFrame matrixFrame = this.Entity.GetGlobalFrame();
					vec2 = rotationSpeed * num3;
					angularVelocity = matrixFrame.rotation.TransformToParent(in vec2);
				}
				else if (weaponClass == WeaponClass.ThrowingAxe)
				{
					num10 = 0.25f * weight * 0.2f * 0.2f + 0.08333333f * weight * num * num;
					num10 += 0.5f * weight * 0.2f * 0.2f;
					rotationSpeed * num3;
					MatrixFrame matrixFrame = this.Entity.GetGlobalFrame();
					vec2 = rotationSpeed * num3;
					angularVelocity = matrixFrame.rotation.TransformToParent(in vec2);
				}
				else if (weaponClass == WeaponClass.Javelin)
				{
					num10 = 0.25f * weight * 0.155f * 0.155f + 0.08333333f * weight * num * num;
				}
				else if (weaponClass == WeaponClass.Stone || weaponClass == WeaponClass.BallistaStone || weaponClass == WeaponClass.SlingStone)
				{
					num10 = 0.4f * weight * 0.1f * 0.1f;
				}
				else if (weaponClass == WeaponClass.Boulder || weaponClass == WeaponClass.BallistaBoulder)
				{
					num10 = 0.4f * weight * 0.4f * 0.4f;
				}
				else
				{
					Debug.FailedAssert("Unknown missile type!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "CalculateBounceBackVelocity", 273);
					num10 = 0f;
				}
				float num11 = 0.5f * num10 * lengthSquared;
				float length = missileVelocity.Length;
				float num12 = MathF.Sqrt((0.5f * weight * length * length - num11) * 2f / weight);
				velocity *= num12 / length;
				float maximumValue = CompressionMission.SpawnedItemVelocityCompressionInfo.GetMaximumValue();
				float maximumValue2 = CompressionMission.SpawnedItemAngularVelocityCompressionInfo.GetMaximumValue();
				if (velocity.LengthSquared > maximumValue * maximumValue)
				{
					velocity = velocity.NormalizedCopy() * maximumValue;
				}
				if (angularVelocity.LengthSquared > maximumValue2 * maximumValue2)
				{
					angularVelocity = angularVelocity.NormalizedCopy() * maximumValue2;
				}
			}

			// Token: 0x06003C4A RID: 15434 RVA: 0x000F2D68 File Offset: 0x000F0F68
			public void PassThroughEntity(GameEntity entity)
			{
				this.AlreadyHitEntityToIgnore = entity;
				Vec3 vec = base.GetVelocity() * 0.8f;
				base.SetVelocity(in vec);
			}
		}

		// Token: 0x020004FF RID: 1279
		public struct SpectatorData
		{
			// Token: 0x17000A61 RID: 2657
			// (get) Token: 0x06003C4B RID: 15435 RVA: 0x000F2D95 File Offset: 0x000F0F95
			// (set) Token: 0x06003C4C RID: 15436 RVA: 0x000F2D9D File Offset: 0x000F0F9D
			public Agent AgentToFollow { get; private set; }

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06003C4D RID: 15437 RVA: 0x000F2DA6 File Offset: 0x000F0FA6
			// (set) Token: 0x06003C4E RID: 15438 RVA: 0x000F2DAE File Offset: 0x000F0FAE
			public IAgentVisual AgentVisualToFollow { get; private set; }

			// Token: 0x17000A63 RID: 2659
			// (get) Token: 0x06003C4F RID: 15439 RVA: 0x000F2DB7 File Offset: 0x000F0FB7
			// (set) Token: 0x06003C50 RID: 15440 RVA: 0x000F2DBF File Offset: 0x000F0FBF
			public SpectatorCameraTypes CameraType { get; private set; }

			// Token: 0x06003C51 RID: 15441 RVA: 0x000F2DC8 File Offset: 0x000F0FC8
			public SpectatorData(Agent agentToFollow, IAgentVisual agentVisualToFollow, SpectatorCameraTypes cameraType)
			{
				this.AgentToFollow = agentToFollow;
				this.CameraType = cameraType;
				this.AgentVisualToFollow = agentVisualToFollow;
			}
		}

		// Token: 0x02000500 RID: 1280
		private class DynamicEntityInfo
		{
			// Token: 0x04001D03 RID: 7427
			public GameEntity Entity;

			// Token: 0x04001D04 RID: 7428
			public Timer TimerToDisable;
		}

		// Token: 0x02000501 RID: 1281
		public enum State
		{
			// Token: 0x04001D06 RID: 7430
			NewlyCreated,
			// Token: 0x04001D07 RID: 7431
			Initializing,
			// Token: 0x04001D08 RID: 7432
			Continuing,
			// Token: 0x04001D09 RID: 7433
			EndingNextFrame,
			// Token: 0x04001D0A RID: 7434
			Over
		}

		// Token: 0x02000502 RID: 1282
		public enum BattleSizeQualifier
		{
			// Token: 0x04001D0C RID: 7436
			Small,
			// Token: 0x04001D0D RID: 7437
			Medium
		}

		// Token: 0x02000503 RID: 1283
		public enum MissionTeamAITypeEnum
		{
			// Token: 0x04001D0F RID: 7439
			NoTeamAI,
			// Token: 0x04001D10 RID: 7440
			FieldBattle,
			// Token: 0x04001D11 RID: 7441
			Siege,
			// Token: 0x04001D12 RID: 7442
			SallyOut,
			// Token: 0x04001D13 RID: 7443
			NavalBattle,
			// Token: 0x04001D14 RID: 7444
			NavalRaid
		}

		// Token: 0x02000504 RID: 1284
		public enum MissileCollisionReaction
		{
			// Token: 0x04001D16 RID: 7446
			Invalid = -1,
			// Token: 0x04001D17 RID: 7447
			Stick,
			// Token: 0x04001D18 RID: 7448
			PassThrough,
			// Token: 0x04001D19 RID: 7449
			BounceBack,
			// Token: 0x04001D1A RID: 7450
			BecomeInvisible,
			// Token: 0x04001D1B RID: 7451
			Count
		}

		// Token: 0x02000505 RID: 1285
		public enum MissionTickAction
		{
			// Token: 0x04001D1D RID: 7453
			TryToSheathWeaponInHand,
			// Token: 0x04001D1E RID: 7454
			RemoveEquippedWeapon,
			// Token: 0x04001D1F RID: 7455
			TryToWieldWeaponInSlot,
			// Token: 0x04001D20 RID: 7456
			DropItem,
			// Token: 0x04001D21 RID: 7457
			RegisterDrownBlow,
			// Token: 0x04001D22 RID: 7458
			RegisterBurnBlow
		}

		// Token: 0x02000506 RID: 1286
		// (Invoke) Token: 0x06003C54 RID: 15444
		public delegate void OnBeforeAgentRemovedDelegate(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow);

		// Token: 0x02000507 RID: 1287
		// (Invoke) Token: 0x06003C58 RID: 15448
		public delegate void OnAddSoundAlarmFactorToAgentsDelegate(Agent alarmCreatorAgent, in Vec3 soundPosition, float soundLevelSquareRoot);

		// Token: 0x02000508 RID: 1288
		// (Invoke) Token: 0x06003C5C RID: 15452
		public delegate void OnMainAgentChangedDelegate(Agent oldAgent);

		// Token: 0x02000509 RID: 1289
		// (Invoke) Token: 0x06003C60 RID: 15456
		public delegate void OnCameraShakeTriggeredDelegate(in Vec3 position, float radius);

		// Token: 0x0200050A RID: 1290
		// (Invoke) Token: 0x06003C64 RID: 15460
		public delegate BodyProperties ComputeTroopBodyPropertiesDelegate(AgentBuildData agentBuildData, BasicCharacterObject characterObject, Equipment equipment, int seed);

		// Token: 0x0200050B RID: 1291
		public sealed class TeamCollection : List<Team>
		{
			// Token: 0x140000B1 RID: 177
			// (add) Token: 0x06003C67 RID: 15463 RVA: 0x000F2DE8 File Offset: 0x000F0FE8
			// (remove) Token: 0x06003C68 RID: 15464 RVA: 0x000F2E20 File Offset: 0x000F1020
			public event Action<Team, Team> OnPlayerTeamChanged;

			// Token: 0x17000A64 RID: 2660
			// (get) Token: 0x06003C69 RID: 15465 RVA: 0x000F2E55 File Offset: 0x000F1055
			// (set) Token: 0x06003C6A RID: 15466 RVA: 0x000F2E5D File Offset: 0x000F105D
			public Team Attacker { get; private set; }

			// Token: 0x17000A65 RID: 2661
			// (get) Token: 0x06003C6B RID: 15467 RVA: 0x000F2E66 File Offset: 0x000F1066
			// (set) Token: 0x06003C6C RID: 15468 RVA: 0x000F2E6E File Offset: 0x000F106E
			public Team Defender { get; private set; }

			// Token: 0x17000A66 RID: 2662
			// (get) Token: 0x06003C6D RID: 15469 RVA: 0x000F2E77 File Offset: 0x000F1077
			// (set) Token: 0x06003C6E RID: 15470 RVA: 0x000F2E7F File Offset: 0x000F107F
			public Team AttackerAlly { get; private set; }

			// Token: 0x17000A67 RID: 2663
			// (get) Token: 0x06003C6F RID: 15471 RVA: 0x000F2E88 File Offset: 0x000F1088
			// (set) Token: 0x06003C70 RID: 15472 RVA: 0x000F2E90 File Offset: 0x000F1090
			public Team DefenderAlly { get; private set; }

			// Token: 0x17000A68 RID: 2664
			// (get) Token: 0x06003C71 RID: 15473 RVA: 0x000F2E99 File Offset: 0x000F1099
			// (set) Token: 0x06003C72 RID: 15474 RVA: 0x000F2EA1 File Offset: 0x000F10A1
			public Team Spectator { get; private set; }

			// Token: 0x17000A69 RID: 2665
			// (get) Token: 0x06003C73 RID: 15475 RVA: 0x000F2EAA File Offset: 0x000F10AA
			// (set) Token: 0x06003C74 RID: 15476 RVA: 0x000F2EB2 File Offset: 0x000F10B2
			public Team Player
			{
				get
				{
					return this._playerTeam;
				}
				set
				{
					if (this._playerTeam != value)
					{
						this.SetPlayerTeamAux((value == null) ? (-1) : base.IndexOf(value));
					}
				}
			}

			// Token: 0x17000A6A RID: 2666
			// (get) Token: 0x06003C75 RID: 15477 RVA: 0x000F2ED0 File Offset: 0x000F10D0
			// (set) Token: 0x06003C76 RID: 15478 RVA: 0x000F2ED8 File Offset: 0x000F10D8
			public Team PlayerEnemy { get; private set; }

			// Token: 0x17000A6B RID: 2667
			// (get) Token: 0x06003C77 RID: 15479 RVA: 0x000F2EE1 File Offset: 0x000F10E1
			// (set) Token: 0x06003C78 RID: 15480 RVA: 0x000F2EE9 File Offset: 0x000F10E9
			public Team PlayerAlly { get; private set; }

			// Token: 0x06003C79 RID: 15481 RVA: 0x000F2EF2 File Offset: 0x000F10F2
			public TeamCollection(Mission mission)
				: base(new List<Team>())
			{
				this._mission = mission;
			}

			// Token: 0x06003C7A RID: 15482 RVA: 0x000F2F06 File Offset: 0x000F1106
			private MBTeam AddNative()
			{
				return new MBTeam(this._mission, MBAPI.IMBMission.AddTeam(this._mission.Pointer));
			}

			// Token: 0x06003C7B RID: 15483 RVA: 0x000F2F28 File Offset: 0x000F1128
			public new void Add(Team t)
			{
				MBDebug.ShowWarning("Pre-created Team can not be added to TeamCollection!");
			}

			// Token: 0x06003C7C RID: 15484 RVA: 0x000F2F34 File Offset: 0x000F1134
			public Team Add(BattleSideEnum side, uint color = 4294967295U, uint color2 = 4294967295U, Banner banner = null, bool isPlayerGeneral = true, bool isPlayerSergeant = false, bool isSettingRelations = true)
			{
				MBDebug.Print("----------Mission-AddTeam-" + side, 0, Debug.DebugColor.White, 17592186044416UL);
				Team team = new Team(this.AddNative(), side, this._mission, color, color2, banner);
				if (!GameNetwork.IsClientOrReplay)
				{
					team.SetPlayerRole(isPlayerGeneral, isPlayerSergeant);
				}
				base.Add(team);
				if (side == BattleSideEnum.None && this.Spectator == null)
				{
					this.Spectator = team;
				}
				foreach (MissionBehavior missionBehavior in this._mission.MissionBehaviors)
				{
					missionBehavior.OnAddTeam(team);
				}
				if (isSettingRelations)
				{
					this.SetRelations(team);
				}
				if (side == BattleSideEnum.Attacker)
				{
					if (this.Attacker == null)
					{
						this.Attacker = team;
					}
					else if (this.AttackerAlly == null)
					{
						this.AttackerAlly = team;
					}
				}
				else if (side == BattleSideEnum.Defender)
				{
					if (this.Defender == null)
					{
						this.Defender = team;
					}
					else if (this.DefenderAlly == null)
					{
						this.DefenderAlly = team;
					}
				}
				this.AdjustPlayerTeams();
				foreach (MissionBehavior missionBehavior2 in this._mission.MissionBehaviors)
				{
					missionBehavior2.AfterAddTeam(team);
				}
				return team;
			}

			// Token: 0x06003C7D RID: 15485 RVA: 0x000F3090 File Offset: 0x000F1290
			public Team Find(MBTeam mbTeam)
			{
				if (mbTeam.IsValid)
				{
					for (int i = 0; i < base.Count; i++)
					{
						Team team = base[i];
						if (team.MBTeam == mbTeam)
						{
							return team;
						}
					}
				}
				return Team.Invalid;
			}

			// Token: 0x06003C7E RID: 15486 RVA: 0x000F30D4 File Offset: 0x000F12D4
			public void ClearResources()
			{
				this.Attacker = null;
				this.AttackerAlly = null;
				this.Defender = null;
				this.DefenderAlly = null;
				this.Spectator = null;
				this._playerTeam = null;
				this.PlayerEnemy = null;
				this.PlayerAlly = null;
				Team.Invalid = null;
			}

			// Token: 0x06003C7F RID: 15487 RVA: 0x000F3114 File Offset: 0x000F1314
			public new void Clear()
			{
				foreach (Team team in this)
				{
					team.Clear();
				}
				base.Clear();
				this.ClearResources();
				MBAPI.IMBMission.ResetTeams(this._mission.Pointer);
			}

			// Token: 0x06003C80 RID: 15488 RVA: 0x000F3180 File Offset: 0x000F1380
			private void SetRelations(Team team)
			{
				BattleSideEnum side = team.Side;
				for (int i = 0; i < base.Count; i++)
				{
					Team team2 = base[i];
					if (side.IsOpponentOf(team2.Side))
					{
						team.SetIsEnemyOf(team2, true);
					}
				}
			}

			// Token: 0x06003C81 RID: 15489 RVA: 0x000F31C4 File Offset: 0x000F13C4
			private void SetPlayerTeamAux(int index)
			{
				Team playerTeam = this._playerTeam;
				this._playerTeam = ((index == -1) ? null : base[index]);
				this.AdjustPlayerTeams();
				Action<Team, Team> onPlayerTeamChanged = this.OnPlayerTeamChanged;
				if (onPlayerTeamChanged == null)
				{
					return;
				}
				onPlayerTeamChanged(playerTeam, this._playerTeam);
			}

			// Token: 0x06003C82 RID: 15490 RVA: 0x000F320C File Offset: 0x000F140C
			private void AdjustPlayerTeams()
			{
				if (this.Player == null)
				{
					this.PlayerEnemy = null;
					this.PlayerAlly = null;
					return;
				}
				if (this.Player != this.Attacker)
				{
					if (this.Player == this.Defender)
					{
						if (this.Attacker != null && this.Player.IsEnemyOf(this.Attacker))
						{
							this.PlayerEnemy = this.Attacker;
						}
						else
						{
							this.PlayerEnemy = null;
						}
						if (this.DefenderAlly != null && this.Player.IsFriendOf(this.DefenderAlly))
						{
							this.PlayerAlly = this.DefenderAlly;
							return;
						}
						this.PlayerAlly = null;
					}
					return;
				}
				if (this.Defender != null && this.Player.IsEnemyOf(this.Defender))
				{
					this.PlayerEnemy = this.Defender;
				}
				else
				{
					this.PlayerEnemy = null;
				}
				if (this.AttackerAlly != null && this.Player.IsFriendOf(this.AttackerAlly))
				{
					this.PlayerAlly = this.AttackerAlly;
					return;
				}
				this.PlayerAlly = null;
			}

			// Token: 0x17000A6C RID: 2668
			// (get) Token: 0x06003C83 RID: 15491 RVA: 0x000F330B File Offset: 0x000F150B
			private int TeamCountNative
			{
				get
				{
					return MBAPI.IMBMission.GetNumberOfTeams(this._mission.Pointer);
				}
			}

			// Token: 0x04001D24 RID: 7460
			private Mission _mission;

			// Token: 0x04001D2A RID: 7466
			private Team _playerTeam;
		}
	}
}
