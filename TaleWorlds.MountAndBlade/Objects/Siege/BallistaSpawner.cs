using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B6 RID: 950
	public class BallistaSpawner : SpawnerBase
	{
		// Token: 0x0600360A RID: 13834 RVA: 0x000DEF23 File Offset: 0x000DD123
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000DEF48 File Offset: 0x000DD148
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			_spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<Ballista>().AddOnDeployTag = this.AddOnDeployTag;
			_spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<Ballista>().RemoveOnDeployTag = this.RemoveOnDeployTag;
			_spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<Ballista>().HorizontalDirectionRestriction = this.DirectionRestrictionDegree * 0.017453292f;
		}

		// Token: 0x0400170C RID: 5900
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x0400170D RID: 5901
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x0400170E RID: 5902
		[EditorVisibleScriptComponentVariable(true)]
		public float DirectionRestrictionDegree = 90f;
	}
}
