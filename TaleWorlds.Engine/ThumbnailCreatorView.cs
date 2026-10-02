using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000095 RID: 149
	[EngineClass("rglThumbnail_creator_view")]
	public sealed class ThumbnailCreatorView : View
	{
		// Token: 0x06000D3C RID: 3388 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		internal ThumbnailCreatorView(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0000EEA9 File Offset: 0x0000D0A9
		[EngineCallback(null, false)]
		internal static void OnThumbnailRenderComplete(string renderId, Texture renderTarget)
		{
			ThumbnailCreatorView.renderCallback(renderId, renderTarget);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0000EEB7 File Offset: 0x0000D0B7
		public static ThumbnailCreatorView CreateThumbnailCreatorView()
		{
			return EngineApplicationInterface.IThumbnailCreatorView.CreateThumbnailCreatorView();
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0000EEC3 File Offset: 0x0000D0C3
		public void RegisterScene(Scene scene, bool usePostFx = true)
		{
			EngineApplicationInterface.IThumbnailCreatorView.RegisterScene(base.Pointer, scene.Pointer, usePostFx);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		public void RegisterCachedEntity(Scene scene, GameEntity entity, string cacheId)
		{
			EngineApplicationInterface.IThumbnailCreatorView.RegisterCachedEntity(base.Pointer, scene.Pointer, entity.Pointer, cacheId);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0000EEFB File Offset: 0x0000D0FB
		public void UnregisterCachedEntity(string cacheId)
		{
			EngineApplicationInterface.IThumbnailCreatorView.UnregisterCachedEntity(base.Pointer, cacheId);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0000EF0E File Offset: 0x0000D10E
		public void RegisterRenderRequest(ref ThumbnailRenderRequest request)
		{
			EngineApplicationInterface.IThumbnailCreatorView.RegisterRenderRequest(base.Pointer, ref request);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0000EF21 File Offset: 0x0000D121
		public void ClearRequests()
		{
			EngineApplicationInterface.IThumbnailCreatorView.ClearRequests(base.Pointer);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0000EF33 File Offset: 0x0000D133
		public void CancelRequest(string renderID)
		{
			EngineApplicationInterface.IThumbnailCreatorView.CancelRequest(base.Pointer, renderID);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0000EF46 File Offset: 0x0000D146
		public int GetNumberOfPendingRequests()
		{
			return EngineApplicationInterface.IThumbnailCreatorView.GetNumberOfPendingRequests(base.Pointer);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0000EF58 File Offset: 0x0000D158
		public bool IsMemoryCleared()
		{
			return EngineApplicationInterface.IThumbnailCreatorView.IsMemoryCleared(base.Pointer);
		}

		// Token: 0x040001CF RID: 463
		public static ThumbnailCreatorView.OnThumbnailRenderCompleteDelegate renderCallback;

		// Token: 0x020000D7 RID: 215
		// (Invoke) Token: 0x06001041 RID: 4161
		public delegate void OnThumbnailRenderCompleteDelegate(string renderId, Texture renderTarget);
	}
}
