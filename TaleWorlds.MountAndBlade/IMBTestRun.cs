using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A9 RID: 425
	[ScriptingInterfaceBase]
	internal interface IMBTestRun
	{
		// Token: 0x060016C9 RID: 5833
		[EngineMethod("auto_continue", false, null, false)]
		int AutoContinue(int type);

		// Token: 0x060016CA RID: 5834
		[EngineMethod("get_fps", false, null, false)]
		int GetFPS();

		// Token: 0x060016CB RID: 5835
		[EngineMethod("enter_edit_mode", false, null, false)]
		bool EnterEditMode();

		// Token: 0x060016CC RID: 5836
		[EngineMethod("open_scene", false, null, false)]
		bool OpenScene(string sceneName);

		// Token: 0x060016CD RID: 5837
		[EngineMethod("close_scene", false, null, false)]
		bool CloseScene();

		// Token: 0x060016CE RID: 5838
		[EngineMethod("save_scene", false, null, false)]
		bool SaveScene();

		// Token: 0x060016CF RID: 5839
		[EngineMethod("open_default_scene", false, null, false)]
		bool OpenDefaultScene();

		// Token: 0x060016D0 RID: 5840
		[EngineMethod("leave_edit_mode", false, null, false)]
		bool LeaveEditMode();

		// Token: 0x060016D1 RID: 5841
		[EngineMethod("new_scene", false, null, false)]
		bool NewScene();

		// Token: 0x060016D2 RID: 5842
		[EngineMethod("start_mission", false, null, false)]
		void StartMission();
	}
}
