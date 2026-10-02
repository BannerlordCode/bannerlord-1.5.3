using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B7 RID: 951
	public interface IPartyScreenPrisonHandler
	{
		// Token: 0x06003755 RID: 14165
		void ExecuteTakeAllPrisonersScript();

		// Token: 0x06003756 RID: 14166
		void ExecuteDoneScript();

		// Token: 0x06003757 RID: 14167
		void ExecuteResetScript();

		// Token: 0x06003758 RID: 14168
		void ExecuteSellAllPrisoners();
	}
}
