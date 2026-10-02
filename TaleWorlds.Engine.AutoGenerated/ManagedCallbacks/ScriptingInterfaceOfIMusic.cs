using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200001D RID: 29
	internal class ScriptingInterfaceOfIMusic : IMusic
	{
		// Token: 0x060003AA RID: 938 RVA: 0x00014E78 File Offset: 0x00013078
		public int GetFreeMusicChannelIndex()
		{
			return ScriptingInterfaceOfIMusic.call_GetFreeMusicChannelIndexDelegate();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00014E84 File Offset: 0x00013084
		public bool IsClipLoaded(int index)
		{
			return ScriptingInterfaceOfIMusic.call_IsClipLoadedDelegate(index);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00014E91 File Offset: 0x00013091
		public bool IsMusicPlaying(int index)
		{
			return ScriptingInterfaceOfIMusic.call_IsMusicPlayingDelegate(index);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00014EA0 File Offset: 0x000130A0
		public void LoadClip(int index, string pathToClip)
		{
			byte[] array = null;
			if (pathToClip != null)
			{
				int byteCount = ScriptingInterfaceOfIMusic._utf8.GetByteCount(pathToClip);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMusic._utf8.GetBytes(pathToClip, 0, pathToClip.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMusic.call_LoadClipDelegate(index, array);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00014EFB File Offset: 0x000130FB
		public void PauseMusic(int index)
		{
			ScriptingInterfaceOfIMusic.call_PauseMusicDelegate(index);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00014F08 File Offset: 0x00013108
		public void PlayDelayed(int index, int delayMilliseconds)
		{
			ScriptingInterfaceOfIMusic.call_PlayDelayedDelegate(index, delayMilliseconds);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00014F16 File Offset: 0x00013116
		public void PlayMusic(int index)
		{
			ScriptingInterfaceOfIMusic.call_PlayMusicDelegate(index);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00014F23 File Offset: 0x00013123
		public void SetVolume(int index, float volume)
		{
			ScriptingInterfaceOfIMusic.call_SetVolumeDelegate(index, volume);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00014F31 File Offset: 0x00013131
		public void StopMusic(int index)
		{
			ScriptingInterfaceOfIMusic.call_StopMusicDelegate(index);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00014F3E File Offset: 0x0001313E
		public void UnloadClip(int index)
		{
			ScriptingInterfaceOfIMusic.call_UnloadClipDelegate(index);
		}

		// Token: 0x0400031B RID: 795
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400031C RID: 796
		public static ScriptingInterfaceOfIMusic.GetFreeMusicChannelIndexDelegate call_GetFreeMusicChannelIndexDelegate;

		// Token: 0x0400031D RID: 797
		public static ScriptingInterfaceOfIMusic.IsClipLoadedDelegate call_IsClipLoadedDelegate;

		// Token: 0x0400031E RID: 798
		public static ScriptingInterfaceOfIMusic.IsMusicPlayingDelegate call_IsMusicPlayingDelegate;

		// Token: 0x0400031F RID: 799
		public static ScriptingInterfaceOfIMusic.LoadClipDelegate call_LoadClipDelegate;

		// Token: 0x04000320 RID: 800
		public static ScriptingInterfaceOfIMusic.PauseMusicDelegate call_PauseMusicDelegate;

		// Token: 0x04000321 RID: 801
		public static ScriptingInterfaceOfIMusic.PlayDelayedDelegate call_PlayDelayedDelegate;

		// Token: 0x04000322 RID: 802
		public static ScriptingInterfaceOfIMusic.PlayMusicDelegate call_PlayMusicDelegate;

		// Token: 0x04000323 RID: 803
		public static ScriptingInterfaceOfIMusic.SetVolumeDelegate call_SetVolumeDelegate;

		// Token: 0x04000324 RID: 804
		public static ScriptingInterfaceOfIMusic.StopMusicDelegate call_StopMusicDelegate;

		// Token: 0x04000325 RID: 805
		public static ScriptingInterfaceOfIMusic.UnloadClipDelegate call_UnloadClipDelegate;

		// Token: 0x0200038E RID: 910
		// (Invoke) Token: 0x06001463 RID: 5219
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFreeMusicChannelIndexDelegate();

		// Token: 0x0200038F RID: 911
		// (Invoke) Token: 0x06001467 RID: 5223
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsClipLoadedDelegate(int index);

		// Token: 0x02000390 RID: 912
		// (Invoke) Token: 0x0600146B RID: 5227
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsMusicPlayingDelegate(int index);

		// Token: 0x02000391 RID: 913
		// (Invoke) Token: 0x0600146F RID: 5231
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadClipDelegate(int index, byte[] pathToClip);

		// Token: 0x02000392 RID: 914
		// (Invoke) Token: 0x06001473 RID: 5235
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseMusicDelegate(int index);

		// Token: 0x02000393 RID: 915
		// (Invoke) Token: 0x06001477 RID: 5239
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PlayDelayedDelegate(int index, int delayMilliseconds);

		// Token: 0x02000394 RID: 916
		// (Invoke) Token: 0x0600147B RID: 5243
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PlayMusicDelegate(int index);

		// Token: 0x02000395 RID: 917
		// (Invoke) Token: 0x0600147F RID: 5247
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVolumeDelegate(int index, float volume);

		// Token: 0x02000396 RID: 918
		// (Invoke) Token: 0x06001483 RID: 5251
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StopMusicDelegate(int index);

		// Token: 0x02000397 RID: 919
		// (Invoke) Token: 0x06001487 RID: 5255
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnloadClipDelegate(int index);
	}
}
