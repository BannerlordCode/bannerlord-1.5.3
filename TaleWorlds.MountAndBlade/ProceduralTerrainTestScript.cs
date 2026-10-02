using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000342 RID: 834
	public class ProceduralTerrainTestScript : ScriptComponentBehavior
	{
		// Token: 0x06002EFA RID: 12026 RVA: 0x000B6002 File Offset: 0x000B4202
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000B600C File Offset: 0x000B420C
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this._pendingGenerate = !base.GameEntity.Scene.ContainsTerrain;
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x000B6047 File Offset: 0x000B4247
		protected internal override void OnTick(float dt)
		{
			if (!this._pendingGenerate)
			{
				return;
			}
			this._pendingGenerate = false;
			this.TryGenerate();
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x000B605F File Offset: 0x000B425F
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "GenerateTerrain")
			{
				this.TryGenerate();
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000B607C File Offset: 0x000B427C
		private void TryGenerate()
		{
			Scene scene = base.Scene;
			if (scene == null)
			{
				return;
			}
			this.GenerateProceduralTerrain(scene);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000B60A4 File Offset: 0x000B42A4
		private void GenerateProceduralTerrain(Scene scene)
		{
			int num = (1 << this.HeightmapDetailLevel) + 1;
			float num2 = this.NodeSize / (float)(num - 1);
			float[] array = new float[num * num];
			float[] array2 = new float[num * num];
			float[] array3 = new float[num * num];
			float[] array4 = new float[num * num];
			float[] array5 = new float[num * num];
			TerrainEditContext terrainEditContext = new TerrainEditContext(scene, this.NodeDimensionX, this.NodeDimensionY, this.NodeSize, this.MinHeight, this.MaxHeight, this.HeightmapDetailLevel, "desert_a");
			for (int i = 0; i < this.NodeDimensionX; i++)
			{
				for (int j = 0; j < this.NodeDimensionY; j++)
				{
					this.FillHeights(array, i, j, num, num2);
					terrainEditContext.SetHeightData(i, j, array);
				}
			}
			int num3 = terrainEditContext.AddLayerFromMaterial("rock_cliff");
			terrainEditContext.SetLayerTexture(num3, TerrainEditContext.TextureSlot.Diffuse, "rock_cliff_b_d");
			terrainEditContext.SetLayerTexture(num3, TerrainEditContext.TextureSlot.NormalMap, "rock_cliff_b_n");
			terrainEditContext.SetLayerTexture(num3, TerrainEditContext.TextureSlot.SpecularMap, "rock_cliff_b_s");
			terrainEditContext.SetLayerTexture(num3, TerrainEditContext.TextureSlot.DisplacementMap, "rock_cliff_b_h");
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.PhysicsMaterial, "stone");
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.UvScaleX, 6f);
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.UvScaleY, 6f);
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.ParallaxAmount, 0.15f);
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.GroundSlopeScale, 1f);
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.FlagUseParallax, 1f);
			terrainEditContext.SetLayerProperty(num3, TerrainEditContext.LayerProperty.FlagSlopeTransparency, 1f);
			int num4 = terrainEditContext.AddLayerFromMaterial("soil_b");
			terrainEditContext.SetLayerTexture(num4, TerrainEditContext.TextureSlot.Diffuse, "ground_soil_b_d");
			terrainEditContext.SetLayerTexture(num4, TerrainEditContext.TextureSlot.NormalMap, "ground_soil_b_n");
			terrainEditContext.SetLayerTexture(num4, TerrainEditContext.TextureSlot.SpecularMap, "ground_soil_b_s");
			terrainEditContext.SetLayerTexture(num4, TerrainEditContext.TextureSlot.DisplacementMap, "ground_soil_b_h");
			terrainEditContext.SetLayerProperty(num4, TerrainEditContext.LayerProperty.PhysicsMaterial, "soil");
			terrainEditContext.SetLayerProperty(num4, TerrainEditContext.LayerProperty.UvScaleX, 5f);
			terrainEditContext.SetLayerProperty(num4, TerrainEditContext.LayerProperty.UvScaleY, 5f);
			terrainEditContext.SetLayerProperty(num4, TerrainEditContext.LayerProperty.ParallaxAmount, 0.15f);
			terrainEditContext.SetLayerProperty(num4, TerrainEditContext.LayerProperty.FlagUseParallax, 1f);
			int num5 = terrainEditContext.AddLayerFromMaterial("flora_habitat_a");
			terrainEditContext.SetLayerTexture(num5, TerrainEditContext.TextureSlot.Diffuse, "ground_grass_c_d");
			terrainEditContext.SetLayerTexture(num5, TerrainEditContext.TextureSlot.NormalMap, "ground_grass_c_n");
			terrainEditContext.SetLayerTexture(num5, TerrainEditContext.TextureSlot.SpecularMap, "ground_grass_c_s");
			terrainEditContext.SetLayerTexture(num5, TerrainEditContext.TextureSlot.DisplacementMap, "ground_grass_c_h");
			terrainEditContext.SetLayerProperty(num5, TerrainEditContext.LayerProperty.PhysicsMaterial, "soil");
			terrainEditContext.SetLayerProperty(num5, TerrainEditContext.LayerProperty.UvScaleX, 5f);
			terrainEditContext.SetLayerProperty(num5, TerrainEditContext.LayerProperty.UvScaleY, 5f);
			terrainEditContext.SetLayerProperty(num5, TerrainEditContext.LayerProperty.ParallaxAmount, 0.15f);
			terrainEditContext.SetLayerProperty(num5, TerrainEditContext.LayerProperty.FlagUseParallax, 1f);
			int num6 = terrainEditContext.AddEmptyLayer("flora_only");
			terrainEditContext.SetLayerProperty(num6, TerrainEditContext.LayerProperty.IsFloraLayer, 1f);
			for (int k = 0; k < this.NodeDimensionX; k++)
			{
				for (int l = 0; l < this.NodeDimensionY; l++)
				{
					this.FillHeights(array, k, l, num, num2);
					ProceduralTerrainTestScript.FillWeights(array, array2, array3, array4, array5, num, num2, (float)k * this.NodeSize, (float)l * this.NodeSize);
					terrainEditContext.SetLayerWeightData(k, l, num3, array2);
					terrainEditContext.SetLayerWeightData(k, l, num4, array3);
					terrainEditContext.SetLayerWeightData(k, l, num5, array4);
					terrainEditContext.SetLayerWeightData(k, l, num6, array5);
				}
			}
			terrainEditContext.AddProceduralFlora(num5, new TerrainEditContext.FloraDefinition
			{
				FloraKindName = this.GrassKindName,
				Density = this.GrassDensity,
				SeedIndex = 3,
				SizeMin = 0.65f,
				SizeMax = 1.4f,
				ColonyRadius = 0f,
				ColonyThreshold = 0f,
				WeightOffset = 0.5f
			});
			terrainEditContext.AddProceduralFlora(num5, new TerrainEditContext.FloraDefinition
			{
				FloraKindName = this.ShrubKindName,
				Density = this.ShrubDensity,
				SeedIndex = 7,
				SizeMin = 0.65f,
				SizeMax = 1.3f,
				ColonyRadius = 0f,
				ColonyThreshold = 0f,
				WeightOffset = 0.5f
			});
			terrainEditContext.AddProceduralFlora(num5, new TerrainEditContext.FloraDefinition
			{
				FloraKindName = this.TreeKindName,
				Density = this.TreeDensity,
				SeedIndex = 42,
				SizeMin = 0.8f,
				SizeMax = 1.4f,
				ColonyRadius = 0f,
				ColonyThreshold = 0.3f,
				WeightOffset = 0.3f
			});
			terrainEditContext.AddProceduralFlora(num6, new TerrainEditContext.FloraDefinition
			{
				FloraKindName = this.UndergrowthKindName,
				Density = this.UndergrowthDensity,
				SeedIndex = 11,
				SizeMin = 0.7f,
				SizeMax = 1.3f,
				ColonyRadius = 0f,
				ColonyThreshold = 0f,
				WeightOffset = 0.5f
			});
			terrainEditContext.AddProceduralFlora(num6, new TerrainEditContext.FloraDefinition
			{
				FloraKindName = this.FlowerKindName,
				Density = this.FlowerDensity,
				SeedIndex = 23,
				SizeMin = 0.8f,
				SizeMax = 1.2f,
				ColonyRadius = 35f,
				ColonyThreshold = 0.4f,
				WeightOffset = 0.4f
			});
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = new Vec3((float)this.NodeDimensionX * this.NodeSize * 0.5f, (float)this.NodeDimensionY * this.NodeSize * 0.5f, 0f, -1f);
			terrainEditContext.AddPlacedFlora(this.TreeKindName, ref identity);
			terrainEditContext.FinalizeEditing();
			terrainEditContext.FinalizeFlora();
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000B66A8 File Offset: 0x000B48A8
		private void FillHeights(float[] heights, int nx, int ny, int verts, float quadLen)
		{
			float num = (float)nx * this.NodeSize;
			float num2 = (float)ny * this.NodeSize;
			for (int i = 0; i < verts; i++)
			{
				for (int j = 0; j < verts; j++)
				{
					float num3 = num + (float)i * quadLen;
					float num4 = num2 + (float)j * quadLen;
					float num5 = ProceduralTerrainTestScript.FbmNoise(num3 * this.NoiseFrequency, num4 * this.NoiseFrequency, this.NoiseOctaves);
					heights[i * verts + j] = this.MinHeight + (num5 * 0.5f + 0.5f) * (this.MaxHeight - this.MinHeight);
				}
			}
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000B673C File Offset: 0x000B493C
		private static float SmoothStep(float edge0, float edge1, float x)
		{
			if (edge1 > edge0)
			{
				float num = (x - edge0) / (edge1 - edge0);
				num = ((num < 0f) ? 0f : ((num > 1f) ? 1f : num));
				return num * num * (3f - 2f * num);
			}
			if (x >= edge0)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000B6798 File Offset: 0x000B4998
		private static void FillWeights(float[] heights, float[] rock, float[] soil, float[] grass, float[] floraOnly, int verts, float quadLen, float nodeWorldX, float nodeWorldY)
		{
			for (int i = 0; i < verts; i++)
			{
				for (int j = 0; j < verts; j++)
				{
					int num = i * verts + j;
					float num2 = heights[Math.Max(i - 1, 0) * verts + j];
					float num3 = heights[Math.Min(i + 1, verts - 1) * verts + j];
					float num4 = heights[i * verts + Math.Max(j - 1, 0)];
					float num5 = heights[i * verts + Math.Min(j + 1, verts - 1)];
					float num6 = (num3 - num2) / (2f * quadLen);
					float num7 = (num5 - num4) / (2f * quadLen);
					float num8 = (float)Math.Sqrt((double)(num6 * num6 + num7 * num7));
					rock[num] = ProceduralTerrainTestScript.SmoothStep(0.4f, 0.85f, num8);
					float num9 = ProceduralTerrainTestScript.SmoothStep(0.12f, 0.45f, num8);
					soil[num] = num9 * (1f - rock[num] * 0.85f);
					float num10 = nodeWorldX + (float)i * quadLen;
					float num11 = nodeWorldY + (float)j * quadLen;
					float num12 = ProceduralTerrainTestScript.FbmNoise(num10 * 0.01f, num11 * 0.01f, 3) * 0.5f + 0.5f;
					float num13 = ProceduralTerrainTestScript.SmoothStep(0.46f, 0.66f, num12);
					float num14 = 1f - ProceduralTerrainTestScript.SmoothStep(0.33f, 0.55f, num8);
					float num15 = 1f - Math.Max(rock[num], soil[num]);
					grass[num] = num13 * num14 * ((num15 < 0f) ? 0f : num15);
					float num16 = 1f - ProceduralTerrainTestScript.SmoothStep(0.25f, 0.6f, num8);
					floraOnly[num] = num13 * num16;
				}
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000B693C File Offset: 0x000B4B3C
		private static float FbmNoise(float x, float y, int octaves)
		{
			float num = 0f;
			float num2 = 0.5f;
			float num3 = 1f;
			for (int i = 0; i < octaves; i++)
			{
				num += ProceduralTerrainTestScript.ValueNoise(x * num3, y * num3) * num2;
				num3 *= 2f;
				num2 *= 0.5f;
			}
			return num;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000B6988 File Offset: 0x000B4B88
		private static float ValueNoise(float x, float y)
		{
			int num = (int)Math.Floor((double)x);
			int num2 = (int)Math.Floor((double)y);
			float num3 = x - (float)num;
			float num4 = y - (float)num2;
			float num5 = num3 * num3 * (3f - 2f * num3);
			float num6 = num4 * num4 * (3f - 2f * num4);
			float num7 = ProceduralTerrainTestScript.PseudoRandom(num, num2);
			float num8 = ProceduralTerrainTestScript.PseudoRandom(num + 1, num2);
			float num9 = ProceduralTerrainTestScript.PseudoRandom(num, num2 + 1);
			float num10 = ProceduralTerrainTestScript.PseudoRandom(num + 1, num2 + 1);
			float num11 = num7 + (num8 - num7) * num5;
			float num12 = num9 + (num10 - num9) * num5;
			return num11 + (num12 - num11) * num6;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000B6A28 File Offset: 0x000B4C28
		private static float PseudoRandom(int x, int y)
		{
			int num = x * 1619 + y * 31337 + 6971;
			int num2 = (num ^ (int)((uint)num >> 16)) * 73244475;
			return ((num2 ^ (int)((uint)num2 >> 16)) & 65535) / 32767.5f - 1f;
		}

		// Token: 0x040012AE RID: 4782
		public int NodeDimensionX = 2;

		// Token: 0x040012AF RID: 4783
		public int NodeDimensionY = 2;

		// Token: 0x040012B0 RID: 4784
		public float NodeSize = 256f;

		// Token: 0x040012B1 RID: 4785
		public float MinHeight;

		// Token: 0x040012B2 RID: 4786
		public float MaxHeight = 90f;

		// Token: 0x040012B3 RID: 4787
		public int HeightmapDetailLevel = 7;

		// Token: 0x040012B4 RID: 4788
		public float NoiseFrequency = 0.008f;

		// Token: 0x040012B5 RID: 4789
		public int NoiseOctaves = 5;

		// Token: 0x040012B6 RID: 4790
		public string GrassKindName = "flora_grass_a_non_shadow";

		// Token: 0x040012B7 RID: 4791
		public string ShrubKindName = "shrub__mix";

		// Token: 0x040012B8 RID: 4792
		public string TreeKindName = "tree_high_a";

		// Token: 0x040012B9 RID: 4793
		public float GrassDensity = 65f;

		// Token: 0x040012BA RID: 4794
		public float ShrubDensity = 8f;

		// Token: 0x040012BB RID: 4795
		public float TreeDensity = 1.5f;

		// Token: 0x040012BC RID: 4796
		public string UndergrowthKindName = "flora_green__mix";

		// Token: 0x040012BD RID: 4797
		public string FlowerKindName = "valley_flower__mix";

		// Token: 0x040012BE RID: 4798
		public float UndergrowthDensity = 40f;

		// Token: 0x040012BF RID: 4799
		public float FlowerDensity = 18f;

		// Token: 0x040012C0 RID: 4800
		public SimpleButton GenerateTerrain;

		// Token: 0x040012C1 RID: 4801
		private bool _pendingGenerate;

		// Token: 0x040012C2 RID: 4802
		private const float RockGradStart = 0.4f;

		// Token: 0x040012C3 RID: 4803
		private const float RockGradFull = 0.85f;

		// Token: 0x040012C4 RID: 4804
		private const float SoilGradStart = 0.12f;

		// Token: 0x040012C5 RID: 4805
		private const float SoilGradFull = 0.45f;

		// Token: 0x040012C6 RID: 4806
		private const float GrassNoiseFreq = 0.01f;

		// Token: 0x040012C7 RID: 4807
		private const float GrassMaskFloor = 0.46f;

		// Token: 0x040012C8 RID: 4808
		private const float GrassMaskCeil = 0.66f;

		// Token: 0x040012C9 RID: 4809
		private const float GrassGradLimit = 0.55f;

		// Token: 0x040012CA RID: 4810
		private const float FloraGradStart = 0.25f;

		// Token: 0x040012CB RID: 4811
		private const float FloraGradFull = 0.6f;
	}
}
