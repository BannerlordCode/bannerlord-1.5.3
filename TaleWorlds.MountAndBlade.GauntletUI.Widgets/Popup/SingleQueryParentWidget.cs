using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Popup
{
	// Token: 0x02000061 RID: 97
	public class SingleQueryParentWidget : Widget
	{
		// Token: 0x0600054C RID: 1356 RVA: 0x00010195 File Offset: 0x0000E395
		public SingleQueryParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001019E File Offset: 0x0000E39E
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.DescriptionScrollbar.IsVisible)
			{
				this.DescriptionScrollablePanel.GamepadNavigationIndex = 0;
				return;
			}
			this.DescriptionScrollablePanel.GamepadNavigationIndex = -1;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000101CD File Offset: 0x0000E3CD
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x000101D5 File Offset: 0x0000E3D5
		[Editor(false)]
		public ScrollablePanel DescriptionScrollablePanel
		{
			get
			{
				return this._descriptionScrollablePanel;
			}
			set
			{
				if (value != this._descriptionScrollablePanel)
				{
					this._descriptionScrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "DescriptionScrollablePanel");
				}
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x000101F3 File Offset: 0x0000E3F3
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x000101FB File Offset: 0x0000E3FB
		[Editor(false)]
		public ScrollbarWidget DescriptionScrollbar
		{
			get
			{
				return this._descriptionScrollbar;
			}
			set
			{
				if (value != this._descriptionScrollbar)
				{
					this._descriptionScrollbar = value;
					base.OnPropertyChanged<ScrollbarWidget>(value, "DescriptionScrollbar");
				}
			}
		}

		// Token: 0x04000245 RID: 581
		private ScrollablePanel _descriptionScrollablePanel;

		// Token: 0x04000246 RID: 582
		private ScrollbarWidget _descriptionScrollbar;
	}
}
