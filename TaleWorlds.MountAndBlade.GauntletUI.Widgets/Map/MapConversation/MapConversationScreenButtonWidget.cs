using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapConversation
{
	// Token: 0x0200012A RID: 298
	public class MapConversationScreenButtonWidget : ButtonWidget
	{
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0002BDDD File Offset: 0x00029FDD
		// (set) Token: 0x06000FCC RID: 4044 RVA: 0x0002BDE5 File Offset: 0x00029FE5
		public Widget ConversationParent { get; set; }

		// Token: 0x06000FCD RID: 4045 RVA: 0x0002BDEE File Offset: 0x00029FEE
		public MapConversationScreenButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0002BDF7 File Offset: 0x00029FF7
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x0002BDFF File Offset: 0x00029FFF
		public bool IsBarterActive
		{
			get
			{
				return this._isBarterActive;
			}
			set
			{
				if (this._isBarterActive != value)
				{
					this._isBarterActive = value;
					this.ConversationParent.IsVisible = !this.IsBarterActive;
				}
			}
		}

		// Token: 0x04000739 RID: 1849
		private bool _isBarterActive;
	}
}
