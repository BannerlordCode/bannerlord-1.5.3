using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Radial
{
	// Token: 0x020000E8 RID: 232
	public class MissionRadialButtonWidget : ButtonWidget
	{
		// Token: 0x06000C09 RID: 3081 RVA: 0x00021647 File Offset: 0x0001F847
		public MissionRadialButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00021650 File Offset: 0x0001F850
		public void ExecuteFocused()
		{
			if (base.IsDisabled)
			{
				this.SetState("DisabledSelected");
			}
			base.EventFired("OnFocused", Array.Empty<object>());
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00021675 File Offset: 0x0001F875
		public void ExecuteUnfocused()
		{
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			this.SetState("Default");
		}
	}
}
