using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015C RID: 348
	public class EncyclopediaFilterListItemButtonWidget : ButtonWidget
	{
		// Token: 0x0600129A RID: 4762 RVA: 0x00033739 File Offset: 0x00031939
		public EncyclopediaFilterListItemButtonWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0003374C File Offset: 0x0003194C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			if (base.IsHovered)
			{
				this.SetState("Hovered");
				return;
			}
			if (base.IsSelected)
			{
				this.SetState("Selected");
				return;
			}
			if (base.IsPressed)
			{
				this.SetState("Pressed");
				return;
			}
			this.SetState("Default");
		}
	}
}
