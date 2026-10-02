using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000166 RID: 358
	public class CardSelectionPopupButtonWidget : ButtonWidget
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0003461C File Offset: 0x0003281C
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00034624 File Offset: 0x00032824
		public CircularAutoScrollablePanelWidget PropertiesContainer { get; set; }

		// Token: 0x060012FD RID: 4861 RVA: 0x0003462D File Offset: 0x0003282D
		public CardSelectionPopupButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00034636 File Offset: 0x00032836
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetState(stateName);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00034650 File Offset: 0x00032850
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetHoverBegin();
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00034668 File Offset: 0x00032868
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetHoverEnd();
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00034680 File Offset: 0x00032880
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetScrollMouse();
		}
	}
}
