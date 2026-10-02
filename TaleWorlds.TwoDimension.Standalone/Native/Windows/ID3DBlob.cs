using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001B RID: 27
	public static class ID3DBlob
	{
		// Token: 0x06000104 RID: 260 RVA: 0x0000693B File Offset: 0x00004B3B
		public static IntPtr GetBufferPointer(IntPtr blob)
		{
			return ((ID3DBlob.FnGetBufferPointer)ID3DBlob.GetRaw(blob, 3, typeof(ID3DBlob.FnGetBufferPointer)))(blob);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006959 File Offset: 0x00004B59
		public static int GetBufferSize(IntPtr blob)
		{
			return (int)(uint)((ID3DBlob.FnGetBufferSize)ID3DBlob.GetRaw(blob, 4, typeof(ID3DBlob.FnGetBufferSize)))(blob);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000697C File Offset: 0x00004B7C
		public static void Release(IntPtr blob)
		{
			if (blob == IntPtr.Zero)
			{
				return;
			}
			((ID3DBlob.FnRelease)ID3DBlob.GetRaw(blob, 2, typeof(ID3DBlob.FnRelease)))(blob);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000069A9 File Offset: 0x00004BA9
		private static Delegate GetRaw(IntPtr obj, int slot, Type t)
		{
			return Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(Marshal.ReadIntPtr(obj), slot * IntPtr.Size), t);
		}

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x0600017D RID: 381
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr FnGetBufferPointer(IntPtr self);

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x06000181 RID: 385
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate UIntPtr FnGetBufferSize(IntPtr self);

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x06000185 RID: 389
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate uint FnRelease(IntPtr self);
	}
}
