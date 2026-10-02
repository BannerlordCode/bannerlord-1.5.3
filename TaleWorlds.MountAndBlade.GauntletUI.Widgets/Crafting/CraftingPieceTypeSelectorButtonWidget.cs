using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016D RID: 365
	public class CraftingPieceTypeSelectorButtonWidget : ButtonWidget
	{
		// Token: 0x0600135B RID: 4955 RVA: 0x00035024 File Offset: 0x00033224
		public CraftingPieceTypeSelectorButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0003502D File Offset: 0x0003322D
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			Widget visualsWidget = this.VisualsWidget;
			if (visualsWidget == null)
			{
				return;
			}
			visualsWidget.SetState(stateName);
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00035047 File Offset: 0x00033247
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x0003504F File Offset: 0x0003324F
		public Widget VisualsWidget
		{
			get
			{
				return this._visualsWidget;
			}
			set
			{
				if (value != this._visualsWidget)
				{
					this._visualsWidget = value;
				}
			}
		}

		// Token: 0x040008D1 RID: 2257
		private Widget _visualsWidget;
	}
}
