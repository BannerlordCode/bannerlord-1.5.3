using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000172 RID: 370
	public class ConversationNameButtonWidget : ButtonWidget
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x000354FD File Offset: 0x000336FD
		public ConversationNameButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00035506 File Offset: 0x00033706
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.RelationBarContainer.IsVisible = this.IsRelationEnabled;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0003551F File Offset: 0x0003371F
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.RelationBarContainer.IsVisible = false;
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00035533 File Offset: 0x00033733
		// (set) Token: 0x0600138B RID: 5003 RVA: 0x0003553B File Offset: 0x0003373B
		[Editor(false)]
		public bool IsRelationEnabled
		{
			get
			{
				return this._isRelationEnabled;
			}
			set
			{
				if (value != this._isRelationEnabled)
				{
					this._isRelationEnabled = value;
					base.OnPropertyChanged(value, "IsRelationEnabled");
				}
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00035559 File Offset: 0x00033759
		// (set) Token: 0x0600138D RID: 5005 RVA: 0x00035561 File Offset: 0x00033761
		[Editor(false)]
		public Widget RelationBarContainer
		{
			get
			{
				return this._relationBarContainer;
			}
			set
			{
				if (value != this._relationBarContainer)
				{
					this._relationBarContainer = value;
					base.OnPropertyChanged<Widget>(value, "RelationBarContainer");
				}
			}
		}

		// Token: 0x040008E5 RID: 2277
		private bool _isRelationEnabled;

		// Token: 0x040008E6 RID: 2278
		private Widget _relationBarContainer;
	}
}
