using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001C RID: 28
	public class DuelSpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00007164 File Offset: 0x00005364
		public override void Initialize()
		{
			base.Initialize();
			this._duelAreaSpawnPoints = new List<GameEntity>[16];
			this._spawnPointSelectors = new bool[16];
			foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTagExpression("spawnpoint_area(_\\d+)*"))
			{
				int num = int.Parse(gameEntity.Tags.Single<string>((string tag) => tag.StartsWith("spawnpoint_area_")).Replace("spawnpoint_area_", "")) - 1;
				if (this._duelAreaSpawnPoints[num] == null)
				{
					this._duelAreaSpawnPoints[num] = new List<GameEntity>();
				}
				this._duelAreaSpawnPoints[num].Add(gameEntity);
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007240 File Offset: 0x00005440
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			int duelAreaIndexIfDuelTeam = Mission.Current.GetMissionBehavior<MissionMultiplayerDuel>().GetDuelAreaIndexIfDuelTeam(team);
			List<GameEntity> list = ((duelAreaIndexIfDuelTeam >= 0) ? this._duelAreaSpawnPoints[duelAreaIndexIfDuelTeam].ToList<GameEntity>() : this.SpawnPoints.ToList<GameEntity>());
			if (duelAreaIndexIfDuelTeam >= 0)
			{
				list.RemoveAt(this._spawnPointSelectors[duelAreaIndexIfDuelTeam] ? 0 : 1);
				this._spawnPointSelectors[duelAreaIndexIfDuelTeam] = !this._spawnPointSelectors[duelAreaIndexIfDuelTeam];
			}
			return base.GetSpawnFrameFromSpawnPoints(list, team, hasMount);
		}

		// Token: 0x04000063 RID: 99
		private const string AreaSpawnPointTagExpression = "spawnpoint_area(_\\d+)*";

		// Token: 0x04000064 RID: 100
		private const string AreaSpawnPointTagPrefix = "spawnpoint_area_";

		// Token: 0x04000065 RID: 101
		private List<GameEntity>[] _duelAreaSpawnPoints;

		// Token: 0x04000066 RID: 102
		private bool[] _spawnPointSelectors;
	}
}
