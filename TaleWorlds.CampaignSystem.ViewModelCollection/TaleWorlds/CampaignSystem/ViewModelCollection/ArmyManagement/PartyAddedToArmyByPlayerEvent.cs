using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x02000165 RID: 357
	public class PartyAddedToArmyByPlayerEvent : EventBase
	{
		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x0007CB8C File Offset: 0x0007AD8C
		// (set) Token: 0x06002314 RID: 8980 RVA: 0x0007CB94 File Offset: 0x0007AD94
		public MobileParty AddedParty { get; private set; }

		// Token: 0x06002315 RID: 8981 RVA: 0x0007CB9D File Offset: 0x0007AD9D
		public PartyAddedToArmyByPlayerEvent(MobileParty addedParty)
		{
			this.AddedParty = addedParty;
		}
	}
}
