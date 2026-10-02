using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000309 RID: 777
	public static class IntermissionVoteItemListExtensions
	{
		// Token: 0x06002C70 RID: 11376 RVA: 0x000AB4D0 File Offset: 0x000A96D0
		public static bool ContainsItem(this List<IntermissionVoteItem> intermissionVoteItems, string id)
		{
			return intermissionVoteItems != null && intermissionVoteItems.FirstOrDefault<IntermissionVoteItem>((IntermissionVoteItem item) => item.Id == id) != null;
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x000AB504 File Offset: 0x000A9704
		public static IntermissionVoteItem Add(this List<IntermissionVoteItem> intermissionVoteItems, string id)
		{
			IntermissionVoteItem intermissionVoteItem = null;
			if (intermissionVoteItems != null)
			{
				int count = intermissionVoteItems.Count;
				IntermissionVoteItem intermissionVoteItem2 = new IntermissionVoteItem(id, count);
				intermissionVoteItems.Add(intermissionVoteItem2);
				intermissionVoteItem = intermissionVoteItem2;
			}
			return intermissionVoteItem;
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000AB530 File Offset: 0x000A9730
		public static IntermissionVoteItem GetItem(this List<IntermissionVoteItem> intermissionVoteItems, string id)
		{
			IntermissionVoteItem intermissionVoteItem = null;
			if (intermissionVoteItems != null)
			{
				intermissionVoteItem = intermissionVoteItems.FirstOrDefault<IntermissionVoteItem>((IntermissionVoteItem item) => item.Id == id);
			}
			return intermissionVoteItem;
		}
	}
}
