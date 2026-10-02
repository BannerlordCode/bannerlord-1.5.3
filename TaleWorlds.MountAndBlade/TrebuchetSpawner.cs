using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000369 RID: 873
	public class TrebuchetSpawner : SpawnerBase
	{
		// Token: 0x06003239 RID: 12857 RVA: 0x000CD9F8 File Offset: 0x000CBBF8
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000CDA1C File Offset: 0x000CBC1C
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			foreach (GameEntity gameEntity in _spawnerMissionHelper.SpawnedEntity.GetChildren())
			{
				if (gameEntity.GetFirstScriptOfType<Trebuchet>() != null)
				{
					gameEntity.GetFirstScriptOfType<Trebuchet>().AddOnDeployTag = this.AddOnDeployTag;
					gameEntity.GetFirstScriptOfType<Trebuchet>().RemoveOnDeployTag = this.RemoveOnDeployTag;
					break;
				}
			}
		}

		// Token: 0x0400153F RID: 5439
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame projectile_pile = MatrixFrame.Zero;

		// Token: 0x04001540 RID: 5440
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x04001541 RID: 5441
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x04001542 RID: 5442
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_a_enabled = true;

		// Token: 0x04001543 RID: 5443
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_b_enabled = true;

		// Token: 0x04001544 RID: 5444
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_c_enabled = true;

		// Token: 0x04001545 RID: 5445
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_d_enabled = true;

		// Token: 0x04001546 RID: 5446
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_e_enabled = true;

		// Token: 0x04001547 RID: 5447
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_f_enabled = true;

		// Token: 0x04001548 RID: 5448
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_g_enabled = true;

		// Token: 0x04001549 RID: 5449
		[EditorVisibleScriptComponentVariable(true)]
		public bool ammo_pos_h_enabled = true;
	}
}
