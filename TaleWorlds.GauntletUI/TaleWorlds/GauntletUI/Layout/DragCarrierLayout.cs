using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.Layout
{
	// Token: 0x0200003F RID: 63
	public class DragCarrierLayout : ILayout
	{
		// Token: 0x0600042A RID: 1066 RVA: 0x000108BA File Offset: 0x0000EABA
		Vector2 ILayout.MeasureChildren(Widget widget, Vector2 measureSpec, SpriteData spriteData, float renderScale)
		{
			Widget child = widget.GetChild(0);
			child.Measure(measureSpec);
			return child.MeasuredSize;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000108D0 File Offset: 0x0000EAD0
		void ILayout.OnLayout(Widget widget, float left, float bottom, float right, float top)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = right - left;
			float num4 = bottom - top;
			widget.GetChild(0).Layout(num, num4, num3, num2);
		}
	}
}
