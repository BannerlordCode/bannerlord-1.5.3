using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000009 RID: 9
	internal class ScriptingInterfaceOfIMBActionSet : IMBActionSet
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00009121 File Offset: 0x00007321
		public bool AreActionsAlternatives(int index, int actionNo1, int actionNo2)
		{
			return ScriptingInterfaceOfIMBActionSet.call_AreActionsAlternativesDelegate(index, actionNo1, actionNo2);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00009130 File Offset: 0x00007330
		public string GetAnimationName(int index, int actionNo)
		{
			if (ScriptingInterfaceOfIMBActionSet.call_GetAnimationNameDelegate(index, actionNo) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00009148 File Offset: 0x00007348
		public bool GetBoneHasParentBone(string actionSetId, sbyte boneIndex)
		{
			byte[] array = null;
			if (actionSetId != null)
			{
				int byteCount = ScriptingInterfaceOfIMBActionSet._utf8.GetByteCount(actionSetId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBActionSet._utf8.GetBytes(actionSetId, 0, actionSetId.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBActionSet.call_GetBoneHasParentBoneDelegate(array, boneIndex);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000091A4 File Offset: 0x000073A4
		public sbyte GetBoneIndexWithId(string actionSetId, string boneId)
		{
			byte[] array = null;
			if (actionSetId != null)
			{
				int byteCount = ScriptingInterfaceOfIMBActionSet._utf8.GetByteCount(actionSetId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBActionSet._utf8.GetBytes(actionSetId, 0, actionSetId.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (boneId != null)
			{
				int byteCount2 = ScriptingInterfaceOfIMBActionSet._utf8.GetByteCount(boneId);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIMBActionSet._utf8.GetBytes(boneId, 0, boneId.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfIMBActionSet.call_GetBoneIndexWithIdDelegate(array, array2);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00009244 File Offset: 0x00007444
		public int GetIndexWithID(string id)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIMBActionSet._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBActionSet._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBActionSet.call_GetIndexWithIDDelegate(array);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000929E File Offset: 0x0000749E
		public string GetNameWithIndex(int index)
		{
			if (ScriptingInterfaceOfIMBActionSet.call_GetNameWithIndexDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000092B5 File Offset: 0x000074B5
		public int GetNumberOfActionSets()
		{
			return ScriptingInterfaceOfIMBActionSet.call_GetNumberOfActionSetsDelegate();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000092C1 File Offset: 0x000074C1
		public int GetNumberOfMonsterUsageSets()
		{
			return ScriptingInterfaceOfIMBActionSet.call_GetNumberOfMonsterUsageSetsDelegate();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000092CD File Offset: 0x000074CD
		public string GetSkeletonName(int index)
		{
			if (ScriptingInterfaceOfIMBActionSet.call_GetSkeletonNameDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x04000024 RID: 36
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000025 RID: 37
		public static ScriptingInterfaceOfIMBActionSet.AreActionsAlternativesDelegate call_AreActionsAlternativesDelegate;

		// Token: 0x04000026 RID: 38
		public static ScriptingInterfaceOfIMBActionSet.GetAnimationNameDelegate call_GetAnimationNameDelegate;

		// Token: 0x04000027 RID: 39
		public static ScriptingInterfaceOfIMBActionSet.GetBoneHasParentBoneDelegate call_GetBoneHasParentBoneDelegate;

		// Token: 0x04000028 RID: 40
		public static ScriptingInterfaceOfIMBActionSet.GetBoneIndexWithIdDelegate call_GetBoneIndexWithIdDelegate;

		// Token: 0x04000029 RID: 41
		public static ScriptingInterfaceOfIMBActionSet.GetIndexWithIDDelegate call_GetIndexWithIDDelegate;

		// Token: 0x0400002A RID: 42
		public static ScriptingInterfaceOfIMBActionSet.GetNameWithIndexDelegate call_GetNameWithIndexDelegate;

		// Token: 0x0400002B RID: 43
		public static ScriptingInterfaceOfIMBActionSet.GetNumberOfActionSetsDelegate call_GetNumberOfActionSetsDelegate;

		// Token: 0x0400002C RID: 44
		public static ScriptingInterfaceOfIMBActionSet.GetNumberOfMonsterUsageSetsDelegate call_GetNumberOfMonsterUsageSetsDelegate;

		// Token: 0x0400002D RID: 45
		public static ScriptingInterfaceOfIMBActionSet.GetSkeletonNameDelegate call_GetSkeletonNameDelegate;

		// Token: 0x02000095 RID: 149
		// (Invoke) Token: 0x06000544 RID: 1348
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool AreActionsAlternativesDelegate(int index, int actionNo1, int actionNo2);

		// Token: 0x02000096 RID: 150
		// (Invoke) Token: 0x06000548 RID: 1352
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationNameDelegate(int index, int actionNo);

		// Token: 0x02000097 RID: 151
		// (Invoke) Token: 0x0600054C RID: 1356
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetBoneHasParentBoneDelegate(byte[] actionSetId, sbyte boneIndex);

		// Token: 0x02000098 RID: 152
		// (Invoke) Token: 0x06000550 RID: 1360
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneIndexWithIdDelegate(byte[] actionSetId, byte[] boneId);

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000554 RID: 1364
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetIndexWithIDDelegate(byte[] id);

		// Token: 0x0200009A RID: 154
		// (Invoke) Token: 0x06000558 RID: 1368
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameWithIndexDelegate(int index);

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x0600055C RID: 1372
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfActionSetsDelegate();

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x06000560 RID: 1376
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfMonsterUsageSetsDelegate();

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x06000564 RID: 1380
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSkeletonNameDelegate(int index);
	}
}
