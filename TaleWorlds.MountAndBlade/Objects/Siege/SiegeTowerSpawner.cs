using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003C4 RID: 964
	public class SiegeTowerSpawner : SpawnerBase
	{
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06003637 RID: 13879 RVA: 0x000DFAB9 File Offset: 0x000DDCB9
		public float RampRotationRadian
		{
			get
			{
				return this.RampRotationDegree * 0.017453292f;
			}
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000DFAC8 File Offset: 0x000DDCC8
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._spawnerEditorHelper = new SpawnerEntityEditorHelper(this);
			this._spawnerEditorHelper.LockGhostParent = false;
			if (this._spawnerEditorHelper.IsValid)
			{
				this._spawnerEditorHelper.SetupGhostMovement(this.PathEntityName);
				this._spawnerEditorHelper.GivePermission("ramp", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.rotation, SpawnerEntityEditorHelper.Axis.x), new Action<float>(this.SetRampRotation));
				this._spawnerEditorHelper.GivePermission("ai_barrier_r", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.scale, SpawnerEntityEditorHelper.Axis.z), new Action<float>(this.SetAIBarrierRight));
				this._spawnerEditorHelper.GivePermission("ai_barrier_l", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.scale, SpawnerEntityEditorHelper.Axis.z), new Action<float>(this.SetAIBarrierLeft));
			}
			this.OnEditorVariableChanged("RampRotationDegree");
			this.OnEditorVariableChanged("BarrierLength");
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000DFB90 File Offset: 0x000DDD90
		private void SetRampRotation(float unusedArgument)
		{
			MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ramp").GetFrame();
			Vec3 vec = new Vec3(-frame.rotation.u.y, frame.rotation.u.x, 0f, -1f);
			float z = frame.rotation.u.z;
			float num = MathF.Atan2(vec.Length, z);
			if ((double)vec.x < 0.0)
			{
				num = -num;
				num += 6.2831855f;
			}
			float num2 = num;
			this.RampRotationDegree = num2 * 57.29578f;
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000DFC34 File Offset: 0x000DDE34
		private void SetAIBarrierRight(float barrierScale)
		{
			this.BarrierLength = barrierScale;
			MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_l").GetFrame();
			MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_r").GetFrame();
			frame.rotation.u = frame2.rotation.u;
			this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_l", frame, false);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000DFCA0 File Offset: 0x000DDEA0
		private void SetAIBarrierLeft(float barrierScale)
		{
			this.BarrierLength = barrierScale;
			MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_l").GetFrame();
			MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_r").GetFrame();
			frame2.rotation.u = frame.rotation.u;
			this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_r", frame2, false);
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000DFD09 File Offset: 0x000DDF09
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._spawnerEditorHelper.Tick(dt);
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000DFD20 File Offset: 0x000DDF20
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "PathEntityName")
			{
				this._spawnerEditorHelper.SetupGhostMovement(this.PathEntityName);
				return;
			}
			if (variableName == "EnableAutoGhostMovement")
			{
				this._spawnerEditorHelper.SetEnableAutoGhostMovement(this.EnableAutoGhostMovement);
				return;
			}
			if (variableName == "RampRotationDegree")
			{
				MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ramp").GetFrame();
				frame.rotation = Mat3.Identity;
				frame.rotation.RotateAboutSide(this.RampRotationRadian);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ramp", frame, true);
				return;
			}
			if (variableName == "BarrierLength")
			{
				MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_l").GetFrame();
				frame2.rotation.u.Normalize();
				frame2.rotation.u = frame2.rotation.u * MathF.Max(0.01f, MathF.Abs(this.BarrierLength));
				MatrixFrame frame3 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_r").GetFrame();
				frame3.rotation.u = frame2.rotation.u;
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_l", frame2, true);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_r", frame3, true);
				return;
			}
			if (variableName == "SpeedModifierFactor")
			{
				this.SpeedModifierFactor = MathF.Clamp(this.SpeedModifierFactor, 0.8f, 1.2f);
			}
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000DFEA9 File Offset: 0x000DE0A9
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000DFEC0 File Offset: 0x000DE0C0
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			SiegeTower firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<SiegeTower>();
			firstScriptOfType.AddOnDeployTag = this.AddOnDeployTag;
			firstScriptOfType.RemoveOnDeployTag = this.RemoveOnDeployTag;
			firstScriptOfType.MaxSpeed *= this.SpeedModifierFactor;
			firstScriptOfType.MinSpeed *= this.SpeedModifierFactor;
			Mat3 identity = Mat3.Identity;
			identity.RotateAboutSide(this.RampRotationRadian);
			firstScriptOfType.AssignParametersFromSpawner(this.PathEntityName, this.TargetWallSegmentTag, this.SideTag, this.SoilNavMeshID1, this.SoilNavMeshID2, this.DitchNavMeshID1, this.DitchNavMeshID2, this.GroundToSoilNavMeshID1, this.GroundToSoilNavMeshID2, this.SoilGenericNavMeshID, this.GroundGenericNavMeshID, identity, this.BarrierTagToRemove);
		}

		// Token: 0x0400173F RID: 5951
		private const float _modifierFactorUpperLimit = 1.2f;

		// Token: 0x04001740 RID: 5952
		private const float _modifierFactorLowerLimit = 0.8f;

		// Token: 0x04001741 RID: 5953
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame wait_pos_ground = MatrixFrame.Zero;

		// Token: 0x04001742 RID: 5954
		[EditorVisibleScriptComponentVariable(true)]
		public string SideTag;

		// Token: 0x04001743 RID: 5955
		[EditorVisibleScriptComponentVariable(true)]
		public string TargetWallSegmentTag = "";

		// Token: 0x04001744 RID: 5956
		[EditorVisibleScriptComponentVariable(true)]
		public string PathEntityName = "Path";

		// Token: 0x04001745 RID: 5957
		[EditorVisibleScriptComponentVariable(true)]
		public int SoilNavMeshID1 = -1;

		// Token: 0x04001746 RID: 5958
		[EditorVisibleScriptComponentVariable(true)]
		public int SoilNavMeshID2 = -1;

		// Token: 0x04001747 RID: 5959
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID1 = -1;

		// Token: 0x04001748 RID: 5960
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID2 = -1;

		// Token: 0x04001749 RID: 5961
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToSoilNavMeshID1 = -1;

		// Token: 0x0400174A RID: 5962
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToSoilNavMeshID2 = -1;

		// Token: 0x0400174B RID: 5963
		[EditorVisibleScriptComponentVariable(true)]
		public int SoilGenericNavMeshID = -1;

		// Token: 0x0400174C RID: 5964
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundGenericNavMeshID = -1;

		// Token: 0x0400174D RID: 5965
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x0400174E RID: 5966
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x0400174F RID: 5967
		[EditorVisibleScriptComponentVariable(true)]
		public float RampRotationDegree;

		// Token: 0x04001750 RID: 5968
		[EditorVisibleScriptComponentVariable(true)]
		public float BarrierLength = 1f;

		// Token: 0x04001751 RID: 5969
		[EditorVisibleScriptComponentVariable(true)]
		public float SpeedModifierFactor = 1f;

		// Token: 0x04001752 RID: 5970
		public bool EnableAutoGhostMovement;

		// Token: 0x04001753 RID: 5971
		[SpawnerBase.SpawnerPermissionField]
		[RestrictedAccess]
		public MatrixFrame ai_barrier_l = MatrixFrame.Zero;

		// Token: 0x04001754 RID: 5972
		[SpawnerBase.SpawnerPermissionField]
		[RestrictedAccess]
		public MatrixFrame ai_barrier_r = MatrixFrame.Zero;

		// Token: 0x04001755 RID: 5973
		[EditorVisibleScriptComponentVariable(true)]
		public string BarrierTagToRemove = string.Empty;
	}
}
