using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000025 RID: 37
	public class HoverToggleWidget : Widget
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00007505 File Offset: 0x00005705
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000750D File Offset: 0x0000570D
		public bool IsOverWidget { get; private set; }

		// Token: 0x060001F2 RID: 498 RVA: 0x00007516 File Offset: 0x00005716
		public HoverToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007520 File Offset: 0x00005720
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsVisible)
			{
				this.IsOverWidget = base.IsPointInsideMeasuredArea(base.EventManager.MousePosition);
				bool flag = Input.MouseMoveX != 0f || Input.MouseMoveY != 0f;
				if (this.IsOverWidget && !this._hoverBegan)
				{
					base.EventFired("HoverBegin", new object[] { flag });
					this._hoverBegan = true;
				}
				else if (!this.IsOverWidget && this._hoverBegan)
				{
					base.EventFired("HoverEnd", new object[] { flag });
					this._hoverBegan = false;
				}
				if (this.WidgetToShow != null)
				{
					this.WidgetToShow.IsVisible = this._hoverBegan;
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000075F2 File Offset: 0x000057F2
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x000075FA File Offset: 0x000057FA
		[Editor(false)]
		public Widget WidgetToShow
		{
			get
			{
				return this._widgetToShow;
			}
			set
			{
				if (this._widgetToShow != value)
				{
					this._widgetToShow = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToShow");
				}
			}
		}

		// Token: 0x040000EA RID: 234
		private bool _hoverBegan;

		// Token: 0x040000EB RID: 235
		private Widget _widgetToShow;
	}
}
