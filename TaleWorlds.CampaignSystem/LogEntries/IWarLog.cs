using System;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x0200036B RID: 875
	public interface IWarLog
	{
		// Token: 0x0600340A RID: 13322
		bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected);
	}
}
