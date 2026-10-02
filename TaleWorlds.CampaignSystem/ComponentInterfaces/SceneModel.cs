using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000207 RID: 519
	public abstract class SceneModel : MBGameModel<SceneModel>
	{
		// Token: 0x0600203E RID: 8254
		public abstract string GetConversationSceneForMapPosition(CampaignVec2 campaignPosition);

		// Token: 0x0600203F RID: 8255
		public abstract string GetBattleSceneForMapPatch(MapPatchData mapPatch, bool isNavalEncounter);
	}
}
