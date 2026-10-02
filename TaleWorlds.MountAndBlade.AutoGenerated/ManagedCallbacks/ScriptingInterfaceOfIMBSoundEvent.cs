using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001F RID: 31
	internal class ScriptingInterfaceOfIMBSoundEvent : IMBSoundEvent
	{
		// Token: 0x06000350 RID: 848 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
		public int CreateEventFromExternalFile(string programmerSoundEventName, string filePath, UIntPtr scene, bool is3d, bool isBlocking)
		{
			byte[] array = null;
			if (programmerSoundEventName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSoundEvent._utf8.GetByteCount(programmerSoundEventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSoundEvent._utf8.GetBytes(programmerSoundEventName, 0, programmerSoundEventName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (filePath != null)
			{
				int byteCount2 = ScriptingInterfaceOfIMBSoundEvent._utf8.GetByteCount(filePath);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIMBSoundEvent._utf8.GetBytes(filePath, 0, filePath.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfIMBSoundEvent.call_CreateEventFromExternalFileDelegate(array, array2, scene, is3d, isBlocking);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000D678 File Offset: 0x0000B878
		public int CreateEventFromSoundBuffer(string programmerSoundEventName, byte[] soundBuffer, UIntPtr scene, bool is3d, bool isBlocking)
		{
			byte[] array = null;
			if (programmerSoundEventName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSoundEvent._utf8.GetByteCount(programmerSoundEventName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSoundEvent._utf8.GetBytes(programmerSoundEventName, 0, programmerSoundEventName.Length, array, 0);
				array[byteCount] = 0;
			}
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(soundBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (soundBuffer != null) ? soundBuffer.Length : 0);
			int num = ScriptingInterfaceOfIMBSoundEvent.call_CreateEventFromSoundBufferDelegate(array, managedArray, scene, is3d, isBlocking);
			pinnedArrayData.Dispose();
			return num;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000D705 File Offset: 0x0000B905
		public bool PlaySound(int fmodEventIndex, in Vec3 position)
		{
			return ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundDelegate(fmodEventIndex, in position);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000D713 File Offset: 0x0000B913
		public bool PlaySoundWithIntParam(int fmodEventIndex, int paramIndex, float paramVal, in Vec3 position)
		{
			return ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundWithIntParamDelegate(fmodEventIndex, paramIndex, paramVal, in position);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000D724 File Offset: 0x0000B924
		public bool PlaySoundWithParam(int soundCodeId, SoundEventParameter parameter, in Vec3 position)
		{
			return ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundWithParamDelegate(soundCodeId, parameter, in position);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000D734 File Offset: 0x0000B934
		public bool PlaySoundWithStrParam(int fmodEventIndex, string paramName, float paramVal, in Vec3 position)
		{
			byte[] array = null;
			if (paramName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSoundEvent._utf8.GetByteCount(paramName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSoundEvent._utf8.GetBytes(paramName, 0, paramName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBSoundEvent.call_PlaySoundWithStrParamDelegate(fmodEventIndex, array, paramVal, in position);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000D7A6 File Offset: 0x0000B9A6
		bool IMBSoundEvent.PlaySound(int fmodEventIndex, in Vec3 position)
		{
			return this.PlaySound(fmodEventIndex, in position);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		bool IMBSoundEvent.PlaySoundWithIntParam(int fmodEventIndex, int paramIndex, float paramVal, in Vec3 position)
		{
			return this.PlaySoundWithIntParam(fmodEventIndex, paramIndex, paramVal, in position);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000D7BD File Offset: 0x0000B9BD
		bool IMBSoundEvent.PlaySoundWithStrParam(int fmodEventIndex, string paramName, float paramVal, in Vec3 position)
		{
			return this.PlaySoundWithStrParam(fmodEventIndex, paramName, paramVal, in position);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000D7CA File Offset: 0x0000B9CA
		bool IMBSoundEvent.PlaySoundWithParam(int soundCodeId, SoundEventParameter parameter, in Vec3 position)
		{
			return this.PlaySoundWithParam(soundCodeId, parameter, in position);
		}

		// Token: 0x040002C3 RID: 707
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002C4 RID: 708
		public static ScriptingInterfaceOfIMBSoundEvent.CreateEventFromExternalFileDelegate call_CreateEventFromExternalFileDelegate;

		// Token: 0x040002C5 RID: 709
		public static ScriptingInterfaceOfIMBSoundEvent.CreateEventFromSoundBufferDelegate call_CreateEventFromSoundBufferDelegate;

		// Token: 0x040002C6 RID: 710
		public static ScriptingInterfaceOfIMBSoundEvent.PlaySoundDelegate call_PlaySoundDelegate;

		// Token: 0x040002C7 RID: 711
		public static ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithIntParamDelegate call_PlaySoundWithIntParamDelegate;

		// Token: 0x040002C8 RID: 712
		public static ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithParamDelegate call_PlaySoundWithParamDelegate;

		// Token: 0x040002C9 RID: 713
		public static ScriptingInterfaceOfIMBSoundEvent.PlaySoundWithStrParamDelegate call_PlaySoundWithStrParamDelegate;

		// Token: 0x0200031E RID: 798
		// (Invoke) Token: 0x06000F68 RID: 3944
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CreateEventFromExternalFileDelegate(byte[] programmerSoundEventName, byte[] filePath, UIntPtr scene, [MarshalAs(UnmanagedType.U1)] bool is3d, [MarshalAs(UnmanagedType.U1)] bool isBlocking);

		// Token: 0x0200031F RID: 799
		// (Invoke) Token: 0x06000F6C RID: 3948
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CreateEventFromSoundBufferDelegate(byte[] programmerSoundEventName, ManagedArray soundBuffer, UIntPtr scene, [MarshalAs(UnmanagedType.U1)] bool is3d, [MarshalAs(UnmanagedType.U1)] bool isBlocking);

		// Token: 0x02000320 RID: 800
		// (Invoke) Token: 0x06000F70 RID: 3952
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool PlaySoundDelegate(int fmodEventIndex, in Vec3 position);

		// Token: 0x02000321 RID: 801
		// (Invoke) Token: 0x06000F74 RID: 3956
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool PlaySoundWithIntParamDelegate(int fmodEventIndex, int paramIndex, float paramVal, in Vec3 position);

		// Token: 0x02000322 RID: 802
		// (Invoke) Token: 0x06000F78 RID: 3960
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool PlaySoundWithParamDelegate(int soundCodeId, SoundEventParameter parameter, in Vec3 position);

		// Token: 0x02000323 RID: 803
		// (Invoke) Token: 0x06000F7C RID: 3964
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool PlaySoundWithStrParamDelegate(int fmodEventIndex, byte[] paramName, float paramVal, in Vec3 position);
	}
}
