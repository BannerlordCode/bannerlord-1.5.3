using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x02000121 RID: 289
	public class MapSiegeQueueIndexTextWidget : TextWidget
	{
		// Token: 0x06000F67 RID: 3943 RVA: 0x0002A96A File Offset: 0x00028B6A
		public MapSiegeQueueIndexTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0002A973 File Offset: 0x00028B73
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			base.IsVisible = base.IntText > 0;
		}
	}
}
