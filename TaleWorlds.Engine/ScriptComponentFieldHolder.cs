using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005A RID: 90
	[EngineStruct("rglScript_component_field_holder", false, null)]
	public struct ScriptComponentFieldHolder
	{
		// Token: 0x040000CC RID: 204
		public MatrixFrame matrixFrame;

		// Token: 0x040000CD RID: 205
		public Vec3 color;

		// Token: 0x040000CE RID: 206
		public Vec3 v3;

		// Token: 0x040000CF RID: 207
		public UIntPtr entityPointer;

		// Token: 0x040000D0 RID: 208
		public UIntPtr texturePointer;

		// Token: 0x040000D1 RID: 209
		public UIntPtr meshPointer;

		// Token: 0x040000D2 RID: 210
		public UIntPtr materialPointer;

		// Token: 0x040000D3 RID: 211
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string s;

		// Token: 0x040000D4 RID: 212
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string enumValue;

		// Token: 0x040000D5 RID: 213
		public double d;

		// Token: 0x040000D6 RID: 214
		public float f;

		// Token: 0x040000D7 RID: 215
		public int b;

		// Token: 0x040000D8 RID: 216
		public int i;
	}
}
