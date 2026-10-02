using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000367 RID: 871
	public class TrajectoryVisualizer : ScriptComponentBehavior
	{
		// Token: 0x06003217 RID: 12823 RVA: 0x000CC2B4 File Offset: 0x000CA4B4
		public void SetTrajectoryParams(Vec3 missileShootingPositionOffset, float missileSpeed, float verticalAngleMinInDegrees, float verticalAngleMaxInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant)
		{
			this._trajectoryParams.MissileShootingPositionOffset = missileShootingPositionOffset;
			this._trajectoryParams.MissileSpeed = missileSpeed;
			this._trajectoryParams.VerticalAngleMinInDegrees = verticalAngleMinInDegrees;
			this._trajectoryParams.VerticalAngleMaxInDegrees = verticalAngleMaxInDegrees;
			this._trajectoryParams.HorizontalAngleRangeInDegrees = horizontalAngleRangeInDegrees;
			this._trajectoryParams.AirFrictionConstant = airFrictionConstant;
			this._trajectoryParams.IsValid = true;
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000CC318 File Offset: 0x000CA518
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000CC320 File Offset: 0x000CA520
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "ShowTrajectory")
			{
				if (this.ShowTrajectory && this._trajectoryMeshHolder == null && !base.GameEntity.IsGhostObject() && this._trajectoryParams.IsValid)
				{
					this._trajectoryMeshHolder = TaleWorlds.Engine.GameEntity.CreateEmpty(base.Scene, false, true, true);
					if (this._trajectoryMeshHolder != null)
					{
						this._trajectoryMeshHolder.EntityFlags |= EntityFlags.DontSaveToScene;
						MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
						Vec3 vec = globalFrame.origin + (globalFrame.rotation.s * this._trajectoryParams.MissileShootingPositionOffset.x + globalFrame.rotation.f * this._trajectoryParams.MissileShootingPositionOffset.y + globalFrame.rotation.u * this._trajectoryParams.MissileShootingPositionOffset.z);
						globalFrame.origin = vec;
						this._trajectoryMeshHolder.SetGlobalFrame(in globalFrame, true);
						this._trajectoryMeshHolder.ComputeTrajectoryVolume(this._trajectoryParams.MissileSpeed, this._trajectoryParams.VerticalAngleMaxInDegrees, this._trajectoryParams.VerticalAngleMinInDegrees, this._trajectoryParams.HorizontalAngleRangeInDegrees, this._trajectoryParams.AirFrictionConstant);
						base.GameEntity.AddChild(this._trajectoryMeshHolder.WeakEntity, true);
						this._trajectoryMeshHolder.SetVisibilityExcludeParents(false);
					}
				}
				if (this._trajectoryMeshHolder != null)
				{
					this._trajectoryMeshHolder.SetVisibilityExcludeParents(this.ShowTrajectory);
				}
			}
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000CC4DA File Offset: 0x000CA6DA
		protected override void OnRemoved(int removeReason)
		{
			if (this._trajectoryMeshHolder != null)
			{
				this._trajectoryMeshHolder.Remove(removeReason);
			}
		}

		// Token: 0x0400151F RID: 5407
		public bool ShowTrajectory;

		// Token: 0x04001520 RID: 5408
		private GameEntity _trajectoryMeshHolder;

		// Token: 0x04001521 RID: 5409
		private TrajectoryVisualizer.TrajectoryParams _trajectoryParams;

		// Token: 0x0200064B RID: 1611
		private struct TrajectoryParams
		{
			// Token: 0x0400218E RID: 8590
			public Vec3 MissileShootingPositionOffset;

			// Token: 0x0400218F RID: 8591
			public float MissileSpeed;

			// Token: 0x04002190 RID: 8592
			public float VerticalAngleMinInDegrees;

			// Token: 0x04002191 RID: 8593
			public float VerticalAngleMaxInDegrees;

			// Token: 0x04002192 RID: 8594
			public float HorizontalAngleRangeInDegrees;

			// Token: 0x04002193 RID: 8595
			public float AirFrictionConstant;

			// Token: 0x04002194 RID: 8596
			public bool IsValid;
		}
	}
}
