using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000092 RID: 146
	public sealed class TerrainEditContext
	{
		// Token: 0x06000D0A RID: 3338 RVA: 0x0000EA1C File Offset: 0x0000CC1C
		public TerrainEditContext(Scene scene, int nodeDimX, int nodeDimY, float nodeSize, float minHeight, float maxHeight, int heightmapDetailLevel = 7, string baseLayerName = "")
		{
			this._scene = scene;
			EngineApplicationInterface.ITerrainEdit.CreateTerrain(scene.Pointer, nodeDimX, nodeDimY, nodeSize, minHeight, maxHeight, heightmapDetailLevel, baseLayerName);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0000EA52 File Offset: 0x0000CC52
		public void SetHeightData(int nodeX, int nodeY, float[] heights)
		{
			EngineApplicationInterface.ITerrainEdit.SetNodeHeightData(this._scene, nodeX, nodeY, heights, heights.Length);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0000EA6A File Offset: 0x0000CC6A
		public int AddLayerFromMaterial(string prefabName)
		{
			return EngineApplicationInterface.ITerrainEdit.AddLayerFromMaterial(this._scene.Pointer, prefabName);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0000EA82 File Offset: 0x0000CC82
		public int AddEmptyLayer(string layerName = "")
		{
			return EngineApplicationInterface.ITerrainEdit.AddEmptyLayer(this._scene.Pointer, layerName ?? "");
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0000EAA3 File Offset: 0x0000CCA3
		public void SetLayerTexture(int layerIndex, TerrainEditContext.TextureSlot slot, string textureName)
		{
			EngineApplicationInterface.ITerrainEdit.SetLayerTexture(this._scene.Pointer, layerIndex, (int)slot, textureName);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0000EABD File Offset: 0x0000CCBD
		public void SetLayerProperty(int layerIndex, TerrainEditContext.LayerProperty property, float value)
		{
			EngineApplicationInterface.ITerrainEdit.SetLayerPropertyFloat(this._scene.Pointer, layerIndex, (int)property, value);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0000EAD7 File Offset: 0x0000CCD7
		public void SetLayerProperty(int layerIndex, TerrainEditContext.LayerProperty property, string value)
		{
			EngineApplicationInterface.ITerrainEdit.SetLayerPropertyString(this._scene.Pointer, layerIndex, (int)property, value);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0000EAF1 File Offset: 0x0000CCF1
		public void SetLayerWeightData(int nodeX, int nodeY, int layerIndex, float[] weights)
		{
			EngineApplicationInterface.ITerrainEdit.SetNodeLayerWeightData(this._scene, nodeX, nodeY, layerIndex, weights, weights.Length);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0000EB0C File Offset: 0x0000CD0C
		public void FinalizeEditing()
		{
			EngineApplicationInterface.ITerrainEdit.Finalize(this._scene.Pointer);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0000EB24 File Offset: 0x0000CD24
		public int AddProceduralFlora(int layerIndex, TerrainEditContext.FloraDefinition def)
		{
			return EngineApplicationInterface.ITerrainEdit.AddProceduralFloraToLayer(this._scene, layerIndex, def.FloraKindName, def.Density, def.SeedIndex, def.SizeMin, def.SizeMax, def.ColonyRadius, def.ColonyThreshold, def.WeightOffset);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0000EB72 File Offset: 0x0000CD72
		public void AddPlacedFlora(string floraKindName, ref MatrixFrame frame)
		{
			EngineApplicationInterface.ITerrainEdit.AddPlacedFlora(this._scene, floraKindName, ref frame);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0000EB86 File Offset: 0x0000CD86
		public void FinalizeFlora()
		{
			EngineApplicationInterface.ITerrainEdit.FinalizeFlora(this._scene);
		}

		// Token: 0x040001CD RID: 461
		private readonly Scene _scene;

		// Token: 0x020000D4 RID: 212
		public enum TextureSlot
		{
			// Token: 0x0400044E RID: 1102
			Diffuse,
			// Token: 0x0400044F RID: 1103
			AreaMap,
			// Token: 0x04000450 RID: 1104
			NormalMap,
			// Token: 0x04000451 RID: 1105
			SpecularMap,
			// Token: 0x04000452 RID: 1106
			SplattingMap,
			// Token: 0x04000453 RID: 1107
			MaterialMap,
			// Token: 0x04000454 RID: 1108
			DisplacementMap
		}

		// Token: 0x020000D5 RID: 213
		public enum LayerProperty
		{
			// Token: 0x04000456 RID: 1110
			PhysicsMaterial,
			// Token: 0x04000457 RID: 1111
			UvScaleX,
			// Token: 0x04000458 RID: 1112
			UvScaleY,
			// Token: 0x04000459 RID: 1113
			UvRotation,
			// Token: 0x0400045A RID: 1114
			ElevationAmount,
			// Token: 0x0400045B RID: 1115
			ParallaxAmount,
			// Token: 0x0400045C RID: 1116
			GroundSlopeScale,
			// Token: 0x0400045D RID: 1117
			SmoothBlendAmount,
			// Token: 0x0400045E RID: 1118
			BigDetailMapMode,
			// Token: 0x0400045F RID: 1119
			BigDetailMapWeight,
			// Token: 0x04000460 RID: 1120
			AlbedoFactorR,
			// Token: 0x04000461 RID: 1121
			AlbedoFactorG,
			// Token: 0x04000462 RID: 1122
			AlbedoFactorB,
			// Token: 0x04000463 RID: 1123
			AlbedoFactorA,
			// Token: 0x04000464 RID: 1124
			FlagUseParallax,
			// Token: 0x04000465 RID: 1125
			FlagUseDisplacement,
			// Token: 0x04000466 RID: 1126
			FlagSlopeTransparency,
			// Token: 0x04000467 RID: 1127
			FlagRandomizedNormal,
			// Token: 0x04000468 RID: 1128
			IsFloraLayer
		}

		// Token: 0x020000D6 RID: 214
		public struct FloraDefinition
		{
			// Token: 0x0600103F RID: 4159 RVA: 0x000151B8 File Offset: 0x000133B8
			public static TerrainEditContext.FloraDefinition Default(string floraKindName)
			{
				return new TerrainEditContext.FloraDefinition
				{
					FloraKindName = floraKindName,
					Density = 0.5f,
					SeedIndex = 0,
					SizeMin = 0.65f,
					SizeMax = 1.55f,
					ColonyRadius = 0f,
					ColonyThreshold = 0.3f,
					WeightOffset = 0.5f
				};
			}

			// Token: 0x04000469 RID: 1129
			public string FloraKindName;

			// Token: 0x0400046A RID: 1130
			public float Density;

			// Token: 0x0400046B RID: 1131
			public int SeedIndex;

			// Token: 0x0400046C RID: 1132
			public float SizeMin;

			// Token: 0x0400046D RID: 1133
			public float SizeMax;

			// Token: 0x0400046E RID: 1134
			public float ColonyRadius;

			// Token: 0x0400046F RID: 1135
			public float ColonyThreshold;

			// Token: 0x04000470 RID: 1136
			public float WeightOffset;
		}
	}
}
