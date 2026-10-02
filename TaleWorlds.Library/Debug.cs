using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000029 RID: 41
	public static class Debug
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000100 RID: 256 RVA: 0x000058F8 File Offset: 0x00003AF8
		// (remove) Token: 0x06000101 RID: 257 RVA: 0x0000592C File Offset: 0x00003B2C
		public static event Action<string, ulong> OnPrint;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000595F File Offset: 0x00003B5F
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005966 File Offset: 0x00003B66
		public static IDebugManager DebugManager { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000596E File Offset: 0x00003B6E
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005975 File Offset: 0x00003B75
		public static ITelemetryManager TelemetryManager { get; set; }

		// Token: 0x06000106 RID: 262 RVA: 0x0000597D File Offset: 0x00003B7D
		public static TelemetryLevelMask GetTelemetryLevelMask()
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return TelemetryLevelMask.Mono_0;
			}
			return telemetryManager.GetTelemetryLevelMask();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005993 File Offset: 0x00003B93
		public static void SetCrashReportCustomString(string customString)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.SetCrashReportCustomString(customString);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000059A7 File Offset: 0x00003BA7
		public static void SetCrashReportCustomStack(string customStack)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.SetCrashReportCustomStack(customStack);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000059BB File Offset: 0x00003BBB
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void Assert(bool condition, string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.Assert(condition, message, callerFile, callerMethod, callerLine);
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000059D4 File Offset: 0x00003BD4
		public static void FailedAssert(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.Assert(false, message, callerFile, callerMethod, callerLine);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000059EC File Offset: 0x00003BEC
		public static void SilentAssert(bool condition, string message = "", bool getDump = false, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.SilentAssert(condition, message, getDump, callerFile, callerMethod, callerLine);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005A07 File Offset: 0x00003C07
		public static void ShowError(string message)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.ShowError(message);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005A1B File Offset: 0x00003C1B
		internal static void DoDelayedexit(int returnCode)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.DoDelayedexit(returnCode);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005A2F File Offset: 0x00003C2F
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void ShowWarning(string message)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.ShowWarning(message);
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005A43 File Offset: 0x00003C43
		public static void ReportMemoryBookmark(string message)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.ReportMemoryBookmark(message);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005A55 File Offset: 0x00003C55
		public static void Print(string message, int logLevel = 0, Debug.DebugColor color = Debug.DebugColor.White, ulong debugFilter = 17592186044416UL)
		{
			if (Debug.DebugManager != null)
			{
				debugFilter &= 18446744069414584320UL;
				if (debugFilter == 0UL)
				{
					return;
				}
				Debug.DebugManager.Print(message, logLevel, color, debugFilter);
				Action<string, ulong> onPrint = Debug.OnPrint;
				if (onPrint == null)
				{
					return;
				}
				onPrint(message, debugFilter);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005A8E File Offset: 0x00003C8E
		public static void ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.ShowMessageBox(lpText, lpCaption, uType);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005AA4 File Offset: 0x00003CA4
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void PrintWarning(string warning, ulong debugFilter = 17592186044416UL)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.PrintWarning(warning, debugFilter);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005AB9 File Offset: 0x00003CB9
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void PrintError(string error, string stackTrace = null, ulong debugFilter = 17592186044416UL)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.PrintError(error, stackTrace, debugFilter);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005ACF File Offset: 0x00003CCF
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void DisplayDebugMessage(string message)
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.DisplayDebugMessage(message);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005AE3 File Offset: 0x00003CE3
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void WatchVariable(string name, object value)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.WatchVariable(name, value);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005AF6 File Offset: 0x00003CF6
		[Conditional("NOT_SHIPPING")]
		[Conditional("ENABLE_PROFILING_APIS_IN_SHIPPING")]
		public static void StartTelemetryConnection(bool showErrors)
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return;
			}
			telemetryManager.StartTelemetryConnection(showErrors);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005B08 File Offset: 0x00003D08
		[Conditional("NOT_SHIPPING")]
		[Conditional("ENABLE_PROFILING_APIS_IN_SHIPPING")]
		public static void StopTelemetryConnection()
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return;
			}
			telemetryManager.StopTelemetryConnection();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005B19 File Offset: 0x00003D19
		[Conditional("NOT_SHIPPING")]
		[Conditional("ENABLE_PROFILING_APIS_IN_SHIPPING")]
		internal static void BeginTelemetryScopeInternal(TelemetryLevelMask levelMask, string scopeName)
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return;
			}
			telemetryManager.BeginTelemetryScopeInternal(levelMask, scopeName);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005B2C File Offset: 0x00003D2C
		[Conditional("NOT_SHIPPING")]
		[Conditional("ENABLE_PROFILING_APIS_IN_SHIPPING")]
		internal static void BeginTelemetryScopeBaseLevelInternal(TelemetryLevelMask levelMask, string scopeName)
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return;
			}
			telemetryManager.BeginTelemetryScopeBaseLevelInternal(levelMask, scopeName);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005B3F File Offset: 0x00003D3F
		[Conditional("NOT_SHIPPING")]
		[Conditional("ENABLE_PROFILING_APIS_IN_SHIPPING")]
		internal static void EndTelemetryScopeInternal()
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return;
			}
			telemetryManager.EndTelemetryScopeInternal();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005B50 File Offset: 0x00003D50
		[Conditional("NOT_SHIPPING")]
		[Conditional("ENABLE_PROFILING_APIS_IN_SHIPPING")]
		internal static void EndTelemetryScopeBaseLevelInternal()
		{
			ITelemetryManager telemetryManager = Debug.TelemetryManager;
			if (telemetryManager == null)
			{
				return;
			}
			telemetryManager.EndTelemetryScopeBaseLevelInternal();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005B61 File Offset: 0x00003D61
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void WriteDebugLineOnScreen(string message)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.WriteDebugLineOnScreen(message);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005B73 File Offset: 0x00003D73
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugLine(Vec3 position, Vec3 direction, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.RenderDebugLine(position, direction, color, depthCheck, time);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005B8C File Offset: 0x00003D8C
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugLineWithThickness(Vec3 position, Vec3 direction, uint color = 4294967295U, bool depthCheck = false, float time = 0f, int thickness = 0)
		{
			Vec3 vec = direction.AsVec2.RightVec().ToVec3(0f);
			vec.Normalize();
			vec *= 0.005f;
			for (int i = 0; i < thickness; i++)
			{
				IDebugManager debugManager = Debug.DebugManager;
				if (debugManager != null)
				{
					debugManager.RenderDebugLine(position + vec * (float)i, direction, color, depthCheck, time);
				}
				IDebugManager debugManager2 = Debug.DebugManager;
				if (debugManager2 != null)
				{
					debugManager2.RenderDebugLine(position + vec * (float)(-(float)i), direction, color, depthCheck, time);
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005C1E File Offset: 0x00003E1E
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugSphere(Vec3 position, float radius, uint color = 4294967295U, bool depthCheck = false, float time = 0f)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.RenderDebugSphere(position, radius, color, depthCheck, time);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005C35 File Offset: 0x00003E35
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugFrame(MatrixFrame frame, float lineLength, float time = 0f)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.RenderDebugFrame(frame, lineLength, time);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005C49 File Offset: 0x00003E49
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugText(float screenX, float screenY, string text, uint color = 4294967295U, float time = 0f)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.RenderDebugText(screenX, screenY, text, color, time);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005C60 File Offset: 0x00003E60
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color = 4294967295U)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.RenderDebugRectWithColor(left, bottom, right, top, color);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005C77 File Offset: 0x00003E77
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void RenderDebugText3D(Vec3 position, string text, uint color = 4294967295U, int screenPosOffsetX = 0, int screenPosOffsetY = 0, float time = 0f)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.RenderDebugText3D(position, text, color, screenPosOffsetX, screenPosOffsetY, time);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005C90 File Offset: 0x00003E90
		public static Vec3 GetDebugVector()
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return Vec3.Zero;
			}
			return debugManager.GetDebugVector();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005CA6 File Offset: 0x00003EA6
		public static void SetDebugVector(Vec3 value)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.SetDebugVector(value);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005CB8 File Offset: 0x00003EB8
		public static void SetTestModeEnabled(bool testModeEnabled)
		{
			IDebugManager debugManager = Debug.DebugManager;
			if (debugManager == null)
			{
				return;
			}
			debugManager.SetTestModeEnabled(testModeEnabled);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005CCA File Offset: 0x00003ECA
		public static void AbortGame()
		{
			if (Debug.DebugManager != null)
			{
				Debug.DebugManager.AbortGame();
			}
		}

		// Token: 0x020000CE RID: 206
		public enum DebugColor
		{
			// Token: 0x04000266 RID: 614
			DarkRed,
			// Token: 0x04000267 RID: 615
			DarkGreen,
			// Token: 0x04000268 RID: 616
			DarkBlue,
			// Token: 0x04000269 RID: 617
			Red,
			// Token: 0x0400026A RID: 618
			Green,
			// Token: 0x0400026B RID: 619
			Blue,
			// Token: 0x0400026C RID: 620
			DarkCyan,
			// Token: 0x0400026D RID: 621
			Cyan,
			// Token: 0x0400026E RID: 622
			DarkYellow,
			// Token: 0x0400026F RID: 623
			Yellow,
			// Token: 0x04000270 RID: 624
			Purple,
			// Token: 0x04000271 RID: 625
			Magenta,
			// Token: 0x04000272 RID: 626
			White,
			// Token: 0x04000273 RID: 627
			BrightWhite
		}

		// Token: 0x020000CF RID: 207
		public enum DebugUserFilter : ulong
		{
			// Token: 0x04000275 RID: 629
			None,
			// Token: 0x04000276 RID: 630
			Unused0,
			// Token: 0x04000277 RID: 631
			Unused1,
			// Token: 0x04000278 RID: 632
			Koray = 4UL,
			// Token: 0x04000279 RID: 633
			Armagan = 8UL,
			// Token: 0x0400027A RID: 634
			Intern = 16UL,
			// Token: 0x0400027B RID: 635
			Mustafa = 32UL,
			// Token: 0x0400027C RID: 636
			Oguzhan = 64UL,
			// Token: 0x0400027D RID: 637
			Omer = 128UL,
			// Token: 0x0400027E RID: 638
			Ates = 256UL,
			// Token: 0x0400027F RID: 639
			Unused3 = 512UL,
			// Token: 0x04000280 RID: 640
			Basak = 1024UL,
			// Token: 0x04000281 RID: 641
			Can = 2048UL,
			// Token: 0x04000282 RID: 642
			Unused4 = 4096UL,
			// Token: 0x04000283 RID: 643
			Cem = 8192UL,
			// Token: 0x04000284 RID: 644
			Unused5 = 16384UL,
			// Token: 0x04000285 RID: 645
			Unused6 = 32768UL,
			// Token: 0x04000286 RID: 646
			Emircan = 65536UL,
			// Token: 0x04000287 RID: 647
			Unused7 = 131072UL,
			// Token: 0x04000288 RID: 648
			All = 4294967295UL,
			// Token: 0x04000289 RID: 649
			Default = 0UL,
			// Token: 0x0400028A RID: 650
			DamageDebug = 72UL
		}

		// Token: 0x020000D0 RID: 208
		public enum DebugSystemFilter : ulong
		{
			// Token: 0x0400028C RID: 652
			None,
			// Token: 0x0400028D RID: 653
			Graphics = 4294967296UL,
			// Token: 0x0400028E RID: 654
			ArtificialIntelligence = 8589934592UL,
			// Token: 0x0400028F RID: 655
			MultiPlayer = 17179869184UL,
			// Token: 0x04000290 RID: 656
			IO = 34359738368UL,
			// Token: 0x04000291 RID: 657
			Network = 68719476736UL,
			// Token: 0x04000292 RID: 658
			CampaignEvents = 137438953472UL,
			// Token: 0x04000293 RID: 659
			MemoryManager = 274877906944UL,
			// Token: 0x04000294 RID: 660
			TCP = 549755813888UL,
			// Token: 0x04000295 RID: 661
			FileManager = 1099511627776UL,
			// Token: 0x04000296 RID: 662
			NaturalInteractionDevice = 2199023255552UL,
			// Token: 0x04000297 RID: 663
			UDP = 4398046511104UL,
			// Token: 0x04000298 RID: 664
			ResourceManager = 8796093022208UL,
			// Token: 0x04000299 RID: 665
			Mono = 17592186044416UL,
			// Token: 0x0400029A RID: 666
			ONO = 35184372088832UL,
			// Token: 0x0400029B RID: 667
			Old = 70368744177664UL,
			// Token: 0x0400029C RID: 668
			Sound = 281474976710656UL,
			// Token: 0x0400029D RID: 669
			CombatLog = 562949953421312UL,
			// Token: 0x0400029E RID: 670
			Notifications = 1125899906842624UL,
			// Token: 0x0400029F RID: 671
			Quest = 2251799813685248UL,
			// Token: 0x040002A0 RID: 672
			Dialog = 4503599627370496UL,
			// Token: 0x040002A1 RID: 673
			Steam = 9007199254740992UL,
			// Token: 0x040002A2 RID: 674
			All = 18446744069414584320UL,
			// Token: 0x040002A3 RID: 675
			DefaultMask = 18446744069414584320UL
		}
	}
}
