using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002A RID: 42
	internal class ScriptingInterfaceOfITableauView : ITableauView
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x000197EC File Offset: 0x000179EC
		public TableauView CreateTableauView(string viewName)
		{
			byte[] array = null;
			if (viewName != null)
			{
				int byteCount = ScriptingInterfaceOfITableauView._utf8.GetByteCount(viewName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITableauView._utf8.GetBytes(viewName, 0, viewName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITableauView.call_CreateTableauViewDelegate(array);
			TableauView tableauView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				tableauView = new TableauView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return tableauView;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00019878 File Offset: 0x00017A78
		public void SetContinousRendering(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetContinousRenderingDelegate(pointer, value);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00019886 File Offset: 0x00017A86
		public void SetDeleteAfterRendering(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetDeleteAfterRenderingDelegate(pointer, value);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00019894 File Offset: 0x00017A94
		public void SetDoNotRenderThisFrame(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetDoNotRenderThisFrameDelegate(pointer, value);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000198A2 File Offset: 0x00017AA2
		public void SetSortingEnabled(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetSortingEnabledDelegate(pointer, value);
		}

		// Token: 0x0400054D RID: 1357
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400054E RID: 1358
		public static ScriptingInterfaceOfITableauView.CreateTableauViewDelegate call_CreateTableauViewDelegate;

		// Token: 0x0400054F RID: 1359
		public static ScriptingInterfaceOfITableauView.SetContinousRenderingDelegate call_SetContinousRenderingDelegate;

		// Token: 0x04000550 RID: 1360
		public static ScriptingInterfaceOfITableauView.SetDeleteAfterRenderingDelegate call_SetDeleteAfterRenderingDelegate;

		// Token: 0x04000551 RID: 1361
		public static ScriptingInterfaceOfITableauView.SetDoNotRenderThisFrameDelegate call_SetDoNotRenderThisFrameDelegate;

		// Token: 0x04000552 RID: 1362
		public static ScriptingInterfaceOfITableauView.SetSortingEnabledDelegate call_SetSortingEnabledDelegate;

		// Token: 0x020005B3 RID: 1459
		// (Invoke) Token: 0x06001CF7 RID: 7415
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTableauViewDelegate(byte[] viewName);

		// Token: 0x020005B4 RID: 1460
		// (Invoke) Token: 0x06001CFB RID: 7419
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetContinousRenderingDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020005B5 RID: 1461
		// (Invoke) Token: 0x06001CFF RID: 7423
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDeleteAfterRenderingDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020005B6 RID: 1462
		// (Invoke) Token: 0x06001D03 RID: 7427
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDoNotRenderThisFrameDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020005B7 RID: 1463
		// (Invoke) Token: 0x06001D07 RID: 7431
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSortingEnabledDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);
	}
}
