using System;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.Missions
{
	// Token: 0x02000057 RID: 87
	public class CameraJumpScript : ScriptComponentBehavior
	{
		// Token: 0x06000375 RID: 885 RVA: 0x0001439D File Offset: 0x0001259D
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000143A0 File Offset: 0x000125A0
		protected override void OnInit()
		{
			this._elapsedDuration = 0f;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000143B0 File Offset: 0x000125B0
		protected override void OnEditorInit()
		{
			this._initialGlobalFrame = base.GameEntity.GetGlobalFrame();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000143D1 File Offset: 0x000125D1
		protected override void OnTick(float dt)
		{
			this.OnJumpTick(dt);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000143DA File Offset: 0x000125DA
		protected override void OnEditorTick(float dt)
		{
			this.OnJumpTick(dt);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000143E4 File Offset: 0x000125E4
		private void OnJumpTick(float dt)
		{
			if (this._elapsedDuration >= 0f)
			{
				this._elapsedDuration += dt;
				if (this._elapsedDuration >= this._waitBeforeCameraJump)
				{
					Mat3 identity = Mat3.Identity;
					identity.ApplyEulerAngles(in this._cameraJumpRotation);
					WeakGameEntity gameEntity = base.GameEntity;
					MatrixFrame matrixFrame = new MatrixFrame(in identity, in this._cameraJumpPosition);
					gameEntity.SetGlobalFrame(in matrixFrame, true);
				}
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001444C File Offset: 0x0001264C
		protected override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "Preview")
			{
				this._elapsedDuration = 0f;
			}
			if (variableName == "Reset")
			{
				base.GameEntity.SetGlobalFrame(in this._initialGlobalFrame, true);
				this._elapsedDuration = -1f;
			}
			if (variableName == "SetCurrentCameraTransform")
			{
				MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
				this._cameraJumpPosition = globalFrame.origin;
				this._cameraJumpRotation = globalFrame.rotation.GetEulerAngles();
			}
		}

		// Token: 0x040001BA RID: 442
		[EditableScriptComponentVariable(true, "WaitBeforeCameraJump")]
		private float _waitBeforeCameraJump = 2f;

		// Token: 0x040001BB RID: 443
		[EditableScriptComponentVariable(true, "CameraJumpPosition")]
		private Vec3 _cameraJumpPosition;

		// Token: 0x040001BC RID: 444
		[EditableScriptComponentVariable(true, "CameraJumpRotation")]
		private Vec3 _cameraJumpRotation;

		// Token: 0x040001BD RID: 445
		public SimpleButton SetCurrentCameraTransform;

		// Token: 0x040001BE RID: 446
		public SimpleButton Preview;

		// Token: 0x040001BF RID: 447
		public SimpleButton Reset;

		// Token: 0x040001C0 RID: 448
		private MatrixFrame _initialGlobalFrame;

		// Token: 0x040001C1 RID: 449
		private float _elapsedDuration = -1f;
	}
}
