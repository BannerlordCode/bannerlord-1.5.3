using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200000D RID: 13
	internal class ScriptingInterfaceOfIMBBannerlordChecker : IMBBannerlordChecker
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0000AEF8 File Offset: 0x000090F8
		public IntPtr GetEngineStructMemberOffset(string className, string memberName)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIMBBannerlordChecker._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBBannerlordChecker._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (memberName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIMBBannerlordChecker._utf8.GetByteCount(memberName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIMBBannerlordChecker._utf8.GetBytes(memberName, 0, memberName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfIMBBannerlordChecker.call_GetEngineStructMemberOffsetDelegate(array, array2);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000AF98 File Offset: 0x00009198
		public int GetEngineStructSize(string str)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIMBBannerlordChecker._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBBannerlordChecker._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBBannerlordChecker.call_GetEngineStructSizeDelegate(array);
		}

		// Token: 0x04000186 RID: 390
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000187 RID: 391
		public static ScriptingInterfaceOfIMBBannerlordChecker.GetEngineStructMemberOffsetDelegate call_GetEngineStructMemberOffsetDelegate;

		// Token: 0x04000188 RID: 392
		public static ScriptingInterfaceOfIMBBannerlordChecker.GetEngineStructSizeDelegate call_GetEngineStructSizeDelegate;

		// Token: 0x020001F3 RID: 499
		// (Invoke) Token: 0x06000ABC RID: 2748
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate IntPtr GetEngineStructMemberOffsetDelegate(byte[] className, byte[] memberName);

		// Token: 0x020001F4 RID: 500
		// (Invoke) Token: 0x06000AC0 RID: 2752
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetEngineStructSizeDelegate(byte[] str);
	}
}
