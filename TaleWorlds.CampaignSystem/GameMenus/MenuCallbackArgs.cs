using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000F0 RID: 240
	public class MenuCallbackArgs
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x00065E31 File Offset: 0x00064031
		// (set) Token: 0x06001655 RID: 5717 RVA: 0x00065E39 File Offset: 0x00064039
		public MenuContext MenuContext { get; private set; }

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x00065E42 File Offset: 0x00064042
		// (set) Token: 0x06001657 RID: 5719 RVA: 0x00065E4A File Offset: 0x0006404A
		public MapState MapState { get; private set; }

		// Token: 0x06001658 RID: 5720 RVA: 0x00065E53 File Offset: 0x00064053
		public MenuCallbackArgs(MenuContext menuContext, TextObject text)
		{
			this.MenuContext = menuContext;
			this.Text = text;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00065E70 File Offset: 0x00064070
		public MenuCallbackArgs(MapState mapState, TextObject text)
		{
			this.MapState = mapState;
			this.Text = text;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00065E8D File Offset: 0x0006408D
		public MenuCallbackArgs(MapState mapState, TextObject text, float dt)
		{
			this.MapState = mapState;
			this.Text = text;
			this.DeltaTime = dt;
		}

		// Token: 0x04000754 RID: 1876
		public float DeltaTime;

		// Token: 0x04000755 RID: 1877
		public bool IsEnabled = true;

		// Token: 0x04000756 RID: 1878
		public TextObject Text;

		// Token: 0x04000757 RID: 1879
		public TextObject Tooltip;

		// Token: 0x04000758 RID: 1880
		public GameMenuOption.IssueQuestFlags OptionQuestData;

		// Token: 0x04000759 RID: 1881
		public GameMenuOption.LeaveType optionLeaveType;

		// Token: 0x0400075A RID: 1882
		public TextObject MenuTitle;
	}
}
