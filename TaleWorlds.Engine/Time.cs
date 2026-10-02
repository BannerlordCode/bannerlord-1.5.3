using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000097 RID: 151
	public static class Time
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0000F104 File Offset: 0x0000D304
		public static float ApplicationTime
		{
			get
			{
				return EngineApplicationInterface.ITime.GetApplicationTime();
			}
		}
	}
}
