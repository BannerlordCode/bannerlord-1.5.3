using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002D RID: 45
	internal class ScriptingInterfaceOfITextureView : ITextureView
	{
		// Token: 0x0600062E RID: 1582 RVA: 0x0001A3A0 File Offset: 0x000185A0
		public TextureView CreateTextureView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITextureView.call_CreateTextureViewDelegate();
			TextureView textureView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				textureView = new TextureView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return textureView;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001A3E9 File Offset: 0x000185E9
		public void SetTexture(UIntPtr pointer, UIntPtr texture_ptr)
		{
			ScriptingInterfaceOfITextureView.call_SetTextureDelegate(pointer, texture_ptr);
		}

		// Token: 0x0400057E RID: 1406
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400057F RID: 1407
		public static ScriptingInterfaceOfITextureView.CreateTextureViewDelegate call_CreateTextureViewDelegate;

		// Token: 0x04000580 RID: 1408
		public static ScriptingInterfaceOfITextureView.SetTextureDelegate call_SetTextureDelegate;

		// Token: 0x020005E1 RID: 1505
		// (Invoke) Token: 0x06001DAF RID: 7599
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTextureViewDelegate();

		// Token: 0x020005E2 RID: 1506
		// (Invoke) Token: 0x06001DB3 RID: 7603
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTextureDelegate(UIntPtr pointer, UIntPtr texture_ptr);
	}
}
