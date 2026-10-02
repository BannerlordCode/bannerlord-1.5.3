using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C3 RID: 451
	[ScriptingInterfaceBase]
	internal interface IMBBannerlordTableauManager
	{
		// Token: 0x06001974 RID: 6516
		[EngineMethod("request_character_tableau_render", false, null, false)]
		void RequestCharacterTableauRender(int characterCodeId, string path, UIntPtr poseEntity, UIntPtr cameraObject, int tableauType);

		// Token: 0x06001975 RID: 6517
		[EngineMethod("initialize_character_tableau_render_system", false, null, false)]
		void InitializeCharacterTableauRenderSystem();

		// Token: 0x06001976 RID: 6518
		[EngineMethod("get_number_of_pending_tableau_requests", false, null, false)]
		int GetNumberOfPendingTableauRequests();
	}
}
