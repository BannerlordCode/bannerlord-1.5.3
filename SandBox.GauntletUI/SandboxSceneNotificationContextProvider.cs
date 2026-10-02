using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace SandBox.GauntletUI
{
	// Token: 0x02000014 RID: 20
	public class SandboxSceneNotificationContextProvider : ISceneNotificationContextProvider
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00008076 File Offset: 0x00006276
		public bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType)
		{
			return relevantType != SceneNotificationData.RelevantContextType.Map || GameStateManager.Current.ActiveState is MapState;
		}
	}
}
