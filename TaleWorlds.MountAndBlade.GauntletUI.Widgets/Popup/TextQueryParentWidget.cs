using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Popup
{
	// Token: 0x02000062 RID: 98
	public class TextQueryParentWidget : Widget
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x00010219 File Offset: 0x0000E419
		public TextQueryParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00010222 File Offset: 0x0000E422
		private void FocusOnTextQuery()
		{
			base.EventManager.FocusedWidget = this.TextInputWidget;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00010235 File Offset: 0x0000E435
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.TextInputWidget != null)
			{
				this.FocusOnTextQuery();
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001024B File Offset: 0x0000E44B
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00010253 File Offset: 0x0000E453
		[Editor(false)]
		public EditableTextWidget TextInputWidget
		{
			get
			{
				return this._editableTextWidget;
			}
			set
			{
				if (value != this._editableTextWidget)
				{
					this._editableTextWidget = value;
					this.FocusOnTextQuery();
				}
			}
		}

		// Token: 0x04000247 RID: 583
		private EditableTextWidget _editableTextWidget;
	}
}
