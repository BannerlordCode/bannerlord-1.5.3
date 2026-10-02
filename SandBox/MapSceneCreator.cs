using System;
using TaleWorlds.CampaignSystem.Map;

namespace SandBox
{
	// Token: 0x0200001C RID: 28
	public class MapSceneCreator : IMapSceneCreator
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00005091 File Offset: 0x00003291
		IMapScene IMapSceneCreator.CreateMapScene()
		{
			return new MapScene();
		}
	}
}
