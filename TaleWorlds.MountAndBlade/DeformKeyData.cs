using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C5 RID: 453
	[EngineStruct("Deform_Key_Data", false, null)]
	public struct DeformKeyData
	{
		// Token: 0x04000892 RID: 2194
		public int GroupId;

		// Token: 0x04000893 RID: 2195
		public int KeyTimePoint;

		// Token: 0x04000894 RID: 2196
		public float KeyMin;

		// Token: 0x04000895 RID: 2197
		public float KeyMax;

		// Token: 0x04000896 RID: 2198
		public float Value;

		// Token: 0x04000897 RID: 2199
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string Id;
	}
}
