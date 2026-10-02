using System;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200003D RID: 61
	internal struct PixelFormatDescriptor
	{
		// Token: 0x0400015F RID: 351
		public ushort nSize;

		// Token: 0x04000160 RID: 352
		public ushort nVersion;

		// Token: 0x04000161 RID: 353
		public uint dwFlags;

		// Token: 0x04000162 RID: 354
		public byte iPixelType;

		// Token: 0x04000163 RID: 355
		public byte cColorBits;

		// Token: 0x04000164 RID: 356
		public byte cRedBits;

		// Token: 0x04000165 RID: 357
		public byte cRedShift;

		// Token: 0x04000166 RID: 358
		public byte cGreenBits;

		// Token: 0x04000167 RID: 359
		public byte cGreenShift;

		// Token: 0x04000168 RID: 360
		public byte cBlueBits;

		// Token: 0x04000169 RID: 361
		public byte cBlueShift;

		// Token: 0x0400016A RID: 362
		public byte cAlphaBits;

		// Token: 0x0400016B RID: 363
		public byte cAlphaShift;

		// Token: 0x0400016C RID: 364
		public byte cAccumBits;

		// Token: 0x0400016D RID: 365
		public byte cAccumRedBits;

		// Token: 0x0400016E RID: 366
		public byte cAccumGreenBits;

		// Token: 0x0400016F RID: 367
		public byte cAccumBlueBits;

		// Token: 0x04000170 RID: 368
		public byte cAccumAlphaBits;

		// Token: 0x04000171 RID: 369
		public byte cDepthBits;

		// Token: 0x04000172 RID: 370
		public byte cStencilBits;

		// Token: 0x04000173 RID: 371
		public byte cAuxBuffers;

		// Token: 0x04000174 RID: 372
		public byte iLayerType;

		// Token: 0x04000175 RID: 373
		public byte bReserved;

		// Token: 0x04000176 RID: 374
		public uint dwLayerMask;

		// Token: 0x04000177 RID: 375
		public uint dwVisibleMask;

		// Token: 0x04000178 RID: 376
		public uint dwDamageMask;
	}
}
