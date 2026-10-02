using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014F RID: 335
	public class DefaultSceneModel : SceneModel
	{
		// Token: 0x06001A57 RID: 6743 RVA: 0x00084EF0 File Offset: 0x000830F0
		public override string GetBattleSceneForMapPatch(MapPatchData mapPatch, bool isNavalEncounter)
		{
			MBList<SingleplayerBattleSceneData> mblist = GameSceneDataManager.Instance.SingleplayerBattleScenes.Where<SingleplayerBattleSceneData>((SingleplayerBattleSceneData scene) => scene.MapIndices.Contains(mapPatch.sceneIndex) && scene.IsNaval == isNavalEncounter).ToMBList<SingleplayerBattleSceneData>();
			string text;
			if (mblist.IsEmpty<SingleplayerBattleSceneData>())
			{
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				CampaignVec2 campaignVec = MobileParty.MainParty.Position;
				TerrainType currentPositionTerrainType2;
				mapSceneWrapper.GetEnvironmentTerrainTypesCount(in campaignVec, out currentPositionTerrainType2);
				mblist = GameSceneDataManager.Instance.SingleplayerBattleScenes.Where<SingleplayerBattleSceneData>((SingleplayerBattleSceneData scene) => scene.Terrain == currentPositionTerrainType2 && scene.IsNaval == isNavalEncounter).ToMBList<SingleplayerBattleSceneData>();
				if (mblist.IsEmpty<SingleplayerBattleSceneData>())
				{
					Debug.FailedAssert("Battle scene for map patch with scene index " + mapPatch.sceneIndex + " does not exist. Picking a random scene", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSceneModel.cs", "GetBattleSceneForMapPatch", 35);
					mblist = GameSceneDataManager.Instance.SingleplayerBattleScenes.Where<SingleplayerBattleSceneData>((SingleplayerBattleSceneData scene) => scene.IsNaval == isNavalEncounter).ToMBList<SingleplayerBattleSceneData>();
					if (mblist.IsEmpty<SingleplayerBattleSceneData>())
					{
						Debug.FailedAssert("naval battles scene mismatch. Picking a random scene", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSceneModel.cs", "GetBattleSceneForMapPatch", 40);
						mblist = GameSceneDataManager.Instance.SingleplayerBattleScenes.ToMBList<SingleplayerBattleSceneData>();
					}
				}
				text = mblist.GetRandomElement<SingleplayerBattleSceneData>().SceneID;
			}
			else if (mblist.Count > 1)
			{
				if (isNavalEncounter)
				{
					IMapScene mapSceneWrapper2 = Campaign.Current.MapSceneWrapper;
					CampaignVec2 campaignVec = MobileParty.MainParty.Position;
					TerrainType currentPositionTerrainType;
					mapSceneWrapper2.GetEnvironmentTerrainTypesCount(in campaignVec, out currentPositionTerrainType);
					List<SingleplayerBattleSceneData> list = mblist.Where<SingleplayerBattleSceneData>((SingleplayerBattleSceneData scene) => scene.Terrain == currentPositionTerrainType).ToList<SingleplayerBattleSceneData>();
					if (!list.IsEmpty<SingleplayerBattleSceneData>())
					{
						text = list.GetRandomElement<SingleplayerBattleSceneData>().SceneID;
					}
					else
					{
						text = mblist.GetRandomElement<SingleplayerBattleSceneData>().SceneID;
					}
				}
				else
				{
					Debug.FailedAssert("Multiple battle scenes for map patch with scene index " + mapPatch.sceneIndex + " are defined. Picking a matching scene randomly", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSceneModel.cs", "GetBattleSceneForMapPatch", 67);
					text = mblist.GetRandomElement<SingleplayerBattleSceneData>().SceneID;
				}
			}
			else
			{
				text = mblist[0].SceneID;
			}
			return text;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0008511C File Offset: 0x0008331C
		public override string GetConversationSceneForMapPosition(CampaignVec2 campaignPosition)
		{
			TerrainType terrainType;
			List<TerrainType> environmentTerrainTypesCount = Campaign.Current.MapSceneWrapper.GetEnvironmentTerrainTypesCount(in campaignPosition, out terrainType);
			TerrainType terrain = DefaultSceneModel.GetTerrainByCount(environmentTerrainTypesCount, terrainType);
			return (GameSceneDataManager.Instance.ConversationScenes.Any<ConversationSceneData>((ConversationSceneData scene) => scene.Terrain == terrain) ? GameSceneDataManager.Instance.ConversationScenes.GetRandomElementWithPredicate<ConversationSceneData>((ConversationSceneData scene) => scene.Terrain == terrain) : GameSceneDataManager.Instance.ConversationScenes.GetRandomElement<ConversationSceneData>()).SceneID;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x000851A4 File Offset: 0x000833A4
		private static TerrainType GetTerrainByCount(List<TerrainType> terrainTypeSamples, TerrainType currentPositionTerrainType)
		{
			for (int i = 0; i < terrainTypeSamples.Count; i++)
			{
				if (terrainTypeSamples[i] == TerrainType.Snow)
				{
					terrainTypeSamples[i] = TerrainType.Plain;
				}
			}
			if (DefaultSceneModel._conversationTerrains.Contains(currentPositionTerrainType))
			{
				int num = (int)((float)terrainTypeSamples.Count * 0.33f);
				for (int j = 0; j < num; j++)
				{
					terrainTypeSamples.Add(currentPositionTerrainType);
				}
			}
			Dictionary<TerrainType, int> dictionary = new Dictionary<TerrainType, int>();
			foreach (TerrainType terrainType in terrainTypeSamples)
			{
				if (DefaultSceneModel._conversationTerrains.Contains(terrainType))
				{
					if (!dictionary.ContainsKey(terrainType))
					{
						dictionary.Add(terrainType, 1);
					}
					else
					{
						Dictionary<TerrainType, int> dictionary2 = dictionary;
						TerrainType terrainType2 = terrainType;
						int num2 = dictionary2[terrainType2];
						dictionary2[terrainType2] = num2 + 1;
					}
				}
			}
			if (dictionary.Count > 0)
			{
				return dictionary.OrderByDescending<KeyValuePair<TerrainType, int>, int>((KeyValuePair<TerrainType, int> t) => t.Value).First<KeyValuePair<TerrainType, int>>().Key;
			}
			return TerrainType.Plain;
		}

		// Token: 0x040008B4 RID: 2228
		private static readonly TerrainType[] _conversationTerrains = new TerrainType[]
		{
			TerrainType.Plain,
			TerrainType.Desert,
			TerrainType.Swamp,
			TerrainType.Steppe,
			TerrainType.OpenSea,
			TerrainType.CoastalSea,
			TerrainType.Lake,
			TerrainType.River,
			TerrainType.Water
		};
	}
}
