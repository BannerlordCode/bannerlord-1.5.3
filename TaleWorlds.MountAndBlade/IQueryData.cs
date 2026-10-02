using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017E RID: 382
	public interface IQueryData
	{
		// Token: 0x06001449 RID: 5193
		void Expire();

		// Token: 0x0600144A RID: 5194
		void Evaluate(float currentTime);

		// Token: 0x0600144B RID: 5195
		void SetSyncGroup(IQueryData[] syncGroup);
	}
}
