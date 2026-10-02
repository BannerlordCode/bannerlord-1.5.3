using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x0200000F RID: 15
	public class BannerVisual : IBannerVisual
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003984 File Offset: 0x00001B84
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000398C File Offset: 0x00001B8C
		public Banner Banner { get; private set; }

		// Token: 0x0600006C RID: 108 RVA: 0x00003995 File Offset: 0x00001B95
		public BannerVisual(Banner banner)
		{
			this.Banner = banner;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000039A4 File Offset: 0x00001BA4
		public void ValidateCreateTableauTextures()
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000039A8 File Offset: 0x00001BA8
		public Texture GetTableauTextureSmall(in BannerDebugInfo debugInfo, Action<Texture> setAction, bool isTableauOrNineGrid = true)
		{
			BannerTextureCreationData bannerTextureCreationData = new BannerTextureCreationData(this.Banner, setAction, null, debugInfo, isTableauOrNineGrid, false);
			return ThumbnailCacheManager.Current.CreateTexture(bannerTextureCreationData).Texture;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000039DC File Offset: 0x00001BDC
		public Texture GetTableauTextureLarge(in BannerDebugInfo debugInfo, Action<Texture> setAction, bool isTableauOrNineGrid = true)
		{
			BannerTextureCreationData bannerTextureCreationData = new BannerTextureCreationData(this.Banner, setAction, null, debugInfo, isTableauOrNineGrid, true);
			return ThumbnailCacheManager.Current.CreateTexture(bannerTextureCreationData).Texture;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003A0F File Offset: 0x00001C0F
		public Texture GetTableauTextureLarge(in BannerDebugInfo debugInfo, Action<Texture> setAction, out BannerTextureCreationData creationData, bool isTableauOrNineGrid = true)
		{
			creationData = new BannerTextureCreationData(this.Banner, setAction, null, debugInfo, isTableauOrNineGrid, true);
			return ThumbnailCacheManager.Current.CreateTexture(creationData).Texture;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A3C File Offset: 0x00001C3C
		public static MatrixFrame GetMeshMatrix(ref Mesh mesh, float marginLeft, float marginTop, float width, float height, bool mirrored, float rotation, float deltaZ)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			float num = width / 1528f;
			float num2 = height / 1528f;
			float num3 = num / mesh.GetBoundingBoxWidth();
			float num4 = num2 / mesh.GetBoundingBoxHeight();
			identity.rotation.RotateAboutUp(rotation);
			if (mirrored)
			{
				identity.rotation.RotateAboutForward(3.1415927f);
			}
			Vec3 vec = new Vec3(num3, num4, 1f, -1f);
			identity.rotation.ApplyScaleLocal(in vec);
			identity.origin.x = 0f;
			identity.origin.y = 0f;
			identity.origin.x = identity.origin.x + marginLeft / 1528f;
			identity.origin.y = identity.origin.y - marginTop / 1528f;
			identity.origin.z = identity.origin.z + deltaZ;
			return identity;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003B1C File Offset: 0x00001D1C
		public MetaMesh ConvertToMultiMesh()
		{
			BannerData bannerDataAtIndex = this.Banner.GetBannerDataAtIndex(0);
			MetaMesh metaMesh = MetaMesh.CreateMetaMesh(null);
			Mesh fromResource = Mesh.GetFromResource(BannerManager.Instance.GetBackgroundMeshName(bannerDataAtIndex.MeshId));
			Mesh mesh = fromResource.CreateCopy();
			fromResource.ManualInvalidate();
			mesh.Color = BannerManager.GetColor(bannerDataAtIndex.ColorId2);
			mesh.Color2 = BannerManager.GetColor(bannerDataAtIndex.ColorId);
			MatrixFrame matrixFrame = BannerVisual.GetMeshMatrix(ref mesh, bannerDataAtIndex.Position.x, bannerDataAtIndex.Position.y, bannerDataAtIndex.Size.x, bannerDataAtIndex.Size.y, bannerDataAtIndex.Mirror, bannerDataAtIndex.RotationValue * 2f * 3.1415927f, 0.5f);
			mesh.SetLocalFrame(matrixFrame);
			metaMesh.AddMesh(mesh);
			mesh.ManualInvalidate();
			for (int i = 1; i < this.Banner.GetBannerDataListCount(); i++)
			{
				BannerData bannerDataAtIndex2 = this.Banner.GetBannerDataAtIndex(i);
				BannerIconData iconDataFromIconId = BannerManager.Instance.GetIconDataFromIconId(bannerDataAtIndex2.MeshId);
				Material fromResource2 = Material.GetFromResource(iconDataFromIconId.MaterialName);
				if (fromResource2 != null)
				{
					Mesh mesh2 = Mesh.CreateMeshWithMaterial(fromResource2);
					float num = (float)(iconDataFromIconId.TextureIndex % 4) * 0.25f;
					float num2 = 1f - (float)(iconDataFromIconId.TextureIndex / 4) * 0.25f;
					Vec2 vec = new Vec2(num, num2);
					Vec2 vec2 = new Vec2(num + 0.25f, num2 - 0.25f);
					UIntPtr uintPtr = mesh2.LockEditDataWrite();
					int num3 = mesh2.AddFaceCorner(new Vec3(-0.5f, -0.5f, 0f, -1f), new Vec3(0f, 0f, 1f, -1f), vec + new Vec2(0f, -0.25f), uint.MaxValue, uintPtr);
					int num4 = mesh2.AddFaceCorner(new Vec3(0.5f, -0.5f, 0f, -1f), new Vec3(0f, 0f, 1f, -1f), vec2, uint.MaxValue, uintPtr);
					int num5 = mesh2.AddFaceCorner(new Vec3(0.5f, 0.5f, 0f, -1f), new Vec3(0f, 0f, 1f, -1f), vec + new Vec2(0.25f, 0f), uint.MaxValue, uintPtr);
					int num6 = mesh2.AddFaceCorner(new Vec3(-0.5f, 0.5f, 0f, -1f), new Vec3(0f, 0f, 1f, -1f), vec, uint.MaxValue, uintPtr);
					mesh2.AddFace(num3, num4, num5, uintPtr);
					mesh2.AddFace(num5, num6, num3, uintPtr);
					mesh2.UnlockEditDataWrite(uintPtr);
					mesh2.SetColorAndStroke(BannerManager.GetColor(bannerDataAtIndex2.ColorId), BannerManager.GetColor(bannerDataAtIndex2.ColorId2), bannerDataAtIndex2.DrawStroke);
					matrixFrame = BannerVisual.GetMeshMatrix(ref mesh2, bannerDataAtIndex2.Position.x, bannerDataAtIndex2.Position.y, bannerDataAtIndex2.Size.x, bannerDataAtIndex2.Size.y, bannerDataAtIndex2.Mirror, bannerDataAtIndex2.RotationValue * 2f * 3.1415927f, (float)i);
					mesh2.SetLocalFrame(matrixFrame);
					metaMesh.AddMesh(mesh2);
				}
			}
			return metaMesh;
		}
	}
}
