using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001D RID: 29
	internal class ScriptingInterfaceOfIMBScreen : IMBScreen
	{
		// Token: 0x0600033B RID: 827 RVA: 0x0000D2BE File Offset: 0x0000B4BE
		public void OnEditModeEnterPress()
		{
			ScriptingInterfaceOfIMBScreen.call_OnEditModeEnterPressDelegate();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000D2CA File Offset: 0x0000B4CA
		public void OnEditModeEnterRelease()
		{
			ScriptingInterfaceOfIMBScreen.call_OnEditModeEnterReleaseDelegate();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000D2D6 File Offset: 0x0000B4D6
		public void OnExitButtonClick()
		{
			ScriptingInterfaceOfIMBScreen.call_OnExitButtonClickDelegate();
		}

		// Token: 0x040002B0 RID: 688
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002B1 RID: 689
		public static ScriptingInterfaceOfIMBScreen.OnEditModeEnterPressDelegate call_OnEditModeEnterPressDelegate;

		// Token: 0x040002B2 RID: 690
		public static ScriptingInterfaceOfIMBScreen.OnEditModeEnterReleaseDelegate call_OnEditModeEnterReleaseDelegate;

		// Token: 0x040002B3 RID: 691
		public static ScriptingInterfaceOfIMBScreen.OnExitButtonClickDelegate call_OnExitButtonClickDelegate;

		// Token: 0x0200030D RID: 781
		// (Invoke) Token: 0x06000F24 RID: 3876
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnEditModeEnterPressDelegate();

		// Token: 0x0200030E RID: 782
		// (Invoke) Token: 0x06000F28 RID: 3880
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnEditModeEnterReleaseDelegate();

		// Token: 0x0200030F RID: 783
		// (Invoke) Token: 0x06000F2C RID: 3884
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnExitButtonClickDelegate();
	}
}
