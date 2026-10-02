using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000025 RID: 37
	internal class ScriptingInterfaceOfIScriptComponent : IScriptComponent
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x000182E6 File Offset: 0x000164E6
		public string GetName(UIntPtr pointer)
		{
			if (ScriptingInterfaceOfIScriptComponent.call_GetNameDelegate(pointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000182FD File Offset: 0x000164FD
		public ScriptComponentBehavior GetScriptComponentBehavior(UIntPtr pointer)
		{
			return DotNetObject.GetManagedObjectWithId(ScriptingInterfaceOfIScriptComponent.call_GetScriptComponentBehaviorDelegate(pointer)) as ScriptComponentBehavior;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00018314 File Offset: 0x00016514
		public void SetVariableEditorWidgetStatus(UIntPtr pointer, string field, bool enabled)
		{
			byte[] array = null;
			if (field != null)
			{
				int byteCount = ScriptingInterfaceOfIScriptComponent._utf8.GetByteCount(field);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIScriptComponent._utf8.GetBytes(field, 0, field.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIScriptComponent.call_SetVariableEditorWidgetStatusDelegate(pointer, array, enabled);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00018370 File Offset: 0x00016570
		public void SetVariableEditorWidgetValue(UIntPtr pointer, string field, RglScriptFieldType fieldType, double value)
		{
			byte[] array = null;
			if (field != null)
			{
				int byteCount = ScriptingInterfaceOfIScriptComponent._utf8.GetByteCount(field);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIScriptComponent._utf8.GetBytes(field, 0, field.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIScriptComponent.call_SetVariableEditorWidgetValueDelegate(pointer, array, fieldType, value);
		}

		// Token: 0x040004C6 RID: 1222
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004C7 RID: 1223
		public static ScriptingInterfaceOfIScriptComponent.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040004C8 RID: 1224
		public static ScriptingInterfaceOfIScriptComponent.GetScriptComponentBehaviorDelegate call_GetScriptComponentBehaviorDelegate;

		// Token: 0x040004C9 RID: 1225
		public static ScriptingInterfaceOfIScriptComponent.SetVariableEditorWidgetStatusDelegate call_SetVariableEditorWidgetStatusDelegate;

		// Token: 0x040004CA RID: 1226
		public static ScriptingInterfaceOfIScriptComponent.SetVariableEditorWidgetValueDelegate call_SetVariableEditorWidgetValueDelegate;

		// Token: 0x02000531 RID: 1329
		// (Invoke) Token: 0x06001AEF RID: 6895
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr pointer);

		// Token: 0x02000532 RID: 1330
		// (Invoke) Token: 0x06001AF3 RID: 6899
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetScriptComponentBehaviorDelegate(UIntPtr pointer);

		// Token: 0x02000533 RID: 1331
		// (Invoke) Token: 0x06001AF7 RID: 6903
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVariableEditorWidgetStatusDelegate(UIntPtr pointer, byte[] field, [MarshalAs(UnmanagedType.U1)] bool enabled);

		// Token: 0x02000534 RID: 1332
		// (Invoke) Token: 0x06001AFB RID: 6907
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVariableEditorWidgetValueDelegate(UIntPtr pointer, byte[] field, RglScriptFieldType fieldType, double value);
	}
}
