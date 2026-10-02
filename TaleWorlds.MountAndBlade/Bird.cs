using System;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032F RID: 815
	public class Bird : MissionObject
	{
		// Token: 0x06002E6E RID: 11886 RVA: 0x000B36A7 File Offset: 0x000B18A7
		private Bird.State ComputeInitialState()
		{
			if (this.CanFly && !this._canLand)
			{
				return Bird.State.Airborne;
			}
			return Bird.State.Perched;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000B36BC File Offset: 0x000B18BC
		protected internal override void OnInit()
		{
			base.OnInit();
			base.GameEntity.SetAnimationSoundActivation(true);
			base.GameEntity.Skeleton.SetAnimationAtChannel(this._idleAnimation, 0, 1f, -1f, 0f);
			base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBRandom.RandomFloat * 0.5f);
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000B3734 File Offset: 0x000B1934
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			base.GameEntity.Skeleton.SetAnimationAtChannel(this._idleAnimation, 0, 1f, -1f, 0f);
			base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, 0f);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000B3789 File Offset: 0x000B1989
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000B3793 File Offset: 0x000B1993
		private void SetState(Bird.State newState)
		{
			this._state = newState;
			this.OnStateChanged();
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000B37A4 File Offset: 0x000B19A4
		private void OnStateChanged()
		{
			switch (this._state)
			{
			case Bird.State.TakingOff:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._takingOffAnimation, 0, 1f, -1f, 0f);
				this._timer.Reset();
				return;
			case Bird.State.Airborne:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._flyCycleAnimation, 0, 1f, -1f, 0f);
				this._timer.Set(-MBRandom.RandomFloatRanged(5f, 15f));
				this.ApplyAnimationDisplacement();
				return;
			case Bird.State.Landing:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._landingAnimation, 0, 1f, -1f, 0f);
				this._timer.Reset();
				this.ApplyAnimationDisplacement();
				return;
			case Bird.State.Perched:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._idleAnimation, 0, 1f, -1f, 0f);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBRandom.RandomFloat * 0.5f);
				this._timer.Set(-MBRandom.RandomFloatRanged(5f, 18f));
				return;
			default:
				Debug.FailedAssert("Unknown state please handle!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Bird.cs", "OnStateChanged", 132);
				return;
			}
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000B390B File Offset: 0x000B1B0B
		protected internal override void OnEditorTick(float dt)
		{
			if (this._timer == null)
			{
				this._timer = new BasicTimer();
				this.SetState(this.ComputeInitialState());
			}
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000B392C File Offset: 0x000B1B2C
		protected internal override void OnTick(float dt)
		{
			if (this._timer == null)
			{
				this._timer = new BasicTimer();
				this.SetState(this.ComputeInitialState());
			}
			switch (this._state)
			{
			case Bird.State.TakingOff:
				if (base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) > 0.99f)
				{
					this.SetState(Bird.State.Airborne);
					return;
				}
				break;
			case Bird.State.Airborne:
				if (this._timer.ElapsedTime <= 0f)
				{
					this.ApplyFlyingMovement(dt);
					return;
				}
				if (this._canLand)
				{
					this.SetState(Bird.State.Landing);
					return;
				}
				base.GameEntity.SetVisibilityExcludeParents(false);
				return;
			case Bird.State.Landing:
				if (base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) > 0.99f)
				{
					this.SetState(Bird.State.Perched);
					return;
				}
				break;
			case Bird.State.Perched:
				if (this.CanFly && this._timer.ElapsedTime > 0f && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) > 0.99f)
				{
					this.SetState(Bird.State.TakingOff);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000B3A38 File Offset: 0x000B1C38
		private void ApplyFlyingMovement(float dt)
		{
			float num = this._flyingSpeedInKph * 0.2777778f * dt;
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 vec = globalFrame.rotation.f.NormalizedCopy();
			globalFrame.origin -= vec * num;
			base.GameEntity.SetGlobalFrame(in globalFrame, true);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000B3AA4 File Offset: 0x000B1CA4
		private void ApplyAnimationDisplacement()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 vec = globalFrame.rotation.f.NormalizedCopy();
			Vec3 vec2 = globalFrame.rotation.u.NormalizedCopy();
			if (this._state == Bird.State.Airborne)
			{
				globalFrame.origin -= vec * 20f - vec2 * 6.5f;
			}
			else if (this._state == Bird.State.Landing)
			{
				globalFrame.origin -= vec * 20f + vec2 * 6.5f;
			}
			base.GameEntity.SetGlobalFrame(in globalFrame, true);
		}

		// Token: 0x0400124D RID: 4685
		[EditableScriptComponentVariable(true, "")]
		private float _flyingSpeedInKph = 14.4f;

		// Token: 0x0400124E RID: 4686
		[EditableScriptComponentVariable(true, "")]
		private string _idleAnimation = "anim_bird_idle";

		// Token: 0x0400124F RID: 4687
		[EditableScriptComponentVariable(true, "")]
		private string _landingAnimation = "anim_bird_landing";

		// Token: 0x04001250 RID: 4688
		[EditableScriptComponentVariable(true, "")]
		private string _takingOffAnimation = "anim_bird_flying";

		// Token: 0x04001251 RID: 4689
		[EditableScriptComponentVariable(true, "")]
		private string _flyCycleAnimation = "anim_bird_cycle";

		// Token: 0x04001252 RID: 4690
		[EditableScriptComponentVariable(true, "")]
		private bool _canLand = true;

		// Token: 0x04001253 RID: 4691
		public bool CanFly;

		// Token: 0x04001254 RID: 4692
		private Bird.State _state;

		// Token: 0x04001255 RID: 4693
		private BasicTimer _timer;

		// Token: 0x02000612 RID: 1554
		private enum State
		{
			// Token: 0x040020C4 RID: 8388
			TakingOff,
			// Token: 0x040020C5 RID: 8389
			Airborne,
			// Token: 0x040020C6 RID: 8390
			Landing,
			// Token: 0x040020C7 RID: 8391
			Perched
		}
	}
}
