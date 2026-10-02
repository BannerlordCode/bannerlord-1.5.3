using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.GauntletUI.SceneNotification
{
	// Token: 0x0200002B RID: 43
	public class NativeSceneNotificationContextProvider : ISceneNotificationContextProvider
	{
		// Token: 0x060001BB RID: 443 RVA: 0x0000A992 File Offset: 0x00008B92
		public bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType)
		{
			return relevantType != SceneNotificationData.RelevantContextType.Mission || GameStateManager.Current.ActiveState is MissionState;
		}
	}
}
