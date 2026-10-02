using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BB RID: 955
	public class MultiplayerBallistaSpawner : BallistaSpawner
	{
		// Token: 0x0600361C RID: 13852 RVA: 0x000DF4E4 File Offset: 0x000DD6E4
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}
	}
}
