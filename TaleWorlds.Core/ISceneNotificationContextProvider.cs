using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000CC RID: 204
	public interface ISceneNotificationContextProvider
	{
		// Token: 0x06000B09 RID: 2825
		bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType);
	}
}
