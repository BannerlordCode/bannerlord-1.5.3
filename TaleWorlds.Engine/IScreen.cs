using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003E RID: 62
	[ApplicationInterfaceBase]
	internal interface IScreen
	{
		// Token: 0x06000651 RID: 1617
		[EngineMethod("get_real_screen_resolution_width", false, null, false)]
		float GetRealScreenResolutionWidth();

		// Token: 0x06000652 RID: 1618
		[EngineMethod("get_real_screen_resolution_height", false, null, false)]
		float GetRealScreenResolutionHeight();

		// Token: 0x06000653 RID: 1619
		[EngineMethod("get_desktop_width", false, null, false)]
		float GetDesktopWidth();

		// Token: 0x06000654 RID: 1620
		[EngineMethod("get_desktop_height", false, null, false)]
		float GetDesktopHeight();

		// Token: 0x06000655 RID: 1621
		[EngineMethod("get_aspect_ratio", false, null, false)]
		float GetAspectRatio();

		// Token: 0x06000656 RID: 1622
		[EngineMethod("get_mouse_visible", false, null, false)]
		bool GetMouseVisible();

		// Token: 0x06000657 RID: 1623
		[EngineMethod("set_mouse_visible", false, null, false)]
		void SetMouseVisible(bool value);

		// Token: 0x06000658 RID: 1624
		[EngineMethod("get_usable_area_percentages", false, null, false)]
		Vec2 GetUsableAreaPercentages();

		// Token: 0x06000659 RID: 1625
		[EngineMethod("is_enter_button_cross", false, null, false)]
		bool IsEnterButtonCross();
	}
}
