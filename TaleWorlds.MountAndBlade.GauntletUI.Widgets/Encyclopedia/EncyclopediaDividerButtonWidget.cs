using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015B RID: 347
	public class EncyclopediaDividerButtonWidget : ButtonWidget
	{
		// Token: 0x06001290 RID: 4752 RVA: 0x00033600 File Offset: 0x00031800
		public EncyclopediaDividerButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00033609 File Offset: 0x00031809
		protected override void HandleClick()
		{
			base.HandleClick();
			this.UpdateItemListVisibility();
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0003361D File Offset: 0x0003181D
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.IsVisible = this.ItemListWidget.ChildCount > 0;
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00033640 File Offset: 0x00031840
		private void UpdateItemListVisibility()
		{
			if (this.ItemListWidget != null && this.ItemListWidget != null)
			{
				this.ItemListWidget.IsVisible = !this.ItemListWidget.IsVisible;
			}
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0003366C File Offset: 0x0003186C
		private void UpdateCollapseIndicator()
		{
			if (this.ItemListWidget != null && this.ItemListWidget != null && this.CollapseIndicator != null)
			{
				if (this.ItemListWidget.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x000336BF File Offset: 0x000318BF
		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x000336E7 File Offset: 0x000318E7
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x000336EF File Offset: 0x000318EF
		public Widget ItemListWidget
		{
			get
			{
				return this._itemListWidget;
			}
			set
			{
				if (value != this._itemListWidget)
				{
					this._itemListWidget = value;
					base.OnPropertyChanged<Widget>(value, "ItemListWidget");
				}
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0003370D File Offset: 0x0003190D
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x00033715 File Offset: 0x00031915
		public Widget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (value != this._collapseIndicator)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged<Widget>(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		// Token: 0x0400087C RID: 2172
		private Widget _itemListWidget;

		// Token: 0x0400087D RID: 2173
		private Widget _collapseIndicator;
	}
}
