using System;
using System.Numerics;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000020 RID: 32
	public interface IText
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600013B RID: 315
		// (set) Token: 0x0600013C RID: 316
		string Value { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600013D RID: 317
		// (set) Token: 0x0600013E RID: 318
		TextHorizontalAlignment HorizontalAlignment { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600013F RID: 319
		// (set) Token: 0x06000140 RID: 320
		TextVerticalAlignment VerticalAlignment { get; set; }

		// Token: 0x06000141 RID: 321
		Vector2 GetPreferredSize(bool fixedWidth, float widthSize, bool fixedHeight, float heightSize, SpriteData spriteData, float renderScale);
	}
}
