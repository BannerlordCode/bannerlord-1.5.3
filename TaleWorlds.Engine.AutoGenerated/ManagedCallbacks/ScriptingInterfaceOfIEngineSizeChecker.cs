using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000010 RID: 16
	internal class ScriptingInterfaceOfIEngineSizeChecker : IEngineSizeChecker
	{
		// Token: 0x06000124 RID: 292 RVA: 0x000101F8 File Offset: 0x0000E3F8
		public IntPtr GetEngineStructMemberOffset(string className, string memberName)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIEngineSizeChecker._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIEngineSizeChecker._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (memberName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIEngineSizeChecker._utf8.GetByteCount(memberName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIEngineSizeChecker._utf8.GetBytes(memberName, 0, memberName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfIEngineSizeChecker.call_GetEngineStructMemberOffsetDelegate(array, array2);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00010298 File Offset: 0x0000E498
		public int GetEngineStructSize(string str)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIEngineSizeChecker._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIEngineSizeChecker._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIEngineSizeChecker.call_GetEngineStructSizeDelegate(array);
		}

		// Token: 0x040000B3 RID: 179
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040000B4 RID: 180
		public static ScriptingInterfaceOfIEngineSizeChecker.GetEngineStructMemberOffsetDelegate call_GetEngineStructMemberOffsetDelegate;

		// Token: 0x040000B5 RID: 181
		public static ScriptingInterfaceOfIEngineSizeChecker.GetEngineStructSizeDelegate call_GetEngineStructSizeDelegate;

		// Token: 0x02000133 RID: 307
		// (Invoke) Token: 0x06000AF7 RID: 2807
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate IntPtr GetEngineStructMemberOffsetDelegate(byte[] className, byte[] memberName);

		// Token: 0x02000134 RID: 308
		// (Invoke) Token: 0x06000AFB RID: 2811
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetEngineStructSizeDelegate(byte[] str);
	}
}
