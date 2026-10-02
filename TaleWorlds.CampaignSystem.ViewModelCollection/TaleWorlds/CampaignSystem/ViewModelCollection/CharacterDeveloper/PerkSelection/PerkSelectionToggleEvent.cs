using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x02000150 RID: 336
	public class PerkSelectionToggleEvent : EventBase
	{
		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x000757E4 File Offset: 0x000739E4
		// (set) Token: 0x060020AD RID: 8365 RVA: 0x000757EC File Offset: 0x000739EC
		public bool IsCurrentlyActive { get; private set; }

		// Token: 0x060020AE RID: 8366 RVA: 0x000757F5 File Offset: 0x000739F5
		public PerkSelectionToggleEvent(bool isCurrentlyActive)
		{
			this.IsCurrentlyActive = isCurrentlyActive;
		}
	}
}
