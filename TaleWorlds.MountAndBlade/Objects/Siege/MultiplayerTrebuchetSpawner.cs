using System;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003C2 RID: 962
	public class MultiplayerTrebuchetSpawner : TrebuchetSpawner
	{
		// Token: 0x0600362A RID: 13866 RVA: 0x000DF5FD File Offset: 0x000DD7FD
		protected internal override void OnPreInit()
		{
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}
	}
}
