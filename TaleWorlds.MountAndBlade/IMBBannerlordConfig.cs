using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C4 RID: 452
	[ScriptingInterfaceBase]
	internal interface IMBBannerlordConfig
	{
		// Token: 0x06001977 RID: 6519
		[EngineMethod("validate_options", false, null, false)]
		void ValidateOptions();
	}
}
