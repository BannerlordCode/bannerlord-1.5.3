using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001CA RID: 458
	public class MBDebugManager : IDebugManager
	{
		// Token: 0x06001B99 RID: 7065 RVA: 0x000605E8 File Offset: 0x0005E7E8
		void IDebugManager.SetCrashReportCustomString(string customString)
		{
			Utilities.SetCrashReportCustomString(customString);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000605F0 File Offset: 0x0005E7F0
		void IDebugManager.SetCrashReportCustomStack(string customStack)
		{
			Utilities.SetCrashReportCustomStack(customStack);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000605F8 File Offset: 0x0005E7F8
		void IDebugManager.ShowWarning(string message)
		{
			MBDebug.ShowWarning(message);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00060600 File Offset: 0x0005E800
		void IDebugManager.ShowError(string message)
		{
			MBDebug.ShowError(message);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x00060608 File Offset: 0x0005E808
		void IDebugManager.ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
			MBDebug.ShowMessageBox(lpText, lpCaption, uType);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00060613 File Offset: 0x0005E813
		void IDebugManager.Assert(bool condition, string message, string callerFile, string callerMethod, int callerLine)
		{
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x00060615 File Offset: 0x0005E815
		void IDebugManager.SilentAssert(bool condition, string message, bool getDump, string callerFile, string callerMethod, int callerLine)
		{
			MBDebug.SilentAssert(condition, message, getDump, callerFile, callerMethod, callerLine);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00060625 File Offset: 0x0005E825
		void IDebugManager.Print(string message, int logLevel, Debug.DebugColor color, ulong debugFilter)
		{
			MBDebug.Print(message, logLevel, color, debugFilter);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00060631 File Offset: 0x0005E831
		void IDebugManager.PrintError(string error, string stackTrace, ulong debugFilter)
		{
			MBDebug.Print(error, 0, Debug.DebugColor.White, debugFilter);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0006063D File Offset: 0x0005E83D
		void IDebugManager.PrintWarning(string warning, ulong debugFilter)
		{
			MBDebug.Print(warning, 0, Debug.DebugColor.White, debugFilter);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00060649 File Offset: 0x0005E849
		void IDebugManager.DisplayDebugMessage(string message)
		{
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0006064B File Offset: 0x0005E84B
		void IDebugManager.WatchVariable(string name, object value)
		{
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0006064D File Offset: 0x0005E84D
		void IDebugManager.WriteDebugLineOnScreen(string message)
		{
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0006064F File Offset: 0x0005E84F
		void IDebugManager.RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x00060651 File Offset: 0x0005E851
		void IDebugManager.RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x00060653 File Offset: 0x0005E853
		void IDebugManager.RenderDebugFrame(MatrixFrame frame, float lineLength, float time)
		{
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x00060655 File Offset: 0x0005E855
		void IDebugManager.RenderDebugText(float screenX, float screenY, string text, uint color, float time)
		{
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x00060657 File Offset: 0x0005E857
		void IDebugManager.RenderDebugText3D(Vec3 position, string text, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x00060659 File Offset: 0x0005E859
		void IDebugManager.RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0006065B File Offset: 0x0005E85B
		Vec3 IDebugManager.GetDebugVector()
		{
			return MBDebug.DebugVector;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00060662 File Offset: 0x0005E862
		void IDebugManager.SetDebugVector(Vec3 value)
		{
			MBDebug.DebugVector = value;
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0006066A File Offset: 0x0005E86A
		void IDebugManager.SetTestModeEnabled(bool testModeEnabled)
		{
			MBDebug.TestModeEnabled = testModeEnabled;
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x00060672 File Offset: 0x0005E872
		void IDebugManager.AbortGame()
		{
			MBDebug.AbortGame(5);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0006067A File Offset: 0x0005E87A
		void IDebugManager.DoDelayedexit(int returnCode)
		{
			Utilities.DoDelayedexit(returnCode);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00060682 File Offset: 0x0005E882
		void IDebugManager.ReportMemoryBookmark(string message)
		{
		}
	}
}
