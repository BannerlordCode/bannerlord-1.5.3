using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200003E RID: 62
	[Flags]
	internal enum PixelFormatDescriptorFlags : uint
	{
		// Token: 0x0400017A RID: 378
		DoubleBuffer = 1U,
		// Token: 0x0400017B RID: 379
		Stereo = 2U,
		// Token: 0x0400017C RID: 380
		DrawToWindow = 4U,
		// Token: 0x0400017D RID: 381
		DrawToBitmap = 8U,
		// Token: 0x0400017E RID: 382
		SupportGDI = 16U,
		// Token: 0x0400017F RID: 383
		SupportOpengl = 32U,
		// Token: 0x04000180 RID: 384
		GenericFormat = 64U,
		// Token: 0x04000181 RID: 385
		NeedPalette = 128U,
		// Token: 0x04000182 RID: 386
		NeedSystemPalette = 256U,
		// Token: 0x04000183 RID: 387
		SwapExchange = 512U,
		// Token: 0x04000184 RID: 388
		SwapCopy = 1024U,
		// Token: 0x04000185 RID: 389
		SwapLayerBuffers = 2048U,
		// Token: 0x04000186 RID: 390
		GenericAccelerated = 4096U,
		// Token: 0x04000187 RID: 391
		SupportDirectDraw = 8192U,
		// Token: 0x04000188 RID: 392
		Direct3DAccelerated = 16384U,
		// Token: 0x04000189 RID: 393
		SupportComposition = 32768U
	}
}
