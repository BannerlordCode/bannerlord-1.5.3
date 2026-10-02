using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BF RID: 959
	public class MultiplayerFireTrebuchetSpawner : TrebuchetSpawner
	{
		// Token: 0x06003624 RID: 13860 RVA: 0x000DF589 File Offset: 0x000DD789
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}
	}
}
