using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005F RID: 95
	public sealed class Material : Resource
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x000087CD File Offset: 0x000069CD
		public static Material GetDefaultMaterial()
		{
			return EngineApplicationInterface.IMaterial.GetDefaultMaterial();
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x000087D9 File Offset: 0x000069D9
		public static Material GetOutlineMaterial(Mesh mesh)
		{
			return EngineApplicationInterface.IMaterial.GetOutlineMaterial(mesh.GetMaterial().Pointer);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x000087F0 File Offset: 0x000069F0
		public static Material GetDefaultTableauSampleMaterial(bool transparency)
		{
			if (!transparency)
			{
				return Material.GetFromResource("sample_shield_matte");
			}
			return Material.GetFromResource("tableau_with_transparency");
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000880C File Offset: 0x00006A0C
		public static Material CreateTableauMaterial(RenderTargetComponent.TextureUpdateEventHandler eventHandler, object objectRef, Material sampleMaterial, int tableauSizeX, int tableauSizeY, bool continuousTableau = false)
		{
			if (sampleMaterial == null)
			{
				sampleMaterial = Material.GetDefaultTableauSampleMaterial(true);
			}
			Material material = sampleMaterial.CreateCopy();
			uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
			ulong shaderFlags = material.GetShaderFlags();
			material.SetShaderFlags(shaderFlags | (ulong)num);
			string text = "";
			Type type = objectRef.GetType();
			MaterialCacheIDGetMethodDelegate materialCacheIDGetMethodDelegate;
			if (!continuousTableau && HasTableauCache.TableauCacheTypes.TryGetValue(type, out materialCacheIDGetMethodDelegate))
			{
				text = materialCacheIDGetMethodDelegate(objectRef);
				text = text.ToLower();
				Texture texture = Texture.CheckAndGetFromResource(text);
				if (texture != null)
				{
					material.SetTexture(Material.MBTextureType.DiffuseMap2, texture);
					return material;
				}
			}
			if (text != "")
			{
				Texture.ScaleTextureWithRatio(ref tableauSizeX, ref tableauSizeY);
			}
			Texture texture2 = Texture.CreateTableauTexture(text, eventHandler, objectRef, tableauSizeX, tableauSizeY);
			if (text != "")
			{
				TableauView tableauView = texture2.TableauView;
				tableauView.SetSaveFinalResultToDisk(true);
				tableauView.SetFileNameToSaveResult(text);
				tableauView.SetFileTypeToSave(View.TextureSaveFormat.TextureTypeDds);
			}
			if (text != "")
			{
				texture2.TransformRenderTargetToResource(text);
			}
			material.SetTexture(Material.MBTextureType.DiffuseMap2, texture2);
			return material;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0000890E File Offset: 0x00006B0E
		internal Material(UIntPtr sourceMaterialPointer)
			: base(sourceMaterialPointer)
		{
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00008917 File Offset: 0x00006B17
		public Material CreateCopy()
		{
			return EngineApplicationInterface.IMaterial.CreateCopy(base.Pointer);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00008929 File Offset: 0x00006B29
		public static Material GetFromResource(string materialName)
		{
			return EngineApplicationInterface.IMaterial.GetFromResource(materialName);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00008936 File Offset: 0x00006B36
		public void SetShader(Shader shader)
		{
			EngineApplicationInterface.IMaterial.SetShader(base.Pointer, shader.Pointer);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0000894E File Offset: 0x00006B4E
		public Shader GetShader()
		{
			return EngineApplicationInterface.IMaterial.GetShader(base.Pointer);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00008960 File Offset: 0x00006B60
		public ulong GetShaderFlags()
		{
			return EngineApplicationInterface.IMaterial.GetShaderFlags(base.Pointer);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00008972 File Offset: 0x00006B72
		public void SetShaderFlags(ulong flagEntry)
		{
			EngineApplicationInterface.IMaterial.SetShaderFlags(base.Pointer, flagEntry);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00008985 File Offset: 0x00006B85
		public void SetMeshVectorArgument(float x, float y, float z, float w)
		{
			EngineApplicationInterface.IMaterial.SetMeshVectorArgument(base.Pointer, x, y, z, w);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000899C File Offset: 0x00006B9C
		public void SetTexture(Material.MBTextureType textureType, Texture texture)
		{
			EngineApplicationInterface.IMaterial.SetTexture(base.Pointer, (int)textureType, texture.Pointer);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000089B5 File Offset: 0x00006BB5
		public void SetTextureAtSlot(int textureSlot, Texture texture)
		{
			EngineApplicationInterface.IMaterial.SetTextureAtSlot(base.Pointer, textureSlot, texture.Pointer);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000089CE File Offset: 0x00006BCE
		public void SetAreaMapScale(float scale)
		{
			EngineApplicationInterface.IMaterial.SetAreaMapScale(base.Pointer, scale);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000089E1 File Offset: 0x00006BE1
		public void SetEnableSkinning(bool enable)
		{
			EngineApplicationInterface.IMaterial.SetEnableSkinning(base.Pointer, enable);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000089F4 File Offset: 0x00006BF4
		public bool UsingSkinning()
		{
			return EngineApplicationInterface.IMaterial.UsingSkinning(base.Pointer);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00008A06 File Offset: 0x00006C06
		public Texture GetTexture(Material.MBTextureType textureType)
		{
			return EngineApplicationInterface.IMaterial.GetTexture(base.Pointer, (int)textureType);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00008A19 File Offset: 0x00006C19
		public Texture GetTextureWithSlot(int textureSlot)
		{
			return EngineApplicationInterface.IMaterial.GetTexture(base.Pointer, textureSlot);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00008A2C File Offset: 0x00006C2C
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00008A3E File Offset: 0x00006C3E
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IMaterial.GetName(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMaterial.SetName(base.Pointer, value);
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00008A51 File Offset: 0x00006C51
		public static Material GetAlphaMaskTableauMaterial()
		{
			return EngineApplicationInterface.IMaterial.GetFromResource("tableau_with_alpha_mask");
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00008A62 File Offset: 0x00006C62
		public Material.MBAlphaBlendMode GetAlphaBlendMode()
		{
			return (Material.MBAlphaBlendMode)EngineApplicationInterface.IMaterial.GetAlphaBlendMode(base.Pointer);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00008A75 File Offset: 0x00006C75
		public void SetAlphaBlendMode(Material.MBAlphaBlendMode alphaBlendMode)
		{
			EngineApplicationInterface.IMaterial.SetAlphaBlendMode(base.Pointer, (int)alphaBlendMode);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00008A88 File Offset: 0x00006C88
		public void SetAlphaTestValue(float alphaTestValue)
		{
			EngineApplicationInterface.IMaterial.SetAlphaTestValue(base.Pointer, alphaTestValue);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00008A9B File Offset: 0x00006C9B
		public float GetAlphaTestValue()
		{
			return EngineApplicationInterface.IMaterial.GetAlphaTestValue(base.Pointer);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00008AAD File Offset: 0x00006CAD
		private bool CheckMaterialShaderFlag(Material.MBMaterialShaderFlags flagEntry)
		{
			return (EngineApplicationInterface.IMaterial.GetShaderFlags(base.Pointer) & (ulong)((long)flagEntry)) > 0UL;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00008AC8 File Offset: 0x00006CC8
		private void SetMaterialShaderFlag(Material.MBMaterialShaderFlags flagEntry, bool value)
		{
			ulong num = (EngineApplicationInterface.IMaterial.GetShaderFlags(base.Pointer) & (ulong)(~(ulong)((long)flagEntry))) | (ulong)((long)flagEntry & (value ? 255L : 0L));
			EngineApplicationInterface.IMaterial.SetShaderFlags(base.Pointer, num);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00008B0C File Offset: 0x00006D0C
		public void AddMaterialShaderFlag(string flagName, bool showErrors)
		{
			EngineApplicationInterface.IMaterial.AddMaterialShaderFlag(base.Pointer, flagName, showErrors);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00008B20 File Offset: 0x00006D20
		public void RemoveMaterialShaderFlag(string flagName)
		{
			EngineApplicationInterface.IMaterial.RemoveMaterialShaderFlag(base.Pointer, flagName);
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00008B33 File Offset: 0x00006D33
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x00008B3C File Offset: 0x00006D3C
		public bool UsingSpecular
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSpecular);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSpecular, value);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00008B46 File Offset: 0x00006D46
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x00008B4F File Offset: 0x00006D4F
		public bool UsingSpecularMap
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSpecularMap);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSpecularMap, value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00008B59 File Offset: 0x00006D59
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x00008B62 File Offset: 0x00006D62
		public bool UsingEnvironmentMap
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseEnvironmentMap);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseEnvironmentMap, value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00008B6C File Offset: 0x00006D6C
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x00008B79 File Offset: 0x00006D79
		public bool UsingSpecularAlpha
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSpecularAlpha);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSpecularAlpha, value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00008B87 File Offset: 0x00006D87
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x00008B91 File Offset: 0x00006D91
		public bool UsingDynamicLight
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseDynamicLight);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseDynamicLight, value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00008B9C File Offset: 0x00006D9C
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x00008BA6 File Offset: 0x00006DA6
		public bool UsingSunLight
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSunLight);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseSunLight, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x00008BB1 File Offset: 0x00006DB1
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x00008BBE File Offset: 0x00006DBE
		public bool UsingFresnel
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseFresnel);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseFresnel, value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00008BCC File Offset: 0x00006DCC
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x00008BD9 File Offset: 0x00006DD9
		public bool IsSunShadowReceiver
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.SunShadowReceiver);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.SunShadowReceiver, value);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00008BE7 File Offset: 0x00006DE7
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x00008BF4 File Offset: 0x00006DF4
		public bool IsDynamicShadowReceiver
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.DynamicShadowReceiver);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.DynamicShadowReceiver, value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00008C02 File Offset: 0x00006E02
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00008C0F File Offset: 0x00006E0F
		public bool UsingDiffuseAlphaMap
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseDiffuseAlphaMap);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseDiffuseAlphaMap, value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00008C1D File Offset: 0x00006E1D
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00008C2A File Offset: 0x00006E2A
		public bool UsingParallaxMapping
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseParallaxMapping);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseParallaxMapping, value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00008C38 File Offset: 0x00006E38
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x00008C45 File Offset: 0x00006E45
		public bool UsingParallaxOcclusion
		{
			get
			{
				return this.CheckMaterialShaderFlag(Material.MBMaterialShaderFlags.UseParallaxOcclusion);
			}
			set
			{
				this.SetMaterialShaderFlag(Material.MBMaterialShaderFlags.UseParallaxOcclusion, value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00008C53 File Offset: 0x00006E53
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x00008C65 File Offset: 0x00006E65
		public MaterialFlags Flags
		{
			get
			{
				return EngineApplicationInterface.IMaterial.GetFlags(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMaterial.SetFlags(base.Pointer, value);
			}
		}

		// Token: 0x020000C6 RID: 198
		public enum MBTextureType
		{
			// Token: 0x040003F2 RID: 1010
			DiffuseMap,
			// Token: 0x040003F3 RID: 1011
			DiffuseMap2,
			// Token: 0x040003F4 RID: 1012
			BumpMap,
			// Token: 0x040003F5 RID: 1013
			EnvironmentMap,
			// Token: 0x040003F6 RID: 1014
			SpecularMap
		}

		// Token: 0x020000C7 RID: 199
		[EngineStruct("rglAlpha_blend_mode", true, "rgl_abm", false)]
		public enum MBAlphaBlendMode : byte
		{
			// Token: 0x040003F8 RID: 1016
			NoAlphaBlend,
			// Token: 0x040003F9 RID: 1017
			Modulate,
			// Token: 0x040003FA RID: 1018
			AddAlpha,
			// Token: 0x040003FB RID: 1019
			Multiply,
			// Token: 0x040003FC RID: 1020
			Add,
			// Token: 0x040003FD RID: 1021
			Max,
			// Token: 0x040003FE RID: 1022
			Factor,
			// Token: 0x040003FF RID: 1023
			AddModulateCombined,
			// Token: 0x04000400 RID: 1024
			NoAlphaBlendNoWrite,
			// Token: 0x04000401 RID: 1025
			ModulateNoWrite,
			// Token: 0x04000402 RID: 1026
			GbufferAlphaBlend,
			// Token: 0x04000403 RID: 1027
			GbufferAlphaBlendWithVtResolve,
			// Token: 0x04000404 RID: 1028
			NoAlphaBlendNoAlphaWrite,
			// Token: 0x04000405 RID: 1029
			Total
		}

		// Token: 0x020000C8 RID: 200
		[Flags]
		private enum MBMaterialShaderFlags
		{
			// Token: 0x04000407 RID: 1031
			UseSpecular = 1,
			// Token: 0x04000408 RID: 1032
			UseSpecularMap = 2,
			// Token: 0x04000409 RID: 1033
			UseHemisphericalAmbient = 4,
			// Token: 0x0400040A RID: 1034
			UseEnvironmentMap = 8,
			// Token: 0x0400040B RID: 1035
			UseDXT5Normal = 16,
			// Token: 0x0400040C RID: 1036
			UseDynamicLight = 32,
			// Token: 0x0400040D RID: 1037
			UseSunLight = 64,
			// Token: 0x0400040E RID: 1038
			UseSpecularAlpha = 128,
			// Token: 0x0400040F RID: 1039
			UseFresnel = 256,
			// Token: 0x04000410 RID: 1040
			SunShadowReceiver = 512,
			// Token: 0x04000411 RID: 1041
			DynamicShadowReceiver = 1024,
			// Token: 0x04000412 RID: 1042
			UseDiffuseAlphaMap = 2048,
			// Token: 0x04000413 RID: 1043
			UseParallaxMapping = 4096,
			// Token: 0x04000414 RID: 1044
			UseParallaxOcclusion = 8192,
			// Token: 0x04000415 RID: 1045
			UseAlphaTestingBit0 = 16384,
			// Token: 0x04000416 RID: 1046
			UseAlphaTestingBit1 = 32768,
			// Token: 0x04000417 RID: 1047
			UseAreaMap = 65536,
			// Token: 0x04000418 RID: 1048
			UseDetailNormalMap = 131072,
			// Token: 0x04000419 RID: 1049
			UseGroundSlopeAlpha = 262144,
			// Token: 0x0400041A RID: 1050
			UseSelfIllumination = 524288,
			// Token: 0x0400041B RID: 1051
			UseColorMapping = 1048576,
			// Token: 0x0400041C RID: 1052
			UseCubicAmbient = 2097152
		}
	}
}
