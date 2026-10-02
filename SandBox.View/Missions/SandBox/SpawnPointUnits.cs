using System;

namespace SandBox.View.Missions.SandBox
{
	// Token: 0x0200002E RID: 46
	public class SpawnPointUnits
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000D552 File Offset: 0x0000B752
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000D55A File Offset: 0x0000B75A
		public string SpName { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000D563 File Offset: 0x0000B763
		// (set) Token: 0x06000131 RID: 305 RVA: 0x0000D56B File Offset: 0x0000B76B
		public SpawnPointUnits.SceneType Place { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000D574 File Offset: 0x0000B774
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000D57C File Offset: 0x0000B77C
		public int MinCount { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000D585 File Offset: 0x0000B785
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000D58D File Offset: 0x0000B78D
		public int MaxCount { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000D596 File Offset: 0x0000B796
		// (set) Token: 0x06000137 RID: 311 RVA: 0x0000D59E File Offset: 0x0000B79E
		public string Type { get; private set; }

		// Token: 0x06000138 RID: 312 RVA: 0x0000D5A7 File Offset: 0x0000B7A7
		public SpawnPointUnits(string sp_name, SpawnPointUnits.SceneType place, int minCount, int maxCount)
		{
			this.SpName = sp_name;
			this.Place = place;
			this.MinCount = minCount;
			this.MaxCount = maxCount;
			this.CurrentCount = 0;
			this.SpawnedAgentCount = 0;
			this.Type = "other";
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000D5E5 File Offset: 0x0000B7E5
		public SpawnPointUnits(string sp_name, SpawnPointUnits.SceneType place, string type, int minCount, int maxCount)
		{
			this.SpName = sp_name;
			this.Place = place;
			this.Type = type;
			this.MinCount = minCount;
			this.MaxCount = maxCount;
			this.CurrentCount = 0;
			this.SpawnedAgentCount = 0;
		}

		// Token: 0x0400008B RID: 139
		public int CurrentCount;

		// Token: 0x0400008D RID: 141
		public int SpawnedAgentCount;

		// Token: 0x02000099 RID: 153
		public enum SceneType
		{
			// Token: 0x04000301 RID: 769
			Center,
			// Token: 0x04000302 RID: 770
			Shipyard,
			// Token: 0x04000303 RID: 771
			Tavern,
			// Token: 0x04000304 RID: 772
			VillageCenter,
			// Token: 0x04000305 RID: 773
			Arena,
			// Token: 0x04000306 RID: 774
			LordsHall,
			// Token: 0x04000307 RID: 775
			Castle,
			// Token: 0x04000308 RID: 776
			Dungeon,
			// Token: 0x04000309 RID: 777
			EmptyShop,
			// Token: 0x0400030A RID: 778
			All,
			// Token: 0x0400030B RID: 779
			NotDetermined
		}
	}
}
