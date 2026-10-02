using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002E RID: 46
	internal class ScriptingInterfaceOfIThumbnailCreatorView : IThumbnailCreatorView
	{
		// Token: 0x06000632 RID: 1586 RVA: 0x0001A40C File Offset: 0x0001860C
		public void CancelRequest(UIntPtr pointer, string render_id)
		{
			byte[] array = null;
			if (render_id != null)
			{
				int byteCount = ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetByteCount(render_id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetBytes(render_id, 0, render_id.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIThumbnailCreatorView.call_CancelRequestDelegate(pointer, array);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001A467 File Offset: 0x00018667
		public void ClearRequests(UIntPtr pointer)
		{
			ScriptingInterfaceOfIThumbnailCreatorView.call_ClearRequestsDelegate(pointer);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001A474 File Offset: 0x00018674
		public ThumbnailCreatorView CreateThumbnailCreatorView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIThumbnailCreatorView.call_CreateThumbnailCreatorViewDelegate();
			ThumbnailCreatorView thumbnailCreatorView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				thumbnailCreatorView = new ThumbnailCreatorView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return thumbnailCreatorView;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001A4BD File Offset: 0x000186BD
		public int GetNumberOfPendingRequests(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIThumbnailCreatorView.call_GetNumberOfPendingRequestsDelegate(pointer);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001A4CA File Offset: 0x000186CA
		public bool IsMemoryCleared(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIThumbnailCreatorView.call_IsMemoryClearedDelegate(pointer);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001A4D8 File Offset: 0x000186D8
		public void RegisterCachedEntity(UIntPtr pointer, UIntPtr scene, UIntPtr entity_ptr, string cacheId)
		{
			byte[] array = null;
			if (cacheId != null)
			{
				int byteCount = ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetByteCount(cacheId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetBytes(cacheId, 0, cacheId.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIThumbnailCreatorView.call_RegisterCachedEntityDelegate(pointer, scene, entity_ptr, array);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001A539 File Offset: 0x00018739
		public void RegisterRenderRequest(UIntPtr pointer, ref ThumbnailRenderRequest request)
		{
			ScriptingInterfaceOfIThumbnailCreatorView.call_RegisterRenderRequestDelegate(pointer, ref request);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001A547 File Offset: 0x00018747
		public void RegisterScene(UIntPtr pointer, UIntPtr scene_ptr, bool use_postfx)
		{
			ScriptingInterfaceOfIThumbnailCreatorView.call_RegisterSceneDelegate(pointer, scene_ptr, use_postfx);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001A558 File Offset: 0x00018758
		public void UnregisterCachedEntity(UIntPtr pointer, string cacheId)
		{
			byte[] array = null;
			if (cacheId != null)
			{
				int byteCount = ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetByteCount(cacheId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetBytes(cacheId, 0, cacheId.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIThumbnailCreatorView.call_UnregisterCachedEntityDelegate(pointer, array);
		}

		// Token: 0x04000581 RID: 1409
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000582 RID: 1410
		public static ScriptingInterfaceOfIThumbnailCreatorView.CancelRequestDelegate call_CancelRequestDelegate;

		// Token: 0x04000583 RID: 1411
		public static ScriptingInterfaceOfIThumbnailCreatorView.ClearRequestsDelegate call_ClearRequestsDelegate;

		// Token: 0x04000584 RID: 1412
		public static ScriptingInterfaceOfIThumbnailCreatorView.CreateThumbnailCreatorViewDelegate call_CreateThumbnailCreatorViewDelegate;

		// Token: 0x04000585 RID: 1413
		public static ScriptingInterfaceOfIThumbnailCreatorView.GetNumberOfPendingRequestsDelegate call_GetNumberOfPendingRequestsDelegate;

		// Token: 0x04000586 RID: 1414
		public static ScriptingInterfaceOfIThumbnailCreatorView.IsMemoryClearedDelegate call_IsMemoryClearedDelegate;

		// Token: 0x04000587 RID: 1415
		public static ScriptingInterfaceOfIThumbnailCreatorView.RegisterCachedEntityDelegate call_RegisterCachedEntityDelegate;

		// Token: 0x04000588 RID: 1416
		public static ScriptingInterfaceOfIThumbnailCreatorView.RegisterRenderRequestDelegate call_RegisterRenderRequestDelegate;

		// Token: 0x04000589 RID: 1417
		public static ScriptingInterfaceOfIThumbnailCreatorView.RegisterSceneDelegate call_RegisterSceneDelegate;

		// Token: 0x0400058A RID: 1418
		public static ScriptingInterfaceOfIThumbnailCreatorView.UnregisterCachedEntityDelegate call_UnregisterCachedEntityDelegate;

		// Token: 0x020005E3 RID: 1507
		// (Invoke) Token: 0x06001DB7 RID: 7607
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CancelRequestDelegate(UIntPtr pointer, byte[] render_id);

		// Token: 0x020005E4 RID: 1508
		// (Invoke) Token: 0x06001DBB RID: 7611
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearRequestsDelegate(UIntPtr pointer);

		// Token: 0x020005E5 RID: 1509
		// (Invoke) Token: 0x06001DBF RID: 7615
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateThumbnailCreatorViewDelegate();

		// Token: 0x020005E6 RID: 1510
		// (Invoke) Token: 0x06001DC3 RID: 7619
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfPendingRequestsDelegate(UIntPtr pointer);

		// Token: 0x020005E7 RID: 1511
		// (Invoke) Token: 0x06001DC7 RID: 7623
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsMemoryClearedDelegate(UIntPtr pointer);

		// Token: 0x020005E8 RID: 1512
		// (Invoke) Token: 0x06001DCB RID: 7627
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterCachedEntityDelegate(UIntPtr pointer, UIntPtr scene, UIntPtr entity_ptr, byte[] cacheId);

		// Token: 0x020005E9 RID: 1513
		// (Invoke) Token: 0x06001DCF RID: 7631
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterRenderRequestDelegate(UIntPtr pointer, ref ThumbnailRenderRequest request);

		// Token: 0x020005EA RID: 1514
		// (Invoke) Token: 0x06001DD3 RID: 7635
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterSceneDelegate(UIntPtr pointer, UIntPtr scene_ptr, [MarshalAs(UnmanagedType.U1)] bool use_postfx);

		// Token: 0x020005EB RID: 1515
		// (Invoke) Token: 0x06001DD7 RID: 7639
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnregisterCachedEntityDelegate(UIntPtr pointer, byte[] cacheId);
	}
}
