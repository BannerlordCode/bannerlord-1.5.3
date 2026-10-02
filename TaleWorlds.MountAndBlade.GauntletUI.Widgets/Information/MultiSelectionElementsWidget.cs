using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x0200014A RID: 330
	public class MultiSelectionElementsWidget : Widget
	{
		// Token: 0x06001184 RID: 4484 RVA: 0x00030609 File Offset: 0x0002E809
		public MultiSelectionElementsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0003061D File Offset: 0x0002E81D
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._updateRequired)
			{
				this.UpdateElementsList();
				this._updateRequired = false;
			}
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0003063B File Offset: 0x0002E83B
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			if (child is ListPanel)
			{
				this._elementContainer = child as ListPanel;
				this._elementContainer.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnElementAdded));
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00030674 File Offset: 0x0002E874
		private void OnElementAdded(Widget parentWidget, Widget addedWidget)
		{
			this._updateRequired = true;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00030680 File Offset: 0x0002E880
		private void UpdateElementsList()
		{
			this._elementsList.Clear();
			for (int i = 0; i < this._elementContainer.ChildCount; i++)
			{
				ButtonWidget buttonWidget = this._elementContainer.GetChild(i).GetChild(0) as ButtonWidget;
				this._elementsList.Add(buttonWidget);
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x000306D2 File Offset: 0x0002E8D2
		// (set) Token: 0x0600118A RID: 4490 RVA: 0x000306DA File Offset: 0x0002E8DA
		[Editor(false)]
		public ButtonWidget DoneButtonWidget
		{
			get
			{
				return this._doneButtonWidget;
			}
			set
			{
				if (this._doneButtonWidget != value)
				{
					this._doneButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DoneButtonWidget");
				}
			}
		}

		// Token: 0x040007F9 RID: 2041
		private bool _updateRequired;

		// Token: 0x040007FA RID: 2042
		private List<ButtonWidget> _elementsList = new List<ButtonWidget>();

		// Token: 0x040007FB RID: 2043
		private ButtonWidget _doneButtonWidget;

		// Token: 0x040007FC RID: 2044
		private ListPanel _elementContainer;
	}
}
