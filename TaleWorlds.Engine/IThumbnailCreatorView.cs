using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000035 RID: 53
	[ApplicationInterfaceBase]
	internal interface IThumbnailCreatorView
	{
		// Token: 0x06000554 RID: 1364
		[EngineMethod("create_thumbnail_creator_view", false, null, false)]
		ThumbnailCreatorView CreateThumbnailCreatorView();

		// Token: 0x06000555 RID: 1365
		[EngineMethod("register_scene", false, null, false)]
		void RegisterScene(UIntPtr pointer, UIntPtr scene_ptr, bool use_postfx);

		// Token: 0x06000556 RID: 1366
		[EngineMethod("clear_requests", false, null, false)]
		void ClearRequests(UIntPtr pointer);

		// Token: 0x06000557 RID: 1367
		[EngineMethod("cancel_request", false, null, false)]
		void CancelRequest(UIntPtr pointer, string render_id);

		// Token: 0x06000558 RID: 1368
		[EngineMethod("register_cached_entity", false, null, false)]
		void RegisterCachedEntity(UIntPtr pointer, UIntPtr scene, UIntPtr entity_ptr, string cacheId);

		// Token: 0x06000559 RID: 1369
		[EngineMethod("unregister_cached_entity", false, null, false)]
		void UnregisterCachedEntity(UIntPtr pointer, string cacheId);

		// Token: 0x0600055A RID: 1370
		[EngineMethod("register_render_request", false, null, false)]
		void RegisterRenderRequest(UIntPtr pointer, ref ThumbnailRenderRequest request);

		// Token: 0x0600055B RID: 1371
		[EngineMethod("get_number_of_pending_requests", false, null, false)]
		int GetNumberOfPendingRequests(UIntPtr pointer);

		// Token: 0x0600055C RID: 1372
		[EngineMethod("is_memory_cleared", false, null, false)]
		bool IsMemoryCleared(UIntPtr pointer);
	}
}
