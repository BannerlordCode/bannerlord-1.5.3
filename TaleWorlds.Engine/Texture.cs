using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000093 RID: 147
	public sealed class Texture : Resource
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0000EB98 File Offset: 0x0000CD98
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		public bool IsReleased { get; private set; }

		// Token: 0x06000D18 RID: 3352 RVA: 0x0000EBA9 File Offset: 0x0000CDA9
		private Texture()
		{
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0000EBB1 File Offset: 0x0000CDB1
		internal Texture(UIntPtr ptr)
			: base(ptr)
		{
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0000EBBA File Offset: 0x0000CDBA
		public int Width
		{
			get
			{
				return EngineApplicationInterface.ITexture.GetWidth(base.Pointer);
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0000EBCC File Offset: 0x0000CDCC
		public int Height
		{
			get
			{
				return EngineApplicationInterface.ITexture.GetHeight(base.Pointer);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0000EBDE File Offset: 0x0000CDDE
		public int MemorySize
		{
			get
			{
				return EngineApplicationInterface.ITexture.GetMemorySize(base.Pointer);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		public bool IsRenderTarget
		{
			get
			{
				return EngineApplicationInterface.ITexture.IsRenderTarget(base.Pointer);
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0000EC02 File Offset: 0x0000CE02
		public static Texture CreateTextureFromPath(PlatformFilePath filePath)
		{
			return EngineApplicationInterface.ITexture.CreateTextureFromPath(filePath);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0000EC0F File Offset: 0x0000CE0F
		public void GetPixelData(byte[] bytes)
		{
			EngineApplicationInterface.ITexture.GetPixelData(base.Pointer, bytes);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x0000EC22 File Offset: 0x0000CE22
		// (set) Token: 0x06000D21 RID: 3361 RVA: 0x0000EC34 File Offset: 0x0000CE34
		public string Name
		{
			get
			{
				return EngineApplicationInterface.ITexture.GetName(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ITexture.SetName(base.Pointer, value);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0000EC47 File Offset: 0x0000CE47
		public void TransformRenderTargetToResource(string name)
		{
			EngineApplicationInterface.ITexture.TransformRenderTargetToResourceTexture(base.Pointer, name);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0000EC5A File Offset: 0x0000CE5A
		public static Texture GetFromResource(string resourceName)
		{
			return EngineApplicationInterface.ITexture.GetFromResource(resourceName);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0000EC67 File Offset: 0x0000CE67
		public bool IsLoaded()
		{
			return EngineApplicationInterface.ITexture.IsLoaded(base.Pointer);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0000EC79 File Offset: 0x0000CE79
		public void GetSDFBoundingBoxData(ref Vec3 min, ref Vec3 max)
		{
			EngineApplicationInterface.ITexture.GetSDFBoundingBoxData(base.Pointer, ref min, ref max);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0000EC8D File Offset: 0x0000CE8D
		public static Texture CheckAndGetFromResource(string resourceName)
		{
			return EngineApplicationInterface.ITexture.CheckAndGetFromResource(resourceName);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		public static void ScaleTextureWithRatio(ref int tableauSizeX, ref int tableauSizeY)
		{
			float num = (float)tableauSizeX;
			float num2 = (float)tableauSizeY;
			int num3 = (int)MathF.Log(num, 2f) + 2;
			float num4 = MathF.Pow(2f, (float)num3) / num;
			tableauSizeX = (int)(num * num4);
			tableauSizeY = (int)(num2 * num4);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0000ECDB File Offset: 0x0000CEDB
		public void PreloadTexture(bool blocking)
		{
			EngineApplicationInterface.ITexture.GetCurObject(base.Pointer, blocking);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0000ECEE File Offset: 0x0000CEEE
		public void Release()
		{
			this.IsReleased = true;
			this.RenderTargetComponent.OnTargetReleased();
			base.ManualInvalidate();
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0000ED08 File Offset: 0x0000CF08
		public void ReleaseImmediately()
		{
			this.IsReleased = true;
			this.RenderTargetComponent.OnTargetReleased();
			EngineApplicationInterface.ITexture.Release(base.Pointer);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0000ED2C File Offset: 0x0000CF2C
		public void ReleaseAfterNumberOfFrames(int frameCount)
		{
			this.RenderTargetComponent.OnTargetReleased();
			EngineApplicationInterface.ITexture.ReleaseAfterNumberOfFrames(base.Pointer, frameCount);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0000ED4A File Offset: 0x0000CF4A
		public static Texture LoadTextureFromPath(string fileName, string folder)
		{
			return EngineApplicationInterface.ITexture.LoadTextureFromPath(fileName, folder);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0000ED58 File Offset: 0x0000CF58
		public static Texture CreateDepthTarget(string name, int width, int height)
		{
			return EngineApplicationInterface.ITexture.CreateDepthTarget(name, width, height);
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0000ED67 File Offset: 0x0000CF67
		public static Texture CreateFromByteArray(byte[] data, int width, int height)
		{
			return EngineApplicationInterface.ITexture.CreateFromByteArray(data, width, height);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0000ED76 File Offset: 0x0000CF76
		public void SaveToFile(string path, bool isRelativePath)
		{
			EngineApplicationInterface.ITexture.SaveToFile(base.Pointer, path, isRelativePath);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0000ED8A File Offset: 0x0000CF8A
		public void SetTextureAsAlwaysValid()
		{
			EngineApplicationInterface.ITexture.SaveTextureAsAlwaysValid(base.Pointer);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0000ED9C File Offset: 0x0000CF9C
		public static Texture CreateFromMemory(byte[] data)
		{
			return EngineApplicationInterface.ITexture.CreateFromMemory(data);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0000EDA9 File Offset: 0x0000CFA9
		public static void ReleaseGpuMemories()
		{
			EngineApplicationInterface.ITexture.ReleaseGpuMemories();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0000EDB5 File Offset: 0x0000CFB5
		public RenderTargetComponent RenderTargetComponent
		{
			get
			{
				return EngineApplicationInterface.ITexture.GetRenderTargetComponent(base.Pointer);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0000EDC7 File Offset: 0x0000CFC7
		public TableauView TableauView
		{
			get
			{
				return EngineApplicationInterface.ITexture.GetTableauView(base.Pointer);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0000EDD9 File Offset: 0x0000CFD9
		public object UserData
		{
			get
			{
				return this.RenderTargetComponent.UserData;
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0000EDE6 File Offset: 0x0000CFE6
		private void SetTableauView(TableauView tableauView)
		{
			EngineApplicationInterface.ITexture.SetTableauView(base.Pointer, tableauView.Pointer);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0000EE00 File Offset: 0x0000D000
		public static Texture CreateTableauTexture(string name, RenderTargetComponent.TextureUpdateEventHandler eventHandler, object objectRef, int tableauSizeX, int tableauSizeY)
		{
			Texture texture = Texture.CreateRenderTarget(name, tableauSizeX, tableauSizeY, true, true, false, false);
			RenderTargetComponent renderTargetComponent = texture.RenderTargetComponent;
			renderTargetComponent.PaintNeeded += eventHandler;
			renderTargetComponent.UserData = objectRef;
			TableauView tableauView = TableauView.CreateTableauView(name);
			tableauView.SetRenderTarget(texture);
			tableauView.SetAutoDepthTargetCreation(true);
			tableauView.SetSceneUsesSkybox(false);
			tableauView.SetClearColor(4294902015U);
			texture.SetTableauView(tableauView);
			return texture;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0000EE5D File Offset: 0x0000D05D
		public static Texture CreateRenderTarget(string name, int width, int height, bool autoMipmaps, bool isTableau, bool createUninitialized = false, bool always_valid = false)
		{
			return EngineApplicationInterface.ITexture.CreateRenderTarget(name, width, height, autoMipmaps, isTableau, createUninitialized, always_valid);
		}
	}
}
