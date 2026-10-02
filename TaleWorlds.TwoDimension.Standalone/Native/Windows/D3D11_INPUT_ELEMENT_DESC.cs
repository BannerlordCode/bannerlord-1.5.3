using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200002B RID: 43
	public struct D3D11_INPUT_ELEMENT_DESC
	{
		// Token: 0x04000110 RID: 272
		[MarshalAs(UnmanagedType.LPStr)]
		public string SemanticName;

		// Token: 0x04000111 RID: 273
		public uint SemanticIndex;

		// Token: 0x04000112 RID: 274
		public uint Format;

		// Token: 0x04000113 RID: 275
		public uint InputSlot;

		// Token: 0x04000114 RID: 276
		public uint AlignedByteOffset;

		// Token: 0x04000115 RID: 277
		public uint InputSlotClass;

		// Token: 0x04000116 RID: 278
		public uint InstanceDataStepRate;
	}
}
