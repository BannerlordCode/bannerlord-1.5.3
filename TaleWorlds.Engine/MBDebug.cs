using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000062 RID: 98
	public static class MBDebug
	{
		// Token: 0x0600097F RID: 2431 RVA: 0x00008D92 File Offset: 0x00006F92
		[CommandLineFunctionality.CommandLineArgumentFunction("toggle_ui", "ui")]
		public static string DisableUI(List<string> strings)
		{
			if (strings.Count != 0)
			{
				return "Invalid input.";
			}
			MBDebug.DisableAllUI = !MBDebug.DisableAllUI;
			if (MBDebug.DisableAllUI)
			{
				return "UI is now disabled.";
			}
			return "UI is now enabled.";
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00008DCD File Offset: 0x00006FCD
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void AssertMemoryUsage(int memoryMB)
		{
			EngineApplicationInterface.IDebug.AssertMemoryUsage(memoryMB);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00008DDA File Offset: 0x00006FDA
		public static void AbortGame(int ExitCode = 5)
		{
			EngineApplicationInterface.IDebug.AbortGame(ExitCode);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public static void ShowWarning(string message)
		{
			bool flag = EngineApplicationInterface.IDebug.Warning(message);
			if (Debugger.IsAttached && flag)
			{
				Debugger.Break();
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00008E10 File Offset: 0x00007010
		public static void ContentWarning(string message)
		{
			bool flag = EngineApplicationInterface.IDebug.ContentWarning(message);
			if (Debugger.IsAttached && flag)
			{
				Debugger.Break();
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00008E38 File Offset: 0x00007038
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void ConditionalContentWarning(bool condition, string message)
		{
			if (!condition)
			{
				bool flag = EngineApplicationInterface.IDebug.ContentWarning(message);
				if (Debugger.IsAttached && flag)
				{
					Debugger.Break();
				}
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00008E64 File Offset: 0x00007064
		public static void ShowError(string message)
		{
			bool flag = EngineApplicationInterface.IDebug.Error(message);
			if (Debugger.IsAttached && flag)
			{
				Debugger.Break();
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00008E8B File Offset: 0x0000708B
		public static int ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
			return EngineApplicationInterface.IDebug.MessageBox(lpText, lpCaption, uType);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00008E9C File Offset: 0x0000709C
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void Assert(bool condition, string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
		{
			if (!condition)
			{
				bool flag = EngineApplicationInterface.IDebug.FailedAssert(message, callerFile, callerMethod, callerLine);
				if (Debugger.IsAttached && flag)
				{
					Debugger.Break();
				}
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00008ECA File Offset: 0x000070CA
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void FailedAssert(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
		{
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00008ECC File Offset: 0x000070CC
		public static void SilentAssert(bool condition, string message = "", bool getDump = false, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
		{
			if (!condition)
			{
				bool flag = EngineApplicationInterface.IDebug.SilentAssert(message, callerFile, callerMethod, callerLine, getDump);
				if (Debugger.IsAttached && flag)
				{
					Debugger.Break();
				}
			}
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00008EFC File Offset: 0x000070FC
		[Conditional("DEBUG_MORE")]
		public static void AssertConditionOrCallerClassName(bool condition, string name)
		{
			StackFrame frame = new StackTrace(2, true).GetFrame(0);
			if (!condition)
			{
				string name2 = frame.GetMethod().DeclaringType.Name;
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00008F2C File Offset: 0x0000712C
		[Conditional("DEBUG_MORE")]
		public static void AssertConditionOrCallerClassNameSearchAllCallstack(bool condition, string name)
		{
			StackTrace stackTrace = new StackTrace(true);
			if (!condition)
			{
				int num = 0;
				while (num < stackTrace.FrameCount && !(stackTrace.GetFrame(num).GetMethod().DeclaringType.Name == name))
				{
					num++;
				}
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00008F74 File Offset: 0x00007174
		public static void Print(string message, int logLevel = 0, Debug.DebugColor color = Debug.DebugColor.White, ulong debugFilter = 17592186044416UL)
		{
			if (MBDebug.DisableLogging)
			{
				return;
			}
			debugFilter &= 18446744069414584320UL;
			if (debugFilter == 0UL)
			{
				return;
			}
			try
			{
				if (EngineApplicationInterface.IDebug != null)
				{
					EngineApplicationInterface.IDebug.WriteLine(logLevel, message, (int)color, debugFilter);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00008FC8 File Offset: 0x000071C8
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void ConsolePrint(string message, Debug.DebugColor color = Debug.DebugColor.White, ulong debugFilter = 17592186044416UL)
		{
			try
			{
				EngineApplicationInterface.IDebug.WriteLine(0, message, (int)color, debugFilter);
			}
			catch
			{
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00008FF8 File Offset: 0x000071F8
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void WriteDebugLineOnScreen(string str)
		{
			EngineApplicationInterface.IDebug.WriteDebugLineOnScreen(str);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00009005 File Offset: 0x00007205
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugText(float screenX, float screenY, string text, uint color = 4294967295U, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugText(screenX, screenY, text, color, time);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00009017 File Offset: 0x00007217
		public static void RenderText(float screenX, float screenY, string text, uint color = 4294967295U, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugText(screenX, screenY, text, color, time);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00009029 File Offset: 0x00007229
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugRect(float left, float bottom, float right, float top)
		{
			EngineApplicationInterface.IDebug.RenderDebugRect(left, bottom, right, top);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00009039 File Offset: 0x00007239
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color = 4294967295U)
		{
			EngineApplicationInterface.IDebug.RenderDebugRectWithColor(left, bottom, right, top, color);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0000904B File Offset: 0x0000724B
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugFrame(MatrixFrame frame, float lineLength, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugFrame(ref frame, lineLength, time);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0000905B File Offset: 0x0000725B
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugText3D(Vec3 worldPosition, string str, uint color = 4294967295U, int screenPosOffsetX = 0, int screenPosOffsetY = 0, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugText3d(worldPosition, str, color, screenPosOffsetX, screenPosOffsetY, time);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0000906F File Offset: 0x0000726F
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugDirectionArrow(Vec3 position, Vec3 direction, uint color = 4294967295U, bool depthCheck = false)
		{
			EngineApplicationInterface.IDebug.RenderDebugDirectionArrow(position, direction, color, depthCheck);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0000907F File Offset: 0x0000727F
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugLine(Vec3 position, Vec3 direction, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugLine(position, direction, color, depthCheck, time);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00009091 File Offset: 0x00007291
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugSphere(Vec3 position, float radius, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugSphere(position, radius, color, depthCheck, time);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000090A3 File Offset: 0x000072A3
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugCapsule(Vec3 p0, Vec3 p1, float radius, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugCapsule(p0, p1, radius, color, depthCheck, time);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000090B8 File Offset: 0x000072B8
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugBoundingBoxOfEntity(GameEntity entity, MatrixFrame frame, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			Vec3 boundingBoxMin = entity.GetBoundingBoxMin();
			Vec3 boundingBoxMax = entity.GetBoundingBoxMax();
			List<Vec3> list = new List<Vec3>();
			list.Add(new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMin.z, -1f));
			list.Add(new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMin.z, -1f));
			list.Add(new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMin.z, -1f));
			list.Add(new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMin.z, -1f));
			list.Add(new Vec3(boundingBoxMin.x, boundingBoxMin.y, boundingBoxMax.z, -1f));
			list.Add(new Vec3(boundingBoxMax.x, boundingBoxMin.y, boundingBoxMax.z, -1f));
			list.Add(new Vec3(boundingBoxMax.x, boundingBoxMax.y, boundingBoxMax.z, -1f));
			list.Add(new Vec3(boundingBoxMin.x, boundingBoxMax.y, boundingBoxMax.z, -1f));
			for (int i = 0; i < list.Count / 2; i++)
			{
				Vec3 vec = list[i];
				frame.TransformToParent(in vec);
				vec = list[(i + 1) % (list.Count / 2)];
				frame.TransformToParent(in vec);
				vec = list[i + list.Count / 2];
				frame.TransformToParent(in vec);
				vec = list[(i + 1) % (list.Count / 2) + list.Count / 2];
				frame.TransformToParent(in vec);
			}
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00009274 File Offset: 0x00007474
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugBoundingBox(BoundingBox box, MatrixFrame frame, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			Vec3 min = box.min;
			Vec3 max = box.max;
			List<Vec3> list = new List<Vec3>();
			list.Add(new Vec3(min.x, min.y, min.z, -1f));
			list.Add(new Vec3(max.x, min.y, min.z, -1f));
			list.Add(new Vec3(max.x, max.y, min.z, -1f));
			list.Add(new Vec3(min.x, max.y, min.z, -1f));
			list.Add(new Vec3(min.x, min.y, max.z, -1f));
			list.Add(new Vec3(max.x, min.y, max.z, -1f));
			list.Add(new Vec3(max.x, max.y, max.z, -1f));
			list.Add(new Vec3(min.x, max.y, max.z, -1f));
			for (int i = 0; i < list.Count / 2; i++)
			{
				Vec3 vec = list[i];
				frame.TransformToParent(in vec);
				vec = list[(i + 1) % (list.Count / 2)];
				frame.TransformToParent(in vec);
				vec = list[i + list.Count / 2];
				frame.TransformToParent(in vec);
				vec = list[(i + 1) % (list.Count / 2) + list.Count / 2];
				frame.TransformToParent(in vec);
			}
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0000942F File Offset: 0x0000762F
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void ClearRenderObjects()
		{
			EngineApplicationInterface.IDebug.ClearAllDebugRenderObjects();
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0000943B File Offset: 0x0000763B
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x00009447 File Offset: 0x00007647
		public static Vec3 DebugVector
		{
			get
			{
				return EngineApplicationInterface.IDebug.GetDebugVector();
			}
			set
			{
				EngineApplicationInterface.IDebug.SetDebugVector(value);
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00009454 File Offset: 0x00007654
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugBoxObject(Vec3 min, Vec3 max, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugBoxObject(min, max, color, depthCheck, time);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00009466 File Offset: 0x00007666
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugBoxObject(Vec3 min, Vec3 max, MatrixFrame frame, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			EngineApplicationInterface.IDebug.RenderDebugBoxObjectWithFrame(min, max, ref frame, color, depthCheck, time);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0000947B File Offset: 0x0000767B
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void PostWarningLine(string line)
		{
			EngineApplicationInterface.IDebug.PostWarningLine(line);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00009488 File Offset: 0x00007688
		public static bool IsErrorReportModeActive()
		{
			return EngineApplicationInterface.IDebug.IsErrorReportModeActive();
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00009494 File Offset: 0x00007694
		public static bool IsErrorReportModePauseMission()
		{
			return EngineApplicationInterface.IDebug.IsErrorReportModePauseMission();
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000094A0 File Offset: 0x000076A0
		public static void SetErrorReportScene(Scene scene)
		{
			UIntPtr uintPtr = ((scene == null) ? UIntPtr.Zero : scene.Pointer);
			EngineApplicationInterface.IDebug.SetErrorReportScene(uintPtr);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000094CF File Offset: 0x000076CF
		public static void SetDumpGenerationDisabled(bool value)
		{
			EngineApplicationInterface.IDebug.SetDumpGenerationDisabled(value);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000094DC File Offset: 0x000076DC
		public static void EchoCommandWindow(string content)
		{
			EngineApplicationInterface.IDebug.EchoCommandWindow(content);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x000094E9 File Offset: 0x000076E9
		[CommandLineFunctionality.CommandLineArgumentFunction("clear", "console")]
		public static string ClearConsole(List<string> strings)
		{
			Console.Clear();
			return "Debug console cleared.";
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x000094F5 File Offset: 0x000076F5
		[CommandLineFunctionality.CommandLineArgumentFunction("echo_command_window", "console")]
		public static string EchoCommandWindow(List<string> strings)
		{
			MBDebug.EchoCommandWindow(strings[0]);
			return "";
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00009508 File Offset: 0x00007708
		[CommandLineFunctionality.CommandLineArgumentFunction("echo_command_window_test", "console")]
		public static string EchoCommandWindowTest(List<string> strings)
		{
			MBDebug.EchoCommandWindowTestAux();
			return "";
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00009514 File Offset: 0x00007714
		private static async void EchoCommandWindowTestAux()
		{
			MBDebug.EchoCommandWindow("5...");
			await Task.Delay(1000);
			MBDebug.EchoCommandWindow("4...");
			await Task.Delay(1000);
			MBDebug.EchoCommandWindow("3...");
			await Task.Delay(1000);
			MBDebug.EchoCommandWindow("2...");
			await Task.Delay(1000);
			MBDebug.EchoCommandWindow("1...");
			await Task.Delay(1000);
			MBDebug.EchoCommandWindow("Tada!");
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00009545 File Offset: 0x00007745
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x00009551 File Offset: 0x00007751
		public static int ShowDebugInfoState
		{
			get
			{
				return EngineApplicationInterface.IDebug.GetShowDebugInfo();
			}
			set
			{
				EngineApplicationInterface.IDebug.SetShowDebugInfo(value);
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0000955E File Offset: 0x0000775E
		public static bool IsTestMode()
		{
			return EngineApplicationInterface.IDebug.IsTestMode();
		}

		// Token: 0x0400010A RID: 266
		public static bool DisableAllUI;

		// Token: 0x0400010B RID: 267
		public static bool TestModeEnabled;

		// Token: 0x0400010C RID: 268
		public static bool ShouldAssertThrowException;

		// Token: 0x0400010D RID: 269
		public static bool IsDisplayingHighLevelAI;

		// Token: 0x0400010E RID: 270
		public static bool DisableLogging;

		// Token: 0x0400010F RID: 271
		private static readonly Dictionary<string, int> ProcessedFrameList = new Dictionary<string, int>();

		// Token: 0x020000C9 RID: 201
		[Flags]
		public enum MessageBoxTypeFlag
		{
			// Token: 0x0400041E RID: 1054
			Ok = 1,
			// Token: 0x0400041F RID: 1055
			Warning = 2,
			// Token: 0x04000420 RID: 1056
			Error = 4,
			// Token: 0x04000421 RID: 1057
			OkCancel = 8,
			// Token: 0x04000422 RID: 1058
			RetryCancel = 16,
			// Token: 0x04000423 RID: 1059
			YesNo = 32,
			// Token: 0x04000424 RID: 1060
			YesNoCancel = 64,
			// Token: 0x04000425 RID: 1061
			Information = 128,
			// Token: 0x04000426 RID: 1062
			Exclamation = 256,
			// Token: 0x04000427 RID: 1063
			Question = 512,
			// Token: 0x04000428 RID: 1064
			AssertFailed = 1024
		}
	}
}
