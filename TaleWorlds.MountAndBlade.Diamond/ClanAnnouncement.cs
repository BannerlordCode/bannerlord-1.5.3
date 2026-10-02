using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000101 RID: 257
	[Serializable]
	public class ClanAnnouncement
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00006FD6 File Offset: 0x000051D6
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00006FDE File Offset: 0x000051DE
		[JsonProperty]
		public int Id { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00006FE7 File Offset: 0x000051E7
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00006FEF File Offset: 0x000051EF
		[JsonProperty]
		public string Announcement { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00006FF8 File Offset: 0x000051F8
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00007000 File Offset: 0x00005200
		[JsonProperty]
		public PlayerId AuthorId { get; private set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00007009 File Offset: 0x00005209
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00007011 File Offset: 0x00005211
		[JsonProperty]
		public DateTime CreationTime { get; private set; }

		// Token: 0x0600058B RID: 1419 RVA: 0x0000701A File Offset: 0x0000521A
		public ClanAnnouncement(int id, string announcement, PlayerId authorId, DateTime creationTime)
		{
			this.Id = id;
			this.Announcement = announcement;
			this.AuthorId = authorId;
			this.CreationTime = creationTime;
		}
	}
}
