using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameOver
{
	// Token: 0x02000153 RID: 339
	public class GameOverCategoryButtonWidget : ButtonWidget
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x00032735 File Offset: 0x00030935
		// (set) Token: 0x06001215 RID: 4629 RVA: 0x0003273D File Offset: 0x0003093D
		public string CategoryID { get; set; }

		// Token: 0x06001216 RID: 4630 RVA: 0x00032746 File Offset: 0x00030946
		public GameOverCategoryButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0003274F File Offset: 0x0003094F
		protected override void HandleClick()
		{
			this.HandleSoundEvent();
			base.HandleClick();
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0003275D File Offset: 0x0003095D
		private void HandleSoundEvent()
		{
			base.EventFired(this.CategoryID, Array.Empty<object>());
		}
	}
}
