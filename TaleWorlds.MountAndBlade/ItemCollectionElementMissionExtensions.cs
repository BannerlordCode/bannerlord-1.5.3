using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000251 RID: 593
	public static class ItemCollectionElementMissionExtensions
	{
		// Token: 0x06002225 RID: 8741 RVA: 0x00077D60 File Offset: 0x00075F60
		public static StackArray.StackArray4Int GetItemHolsterIndices(this ItemObject item)
		{
			StackArray.StackArray4Int stackArray4Int = default(StackArray.StackArray4Int);
			for (int i = 0; i < item.ItemHolsters.Length; i++)
			{
				stackArray4Int[i] = ((item.ItemHolsters[i].Length > 0) ? MBItem.GetItemHolsterIndex(item.ItemHolsters[i]) : (-1));
			}
			for (int j = item.ItemHolsters.Length; j < 4; j++)
			{
				stackArray4Int[j] = -1;
			}
			return stackArray4Int;
		}
	}
}
