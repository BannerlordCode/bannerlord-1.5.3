using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BE RID: 958
	public class MultiplayerFireMangonelSpawner : MangonelSpawner
	{
		// Token: 0x06003622 RID: 13858 RVA: 0x000DF572 File Offset: 0x000DD772
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelperFire = new SpawnerEntityMissionHelper(this, true);
		}
	}
}
