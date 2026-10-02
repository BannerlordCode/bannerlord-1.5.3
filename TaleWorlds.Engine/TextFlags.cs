using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006D RID: 109
	[Flags]
	[EngineStruct("rglText_flags", false, null, FirstCharacterUppercase = false)]
	public enum TextFlags
	{
		// Token: 0x0400014B RID: 331
		RglTfNone = 0,
		// Token: 0x0400014C RID: 332
		RglTfHAlignLeft = 1,
		// Token: 0x0400014D RID: 333
		RglTfHAlignRight = 2,
		// Token: 0x0400014E RID: 334
		RglTfHAlignCenter = 3,
		// Token: 0x0400014F RID: 335
		RglTfVAlignTop = 4,
		// Token: 0x04000150 RID: 336
		RglTfVAlignDown = 8,
		// Token: 0x04000151 RID: 337
		RglTfVAlignCenter = 12,
		// Token: 0x04000152 RID: 338
		RglTfSingleLine = 16,
		// Token: 0x04000153 RID: 339
		RglTfMultiline = 32,
		// Token: 0x04000154 RID: 340
		RglTfItalic = 64,
		// Token: 0x04000155 RID: 341
		RglTfCutTextFromLeft = 128,
		// Token: 0x04000156 RID: 342
		RglTfDoubleSpace = 256,
		// Token: 0x04000157 RID: 343
		RglTfWithOutline = 512,
		// Token: 0x04000158 RID: 344
		RglTfHalfSpace = 1024
	}
}
