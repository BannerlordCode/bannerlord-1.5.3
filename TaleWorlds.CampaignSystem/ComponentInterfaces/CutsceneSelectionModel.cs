using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FF RID: 511
	public abstract class CutsceneSelectionModel : MBGameModel<CutsceneSelectionModel>
	{
		// Token: 0x06002001 RID: 8193
		public abstract SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom);
	}
}
