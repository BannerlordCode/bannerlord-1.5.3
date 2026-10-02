using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000173 RID: 371
	public class ConversationOptionListPanel : ListPanel
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x0003557F File Offset: 0x0003377F
		// (set) Token: 0x0600138F RID: 5007 RVA: 0x00035587 File Offset: 0x00033787
		public ButtonWidget OptionButtonWidget { get; set; }

		// Token: 0x06001390 RID: 5008 RVA: 0x00035590 File Offset: 0x00033790
		public ConversationOptionListPanel(UIContext context)
			: base(context)
		{
		}
	}
}
