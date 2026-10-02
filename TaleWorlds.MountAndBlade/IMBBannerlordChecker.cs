using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C2 RID: 450
	[ScriptingInterfaceBase]
	internal interface IMBBannerlordChecker
	{
		// Token: 0x06001972 RID: 6514
		[EngineMethod("get_engine_struct_size", false, null, false)]
		int GetEngineStructSize(string str);

		// Token: 0x06001973 RID: 6515
		[EngineMethod("get_engine_struct_member_offset", false, null, false)]
		IntPtr GetEngineStructMemberOffset(string className, string memberName);
	}
}
