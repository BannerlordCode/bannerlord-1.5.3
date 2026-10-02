using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021B RID: 539
	public struct FormationSceneSpawnEntry
	{
		// Token: 0x06001F7E RID: 8062 RVA: 0x0006D7C6 File Offset: 0x0006B9C6
		public FormationSceneSpawnEntry(FormationClass formationClass, GameEntity spawnEntity, GameEntity reinforcementSpawnEntity)
		{
			this.FormationClass = formationClass;
			this.SpawnEntity = spawnEntity;
			this.ReinforcementSpawnEntity = reinforcementSpawnEntity;
		}

		// Token: 0x04000AC7 RID: 2759
		public readonly FormationClass FormationClass;

		// Token: 0x04000AC8 RID: 2760
		public readonly GameEntity SpawnEntity;

		// Token: 0x04000AC9 RID: 2761
		public readonly GameEntity ReinforcementSpawnEntity;
	}
}
