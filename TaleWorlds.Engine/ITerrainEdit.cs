using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200002F RID: 47
	[ApplicationInterfaceBase]
	internal interface ITerrainEdit
	{
		// Token: 0x0600050E RID: 1294
		[EngineMethod("create_terrain", false, null, false)]
		void CreateTerrain(UIntPtr scenePointer, int nodeDimX, int nodeDimY, float nodeSize, float minHeight, float maxHeight, int heightmapDetailLevel, string baseLayerName);

		// Token: 0x0600050F RID: 1295
		[EngineMethod("set_node_height_data", false, null, false)]
		void SetNodeHeightData(Scene scene, int nodeX, int nodeY, float[] heights, int heightCount);

		// Token: 0x06000510 RID: 1296
		[EngineMethod("add_layer_from_material", false, null, false)]
		int AddLayerFromMaterial(UIntPtr scenePointer, string layerPrefabName);

		// Token: 0x06000511 RID: 1297
		[EngineMethod("add_empty_layer", false, null, false)]
		int AddEmptyLayer(UIntPtr scenePointer, string layerName);

		// Token: 0x06000512 RID: 1298
		[EngineMethod("set_layer_texture", false, null, false)]
		void SetLayerTexture(UIntPtr scenePointer, int layerIndex, int textureType, string textureName);

		// Token: 0x06000513 RID: 1299
		[EngineMethod("set_layer_property_float", false, null, false)]
		void SetLayerPropertyFloat(UIntPtr scenePointer, int layerIndex, int propertyId, float value);

		// Token: 0x06000514 RID: 1300
		[EngineMethod("set_layer_property_string", false, null, false)]
		void SetLayerPropertyString(UIntPtr scenePointer, int layerIndex, int propertyId, string value);

		// Token: 0x06000515 RID: 1301
		[EngineMethod("set_node_layer_weight_data", false, null, false)]
		void SetNodeLayerWeightData(Scene scene, int nodeX, int nodeY, int layerIndex, float[] weights, int weightCount);

		// Token: 0x06000516 RID: 1302
		[EngineMethod("finalize", false, null, false)]
		void Finalize(UIntPtr scenePointer);

		// Token: 0x06000517 RID: 1303
		[EngineMethod("add_procedural_flora_to_layer", false, null, false)]
		int AddProceduralFloraToLayer(Scene scene, int layerIndex, string floraKindName, float density, int seedIndex, float sizeMin, float sizeMax, float colonyRadius, float colonyThreshold, float weightOffset);

		// Token: 0x06000518 RID: 1304
		[EngineMethod("add_placed_flora", false, null, false)]
		void AddPlacedFlora(Scene scene, string floraKindName, ref MatrixFrame frame);

		// Token: 0x06000519 RID: 1305
		[EngineMethod("finalize_flora", false, null, false)]
		void FinalizeFlora(Scene scene);
	}
}
