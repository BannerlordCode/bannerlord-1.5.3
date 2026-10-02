using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000010 RID: 16
	internal class ScriptingInterfaceOfIMBDebugExtensions : IMBDebugExtensions
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000B0B4 File Offset: 0x000092B4
		public void OverrideNativeParameter(string paramName, float value)
		{
			byte[] array = null;
			if (paramName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBDebugExtensions._utf8.GetByteCount(paramName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBDebugExtensions._utf8.GetBytes(paramName, 0, paramName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBDebugExtensions.call_OverrideNativeParameterDelegate(array, value);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000B10F File Offset: 0x0000930F
		public void ReloadNativeParameters()
		{
			ScriptingInterfaceOfIMBDebugExtensions.call_ReloadNativeParametersDelegate();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000B11C File Offset: 0x0000931C
		public void RenderDebugArcOnTerrain(UIntPtr scenePointer, ref MatrixFrame frame, float radius, float beginAngle, float endAngle, uint color, bool depthCheck, bool isDotted)
		{
			ScriptingInterfaceOfIMBDebugExtensions.call_RenderDebugArcOnTerrainDelegate(scenePointer, ref frame, radius, beginAngle, endAngle, color, depthCheck, isDotted);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000B140 File Offset: 0x00009340
		public void RenderDebugCircleOnTerrain(UIntPtr scenePointer, ref MatrixFrame frame, float radius, uint color, bool depthCheck, bool isDotted)
		{
			ScriptingInterfaceOfIMBDebugExtensions.call_RenderDebugCircleOnTerrainDelegate(scenePointer, ref frame, radius, color, depthCheck, isDotted);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000B158 File Offset: 0x00009358
		public void RenderDebugLineOnTerrain(UIntPtr scenePointer, Vec3 position, Vec3 direction, uint color, bool depthCheck, float time, bool isDotted, float pointDensity)
		{
			ScriptingInterfaceOfIMBDebugExtensions.call_RenderDebugLineOnTerrainDelegate(scenePointer, position, direction, color, depthCheck, time, isDotted, pointDensity);
		}

		// Token: 0x0400018F RID: 399
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000190 RID: 400
		public static ScriptingInterfaceOfIMBDebugExtensions.OverrideNativeParameterDelegate call_OverrideNativeParameterDelegate;

		// Token: 0x04000191 RID: 401
		public static ScriptingInterfaceOfIMBDebugExtensions.ReloadNativeParametersDelegate call_ReloadNativeParametersDelegate;

		// Token: 0x04000192 RID: 402
		public static ScriptingInterfaceOfIMBDebugExtensions.RenderDebugArcOnTerrainDelegate call_RenderDebugArcOnTerrainDelegate;

		// Token: 0x04000193 RID: 403
		public static ScriptingInterfaceOfIMBDebugExtensions.RenderDebugCircleOnTerrainDelegate call_RenderDebugCircleOnTerrainDelegate;

		// Token: 0x04000194 RID: 404
		public static ScriptingInterfaceOfIMBDebugExtensions.RenderDebugLineOnTerrainDelegate call_RenderDebugLineOnTerrainDelegate;

		// Token: 0x020001F9 RID: 505
		// (Invoke) Token: 0x06000AD4 RID: 2772
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OverrideNativeParameterDelegate(byte[] paramName, float value);

		// Token: 0x020001FA RID: 506
		// (Invoke) Token: 0x06000AD8 RID: 2776
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReloadNativeParametersDelegate();

		// Token: 0x020001FB RID: 507
		// (Invoke) Token: 0x06000ADC RID: 2780
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugArcOnTerrainDelegate(UIntPtr scenePointer, ref MatrixFrame frame, float radius, float beginAngle, float endAngle, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, [MarshalAs(UnmanagedType.U1)] bool isDotted);

		// Token: 0x020001FC RID: 508
		// (Invoke) Token: 0x06000AE0 RID: 2784
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugCircleOnTerrainDelegate(UIntPtr scenePointer, ref MatrixFrame frame, float radius, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, [MarshalAs(UnmanagedType.U1)] bool isDotted);

		// Token: 0x020001FD RID: 509
		// (Invoke) Token: 0x06000AE4 RID: 2788
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugLineOnTerrainDelegate(UIntPtr scenePointer, Vec3 position, Vec3 direction, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time, [MarshalAs(UnmanagedType.U1)] bool isDotted, float pointDensity);
	}
}
