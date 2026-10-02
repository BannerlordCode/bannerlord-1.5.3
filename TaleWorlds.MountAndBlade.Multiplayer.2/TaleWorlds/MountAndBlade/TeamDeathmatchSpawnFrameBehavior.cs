using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001D RID: 29
	public class TeamDeathmatchSpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06000194 RID: 404 RVA: 0x000072B9 File Offset: 0x000054B9
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			return base.GetSpawnFrameFromSpawnPoints(this.SpawnPoints.ToList<GameEntity>(), team, hasMount);
		}
	}
}
