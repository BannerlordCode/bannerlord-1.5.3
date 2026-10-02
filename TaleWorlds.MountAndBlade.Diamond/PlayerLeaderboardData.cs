using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000149 RID: 329
	[Serializable]
	public class PlayerLeaderboardData
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0000D773 File Offset: 0x0000B973
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0000D77B File Offset: 0x0000B97B
		public PlayerId PlayerId { get; set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0000D784 File Offset: 0x0000B984
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x0000D78C File Offset: 0x0000B98C
		public string RankId { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0000D795 File Offset: 0x0000B995
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0000D79D File Offset: 0x0000B99D
		public int Rating { get; set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0000D7A6 File Offset: 0x0000B9A6
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0000D7AE File Offset: 0x0000B9AE
		public string Name { get; set; }

		// Token: 0x06000925 RID: 2341 RVA: 0x0000D7B7 File Offset: 0x0000B9B7
		public PlayerLeaderboardData(PlayerId playerId, string rankId, int rating, string name)
		{
			this.PlayerId = playerId;
			this.RankId = rankId;
			this.Rating = rating;
			this.Name = name;
		}
	}
}
