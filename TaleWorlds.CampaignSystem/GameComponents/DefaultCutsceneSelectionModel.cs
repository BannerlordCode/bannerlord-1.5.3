using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000112 RID: 274
	public class DefaultCutsceneSelectionModel : CutsceneSelectionModel
	{
		// Token: 0x060017F5 RID: 6133 RVA: 0x0007171D File Offset: 0x0006F91D
		public override SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom)
		{
			return new KingdomDestroyedSceneNotificationItem(kingdom, CampaignTime.Now);
		}
	}
}
