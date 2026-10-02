using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003C3 RID: 963
	public class SiegeLadderSpawner : SpawnerBase
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x000DF614 File Offset: 0x000DD814
		public float UpperStateRotationRadian
		{
			get
			{
				return this.UpperStateRotationDegree * 0.017453292f;
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x000DF622 File Offset: 0x000DD822
		public float DownStateRotationRadian
		{
			get
			{
				return this.DownStateRotationDegree * 0.017453292f;
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000DF630 File Offset: 0x000DD830
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._spawnerEditorHelper = new SpawnerEntityEditorHelper(this);
			if (this._spawnerEditorHelper.IsValid)
			{
				this._spawnerEditorHelper.GivePermission("ladder_up_state", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.rotation, SpawnerEntityEditorHelper.Axis.x), new Action<float>(this.OnLadderUpStateChange));
				this._spawnerEditorHelper.GivePermission("ladder_down_state", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.rotation, SpawnerEntityEditorHelper.Axis.x), new Action<float>(this.OnLadderDownStateChange));
			}
			this.OnEditorVariableChanged("UpperStateRotationDegree");
			this.OnEditorVariableChanged("DownStateRotationDegree");
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000DF6B8 File Offset: 0x000DD8B8
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._spawnerEditorHelper.Tick(dt);
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000DF6CD File Offset: 0x000DD8CD
		private void OnLadderUpStateChange(float rotation)
		{
			if (rotation > -0.20135832f)
			{
				rotation = -0.20135832f;
				this.UpperStateRotationDegree = rotation * 57.29578f;
				this.OnEditorVariableChanged("UpperStateRotationDegree");
				return;
			}
			this.UpperStateRotationDegree = rotation * 57.29578f;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000DF704 File Offset: 0x000DD904
		private void OnLadderDownStateChange(float unusedArgument)
		{
			GameEntity ghostEntityOrChild = this._spawnerEditorHelper.GetGhostEntityOrChild("ladder_down_state");
			this.DownStateRotationDegree = Vec3.AngleBetweenTwoVectors(Vec3.Up, ghostEntityOrChild.GetFrame().rotation.u) * 57.29578f;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000DF748 File Offset: 0x000DD948
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "UpperStateRotationDegree")
			{
				if (this.UpperStateRotationDegree > -11.536982f)
				{
					this.UpperStateRotationDegree = -11.536982f;
				}
				MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ladder_up_state").GetFrame();
				frame.rotation = Mat3.Identity;
				frame.rotation.RotateAboutSide(this.UpperStateRotationRadian);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ladder_up_state", frame, true);
				return;
			}
			if (variableName == "DownStateRotationDegree")
			{
				MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ladder_down_state").GetFrame();
				frame2.rotation = Mat3.Identity;
				frame2.rotation.RotateAboutUp(1.5707964f);
				frame2.rotation.RotateAboutSide(this.DownStateRotationRadian);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ladder_down_state", frame2, true);
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000DF82C File Offset: 0x000DDA2C
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (base.Scene.IsMultiplayerScene())
			{
				if (this.OnWallNavMeshId == 0 || this.OnWallNavMeshId % 10 == 1)
				{
					MBEditor.AddEntityWarning(base.GameEntity, "OnWallNavMeshId's ones digit cannot be 1 and OnWallNavMeshId cannot be 0 in a multiplayer scene.");
					flag = true;
				}
			}
			else if (this.OnWallNavMeshId == -1 || this.OnWallNavMeshId == 0 || this.OnWallNavMeshId % 10 == 1)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "OnWallNavMeshId's ones digit cannot be 1 and OnWallNavMeshId cannot be -1 or 0 in a singleplayer scene.");
				flag = true;
			}
			if (this.OnWallNavMeshId != -1)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.Scene.GetEntitiesAsWeak(ref list);
				foreach (WeakGameEntity weakGameEntity in list)
				{
					SiegeLadderSpawner firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SiegeLadderSpawner>();
					if (firstScriptOfType != null && weakGameEntity != base.GameEntity && this.OnWallNavMeshId == firstScriptOfType.OnWallNavMeshId && base.GameEntity.GetVisibilityLevelMaskIncludingParents() == weakGameEntity.GetVisibilityLevelMaskIncludingParents())
					{
						MBEditor.AddEntityWarning(base.GameEntity, "OnWallNavMeshId must not be shared with any other siege ladder.");
					}
				}
			}
			return flag;
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000DF950 File Offset: 0x000DDB50
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000DF968 File Offset: 0x000DDB68
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			SiegeLadder firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<SiegeLadder>();
			firstScriptOfType.AddOnDeployTag = this.AddOnDeployTag;
			firstScriptOfType.RemoveOnDeployTag = this.RemoveOnDeployTag;
			firstScriptOfType.AssignParametersFromSpawner(this.SideTag, this.TargetWallSegmentTag, this.OnWallNavMeshId, this.DownStateRotationRadian, this.UpperStateRotationRadian, this.BarrierTagToRemove, this.IndestructibleMerlonsTag);
			List<GameEntity> list = new List<GameEntity>();
			_spawnerMissionHelper.SpawnedEntity.GetChildrenRecursive(ref list);
			list.Find((GameEntity x) => x.Name == "initial_wait_pos").GetFirstScriptOfType<TacticalPosition>().SetWidth(this.TacticalPositionWidth);
		}

		// Token: 0x0400172F RID: 5935
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame fork_holder = MatrixFrame.Zero;

		// Token: 0x04001730 RID: 5936
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame initial_wait_pos = MatrixFrame.Zero;

		// Token: 0x04001731 RID: 5937
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame use_push = MatrixFrame.Zero;

		// Token: 0x04001732 RID: 5938
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame stand_position_wall_push = MatrixFrame.Zero;

		// Token: 0x04001733 RID: 5939
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame distance_holder = MatrixFrame.Zero;

		// Token: 0x04001734 RID: 5940
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame stand_position_ground_wait = MatrixFrame.Zero;

		// Token: 0x04001735 RID: 5941
		[EditorVisibleScriptComponentVariable(true)]
		public string SideTag;

		// Token: 0x04001736 RID: 5942
		[EditorVisibleScriptComponentVariable(true)]
		public string TargetWallSegmentTag = "";

		// Token: 0x04001737 RID: 5943
		[EditorVisibleScriptComponentVariable(true)]
		public int OnWallNavMeshId = -1;

		// Token: 0x04001738 RID: 5944
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x04001739 RID: 5945
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x0400173A RID: 5946
		[EditorVisibleScriptComponentVariable(true)]
		public float UpperStateRotationDegree;

		// Token: 0x0400173B RID: 5947
		[EditorVisibleScriptComponentVariable(true)]
		public float DownStateRotationDegree = 90f;

		// Token: 0x0400173C RID: 5948
		public float TacticalPositionWidth = 1f;

		// Token: 0x0400173D RID: 5949
		[EditorVisibleScriptComponentVariable(true)]
		public string BarrierTagToRemove = string.Empty;

		// Token: 0x0400173E RID: 5950
		[EditorVisibleScriptComponentVariable(true)]
		public string IndestructibleMerlonsTag = string.Empty;
	}
}
