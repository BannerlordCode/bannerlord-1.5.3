using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004D RID: 77
	public interface ReferenceIMBEvent<T1, T2, T3> : IMbEventBase
	{
		// Token: 0x060008B3 RID: 2227
		void AddNonSerializedListener(object owner, ReferenceAction<T1, T2, T3> action);
	}
}
