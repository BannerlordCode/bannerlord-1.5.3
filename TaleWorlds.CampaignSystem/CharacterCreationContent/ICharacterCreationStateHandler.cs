using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x0200021B RID: 539
	public interface ICharacterCreationStateHandler
	{
		// Token: 0x060020C6 RID: 8390
		void OnCharacterCreationFinalized();

		// Token: 0x060020C7 RID: 8391
		void OnRefresh();

		// Token: 0x060020C8 RID: 8392
		void OnStageCreated(CharacterCreationStageBase stage);
	}
}
