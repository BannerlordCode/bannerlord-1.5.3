using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000047 RID: 71
	public interface IMbEvent<out T> : IMbEventBase
	{
		// Token: 0x0600089E RID: 2206
		void AddNonSerializedListener(object owner, Action<T> action);
	}
}
