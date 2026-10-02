using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007D RID: 125
	public class OptionsGamepadVisualWidget : Widget
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00014087 File Offset: 0x00012287
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001408F File Offset: 0x0001228F
		public Widget ParentAreaWidget { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00014098 File Offset: 0x00012298
		private float _verticalMarginBetweenKeys
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001409F File Offset: 0x0001229F
		public OptionsGamepadVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000140C0 File Offset: 0x000122C0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initalized)
			{
				using (List<Widget>.Enumerator enumerator = base.ParentWidget.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						OptionsGamepadKeyLocationWidget optionsGamepadKeyLocationWidget;
						if ((optionsGamepadKeyLocationWidget = enumerator.Current as OptionsGamepadKeyLocationWidget) != null)
						{
							this._allKeyLocations.Add(optionsGamepadKeyLocationWidget);
						}
					}
				}
				this._initalized = true;
			}
			if (this._isKeysDirty)
			{
				this._allKeyLocations.ForEach(delegate(OptionsGamepadKeyLocationWidget k)
				{
					k.SetKeyProperties(string.Empty, this.ParentAreaWidget);
				});
				foreach (Widget widget in base.Children)
				{
					OptionsGamepadOptionItemListPanel optionItem;
					if ((optionItem = widget as OptionsGamepadOptionItemListPanel) != null)
					{
						OptionsGamepadKeyLocationWidget optionsGamepadKeyLocationWidget2 = this._allKeyLocations.Find((OptionsGamepadKeyLocationWidget l) => l.KeyID == optionItem.KeyId);
						if (optionsGamepadKeyLocationWidget2 != null)
						{
							optionItem.SetKeyProperties(optionsGamepadKeyLocationWidget2, this.ParentAreaWidget);
						}
						else
						{
							optionItem.IsVisible = false;
						}
					}
				}
				this._isKeysDirty = false;
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000141F8 File Offset: 0x000123F8
		private void OnActionTextChanged()
		{
			this._isKeysDirty = true;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00014204 File Offset: 0x00012404
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this._isKeysDirty = true;
			OptionsGamepadOptionItemListPanel optionsGamepadOptionItemListPanel;
			if ((optionsGamepadOptionItemListPanel = child as OptionsGamepadOptionItemListPanel) != null && !this._allChildKeyItems.Contains(optionsGamepadOptionItemListPanel))
			{
				this._allChildKeyItems.Add(optionsGamepadOptionItemListPanel);
				optionsGamepadOptionItemListPanel.OnActionTextChanged += this.OnActionTextChanged;
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00014258 File Offset: 0x00012458
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			this._isKeysDirty = true;
			OptionsGamepadOptionItemListPanel optionsGamepadOptionItemListPanel;
			if ((optionsGamepadOptionItemListPanel = child as OptionsGamepadOptionItemListPanel) != null && this._allChildKeyItems.Contains(optionsGamepadOptionItemListPanel))
			{
				this._allChildKeyItems.Remove(optionsGamepadOptionItemListPanel);
				optionsGamepadOptionItemListPanel.OnActionTextChanged -= this.OnActionTextChanged;
			}
		}

		// Token: 0x040002F3 RID: 755
		private List<OptionsGamepadKeyLocationWidget> _allKeyLocations = new List<OptionsGamepadKeyLocationWidget>();

		// Token: 0x040002F4 RID: 756
		private List<OptionsGamepadOptionItemListPanel> _allChildKeyItems = new List<OptionsGamepadOptionItemListPanel>();

		// Token: 0x040002F6 RID: 758
		private bool _initalized;

		// Token: 0x040002F7 RID: 759
		private bool _isKeysDirty;
	}
}
