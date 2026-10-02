using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000032 RID: 50
	internal class ScriptingInterfaceOfIVideoPlayerView : IVideoPlayerView
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		public VideoPlayerView CreateVideoPlayerView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIVideoPlayerView.call_CreateVideoPlayerViewDelegate();
			VideoPlayerView videoPlayerView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				videoPlayerView = new VideoPlayerView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return videoPlayerView;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001C331 File Offset: 0x0001A531
		public void Finalize(UIntPtr pointer)
		{
			ScriptingInterfaceOfIVideoPlayerView.call_FinalizeDelegate(pointer);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001C33E File Offset: 0x0001A53E
		public bool IsVideoFinished(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIVideoPlayerView.call_IsVideoFinishedDelegate(pointer);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001C34C File Offset: 0x0001A54C
		public void PlayVideo(UIntPtr pointer, string videoFileName, string soundFileName, float framerate, bool looping)
		{
			byte[] array = null;
			if (videoFileName != null)
			{
				int byteCount = ScriptingInterfaceOfIVideoPlayerView._utf8.GetByteCount(videoFileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIVideoPlayerView._utf8.GetBytes(videoFileName, 0, videoFileName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (soundFileName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIVideoPlayerView._utf8.GetByteCount(soundFileName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIVideoPlayerView._utf8.GetBytes(soundFileName, 0, soundFileName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIVideoPlayerView.call_PlayVideoDelegate(pointer, array, array2, framerate, looping);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001C3EE File Offset: 0x0001A5EE
		public void StopVideo(UIntPtr pointer)
		{
			ScriptingInterfaceOfIVideoPlayerView.call_StopVideoDelegate(pointer);
		}

		// Token: 0x04000631 RID: 1585
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000632 RID: 1586
		public static ScriptingInterfaceOfIVideoPlayerView.CreateVideoPlayerViewDelegate call_CreateVideoPlayerViewDelegate;

		// Token: 0x04000633 RID: 1587
		public static ScriptingInterfaceOfIVideoPlayerView.FinalizeDelegate call_FinalizeDelegate;

		// Token: 0x04000634 RID: 1588
		public static ScriptingInterfaceOfIVideoPlayerView.IsVideoFinishedDelegate call_IsVideoFinishedDelegate;

		// Token: 0x04000635 RID: 1589
		public static ScriptingInterfaceOfIVideoPlayerView.PlayVideoDelegate call_PlayVideoDelegate;

		// Token: 0x04000636 RID: 1590
		public static ScriptingInterfaceOfIVideoPlayerView.StopVideoDelegate call_StopVideoDelegate;

		// Token: 0x0200068F RID: 1679
		// (Invoke) Token: 0x06002067 RID: 8295
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateVideoPlayerViewDelegate();

		// Token: 0x02000690 RID: 1680
		// (Invoke) Token: 0x0600206B RID: 8299
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeDelegate(UIntPtr pointer);

		// Token: 0x02000691 RID: 1681
		// (Invoke) Token: 0x0600206F RID: 8303
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsVideoFinishedDelegate(UIntPtr pointer);

		// Token: 0x02000692 RID: 1682
		// (Invoke) Token: 0x06002073 RID: 8307
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PlayVideoDelegate(UIntPtr pointer, byte[] videoFileName, byte[] soundFileName, float framerate, [MarshalAs(UnmanagedType.U1)] bool looping);

		// Token: 0x02000693 RID: 1683
		// (Invoke) Token: 0x06002077 RID: 8311
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StopVideoDelegate(UIntPtr pointer);
	}
}
