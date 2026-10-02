using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008F RID: 143
	public struct SingleplayerBattleSceneData
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x00056679 File Offset: 0x00054879
		// (set) Token: 0x0600129E RID: 4766 RVA: 0x00056681 File Offset: 0x00054881
		public string SceneID { get; private set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x0005668A File Offset: 0x0005488A
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x00056692 File Offset: 0x00054892
		public TerrainType Terrain { get; private set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0005669B File Offset: 0x0005489B
		// (set) Token: 0x060012A2 RID: 4770 RVA: 0x000566A3 File Offset: 0x000548A3
		public List<TerrainType> TerrainTypes { get; private set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x000566AC File Offset: 0x000548AC
		// (set) Token: 0x060012A4 RID: 4772 RVA: 0x000566B4 File Offset: 0x000548B4
		public ForestDensity ForestDensity { get; private set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x000566BD File Offset: 0x000548BD
		// (set) Token: 0x060012A6 RID: 4774 RVA: 0x000566C5 File Offset: 0x000548C5
		public List<int> MapIndices { get; private set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x000566CE File Offset: 0x000548CE
		// (set) Token: 0x060012A8 RID: 4776 RVA: 0x000566D6 File Offset: 0x000548D6
		public bool IsNaval { get; private set; }

		// Token: 0x060012A9 RID: 4777 RVA: 0x000566DF File Offset: 0x000548DF
		public SingleplayerBattleSceneData(string sceneID, TerrainType terrain, List<TerrainType> terrainTypes, ForestDensity forestDensity, List<int> mapIndices, bool isNaval)
		{
			this.SceneID = sceneID;
			this.Terrain = terrain;
			this.TerrainTypes = terrainTypes;
			this.ForestDensity = forestDensity;
			this.MapIndices = mapIndices;
			this.IsNaval = isNaval;
		}
	}
}
