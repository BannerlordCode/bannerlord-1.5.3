using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200002B RID: 43
	public class DiamondDebugManager : IDebugManager
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00005CDD File Offset: 0x00003EDD
		public DiamondDebugManager(ParameterContainer parameters)
		{
			this._parameters = parameters;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005CEC File Offset: 0x00003EEC
		public DiamondDebugManager()
		{
			this._parameters = null;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005CFB File Offset: 0x00003EFB
		void IDebugManager.SetCrashReportCustomString(string customString)
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005CFD File Offset: 0x00003EFD
		void IDebugManager.SetCrashReportCustomStack(string customStack)
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005CFF File Offset: 0x00003EFF
		void IDebugManager.ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005D01 File Offset: 0x00003F01
		void IDebugManager.ShowError(string message)
		{
			this.PrintMessage(message, DiamondDebugManager.DiamondDebugCategory.Error);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005D0B File Offset: 0x00003F0B
		void IDebugManager.ShowWarning(string message)
		{
			this.PrintMessage(message, DiamondDebugManager.DiamondDebugCategory.Warning);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005D15 File Offset: 0x00003F15
		void IDebugManager.Assert(bool condition, string message, string callerFile, string callerMethod, int callerLine)
		{
			if (!condition)
			{
				throw new Exception(string.Format("Assertion failed: {0} in {1}, line:{2}", message, callerFile, callerLine));
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005D33 File Offset: 0x00003F33
		void IDebugManager.SilentAssert(bool condition, string message, bool getDump, string callerFile, string callerMethod, int callerLine)
		{
			if (!condition)
			{
				this.PrintMessage(string.Format("Assertion failed: {0} in {1}, line:{2}", message, callerMethod, callerLine), DiamondDebugManager.DiamondDebugCategory.Warning);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005D53 File Offset: 0x00003F53
		void IDebugManager.Print(string message, int logLevel, Debug.DebugColor color, ulong debugFilter)
		{
			this.PrintMessage(message, DiamondDebugManager.DiamondDebugCategory.General);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005D5D File Offset: 0x00003F5D
		void IDebugManager.PrintError(string error, string stackTrace, ulong debugFilter)
		{
			this.PrintMessage(error + stackTrace, DiamondDebugManager.DiamondDebugCategory.Error);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005D6D File Offset: 0x00003F6D
		void IDebugManager.PrintWarning(string warning, ulong debugFilter)
		{
			this.PrintMessage(warning, DiamondDebugManager.DiamondDebugCategory.Warning);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005D77 File Offset: 0x00003F77
		void IDebugManager.DisplayDebugMessage(string message)
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005D79 File Offset: 0x00003F79
		void IDebugManager.WatchVariable(string name, object value)
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005D7B File Offset: 0x00003F7B
		void IDebugManager.WriteDebugLineOnScreen(string message)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005D7D File Offset: 0x00003F7D
		void IDebugManager.RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005D7F File Offset: 0x00003F7F
		void IDebugManager.RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005D81 File Offset: 0x00003F81
		void IDebugManager.RenderDebugFrame(MatrixFrame frame, float lineLength, float time)
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005D83 File Offset: 0x00003F83
		void IDebugManager.RenderDebugText(float screenX, float screenY, string text, uint color, float time)
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005D85 File Offset: 0x00003F85
		void IDebugManager.RenderDebugText3D(Vec3 position, string text, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005D87 File Offset: 0x00003F87
		void IDebugManager.RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005D89 File Offset: 0x00003F89
		Vec3 IDebugManager.GetDebugVector()
		{
			return Vec3.Zero;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005D90 File Offset: 0x00003F90
		void IDebugManager.SetDebugVector(Vec3 value)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005D92 File Offset: 0x00003F92
		void IDebugManager.SetTestModeEnabled(bool testModeEnabled)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005D94 File Offset: 0x00003F94
		void IDebugManager.AbortGame()
		{
			Environment.Exit(-5);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005D9D File Offset: 0x00003F9D
		void IDebugManager.DoDelayedexit(int returnCode)
		{
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005DA0 File Offset: 0x00003FA0
		public int GetLogLevel()
		{
			int num;
			if (this._parameters != null && this._parameters.TryGetParameterAsInt("LogLevel", out num))
			{
				return num;
			}
			return 1;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005DCC File Offset: 0x00003FCC
		protected void PrintMessage(string message, DiamondDebugManager.DiamondDebugCategory debugCategory)
		{
			if (this.GetLogLevel() <= (int)debugCategory)
			{
				Console.Out.Flush();
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = DiamondDebugManager._colors[debugCategory];
				Console.Write(message);
				Console.ResetColor();
				Console.WriteLine();
				Console.Out.Flush();
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005E1C File Offset: 0x0000401C
		void IDebugManager.ReportMemoryBookmark(string message)
		{
		}

		// Token: 0x040000A3 RID: 163
		private static Dictionary<DiamondDebugManager.DiamondDebugCategory, ConsoleColor> _colors = new Dictionary<DiamondDebugManager.DiamondDebugCategory, ConsoleColor>
		{
			{
				DiamondDebugManager.DiamondDebugCategory.General,
				ConsoleColor.Green
			},
			{
				DiamondDebugManager.DiamondDebugCategory.Warning,
				ConsoleColor.Yellow
			},
			{
				DiamondDebugManager.DiamondDebugCategory.Error,
				ConsoleColor.Red
			}
		};

		// Token: 0x040000A4 RID: 164
		private ParameterContainer _parameters;

		// Token: 0x020000D1 RID: 209
		public enum DiamondDebugCategory
		{
			// Token: 0x040002A5 RID: 677
			General,
			// Token: 0x040002A6 RID: 678
			Warning,
			// Token: 0x040002A7 RID: 679
			Error
		}
	}
}
