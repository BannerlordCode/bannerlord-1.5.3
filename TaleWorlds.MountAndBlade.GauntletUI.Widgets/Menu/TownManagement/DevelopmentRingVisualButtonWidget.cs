using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010E RID: 270
	public class DevelopmentRingVisualButtonWidget : ButtonWidget
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x00028063 File Offset: 0x00026263
		public DevelopmentRingVisualButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0002806C File Offset: 0x0002626C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!base.IsSelected)
			{
				this.SetState(base.ParentWidget.CurrentState);
				return;
			}
			this.SetState("Selected");
		}
	}
}
