using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x0200014F RID: 335
	public class PerkSelectedByPlayerEvent : EventBase
	{
		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060020A9 RID: 8361 RVA: 0x000757C4 File Offset: 0x000739C4
		// (set) Token: 0x060020AA RID: 8362 RVA: 0x000757CC File Offset: 0x000739CC
		public PerkObject SelectedPerk { get; private set; }

		// Token: 0x060020AB RID: 8363 RVA: 0x000757D5 File Offset: 0x000739D5
		public PerkSelectedByPlayerEvent(PerkObject selectedPerk)
		{
			this.SelectedPerk = selectedPerk;
		}
	}
}
