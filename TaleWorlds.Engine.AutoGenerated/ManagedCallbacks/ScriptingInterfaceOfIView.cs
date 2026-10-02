using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000033 RID: 51
	internal class ScriptingInterfaceOfIView : IView
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0001C40F File Offset: 0x0001A60F
		public void SetAutoDepthTargetCreation(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetAutoDepthTargetCreationDelegate(ptr, value);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001C41D File Offset: 0x0001A61D
		public void SetClearColor(UIntPtr ptr, uint rgba)
		{
			ScriptingInterfaceOfIView.call_SetClearColorDelegate(ptr, rgba);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001C42B File Offset: 0x0001A62B
		public void SetDebugRenderFunctionality(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetDebugRenderFunctionalityDelegate(ptr, value);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001C439 File Offset: 0x0001A639
		public void SetDepthTarget(UIntPtr ptr, UIntPtr texture_ptr)
		{
			ScriptingInterfaceOfIView.call_SetDepthTargetDelegate(ptr, texture_ptr);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001C447 File Offset: 0x0001A647
		public void SetEnable(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetEnableDelegate(ptr, value);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001C458 File Offset: 0x0001A658
		public void SetFileNameToSaveResult(UIntPtr ptr, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIView._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIView._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIView.call_SetFileNameToSaveResultDelegate(ptr, array);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001C4B4 File Offset: 0x0001A6B4
		public void SetFilePathToSaveResult(UIntPtr ptr, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIView._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIView._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIView.call_SetFilePathToSaveResultDelegate(ptr, array);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001C50F File Offset: 0x0001A70F
		public void SetFileTypeToSave(UIntPtr ptr, int type)
		{
			ScriptingInterfaceOfIView.call_SetFileTypeToSaveDelegate(ptr, type);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001C51D File Offset: 0x0001A71D
		public void SetOffset(UIntPtr ptr, float x, float y)
		{
			ScriptingInterfaceOfIView.call_SetOffsetDelegate(ptr, x, y);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001C52C File Offset: 0x0001A72C
		public void SetRenderOnDemand(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetRenderOnDemandDelegate(ptr, value);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001C53A File Offset: 0x0001A73A
		public void SetRenderOption(UIntPtr ptr, int optionEnum, bool value)
		{
			ScriptingInterfaceOfIView.call_SetRenderOptionDelegate(ptr, optionEnum, value);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001C549 File Offset: 0x0001A749
		public void SetRenderOrder(UIntPtr ptr, int value)
		{
			ScriptingInterfaceOfIView.call_SetRenderOrderDelegate(ptr, value);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001C557 File Offset: 0x0001A757
		public void SetRenderTarget(UIntPtr ptr, UIntPtr texture_ptr)
		{
			ScriptingInterfaceOfIView.call_SetRenderTargetDelegate(ptr, texture_ptr);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001C565 File Offset: 0x0001A765
		public void SetSaveFinalResultToDisk(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetSaveFinalResultToDiskDelegate(ptr, value);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001C573 File Offset: 0x0001A773
		public void SetScale(UIntPtr ptr, float x, float y)
		{
			ScriptingInterfaceOfIView.call_SetScaleDelegate(ptr, x, y);
		}

		// Token: 0x04000637 RID: 1591
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000638 RID: 1592
		public static ScriptingInterfaceOfIView.SetAutoDepthTargetCreationDelegate call_SetAutoDepthTargetCreationDelegate;

		// Token: 0x04000639 RID: 1593
		public static ScriptingInterfaceOfIView.SetClearColorDelegate call_SetClearColorDelegate;

		// Token: 0x0400063A RID: 1594
		public static ScriptingInterfaceOfIView.SetDebugRenderFunctionalityDelegate call_SetDebugRenderFunctionalityDelegate;

		// Token: 0x0400063B RID: 1595
		public static ScriptingInterfaceOfIView.SetDepthTargetDelegate call_SetDepthTargetDelegate;

		// Token: 0x0400063C RID: 1596
		public static ScriptingInterfaceOfIView.SetEnableDelegate call_SetEnableDelegate;

		// Token: 0x0400063D RID: 1597
		public static ScriptingInterfaceOfIView.SetFileNameToSaveResultDelegate call_SetFileNameToSaveResultDelegate;

		// Token: 0x0400063E RID: 1598
		public static ScriptingInterfaceOfIView.SetFilePathToSaveResultDelegate call_SetFilePathToSaveResultDelegate;

		// Token: 0x0400063F RID: 1599
		public static ScriptingInterfaceOfIView.SetFileTypeToSaveDelegate call_SetFileTypeToSaveDelegate;

		// Token: 0x04000640 RID: 1600
		public static ScriptingInterfaceOfIView.SetOffsetDelegate call_SetOffsetDelegate;

		// Token: 0x04000641 RID: 1601
		public static ScriptingInterfaceOfIView.SetRenderOnDemandDelegate call_SetRenderOnDemandDelegate;

		// Token: 0x04000642 RID: 1602
		public static ScriptingInterfaceOfIView.SetRenderOptionDelegate call_SetRenderOptionDelegate;

		// Token: 0x04000643 RID: 1603
		public static ScriptingInterfaceOfIView.SetRenderOrderDelegate call_SetRenderOrderDelegate;

		// Token: 0x04000644 RID: 1604
		public static ScriptingInterfaceOfIView.SetRenderTargetDelegate call_SetRenderTargetDelegate;

		// Token: 0x04000645 RID: 1605
		public static ScriptingInterfaceOfIView.SetSaveFinalResultToDiskDelegate call_SetSaveFinalResultToDiskDelegate;

		// Token: 0x04000646 RID: 1606
		public static ScriptingInterfaceOfIView.SetScaleDelegate call_SetScaleDelegate;

		// Token: 0x02000694 RID: 1684
		// (Invoke) Token: 0x0600207B RID: 8315
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAutoDepthTargetCreationDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000695 RID: 1685
		// (Invoke) Token: 0x0600207F RID: 8319
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetClearColorDelegate(UIntPtr ptr, uint rgba);

		// Token: 0x02000696 RID: 1686
		// (Invoke) Token: 0x06002083 RID: 8323
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDebugRenderFunctionalityDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000697 RID: 1687
		// (Invoke) Token: 0x06002087 RID: 8327
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDepthTargetDelegate(UIntPtr ptr, UIntPtr texture_ptr);

		// Token: 0x02000698 RID: 1688
		// (Invoke) Token: 0x0600208B RID: 8331
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEnableDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000699 RID: 1689
		// (Invoke) Token: 0x0600208F RID: 8335
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFileNameToSaveResultDelegate(UIntPtr ptr, byte[] name);

		// Token: 0x0200069A RID: 1690
		// (Invoke) Token: 0x06002093 RID: 8339
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFilePathToSaveResultDelegate(UIntPtr ptr, byte[] name);

		// Token: 0x0200069B RID: 1691
		// (Invoke) Token: 0x06002097 RID: 8343
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFileTypeToSaveDelegate(UIntPtr ptr, int type);

		// Token: 0x0200069C RID: 1692
		// (Invoke) Token: 0x0600209B RID: 8347
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetOffsetDelegate(UIntPtr ptr, float x, float y);

		// Token: 0x0200069D RID: 1693
		// (Invoke) Token: 0x0600209F RID: 8351
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderOnDemandDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200069E RID: 1694
		// (Invoke) Token: 0x060020A3 RID: 8355
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderOptionDelegate(UIntPtr ptr, int optionEnum, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200069F RID: 1695
		// (Invoke) Token: 0x060020A7 RID: 8359
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderOrderDelegate(UIntPtr ptr, int value);

		// Token: 0x020006A0 RID: 1696
		// (Invoke) Token: 0x060020AB RID: 8363
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderTargetDelegate(UIntPtr ptr, UIntPtr texture_ptr);

		// Token: 0x020006A1 RID: 1697
		// (Invoke) Token: 0x060020AF RID: 8367
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSaveFinalResultToDiskDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020006A2 RID: 1698
		// (Invoke) Token: 0x060020B3 RID: 8371
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetScaleDelegate(UIntPtr ptr, float x, float y);
	}
}
