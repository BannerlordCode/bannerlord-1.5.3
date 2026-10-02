using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200001C RID: 28
	internal class ScriptingInterfaceOfIMouseManager : IMouseManager
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00014DC6 File Offset: 0x00012FC6
		public void ActivateMouseCursor(int id)
		{
			ScriptingInterfaceOfIMouseManager.call_ActivateMouseCursorDelegate(id);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00014DD3 File Offset: 0x00012FD3
		public void LockCursorAtCurrentPosition(bool lockCursor)
		{
			ScriptingInterfaceOfIMouseManager.call_LockCursorAtCurrentPositionDelegate(lockCursor);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00014DE0 File Offset: 0x00012FE0
		public void LockCursorAtPosition(float x, float y)
		{
			ScriptingInterfaceOfIMouseManager.call_LockCursorAtPositionDelegate(x, y);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00014DF0 File Offset: 0x00012FF0
		public void SetMouseCursor(int id, string mousePath)
		{
			byte[] array = null;
			if (mousePath != null)
			{
				int byteCount = ScriptingInterfaceOfIMouseManager._utf8.GetByteCount(mousePath);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMouseManager._utf8.GetBytes(mousePath, 0, mousePath.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMouseManager.call_SetMouseCursorDelegate(id, array);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00014E4B File Offset: 0x0001304B
		public void ShowCursor(bool show)
		{
			ScriptingInterfaceOfIMouseManager.call_ShowCursorDelegate(show);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00014E58 File Offset: 0x00013058
		public void UnlockCursor()
		{
			ScriptingInterfaceOfIMouseManager.call_UnlockCursorDelegate();
		}

		// Token: 0x04000314 RID: 788
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000315 RID: 789
		public static ScriptingInterfaceOfIMouseManager.ActivateMouseCursorDelegate call_ActivateMouseCursorDelegate;

		// Token: 0x04000316 RID: 790
		public static ScriptingInterfaceOfIMouseManager.LockCursorAtCurrentPositionDelegate call_LockCursorAtCurrentPositionDelegate;

		// Token: 0x04000317 RID: 791
		public static ScriptingInterfaceOfIMouseManager.LockCursorAtPositionDelegate call_LockCursorAtPositionDelegate;

		// Token: 0x04000318 RID: 792
		public static ScriptingInterfaceOfIMouseManager.SetMouseCursorDelegate call_SetMouseCursorDelegate;

		// Token: 0x04000319 RID: 793
		public static ScriptingInterfaceOfIMouseManager.ShowCursorDelegate call_ShowCursorDelegate;

		// Token: 0x0400031A RID: 794
		public static ScriptingInterfaceOfIMouseManager.UnlockCursorDelegate call_UnlockCursorDelegate;

		// Token: 0x02000388 RID: 904
		// (Invoke) Token: 0x0600144B RID: 5195
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ActivateMouseCursorDelegate(int id);

		// Token: 0x02000389 RID: 905
		// (Invoke) Token: 0x0600144F RID: 5199
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LockCursorAtCurrentPositionDelegate([MarshalAs(UnmanagedType.U1)] bool lockCursor);

		// Token: 0x0200038A RID: 906
		// (Invoke) Token: 0x06001453 RID: 5203
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LockCursorAtPositionDelegate(float x, float y);

		// Token: 0x0200038B RID: 907
		// (Invoke) Token: 0x06001457 RID: 5207
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMouseCursorDelegate(int id, byte[] mousePath);

		// Token: 0x0200038C RID: 908
		// (Invoke) Token: 0x0600145B RID: 5211
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ShowCursorDelegate([MarshalAs(UnmanagedType.U1)] bool show);

		// Token: 0x0200038D RID: 909
		// (Invoke) Token: 0x0600145F RID: 5215
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnlockCursorDelegate();
	}
}
