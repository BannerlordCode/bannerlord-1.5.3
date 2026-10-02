using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200000E RID: 14
	internal class ScriptingInterfaceOfIDebug : IDebug
	{
		// Token: 0x060000ED RID: 237 RVA: 0x0000F8D7 File Offset: 0x0000DAD7
		public void AbortGame(int ExitCode)
		{
			ScriptingInterfaceOfIDebug.call_AbortGameDelegate(ExitCode);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000F8E4 File Offset: 0x0000DAE4
		public void AssertMemoryUsage(int memoryMB)
		{
			ScriptingInterfaceOfIDebug.call_AssertMemoryUsageDelegate(memoryMB);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000F8F1 File Offset: 0x0000DAF1
		public void ClearAllDebugRenderObjects()
		{
			ScriptingInterfaceOfIDebug.call_ClearAllDebugRenderObjectsDelegate();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000F900 File Offset: 0x0000DB00
		public bool ContentWarning(string MessageString)
		{
			byte[] array = null;
			if (MessageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(MessageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(MessageString, 0, MessageString.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_ContentWarningDelegate(array);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000F95C File Offset: 0x0000DB5C
		public void EchoCommandWindow(string content)
		{
			byte[] array = null;
			if (content != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(content);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(content, 0, content.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_EchoCommandWindowDelegate(array);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000F9B8 File Offset: 0x0000DBB8
		public bool Error(string MessageString)
		{
			byte[] array = null;
			if (MessageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(MessageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(MessageString, 0, MessageString.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_ErrorDelegate(array);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000FA14 File Offset: 0x0000DC14
		public bool FailedAssert(string messageString, string callerFile, string callerMethod, int callerLine)
		{
			byte[] array = null;
			if (messageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(messageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(messageString, 0, messageString.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (callerFile != null)
			{
				int byteCount2 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerFile);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerFile, 0, callerFile.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (callerMethod != null)
			{
				int byteCount3 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerMethod);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerMethod, 0, callerMethod.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_FailedAssertDelegate(array, array2, array3, callerLine);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000FAFE File Offset: 0x0000DCFE
		public Vec3 GetDebugVector()
		{
			return ScriptingInterfaceOfIDebug.call_GetDebugVectorDelegate();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000FB0A File Offset: 0x0000DD0A
		public int GetShowDebugInfo()
		{
			return ScriptingInterfaceOfIDebug.call_GetShowDebugInfoDelegate();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000FB16 File Offset: 0x0000DD16
		public bool IsErrorReportModeActive()
		{
			return ScriptingInterfaceOfIDebug.call_IsErrorReportModeActiveDelegate();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000FB22 File Offset: 0x0000DD22
		public bool IsErrorReportModePauseMission()
		{
			return ScriptingInterfaceOfIDebug.call_IsErrorReportModePauseMissionDelegate();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000FB2E File Offset: 0x0000DD2E
		public bool IsTestMode()
		{
			return ScriptingInterfaceOfIDebug.call_IsTestModeDelegate();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000FB3C File Offset: 0x0000DD3C
		public int MessageBox(string lpText, string lpCaption, uint uType)
		{
			byte[] array = null;
			if (lpText != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(lpText);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(lpText, 0, lpText.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (lpCaption != null)
			{
				int byteCount2 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(lpCaption);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(lpCaption, 0, lpCaption.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_MessageBoxDelegate(array, array2, uType);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000FBDC File Offset: 0x0000DDDC
		public void PostWarningLine(string line)
		{
			byte[] array = null;
			if (line != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(line);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(line, 0, line.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_PostWarningLineDelegate(array);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000FC36 File Offset: 0x0000DE36
		public void RenderDebugBoxObject(Vec3 min, Vec3 max, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugBoxObjectDelegate(min, max, color, depthCheck, time);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000FC49 File Offset: 0x0000DE49
		public void RenderDebugBoxObjectWithFrame(Vec3 min, Vec3 max, ref MatrixFrame frame, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugBoxObjectWithFrameDelegate(min, max, ref frame, color, depthCheck, time);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000FC5E File Offset: 0x0000DE5E
		public void RenderDebugCapsule(Vec3 p0, Vec3 p1, float radius, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugCapsuleDelegate(p0, p1, radius, color, depthCheck, time);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000FC73 File Offset: 0x0000DE73
		public void RenderDebugDirectionArrow(Vec3 position, Vec3 direction, uint color, bool depthCheck)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugDirectionArrowDelegate(position, direction, color, depthCheck);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000FC84 File Offset: 0x0000DE84
		public void RenderDebugFrame(ref MatrixFrame frame, float lineLength, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugFrameDelegate(ref frame, lineLength, time);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000FC93 File Offset: 0x0000DE93
		public void RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugLineDelegate(position, direction, color, depthCheck, time);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000FCA6 File Offset: 0x0000DEA6
		public void RenderDebugRect(float left, float bottom, float right, float top)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugRectDelegate(left, bottom, right, top);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000FCB7 File Offset: 0x0000DEB7
		public void RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugRectWithColorDelegate(left, bottom, right, top, color);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000FCCA File Offset: 0x0000DECA
		public void RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugSphereDelegate(position, radius, color, depthCheck, time);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000FCE0 File Offset: 0x0000DEE0
		public void RenderDebugText(float screenX, float screenY, string str, uint color, float time)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_RenderDebugTextDelegate(screenX, screenY, array, color, time);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000FD40 File Offset: 0x0000DF40
		public void RenderDebugText3d(Vec3 worldPosition, string str, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_RenderDebugText3dDelegate(worldPosition, array, color, screenPosOffsetX, screenPosOffsetY, time);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
		public void SetDebugVector(Vec3 debugVector)
		{
			ScriptingInterfaceOfIDebug.call_SetDebugVectorDelegate(debugVector);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000FDAF File Offset: 0x0000DFAF
		public void SetDumpGenerationDisabled(bool Disabled)
		{
			ScriptingInterfaceOfIDebug.call_SetDumpGenerationDisabledDelegate(Disabled);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		public void SetErrorReportScene(UIntPtr scenePointer)
		{
			ScriptingInterfaceOfIDebug.call_SetErrorReportSceneDelegate(scenePointer);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000FDC9 File Offset: 0x0000DFC9
		public void SetShowDebugInfo(int value)
		{
			ScriptingInterfaceOfIDebug.call_SetShowDebugInfoDelegate(value);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		public bool SilentAssert(string messageString, string callerFile, string callerMethod, int callerLine, bool getDump)
		{
			byte[] array = null;
			if (messageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(messageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(messageString, 0, messageString.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (callerFile != null)
			{
				int byteCount2 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerFile);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerFile, 0, callerFile.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (callerMethod != null)
			{
				int byteCount3 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerMethod);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerMethod, 0, callerMethod.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_SilentAssertDelegate(array, array2, array3, callerLine, getDump);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000FEC4 File Offset: 0x0000E0C4
		public bool Warning(string MessageString)
		{
			byte[] array = null;
			if (MessageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(MessageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(MessageString, 0, MessageString.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_WarningDelegate(array);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000FF20 File Offset: 0x0000E120
		public void WriteDebugLineOnScreen(string line)
		{
			byte[] array = null;
			if (line != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(line);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(line, 0, line.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_WriteDebugLineOnScreenDelegate(array);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public void WriteLine(int logLevel, string line, int color, ulong filter)
		{
			byte[] array = null;
			if (line != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(line);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(line, 0, line.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_WriteLineDelegate(logLevel, array, color, filter);
		}

		// Token: 0x04000080 RID: 128
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000081 RID: 129
		public static ScriptingInterfaceOfIDebug.AbortGameDelegate call_AbortGameDelegate;

		// Token: 0x04000082 RID: 130
		public static ScriptingInterfaceOfIDebug.AssertMemoryUsageDelegate call_AssertMemoryUsageDelegate;

		// Token: 0x04000083 RID: 131
		public static ScriptingInterfaceOfIDebug.ClearAllDebugRenderObjectsDelegate call_ClearAllDebugRenderObjectsDelegate;

		// Token: 0x04000084 RID: 132
		public static ScriptingInterfaceOfIDebug.ContentWarningDelegate call_ContentWarningDelegate;

		// Token: 0x04000085 RID: 133
		public static ScriptingInterfaceOfIDebug.EchoCommandWindowDelegate call_EchoCommandWindowDelegate;

		// Token: 0x04000086 RID: 134
		public static ScriptingInterfaceOfIDebug.ErrorDelegate call_ErrorDelegate;

		// Token: 0x04000087 RID: 135
		public static ScriptingInterfaceOfIDebug.FailedAssertDelegate call_FailedAssertDelegate;

		// Token: 0x04000088 RID: 136
		public static ScriptingInterfaceOfIDebug.GetDebugVectorDelegate call_GetDebugVectorDelegate;

		// Token: 0x04000089 RID: 137
		public static ScriptingInterfaceOfIDebug.GetShowDebugInfoDelegate call_GetShowDebugInfoDelegate;

		// Token: 0x0400008A RID: 138
		public static ScriptingInterfaceOfIDebug.IsErrorReportModeActiveDelegate call_IsErrorReportModeActiveDelegate;

		// Token: 0x0400008B RID: 139
		public static ScriptingInterfaceOfIDebug.IsErrorReportModePauseMissionDelegate call_IsErrorReportModePauseMissionDelegate;

		// Token: 0x0400008C RID: 140
		public static ScriptingInterfaceOfIDebug.IsTestModeDelegate call_IsTestModeDelegate;

		// Token: 0x0400008D RID: 141
		public static ScriptingInterfaceOfIDebug.MessageBoxDelegate call_MessageBoxDelegate;

		// Token: 0x0400008E RID: 142
		public static ScriptingInterfaceOfIDebug.PostWarningLineDelegate call_PostWarningLineDelegate;

		// Token: 0x0400008F RID: 143
		public static ScriptingInterfaceOfIDebug.RenderDebugBoxObjectDelegate call_RenderDebugBoxObjectDelegate;

		// Token: 0x04000090 RID: 144
		public static ScriptingInterfaceOfIDebug.RenderDebugBoxObjectWithFrameDelegate call_RenderDebugBoxObjectWithFrameDelegate;

		// Token: 0x04000091 RID: 145
		public static ScriptingInterfaceOfIDebug.RenderDebugCapsuleDelegate call_RenderDebugCapsuleDelegate;

		// Token: 0x04000092 RID: 146
		public static ScriptingInterfaceOfIDebug.RenderDebugDirectionArrowDelegate call_RenderDebugDirectionArrowDelegate;

		// Token: 0x04000093 RID: 147
		public static ScriptingInterfaceOfIDebug.RenderDebugFrameDelegate call_RenderDebugFrameDelegate;

		// Token: 0x04000094 RID: 148
		public static ScriptingInterfaceOfIDebug.RenderDebugLineDelegate call_RenderDebugLineDelegate;

		// Token: 0x04000095 RID: 149
		public static ScriptingInterfaceOfIDebug.RenderDebugRectDelegate call_RenderDebugRectDelegate;

		// Token: 0x04000096 RID: 150
		public static ScriptingInterfaceOfIDebug.RenderDebugRectWithColorDelegate call_RenderDebugRectWithColorDelegate;

		// Token: 0x04000097 RID: 151
		public static ScriptingInterfaceOfIDebug.RenderDebugSphereDelegate call_RenderDebugSphereDelegate;

		// Token: 0x04000098 RID: 152
		public static ScriptingInterfaceOfIDebug.RenderDebugTextDelegate call_RenderDebugTextDelegate;

		// Token: 0x04000099 RID: 153
		public static ScriptingInterfaceOfIDebug.RenderDebugText3dDelegate call_RenderDebugText3dDelegate;

		// Token: 0x0400009A RID: 154
		public static ScriptingInterfaceOfIDebug.SetDebugVectorDelegate call_SetDebugVectorDelegate;

		// Token: 0x0400009B RID: 155
		public static ScriptingInterfaceOfIDebug.SetDumpGenerationDisabledDelegate call_SetDumpGenerationDisabledDelegate;

		// Token: 0x0400009C RID: 156
		public static ScriptingInterfaceOfIDebug.SetErrorReportSceneDelegate call_SetErrorReportSceneDelegate;

		// Token: 0x0400009D RID: 157
		public static ScriptingInterfaceOfIDebug.SetShowDebugInfoDelegate call_SetShowDebugInfoDelegate;

		// Token: 0x0400009E RID: 158
		public static ScriptingInterfaceOfIDebug.SilentAssertDelegate call_SilentAssertDelegate;

		// Token: 0x0400009F RID: 159
		public static ScriptingInterfaceOfIDebug.WarningDelegate call_WarningDelegate;

		// Token: 0x040000A0 RID: 160
		public static ScriptingInterfaceOfIDebug.WriteDebugLineOnScreenDelegate call_WriteDebugLineOnScreenDelegate;

		// Token: 0x040000A1 RID: 161
		public static ScriptingInterfaceOfIDebug.WriteLineDelegate call_WriteLineDelegate;

		// Token: 0x02000102 RID: 258
		// (Invoke) Token: 0x06000A33 RID: 2611
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AbortGameDelegate(int ExitCode);

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x06000A37 RID: 2615
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AssertMemoryUsageDelegate(int memoryMB);

		// Token: 0x02000104 RID: 260
		// (Invoke) Token: 0x06000A3B RID: 2619
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearAllDebugRenderObjectsDelegate();

		// Token: 0x02000105 RID: 261
		// (Invoke) Token: 0x06000A3F RID: 2623
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ContentWarningDelegate(byte[] MessageString);

		// Token: 0x02000106 RID: 262
		// (Invoke) Token: 0x06000A43 RID: 2627
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EchoCommandWindowDelegate(byte[] content);

		// Token: 0x02000107 RID: 263
		// (Invoke) Token: 0x06000A47 RID: 2631
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ErrorDelegate(byte[] MessageString);

		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x06000A4B RID: 2635
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool FailedAssertDelegate(byte[] messageString, byte[] callerFile, byte[] callerMethod, int callerLine);

		// Token: 0x02000109 RID: 265
		// (Invoke) Token: 0x06000A4F RID: 2639
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetDebugVectorDelegate();

		// Token: 0x0200010A RID: 266
		// (Invoke) Token: 0x06000A53 RID: 2643
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetShowDebugInfoDelegate();

		// Token: 0x0200010B RID: 267
		// (Invoke) Token: 0x06000A57 RID: 2647
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsErrorReportModeActiveDelegate();

		// Token: 0x0200010C RID: 268
		// (Invoke) Token: 0x06000A5B RID: 2651
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsErrorReportModePauseMissionDelegate();

		// Token: 0x0200010D RID: 269
		// (Invoke) Token: 0x06000A5F RID: 2655
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsTestModeDelegate();

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x06000A63 RID: 2659
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int MessageBoxDelegate(byte[] lpText, byte[] lpCaption, uint uType);

		// Token: 0x0200010F RID: 271
		// (Invoke) Token: 0x06000A67 RID: 2663
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PostWarningLineDelegate(byte[] line);

		// Token: 0x02000110 RID: 272
		// (Invoke) Token: 0x06000A6B RID: 2667
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugBoxObjectDelegate(Vec3 min, Vec3 max, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000111 RID: 273
		// (Invoke) Token: 0x06000A6F RID: 2671
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugBoxObjectWithFrameDelegate(Vec3 min, Vec3 max, ref MatrixFrame frame, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000112 RID: 274
		// (Invoke) Token: 0x06000A73 RID: 2675
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugCapsuleDelegate(Vec3 p0, Vec3 p1, float radius, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000113 RID: 275
		// (Invoke) Token: 0x06000A77 RID: 2679
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugDirectionArrowDelegate(Vec3 position, Vec3 direction, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck);

		// Token: 0x02000114 RID: 276
		// (Invoke) Token: 0x06000A7B RID: 2683
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugFrameDelegate(ref MatrixFrame frame, float lineLength, float time);

		// Token: 0x02000115 RID: 277
		// (Invoke) Token: 0x06000A7F RID: 2687
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugLineDelegate(Vec3 position, Vec3 direction, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000116 RID: 278
		// (Invoke) Token: 0x06000A83 RID: 2691
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugRectDelegate(float left, float bottom, float right, float top);

		// Token: 0x02000117 RID: 279
		// (Invoke) Token: 0x06000A87 RID: 2695
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugRectWithColorDelegate(float left, float bottom, float right, float top, uint color);

		// Token: 0x02000118 RID: 280
		// (Invoke) Token: 0x06000A8B RID: 2699
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugSphereDelegate(Vec3 position, float radius, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000119 RID: 281
		// (Invoke) Token: 0x06000A8F RID: 2703
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugTextDelegate(float screenX, float screenY, byte[] str, uint color, float time);

		// Token: 0x0200011A RID: 282
		// (Invoke) Token: 0x06000A93 RID: 2707
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugText3dDelegate(Vec3 worldPosition, byte[] str, uint color, int screenPosOffsetX, int screenPosOffsetY, float time);

		// Token: 0x0200011B RID: 283
		// (Invoke) Token: 0x06000A97 RID: 2711
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDebugVectorDelegate(Vec3 debugVector);

		// Token: 0x0200011C RID: 284
		// (Invoke) Token: 0x06000A9B RID: 2715
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDumpGenerationDisabledDelegate([MarshalAs(UnmanagedType.U1)] bool Disabled);

		// Token: 0x0200011D RID: 285
		// (Invoke) Token: 0x06000A9F RID: 2719
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetErrorReportSceneDelegate(UIntPtr scenePointer);

		// Token: 0x0200011E RID: 286
		// (Invoke) Token: 0x06000AA3 RID: 2723
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetShowDebugInfoDelegate(int value);

		// Token: 0x0200011F RID: 287
		// (Invoke) Token: 0x06000AA7 RID: 2727
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool SilentAssertDelegate(byte[] messageString, byte[] callerFile, byte[] callerMethod, int callerLine, [MarshalAs(UnmanagedType.U1)] bool getDump);

		// Token: 0x02000120 RID: 288
		// (Invoke) Token: 0x06000AAB RID: 2731
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool WarningDelegate(byte[] MessageString);

		// Token: 0x02000121 RID: 289
		// (Invoke) Token: 0x06000AAF RID: 2735
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteDebugLineOnScreenDelegate(byte[] line);

		// Token: 0x02000122 RID: 290
		// (Invoke) Token: 0x06000AB3 RID: 2739
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteLineDelegate(int logLevel, byte[] line, int color, ulong filter);
	}
}
