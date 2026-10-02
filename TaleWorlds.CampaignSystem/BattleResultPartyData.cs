using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002E RID: 46
	public struct BattleResultPartyData
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00014813 File Offset: 0x00012A13
		public BattleResultPartyData(PartyBase party)
		{
			this.Party = party;
			this.Characters = new List<CharacterObject>();
		}

		// Token: 0x0400001F RID: 31
		public readonly PartyBase Party;

		// Token: 0x04000020 RID: 32
		public readonly List<CharacterObject> Characters;
	}
}
