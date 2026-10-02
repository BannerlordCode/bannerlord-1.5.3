using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003B RID: 59
	[ApplicationInterfaceBase]
	internal interface IEngineSizeChecker
	{
		// Token: 0x0600062E RID: 1582
		[EngineMethod("get_engine_struct_size", false, null, false)]
		int GetEngineStructSize(string str);

		// Token: 0x0600062F RID: 1583
		[EngineMethod("get_engine_struct_member_offset", false, null, false)]
		IntPtr GetEngineStructMemberOffset(string className, string memberName);
	}
}
