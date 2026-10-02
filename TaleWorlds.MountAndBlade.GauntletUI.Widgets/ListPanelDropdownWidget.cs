using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002D RID: 45
	public class ListPanelDropdownWidget : DropdownWidget
	{
		// Token: 0x06000247 RID: 583 RVA: 0x00008484 File Offset: 0x00006684
		public ListPanelDropdownWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000848D File Offset: 0x0000668D
		protected override void OpenPanel()
		{
			base.OpenPanel();
			if (this.ListPanelContainer != null)
			{
				this.ListPanelContainer.IsVisible = true;
			}
			base.Button.IsSelected = true;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000084B5 File Offset: 0x000066B5
		protected override void ClosePanel()
		{
			if (this.ListPanelContainer != null)
			{
				this.ListPanelContainer.IsVisible = false;
			}
			base.Button.IsSelected = false;
			base.ClosePanel();
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000084DD File Offset: 0x000066DD
		// (set) Token: 0x0600024B RID: 587 RVA: 0x000084E5 File Offset: 0x000066E5
		[Editor(false)]
		public Widget ListPanelContainer
		{
			get
			{
				return this._listPanelContainer;
			}
			set
			{
				if (this._listPanelContainer != value)
				{
					this._listPanelContainer = value;
					base.OnPropertyChanged<Widget>(value, "ListPanelContainer");
				}
			}
		}

		// Token: 0x04000110 RID: 272
		private Widget _listPanelContainer;
	}
}
