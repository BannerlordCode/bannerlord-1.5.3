using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000031 RID: 49
	[ApplicationInterfaceBase]
	internal interface IView
	{
		// Token: 0x06000539 RID: 1337
		[EngineMethod("set_render_option", false, null, false)]
		void SetRenderOption(UIntPtr ptr, int optionEnum, bool value);

		// Token: 0x0600053A RID: 1338
		[EngineMethod("set_render_order", false, null, false)]
		void SetRenderOrder(UIntPtr ptr, int value);

		// Token: 0x0600053B RID: 1339
		[EngineMethod("set_render_target", false, null, false)]
		void SetRenderTarget(UIntPtr ptr, UIntPtr texture_ptr);

		// Token: 0x0600053C RID: 1340
		[EngineMethod("set_depth_target", false, null, false)]
		void SetDepthTarget(UIntPtr ptr, UIntPtr texture_ptr);

		// Token: 0x0600053D RID: 1341
		[EngineMethod("set_scale", false, null, false)]
		void SetScale(UIntPtr ptr, float x, float y);

		// Token: 0x0600053E RID: 1342
		[EngineMethod("set_offset", false, null, false)]
		void SetOffset(UIntPtr ptr, float x, float y);

		// Token: 0x0600053F RID: 1343
		[EngineMethod("set_debug_render_functionality", false, null, false)]
		void SetDebugRenderFunctionality(UIntPtr ptr, bool value);

		// Token: 0x06000540 RID: 1344
		[EngineMethod("set_clear_color", false, null, false)]
		void SetClearColor(UIntPtr ptr, uint rgba);

		// Token: 0x06000541 RID: 1345
		[EngineMethod("set_enable", false, null, false)]
		void SetEnable(UIntPtr ptr, bool value);

		// Token: 0x06000542 RID: 1346
		[EngineMethod("set_render_on_demand", false, null, false)]
		void SetRenderOnDemand(UIntPtr ptr, bool value);

		// Token: 0x06000543 RID: 1347
		[EngineMethod("set_auto_depth_creation", false, null, false)]
		void SetAutoDepthTargetCreation(UIntPtr ptr, bool value);

		// Token: 0x06000544 RID: 1348
		[EngineMethod("set_save_final_result_to_disk", false, null, false)]
		void SetSaveFinalResultToDisk(UIntPtr ptr, bool value);

		// Token: 0x06000545 RID: 1349
		[EngineMethod("set_file_name_to_save_result", false, null, false)]
		void SetFileNameToSaveResult(UIntPtr ptr, string name);

		// Token: 0x06000546 RID: 1350
		[EngineMethod("set_file_type_to_save", false, null, false)]
		void SetFileTypeToSave(UIntPtr ptr, int type);

		// Token: 0x06000547 RID: 1351
		[EngineMethod("set_file_path_to_save_result", false, null, false)]
		void SetFilePathToSaveResult(UIntPtr ptr, string name);
	}
}
