using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000032 RID: 50
	[ApplicationInterfaceBase]
	internal interface ITableauView
	{
		// Token: 0x06000548 RID: 1352
		[EngineMethod("create_tableau_view", false, null, false)]
		TableauView CreateTableauView(string viewName);

		// Token: 0x06000549 RID: 1353
		[EngineMethod("set_sort_meshes", false, null, false)]
		void SetSortingEnabled(UIntPtr pointer, bool value);

		// Token: 0x0600054A RID: 1354
		[EngineMethod("set_continous_rendering", false, null, false)]
		void SetContinousRendering(UIntPtr pointer, bool value);

		// Token: 0x0600054B RID: 1355
		[EngineMethod("set_do_not_render_this_frame", false, null, false)]
		void SetDoNotRenderThisFrame(UIntPtr pointer, bool value);

		// Token: 0x0600054C RID: 1356
		[EngineMethod("set_delete_after_rendering", false, null, false)]
		void SetDeleteAfterRendering(UIntPtr pointer, bool value);
	}
}
