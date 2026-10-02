using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000089 RID: 137
	public interface IGameStateManagerOwner
	{
		// Token: 0x060008AA RID: 2218
		void OnStateStackEmpty();

		// Token: 0x060008AB RID: 2219
		void OnStateChanged(GameState oldState);
	}
}
