using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000126 RID: 294
	public readonly struct ClanCardSelectionInfo
	{
		// Token: 0x06001A8D RID: 6797 RVA: 0x0006455D File Offset: 0x0006275D
		public ClanCardSelectionInfo(TextObject title, IEnumerable<ClanCardSelectionItemInfo> items, Action<List<object>, Action> onClosedAction, bool isMultiSelection, int minimumSelection = 1, int maximumSelection = 0)
		{
			this.Title = title;
			this.Items = items;
			this.OnClosedAction = onClosedAction;
			this.IsMultiSelection = isMultiSelection;
			this.MinimumSelection = minimumSelection;
			this.MaximumSelection = maximumSelection;
		}

		// Token: 0x04000C3F RID: 3135
		public readonly TextObject Title;

		// Token: 0x04000C40 RID: 3136
		public readonly IEnumerable<ClanCardSelectionItemInfo> Items;

		// Token: 0x04000C41 RID: 3137
		public readonly Action<List<object>, Action> OnClosedAction;

		// Token: 0x04000C42 RID: 3138
		public readonly bool IsMultiSelection;

		// Token: 0x04000C43 RID: 3139
		public readonly int MinimumSelection;

		// Token: 0x04000C44 RID: 3140
		public readonly int MaximumSelection;
	}
}
