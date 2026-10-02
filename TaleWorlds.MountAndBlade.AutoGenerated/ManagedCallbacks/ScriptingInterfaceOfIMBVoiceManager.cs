using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000022 RID: 34
	internal class ScriptingInterfaceOfIMBVoiceManager : IMBVoiceManager
	{
		// Token: 0x0600036C RID: 876 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		public int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(string className)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIMBVoiceManager._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBVoiceManager._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate(array);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000D944 File Offset: 0x0000BB44
		public void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(string className, int[] definitionIndices)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIMBVoiceManager._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBVoiceManager._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			PinnedArrayData<int> pinnedArrayData = new PinnedArrayData<int>(definitionIndices, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate(array, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		public int GetVoiceTypeIndex(string voiceType)
		{
			byte[] array = null;
			if (voiceType != null)
			{
				int byteCount = ScriptingInterfaceOfIMBVoiceManager._utf8.GetByteCount(voiceType);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBVoiceManager._utf8.GetBytes(voiceType, 0, voiceType.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceTypeIndexDelegate(array);
		}

		// Token: 0x040002D8 RID: 728
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002D9 RID: 729
		public static ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate call_GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate;

		// Token: 0x040002DA RID: 730
		public static ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate call_GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate;

		// Token: 0x040002DB RID: 731
		public static ScriptingInterfaceOfIMBVoiceManager.GetVoiceTypeIndexDelegate call_GetVoiceTypeIndexDelegate;

		// Token: 0x02000330 RID: 816
		// (Invoke) Token: 0x06000FB0 RID: 4016
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate(byte[] className);

		// Token: 0x02000331 RID: 817
		// (Invoke) Token: 0x06000FB4 RID: 4020
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate(byte[] className, IntPtr definitionIndices);

		// Token: 0x02000332 RID: 818
		// (Invoke) Token: 0x06000FB8 RID: 4024
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVoiceTypeIndexDelegate(byte[] voiceType);
	}
}
