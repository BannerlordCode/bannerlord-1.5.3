using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000082 RID: 130
	public class SettlementNameplateManagerWidget : Widget
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x0001583D File Offset: 0x00013A3D
		public SettlementNameplateManagerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001585C File Offset: 0x00013A5C
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			this._visibleNameplates.Clear();
			for (int i = 0; i < this._allChildrenNameplates.Count; i++)
			{
				SettlementNameplateWidget settlementNameplateWidget = this._allChildrenNameplates[i];
				if (settlementNameplateWidget != null && settlementNameplateWidget.IsVisibleOnMap)
				{
					this._visibleNameplates.Add(settlementNameplateWidget);
				}
			}
			this._visibleNameplates.Sort();
			for (int j = 0; j < this._visibleNameplates.Count; j++)
			{
				SettlementNameplateWidget settlementNameplateWidget2 = this._visibleNameplates[j];
				settlementNameplateWidget2.DisableRender = false;
				settlementNameplateWidget2.Render(twoDimensionContext, drawContext);
				settlementNameplateWidget2.DisableRender = true;
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000158F0 File Offset: 0x00013AF0
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.DisableRender = true;
			this._allChildrenNameplates.Add(child as SettlementNameplateWidget);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00015911 File Offset: 0x00013B11
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			this._allChildrenNameplates.Remove(child as SettlementNameplateWidget);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001592C File Offset: 0x00013B2C
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			this._allChildrenNameplates.Clear();
			this._allChildrenNameplates = null;
		}

		// Token: 0x04000330 RID: 816
		private readonly List<SettlementNameplateWidget> _visibleNameplates = new List<SettlementNameplateWidget>();

		// Token: 0x04000331 RID: 817
		private List<SettlementNameplateWidget> _allChildrenNameplates = new List<SettlementNameplateWidget>();
	}
}
