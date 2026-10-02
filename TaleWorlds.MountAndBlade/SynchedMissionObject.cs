using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036C RID: 876
	public class SynchedMissionObject : MissionObject
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x000CE650 File Offset: 0x000CC850
		// (set) Token: 0x06003263 RID: 12899 RVA: 0x000CE658 File Offset: 0x000CC858
		public uint Color { get; private set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x000CE661 File Offset: 0x000CC861
		// (set) Token: 0x06003265 RID: 12901 RVA: 0x000CE669 File Offset: 0x000CC869
		public uint Color2 { get; private set; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x000CE672 File Offset: 0x000CC872
		public bool SynchronizeCompleted
		{
			get
			{
				return this._synchState == SynchedMissionObject.SynchState.SynchronizeCompleted;
			}
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000CE67D File Offset: 0x000CC87D
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x000CE691 File Offset: 0x000CC891
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (!this.SynchronizeCompleted)
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x000CE6AC File Offset: 0x000CC8AC
		protected internal override void OnTick(float dt)
		{
			if (!this.SynchronizeCompleted)
			{
				MatrixFrame frame = base.GameEntity.GetFrame();
				if ((this._synchState == SynchedMissionObject.SynchState.SynchronizePosition && this._lastSynchedFrame.origin.NearlyEquals(in frame.origin, 1E-05f)) || this._lastSynchedFrame.NearlyEquals(frame, 1E-05f))
				{
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeCompleted);
					return;
				}
				MatrixFrame matrixFrame;
				matrixFrame.origin = ((this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime) ? MBMath.Lerp(this._firstFrame.origin, this._lastSynchedFrame.origin, this._timer / this._duration, 0.2f * dt) : MBMath.Lerp(frame.origin, this._lastSynchedFrame.origin, 8f * dt, 0.2f * dt));
				if (this._synchState == SynchedMissionObject.SynchState.SynchronizeFrame || this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime)
				{
					matrixFrame.rotation.s = MBMath.Lerp(frame.rotation.s, this._lastSynchedFrame.rotation.s, 8f * dt, 0.2f * dt);
					matrixFrame.rotation.f = MBMath.Lerp(frame.rotation.f, this._lastSynchedFrame.rotation.f, 8f * dt, 0.2f * dt);
					matrixFrame.rotation.u = MBMath.Lerp(frame.rotation.u, this._lastSynchedFrame.rotation.u, 8f * dt, 0.2f * dt);
					if (matrixFrame.origin != this._lastSynchedFrame.origin || matrixFrame.rotation.s != this._lastSynchedFrame.rotation.s || matrixFrame.rotation.f != this._lastSynchedFrame.rotation.f || matrixFrame.rotation.u != this._lastSynchedFrame.rotation.u)
					{
						matrixFrame.rotation.Orthonormalize();
						if (this._lastSynchedFrame.rotation.HasScale())
						{
							Vec3 scaleVector = this._lastSynchedFrame.rotation.GetScaleVector();
							matrixFrame.rotation.ApplyScaleLocal(in scaleVector);
						}
					}
					base.GameEntity.SetFrame(ref matrixFrame, true);
				}
				else
				{
					base.GameEntity.SetLocalPosition(matrixFrame.origin);
				}
				this._timer = MathF.Min(this._timer + dt, this._duration);
			}
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x000CE938 File Offset: 0x000CCB38
		private void SetSynchState(SynchedMissionObject.SynchState newState)
		{
			if (newState != this._synchState)
			{
				this._synchState = newState;
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x000CE956 File Offset: 0x000CCB56
		public void SetLocalPositionSmoothStep(ref Vec3 targetPosition)
		{
			this._lastSynchedFrame.origin = targetPosition;
			this.SetSynchState(SynchedMissionObject.SynchState.SynchronizePosition);
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x000CE970 File Offset: 0x000CCB70
		public virtual void SetVisibleSynched(bool value, bool forceChildrenVisible = false)
		{
			bool flag = base.GameEntity.IsVisibleIncludeParents() != value;
			List<WeakGameEntity> list = null;
			if (!flag && forceChildrenVisible)
			{
				list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				using (List<WeakGameEntity>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetPhysicsState() != value)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (base.GameEntity.IsValid && flag)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectVisibility(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.SetVisibilityExcludeParents(value);
				if (forceChildrenVisible)
				{
					if (list == null)
					{
						list = new List<WeakGameEntity>();
						base.GameEntity.GetChildrenRecursive(ref list);
					}
					foreach (WeakGameEntity weakGameEntity in list)
					{
						weakGameEntity.SetVisibilityExcludeParents(value);
					}
				}
			}
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x000CEA9C File Offset: 0x000CCC9C
		public virtual void SetPhysicsStateSynched(bool value, bool setChildren = true)
		{
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000CEA9E File Offset: 0x000CCC9E
		public virtual void SetDisabledSynched()
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetMissionObjectDisabled(base.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			base.SetDisabledAndMakeInvisible(false, false);
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000CEACC File Offset: 0x000CCCCC
		public void SetFrameSynched(ref MatrixFrame frame, bool isClient = false)
		{
			MatrixFrame frame2 = base.GameEntity.GetFrame();
			if ((in frame2) != (in frame) || this._synchState != SynchedMissionObject.SynchState.SynchronizeCompleted)
			{
				this._duration = 0f;
				this._timer = 0f;
				if (GameNetwork.IsClientOrReplay)
				{
					this._lastSynchedFrame = frame;
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrame);
					return;
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectFrame(base.Id, ref frame));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeCompleted);
				base.GameEntity.SetFrame(ref frame, true);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000CEB78 File Offset: 0x000CCD78
		public void SetGlobalFrameSynched(ref MatrixFrame frame, bool isClient = false)
		{
			this._duration = 0f;
			this._timer = 0f;
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			if ((in matrixFrame) != (in frame))
			{
				if (GameNetwork.IsClientOrReplay)
				{
					MatrixFrame matrixFrame2;
					if (!base.GameEntity.Parent.IsValid)
					{
						matrixFrame2 = frame;
					}
					else
					{
						matrixFrame = base.GameEntity.Parent.GetGlobalFrame();
						matrixFrame2 = matrixFrame.TransformToLocalNonOrthogonal(in frame);
					}
					this._lastSynchedFrame = matrixFrame2;
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrame);
					return;
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectGlobalFrame(base.Id, ref frame));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeCompleted);
				base.GameEntity.SetGlobalFrame(in frame, true);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000CEC58 File Offset: 0x000CCE58
		public void SetFrameSynchedOverTime(ref MatrixFrame frame, float duration, bool isClient = false)
		{
			MatrixFrame frame2 = base.GameEntity.GetFrame();
			if ((in frame2) != (in frame) || duration.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._firstFrame = base.GameEntity.GetFrame();
				this._lastSynchedFrame = frame;
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
				this._duration = (duration.ApproximatelyEqualsTo(0f, 1E-05f) ? 0.1f : duration);
				this._timer = 0f;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectFrameOverTime(base.Id, ref frame, duration));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000CED1C File Offset: 0x000CCF1C
		public void SetGlobalFrameSynchedOverTime(ref MatrixFrame frame, float duration, bool isClient = false)
		{
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			if ((in matrixFrame) != (in frame) || duration.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._firstFrame = base.GameEntity.GetFrame();
				MatrixFrame matrixFrame2;
				if (!base.GameEntity.Parent.IsValid)
				{
					matrixFrame2 = frame;
				}
				else
				{
					matrixFrame = base.GameEntity.Parent.GetGlobalFrame();
					matrixFrame2 = matrixFrame.TransformToLocalNonOrthogonal(in frame);
				}
				this._lastSynchedFrame = matrixFrame2;
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
				this._duration = (duration.ApproximatelyEqualsTo(0f, 1E-05f) ? 0.1f : duration);
				this._timer = 0f;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectGlobalFrameOverTime(base.Id, ref frame, duration));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000CEE16 File Offset: 0x000CD016
		public void SetAnimationAtChannelSynched(string animationName, int channelNo, float animationSpeed = 1f)
		{
			this.SetAnimationAtChannelSynched(MBAnimation.GetAnimationIndexWithName(animationName), channelNo, animationSpeed);
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000CEE28 File Offset: 0x000CD028
		public void SetAnimationAtChannelSynched(int animationIndex, int channelNo, float animationSpeed = 1f)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				int animationIndexAtChannel = base.GameEntity.Skeleton.GetAnimationIndexAtChannel(channelNo);
				bool flag = true;
				if (animationIndexAtChannel == animationIndex && base.GameEntity.Skeleton.GetAnimationSpeedAtChannel(channelNo).ApproximatelyEqualsTo(animationSpeed, 1E-05f) && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(channelNo) < 0.02f)
				{
					flag = false;
				}
				if (flag)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationAtChannel(base.Id, channelNo, animationIndex, animationSpeed));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
				}
			}
			base.GameEntity.Skeleton.SetAnimationAtChannel(animationIndex, channelNo, animationSpeed, -1f, 0f);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000CEEE8 File Offset: 0x000CD0E8
		public void SetAnimationChannelParameterSynched(int channelNo, float parameter)
		{
			if (!base.GameEntity.Skeleton.GetAnimationParameterAtChannel(channelNo).ApproximatelyEqualsTo(parameter, 1E-05f))
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationChannelParameter(base.Id, channelNo, parameter));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(channelNo, parameter);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000CEF60 File Offset: 0x000CD160
		public void SetAnimationChannelSpeedSynched(int channelNo, float speed)
		{
			if (!base.GameEntity.Skeleton.GetAnimationSpeedAtChannel(channelNo).ApproximatelyEqualsTo(speed, 1E-05f))
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationChannelSpeed(base.Id, channelNo, speed));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.Skeleton.SetAnimationSpeedAtChannel(channelNo, speed);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000CEFD8 File Offset: 0x000CD1D8
		public void PauseSkeletonAnimationSynched()
		{
			if (!base.GameEntity.IsSkeletonAnimationPaused())
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationPaused(base.Id, true));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.PauseSkeletonAnimation();
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000CF038 File Offset: 0x000CD238
		public void ResumeSkeletonAnimationSynched()
		{
			if (base.GameEntity.IsSkeletonAnimationPaused())
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationPaused(base.Id, false));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.ResumeSkeletonAnimation();
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000CF098 File Offset: 0x000CD298
		public void BurstParticlesSynched(bool doChildren = true)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new BurstMissionObjectParticles(base.Id, false));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			base.GameEntity.BurstEntityParticle(doChildren);
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000CF0DC File Offset: 0x000CD2DC
		public void ApplyImpulseSynched(Vec3 localPosition, Vec3 impulse)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetMissionObjectImpulse(base.Id, localPosition, impulse));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			base.GameEntity.ApplyLocalImpulseToDynamicBody(localPosition, impulse);
			this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000CF12C File Offset: 0x000CD32C
		public void AddBodyFlagsSynched(BodyFlags flags, bool applyToChildren = true)
		{
			if ((base.GameEntity.BodyFlag & flags) != flags)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new AddMissionObjectBodyFlags(base.Id, flags, applyToChildren));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.AddBodyFlags(flags, applyToChildren);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchBodyFlags;
			}
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000CF190 File Offset: 0x000CD390
		public void RemoveBodyFlagsSynched(BodyFlags flags, bool applyToChildren = true)
		{
			if ((base.GameEntity.BodyFlag & flags) != BodyFlags.None)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new RemoveMissionObjectBodyFlags(base.Id, flags, applyToChildren));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.RemoveBodyFlags(flags, applyToChildren);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchBodyFlags;
			}
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000CF1F4 File Offset: 0x000CD3F4
		public void SetTeamColors(uint color, uint color2)
		{
			this.Color = color;
			this.Color2 = color2;
			base.GameEntity.SetColor(color, color2, "use_team_color");
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x000CF224 File Offset: 0x000CD424
		public virtual void SetTeamColorsSynched(uint color, uint color2)
		{
			if (base.GameEntity.IsValid)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectColors(base.Id, color, color2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.SetTeamColors(color, color2);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SyncColors;
			}
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000CF280 File Offset: 0x000CD480
		public virtual void WriteToNetwork()
		{
			GameNetworkMessage.WriteBoolToPacket(base.GameEntity.GetVisibilityExcludeParents());
			GameNetworkMessage.WriteBoolToPacket(this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchTransform));
			if (this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchTransform))
			{
				GameNetworkMessage.WriteMatrixFrameToPacket(base.GameEntity.GetFrame());
				GameNetworkMessage.WriteBoolToPacket(this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
				if (this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime)
				{
					GameNetworkMessage.WriteMatrixFrameToPacket(this._lastSynchedFrame);
					GameNetworkMessage.WriteFloatToPacket(this._duration - this._timer, CompressionMission.FlagCapturePointDurationCompressionInfo);
				}
			}
			Skeleton skeleton = base.GameEntity.Skeleton;
			GameNetworkMessage.WriteBoolToPacket(skeleton != null);
			if (skeleton != null)
			{
				int animationIndexAtChannel = skeleton.GetAnimationIndexAtChannel(0);
				bool flag = animationIndexAtChannel >= 0;
				GameNetworkMessage.WriteBoolToPacket(flag && this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchAnimation));
				if (flag && this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchAnimation))
				{
					float animationSpeedAtChannel = skeleton.GetAnimationSpeedAtChannel(0);
					float animationParameterAtChannel = skeleton.GetAnimationParameterAtChannel(0);
					GameNetworkMessage.WriteIntToPacket(animationIndexAtChannel, CompressionBasic.AnimationIndexCompressionInfo);
					GameNetworkMessage.WriteFloatToPacket(animationSpeedAtChannel, CompressionBasic.AnimationSpeedCompressionInfo);
					GameNetworkMessage.WriteFloatToPacket(animationParameterAtChannel, CompressionBasic.AnimationProgressCompressionInfo);
					GameNetworkMessage.WriteBoolToPacket(base.GameEntity.IsSkeletonAnimationPaused());
				}
			}
			GameNetworkMessage.WriteBoolToPacket(this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SyncColors));
			if (this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SyncColors))
			{
				GameNetworkMessage.WriteUintToPacket(this.Color, CompressionBasic.ColorCompressionInfo);
				GameNetworkMessage.WriteUintToPacket(this.Color2, CompressionBasic.ColorCompressionInfo);
			}
			GameNetworkMessage.WriteBoolToPacket(base.IsDisabled);
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000CF3F4 File Offset: 0x000CD5F4
		public virtual void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			BaseSynchedMissionObjectReadableRecord item = synchedMissionObjectReadableRecord.Item1;
			if (item.SynchTransform)
			{
				MatrixFrame gameObjectFrame = item.GameObjectFrame;
				base.GameEntity.SetFrame(ref gameObjectFrame, true);
				if (item.SynchronizeFrameOverTime)
				{
					this._firstFrame = item.GameObjectFrame;
					this._lastSynchedFrame = item.LastSynchedFrame;
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
					this._duration = item.Duration;
					this._timer = 0f;
					if (this._duration.ApproximatelyEqualsTo(0f, 1E-05f))
					{
						this._duration = 0.1f;
					}
				}
			}
			if (item.HasSkeleton && item.SynchAnimation)
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(item.AnimationIndex, 0, item.AnimationSpeed, 0f, 0f);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, item.AnimationParameter);
				if (item.IsSkeletonAnimationPaused)
				{
					base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, base.GameEntity.GetGlobalFrame(), true);
					base.GameEntity.PauseSkeletonAnimation();
				}
				else
				{
					base.GameEntity.ResumeSkeletonAnimation();
				}
			}
			if (item.SynchColors)
			{
				this.SetTeamColors(item.Color, item.Color2);
			}
			if (item.IsDisabled)
			{
				base.SetDisabledAndMakeInvisible(false, false);
			}
			if (allowVisibilityUpdate)
			{
				base.GameEntity.SetVisibilityExcludeParents(item.SetVisibilityExcludeParents);
			}
		}

		// Token: 0x04001569 RID: 5481
		private SynchedMissionObject.SynchFlags _initialSynchFlags;

		// Token: 0x0400156A RID: 5482
		private SynchedMissionObject.SynchState _synchState;

		// Token: 0x0400156B RID: 5483
		private MatrixFrame _lastSynchedFrame;

		// Token: 0x0400156C RID: 5484
		private MatrixFrame _firstFrame;

		// Token: 0x0400156D RID: 5485
		private float _timer;

		// Token: 0x0400156E RID: 5486
		private float _duration;

		// Token: 0x0200064D RID: 1613
		private enum SynchState
		{
			// Token: 0x040021A0 RID: 8608
			SynchronizeCompleted,
			// Token: 0x040021A1 RID: 8609
			SynchronizePosition,
			// Token: 0x040021A2 RID: 8610
			SynchronizeFrame,
			// Token: 0x040021A3 RID: 8611
			SynchronizeFrameOverTime
		}

		// Token: 0x0200064E RID: 1614
		[Flags]
		public enum SynchFlags : uint
		{
			// Token: 0x040021A5 RID: 8613
			SynchNone = 0U,
			// Token: 0x040021A6 RID: 8614
			SynchTransform = 1U,
			// Token: 0x040021A7 RID: 8615
			SynchAnimation = 2U,
			// Token: 0x040021A8 RID: 8616
			SynchBodyFlags = 4U,
			// Token: 0x040021A9 RID: 8617
			SyncColors = 8U,
			// Token: 0x040021AA RID: 8618
			SynchAll = 4294967295U
		}
	}
}
