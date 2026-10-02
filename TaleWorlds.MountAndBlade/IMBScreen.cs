using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B7 RID: 439
	[ScriptingInterfaceBase]
	internal interface IMBScreen
	{
		// Token: 0x06001919 RID: 6425
		[EngineMethod("on_exit_button_click", false, null, false)]
		void OnExitButtonClick();

		// Token: 0x0600191A RID: 6426
		[EngineMethod("on_edit_mode_enter_press", false, null, false)]
		void OnEditModeEnterPress();

		// Token: 0x0600191B RID: 6427
		[EngineMethod("on_edit_mode_enter_release", false, null, false)]
		void OnEditModeEnterRelease();
	}
}
