using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003D RID: 61
	[ApplicationInterfaceBase]
	internal interface ITime
	{
		// Token: 0x06000650 RID: 1616
		[EngineMethod("get_application_time", false, null, false)]
		float GetApplicationTime();
	}
}
