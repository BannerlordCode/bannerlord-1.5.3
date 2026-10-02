using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000049 RID: 73
	public interface ReferenceIMBEvent<T1> : IMbEventBase
	{
		// Token: 0x060008A5 RID: 2213
		void AddNonSerializedListener(object owner, ReferenceAction<T1> action);
	}
}
