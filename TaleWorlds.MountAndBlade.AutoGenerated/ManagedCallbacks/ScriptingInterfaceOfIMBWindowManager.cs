using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000023 RID: 35
	internal class ScriptingInterfaceOfIMBWindowManager : IMBWindowManager
	{
		// Token: 0x06000371 RID: 881 RVA: 0x0000DA26 File Offset: 0x0000BC26
		public void DontChangeCursorPos()
		{
			ScriptingInterfaceOfIMBWindowManager.call_DontChangeCursorPosDelegate();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000DA32 File Offset: 0x0000BC32
		public void EraseMessageLines()
		{
			ScriptingInterfaceOfIMBWindowManager.call_EraseMessageLinesDelegate();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000DA3E File Offset: 0x0000BC3E
		public Vec2 GetScreenResolution()
		{
			return ScriptingInterfaceOfIMBWindowManager.call_GetScreenResolutionDelegate();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000DA4A File Offset: 0x0000BC4A
		public void PreDisplay()
		{
			ScriptingInterfaceOfIMBWindowManager.call_PreDisplayDelegate();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DA56 File Offset: 0x0000BC56
		public void ScreenToWorld(UIntPtr pointer, float screenX, float screenY, float z, ref Vec3 worldSpacePosition)
		{
			ScriptingInterfaceOfIMBWindowManager.call_ScreenToWorldDelegate(pointer, screenX, screenY, z, ref worldSpacePosition);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000DA69 File Offset: 0x0000BC69
		public float WorldToScreen(UIntPtr cameraPointer, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return ScriptingInterfaceOfIMBWindowManager.call_WorldToScreenDelegate(cameraPointer, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		public float WorldToScreenWithFixedZ(UIntPtr cameraPointer, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return ScriptingInterfaceOfIMBWindowManager.call_WorldToScreenWithFixedZDelegate(cameraPointer, cameraPosition, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x040002DC RID: 732
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002DD RID: 733
		public static ScriptingInterfaceOfIMBWindowManager.DontChangeCursorPosDelegate call_DontChangeCursorPosDelegate;

		// Token: 0x040002DE RID: 734
		public static ScriptingInterfaceOfIMBWindowManager.EraseMessageLinesDelegate call_EraseMessageLinesDelegate;

		// Token: 0x040002DF RID: 735
		public static ScriptingInterfaceOfIMBWindowManager.GetScreenResolutionDelegate call_GetScreenResolutionDelegate;

		// Token: 0x040002E0 RID: 736
		public static ScriptingInterfaceOfIMBWindowManager.PreDisplayDelegate call_PreDisplayDelegate;

		// Token: 0x040002E1 RID: 737
		public static ScriptingInterfaceOfIMBWindowManager.ScreenToWorldDelegate call_ScreenToWorldDelegate;

		// Token: 0x040002E2 RID: 738
		public static ScriptingInterfaceOfIMBWindowManager.WorldToScreenDelegate call_WorldToScreenDelegate;

		// Token: 0x040002E3 RID: 739
		public static ScriptingInterfaceOfIMBWindowManager.WorldToScreenWithFixedZDelegate call_WorldToScreenWithFixedZDelegate;

		// Token: 0x02000333 RID: 819
		// (Invoke) Token: 0x06000FBC RID: 4028
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DontChangeCursorPosDelegate();

		// Token: 0x02000334 RID: 820
		// (Invoke) Token: 0x06000FC0 RID: 4032
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EraseMessageLinesDelegate();

		// Token: 0x02000335 RID: 821
		// (Invoke) Token: 0x06000FC4 RID: 4036
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetScreenResolutionDelegate();

		// Token: 0x02000336 RID: 822
		// (Invoke) Token: 0x06000FC8 RID: 4040
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PreDisplayDelegate();

		// Token: 0x02000337 RID: 823
		// (Invoke) Token: 0x06000FCC RID: 4044
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ScreenToWorldDelegate(UIntPtr pointer, float screenX, float screenY, float z, ref Vec3 worldSpacePosition);

		// Token: 0x02000338 RID: 824
		// (Invoke) Token: 0x06000FD0 RID: 4048
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float WorldToScreenDelegate(UIntPtr cameraPointer, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);

		// Token: 0x02000339 RID: 825
		// (Invoke) Token: 0x06000FD4 RID: 4052
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float WorldToScreenWithFixedZDelegate(UIntPtr cameraPointer, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);
	}
}
