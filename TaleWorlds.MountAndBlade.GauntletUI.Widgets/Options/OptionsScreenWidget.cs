using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000079 RID: 121
	public class OptionsScreenWidget : Widget
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x000138E1 File Offset: 0x00011AE1
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x000138E9 File Offset: 0x00011AE9
		public Widget VideoMemoryUsageWidget { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000138F2 File Offset: 0x00011AF2
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x000138FA File Offset: 0x00011AFA
		public RichTextWidget CurrentOptionDescriptionWidget { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00013903 File Offset: 0x00011B03
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x0001390B File Offset: 0x00011B0B
		public RichTextWidget CurrentOptionNameWidget { get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00013914 File Offset: 0x00011B14
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x0001391C File Offset: 0x00011B1C
		public RichTextWidget CurrentOptionExtraInformationWidget { get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00013925 File Offset: 0x00011B25
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0001392D File Offset: 0x00011B2D
		public Widget CurrentOptionImageWidget { get; set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00013936 File Offset: 0x00011B36
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0001393E File Offset: 0x00011B3E
		public TabToggleWidget PerformanceTabToggle { get; set; }

		// Token: 0x060006B1 RID: 1713 RVA: 0x00013947 File Offset: 0x00011B47
		public OptionsScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00013950 File Offset: 0x00011B50
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.PerformanceTabToggle.TabControlWidget.OnActiveTabChange += this.OnActiveTabChange;
				this.VideoMemoryUsageWidget.IsVisible = false;
				this._initialized = true;
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00013990 File Offset: 0x00011B90
		private void OnActiveTabChange()
		{
			this.VideoMemoryUsageWidget.IsVisible = this.PerformanceTabToggle.TabControlWidget.ActiveTab.Id == "PerformanceOptionsPage";
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000139BC File Offset: 0x00011BBC
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			TabToggleWidget performanceTabToggle = this.PerformanceTabToggle;
			if (((performanceTabToggle != null) ? performanceTabToggle.TabControlWidget : null) != null)
			{
				this.PerformanceTabToggle.TabControlWidget.OnActiveTabChange += this.OnActiveTabChange;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000139F4 File Offset: 0x00011BF4
		public void SetCurrentOption(Widget currentOptionWidget, Sprite newgraphicsSprite)
		{
			if (this._currentOptionWidget != currentOptionWidget)
			{
				this._currentOptionWidget = currentOptionWidget;
				string text = "";
				string text2 = "";
				string text3 = "";
				if (this._currentOptionWidget != null)
				{
					OptionsItemWidget optionsItemWidget;
					OptionsKeyItemListPanel optionsKeyItemListPanel;
					if ((optionsItemWidget = this._currentOptionWidget as OptionsItemWidget) != null)
					{
						text = optionsItemWidget.OptionDescription;
						text2 = optionsItemWidget.OptionTitle;
					}
					else if ((optionsKeyItemListPanel = this._currentOptionWidget as OptionsKeyItemListPanel) != null)
					{
						text = optionsKeyItemListPanel.OptionDescription;
						text2 = optionsKeyItemListPanel.OptionTitle;
						text3 = optionsKeyItemListPanel.OptionExtraInformation;
					}
				}
				if (this.CurrentOptionDescriptionWidget != null)
				{
					this.CurrentOptionDescriptionWidget.Text = text;
				}
				if (this.CurrentOptionNameWidget != null)
				{
					this.CurrentOptionNameWidget.Text = text2;
				}
				if (this.CurrentOptionExtraInformationWidget != null)
				{
					this.CurrentOptionExtraInformationWidget.Text = text3;
				}
			}
			if (this.CurrentOptionImageWidget != null && this.CurrentOptionImageWidget.Sprite != newgraphicsSprite)
			{
				this.CurrentOptionImageWidget.Sprite = newgraphicsSprite;
				if (newgraphicsSprite != null)
				{
					float num = this.CurrentOptionImageWidget.SuggestedWidth / (float)newgraphicsSprite.Width;
					this.CurrentOptionImageWidget.SuggestedHeight = (float)newgraphicsSprite.Height * num;
				}
			}
		}

		// Token: 0x040002D4 RID: 724
		private Widget _currentOptionWidget;

		// Token: 0x040002DB RID: 731
		private bool _initialized;
	}
}
