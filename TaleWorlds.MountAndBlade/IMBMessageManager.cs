using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BC RID: 444
	[ScriptingInterfaceBase]
	internal interface IMBMessageManager
	{
		// Token: 0x06001932 RID: 6450
		[EngineMethod("display_message", false, null, false)]
		void DisplayMessage(string message);

		// Token: 0x06001933 RID: 6451
		[EngineMethod("display_message_with_color", false, null, false)]
		void DisplayMessageWithColor(string message, uint color);

		// Token: 0x06001934 RID: 6452
		[EngineMethod("set_message_manager", false, null, false)]
		void SetMessageManager(MessageManagerBase messageManager);
	}
}
