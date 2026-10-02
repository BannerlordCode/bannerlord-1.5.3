using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200007C RID: 124
	public interface IAgent
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000856 RID: 2134
		BasicCharacterObject Character { get; }

		// Token: 0x06000857 RID: 2135
		bool IsEnemyOf(IAgent agent);

		// Token: 0x06000858 RID: 2136
		bool IsFriendOf(IAgent agent);

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000859 RID: 2137
		AgentState State { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600085A RID: 2138
		IMissionTeam Team { get; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600085B RID: 2139
		IAgentOriginBase Origin { get; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600085C RID: 2140
		float Age { get; }

		// Token: 0x0600085D RID: 2141
		bool IsActive();

		// Token: 0x0600085E RID: 2142
		void SetAsConversationAgent(bool set);

		// Token: 0x0600085F RID: 2143
		void OnConversationStarted();
	}
}
