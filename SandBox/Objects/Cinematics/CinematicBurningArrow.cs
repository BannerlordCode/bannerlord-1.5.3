using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.Objects.Cinematics
{
	// Token: 0x02000040 RID: 64
	public class CinematicBurningArrow : ScriptComponentBehavior
	{
		// Token: 0x0600025F RID: 607 RVA: 0x0000E284 File Offset: 0x0000C484
		public void StartMovement()
		{
			this._initialFrameCacheForShootArrowButton = base.GameEntity.GetFrame();
			this._initialGlobalFrameCacheForShootArrowButton = base.GameEntity.GetGlobalFrame();
			this._state = CinematicBurningArrow.BurningArrowState.StartMovement;
			this._speedVector = this._speed * this._initialFrameCacheForShootArrowButton.rotation.u;
			this._arrowSound = SoundEvent.CreateEventFromString("event:/mission/ambient/special/alert_arrow", base.Scene);
			this._arrowSound.Play();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000E303 File Offset: 0x0000C503
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000E308 File Offset: 0x0000C508
		protected override void OnInit()
		{
			base.OnInit();
			base.GameEntity.SetVisibilityExcludeParents(false);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000E32C File Offset: 0x0000C52C
		protected override void OnTick(float dt)
		{
			this.Tick(dt);
			if (!this._speed.Equals(this._speedCache))
			{
				this._speedCache = this._speed;
				this._speedVector = this._speed * this._initialFrameCacheForShootArrowButton.rotation.u;
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000E380 File Offset: 0x0000C580
		protected override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.Tick(dt);
			Vec3 vec;
			Vec3 vec2;
			if (this._state == CinematicBurningArrow.BurningArrowState.None)
			{
				vec = base.GameEntity.GetGlobalFrame().origin;
				vec2 = this._speed * base.GameEntity.GetGlobalFrame().rotation.u;
			}
			else
			{
				vec = this._initialGlobalFrameCacheForShootArrowButton.origin;
				vec2 = this._speed * this._initialGlobalFrameCacheForShootArrowButton.rotation.u;
			}
			List<Vec3> list = new List<Vec3>();
			list.Add(vec);
			float num = 0f;
			float num2 = this._speed * 100f / 15f;
			int num3 = 1;
			while ((float)num3 < num2)
			{
				num += 0.03f;
				list.Add(this.GetPositionAtTime(in vec, in vec2, num));
				num3++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i != list.Count - 1)
				{
					Vec3 vec3 = list[i];
					Vec3 vec4 = list[i + 1];
				}
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000E490 File Offset: 0x0000C690
		private Vec3 GetPositionAtTime(in Vec3 startPosition, in Vec3 speedVector, float time)
		{
			Vec3 zero = Vec3.Zero;
			zero.x = startPosition.x + speedVector.x * time;
			zero.y = startPosition.y + speedVector.y * time;
			zero.z = startPosition.z + speedVector.z * time - 4.9f * time * time;
			return zero;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		private void Tick(float dt)
		{
			if (this._state != CinematicBurningArrow.BurningArrowState.EndMovement)
			{
				if (this._state == CinematicBurningArrow.BurningArrowState.StartMovement)
				{
					base.GameEntity.SetVisibilityExcludeParents(true);
					this._state = CinematicBurningArrow.BurningArrowState.MovementInProgress;
				}
				if (this._state == CinematicBurningArrow.BurningArrowState.MovementInProgress)
				{
					this.Move(dt);
				}
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000E538 File Offset: 0x0000C738
		private void Move(float dt)
		{
			if (this._speed <= 0f || this._arrowMovementTimer >= 4f)
			{
				base.GameEntity.SetVisibilityExcludeParents(false);
				this._state = CinematicBurningArrow.BurningArrowState.EndMovement;
				this._arrowSound.Stop();
				this._arrowMovementTimer = 0f;
				return;
			}
			MatrixFrame frame = base.GameEntity.GetFrame();
			this._speedVector.z = this._speedVector.z - 9.8f * dt;
			Vec3 vec = frame.origin + this._speedVector * dt;
			this.LookAtWithZAsForward(ref frame, this._speedVector.NormalizedCopy(), Vec3.Up);
			frame.origin = vec;
			base.GameEntity.SetFrame(ref frame, true);
			this._arrowSound.SetPosition(base.GameEntity.GlobalPosition);
			this._arrowMovementTimer += dt;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000E62C File Offset: 0x0000C82C
		private void LookAtWithZAsForward(ref MatrixFrame frame, Vec3 direction, Vec3 upVector)
		{
			Vec3 vec = direction;
			vec.Normalize();
			Vec3 vec2 = Vec3.CrossProduct(vec, upVector);
			vec2.Normalize();
			Vec3 vec3 = Vec3.CrossProduct(vec2, vec);
			vec3.Normalize();
			frame.rotation.s = vec2;
			frame.rotation.f = vec3;
			frame.rotation.u = -vec;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000E690 File Offset: 0x0000C890
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "ShootArrow")
			{
				if (this._state != CinematicBurningArrow.BurningArrowState.None)
				{
					this._state = CinematicBurningArrow.BurningArrowState.None;
					base.GameEntity.SetFrame(ref this._initialFrameCacheForShootArrowButton, true);
				}
				this.StartMovement();
			}
			if (variableName == "StopMovement")
			{
				this._state = CinematicBurningArrow.BurningArrowState.None;
				base.GameEntity.SetFrame(ref this._initialFrameCacheForShootArrowButton, true);
				this._arrowMovementTimer = 0f;
			}
		}

		// Token: 0x040000F2 RID: 242
		private const float Gravity = 9.8f;

		// Token: 0x040000F3 RID: 243
		private CinematicBurningArrow.BurningArrowState _state;

		// Token: 0x040000F4 RID: 244
		private float _speedCache;

		// Token: 0x040000F5 RID: 245
		[EditableScriptComponentVariable(true, "")]
		private float _speed = 10f;

		// Token: 0x040000F6 RID: 246
		private Vec3 _speedVector = Vec3.Zero;

		// Token: 0x040000F7 RID: 247
		private float _arrowMovementTimer;

		// Token: 0x040000F8 RID: 248
		private SoundEvent _arrowSound;

		// Token: 0x040000F9 RID: 249
		private MatrixFrame _initialFrameCacheForShootArrowButton;

		// Token: 0x040000FA RID: 250
		private MatrixFrame _initialGlobalFrameCacheForShootArrowButton;

		// Token: 0x040000FB RID: 251
		public SimpleButton ShootArrow;

		// Token: 0x040000FC RID: 252
		public SimpleButton StopMovement;

		// Token: 0x02000156 RID: 342
		private enum BurningArrowState
		{
			// Token: 0x04000694 RID: 1684
			None,
			// Token: 0x04000695 RID: 1685
			StartMovement,
			// Token: 0x04000696 RID: 1686
			MovementInProgress,
			// Token: 0x04000697 RID: 1687
			EndMovement
		}
	}
}
