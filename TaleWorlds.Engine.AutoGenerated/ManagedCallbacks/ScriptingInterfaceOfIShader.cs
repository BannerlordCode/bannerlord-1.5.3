using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000026 RID: 38
	internal class ScriptingInterfaceOfIShader : IShader
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x000183E4 File Offset: 0x000165E4
		public Shader GetFromResource(string shaderName)
		{
			byte[] array = null;
			if (shaderName != null)
			{
				int byteCount = ScriptingInterfaceOfIShader._utf8.GetByteCount(shaderName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIShader._utf8.GetBytes(shaderName, 0, shaderName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIShader.call_GetFromResourceDelegate(array);
			Shader shader = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				shader = new Shader(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return shader;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00018470 File Offset: 0x00016670
		public ulong GetMaterialShaderFlagMask(UIntPtr shaderPointer, string flagName, bool showError)
		{
			byte[] array = null;
			if (flagName != null)
			{
				int byteCount = ScriptingInterfaceOfIShader._utf8.GetByteCount(flagName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIShader._utf8.GetBytes(flagName, 0, flagName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIShader.call_GetMaterialShaderFlagMaskDelegate(shaderPointer, array, showError);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000184CC File Offset: 0x000166CC
		public string GetName(UIntPtr shaderPointer)
		{
			if (ScriptingInterfaceOfIShader.call_GetNameDelegate(shaderPointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000184E3 File Offset: 0x000166E3
		public void Release(UIntPtr shaderPointer)
		{
			ScriptingInterfaceOfIShader.call_ReleaseDelegate(shaderPointer);
		}

		// Token: 0x040004CB RID: 1227
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004CC RID: 1228
		public static ScriptingInterfaceOfIShader.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x040004CD RID: 1229
		public static ScriptingInterfaceOfIShader.GetMaterialShaderFlagMaskDelegate call_GetMaterialShaderFlagMaskDelegate;

		// Token: 0x040004CE RID: 1230
		public static ScriptingInterfaceOfIShader.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040004CF RID: 1231
		public static ScriptingInterfaceOfIShader.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x02000535 RID: 1333
		// (Invoke) Token: 0x06001AFF RID: 6911
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] shaderName);

		// Token: 0x02000536 RID: 1334
		// (Invoke) Token: 0x06001B03 RID: 6915
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetMaterialShaderFlagMaskDelegate(UIntPtr shaderPointer, byte[] flagName, [MarshalAs(UnmanagedType.U1)] bool showError);

		// Token: 0x02000537 RID: 1335
		// (Invoke) Token: 0x06001B07 RID: 6919
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr shaderPointer);

		// Token: 0x02000538 RID: 1336
		// (Invoke) Token: 0x06001B0B RID: 6923
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr shaderPointer);
	}
}
