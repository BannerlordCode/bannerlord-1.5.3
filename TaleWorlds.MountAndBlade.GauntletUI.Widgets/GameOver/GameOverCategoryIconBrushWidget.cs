using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameOver
{
	// Token: 0x02000154 RID: 340
	public class GameOverCategoryIconBrushWidget : BrushWidget
	{
		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00032770 File Offset: 0x00030970
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x00032778 File Offset: 0x00030978
		public string CategoryID { get; set; }

		// Token: 0x0600121B RID: 4635 RVA: 0x00032781 File Offset: 0x00030981
		public GameOverCategoryIconBrushWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnManualLateUpdate), 4);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000327A3 File Offset: 0x000309A3
		private void OnManualLateUpdate(float obj)
		{
			base.Brush = base.Context.GetBrush("GameOver.Category.Visual." + this.CategoryID);
		}
	}
}
