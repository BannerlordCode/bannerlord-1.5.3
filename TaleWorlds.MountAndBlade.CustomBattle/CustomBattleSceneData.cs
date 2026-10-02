using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000010 RID: 16
	public struct CustomBattleSceneData
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000849E File Offset: 0x0000669E
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000084A6 File Offset: 0x000066A6
		public string SceneID { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000084AF File Offset: 0x000066AF
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x000084B7 File Offset: 0x000066B7
		public TextObject Name { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000084C0 File Offset: 0x000066C0
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000084C8 File Offset: 0x000066C8
		public TerrainType Terrain { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000084D1 File Offset: 0x000066D1
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000084D9 File Offset: 0x000066D9
		public List<TerrainType> TerrainTypes { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000084E2 File Offset: 0x000066E2
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000084EA File Offset: 0x000066EA
		public ForestDensity ForestDensity { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000084F3 File Offset: 0x000066F3
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000084FB File Offset: 0x000066FB
		public bool IsSiegeMap { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00008504 File Offset: 0x00006704
		// (set) Token: 0x060000FF RID: 255 RVA: 0x0000850C File Offset: 0x0000670C
		public bool IsVillageMap { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00008515 File Offset: 0x00006715
		// (set) Token: 0x06000101 RID: 257 RVA: 0x0000851D File Offset: 0x0000671D
		public bool IsLordsHallMap { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00008526 File Offset: 0x00006726
		// (set) Token: 0x06000103 RID: 259 RVA: 0x0000852E File Offset: 0x0000672E
		public string ForcedSceneLevel { get; private set; }

		// Token: 0x06000104 RID: 260 RVA: 0x00008538 File Offset: 0x00006738
		public CustomBattleSceneData(string sceneID, TextObject name, TerrainType terrain, List<TerrainType> terrainTypes, ForestDensity forestDensity, bool isSiegeMap, bool isVillageMap, bool isLordsHallMap, string forcedSceneLevel)
		{
			this.SceneID = sceneID;
			this.Name = name;
			this.Terrain = terrain;
			this.TerrainTypes = terrainTypes;
			this.ForestDensity = forestDensity;
			this.IsSiegeMap = isSiegeMap;
			this.IsVillageMap = isVillageMap;
			this.IsLordsHallMap = isLordsHallMap;
			this.ForcedSceneLevel = forcedSceneLevel;
		}
	}
}
