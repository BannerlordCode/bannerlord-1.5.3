using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D6 RID: 726
	public class SiegeSpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06002A1A RID: 10778 RVA: 0x0009EEF0 File Offset: 0x0009D0F0
		public override void Initialize()
		{
			base.Initialize();
			this._spawnPointsByTeam = new List<GameEntity>[2];
			this._spawnZonesByTeam = new List<GameEntity>[2];
			this._spawnPointsByTeam[1] = this.SpawnPoints.Where<GameEntity>((GameEntity x) => x.HasTag("attacker")).ToList<GameEntity>();
			this._spawnPointsByTeam[0] = this.SpawnPoints.Where<GameEntity>((GameEntity x) => x.HasTag("defender")).ToList<GameEntity>();
			this._spawnZonesByTeam[1] = (from sz in this._spawnPointsByTeam[1].Select<GameEntity, GameEntity>((GameEntity sp) => sp.Parent).Distinct<GameEntity>()
				where sz != null
				select sz).ToList<GameEntity>();
			this._spawnZonesByTeam[0] = (from sz in this._spawnPointsByTeam[0].Select<GameEntity, GameEntity>((GameEntity sp) => sp.Parent).Distinct<GameEntity>()
				where sz != null
				select sz).ToList<GameEntity>();
			this._activeSpawnZoneIndex = 0;
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x0009F054 File Offset: 0x0009D254
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			List<GameEntity> list = new List<GameEntity>();
			GameEntity gameEntity = this._spawnZonesByTeam[(int)team.Side].First<GameEntity>((GameEntity sz) => sz.HasTag(string.Format("{0}{1}", "sp_zone_", this._activeSpawnZoneIndex)));
			list.AddRange(from sp in gameEntity.GetChildren()
				where sp.HasTag("spawnpoint")
				select sp);
			return base.GetSpawnFrameFromSpawnPoints(list, team, hasMount);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x0009F0BF File Offset: 0x0009D2BF
		public void OnFlagDeactivated(FlagCapturePoint flag)
		{
			this._activeSpawnZoneIndex++;
		}

		// Token: 0x04001020 RID: 4128
		public const string SpawnZoneTagAffix = "sp_zone_";

		// Token: 0x04001021 RID: 4129
		public const string SpawnZoneEnableTagAffix = "enable_";

		// Token: 0x04001022 RID: 4130
		public const string SpawnZoneDisableTagAffix = "disable_";

		// Token: 0x04001023 RID: 4131
		public const int StartingActiveSpawnZoneIndex = 0;

		// Token: 0x04001024 RID: 4132
		private List<GameEntity>[] _spawnPointsByTeam;

		// Token: 0x04001025 RID: 4133
		private List<GameEntity>[] _spawnZonesByTeam;

		// Token: 0x04001026 RID: 4134
		private int _activeSpawnZoneIndex;
	}
}
