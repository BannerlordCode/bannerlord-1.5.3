using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000090 RID: 144
	public struct ConversationSceneData
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0005670E File Offset: 0x0005490E
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x00056716 File Offset: 0x00054916
		public string SceneID { get; private set; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0005671F File Offset: 0x0005491F
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x00056727 File Offset: 0x00054927
		public TerrainType Terrain { get; private set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x00056730 File Offset: 0x00054930
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x00056738 File Offset: 0x00054938
		public List<TerrainType> TerrainTypes { get; private set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x00056741 File Offset: 0x00054941
		// (set) Token: 0x060012B1 RID: 4785 RVA: 0x00056749 File Offset: 0x00054949
		public ForestDensity ForestDensity { get; private set; }

		// Token: 0x060012B2 RID: 4786 RVA: 0x00056752 File Offset: 0x00054952
		public ConversationSceneData(string sceneID, TerrainType terrain, List<TerrainType> terrainTypes, ForestDensity forestDensity)
		{
			this.SceneID = sceneID;
			this.Terrain = terrain;
			this.TerrainTypes = terrainTypes;
			this.ForestDensity = forestDensity;
		}
	}
}
