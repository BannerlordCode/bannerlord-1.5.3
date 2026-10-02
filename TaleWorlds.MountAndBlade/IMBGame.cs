using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BE RID: 446
	[ScriptingInterfaceBase]
	internal interface IMBGame
	{
		// Token: 0x0600193C RID: 6460
		[EngineMethod("start_new", false, null, false)]
		void StartNew();

		// Token: 0x0600193D RID: 6461
		[EngineMethod("load_module_data", false, null, false)]
		void LoadModuleData(bool isLoadGame);
	}
}
