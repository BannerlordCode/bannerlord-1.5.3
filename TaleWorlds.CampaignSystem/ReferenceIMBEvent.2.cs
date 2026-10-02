using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004B RID: 75
	public interface ReferenceIMBEvent<T1, T2> : IMbEventBase
	{
		// Token: 0x060008AC RID: 2220
		void AddNonSerializedListener(object owner, ReferenceAction<T1, T2> action);
	}
}
