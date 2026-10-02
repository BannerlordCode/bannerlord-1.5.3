using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003B5 RID: 949
	public interface IPartyScreenLogicHandler
	{
		// Token: 0x06003752 RID: 14162
		void RequestUserInput(string text, Action accept, Action cancel);
	}
}
