using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000051 RID: 81
	public interface IMbEvent<out T1, out T2, out T3> : IMbEventBase
	{
		// Token: 0x060008C1 RID: 2241
		void AddNonSerializedListener(object owner, Action<T1, T2, T3> action);
	}
}
