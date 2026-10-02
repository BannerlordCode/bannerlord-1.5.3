using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004F RID: 79
	public interface IMbEvent<out T1, out T2> : IMbEventBase
	{
		// Token: 0x060008BA RID: 2234
		void AddNonSerializedListener(object owner, Action<T1, T2> action);
	}
}
