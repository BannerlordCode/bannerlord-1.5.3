using System;

namespace TaleWorlds.PlatformService
{
	// Token: 0x0200000E RID: 14
	public interface ISessionService
	{
		// Token: 0x06000056 RID: 86
		void OnJoinJoinableSession(string connectionString);

		// Token: 0x06000057 RID: 87
		void OnLeaveJoinableSession();
	}
}
