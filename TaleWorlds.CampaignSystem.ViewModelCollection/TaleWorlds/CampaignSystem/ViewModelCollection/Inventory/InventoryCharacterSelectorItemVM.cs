using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200009E RID: 158
	public class InventoryCharacterSelectorItemVM : SelectorItemVM
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0003E239 File Offset: 0x0003C439
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x0003E241 File Offset: 0x0003C441
		public string CharacterID { get; private set; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0003E24A File Offset: 0x0003C44A
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x0003E252 File Offset: 0x0003C452
		public Hero Hero { get; private set; }

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0003E25B File Offset: 0x0003C45B
		public InventoryCharacterSelectorItemVM(string characterID, Hero hero, TextObject characterName)
			: base(characterName)
		{
			this.Hero = hero;
			this.CharacterID = characterID;
		}
	}
}
