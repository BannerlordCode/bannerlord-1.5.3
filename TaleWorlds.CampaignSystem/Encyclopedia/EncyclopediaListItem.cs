using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200017E RID: 382
	public struct EncyclopediaListItem
	{
		// Token: 0x06001BFC RID: 7164 RVA: 0x000909F5 File Offset: 0x0008EBF5
		public EncyclopediaListItem(object obj, string name, string description, string id, string typeName, bool playerCanSeeValues, Action onShowTooltip = null)
		{
			this.Object = obj;
			this.Name = name;
			this.Description = description;
			this.Id = id;
			this.TypeName = typeName;
			this.PlayerCanSeeValues = playerCanSeeValues;
			this.OnShowTooltip = onShowTooltip;
		}

		// Token: 0x04000951 RID: 2385
		public readonly object Object;

		// Token: 0x04000952 RID: 2386
		public readonly string Name;

		// Token: 0x04000953 RID: 2387
		public readonly string Description;

		// Token: 0x04000954 RID: 2388
		public readonly string Id;

		// Token: 0x04000955 RID: 2389
		public readonly string TypeName;

		// Token: 0x04000956 RID: 2390
		public readonly bool PlayerCanSeeValues;

		// Token: 0x04000957 RID: 2391
		public readonly Action OnShowTooltip;
	}
}
