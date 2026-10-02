using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000024 RID: 36
	internal class ScriptingInterfaceOfIScreen : IScreen
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00018265 File Offset: 0x00016465
		public float GetAspectRatio()
		{
			return ScriptingInterfaceOfIScreen.call_GetAspectRatioDelegate();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00018271 File Offset: 0x00016471
		public float GetDesktopHeight()
		{
			return ScriptingInterfaceOfIScreen.call_GetDesktopHeightDelegate();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001827D File Offset: 0x0001647D
		public float GetDesktopWidth()
		{
			return ScriptingInterfaceOfIScreen.call_GetDesktopWidthDelegate();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00018289 File Offset: 0x00016489
		public bool GetMouseVisible()
		{
			return ScriptingInterfaceOfIScreen.call_GetMouseVisibleDelegate();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00018295 File Offset: 0x00016495
		public float GetRealScreenResolutionHeight()
		{
			return ScriptingInterfaceOfIScreen.call_GetRealScreenResolutionHeightDelegate();
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000182A1 File Offset: 0x000164A1
		public float GetRealScreenResolutionWidth()
		{
			return ScriptingInterfaceOfIScreen.call_GetRealScreenResolutionWidthDelegate();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000182AD File Offset: 0x000164AD
		public Vec2 GetUsableAreaPercentages()
		{
			return ScriptingInterfaceOfIScreen.call_GetUsableAreaPercentagesDelegate();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000182B9 File Offset: 0x000164B9
		public bool IsEnterButtonCross()
		{
			return ScriptingInterfaceOfIScreen.call_IsEnterButtonCrossDelegate();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000182C5 File Offset: 0x000164C5
		public void SetMouseVisible(bool value)
		{
			ScriptingInterfaceOfIScreen.call_SetMouseVisibleDelegate(value);
		}

		// Token: 0x040004BC RID: 1212
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004BD RID: 1213
		public static ScriptingInterfaceOfIScreen.GetAspectRatioDelegate call_GetAspectRatioDelegate;

		// Token: 0x040004BE RID: 1214
		public static ScriptingInterfaceOfIScreen.GetDesktopHeightDelegate call_GetDesktopHeightDelegate;

		// Token: 0x040004BF RID: 1215
		public static ScriptingInterfaceOfIScreen.GetDesktopWidthDelegate call_GetDesktopWidthDelegate;

		// Token: 0x040004C0 RID: 1216
		public static ScriptingInterfaceOfIScreen.GetMouseVisibleDelegate call_GetMouseVisibleDelegate;

		// Token: 0x040004C1 RID: 1217
		public static ScriptingInterfaceOfIScreen.GetRealScreenResolutionHeightDelegate call_GetRealScreenResolutionHeightDelegate;

		// Token: 0x040004C2 RID: 1218
		public static ScriptingInterfaceOfIScreen.GetRealScreenResolutionWidthDelegate call_GetRealScreenResolutionWidthDelegate;

		// Token: 0x040004C3 RID: 1219
		public static ScriptingInterfaceOfIScreen.GetUsableAreaPercentagesDelegate call_GetUsableAreaPercentagesDelegate;

		// Token: 0x040004C4 RID: 1220
		public static ScriptingInterfaceOfIScreen.IsEnterButtonCrossDelegate call_IsEnterButtonCrossDelegate;

		// Token: 0x040004C5 RID: 1221
		public static ScriptingInterfaceOfIScreen.SetMouseVisibleDelegate call_SetMouseVisibleDelegate;

		// Token: 0x02000528 RID: 1320
		// (Invoke) Token: 0x06001ACB RID: 6859
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAspectRatioDelegate();

		// Token: 0x02000529 RID: 1321
		// (Invoke) Token: 0x06001ACF RID: 6863
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDesktopHeightDelegate();

		// Token: 0x0200052A RID: 1322
		// (Invoke) Token: 0x06001AD3 RID: 6867
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDesktopWidthDelegate();

		// Token: 0x0200052B RID: 1323
		// (Invoke) Token: 0x06001AD7 RID: 6871
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetMouseVisibleDelegate();

		// Token: 0x0200052C RID: 1324
		// (Invoke) Token: 0x06001ADB RID: 6875
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRealScreenResolutionHeightDelegate();

		// Token: 0x0200052D RID: 1325
		// (Invoke) Token: 0x06001ADF RID: 6879
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRealScreenResolutionWidthDelegate();

		// Token: 0x0200052E RID: 1326
		// (Invoke) Token: 0x06001AE3 RID: 6883
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetUsableAreaPercentagesDelegate();

		// Token: 0x0200052F RID: 1327
		// (Invoke) Token: 0x06001AE7 RID: 6887
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEnterButtonCrossDelegate();

		// Token: 0x02000530 RID: 1328
		// (Invoke) Token: 0x06001AEB RID: 6891
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMouseVisibleDelegate([MarshalAs(UnmanagedType.U1)] bool value);
	}
}
