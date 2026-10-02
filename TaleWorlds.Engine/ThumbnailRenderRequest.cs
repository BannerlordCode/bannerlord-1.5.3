using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000096 RID: 150
	[EngineStruct("rglThumbnail_render_request", false, null)]
	public struct ThumbnailRenderRequest
	{
		// Token: 0x06000D47 RID: 3399 RVA: 0x0000EF6C File Offset: 0x0000D16C
		public static ThumbnailRenderRequest CreateWithTexture(Scene scene, Camera camera, Texture texture, GameEntity entity, string renderId, string debugName, int allocationGroupIndex)
		{
			return new ThumbnailRenderRequest
			{
				ScenePointer = scene.Pointer,
				CameraPointer = camera.Pointer,
				TexturePointer = texture.Pointer,
				EntityPointer = entity.Pointer,
				RenderId = renderId,
				DebugName = debugName,
				AllocationGroupIndex = allocationGroupIndex
			};
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
		public static ThumbnailRenderRequest CreateWithoutTexture(Scene scene, Camera camera, GameEntity entity, string renderId, int width, int height, string debugName, int allocationGroupIndex)
		{
			return new ThumbnailRenderRequest
			{
				ScenePointer = scene.Pointer,
				CameraPointer = camera.Pointer,
				EntityPointer = entity.Pointer,
				RenderId = renderId,
				Width = width,
				Height = height,
				DebugName = debugName,
				AllocationGroupIndex = allocationGroupIndex
			};
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0000F040 File Offset: 0x0000D240
		public static ThumbnailRenderRequest CreateForCachedEntity(Scene scene, Camera camera, Texture texture, string cachedEntityId, string renderId, string debugName, int allocationGroupIndex)
		{
			return new ThumbnailRenderRequest
			{
				ScenePointer = scene.Pointer,
				CameraPointer = camera.Pointer,
				TexturePointer = texture.Pointer,
				CachedEntityId = cachedEntityId,
				RenderId = renderId,
				DebugName = debugName,
				AllocationGroupIndex = allocationGroupIndex
			};
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
		public static ThumbnailRenderRequest CreateForCachedEntityWithoutTexture(Scene scene, Camera camera, string cachedEntityId, string renderId, int width, int height, string debugName, int allocationGroupIndex)
		{
			return new ThumbnailRenderRequest
			{
				ScenePointer = scene.Pointer,
				CameraPointer = camera.Pointer,
				CachedEntityId = cachedEntityId,
				RenderId = renderId,
				Width = width,
				Height = height,
				DebugName = debugName,
				AllocationGroupIndex = allocationGroupIndex
			};
		}

		// Token: 0x040001D0 RID: 464
		public UIntPtr ScenePointer;

		// Token: 0x040001D1 RID: 465
		public UIntPtr CameraPointer;

		// Token: 0x040001D2 RID: 466
		public UIntPtr TexturePointer;

		// Token: 0x040001D3 RID: 467
		public string CachedEntityId;

		// Token: 0x040001D4 RID: 468
		public UIntPtr EntityPointer;

		// Token: 0x040001D5 RID: 469
		public int Width;

		// Token: 0x040001D6 RID: 470
		public int Height;

		// Token: 0x040001D7 RID: 471
		public string RenderId;

		// Token: 0x040001D8 RID: 472
		public string DebugName;

		// Token: 0x040001D9 RID: 473
		public int AllocationGroupIndex;
	}
}
