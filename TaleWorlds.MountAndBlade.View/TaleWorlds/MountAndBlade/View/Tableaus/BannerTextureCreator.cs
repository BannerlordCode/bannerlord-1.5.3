using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000030 RID: 48
	internal static class BannerTextureCreator
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00009918 File Offset: 0x00007B18
		internal static void Initialize(ThumbnailCreatorView thumbnailCreatorView)
		{
			BannerTextureCreator._thumbnailCreatorView = thumbnailCreatorView;
			BannerTextureCreator._scene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
			BannerTextureCreator._scene.DisableStaticShadows(true);
			BannerTextureCreator._scene.SetName("ThumbnailCacheManager.BannerScene");
			BannerTextureCreator._scene.SetDefaultLighting();
			BannerTextureCreator._thumbnailCreatorView.RegisterScene(BannerTextureCreator._scene, false);
			BannerTextureCreator._bannerCamera = BannerTextureCreator.CreateDefaultBannerCamera();
			BannerTextureCreator._nineGridBannerCamera = BannerTextureCreator.CreateNineGridBannerCamera();
			BannerTextureCreator._bannerTableauGPUAllocationIndex = Utilities.RegisterGPUAllocationGroup("BannerTableauCache");
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009994 File Offset: 0x00007B94
		internal static void OnFinalize()
		{
			Scene scene = BannerTextureCreator._scene;
			if (scene != null)
			{
				scene.ClearDecals();
			}
			Scene scene2 = BannerTextureCreator._scene;
			if (scene2 != null)
			{
				scene2.ClearAll();
			}
			Scene scene3 = BannerTextureCreator._scene;
			if (scene3 != null)
			{
				scene3.ManualInvalidate();
			}
			Camera bannerCamera = BannerTextureCreator._bannerCamera;
			if (bannerCamera != null)
			{
				bannerCamera.ReleaseCamera();
			}
			BannerTextureCreator._bannerCamera = null;
			Camera nineGridBannerCamera = BannerTextureCreator._nineGridBannerCamera;
			if (nineGridBannerCamera != null)
			{
				nineGridBannerCamera.ReleaseCamera();
			}
			BannerTextureCreator._nineGridBannerCamera = null;
			BannerTextureCreator._scene = null;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009A04 File Offset: 0x00007C04
		internal static Texture CreateTexture(BannerThumbnailCreationBaseData bannerCreationData)
		{
			bool isTableauOrNineGrid = bannerCreationData.IsTableauOrNineGrid;
			bool isLarge = bannerCreationData.IsLarge;
			Action<Texture> setAction = bannerCreationData.SetAction;
			string renderId = bannerCreationData.RenderId;
			BannerDebugInfo debugInfo = bannerCreationData.DebugInfo;
			Banner banner = bannerCreationData.Banner;
			bool flag = !(bannerCreationData is BannerTextureCreationData);
			int num = 512;
			int num2 = 512;
			Camera camera = BannerTextureCreator._bannerCamera;
			if (isTableauOrNineGrid)
			{
				camera = BannerTextureCreator._nineGridBannerCamera;
				if (isLarge)
				{
					num = 1024;
					num2 = 1024;
				}
			}
			MatrixFrame identity = MatrixFrame.Identity;
			if (Game.Current == null)
			{
				banner.SetBannerVisual(((IBannerVisualCreator)new BannerVisualCreator()).CreateBannerVisual(banner));
			}
			string text = ThumbnailDebugUtility.CreateDebugIdFrom(renderId, "ban", debugInfo.CreateName());
			Texture texture = Texture.CreateRenderTarget(text, num, num2, false, false, true, !flag);
			if (!flag && setAction != null)
			{
				setAction(texture);
			}
			if (!Banner.IsValidBannerCode(banner.BannerCode))
			{
				Debug.FailedAssert("Banner code is not valid: " + banner.BannerCode, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\BannerTextureCreator.cs", "CreateTexture", 93);
				return texture;
			}
			MetaMesh metaMesh = banner.ConvertToMultiMesh();
			GameEntity gameEntity = BannerTextureCreator._scene.AddItemEntity(ref identity, metaMesh);
			metaMesh.ManualInvalidate();
			gameEntity.SetVisibilityExcludeParents(false);
			ThumbnailRenderRequest thumbnailRenderRequest = ThumbnailRenderRequest.CreateWithTexture(BannerTextureCreator._scene, camera, texture, gameEntity, renderId, text, BannerTextureCreator._bannerTableauGPUAllocationIndex);
			BannerTextureCreator._thumbnailCreatorView.RegisterRenderRequest(ref thumbnailRenderRequest);
			return texture;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009B54 File Offset: 0x00007D54
		internal static Camera CreateDefaultBannerCamera()
		{
			return BannerTextureCreator.CreateCamera(0.33333334f, 0.6666667f, -0.6666667f, -0.33333334f, 0.001f, 510f);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009B79 File Offset: 0x00007D79
		internal static Camera CreateNineGridBannerCamera()
		{
			return BannerTextureCreator.CreateCamera(0f, 1f, -1f, 0f, 0.001f, 510f);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009BA0 File Offset: 0x00007DA0
		private static Camera CreateCamera(float left, float right, float bottom, float top, float near, float far)
		{
			Camera camera = Camera.CreateCamera();
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin.z = 400f;
			camera.Frame = identity;
			camera.LookAt(new Vec3(0f, 0f, 400f, -1f), new Vec3(0f, 0f, 0f, -1f), new Vec3(0f, 1f, 0f, -1f));
			camera.SetViewVolume(false, left, right, bottom, top, near, far);
			return camera;
		}

		// Token: 0x04000071 RID: 113
		private static Scene _scene;

		// Token: 0x04000072 RID: 114
		private static Camera _bannerCamera;

		// Token: 0x04000073 RID: 115
		private static Camera _nineGridBannerCamera;

		// Token: 0x04000074 RID: 116
		private static ThumbnailCreatorView _thumbnailCreatorView;

		// Token: 0x04000075 RID: 117
		private static int _bannerTableauGPUAllocationIndex;
	}
}
