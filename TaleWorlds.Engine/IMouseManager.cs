using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000038 RID: 56
	[ApplicationInterfaceBase]
	internal interface IMouseManager
	{
		// Token: 0x06000587 RID: 1415
		[EngineMethod("activate_mouse_cursor", false, null, false)]
		void ActivateMouseCursor(int id);

		// Token: 0x06000588 RID: 1416
		[EngineMethod("set_mouse_cursor", false, null, false)]
		void SetMouseCursor(int id, string mousePath);

		// Token: 0x06000589 RID: 1417
		[EngineMethod("show_cursor", false, null, false)]
		void ShowCursor(bool show);

		// Token: 0x0600058A RID: 1418
		[EngineMethod("lock_cursor_at_current_pos", false, null, false)]
		void LockCursorAtCurrentPosition(bool lockCursor);

		// Token: 0x0600058B RID: 1419
		[EngineMethod("lock_cursor_at_position", false, null, false)]
		void LockCursorAtPosition(float x, float y);

		// Token: 0x0600058C RID: 1420
		[EngineMethod("unlock_cursor", false, null, false)]
		void UnlockCursor();
	}
}
