using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D4 RID: 724
	public class FFASpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06002A13 RID: 10771 RVA: 0x0009E9ED File Offset: 0x0009CBED
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			return base.GetSpawnFrameFromSpawnPoints(this.SpawnPoints.ToList<GameEntity>(), null, hasMount);
		}
	}
}
