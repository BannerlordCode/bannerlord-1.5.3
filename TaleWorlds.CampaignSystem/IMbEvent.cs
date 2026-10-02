using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003C RID: 60
	public interface IMbEvent
	{
		// Token: 0x060003F4 RID: 1012
		void AddNonSerializedListener(object owner, Action action);

		// Token: 0x060003F5 RID: 1013
		void ClearListeners(object o);
	}
}
