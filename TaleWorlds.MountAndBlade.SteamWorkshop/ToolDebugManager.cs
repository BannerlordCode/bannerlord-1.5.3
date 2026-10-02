using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.SteamWorkshop
{
	// Token: 0x02000005 RID: 5
	internal class ToolDebugManager : IDebugManager
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.SetCrashReportCustomString(string customString)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.SetCrashReportCustomStack(string customStack)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.ShowWarning(string message)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.ShowError(string message)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002480 File Offset: 0x00000680
		void IDebugManager.Assert(bool condition, string message, string callerFile, string callerMethod, int callerLine)
		{
			if (!condition)
			{
				Program.Log("assert:" + message);
				Program.ExitProgram(-10);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.SilentAssert(bool condition, string message, bool getDump, string callerFile, string callerMethod, int callerLine)
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000249C File Offset: 0x0000069C
		void IDebugManager.Print(string message, int logLevel, Debug.DebugColor color, ulong debugFilter)
		{
			Program.Log(message);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.PrintError(string error, string stackTrace, ulong debugFilter)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.PrintWarning(string warning, ulong debugFilter)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.DisplayDebugMessage(string message)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.WatchVariable(string name, object value)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.WriteDebugLineOnScreen(string message)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.RenderDebugFrame(MatrixFrame frame, float lineLength, float time)
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.RenderDebugText(float screenX, float screenY, string text, uint color, float time)
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.RenderDebugText3D(Vec3 position, string text, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000024A4 File Offset: 0x000006A4
		Vec3 IDebugManager.GetDebugVector()
		{
			return Vec3.Zero;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.SetDebugVector(Vec3 value)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.SetTestModeEnabled(bool testModeEnabled)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.AbortGame()
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.DoDelayedexit(int returnCode)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002048 File Offset: 0x00000248
		void IDebugManager.ReportMemoryBookmark(string message)
		{
		}
	}
}
