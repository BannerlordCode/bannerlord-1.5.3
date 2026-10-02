using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BA RID: 954
	public class MangonelSpawner : SpawnerBase
	{
		// Token: 0x06003619 RID: 13849 RVA: 0x000DF3DC File Offset: 0x000DD5DC
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000DF400 File Offset: 0x000DD600
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			foreach (GameEntity gameEntity in _spawnerMissionHelper.SpawnedEntity.GetChildren())
			{
				if (gameEntity.GetFirstScriptOfType<Mangonel>() != null)
				{
					gameEntity.GetFirstScriptOfType<Mangonel>().AddOnDeployTag = this.AddOnDeployTag;
					gameEntity.GetFirstScriptOfType<Mangonel>().RemoveOnDeployTag = this.RemoveOnDeployTag;
					break;
				}
			}
		}

		// Token: 0x0400171F RID: 5919
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame projectile_pile = MatrixFrame.Zero;

		// Token: 0x04001720 RID: 5920
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x04001721 RID: 5921
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x04001722 RID: 5922
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_a_enabled = true;

		// Token: 0x04001723 RID: 5923
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_b_enabled = true;

		// Token: 0x04001724 RID: 5924
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_c_enabled = true;

		// Token: 0x04001725 RID: 5925
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_d_enabled = true;

		// Token: 0x04001726 RID: 5926
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_e_enabled = true;

		// Token: 0x04001727 RID: 5927
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_f_enabled = true;

		// Token: 0x04001728 RID: 5928
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_g_enabled = true;

		// Token: 0x04001729 RID: 5929
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_h_enabled = true;
	}
}
