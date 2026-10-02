using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000018 RID: 24
	internal class ScriptingInterfaceOfIMBMessageManager : IMBMessageManager
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000C01C File Offset: 0x0000A21C
		public void DisplayMessage(string message)
		{
			byte[] array = null;
			if (message != null)
			{
				int byteCount = ScriptingInterfaceOfIMBMessageManager._utf8.GetByteCount(message);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBMessageManager._utf8.GetBytes(message, 0, message.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBMessageManager.call_DisplayMessageDelegate(array);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000C078 File Offset: 0x0000A278
		public void DisplayMessageWithColor(string message, uint color)
		{
			byte[] array = null;
			if (message != null)
			{
				int byteCount = ScriptingInterfaceOfIMBMessageManager._utf8.GetByteCount(message);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBMessageManager._utf8.GetBytes(message, 0, message.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBMessageManager.call_DisplayMessageWithColorDelegate(array, color);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C0D3 File Offset: 0x0000A2D3
		public void SetMessageManager(MessageManagerBase messageManager)
		{
			ScriptingInterfaceOfIMBMessageManager.call_SetMessageManagerDelegate((messageManager != null) ? messageManager.GetManagedId() : 0);
		}

		// Token: 0x040001F6 RID: 502
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001F7 RID: 503
		public static ScriptingInterfaceOfIMBMessageManager.DisplayMessageDelegate call_DisplayMessageDelegate;

		// Token: 0x040001F8 RID: 504
		public static ScriptingInterfaceOfIMBMessageManager.DisplayMessageWithColorDelegate call_DisplayMessageWithColorDelegate;

		// Token: 0x040001F9 RID: 505
		public static ScriptingInterfaceOfIMBMessageManager.SetMessageManagerDelegate call_SetMessageManagerDelegate;

		// Token: 0x02000258 RID: 600
		// (Invoke) Token: 0x06000C50 RID: 3152
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisplayMessageDelegate(byte[] message);

		// Token: 0x02000259 RID: 601
		// (Invoke) Token: 0x06000C54 RID: 3156
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisplayMessageWithColorDelegate(byte[] message, uint color);

		// Token: 0x0200025A RID: 602
		// (Invoke) Token: 0x06000C58 RID: 3160
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMessageManagerDelegate(int messageManager);
	}
}
