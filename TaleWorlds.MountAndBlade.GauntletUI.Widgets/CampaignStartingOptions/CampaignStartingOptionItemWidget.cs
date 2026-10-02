using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CampaignStartingOptions
{
	// Token: 0x0200018F RID: 399
	public class CampaignStartingOptionItemWidget : Widget
	{
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060014C8 RID: 5320 RVA: 0x00038B7C File Offset: 0x00036D7C
		// (set) Token: 0x060014C9 RID: 5321 RVA: 0x00038B84 File Offset: 0x00036D84
		public Widget BooleanOption { get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x00038B8D File Offset: 0x00036D8D
		// (set) Token: 0x060014CB RID: 5323 RVA: 0x00038B95 File Offset: 0x00036D95
		public Widget SliderOption { get; set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x00038B9E File Offset: 0x00036D9E
		// (set) Token: 0x060014CD RID: 5325 RVA: 0x00038BA6 File Offset: 0x00036DA6
		public Widget SelectionOption { get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x00038BAF File Offset: 0x00036DAF
		// (set) Token: 0x060014CF RID: 5327 RVA: 0x00038BB7 File Offset: 0x00036DB7
		public Widget InputOption { get; set; }

		// Token: 0x060014D0 RID: 5328 RVA: 0x00038BC0 File Offset: 0x00036DC0
		public CampaignStartingOptionItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00038BCC File Offset: 0x00036DCC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = base.EventManager.HoveredWidget != null && (base.EventManager.HoveredWidget == this || base.CheckIsMyChildRecursive(base.EventManager.HoveredWidget));
			if (flag && !this._isFocused)
			{
				base.EventFired("FocusBegin", Array.Empty<object>());
			}
			else if (!flag && this._isFocused)
			{
				base.EventFired("FocusEnd", Array.Empty<object>());
			}
			this._isFocused = flag;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00038C54 File Offset: 0x00036E54
		private void ResetNavigationIndices()
		{
			if (base.GamepadNavigationIndex == -1)
			{
				return;
			}
			Widget booleanOption = this.BooleanOption;
			if (booleanOption != null && booleanOption.IsVisible)
			{
				this.BooleanOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
			}
			else
			{
				Widget sliderOption = this.SliderOption;
				if (sliderOption != null && sliderOption.IsVisible)
				{
					this.SliderOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
				}
				else
				{
					Widget selectionOption = this.SelectionOption;
					if (selectionOption != null && selectionOption.IsVisible)
					{
						this.SelectionOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
					}
					else
					{
						Widget inputOption = this.InputOption;
						if (inputOption != null && inputOption.IsVisible)
						{
							this.InputOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
						}
					}
				}
			}
			base.GamepadNavigationIndex = -1;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00038D0C File Offset: 0x00036F0C
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			this.ResetNavigationIndices();
		}

		// Token: 0x0400097C RID: 2428
		private bool _isFocused;
	}
}
