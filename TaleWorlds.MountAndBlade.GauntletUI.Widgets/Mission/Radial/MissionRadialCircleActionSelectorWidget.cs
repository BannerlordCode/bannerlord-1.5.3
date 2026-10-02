using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Radial
{
	// Token: 0x020000E9 RID: 233
	public class MissionRadialCircleActionSelectorWidget : CircleActionSelectorWidget
	{
		// Token: 0x06000C0C RID: 3084 RVA: 0x00021696 File Offset: 0x0001F896
		public MissionRadialCircleActionSelectorWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000216A0 File Offset: 0x0001F8A0
		protected override void OnSelectedIndexChanged(int selectedIndex)
		{
			base.OnSelectedIndexChanged(selectedIndex);
			for (int i = 0; i < base.Children.Count; i++)
			{
				MissionRadialButtonWidget missionRadialButtonWidget;
				if ((missionRadialButtonWidget = base.Children[i] as MissionRadialButtonWidget) != null)
				{
					if (i == selectedIndex)
					{
						missionRadialButtonWidget.ExecuteFocused();
					}
					else
					{
						missionRadialButtonWidget.ExecuteUnfocused();
					}
				}
			}
		}
	}
}
