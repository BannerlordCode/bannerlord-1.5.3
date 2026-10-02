using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000133 RID: 307
	public class DecisionSupporterGridWidget : GridWidget
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x0002C955 File Offset: 0x0002AB55
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x0002C95D File Offset: 0x0002AB5D
		public int VisibleCount { get; set; } = 4;

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x0002C966 File Offset: 0x0002AB66
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x0002C96E File Offset: 0x0002AB6E
		public TextWidget MoreTextWidget { get; set; }

		// Token: 0x0600101D RID: 4125 RVA: 0x0002C977 File Offset: 0x0002AB77
		public DecisionSupporterGridWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0002C987 File Offset: 0x0002AB87
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.IsVisible = child.GetSiblingIndex() < this.VisibleCount;
			this.UpdateMoreText();
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0002C9AC File Offset: 0x0002ABAC
		private void UpdateMoreText()
		{
			if (this.MoreTextWidget != null)
			{
				this.MoreTextWidget.IsVisible = base.ChildCount > this.VisibleCount;
				if (this.MoreTextWidget.IsVisible)
				{
					this.MoreTextWidget.Text = "+" + (base.ChildCount - this.VisibleCount);
				}
			}
		}
	}
}
