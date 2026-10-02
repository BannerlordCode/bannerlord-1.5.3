using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003C0 RID: 960
	public class MultiplayerMangonelSpawner : MangonelSpawner
	{
		// Token: 0x06003626 RID: 13862 RVA: 0x000DF5A0 File Offset: 0x000DD7A0
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}
	}
}
