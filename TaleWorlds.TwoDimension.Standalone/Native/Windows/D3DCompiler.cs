using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x0200001A RID: 26
	public static class D3DCompiler
	{
		// Token: 0x06000102 RID: 258
		[DllImport("d3dcompiler_47.dll")]
		public static extern int D3DCompile(IntPtr pSrcData, IntPtr srcDataSize, [MarshalAs(UnmanagedType.LPStr)] string pSourceName, IntPtr pDefines, IntPtr pInclude, [MarshalAs(UnmanagedType.LPStr)] string pEntrypoint, [MarshalAs(UnmanagedType.LPStr)] string pTarget, uint Flags1, uint Flags2, out IntPtr ppCode, out IntPtr ppErrorMsgs);

		// Token: 0x06000103 RID: 259 RVA: 0x00006908 File Offset: 0x00004B08
		public static string GetErrorMessage(IntPtr ppErrorMsgs)
		{
			if (ppErrorMsgs == IntPtr.Zero)
			{
				return string.Empty;
			}
			int bufferSize = ID3DBlob.GetBufferSize(ppErrorMsgs);
			return Marshal.PtrToStringAnsi(ID3DBlob.GetBufferPointer(ppErrorMsgs), bufferSize);
		}
	}
}
