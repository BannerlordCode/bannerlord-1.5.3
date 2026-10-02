using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.Layout
{
	// Token: 0x02000048 RID: 72
	public class TextLayout : ILayout
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x00011E29 File Offset: 0x00010029
		public TextLayout(IText text)
		{
			this._defaultLayout = new DefaultLayout();
			this._text = text;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00011E44 File Offset: 0x00010044
		Vector2 ILayout.MeasureChildren(Widget widget, Vector2 measureSpec, SpriteData spriteData, float renderScale)
		{
			Vector2 vector = this._defaultLayout.MeasureChildren(widget, measureSpec, spriteData, renderScale);
			bool flag = widget.WidthSizePolicy != SizePolicy.CoverChildren || widget.MaxWidth != 0f;
			bool flag2 = widget.HeightSizePolicy != SizePolicy.CoverChildren || widget.MaxHeight != 0f;
			float x = measureSpec.X;
			float y = measureSpec.Y;
			Vector2 preferredSize = this._text.GetPreferredSize(flag, x, flag2, y, spriteData, renderScale);
			if (vector.X < preferredSize.X)
			{
				vector.X = preferredSize.X;
			}
			if (vector.Y < preferredSize.Y)
			{
				vector.Y = preferredSize.Y;
			}
			return vector;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00011EFB File Offset: 0x000100FB
		void ILayout.OnLayout(Widget widget, float left, float bottom, float right, float top)
		{
			this._defaultLayout.OnLayout(widget, left, bottom, right, top);
		}

		// Token: 0x04000229 RID: 553
		private ILayout _defaultLayout;

		// Token: 0x0400022A RID: 554
		private IText _text;
	}
}
