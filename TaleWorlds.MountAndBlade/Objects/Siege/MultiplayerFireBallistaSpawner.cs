using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BD RID: 957
	public class MultiplayerFireBallistaSpawner : BallistaSpawner
	{
		// Token: 0x06003620 RID: 13856 RVA: 0x000DF55B File Offset: 0x000DD75B
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}
	}
}
