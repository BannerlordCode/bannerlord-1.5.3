using System;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006E RID: 110
	public static class MouseManager
	{
		// Token: 0x06000A4C RID: 2636 RVA: 0x0000A66E File Offset: 0x0000886E
		public static void ActivateMouseCursor(CursorType mouseId)
		{
			EngineApplicationInterface.IMouseManager.ActivateMouseCursor((int)mouseId);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0000A67B File Offset: 0x0000887B
		public static void SetMouseCursor(CursorType mouseId, string mousePath)
		{
			EngineApplicationInterface.IMouseManager.SetMouseCursor((int)mouseId, mousePath);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0000A689 File Offset: 0x00008889
		public static void ShowCursor(bool show)
		{
			EngineApplicationInterface.IMouseManager.ShowCursor(show);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0000A696 File Offset: 0x00008896
		public static void LockCursorAtCurrentPosition(bool lockCursor)
		{
			EngineApplicationInterface.IMouseManager.LockCursorAtCurrentPosition(lockCursor);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0000A6A3 File Offset: 0x000088A3
		public static void LockCursorAtPosition(float x, float y)
		{
			EngineApplicationInterface.IMouseManager.LockCursorAtPosition(x, y);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0000A6B1 File Offset: 0x000088B1
		public static void UnlockCursor()
		{
			EngineApplicationInterface.IMouseManager.UnlockCursor();
		}
	}
}
