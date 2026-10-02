using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000009 RID: 9
	public class CustomBattleSceneNotificationContextProvider : ISceneNotificationContextProvider
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00005AB9 File Offset: 0x00003CB9
		public bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType)
		{
			return relevantType != SceneNotificationData.RelevantContextType.CustomBattle || GameStateManager.Current.ActiveState is CustomBattleState;
		}
	}
}
