using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BD RID: 445
	[ScriptingInterfaceBase]
	internal interface IMBWindowManager
	{
		// Token: 0x06001935 RID: 6453
		[EngineMethod("erase_message_lines", false, null, false)]
		void EraseMessageLines();

		// Token: 0x06001936 RID: 6454
		[EngineMethod("world_to_screen", false, null, true)]
		float WorldToScreen(UIntPtr cameraPointer, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);

		// Token: 0x06001937 RID: 6455
		[EngineMethod("world_to_screen_with_fixed_z", false, null, false)]
		float WorldToScreenWithFixedZ(UIntPtr cameraPointer, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);

		// Token: 0x06001938 RID: 6456
		[EngineMethod("dont_change_cursor_pos", false, null, false)]
		void DontChangeCursorPos();

		// Token: 0x06001939 RID: 6457
		[EngineMethod("pre_display", false, null, false)]
		void PreDisplay();

		// Token: 0x0600193A RID: 6458
		[EngineMethod("screen_to_world", false, null, false)]
		void ScreenToWorld(UIntPtr pointer, float screenX, float screenY, float z, ref Vec3 worldSpacePosition);

		// Token: 0x0600193B RID: 6459
		[EngineMethod("get_screen_resolution", false, null, false)]
		Vec2 GetScreenResolution();
	}
}
