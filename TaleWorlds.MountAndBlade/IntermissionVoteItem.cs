using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000308 RID: 776
	public class IntermissionVoteItem
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000AB487 File Offset: 0x000A9687
		// (set) Token: 0x06002C6C RID: 11372 RVA: 0x000AB48F File Offset: 0x000A968F
		public int VoteCount { get; private set; }

		// Token: 0x06002C6D RID: 11373 RVA: 0x000AB498 File Offset: 0x000A9698
		public IntermissionVoteItem(string id, int index)
		{
			this.Id = id;
			this.Index = index;
			this.VoteCount = 0;
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000AB4B5 File Offset: 0x000A96B5
		public void SetVoteCount(int voteCount)
		{
			this.VoteCount = voteCount;
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000AB4BE File Offset: 0x000A96BE
		public void IncreaseVoteCount(int incrementAmount)
		{
			this.VoteCount += incrementAmount;
		}

		// Token: 0x0400117F RID: 4479
		public readonly string Id;

		// Token: 0x04001180 RID: 4480
		public readonly int Index;
	}
}
