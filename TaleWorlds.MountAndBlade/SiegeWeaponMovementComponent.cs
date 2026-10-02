using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035D RID: 861
	public class SiegeWeaponMovementComponent : UsableMissionObjectComponent
	{
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003189 RID: 12681 RVA: 0x000C925F File Offset: 0x000C745F
		public bool HasApproachedTarget
		{
			get
			{
				return !this._pathTracker.PathExists() || this._pathTracker.PathTraveledPercentage > 0.7f;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x000C9282 File Offset: 0x000C7482
		// (set) Token: 0x0600318B RID: 12683 RVA: 0x000C928A File Offset: 0x000C748A
		public Vec3 Velocity { get; private set; }

		// Token: 0x0600318C RID: 12684 RVA: 0x000C9294 File Offset: 0x000C7494
		protected internal override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			this._path = scene.GetPathWithName(this.PathEntityName);
			MatrixFrame matrixFrame = this.MainObject.GameEntity.GetFrame();
			Vec3 scaleVector = matrixFrame.rotation.GetScaleVector();
			this._wheels = GameEntity.CreateFromWeakEntity(this.MainObject.GameEntity).CollectChildrenEntitiesWithTag("wheel");
			this._standingPoints = this.MainObject.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPoint>("move");
			this._pathTracker = new PathTracker(this._path, scaleVector);
			this._pathTracker.Reset();
			this.SetTargetFrame();
			MatrixFrame globalFrame = this.MainObject.GameEntity.GetGlobalFrame();
			this._standingPointLocalIKFrames = new MatrixFrame[this._standingPoints.Count];
			for (int i = 0; i < this._standingPoints.Count; i++)
			{
				MatrixFrame[] standingPointLocalIKFrames = this._standingPointLocalIKFrames;
				int num = i;
				matrixFrame = this._standingPoints[i].GameEntity.GetGlobalFrame();
				standingPointLocalIKFrames[num] = matrixFrame.TransformToLocal(in globalFrame);
				this._standingPoints[i].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
			}
			this.Velocity = Vec3.Zero;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000C93D0 File Offset: 0x000C75D0
		public void HighlightPath()
		{
			MatrixFrame[] array = new MatrixFrame[this._path.NumberOfPoints];
			this._path.GetPoints(array);
			for (int i = 1; i < this._path.NumberOfPoints; i++)
			{
				MatrixFrame matrixFrame = array[i];
			}
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000C9420 File Offset: 0x000C7620
		public void SetupGhostEntity()
		{
			Path pathWithName = this.MainObject.Scene.GetPathWithName(this.PathEntityName);
			Vec3 scaleVector = this.MainObject.GameEntity.GetFrame().rotation.GetScaleVector();
			this._pathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostEntityPathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostObjectPos = ((pathWithName != null) ? pathWithName.GetTotalLength() : 0f);
			this._wheels = GameEntity.CreateFromWeakEntity(this.MainObject.GameEntity).CollectChildrenEntitiesWithTag("wheel");
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x000C94BB File Offset: 0x000C76BB
		public bool HasArrivedAtTarget
		{
			get
			{
				return !this._pathTracker.PathExists() || this._pathTracker.HasReachedEnd;
			}
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000C94D8 File Offset: 0x000C76D8
		private void SetPath()
		{
			Path pathWithName = this.MainObject.Scene.GetPathWithName(this.PathEntityName);
			Vec3 scaleVector = this.MainObject.GameEntity.GetFrame().rotation.GetScaleVector();
			this._pathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostEntityPathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostObjectPos = ((pathWithName != null) ? pathWithName.GetTotalLength() : 0f);
			this.UpdateGhostObject(0f);
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003191 RID: 12689 RVA: 0x000C955E File Offset: 0x000C775E
		// (set) Token: 0x06003192 RID: 12690 RVA: 0x000C9566 File Offset: 0x000C7766
		public float CurrentSpeed { get; private set; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x000C956F File Offset: 0x000C776F
		// (set) Token: 0x06003194 RID: 12692 RVA: 0x000C9577 File Offset: 0x000C7777
		public int MovementSoundCodeID { get; set; }

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x000C9580 File Offset: 0x000C7780
		// (set) Token: 0x06003196 RID: 12694 RVA: 0x000C9588 File Offset: 0x000C7788
		public float MinSpeed { get; set; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x000C9591 File Offset: 0x000C7791
		// (set) Token: 0x06003198 RID: 12696 RVA: 0x000C9599 File Offset: 0x000C7799
		public float MaxSpeed { get; set; }

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x000C95A2 File Offset: 0x000C77A2
		// (set) Token: 0x0600319A RID: 12698 RVA: 0x000C95AA File Offset: 0x000C77AA
		public string PathEntityName { get; set; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600319B RID: 12699 RVA: 0x000C95B3 File Offset: 0x000C77B3
		// (set) Token: 0x0600319C RID: 12700 RVA: 0x000C95BB File Offset: 0x000C77BB
		public float GhostEntitySpeedMultiplier { get; set; }

		// Token: 0x17000946 RID: 2374
		// (set) Token: 0x0600319D RID: 12701 RVA: 0x000C95C4 File Offset: 0x000C77C4
		public float WheelDiameter
		{
			set
			{
				this._wheelDiameter = value;
				this._wheelCircumference = this._wheelDiameter * 3.1415927f;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x000C95DF File Offset: 0x000C77DF
		// (set) Token: 0x0600319F RID: 12703 RVA: 0x000C95E7 File Offset: 0x000C77E7
		public SynchedMissionObject MainObject { get; set; }

		// Token: 0x060031A0 RID: 12704 RVA: 0x000C95F0 File Offset: 0x000C77F0
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.UpdateGhostObject(dt);
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000C9600 File Offset: 0x000C7800
		public void SetGhostVisibility(bool isVisible)
		{
			this.MainObject.GameEntity.CollectChildrenEntitiesWithTag("ghost_object").FirstOrDefault<WeakGameEntity>().SetVisibilityExcludeParents(isVisible);
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000C9633 File Offset: 0x000C7833
		public void OnEditorInit()
		{
			this.SetPath();
			this._wheels = GameEntity.CreateFromWeakEntity(this.MainObject.GameEntity).CollectChildrenEntitiesWithTag("wheel");
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000C965C File Offset: 0x000C785C
		private void UpdateGhostObject(float dt)
		{
			if (this._pathTracker.HasChanged)
			{
				this.SetPath();
				this._pathTracker.Advance(this._pathTracker.GetPathLength());
				this._ghostEntityPathTracker.Advance(this._ghostEntityPathTracker.GetPathLength());
			}
			List<WeakGameEntity> list = this.MainObject.GameEntity.CollectChildrenEntitiesWithTag("ghost_object");
			if (this.MainObject.GameEntity.IsSelectedOnEditor())
			{
				if (this._pathTracker.IsValid)
				{
					float num = 10f;
					if (Input.DebugInput.IsShiftDown())
					{
						num = 1f;
					}
					if (Input.DebugInput.IsKeyDown(InputKey.MouseScrollUp))
					{
						this._ghostObjectPos += dt * num;
					}
					else if (Input.DebugInput.IsKeyDown(InputKey.MouseScrollDown))
					{
						this._ghostObjectPos -= dt * num;
					}
					this._ghostObjectPos = MBMath.ClampFloat(this._ghostObjectPos, 0f, this._pathTracker.GetPathLength());
				}
				else
				{
					this._ghostObjectPos = 0f;
				}
			}
			if (list.Count > 0)
			{
				WeakGameEntity weakGameEntity = list[0];
				IPathHolder pathHolder;
				if ((pathHolder = this.MainObject as IPathHolder) != null && pathHolder.EditorGhostEntityMove)
				{
					if (this._ghostEntityPathTracker.IsValid)
					{
						this._ghostEntityPathTracker.Advance(0.05f * this.GhostEntitySpeedMultiplier);
						MatrixFrame matrixFrame = this.LinearInterpolatedIK(ref this._ghostEntityPathTracker);
						weakGameEntity.SetGlobalFrame(in matrixFrame, true);
						if (this._ghostEntityPathTracker.HasReachedEnd)
						{
							this._ghostEntityPathTracker.Reset();
							return;
						}
					}
				}
				else if (this._pathTracker.IsValid)
				{
					this._pathTracker.Advance(this._ghostObjectPos);
					MatrixFrame matrixFrame2 = this.LinearInterpolatedIK(ref this._pathTracker);
					MatrixFrame matrixFrame3 = this.FindGroundFrameForWheels(ref matrixFrame2);
					weakGameEntity.SetGlobalFrame(in matrixFrame3, true);
					this._pathTracker.Reset();
				}
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000C9840 File Offset: 0x000C7A40
		private void RotateWheels(float angleInRadian)
		{
			foreach (GameEntity gameEntity in this._wheels)
			{
				MatrixFrame frame = gameEntity.GetFrame();
				frame.rotation.RotateAboutSide(angleInRadian);
				gameEntity.SetFrame(ref frame, true);
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000C98A8 File Offset: 0x000C7AA8
		private MatrixFrame LinearInterpolatedIK(ref PathTracker pathTracker)
		{
			MatrixFrame matrixFrame;
			Vec3 vec;
			pathTracker.CurrentFrameAndColor(out matrixFrame, out vec);
			MatrixFrame matrixFrame2 = this.FindGroundFrameForWheels(ref matrixFrame);
			return MatrixFrame.Lerp(in matrixFrame, in matrixFrame2, vec.x);
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x000C98D8 File Offset: 0x000C7AD8
		public void SetDistanceTraveledAsClient(float distance)
		{
			this._advancementError = distance - this._pathTracker.TotalDistanceTraveled;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000C98ED File Offset: 0x000C7AED
		public override bool IsOnTickRequired()
		{
			return true;
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000C98F0 File Offset: 0x000C7AF0
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._ghostEntityPathTracker != null)
			{
				this.UpdateGhostObject(dt);
			}
			if (!this._pathTracker.PathExists() || this._pathTracker.HasReachedEnd)
			{
				this.CurrentSpeed = 0f;
				if (!GameNetwork.IsClientOrReplay)
				{
					foreach (StandingPoint standingPoint in this._standingPoints)
					{
						standingPoint.SetIsDeactivatedSynched(true);
					}
				}
			}
			this.TickSound();
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000C998C File Offset: 0x000C7B8C
		public void TickParallelManually(float dt)
		{
			if (this._pathTracker.PathExists() && !this._pathTracker.HasReachedEnd)
			{
				int num = 0;
				foreach (StandingPoint standingPoint in this._standingPoints)
				{
					if (standingPoint.HasUser && !standingPoint.UserAgent.IsInBeingStruckAction)
					{
						num++;
					}
				}
				if (num > 0)
				{
					int count = this._standingPoints.Count;
					this.CurrentSpeed = MBMath.Lerp(this.MinSpeed, this.MaxSpeed, (float)(num - 1) / (float)(count - 1), 1E-05f);
					MatrixFrame globalFrame = this.MainObject.GameEntity.GetGlobalFrame();
					for (int i = 0; i < this._standingPoints.Count; i++)
					{
						StandingPoint standingPoint2 = this._standingPoints[i];
						if (standingPoint2.HasUser)
						{
							Agent userAgent = standingPoint2.UserAgent;
							ActionIndexCache actionIndexCache = userAgent.GetCurrentAction(0);
							ActionIndexCache actionIndexCache2 = userAgent.GetCurrentAction(1);
							if (actionIndexCache != ActionIndexCache.act_usage_siege_machine_push)
							{
								if (userAgent.SetActionChannel(0, in ActionIndexCache.act_usage_siege_machine_push, false, (AnimFlags)0UL, 0f, this.CurrentSpeed, MBAnimation.GetAnimationBlendInPeriod(MBActionSet.GetAnimationIndexOfAction(userAgent.ActionSet, in ActionIndexCache.act_usage_siege_machine_push)) * this.CurrentSpeed, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache = ActionIndexCache.act_usage_siege_machine_push;
								}
								else if (MBMath.IsBetween((int)userAgent.GetCurrentActionType(0), 48, 52) && actionIndexCache != ActionIndexCache.act_strike_bent_over && userAgent.SetActionChannel(0, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache = ActionIndexCache.act_strike_bent_over;
								}
							}
							if (actionIndexCache2 != ActionIndexCache.act_usage_siege_machine_push)
							{
								if (userAgent.SetActionChannel(1, in ActionIndexCache.act_usage_siege_machine_push, false, (AnimFlags)0UL, 0f, this.CurrentSpeed, MBAnimation.GetAnimationBlendInPeriod(MBActionSet.GetAnimationIndexOfAction(userAgent.ActionSet, in ActionIndexCache.act_usage_siege_machine_push)) * this.CurrentSpeed, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache2 = ActionIndexCache.act_usage_siege_machine_push;
								}
								else if (MBMath.IsBetween((int)userAgent.GetCurrentActionType(1), 48, 52) && actionIndexCache2 != ActionIndexCache.act_strike_bent_over && userAgent.SetActionChannel(1, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache2 = ActionIndexCache.act_strike_bent_over;
								}
							}
							if (actionIndexCache == ActionIndexCache.act_usage_siege_machine_push)
							{
								userAgent.SetCurrentActionSpeed(0, this.CurrentSpeed);
							}
							if (actionIndexCache2 == ActionIndexCache.act_usage_siege_machine_push)
							{
								userAgent.SetCurrentActionSpeed(1, this.CurrentSpeed);
							}
							if ((actionIndexCache == ActionIndexCache.act_usage_siege_machine_push || actionIndexCache == ActionIndexCache.act_strike_bent_over) && (actionIndexCache2 == ActionIndexCache.act_usage_siege_machine_push || actionIndexCache2 == ActionIndexCache.act_strike_bent_over))
							{
								standingPoint2.UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
							}
							else
							{
								standingPoint2.UserAgent.ClearHandInverseKinematics();
								if (!GameNetwork.IsClientOrReplay && userAgent.Controller != AgentControllerType.AI)
								{
									userAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
						}
					}
				}
				else
				{
					this.CurrentSpeed = this._advancementError;
				}
				if (!this.CurrentSpeed.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					float num2 = this.CurrentSpeed * dt;
					if (!this._advancementError.ApproximatelyEqualsTo(0f, 1E-05f))
					{
						float num3 = 3f * this.CurrentSpeed * dt * (float)MathF.Sign(this._advancementError);
						if (MathF.Abs(num3) >= MathF.Abs(this._advancementError))
						{
							num3 = this._advancementError;
							this._advancementError = 0f;
						}
						else
						{
							this._advancementError -= num3;
						}
						num2 += num3;
					}
					this._pathTracker.Advance(num2);
					this.SetTargetFrame();
					float num4 = num2 / this._wheelCircumference * 2f * 3.1415927f;
					this.RotateWheels(num4);
					if (GameNetwork.IsServerOrRecorder && this._pathTracker.TotalDistanceTraveled - this._lastSynchronizedDistance > 1f)
					{
						this._lastSynchronizedDistance = this._pathTracker.TotalDistanceTraveled;
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetSiegeMachineMovementDistance(this.MainObject.Id, this._lastSynchronizedDistance));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
				}
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000C9E24 File Offset: 0x000C8024
		public MatrixFrame GetInitialFrame()
		{
			PathTracker pathTracker = new PathTracker(this._path, Vec3.One);
			pathTracker.Reset();
			return this.LinearInterpolatedIK(ref pathTracker);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000C9E50 File Offset: 0x000C8050
		private void SetTargetFrame()
		{
			if (!this._pathTracker.PathExists())
			{
				return;
			}
			MatrixFrame matrixFrame = this.LinearInterpolatedIK(ref this._pathTracker);
			WeakGameEntity gameEntity = this.MainObject.GameEntity;
			this.Velocity = gameEntity.GlobalPosition;
			gameEntity.SetGlobalFrame(in matrixFrame, false);
			this.Velocity = (gameEntity.GlobalPosition - this.Velocity).NormalizedCopy() * this.CurrentSpeed;
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000C9EC8 File Offset: 0x000C80C8
		public MatrixFrame GetTargetFrame()
		{
			float totalDistanceTraveled = this._pathTracker.TotalDistanceTraveled;
			this._pathTracker.Advance(1000000f);
			MatrixFrame currentFrame = this._pathTracker.CurrentFrame;
			this._pathTracker.Reset();
			this._pathTracker.Advance(totalDistanceTraveled);
			return currentFrame;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000C9F13 File Offset: 0x000C8113
		public void SetDestinationNavMeshIdState(bool enabled)
		{
			if (this.NavMeshIdToDisableOnDestination != -1)
			{
				Mission.Current.Scene.SetAbilityOfFacesWithId(this.NavMeshIdToDisableOnDestination, enabled);
			}
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000C9F38 File Offset: 0x000C8138
		public void MoveToTargetAsClient()
		{
			if (this._pathTracker.IsValid)
			{
				float totalDistanceTraveled = this._pathTracker.TotalDistanceTraveled;
				this._pathTracker.Advance(1000000f);
				this.SetTargetFrame();
				float num = (this._pathTracker.TotalDistanceTraveled - totalDistanceTraveled) / this._wheelCircumference * 2f * 3.1415927f;
				this.RotateWheels(num);
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000C9F9C File Offset: 0x000C819C
		private void TickSound()
		{
			if (this.CurrentSpeed > 0f)
			{
				this.PlayMovementSound();
				return;
			}
			this.StopMovementSound();
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000C9FB8 File Offset: 0x000C81B8
		private void PlayMovementSound()
		{
			if (!this._isMoveSoundPlaying)
			{
				this._movementSound = SoundEvent.CreateEvent(this.MovementSoundCodeID, this.MainObject.GameEntity.Scene);
				this._movementSound.Play();
				this._isMoveSoundPlaying = true;
			}
			this._movementSound.SetPosition(this.MainObject.GameEntity.GlobalPosition);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000CA022 File Offset: 0x000C8222
		private void StopMovementSound()
		{
			if (this._isMoveSoundPlaying)
			{
				this._movementSound.Stop();
				this._isMoveSoundPlaying = false;
			}
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000CA03E File Offset: 0x000C823E
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.CurrentSpeed = 0f;
			this._lastSynchronizedDistance = 0f;
			this._advancementError = 0f;
			this._pathTracker.Reset();
			this.SetTargetFrame();
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000CA078 File Offset: 0x000C8278
		public float GetTotalDistanceTraveledForPathTracker()
		{
			return this._pathTracker.TotalDistanceTraveled;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000CA085 File Offset: 0x000C8285
		private MatrixFrame FindGroundFrameForWheels(ref MatrixFrame frame)
		{
			return SiegeWeaponMovementComponent.FindGroundFrameForWheelsStatic(ref frame, this.AxleLength, this._wheelDiameter, this.MainObject.GameEntity, this._wheels, this.MainObject.Scene);
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000CA0B5 File Offset: 0x000C82B5
		public void SetTotalDistanceTraveledForPathTracker(float distanceTraveled)
		{
			this._pathTracker.TotalDistanceTraveled = distanceTraveled;
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000CA0C3 File Offset: 0x000C82C3
		public void SetTargetFrameForPathTracker()
		{
			this.SetTargetFrame();
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000CA0CC File Offset: 0x000C82CC
		public static MatrixFrame FindGroundFrameForWheelsStatic(ref MatrixFrame frame, float axleLength, float wheelDiameter, WeakGameEntity gameEntity, List<GameEntity> wheels, Scene scene)
		{
			Vec3.StackArray8Vec3 stackArray8Vec = default(Vec3.StackArray8Vec3);
			bool visibilityExcludeParents = gameEntity.GetVisibilityExcludeParents();
			if (visibilityExcludeParents)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
			int num = 0;
			using (new TWSharedMutexReadLock(Scene.PhysicsAndRayCastLock))
			{
				foreach (GameEntity gameEntity2 in wheels)
				{
					Vec3 vec = frame.TransformToParent(in gameEntity2.GetFrame().origin);
					Vec3 vec2 = vec + frame.rotation.s * axleLength + (wheelDiameter * 0.5f + 0.5f) * frame.rotation.u;
					Vec3 vec3 = vec - frame.rotation.s * axleLength + (wheelDiameter * 0.5f + 0.5f) * frame.rotation.u;
					vec2.z = scene.GetGroundHeightAtPosition(vec2, BodyFlags.CommonCollisionExcludeFlags);
					vec3.z = scene.GetGroundHeightAtPosition(vec3, BodyFlags.CommonCollisionExcludeFlags);
					stackArray8Vec[num++] = vec2;
					stackArray8Vec[num++] = vec3;
				}
			}
			if (visibilityExcludeParents)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			Vec3 vec4 = default(Vec3);
			for (int i = 0; i < num; i++)
			{
				vec4 += stackArray8Vec[i];
			}
			vec4 /= (float)num;
			for (int j = 0; j < num; j++)
			{
				Vec3 vec5 = stackArray8Vec[j] - vec4;
				num2 += vec5.x * vec5.x;
				num3 += vec5.x * vec5.y;
				num4 += vec5.y * vec5.y;
				num5 += vec5.x * vec5.z;
				num6 += vec5.y * vec5.z;
			}
			float num7 = num2 * num4 - num3 * num3;
			float num8 = (num6 * num3 - num5 * num4) / num7;
			float num9 = (num3 * num5 - num2 * num6) / num7;
			MatrixFrame matrixFrame;
			matrixFrame.origin = vec4;
			matrixFrame.rotation.u = new Vec3(num8, num9, 1f, -1f);
			matrixFrame.rotation.u.Normalize();
			matrixFrame.rotation.f = frame.rotation.f;
			matrixFrame.rotation.f = matrixFrame.rotation.f - Vec3.DotProduct(matrixFrame.rotation.f, matrixFrame.rotation.u) * matrixFrame.rotation.u;
			matrixFrame.rotation.f.Normalize();
			matrixFrame.rotation.s = Vec3.CrossProduct(matrixFrame.rotation.f, matrixFrame.rotation.u);
			matrixFrame.rotation.s.Normalize();
			return matrixFrame;
		}

		// Token: 0x040014DA RID: 5338
		public const string GhostObjectTag = "ghost_object";

		// Token: 0x040014DB RID: 5339
		private const string WheelTag = "wheel";

		// Token: 0x040014DC RID: 5340
		public const string MoveStandingPointTag = "move";

		// Token: 0x040014DD RID: 5341
		public float AxleLength = 2.45f;

		// Token: 0x040014DE RID: 5342
		public int NavMeshIdToDisableOnDestination = -1;

		// Token: 0x040014DF RID: 5343
		private float _ghostObjectPos;

		// Token: 0x040014E0 RID: 5344
		private List<GameEntity> _wheels;

		// Token: 0x040014E1 RID: 5345
		private List<StandingPoint> _standingPoints;

		// Token: 0x040014E2 RID: 5346
		private MatrixFrame[] _standingPointLocalIKFrames;

		// Token: 0x040014E3 RID: 5347
		private SoundEvent _movementSound;

		// Token: 0x040014E4 RID: 5348
		private float _wheelCircumference;

		// Token: 0x040014E5 RID: 5349
		private bool _isMoveSoundPlaying;

		// Token: 0x040014E6 RID: 5350
		private float _wheelDiameter;

		// Token: 0x040014E7 RID: 5351
		private Path _path;

		// Token: 0x040014E8 RID: 5352
		private PathTracker _pathTracker;

		// Token: 0x040014E9 RID: 5353
		private PathTracker _ghostEntityPathTracker;

		// Token: 0x040014EA RID: 5354
		private float _advancementError;

		// Token: 0x040014EB RID: 5355
		private float _lastSynchronizedDistance;
	}
}
