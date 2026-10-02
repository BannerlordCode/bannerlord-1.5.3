using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000036 RID: 54
	public class ParallaxContainerWidget : Widget
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000A584 File Offset: 0x00008784
		public ParallaxContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000A598 File Offset: 0x00008798
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			using (List<ParallaxItemBrushWidget>.Enumerator enumerator = this._parallaxItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current.InitialDirection)
					{
					}
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000A60C File Offset: 0x0000880C
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			ParallaxItemBrushWidget parallaxItemBrushWidget;
			if ((parallaxItemBrushWidget = child as ParallaxItemBrushWidget) != null)
			{
				this._parallaxItems.Add(parallaxItemBrushWidget);
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000A638 File Offset: 0x00008838
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			ParallaxItemBrushWidget parallaxItemBrushWidget;
			if ((parallaxItemBrushWidget = child as ParallaxItemBrushWidget) != null)
			{
				this._parallaxItems.Remove(parallaxItemBrushWidget);
			}
		}

		// Token: 0x0400014E RID: 334
		private List<ParallaxItemBrushWidget> _parallaxItems = new List<ParallaxItemBrushWidget>();
	}
}
