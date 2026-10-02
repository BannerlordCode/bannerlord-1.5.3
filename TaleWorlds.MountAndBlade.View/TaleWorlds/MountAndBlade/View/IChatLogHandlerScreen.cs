using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000017 RID: 23
	public interface IChatLogHandlerScreen
	{
		// Token: 0x06000099 RID: 153
		void TryUpdateChatLogLayerParameters(ref bool isTeamChatAvailable, ref bool inputEnabled, ref bool isToggleChatHintAvailable, ref bool isMouseVisible, ref InputContext inputContext);
	}
}
