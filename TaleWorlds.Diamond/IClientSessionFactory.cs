using System;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000014 RID: 20
	public interface IClientSessionFactory
	{
		// Token: 0x06000078 RID: 120
		IClientSession CreateSession(int aliveCheckInterval);
	}
}
