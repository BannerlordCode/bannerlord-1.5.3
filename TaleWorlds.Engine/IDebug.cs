using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000036 RID: 54
	[ApplicationInterfaceBase]
	internal interface IDebug
	{
		// Token: 0x0600055D RID: 1373
		[EngineMethod("write_debug_line_on_screen", false, null, false)]
		void WriteDebugLineOnScreen(string line);

		// Token: 0x0600055E RID: 1374
		[EngineMethod("abort_game", false, null, false)]
		void AbortGame(int ExitCode);

		// Token: 0x0600055F RID: 1375
		[EngineMethod("assert_memory_usage", false, null, false)]
		void AssertMemoryUsage(int memoryMB);

		// Token: 0x06000560 RID: 1376
		[EngineMethod("write_line", false, null, false)]
		void WriteLine(int logLevel, string line, int color, ulong filter);

		// Token: 0x06000561 RID: 1377
		[EngineMethod("render_debug_direction_arrow", false, null, false)]
		void RenderDebugDirectionArrow(Vec3 position, Vec3 direction, uint color, bool depthCheck);

		// Token: 0x06000562 RID: 1378
		[EngineMethod("render_debug_line", false, null, false)]
		void RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time);

		// Token: 0x06000563 RID: 1379
		[EngineMethod("render_debug_sphere", false, null, false)]
		void RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time);

		// Token: 0x06000564 RID: 1380
		[EngineMethod("render_debug_capsule", false, null, false)]
		void RenderDebugCapsule(Vec3 p0, Vec3 p1, float radius, uint color, bool depthCheck, float time);

		// Token: 0x06000565 RID: 1381
		[EngineMethod("render_debug_frame", false, null, false)]
		void RenderDebugFrame(ref MatrixFrame frame, float lineLength, float time);

		// Token: 0x06000566 RID: 1382
		[EngineMethod("render_debug_text3d", false, null, false)]
		void RenderDebugText3d(Vec3 worldPosition, string str, uint color, int screenPosOffsetX, int screenPosOffsetY, float time);

		// Token: 0x06000567 RID: 1383
		[EngineMethod("render_debug_text", false, null, false)]
		void RenderDebugText(float screenX, float screenY, string str, uint color, float time);

		// Token: 0x06000568 RID: 1384
		[EngineMethod("render_debug_rect", false, null, false)]
		void RenderDebugRect(float left, float bottom, float right, float top);

		// Token: 0x06000569 RID: 1385
		[EngineMethod("render_debug_rect_with_color", false, null, false)]
		void RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color);

		// Token: 0x0600056A RID: 1386
		[EngineMethod("clear_all_debug_render_objects", false, null, false)]
		void ClearAllDebugRenderObjects();

		// Token: 0x0600056B RID: 1387
		[EngineMethod("get_debug_vector", false, null, false)]
		Vec3 GetDebugVector();

		// Token: 0x0600056C RID: 1388
		[EngineMethod("set_debug_vector", false, null, false)]
		void SetDebugVector(Vec3 debugVector);

		// Token: 0x0600056D RID: 1389
		[EngineMethod("render_debug_box_object", false, null, false)]
		void RenderDebugBoxObject(Vec3 min, Vec3 max, uint color, bool depthCheck, float time);

		// Token: 0x0600056E RID: 1390
		[EngineMethod("render_debug_box_object_with_frame", false, null, false)]
		void RenderDebugBoxObjectWithFrame(Vec3 min, Vec3 max, ref MatrixFrame frame, uint color, bool depthCheck, float time);

		// Token: 0x0600056F RID: 1391
		[EngineMethod("post_warning_line", false, null, false)]
		void PostWarningLine(string line);

		// Token: 0x06000570 RID: 1392
		[EngineMethod("is_error_report_mode_active", false, null, false)]
		bool IsErrorReportModeActive();

		// Token: 0x06000571 RID: 1393
		[EngineMethod("is_error_report_mode_pause_mission", false, null, false)]
		bool IsErrorReportModePauseMission();

		// Token: 0x06000572 RID: 1394
		[EngineMethod("set_error_report_scene", false, null, false)]
		void SetErrorReportScene(UIntPtr scenePointer);

		// Token: 0x06000573 RID: 1395
		[EngineMethod("set_dump_generation_disabled", false, null, false)]
		void SetDumpGenerationDisabled(bool Disabled);

		// Token: 0x06000574 RID: 1396
		[EngineMethod("message_box", false, null, false)]
		int MessageBox(string lpText, string lpCaption, uint uType);

		// Token: 0x06000575 RID: 1397
		[EngineMethod("get_show_debug_info", false, null, false)]
		int GetShowDebugInfo();

		// Token: 0x06000576 RID: 1398
		[EngineMethod("set_show_debug_info", false, null, false)]
		void SetShowDebugInfo(int value);

		// Token: 0x06000577 RID: 1399
		[EngineMethod("error", false, null, false)]
		bool Error(string MessageString);

		// Token: 0x06000578 RID: 1400
		[EngineMethod("warning", false, null, false)]
		bool Warning(string MessageString);

		// Token: 0x06000579 RID: 1401
		[EngineMethod("content_warning", false, null, false)]
		bool ContentWarning(string MessageString);

		// Token: 0x0600057A RID: 1402
		[EngineMethod("failed_assert", false, null, false)]
		bool FailedAssert(string messageString, string callerFile, string callerMethod, int callerLine);

		// Token: 0x0600057B RID: 1403
		[EngineMethod("silent_assert", false, null, false)]
		bool SilentAssert(string messageString, string callerFile, string callerMethod, int callerLine, bool getDump);

		// Token: 0x0600057C RID: 1404
		[EngineMethod("is_test_mode", false, null, false)]
		bool IsTestMode();

		// Token: 0x0600057D RID: 1405
		[EngineMethod("echo_command_window", false, null, false)]
		void EchoCommandWindow(string content);
	}
}
