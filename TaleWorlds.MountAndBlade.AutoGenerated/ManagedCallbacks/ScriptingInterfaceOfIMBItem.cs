using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000016 RID: 22
	internal class ScriptingInterfaceOfIMBItem : IMBItem
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public void GetHolsterFrameByIndex(int index, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIMBItem.call_GetHolsterFrameByIndexDelegate(index, ref outFrame);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000BB04 File Offset: 0x00009D04
		public int GetItemHolsterIndex(string itemholstername)
		{
			byte[] array = null;
			if (itemholstername != null)
			{
				int byteCount = ScriptingInterfaceOfIMBItem._utf8.GetByteCount(itemholstername);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBItem._utf8.GetBytes(itemholstername, 0, itemholstername.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBItem.call_GetItemHolsterIndexDelegate(array);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000BB60 File Offset: 0x00009D60
		public bool GetItemIsPassiveUsage(string itemUsageName)
		{
			byte[] array = null;
			if (itemUsageName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBItem._utf8.GetByteCount(itemUsageName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBItem._utf8.GetBytes(itemUsageName, 0, itemUsageName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBItem.call_GetItemIsPassiveUsageDelegate(array);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000BBBC File Offset: 0x00009DBC
		public int GetItemUsageIndex(string itemusagename)
		{
			byte[] array = null;
			if (itemusagename != null)
			{
				int byteCount = ScriptingInterfaceOfIMBItem._utf8.GetByteCount(itemusagename);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBItem._utf8.GetBytes(itemusagename, 0, itemusagename.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBItem.call_GetItemUsageIndexDelegate(array);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000BC18 File Offset: 0x00009E18
		public int GetItemUsageReloadActionCode(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection)
		{
			byte[] array = null;
			if (itemUsageName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBItem._utf8.GetByteCount(itemUsageName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBItem._utf8.GetBytes(itemUsageName, 0, itemUsageName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBItem.call_GetItemUsageReloadActionCodeDelegate(array, usageDirection, isMounted, leftHandUsageSetIndex, isLeftStance, isLowLookDirection);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000BC7C File Offset: 0x00009E7C
		public int GetItemUsageSetFlags(string ItemUsageName)
		{
			byte[] array = null;
			if (ItemUsageName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBItem._utf8.GetByteCount(ItemUsageName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBItem._utf8.GetBytes(ItemUsageName, 0, ItemUsageName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBItem.call_GetItemUsageSetFlagsDelegate(array);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		public int GetItemUsageStrikeType(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection)
		{
			byte[] array = null;
			if (itemUsageName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBItem._utf8.GetByteCount(itemUsageName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBItem._utf8.GetBytes(itemUsageName, 0, itemUsageName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBItem.call_GetItemUsageStrikeTypeDelegate(array, usageDirection, isMounted, leftHandUsageSetIndex, isLeftStance, isLowLookDirection);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000BD3A File Offset: 0x00009F3A
		public float GetMissileRange(float shootSpeed, float zDiff)
		{
			return ScriptingInterfaceOfIMBItem.call_GetMissileRangeDelegate(shootSpeed, zDiff);
		}

		// Token: 0x040001D7 RID: 471
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001D8 RID: 472
		public static ScriptingInterfaceOfIMBItem.GetHolsterFrameByIndexDelegate call_GetHolsterFrameByIndexDelegate;

		// Token: 0x040001D9 RID: 473
		public static ScriptingInterfaceOfIMBItem.GetItemHolsterIndexDelegate call_GetItemHolsterIndexDelegate;

		// Token: 0x040001DA RID: 474
		public static ScriptingInterfaceOfIMBItem.GetItemIsPassiveUsageDelegate call_GetItemIsPassiveUsageDelegate;

		// Token: 0x040001DB RID: 475
		public static ScriptingInterfaceOfIMBItem.GetItemUsageIndexDelegate call_GetItemUsageIndexDelegate;

		// Token: 0x040001DC RID: 476
		public static ScriptingInterfaceOfIMBItem.GetItemUsageReloadActionCodeDelegate call_GetItemUsageReloadActionCodeDelegate;

		// Token: 0x040001DD RID: 477
		public static ScriptingInterfaceOfIMBItem.GetItemUsageSetFlagsDelegate call_GetItemUsageSetFlagsDelegate;

		// Token: 0x040001DE RID: 478
		public static ScriptingInterfaceOfIMBItem.GetItemUsageStrikeTypeDelegate call_GetItemUsageStrikeTypeDelegate;

		// Token: 0x040001DF RID: 479
		public static ScriptingInterfaceOfIMBItem.GetMissileRangeDelegate call_GetMissileRangeDelegate;

		// Token: 0x0200023B RID: 571
		// (Invoke) Token: 0x06000BDC RID: 3036
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetHolsterFrameByIndexDelegate(int index, ref MatrixFrame outFrame);

		// Token: 0x0200023C RID: 572
		// (Invoke) Token: 0x06000BE0 RID: 3040
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetItemHolsterIndexDelegate(byte[] itemholstername);

		// Token: 0x0200023D RID: 573
		// (Invoke) Token: 0x06000BE4 RID: 3044
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetItemIsPassiveUsageDelegate(byte[] itemUsageName);

		// Token: 0x0200023E RID: 574
		// (Invoke) Token: 0x06000BE8 RID: 3048
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetItemUsageIndexDelegate(byte[] itemusagename);

		// Token: 0x0200023F RID: 575
		// (Invoke) Token: 0x06000BEC RID: 3052
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetItemUsageReloadActionCodeDelegate(byte[] itemUsageName, int usageDirection, [MarshalAs(UnmanagedType.U1)] bool isMounted, int leftHandUsageSetIndex, [MarshalAs(UnmanagedType.U1)] bool isLeftStance, [MarshalAs(UnmanagedType.U1)] bool isLowLookDirection);

		// Token: 0x02000240 RID: 576
		// (Invoke) Token: 0x06000BF0 RID: 3056
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetItemUsageSetFlagsDelegate(byte[] ItemUsageName);

		// Token: 0x02000241 RID: 577
		// (Invoke) Token: 0x06000BF4 RID: 3060
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetItemUsageStrikeTypeDelegate(byte[] itemUsageName, int usageDirection, [MarshalAs(UnmanagedType.U1)] bool isMounted, int leftHandUsageSetIndex, [MarshalAs(UnmanagedType.U1)] bool isLeftStance, [MarshalAs(UnmanagedType.U1)] bool isLowLookDirection);

		// Token: 0x02000242 RID: 578
		// (Invoke) Token: 0x06000BF8 RID: 3064
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetMissileRangeDelegate(float shootSpeed, float zDiff);
	}
}
