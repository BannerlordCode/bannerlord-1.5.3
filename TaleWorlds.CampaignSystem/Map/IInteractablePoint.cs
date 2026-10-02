using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000226 RID: 550
	public interface IInteractablePoint
	{
		// Token: 0x06002142 RID: 8514
		CampaignVec2 GetInteractionPosition(MobileParty interactingParty);

		// Token: 0x06002143 RID: 8515
		bool CanPartyInteract(MobileParty mobileParty, float dt);

		// Token: 0x06002144 RID: 8516
		void OnPartyInteraction(MobileParty mobileParty);
	}
}
