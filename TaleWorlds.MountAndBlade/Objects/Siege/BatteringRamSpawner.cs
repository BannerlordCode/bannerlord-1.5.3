using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B7 RID: 951
	public class BatteringRamSpawner : SpawnerBase
	{
		// Token: 0x0600360D RID: 13837 RVA: 0x000DEFC6 File Offset: 0x000DD1C6
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._spawnerEditorHelper = new SpawnerEntityEditorHelper(this);
			this._spawnerEditorHelper.LockGhostParent = false;
			if (this._spawnerEditorHelper.IsValid)
			{
				this._spawnerEditorHelper.SetupGhostMovement(this.PathEntityName);
			}
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000DF004 File Offset: 0x000DD204
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._spawnerEditorHelper.Tick(dt);
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000DF01C File Offset: 0x000DD21C
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
			if (variableName == "SpeedModifierFactor")
			{
				this.SpeedModifierFactor = MathF.Clamp(this.SpeedModifierFactor, 0.8f, 1.2f);
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000DF098 File Offset: 0x000DD298
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (!base.Scene.IsMultiplayerScene() && base.Scene.FindWeakEntitiesWithTag("ditch_filler").FirstOrDefault<WeakGameEntity>((WeakGameEntity df) => df.HasTag(this.SideTag)) != null)
			{
				if (this.DitchNavMeshID_1 >= 0 && !base.Scene.IsAnyFaceWithId(this.DitchNavMeshID_1))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'DitchNavMeshID_1' id.");
					flag = true;
				}
				if (this.DitchNavMeshID_2 >= 0 && !base.Scene.IsAnyFaceWithId(this.DitchNavMeshID_2))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'DitchNavMeshID_2' id.");
					flag = true;
				}
				if (this.GroundToBridgeNavMeshID_1 >= 0 && !base.Scene.IsAnyFaceWithId(this.GroundToBridgeNavMeshID_1))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'GroundToBridgeNavMeshID_1' id.");
					flag = true;
				}
				if (this.GroundToBridgeNavMeshID_2 >= 0 && !base.Scene.IsAnyFaceWithId(this.GroundToBridgeNavMeshID_2))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'GroundToBridgeNavMeshID_1' id.");
					flag = true;
				}
				if (this.BridgeNavMeshID_1 >= 0 && !base.Scene.IsAnyFaceWithId(this.BridgeNavMeshID_1))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'BridgeNavMeshID_1' id.");
					flag = true;
				}
				if (this.BridgeNavMeshID_2 >= 0 && !base.Scene.IsAnyFaceWithId(this.BridgeNavMeshID_2))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'BridgeNavMeshID_2' id.");
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000DF1FD File Offset: 0x000DD3FD
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000DF214 File Offset: 0x000DD414
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			BatteringRam firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<BatteringRam>();
			firstScriptOfType.AddOnDeployTag = this.AddOnDeployTag;
			firstScriptOfType.RemoveOnDeployTag = this.RemoveOnDeployTag;
			firstScriptOfType.MaxSpeed *= this.SpeedModifierFactor;
			firstScriptOfType.MinSpeed *= this.SpeedModifierFactor;
			firstScriptOfType.AssignParametersFromSpawner(this.GateTag, this.SideTag, this.BridgeNavMeshID_1, this.BridgeNavMeshID_2, this.DitchNavMeshID_1, this.DitchNavMeshID_2, this.GroundToBridgeNavMeshID_1, this.GroundToBridgeNavMeshID_2, this.PathEntityName);
		}

		// Token: 0x0400170F RID: 5903
		private const float _modifierFactorUpperLimit = 1.2f;

		// Token: 0x04001710 RID: 5904
		private const float _modifierFactorLowerLimit = 0.8f;

		// Token: 0x04001711 RID: 5905
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame wait_pos_ground = MatrixFrame.Zero;

		// Token: 0x04001712 RID: 5906
		[EditorVisibleScriptComponentVariable(true)]
		public string SideTag;

		// Token: 0x04001713 RID: 5907
		[EditorVisibleScriptComponentVariable(true)]
		public string GateTag = "";

		// Token: 0x04001714 RID: 5908
		[EditorVisibleScriptComponentVariable(true)]
		public string PathEntityName = "Path";

		// Token: 0x04001715 RID: 5909
		[EditorVisibleScriptComponentVariable(true)]
		public int BridgeNavMeshID_1 = 8;

		// Token: 0x04001716 RID: 5910
		[EditorVisibleScriptComponentVariable(true)]
		public int BridgeNavMeshID_2 = 8;

		// Token: 0x04001717 RID: 5911
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID_1 = 9;

		// Token: 0x04001718 RID: 5912
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID_2 = 10;

		// Token: 0x04001719 RID: 5913
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToBridgeNavMeshID_1 = 12;

		// Token: 0x0400171A RID: 5914
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToBridgeNavMeshID_2 = 13;

		// Token: 0x0400171B RID: 5915
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x0400171C RID: 5916
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x0400171D RID: 5917
		[EditorVisibleScriptComponentVariable(true)]
		public float SpeedModifierFactor = 1f;

		// Token: 0x0400171E RID: 5918
		public bool EnableAutoGhostMovement;
	}
}
