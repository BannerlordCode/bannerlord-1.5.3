using System;
using TaleWorlds.Library;

namespace TaleWorlds.ScreenSystem
{
	// Token: 0x02000005 RID: 5
	public interface IScreenManagerEngineConnection
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000019 RID: 25
		float RealScreenResolutionWidth { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26
		float RealScreenResolutionHeight { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27
		float AspectRatio { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28
		Vec2 DesktopResolution { get; }

		// Token: 0x0600001D RID: 29
		void ActivateMouseCursor(CursorType mouseId);

		// Token: 0x0600001E RID: 30
		void SetMouseVisible(bool value);

		// Token: 0x0600001F RID: 31
		bool GetMouseVisible();

		// Token: 0x06000020 RID: 32
		bool GetIsEnterButtonRDown();

		// Token: 0x06000021 RID: 33
		void BeginDebugPanel(string panelTitle);

		// Token: 0x06000022 RID: 34
		void EndDebugPanel();

		// Token: 0x06000023 RID: 35
		void DrawDebugText(string text);

		// Token: 0x06000024 RID: 36
		bool DrawDebugTreeNode(string text);

		// Token: 0x06000025 RID: 37
		void PopDebugTreeNode();
	}
}
